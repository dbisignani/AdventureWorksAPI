using AdventureWorksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiExercise.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public IEmployeeDataRepository _repo;
        ModelFactory _ModelFactory;
        public BaseApiController(IEmployeeDataRepository repo)
        {
            _repo = repo;
        }

        public ModelFactory TheModelFactory
        {
            get
            {
                if (_ModelFactory == null)
                {
                    _ModelFactory = new ModelFactory(this.Request, _repo);
                }
                return _ModelFactory;
            }

        }

    }
}
