using Brobet.Models;
using Brobet.Services;
using Brobet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Web.Mvc;
using Newtonsoft.Json;
using Brobet.ViewModelServices;

namespace Brobet.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index(int daysFromNow = 0)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }

            var vmService = new FixtureViewModelService();
            var model = vmService.GetIndexViewModel(daysFromNow);

            ViewBag.ActiveTab = "home";

            return View(model);
        }

        public ActionResult IndexPartial(int daysFromNow = 0)
        {
            var accountServices = new AccountServices();
            if (!accountServices.isLoggedIn())
            {
                return Redirect("/Account/Login");
            }

            var vmService = new FixtureViewModelService();
            var model = vmService.GetIndexViewModel(daysFromNow);

            ViewBag.ActiveTab = "home";

            return PartialView("_IndexPartial", model);
        }

        public ActionResult Team(int id)
        {
            var team = db.Teams.SingleOrDefault(t => t.id == id);


            return View(team);
        }

        public ActionResult Contact()
        {
            var accountService = new AccountServices();
            if(accountService.isLoggedIn())
            {
                ViewBag.Message = "You are logged in, " + accountService.GetCurrentUserName();
            }
            else
            {
                ViewBag.Message = "Your contact page.";
            }


            return View();
        }

        public string GetFixturesJson(int daysFromNow = 0)
        {
            var fixtureService = new FixtureService();
            var fixtures = fixtureService.GetFixtures(daysFromNow);

            return JsonConvert.SerializeObject(fixtures);
        }
    }
}