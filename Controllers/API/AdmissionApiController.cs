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
    
 
    public class AdmissionApiController : ApiController
    {
        readonly AdmissionGateway _gtAdmissionGateway = new AdmissionGateway();

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] AdmissionModel mAdmission)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                if (mAdmission.PtRegId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Patient Name is Empty" }, formatter);
                }
                if (mAdmission.RefDrId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Dotor Name Empty" }, formatter);
                }
                if (mAdmission.BedId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Bed No is Empty" }, formatter);
                }

                if (mAdmission.UserName == "")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "UserName is Empty" }, formatter);
                }
                else
                {
                    string msg = await _gtAdmissionGateway.Save(mAdmission);
                    //   _gtInvoice.PrintReport("DiagnosisInvoiceRPT.rpt", "SP_GET_INVOICE_PRINT", "" + mInvoice.ElementAt(0).InvMasterId + "", "SP_GET_INVOICE_PRINT", "", "", "S");
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }


        public HttpResponseMessage GetPatientAdmissionView(int id)
        {
            _gtAdmissionGateway.PrintReport("AdmissionFormRPT.rpt", "VW_IN_ADMISSION_LIST", " WHERE Id=" + id + "", "VW_IN_ADMISSION_LIST", "", "", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }

        public HttpResponseMessage GetAdmittedPatientList(int id,string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtAdmissionGateway.GetAdmittedPatientList(id, searchString), formatter);
        }

        
        
        
        
        
        
        
        
        
        
        
        public HttpResponseMessage GetIndoorDepartmentListByPno(int deptId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtAdmissionGateway.GetIndoorDepartmentListByPno(deptId), formatter);
        }
        public HttpResponseMessage GetIndoorPackageList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtAdmissionGateway.GetIndoorPackageList(), formatter);
        }

    }
}
