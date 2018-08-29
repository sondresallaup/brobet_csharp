using Brobet.Services;
using Brobet.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;

namespace Brobet.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            var accountServices = new AccountServices();
            if(accountServices.isLoggedIn())
            {
                return Redirect("/Bet/");
            }

            return View();
        }

        public ActionResult Login()
        {
            var accountServices = new AccountServices();
            if (accountServices.isLoggedIn())
            {
                return Redirect("/Bet/");
            }

            return View();
        }

        public ActionResult Logout()
        {
            var accountServices = new AccountServices();
            accountServices.Logout();
            return Redirect("/Bet/");
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

            ViewBag.ActiveTab = "profile";

            return View("User", vm);
        }

        public ActionResult MePartial()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new AccountViewModelService();
            var vm = vmService.GetMeAccountViewModel();

            ViewBag.ActiveTab = "profile";

            return PartialView("_UserPartial", vm);
        }

        public ActionResult FillUpWallet()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new AccountViewModelService();
            var vm = vmService.GetFillUpWalletViewModel();

            ViewBag.ActiveTab = "profile";

            return View(vm);
        }

        public ActionResult FillUpWalletPartial()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new AccountViewModelService();
            var vm = vmService.GetFillUpWalletViewModel();

            ViewBag.ActiveTab = "profile";

            return PartialView("_FillUpWalletPartial", vm);
        }

        public ActionResult Avatar()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new AccountViewModelService();
            var vm = vmService.GetMeAccountViewModel();

            return View(vm);
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

        public ActionResult TransactionsPartial()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }
            var vmService = new AccountViewModelService();
            var vm = vmService.GetTransactionsViewModel();

            return PartialView("_TransactionsPartial", vm);
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
            Response.Cookies[0].Expires = DateTime.Now.AddDays(600);
            return Json(new
            {
                response = "SUCCESS",
                userId = accountServices.GetUserId(username),
                token = getToken()
            });
        }

        public ActionResult Charge(string stripeEmail, string stripeToken)
        {
            var accountServices = new AccountServices();
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = 10000,
                Description = "Fill up wallet",
                Currency = "nok",
                CustomerId = customer.Id
            });
            accountServices.FillAccount(100);

            return Redirect("/Account/Me");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var accountServices = new AccountServices();
            accountServices.Login(username, password);
            Response.Cookies[0].Expires = DateTime.Now.AddDays(600);
            return Json(new
            {
                response = "SUCCESS",
                userId = accountServices.GetUserId(username),
                token = getToken()
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

        [HttpPost]
        public ActionResult SaveAvatar(string avatarUrl)
        {
            var accountServices = new AccountServices();
            accountServices.SaveAvatar(avatarUrl);
            return Json(new
            {
                response = "SUCCESS"
            });
        }

        private Dictionary<string, string> getToken()
        {

            var token = new Dictionary<string, string>();
            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                token.Add("value", Request.Cookies[".ASPXAUTH"].Value);
                token.Add("expires", Request.Cookies[".ASPXAUTH"].Expires.ToString());
            }
            return token;
        }
    }
}