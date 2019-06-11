using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorksAPI.Models
{
    public class EmployeeBaseData
    {
        protected string _LastName = null;
        protected string _FirstName = null;

        public string LastName { get { return _LastName; } set { _LastName = value; } }
        public string FirstName { get { return _FirstName; }  set { _FirstName = value; } }
        public int BusinessEntityID { get; set; }
        public string Url { get; set; }
    }
        

    public class EmployeeData : EmployeeBaseData
    {
        //private IEmployeeData _Repo;

        public List<DepartmentModel> EmployeeDepartments { get; set; }
        public List<AddressModel> EmployeeAddresses { get; set; }
        public string EmployeeName {
            get
            {
                return _LastName + ", " + _FirstName;
            }
        }
        public EmployeeData()
        {

        }

    }
}