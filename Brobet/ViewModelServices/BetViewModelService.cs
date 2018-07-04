using Brobet.Services;
using Brobet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModelServices
{
    public class BetViewModelService
    {
        public BetViewModelService()
        {

        }

        public BetListViewModel GetBetListViewModel()
        {
            var vm = new BetListViewModel();
            var service = new BetService();

            vm.bets = service.GetBets().Select(bt => new BetListViewModel.BetRequest
            {
                id = bt.id,
                friendName = bt.Friend.username,
                bet = bt.bet,
                status = bt.status,
                homeAmount = bt.homeAmount,
                awayAmount = bt.awayAmount,
                fixtureTitle = bt.Fixture.LocalTeam.name + " vs " + bt.Fixture.VisitorTeam.name,
                hasWon = bt.hasWon
            }).ToList();
            
            vm.sentBetRequests = service.GetSentBetRequests().Select(bt => new BetListViewModel.BetRequest
            {
                id = bt.id,
                friendName = bt.ToUser.username,
                bet = bt.initiatorBet,
                homeAmount = bt.homeAmount,
                awayAmount = bt.awayAmount,
                fixtureTitle = bt.Fixture.LocalTeam.name + " vs " + bt.Fixture.VisitorTeam.name
            }).ToList();

            vm.receivedBetRequests = service.GetReceivedBetRequests().Select(bt => new BetListViewModel.BetRequest
            {
                id = bt.id,
                friendName = bt.FromUser.username,
                bet = service.OppositeBet(bt.initiatorBet),
                homeAmount = bt.homeAmount,
                awayAmount = bt.awayAmount,
                fixtureTitle = bt.Fixture.LocalTeam.name + " vs " + bt.Fixture.VisitorTeam.name
            }).ToList();
            return vm;
        }
    }
}