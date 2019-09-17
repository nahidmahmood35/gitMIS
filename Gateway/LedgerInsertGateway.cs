using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;

namespace HospitalManagementApp_Api.Gateway
{
    
    public class LedgerInsertGateway
    {
        readonly DbConnection _db=new DbConnection();
        public void InsertLedgerOfAdmittedPatient(string refNo,int regId,  int indoorId, int itemId, double charge,double quantity, double vat,double serviceCharge,double payableAmt,double collAmt,double lessAmt,double rtnAmt,double adjustAmt,int honoriumDrId,int subsubPnoId,string userName,int bedId,int refDrId,int admitDrId,int underDrId,double cashAmt,double cardAmt,double chequeAmt,int cardBankId,int cheaqueBankId,string cardNo,string cheaqueNo,string remarks, SqlTransaction transaction,SqlConnection con)
        {
            
            try
            {
                const string query = @"INSERT INTO tbl_IN_LEDGER_OF_ADMITTED_PATIENT (RegId, IndoorId, BedId, RefNo, RefDate, ItemId, Charge, Quantity, Vat, ServiceCharge, PayableAmt, CollAmt, LessAmt, RtnAmt, AdjustAmt, AdmitDrId, UnderDrId, RefDrId,  HonoriumDrId, SubSubPnoId, UserName,CashAmt,CardAmt,ChequeAmt,CardNo,CardBankId,ChequeNo,ChequeBankId,Remarks) VALUES (@RegId, @IndoorId, @BedId, @RefNo, @RefDate, @ItemId, @Charge, @Quantity, @Vat, @ServiceCharge, @PayableAmt, @CollAmt, @LessAmt, @RtnAmt, @AdjustAmt, @AdmitDrId, @UnderDrId, @RefDrId,   @HonoriumDrId, @SubSubPnoId, @UserName,@CashAmt,@CardAmt,@ChequeAmt,@CardNo,@CardBankId,@ChequeNo,@ChequeBankId,@Remarks)";
                var cmd = new SqlCommand(query, con, transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@RegId", regId);
                cmd.Parameters.AddWithValue("@IndoorId", indoorId);
                cmd.Parameters.AddWithValue("@BedId", bedId);
                cmd.Parameters.AddWithValue("@RefNo", refNo);
                cmd.Parameters.AddWithValue("@RefDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ItemId", itemId);
                cmd.Parameters.AddWithValue("@Charge", charge);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Vat", vat);
                cmd.Parameters.AddWithValue("@ServiceCharge", serviceCharge);
                cmd.Parameters.AddWithValue("@PayableAmt", payableAmt);
                cmd.Parameters.AddWithValue("@CollAmt", collAmt);
                cmd.Parameters.AddWithValue("@LessAmt", lessAmt);
                cmd.Parameters.AddWithValue("@RtnAmt", rtnAmt);
                cmd.Parameters.AddWithValue("@AdjustAmt", adjustAmt);
                cmd.Parameters.AddWithValue("@AdmitDrId", admitDrId);
                cmd.Parameters.AddWithValue("@UnderDrId", underDrId);
                cmd.Parameters.AddWithValue("@RefDrId", refDrId);
                cmd.Parameters.AddWithValue("@HonoriumDrId", honoriumDrId);
                cmd.Parameters.AddWithValue("@SubSubPnoId", subsubPnoId);
                cmd.Parameters.AddWithValue("@UserName", userName);

                cmd.Parameters.AddWithValue("@CashAmt", cashAmt);
                cmd.Parameters.AddWithValue("@CardAmt", cardAmt);
                cmd.Parameters.AddWithValue("@ChequeAmt", chequeAmt);
                cmd.Parameters.AddWithValue("@CardNo", cardNo);
                cmd.Parameters.AddWithValue("@CardBankId", cardBankId);
                cmd.Parameters.AddWithValue("@ChequeNo", cheaqueNo);
                cmd.Parameters.AddWithValue("@ChequeBankId", cheaqueBankId);
                cmd.Parameters.AddWithValue("@Remarks", remarks);
       


//double cashAmt,double cardAmt,double chequeAmt,int cardBankId,int cheaqueBankId,string cardNo,string cheaqueNo,




                cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}