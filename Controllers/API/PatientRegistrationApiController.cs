using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class PatientRegistrationApiController : ApiController
    {
        readonly DoctorGateway _dGateway = new DoctorGateway();
        readonly PatientGateway _gtPatient = new PatientGateway();
        readonly DropDownGateway _gtdropDownGateway = new DropDownGateway();

        public async Task<HttpResponseMessage> Post([FromBody] PatientModel mPatient)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                if (string.IsNullOrEmpty(mPatient.PtName))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Name is Empty" }, formatter);
                }

                if (string.IsNullOrEmpty(mPatient.UserName))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "UserName is Empty" }, formatter);
                }

                else
                {

                    if (_gtPatient.FncSeekRecordNew("tbl_PATIENT_REGISTRATION", "Id=" + mPatient.PtRegId + "") == false)
                    {
                        string msg = await _gtPatient.Save(mPatient);
                        return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                    }
                    else
                    {
                        string msg = await _gtPatient.Update(mPatient);
                        return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }

        public HttpResponseMessage GetPatientRegistrationList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtPatient.GetPatientlList(searchString), formatter);
        }

        public HttpResponseMessage GetReligionList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.GetIdNameForDropDownBox("SELECT Id,Name FROm tbl_RELIGION_INFO Order By Id"), formatter);
        }
        public HttpResponseMessage GetBloodGroupList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.GetIdNameForDropDownBox("SELECT Id,Name FROm tbl_BLOOD_GROUP_INFO Order By Id"), formatter);
        }
        public HttpResponseMessage GetInfoFromList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.GetIdNameForDropDownBox("SELECT Id,Name FROm tbl_INTRODUCE_INFO Order By Id"), formatter);
        }
        public HttpResponseMessage GetDeleteById(int id)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _dGateway.DeleteInsert("DELETE FROM tbl_PATIENT_REGISTRATION WHERE Id="+ id +""), formatter);
        }


        public HttpResponseMessage GetSaveIntoduceInfo(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtdropDownGateway.FncSeekRecordNew("tbl_INTRODUCE_INFO", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtdropDownGateway.SaveWithReturnId("tbl_INTRODUCE_INFO",name), formatter);
        }


    }
}
