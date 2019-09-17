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
    public class DoctorGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(List<DoctorModel> aModel)
        {
            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                const string query = @"INSERT INTO tbl_DOCTOR_INFO (Code,Name,Category, Gender, Degree, Type, Speciality, DepartmentId, PresentAddress, Area, 
                                        Thana, Zilla, MobileNo, Email, DateOfBirth, SpouseName, SpouseDOB, MarriageDate, TakeComision, RefType, Availability, 
                                        MIOId, PrescriptionStatus, NameBangla, Details, VisitNewPatient, VisitOldPatient, NoOfPatientPerDay, UserName,  BranchId, EntryTime) 
                                        OUTPUT INSERTED.ID VALUES ( @Code,@Name,@Category, @Gender, @Degree, @Type, @Speciality, @DepartmentId, @PresentAddress, @Area, @Thana, @Zilla, 
                                        @MobileNo, @Email, @DateOfBirth, @SpouseName, @SpouseDOB, @MarriageDate, @TakeComision, @RefType, @Availability, @MIOId, 
                                        @PrescriptionStatus, @NameBangla, @Details, @VisitNewPatient, @VisitOldPatient, @NoOfPatientPerDay, @UserName,@BranchId, @EntryTime) ";

              //  Con.Open();
                var cmd = new SqlCommand(query, Con,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Code", aModel.ElementAt(0).Code);
                cmd.Parameters.AddWithValue("@Name", aModel.ElementAt(0).Name);
                cmd.Parameters.AddWithValue("@Category", aModel.ElementAt(0).Category);
                cmd.Parameters.AddWithValue("@Gender", aModel.ElementAt(0).Gender);
                cmd.Parameters.AddWithValue("@Degree", aModel.ElementAt(0).Degree);
                cmd.Parameters.AddWithValue("@Type", aModel.ElementAt(0).Type);
                cmd.Parameters.AddWithValue("@Speciality", aModel.ElementAt(0).Speciality);
                cmd.Parameters.AddWithValue("@DepartmentId", aModel.ElementAt(0).DepartmentId);
                cmd.Parameters.AddWithValue("@PresentAddress", aModel.ElementAt(0).PresentAddress);
                cmd.Parameters.AddWithValue("@Area", aModel.ElementAt(0).Area);
                cmd.Parameters.AddWithValue("@Thana", aModel.ElementAt(0).Thana);
                cmd.Parameters.AddWithValue("@Zilla", aModel.ElementAt(0).Zilla);
                cmd.Parameters.AddWithValue("@MobileNo", aModel.ElementAt(0).MobileNo);
                cmd.Parameters.AddWithValue("@Email", aModel.ElementAt(0).Email);
                cmd.Parameters.AddWithValue("@DateOfBirth", aModel.ElementAt(0).DateOfBirth.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SpouseName", aModel.ElementAt(0).SpouseName);
                cmd.Parameters.AddWithValue("@SpouseDOB", aModel.ElementAt(0).SpouseDob.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MarriageDate", aModel.ElementAt(0).MarriageDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@TakeComision", aModel.ElementAt(0).TakeComisionId);
                cmd.Parameters.AddWithValue("@RefType", aModel.ElementAt(0).RefType);
                cmd.Parameters.AddWithValue("@Availability", aModel.ElementAt(0).Availability);
                cmd.Parameters.AddWithValue("@MIOId", aModel.ElementAt(0).MioId);
                cmd.Parameters.AddWithValue("@PrescriptionStatus", aModel.ElementAt(0).PrescriptionStatus);
                cmd.Parameters.AddWithValue("@NameBangla", aModel.ElementAt(0).NameBangla);
                cmd.Parameters.AddWithValue("@Details", aModel.ElementAt(0).Details);
                cmd.Parameters.AddWithValue("@VisitNewPatient", aModel.ElementAt(0).VisitNewPatient);
                cmd.Parameters.AddWithValue("@VisitOldPatient", aModel.ElementAt(0).VisitOldPatient);
                cmd.Parameters.AddWithValue("@NoOfPatientPerDay", aModel.ElementAt(0).NoOfPatientPerDay);
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));// System.Web.HttpContext.Current.Session["BranchId"]);
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());            
                var drId = (int)cmd.ExecuteScalar();



                if(aModel.ElementAt(0).PnoId!=0)
                {
                    aModel.ForEach(z => z.DrId = drId);
                    aModel.ForEach(z => z.EntryTime = DateTime.Now.ToShortTimeString());
                    DataTable dt = ConvertListDataTable(aModel);
                    var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_GROUPWISE_DOCTOR_REFFEREL_FEE" };
                    objbulk.ColumnMappings.Add("DrId", "DrId");
                    objbulk.ColumnMappings.Add("PnoId", "DeptId");
                    objbulk.ColumnMappings.Add("PnoCharge", "RefFeePc");
                    objbulk.ColumnMappings.Add("RefFeePcOrTk", "RefFeePcOrTk");
                    objbulk.ColumnMappings.Add("UserName", "UserName");
                    objbulk.ColumnMappings.Add("EntryTime", "EntryTime");
                    objbulk.WriteToServer(dt); 
                }
                


                _trans.Commit();
                Con.Close();
                return Task.FromResult<string>("Saved Success");
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

        public List<DoctorModel> GetDoctorList(string searchString)
        {
            try
            {
                var lists = new List<DoctorModel>();
                string query = "",condition="";
                if (searchString != "")
                {
                    condition = "WHERE (Code+Name+MobileNo+Degree) LIKE '%' + @Param + '%'";
                }
                
                query = @"SELECT Id, Code, Name, Category, Gender, Degree, Type, Speciality, DepartmentId, PresentAddress, Area, Thana, Zilla, MobileNo, Email, 
                        DateOfBirth, SpouseName, SpouseDOB, MarriageDate, TakeComision, RefType, Availability, MIOId, PrescriptionStatus, NameBangla, Details, VisitNewPatient, 
                        VisitOldPatient, NoOfPatientPerDay FROM tbl_DOCTOR_INFO " + condition + "";
               
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", searchString);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new DoctorModel()
                    {
                        DrId = Convert.ToInt32(rdr["Id"]),
                        Code = rdr["Code"].ToString(),
                        Name = rdr["Name"].ToString(),
                        Category =rdr["Category"].ToString(),
                        Gender= rdr["Gender"].ToString(),
                        Degree = rdr["Degree"].ToString(),
                        Type = rdr["Type"].ToString(),
                        Speciality = rdr["Speciality"].ToString(),
                        DepartmentId =Convert.ToInt32(rdr["DepartmentId"]),
                        PresentAddress = rdr["PresentAddress"].ToString(),
                        Area = rdr["Area"].ToString(),
                        Thana = rdr["Thana"].ToString(),
                        Zilla = rdr["Zilla"].ToString(),
                        MobileNo = rdr["MobileNo"].ToString(),
                        Email= rdr["Email"].ToString(),
                        DateOfBirth =Convert.ToDateTime(rdr["DateOfBirth"]),
                        SpouseName= rdr["SpouseName"].ToString(),
                        SpouseDob = Convert.ToDateTime(rdr["SpouseDOB"]),
                        MarriageDate = Convert.ToDateTime(rdr["MarriageDate"]),
                        TakeComisionId = Convert.ToInt32(rdr["TakeComision"]),
                        RefType = rdr["RefType"].ToString(),
                        Availability = rdr["Availability"].ToString(),
                        MioId =Convert.ToInt32(rdr["MIOId"]),
                        PrescriptionStatus = Convert.ToInt32(rdr["PrescriptionStatus"]),
                        NameBangla= rdr["NameBangla"].ToString(),
                        Details= rdr["Details"].ToString(),
                        VisitNewPatient=Convert.ToDouble(rdr["VisitNewPatient"]),
                        VisitOldPatient = Convert.ToDouble(rdr["VisitOldPatient"]),
                        NoOfPatientPerDay= Convert.ToInt32(rdr["NoOfPatientPerDay"]),
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
                    Con.Close();
                }
                return new List<DoctorModel>();
            }
        }
