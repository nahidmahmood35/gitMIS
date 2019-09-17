using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;
using System.Threading.Tasks;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class PharStockReceiveApiController : ApiController
    {
        readonly PharCompanyGateway _pharCompany = new PharCompanyGateway();
        readonly PharProductGateway _pharProduct = new PharProductGateway();
        readonly PharPurchaseGateway _pharPurchase = new PharPurchaseGateway();
       


        public async Task<HttpResponseMessage> Save([FromBody] List<PharPurchaseModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                string msg = await _pharPurchase.Save(aModels);
               
                _pharPurchase.PrintReportPhar("PharPurchaseInvoice.rpt", "VW_PHAR_PURCHASE_INVOICE", " WHERE SMInv='"+ aModels.ElementAt(0).InvoiceNo +"'", "VW_PHAR_PURCHASE_INVOICE", "Purchase Invoice", "", "V");

                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }
        
        public HttpResponseMessage GetComapanyList(int param)
        {
           var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _pharCompany.GetCompanyList(param), formatter);
        }
        
        public HttpResponseMessage GetMedicineList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _pharCompany.GetIdNameForDropDownBox("SELECT Id,Name From tbl_PHAR_PRODUCT Order by Id"), formatter);
        }

        public HttpResponseMessage GetProductDetailById(string param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _pharProduct.GetPharProductList(param), formatter);
        }


        public HttpResponseMessage GetReprintInvoice(string refNo)
        {
            _pharPurchase.PrintReportPhar("PharPurchaseInvoice.rpt", "VW_PHAR_PURCHASE_INVOICE", " WHERE SMInv='" + refNo + "'", "VW_PHAR_PURCHASE_INVOICE", "Purchase Invoice", "", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }

         public HttpResponseMessage GetInvoiceList(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _pharPurchase.GetPharInvoiceList("0",userName), formatter);
        }

        

    }
}
