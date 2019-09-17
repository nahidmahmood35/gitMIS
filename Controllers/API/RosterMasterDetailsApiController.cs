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
    public class RosterMasterDetailsApiController : ApiController
    {
        readonly RosterMasterDetailsGateway _gtRosterMasterDetailsGateway = new RosterMasterDetailsGateway();
        public HttpResponseMessage GetEmployeeList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.GetIdCasCadeDropDown("Select distinct EmpId AS Id,b.Code+'-'+b.Name AS Name from tbl_HR_ROSTER_MASTER AS a  inner join tbl_HR_GLO_EMPLOYEE AS b ON a.EmpId=b.Id "), formatter);
        }
        public HttpResponseMessage GetDepartmentList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.GetIdCasCadeDropDown("Select distinct b.DeparmentId AS Id,c.Name AS Name from tbl_HR_ROSTER_MASTER AS a inner Join tbl_HR_GLO_EMPLOYEE AS b ON a.EmpId=b.Id inner Join tbl_DEPARTMENT_HR AS c ON b.DeparmentId=c.Id"), formatter);
        }
        public HttpResponseMessage GetShiftList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.GetIdCasCadeDropDown("Select Id,ShiftType AS Name from tbl_HR_ROSTER_SHIFT_INFO "), formatter);
        }


        public HttpResponseMessage GetEmployeeDetalsList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.GetEmployeelDetalsList(searchString), formatter);

        }
        public HttpResponseMessage GetCreaye(string formDate, string toDate, int emId)
        {
            var formatter = RequestFormat.JsonFormaterString();
           
            if (_gtRosterMasterDetailsGateway.FncSeekRecordNew("tbl_HR_ROSTER_DETAILS", "EmpId=" + emId + " AND Date='" + formDate + "' "))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Employee code already exists" }, formatter);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.Save(formDate, toDate, emId), formatter);

        }

        public HttpResponseMessage GetDelete(string formDate, string toDate, int emId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.Delete(formDate, toDate, emId), formatter);

        }

        public HttpResponseMessage GetAll(int emId)
        {
            var formatter = RequestFormat.JsonFormaterString();
           
            if (_gtRosterMasterDetailsGateway.FncSeekRecordNew("tbl_HR_ROSTER_DETAILS", "EmpId=" + emId + ""))
            {
                return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.GetAll(emId), formatter);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Data Not found" }, formatter);
            }

            

        }



        public HttpResponseMessage GetReport(string formDate, string toDate, int emId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtRosterMasterDetailsGateway.FncSeekRecordNew("tbl_HR_ROSTER_DETAILS","EmpId=" + emId + " AND Date='" + formDate + "' ") && _gtRosterMasterDetailsGateway.FncSeekRecordNew("tbl_HR_ROSTER_DETAILS", "EmpId=" + emId + " AND Date='" + toDate + "' "))
            {
                string condition = "";
                condition = "where EmpId=" + emId + " AND Date BETWEEN CONVERT(date,'" + formDate + "',102) and CONVERT(date,'" + toDate + "',102)";
                return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.PrintReportHr("RosterScheduleReportFile.rpt", "VW_HR_ROSTER_SCHEDULE", condition, "VW_HR_ROSTER_SCHEDULE", " ROSTER SCHEDULE", "Reporting Date From " + formDate + "  to " + toDate + "  ", "V"), formatter);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Please Select valid Date!!" }, formatter);
            }
            
            }

        public HttpResponseMessage GetEdit(int timeShiftId, int rowId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.GetEdit(timeShiftId, rowId), formatter);
        }

        public HttpResponseMessage GetReportDepartment(int DepartmentId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            
                string condition = "";
                //condition = "where EmpId=" + emId + " AND Date BETWEEN CONVERT(date,'" + formDate + "',102) and CONVERT(date,'" + toDate + "',102)";
                condition = "where DeparmentId=" + DepartmentId + "";
                return Request.CreateResponse(HttpStatusCode.OK, _gtRosterMasterDetailsGateway.PrintReportHr("RosterScheduleDepartmentReportFile.rpt", "VW_HR_ROSTER_SCHEDULE_DEPARTMENT", condition, "VW_HR_ROSTER_SCHEDULE_DEPARTMENT", " ROSTER SCHEDULE DEPARTMENT", "", "V"), formatter);
           

        }

    }
}