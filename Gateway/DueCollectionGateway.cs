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
    public class DueCollectionGateway:DbConnection
    {
        private SqlTransaction _trans;
        internal List<InvoiceModel> GetDueInvoiceList(string param)
        {
            try
            {
                var lists = new List<InvoiceModel>();
                string cnd = "";
                if (param!="")
                {
                    cnd = "WHERE (PtRegNo+Name+MobileNo+InvoiceNo) LIKE '%' + @param + '%'";
                }
                string query = @"SELECT RefDrId,RegId,InvMasterId,PtRegNo,Name,InvoiceNo,InvoiceDate,Due,MobileNo FROM VW_DUE_INVOICE_LIST " + cnd + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@param", param);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvoiceModel()
                    {
                        DrId = Convert.ToInt32(rdr["RefDrId"]),
                        PtRegId= Convert.ToInt32(rdr["RegId"]),
                        InvMasterId = Convert.ToInt32(rdr["InvMasterId"]),
                        PtRegNo = rdr["PtRegNo"].ToString(),
                        PtName = rdr["Name"].ToString(),
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                        DueAmt = Convert.ToDouble(rdr["Due"]),
                        PtMobileNo = rdr["MobileNo"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open) { Con.Close(); }
                throw;
            }
        }
        internal double GetMaxLessAmountInDueCollection(int invMasterId,int lessFrom)
        {
            try
            {
                double hnrAmt =Convert.ToDouble(ReturnFieldValue("tbl_DOCTOR_HONORIUM_LEDGER", "InvMasterId=" + invMasterId + "","Isnull(SUM(HonoriumAmount),0)"));
                double lessAmt = Convert.ToDouble(ReturnFieldValue("tbl_DOCTOR_HONORIUM_LEDGER", "InvMasterId=" + invMasterId + "", "Isnull(SUM(LessAmount),0)"));
                double maxhnrAmt = 0;
                if (lessFrom == 1)
                {
                    maxhnrAmt = hnrAmt - lessAmt;
                }
                else if (lessFrom==2)
                {
                    maxhnrAmt = hnrAmt - lessAmt / 2;
                }
                return maxhnrAmt;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        internal Task<string> Save(InvoiceModel mInvoice)
        {
            try
            {
                Con.Open();
                Thread.Sleep(50);
                _trans = Con.BeginTransaction();
                int indoorId = 0;
                int bedId = 0;

                mInvoice.TrNo =  GetTrNoWithOpenCon("TrNo", "tbl_INVOICE_DUE_COLLECTION", _trans);
                const string query = @"INSERT INTO tbl_INVOICE_DUE_COLLECTION (DrId, RegId, IndoorId, InvMasterId, TrNo, TrDate, PaidAmt, DscountPc, DiscountPcOrTk, DiscountFrom, DiscountAmt, Remarks,  CashAmt, CardAmt, ChequeAmt, CardNumber, CardBankId, ChqNo, ChqBankId, BedId, UserName, BranchId,SubSubPnoId) 
                                    VALUES (@DrId,@RegId, @IndoorId, @InvMasterId, @TrNo, @TrDate, @PaidAmt, @DscountPc, @DiscountPcOrTk, @DiscountFrom, @DiscountAmt, @Remarks,  @CashAmt, @CardAmt,@ChequeAmt, @CardNumber, @CardBankId, @ChqNo, @ChqBankId, @BedId, @UserName, @BranchId,@SubSubPnoId)";

                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@DrId", ReturnFieldValueOpenCon("tbl_INVOICE_MASTER", "Id=" + mInvoice.InvMasterId + "", "RefDrId",_trans));
                cmd.Parameters.AddWithValue("@RegId", mInvoice.PtRegId);
                cmd.Parameters.AddWithValue("@IndoorId", indoorId);
                cmd.Parameters.AddWithValue("@InvMasterId", mInvoice.InvMasterId);
                cmd.Parameters.AddWithValue("@TrNo", mInvoice.TrNo);
                cmd.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PaidAmt", mInvoice.CashAmount + mInvoice.CardAmount + mInvoice.CheaqueAmount);
                cmd.Parameters.AddWithValue("@DscountPc", mInvoice.LessPc);
                cmd.Parameters.AddWithValue("@DiscountPcOrTk", mInvoice.LessPcOrTk);
                cmd.Parameters.AddWithValue("@DiscountFrom", mInvoice.LessFrom);
                cmd.Parameters.AddWithValue("@DiscountAmt", mInvoice.LessAmount);
                cmd.Parameters.AddWithValue("@Remarks", mInvoice.Remarks);
                cmd.Parameters.AddWithValue("@CashAmt", mInvoice.CashAmount);
                cmd.Parameters.AddWithValue("@CardAmt", mInvoice.CardAmount);
                cmd.Parameters.AddWithValue("@ChequeAmt", mInvoice.CheaqueAmount);
                cmd.Parameters.AddWithValue("@CardNumber", mInvoice.CardNumber);
                cmd.Parameters.AddWithValue("@CardBankId", mInvoice.CardBankId);
                cmd.Parameters.AddWithValue("@ChqNo", mInvoice.CheaqueNumber);
                cmd.Parameters.AddWithValue("@ChqBankId", mInvoice.CheaqueBankId);
                cmd.Parameters.AddWithValue("@BedId", bedId);
                cmd.Parameters.AddWithValue("@SubSubPnoId", 69);
                cmd.Parameters.AddWithValue("@UserName", mInvoice.UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(mInvoice.UserName, _trans));
                cmd.ExecuteNonQuery();
                InsertIntoInvoiceLedger(mInvoice.PtRegId, indoorId, mInvoice.InvMasterId, mInvoice.TrNo, DateTime.Now, 0, mInvoice.LessAmount, mInvoice.ReceiveAmount, 0, 69, "DueColl", mInvoice.UserName, _trans);
                if (mInvoice.LessAmount>0)
                {
                    InsertIntoHonoriumLedger(mInvoice.DrId, mInvoice.TrNo, DateTime.Now, mInvoice.InvMasterId, 0, 0, mInvoice.LessAmount, 0, 65, mInvoice.UserName, _trans);    
                }
                
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
    }
}