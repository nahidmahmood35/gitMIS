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
    public class EmployeeGLOApiController : ApiController
    {
        readonly EmployeeGetway _gtEmployeeGetway = new EmployeeGetway();
        readonly DropDownGateway _gtDropDownGateway = new DropDownGateway();
        public async Task<HttpResponseMessage> Post([FromBody] List<EmployeeModel> mEmployeeModel)
        {

            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {
                if (string.IsNullOrEmpty(mEmployeeModel.ElementAt(0).EmCode))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Employee Code is Empty" }, formatter);
                }
                if (string.IsNullOrEmpty(mEmployeeModel.ElementAt(0).EmName))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "Employee Name is Empty" }, formatter);
                }
                else
                {


                    if (_gtEmployeeGetway.FncSeekRecordNew("tbl_HR_GLO_EMPLOYEE", "Id=" + mEmployeeModel.ElementAt(0).EmId + ""))
                    {
                        msg = await _gtEmployeeGetway.Update(mEmployeeModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation { Output = "success", Msg = msg }, formatter);
                    }
                    else
                    {
                        if (_gtEmployeeGetway.FncSeekRecordNew("tbl_HR_GLO_EMPLOYEE", "Code='" + mEmployeeModel.ElementAt(0).EmCode + "'"))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Employee code already exists" }, formatter);
                        }


                        msg = await _gtEmployeeGetway.Save(mEmployeeModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation { Output = "success", Msg = msg }, formatter);
                    }

                }
                //if (mEmployeeModel.ElementAt(0).PtIndoorId == 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Name is Null" });
                //}
                //else
                //{
                //    string msg = await _gtEmployeeGetway.Save(mEmployeeModel);
                //    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formate);
                //}
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }
        
        public HttpResponseMessage GetGenderList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Bname AS Name From tbl_GENDER_INFO_MST Order By Id"), formatter);
        }

        public HttpResponseMessage GetNationalityList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Bname AS Name From tbl_NATIONALITY_INFO Order By Id"), formatter);
        }

        public HttpResponseMessage GetReligionList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Bname AS Name From tbl_RELIGION_INFO Order By Id"), formatter);
        }

        public HttpResponseMessage GetDeparmentList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Bname AS Name From tbl_DEPARTMENT_HR Order By Id"), formatter);
        }

        public HttpResponseMessage GetDesignationList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Bname AS Name From tbl_DESIGNATION_HR Order By Id"), formatter);
        }

        public HttpResponseMessage GetCompanyBankName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_BANK Order By Id"), formatter);
        }

        public HttpResponseMessage GetEarrinngName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK,
                _gtEmployeeGetway.GetIdCasCadeDropDown2(
                    "select Id,SalaryNameBD AS Name,SalaryType AS AnotherName from tbl_HR_GLO_SALARY_TYPE where SalaryType = 'e' AND Validity = 1"),
                formatter);
        }

        public HttpResponseMessage GetDetecttionName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK,
                _gtEmployeeGetway.GetIdCasCadeDropDown2(
                    "select Id,SalaryNameBD AS Name,SalaryType AS AnotherName from tbl_HR_GLO_SALARY_TYPE where SalaryType = 'd' AND Validity = 1"),
                formatter);
        }

        public HttpResponseMessage GetProjectId()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Bname AS Name From tbl_PROJECT_HR Order By Id"), formatter);
        }
        public HttpResponseMessage GetBranchNameByBankId(int CatId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name,BankId AS Cat_Id From tbl_BRANCH_OF_BANK_HR WHERE BankId = " + CatId + " Order By Id"), formatter);
        }
        public HttpResponseMessage GetEmployeeDetalsList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetEmployeelDetalsList(searchString), formatter);
        }
        public HttpResponseMessage GetEmployeeList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetEmployeelList(searchString), formatter);
        }
	}
}