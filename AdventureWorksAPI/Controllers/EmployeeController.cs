using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdventureWorksAPI.Models;
using System.Web.Http.Cors;

namespace WebApiExercise.Controllers
{
    [EnableCors(origins: "http://localhost:57357", headers: "*", methods: "*")]
    public class EmployeeController : BaseApiController
    {
        public EmployeeController(IEmployeeDataRepository repo) : base(repo)
        {

        }

        public IHttpActionResult Get()
        {
            List<EmployeeBaseData> temp = TheModelFactory.GetEmployees();
            return Ok(temp);
            //return TheModelFactory.GetEmployees();
        }

        public HttpResponseMessage GetBaseAddress()
        {
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.BaseAddress());
        }

        public HttpResponseMessage GetEmployee(int BusinessEntityId)
        {
            EmployeeData temp = TheModelFactory.CreateEmployeeData(BusinessEntityId);
            if (temp == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, temp);
        }

        //[HttpGet]
        //public HttpResponseMessage GetEmployeeAddress(int BusinessEntityId)
        //{

        //}


        [HttpGet]
        public HttpResponseMessage FindEmployee(string SearchText)
        {
            List<EmployeeBaseData> Employees = TheModelFactory.FindEmployee(SearchText);
            return Request.CreateResponse(HttpStatusCode.OK, Employees);
        }

        [HttpPost]
        public HttpResponseMessage PostAddress([FromBody]EmployeeAddressDepartment ead)
        {
            if (TheModelFactory.UpdateAddress(ead))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Update failed");
            }
            //return Request.CreateResponse(HttpStatusCode.OK);
        }

        //Create a new address
        public HttpResponseMessage AddAddress([FromBody]AddressModel model)
        {
            if ((model.AddressLine1 == null) || 
                (model.AddressLine2 == null) ||
                (model.City==null) ||
                (model.StateProvinceID==0) ||
                (model.PostalCode==null))
            {
                return Request.CreateResponse(HttpStatusCode.PartialContent, "All Fields expected");
            }

            if (TheModelFactory.AddAddress(model))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Update failed");
            }
            
        }
    }
}
