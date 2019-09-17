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
    public class PatientGateway : DbConnection
    {
        public Task<string> Save(PatientModel aModel)
        {
            try
            {
                const string msg = ""; //Image
                string regNo = GetAutoIncrementNumberFromStoreProcedure(1);

                const string query = @"INSERT INTO tbl_PATIENT_REGISTRATION (PtRegNo, Name, MobileNo, DateOfBirth, GenderId, FatherName, MotherName, SpouseName, Address, ReligionId, Area, Occupation, BloodGroupId, NationalIdNo, PassportNo, IntroducerId, IntroducerName, BranchId, UserName) 
                             VALUES (@PtRegNo, @Name, @MobileNo, @DateOfBirth, @GenderId, @FatherName, @MotherName, @SpouseName, @Address, @ReligionId, @Area, @Occupation, @BloodGroupId, @NationalIdNo, @PassportNo, @IntroducerId, @IntroducerName, @BranchId, @UserName)";
                Con.Open();
                
                var cmd = new SqlCommand(query,Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PtRegNo", regNo);
                cmd.Parameters.AddWithValue("@Name", aModel.PtName);
                cmd.Parameters.AddWithValue("@MobileNo",aModel.PtMobileNo );
                cmd.Parameters.AddWithValue("@DateOfBirth", aModel.PtDob.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@GenderId", aModel.PtGendeId);
                cmd.Parameters.AddWithValue("@FatherName", aModel.PtFatherName);
                cmd.Parameters.AddWithValue("@MotherName", aModel.PtMotherName);
                cmd.Parameters.AddWithValue("@SpouseName", aModel.PtSpooseName);
                cmd.Parameters.AddWithValue("@Address", aModel.PtAddress);
                cmd.Parameters.AddWithValue("@ReligionId", aModel.PtReligionId);
                cmd.Parameters.AddWithValue("@Occupation", aModel.PtOccupation);
                cmd.Parameters.AddWithValue("@BloodGroupId", aModel.PtBloodGroupId);
                cmd.Parameters.AddWithValue("@NationalIdNo", aModel.PtNationalIdNo );
                cmd.Parameters.AddWithValue("@PassportNo", aModel.PtPassportNo );
                cmd.Parameters.AddWithValue("@IntroducerId", aModel.PtIntroducerId);
                cmd.Parameters.AddWithValue("@IntroducerName", aModel.PtIntroducerName);
                cmd.Parameters.AddWithValue("@Area", aModel.PtArea);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@BranchId", ReturnFieldValueOpenCon("tbl_USER_BRANCH_INFO", "UserName='" + aModel.UserName + "'", "BranchId"));//System.Web.HttpContext.Current.Session["BranchId"]);
                
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Save uccess");
            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }
        }

        public Task<string> Update(PatientModel aModel)
        {
            try
            {
                const string query = @"UPDATE tbl_PATIENT_REGISTRATION SET  Name=@Name, MobileNo=@MobileNo, DateOfBirth=@DateOfBirth, GenderId=@GenderId, FatherName=@FatherName, MotherName=@MotherName, SpouseName=@SpouseName, Address=@Address, ReligionId=@ReligionId, Area=@Area, Occupation=@Occupation, BloodGroupId=@BloodGroupId, NationalIdNo=@NationalIdNo, PassportNo=@PassportNo, IntroducerId=@IntroducerId, IntroducerName=@IntroducerName, BranchId=@BranchId, UserName=@UserName WHERE Id=@Id ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", aModel.PtRegId);
                cmd.Parameters.AddWithValue("@Name", aModel.PtName);
                cmd.Parameters.AddWithValue("@MobileNo", aModel.PtMobileNo);
                cmd.Parameters.AddWithValue("@DateOfBirth", aModel.PtDob.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@GenderId", aModel.PtGendeId);
                cmd.Parameters.AddWithValue("@FatherName", aModel.PtFatherName);
                cmd.Parameters.AddWithValue("@MotherName", aModel.PtMotherName);
                cmd.Parameters.AddWithValue("@SpouseName", aModel.PtSpooseName);
                cmd.Parameters.AddWithValue("@Address", aModel.PtAddress);
                cmd.Parameters.AddWithValue("@ReligionId", aModel.PtReligionId);
                cmd.Parameters.AddWithValue("@Occupation", aModel.PtOccupation);
                cmd.Parameters.AddWithValue("@BloodGroupId", aModel.PtBloodGroupId);
                cmd.Parameters.AddWithValue("@NationalIdNo", aModel.PtNationalIdNo);
                cmd.Parameters.AddWithValue("@PassportNo", aModel.PtPassportNo);
                cmd.Parameters.AddWithValue("@IntroducerId", aModel.PtIntroducerId);
                cmd.Parameters.AddWithValue("@IntroducerName", aModel.PtIntroducerName);
                cmd.Parameters.AddWithValue("@Area", aModel.PtArea);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@BranchId", ReturnFieldValueOpenCon("tbl_USER_BRANCH_INFO", "UserName='" + aModel.UserName + "'", "BranchId"));//System.Web.HttpContext.Current.Session["BranchId"]);
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Update success");
            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }
        }

        public List<PatientModel> GetPatientlList(string searchString)
        {
            try
            {
                string msg = "",condition="";
                var list = new List<PatientModel>();
                string query = "";
                if (searchString != "0") { condition = "WHERE (a.Name+a.MobileNo+a.Address+a.PtRegNo) LIKE '%' + '" + searchString + "' + '%' "; }
                query = @"SELECT Top 100 a.Id, a.PtRegNo, a.Name, a.MobileNo, a.DateOfBirth, a.GenderId,b.Name AS GenderName, a.FatherName, a.MotherName, a.SpouseName, a.Address, a.ReligionId, a.Area, a.Occupation, a.BloodGroupId, a.NationalIdNo, a.PassportNo, a.IntroducerId, a.IntroducerName  From tbl_PATIENT_REGISTRATION a LEFT JOIN tbl_GENDER_INFO_MST b ON a.GenderId=b.Id   " + condition + "";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", searchString);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                list.Add(new PatientModel()
                {
                    PtRegNo = rdr["PtRegNo"].ToString(),
                    PtRegId = Convert.ToInt32(rdr["Id"]),
                    PtName = rdr["Name"].ToString(),
                    PtMobileNo = rdr["MobileNo"].ToString(),
                    PtDob = Convert.ToDateTime(rdr["DateOfBirth"]),
                    PtGendeId =Convert.ToInt32(rdr["GenderId"]),
                    PtGenderName = rdr["GenderName"].ToString(),
                    PtFatherName = rdr["FatherName"].ToString(),
                    PtMotherName = rdr["MotherName"].ToString(),
                    PtSpooseName = rdr["SpouseName"].ToString(),
                    PtAddress = rdr["Address"].ToString(),
                    PtReligionId = Convert.ToInt32(rdr["ReligionId"]),
                    PtArea = rdr["Area"].ToString(),
                    PtBloodGroupId = Convert.ToInt32(rdr["BloodGroupId"]),
                    PtOccupation = rdr["Occupation"].ToString(),
                    PtPassportNo = rdr["PassportNo"].ToString(),
                    PtIntroducerId =Convert.ToInt32(rdr["IntroducerId"].ToString()),
                    PtNationalIdNo = rdr["NationalIdNo"].ToString(),
                    PtIntroducerName = rdr["IntroducerName"].ToString(),
                });
                }
                rdr.Close();
                Con.Close();



                foreach (var patientModel in list)
                {
                    patientModel.PtAgeDetail = GetCurrentAgeOfaPatient(patientModel.PtDob.ToString("yyyy-MM-dd"));
                }
                return list;
            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<PatientModel>();
            }
        }

        public string Delete(int Id)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM PatientRegistration WHERE PtRegNo=@PtId";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PtId", Id);
                int rtn = cmd.ExecuteNonQuery();
                msg=rtn==1?  "Delete Success" : "Delete Failed";
                Con.Close();
                return msg;
            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                return exception.ToString();
            }
        }

        
    }
}