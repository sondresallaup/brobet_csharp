using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.Models.SportsApiModels
{
    public class DataObject<T> where T : BaseSportsApiModel
    {
        public T[] data { get; set; }
    }
}