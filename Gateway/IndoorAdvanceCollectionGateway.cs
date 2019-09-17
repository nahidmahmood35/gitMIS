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
    public class IndoorAdvanceCollectionGateway:DbConnection
    {
        private SqlTransaction _trans;
        readonly LedgerInsertGateway _insertGateway=new LedgerInsertGateway();
        public string Save(AdmissionModel aModel)
        {
            try
            {
                const string msg = ""; 
                Con.Open();
                _trans = Con.BeginTransaction();
                string refNo = "AC" + GetTrNoWithOpenCon("RefNo", "tbl_IN_LEDGER_OF_ADMITTED_PATIENT", _trans);
                _insertGateway.InsertLedgerOfAdmittedPatient(refNo,Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.PtIndoorId + "", "RegId",_trans)), aModel.PtIndoorId, 1969, aModel.TotalAmount, 1, 0, 0, 0, aModel.TotalAmount, 0, 0, 0, 0, GetSubSubPnoByIndoorId(aModel.PtIndoorId,_trans), aModel.UserName, Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.PtIndoorId + "", "BedId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.PtIndoorId + "", "RefDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.PtIndoorId + "", "AdmitDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + aModel.PtIndoorId + "", "UnderDrId", _trans)), aModel.CashAmount, aModel.CardAmount, aModel.CheaqueAmount, aModel.CardBankId, aModel.CheaqueBankId, aModel.CardNumber, aModel.CheaqueNumber, aModel.Remarks, _trans, Con);
                _trans.Commit();
                Con.Close();
                return "Save uccess";
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                    _trans.Rollback();
                }
                return exception.Message;
            }
        }

        internal List<AdmissionModel> GetAdvanceCollectionList()
        {
            try
            {
                var list = new List<AdmissionModel>();
                const string query =@"SELECT a.Id, a.IndoorId,b.PatientId,b.Admissiondate,c.Name,c.MobileNo,a.BedId,d.RoomNo,d.FloorNo,d.Description,a.RefNo,a.RefDate,a.Charge,e.SubPno AS DeptName 
                                FROM tbl_IN_LEDGER_OF_ADMITTED_PATIENT a 
                                LEFT JOIN tbl_IN_PATIENT_ADMISSION b ON a.IndoorId=b.Id
                                LEFT JOIN tbl_PATIENT_REGISTRATION c ON b.RegId=c.Id
                                LEFT JOIN tbl_IN_BED_INFO d ON a.BedId=d.Id
                                LEFT JOIN Project e ON d.DeptId=e.IdNo
                                WHERE a.ItemId=1969";
                Con.Open();
                var cmd = new SqlCommand(query,Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new AdmissionModel()
                    {
                        Id =  Convert.ToInt32(rdr["Id"]),
                        PtIndoorId = Convert.ToInt32(rdr["IndoorId"]),
                        PatientId = rdr["PatientId"].ToString(),
                        AdmissionDate =Convert.ToDateTime(rdr["Admissiondate"]),
                        Name = rdr["Name"].ToString(),
                        MobileNo = rdr["MobileNo"].ToString(),
                        RoomNo = rdr["RoomNo"].ToString(),
                        FloorNo = rdr["FloorNo"].ToString(),
                        Description = rdr["Description"].ToString(),
                        InvoiceNo = rdr["RefNo"].ToString(),
                        InvoiceDate =Convert.ToDateTime(rdr["RefDate"]),
                        DeptName = rdr["DeptName"].ToString(),
                        Charge = Convert.ToDouble(rdr["Charge"]),
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
                throw;
            }
        }
    }
}