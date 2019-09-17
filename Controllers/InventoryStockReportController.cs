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
    public class InventoryStockReportController : Controller
    {
        readonly InventoryStockReportGateway _gt = new InventoryStockReportGateway();
        public ActionResult Index()
        {
            return View();
        }

        //public HttpResponseMessage GetGenderList()
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, _gt.GetIdCasCadeDropDown("SELECT Id,Bname AS Name From tbl_GENDER_INFO_MST Order By Id"), formatter);
        //}

        public JsonResult GetSupplierList()
        {
            var data = _gt.GetIdCasCadeDropDown(" SELECT Id,name AS Name From tbl_INVSTOCK_Supplier_info Order By Id");
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCatagoryList()
        {
            var data = _gt.GetIdCasCadeDropDown("SELECT DISTINCT c.Id,c.Name From tbl_INVSTOCK_StockLedger AS a left join tbl_INVSTOCK_SalesProductList AS b On a.ItemId=b.Id inner join tbl_INVSTOCK_ProductCategory AS c ON b.ProductCategoryId=c.Id");
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreNameList()
        {
            var data = _gt.GetIdCasCadeDropDown("Select IdNo AS Id,SubsubPNo AS Name from project where MainPNO='Inventory';");
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public class ReportPrint
        {
            public string DateFrom { get; set; }
            public string ReportId { get; set; }
            public string DateTo { get; set; }
            public int CompanyId { get; set; }
            public int ProductId { get; set; }
            public int CatagoryId { get; set; }
            public int PnoId { get; set; }
   
        }

        public string GetReportView(ReportPrint aPrint)
        {
            //string lcCondition = "";
            switch (aPrint.ReportId)
            {
                    
                case "StockSummary":
                    string lcCondition = "where 1=1 AND EntryDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' ";
                    if (aPrint.CompanyId!=0)
                    {
                        lcCondition += "And supplierId=" + aPrint.CompanyId + " ";
                    }
                    if (aPrint.ProductId != 0)
                    {
                        lcCondition += "And ProductId=" + aPrint.ProductId + " ";
                    }
                    if (aPrint.CatagoryId != 0)
                    {
                        lcCondition += "And catagoryId=" + aPrint.CatagoryId + " ";
                    }
                    if (aPrint.PnoId != 0)
                    {
                        lcCondition += "And PnoId=" + aPrint.PnoId + " ";
                    }
                    _gt.PrintReportHr("InvStockSummaryReport.rpt", "VW_INVSTOCK_StockSummary_Report", lcCondition, "VW_INVSTOCK_StockSummary_Report", "Stock Summary Report ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                    break;


                case "StockBalance":
                    string lcCondition2 = "where 1=1 AND EntryDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' ";
                    //if (aPrint.CompanyId != 0)
                    //{
                    //    lcCondition2 += "And supplierId=" + aPrint.CompanyId + " ";
                    //}
                    if (aPrint.ProductId != 0)
                    {
                        lcCondition2 += "And ProductId=" + aPrint.ProductId + " ";
                    }
                    if (aPrint.CatagoryId != 0)
                    {
                        lcCondition2 += "And catagoryId=" + aPrint.CatagoryId + " ";
                    }
                    if (aPrint.PnoId != 0)
                    {
                        lcCondition2 += "And PnoId=" + aPrint.PnoId + " ";
                    }
                    _gt.PrintReportHr("InvStockBalanceReport.rpt", "VW_INVSTOCK_Balance_report", lcCondition2, "VW_INVSTOCK_Balance_report", "Stock Balance Report ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                    break;
                case "StockIn":
                    string lcCondition3 = "where 1=1 AND EntryDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' ";
                    //if (aPrint.CompanyId != 0)
                    //{
                    //    lcCondition2 += "And supplierId=" + aPrint.CompanyId + " ";
                    //}
                    //if (aPrint.ProductId != 0)
                    //{
                    //    lcCondition2 += "And ProductId=" + aPrint.ProductId + " ";
                    //}
                    //if (aPrint.CatagoryId != 0)
                    //{
                    //    lcCondition2 += "And catagoryId=" + aPrint.CatagoryId + " ";
                    //}
                    //if (aPrint.PnoId != 0)
                    //{
                    //    lcCondition2 += "And PnoId=" + aPrint.PnoId + " ";StockLedger
                    //}
                    _gt.PrintReportHr("InvStockReceivedReport.rpt", "VW_INVSTOCK_Received", lcCondition3, "VW_INVSTOCK_Received", "Stock Received Report ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                    break;
                case "StockLedger":
                    string lcCondition4 = "where 1=1 AND EntryDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' ";
                    //if (aPrint.CompanyId != 0)
                    //{
                    //    lcCondition2 += "And supplierId=" + aPrint.CompanyId + " ";
                    //}
                    if (aPrint.ProductId != 0)
                    {
                        lcCondition4 += "And ProductId=" + aPrint.ProductId + " ";
                    }
                    if (aPrint.CatagoryId != 0)
                    {
                        lcCondition4 += "And catagoryId=" + aPrint.CatagoryId + " ";
                    }
                    if (aPrint.PnoId != 0)
                    {
                        lcCondition4 += "And PnoId=" + aPrint.PnoId + " ";
                    }
                    _gt.PrintReportHr("InvStockLedgerReport.rpt", "VW_INVSTOCK_Ledger_Report", lcCondition4, "VW_INVSTOCK_Ledger_Report", "Stock Ledger Report ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                    break;
                //case "ExpiredItemList":
                //    string firstCond = "  WHERE BranchId=" + _branchId + " ";
                //    string secondCond = "";
                //    if (aPrint.ComId != 0) { secondCond = " AND CompanyId=" + aPrint.ComId + " "; }
                //    string lcCondition = firstCond + secondCond + "   AND ExpireDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' ";
                //    _salesGateway.PrintReportPhar("PharExpiredItemList.rpt", "VW_PHAR_EXPIRED_ITEM_LIST", lcCondition, "VW_PHAR_EXPIRED_ITEM_LIST", "Expired Item Report ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                //    break;

                //case "PurchaseLedgerSummary":
                //    int comId = 0;
                //    if (aPrint.ComId != 0) { comId = aPrint.ComId; }
                //    lcCondition = " '" + aPrint.DateFrom + "','" + aPrint.DateTo + "' ," + comId + "," + _branchId + "   ";
                //    _salesGateway.PrintReportPhar("PharPurchaseLedgerSummary.rpt", "SP_PURCHASE_LEDGER_SUMMARY", lcCondition, "SP_PURCHASE_LEDGER_SUMMARY", "Purchase Ledger Summary Report ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "S");
                //    break;

                //case "ReminderStockByCompany":
                //    lcCondition = "  WHERE BranchId=" + _branchId + " ";
                //    if (aPrint.ComId != 0) { lcCondition = lcCondition + "   AND CompanyId=" + aPrint.ComId + " "; }
                //    _salesGateway.PrintReportPhar("PharReminderStockByCom.rpt", "VW_PHAR_REMINDERSTOCK_BY_COM", lcCondition, "VW_PHAR_REMINDERSTOCK_BY_COM", " Reminder Stock Company Wise Report ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                //    break;

                //case "SalesCollectionByUser":
                //    lcCondition = "   WHERE trDate BETWEEN '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "' AND BranchId=" + _branchId + " ORDER BY TrDate ";
                //    _salesGateway.PrintReportPhar("PharSalesCollectionByUser.rpt", "VW_PHAR_SALES_AND_COLL_BY_USER", lcCondition, "VW_PHAR_SALES_AND_COLL_BY_USER", " Sales Collection Report ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                //    break;

                //case "InvoiceRegisterPurchase":
                //    secondCond = "";
                //    if (aPrint.ComId != 0) { secondCond = " AND CompanyId=" + aPrint.ComId + " "; }
                //    lcCondition = "  WHERE BranchId=" + _branchId + " " + secondCond + " ORDER BY ItemName  ";
                //    _salesGateway.PrintReportPhar("PharInvoiceRegisterPurchase.rpt", "VW_PHAR_INVOICE_REGISTER_PURCHASE", lcCondition, "VW_PHAR_INVOICE_REGISTER_PURCHASE", "Invoice Register Purchase ", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                //    break;

                //case "CurrentStockPosition":
                //    lcCondition = "  WHERE BranchId=" + _branchId + " ORDER BY Name ASC ";
                //    _salesGateway.PrintReportPhar("PharCurrentStockPosition.rpt", "VW_PHAR_CURRENT_STOCK_POSITION", lcCondition, "VW_PHAR_CURRENT_STOCK_POSITION", "Current Stock Position", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "V");
                //    break;

                //case "PurchaseLedger":
                //    lcCondition = " '" + aPrint.DateFrom + "','" + aPrint.DateTo + "' ," + aPrint.ComId + "," + _branchId + "   ";
                //    _salesGateway.PrintReportPhar("PharPurchaseLedger.rpt", "SP_PHAR_PURCHASE_LEDGER", lcCondition, "SP_PHAR_PURCHASE_LEDGER", "Purchase Ledger", "Reporting Date From " + aPrint.DateFrom + "  to " + aPrint.DateTo + " ", "S");
                //    break;



                case "NEWSWITCH":

                    break;


            }
            return "";
        }
	}
}