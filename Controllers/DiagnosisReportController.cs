using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway.DB_Helper;

namespace HospitalManagementApp_Api.Controllers
{
    public class DiagnosisReportController : Controller
    {
        readonly DbConnection _db=new DbConnection();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string Index(ReportPrint aPrint)
        {
            string lcCondition = "";
            switch (aPrint.ReportFileName)
            {
                case "DiagnosisInvoiceLedger":
                    lcCondition = "69,'" + aPrint.DateFrom + "','" + aPrint.DateTo + "',"+ aPrint.RegId +"";
                    _db.PrintReport("DiagnosisInvoiceLedger.rpt", "SP_GET_INVOICE_LEDGER", lcCondition, "SP_GET_INVOICE_LEDGER", "Invoice Ledger", "Reporting date from " + aPrint.DateFrom + " to " + aPrint.DateTo, "S");
                    break;
                case "DiagnosisDueInvoice":
                    lcCondition = "WHERE SubSubPnoId=69";
                    if (aPrint.RegId!=0){lcCondition += "AND RegId="+ aPrint.RegId +"";}
                    _db.PrintReport("DueInvoiceList.rpt", "VW_DUE_INVOICE_LIST", lcCondition, "VW_DUE_INVOICE_LIST", "Due Invoice List", "", "V");
                    break;
                case "DatewiseCollectionReport":
                    lcCondition = "WHERE TrDate Between  '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' AND  SubSubPnoId=69 Order By Id";
                    if (aPrint.RegId != 0) { lcCondition += "AND RegId=" + aPrint.RegId + ""; }
                    _db.PrintReport("DatewiseCollection.rpt", "VW_GET_DATEWISE_COLLECTION", lcCondition, "VW_GET_DATEWISE_COLLECTION", "Datewise Collection", "Reporting date from " + aPrint.DateFrom + " to " + aPrint.DateTo, "V");
                    break;
                case "SalesLedgerSummarized":
                    lcCondition = "'" + aPrint.DateFrom + "','" + aPrint.DateTo + "',"+ aPrint.RegId +"";
                    _db.PrintReport("SalesLedgerSummarized.rpt", "SP_GET_INVOICE_LEDGER", lcCondition, "SP_GET_INVOICE_LEDGER", "Invoice Ledger", "Reporting date from " + aPrint.DateFrom + " to " + aPrint.DateTo, "S");
                    break;

                    

            }

            return "";
        }
        public class ReportPrint
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public int RegId { get; set; }
            public string ReportFileName { get; set; }
            public int SubSubPnoId { get; set; }
        }
	}
}