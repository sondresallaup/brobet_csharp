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
            var fixtures = db.Fixtures.Where(f => !(f.deleted.HasValue && f.deleted.Value) && f.date == date).OrderBy(f => f.startingAt).GroupBy(f => f.seasonId).ToDictionary(f => f.Key.Value, f => f.Select(fa => new FixtureViewModel(fa)).ToList());
            return fixtures;
        }

        public Dictionary<int, List<FixtureViewModel>> GetAdminFixtures()
        {
            var toDate = DateTime.Today.AddDays(10);
            var fixtures = db.Fixtures.Where(f => f.date >= DateTime.Today && f.date <= toDate).OrderBy(f => f.startingAt).GroupBy(f => f.seasonId).ToDictionary(f => f.Key.Value, f => f.Select(fa => new FixtureViewModel(fa)).ToList());
            return fixtures;
        }

        public FixtureViewModel GetFixture(int id)
        {
            var fixture = db.Fixtures.SingleOrDefault(f => f.id == id);
            return new FixtureViewModel(fixture);
        }

        public void UpdateFixture(int id, double homeOdds, double drawOdds, double awayOdds)
        {
            var fixture = db.Fixtures.SingleOrDefault(f => f.id == id);
            fixture.homeOdds = homeOdds;
            fixture.drawOdds = drawOdds;
            fixture.awayOdds = awayOdds;
            db.SaveChanges();
        }

        public void DeleteFixture(int id, bool delete = true)
        {
            var fixture = db.Fixtures.SingleOrDefault(f => f.id == id);
            fixture.deleted = delete;
            db.SaveChanges(); 
            // TODO: Delete bets and warn affected users
        }
    }
}