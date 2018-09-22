using Brobet.Services;
using Brobet.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Brobet.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "ADMIN")]
        public ActionResult Fixtures()
        {
            var service = new AdminViewModelService();
            var vm = service.CreateAdminFixturesViewModel();
            return View(vm);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult UpdateFixture(int id, double homeOdds, double drawOdds, double awayOdds)
        {
            var service = new FixtureService();
            service.UpdateFixture(id, homeOdds, drawOdds, awayOdds);

            return Json(new
            {
                response = "SUCCESS"
            });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult DeleteFixture(int id, bool delete = true)
        {
            var service = new FixtureService();
            service.DeleteFixture(id, delete);

            return Json(new
            {
                response = "SUCCESS"
            });
        }
    }
}