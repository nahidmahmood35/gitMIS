using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class IndoorProcedureEntryGateway:DbConnection
    {
        SqlTransaction _trans;
        readonly LedgerInsertGateway _ledgerInsertGateway=new LedgerInsertGateway();
        internal Task<string> Save(List<AdmissionModel> aModel)
        {
            try
            {
                Con.Open();
                _trans = Con.BeginTransaction();
                string refNo ="AP"+ GetTrNoWithOpenCon("RefNo", "tbl_IN_LEDGER_OF_ADMITTED_PATIENT", _trans);
                int regId =Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel[0].PtIndoorId + "","RegId", _trans));
                int subsubPnoId = GetSubSubPnoByIndoorId(aModel[0].PtIndoorId, _trans);
                foreach (var adM in aModel)
                {
                    _ledgerInsertGateway.InsertLedgerOfAdmittedPatient(refNo, regId, adM.PtIndoorId, adM.ItemId, adM.Charge, adM.Quantity, adM.Vat, adM.ServiceCharge, adM.Charge * adM.Quantity + adM.Vat + adM.ServiceCharge, 0, 0, 0, 0, adM.DrId, subsubPnoId, adM.UserName, Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + adM.PtIndoorId + "", "BedId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + adM.PtIndoorId + "", "RefDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + adM.PtIndoorId + "", "AdmitDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + adM.PtIndoorId + "", "UnderDrId", _trans)), 0, 0, 0, 0, 0, "N/A", "N/A", adM.Remarks, _trans, Con);
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

        internal List<AdmissionModel> GetProcedureEntryList()
        {
            try
            {
                var lists = new List<AdmissionModel>();
                const string query = "SELECT Distinct RefNo,RefDate,Patientid,Name,Address,MobileNo FROM GET_IN_PROCEDURE_ENTRY_LIST  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new AdmissionModel()
                    {
                        InvoiceNo = rdr["RefNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["RefDate"]),
                        PatientId = rdr["PatientId"].ToString(),
                        Name = rdr["Name"].ToString(),
                        PtAddress = rdr["Address"].ToString(),
                        PtMobileNo = rdr["MobileNo"].ToString(),
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
                    _trans.Rollback();
                    Con.Close();
                }
                throw ;
            }
        }
    }
}