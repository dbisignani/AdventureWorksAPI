using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorksAPI.Models
{
    public class EmployeeAddressDepartment
    {
        public int BusinessEntityID { get; set; }
        public int AddressID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
    }
}