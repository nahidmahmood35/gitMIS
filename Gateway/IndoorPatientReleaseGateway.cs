using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;
using System.Data;

namespace HospitalManagementApp_Api.Gateway
{
    public class IndoorPatientReleaseGateway:DbConnection
    {
        private SqlTransaction _trans;
        readonly LedgerInsertGateway _gtLedgerInsertGateway=new LedgerInsertGateway();
        internal Task<string> Save(List<PatientReleaseModel> aModel)
        {
           try
                {
                    Con.Open();
                    _trans = Con.BeginTransaction();


                    int refDrId = Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.ElementAt(0).PtIndoorId + "", "RefDrId", _trans));
                    int underDrId = Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.ElementAt(0).PtIndoorId + "", "UnderDrId", _trans));
                    int admitDrId = Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.ElementAt(0).PtIndoorId + "", "AdmitDrId", _trans));
                    int bedId = Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.ElementAt(0).PtIndoorId + "", "BedId", _trans));
                    int regId = Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.ElementAt(0).PtIndoorId + "", "RegId", _trans));

                    string refNo = "PR" + GetTrNoWithOpenCon("ReceiptNo", "tbl_IN_PATIENT_RELEASE_MASTER", _trans);
                    const string query = @"INSERT INTO tbl_IN_PATIENT_RELEASE_MASTER(ReceiptNo, ReleaseDate,RegId, IndoorId, TotalDays, AdvanceAmt, TotalAmt, LessAmt, CollAmt, ServiceChargeAmt, ReturnAmt, BedId,  RefDrId, UnderDrId, PatientStatus, CorporateId, PackId, CashAmt, CardAmt, ChequeAmt, CardBankId, ChequeBankId, CardNumber, ChequeNumber, UserName, BranchId) OUTPUT INSERTED.ID VALUES (@ReceiptNo, @ReleaseDate,@RegId, @IndoorId, @TotalDays, @AdvanceAmt, @TotalAmt, @LessAmt, @CollAmt, @ServiceChargeAmt, @ReturnAmt, @BedId,  @RefDrId, @UnderDrId, @PatientStatus, @CorporateId, @PackId, @CashAmt, @CardAmt, @ChequeAmt, @CardBankId, @ChequeBankId, @CardNumber, @ChequeNumber, @UserName, @BranchId)";
                    var cmd = new SqlCommand(query, Con, _trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReceiptNo",refNo);
                    cmd.Parameters.AddWithValue("@ReleaseDate",DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@IndoorId",aModel.ElementAt(0).PtIndoorId);
                    cmd.Parameters.AddWithValue("@RegId", regId);
                    cmd.Parameters.AddWithValue("@TotalDays", aModel.ElementAt(0).TotalDays);
                    cmd.Parameters.AddWithValue("@AdvanceAmt", aModel.ElementAt(0).AdvanceAmt);
                    cmd.Parameters.AddWithValue("@TotalAmt", aModel.ElementAt(0).TotalAmount);
                    cmd.Parameters.AddWithValue("@LessAmt", aModel.ElementAt(0).LessAmount);
                    cmd.Parameters.AddWithValue("@CollAmt", aModel.ElementAt(0).CollAmt);
                    cmd.Parameters.AddWithValue("@ServiceChargeAmt", aModel.ElementAt(0).TotalServiceCharge);
                    cmd.Parameters.AddWithValue("@ReturnAmt", aModel.ElementAt(0).ReturnAmount);
                    cmd.Parameters.AddWithValue("@BedId", aModel.ElementAt(0).BedId);
                    cmd.Parameters.AddWithValue("@RefDrId", refDrId);
                    cmd.Parameters.AddWithValue("@UnderDrId", underDrId);
                    cmd.Parameters.AddWithValue("@PatientStatus", aModel.ElementAt(0).PatientStatus);
                    cmd.Parameters.AddWithValue("@CorporateId", aModel.ElementAt(0).CorporateId);
                    cmd.Parameters.AddWithValue("@PackId", aModel.ElementAt(0).PackageId);
                    cmd.Parameters.AddWithValue("@CashAmt", aModel.ElementAt(0).CashAmount);
                    cmd.Parameters.AddWithValue("@CardAmt", aModel.ElementAt(0).CardAmount);
                    cmd.Parameters.AddWithValue("@ChequeAmt", aModel.ElementAt(0).CheaqueAmount);
                    cmd.Parameters.AddWithValue("@CardBankId", aModel.ElementAt(0).CardBankId);
                    cmd.Parameters.AddWithValue("@ChequeBankId", aModel.ElementAt(0).CheaqueBankId);
                    cmd.Parameters.AddWithValue("@CardNumber", aModel.ElementAt(0).CardNumber);
                    cmd.Parameters.AddWithValue("@ChequeNumber", aModel.ElementAt(0).CheaqueNumber);
                    cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                    cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName,_trans));
                    var masterId=(int)cmd.ExecuteScalar();

                    _gtLedgerInsertGateway.InsertLedgerOfAdmittedPatient(refNo,regId, aModel.ElementAt(0).PtIndoorId, 85, aModel.ElementAt(0).TotalAmount, aModel.ElementAt(0).Quantity, aModel.ElementAt(0).Vat, aModel.ElementAt(0).ServiceCharge, 0, 0, 0, 0, 0, 0, 82, aModel.ElementAt(0).UserName, bedId, refDrId, admitDrId, underDrId, aModel.ElementAt(0).CashAmount, aModel.ElementAt(0).CardAmount, aModel.ElementAt(0).CheaqueAmount, aModel.ElementAt(0).CardBankId, aModel.ElementAt(0).CheaqueBankId, aModel.ElementAt(0).CardNumber, aModel.ElementAt(0).CheaqueNumber, aModel.ElementAt(0).Remarks, _trans, Con);


                

               
               
               
               
               aModel.ForEach(z => z.InvMasterId = masterId);
                    
                    DataTable dt = ConvertListDataTable(aModel);
                    var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_IN_PATIENT_RELEASE_DETAIL" };
                    objbulk.ColumnMappings.Add("InvMasterId", "ReleaseMasterId");
                    objbulk.ColumnMappings.Add("ItemId", "ItemId");
                    objbulk.ColumnMappings.Add("Charge", "Charge");
                    objbulk.ColumnMappings.Add("Quantity", "Quantity");
                    objbulk.ColumnMappings.Add("TotalCharge", "TotalCharge");
                    objbulk.ColumnMappings.Add("Vat", "VatAmt");
                    objbulk.ColumnMappings.Add("ServiceCharge", "ServiceChargeAmt");
                  //  objbulk.ColumnMappings.Add("ItemwiseLess", "LessAmt");
                    objbulk.ColumnMappings.Add("ItemTotal", "NetTotalAmt");
                    objbulk.ColumnMappings.Add("MaxRefFee", "AmtGivenToDr");
                    objbulk.ColumnMappings.Add("DrId", "DrId");
                    objbulk.WriteToServer(dt);

                 
               
               
                    foreach (var mdl in aModel)
                    {
                        if (mdl.ItemId == 2312)
                        {
                            //int invmasterId = Convert.ToInt32(ReturnFieldValueOpenCon("VW_DUE_INVOICE_LIST", "Sales=" + mdl.ItemTotal + "", "InvmasterId",_trans));

                            var lists = GetDueInvoiceListByIndoorId(mdl.PtIndoorId,_trans);
                            foreach (var list in lists)
                            {
                                if (mdl.ItemTotal>0)
                                {
                                    InsertIntoInvoiceLedger(regId, mdl.PtIndoorId, list.InvMasterId, refNo, DateTime.Now, 0, 0, list.DueAmt, 0, GetSubSubPnoByIndoorId(list.PtIndoorId,_trans), "Coll From Release", mdl.UserName, _trans);
                                    mdl.ItemTotal -= list.DueAmt;
                                }
                                
                            }
                            
                            
                            
                            
                            
                        }
                    }


                    foreach (var mdl in aModel)
                    {
                        if (mdl.DrId != 0)
                        {
                            InsertIntoHonoriumLedger(aModel.ElementAt(0).DrId, refNo, DateTime.Now, masterId, mdl.Charge * mdl.Quantity, mdl.Charge * mdl.Quantity, 0, 0, 72, mdl.UserName, _trans);
                        }
                    }





                    DeleteInsert("Update tbl_IN_PATIENT_ADMISSION SET IsRelease=1 WHERE Id='" + aModel[0].PtIndoorId + "'", _trans);
                    DeleteInsert("Update tbl_IN_BED_INFO SET IsBooked=0 WHERE Id='" + aModel[0].BedId + "'", _trans);





                    aModel.RemoveAll(r => Math.Abs(r.ItemwiseLess) < 1);
                    aModel.ForEach(z => z.RefDrId = refDrId);
                    aModel.ForEach(z => z.UnderDrId = underDrId);
                    aModel.ForEach(z => z.AdmitDrId = admitDrId);
                    aModel.ForEach(z => z.BedId = bedId);
                    aModel.ForEach(z => z.InvoiceNo = refNo);
                    aModel.ForEach(z => z.InvoiceDate = DateTime.Now);
                    aModel.ForEach(z => z.SubSubPnoId = GetSubSubPnoByIndoorId(aModel[0].PtIndoorId,_trans));

                    dt = ConvertListDataTable(aModel);
                    
                    
               
               
                    objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_IN_LEDGER_OF_ADMITTED_PATIENT" };
                    objbulk.ColumnMappings.Add("PtIndoorId", "IndoorId");
                    objbulk.ColumnMappings.Add("BedId", "BedId");
                    objbulk.ColumnMappings.Add("InvoiceNo", "RefNo");
                    objbulk.ColumnMappings.Add("InvoiceDate", "RefDate");
                    objbulk.ColumnMappings.Add("ItemId", "ItemId");
                    objbulk.ColumnMappings.Add("ItemwiseLess", "AdjustAmt");
                    objbulk.ColumnMappings.Add("UserName", "UserName");
                    objbulk.ColumnMappings.Add("SubSubPnoId", "SubSubPnoId");
                    objbulk.ColumnMappings.Add("RefDrId", "RefDrId");
                    objbulk.ColumnMappings.Add("UnderDrId", "UnderDrId");
                    objbulk.ColumnMappings.Add("AdmitDrId", "AdmitDrId");
                    objbulk.WriteToServer(dt);




              
                   
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

        private IEnumerable<InvoiceModel> GetDueInvoiceListByIndoorId(int indoorId,SqlTransaction trans)
        {
            var lists = new List<InvoiceModel>();
            string query = "SELECT InvmasterId,Due FROM  VW_DUE_INVOICE_LIST WHERE IndoorId='"+ indoorId +"'";
          //  Con.Open();
            var cmd = new SqlCommand(query, Con, trans);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new InvoiceModel()
                {
                    InvMasterId = Convert.ToInt32(rdr["InvmasterId"]),
                    DueAmt = Convert.ToDouble(rdr["Due"]),
                });
            }
            rdr.Close();
            //Con.Close();
            return lists;

        }

        internal List<PatientReleaseModel> GetClinicaldetail(int indoorId)
        {
            try
            {
                var lists = new List<PatientReleaseModel>();

//                const string query = @"SELECT a.ItemId,a.Charge,SUM(Quantity)AS Qty,SUM(a.Vat) AS vatAmt,SUM(a.ServiceCharge) AS ServiceChargeAmt,SUM(PayableAmt) AS PayableAmt,
//                                    a.HonoriumDrId,Isnull(f.Name,'N/A') AS DrName,
//                                    CASE WHEN a.ItemId=1942 then e.Description +' Of:-'+c.Description
//                                    else e.Description END  AS Description,e.IsAdjustAmt 
//                                    FROM tbl_IN_LEDGER_OF_ADMITTED_PATIENT a 
//                                    LEFT JOIN tbl_IN_PATIENT_ADMISSION b ON a.IndoorId=b.Id
//                                    LEFT JOIN tbl_IN_BED_INFO c ON a.BedId=c.Id
//                                    LEFT JOIN tbl_Patient_Registration d ON b.RegId=d.Id
//                                    LEFT JOIN tbl_CLINICAL_CHART e ON a.ItemId=e.Id
//                                    LEFT JOIN tbl_DOCTOR_INFO f ON a.HonoriumDrId=f.Id
//                                    WHERE a.ItemId<>1969 AND a.IndoorId=@indoorId
//                                    GROUP BY a.ItemId,a.Charge,e.Description,a.HonoriumDrId,f.Name,c.Description,e.IsAdjustAmt";


               const string query = @"SELECT a.ItemId,a.Charge ,
                            SUM(Quantity)AS Qty,SUM(a.Vat) AS vatAmt,SUM(a.ServiceCharge) AS ServiceChargeAmt,SUM(PayableAmt) AS PayableAmt,
                            a.HonoriumDrId,Isnull(f.Name,'N/A') AS DrName,
                            CASE WHEN a.ItemId=1942 then e.Description +' Of:-'+c.Description
                            else e.Description END  AS Description,e.IsAdjustAmt 
                            FROM tbl_IN_LEDGER_OF_ADMITTED_PATIENT a 
                            LEFT JOIN tbl_IN_PATIENT_ADMISSION b ON a.IndoorId=b.Id
                            LEFT JOIN tbl_IN_BED_INFO c ON a.BedId=c.Id
                            LEFT JOIN tbl_Patient_Registration d ON b.RegId=d.Id
                            LEFT JOIN tbl_CLINICAL_CHART e ON a.ItemId=e.Id
                            LEFT JOIN tbl_DOCTOR_INFO f ON a.HonoriumDrId=f.Id
                            WHERE a.ItemId NOT IN('2312') AND a.IndoorId=@indoorId
                            GROUP BY a.ItemId,a.Charge, e.Description,a.HonoriumDrId,f.Name,c.Description,e.IsAdjustAmt
                            UNION ALL
                            SELECT a.ItemId,SUM(a.Charge) AS Charge ,
                            1 AS Qty,SUM(a.Vat) AS vatAmt,SUM(a.ServiceCharge) AS ServiceChargeAmt,SUM(PayableAmt) AS PayableAmt,
                            a.HonoriumDrId,Isnull(f.Name,'N/A') AS DrName,
                            CASE WHEN a.ItemId=1942 then e.Description +' Of:-'+c.Description
                            else e.Description END  AS Description,e.IsAdjustAmt 
                            FROM tbl_IN_LEDGER_OF_ADMITTED_PATIENT a 
                            LEFT JOIN tbl_IN_PATIENT_ADMISSION b ON a.IndoorId=b.Id
                            LEFT JOIN tbl_IN_BED_INFO c ON a.BedId=c.Id
                            LEFT JOIN tbl_Patient_Registration d ON b.RegId=d.Id
                            LEFT JOIN tbl_CLINICAL_CHART e ON a.ItemId=e.Id
                            LEFT JOIN tbl_DOCTOR_INFO f ON a.HonoriumDrId=f.Id
                            WHERE a.ItemId IN('2312') AND a.IndoorId=@indoorId
                            GROUP BY a.ItemId,e.Description,a.HonoriumDrId,f.Name,c.Description,e.IsAdjustAmt";
                
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@indoorId", indoorId);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PatientReleaseModel()
                    {
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        Charge = Convert.ToDouble(rdr["Charge"]),
                        Quantity = Convert.ToInt32(rdr["Qty"]),
                        Vat = Convert.ToDouble(rdr["vatAmt"]),
                        ServiceCharge = Convert.ToDouble(rdr["ServiceChargeAmt"]),
                        BalAmt = Convert.ToDouble(rdr["PayableAmt"]),
                        Description = rdr["Description"].ToString(),
                        RefDrName = rdr["DrName"].ToString(),
                        DrId =  Convert.ToInt32(rdr["HonoriumDrId"]),
                        IsAdjustAmtId =   Convert.ToInt32(rdr["IsAdjustAmt"]),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception eException)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                throw;
            }
        }

        internal object GetIndoorPatientReleaseList()
        {
            try
            {
                var lists = new List<PatientReleaseModel>();
                const string query = @"SELECT * FROM VW_IN_PATIENT_RELEASE_LIST";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PatientReleaseModel()
                    {
                        ReleaseId = Convert.ToInt32(rdr["Id"]),
                        ReleaseDate = Convert.ToDateTime(rdr["ReleaseDate"]),
                        PatientId = rdr["PatientId"].ToString(),
                        TotalAmount = Convert.ToDouble(rdr["TotalAmt"]),
                        AdmissionDate = Convert.ToDateTime(rdr["AdmissionDate"]),
                        BedNo = rdr["BedNo"].ToString(),
                        PtName = rdr["PtName"].ToString(),
                        PtMobileNo = rdr["PtMobileNo"].ToString(),
                        PtAddress = rdr["Address"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception eException)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                throw;
            }
        }

        internal List<PatientReleaseModel> GetCurrentPatientDue(int indoorId)
        {
            try
            {
                var lists = new List<PatientReleaseModel>();
                const string query = @"SELECT * FROM VW_GET_CURRENT_PATIENT_DUE WHERE IndoorId=@IndoorId";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@IndoorId", indoorId);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new PatientReleaseModel()
                    {
                        InvoiceNo = rdr["RefNo"].ToString(),
                        ReleaseDate = Convert.ToDateTime(rdr["RefDate"]),
                        ItemId = Convert.ToInt32(rdr["ItemId"]),
                        Charge = Convert.ToDouble(rdr["Charge"]),
                        Quantity = Convert.ToInt32(rdr["Quantity"]),
                        Vat =Convert.ToDouble(rdr["Vat"]),
                        ServiceCharge = Convert.ToDouble(rdr["ServiceCharge"]),
                        PayableAmount = Convert.ToDouble(rdr["PayableAmt"]),
                        CollAmt = Convert.ToDouble(rdr["CollAmt"]),
                        LessAmount = Convert.ToDouble(rdr["LessAmt"]),
                        ReturnAmount = Convert.ToDouble(rdr["RtnAmt"]),
                        AdjustAmount = Convert.ToDouble(rdr["AdjustAmt"]),
                        Description = rdr["Description"].ToString(),
                        PatientId = rdr["PatientId"].ToString(),
                        AdmissionDate = Convert.ToDateTime(rdr["AdmissionDate"]),
                        BedNo = rdr["BedNo"].ToString(),
                        PtName = rdr["Name"].ToString(),
                        PtMobileNo = rdr["MobileNo"].ToString(),
                        PtAddress = rdr["Address"].ToString(),
                        FloorNo = rdr["FloorNo"].ToString(),
                        RoomNo = rdr["RoomNo"].ToString(),
                        DeptName = rdr["DeptName"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception eException)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                throw;
            }
        }
    }
}