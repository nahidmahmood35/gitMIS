using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class RosterMasterGateway : DbConnection
    {
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
                        // CatId = Convert.ToInt32(rdr["CatId"])
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


        public List<EmployeeModel> GetEmployeelDetalsList(string searchString)
        {
            try
            {

                string msg = "", condition = "";
                var list = new List<EmployeeModel>();
                string query = "";
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%' + '" + searchString + "' + '%' "; }
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%" + searchString + "%' "; }
                if (searchString != "0") { condition = " WHERE a.id =" + searchString + " "; }
                query = @"Select a.Name,b.BName as departmentName,c.BName AS designationName 
                            from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_DEPARTMENT_HR AS b ON a.DeparmentId=b.Id
                            inner join tbl_DESIGNATION_HR AS c ON a.Id=c.Id " + condition + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new EmployeeModel()
                    {
                        EmName = rdr["Name"].ToString(),
                        DepartmentName = rdr["departmentName"].ToString(),
                        EmDesignationName = rdr["designationName"].ToString(),
            
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
                return new List<EmployeeModel>();
            }
        }

        public List<EmployeeModel> GetEmployeelDetalsListMaster(string searchString)
        {
            try
            {

                string msg = "", condition = "";
                var list = new List<EmployeeModel>();
                string query = "";
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%' + '" + searchString + "' + '%' "; }
                //if (searchString != "0") { condition = "WHERE (Code+Name+MobileNo) LIKE N'%" + searchString + "%' "; }
                if (searchString != "0") { condition = " WHERE a.id =" + searchString + " "; }
                query = @"Select a.Name,b.BName as departmentName,c.BName AS designationName,d.ShiftStaus,
                        d.DayName AS DayNameId,d.GeneralShift AS GeneralShiftId,e.DayName,f.ShiftType AS GeneralShift
                        from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_DEPARTMENT_HR AS b ON a.DeparmentId=b.Id
                        inner join tbl_DESIGNATION_HR AS c ON a.Id=c.Id 
                        inner join tbl_HR_ROSTER_MASTER AS d ON a.Id=d.EmpId
                        inner Join tbl_HR_DAY_NAME_INFO AS e ON d.DayName=e.Id
                        inner join tbl_HR_ROSTER_SHIFT_INFO f ON d.GeneralShift=f.Id " + condition + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new EmployeeModel()
                    {
                        EmName = rdr["Name"].ToString(),
                        DepartmentName = rdr["departmentName"].ToString(),
                        EmDesignationName = rdr["designationName"].ToString(),
                        EmShiftStatus = Convert.ToInt32(rdr["ShiftStaus"]),
                        DayNameId = Convert.ToInt32(rdr["DayNameId"]),
                        ShiftNameId = Convert.ToInt32(rdr["GeneralShiftId"]),
                        DayName = rdr["DayName"].ToString(),
                        ShiftName = rdr["GeneralShift"].ToString(),
                        
                        

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
                return new List<EmployeeModel>();
            }
        }

        public Task<string> Save(List<EmployeeModel> aModel)
        {
            try
            {
                foreach (var employeeModel in aModel)
                {
                    Con.Open();
                    const string query =
                        @"INSERT INTO tbl_HR_ROSTER_MASTER (EmpId,ShiftStaus,DayName,GeneralShift,EntryDate,UserName) VALUES (@EmpId,@ShiftStaus,@DayName,@GeneralShift,@EntryDate,@UserName)";
                    var cmd = new SqlCommand(query, Con);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@EmpId", employeeModel.EmId);
                    cmd.Parameters.AddWithValue("@ShiftStaus", employeeModel.EmShiftStatus);
                    cmd.Parameters.AddWithValue("@DayName", employeeModel.DayNameId);
                    cmd.Parameters.AddWithValue("@GeneralShift", employeeModel.ShiftNameId);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@UserName", employeeModel.EmUserName);
                    cmd.ExecuteNonQuery();

                    Con.Close();
                }
                return Task.FromResult("Save successful");
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