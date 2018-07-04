using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.Services
{
    public class BetService
    {
        private Entities db = new Entities();

        public BetService()
        {

        }

        public List<BetViewModel> GetBets()
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var sent = currentUser.SentBets.Select(b => new BetViewModel
            {
                id = b.id,
                homeAmount = b.homeAmount,
                awayAmount = b.awayAmount,
                status = b.status,
                Fixture = b.Fixture,
                Friend = b.ToUser,
                bet = b.initiatorBet,
                hasWon = b.winnerId != null ? b.winnerId == currentUser.userId : false
            }).ToList();


            var received = currentUser.ReceivedBets.Select(b => new BetViewModel
            {
                id = b.id,
                homeAmount = b.homeAmount,
                awayAmount = b.awayAmount,
                status = b.status,
                Fixture = b.Fixture,
                Friend = b.FromUser,
                bet = this.OppositeBet(b.initiatorBet),
                hasWon = b.winnerId != null ? b.winnerId == currentUser.userId : false
            }).ToList();

            sent.AddRange(received);

            return sent;
        }

        public class BetViewModel
        {
            public int id { get; set; }
            public int homeAmount { get; set; }
            public int awayAmount { get; set; }
            public string status { get; set; }
            public string bet { get; set; }
            public Fixture Fixture { get; set; }
            public User Friend { get; set; }
            public bool hasWon { get; set; }
        }

        public List<BetRequest> GetSentBetRequests()
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            return currentUser.SentBetRequests.Where(br => !br.accepted).ToList();
        }

        public List<BetRequest> GetReceivedBetRequests()
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            return currentUser.ReceivedBetRequests.Where(br => !br.accepted).ToList();
        }

        public string AcceptRequest(int requestId)
        {
            // Take money from acceptor
            var accountService = new AccountServices();
            var fromUserId = accountService.GetCurrentUserId();

            var request = db.BetRequests.SingleOrDefault(br => br.id == requestId);
            if(request.toUserId != fromUserId)
            {
                return "UNAUTHORIZED";
            }
            request.accepted = true;

            var bet = new Bet
            {
                fromUserId = request.fromUserId,
                toUserId = request.toUserId,
                fixtureId = request.fixtureId,
                initiatorBet = request.initiatorBet,
                homeAmount = request.homeAmount,
                awayAmount = request.awayAmount,
                date = DateTime.Now,
                status = "NS"
            };
            db.Bets.Add(bet);
            db.SaveChanges();


            return "SUCCESS";
        }

        public string CreateBetRequest(int toUserId, int fixtureId, string initiatorBet, int homeAmount, int awayAmount)
        {
            // TODO: Take money from initiator
            var accountService = new AccountServices();
            var fromUserId = accountService.GetCurrentUserId();

            if(initiatorBet != "HOME" && initiatorBet != "AWAY")
            {
                return "ILLEGAL_BET";
            }

            var betRequest = new BetRequest
            {
                fromUserId = fromUserId,
                toUserId = toUserId,
                fixtureId = fixtureId,
                initiatorBet = initiatorBet,
                homeAmount = homeAmount,
                awayAmount = awayAmount,
                accepted = false,
                date = DateTime.Now
            };
            db.BetRequests.Add(betRequest);
            db.SaveChanges();

            return "SUCCESS";
        }

        public string OppositeBet(string initiatorBet)
        {
            if (initiatorBet == "HOME")
            {
                return "AWAY";
            }
            else if (initiatorBet == "AWAY")
            {
                return "HOME";
            }
            return null;
        }
    }
}