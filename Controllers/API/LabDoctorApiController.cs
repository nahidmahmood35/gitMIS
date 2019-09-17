using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web.Mvc;
using System.Web.Http.Cors;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class LabDoctorApiController : ApiController
    {
        readonly LabDoctorGateway _gt= new LabDoctorGateway();

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] DoctorModel mLabDoctor)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                string msg = "";
                if (_gt.FncSeekRecordNew("tbl_LAB_DOCTOR_INFO","Id="+ mLabDoctor.DrId +""))
                {
                    msg = await _gt.Update(mLabDoctor);    
                }
                else
                {
                    msg = await _gt.Save(mLabDoctor);    
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }
        public HttpResponseMessage GetLabDoctorList(int id,string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetLabDoctorList(id, searchString), formatter);
        }

    }
}
