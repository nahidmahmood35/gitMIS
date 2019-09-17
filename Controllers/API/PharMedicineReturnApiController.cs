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
    public class PharMedicineReturnApiController : ApiController
    {
        readonly PharMedicineReturnGateway _gt = new PharMedicineReturnGateway();
        public async Task<HttpResponseMessage> Save([FromBody] List<PharSalesModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                string msg = await _gt.Save(aModels);
                _gt.PrintReportPhar("PharMedicineReturn.rpt", "VW_PHAR_MEDICINE_RETURN", "WHERE Id='" + aModels.ElementAt(0).InvMasterId + "'", "VW_PHAR_MEDICINE_RETURN", "Return Invoice", "", "V");
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }


        public HttpResponseMessage GetPharProductList(string param,string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharProductList(param, userName), formatter);
        }

        public HttpResponseMessage GetReprintInvoice(string refNo)
        {
            _gt.PrintReportPhar("PharMedicineReturn.rpt", "VW_PHAR_MEDICINE_RETURN", " WHERE InvoiceNo='" + refNo + "'", "VW_PHAR_MEDICINE_RETURN", "Return Invoice", "", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }

        public HttpResponseMessage GetInvoiceList(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharInvoiceList("0",userName), formatter);
        }






    }
}
