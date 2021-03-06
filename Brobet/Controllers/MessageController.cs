﻿using Brobet.Services;
using Brobet.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Brobet.Controllers
{
    public class MessageController : Controller
    {
        public ActionResult Index()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new MessageViewModelService();
            var vm = vmService.GetMessageListViewModel();

            ViewBag.ActiveTab = "message";
            return View(vm);
        }

        public ActionResult IndexPartial()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new MessageViewModelService();
            var vm = vmService.GetMessageListViewModel();

            ViewBag.ActiveTab = "message";
            return PartialView("_IndexPartial", vm);
        }

        public ActionResult Friendship(int id)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new MessageViewModelService();
            var vm = vmService.GetFriendshipViewModel(id);
            return View(vm);
        }

        public ActionResult FriendshipPartial(int id)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new MessageViewModelService();
            var vm = vmService.GetFriendshipViewModel(id);
            return PartialView("_FriendshipPartial", vm);
        }

        public ActionResult MessagesPartial(int id)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new MessageViewModelService();
            var vm = vmService.GetFriendshipViewModel(id);
            return PartialView("_MessagesPartial", vm);
        }

        [HttpPost]
        public ActionResult Compose(int friendshipId, string messageContent)
        {

            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Json(new
                {
                    response = "ERROR"
                });
            }
            var messageService = new MessageService();
            messageService.ComposeMessage(friendshipId, messageContent);
            return Json(new
            {
                response = "SUCCESS"
            });
        }
    }
}