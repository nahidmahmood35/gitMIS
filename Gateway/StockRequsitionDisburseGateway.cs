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
namespace HospitalManagementApp_Api.Gateway
{
    public class StockRequsitionDisburseGateway : DbConnection
    {
        public List<InvStockRequisitionModel> GetInventoryRequisitionList(string userName)
        {
            try
            {

                int branchId = 0;
                int status = 1;
                branchId = GetBranchIdByuserName(userName);
                string condition = "";
                var lists = new List<InvStockRequisitionModel>();
                string query = "";
                //if (searchString != "0") { condition = " AND (ReqNo) LIKE '%' + '" + searchString + "' + '%' "; }

                //                query = @"SELECT a.Id,a.ReqNo,a.ReqDate,a.ReqBy,a.ReqNote,a.ReqTime,b.SubsubPNo
                //	                    FROM tbl_INVSTOCK_REQUISITION as a
                //	                    LEFT JOIN project as b on a.DeptId=b.IdNo WHERE a.BranchId=" + branchId + " AND a.Status=" + status + " ";
                query = @"  SELECT a.Id,a.ReqNo,a.ReqDate,a.ReqBy,a.ReqNote,a.ReqTime,b.SubsubPNo,case when a.Status =3 then 'New' else 'Old' end as Status
	                    FROM tbl_INVSTOCK_REQUISITION as a
	                    LEFT JOIN project as b on a.DeptId=b.IdNo WHERE a.BranchId=" + branchId + " AND a.Status = 3 ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvStockRequisitionModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"]),
                        ReqBy = rdr["ReqBy"].ToString(),
                        ReqNote = rdr["ReqNote"].ToString(),
                        DeptName = rdr["SubsubPNo"].ToString(),
                        Status = rdr["Status"].ToString(),

                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<InvStockRequisitionModel>();
            }
        }

        private SqlTransaction _trans;
        public Task<string> Update(List<InvStockRequisitionModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                const string query = @"UPDATE  tbl_INVSTOCK_REQUISITION SET
                                DisburseBy=@DisburseBy,DisburseDate=@DisburseDate,DisburseTime=@DisburseTime,UserName=@UserName,UserDtls=@UserDtls,DisburseNote=@DisburseNote,Status=@Status
                                WHERE ReqNo=@ReqNo ";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", aModel.ElementAt(0).ReqNo);
                cmd.Parameters.AddWithValue("@DisburseBy", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@DisburseDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DisburseTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                //  cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@UserDtls", aModel.ElementAt(0).UserDtls);
                cmd.Parameters.AddWithValue("@DisburseNote", aModel.ElementAt(0).DisburseNote);
                cmd.Parameters.AddWithValue("@Status", 4);
                cmd.ExecuteNonQuery();


                for (int i = 0; i < aModel.Count; i++)
                {

                    const string lcQuery = @"UPDATE  tbl_INVSTOCK_REQUISITION_DETAIL SET  DisburseQty=@DisburseQty WHERE MasterId=@MasterId AND ProductId=@ProductId";
                    var cmd2 = new SqlCommand(lcQuery, Con, _trans);
                    cmd2.Parameters.Clear();
                    cmd2.Parameters.AddWithValue("@MasterId", aModel.ElementAt(i).MasterId);
                    cmd2.Parameters.AddWithValue("@ProductId", aModel.ElementAt(i).ProductId);
                    cmd2.Parameters.AddWithValue("@DisburseQty", aModel.ElementAt(i).DisburseQty);
                    cmd2.ExecuteNonQuery();
                }
                //------------tbl_INVSTOCK_StockLedger--------------------Start------------
                int PnoId = 84;
                var branchId = GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans);
                string RefNo = GetAutoIncrementNumberFromStoreProcedure(17, _trans);

