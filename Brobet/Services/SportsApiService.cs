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

        public void UpdateFixtures(int apiSeasonUrl)
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
                    fixture.startingAt = DateTime.Parse(apiFixture.time.starting_at.date_time);
                    fixture.updatedAt = DateTime.Now;
                    fixture.scores = new JavaScriptSerializer().Serialize(apiFixture.scores);
                    fixture.time = new JavaScriptSerializer().Serialize(apiFixture.time);
                    fixture.standings = new JavaScriptSerializer().Serialize(apiFixture.standings);

                    var homeScore = apiFixture.scores.localteam_score;
                    var awayScore = apiFixture.scores.visitorteam_score;

                    // Update bets if finished
                    if (fixture.status != "FT" && apiFixture.time.status == "FT")
                    {
                        foreach(var bet in fixture.Bets)
                        {
                            bet.status = "FINISHED";
                            var winner = "NONE";
                            if(homeScore == awayScore) // Tie
                            {
                                continue; // Ties are not yet implemented
                            }
                            else if(homeScore > awayScore) // Home win
                            {
                                if(bet.initiatorBet == "HOME") // From user has won
                                {
                                    winner = "FROM_USER";
                                }
                                else if(bet.initiatorBet == "AWAY") // To user has won
                                {
                                    winner = "TO_USER";
                                }
                            }
                            else if(homeScore < awayScore) // Away win
                            {
                                if (bet.initiatorBet == "HOME") // To user has won
                                {
                                    winner = "TO_USER";
                                }
                                else if (bet.initiatorBet == "AWAY") // From user has won
                                {
                                    winner = "FROM_USER";
                                }
                            }
                            if(winner == "FROM_USER")
                            {
                                bet.winnerId = bet.fromUserId;
                                // Tranfer money to from user
                                var betStr = bet.initiatorBet;
                                var amount = betservice.UserBetAmount(betStr, bet.homeAmount, bet.awayAmount);
                                var transaction = new Transaction
                                {
                                    userId = bet.fromUserId,
                                    amount = amount,
                                    date = DateTime.Now,
                                    description = "You have won!",
                                    betId = bet.id
                                };
                                db.Transactions.Add(transaction);
                            }
                            else if(winner == "TO_USER")
                            {
                                bet.winnerId = bet.toUserId;
                                // Tranfer money to to user
                                var betStr = betservice.OppositeBet(bet.initiatorBet);
                                var amount = betservice.UserBetAmount(betStr, bet.homeAmount, bet.awayAmount);
                                var transaction = new Transaction
                                {
                                    userId = bet.toUserId,
                                    amount = amount,
                                    date = DateTime.Now,
                                    description = "You have won!",
                                    betId = bet.id
                                };
                                db.Transactions.Add(transaction);
                            }
                        }
                    }

                    // Return money for unaccepted bet requests
                    if(fixture.status != "LIVE" && apiFixture.time.status == "LIVE")
                    {
                        foreach(var request in fixture.BetRequests.Where(br => !br.accepted))
                        {
                            var requestTransaction = request.Transactions.SingleOrDefault();
                            requestTransaction.description = "Bet request not accpeted. Money is returned";
                            var returnTransaction = new Transaction
                            {
                                userId = requestTransaction.userId,
                                amount = (requestTransaction.amount * -1),
                                date = DateTime.Now,
                                betRequestId = requestTransaction.betRequestId,
                                description = "Returned money ref. transaction id: " + requestTransaction.id
                            };
                            db.Transactions.Add(returnTransaction);
                        }
                    }
                    fixture.status = apiFixture.time.status;

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