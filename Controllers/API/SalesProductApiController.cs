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
    public class SalesProductApiController : ApiController
    {
        SalesProductGateway _gt = new SalesProductGateway();

        public async Task<HttpResponseMessage> Save_SalesProduct([FromBody] SalesProductModel aModel)
        {

            var formate = RequestFormat.JsonFormaterString();
            try
            {
                if (string.IsNullOrEmpty(aModel.ProductName))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "error", Msg = "Name is Null" });
                }
                else
                {
                    if (_gt.FncSeekRecordNew("PartyInfo", "Id=" + aModel.IdNo + "") == false)
                    {
                        if (_gt.FncSeekRecordNew("PartyInfo", "Name='" + aModel.ProductName + "'") == true)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK,
                                new Confirmation { Output = "error", Msg = "Name Already Exist" }, formate);
                        }
                        else
                        {
                            string msg = await _gt.Save(aModel);
                            return Request.CreateResponse(HttpStatusCode.OK,
                                new Confirmation { Output = "success", Msg = msg }, formate);
                        }

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
                return Request.CreateResponse(HttpStatusCode.OK,
                    new Confirmation { Output = "error", }, formate);
            }
        }


        public HttpResponseMessage GetProductList(int searchString)
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gt.GetSalesProductList(searchString), formatter);
        }



    }
}
