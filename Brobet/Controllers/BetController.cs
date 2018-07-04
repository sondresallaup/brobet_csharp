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
            return View(vm);
        }

        [HttpPost]
        public ActionResult Place(int toUserId, int fixtureId, string initiatorBet, int homeAmount, int awayAmount)
        {
            var betService = new BetService();
            var result = betService.CreateBetRequest(toUserId, fixtureId, initiatorBet, homeAmount, awayAmount);
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
    }
}