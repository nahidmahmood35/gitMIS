using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway.DB_Helper;

namespace HospitalManagementApp_Api.Controllers
{
    public class InventoryStockrRceiveController : Controller
    {
        readonly DbConnection _gt=new DbConnection();
        public ActionResult Index()
        {
            return View();
        }


        public string GetReportView(string invNo)
        {
            _gt.PrintReportHr("INVSTOCK_PURCHASE_INVOICE_ReportFile.rpt", "VW_INVSTOCK_PURCHASE_INVOICE", " WHERE InvoiceNo='" + invNo + "'", "VW_INVSTOCK_PURCHASE_INVOICE", "Purchase Invoice", "", "V");
            return "";
        }



	}
}