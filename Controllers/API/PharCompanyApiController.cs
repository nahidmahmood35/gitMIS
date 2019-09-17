using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers
{
    public class PharCompanyApiController : ApiController
    {
        readonly PharCompanyGateway _gt=new PharCompanyGateway();


        public async Task<HttpResponseMessage> Save_COMPANY_PHAR([FromBody] PharSalesModel aModel)
        {
            var formate = RequestFormat.JsonFormaterString();
            try
            {
                if (string.IsNullOrEmpty(aModel.Name))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "Name is Empty" }, formate);
                }

                //if (aModel.PdGroupId == 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK,
                //        new Confirmation { Output = "error", Msg = "PdGroupe Id Cannot be Zero" }, formate);
                //}

                else
                {
                    if (_gt.FncSeekRecordNew("tbl_PHAR_COMPANY", "Id=" + aModel.Id + "") == false)
                    {

                        string msg = await _gt.Save(aModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation { Output = "success", Msg = msg }, formate);

                    }
                    else
                    {
                        string msg = await _gt.Update(aModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation { Output = "success", Msg = msg }, formate);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK
                    , new Confirmation { Output = "error", Msg = ex.ToString() }, formate);
            }
        }




        public HttpResponseMessage GetGetCompanyProductListList(int param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetCompanyProductList(param), formatter);
        }


        public HttpResponseMessage Delete(int param)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.Delete(param), formatter);
        }






    }
}
