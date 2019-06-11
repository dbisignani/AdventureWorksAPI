using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;

namespace AdventureWorksAPI.Models
{
    public interface IEmployeeDataRepository
    {
        List<Employee> GetAllEmployees();
        List<EmployeeBaseData> FindEmployee(string SearchText);
        List<EmployeeAddressDepartment> GetEmployeesInDepartment(int DepartmentID);
        List<Department> GetEmployeeDepartment(int BusinessEnitityId);
        List<Department> GetAllDepartments();
        bool AddDepartment(int BusinessEntityId, Department d);
        List<BusinessEntityAddress> GetEmployeeAddress(int BusinesEntityId, out Employee e);
        Address GetAddress(int AddressID);
        bool AddAddress(AddressModel a);
        bool UpdateAddress(AddressModel model);
        bool UpdateDatabase();
        List<SalesPerson> GetSalesPersons();
        List<SalesTerritory> GetSalesTerritory(int BusinessEntityId, out decimal CommissionPct);
    }

    public class EmployeeDataRepository : IEmployeeDataRepository
    {
        public Employee Employee { get; set; }
        public int DepartmentID { get; private set; }

        private AdventureWorks2016Entities _Context;
        public EmployeeDataRepository(AdventureWorks2016Entities Context)
        {
            _Context = Context;
        }

        public List<Employee> GetAllEmployees()
        {
            this.Employee = null;
            return _Context.Employees.Include(p=>p.Person).ToList();

            //to get person table information use employees[].Person.<column>
          }


        public List<EmployeeBaseData> FindEmployee(string SearchText)
        {
            List<EmployeeBaseData> EmployeeSearchResults = new List<EmployeeBaseData>();
            SqlParameter clientIdParameter = new SqlParameter("@SearchText", SearchText);
            //Can I change the type to EmployeeBaseData and get rid of the SearchPerson Model?
            List<SearchPersonModel> result = _Context.Database
                .SqlQuery<SearchPersonModel>("SearchPersons @SearchText", clientIdParameter)
                .ToList();
            foreach (SearchPersonModel spm in result)
            {
                EmployeeSearchResults.Add( new EmployeeBaseData { LastName=spm.LastName, FirstName=spm.FirstName, BusinessEntityID=spm.BusinessEntityId});
            }
            return EmployeeSearchResults;
        }

        public List<BusinessEntityAddress> GetEmployeeAddress(int BusinessEntityId, out Employee eout)
        {
            //context.Entry(post).Reference(p => p.Blog).Load();
            this.Employee = null;
            Employee emp = _Context.Employees.Where(e => e.BusinessEntityID == BusinessEntityId).FirstOrDefault();
            if (emp==null)
            {
                eout = null;
                return null;
            }
            this.Employee = emp;
            List<BusinessEntityAddress> BEA = _Context.BusinessEntityAddresses.Where(bea => bea.BusinessEntityID == emp.BusinessEntityID).Include(z=>z.Address).Include(x=>x.Address.StateProvince).ToList();
            eout = emp;
            return BEA;
            //List<AddressModel> addrs = new List<AddressModel>();
            //foreach (BusinessEntityAddress addr in BEA)
            //{
            //    string x = addr.Address.StateProvince.StateProvinceCode;
            //    addrs.Add(new AddressModel{BusinessEntityID=addr.BusinessEntityID,AddressID=addr.AddressID,
                                           
            //        StateProvinceCode =addr.Address.StateProvince.StateProvinceCode);
            //}
            //eout = emp;
            //return addrs;
        }

        public Address GetAddress(int AddressID)
        {
            Address a = _Context.Addresses.Where(adr => adr.AddressID == AddressID).FirstOrDefault();
            return a;
        }

        public bool UpdateAddress(AddressModel model)
        {
            Address addr = _Context.Addresses.Where(a => a.AddressID == model.AddressID).FirstOrDefault();
            if (model.AddressLine1 != null)
            {
                addr.AddressLine1 = model.AddressLine1;
            }
            if (model.AddressLine2 != null)
            {
                addr.AddressLine2 = model.AddressLine2;
            }
            if (model.City != null)
            {
                addr.City = model.City;
            }
            if (model.StateProvinceID != 0)
            {
                addr.StateProvinceID = model.StateProvinceID;
            }
            if (model.PostalCode != null)
            {
                addr.PostalCode = model.PostalCode;
            }
            addr.ModifiedDate = DateTime.Now;
            try
            {
                _Context.SaveChanges();
            }
            catch (Exception e)
            {
                string Message = e.Message;
                return false;
            }
            return true;
        }

