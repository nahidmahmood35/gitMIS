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
    public class PharSubStoreSalesApiController : ApiController
    {
        readonly  PharSubStoreSalesGateway _gt=new PharSubStoreSalesGateway();
        readonly  PharProductGateway _productGateway =new PharProductGateway();


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
                    double currentQty = _gt.GetPharStock(item.ItemId, item.UserName, item.SubSubPnoId);
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
                    msg = await _gt.Save(aModels);
                    _gt.PrintReportPhar("PharMedicineSales.rpt", "VW_PHAR_MEDICINE_SALES", "WHERE InvoiceNo='" + aModels.ElementAt(0).InvoiceNo + "'", "VW_PHAR_MEDICINE_SALES", "Sales Invoice", "", "V");

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



        public HttpResponseMessage GetDepartmentList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetIdNameForDropDownBox("SELECT IdNo as Id,SubsubPNo as Name From project Order By Id"), formatter);
        }


        public HttpResponseMessage GetMedicineInfoById(int param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _productGateway.GetMedicineInfoById(param), formatter);
        }


        public HttpResponseMessage GetPharStock(int searchString, string userName,int pno)
        
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharStock(searchString, userName,pno), formatter);
        }



    }
}
