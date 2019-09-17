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
    public class PharPurchaseGateway : DbConnection
    {
        private SqlTransaction _trans;

        public Task<string> Save(List<PharPurchaseModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(2,_trans);
                string trNo = invoiceNo;//GetTrNo("TrNo", "tbl_PHAR_PURCHASE_LEDGER",_trans);
                string trNo1 = invoiceNo;//GetTrNo("RefNo", "tbl_PHAR_STOCKLEDGER", _trans);  
                int maxId = Convert.ToInt32(GetBarCodeMaxId("tbl_PHAR_STOCKLEDGER",_trans)); 
                
                foreach (var pharPurchaseModel in aModel)
                {
                    pharPurchaseModel.BarCodeId = DateTime.Now.ToString("yyMMdd") + pharPurchaseModel.ItemId.ToString().PadLeft(4,'0') + maxId.ToString().PadLeft(4, '0');
                    maxId++;
                }
                const string query = @"INSERT INTO tbl_PHAR_STOCK_IN_MAIN (InvoiceNo,InvoiceDate,SlipNo,SlipDate,CompanyId,TotalPrice,Remarks,UserName,BranchId,EntryDate,EntryTime,PNo,Spd) VALUES (@InvoiceNo,@InvoiceDate,@SlipNo,@SlipDate,@CompanyId,@TotalPrice,@Remarks,@UserName,@BranchId,@EntryDate,@EntryTime,@PNo,@Spd)";
                var cmd = new SqlCommand(query, Con,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SlipNo", aModel.ElementAt(0).SlipNo);
                cmd.Parameters.AddWithValue("@SlipDate", aModel.ElementAt(0).SlipDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@CompanyId", aModel.ElementAt(0).CompanyId);
                cmd.Parameters.AddWithValue("@TotalPrice", aModel.ElementAt(0).TotalPrice);
                cmd.Parameters.AddWithValue("@Remarks", aModel.ElementAt(0).Remarks);
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName,_trans));
                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@Spd", aModel.ElementAt(0).SpdAmt);
                cmd.Parameters.AddWithValue("@Pno", "Hospital");
                cmd.ExecuteNonQuery();

                const string query2 = @"INSERT INTO tbl_PHAR_PURCHASE_LEDGER (TrNo,TrDate,InvoiceNo,InvoiceDate,ComPanyId,PurchaseAmount,LessAmount,PaymentAmount,Status,SubSubPnoId,UserName,PaymentStatus,BranchId,EntryDate,EntryTime) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@ComPanyId,@PurchaseAmount,@LessAmount,@PaymentAmount,@Status,@SubSubPnoId,@UserName,@PaymentStatus,@BranchId,@EntryDate,@EntryTime)";
                var cmd2 = new SqlCommand(query2, Con,_trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@TrNo", trNo);
                cmd2.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd2.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@ComPanyId", aModel.ElementAt(0).CompanyId);
                cmd2.Parameters.AddWithValue("@PurchaseAmount", aModel.ElementAt(0).TotalPrice);
                cmd2.Parameters.AddWithValue("@LessAmount", 0);
                cmd2.Parameters.AddWithValue("@PaymentAmount",aModel.ElementAt(0).PaymentAmount);
                cmd2.Parameters.AddWithValue("@Status","Stock Receive" );
                cmd2.Parameters.AddWithValue("@SubSubPnoId", "65");
                cmd2.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd2.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd2.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd2.Parameters.AddWithValue("@PaymentStatus", aModel.ElementAt(0).PaymentAmount > 0 ? 2 : 1);
                cmd2.Parameters.AddWithValue("@EntryDate",DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.ExecuteNonQuery();

                aModel.ForEach(z => z.InvoiceNo = invoiceNo);
                aModel.ForEach(z => z.InvoiceDate = DateTime.Now.Date);
                aModel.ForEach(z => z.TrNo = trNo1);
                aModel.ForEach(z => z.TrDate = DateTime.Now.Date);
                aModel.ForEach(z=>z.EntryTime=DateTime.Now.ToShortTimeString());
                aModel.ForEach(z => z.BranchId = Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                aModel.ForEach(z => z.Status = "In");
                aModel.ForEach(z => z.SubSubPnoId = 65);

                DataTable dt = ConvertListDataTable(aModel);
                var objbulk = new SqlBulkCopy(Con,SqlBulkCopyOptions.Default,_trans) { DestinationTableName = "tbl_PHAR_STOCKLEDGER" };
                objbulk.ColumnMappings.Add("TrNo", "RefNo");
                objbulk.ColumnMappings.Add("TrDate", "RefDate");
                objbulk.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                objbulk.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                objbulk.ColumnMappings.Add("CompanyId", "CompanyId");
                objbulk.ColumnMappings.Add("ItemId", "ItemId");
                objbulk.ColumnMappings.Add("BarCodeId", "BarCodeId");
                objbulk.ColumnMappings.Add("Quantity", "InQty");
                objbulk.ColumnMappings.Add("PurchasePrice", "PurchasePrice");
                objbulk.ColumnMappings.Add("SalesPrice", "SalesPrice");
                objbulk.ColumnMappings.Add("ExpireDate", "ExpireDate");
                objbulk.ColumnMappings.Add("Status", "Status");
                objbulk.ColumnMappings.Add("SubSubPnoId", "SubSubPnoId");
                objbulk.ColumnMappings.Add("UserName", "UserName");
                objbulk.ColumnMappings.Add("BranchId", "BranchId");
                objbulk.ColumnMappings.Add("EntryTime", "EntryTime");
                objbulk.WriteToServer(dt);

                objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_PHAR_STOCK_IN_DETAILS" };                
                objbulk.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                objbulk.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                objbulk.ColumnMappings.Add("CompanyId", "CompanyId");
                objbulk.ColumnMappings.Add("ItemId", "ItemId");
                objbulk.ColumnMappings.Add("Quantity", "InQty");
                objbulk.ColumnMappings.Add("BonusQty", "BonusQty");
                objbulk.ColumnMappings.Add("InvPrice", "InvPrice");
                objbulk.ColumnMappings.Add("SubSubPnoId", "SubSubPnoId");
                objbulk.ColumnMappings.Add("ExpireDate", "ExpireDate");
                objbulk.ColumnMappings.Add("VatAmt", "VatAmt");
                objbulk.ColumnMappings.Add("DiscountAmt", "ComisionAmt");
                objbulk.ColumnMappings.Add("SalesPrice", "SalesPrice");
                objbulk.ColumnMappings.Add("UserName", "UserName");
                objbulk.ColumnMappings.Add("BranchId", "BranchId");
                objbulk.ColumnMappings.Add("EntryTime", "EntryTime");
                objbulk.ColumnMappings.Add("TotalTp", "TotalTP");
                objbulk.WriteToServer(dt);

                for (int i = 0; i < aModel.Count; i++)
                {
                    const string query3 = @"UPDATE  tbl_PHAR_PRODUCT SET Tp=@Tp,SalesPrice=@SalesPrice WHERE Id=@Id";
                    var cmd3 = new SqlCommand(query3, Con, _trans);
                    cmd3.Parameters.Clear();
                    cmd3.Parameters.AddWithValue("@Tp", aModel.ElementAt(i).Tp);
                    cmd3.Parameters.AddWithValue("@SalesPrice", aModel.ElementAt(i).SalesPrice);
                    cmd3.Parameters.AddWithValue("@Id", aModel.ElementAt(i).ItemId);
                    cmd3.ExecuteNonQuery();
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




        public List<PharPurchaseModel> GetPharInvoiceList(string searchString,string userName)
        {
            try
            {
                int branchId = 0;
                branchId = GetBranchIdByuserName(userName);
                string condition = "";
                var lists = new List<PharPurchaseModel>();
                string query = "";
                if (searchString != "0") { condition = " AND (InvoiceNo) LIKE '%' + '" + searchString + "' + '%' "; }
                query = @"SELECT Id,InvoiceNo,InvoiceDate,SlipNo,TotalPrice FROM tbl_PHAR_STOCK_IN_MAIN  WHERE  BranchId=" + branchId + "  " + condition + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PharPurchaseModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        SlipNo = rdr["SlipNo"].ToString(),
                        TotalPrice = Convert.ToDouble(rdr["TotalPrice"]),
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
                return new List<PharPurchaseModel>();
            }
        }



    }
}