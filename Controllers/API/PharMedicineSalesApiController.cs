using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class PharMedicineSalesApiController : ApiController
    {

        readonly PharSalesModel _pharSales = new PharSalesModel();
        readonly PharSalesGateway _salesGateway = new PharSalesGateway();
        readonly PharProductGateway _productGateway = new PharProductGateway();
      
        public async Task<HttpResponseMessage> Save([FromBody] List<PharSalesModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                string msg = "";
                bool stasus = true;
                string itemName = "";

                foreach (var item in aModels)
                {
                    double currentQty = _salesGateway.GetPharStock(item.ItemId, item.UserName);
                    double outQty = item.Quantity;
                    if (currentQty < outQty)
                    {
                        stasus = false;
                        itemName = item.Name;
                       // break;
                    }
                }


                if (stasus)
                {
                    msg = await _salesGateway.Save(aModels);
                    _salesGateway.PrintReportPhar("PharMedicineSales.rpt", "VW_PHAR_MEDICINE_SALES", "WHERE InvoiceNo='" + aModels.ElementAt(0).InvoiceNo + "'", "VW_PHAR_MEDICINE_SALES", "Sales Invoice", "", "V");

                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "success", Msg = msg }, formate);
                }

                msg = "No Enough Stock For " + itemName;

                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "error", Msg = msg });

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }

        public HttpResponseMessage GetMedicineName(string searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _productGateway.GetPharProductList(searchString), formatter);
        }


        public HttpResponseMessage GetMedicineInfoById(int param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _productGateway.GetMedicineInfoById(param), formatter);
        }


        public HttpResponseMessage GetBankList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _salesGateway.GetIdNameForDropDownBox("SELECT Id,Name FROm tbl_BANK Order By Id"), formatter);
        }

        public HttpResponseMessage GetPharStock(int searchString,string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _salesGateway.GetPharStock(searchString,userName), formatter);
        }



        public HttpResponseMessage GetReprintInvoice(string refNo)
        {
            _salesGateway.PrintReportPhar("PharMedicineSales.rpt", "VW_PHAR_MEDICINE_SALES", " WHERE InvoiceNo='" + refNo + "'", "VW_PHAR_MEDICINE_SALES", "Sales Invoice", "", "V");
          
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }

        public HttpResponseMessage GetInvoiceList(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _salesGateway.GetPharInvoiceList("0",userName), formatter);
        }
     














    }
}
