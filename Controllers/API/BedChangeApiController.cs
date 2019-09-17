using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")] 
    public class BedChangeApiController : ApiController
    {
        readonly BedInfoGateway _gt = new BedInfoGateway();

        public HttpResponseMessage Post([FromBody] AdmissionModel aModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.SaveBedChange(aModel.PtIndoorId, aModel.BedCharge, aModel.BedId, aModel.UserName, aModel.Remarks), formatter);
        }

    }
}
