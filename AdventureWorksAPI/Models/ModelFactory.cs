using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace AdventureWorksAPI.Models
{
    public class ModelFactory
    {
        private IEmployeeDataRepository _repo;
        HttpRequestMessage _request;
        public ModelFactory(HttpRequestMessage request, IEmployeeDataRepository repo)
        {
            _repo = repo;
            _request = request;
        }

        public string BaseAddress()
        {
            return new UrlHelper(_request).Link("BaseAddress",new {  });
        }

        public List<EmployeeBaseData> GetEmployees()
        {
            UrlHelper uh = new UrlHelper(_request); 
            List<EmployeeBaseData> Employees = _repo.GetAllEmployees()
                                   .Select(emp => new EmployeeBaseData
                                   {
                                       LastName = emp.Person.LastName,
                                       FirstName = emp.Person.FirstName,
                                       BusinessEntityID = emp.BusinessEntityID,
                                       Url= uh.Link("EmployeeData", new { BusinessEntityId = emp.BusinessEntityID })
                                   }).ToList();
            return Employees;
        }

        public List<EmployeeBaseData> FindEmployee(string SearchText)
        {
            List<EmployeeBaseData> EBDs = new List<EmployeeBaseData>();
            UrlHelper uh = new UrlHelper(_request);
            foreach (EmployeeBaseData ebd in _repo.FindEmployee(SearchText))
            {
                ebd.Url = uh.Link("EmployeeData", new { BusinessEntityId = ebd.BusinessEntityID });
                EBDs.Add(ebd);
            }
            return EBDs;
        }

        public EmployeeData CreateEmployeeData(int  BusinessEntityId)
        {
            AddressModel am;
            UrlHelper uh = new UrlHelper(_request);
            EmployeeData ed = new EmployeeData();
            ed.EmployeeAddresses = new List<AddressModel>();
            ed.EmployeeDepartments = new List<DepartmentModel>();
            List<BusinessEntityAddress> Addresses =  _repo.GetEmployeeAddress(BusinessEntityId, out Employee e);
            if (Addresses==null)
            {
                return null;
            }
            ed.LastName = e.Person.LastName;
            ed.FirstName = e.Person.FirstName;
            ed.BusinessEntityID = BusinessEntityId;
            e.BusinessEntityID = BusinessEntityId;
            foreach (var a in Addresses )
            {
                //, StateProvinceCode=a.BusinessEntityAddresses.Address.StateProvince.StateProvinceCode
                am = new AddressModel { AddressID = a.Address.AddressID, AddressLine1 = a.Address.AddressLine1, AddressLine2 = a.Address.AddressLine2, City = a.Address.City, StateProvinceID = a.Address.StateProvinceID,StateProvinceCode=a.Address.StateProvince.StateProvinceCode, PostalCode = a.Address.PostalCode, Url = uh.Link("EmployeeData", new { BusinessEntityId = BusinessEntityId }) };
                ed.EmployeeAddresses.Add(am);
            }

            List<Department> Departments = _repo.GetEmployeeDepartment(BusinessEntityId);
            foreach (Department d in Departments)
            {
                ed.EmployeeDepartments.Add(new DepartmentModel { DepartmentID = d.DepartmentID, Name = d.Name, GroupName = d.GroupName,Url = uh.Link("EmployeeData", new { BusinessEntityId = BusinessEntityId }) });
            }
            
            ed.Url = uh.Link("EmployeeData",new { BusinessEntityId=BusinessEntityId});
            return ed;
        }

        public AddressModel Parse(int AddressID)
        {
            Address addr = _repo.GetAddress(AddressID);
            AddressModel am = new AddressModel();
            am.AddressID = addr.AddressID;
            am.AddressLine1 = addr.AddressLine1;
            am.AddressLine2 = addr.AddressLine2;
            am.City = addr.City;
            am.StateProvinceID = addr.StateProvinceID;
            am.PostalCode = addr.PostalCode;
            return am;
        }

        //Pass through function for consistency; allows code to be added at a future date.
        public bool UpdateAddress(EmployeeAddressDepartment ead)
        {
            Address a = _repo.GetAddress(ead.AddressID);
            if (a==null)
            {
                return false;
            }
            AddressModel am = new AddressModel() {

                AddressID = Convert.ToInt32(ead.AddressID),
                AddressLine1 = ead.AddressLine1,
                AddressLine2 = ead.AddressLine2,
                City = ead.City,
                StateProvinceID = a.StateProvinceID,
                PostalCode = ead.PostalCode,
                BusinessEntityID = Convert.ToInt32(ead.BusinessEntityID),
                ModifiedDate = new DateTime(2019, 1, 1) //This date will not be used to update the DB;
            };
        
            if (_repo.UpdateAddress(am))
            {
                return true;
            }
            return false;
        }

        //Pass through function for consistency
        public bool AddAddress(AddressModel am)
        {
            return _repo.AddAddress(am);
        }

        public List<DepartmentModel> GetDepartments()
        {
            UrlHelper uh = new UrlHelper(_request);
            //string Url = uh.Link("Departments", new { DepartmentID = d.DepartmentID });
            return _repo.GetAllDepartments().ToList().Select(d=> new DepartmentModel { DepartmentID = d.DepartmentID, Name = d.Name, GroupName = d.GroupName, Url=uh.Link("Departments", new { DepartmentID = d.DepartmentID }) }).ToList();
        }

        public List<EmployeeAddressDepartment> GetEmployeesInDepartment(int DepartmentID)
        {
           return _repo.GetEmployeesInDepartment(DepartmentID);
        }

        //public List<AddressModel> GetEmployeeAddress(int BusinessEntityId)
        //{
        //    Employee e;
        //    List<BusinessEntityAddress> BEA = _repo.GetEmployeeAddress(BusinessEntityId, out e);
        //    //AddressModel ad = new AddressModel { BusinessEntityID =BEA.}
        //    return null;

        //    //return _repo.GetEmployeeAddress(BusinessEntityId, out e);
        //}

        public SalesRep CreateSalesRep(int BusinessEntityId)
        {
            decimal CommissionPct;
            SalesRep sr = new SalesRep();
            sr.Territories =_repo.GetSalesTerritory(BusinessEntityId, out CommissionPct).Select(t => new Territory { TerritoryName = t.Name, TerritoryID = t.TerritoryID, SalesYTD = t.SalesYTD } ).ToList(); ;
            sr.CommissionPct = CommissionPct;
            return sr;
        }



    }
}