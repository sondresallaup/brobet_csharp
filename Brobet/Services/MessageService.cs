using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.Services
{
    public class MessageService
    {
        private Entities db = new Entities();

        public List<Friendship> GetFriendships()
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var sentFriendships = currentUser.SentFriendships.ToList();
            var receivedFriendships = currentUser.ReceivedFriendships.ToList();
            sentFriendships.AddRange(receivedFriendships);
            return sentFriendships;
        }

        public Friendship GetFriendship(int id)
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var friendship = db.Friendships.SingleOrDefault(f => f.id == id);
            if(friendship.fromUserId != currentUser.userId && friendship.toUserId != currentUser.userId)
            {
                return null;
            }
            return friendship;
        }

        public List<ExtendedMessage> GetMessages(int friendshipId)
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var friendship = this.GetFriendship(friendshipId);
            if (friendship == null) return null;
            var friendId = friendship.fromUserId == currentUser.userId ? friendship.toUserId : friendship.fromUserId;
            var extendedMessages = new List<ExtendedMessage>();
            var messages = friendship.Messages.Select(m => new ExtendedMessage
            {
                id = m.id,
                isBet = false,
                isCurrentUser = m.senderId == currentUser.userId,
                messageContent = m.messageContent,
                date = m.date
            }).ToList();

            var sentBetRequests = currentUser.SentBetRequests.Where(b => b.toUserId == friendId && !b.accepted).Select(m => new ExtendedMessage
            {
                id = m.id,
                isBet = true,
                accepted = false,
                isCurrentUser = true,
                fixtureName = m.Fixture.LocalTeam.name + " vs " + m.Fixture.VisitorTeam.name,
                localTeamLogo = m.Fixture.LocalTeam.logoUrl,
                localTeamName = m.Fixture.LocalTeam.name,
                visitorTeamLogo = m.Fixture.VisitorTeam.logoUrl,
                visitorTeamName = m.Fixture.VisitorTeam.name,
                fixtureDate = m.Fixture.startingAt.Value,
                date = m.date,
                url = "/Bet/SentBetRequest/" + m.id
            }).ToList();

            var receivedBetRequests = currentUser.ReceivedBetRequests.Where(b => b.fromUserId == friendId && !b.accepted).Select(m => new ExtendedMessage
            {
                id = m.id,
                isBet = true,
                accepted = false,
                isCurrentUser = false,
                fixtureName = m.Fixture.LocalTeam.name + " vs " + m.Fixture.VisitorTeam.name,
                localTeamLogo = m.Fixture.LocalTeam.logoUrl,
                localTeamName = m.Fixture.LocalTeam.name,
                visitorTeamLogo = m.Fixture.VisitorTeam.logoUrl,
                visitorTeamName = m.Fixture.VisitorTeam.name,
                fixtureDate = m.Fixture.startingAt.Value,
                date = m.date,
                url = "/Bet/ReceivedBetRequest/" + m.id
            }).ToList();

            var sentBets = currentUser.SentBets.Where(b => b.toUserId == friendId).Select(m => new ExtendedMessage
            {
                id = m.id,
                isBet = true,
                accepted = true,
                isCurrentUser = true,
                fixtureName = m.Fixture.LocalTeam.name + " vs " + m.Fixture.VisitorTeam.name,
                localTeamLogo = m.Fixture.LocalTeam.logoUrl,
                localTeamName = m.Fixture.LocalTeam.name,
                visitorTeamLogo = m.Fixture.VisitorTeam.logoUrl,
                visitorTeamName = m.Fixture.VisitorTeam.name,
                fixtureDate = m.Fixture.startingAt.Value,
                date = m.date,
                url = "/Bet/BetDetails/" + m.id
            }).ToList();

            var receivedBets = currentUser.ReceivedBets.Where(b => b.fromUserId == friendId).Select(m => new ExtendedMessage
            {
                id = m.id,
                isBet = true,
                accepted = true,
                isCurrentUser = false,
                fixtureName = m.Fixture.LocalTeam.name + " vs " + m.Fixture.VisitorTeam.name,
                localTeamLogo = m.Fixture.LocalTeam.logoUrl,
                localTeamName = m.Fixture.LocalTeam.name,
                visitorTeamLogo = m.Fixture.VisitorTeam.logoUrl,
                visitorTeamName = m.Fixture.VisitorTeam.name,
                fixtureDate = m.Fixture.startingAt.Value,
                date = m.date,
                url = "/Bet/BetDetails/" + m.id
            }).ToList();

            extendedMessages.AddRange(messages);
            extendedMessages.AddRange(sentBetRequests);
            extendedMessages.AddRange(receivedBetRequests);
            extendedMessages.AddRange(sentBets);
            extendedMessages.AddRange(receivedBets);

            return extendedMessages.OrderBy(m => m.date).ToList();
        }

        public void ComposeMessage(int friendshipId, string messageContent)
        {
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var friendship = db.Friendships.SingleOrDefault(f => f.id == friendshipId);
            if (friendship.fromUserId != currentUser.userId && friendship.toUserId != currentUser.userId)
            {
                return;
            }
            var message = new Message
            {
                senderId = currentUser.userId,
                friendshipId = friendshipId,
                messageContent = messageContent,
                date = DateTime.Now
            };
            db.Messages.Add(message);
            db.SaveChanges();

            var toUserId = friendship.fromUserId;
            if(toUserId == currentUser.userId)
            {
                toUserId = friendship.toUserId;
            }

            PushNotificationService.SendNotification(currentUser.username, messageContent, toUserId);
        }

        public class ExtendedMessage
        {
            public int id { get; set; }
            public bool isBet { get; set; }
            public bool accepted { get; set; }
            public bool isCurrentUser { get; set; }
            public string messageContent { get; set; }
            public string fixtureName { get; set; }
            public string visitorTeamName { get; set; }
            public string localTeamName { get; set; }
            public string visitorTeamLogo { get; set; }
            public string localTeamLogo { get; set; }
            public DateTime fixtureDate { get; set; }
            public string url { get; set; }
            public DateTime date { get; set; }
        }
    }
}