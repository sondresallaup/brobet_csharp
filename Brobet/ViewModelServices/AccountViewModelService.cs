﻿using Brobet.Services;
using Brobet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModelServices
{
    public class AccountViewModelService
    {
        public AccountViewModel GetMeAccountViewModel()
        {
            var service = new AccountServices();
            return new AccountViewModel
            {
                isMe = true,
                userId = service.GetCurrentUserId(),
                username = service.GetCurrentUserName(),
                friends = service.GetFriends().Select(fr => new AccountViewModel.Friend
                {
                    userId = fr.userId,
                    username = fr.username
                }).ToList(),
                sentFriendRequests = service.GetSentFriendRequests().Select(fr => new AccountViewModel.Friend
                {
                    userId = fr.toUserId,
                    username = fr.ToUser.username,
                    dateString = fr.date.ToShortDateString()
                }).ToList(),
                receivedFriendRequests = service.GetReceivedFriendRequests().Select(fr => new AccountViewModel.Friend
                {
                    userId = fr.fromUserId,
                    username = fr.FromUser.username,
                    dateString = fr.date.ToShortDateString()
                }).ToList()
            };
        }

        public AccountViewModel GetAccountViewModel(int userId)
        {
            var service = new AccountServices();
            var user = service.GetUser(userId);
            return new AccountViewModel
            {
                isMe = false,
                userId = user.userId,
                username = user.username
            };
        }
    }
}