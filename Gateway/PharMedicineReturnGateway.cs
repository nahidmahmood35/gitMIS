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
    public class PharMedicineReturnGateway : DbConnection
    {
             private SqlTransaction _trans;


        public Task<string> Save(List<PharSalesModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(11, _trans);
             //   string TrNo = GetAutoIncrementNumberFromStoreProcedure(7, _trans);
          //    string trNo = GetTrNo("TrNo", "tbl_PHAR_SALES_LEDGER", _trans);
                string trNo1 = invoiceNo;// GetTrNo("RefNo", "tbl_PHAR_STOCKLEDGER", _trans);

                //  tbl_PHAR_RETURN_MASTER 
                const string query = @"INSERT INTO tbl_PHAR_RETURN_MASTER (InvoiceNo,InvoiceDate,MainDeptId,RegId,IndoorId,TotalAmt,LessPc,LessPcOrTk,LessAmt,AdvanceAmt,Remarks,SubSubPnoId,CorporateId,UserName,BranchId) OUTPUT INSERTED.ID VALUES (@InvoiceNo,@InvoiceDate,@MainDeptId,@RegId,@IndoorId,@TotalAmt,@LessPc,@LessPcOrTk,@LessAmt,@AdvanceAmt,@Remarks,@SubSubPnoId,@CorporateId,@UserName,@BranchId)";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MainDeptId", aModel.ElementAt(0).MainDeptId);
                cmd.Parameters.AddWithValue("@RegId", aModel.ElementAt(0).RegNo);                    
                cmd.Parameters.AddWithValue("@IndoorId", aModel.ElementAt(0).IndoorId);
                cmd.Parameters.AddWithValue("@TotalAmt", aModel.ElementAt(0).TotalAmt);
                cmd.Parameters.AddWithValue("@LessPc", aModel.ElementAt(0).LessAmt);
                cmd.Parameters.AddWithValue("@LessPcOrTk", aModel.ElementAt(0).LessPcOrTk);
                cmd.Parameters.AddWithValue("@LessAmt", aModel.ElementAt(0).TotalLess);
                cmd.Parameters.AddWithValue("@AdvanceAmt", 0);
                cmd.Parameters.AddWithValue("@Remarks", aModel.ElementAt(0).Remarks);
                cmd.Parameters.AddWithValue("@SubSubPnoId", aModel.ElementAt(0).SubSubPnoId);
                cmd.Parameters.AddWithValue("@CorporateId", aModel.ElementAt(0).CorporateId);
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                var invMasterId = (int)cmd.ExecuteScalar();

                // tbl_PHAR_SALES_LEDGER
                const string query2 = @"INSERT INTO tbl_PHAR_SALES_LEDGER (TrNo,TrDate,InvoiceNo,InvoiceDate,MainDeptId,RegId,InddorId,Sales,Less,Status,Collection,RtnAmt,CashRtn,SubSubPnoId,UserName,BranchId) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@MainDeptId,@RegId,@InddorId,@Sales,@Less,@Status,@Collection,@RtnAmt,@CashRtn,@SubSubPnoId,@UserName,@BranchId)";
                var cmd2 = new SqlCommand(query2, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@TrNo", invoiceNo);
                cmd2.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@InvoiceNo",  aModel.ElementAt(0).InvoiceNo);
                cmd2.Parameters.AddWithValue("@InvoiceDate", aModel.ElementAt(0).InvoiceDate);
                cmd2.Parameters.AddWithValue("@MainDeptId", aModel.ElementAt(0).MainDeptId);
                cmd2.Parameters.AddWithValue("@RegId", aModel.ElementAt(0).RegNo);
                cmd2.Parameters.AddWithValue("@InddorId", 0);
                cmd2.Parameters.AddWithValue("@Sales", 0);
                cmd2.Parameters.AddWithValue("@Less", 0);
                cmd2.Parameters.AddWithValue("@Status", "Rtn");
                cmd2.Parameters.AddWithValue("@Collection", 0);
                cmd2.Parameters.AddWithValue("@CashRtn", aModel.ElementAt(0).CashRtn);
                cmd2.Parameters.AddWithValue("@RtnAmt", aModel.ElementAt(0).RtnAmt);
                cmd2.Parameters.AddWithValue("@SubSubPnoId",aModel.ElementAt(0).SubSubPnoId);
                cmd2.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd2.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd2.ExecuteNonQuery();

              //  aModel.ForEach(z => z.InvoiceNo = invoiceNo);
                aModel.ForEach(z => z.TrNo = trNo1);
                aModel.ForEach(z => z.InvoiceDate = DateTime.Now.Date);
                aModel.ForEach(z => z.InvMasterId = invMasterId);
                aModel.ForEach(z => z.EntryTime = DateTime.Now.ToShortTimeString());
                aModel.ForEach(z => z.TrDate = DateTime.Now.Date);
                aModel.ForEach(z => z.Status = "Rtn");
                aModel.ForEach(z => z.BranchId = Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                //aModel.ForEach(z => z.SubSubPnoId);


                foreach (var salesMain in aModel)
                {
                    var listItem = new List<PharSalesModel>();
                    int branchId = 1;
                    int subSubPno = 65;
                    string queryItem = @"select a.PurchasePrice,a.CompanyId,a.ItemId,a.SubSubPnoId,a.BranchId,a.ExpireDate,a.BarCodeId,a.OutQty,a.InQty,a.RtnQty,a.InvoiceNo,a.SalesPrice 
                        from tbl_PHAR_STOCKLEDGER as a 
                        where a.ItemId="+salesMain.ItemId+"	and a.OutQty>0 and a.InvoiceNo="+salesMain.InvoiceNo+" and a.SubSubPnoId="+subSubPno+" and a.BranchId="+branchId+" ";
                    cmd = new SqlCommand(queryItem, Con, _trans);
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listItem.Add(new PharSalesModel()
                        {
                            BarCodeId = rdr["BarCodeId"].ToString(),
                            ItemId = Convert.ToInt32(rdr["ItemId"]),
                            CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                            PurchasePrice = Convert.ToDouble(rdr["PurchasePrice"]),
                            SalesPrice = Convert.ToDouble(rdr["SalesPrice"]),
                            ExpireDate = Convert.ToDateTime(rdr["ExpireDate"]),
                            OutQty = Convert.ToDouble(rdr["OutQty"]),
                        });
                    }
                    rdr.Close();

                    foreach (var item in listItem)
                    {
                        if (salesMain.Quantity != 0)
                        {
                            if (salesMain.Quantity >= item.OutQty)
                            {
                                //  tbl_PHAR_STOCKLEDGER
                                var query_sl1 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,RtnQty,PurchasePrice,SalesPrice,ExpireDate,Status,SubSubPnoId,UserName,BranchId,EntryTime) VALUES (@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@RtnQty,@PurchasePrice,@SalesPrice,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId,@EntryTime)";
                                cmd = new SqlCommand(query_sl1, Con, _trans);
                                cmd.Parameters.Clear();
                                //  var s = salesMain.ExpireDate.ToString("yyyy-MM-dd");
                                cmd.Parameters.AddWithValue("@RefNo", trNo1);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", salesMain.RtnInvNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.Parameters.AddWithValue("@RtnQty", item.OutQty);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@SalesPrice", item.SalesPrice);
                                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@Status", salesMain.Status);
                                cmd.Parameters.AddWithValue("@SubSubPnoId", "65");
                                cmd.Parameters.AddWithValue("@UserName", salesMain.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(salesMain.UserName, _trans));
                                cmd.Parameters.AddWithValue("@EntryTime", salesMain.EntryTime);
                                cmd.ExecuteNonQuery();

                                //  tbl_PHAR_RETURN_DETAIL
                                var query_sd2 = @"INSERT INTO tbl_PHAR_RETURN_DETAIL (InvMasterId,ItemId,CompanyId,ProductQty,SalesPrice,AvgPurchasePrice,BarCodeId) VALUES (@InvMasterId,@ItemId,@CompanyId,@ProductQty,@SalesPrice,@AvgPurchasePrice,@BarCodeId)";
                                cmd = new SqlCommand(query_sd2, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@InvMasterId", invMasterId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ProductQty", item.OutQty);
                                cmd.Parameters.AddWithValue("@SalesPrice", item.SalesPrice);
                                cmd.Parameters.AddWithValue("@AvgPurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.ExecuteNonQuery();

                                salesMain.Quantity -= item.OutQty;

                            }
                            else
                            {

                                //  tbl_PHAR_STOCKLEDGER
                                var query_sl1 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,RtnQty,PurchasePrice,SalesPrice,ExpireDate,Status,SubSubPnoId,UserName,BranchId,EntryTime) VALUES (@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@RtnQty,@PurchasePrice,@SalesPrice,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId,@EntryTime)";
                                cmd = new SqlCommand(query_sl1, Con, _trans);
                                cmd.Parameters.Clear();
                                //  var s = salesMain.ExpireDate.ToString("yyyy-MM-dd");
                                cmd.Parameters.AddWithValue("@RefNo", trNo1);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", salesMain.RtnInvNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.Parameters.AddWithValue("@RtnQty", salesMain.Quantity);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@SalesPrice", item.SalesPrice);
                                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@Status", salesMain.Status);
                                cmd.Parameters.AddWithValue("@SubSubPnoId", "65");
                                cmd.Parameters.AddWithValue("@UserName", salesMain.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(salesMain.UserName, _trans));
                                cmd.Parameters.AddWithValue("@EntryTime", salesMain.EntryTime);
                                cmd.ExecuteNonQuery();


                                //  tbl_PHAR_RETURN_DETAIL
                                var query_sd2 = @"INSERT INTO tbl_PHAR_RETURN_DETAIL (InvMasterId,ItemId,CompanyId,ProductQty,SalesPrice,AvgPurchasePrice,BarCodeId) VALUES (@InvMasterId,@ItemId,@CompanyId,@ProductQty,@SalesPrice,@AvgPurchasePrice,@BarCodeId)";
                                cmd = new SqlCommand(query_sd2, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@InvMasterId", salesMain.InvMasterId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ProductQty", salesMain.Quantity);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice);
                                cmd.Parameters.AddWithValue("@AvgPurchasePrice", item.PurchasePrice);
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


        public List<PharSalesModel> GetPharProductList(string searchString,string userName)
        {
            try
            {
                string condition = "";
                var lists = new List<PharSalesModel>();

                int branchId = 0;
                int subSubPno = 65;
                branchId  = GetBranchIdByuserName(userName);
                string query = "";
                if (searchString != "0") { condition = " HAVING  a.SubSubPnoId="+subSubPno+" AND a.BranchId="+branchId+" AND a.InvoiceNo="+searchString+" "; }

                query = @" SELECT b.Id as InvMasterId,a.ItemLessTk,a.ItemId,d.Name,a.InvoiceNo,a.SubSubPnoId,a.BranchId,a.InvoiceDate,b.MainDeptId,b.LessAmt,b.LessPc,b.LessPcOrTk,b.TotalAmt,b.AdvanceAmt,SUM(b.TotalAmt-b.AdvanceAmt-b.LessAmt) as DueAmt,SUM(a.OutQty-a.RtnQty) AS BalQty,(SELECT Top 1 SalesPrice FROM tbl_PHAR_SALES_DETAIL WHERE ItemId=a.ItemId AND InvMasterId=b.Id)as SlPrice
                            FROM tbl_PHAR_STOCKLEDGER a LEFT JOIN tbl_PHAR_SALES_MASTER b ON a.InvoiceNo=b.InvoiceNo AND a.InvoiceDate=b.InvoiceDate
                            LEFT JOIN  tbl_PHAR_PRODUCT as d ON d.Id=a.ItemId
                            WHERE  Status IN ('OUT','RTN')
                            Group by a.ItemId,a.InvoiceNo,a.ItemLessTk,a.InvoiceDate,a.SubSubPnoId,a.BranchId,b.Id,d.Name,b.MainDeptId,b.LessAmt,b.LessPc,b.LessPcOrTk,b.TotalAmt,b.AdvanceAmt  " + condition + " ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharSalesModel()
                    {
                       InvMasterId = Convert.ToInt32(rdr["InvMasterId"]),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        Name = rdr["Name"].ToString(),
                      //  CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                     //   Quantity = Convert.ToDouble(rdr["ProductQty"]),
                        SalesPrice = Convert.ToDouble(rdr["SlPrice"]),
                      //  AvgPurchasePrice = Convert.ToDouble(rdr["AvgPurchasePrice"]),
                      //  Id = Convert.ToInt32(rdr["Id"]),
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                      //  MainDeptId = Convert.ToInt32(rdr["MainDeptId"]),
                    //    RegId=Convert.ToInt32(rdr["RegId"]),
                    //    IndoorId = Convert.ToInt32(rdr["IndoorId"]),
                        TotalAmt = Convert.ToDouble(rdr["TotalAmt"]),
                        LessPc=Convert.ToDouble(rdr["LessPc"]),
                        LessPcOrTk = rdr["LessPcOrTk"].ToString(),
                        LessAmt = Convert.ToDouble(rdr["LessAmt"]),
                       ItemLessTk = Convert.ToDouble(rdr["ItemLessTk"]),
                      //  BarCodeId = rdr["BarCodeId"].ToString(),
                        BalQty = Convert.ToDouble(rdr["BalQty"]),
                       AdvanceAmt = Convert.ToDouble(rdr["AdvanceAmt"]),
                       DueAmt = Convert.ToDouble(rdr["DueAmt"]),
                       //Remarks = rdr["CompanyId"].ToString(),
                        // IsTransfer=
                      //  SubSubPnoId = Convert.ToInt32(rdr["SubSubPnoId"]),
                      //  CorporateId = Convert.ToInt32(rdr["CorporateId"]),
                      //  BranchId = Convert.ToInt32(rdr["BranchId"]),
                        //Id
                       
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

                query = @"SELECT Id,InvoiceNo,InvoiceDate,CustomerName,TotalAmt FROM tbl_PHAR_RETURN_MASTER  WHERE  BranchId=" + branchId + "  " + condition + " ";
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