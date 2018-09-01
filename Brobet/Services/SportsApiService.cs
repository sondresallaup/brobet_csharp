using Brobet.Models;
using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;

namespace Brobet.Services
{
    public class SportsApiService
    {
        private Entities db = new Entities();
        private string baseUrl = "https://soccer.sportmonks.com/api/v2.0/";
        private string sportmonksApiToken = "?api_token=ygRCt7c4tAZSvzIc8hbBPMC0g3Evq7u4SEkB1J6trJYAjp090fBzjssfEgz5";
        RavenClient ravenClient = new RavenClient("https://d7435dc909824e839a8f4556615ee45b@sentry.io/1272852");

        public SportsApiService()
        {

        }

        public void UpdateFixturesBetweenDates(string fromDate, string toDate)
        {
            try
            {
                var betservice = new BetService();
                // fixtures/between/2018-08-11/2018-08-13?api_token=__TOKEN__&include=events
                // CL = 2, EL = 5, PL = 8, Eliteserien = 444
                string URL = baseUrl + "fixtures/between/" + fromDate + "/" + toDate;
                string urlParameters = sportmonksApiToken + "&leagues=2,5,8,444,12,24,82,301384,564";
                //string urlParameters = sportmonksApiToken + "&include=events&leagues=2,5,8,444,12,24,82,301384,564";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    var dataObjects = response.Content.ReadAsAsync<Models.SportsApiModels.Fixtures>().Result;
                    foreach (var apiFixture in dataObjects.data)
                    {
                        this.saveFixtureToDb(apiFixture);
                    }
                    db.SaveChanges();

                }
            }
            catch (Exception exception)
            {
                ravenClient.Capture(new SentryEvent(exception));
            }
        }

        public void UpdateAllFixtures(int apiSeasonUrl)
        {
            try
            {
                var betservice = new BetService();
                string URL = baseUrl + "seasons/" + apiSeasonUrl;
                string urlParameters = sportmonksApiToken + "&include=fixtures";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    var dataObjects = response.Content.ReadAsAsync<Models.SportsApiModels.Season>().Result;
                    foreach (var apiFixture in dataObjects.data.fixtures.data)
                    {
                        this.saveFixtureToDb(apiFixture);
                    }
                    db.SaveChanges();

                }
            }
            catch (Exception exception)
            {
                ravenClient.Capture(new SentryEvent(exception));
            }
        }

        public void SaveTeams(int apiSeasonUrl)
        {
            string URL = baseUrl + "teams/season/" + apiSeasonUrl;
            string urlParameters = sportmonksApiToken;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsAsync<Models.SportsApiModels.DataObject<Models.SportsApiModels.Team>>().Result;
                foreach (var apiTeam in dataObjects.data)
                {
                    var team = db.Teams.SingleOrDefault(t => t.apiId == apiTeam.id);
                    if (team == null)
                    {
                        team = new Team
                        {
                            apiId = apiTeam.id,
                            name = apiTeam.name,
                            logoUrl = apiTeam.logo_path
                        };
                        db.Teams.Add(team);
                    }
                }
                db.SaveChanges();

            }
        }

        private void saveFixtureToDb(Models.SportsApiModels.Fixture apiFixture)
        {
            var fixture = db.Fixtures.SingleOrDefault(f => f.apiId == apiFixture.id);
            if (fixture == null)
            {
                var localTeam = db.Teams.SingleOrDefault(t => t.apiId == apiFixture.localteam_id);
                var visitorTeam = db.Teams.SingleOrDefault(t => t.apiId == apiFixture.visitorteam_id);
                if (localTeam == null || visitorTeam == null)
                {
                    return;
                }
                fixture = new Fixture
                {
                    apiId = apiFixture.id,
                    localTeamId = localTeam.id,
                    visitorTeamId = visitorTeam.id,
                    localTeamApiId = apiFixture.localteam_id,
                    visitorTeamApiId = apiFixture.visitorteam_id,
                    seasonId = apiFixture.season_id,
                    stageId = apiFixture.stage_id,
                    roundId = apiFixture.round_id,
                    groupId = apiFixture.group_id,
                    venueId = apiFixture.venue_id,
                    refereeId = apiFixture.referee_id,
                };
                db.Fixtures.Add(fixture);
            }
            fixture.date = DateTime.Parse(apiFixture.time.starting_at.date);
            fixture.startingAt = DateTime.Parse(apiFixture.time.starting_at.date_time);
            fixture.updatedAt = DateTime.Now;
            fixture.scores = new JavaScriptSerializer().Serialize(apiFixture.scores);
            fixture.time = new JavaScriptSerializer().Serialize(apiFixture.time);
            fixture.standings = new JavaScriptSerializer().Serialize(apiFixture.standings);
            //fixture.events = new JavaScriptSerializer().Serialize(apiFixture.events);

            var homeScore = apiFixture.scores.localteam_score;
            var awayScore = apiFixture.scores.visitorteam_score;

            // Update bets if finished
            if (fixture.status != "FT" && apiFixture.time.status == "FT")
            {
                var fixtureMessage = fixture.LocalTeam.name + " vs " + fixture.VisitorTeam.name;
                foreach (var bet in fixture.Bets)
                {
                    bet.status = "FINISHED";
                    var winner = "NONE";
                    var winnerValue = "";
                    if (homeScore == awayScore) // Tie
                    {
                        winnerValue = "x";
                    }
                    else if (homeScore > awayScore) // Home win
                    {
                        winnerValue = "h";
                    }
                    else if (homeScore < awayScore) // Away win
                    {
                        winnerValue = "a";
                    }
                    foreach (var betObject in bet.FromBetObjects)
                    {
                        if (betObject.value == winnerValue)
                        {
                            betObject.status = "WON";
                            winner = "FROM_USER";
                        }
                        else
                        {
                            betObject.status = "LOST";
                        }
                    }
                    foreach (var betObject in bet.ToBetObjects)
                    {
                        if (betObject.value == winnerValue)
                        {
                            betObject.status = "WON";
                            winner = "TO_USER";
                        }
                        else
                        {
                            betObject.status = "LOST";
                        }
                    }
                    if (winner == "FROM_USER")
                    {
                        bet.winnerId = bet.fromUserId;
                        PushNotificationService.SendNotification(bet.ToUser.username, fixtureMessage + ": Full time. You won", bet.fromUserId);
                        PushNotificationService.SendNotification(bet.FromUser.username, fixtureMessage + ": Full time. You lost", bet.toUserId);
                    }
                    else if (winner == "TO_USER")
                    {
                        bet.winnerId = bet.toUserId;
                        PushNotificationService.SendNotification(bet.FromUser.username, fixtureMessage + ": Full time. You won", bet.toUserId);
                        PushNotificationService.SendNotification(bet.ToUser.username, fixtureMessage + ": Full time. You lost", bet.fromUserId);
                    }
                    else if (winner == "NONE")
                    {
                        PushNotificationService.SendNotification(bet.FromUser.username, fixtureMessage + ": Full time. Neither of you won", bet.toUserId);
                        PushNotificationService.SendNotification(bet.ToUser.username, fixtureMessage + ": Full time. Neither of you won", bet.fromUserId);
                    }
                }
            }
            fixture.status = apiFixture.time.status;
        }
    }
}