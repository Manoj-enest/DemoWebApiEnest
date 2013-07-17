using DemoWebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DemoWebApi.Controllers
{
    public class TestController : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get()
        {
            return GlobalFunctions.ReturnResponse(HttpStatusCode.OK, "Created", "Get value 1,Get value 2", Request);
        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            return GlobalFunctions.ReturnResponse(HttpStatusCode.OK, "Created", "Get value by Id : " + id.ToString(), Request);
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]string value)
        {
            return GlobalFunctions.ReturnResponse(HttpStatusCode.Created, "Created successfully", "Created successfully", Request);
        }

        // PUT api/values/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            return GlobalFunctions.ReturnResponse(HttpStatusCode.OK, "Updated successfully", "Updated successfully", Request);
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(int id)
        {
            return GlobalFunctions.ReturnResponse(HttpStatusCode.OK, "Deleted successfully", "Deleted successfully", Request);
        }
    }
}