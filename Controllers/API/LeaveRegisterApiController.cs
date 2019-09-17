using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;
using Microsoft.SqlServer.Server;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class LeaveRegisterApiController : ApiController
    {
        readonly LeaveRegisterGateway _LeaveRegisterGateway = new LeaveRegisterGateway();
        readonly EmployeeGetway _gtEmployeeGetway = new EmployeeGetway();
        readonly DropDownGateway _gtDropDownGateway = new DropDownGateway();
        

        public HttpResponseMessage GetEmployeeList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Code AS Name From tbl_EMPLOYEE_HR Order By Id"), formatter);
        }
        public HttpResponseMessage GetPurposeId()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id, Name From tbl_HR_LEAVE_PURPOSE Order By Id"), formatter);
        }
        public HttpResponseMessage GetLeaveTypeId()
        {
            var currantYear = DateTime.Now.Year.ToString();
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _LeaveRegisterGateway.GetIdCasCadeDropDown("SELECT Id, Name From tbl_HR_LEAVE_TYPE WHERE Year=" + currantYear + " Order By Id "), formatter);
        }
        public HttpResponseMessage GetSaveNewPurposeType(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_LeaveRegisterGateway.FncSeekRecordNew("tbl_HR_LEAVE_PURPOSE", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);

            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtDropDownGateway.SaveWithReturnId("tbl_HR_LEAVE_PURPOSE", name), formatter);


        }
        public HttpResponseMessage GetSaveNewleaveType(string name, int day)
        {
            var currantYear = DateTime.Now.Year.ToString();
            var formatter = RequestFormat.JsonFormaterString();
            if (_LeaveRegisterGateway.FncSeekRecordNew("tbl_HR_LEAVE_TYPE", "Name='" + name + "' AND Year='" + currantYear + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);

            }
            return Request.CreateResponse(HttpStatusCode.OK, _LeaveRegisterGateway.SaveWithReturnId("tbl_HR_LEAVE_TYPE", name, day), formatter);


        }
        public HttpResponseMessage GetLeaveCulculation(string code, int leaveType)
        {
           
            var formatter = RequestFormat.JsonFormaterString();
            var restLeave = _LeaveRegisterGateway.LeaveCalculation(code, leaveType);
           
            return Request.CreateResponse(HttpStatusCode.OK, restLeave, formatter);


        }

        public async Task<HttpResponseMessage> Post([FromBody] LeaveRegisterModel mLeaveRegisterModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {
                msg = await _LeaveRegisterGateway.Save(mLeaveRegisterModel);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formatter);
                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }

        }
        
	}
}