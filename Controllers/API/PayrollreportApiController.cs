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
    public class PayrollreportApiController : ApiController
    {
        readonly PayrollreportGateway _gtPayrollreportGateway = new PayrollreportGateway();
        public HttpResponseMessage GetEmployeeDetalsList(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.GetEmployeelList(searchString), formatter);
        }
        public HttpResponseMessage GetPaySlipGov(int employeeId, int monthId, int year)
        {
            var formatter = RequestFormat.JsonFormaterString();
            bool m;
            if (employeeId == 0)
            {
                
                m = _gtPayrollreportGateway.FncSeekRecordNew("VW_EEmployee_playSlip_GLOBEL2", " MonthId='" + monthId + "' AND Year='" + year + "'");


                if (m == true)
                {
                    try
                    {

                        string condition = "";
                        condition = "WHERE  MonthId='" + monthId + "' AND Year='" + year + "'";

                        return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("PaySlipGovReportFile2.rpt", "VW_EEmployee_playSlip_GLOBEL2", condition, "VW_EEmployee_playSlip_GLOBEL2", "Pay Slip", "", "V"), formatter);

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "This employee salary are not created.!!" }, formatter);
                }

            }
            else
            {
                
                m = _gtPayrollreportGateway.FncSeekRecordNew("VW_EEmployee_playSlip_GLOBEL2", " EmpId='" + employeeId + "' AND MonthId='" + monthId + "' AND Year='" + year + "'");


                if (m == true)
                {
                    try
                    {

                        string condition = "";
                        condition = "WHERE EmpId='" + employeeId + "' AND MonthId='" + monthId + "' AND Year='" + year + "'";

                        return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("PaySlipGovReportFile2.rpt", "VW_EEmployee_playSlip_GLOBEL2", condition, "VW_EEmployee_playSlip_GLOBEL2", "Pay Slip", "", "V"), formatter);

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "This employee salary are not created.!!" }, formatter);
                }

            }
            


        }
        public HttpResponseMessage GetFutureFund(int employeeId, int monthId, int year)
        {
            var formatter = RequestFormat.JsonFormaterString();
            bool m;
            if (employeeId == 0)
            {

                m = _gtPayrollreportGateway.FncSeekRecordNew("VW_HR_REPORT_FUTURE_FUND_DETACTION", " MonthId='" + monthId + "' AND Year='" + year + "'");


                if (m == true)
                {
                    try
                    {

                        string condition = "";
                        condition = "WHERE  MonthId='" + monthId + "' AND Year='" + year + "'";

                        return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("VobissoTahabilReportFile.rpt", "VW_HR_REPORT_FUTURE_FUND_DETACTION", condition, "VW_HR_REPORT_FUTURE_FUND_DETACTION", "Future Fand Detection", "", "V"), formatter);

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "This employee salary are not created.!!" }, formatter);
                }

            }
            else
            {

                m = _gtPayrollreportGateway.FncSeekRecordNew("VW_HR_REPORT_FUTURE_FUND_DETACTION", " EmpId='" + employeeId + "' AND MonthId='" + monthId + "' AND Year='" + year + "'");


                if (m == true)
                {
                    try
                    {

                        string condition = "";
                        condition = "WHERE EmpId='" + employeeId + "' AND MonthId='" + monthId + "' AND Year='" + year + "'";

                        return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("VobissoTahabilReportFile.rpt", "VW_HR_REPORT_FUTURE_FUND_DETACTION", condition, "VW_HR_REPORT_FUTURE_FUND_DETACTION", "Future Fand Detection", "", "V"), formatter);

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "This employee salary are not created.!!" }, formatter);
                }

            }



        }
        public HttpResponseMessage GetWelfareInsurance(int employeeId, int monthId, int year)
        {
            var formatter = RequestFormat.JsonFormaterString();
            bool m;
            if (employeeId == 0)
            {

                m = _gtPayrollreportGateway.FncSeekRecordNew("VW_HR_REPORT_FUTURE_WelfareFUND_InsuranceFund", " MonthId='" + monthId + "' AND Year='" + year + "'");


                if (m == true)
                {
                    try
                    {

                        string condition = "";
                        condition = "WHERE  MonthId='" + monthId + "' AND Year='" + year + "'";

                        return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("KollanTahabilReportFile.rpt", "VW_HR_REPORT_FUTURE_WelfareFUND_InsuranceFund", condition, "VW_HR_REPORT_FUTURE_WelfareFUND_InsuranceFund", "Future Fand Detection", "", "V"), formatter);

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "This employee salary are not created.!!" }, formatter);
                }

            }
            else
            {

                m = _gtPayrollreportGateway.FncSeekRecordNew("VW_HR_REPORT_FUTURE_WelfareFUND_InsuranceFund", " EmpId='" + employeeId + "' AND MonthId='" + monthId + "' AND Year='" + year + "'");


                if (m == true)
                {
                    try
                    {

                        string condition = "";
                        condition = "WHERE EmpId='" + employeeId + "' AND MonthId='" + monthId + "' AND Year='" + year + "'";

                        return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("KollanTahabilReportFile.rpt", "VW_HR_REPORT_FUTURE_WelfareFUND_InsuranceFund", condition, "VW_HR_REPORT_FUTURE_WelfareFUND_InsuranceFund", "Future Fand Detection", "", "V"), formatter);

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "This employee salary are not created.!!" }, formatter);
                }

            }



        }
        public HttpResponseMessage GetThirdAndFourthClassEmployeePayslip(int monthId, int year)
        {
            var formatter = RequestFormat.JsonFormaterString();
            bool m;
            m = _gtPayrollreportGateway.FncSeekRecordNew("VW_HR_REPORT_PAYSLIP_GOV_GLOBAL", " MonthId='" + monthId + "' AND Year='" + year + "'AND Grade between 10 and 20");


            if (m == true)
            {
                try
                {

                    string condition = "";
                    condition = "WHERE  MonthId='" + monthId + "' AND Year='" + year + "'AND Grade between 10 and 20";

                    return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("PaySlipFor3-4EmployeeReportFile.rpt", "VW_HR_REPORT_PAYSLIP_GOV_GLOBAL", condition, "VW_HR_REPORT_PAYSLIP_GOV_GLOBAL", "Third and Fourth Class Employee Payslip", "", "V"), formatter);

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = " employee's salary are not created.!!" }, formatter);
            }

        }

        public HttpResponseMessage GetSalarySheet(int DepartmentId)
        {
            try
            {
                var formatter = RequestFormat.JsonFormaterString();
                string condition = "";
                condition = "WHERE DepartmentId='" + DepartmentId + "'";

                return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("SalarySheetReportFile.rpt", "VW_HR_SALARY_SHEET", condition, "VW_HR_SALARY_SHEET", "Salary Sheet", "", "V"), formatter);

            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public HttpResponseMessage GetAckCash(int DepartmentId, int MonthId, int Year)
        {
            try
            {
                var formatter = RequestFormat.JsonFormaterString();
                string condition = "";
                condition = "WHERE DeparmentId='" + DepartmentId + "' AND MonthId='" + MonthId + "'AND AttYear='" + Year + "'";

                return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("AcknowledgmentCashReportFile.rpt", "VW_HR_ACKNOWLEDGMENT_CASH_SALARY", condition, "VW_HR_ACKNOWLEDGMENT_CASH_SALARY", "ACKNOWLEDGMENT CASH SALARY", "", "V"), formatter);

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public HttpResponseMessage GetAckBank(int DepartmentId, int MonthId, int Year)
        {
            try
            {
                var formatter = RequestFormat.JsonFormaterString();
                string condition = "";
                condition = "WHERE DeparmentId='" + DepartmentId + "' AND MonthId='" + MonthId + "'AND AttYear='" + Year + "'";

                return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("AcknowledgmentBankReportFile.rpt", "VW_HR_ACKNOWLEDGMENT_BANK_SALARY", condition, "VW_HR_ACKNOWLEDGMENT_BANK_SALARY", "ACKNOWLEDGMENT Bank SALARY", "", "V"), formatter);

            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public HttpResponseMessage GetBankCash(int monthId, int year)
        {
            var formatter = RequestFormat.JsonFormaterString();
            bool m;
            m = _gtPayrollreportGateway.FncSeekRecordNew("VW_HR_PAYROLL_CASHBANK", " MonthId='" + monthId + "' AND Year='" + year + "'");


            if (m == true)
            {
                try
                {

                    string condition = "";
                    condition = "WHERE  MonthId='" + monthId + "' AND Year='" + year + "'";

                    return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("PaySlipCashBankReport.rpt", "VW_HR_PAYROLL_CASHBANK", condition, "VW_HR_PAYROLL_CASHBANK", "Cash And Bank", "", "V"), formatter);

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = " employee's salary are not created.!!" }, formatter);
            }

        }

        public HttpResponseMessage GetBonus(int employeeId, int year, int bonusFor)
        {
            var formatter = RequestFormat.JsonFormaterString();
            bool m;
            m = _gtPayrollreportGateway.FncSeekRecordNew("VW_HR_BONUS_FOR_OFFICER", " EmpId='" + employeeId + "' AND AttYear='" + year + "' AND BonusFor=" + bonusFor + "");


            if (m == true)
            {
                try
                {

                    string condition = "";
                    condition = "WHERE  EmpId='" + employeeId + "' AND AttYear='" + year + "' AND BonusFor=" + bonusFor + "";

                    return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("PaySlipBonusOfficer.rpt", "VW_HR_BONUS_FOR_OFFICER", condition, "VW_HR_BONUS_FOR_OFFICER", "Bonus Report", "", "V"), formatter);

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = " employee's salary are not created.!!" }, formatter);
            }

        }

        public HttpResponseMessage GetBonusFor10To20GredeEmployee(int year, int bonusFor)
        {
            var formatter = RequestFormat.JsonFormaterString();
            bool m;
            m = _gtPayrollreportGateway.FncSeekRecordNew("VW_HR_BONUS_FOR_OFFICER", " AttYear=" + year + " and BonusFor=" + bonusFor + "  AND Grade between 10 and 20");


            if (m == true)
            {
                try
                {

                    string condition = "";
                    condition = " where AttYear=" + year + " and BonusFor=" + bonusFor + "  AND Grade between 10 and 20";

                    return Request.CreateResponse(HttpStatusCode.OK, _gtPayrollreportGateway.PrintReportHr("PaySlipBonusFor10-20EmployeeReportFile.rpt", "VW_HR_BONUS_FOR_OFFICER", condition, "VW_HR_BONUS_FOR_OFFICER", "Bonus Report", "", "V"), formatter);

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = " employee's salary are not created.!!" }, formatter);
            }

        }
	}
}