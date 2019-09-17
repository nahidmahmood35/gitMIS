using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;
using Microsoft.Ajax.Utilities;

namespace HospitalManagementApp_Api.Gateway
{
    public class PharRequisitionDisburseGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Update(List<PharStockRequisitionModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
            //    string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(12, _trans);
            //    string trNo1 = GetTrNo("RefNo", "tbl_PHAR_STOCKLEDGER", _trans);
                const string query = @"UPDATE  tbl_PHAR_STOCK_REQUISITION SET
                DisburseBy=@DisburseBy,DisburseDate=@DisburseDate,DisburseTime=@DisburseTime,UserName=@UserName,UserDtls=@UserDtls,DisburseNote=@DisburseNote,Status=@Status
                WHERE ReqNo=@ReqNo ";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", aModel.ElementAt(0).ReqNo);
                cmd.Parameters.AddWithValue("@DisburseBy", aModel.ElementAt(0).DisburseBy);
                cmd.Parameters.AddWithValue("@DisburseDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DisburseTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@DisburseNote", aModel.ElementAt(0).DisburseNote);
                cmd.Parameters.AddWithValue("@Status", 4);
                cmd.ExecuteNonQuery();
               var branchId= GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans);

                for (int i = 0; i < aModel.Count; i++)
                {
                    const string lcQuery = @"UPDATE  tbl_PHAR_STOCK_REQUISITION_DETAIL SET  DisburseQty=@DisburseQty WHERE MasterId=@MasterId AND ProductId=@ProductId";
                    var cmd2 = new SqlCommand(lcQuery, Con, _trans);
                    cmd2.Parameters.Clear();
                    cmd2.Parameters.AddWithValue("@MasterId", aModel.ElementAt(i).MasterId);
                    cmd2.Parameters.AddWithValue("@ProductId", aModel.ElementAt(i).ProductId);
                    cmd2.Parameters.AddWithValue("@DisburseQty", aModel.ElementAt(i).DisburseQty);
                    cmd2.ExecuteNonQuery();
                }

                foreach (var stockReq in aModel)
                {
                    var listItem = new List<PharStockRequisitionModel>();
                    int subSubPnoId = 65;
                    string queryItem =
                        "select a.PurchasePrice,a.SalesPrice,a.SubSubPnoId,a.CompanyId,a.ItemId,a.ExpireDate,a.BarCodeId,(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) as BalQty " +
                        "from tbl_PHAR_STOCKLEDGER as a where a.SubSubPnoId=" + subSubPnoId + " and a.BranchId=" + branchId + " and a.ItemId=" + stockReq.ProductId + " order by InvoiceDate";
                    cmd = new SqlCommand(queryItem, Con, _trans);
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listItem.Add(new PharStockRequisitionModel()
                        {
                            BarCodeId = rdr["BarCodeId"].ToString(),
                            BalQty = Convert.ToDouble(rdr["BalQty"].ToString()),
                            ProductId = Convert.ToInt32(rdr["ItemId"]),
                            CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                            PurchasePrice = Convert.ToDouble(rdr["PurchasePrice"]),
                            SalesPrice = Convert.ToDouble(rdr["SalesPrice"]),
                            ExpireDate = Convert.ToDateTime(rdr["ExpireDate"]),
                        });
                    }
                    rdr.Close();

                    foreach (var item in listItem)
                    {
                        if (stockReq.DisburseQty != 0)
                        {
                            if (stockReq.DisburseQty >= item.BalQty)
                            {
                                //  tbl_PHAR_STOCKLEDGER Pharmacy OUt
                                var query_sl1 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,OutQty,PurchasePrice,SalesPrice,ExpireDate,Status,SubSubPnoId,UserName,BranchId) VALUES (@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@OutQty,@PurchasePrice,@SalesPrice,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId)";
                                cmd = new SqlCommand(query_sl1, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@RefNo", stockReq.ReqNo);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", stockReq.ReqNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ProductId);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.Parameters.AddWithValue("@OutQty", item.BalQty);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@SalesPrice", item.SalesPrice);
                                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@Status", "Disb Out Main");
                                cmd.Parameters.AddWithValue("@SubSubPnoId", "65");
                                cmd.Parameters.AddWithValue("@UserName", stockReq.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(stockReq.UserName, _trans));
                                cmd.ExecuteNonQuery();

                                //  tbl_PHAR_STOCKLEDGER SubStore IN
                                var query_In = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,InQty,PurchasePrice,SalesPrice,ExpireDate,Status,SubSubPnoId,UserName,BranchId) VALUES (@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@InQty,@PurchasePrice,@SalesPrice,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId)";
                             var    cmd4 = new SqlCommand(query_In, Con, _trans);
                             cmd4.Parameters.Clear();
                             cmd4.Parameters.AddWithValue("@RefNo", stockReq.ReqNo);
                             cmd4.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                             cmd4.Parameters.AddWithValue("@InvoiceNo", stockReq.ReqNo);
                             cmd4.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                             cmd4.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                             cmd4.Parameters.AddWithValue("@ItemId", item.ProductId);
                             cmd4.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                             cmd4.Parameters.AddWithValue("@InQty", item.BalQty);
                             cmd4.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                             cmd4.Parameters.AddWithValue("@SalesPrice", item.SalesPrice);
                             cmd4.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                             cmd4.Parameters.AddWithValue("@Status", "Disb In");
                             cmd4.Parameters.AddWithValue("@SubSubPnoId",stockReq.DeptId);
                             cmd4.Parameters.AddWithValue("@UserName", stockReq.UserName);
                             cmd4.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(stockReq.UserName, _trans));
                             cmd4.ExecuteNonQuery();
                             stockReq.DisburseQty -= item.BalQty;


                            }
                            else
                            {
                                var query_sl2 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,OutQty,PurchasePrice,SalesPrice,ExpireDate,Status,SubSubPnoId,UserName,BranchId) VALUES(@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@OutQty,@PurchasePrice,@SalesPrice,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId)";
                                cmd = new SqlCommand(query_sl2, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@RefNo", stockReq.ReqNo);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", stockReq.ReqNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ProductId);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.Parameters.AddWithValue("@OutQty", stockReq.DisburseQty);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@SalesPrice", item.SalesPrice);
                                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@Status", "Disb Out Main");
                                cmd.Parameters.AddWithValue("@SubSubPnoId", "65");
                                cmd.Parameters.AddWithValue("@UserName", stockReq.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(stockReq.UserName, _trans));
                                cmd.ExecuteNonQuery();

                                // 

                                var query_In2 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,InQty,PurchasePrice,SalesPrice,ExpireDate,Status,SubSubPnoId,UserName,BranchId) VALUES(@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@InQty,@PurchasePrice,@SalesPrice,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId)";
                               var  cmd5 = new SqlCommand(query_In2, Con, _trans);
                                cmd5.Parameters.Clear();
                                cmd5.Parameters.AddWithValue("@RefNo", stockReq.ReqNo);
                                cmd5.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd5.Parameters.AddWithValue("@InvoiceNo", stockReq.ReqNo);
                                cmd5.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd5.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd5.Parameters.AddWithValue("@ItemId", item.ProductId);
                                cmd5.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd5.Parameters.AddWithValue("@InQty", stockReq.DisburseQty);
                                cmd5.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd5.Parameters.AddWithValue("@SalesPrice", item.SalesPrice);
                                cmd5.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd5.Parameters.AddWithValue("@Status", "Disb In");
                                cmd5.Parameters.AddWithValue("@SubSubPnoId",stockReq.DeptId);
                                cmd5.Parameters.AddWithValue("@UserName", stockReq.UserName);
                                cmd5.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(stockReq.UserName, _trans));
                                cmd5.ExecuteNonQuery();
                                stockReq.DisburseQty -= stockReq.DisburseQty;
                            }
                        }

                        else
                        {
                            break;
                        }
                    }
                  

                }
                _trans.Commit();
                Con.Close();
                return Task.FromResult("Save Successful");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }

        }





















    }
}