////////////////////////////////// DELETE
        public string Delete(int id)
        {
            try
            {
                string msg = "";
                const string query = @"DELETE FROM DrInfo WHERE Id=@DrId";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@DrId", id);
                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Delete Success" : "Delete Failed";
                Con.Close();
                return msg;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return exception.ToString();
            }
        }


        internal Task<string> Update(List<DoctorModel> aModel)
        {

            try
            {
                Con.Open();
                Thread.Sleep(5);
                _trans = Con.BeginTransaction();
                const string query = @"UPDATE  tbl_DOCTOR_INFO SET Name=@Name,Category=@Category, Gender=@Gender, Degree=@Degree,Type=@Type, Speciality=@Speciality, DepartmentId=@DepartmentId, PresentAddress=@PresentAddress, Area=@Area, Thana=@Thana, Zilla=@Zilla, 
MobileNo=@MobileNo, Email=@Email, DateOfBirth=@DateOfBirth, SpouseName=@SpouseName, SpouseDOB=@SpouseDOB,MarriageDate=@MarriageDate, TakeComision=@TakeComision, RefType=@RefType, Availability=@Availability, MIOId=@MIOId, 
PrescriptionStatus=@PrescriptionStatus, NameBangla=@NameBangla, Details=@Details, VisitNewPatient=@VisitNewPatient, VisitOldPatient=@VisitOldPatient, NoOfPatientPerDay=@NoOfPatientPerDay, UserName=@UserName,BranchId=@BranchId,EntryTime=@EntryTime WHERE Id=@Id ";
                
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", aModel.ElementAt(0).DrId);                
                cmd.Parameters.AddWithValue("@Name", aModel.ElementAt(0).Name);
                cmd.Parameters.AddWithValue("@Category", aModel.ElementAt(0).Category);
                cmd.Parameters.AddWithValue("@Gender", aModel.ElementAt(0).Gender);
                cmd.Parameters.AddWithValue("@Degree", aModel.ElementAt(0).Degree);
                cmd.Parameters.AddWithValue("@Type", aModel.ElementAt(0).Type);
                cmd.Parameters.AddWithValue("@Speciality", aModel.ElementAt(0).Speciality);
                cmd.Parameters.AddWithValue("@DepartmentId", aModel.ElementAt(0).DepartmentId);
                cmd.Parameters.AddWithValue("@PresentAddress", aModel.ElementAt(0).PresentAddress);
                cmd.Parameters.AddWithValue("@Area", aModel.ElementAt(0).Area);
                cmd.Parameters.AddWithValue("@Thana", aModel.ElementAt(0).Thana);
                cmd.Parameters.AddWithValue("@Zilla", aModel.ElementAt(0).Zilla);
                cmd.Parameters.AddWithValue("@MobileNo", aModel.ElementAt(0).MobileNo);
                cmd.Parameters.AddWithValue("@Email", aModel.ElementAt(0).Email);
                cmd.Parameters.AddWithValue("@DateOfBirth", aModel.ElementAt(0).DateOfBirth.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SpouseName", aModel.ElementAt(0).SpouseName);
                cmd.Parameters.AddWithValue("@SpouseDOB", aModel.ElementAt(0).SpouseDob.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MarriageDate", aModel.ElementAt(0).MarriageDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@TakeComision", aModel.ElementAt(0).TakeComisionId);
                cmd.Parameters.AddWithValue("@RefType", aModel.ElementAt(0).RefType);
                cmd.Parameters.AddWithValue("@Availability", aModel.ElementAt(0).Availability);
                cmd.Parameters.AddWithValue("@MIOId", aModel.ElementAt(0).MioId);
                cmd.Parameters.AddWithValue("@PrescriptionStatus", aModel.ElementAt(0).PrescriptionStatus);
                cmd.Parameters.AddWithValue("@NameBangla", aModel.ElementAt(0).NameBangla);
                cmd.Parameters.AddWithValue("@Details", aModel.ElementAt(0).Details);
                cmd.Parameters.AddWithValue("@VisitNewPatient", aModel.ElementAt(0).VisitNewPatient);
                cmd.Parameters.AddWithValue("@VisitOldPatient", aModel.ElementAt(0).VisitOldPatient);
                cmd.Parameters.AddWithValue("@NoOfPatientPerDay", aModel.ElementAt(0).NoOfPatientPerDay);
                cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aModel.ElementAt(0).UserName, _trans));// System.Web.HttpContext.Current.Session["BranchId"]);
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());  
                cmd.ExecuteNonQuery();       



                if (aModel.ElementAt(0).PnoId != 0)
                {
                    string querydel = @"DELETE FROM  tbl_GROUPWISE_DOCTOR_REFFEREL_FEE WHERE DrId=" + aModel.ElementAt(0).DrId + " ";
                    var cmddel = new SqlCommand(querydel, Con, _trans);
                    cmddel.ExecuteNonQuery();
                    aModel.ForEach(z => z.EntryTime = DateTime.Now.ToShortTimeString());
                    DataTable dt = ConvertListDataTable(aModel);
                    var objbulk = new SqlBulkCopy(Con, SqlBulkCopyOptions.Default, _trans) { DestinationTableName = "tbl_GROUPWISE_DOCTOR_REFFEREL_FEE" };
                    objbulk.ColumnMappings.Add("DrId", "DrId");
                    objbulk.ColumnMappings.Add("PnoId", "DeptId");
                    objbulk.ColumnMappings.Add("PnoCharge", "RefFeePc");
                    objbulk.ColumnMappings.Add("RefFeePcOrTk", "RefFeePcOrTk");
                    objbulk.ColumnMappings.Add("UserName", "UserName");
                    objbulk.ColumnMappings.Add("EntryTime", "EntryTime");
                    objbulk.WriteToServer(dt);
                }

                _trans.Commit();
                Con.Close();
                return Task.FromResult<string>("Update Success");              
             
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


