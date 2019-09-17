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
    public class PayRegisterGlobalApiController : ApiController

    {
        readonly EmployeeGetway _gtEmployeeGetway = new EmployeeGetway();
        readonly PayRegisterGlobalGateway _gtPayRegisterGlobalGateway = new PayRegisterGlobalGateway();

        public HttpResponseMessage GetEmployeeCode()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("Select DISTINCT a.EmpId AS Id,b.Code AS Name from tbl_HR_GLO_PAYREGISTER AS a inner join tbl_HR_GLO_EMPLOYEE AS b ON a.EmpId = b.Id  "), formatter);
        }

        public HttpResponseMessage GetMonthInfo()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("Select Id,Name from tbl_HR_MONTH_INFO"), formatter);
        }


        public HttpResponseMessage GetEmployeeBankName()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name From tbl_BANK Order By Id"), formatter);
        }

        public HttpResponseMessage GetEmployeeBankBranceName(int CatId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtEmployeeGetway.GetIdCasCadeDropDown("SELECT Id,Name,BankId AS Cat_Id From tbl_BRANCH_OF_BANK_HR WHERE BankId = " + CatId + " Order By Id"), formatter);
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



        public HttpResponseMessage GetEmployeeDetalsList(int EmpId, int Month, int Year)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (EmpId == 0 || Month == 0 || Year == 0 )
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation {Output = "error", Msg = "please Selete Employee Code,Month and Year!!!"},
                    formatter);
            }
            if (_gtEmployeeGetway.FncSeekRecordNew("tbl_HR_GLO_PAYREGISTER", "EmpId=" + EmpId + " AND MonthId=" + Month + " AND Year=" + Year + "") == false)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "error", Msg = "This Employee Salary Are Not Create" },
                    formatter);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, _gtPayRegisterGlobalGateway.GetEmployeelDetalsList(EmpId, Month, Year), formatter);
            }
      
        }

        public async Task<HttpResponseMessage> Post([FromBody] List<EmployeeModel> mEmployeeModel)
        {

            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {
                msg = await _gtPayRegisterGlobalGateway.Save(mEmployeeModel);
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
