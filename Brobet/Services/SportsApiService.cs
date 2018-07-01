using Brobet.Models;
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

        public SportsApiService()
        {

        }

        public void SaveFixtures(int apiSeasonUrl)
        {
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
                    var fixture = db.Fixtures.SingleOrDefault(f => f.apiId == apiFixture.id);
                    if (fixture == null)
                    {
                        var localTeam = db.Teams.SingleOrDefault(t => t.apiId == apiFixture.localteam_id);
                        var visitorTeam = db.Teams.SingleOrDefault(t => t.apiId == apiFixture.visitorteam_id);
                        if(localTeam == null || visitorTeam == null)
                        {
                            continue;
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
                    fixture.status = apiFixture.time.status;
                    fixture.startingAt = DateTime.Parse(apiFixture.time.starting_at.date_time);
                    fixture.updatedAt = DateTime.Now;
                    fixture.scores = new JavaScriptSerializer().Serialize(apiFixture.scores);
                    fixture.time = new JavaScriptSerializer().Serialize(apiFixture.time);
                    fixture.standings = new JavaScriptSerializer().Serialize(apiFixture.standings);

                }
                db.SaveChanges();

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
    }
}