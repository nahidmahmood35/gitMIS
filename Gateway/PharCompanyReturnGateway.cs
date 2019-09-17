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
	public class PharCompanyReturnGateway : DbConnection
	{
        private SqlTransaction _trans;



        public Task<string> Save(List<PharSalesModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(9, _trans);
                string trNo1 = invoiceNo;// GetTrNo("RefNo", "tbl_PHAR_STOCKLEDGER", _trans);
                string purchaseTrNo=GetAutoIncrementNumberFromStoreProcedure(2, _trans);
                //  tbl_PHAR_COMPANY_RETURN_MASTER
                const string query = @"INSERT INTO tbl_PHAR_COMPANY_RETURN_MASTER (InvoiceNo,InvoiceDate,TotalAmt,AdvanceAmt,Remarks,SubSubPnoId,CorporateId,UserName,BranchId) OUTPUT INSERTED.ID VALUES (@InvoiceNo,@InvoiceDate,@TotalAmt,@AdvanceAmt,@Remarks,@SubSubPnoId,@CorporateId,@UserName,@BranchId)";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
              //  cmd.Parameters.AddWithValue("@MainDeptId", aModel.ElementAt(0).MainDeptId);
              //  cmd.Parameters.AddWithValue("@RegId", aModel.ElementAt(0).RegNo);                    //RegId
              //  cmd.Parameters.AddWithValue("@IndoorId", aModel.ElementAt(0).IndoorId);
                cmd.Parameters.AddWithValue("@TotalAmt", aModel.ElementAt(0).TotalAmt);
                //cmd.Parameters.AddWithValue("@LessPc", aModel.ElementAt(0).LessAmt);
                //cmd.Parameters.AddWithValue("@LessPcOrTk", aModel.ElementAt(0).LessPcOrTk);
                //cmd.Parameters.AddWithValue("@LessAmt", aModel.ElementAt(0).TotalLess);
                cmd.Parameters.AddWithValue("@AdvanceAmt", 0);
                cmd.Parameters.AddWithValue("@Remarks", aModel.ElementAt(0).Remarks);
                cmd.Parameters.AddWithValue("@SubSubPnoId", "65"); //65
                cmd.Parameters.AddWithValue("@CorporateId", aModel.ElementAt(0).CorporateId); //0
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName); 
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                var invMasterId = (int)cmd.ExecuteScalar();

                // tbl_PHAR_PURCHASE_LEDGER
                const string query2 = @"INSERT INTO tbl_PHAR_PURCHASE_LEDGER (TrNo,TrDate,InvoiceNo,InvoiceDate,ComPanyId,PurchaseAmount,LessAmount,PaymentAmount,ReturnAmount,Status,SubSubPnoId,UserName,PaymentStatus,BranchId,EntryDate,EntryTime) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@ComPanyId,@PurchaseAmount,@LessAmount,@PaymentAmount,@ReturnAmount,@Status,@SubSubPnoId,@UserName,@PaymentStatus,@BranchId,@EntryDate,@EntryTime)";
                var cmd2 = new SqlCommand(query2, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@TrNo", purchaseTrNo);
                cmd2.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@InvoiceNo", purchaseTrNo);
                cmd2.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@ComPanyId", aModel.ElementAt(0).CompanyId);
                cmd2.Parameters.AddWithValue("@PurchaseAmount",0);
                cmd2.Parameters.AddWithValue("@LessAmount", 0);
                cmd2.Parameters.AddWithValue("@PaymentAmount", 0);
                cmd2.Parameters.AddWithValue("@ReturnAmount", aModel.ElementAt(0).TotalAmt);
                cmd2.Parameters.AddWithValue("@Status", "Company Return");
                cmd2.Parameters.AddWithValue("@SubSubPnoId", "65");
                cmd2.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd2.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd2.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd2.Parameters.AddWithValue("@PaymentStatus",3);
                cmd2.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));

                cmd2.ExecuteNonQuery();

            //  aModel.ForEach(z => z.InvoiceNo = invoiceNo);
                aModel.ForEach(z => z.TrNo = trNo1);
                aModel.ForEach(z => z.InvoiceDate = DateTime.Now.Date);
                aModel.ForEach(z => z.InvMasterId = invMasterId);
                aModel.ForEach(z => z.EntryTime = DateTime.Now.ToShortTimeString());
                aModel.ForEach(z => z.TrDate = DateTime.Now.Date);
                aModel.ForEach(z => z.Status = "Purchase Rtn");
                aModel.ForEach(z => z.BranchId = Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                //aModel.ForEach(z => z.SubSubPnoId);


                foreach (var salesMain in aModel)  // com id filter
                {
                    var listItem = new List<PharSalesModel>();
                    string queryItem = "select a.PurchasePrice,a.CompanyId,a.ItemId,a.ExpireDate,a.BarCodeId,sum(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) as BalQty from tbl_PHAR_STOCKLEDGER as a where a.BranchId=1 and a.ItemId=" + salesMain.ItemId + " and a.CompanyId=" + salesMain.CompanyId + " group by a.PurchasePrice,a.CompanyId,a.ItemId,a.ExpireDate,a.BarCodeId order by a.ExpireDate";
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
                                var query_sl1 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,PRtnQty,PurchasePrice,SalesPrice,ExpireDate,Status,SubSubPnoId,UserName,BranchId,EntryTime) VALUES (@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@PRtnQty,@PurchasePrice,@SalesPrice,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId,@EntryTime)";
                                cmd = new SqlCommand(query_sl1, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@RefNo", trNo1);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.Parameters.AddWithValue("@PRtnQty", item.TotalQuantity);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice); //0 
                                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@Status", salesMain.Status);
                                cmd.Parameters.AddWithValue("@SubSubPnoId", "65");
                                cmd.Parameters.AddWithValue("@UserName", salesMain.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(salesMain.UserName, _trans));
                                cmd.Parameters.AddWithValue("@EntryTime", salesMain.EntryTime);
                                cmd.ExecuteNonQuery();

                                //  tbl_PHAR_COMPANY_RETURN_DETAIL
                                var query_sd2 = @"INSERT INTO tbl_PHAR_COMPANY_RETURN_DETAIL (InvMasterId,ItemId,CompanyId,ProductQty,SalesPrice,AvgPurchasePrice) VALUES (@InvMasterId,@ItemId,@CompanyId,@ProductQty,@SalesPrice,@AvgPurchasePrice)";
                                cmd = new SqlCommand(query_sd2, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@InvMasterId", salesMain.InvMasterId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ProductQty", item.TotalQuantity);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice); //0
                                cmd.Parameters.AddWithValue("@AvgPurchasePrice", item.PurchasePrice);
                              //cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.ExecuteNonQuery();
                                salesMain.Quantity -= item.TotalQuantity;

                            }
                            else
                            {

                                //  tbl_PHAR_STOCKLEDGER
                                var query_sl1 = @"INSERT INTO tbl_PHAR_STOCKLEDGER (RefNo,RefDate,InvoiceNo,InvoiceDate,CompanyId,ItemId,BarCodeId,PRtnQty,PurchasePrice,SalesPrice,ExpireDate,Status,SubSubPnoId,UserName,BranchId,EntryTime) VALUES (@RefNo,@RefDate,@InvoiceNo,@InvoiceDate,@CompanyId,@ItemId,@BarCodeId,@PRtnQty,@PurchasePrice,@SalesPrice,@ExpireDate,@Status,@SubSubPnoId,@UserName,@BranchId,@EntryTime)";
                                cmd = new SqlCommand(query_sl1, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@RefNo", trNo1);
                                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
                                cmd.Parameters.AddWithValue("@PRtnQty", salesMain.Quantity);
                                cmd.Parameters.AddWithValue("@PurchasePrice", item.PurchasePrice);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice); //0
                                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@Status", salesMain.Status);
                                cmd.Parameters.AddWithValue("@SubSubPnoId", "65");
                                cmd.Parameters.AddWithValue("@UserName", salesMain.UserName);
                                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(salesMain.UserName, _trans));
                                cmd.Parameters.AddWithValue("@EntryTime", salesMain.EntryTime);
                                cmd.ExecuteNonQuery();


                                //  tbl_PHAR_COMPANY_RETURN_DETAIL
                                var query_sd2 = @"INSERT INTO tbl_PHAR_COMPANY_RETURN_DETAIL (InvMasterId,ItemId,CompanyId,ProductQty,SalesPrice,AvgPurchasePrice) VALUES (@InvMasterId,@ItemId,@CompanyId,@ProductQty,@SalesPrice,@AvgPurchasePrice)";
                                cmd = new SqlCommand(query_sd2, Con, _trans);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@InvMasterId", salesMain.InvMasterId);
                                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                                cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                                cmd.Parameters.AddWithValue("@ProductQty", salesMain.Quantity);
                                cmd.Parameters.AddWithValue("@SalesPrice", salesMain.SalesPrice);
                                cmd.Parameters.AddWithValue("@AvgPurchasePrice", item.PurchasePrice);
                                //cmd.Parameters.AddWithValue("@BarCodeId", item.BarCodeId);
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

        public List<PharSalesModel> GetPharCompanyReturnMedicineInfo(int itemId, int companyId)
        {
            try
            {
                var lists = new List<PharSalesModel>();
                string query = "";
                int subSubPnoId = 65;

                if (companyId != 0 )
                {
                    query = @"  SELECT SUM(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) AS BalQty,a.CompanyId,a.SubSubPnoId,a.ItemId,b.Name,b.Tp
                             FROM   tbl_PHAR_STOCKLEDGER AS a LEFT JOIN tbl_PHAR_PRODUCT AS b ON a.ItemId=b.Id
                             GROUP BY a.CompanyId,a.ItemId,b.Name,b.Tp,a.SubSubPnoId 
                             HAVING a.CompanyId=" + companyId + " AND a.ItemId=" + itemId + " AND SubSubPnoId=" + subSubPnoId + " ";

                }
                else
                {
                    query = @" select Sum(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) as BalQty,a.CompanyId,a.ItemId,b.Name,b.Tp
                                 from tbl_PHAR_STOCKLEDGER as a left join tbl_PHAR_PRODUCT as b on a.ItemId=b.Id
                                 group by a.CompanyId,a.ItemId,b.Name,b.Tp ";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharSalesModel()
                    {

                        BalQty = Convert.ToDouble(rdr["BalQty"]),
                        CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        Name = rdr["Name"].ToString(),
                        Tp = Convert.ToDouble(rdr["Tp"]),

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

		public List<PharSalesModel> GetCompanyProductReturnList(int companyId,string name)
		{
			try
			{
				var lists = new List<PharSalesModel>();
				string query = "";

                if (companyId != 0)
				{
                    query = @"  SELECT SUM(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) AS BalQty,a.CompanyId,a.ItemId,b.Name,b.Tp
                             FROM   tbl_PHAR_STOCKLEDGER AS a LEFT JOIN tbl_PHAR_PRODUCT AS b ON a.ItemId=b.Id
                             GROUP BY a.CompanyId,a.ItemId,b.Name,b.Tp 
                             HAVING a.CompanyId=" + companyId + " AND b.Name LIKE '%' + '" + name + "' + '%' ";
                                                                           
				}
				else
				{
                    query = @" select Sum(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) as BalQty,a.CompanyId,a.ItemId,b.Name,b.Tp
                                 from tbl_PHAR_STOCKLEDGER as a left join tbl_PHAR_PRODUCT as b on a.ItemId=b.Id
                                 group by a.CompanyId,a.ItemId,b.Name,b.Tp ";
				}
				Con.Open();
				var cmd = new SqlCommand(query, Con);
				cmd.Parameters.Clear();
				var rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					lists.Add(new PharSalesModel()
					{
						
						BalQty = Convert.ToDouble(rdr["BalQty"]),
                        CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
						Name = rdr["Name"].ToString(),
                        Tp = Convert.ToDouble(rdr["Tp"]),

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


	    public List<PharSalesModel> GetMedicineListByComapanyId(int companyId,string userName)
	    {

            try
            {

                int branchId = 0;
                branchId = GetBranchIdByuserName(userName);
                var lists = new List<PharSalesModel>();
                string query = "";

                if (companyId != 0)
                {
                    query = @"SELECT SUM(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) AS BalQty,a.CompanyId,a.BranchId,a.ItemId,b.Name,b.Tp,b.Code,b.ProductUnit,b.ReminderStock
                             FROM   tbl_PHAR_STOCKLEDGER AS a LEFT JOIN tbl_PHAR_PRODUCT AS b ON a.ItemId=b.Id
                             GROUP BY a.CompanyId,a.ItemId,b.Name,b.Tp,b.Code,b.ProductUnit,b.ReminderStock,a.BranchId
                             HAVING a.CompanyId=" + companyId + " AND a.BranchId="+branchId+"   ";

                }
                else
                {
                    query = @"SELECT SUM(a.InQty-a.OutQty+a.RtnQty+a.BonusQty-a.PRtnQty) AS BalQty,a.CompanyId,a.BranchId,a.ItemId,b.Name,b.Tp,b.Code,b.ProductUnit,b.ReminderStock
                             FROM   tbl_PHAR_STOCKLEDGER AS a LEFT JOIN tbl_PHAR_PRODUCT AS b ON a.ItemId=b.Id
                             GROUP BY a.CompanyId,a.ItemId,b.Name,b.Tp,b.Code,b.ProductUnit,b.ReminderStock,a.BranchId ";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharSalesModel()
                    {

                        BalQty = Convert.ToDouble(rdr["BalQty"]),
                        CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        Name = rdr["Name"].ToString(),
                        Tp = Convert.ToDouble(rdr["Tp"]),
                        Code = rdr["Code"].ToString(),
                        ProductUnit=rdr["ProductUnit"].ToString(),
                        ReminderStock = Convert.ToInt32(rdr["ReminderStock"]),

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