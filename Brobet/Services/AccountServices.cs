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

        public string GetCurrentUserName()
        {
            return WebSecurity.CurrentUserName;
        }

        public User GetCurrentUser()
        {
            var currentUserId = this.GetCurrentUserId();
            return db.Users.SingleOrDefault(u => u.userId == currentUserId);
        }

        public bool isUserNameInUse(string username)
        {
            return WebSecurity.UserExists(username);
        }

        public int createUserLogin(string username, string password)
        {
            WebSecurity.CreateUserAndAccount(username, password);
            var userId = WebSecurity.GetUserId(username);
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

            return true;
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
    }
}