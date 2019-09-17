using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class LeaveRegisterGateway : DbConnection

    {
        private SqlTransaction _trans;
        public List<IdNameForDropdownModel> GetIdCasCadeDropDown(string query)
        {
            var lists = new List<IdNameForDropdownModel>();
            try
            {
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                       // NameSecent = rdr["Year"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }
        public Task<int> SaveWithReturnId(string tableName, string name, int day)
        {
            try
            {
                if (Con.State == ConnectionState.Open){Con.Close();}

                int rtn = 0;
                var currantYear = Convert.ToInt32(DateTime.Now.Year) + 5;
                Con.Open();
                for (int i = 5; i > -1; i--)
                {
                    
                    var year = currantYear - (5-i);
                    string query = @"INSERT INTO " + tableName + " (Name,Days,Year) OUTPUT INSERTED.ID VALUES (@Name,@Days,@Year)";
                   
                    var cmd = new SqlCommand(query, Con);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Days", day);
                    cmd.Parameters.AddWithValue("@Year", year);
                    rtn = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    }
                Con.Close();
                return Task.FromResult(rtn);
                
               
            }
            catch (Exception ex)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(0);
            }
        }

        public int LeaveCalculation(string code, int leaveType)
        {
            try
            {
                if (Con.State == ConnectionState.Open) { Con.Close(); }
                var currantYear = DateTime.Now.Year;
                string queryForLeaveReg = "";
                int resultForLeaveReg = 0;
                queryForLeaveReg = "SELECT COUNT(EmpId) as Description FROM tbl_HR_LEAVE_REGISTER WHERE LeaveTypeId='" + leaveType + "' AND YEAR(EntryDate) ='" + currantYear + "'AND EmpId ='" + code + "'"; 
                Con.Open();
                var cmd = new SqlCommand(queryForLeaveReg, Con);
                SqlDataReader aReader = cmd.ExecuteReader();
                while (aReader.Read())
                {
                    resultForLeaveReg = Convert.ToInt32(aReader["Description"]);
                }
                aReader.Close();
                Con.Close();

                //return resultForLeaveReg;
                if (Con.State == ConnectionState.Open) { Con.Close(); }
                string queryForLeaveType = "";
                int resultForLeaveType = 0;
                queryForLeaveType = "SELECT Days from tbl_HR_LEAVE_TYPE where id = '" + leaveType + "'";
                Con.Open();
                
                 cmd = new SqlCommand(queryForLeaveType, Con);
                 aReader = cmd.ExecuteReader();
                while (aReader.Read())
                {
                    resultForLeaveType = Convert.ToInt32(aReader["Days"]);
                }
                aReader.Close();
                Con.Close();
                int restLeave = resultForLeaveType - resultForLeaveReg;
                return restLeave;

            }
            catch (Exception ex)
            {

                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return 0;
            }
            
        }
        public Task<string> Save(LeaveRegisterModel aModel)
        {
            
            try
            {
                string trNo = GetTrNo("TrNo", "tbl_HR_LEAVE_REGISTER", _trans);
                Con.Open();
                for (int i = -1; i < (aModel.Day-1); i++)
                {
                    const string query = @"INSERT INTO tbl_HR_LEAVE_REGISTER (TrNo,EmpId,TrDate,LeaveDate,LeaveTypeId,Remarks) VALUES (@TrNo,@EmpId,@TrDate,@LeaveDate,@LeaveTypeId,@Remarks);";
                    
                    var cmd = new SqlCommand(query, Con);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrNo", trNo);
                    cmd.Parameters.AddWithValue("@EmpId", aModel.EmCode);
                    cmd.Parameters.AddWithValue("@TrDate", aModel.EntryDay.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@LeaveDate", aModel.LeaveFrom.AddDays(i+1).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@LeaveTypeId", aModel.LeaveTypeId);
                    cmd.Parameters.AddWithValue("@Remarks", aModel.Remark);
                    //cmd.Parameters.AddWithValue("@UserName", aModel.ElementAt(0).UserName);
                    //cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(mInvoice.ElementAt(0).UserName));
                   
                    cmd.ExecuteNonQuery();
                    
                }
                
               Con.Close();
               return Task.FromResult<string>("Save uccess");
                
                


            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return Task.FromResult(exception.Message);
            }
        }
    }
}