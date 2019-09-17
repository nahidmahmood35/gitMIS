using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class RosterMasterDetailsGateway : DbConnection
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


        public Task<string> Save(string formDate, string toDate, int emId)
        {
            try
            {
                Con.Close();
                DateTime fdate = Convert.ToDateTime(formDate);
                DateTime Tdate = Convert.ToDateTime(toDate);
                int totelday = (int)(Tdate - fdate).TotalDays;
                
                
                
                
                
                //Con.Open();
               
                
                //for (int i = 0; i <= totelday; i++)
                //{
                //    DateTime startDate = Convert.ToDateTime(formDate).AddDays(i);
                //    int dayNumber = DateNameToInteger(startDate);
                    
                //    const string query = @"Select GeneralShift from tbl_HR_ROSTER_MASTER where EmpId=@EmpId AND DayName=@DayName";
                //    var cmd = new SqlCommand(query, Con);
                //    cmd.Parameters.Clear();
                //    cmd.Parameters.AddWithValue("@EmpId", emId);
                //    cmd.Parameters.AddWithValue("@DayName", dayNumber);
                //    var rdr = cmd.ExecuteReader();
                //    int GeneralShift = 0;
                //    while (rdr.Read())
                //    {
                //        GeneralShift = Convert.ToInt32(rdr["GeneralShift"]);
                       
                //    }
                //    rdr.Close();
                //    const string query2 = @"INSERT INTO tbl_HR_ROSTER_DETAILS (EmpId,Date,DayName,GeneralShift,EntryDate) VALUES (@EmpId,@Date,@DayName,@GeneralShift,@EntryDate)";
                //    var cmd1 = new SqlCommand(query2, Con);
                //    cmd1.Parameters.Clear();
                //    cmd1.Parameters.AddWithValue("@EmpId", emId);
                //    cmd1.Parameters.AddWithValue("@Date", startDate);
                //    cmd1.Parameters.AddWithValue("@DayName", dayNumber);
                //    cmd1.Parameters.AddWithValue("@GeneralShift", GeneralShift);
                //    cmd1.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                //    cmd1.ExecuteNonQuery();
                    
                //}
                //Con.Close();
              //  Con.Open();

                for (int i = 0; i <= totelday; i++)
                {
                    DateTime startDate = Convert.ToDateTime(formDate).AddDays(i);
                    int dayNumber = DateNameToInteger(startDate);
                    
                    var aList=GetRosterData(dayNumber,emId);

                    Con.Open();
                    foreach (RosterViewModel model in aList)
                    {
                        const string query2 = @"INSERT INTO tbl_HR_ROSTER_DETAILS (EmpId,Date,DayName,GeneralShift,EntryDate) VALUES (@EmpId,@Date,@DayName,@GeneralShift,@EntryDate)";
                        var cmd1 = new SqlCommand(query2, Con);
                        cmd1.Parameters.Clear();
                        cmd1.Parameters.AddWithValue("@EmpId", emId);
                        cmd1.Parameters.AddWithValue("@Date", startDate);
                        cmd1.Parameters.AddWithValue("@DayName", model.DayName);
                        cmd1.Parameters.AddWithValue("@GeneralShift", model.ShiftId);
                        cmd1.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd1.ExecuteNonQuery();

                    }
                    Con.Close();
                    
                   




             

                }
              //  Con.Close();

                
                
                
                return Task.FromResult("Saved Success");

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

        private List<RosterViewModel> GetRosterData(int dayNumber,int empId)
        {
            Con.Open();
            var list=new List<RosterViewModel>();
            string query = "Select * from tbl_HR_ROSTER_MASTER WHERE EmpId="+ empId +" AND DayName="+ dayNumber +"";
            var cmd=new SqlCommand(query,Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new RosterViewModel()
                {
                    DayName = Convert.ToInt32(rdr["DayName"]),
                    ShiftId= Convert.ToInt32(rdr["GeneralShift"]),
                });
            }            
            Con.Close();
            //foreach (RosterViewModel roster in list)
            //{
            //    roster.ShiftName = ReturnFieldValue("tbl_HR_ROSTER_SHIFT_INFO", "Id=" + roster.ShiftId + "","ShiftType");
            //}

            return list;

        }

        public int DateNameToInteger(DateTime date)
        {
            string formDateName = date.DayOfWeek.ToString();
            if (formDateName == "Saturday")
            {
                return 1;
            }
            else if (formDateName == "Sunday")
            {
                return 2; 
            }
           else if (formDateName == "Monday")
            {
                return 3;
            }
            else if (formDateName == "Tuesday")
            {
                return 4;
            }
            else if (formDateName == "Wednesday")
            {
                return 5;
            }
            else if (formDateName == "Thursday")
            {
                return 6;
            }
            else
            {
                return 7;  
            }
        }

        public Task<string> Delete(string formDate, string toDate, int emId)
        {
            try
            {
                const string query2 = @"Delete from tbl_HR_ROSTER_DETAILS where EmpId=@EmpId AND Date BETWEEN CONVERT(date,@formDate,102) and CONVERT(date,@toDate,102)";
                
                Con.Open();
                    var cmd1 = new SqlCommand(query2, Con);
                    cmd1.Parameters.Clear();
                    cmd1.Parameters.AddWithValue("@formDate", formDate);
                    cmd1.Parameters.AddWithValue("@toDate", toDate);
                    cmd1.Parameters.AddWithValue("@EmpId", emId);
                    
                    cmd1.ExecuteNonQuery();
                Con.Close();
                
               
                return Task.FromResult("Delete Success");
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


        public List<EmployeeModel> GetAll(int emId)
        {
            try
            {

                string msg = "", condition = "";
                var list = new List<EmployeeModel>();
                string query = "";

                if (emId != 0) { condition = " WHERE a.EmpId =" + emId + " "; }
                query = @"Select a.Id,a.EmpId,a.Date,a.DayName AS DayNameId,b.DayName,a.GeneralShift AS GeneralShiftId,c.ShiftType AS GeneralShiftName   from tbl_HR_ROSTER_DETAILS AS a inner join tbl_HR_DAY_NAME_INFO AS b ON a.DayName=b.Id
                          inner join tbl_HR_ROSTER_SHIFT_INFO AS c ON a.GeneralShift=c.Id " + condition + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new EmployeeModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        EmId = Convert.ToInt32(rdr["EmpId"]),
                        EmFirstDate = Convert.ToDateTime(rdr["Date"]),
                        DayNameId = Convert.ToInt32(rdr["DayNameId"]),
                        DayName = rdr["DayName"].ToString(),
                        ShiftNameId = Convert.ToInt32(rdr["GeneralShiftId"]),
                        ShiftName = rdr["GeneralShiftName"].ToString(),
                   
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


        public Task<string> GetEdit(int timeShiftId, int rowId)
        {
            try
            {
                const string query2 = @"update tbl_HR_ROSTER_DETAILS SET GeneralShift=@GeneralShift where Id=@Id";

                Con.Open();
                var cmd1 = new SqlCommand(query2, Con);
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@GeneralShift", timeShiftId);
                cmd1.Parameters.AddWithValue("@Id", rowId);
                

                cmd1.ExecuteNonQuery();
                Con.Close();


                return Task.FromResult("Update Success");
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