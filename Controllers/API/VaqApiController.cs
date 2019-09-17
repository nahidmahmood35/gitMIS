using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class VaqApiController : ApiController
    {
        readonly VaqGateway _dtGateway=new VaqGateway();
        public HttpResponseMessage GetVaqList(int itemId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dtGateway.GetVaqList(itemId), formatter);
        }
    }
}
