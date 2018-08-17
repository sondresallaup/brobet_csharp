using Brobet.Services;
using Brobet.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Brobet.Controllers
{
    public class BetController : Controller
    {
        public ActionResult List()
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }

            var vms = new BetViewModelService();
            var vm = vms.GetBetListViewModel();

            ViewBag.ActiveTab = "bets";
            return View(vm);
        }

        public ActionResult CreateRequest(int id)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }

            var vms = new BetViewModelService();
            var vm = vms.GetCreateBetRequestViewModel(id);
            return View("BetRequest", vm);
        }

        public ActionResult BetDetails(int id)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }

            var vms = new BetViewModelService();
            var vm = vms.GetBetDetailsViewModel(id);
            return View("BetRequest", vm);
        }

        public ActionResult SentBetRequest(int id)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }

            var vms = new BetViewModelService();
            var vm = vms.GetSentBetRequestViewModel(id);
            return View("BetRequest", vm);
        }

        public ActionResult ReceivedBetRequest(int id)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }

            var vms = new BetViewModelService();
            var vm = vms.GetReceivedBetRequestViewModel(id);
            return View("BetRequest", vm);
        }

        [HttpPost]
        public ActionResult Place(int toUserId, int fixtureId, int fromAmount, int toAmount, string[] fromBets, string[] toBets)
        {
            var betService = new BetService();
            var result = betService.CreateBetRequest(toUserId, fixtureId, fromAmount, toAmount, fromBets, toBets);
            return Json(new
            {
                response = result
            });
        }

        [HttpPost]
        public ActionResult AcceptRequest(int id)
        {
            var betService = new BetService();
            var result = betService.AcceptRequest(id);
            return Json(new
            {
                response = result
            });
        }

        [HttpPost]
        public ActionResult MarkAsPayed(int id)
        {
            var betService = new BetService();
            var result = betService.MarkAsPayed(id);
            return Json(new
            {
                response = result
            });
        }
    }
}