using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;
using System.Data.SqlClient;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class IndoorPatientReleaseApiController : ApiController
    {
        readonly IndoorPatientReleaseGateway _gt = new IndoorPatientReleaseGateway();

        public async Task<HttpResponseMessage> Post([FromBody] List<PatientReleaseModel> aModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = await _gt.Save(aModel);
            return Request.CreateResponse(HttpStatusCode.OK, msg, formatter);
        }

        public HttpResponseMessage GetReprintIndoorPatientRelease(int releaseId)
        {
            _gt.PrintReport("IndoorPatientReleaseInvoiceRPT.rpt", "VW_IN_PATIENT_RELEASE_PRINT", " WHERE Id='" + releaseId + "'", "VW_IN_PATIENT_RELEASE_PRINT", "", "", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }
        
        public HttpResponseMessage GetIndoorPatientReleaseList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetIndoorPatientReleaseList(), formatter);
        }

        public HttpResponseMessage GetClinicaldetail(int indoorId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gt.FncSeekRecordNew("tbl_IN_PATIENT_ADMISSION", "Id=" + indoorId + "")==false)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Invalid PatientId ", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetClinicaldetail(indoorId), formatter);
        }

        public HttpResponseMessage GetOperationName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetIdNameForDropDownBox("SELECT Id,Name From tbl_IN_OT_NAME_INFO Order By Id"), formatter);
        }
        public HttpResponseMessage GetCurrentPatientDue(int indoorId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gt.FncSeekRecordNew("tbl_IN_PATIENT_ADMISSION", "Id=" + indoorId + "") == false)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Invalid PatientId ", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetCurrentPatientDue(indoorId), formatter);
        }

    }
}