                foreach (var stockReq in aModel)
                {
                    var listItem = new List<InvStockRequisitionModel>();

                   // string queryItem = @"select a.InvoiceNo,a.PurchasePrice,a.PnoId,a.CompanyId,a.ItemId,(a.InQty-a.OutQty+a.RtnQty) as BalQty from tbl_INVSTOCK_StockLedger as a where a.PnoId=" + PnoId + " and a.BranchId=" + branchId + " and a.ItemId=" + stockReq.ProductId + " order by InvoiceDate";
                    string queryItem = @"select a.InvoiceNo,a.PurchasePrice,a.PnoId,a.CompanyId,a.ItemId,SUM(a.InQty-a.OutQty+a.RtnQty)as BalQty from tbl_INVSTOCK_StockLedger as a where a.PnoId=" + PnoId + " and a.BranchId=" + branchId + " and a.ItemId=" + stockReq.ProductId + "  group by a.InvoiceNo,a.PurchasePrice,a.PnoId,a.CompanyId,a.ItemId,InvoiceDate  HAVING SUM(a.InQty-a.OutQty+a.RtnQty)>0  order by InvoiceDate";
                    cmd = new SqlCommand(queryItem, Con, _trans);
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listItem.Add(new InvStockRequisitionModel()
                        {
                            
                            BalQty = Convert.ToDouble(rdr["BalQty"].ToString()),
                            ProductId = Convert.ToInt32(rdr["ItemId"]),
                            CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                            PurchasePrice = Convert.ToDouble(rdr["PurchasePrice"]),
                            PnoId = Convert.ToInt32(rdr["PnoId"]),
                            InvoiceNo = rdr["InvoiceNo"].ToString(),
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
                                var query_sl1 = @"INSERT INTO tbl_INVSTOCK_StockLedger (RefNo, RefDate, InvoiceNo, InvoiceDate, CompanyId, ItemId, PurchasePrice, OutQty, Status, PnoId, EntryDate, UserName, BranchId, Valid, EntryTime, RecevedBy, TrStatus) VALUES (@RefNo, @RefDate, @InvoiceNo, @InvoiceDate, @CompanyId, @ItemId, @PurchasePrice, @OutQty, @Status, @PnoId, @EntryDate, @UserName, @BranchId, @Valid, @EntryTime, @RecevedBy, @TrStatus)";
                                cmd = new SqlCommand(query_sl1, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@RefNo", RefNo);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", item.InvoiceNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ProductId);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@OutQty", item.BalQty);
                                cmd.Parameters.AddWithValue("@Status", "Out");
                                cmd.Parameters.AddWithValue("@PnoId", "84");
                                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@UserName", stockReq.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(stockReq.UserName, _trans));
                                cmd.Parameters.AddWithValue("@Valid", "1");
                                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                                cmd.Parameters.AddWithValue("@RecevedBy", stockReq.DisburseTo);
                                cmd.Parameters.AddWithValue("@TrStatus", stockReq.ReqNo);
                                cmd.ExecuteNonQuery();

                                //  tbl_PHAR_STOCKLEDGER SubStore IN
                                var query_In = @"INSERT INTO tbl_INVSTOCK_StockLedger (RefNo, RefDate, InvoiceNo, InvoiceDate, CompanyId, ItemId, PurchasePrice, InQty, Status, PnoId, EntryDate, UserName, BranchId, Valid, EntryTime, RecevedBy, TrStatus) VALUES (@RefNo, @RefDate, @InvoiceNo, @InvoiceDate, @CompanyId, @ItemId, @PurchasePrice, @InQty, @Status, @PnoId, @EntryDate, @UserName, @BranchId, @Valid, @EntryTime, @RecevedBy, @TrStatus)";
                                var cmd4 = new SqlCommand(query_In, Con, _trans);
                                cmd4.Parameters.Clear();
                                cmd4.Parameters.AddWithValue("@RefNo", RefNo);
                                cmd4.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd4.Parameters.AddWithValue("@InvoiceNo", item.InvoiceNo);
                                cmd4.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd4.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd4.Parameters.AddWithValue("@ItemId", item.ProductId);
                                cmd4.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd4.Parameters.AddWithValue("@InQty", item.BalQty);
                                cmd4.Parameters.AddWithValue("@Status", "In");
                                cmd4.Parameters.AddWithValue("@PnoId", stockReq.DeptId);
                                cmd4.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd4.Parameters.AddWithValue("@UserName", stockReq.UserName);
                                cmd4.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(stockReq.UserName, _trans));
                                cmd4.Parameters.AddWithValue("@Valid", "1");
                                cmd4.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                                cmd4.Parameters.AddWithValue("@RecevedBy", stockReq.DisburseTo);
                                cmd4.Parameters.AddWithValue("@TrStatus", stockReq.ReqNo);
                                cmd4.ExecuteNonQuery();
                                stockReq.DisburseQty -= item.BalQty;


                            }
                            else
                            {
                                var query_sl1 = @"INSERT INTO tbl_INVSTOCK_StockLedger (RefNo, RefDate, InvoiceNo, InvoiceDate, CompanyId, ItemId, PurchasePrice, OutQty, Status, PnoId, EntryDate, UserName, BranchId, Valid, EntryTime, RecevedBy, TrStatus) VALUES (@RefNo, @RefDate, @InvoiceNo, @InvoiceDate, @CompanyId, @ItemId, @PurchasePrice, @OutQty, @Status, @PnoId, @EntryDate, @UserName, @BranchId, @Valid, @EntryTime, @RecevedBy, @TrStatus)";
                                cmd = new SqlCommand(query_sl1, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@RefNo", RefNo);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", item.InvoiceNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ProductId);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@OutQty", stockReq.DisburseQty);
                                cmd.Parameters.AddWithValue("@Status", "Out");
                                cmd.Parameters.AddWithValue("@PnoId", "84");
                                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@UserName", stockReq.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(stockReq.UserName, _trans));
                                cmd.Parameters.AddWithValue("@Valid", "1");
                                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                                cmd.Parameters.AddWithValue("@RecevedBy", stockReq.DisburseTo);
                                cmd.Parameters.AddWithValue("@TrStatus", stockReq.ReqNo);
                                cmd.ExecuteNonQuery();

                                // 

                                var query_In = @"INSERT INTO tbl_INVSTOCK_StockLedger (RefNo, RefDate, InvoiceNo, InvoiceDate, CompanyId, ItemId, PurchasePrice, InQty, Status, PnoId, EntryDate, UserName, BranchId, Valid, EntryTime, RecevedBy, TrStatus) VALUES (@RefNo, @RefDate, @InvoiceNo, @InvoiceDate, @CompanyId, @ItemId, @PurchasePrice, @InQty, @Status, @PnoId, @EntryDate, @UserName, @BranchId, @Valid, @EntryTime, @RecevedBy, @TrStatus)";
                                var cmd4 = new SqlCommand(query_In, Con, _trans);
                                cmd4.Parameters.Clear();
                                cmd4.Parameters.AddWithValue("@RefNo", RefNo);
                                cmd4.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd4.Parameters.AddWithValue("@InvoiceNo", item.InvoiceNo);
                                cmd4.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd4.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd4.Parameters.AddWithValue("@ItemId", item.ProductId);
                                cmd4.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd4.Parameters.AddWithValue("@InQty", stockReq.DisburseQty);
                                cmd4.Parameters.AddWithValue("@Status", "In");
                                cmd4.Parameters.AddWithValue("@PnoId", stockReq.DeptId);
                                cmd4.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd4.Parameters.AddWithValue("@UserName", stockReq.UserName);
                                cmd4.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(stockReq.UserName, _trans));
                                cmd4.Parameters.AddWithValue("@Valid", "1");
                                cmd4.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                                cmd4.Parameters.AddWithValue("@RecevedBy", stockReq.DisburseTo);
                                cmd4.Parameters.AddWithValue("@TrStatus", stockReq.ReqNo);
                                cmd4.ExecuteNonQuery();
                                stockReq.DisburseQty -= stockReq.DisburseQty;
                            }
                        }

                        else
                        {
                            break;
                        }
                    }
                }
                //------------tbl_INVSTOCK_StockLedger--------------------End------------
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