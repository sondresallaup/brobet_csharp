using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModels
{
    public class IndexViewModel
    {
        public Dictionary<int, FixtureViewModel> fixtures { get; set; }
    }
}