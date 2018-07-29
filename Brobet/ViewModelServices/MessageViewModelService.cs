using Brobet.Services;
using Brobet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModelServices
{
    public class MessageViewModelService
    {
        public MessageListViewModel GetMessageListViewModel()
        {
            var service = new MessageService();
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var vm = new MessageListViewModel();
            vm.friendships = service.GetFriendships().Select(f => new FriendshipViewModel
            {
                id = f.id,
                friendName = f.fromUserId == currentUser.userId ? f.ToUser.username : f.FromUser.username
            }).ToList();
            return vm;
        }

        public FriendshipViewModel GetFriendshipViewModel(int friendshipId)
        {
            var service = new MessageService();
            var accountService = new AccountServices();
            var currentUser = accountService.GetCurrentUser();
            var friendship = service.GetFriendship(friendshipId);
            if(friendship == null)
            {
                return null;
            }
            var vm = new FriendshipViewModel();
            vm.id = friendship.id;
            vm.friendName = friendship.fromUserId == currentUser.userId ? friendship.ToUser.username : friendship.FromUser.username;
            vm.messages = service.GetMessages(friendshipId).Select(m => new MessageViewModel
            {
                id = m.id,
                messageContent = m.messageContent,
                isCurrentUser = m.isCurrentUser,
                isBet = m.isBet,
                accepted = m.accepted,
                fixtureName = m.fixtureName,
                localTeamLogo = m.localTeamLogo,
                localTeamName = m.localTeamName,
                visitorTeamLogo = m.visitorTeamLogo,
                visitorTeamName = m.visitorTeamName,
                fixtureDate = m.fixtureDate,
                url = m.url
            }).ToList();
            return vm;
        }
    }
}