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
    public class DueCollectionApiController : ApiController
    {
        readonly DueCollectionGateway _gtDueColl = new DueCollectionGateway();
        #region
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        
        public async Task<HttpResponseMessage> Post([FromBody] InvoiceModel mInvoice)
        {
            var formatter = RequestFormat.JsonFormaterString();
            try
            {
                if (mInvoice.PtRegId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Patient Name Is Empty" }, formatter);
                }

                if (mInvoice.UserName == "")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "UserName is Empty" }, formatter);
                }
                if (mInvoice.InvMasterId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Invoice No is Empty" }, formatter);
                }

                double amt = mInvoice.CashAmount + mInvoice.CheaqueAmount +mInvoice.CardAmount;
                if (amt == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Amount is Empty" }, formatter);
                }
                else
                {
                    string msg = await _gtDueColl.Save(mInvoice);
                    //gtInvoice.PrintReport("DiagnosisInvoiceRPT.rpt", "SP_GET_INVOICE_PRINT", "" + mInvoice.ElementAt(0).InvMasterId + "", "SP_GET_INVOICE_PRINT", "", "", "S");
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formatter);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }
        #endregion

        //public HttpResponseMessage GetInvoicePrint(int id)
        //{
        //    int idN = Convert.ToInt32(_gtInvoice.ReturnFieldValue("tbl_INVOICE_MASTER", "", "MAX(Id)"));
        //    _gtInvoice.PrintReport("DiagnosisInvoiceRPT.rpt", "SP_GET_INVOICE_PRINT", "" + idN + "", "SP_GET_INVOICE_PRINT", "", "", "S");
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        //}



        public HttpResponseMessage GetDueInvoiceList(string param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtDueColl.GetDueInvoiceList(param), formatter);
        }
        public HttpResponseMessage GetMaxLessAmountInDueCollection(int invMasterId, int lessFrom)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtDueColl.GetMaxLessAmountInDueCollection(invMasterId,lessFrom), formatter);
        }














    }
}
