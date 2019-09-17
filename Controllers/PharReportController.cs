using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers
{
    public class PharReportController : Controller
    {
        readonly PharSalesModel _pharSales = new PharSalesModel();
        readonly PharSalesGateway _salesGateway = new PharSalesGateway();
        readonly PharProductGateway _productGateway = new PharProductGateway();

        // GET: /PharReport/
        public ActionResult Index()
        {
            return View();
        }
        public class ReportPrint
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string CustId { get; set; }
            public string ItemId { get; set; }
            public string SuppId { get; set; }
            public string CatId { get; set; }
            public string BrandId { get; set; }
            public string ReportFileName { get; set; }
            public int Id { get; set; }
        }

        public JsonResult GetPharSalesPrint(ReportPrint aPrint)
        {

            int invN = 0;
            if (aPrint.Id == 0)
            {
                int idN = Convert.ToInt32(_salesGateway.ReturnFieldValue("tbl_PHAR_SALES_MASTER", "", "MAX(Id)"));
                invN = Convert.ToInt32(_salesGateway.ReturnFieldValue("tbl_PHAR_SALES_MASTER", " Id=" + idN + " ", "InvoiceNo"));

            }
            else { invN = aPrint.Id; }
            _salesGateway.PrintReportPhar("PharMedicineSales.rpt", "VW_PHAR_MEDICINE_SALES", "WHERE InvoiceNo='" + invN + "'", "VW_PHAR_MEDICINE_SALES", "Invoice", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + "  ", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Json("", JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetPharSalesPrint2(ReportPrint aPrint)
        {

            string lcCondition = "WHERE TrDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "'";
            _salesGateway.PrintReportPhar("PharSalesCollection.rpt", "VW_PHAR_SALES_COLLECTION", lcCondition, "VW_PHAR_SALES_COLLECTION", "Medicine Sales", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + "  ", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPharSalesPrint3(ReportPrint aPrint)
        {
            string lcCondition = "WHERE TrDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "'";
            _salesGateway.PrintReportPhar("PharMedicineSalesAndCollection.rpt", "VW_PHAR_MEDICINE_SALES_COLLECTION", lcCondition, "VW_PHAR_MEDICINE_SALES_COLLECTION", "Sales And Collection", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + "  ", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPharSalesPrint4(ReportPrint aPrint)
        {
            string lcCondition = "WHERE TrDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "'";
            _salesGateway.PrintReportPhar("PharInvoiceRegister(DueCollection).rpt", "VW_PHAR_INVOICE_REG_DUE_COLL", lcCondition, "VW_PHAR_INVOICE_REG_DUE_COLL", "Invoic Register Due Collection", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + "  ", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPharSalesPrint5(ReportPrint aPrint)
        {
            string lcCondition = "WHERE TrDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "'";
            _salesGateway.PrintReportPhar("PharDueInvoiceList(Detail).rpt", "VW_PHAR_DUE_INVOICE_LIST_DETAILS", lcCondition, "VW_PHAR_DUE_INVOICE_LIST_DETAILS", "Due Invoice List(Details)", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPharSalesPrint7(ReportPrint aPrint)
        {
            string lcCondition = "WHERE InvoiceDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "'";
            _salesGateway.PrintReportPhar("PharProfitReportSales.rpt", "VW_PHAR_PROFIT_REPORT_SALES", lcCondition, "VW_PHAR_PROFIT_REPORT_SALES", "Profit Refort Pharma Sales", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Json("", JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetPharSalesPrint8(ReportPrint aPrint)
        {
            string lcCondition = " WHERE TrDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' Order By Trdate ASC";
            _salesGateway.PrintReportPhar("PharSalesSummary(UserWise).rpt", "VW_SELES_AND_COLL_REPORT_USER_WISE_SUMMARY", lcCondition, "VW_SELES_AND_COLL_REPORT_USER_WISE_SUMMARY", "Sales and Coll Report User Wise Summary", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPharSalesPrint9(ReportPrint aPrint)
        {
            string lcCondition = " WHERE TrDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' Order By Trdate ASC";
            //_salesGateway.PrintReportPhar("NetSalesReport(OutletWise).rpt", "VW_SELES_AND_COLL_REPORT_USER_WISE_SUMMARY", lcCondition, "VW_SELES_AND_COLL_REPORT_USER_WISE_SUMMARY", "Sales and Coll Report User Wise Summary", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");

            _salesGateway.PrintReportPhar("Test.rpt", "VW_SELES_AND_COLL_REPORT_USER_WISE_SUMMARY", lcCondition, "VW_SELES_AND_COLL_REPORT_USER_WISE_SUMMARY", "Sales and Coll Report User Wise Summary", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
            
            var formatter = RequestFormat.JsonFormaterString();
            return Json("", JsonRequestBehavior.AllowGet);
        }





	}
}