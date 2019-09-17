using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;
namespace HospitalManagementApp_Api.Gateway
{
    public class InvStockReceiveGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<int> SaveCompanyWithReturnId(string tableName, string name, string address, double opBalance, string contact)
        {
            try
            {
                string query = @"INSERT INTO " + tableName + " (Name,Address,Contact,OpeningBalance,PartyStatus) OUTPUT INSERTED.ID VALUES (@Name,@Address,@Contact,@OpeningBalance,@PartyStatus)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Contact", contact);
                cmd.Parameters.AddWithValue("@OpeningBalance", opBalance);
                cmd.Parameters.AddWithValue("@PartyStatus", "On");
                int rtn = Convert.ToInt32(cmd.ExecuteScalar());
                Con.Close();
                return Task.FromResult(rtn);
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(0);
            }
        }

        public List<InvStockProductRegistrationModel> GetProductList(string searchString)
        {
            try
            {

                string msg = "", condition = "";
                var list = new List<InvStockProductRegistrationModel>();
                string query = "";
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%' + '" + searchString + "' + '%' "; }
                if (searchString != "0") { condition = "WHERE (CONVERT(varchar(10), ID)+ProductName) LIKE '%" + searchString + "%' "; }
                //if (searchString != "0") { condition = "WHERE a.id =" + searchString + " "; }
                query = @"Select Id,ProductName,UnitPrice,Unit,rackNumber,cellNumber from tbl_INVSTOCK_SalesProductList " + condition + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new InvStockProductRegistrationModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ProductName = rdr["ProductName"].ToString(),
                        UnitPrice = Convert.ToDouble(rdr["UnitPrice"]),
                        Unit = rdr["Unit"].ToString(),
                        rackNumber = rdr["rackNumber"].ToString(),
                        cellNumber = rdr["cellNumber"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<InvStockProductRegistrationModel>();
            }
        }


        public Task<string> Save(List<InvStockReceiveModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(13, _trans);
                //string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(13);
                string trNo = invoiceNo;
                string trNo1 = invoiceNo;
                //const string query = @"INSERT INTO tbl_INVSTOCK_InMain (InvoiceNo, InvoiceDate, SlipNo, SlipDate, CompanyId, TotalPrice, Remarks, UserName, BranchId, EntryDate, EntryTime, Valid, PNo) VALUES (@InvoiceNo, @InvoiceDate, @SlipNo, @SlipDate, @CompanyId, @TotalPrice, @Remarks, @UserName, @BranchId, @EntryDate, @EntryTime, @Valid, @PNo)";
                const string query = @"INSERT INTO tbl_INVSTOCK_InMain (InvoiceNo, InvoiceDate, SlipNo, SlipDate, PoNo, CompanyId, TotalPrice, Vat, Less, Remarks, UserName, BranchId, EntryDate, EntryTime, Valid, PNo) VALUES (@InvoiceNo, @InvoiceDate, @SlipNo, @SlipDate, @PoNo, @CompanyId, @TotalPrice, @Vat, @Less, @Remarks, @UserName, @BranchId, @EntryDate, @EntryTime, @Valid, @PNo)";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SlipNo", aModel.ElementAt(0).SlipNo);
                cmd.Parameters.AddWithValue("@SlipDate", aModel.ElementAt(0).SlipDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PoNo", aModel.ElementAt(0).PoNo);
                cmd.Parameters.AddWithValue("@CompanyId", aModel.ElementAt(0).CompanyId);
                cmd.Parameters.AddWithValue("@TotalPrice", (aModel.ElementAt(0).TotalPrice + aModel.ElementAt(0).VatAmount - aModel.ElementAt(0).LessAmount));
                cmd.Parameters.AddWithValue("@Vat", aModel.ElementAt(0).VatAmount);
                cmd.Parameters.AddWithValue("@Less", aModel.ElementAt(0).LessAmount);
                cmd.Parameters.AddWithValue("@Remarks", aModel.ElementAt(0).Remarks);
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@Valid", 1);
                cmd.Parameters.AddWithValue("@PNo", aModel.ElementAt(0).DepartmentId);
                cmd.ExecuteNonQuery();

                //const string query2 = @"INSERT INTO tbl_INVSTOCK_PurchaseLedger (TrNo, TrDate, InvoiceNo, InvoiceDate, CompanyId, PurchaseAmount, LessAmount, PaymentAmount, Status, PnoId, UserName, BranchId, PaymentStatus, EntryDate, Valid, EntryTime) VALUES (@TrNo, @TrDate, @InvoiceNo, @InvoiceDate, @CompanyId, @PurchaseAmount, @LessAmount, @PaymentAmount, @Status, @PnoId, @UserName, @BranchId, @PaymentStatus, @EntryDate, @Valid, @EntryTime)";
                //var cmd2 = new SqlCommand(query2, Con);
                const string query2 = @"INSERT INTO tbl_INVSTOCK_PurchaseLedger (TrNo, TrDate, InvoiceNo, InvoiceDate, CompanyId, PurchaseAmount, LessAmount, PaymentAmount, Status, PnoId, UserName, BranchId, PaymentStatus, EntryDate, Valid, EntryTime) VALUES (@TrNo, @TrDate, @InvoiceNo, @InvoiceDate, @CompanyId, @PurchaseAmount, @LessAmount, @PaymentAmount, @Status, @PnoId, @UserName, @BranchId, @PaymentStatus, @EntryDate, @Valid, @EntryTime)";
                var cmd2 = new SqlCommand(query2, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@TrNo", trNo);
                cmd2.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd2.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@ComPanyId", aModel.ElementAt(0).CompanyId);
                cmd2.Parameters.AddWithValue("@PurchaseAmount", ((aModel.ElementAt(0).TotalPrice) + (aModel.ElementAt(0).VatAmount)));
                cmd2.Parameters.AddWithValue("@LessAmount", aModel.ElementAt(0).LessAmount);
                cmd2.Parameters.AddWithValue("@PaymentAmount", ((aModel.ElementAt(0).TotalPrice) + (aModel.ElementAt(0).VatAmount) - (aModel.ElementAt(0).LessAmount)));
                cmd2.Parameters.AddWithValue("@Status", "Stock Receive");
                cmd2.Parameters.AddWithValue("@PnoId", aModel.ElementAt(0).DepartmentId);
                cmd2.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd2.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd2.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd2.Parameters.AddWithValue("@PaymentStatus", 0);
                cmd2.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@Valid", 1);
                cmd2.ExecuteNonQuery();


                //aModel.ForEach(z => z.InvoiceNo = invoiceNo);
                //aModel.ForEach(z => z.InvoiceDate = DateTime.Now.Date);
                //aModel.ForEach(z => z.TrNo = trNo1);
                //aModel.ForEach(z => z.TrDate = DateTime.Now.Date);
                //aModel.ForEach(z => z.EntryTime = DateTime.Now.ToShortTimeString());
                //aModel.ForEach(z => z.EntryDate = DateTime.Now.ToString("yyyy-MM-dd"));
                //aModel.ForEach(z => z.UserName = aModel.ElementAt(0).UserName);
                //aModel.ForEach(z => z.BranchId = Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                //aModel.ForEach(z => z.Status = "In");
                //aModel.ForEach(z => z.Valid = 1);
                //aModel.ForEach(z => z.PnoId = aModel.ElementAt(0).DepartmentId);

                //DataTable dt = ConvertListDataTable(aModel);
                //var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVSTOCK_StockLedger" };
                //objbulk.ColumnMappings.Add("TrNo", "RefNo");//bame e model er nam danea tabil er colum er nam
                //objbulk.ColumnMappings.Add("TrDate", "RefDate");
                //objbulk.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                //objbulk.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                //objbulk.ColumnMappings.Add("CompanyId", "CompanyId");
                //objbulk.ColumnMappings.Add("ItemId", "ItemId");
                //objbulk.ColumnMappings.Add("quantity", "InQty");
                //objbulk.ColumnMappings.Add("pricePerProduct", "PurchasePrice");
                //objbulk.ColumnMappings.Add("Status", "Status");
                //objbulk.ColumnMappings.Add("PnoId", "PnoId");
                //objbulk.ColumnMappings.Add("EntryDate", "EntryDate");
                //objbulk.ColumnMappings.Add("UserName", "UserName");
                //objbulk.ColumnMappings.Add("BranchId", "BranchId");
                //objbulk.ColumnMappings.Add("Valid", "Valid");
                //objbulk.ColumnMappings.Add("EntryTime", "EntryTime");
                //objbulk.WriteToServer(dt);

                //objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVSTOCK_InDetails" };
                //objbulk.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                //objbulk.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                //objbulk.ColumnMappings.Add("CompanyId", "CompanyId");
                //objbulk.ColumnMappings.Add("ItemId", "ItemId");
                //objbulk.ColumnMappings.Add("quantity", "InQty");
                //objbulk.ColumnMappings.Add("pricePerProduct", "InvPrice");
                //objbulk.ColumnMappings.Add("Valid", "Valid");
                //objbulk.ColumnMappings.Add("expireDate", "ExpireDate");
                //objbulk.ColumnMappings.Add("UserName", "UserName");
                //objbulk.ColumnMappings.Add("BranchId", "BranchId");
                //objbulk.ColumnMappings.Add("EntryTime", "EntryTime");
                //objbulk.ColumnMappings.Add("EntryDate", "EntryDate");
                //objbulk.ColumnMappings.Add("vatPerItem", "vatPerItem");
                //objbulk.ColumnMappings.Add("lessPerItem", "lessPerItem");
                //objbulk.WriteToServer(dt);


                //---------------------test area----------------
                for (int i = 0; i < aModel.Count; i++)
                {
                    const string query4 = @"INSERT INTO tbl_INVSTOCK_StockLedger (RefNo, RefDate, InvoiceNo, InvoiceDate, CompanyId, ItemId, PurchasePrice, InQty, Status, PnoId, EntryDate, UserName, BranchId, Valid, EntryTime) 
                                            VALUES (@RefNo, @RefDate, @InvoiceNo, @InvoiceDate, @CompanyId, @ItemId, @PurchasePrice, @InQty, @Status, @PnoId, @EntryDate, @UserName, @BranchId, @Valid, @EntryTime)";
                    var cmd4 = new SqlCommand(query4, Con, _trans);
                    cmd4.Parameters.Clear();
                    cmd4.Parameters.AddWithValue("@RefNo",trNo1);
                    cmd4.Parameters.AddWithValue("@RefDate",DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd4.Parameters.AddWithValue("@InvoiceNo",invoiceNo);
                    cmd4.Parameters.AddWithValue("@InvoiceDate",DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd4.Parameters.AddWithValue("@CompanyId",aModel.ElementAt(i).CompanyId);
                    cmd4.Parameters.AddWithValue("@ItemId",aModel.ElementAt(i).ItemId);
                    cmd4.Parameters.AddWithValue("@PurchasePrice",(aModel.ElementAt(i).pricePerProduct + aModel.ElementAt(i).vatPerItem - aModel.ElementAt(i).lessPerItem));
                    cmd4.Parameters.AddWithValue("@InQty",aModel.ElementAt(i).quantity);
                    cmd4.Parameters.AddWithValue("@Status","In");
                    cmd4.Parameters.AddWithValue("@PnoId",aModel.ElementAt(i).DepartmentId);
                    cmd4.Parameters.AddWithValue("@EntryDate",DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd4.Parameters.AddWithValue("@UserName",aModel.ElementAt(i).UserName);
                    cmd4.Parameters.AddWithValue("@BranchId",Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                    cmd4.Parameters.AddWithValue("@Valid",1);
                    cmd4.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                    cmd4.ExecuteNonQuery();
                }

                for (int i = 0; i < aModel.Count; i++)
                {
                    const string query5 = @"INSERT INTO tbl_INVSTOCK_InDetails ( InvoiceNo, InvoiceDate, CompanyId, ItemId, InQty, InvPrice, vatPerItem, lessPerItem, Valid, ExpireDate, UserName, BranchId, EntryDate, EntryTime) 
                                            VALUES (@InvoiceNo, @InvoiceDate, @CompanyId, @ItemId, @InQty, @InvPrice, @vatPerItem, @lessPerItem, @Valid, @ExpireDate, @UserName, @BranchId, @EntryDate, @EntryTime)";
                    var cmd5 = new SqlCommand(query5, Con, _trans);
                    cmd5.Parameters.Clear();
                    
                    cmd5.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                    cmd5.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd5.Parameters.AddWithValue("@CompanyId", aModel.ElementAt(i).CompanyId);
                    cmd5.Parameters.AddWithValue("@ItemId", aModel.ElementAt(i).ItemId);
                    cmd5.Parameters.AddWithValue("@InvPrice", aModel.ElementAt(i).pricePerProduct);
                    cmd5.Parameters.AddWithValue("@InQty", aModel.ElementAt(i).quantity);
                    cmd5.Parameters.AddWithValue("@vatPerItem", aModel.ElementAt(i).vatPerItem);
                    cmd5.Parameters.AddWithValue("@lessPerItem", aModel.ElementAt(i).lessPerItem);
                    cmd5.Parameters.AddWithValue("@ExpireDate", aModel.ElementAt(i).expireDate);
                    cmd5.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd5.Parameters.AddWithValue("@UserName", aModel.ElementAt(i).UserName);
                    cmd5.Parameters.AddWithValue("@BranchId", Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                    cmd5.Parameters.AddWithValue("@Valid", 1);
                    cmd5.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                    cmd5.ExecuteNonQuery();
                }
                //---------------------test area----------------

                for (int i = 0; i < aModel.Count; i++)
                {
                    const string query3 = @"UPDATE  tbl_INVSTOCK_SalesProductList SET UnitPrice=@UnitPrice,RackNumber=@RackNumber,cellNumber=@cellNumber WHERE Id=@Id";
                    var cmd3 = new SqlCommand(query3, Con, _trans);
                    //var cmd3 = new SqlCommand(query3, Con);
                    cmd3.Parameters.Clear();
                    //var pricePerProduct = aModel.ElementAt(i).pricePerProduct;
                    //var pricePerProduct = aModel.ElementAt(i).vatPerItem;
                    //var pricePerProduct = aModel.ElementAt(i).vatPerItem;
                    var price = (aModel.ElementAt(i).pricePerProduct + aModel.ElementAt(i).vatPerItem - aModel.ElementAt(i).lessPerItem);
                    cmd3.Parameters.AddWithValue("@UnitPrice", price);
                    cmd3.Parameters.AddWithValue("@RackNumber", aModel.ElementAt(i).rackNo);
                    cmd3.Parameters.AddWithValue("@cellNumber", aModel.ElementAt(i).callNo);
                    cmd3.Parameters.AddWithValue("@Id", aModel.ElementAt(i).ItemId);
                    cmd3.ExecuteNonQuery();
                }

                _trans.Commit();
                Con.Close();
                //return Task.FromResult("Save successful");
                return Task.FromResult(invoiceNo);
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
        public Task<string> Update(List<InvStockReceiveModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                string invoiceNo = aModel[0].InvoiceNo;
                //string invoiceNo = GetAutoIncrementNumberFromStoreProcedure(13);
                string trNo = invoiceNo;
                string trNo1 = invoiceNo;
                //--------------Delete All---------------------
                string query11 = @"DELETE FROM tbl_INVSTOCK_InMain WHERE InvoiceNo='" + aModel[0].InvoiceNo + "'";
                string query12 = @"DELETE FROM tbl_INVSTOCK_InDetails WHERE InvoiceNo='" + aModel[0].InvoiceNo + "'";
                string query13 = @"DELETE FROM tbl_INVSTOCK_StockLedger WHERE InvoiceNo='" + aModel[0].InvoiceNo + "'";
                string query14 = @"DELETE FROM tbl_INVSTOCK_PurchaseLedger WHERE InvoiceNo='" + aModel[0].InvoiceNo + "'";

                var cmd11 = new SqlCommand(query11, Con, _trans);
                cmd11.Parameters.Clear();
                cmd11.ExecuteNonQuery();
                var cmd12 = new SqlCommand(query12, Con, _trans);
                cmd12.Parameters.Clear();
                cmd12.ExecuteNonQuery();
                var cmd13 = new SqlCommand(query13, Con, _trans);
                cmd13.Parameters.Clear();
                cmd13.ExecuteNonQuery();
                var cmd14 = new SqlCommand(query14, Con, _trans);
                cmd14.Parameters.Clear();
                cmd14.ExecuteNonQuery();
                //--------------Delete All---------------------
                const string query = @"INSERT INTO tbl_INVSTOCK_InMain (InvoiceNo, InvoiceDate, SlipNo, SlipDate, PoNo, CompanyId, TotalPrice, Vat, Less, Remarks, UserName, BranchId, EntryDate, EntryTime, Valid, PNo) VALUES (@InvoiceNo, @InvoiceDate, @SlipNo, @SlipDate, @PoNo, @CompanyId, @TotalPrice, @Vat, @Less, @Remarks, @UserName, @BranchId, @EntryDate, @EntryTime, @Valid, @PNo)";
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SlipNo", aModel.ElementAt(0).SlipNo);
                cmd.Parameters.AddWithValue("@SlipDate", aModel.ElementAt(0).SlipDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PoNo", aModel.ElementAt(0).PoNo);
                cmd.Parameters.AddWithValue("@CompanyId", aModel.ElementAt(0).CompanyId);
                cmd.Parameters.AddWithValue("@TotalPrice", (aModel.ElementAt(0).TotalPrice + aModel.ElementAt(0).VatAmount - aModel.ElementAt(0).LessAmount));
                cmd.Parameters.AddWithValue("@Vat", aModel.ElementAt(0).VatAmount);
                cmd.Parameters.AddWithValue("@Less", aModel.ElementAt(0).LessAmount);
                cmd.Parameters.AddWithValue("@Remarks", aModel.ElementAt(0).Remarks);
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@Valid", 1);
                cmd.Parameters.AddWithValue("@PNo", aModel.ElementAt(0).DepartmentId);
                cmd.ExecuteNonQuery();

                //const string query2 = @"INSERT INTO tbl_INVSTOCK_PurchaseLedger (TrNo, TrDate, InvoiceNo, InvoiceDate, CompanyId, PurchaseAmount, LessAmount, PaymentAmount, Status, PnoId, UserName, BranchId, PaymentStatus, EntryDate, Valid, EntryTime) VALUES (@TrNo, @TrDate, @InvoiceNo, @InvoiceDate, @CompanyId, @PurchaseAmount, @LessAmount, @PaymentAmount, @Status, @PnoId, @UserName, @BranchId, @PaymentStatus, @EntryDate, @Valid, @EntryTime)";
                //var cmd2 = new SqlCommand(query2, Con);
                const string query2 = @"INSERT INTO tbl_INVSTOCK_PurchaseLedger (TrNo, TrDate, InvoiceNo, InvoiceDate, CompanyId, PurchaseAmount, LessAmount, PaymentAmount, Status, PnoId, UserName, BranchId, PaymentStatus, EntryDate, Valid, EntryTime) VALUES (@TrNo, @TrDate, @InvoiceNo, @InvoiceDate, @CompanyId, @PurchaseAmount, @LessAmount, @PaymentAmount, @Status, @PnoId, @UserName, @BranchId, @PaymentStatus, @EntryDate, @Valid, @EntryTime)";
                var cmd2 = new SqlCommand(query2, Con, _trans);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@TrNo", trNo);
                cmd2.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                cmd2.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@ComPanyId", aModel.ElementAt(0).CompanyId);
                cmd2.Parameters.AddWithValue("@PurchaseAmount", ((aModel.ElementAt(0).TotalPrice) + (aModel.ElementAt(0).VatAmount)));
                cmd2.Parameters.AddWithValue("@LessAmount", aModel.ElementAt(0).LessAmount);
                cmd2.Parameters.AddWithValue("@PaymentAmount", ((aModel.ElementAt(0).TotalPrice) + (aModel.ElementAt(0).VatAmount) - (aModel.ElementAt(0).LessAmount)));
                cmd2.Parameters.AddWithValue("@Status", "Stock Receive");
                cmd2.Parameters.AddWithValue("@PnoId", aModel.ElementAt(0).DepartmentId);
                cmd2.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd2.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));
                cmd2.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd2.Parameters.AddWithValue("@PaymentStatus", 0);
                cmd2.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@Valid", 1);
                cmd2.ExecuteNonQuery();


                //aModel.ForEach(z => z.InvoiceNo = invoiceNo);
                //aModel.ForEach(z => z.InvoiceDate = DateTime.Now.Date);
                //aModel.ForEach(z => z.TrNo = trNo1);
                //aModel.ForEach(z => z.TrDate = DateTime.Now.Date);
                //aModel.ForEach(z => z.EntryTime = DateTime.Now.ToShortTimeString());
                //aModel.ForEach(z => z.EntryDate = DateTime.Now.ToString("yyyy-MM-dd"));
                //aModel.ForEach(z => z.UserName = aModel.ElementAt(0).UserName);
                //aModel.ForEach(z => z.BranchId = Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                //aModel.ForEach(z => z.Status = "In");
                //aModel.ForEach(z => z.Valid = 1);
                //aModel.ForEach(z => z.PnoId = aModel.ElementAt(0).DepartmentId);

                //DataTable dt = ConvertListDataTable(aModel);
                //var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVSTOCK_StockLedger" };
                //objbulk.ColumnMappings.Add("TrNo", "RefNo");//bame e model er nam danea tabil er colum er nam
                //objbulk.ColumnMappings.Add("TrDate", "RefDate");
                //objbulk.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                //objbulk.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                //objbulk.ColumnMappings.Add("CompanyId", "CompanyId");
                //objbulk.ColumnMappings.Add("ItemId", "ItemId");
                //objbulk.ColumnMappings.Add("quantity", "InQty");
                //objbulk.ColumnMappings.Add("pricePerProduct", "PurchasePrice");
                //objbulk.ColumnMappings.Add("Status", "Status");
                //objbulk.ColumnMappings.Add("PnoId", "PnoId");
                //objbulk.ColumnMappings.Add("EntryDate", "EntryDate");
                //objbulk.ColumnMappings.Add("UserName", "UserName");
                //objbulk.ColumnMappings.Add("BranchId", "BranchId");
                //objbulk.ColumnMappings.Add("Valid", "Valid");
                //objbulk.ColumnMappings.Add("EntryTime", "EntryTime");
                //objbulk.WriteToServer(dt);

                //objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVSTOCK_InDetails" };
                //objbulk.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                //objbulk.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                //objbulk.ColumnMappings.Add("CompanyId", "CompanyId");
                //objbulk.ColumnMappings.Add("ItemId", "ItemId");
                //objbulk.ColumnMappings.Add("quantity", "InQty");
                //objbulk.ColumnMappings.Add("pricePerProduct", "InvPrice");
                //objbulk.ColumnMappings.Add("Valid", "Valid");
                //objbulk.ColumnMappings.Add("expireDate", "ExpireDate");
                //objbulk.ColumnMappings.Add("UserName", "UserName");
                //objbulk.ColumnMappings.Add("BranchId", "BranchId");
                //objbulk.ColumnMappings.Add("EntryTime", "EntryTime");
                //objbulk.ColumnMappings.Add("EntryDate", "EntryDate");
                //objbulk.ColumnMappings.Add("vatPerItem", "vatPerItem");
                //objbulk.ColumnMappings.Add("lessPerItem", "lessPerItem");
                //objbulk.WriteToServer(dt);
                //---------------------test area----------------
                for (int i = 0; i < aModel.Count; i++)
                {
                    const string query4 = @"INSERT INTO tbl_INVSTOCK_StockLedger (RefNo, RefDate, InvoiceNo, InvoiceDate, CompanyId, ItemId, PurchasePrice, InQty, Status, PnoId, EntryDate, UserName, BranchId, Valid, EntryTime) 
                                            VALUES (@RefNo, @RefDate, @InvoiceNo, @InvoiceDate, @CompanyId, @ItemId, @PurchasePrice, @InQty, @Status, @PnoId, @EntryDate, @UserName, @BranchId, @Valid, @EntryTime)";
                    var cmd4 = new SqlCommand(query4, Con, _trans);
                    cmd4.Parameters.Clear();
                    cmd4.Parameters.AddWithValue("@RefNo", trNo1);
                    cmd4.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd4.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                    cmd4.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd4.Parameters.AddWithValue("@CompanyId", aModel.ElementAt(i).CompanyId);
                    cmd4.Parameters.AddWithValue("@ItemId", aModel.ElementAt(i).ItemId);
                    cmd4.Parameters.AddWithValue("@PurchasePrice", (aModel.ElementAt(i).pricePerProduct + aModel.ElementAt(i).vatPerItem - aModel.ElementAt(i).lessPerItem));
                    cmd4.Parameters.AddWithValue("@InQty", aModel.ElementAt(i).quantity);
                    cmd4.Parameters.AddWithValue("@Status", "In");
                    cmd4.Parameters.AddWithValue("@PnoId", aModel.ElementAt(i).DepartmentId);
                    cmd4.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd4.Parameters.AddWithValue("@UserName", aModel.ElementAt(i).UserName);
                    cmd4.Parameters.AddWithValue("@BranchId", Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                    cmd4.Parameters.AddWithValue("@Valid", 1);
                    cmd4.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                    cmd4.ExecuteNonQuery();
                }

                for (int i = 0; i < aModel.Count; i++)
                {
                    const string query5 = @"INSERT INTO tbl_INVSTOCK_InDetails ( InvoiceNo, InvoiceDate, CompanyId, ItemId, InQty, InvPrice, vatPerItem, lessPerItem, Valid, ExpireDate, UserName, BranchId, EntryDate, EntryTime) 
                                            VALUES (@InvoiceNo, @InvoiceDate, @CompanyId, @ItemId, @InQty, @InvPrice, @vatPerItem, @lessPerItem, @Valid, @ExpireDate, @UserName, @BranchId, @EntryDate, @EntryTime)";
                    var cmd5 = new SqlCommand(query5, Con, _trans);
                    cmd5.Parameters.Clear();

                    cmd5.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                    cmd5.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd5.Parameters.AddWithValue("@CompanyId", aModel.ElementAt(i).CompanyId);
                    cmd5.Parameters.AddWithValue("@ItemId", aModel.ElementAt(i).ItemId);
                    cmd5.Parameters.AddWithValue("@InvPrice", aModel.ElementAt(i).pricePerProduct);
                    cmd5.Parameters.AddWithValue("@InQty", aModel.ElementAt(i).quantity);
                    cmd5.Parameters.AddWithValue("@vatPerItem", aModel.ElementAt(i).vatPerItem);
                    cmd5.Parameters.AddWithValue("@lessPerItem", aModel.ElementAt(i).lessPerItem);
                    cmd5.Parameters.AddWithValue("@ExpireDate", aModel.ElementAt(i).expireDate);
                    cmd5.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd5.Parameters.AddWithValue("@UserName", aModel.ElementAt(i).UserName);
                    cmd5.Parameters.AddWithValue("@BranchId", Convert.ToInt32(GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans)));
                    cmd5.Parameters.AddWithValue("@Valid", 1);
                    cmd5.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                    cmd5.ExecuteNonQuery();
                }
                //---------------------test area----------------

                for (int i = 0; i < aModel.Count; i++)
                {
                    const string query3 = @"UPDATE  tbl_INVSTOCK_SalesProductList SET UnitPrice=@UnitPrice,RackNumber=@RackNumber,cellNumber=@cellNumber WHERE Id=@Id";
                    var cmd3 = new SqlCommand(query3, Con, _trans);
                    //var cmd3 = new SqlCommand(query3, Con);
                    cmd3.Parameters.Clear();

                    var price = (aModel.ElementAt(i).pricePerProduct + aModel.ElementAt(i).vatPerItem - aModel.ElementAt(i).lessPerItem);
                    cmd3.Parameters.AddWithValue("@UnitPrice", price);
                    cmd3.Parameters.AddWithValue("@RackNumber", aModel.ElementAt(i).rackNo);
                    cmd3.Parameters.AddWithValue("@cellNumber", aModel.ElementAt(i).callNo);
                    cmd3.Parameters.AddWithValue("@Id", aModel.ElementAt(i).ItemId);
                    cmd3.ExecuteNonQuery();
                }

                _trans.Commit();
                Con.Close();
                return Task.FromResult("Update successful");
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

        public List<InvStockReceiveModel> GetRequisitionList()
        {
            try
            {
                //int status = 1;
                //int branchId = 0;
                //branchId = GetBranchIdByuserName(userName);
                var lists = new List<InvStockReceiveModel>();
                string query = "";

                query = @"Select InvoiceNo,InvoiceDate,SlipNo,PoNo,b.Name AS companyName,c.SubsubPNo AS departmentName from tbl_INVSTOCK_InMain AS a inner join tbl_INVSTOCK_Supplier_info AS b ON a.CompanyId=b.Id inner join project AS c ON a.PNo=c.IdNo";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvStockReceiveModel()
                    {
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        SlipNo = rdr["SlipNo"].ToString(),
                        PoNo = rdr["PoNo"].ToString(),
                        CompanyName = rdr["companyName"].ToString(),
                        PnoName = rdr["departmentName"].ToString(),

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
                return new List<InvStockReceiveModel>();
            }
        }

        public List<InvStockReceiveModel> GetInvoiceDetails(String InvoiceId)
        {
            try
            {
                //int status = 1;
                //int branchId = 0;
                //branchId = GetBranchIdByuserName(userName);
                var lists = new List<InvStockReceiveModel>();
                string query = "";

                query = @"Select a.Id, a.InvoiceNo, a.InvoiceDate, SlipNo, SlipDate, PoNo, a.CompanyId, TotalPrice, Vat, Less, Remarks, a.UserName, a.BranchId, a.EntryDate, a.EntryTime, a.Valid, PNo,
                            b.ItemId,c.ProductName,b.InQty,b.InvPrice,b.ExpireDate,c.RackNumber,c.cellNumber,c.Unit,c.UnitPrice,b.lessPerItem,b.vatPerItem
                            from tbl_INVSTOCK_InMain AS a inner join tbl_INVSTOCK_InDetails AS b ON a.InvoiceNo=b.InvoiceNo
							                              inner join tbl_INVSTOCK_SalesProductList AS c ON b.ItemId=c.Id
                            where a.InvoiceNo='" + InvoiceId + "'";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvStockReceiveModel()
                    {
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        SlipNo = rdr["SlipNo"].ToString(),
                        SlipDate = Convert.ToDateTime(rdr["SlipDate"]),
                        PoNo = rdr["PoNo"].ToString(),
                        CompanyId = Convert.ToInt32(rdr["CompanyId"]),
                        totalPricePerProduct = Convert.ToDouble(rdr["TotalPrice"]),
                        VatAmount = Convert.ToDouble(rdr["Vat"]),
                        LessAmount = Convert.ToDouble(rdr["Less"]),
                        Remarks = rdr["Remarks"].ToString(),
                        PnoId = Convert.ToInt32(rdr["PNo"]),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        ProductName = rdr["ProductName"].ToString(),
                        quantity = Convert.ToDouble(rdr["InQty"]),
                        pricePerProduct = Convert.ToDouble(rdr["InvPrice"]),
                        expireDate = Convert.ToDateTime(rdr["ExpireDate"]),
                        //CompanyName = rdr["companyName"].ToString(),
                        //PnoName = rdr["departmentName"].ToString(),
                        rackNo = rdr["RackNumber"].ToString(),
                        callNo = rdr["cellNumber"].ToString(),
                        Unit = rdr["Unit"].ToString(),

                        vatPerItem = Convert.ToDouble(rdr["lessPerItem"]),
                        lessPerItem = Convert.ToDouble(rdr["vatPerItem"]),


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
                return new List<InvStockReceiveModel>();
            }
        }

    }
}