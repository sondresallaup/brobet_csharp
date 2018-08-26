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

        public Dictionary<int, List<FixtureViewModel>> GetFixtures(int daysFromNow = 0)
        {
            var date = DateTime.Today.AddDays(daysFromNow);
            var fixtures = db.Fixtures.Where(f => f.date == date).OrderBy(f => f.startingAt).GroupBy(f => f.seasonId).ToDictionary(f => f.Key.Value, f => f.Select(fa => new FixtureViewModel(fa)).ToList());
            return fixtures;
        }

        public FixtureViewModel GetFixture(int id)
        {
            var fixture = db.Fixtures.SingleOrDefault(f => f.id == id);
            return new FixtureViewModel(fixture);
        }
    }
}