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
    public class AdmissionGateway:DbConnection
    {
        private SqlTransaction _trans;
        readonly LedgerInsertGateway _aLedgerInsertGateway=new LedgerInsertGateway();
        public Task<string> Save(AdmissionModel aModel)
        {
            try
            {
                const string msg = ""; //Image
                
                
                Con.Open();                
                string ptId ="P"+ GetTrNoWithOpenCon("PatientId","tbl_IN_PATIENT_ADMISSION",_trans);
                _trans = Con.BeginTransaction();

                const string query = @"INSERT INTO tbl_IN_PATIENT_ADMISSION (PatientId, RegId,  AdmissionDate, AdmissionTime, BedId, BedCharge,  AdmissionCharge, ContractAmount, R_Name_1, Relation_1, R_Phone_1, R_Name_2, Relation_2, R_Phone_2, RefDrId, AdmitDrId, UnderDrId, MpoId, UserName, BranchId,Age,SubSubPnoId) 
                             OUTPUT INSERTED.ID VALUES  (@PatientId, @RegId,   @AdmissionDate, @AdmissionTime, @BedId, @BedCharge,  @AdmissionCharge, @ContractAmount, @R_Name_1, @Relation_1, @R_Phone_1, @R_Name_2, @Relation_2, @R_Phone_2, @RefDrId, @AdmitDrId, @UnderDrId, @MpoId, @UserName, @BranchId,@Age,@SubSubPnoId)";
              
                var cmd = new SqlCommand(query, Con,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PatientId", ptId);
                cmd.Parameters.AddWithValue("@RegId", aModel.PtRegId);
                cmd.Parameters.AddWithValue("@AdmissionDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@AdmissionTime", DateTime.Now.ToString("h:mm:ss tt"));
                cmd.Parameters.AddWithValue("@BedId", aModel.BedId);
                cmd.Parameters.AddWithValue("@BedCharge", aModel.BedCharge);
                cmd.Parameters.AddWithValue("@AdmissionCharge", aModel.AdmissionCharge);
                cmd.Parameters.AddWithValue("@ContractAmount", aModel.ContractAmount);
                cmd.Parameters.AddWithValue("@R_Name_1", aModel.RName1);
                cmd.Parameters.AddWithValue("@Relation_1", aModel.Relation1);
                cmd.Parameters.AddWithValue("@R_Phone_1", aModel.RPhone1);
                cmd.Parameters.AddWithValue("@R_Name_2", aModel.RName2);
                cmd.Parameters.AddWithValue("@Relation_2", aModel.Relation2);
                cmd.Parameters.AddWithValue("@R_Phone_2", aModel.RPhone2);
                cmd.Parameters.AddWithValue("@RefDrId", aModel.RefDrId);
                cmd.Parameters.AddWithValue("@AdmitDrId", aModel.AdmitDrId);
                cmd.Parameters.AddWithValue("@UnderDrId", aModel.UnderDrId);
                cmd.Parameters.AddWithValue("@MpoId", aModel.MioId);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@BranchId", 1);//System.Web.HttpContext.Current.Session["BranchId"]);
                cmd.Parameters.AddWithValue("@Age", aModel.PtAgeDetail);
                cmd.Parameters.AddWithValue("@SubSubPnoId", aModel.SubSubPnoId);


                var ptIndoorId=(int)cmd.ExecuteScalar();
                
                int regId = Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptIndoorId + "", "RegId", _trans));
                //int subSubPnoId =Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_BED_INFO", "Id=" + aModel.BedId + "", "DeptId", _trans));
                
                _aLedgerInsertGateway.InsertLedgerOfAdmittedPatient(ptId, regId, ptIndoorId, 1941, aModel.AdmissionCharge, 1, 0, 0, aModel.AdmissionCharge, 0, 0, 0, 0, 0, aModel.SubSubPnoId, aModel.UserName, aModel.BedId, Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptIndoorId + "", "RefDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptIndoorId + "", "AdmitDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptIndoorId + "", "UnderDrId", _trans)), 0, 0, 0, 0, 0, "N/A", "N/A", "N/A", _trans, Con);
                _aLedgerInsertGateway.InsertLedgerOfAdmittedPatient(ptId,regId, ptIndoorId, 1942, aModel.BedCharge, 1, 0, 0, aModel.BedCharge, 0, 0, 0, 0, 0, aModel.SubSubPnoId, aModel.UserName, aModel.BedId, Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptIndoorId + "", "RefDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptIndoorId + "", "AdmitDrId", _trans)), Convert.ToInt32(ReturnFieldValueOpenCon("tbl_IN_PATIENT_ADMISSION", "Id=" + ptIndoorId + "", "UnderDrId", _trans)), 0, 0, 0, 0, 0, "N/A", "N/A", "Bed Charge Of : " + ReturnFieldValueOpenCon("tbl_IN_BED_INFO", "Id=" + aModel.BedId + "", "Description", _trans), _trans, Con);
                _trans.Commit();
                Con.Close();
                DeleteInsert("Update tbl_IN_BED_INFO SET IsBooked=1 WHERE Id=" + aModel.BedId + "");
               
                return Task.FromResult<string>("Save uccess");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                    _trans.Rollback();
                }
                return Task.FromResult(exception.Message);
            }
        }
       
        internal List<ProjectModel> GetIndoorDepartmentListByPno(int deptId)
        {
            try
            {
                string condition = "";
                var list = new List<ProjectModel>();
                string query = "";
                if (deptId != 0)
                {
                    condition = "AND IdNo=@Param ";
                }
                query = @"SELECT IdNo,SubSubPno,AdmissionCharge From Project WHERE Pno='Indoor' " + condition + "";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", deptId);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ProjectModel()
                    {
                        Title = rdr["SubSubPno"].ToString(),
                        ProjectId = Convert.ToInt32(rdr["IdNo"]),
                        AdmCharge = Convert.ToDouble(rdr["AdmissionCharge"]),
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
                throw ;
            }
        }

        internal List<IdNameForDropdownModel> GetIndoorPackageList()
        {
            try
            {
                
                var list = new List<IdNameForDropdownModel>();
                string query = "";

                query = @"SELECT Id,Name From tbl_PACKAGE_INFO_MST WHERE SubSubPnoId=72";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
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

        internal List<AdmissionModel> GetAdmittedPatientList(int id, string searchString)
        {
            try
            {
                string query = "", cond = "";
                var list = new List<AdmissionModel>();

               
                if (id != 0)
                {
                    cond = "WHERE Id=@Id";
                }
                if (searchString != "0")
                {
                    if (id!= 0)
                    {
                        cond += "AND (PtName+PtMobileNo) like '%'+'" + searchString + "'+'%'";
                    }
                    else
                    {
                        cond += "WHERE (PtName+PtMobileNo) like '%'+'" + searchString + "'+'%'";
                    }
                }





                query = @"SELECT * From VW_IN_ADMISSION_LIST "+ cond +"  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new AdmissionModel()
                    {
                        PtIndoorId = Convert.ToInt32(rdr["Id"]),
                        PatientId = rdr["PatientId"].ToString(),
                        PtAgeDetail = rdr["Age"].ToString(),
                        PtRegId = Convert.ToInt32(rdr["RegId"]),
                        AdmissionDate = Convert.ToDateTime(rdr["AdmissionDate"]),
                        PtRegNo = rdr["PtRegNo"].ToString(),
                        AdmissionTime = rdr["AdmissionTime"].ToString(),
                        BedId = Convert.ToInt32(rdr["BedId"]),
                        RName1 = rdr["R_Name_1"].ToString(),
                        RName2 = rdr["R_Name_2"].ToString(),
                        Relation1 = rdr["Relation_1"].ToString(),
                        Relation2 = rdr["Relation_2"].ToString(),
                        RPhone1 = rdr["R_Phone_1"].ToString(),
                        RPhone2 = rdr["R_Phone_2"].ToString(),

                        RefDrId = Convert.ToInt32(rdr["RefDrId"]),
                        UnderDrId = Convert.ToInt32(rdr["UnderDrId"]),
                        AdmitDrId = Convert.ToInt32(rdr["AdmitDrId"]),


                        RefDrName = rdr["RefDrName"].ToString(),
                        UnderDrName = rdr["UnderDrName"].ToString(),
                        AdmitDrName = rdr["AdmitDrName"].ToString(),

                        PtName = rdr["PtName"].ToString(),
                        PtMobileNo = rdr["PtMobileNo"].ToString(),
                        PtFatherName = rdr["FatherName"].ToString(),
                        PtMotherName = rdr["MotherName"].ToString(),
                        SpouseName = rdr["SpouseName"].ToString(),
                        PtGendeId = Convert.ToInt32(rdr["GenderId"]),
                        PtGenderName = rdr["GenderName"].ToString(),

                        PtOccupation = rdr["Occupation"].ToString(),
                        PtReligionId = Convert.ToInt32(rdr["ReligionId"]),
                        PtReligionName = rdr["ReligionName"].ToString(),
                        PtAddress = rdr["Address"].ToString(),
                        FloorNo = rdr["FloorNo"].ToString(),
                        BedNo = rdr["BedNo"].ToString(),
                        NoOfCount = Convert.ToInt32(rdr["NoOfDays"]),

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