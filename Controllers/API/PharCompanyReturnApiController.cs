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
    public class PharCompanyReturnApiController : ApiController
    {
        readonly PharCompanyReturnGateway _gt=new PharCompanyReturnGateway();

     //   _gateway.GetCompanyProductReturnList();


        public async Task<HttpResponseMessage> Save([FromBody] List<PharSalesModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                string msg = await _gt.Save(aModels);
                   _gt.PrintReportPhar("PharCompanyReturn.rpt", "VW_PHAR_COMPANY_RTN", " WHERE Id='" + aModels.ElementAt(0).InvMasterId + "'", "VW_PHAR_COMPANY_RTN", "Company Return Invoice", "", "V");
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }




        public HttpResponseMessage GetCompanyProductReturnList(int companyId, string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetCompanyProductReturnList(companyId,name), formatter);
        }


        public HttpResponseMessage GetPharCompanyReturnMedicineInfo(int itemId, int companyId)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharCompanyReturnMedicineInfo(itemId,companyId), formatter);
        }



        public HttpResponseMessage GetMedicineListByComapanyId(int companyId, string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetMedicineListByComapanyId(companyId,userName), formatter);
        }

      

        //public HttpResponseMessage GetReprintInvoice(string refNo)
        //{
        //    _gt.PrintReportPhar("PharMedicineReturn.rpt", "VW_PHAR_MEDICINE_RETURN", " WHERE InvoiceNo='" + refNo + "'", "VW_PHAR_MEDICINE_RETURN", "Return Invoice", "", "V");
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        //}

        //public HttpResponseMessage GetInvoiceList(string userName)
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharInvoiceList("0", userName), formatter);
        //}




















    }
}
