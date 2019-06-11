using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorksAPI.Models
{
    public class SearchPersonModel
    {
        public int BusinessEntityId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}