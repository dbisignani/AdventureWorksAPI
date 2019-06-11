using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorksAPI.Models
{
    public class SalesRep
    {
        public string Url { get; set; }
        public List<Territory> Territories { get; set; }
        public decimal CommissionPct { get; set; }
    }

    public class Territory
    {
        public string TerritoryName { get; set; }
        public int TerritoryID { get; set; }
        public decimal SalesYTD { get; set; }

    }
}