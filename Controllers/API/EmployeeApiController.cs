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
    public class EmployeeApiController : ApiController
    {
        readonly EmployeeGetway _gtEmployeeGetway = new EmployeeGetway();
        readonly DropDownGateway _gtDropDownGateway = new DropDownGateway();
        public async Task<HttpResponseMessage> Post([FromBody] EmployeeModel mEmployeeModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {
               
                if (string.IsNullOrEmpty(mEmployeeModel.EmCode))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Employee Code is Empty" }, formatter);
                }
                if (string.IsNullOrEmpty(mEmployeeModel.EmName))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation {Output = "error", Msg = "Employee Name is Empty"}, formatter);
                }
                else
                {


                    if (_gtEmployeeGetway.FncSeekRecordNew("tbl_EMPLOYEE_HR", "Id=" + mEmployeeModel.EmId + ""))
                    {
                        //msg = await _gtEmployeeGetway.Update(mEmployeeModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation {Output = "success", Msg = msg}, formatter);
                    }
                    else
                    {
                        if (_gtEmployeeGetway.FncSeekRecordNew("tbl_EMPLOYEE_HR", "Code='" + mEmployeeModel.EmCode + "'"))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Employee code already exists" }, formatter);
                        }


                       //           msg = await _gtEmployeeGetway.Save(mEmployeeModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation {Output = "success", Msg = msg}, formatter);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
            
        }

        public HttpResponseMessage GetGenderList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_GENDER_INFO_MST Order By Id"), formatter);
        }

        public HttpResponseMessage GetNationalityList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_NATIONALITY_INFO Order By Id"), formatter);
        }

        public HttpResponseMessage GetReligionList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_RELIGION_INFO Order By Id"), formatter);
        }

        public HttpResponseMessage GetDeparmentList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_DEPARTMENT_HR Order By Id"), formatter);
        }

        public HttpResponseMessage GetDesignationList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_DESIGNATION_HR Order By Id"), formatter);
        }

        public HttpResponseMessage GetCompanyBankName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_BANK Order By Id"), formatter);
        }

        public HttpResponseMessage GetProjectId()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_PROJECT_HR Order By Id"), formatter);
        }
        public HttpResponseMessage GetBranchNameByBankId(int CatId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name,BankId AS Cat_Id From tbl_BRANCH_OF_BANK_HR WHERE BankId = " + CatId + " Order By Id"), formatter);
        }



        public HttpResponseMessage GetSaveDepartmentName(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtDropDownGateway.FncSeekRecordNew("tbl_DEPARTMENT_HR", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
                
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtDropDownGateway.SaveWithReturnId("tbl_DEPARTMENT_HR", name), formatter);

           
        }

        public HttpResponseMessage GetSaveDesignationName(string name)
        {
            string msg = "";
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtDropDownGateway.FncSeekRecordNew("tbl_DESIGNATION_HR", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtDropDownGateway.SaveWithReturnId("tbl_DESIGNATION_HR", name), formatter);

            
            


        }

        public HttpResponseMessage GetSaveBankName(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtDropDownGateway.FncSeekRecordNew("tbl_BANK", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtDropDownGateway.SaveWithReturnId("tbl_BANK", name), formatter);
        }

        public HttpResponseMessage GetSaveProjectName(string name)
        {

            var formatter = RequestFormat.JsonFormaterString();
            if (_gtDropDownGateway.FncSeekRecordNew("tbl_PROJECT_HR", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtDropDownGateway.SaveWithReturnId("tbl_PROJECT_HR", name), formatter);


        }




        public HttpResponseMessage GetSaveBankBranchName(string param, int catId)
        {

            string msg = "";
            var formatter = RequestFormat.JsonFormaterString();
            
            if (_gtDropDownGateway.FncSeekRecordNew("tbl_BRANCH_OF_BANK_HR", "Name='" + param + "' AND BankId = '" + catId + "'"))
            {
                msg = "This Name Already Exist";
                return Request.CreateResponse(HttpStatusCode.OK, msg, formatter);
            }
            msg =_gtEmployeeGetway.SaveNameOnly("INSERT INTO tbl_BRANCH_OF_BANK_HR (Name,BankId) OUTPUT INSERTED.ID VALUES ('" + param + "'," + catId + ")").ToString();

            return Request.CreateResponse(HttpStatusCode.OK, msg, formatter);
            

        }

        public HttpResponseMessage GetEmployeeDetalsList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetEmployeelList(searchString), formatter);
        }

        public HttpResponseMessage Test()
        {

            //string msg = _aOpdTicketGateway.Save();
            string msg = _gtEmployeeGetway.PrintReportHr("EmployeeTest.rpt", "VW_TEST_EMPLOYEE", "", "VW_TEST_EMPLOYEE", "Employee Test Report", "", "V");
            //return Json(msg, JsonRequestBehavior.AllowGet);
            //return "";
           return Request.CreateErrorResponse(HttpStatusCode.OK, msg);
        }
	}
}