        public bool AddAddress(AddressModel model)
        {
            Address addr = new Address();
            addr.AddressLine1 = model.AddressLine1;
            addr.AddressLine2 = model.AddressLine2;
            addr.City = model.City;
            addr.StateProvinceID = model.StateProvinceID;
            addr.PostalCode = model.PostalCode;
            addr.SpatialLocation = null;
            addr.ModifiedDate = DateTime.Now;
            try
            {
                _Context.Addresses.Add(addr);
                _Context.SaveChanges();
            }
            catch (Exception e)
            {
                string Message = e.Message;
                return false;
            }
            return true;
        }

        public List<Department> GetAllDepartments()
        {
            return _Context.Departments.ToList();
        }

        //Perform this query using the SP because it is more straightforward than attempting to use LINQ and EntityFramework to get the same information.
        public List<EmployeeAddressDepartment> GetEmployeesInDepartment(int DepartmentIDTemp)
        {
            List<EmployeeAddressDepartment> EmployeesInDepartment = new List<EmployeeAddressDepartment>();
            SqlParameter DepartmentIdParameter = new SqlParameter("@DepartmentID", DepartmentIDTemp);
            List<EmployeeAddressDepartment> result = _Context.Database
                .SqlQuery<EmployeeAddressDepartment>("dbo.GetEmployeeInDepartment @DepartmentID", DepartmentIdParameter)
                .ToList();

            return result;


            //This does not work.
            //foreach (SearchPersonModel spm in result)
            //{
            //    EmployeeSearchResults.Add(new EmployeeBaseData { LastName = spm.LastName, FirstName = spm.FirstName, BusinessEntityId = spm.BusinessEntityId });
            //}
            //return EmployeeSearchResults;
        }
        //I cannot make this work
        //public List<Department> GetEmployeesInDepartment(int DepartmentIDTemp)
        //{
        //    int DepartmentIDTemp2 = DepartmentIDTemp;
        //    //This does not work because the include paths are not in the database  ctx.Students.Include("Standard.Teachers")
        //    List<Department> Departments = _Context.Departments.Where(d => d.DepartmentID == DepartmentIDTemp2).Include(a => a.EmployeeDepartmentHistories)  //.ToList(); //.Include(edh=>edh.BusinessEntityID).Include("Person.Address").ToList();
        //    return Departments;
        //}

        public List<Department> GetEmployeeDepartment(int BusinessEntityId)
        {
            this.Employee = null;
            List<Employee> Employee = _Context.Employees.Where(e => e.BusinessEntityID == BusinessEntityId).Include(b => b.EmployeeDepartmentHistories.Select(d=>d.Department)).ToList();
            List<Department> Departments = new List<Department>();
            foreach (Employee emp in Employee)
            {
                foreach (EmployeeDepartmentHistory edh in emp.EmployeeDepartmentHistories)
                {
                    Departments.Add(edh.Department);
                }
                    
            }
            return Departments;
        }

        public bool AddDepartment(int BusinessEntityId, Department d)
        {
            throw new NotImplementedException();
        }

        public List<SalesPerson> GetSalesPersons()
        {
            Employee = null;
            return _Context.SalesPersons.ToList();
        }


        public List<SalesTerritory> GetSalesTerritory(int BusinessEntityId, out decimal CommissionPct)
        {
            decimal cp=0;
            //Allows a salesrep to have more than one territory.
            List<SalesTerritory> SalesTerritories=new List<SalesTerritory>();
            List<SalesPerson> SPs=_Context.SalesPersons.Where(sp => sp.BusinessEntityID == BusinessEntityId).Include(sp2 => sp2.SalesTerritory).ToList();
            foreach (SalesPerson sp3 in SPs)
            {
                cp = sp3.CommissionPct;
                SalesTerritories.Add(sp3.SalesTerritory);
            }
            CommissionPct = cp;
            return SalesTerritories;
        }

        public bool UpdateDatabase()
        {
            try
            {
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                string ExceptionMessage = ex.Message;
                return false;
            }
            return true;
        }

    }

}

