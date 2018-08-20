using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace Brobet.Services
{
    public class AccountServices
    {
        private Entities db = new Entities();

        public AccountServices()
        {

        }

        public bool isLoggedIn()
        {
            return WebSecurity.IsAuthenticated && this.GetCurrentUserId() > 0;
        }

        public int GetCurrentUserId()
        {
            return WebSecurity.CurrentUserId;
        }

        public int GetUserId(string username)
        {
            return WebSecurity.GetUserId(username);
        }

        public string GetCurrentUserName()
        {
            return WebSecurity.CurrentUserName;
        }

        public User GetCurrentUser()
        {
            var currentUserId = this.GetCurrentUserId();
            return db.Users.SingleOrDefault(u => u.userId == currentUserId);
        }

        public string GetAvatar(int userId)
        {
            return this.GetUser(userId).avatarUrl ?? "https://avataaars.io/?avatarStyle=Transparent&clotheType=Hoodie&clotheColor=Blue03";
        }

        public bool isUserNameInUse(string username)
        {
            return WebSecurity.UserExists(username);
        }

        public int createUserLogin(string username, string password)
        {
            WebSecurity.CreateUserAndAccount(username, password);
            var userId = WebSecurity.GetUserId(username);
            // 1000 kr sign up bonus
            var transaction = new Transaction
            {
                userId = userId,
                amount = 1000,
                date = DateTime.Now,
                description = "Sign up bonus"
            };
            db.Transactions.Add(transaction);
            db.SaveChanges();
            this.Login(username, password);
            return userId;
        }

        public void Login(string username, string password)
        {
            WebSecurity.Login(username, password);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public User GetUser(int userId)
        {
            return db.Users.SingleOrDefault(u => u.userId == userId);
        }

        public bool AcceptFriendRequest(int otherUserId)
        {
            var currentUser = this.GetCurrentUser();
            var friendRequest = db.FriendRequests.SingleOrDefault(fr => fr.toUserId == currentUser.userId && fr.fromUserId == otherUserId);
            if(friendRequest == null)
            {
                return false;
            }
            friendRequest.accepted = true;

            var friendship = new Friendship
            {
                fromUserId = otherUserId,
                toUserId = currentUser.userId,
                date = DateTime.Now
            };
            db.Friendships.Add(friendship);
            db.SaveChanges();

            var messageContent = "Accepted friend request";
            PushNotificationService.SendNotification(currentUser.username, messageContent, otherUserId);

            return true;
        }

        public bool SendFriendRequest(string toUserName)
        {
            var fromUser = this.GetCurrentUser();
            var toUser = db.Users.SingleOrDefault(u => u.username == toUserName);

            if (toUser == null)
            {
                return false;
            }
            if (fromUser.userId == toUser.userId)
            {
                return false;
            }
            if(HasFriendRequestWithUser(toUser))
            {
                return false;
            }
            var request = new FriendRequest
            {
                fromUserId = fromUser.userId,
                toUserId = toUser.userId,
                date = DateTime.Now,
                accepted = false
            };
            db.FriendRequests.Add(request);
            db.SaveChanges();

            var messageContent = "Friend request";
            PushNotificationService.SendNotification(fromUser.username, messageContent, toUser.userId);

            return true;
        }

        public void SaveAvatar(string avatarUrl)
        {
            var currentUser = this.GetCurrentUser();
            currentUser.avatarUrl = avatarUrl;
            this.db.SaveChanges();
        }

        public bool HasFriendRequestWithUser(User user)
        {
            var fromUser = this.GetCurrentUser();
            return (db.FriendRequests.Any(fr => fr.fromUserId == fromUser.userId && fr.toUserId == user.userId) || db.FriendRequests.Any(fr => fr.fromUserId == fromUser.userId && fr.toUserId == user.userId));
        }

        public List<User> GetFriends()
        {
            var user = this.GetCurrentUser();
            var sent = user.SentFriendships.Select(f => new User
            {
                userId = f.toUserId,
                username = f.ToUser.username
            }).ToList();
            var received = user.ReceivedFriendships.Select(f => new User
            {
                userId = f.fromUserId,
                username = f.FromUser.username
            }).ToList();
            sent.AddRange(received);
            return sent;
        }

        public List<FriendRequest> GetSentFriendRequests()
        {
            var user = this.GetCurrentUser();
            return user.SentFriendRequests.Where(fr => !fr.accepted).ToList();
        }

        public List<FriendRequest> GetReceivedFriendRequests()
        {
            var user = this.GetCurrentUser();
            return user.ReceivedFriendRequests.Where(fr => !fr.accepted).ToList();
        }

        public List<Transaction> GetTransactions()
        {
            var user = this.GetCurrentUser();
            return user.Transactions.OrderByDescending(t => t.date).ToList();
        }

        public void FillAccount(int amount)
        {
            var user = this.GetCurrentUser();
            var transaction = new Transaction
            {
                amount = amount,
                date = DateTime.Now,
                description = "Credit card"
            };
            user.Transactions.Add(transaction);
            db.SaveChanges();
        }

        public decimal GetAccountBalance()
        {
            var transactions = this.GetTransactions();
            var balance = transactions.Sum(t => t.amount);
            return balance;
        }
    }
}