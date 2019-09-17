using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;


namespace HospitalManagementApp_Api.Controllers.API
{
    public class OpdApiController : ApiController
    {
        readonly OpdGateway _gtOpd = new OpdGateway();
        #region
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> Post([FromBody] OpdModel mInvoice)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                
                if (mInvoice.UserName == "")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "UserName is Empty" }, formatter);
                }
                else
                {
                    string msg = await _gtOpd.Save(mInvoice);
                 //   _gtInvoice.PrintReport("DiagnosisInvoiceRPT.rpt", "SP_GET_INVOICE_PRINT", "" + mInvoice.ElementAt(0).InvMasterId + "", "SP_GET_INVOICE_PRINT", "", "", "S");
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }
        #endregion

        public HttpResponseMessage GetInvoicePrint(int id)
        {
            if (id==0)
            {
                id = Convert.ToInt32(_gtOpd.ReturnFieldValue("tbl_INVOICE_MASTER", "", "MAX(Id)"));    
            }

            _gtOpd.PrintReport("OpdTicketRPT.rpt", "GET_OPD_TICKET_LIST", "WHERE Id="+ id +" AND SubSubPnoId=78", "GET_OPD_TICKET_LIST", "Opd Ticket", "", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }
        //[System.Web.Http.AcceptVerbs("GET", "POST")]
        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage Get([FromBody] List<InvoiceModel> mInvoice)
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    var amount =  _gtInvoice.GetMaximumLessAmountByDoctor(mInvoice);
        //    return Request.CreateResponse(HttpStatusCode.OK, amount, formatter);
        //}
        //public HttpResponseMessage GetInvoiceList(string dateFrom,string dateTo)
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, _gtInvoice.GetInvoiceList(dateFrom,dateTo), formatter);
        //}


    }
}
