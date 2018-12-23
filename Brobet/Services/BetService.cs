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

        public BetViewModel GetBet(int id)
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var bet = db.Bets.SingleOrDefault(b => b.id == id);
            return new BetViewModel
            {
                id = bet.id,
                Fixture = bet.Fixture,
                Friend = currentUser.userId == bet.fromUserId ? bet.ToUser : bet.FromUser,
                FromUser = bet.FromUser,
                ToUser = bet.ToUser,
                fromAmount = bet.fromAmount,
                toAmount = bet.toAmount,
                fromBetObjects = bet.FromBetObjects.ToList(),
                toBetObjects = bet.ToBetObjects.ToList(),
                status = bet.status,
                Winner = bet.Winner,
                winnerString = bet.Winner != null ? (bet.Winner.userId == currentUser.userId ? "CURRENT_USER" : "OTHER_USER") : "NONE",
                payed = bet.payed.HasValue && bet.payed.Value
            };
        }

        public BetViewModel GetBetRequest(int id)
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var betRequest = db.BetRequests.SingleOrDefault(b => b.id == id);
            return new BetViewModel
            {
                id = betRequest.id,
                Fixture = betRequest.Fixture,
                Friend = currentUser.userId == betRequest.fromUserId ? betRequest.ToUser : betRequest.FromUser,
                FromUser = betRequest.FromUser,
                ToUser = betRequest.ToUser,
                fromAmount = betRequest.fromAmount,
                toAmount = betRequest.toAmount,
                fromBetObjects = betRequest.FromBetObjects.ToList(),
                toBetObjects = betRequest.ToBetObjects.ToList()
            };
        }

        public List<BetViewModel> GetBets()
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var sent = currentUser.SentBets.Select(b => new BetViewModel
            {
                id = b.id,
                fromAmount = b.fromAmount,
                toAmount = b.toAmount,
                status = b.status,
                Fixture = b.Fixture,
                Friend = b.ToUser,
                date = b.date,
                isFromCurrentUser = true,
                Winner = b.Winner,
                winnerString = b.Winner != null ? (b.Winner.userId == currentUser.userId ? "CURRENT_USER" : "OTHER_USER") : "NONE",
                CurrentUserBetObjects = b.FromBetObjects.ToList(),
                FromBetObjects = b.FromBetObjects.ToList(),
                ToBetObjects = b.ToBetObjects.ToList(),
                //bet = b.initiatorBet,
                hasWon = b.winnerId != null ? b.winnerId == currentUser.userId : false
            }).ToList();


            var received = currentUser.ReceivedBets.Select(b => new BetViewModel
            {
                id = b.id,
                fromAmount = b.fromAmount,
                toAmount = b.toAmount,
                status = b.status,
                Fixture = b.Fixture,
                Friend = b.FromUser,
                date = b.date,
                Winner = b.Winner,
                winnerString = b.Winner != null ? (b.Winner.userId == currentUser.userId ? "CURRENT_USER" : "OTHER_USER") : "NONE",
                isFromCurrentUser = false,
                CurrentUserBetObjects = b.ToBetObjects.ToList(),
                FromBetObjects = b.FromBetObjects.ToList(),
                ToBetObjects = b.ToBetObjects.ToList(),
                //bet = this.OppositeBet(b.initiatorBet),
                hasWon = b.winnerId != null ? b.winnerId == currentUser.userId : false
            }).ToList();

            sent.AddRange(received);

            return sent;
        }

        public class BetViewModel
        {
            public int id { get; set; }
            public int fromAmount { get; set; }
            public int toAmount { get; set; }
            public string status { get; set; }
            public string bet { get; set; }
            public bool payed { get; set; }
            public DateTime date { get; set; }
            public Fixture Fixture { get; set; }
            public User Friend { get; set; }
            public User FromUser { get; set; }
            public User ToUser { get; set; }
            public User Winner { get; set; }
            public string winnerString { get; set; }
            public List<BetObject> CurrentUserBetObjects { get; set; }
            public List<BetObject> FromBetObjects { get; set; }
            public List<BetObject> ToBetObjects { get; set; }
            public bool hasWon { get; set; }
            public bool isFromCurrentUser { get; set; }
            public List<BetObject> fromBetObjects { get; set; }
            public List<BetObject> toBetObjects { get; set; }
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
            var accountService = new AccountServices();
            var currentUserId = accountService.GetCurrentUserId();

            var request = db.BetRequests.SingleOrDefault(br => br.id == requestId);
            if(request.toUserId != currentUserId)
            {
                return "UNAUTHORIZED";
            }
            if(request.Fixture.status != "NS")
            {
                return "LIVEBET_NOT_SUPPORTED";
            }
            request.accepted = true;

            var bet = new Bet
            {
                fromUserId = request.fromUserId,
                toUserId = request.toUserId,
                fixtureId = request.fixtureId,
                FromBetObjects = request.FromBetObjects,
                ToBetObjects = request.ToBetObjects,
                fromAmount = request.fromAmount,
                toAmount = request.toAmount,
                date = DateTime.Now,
                status = "NS"
            };
            db.Bets.Add(bet);


            db.SaveChanges();
            var fixture = request.Fixture;
            var messageContent = "Accepted bet: " + fixture.LocalTeam.name + " vs " + fixture.VisitorTeam.name;
            PushNotificationService.SendNotification(request.ToUser.username, messageContent, request.FromUser.userId);


            return "SUCCESS";
        }

        public string Pay(int betId)
        {
            var accountService = new AccountServices();
            var currentUserId = accountService.GetCurrentUserId();

            var bet = db.Bets.SingleOrDefault(b => b.id == betId);

            if(bet.payed.HasValue && bet.payed.Value)
            {
                return "BET_ALREADY_PAID";
            }

            if (bet.toUserId != currentUserId && bet.fromUserId != currentUserId)
            {
                return "UNAUTHORIZED";
            }
            //var amount = 0;
            var isFromUser = bet.fromUserId == currentUserId;
            var otherUser = bet.FromUser;
            if (isFromUser)
            {
                //amount = bet.fromAmount;
                otherUser = bet.ToUser;
            }
            //else
            //{
            //    amount = bet.toAmount;
            //}
            //if(amount > accountService.GetAccountBalance())
            //{
            //    return "NOT_ENOUGH_MONEY";
            //}

            //var currentUserTransaction = new Transaction
            //{
            //    amount = (amount * -1),
            //    description = "Bet lost",
            //    betId = bet.id,
            //    userId = accountService.GetCurrentUserId(),
            //    date = DateTime.Now
            //};
            //db.Transactions.Add(currentUserTransaction);
            //db.SaveChanges();

            //var otherUserTransaction = new Transaction
            //{
            //    amount = amount,
            //    description = "Bet won",
            //    betId = bet.id,
            //    date = DateTime.Now
            //};
            //otherUser.Transactions.Add(otherUserTransaction);
            bet.payed = true;
            db.SaveChanges();
            var fixture = bet.Fixture;
            var messageContent = "Paid bet: " + fixture.LocalTeam.name + " vs " + fixture.VisitorTeam.name;

            PushNotificationService.SendNotification(accountService.GetCurrentUserName(), messageContent, otherUser.userId);
            return "SUCCESS";
        }

        public string CreateBetRequest(int toUserId, int fixtureId, int fromAmount, int toAmount, string[] fromBets, string[] toBets)
        {
            var accountService = new AccountServices();
            var fromUser = accountService.GetCurrentUser();

            var fixture = db.Fixtures.SingleOrDefault(f => f.id == fixtureId);
            if(fixture.status != "NS")
            {
                return "LIVEBET_NOT_SUPPORTED";
            }

            var betRequest = new BetRequest
            {
                fromUserId = fromUser.userId,
                toUserId = toUserId,
                fixtureId = fixtureId,
                fromAmount = fromAmount,
                toAmount = toAmount,
                accepted = false,
                date = DateTime.Now
            };
            db.BetRequests.Add(betRequest);

            var betType = db.BetTypes.SingleOrDefault(bt => bt.type == "FULL_TIME");
            foreach (var betValue in fromBets)
            {
                var betObject = new BetObject
                {
                    BetType = betType,
                    FromBetRequest = betRequest,
                    value = betValue
                };
                db.BetObjects.Add(betObject);
            }
            foreach (var betValue in toBets)
            {
                var betObject = new BetObject
                {
                    BetType = betType,
                    ToBetRequest = betRequest,
                    value = betValue
                };
                db.BetObjects.Add(betObject);
            }


            db.SaveChanges();

            var messageContent = "Bet request: " + fixture.LocalTeam.name + " vs " + fixture.VisitorTeam.name;
            PushNotificationService.SendNotification(fromUser.username, messageContent, toUserId);

            return "SUCCESS";
        }
    }
}