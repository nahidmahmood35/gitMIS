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
    public class InvStockReceiveApiController : ApiController
    {
        readonly InvStokeProductRegistrationGatway _gtInvStokeProductRegistrationGatway = new InvStokeProductRegistrationGatway();
        readonly InvStockReceiveGateway _gtInvStockReceiveGateway = new InvStockReceiveGateway();
        public HttpResponseMessage GetCompanyList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStokeProductRegistrationGatway.GetIdCasCadeDropDown("Select Id, Name from tbl_INVSTOCK_Supplier_info Order By Id"), formatter);
        }
        public HttpResponseMessage GetDepartmentList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStokeProductRegistrationGatway.GetIdCasCadeDropDown("Select IdNo as Id, SubsubPNo AS Name from project where MainPNO='Inventory' AND Pno='Inventory' AND SubPNo='Inventory' order by IdNo"), formatter);
        }

        public HttpResponseMessage GetSaveCompanyInfo(string name, string address, double opBalance, string contact)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtInvStokeProductRegistrationGatway.FncSeekRecordNew("tbl_INVSTOCK_Supplier_info", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);

            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStockReceiveGateway.SaveCompanyWithReturnId("tbl_INVSTOCK_Supplier_info", name, address, opBalance, contact), formatter);


        }

        public HttpResponseMessage GetProductInfo(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStockReceiveGateway.GetProductList(searchString), formatter);
        }

        public async Task<HttpResponseMessage> Save([FromBody] List<InvStockReceiveModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                if (aModels[0].InvoiceNo == "0")
                {
                    string msg = await _gtInvStockReceiveGateway.Save(aModels);
                    InventoryStockrRceiveController dt=new InventoryStockrRceiveController();
                    dt.GetReportView(msg);
                    //_gtInvStockReceiveGateway.PrintReportHr("INVSTOCK_PURCHASE_INVOICE_ReportFile.rpt", "VW_INVSTOCK_PURCHASE_INVOICE", " WHERE InvoiceNo='" + msg + "'", "VW_INVSTOCK_PURCHASE_INVOICE", "Purchase Invoice", "", "V");
                    return Request.CreateResponse(HttpStatusCode.OK,new Confirmation {Output = "success", Msg = msg}, formate);

                }
                else
                {
                    string msg = await _gtInvStockReceiveGateway.Update(aModels);
                     _gtInvStockReceiveGateway.PrintReportHr("INVSTOCK_PURCHASE_INVOICE_ReportFile.rpt", "VW_INVSTOCK_PURCHASE_INVOICE", " WHERE InvoiceNo='" + aModels.ElementAt(0).InvoiceNo + "'", "VW_INVSTOCK_PURCHASE_INVOICE", "Purchase Invoice", "", "V");
                    return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "success", Msg = msg }, formate);
                }
             
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }

        public HttpResponseMessage GetRequisitionListTable()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStockReceiveGateway.GetRequisitionList(), formatter);
        }

        public HttpResponseMessage GetInvoiceDetails(String InvoiceId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStockReceiveGateway.GetInvoiceDetails(InvoiceId), formatter);
        }
	}
    
}