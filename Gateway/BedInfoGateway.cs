using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Antlr.Runtime.Misc;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class BedInfoGateway : DbConnection
    {
        public Task<string> Save(BedModel aModel)
        {
            try
            {
                const string query = @"INSERT INTO tbl_IN_BED_INFO (RoomNo, FloorNo, Description, TypeOfBedId, DeptId, Charge, ServiceCharge, ServiceChargePCOrTk, BedStatus, UserName, BranchId,AdmissionCharge) 
										VALUES (@RoomNo, @FloorNo, @Description, @TypeOfBedId, @DeptId, @Charge, @ServiceCharge, @ServiceChargePCOrTk, @BedStatus, @UserName, @BranchId,@AdmissionCharge)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@RoomNo", aModel.RoomNo);
                cmd.Parameters.AddWithValue("@FloorNo", aModel.FloorNo);
                cmd.Parameters.AddWithValue("@Description", aModel.Description);
                cmd.Parameters.AddWithValue("@TypeOfBedId", aModel.TypeOfBedId);
                cmd.Parameters.AddWithValue("@DeptId", aModel.DeptId);
                cmd.Parameters.AddWithValue("@Charge", aModel.Charge);
                cmd.Parameters.AddWithValue("@ServiceCharge", aModel.ServiceCharge);
                cmd.Parameters.AddWithValue("@ServiceChargePCOrTk", aModel.ServiceChargePcOrTk);
                cmd.Parameters.AddWithValue("@BedStatus", aModel.BedStatus);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.UserName));
                cmd.Parameters.AddWithValue("@AdmissionCharge", aModel.AdmissionCharge);

                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult("Save Successful");

            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.ToString());
            }

        }
        public Task<string> Update(BedModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"UPDATE tbl_IN_BED_INFO  SET (RoomNo=@RoomNo, FloorNo=@FloorNo, Description=@Description, TyepOfBed=@TyepOfBed, DeptId=@DeptId, Charge=@Charge, ServiceCharge=@ServiceCharge, ServiceChargePCOrTk=@ServiceChargePCOrTk, BedStatus=@BedStatus, UserName=@UserName, BranchId=@BranchId,AdmissionCharge=@AdmissionCharge WHERE Id=@BedId";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@BedId", aModel.BedId);
                cmd.Parameters.AddWithValue("@RoomNo", aModel.RoomNo);
                cmd.Parameters.AddWithValue("@FloorNo", aModel.FloorNo);
                cmd.Parameters.AddWithValue("@Description", aModel.Description);
                cmd.Parameters.AddWithValue("@TypeOfBed", aModel.TypeOfBedId);
                cmd.Parameters.AddWithValue("@DeptId", aModel.DeptId);
                cmd.Parameters.AddWithValue("@Charge", aModel.Charge);
                cmd.Parameters.AddWithValue("@ServiceCharge", aModel.ServiceCharge);
                cmd.Parameters.AddWithValue("@ServiceChargePCOrTk", aModel.ServiceChargePcOrTk);
                cmd.Parameters.AddWithValue("@BedStatus", aModel.BedStatus);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserName(aModel.UserName));
                cmd.Parameters.AddWithValue("@AdmissionCharge", aModel.AdmissionCharge);
                cmd.ExecuteNonQuery();
                Con.Close();
               return  Task.FromResult("Update Successful");
            }

            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.ToString());
            }

        }
        public List<BedModel> GetAvailabeBedList(string searchString)
        {
            try
            {
                var list = new List<BedModel>();
                string query = "";
                query = @"SELECT * FROM tbl_IN_BED_INFO WHERE  (Description+RoomNo+FloorNo) LIKE '%' + @param + '%' AND IsBooked=0";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@param", searchString);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new BedModel()
                        {
                            BedId = Convert.ToInt32(rdr["Id"]),
                            RoomNo = rdr["RoomNo"].ToString(),
                            FloorNo = rdr["FloorNo"].ToString(),
                            TypeOfBedId = Convert.ToInt32(rdr["TypeOfBedId"]),
                            DeptId =Convert.ToInt32(rdr["DeptId"].ToString()),
                            Description = rdr["Description"].ToString(),
                            Charge = Convert.ToDouble(rdr["Charge"]),
                            ServiceCharge = Convert.ToDouble(rdr["ServiceCharge"]),
                            ServiceChargePcOrTk= rdr["ServiceChargePCOrTk"].ToString(),
                            BedStatus = Convert.ToInt32(rdr["BedStatus"]),
                            AdmissionCharge = Convert.ToDouble(rdr["AdmissionCharge"]),
                        }
                        );
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
                return null;
            }            
        }
        public List<BedModel> GetAllBedList(string searchString)
        {
            try
            {
                var list = new List<BedModel>();
                string query = "";
                query = @"SELECT * FROM tbl_IN_BED_INFO where (Description+RoomNo+FloorNo)  LIKE '%' + @param + '%'";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@param",searchString);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new BedModel()
                    {
                        BedId = Convert.ToInt32(rdr["Id"]),
                        RoomNo = rdr["RoomNo"].ToString(),
                        FloorNo = rdr["FloorNo"].ToString(),
                        TypeOfBedId = Convert.ToInt32(rdr["TypeOfBedId"]),
                        DeptId = Convert.ToInt32(rdr["DeptId"].ToString()),
                        Description = rdr["Description"].ToString(),
                        Charge = Convert.ToDouble(rdr["Charge"]),
                        ServiceCharge = Convert.ToDouble(rdr["ServiceCharge"]),
                        ServiceChargePcOrTk = rdr["ServiceChargePCOrTk"].ToString(),
                        BedStatus = Convert.ToInt32(rdr["BedStatus"]),
                        AdmissionCharge = Convert.ToDouble(rdr["AdmissionCharge"]),
                    }
                        );
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
                return null;
            }
        }



        #region For Bed Change

        internal List<BedModel> GetBedInfoFromLedgerCountFromLedgerByPtId(int ptId)
        {
            try
            {
                var list = new List<BedModel>();
                string query = "";
                query = @"SELECT a.BedId,b.Description AS BedNo,c.Name As BedType,b.FloorNo,b.RoomNo,COUNT(BedId)AS NoOfDay
                        FROM tbl_IN_LEDGER_OF_ADMITTED_PATIENT  a 
                        LEFT JOIN tbl_IN_BED_INFO b ON a.BedId=b.Id
                        LEFT JOIN tbl_IN_TYPES_OF_BED c ON b.TypeOfBedId=c.Id WHERE a.ItemId=1942 AND IndoorId=@ptId GROUP BY a.BedId,b.Description,c.Name,b.FloorNo,b.RoomNo";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ptId", ptId);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new BedModel()
                    {
                        BedId = Convert.ToInt32(rdr["BedId"]),
                        RoomNo = rdr["RoomNo"].ToString(),
                        FloorNo = rdr["FloorNo"].ToString(),
                        TypeOfBedName = rdr["BedType"].ToString(),
                        BedNo = rdr["BedNo"].ToString(),
                        NoOfCount = Convert.ToInt32(rdr["NoOfDay"]),
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
                return null;
            } 
        }

        private SqlTransaction _trans;
        readonly LedgerInsertGateway _insertGateway=new LedgerInsertGateway();
        internal string SaveBedChange(int ptId, double bedCharge, int bedId,string userName,string remarks)
        {
            try
            {
                int prevBedId =Convert.ToInt32(ReturnFieldValue("tbl_IN_PATIENT_ADMISSION", "Id=" + ptId + "", "BedId"));
                Con.Open();
                _trans = Con.BeginTransaction();
                _insertGateway.InsertLedgerOfAdmittedPatient("BC"+GetTrNoWithOpenCon("RefNo","tbl_IN_LEDGER_OF_ADMITTED_PATIENT",_trans),Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptId + "","RegId", _trans)), ptId, 1942, bedCharge, 1, 0, 0, bedCharge, 0, 0, 0, 0,  0,80,  userName, bedId, Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptId + "", "RefDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptId + "", "AdmitDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptId + "", "UnderDrId", _trans)),0,0,0,0,0,"N/A","N/A",remarks, _trans, Con);
                    DeleteInsert("Update tbl_IN_PATIENT_ADMISSION SET BedId=" + bedId + " WHERE Id=" + ptId + "",_trans);
                    DeleteInsert("Update tbl_IN_BED_INFO SET IsBooked=0 WHERE Id=" + prevBedId + "", _trans);
                    DeleteInsert("Update tbl_IN_BED_INFO SET IsBooked=1 WHERE Id=" + bedId + "", _trans);
                _trans.Commit();
                Con.Close();
               
                return "Update Successful";
            }

            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                return exception.ToString();
            }
        }
        #endregion
    }
}