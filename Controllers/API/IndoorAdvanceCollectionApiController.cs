using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class IndoorAdvanceCollectionApiController : ApiController
    {
        readonly IndoorAdvanceCollectionGateway _gt = new IndoorAdvanceCollectionGateway();
        public  HttpResponseMessage Post([FromBody] AdmissionModel aModel)
        {

            var formate = RequestFormat.JsonFormaterString();
            try
            {
                if (aModel.PtIndoorId==0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "error", Msg = "Name is Null" });
                }
                if (Math.Abs(aModel.TotalAmount) < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = "Charge Can" });
                }
                else
                {
                    string msg=_gt.Save(aModel);
                    return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "success", Msg = msg }, formate);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,new Confirmation { Output = "error", Msg=ex.ToString() }, formate);
            }
        }
        
        public HttpResponseMessage GetReprintAdvanceCollection(int id)
        {
            _gt.PrintReport("IndoorAdvanceCollectionRPT.rpt", "GET_IN_ADVANCE_COLLECTION_LIST", " WHERE Id=" + id + "", "GET_IN_ADVANCE_COLLECTION_LIST", "Advance Collection", "", "V");
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, "Success", formatter);
        }

        public HttpResponseMessage GetTotalAdvanceCollectionByIndoorId(int indoorId)
        {
            double amt =Convert.ToDouble(_gt.ReturnFieldValue("tbl_IN_LEDGER_OF_ADMITTED_PATIENT","IndoorId=" + indoorId + " AND ItemId=1969", "ISNull(SUM(CollAmt),0)"));
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, amt, formatter);
        }
        public HttpResponseMessage GetAdvanceCollectionList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetAdvanceCollectionList(), formatter);
        }



      

    }
}
