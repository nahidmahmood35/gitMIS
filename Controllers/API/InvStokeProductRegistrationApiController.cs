using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;
using Microsoft.SqlServer.Server;

namespace HospitalManagementApp_Api.Controllers.API
{
    public class InvStokeProductRegistrationApiController : ApiController
    {
        readonly InvStokeProductRegistrationGatway _gtInvStokeProductRegistrationGatway = new InvStokeProductRegistrationGatway();

        public async Task<HttpResponseMessage> Post([FromBody] InvStockProductRegistrationModel mInvStockProductRegistrationModel)
        {
            var formatter = RequestFormat.JsonFormaterString();
            string msg = "";
            try
            {
                if (mInvStockProductRegistrationModel.Id == 0)
                {
                    if (mInvStockProductRegistrationModel.ProductName == "")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation {Output = "error", Msg = "Product Name  is Empty"}, formatter);
                    }
                    else if (mInvStockProductRegistrationModel.ProductCategory == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation {Output = "error", Msg = "Please Select Product Category"}, formatter);
                    }
                    else if (mInvStockProductRegistrationModel.Unit == "")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation {Output = "error", Msg = " Unit is Empty"}, formatter);
                    }
                    else
                    {
                        msg = await _gtInvStokeProductRegistrationGatway.Save(mInvStockProductRegistrationModel);
                        return Request.CreateResponse(HttpStatusCode.OK,
                            new Confirmation {Output = "success", Msg = msg}, formatter);
                    }

                }
                else
                {
                    msg = await _gtInvStokeProductRegistrationGatway.Update(mInvStockProductRegistrationModel);
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new Confirmation { Output = "Update success", Msg = msg }, formatter);
                }

                
                

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Confirmation { Output = "error", Msg = ex.ToString() }, formatter);
            }
        }


        public HttpResponseMessage GetProductCategoryList()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStokeProductRegistrationGatway.GetIdCasCadeDropDown("Select Id, Name from tbl_INVSTOCK_ProductCategory Order By Id"), formatter);
        }
        public HttpResponseMessage GetDepreciationMethod()
        {
            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStokeProductRegistrationGatway.GetIdCasCadeDropDown("Select Id, Name from tbl_Depreciation_Method_List Order By Id"), formatter);
        }

        public HttpResponseMessage GetSaveProductCategory(string name)
        {
            var formatter = RequestFormat.JsonFormaterString();
            if (_gtInvStokeProductRegistrationGatway.FncSeekRecordNew("tbl_INVSTOCK_ProductCategory", "Name='" + name + "'"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "This Name Already Exist", formatter);

            }
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStokeProductRegistrationGatway.SaveWithReturnId("tbl_INVSTOCK_ProductCategory", name), formatter);


        }

        public HttpResponseMessage GetProductList()
        {

            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStokeProductRegistrationGatway.GetProductList(), formatter);

        }

        public HttpResponseMessage GetProductById(int Id)
        {

            var formatter = RequestFormat.JsonFormaterString();
            return Request.CreateResponse(HttpStatusCode.OK, _gtInvStokeProductRegistrationGatway.GetProductById(Id), formatter);

        }
    }
}