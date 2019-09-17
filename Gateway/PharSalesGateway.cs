using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class PharSalesGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(List<PharSalesModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(4, _trans);
                string trNo = GetTrNo("TrNo", "tbl_PHAR_SALES_LEDGER", _trans);
                string trNo1 = GetTrNo("RefNo", "tbl_PHAR_STOCKLEDGER", _trans);
                //  tbl_PHAR_SALES_MASTER
                const string query = @"INSERT INTO tbl_PHAR_SALES_MASTER (InvoiceNo,InvoiceDate,MainDeptId,RegId,IndoorId,TotalAmt,LessPc,LessPcOrTk,LessAmt,AdvanceAmt,Remarks,SubSubPnoId,CorporateId,UserName,BranchId) OUTPUT INSERTED.ID VALUES (@InvoiceNo,@InvoiceDate,@MainDeptId,@RegId,@IndoorId,@TotalAmt,@LessPc,@LessPcOrTk,@LessAmt,@AdvanceAmt,@Remarks,@SubSubPnoId,@CorporateId,@UserName,@BranchId)";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MainDeptId", aModel.ElementAt(0).MainDeptId);
                cmd.Parameters.AddWithValue("@RegId", aModel.ElementAt(0).RegNo);    //RegId
                cmd.Parameters.AddWithValue("@IndoorId", aModel.ElementAt(0).IndoorId);
                cmd.Parameters.AddWithValue("@TotalAmt", aModel.ElementAt(0).TotalAmt);
                cmd.Parameters.AddWithValue("@LessPc", aModel.ElementAt(0).LessAmt);
                cmd.Parameters.AddWithValue("@LessPcOrTk", aModel.ElementAt(0).LessPcOrTk);
                cmd.Parameters.AddWithValue("@LessAmt", aModel.ElementAt(0).TotalLess);
                cmd.Parameters.AddWithValue("@AdvanceAmt", aModel.ElementAt(0).AdvanceAmt);
                cmd.Parameters.AddWithValue("@Remarks", aModel.ElementAt(0).Remarks);
                cmd.Parameters.AddWithValue("@SubSubPnoId", "65");
                cmd.Parameters.AddWithValue("@CorporateId", aModel.ElementAt(0).CorporateId);
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                var invMasterId = (int)cmd.ExecuteScalar();

                // tbl_PHAR_SALES_LEDGER
                const string query2 = @"INSERT INTO tbl_PHAR_SALES_LEDGER (TrNo,TrDate,InvoiceNo,InvoiceDate,MainDeptId,RegId,InddorId,Sales,Less,Status,Collection,RtnAmt,CashRtn,SubSubPnoId,UserName,BranchId) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@MainDeptId,@RegId,@InddorId,@Sales,@Less,@Status,@Collection,@RtnAmt,@CashRtn,@SubSubPnoId,@UserName,@BranchId)";
                var cmd2 = new SqlCommand(query2, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@TrNo", trNo);
                cmd2.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd2.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@MainDeptId", 0);
                cmd2.Parameters.AddWithValue("@RegId", aModel.ElementAt(0).RegNo);
                cmd2.Parameters.AddWithValue("@InddorId", 0);
                cmd2.Parameters.AddWithValue("@Sales", aModel.ElementAt(0).TotalAmt);
                cmd2.Parameters.AddWithValue("@Less", aModel.ElementAt(0).TotalLess);
                cmd2.Parameters.AddWithValue("@Status", "Sales");
                cmd2.Parameters.AddWithValue("@Collection", aModel.ElementAt(0).AdvanceAmt);
                cmd2.Parameters.AddWithValue("@RtnAmt", 0);
                cmd2.Parameters.AddWithValue("@CashRtn", 0);
                cmd2.Parameters.AddWithValue("@SubSubPnoId", "65");
                cmd2.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd2.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd2.ExecuteNonQuery();
                var branchId = GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans);

                aModel.ForEach(z => z.InvoiceNo = invoiceNo);
                aModel.ForEach(z => z.TrNo = trNo1);
                aModel.ForEach(z => z.InvoiceDate = DateTime.Now.Date);
                aModel.ForEach(z => z.InvMasterId = invMasterId);
                aModel.ForEach(z => z.EntryTime = DateTime.Now.ToShortTimeString());
                aModel.ForEach(z => z.TrDate = DateTime.Now.Date);
                aModel.ForEach(z => z.Status = "Out");
                aModel.ForEach(z => z.BranchId = Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                aModel.ForEach(z => z.SubSubPnoId = 65);
                DataTable dt = ConvertListDataTable(aModel);

              

                foreach (var salesMain in aModel)
                {
                    var listItem = new List<PharSalesModel>();
                    int SubSubPnoId = 65;
                    string queryItem = "select a.PurchasePrice,a.CompanyId,a.SubSubPnoId,a.BranchId,a.ItemId,a.ExpireDate,a.BarCodeId,(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) as BalQty " +
                                       "from tbl_PHAR_STOCKLEDGER as a where a.BranchId=" + branchId + " and a.SubSubPnoId=" + SubSubPnoId + " and a.ItemId=" + salesMain.ItemId + " order by InvoiceDate";
                    cmd = new SqlCommand(queryItem, Con, _trans);
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listItem.Add(new PharSalesModel()
                        {
                            BarCodeId = rdr["BarCodeId"].ToString(),
                            TotalQuantity = Convert.ToDouble(rdr["BalQty"].ToString()),
                            ItemId = Convert.ToInt32(rdr["ItemId"]),
                            CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                            PurchasePrice = Convert.ToDouble(rdr["PurchasePrice"]),
                            ExpireDate = Convert.ToDateTime(rdr["ExpireDate"]),
                        });
                    }
                    rdr.Close();

                    foreach (var item in listItem)
                    {
                        if (salesMain.Quantity != 0)
                        {
                            if (salesMain.Quantity >= item.TotalQuantity)
                            {
                                //  tbl_PHAR_STOCKLEDGER
                                var query_sl1 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,OutQty,PurchasePrice,SalesPrice,ItemLessTk,ExpireDate,Status,SubSubPnoId,UserName,BranchId,EntryTime) VALUES (@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@OutQty,@PurchasePrice,@SalesPrice,@ItemLessTk,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId,@EntryTime)";
                                cmd = new SqlCommand(query_sl1, Con, _trans);
                                cmd.Parameters.Clear();
                              //  var s = salesMain.ExpireDate.ToString("yyyy-MM-dd");
                                cmd.Parameters.AddWithValue("@RefNo", trNo1);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.Parameters.AddWithValue("@OutQty", item.TotalQuantity);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice);
                                cmd.Parameters.AddWithValue("@ItemLessTk", salesMain.ItemLessTk);
                                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@Status", salesMain.Status);
                                cmd.Parameters.AddWithValue("@SubSubPnoId", salesMain.SubSubPnoId);
                                cmd.Parameters.AddWithValue("@UserName", salesMain.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(salesMain.UserName, _trans));
                                cmd.Parameters.AddWithValue("@EntryTime", salesMain.EntryTime);
                                cmd.ExecuteNonQuery();

                                //  tbl_PHAR_SALES_DETAIL
                                var query_sd1 = @"INSERT INTO tbl_PHAR_SALES_DETAIL (InvMasterId,ItemId,CompanyId,ProductQty,SalesPrice,AvgPurchasePrice,ItemLessTk,BarCodeId) VALUES (@InvMasterId,@ItemId,@CompanyId,@ProductQty,@SalesPrice,@AvgPurchasePrice,@ItemLessTk,@BarCodeId)";
                                cmd = new SqlCommand(query_sd1, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@InvMasterId", salesMain.InvMasterId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ProductQty", item.TotalQuantity);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice);
                                cmd.Parameters.AddWithValue("@AvgPurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@ItemLessTk", salesMain.ItemLessTk);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.ExecuteNonQuery();
                                salesMain.Quantity -= item.TotalQuantity;

                            }
                            else
                            {
                                var query_sl2 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,OutQty,PurchasePrice,SalesPrice,ItemLessTk,ExpireDate,Status,SubSubPnoId,UserName,BranchId,EntryTime) VALUES(@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@OutQty,@PurchasePrice,@SalesPrice,@ItemLessTk,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId,@EntryTime)";
                                cmd = new SqlCommand(query_sl2, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@RefNo", trNo1);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.Parameters.AddWithValue("@OutQty", salesMain.Quantity);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice);
                                cmd.Parameters.AddWithValue("@ItemLessTk", salesMain.ItemLessTk);
                                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@Status", salesMain.Status);
                                cmd.Parameters.AddWithValue("@SubSubPnoId", salesMain.SubSubPnoId);
                                cmd.Parameters.AddWithValue("@UserName", salesMain.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(salesMain.UserName, _trans));
                                cmd.Parameters.AddWithValue("@EntryTime", salesMain.EntryTime);
                                cmd.ExecuteNonQuery();

                                //  tbl_PHAR_SALES_DETAIL
                                var query_sd2 = @"INSERT INTO tbl_PHAR_SALES_DETAIL (InvMasterId,ItemId,CompanyId,ProductQty,SalesPrice,AvgPurchasePrice,ItemLessTk,BarCodeId) VALUES (@InvMasterId,@ItemId,@CompanyId,@ProductQty,@SalesPrice,@AvgPurchasePrice,@ItemLessTk,@BarCodeId)";
                                cmd = new SqlCommand(query_sd2, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@InvMasterId", salesMain.InvMasterId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ProductQty", salesMain.Quantity);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice);
                                cmd.Parameters.AddWithValue("@AvgPurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@ItemLessTk", salesMain.ItemLessTk);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.ExecuteNonQuery();
                                salesMain.Quantity -= salesMain.Quantity;
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
                return Task.FromResult("Save successful");
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

        public int GetPharStock(int searchString, string userName)
        {
            try
            {
                int branchId = 0;
                branchId = GetBranchIdByuserName(userName);
                int stock = 0;
                
                int subsubPno = 65;
                 
                string query = "SELECT dbo.FN_GET_CURRENT_QTY_BY_ID(" + searchString + ","+branchId+","+subsubPno+") AS BalQty";
                
                Con.Open();
             
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    stock = Convert.ToInt32(rdr["BalQty"]);
                }
                rdr.Close();
                Con.Close();
                return stock;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return 0;
            }
        }


        public List<PharSalesModel> GetPharInvoiceList(string searchString,string userName)
        {
            try
            {

                int branchId = 0;
                branchId = GetBranchIdByuserName(userName);
                string condition = "";
                var lists = new List<PharSalesModel>();
                string query = "";
                if (searchString != "0") { condition = " AND  (InvoiceNo) LIKE '%' + '" + searchString + "' + '%' "; }

                query = @"SELECT Id,InvoiceNo,InvoiceDate,CustomerName,TotalAmt FROM tbl_PHAR_SALES_MASTER WHERE  BranchId=" + branchId + "  " + condition + " ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharSalesModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        CustomerName = rdr["CustomerName"].ToString(),
                        TotalPrice = Convert.ToDouble(rdr["TotalAmt"]),
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
                return new List<PharSalesModel>();
            }
        }


        
    }
}