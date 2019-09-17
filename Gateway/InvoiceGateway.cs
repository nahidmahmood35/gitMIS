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
    public class InvoiceGateway:DbConnection
    {
        private SqlTransaction _trans;
        readonly LedgerInsertGateway _insLdgr=new LedgerInsertGateway();
        private int bedId = 0,subsubPnoId=69;
        public double GetMaximumLessAmountByDoctor(List<InvoiceModel> mInvoice)
        {
            try
            {
                double maxRefFeeAmount = 0;
                foreach (var invoiceModel in mInvoice)
                    {
                    if (invoiceModel.ReportGroupName=="n/a")
                    {
                        int deptId = Convert.ToInt32(ReturnFieldValue("tbl_CLINICAL_CHART", "Id=" + invoiceModel.ItemId + "", "SubSubPnoId"));
                        if (FncSeekRecordNew("tbl_GROUPWISE_DOCTOR_REFFEREL_FEE", "DrId=" + invoiceModel.DrId + " AND DeptId=" + deptId + ""))
                        {
                            maxRefFeeAmount += Convert.ToDouble(ReturnFieldValue("tbl_GROUPWISE_DOCTOR_REFFEREL_FEE", "DrId=" + invoiceModel.DrId + " AND DeptId=" + deptId + "", "CASE WHEN RefFeePcOrTk = 'Tk.' THEN RefFeePc ELSE CAST(RefFeePc*0.01*" + invoiceModel.Charge + " AS Money) END"));
                        }
                        else
                        {
                            maxRefFeeAmount += Convert.ToDouble(ReturnFieldValue("tbl_CLINICAL_CHART", "Id=" + invoiceModel.ItemId + "", "CASE WHEN RefFeePcOrTk = 'Tk.' THEN RefFee ELSE CAST(RefFee*0.01*Charge AS Money) END"));
                        }
                    }
                
                
                }
                return maxRefFeeAmount;
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

        internal Task<string> Save(List<InvoiceModel> mInvoice)
        {
            try
            {
                double maxRefAmount = GetMaximumLessAmountByDoctor(mInvoice);
                Con.Open();
                Thread.Sleep(50);
                
                _trans = Con.BeginTransaction();
                string invNo ="DI"+ GetTrNoWithOpenCon("InvoiceNo", "tbl_INVOICE_MASTER", _trans);
                const string query = @"INSERT INTO tbl_INVOICE_MASTER (RegId, IndoorId,  PatientStatus, InvoiceNo, InvoiceDate, Age, RefDrId, ConsultantId, DiscoutPC, DiscountPcOrTk, DiscountFrom, DiscountAmt, TotalAmt, AdvanceAmt, PaidAmount,  SubSubPnoId, CorporateId, Remarks, PackageId, CashAmt, CardAmt, ChequeAmt, CardNo, CardBankId, ChqNo, ChqBankId,  UserName, BranchId,IsUrgent,BedId) 
                            OUTPUT INSERTED.ID VALUES (@RegId, @IndoorId,  @PatientStatus, @InvoiceNo, @InvoiceDate, @Age, @RefDrId, @ConsultantId, @DiscoutPC, @DiscountPcOrTk, @DiscountFrom, @DiscountAmt, @TotalAmt, @AdvanceAmt, @PaidAmount,   @SubSubPnoId, @CorporateId, @Remarks, @PackageId, @CashAmt, @CardAmt, @ChequeAmt, @CardNo, @CardBankId, @ChqNo, @ChqBankId,   @UserName, @BranchId,@IsUrgent,@BedId)";
                
                var cmd = new SqlCommand(query, Con,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@RegId", mInvoice[0].PtRegId);
                cmd.Parameters.AddWithValue("@IndoorId", mInvoice[0].PtIndoorId);

                if (FncSeekRecordNew("tbl_IN_PATIENT_ADMISSION", "Id=" + mInvoice.ElementAt(0).PtIndoorId + "",_trans))
                {
                    bedId=Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + mInvoice.ElementAt(0).PtIndoorId + "","BedId",_trans));
                    subsubPnoId = GetSubSubPnoByIndoorId(mInvoice[0].PtIndoorId, _trans);    
                }
                









                cmd.Parameters.AddWithValue("@BedId", bedId);    
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
                cmd.Parameters.AddWithValue("@SubSubPnoId", subsubPnoId);
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

                InsertIntoInvoiceLedger(mInvoice[0].PtRegId, mInvoice[0].PtIndoorId, invmasterId, GetTrNoWithOpenCon("TrNo", "tbl_INVOICE_LEDGER", _trans), DateTime.Now, mInvoice.ElementAt(0).TotalAmount, mInvoice.ElementAt(0).LessAmount, mInvoice.ElementAt(0).ReceiveAmount, 0, subsubPnoId, collStatus, mInvoice.ElementAt(0).UserName, _trans);
                InsertIntoHonoriumLedger(mInvoice.ElementAt(0).DrId, GetTrNoWithOpenCon("TrNo", "tbl_DOCTOR_HONORIUM_LEDGER", _trans), DateTime.Now, invmasterId, mInvoice.ElementAt(0).TotalAmount, maxRefAmount, mInvoice.ElementAt(0).LessAmount, 0, subsubPnoId, mInvoice.ElementAt(0).UserName, _trans);

                
                if (mInvoice[0].PtIndoorId!=0)
                {
                    int regId = Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + mInvoice[0].PtIndoorId + "", "RegId", _trans));
                    _insLdgr.InsertLedgerOfAdmittedPatient(invNo, regId, mInvoice[0].PtIndoorId, 2312, mInvoice[0].TotalAmount, 1, 0, 0, mInvoice[0].TotalAmount, 0, 0, 0, 0, 0, subsubPnoId, mInvoice[0].UserName, bedId, Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + mInvoice[0].PtIndoorId + "", "RefDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + mInvoice[0].PtIndoorId + "", "AdmitDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + mInvoice[0].PtIndoorId + "", "UnderDrId", _trans)), 0, 0, 0, 0, 0, "N/A", "N/A", mInvoice[0].Remarks, _trans, Con);
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




      











//        public void InsertIntoInvoiceLedger(int regId, int  indoorId, int invmasterId , string trNo, DateTime trDate, double salesAmt, double lessAmt,double collAmt , double rtnAmt,int subsubPnoId,string saleCollStatus,string userName,SqlTransaction transaction)
//        {
//            try
//            {
//                const string query = @"INSERT INTO tbl_INVOICE_LEDGER (RegId, IndoorId, InvMasterId, TrNo, TrDate, SalesAmt, LessAmt, CollectionAmt, ReturnAmt, SubSubPnoId, SalesCollStatus, UserName, BranchId) 
//                                        VALUES (@RegId, @IndoorId, @InvMasterId, @TrNo, @TrDate, @SalesAmt, @LessAmt, @CollectionAmt, @ReturnAmt, @SubSubPnoId, @SalesCollStatus, @UserName, @BranchId)";
//                var aComand = new SqlCommand(query, Con, transaction);
//                aComand.Parameters.Clear();
//                aComand.Parameters.AddWithValue("@RegId", regId);
//                aComand.Parameters.AddWithValue("@IndoorId", indoorId);
//                aComand.Parameters.AddWithValue("@InvMasterId", invmasterId);
//                aComand.Parameters.AddWithValue("@TrNo", trNo);
//                aComand.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
//                aComand.Parameters.AddWithValue("@SalesAmt", salesAmt);
//                aComand.Parameters.AddWithValue("@LessAmt", lessAmt);
//                aComand.Parameters.AddWithValue("@CollectionAmt", collAmt);
//                aComand.Parameters.AddWithValue("@ReturnAmt", rtnAmt);
//                aComand.Parameters.AddWithValue("@SubSubPnoId", subsubPnoId);
//                aComand.Parameters.AddWithValue("@SalesCollStatus", saleCollStatus);
//                aComand.Parameters.AddWithValue("@UserName", userName);
        //                aComand.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(userName,transaction));
//                //  Con.Open();
//                aComand.ExecuteNonQuery();
//                //   Con.Close();
//            }
//            catch (Exception exception)
//            {
                
//                throw;
//            }                                                   
                
//        }

        internal List<ClinicalChartModel> GetPackageItemDetail(int mainpackId)
        {
            try
            {
                var lists = new List<ClinicalChartModel>();
                const string query = @"SELECT a.*,b.Pcode,b.Description,b.Charge 
                                FROM tbl_PACKAGE_INFO_DTL a LEFT JOIN tbl_CLINICAL_CHART b ON a.ItemId=b.Id WHERE a.MainPackageId=@mainpackId";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@mainpackId", mainpackId);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new ClinicalChartModel()
                    {
                        ItemId=Convert.ToInt32(rdr["ItemId"]),
                        PCode = rdr["Pcode"].ToString(),
                        Description = rdr["Description"].ToString(),
                        Charge = Convert.ToDouble(rdr["Charge"]),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;

            }
            catch (Exception)
            {
                if (Con.State == ConnectionState.Open){Con.Close();}
                throw;
            }
        }



        internal List<InvoiceModel> GetInvoiceList(string dateFrom, string dateTo)
        {
            try
            {
                var lists = new List<InvoiceModel>();
                string query = @"EXEC SP_GET_INVOICE_LEDGER 69,'" + dateFrom + "','" + dateTo + "',0";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvoiceModel()
                    {
                       InvMasterId= Convert.ToInt32(rdr["InvMasterId"]),
                       InvoiceNo = rdr["InvoiceNo"].ToString(),
                       InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"]),
                       TrDate = Convert.ToDateTime(rdr["TrDate"]),
                       TrNo = rdr["TrNo"].ToString(),
                       PtRegNo = rdr["PtRegNo"].ToString(),
                       PtName = rdr["Name"].ToString(),
                       MobileNo = rdr["MobileNo"].ToString(),
                       PtGenderName = rdr["Gender"].ToString(),
                       PtAddress = rdr["Address"].ToString(), 
                       TotalAmount = Convert.ToDouble(rdr["SalesAmt"]),
                       LessAmount = Convert.ToDouble(rdr["LessAmt"]),
                       ReceiveAmount = Convert.ToDouble(rdr["CollAmt"]),
                       ReturnAmount = Convert.ToDouble(rdr["RtnAmt"]),
                       BalAmt = Convert.ToDouble(rdr["BalAmt"]),
                    });
                }
                Con.Close();
                return lists;
            }
            catch (Exception )
            {

                if (Con.State == ConnectionState.Open) { Con.Close(); }
                throw;
            }
        }
    }
}