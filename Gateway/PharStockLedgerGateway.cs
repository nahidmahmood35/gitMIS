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
    public class PharStockLedgerGateway : DbConnection     
    {



        ///////////////////////////SAVE

        public string Save(PharStockLedgerModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"INSERT INTO PharStockLedger (RefNo,TranDate,CompanyID,ProductName,InQty,OutQty,RtnQty,Price,Status,UserName,Valid,RowId,PNO,ShortQty,ExcessQty,DamageQty,SampleQty,BonusQty,SlipNo,SaleNo,PrevRefNo,PrevRefDate,VoucherType,StoreName,PatientId,TP,ExpireDate,BatchNo,TransferTo,RefDRCode,Package,PackCode) 
										VALUES (@RefNo,@TranDate,@CompanyID,@ProductName,@InQty,@OutQty,@RtnQty,@Price,@Status,@UserName,@Valid,@RowId,@PNO,@ShortQty,@ExcessQty,@DamageQty,@SampleQty,@BonusQty,@SlipNo,@SaleNo,@PrevRefNo,@PrevRefDate,@VoucherType,@StoreName,@PatientId,@TP,@ExpireDate,@BatchNo,@TransferTo,@RefDRCode,@Package,@PackCode)";
                Con.Open();
                var cmd = new SqlCommand(query,Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@RefNo", aModel.RefNo);
                cmd.Parameters.AddWithValue("@TranDate", aModel.TranDate);
                cmd.Parameters.AddWithValue("@CompanyID", aModel.CompanyID);
                cmd.Parameters.AddWithValue("@ProductName", aModel.ProductName);
                cmd.Parameters.AddWithValue("@InQty", aModel.InQty);
                cmd.Parameters.AddWithValue("@OutQty", aModel.OutQty);
                cmd.Parameters.AddWithValue("@RtnQty", aModel.RtnQty);
                cmd.Parameters.AddWithValue("@Price", aModel.Price);
                cmd.Parameters.AddWithValue("@Status", aModel.Status);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@Valid", aModel.Valid);
                cmd.Parameters.AddWithValue("@RowId", aModel.RowId);
                cmd.Parameters.AddWithValue("@PNO", aModel.PNO);
                cmd.Parameters.AddWithValue("@ShortQty", aModel.ShortQty);
                cmd.Parameters.AddWithValue("@ExcessQty", aModel.ExcessQty);
                cmd.Parameters.AddWithValue("@DamageQty", aModel.DamageQty);
                cmd.Parameters.AddWithValue("@SampleQty", aModel.SampleQty);
                cmd.Parameters.AddWithValue("@BonusQty", aModel.BonusQty);
                cmd.Parameters.AddWithValue("@SlipNo", aModel.SlipNo);
                cmd.Parameters.AddWithValue("@SaleNo", aModel.SaleNo);
                cmd.Parameters.AddWithValue("@PrevRefNo", aModel.PrevRefNo);
                cmd.Parameters.AddWithValue("@PrevRefDate", aModel.PrevRefDate);
                cmd.Parameters.AddWithValue("@VoucherType", aModel.VoucherType);
                cmd.Parameters.AddWithValue("@StoreName", aModel.StoreName);
                cmd.Parameters.AddWithValue("@PatientId", aModel.PatientId);
                cmd.Parameters.AddWithValue("@TP", aModel.TP);
                cmd.Parameters.AddWithValue("@ExpireDate", aModel.ExpireDate);
                cmd.Parameters.AddWithValue("@BatchNo", aModel.BatchNo);
                cmd.Parameters.AddWithValue("@TransferTo", aModel.TransferTo);
                cmd.Parameters.AddWithValue("@RefDRCode", aModel.RefDRCode);
                cmd.Parameters.AddWithValue("@Package", aModel.Package);
                cmd.Parameters.AddWithValue("@PackCode", aModel.PackCode);

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

        public string Update(PharStockLedgerModel aModel)
        {

            try
            {
                string msg = "";
                const string query= @"UPDATE PharStockLedger 
								SET RefNo=@RefNo,TranDate=@TranDate,CompanyID=@CompanyID,ProductName=@ProductName,InQty=@InQty,OutQty=@OutQty,RtnQty=@RtnQty,Price=@Price,Status=@Status,UserName=@UserName,Valid=@Valid,RowId=@RowId,PNO=@PNO,ShortQty=@ShortQty,ExcessQty=@ExcessQty,DamageQty=@DamageQty,SampleQty=@SampleQty,BonusQty=@BonusQty,SlipNo=@SlipNo,SaleNo=@SaleNo,PrevRefNo=@PrevRefNo,PrevRefDate=@PrevRefDate,VoucherType=@VoucherType,StoreName=@StoreName,PatientId=@PatientId,TP=@TP,ExpireDate=@ExpireDate,BatchNo=@BatchNo,TransferTo=@TransferTo,RefDRCode=@RefDRCode,Package=@Package,PackCode=@PackCode
								WHERE CompanyID=@CompanyID";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@RefNo", aModel.RefNo);
                cmd.Parameters.AddWithValue("@TranDate", aModel.TranDate);
                cmd.Parameters.AddWithValue("@CompanyID", aModel.CompanyID);
                cmd.Parameters.AddWithValue("@ProductName", aModel.ProductName);
                cmd.Parameters.AddWithValue("@InQty", aModel.InQty);
                cmd.Parameters.AddWithValue("@OutQty", aModel.OutQty);
                cmd.Parameters.AddWithValue("@RtnQty", aModel.RtnQty);
                cmd.Parameters.AddWithValue("@Price", aModel.Price);
                cmd.Parameters.AddWithValue("@Status", aModel.Status);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@Valid", aModel.Valid);
                cmd.Parameters.AddWithValue("@RowId", aModel.RowId);
                cmd.Parameters.AddWithValue("@PNO", aModel.PNO);
                cmd.Parameters.AddWithValue("@ShortQty", aModel.ShortQty);
                cmd.Parameters.AddWithValue("@ExcessQty", aModel.ExcessQty);
                cmd.Parameters.AddWithValue("@DamageQty", aModel.DamageQty);
                cmd.Parameters.AddWithValue("@SampleQty", aModel.SampleQty);
                cmd.Parameters.AddWithValue("@BonusQty", aModel.BonusQty);
                cmd.Parameters.AddWithValue("@SlipNo", aModel.SlipNo);
                cmd.Parameters.AddWithValue("@SaleNo", aModel.SaleNo);
                cmd.Parameters.AddWithValue("@PrevRefNo", aModel.PrevRefNo);
                cmd.Parameters.AddWithValue("@PrevRefDate", aModel.PrevRefDate);
                cmd.Parameters.AddWithValue("@VoucherType", aModel.VoucherType);
                cmd.Parameters.AddWithValue("@StoreName", aModel.StoreName);
                cmd.Parameters.AddWithValue("@PatientId", aModel.PatientId);
                cmd.Parameters.AddWithValue("@TP", aModel.TP);
                cmd.Parameters.AddWithValue("@ExpireDate", aModel.ExpireDate);
                cmd.Parameters.AddWithValue("@BatchNo", aModel.BatchNo);
                cmd.Parameters.AddWithValue("@TransferTo", aModel.TransferTo);
                cmd.Parameters.AddWithValue("@RefDRCode", aModel.RefDRCode);
                cmd.Parameters.AddWithValue("@Package", aModel.Package);
                cmd.Parameters.AddWithValue("@PackCode", aModel.PackCode);
                
                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Update Success" : "Update Failed";
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


        public List<PharStockLedgerModel> GetPharStockLedgerList(int param)
        {

            try
            {
                var list=new List<PharStockLedgerModel>();
                 string query ="";
                if (param!=0)
                {
                    query = @"SELECT RefNo,TranDate,CompanyID,ProductName,InQty,OutQty,RtnQty,Price,Status,UserName,Valid,RowId,PNO,ShortQty,ExcessQty,DamageQty,SampleQty,BonusQty,SlipNo,SaleNo,PrevRefNo,PrevRefDate,VoucherType,StoreName,PatientId,TP,ExpireDate,BatchNo,TransferTo,RefDRCode,Package,PackCode
							From PharStockLedger WHERE Id=@Param"; 
                }
                else
                {
                    query = @"SELECT RefNo,TranDate,CompanyID,ProductName,InQty,OutQty,RtnQty,Price,Status,UserName,Valid,RowId,PNO,ShortQty,ExcessQty,DamageQty,SampleQty,BonusQty,SlipNo,SaleNo,PrevRefNo,PrevRefDate,VoucherType,StoreName,PatientId,TP,ExpireDate,BatchNo,TransferTo,RefDRCode,Package,PackCode
                                From PharStockLedger"; 
                }
                Con.Open();
                var cmd = new SqlCommand(query,Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    
                    list.Add(new PharStockLedgerModel()
                    {
                        RefNo = rdr["RefNo"].ToString(),
                        TranDate = Convert.ToDateTime(rdr["TranDate"]),
                        CompanyID = rdr["CompanyID"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        InQty = Convert.ToDouble(rdr["InQty"]),
                        OutQty = Convert.ToDouble(rdr["OutQty"]),
                        RtnQty = Convert.ToDouble(rdr["RtnQty"]),
                        Price = Convert.ToDouble(rdr["Price"]),
                        Status = rdr["Status"].ToString(),
                        UserName = rdr["UserName"].ToString(),
                        Valid = Convert.ToInt32(rdr["Valid"]),
                        RowId = Convert.ToInt32(rdr["RowId"]),
                        PNO = rdr["PNO"].ToString(),
                        ShortQty = Convert.ToDouble(rdr["ShortQty"]),
                        ExcessQty = Convert.ToDouble(rdr["ExcessQty"]),
                        DamageQty = Convert.ToDouble(rdr["DamageQty"]),
                        SampleQty = Convert.ToDouble(rdr["SampleQty"]),
                        BonusQty = Convert.ToDouble(rdr["BonusQty"]),
                        SlipNo = rdr["SlipNo"].ToString(),
                        SaleNo = rdr["SaleNo"].ToString(),
                        PrevRefNo = rdr["PrevRefNo"].ToString(),
                        PrevRefDate = Convert.ToDateTime(rdr["PrevRefDate"]),
                        VoucherType = rdr["VoucherType"].ToString(),
                        StoreName = rdr["StoreName"].ToString(),
                        PatientId = rdr["PatientId"].ToString(),
                        TP = Convert.ToDouble(rdr["TP"]),
                        ExpireDate = Convert.ToDateTime(rdr["ExpireDate"]),
                        BatchNo = rdr["BatchNo"].ToString(),
                        TransferTo = rdr["TransferTo"].ToString(),
                        RefDRCode = rdr["RefDRCode"].ToString(),
                        Package = Convert.ToInt32(rdr["Package"]),
                        PackCode = rdr["PackCode"].ToString(),   

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
                return new List<PharStockLedgerModel>();
            }


        }

        ///////////////////////////DELETE

        public string Delete(string Id)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM PharStockLedger WHERE CompanyID=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", Id);
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