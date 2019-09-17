using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    public class RosterMasterApiController : ApiController
    {
        readonly RosterMasterGateway _gtRosterMasterGateway = new RosterMasterGateway();

        public HttpResponseMessage GetEmployeeList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterGateway.GetIdCasCadeDropDown("SELECT Id,Code+'-'+Name AS Name FROM tbl_HR_GLO_EMPLOYEE"), formatter);
        }

        public HttpResponseMessage GetDayName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterGateway.GetIdCasCadeDropDown("Select Id,DayName AS Name from tbl_HR_DAY_NAME_INFO"), formatter);
        }


        public HttpResponseMessage GetGeneralShift()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterGateway.GetIdCasCadeDropDown("Select Id,ShiftType AS Name from tbl_HR_ROSTER_SHIFT_INFO"), formatter);
        }


        public HttpResponseMessage GetEmployeeDetalsList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtRosterMasterGateway.FncSeekRecordNew("tbl_HR_ROSTER_MASTER", "EmpId='" + searchString + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterGateway.GetEmployeelDetalsListMaster(searchString), formatter);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterGateway.GetEmployeelDetalsList(searchString), formatter);
            }
           
        }


        public async Task<HttpResponseMessage> Save([FromBody] List<EmployeeModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {



                string msg = await _gtRosterMasterGateway.Save(aModels);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }
         
	}
}