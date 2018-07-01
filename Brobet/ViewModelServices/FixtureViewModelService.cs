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

        public IndexViewModel GetIndexViewModel()
        {
            var model = new IndexViewModel();
            var fixtureService = new FixtureService();
            model.fixtures = fixtureService.GetFixtures();
            return model;
        }
    }
}