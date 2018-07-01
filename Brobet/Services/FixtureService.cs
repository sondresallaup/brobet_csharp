using Brobet.Models;
using Brobet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.Services
{
    public class FixtureService
    {
        private Entities db = new Entities();

        public FixtureService() { }

        public Dictionary<int, FixtureViewModel> GetFixtures()
        {
            var fixtures = db.Fixtures.Where(f => f.date == DateTime.Today).OrderBy(f => f.startingAt).ToDictionary(f => f.id, f => new FixtureViewModel(f));
            return fixtures;
        }

    }
}