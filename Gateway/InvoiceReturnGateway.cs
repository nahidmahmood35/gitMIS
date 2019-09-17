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
    public class InvoiceReturnGateway : DbConnection
    {
        private SqlTransaction _trans;
        
        internal Task<string> Save(List<InvoiceModel> mInvoice)
        {
            try
            {
              
                Con.Open();
                Thread.Sleep(50);
                
                _trans = Con.BeginTransaction();
                string invNo = GetTrNoWithOpenCon("InvoiceNo", "tbl_INVOICE_MASTER", _trans);
                const string query = @"INSERT INTO tbl_INVOICE_MASTER (RegId, IndoorId,  PatientStatus, InvoiceNo, InvoiceDate, Age, RefDrId, ConsultantId, DiscoutPC, DiscountPcOrTk, DiscountFrom, DiscountAmt, TotalAmt, AdvanceAmt, PaidAmount,  SubSubPnoId, CorporateId, Remarks, PackageId, CashAmt, CardAmt, ChequeAmt, CardNo, CardBankId, ChqNo, ChqBankId,  UserName, BranchId,IsUrgent) 
                            OUTPUT INSERTED.ID VALUES (@RegId, @IndoorId,  @PatientStatus, @InvoiceNo, @InvoiceDate, @Age, @RefDrId, @ConsultantId, @DiscoutPC, @DiscountPcOrTk, @DiscountFrom, @DiscountAmt, @TotalAmt, @AdvanceAmt, @PaidAmount,   @SubSubPnoId, @CorporateId, @Remarks, @PackageId, @CashAmt, @CardAmt, @ChequeAmt, @CardNo, @CardBankId, @ChqNo, @ChqBankId,   @UserName, @BranchId,@IsUrgent)";
                
                var cmd = new SqlCommand(query, Con,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@RegId", mInvoice.ElementAt(0).PtRegId);
                cmd.Parameters.AddWithValue("@IndoorId", mInvoice.ElementAt(0).PtIndoorId);
                cmd.Parameters.AddWithValue("@PatientStatus", mInvoice.ElementAt(0).PatientStatus);
                cmd.Parameters.AddWithValue("@InvoiceNo", invNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Age", mInvoice.ElementAt(0).PtAgeDetail);
                cmd.Parameters.AddWithValue("@RefDrId", mInvoice.ElementAt(0).DrId);
                cmd.Parameters.AddWithValue("@ConsultantId", mInvoice.ElementAt(0).ConsultantId);
                cmd.Parameters.AddWithValue("@DiscoutPC", mInvoice.ElementAt(0).LessPc);
                cmd.Parameters.AddWithValue("@DiscountPcOrTk", mInvoice.ElementAt(0).LessPcOrTk);
                cmd.Parameters.AddWithValue("@DiscountFrom", mInvoice.ElementAt(0).LessFrom);
                cmd.Parameters.AddWithValue("@DiscountAmt", mInvoice.ElementAt(0).LessAmount);
                cmd.Parameters.AddWithValue("@TotalAmt", mInvoice.ElementAt(0).TotalAmount);
                cmd.Parameters.AddWithValue("@AdvanceAmt", mInvoice.ElementAt(0).ReceiveAmount);
                cmd.Parameters.AddWithValue("@PaidAmount", mInvoice.ElementAt(0).ReceiveAmount);
                cmd.Parameters.AddWithValue("@SubSubPnoId", 69);
                cmd.Parameters.AddWithValue("@CorporateId", mInvoice.ElementAt(0).CorporateId);
                cmd.Parameters.AddWithValue("@Remarks", mInvoice.ElementAt(0).Remarks);
                cmd.Parameters.AddWithValue("@PackageId", mInvoice.ElementAt(0).PackageId);
                cmd.Parameters.AddWithValue("@CashAmt", mInvoice.ElementAt(0).CashAmount);
                cmd.Parameters.AddWithValue("@CardAmt", mInvoice.ElementAt(0).CardAmount);
                cmd.Parameters.AddWithValue("@ChequeAmt", mInvoice.ElementAt(0).CheaqueAmount);
                cmd.Parameters.AddWithValue("@CardNo", mInvoice.ElementAt(0).CardNumber);
                cmd.Parameters.AddWithValue("@CardBankId", mInvoice.ElementAt(0).CardBankId);
                cmd.Parameters.AddWithValue("@ChqNo", mInvoice.ElementAt(0).CheaqueNumber);
                cmd.Parameters.AddWithValue("@ChqBankId", mInvoice.ElementAt(0).CheaqueBankId);
                cmd.Parameters.AddWithValue("@IsUrgent", mInvoice.ElementAt(0).IsUrgent);
                cmd.Parameters.AddWithValue("@UserName", mInvoice.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(mInvoice.ElementAt(0).UserName,_trans));

                var invmasterId=(int)cmd.ExecuteScalar();

                //mInvoice.ElementAt(0).IsUrgent



                foreach (var invoiceModel in mInvoice)
                {
                    if (invoiceModel.ReportGroupName!="n/a")
                    {
                        const string lcQuery = @"INSERT INTO tbl_INVOICE_VAQ_DETAIL (InvMasterId, ItemId, VaqId, Price)VALUES(@InvMasterId, @ItemId, @VaqId, @Price)";
                        cmd = new SqlCommand(lcQuery, Con, _trans);
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@InvMasterId", invmasterId);
                        cmd.Parameters.AddWithValue("@ItemId", invoiceModel.ItemIdForVaq);
                        cmd.Parameters.AddWithValue("@VaqId", invoiceModel.ItemId);
                        cmd.Parameters.AddWithValue("@Price", invoiceModel.Charge);
                        cmd.ExecuteNonQuery();
                    }
                }
                mInvoice.RemoveAll(r => r.ReportGroupName !="n/a");

                mInvoice.ForEach(z => z.InvMasterId= invmasterId);
                mInvoice.ForEach(z => z.InvoiceDate = DateTime.Now.Date);
                mInvoice.ForEach(z => z.DeliveryDate=DateTime.Now.AddDays(Convert.ToInt32(ReturnFieldValueOpenCon("tbl_CLINICAL_CHART","Id="+ z.ItemId +"","ReportDeliverDuration",_trans))));
                mInvoice.ForEach(z => z.ItemwiseLess = z.Charge*z.LessAmount/z.TotalAmount);
                mInvoice.ForEach(z => z.Quantity = 1);
                

                DataTable dt = ConvertListDataTable(mInvoice);
                
                var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_INVOICE_DETAIL" };
                    objbulk.ColumnMappings.Add("InvMasterId", "InvMasterId");
                    objbulk.ColumnMappings.Add("ItemId", "ItemId");
                    objbulk.ColumnMappings.Add("Charge", "Charge");
                    objbulk.ColumnMappings.Add("IndoorSeviceDrId", "IndoorServiceDrId");
                    objbulk.ColumnMappings.Add("Quantity", "Quantity");
                    objbulk.ColumnMappings.Add("DeliveryDate", "RptDeliveryDate");
                    objbulk.ColumnMappings.Add("ItemwiseLess", "ItemWiseLess");
                    objbulk.ColumnMappings.Add("Vat", "VATAmt");
                    objbulk.ColumnMappings.Add("ServiceCharge", "ServiceCharge");

                    objbulk.WriteToServer(dt);
                
                
                string collStatus = mInvoice.ElementAt(0).CashAmount > 0 ? "Sales And Collection" : "Sales";

                
                
                _trans.Commit();
                Con.Close();
                return Task.FromResult<string>("Save Successful");
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

        

      

        internal List<InvoiceModel> GetInvoiceDetails(int invMasterId)
        {
            try
            {
                var lists = new List<InvoiceModel>();
                string query = @"SELECT * FROM VW_GET_INVOICE_DETAIL_FOR_UPDATE WHERE InvMasterId="+ invMasterId +"";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvoiceModel()
                    {
                        InvMasterId = Convert.ToInt32(rdr["InvMasterId"]),
                       // InvoiceNo = rdr["InvoiceNo"].ToString(),
                       // InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        TotalAmount = Convert.ToDouble(rdr["SalesAmt"]),
                        LessAmount = Convert.ToDouble(rdr["LessAmt"]),
                        CollectionFee = Convert.ToDouble(rdr["CollAmt"]),
                        ReturnAmount = Convert.ToDouble(rdr["RtnAmt"]),
                        PtName = rdr["PtName"].ToString(),
                        PtMobileNo = rdr["PtMobileNo"].ToString(),
                        PtAddress = rdr["Address"].ToString(),
                        PtGenderName = rdr["GenderName"].ToString(),
                        ConsultantId = Convert.ToInt32(rdr["ConsultantId"]),
                        DrName = rdr["DrName"].ToString(),
                        Code = rdr["DrCode"].ToString(),
                        Charge = Convert.ToDouble(rdr["Charge"]),
                        PCode = rdr["PCode"].ToString(),
                        Description = rdr["Description"].ToString(),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                    });
                }
                Con.Close();


                foreach (var dstMdl in lists)
                {
                    dstMdl.ItemwiseLess = dstMdl.Charge*dstMdl.LessAmount/dstMdl.TotalAmount;
                }










                return lists;
            }
            catch (Exception ex)
            {

                if (Con.State == ConnectionState.Open) { Con.Close(); }
                throw;
            }
        }
    }
}