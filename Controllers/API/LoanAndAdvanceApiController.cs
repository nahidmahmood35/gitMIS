using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;
using Microsoft.SqlServer.Server;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class LoanAndAdvanceApiController : ApiController
    {
       // readonly EmployeeGetway _gtEmployeeGetway = new EmployeeGetway();
         readonly LoanAndAdvanceGatway _loanAndAdvanceGatway = new LoanAndAdvanceGatway();
        public HttpResponseMessage GetEmployeeId()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _loanAndAdvanceGatway.GetIdCasCadeDropDown("SELECT Id,Code AS Name From tbl_HR_GLO_EMPLOYEE Order By Id"), formatter);
        }

        public HttpResponseMessage GetEmployeeDetailById(string param)
        {

            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _loanAndAdvanceGatway.GetEmployeeDetailById(param), formatter);

        }

        public HttpResponseMessage GetLoanId()
        {

            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _loanAndAdvanceGatway.GetAutoIncrementNumberFromStoreProcedure(3), formatter);

        }

        public HttpResponseMessage GetLoanType(string param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _loanAndAdvanceGatway.GetIdCasCadeDropDown("Select Id,SalaryNameBD as Name from tbl_HR_GLO_SALARY_TYPE where Validity = 1 AND SalaryType = 'd' AND Id NOT IN (select aa.LoanTypeId from (select LoanTypeId from tbl_HR_LOAN_LEDGER group by LoanTypeId having SUM(LoanAmt-PaidAmount) <> 0 Union all Select SalaryTypeId from tbl_HR_GLO_EMPLOYEE_DLS where Amount>0 And EmpId=" + param + ") aa group by aa.LoanTypeId )"), formatter);
        }

        public async Task<HttpResponseMessage> Post([FromBody] LoanAndAdvanceModel mLoanAndAdvanceModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {

                if (mLoanAndAdvanceModel.EmployeeId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Employee Code is Empty" }, formatter);
                }
                else
                {
                    msg = await _loanAndAdvanceGatway.Save(mLoanAndAdvanceModel);
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation {Output = "success", Msg = msg}, formatter);
                }


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }

        }

        public HttpResponseMessage GetUpdateLoanInstallment(int EmpId, int LoanId, int Amount)
        {

            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _loanAndAdvanceGatway.GetUpdateLoanInstallment(EmpId, LoanId, Amount), formatter);

        }
        public HttpResponseMessage GetLoanList()
        {

            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _loanAndAdvanceGatway.GetLoanList(), formatter);

        }
        public HttpResponseMessage GetLoanSheet(int EmpId)
        {
            try
            {
                var formatter = RequestFormat.JsonFormaterString();
                string condition = "";
                condition = "WHERE EmpId='" + EmpId + "'";

                return Request.CreateResponse(HttpStatusCode.OK, _loanAndAdvanceGatway.PrintReportHr("EmployeeLoanReportFile.rpt", "VW_HR_ENPLOYEE_LOAN_REPORT", condition, "VW_HR_ENPLOYEE_LOAN_REPORT", "Loan Sheet", "", "V"), formatter);

            }
            catch (Exception ex)

            {

                throw;
            }

        }
      
    }
}