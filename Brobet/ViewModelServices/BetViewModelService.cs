using Brobet.Services;
using Brobet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Brobet.ViewModelServices
{
    public class BetViewModelService
    {
        public BetViewModelService()
        {

        }

        public BetOverviewViewModel GetBetOverviewViewModel()
        {
            var vm = new BetOverviewViewModel();
            var service = new BetService();
            vm.activeBets = service.GetBets().Where(b => b.status != "FINISHED").OrderByDescending(b => b.date).Select(b => new BetOverviewViewModel.Bet
            {
                id = b.id,
                isFromCurrentUser = b.isFromCurrentUser,
                currentUserBetObjects = b.CurrentUserBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                fromUserBetObjects = b.FromBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                toUserBetObjects = b.ToBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                fromAmount = b.fromAmount,
                toAmount = b.toAmount,
                friend = new BetOverviewViewModel.Friend
                {
                    id = b.Friend.userId,
                    username = b.Friend.username
                },
                fixture = new BetOverviewViewModel.Fixture
                {
                    homeTeam = new BetOverviewViewModel.Fixture.Team
                    {
                        name = b.Fixture.LocalTeam.name
                    },
                    awayTeam = new BetOverviewViewModel.Fixture.Team
                    {
                        name = b.Fixture.VisitorTeam.name
                    }
                }
            }).ToList();
            vm.previousBets = service.GetBets().Where(b => b.status == "FINISHED").OrderByDescending(b => b.date).Select(b => new BetOverviewViewModel.Bet
            {
                id = b.id,
                isFromCurrentUser = b.isFromCurrentUser,
                winner = b.Winner?.userId,
                winnerString = b.winnerString,
                currentUserBetObjects = b.CurrentUserBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                fromUserBetObjects = b.FromBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                toUserBetObjects = b.ToBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                fromAmount = b.fromAmount,
                toAmount = b.toAmount,
                friend = new BetOverviewViewModel.Friend
                {
                    id = b.Friend.userId,
                    username = b.Friend.username,
                    avatar = b.Friend.avatarUrl
                },
                fixture = new BetOverviewViewModel.Fixture
                {
                    homeTeam = new BetOverviewViewModel.Fixture.Team
                    {
                        name = b.Fixture.LocalTeam.name,
                        score = Json.Decode(b.Fixture.scores)["localteam_score"]
                    },
                    awayTeam = new BetOverviewViewModel.Fixture.Team
                    {
                        name = b.Fixture.VisitorTeam.name,
                        score = Json.Decode(b.Fixture.scores)["visitorteam_score"]
                    }
                }
            }).ToList();
            vm.sentBetRequests = service.GetSentBetRequests().Where(b => !b.accepted && b.Fixture.status != "FT").OrderByDescending(b => b.date).Select(b => new BetOverviewViewModel.Bet
            {
                id = b.id,
                isFromCurrentUser = true,
                currentUserBetObjects = b.FromBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                fromUserBetObjects = b.FromBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                toUserBetObjects = b.ToBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                fromAmount = b.fromAmount,
                toAmount = b.toAmount,
                friend = new BetOverviewViewModel.Friend
                {
                    id = b.ToUser.userId,
                    username = b.ToUser.username,
                    avatar = b.ToUser.avatarUrl
                },
                fixture = new BetOverviewViewModel.Fixture
                {
                    homeTeam = new BetOverviewViewModel.Fixture.Team
                    {
                        name = b.Fixture.LocalTeam.name
                    },
                    awayTeam = new BetOverviewViewModel.Fixture.Team
                    {
                        name = b.Fixture.VisitorTeam.name
                    }
                }
            }).ToList();
            vm.receivedBetRequests = service.GetReceivedBetRequests().Where(b => !b.accepted && b.Fixture.status != "FT").OrderByDescending(b => b.date).Select(b => new BetOverviewViewModel.Bet
            {
                id = b.id,
                isFromCurrentUser = false,
                currentUserBetObjects = b.ToBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                fromUserBetObjects = b.FromBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                toUserBetObjects = b.ToBetObjects.Select(cbo => new BetOverviewViewModel.Bet.BetObject
                {
                    type = cbo.betTypeId,
                    value = cbo.value
                }).ToList(),
                fromAmount = b.fromAmount,
                toAmount = b.toAmount,
                friend = new BetOverviewViewModel.Friend
                {
                    id = b.FromUser.userId,
                    username = b.FromUser.username,
                    avatar = b.FromUser.avatarUrl
                },
                fixture = new BetOverviewViewModel.Fixture
                {
                    homeTeam = new BetOverviewViewModel.Fixture.Team
                    {
                        name = b.Fixture.LocalTeam.name
                    },
                    awayTeam = new BetOverviewViewModel.Fixture.Team
                    {
                        name = b.Fixture.VisitorTeam.name
                    }
                }
            }).ToList();
            return vm;
        }

        public BetRequestViewModel GetCreateBetRequestViewModel(int fixtureId)
        {
            var vm = new BetRequestViewModel();
            var service = new BetService();
            var fixtureService = new FixtureService();
            var accountService = new AccountServices();
            vm.initial = true;
            vm.isFromUser = true;
            vm.editable = true;
            vm.backUrl = "/";
            vm.fixture = fixtureService.GetFixture(fixtureId);
            vm.friends = accountService.GetFriends().Select(fr => new BetRequestViewModel.Friend
            {
                userId = fr.userId,
                username = fr.username
            }).ToList();

            return vm;
        }

        public BetRequestViewModel GetBetDetailsViewModel(int betId)
        {
            var vm = new BetRequestViewModel();
            var service = new BetService();
            var fixtureService = new FixtureService();
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var bet = service.GetBet(betId);
            vm.initial = false;
            vm.isFromUser = currentUser.userId == bet.FromUser.userId;
            vm.editable = false;
            vm.backUrl = "/";
            vm.betId = betId;
            vm.isPayed = bet.payed;
            vm.fixture = new FixtureViewModel
            {
                id = bet.Fixture.id,
                localTeamName = bet.Fixture.LocalTeam.name,
                visitorTeamName = bet.Fixture.VisitorTeam.name,
                localTeamLogo = bet.Fixture.LocalTeam.logoUrl,
                visitorTeamLogo = bet.Fixture.VisitorTeam.logoUrl,
                status = bet.Fixture.status,
                scores = bet.Fixture.scores,
                startingAt = bet.Fixture.startingAt,
                homeOdds = bet.Fixture.homeOdds,
                drawOdds = bet.Fixture.drawOdds,
                awayOdds = bet.Fixture.awayOdds,
                startingAtAsString = bet.Fixture.startingAt.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
            };
            vm.friend = new BetRequestViewModel.Friend
            {
                userId = bet.Friend.userId,
                username = bet.Friend.username
            };
            vm.fromAmount = bet.fromAmount;
            vm.toAmount = bet.toAmount;

            vm.fromBets = bet.fromBetObjects.Select(o => o.value).ToArray();
            vm.toBets = bet.toBetObjects.Select(o => o.value).ToArray();

            if(bet.status == "FINISHED")
            {
                string winningBetObject = null;
                if(bet.fromBetObjects.Any(o => o.status == "WON"))
                {
                    winningBetObject = bet.fromBetObjects.FirstOrDefault(o => o.status == "WON").value;
                }
                if (bet.toBetObjects.Any(o => o.status == "WON"))
                {
                    winningBetObject = bet.toBetObjects.FirstOrDefault(o => o.status == "WON").value;
                }
                vm.winningBetObject = winningBetObject;

                if(bet.Winner != null)
                {
                    if(bet.Winner.userId == currentUser.userId) // Current user won
                    {
                        vm.winner = "CURRENT_USER";
                    }
                    else
                    {
                        vm.winner = "OTHER_USER";
                    }
                }
                else // Neither won
                {
                    vm.winner = "NO_ONE";
                }
            }

            return vm;
        }

        public BetRequestViewModel GetSentBetRequestViewModel(int betRequestId)
        {
            var vm = new BetRequestViewModel();
            var service = new BetService();
            var fixtureService = new FixtureService();
            var accountService = new AccountServices();
            vm.initial = false;
            vm.isFromUser = true;
            vm.editable = false;
            vm.backUrl = "/";
            var betRequest = service.GetBetRequest(betRequestId);
            vm.betRequestId = betRequestId;
            vm.fixture = new FixtureViewModel
            {
                id = betRequest.Fixture.id,
                localTeamName = betRequest.Fixture.LocalTeam.name,
                visitorTeamName = betRequest.Fixture.VisitorTeam.name,
                localTeamLogo = betRequest.Fixture.LocalTeam.logoUrl,
                visitorTeamLogo = betRequest.Fixture.VisitorTeam.logoUrl,
                status = betRequest.Fixture.status,
                scores = betRequest.Fixture.scores,
                homeOdds = betRequest.Fixture.homeOdds,
                drawOdds = betRequest.Fixture.drawOdds,
                awayOdds = betRequest.Fixture.awayOdds,
                startingAt = betRequest.Fixture.startingAt,
                startingAtAsString = betRequest.Fixture.startingAt.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
            };
            vm.friend = new BetRequestViewModel.Friend
            {
                userId = betRequest.Friend.userId,
                username = betRequest.Friend.username
            };
            vm.fromAmount = betRequest.fromAmount;
            vm.toAmount = betRequest.toAmount;

            vm.fromBets = betRequest.fromBetObjects.Select(o => o.value).ToArray();
            vm.toBets = betRequest.toBetObjects.Select(o => o.value).ToArray();

            return vm;
        }

        public BetRequestViewModel GetReceivedBetRequestViewModel(int betRequestId)
        {
            var vm = new BetRequestViewModel();
            var service = new BetService();
            var fixtureService = new FixtureService();
            var accountService = new AccountServices();
            vm.initial = false;
            vm.isFromUser = false;
            vm.editable = true;
            vm.backUrl = "/";
            var betRequest = service.GetBetRequest(betRequestId);
            vm.betRequestId = betRequestId;
            vm.fixture = new FixtureViewModel
            {
                id = betRequest.Fixture.id,
                localTeamName = betRequest.Fixture.LocalTeam.name,
                visitorTeamName = betRequest.Fixture.VisitorTeam.name,
                localTeamLogo = betRequest.Fixture.LocalTeam.logoUrl,
                visitorTeamLogo = betRequest.Fixture.VisitorTeam.logoUrl,
                status = betRequest.Fixture.status,
                scores = betRequest.Fixture.scores,
                homeOdds = betRequest.Fixture.homeOdds,
                drawOdds = betRequest.Fixture.drawOdds,
                awayOdds = betRequest.Fixture.awayOdds,
                startingAt = betRequest.Fixture.startingAt,
                startingAtAsString = betRequest.Fixture.startingAt.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
            };
            vm.friend = new BetRequestViewModel.Friend
            {
                userId = betRequest.Friend.userId,
                username = betRequest.Friend.username
            };
            vm.fromAmount = betRequest.fromAmount;
            vm.toAmount = betRequest.toAmount;

            vm.fromBets = betRequest.fromBetObjects.Select(o => o.value).ToArray();
            vm.toBets = betRequest.toBetObjects.Select(o => o.value).ToArray();

            return vm;
        }

        public BetListViewModel GetBetListViewModel()
        {
            var vm = new BetListViewModel();
            var service = new BetService();

            vm.bets = service.GetBets().OrderByDescending(b => b.date).Select(bt => new BetListViewModel.BetRequest
            {
                id = bt.id,
                friendName = bt.Friend.username,
                //bet = bt.bet,
                status = bt.status,
                fromAmount = bt.fromAmount,
                toAmount = bt.toAmount,
                fixtureTitle = bt.Fixture.LocalTeam.name + " vs " + bt.Fixture.VisitorTeam.name,
                hasWon = bt.hasWon
            }).ToList();
            
            vm.sentBetRequests = service.GetSentBetRequests().OrderByDescending(b => b.date).Select(bt => new BetListViewModel.BetRequest
            {
                id = bt.id,
                friendName = bt.ToUser.username,
                //bet = bt.initiatorBet,
                fromAmount = bt.fromAmount,
                toAmount = bt.toAmount,
                fixtureTitle = bt.Fixture.LocalTeam.name + " vs " + bt.Fixture.VisitorTeam.name
            }).ToList();

            vm.receivedBetRequests = service.GetReceivedBetRequests().OrderByDescending(b => b.date).Select(bt => new BetListViewModel.BetRequest
            {
                id = bt.id,
                friendName = bt.FromUser.username,
                //bet = service.OppositeBet(bt.initiatorBet),
                fromAmount = bt.fromAmount,
                toAmount = bt.toAmount,
                fixtureTitle = bt.Fixture.LocalTeam.name + " vs " + bt.Fixture.VisitorTeam.name
            }).ToList();
            return vm;
        }
    }
}