using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Brobet.Controllers
{
    public class FixtureController : Controller
    {
        private Entities db = new Entities();
        // GET: Fixture
        public ActionResult Index(int id)
        {
            var fixture = db.Fixtures.SingleOrDefault(f => f.id == id);
            return View(fixture);
        }
    }
}