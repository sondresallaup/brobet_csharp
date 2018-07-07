using Brobet.Services;
using Brobet.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Brobet.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            var accountServices = new AccountServices();
            if(accountServices.isLoggedIn())
            {
                return Redirect("/");
            }

            return View();
        }

        public ActionResult Login()
        {
            var accountServices = new AccountServices();
            if (accountServices.isLoggedIn())
            {
                return Redirect("/");
            }

            return View();
        }

        public ActionResult Logout()
        {
            var accountServices = new AccountServices();
            accountServices.Logout();
            return Redirect("/");
        }

        public ActionResult Me()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new AccountViewModelService();
            var vm = vmService.GetMeAccountViewModel();

            return View("User", vm);
        }

        public ActionResult UserProfile(int id)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new AccountViewModelService();
            var vm = vmService.GetAccountViewModel(id);

            return View("User", vm);
        }

        public ActionResult Transactions()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new AccountViewModelService();
            var vm = vmService.GetTransactionsViewModel();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            var accountServices = new AccountServices();
            if(accountServices.isUserNameInUse(username))
            {
                return Json(new
                {
                    response = "ERROR",
                    message = "USERNAME_IN_USE"
                });
            }
            accountServices.createUserLogin(username, password);
            Response.Cookies[0].Expires = DateTime.Now.AddDays(30);
            return Json(new
            {
                response = "SUCCESS"
            });
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var accountServices = new AccountServices();
            accountServices.Login(username, password);
            Response.Cookies[0].Expires = DateTime.Now.AddDays(30);
            return Json(new
            {
                response = "SUCCESS"
            });
        }

        [HttpPost]
        public ActionResult SendFriendRequest(string username)
        {
            var accountServices = new AccountServices();
            var succeeded = accountServices.SendFriendRequest(username);
            if(succeeded)
            {
                return Json(new
                {
                    response = "SUCCESS"
                });
            }
            else
            {
                return Json(new
                {
                    response = "ERROR"
                });
            }
        }
        
        public ActionResult AcceptFriendRequest(int id)
        {
            var accountServices = new AccountServices();
            var succeeded = accountServices.AcceptFriendRequest(id);
            return Redirect("/Account/Me");
        }
    }
}