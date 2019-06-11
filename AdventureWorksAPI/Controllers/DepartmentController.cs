using AdventureWorksAPI.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http.Cors;

namespace WebApiExercise.Controllers
{
    [EnableCors(origins: "http://localhost:57357", headers: "*", methods: "*")]
    public class DepartmentController : BaseApiController
    {
        public DepartmentController(IEmployeeDataRepository repo) : base(repo)
        {

        }
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.GetDepartments());
        }

        public HttpResponseMessage Get(int DepartmentID)
        {
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.GetEmployeesInDepartment(DepartmentID));
        }
    }
}
