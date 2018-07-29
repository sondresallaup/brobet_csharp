using Brobet.Services;
using Brobet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModelServices
{
    public class FixtureViewModelService
    {
        public FixtureViewModelService() { }

        public IndexViewModel GetIndexViewModel(int daysFromNow = 0)
        {
            var model = new IndexViewModel();
            var fixtureService = new FixtureService();
            var accountService = new AccountServices();
            model.daysFromNow = daysFromNow;
            model.currentDate = DateTime.Now.AddDays(daysFromNow);
            model.fixtures = fixtureService.GetFixtures(daysFromNow);
            model.friends = accountService.GetFriends().Select(fr => new IndexViewModel.Friend
            {
                userId = fr.userId,
                username = fr.username
            }).ToList();
            return model;
        }
    }
}