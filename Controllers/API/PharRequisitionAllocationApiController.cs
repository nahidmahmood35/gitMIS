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
    public class PharRequisitionAllocationApiController : ApiController
    {
        readonly  PharRequisitionAllocationGateway _gt=new PharRequisitionAllocationGateway();


        public async Task<HttpResponseMessage> Save([FromBody] List<PharStockRequisitionModel> aModels)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                string msg = await _gt.Update(aModels);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "success", Msg = msg }, formate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }





        public HttpResponseMessage GetPharRequisitionList(string searchString,int status,string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharRequisitionList(searchString,status,userName), formatter);
        }

      //  GetPharRequisitionDetailList


        //public HttpResponseMessage GetPharRequisitionDetailList(string searchString)
        
        //{
        //    var formatter = RequestFormat.JsonFormaterString();
        //    return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharRequisitionDetailList(searchString), formatter);
        //}

        public HttpResponseMessage GetPharRequisitionListTable(string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharRequisitionList("0",1,userName), formatter);
        }



        public HttpResponseMessage GetPharRequisitionDetailList(string searchString,string userName)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetPharRequisitionDetailList(searchString,userName), formatter);
        }



    }
}
