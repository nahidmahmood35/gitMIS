using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class PharStockInDetailsGateway : DbConnection
    {



        ///////////////////////////SAVE

        public string Save(PharStockInDetailsModel aModel )
        {
            try
            {
                string msg = "";
                const string query = @"INSERT INTO PharStockInDetails (InvoiceNo,InvoiceDate,CompanyId,ProductName,ProductUnit,ProductQty,InvPrice,Valid,Row_Id,PNo,SlipNo,ExpireDate,TotalTP,VatAmt,DiscountAmt,VoucherType,StoreName,RefNo,RefDate,SalesPrice,BonusQty) 
										VALUES (@InvoiceNo,@InvoiceDate,@CompanyId,@ProductName,@ProductUnit,@ProductQty,@InvPrice,@Valid,@Row_Id,@PNo,@SlipNo,@ExpireDate,@TotalTP,@VatAmt,@DiscountAmt,@VoucherType,@StoreName,@RefNo,@RefDate,@SalesPrice,@BonusQty)";
                Con.Open();
                var cmd = new SqlCommand(query,Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", aModel.InvoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", aModel.InvoiceDate);
                cmd.Parameters.AddWithValue("@CompanyId", aModel.CompanyId);
                cmd.Parameters.AddWithValue("@ProductName", aModel.ProductName);
                cmd.Parameters.AddWithValue("@ProductUnit", aModel.ProductUnit);
                cmd.Parameters.AddWithValue("@ProductQty", aModel.ProductQty);
                cmd.Parameters.AddWithValue("@InvPrice", aModel.InvPrice);
                cmd.Parameters.AddWithValue("@Valid", aModel.Valid);
                cmd.Parameters.AddWithValue("@Row_Id", aModel.Row_Id);
                cmd.Parameters.AddWithValue("@PNo", aModel.PNo);
                cmd.Parameters.AddWithValue("@SlipNo", aModel.SlipNo);
                cmd.Parameters.AddWithValue("@ExpireDate", aModel.ExpireDate);
                cmd.Parameters.AddWithValue("@TotalTP", aModel.TotalTP);
                cmd.Parameters.AddWithValue("@VatAmt", aModel.VatAmt);
                cmd.Parameters.AddWithValue("@DiscountAmt", aModel.DiscountAmt);
                cmd.Parameters.AddWithValue("@VoucherType", aModel.VoucherType);
                cmd.Parameters.AddWithValue("@StoreName", aModel.StoreName);
                cmd.Parameters.AddWithValue("@RefNo", aModel.RefNo);
                cmd.Parameters.AddWithValue("@RefDate", aModel.RefDate);
                cmd.Parameters.AddWithValue("@SalesPrice", aModel.SalesPrice);
                cmd.Parameters.AddWithValue("@BonusQty", aModel.BonusQty);


                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Saved Success" : "Saved Failed";
                Con.Close();
                return msg;


            }
            catch (Exception exception) 
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                return exception.ToString();
            }


        }


        ///////////////////////////UPDATE

        public string Update(PharStockInDetailsModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"UPDATE PharStockInDetails 
								SET InvoiceNo=@InvoiceNo,InvoiceDate=@InvoiceDate,CompanyId=@CompanyId,ProductName=@ProductName,ProductUnit=@ProductUnit,ProductQty=@ProductQty,InvPrice=@InvPrice,Valid=@Valid,Row_Id=@Row_Id,PNo=@PNo,SlipNo=@SlipNo,ExpireDate=@ExpireDate,TotalTP=@TotalTP,VatAmt=@VatAmt,DiscountAmt=@DiscountAmt,VoucherType=@VoucherType,StoreName=@StoreName,RefNo=@RefNo,RefDate=@RefDate,SalesPrice=@SalesPrice,BonusQty=@BonusQty
								WHERE Row_Id=@Row_Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", aModel.InvoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", aModel.InvoiceDate);
                cmd.Parameters.AddWithValue("@CompanyId", aModel.CompanyId);
                cmd.Parameters.AddWithValue("@ProductName", aModel.ProductName);
                cmd.Parameters.AddWithValue("@ProductUnit", aModel.ProductUnit);
                cmd.Parameters.AddWithValue("@ProductQty", aModel.ProductQty);
                cmd.Parameters.AddWithValue("@InvPrice", aModel.InvPrice);
                cmd.Parameters.AddWithValue("@Valid", aModel.Valid);
                cmd.Parameters.AddWithValue("@Row_Id", aModel.Row_Id);
                cmd.Parameters.AddWithValue("@PNo", aModel.PNo);
                cmd.Parameters.AddWithValue("@SlipNo", aModel.SlipNo);
                cmd.Parameters.AddWithValue("@ExpireDate", aModel.ExpireDate);
                cmd.Parameters.AddWithValue("@TotalTP", aModel.TotalTP);
                cmd.Parameters.AddWithValue("@VatAmt", aModel.VatAmt);
                cmd.Parameters.AddWithValue("@DiscountAmt", aModel.DiscountAmt);
                cmd.Parameters.AddWithValue("@VoucherType", aModel.VoucherType);
                cmd.Parameters.AddWithValue("@StoreName", aModel.StoreName);
                cmd.Parameters.AddWithValue("@RefNo", aModel.RefNo);
                cmd.Parameters.AddWithValue("@RefDate", aModel.RefDate);
                cmd.Parameters.AddWithValue("@SalesPrice", aModel.SalesPrice);
                cmd.Parameters.AddWithValue("@BonusQty", aModel.BonusQty);




                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Update Success" : "Update Failed";
                Con.Close();
                return msg;

            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();

                }
                return exception.ToString();
            }

        }


        ///////////////////////////LIST

        public List<PharStockInDetailsModel> GetPharStockInDetailsList(int param)
        {
            try
            {
                var list = new List<PharStockInDetailsModel>();
                string query = "";
                if (param!=0)
                {
                    query = @"SELECT InvoiceNo,InvoiceDate,CompanyId,ProductName,ProductUnit,ProductQty,InvPrice,Valid,Row_Id,PNo,SlipNo,ExpireDate,TotalTP,VatAmt,DiscountAmt,VoucherType,StoreName,RefNo,RefDate,SalesPrice,BonusQty
							From PharStockInDetails WHERE Id=@Param";
               
                }
                else
                {
                    query = @"SELECT InvoiceNo,InvoiceDate,CompanyId,ProductName,ProductUnit,ProductQty,InvPrice,Valid,Row_Id,PNo,SlipNo,ExpireDate,TotalTP,VatAmt,DiscountAmt,VoucherType,StoreName,RefNo,RefDate,SalesPrice,BonusQty
                            From PharStockInDetails";
                }

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new PharStockInDetailsModel()
                    {

                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        CompanyId = rdr["CompanyId"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        ProductUnit = Convert.ToInt32(rdr["ProductUnit"]),
                        ProductQty = Convert.ToDouble(rdr["ProductQty"]),
                        InvPrice = Convert.ToDouble(rdr["InvPrice"]),
                        Valid = Convert.ToInt32(rdr["Valid"]),
                        Row_Id = Convert.ToInt32(rdr["Row_Id"]),
                        PNo = rdr["PNo"].ToString(),
                        SlipNo = rdr["SlipNo"].ToString(),
                        ExpireDate = Convert.ToDateTime(rdr["ExpireDate"]),
                        TotalTP = Convert.ToDouble(rdr["TotalTP"]),
                        VatAmt = Convert.ToDouble(rdr["VatAmt"]),
                        DiscountAmt = Convert.ToDouble(rdr["DiscountAmt"]),
                        VoucherType = rdr["VoucherType"].ToString(),
                        StoreName = rdr["StoreName"].ToString(),
                        RefNo = rdr["RefNo"].ToString(),
                        RefDate = Convert.ToDateTime(rdr["RefDate"]),
                        SalesPrice = Convert.ToDouble(rdr["SalesPrice"]),
                        BonusQty = Convert.ToDouble(rdr["BonusQty"]),

                    });

                }

                rdr.Close();
                Con.Close();
                return list;



            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<PharStockInDetailsModel>();
            }

        }


        ///////////////////////////DELETE

        public string Delete(string Id)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM PharStockInDetails WHERE PNo=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id",Id);

                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Delete Success" : "Delete Failed";
                Con.Close();
                return msg;
            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                return exception.ToString();
            }


        }




    }
}