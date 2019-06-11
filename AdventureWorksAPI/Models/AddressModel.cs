using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorksAPI.Models
{

    public class AddressModel
    {
        public int BusinessEntityID { get; set; }
        public int AddressID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int StateProvinceID { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string Url { get; set; }
    }

    public class IncomingAddressModel
    {
        public string BusinessEntityID { get; set; }
        public string AddressID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string ModifiedDate { get; set; }
        public string Url { get; set; }
    }

    public class TestClass
    {
        public string Something { get; set; }
    }
}