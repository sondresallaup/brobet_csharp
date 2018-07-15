﻿using Brobet.Services;
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
            vm.fixture = new FixtureViewModel
            {
                id = bet.Fixture.id,
                localTeamName = bet.Fixture.LocalTeam.name,
                visitorTeamName = bet.Fixture.VisitorTeam.name
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
                visitorTeamName = betRequest.Fixture.VisitorTeam.name
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
                visitorTeamName = betRequest.Fixture.VisitorTeam.name
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