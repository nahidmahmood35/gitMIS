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
    public class EmployeeIncrementGateway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(List<EmployeeModel> aModel)
        {
            try
            {

                Con.Open();
                Thread.Sleep(50);
                _trans = Con.BeginTransaction();

                foreach (var employevar in aModel)
                {
                    const string query = @"INSERT INTO tbl_HR_Increment (EmCode, EmpId, Grade, Department, Designation, Project, SalaryType, SalaryTypeId, Amount, Year, Month, UserName, EntryDate) VALUES (@EmCode, @EmpId, @Grade, @Department, @Designation, @Project, @SalaryType, @SalaryTypeId, @Amount, @Year, @Month, @UserName, @EntryDate)";
                    var cmd = new SqlCommand(query, Con, _trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@EmCode", employevar.EmCode);
                    cmd.Parameters.AddWithValue("@EmpId", employevar.EmId);
                    cmd.Parameters.AddWithValue("@Grade", employevar.Grade);
                    cmd.Parameters.AddWithValue("@Department", employevar.DepartmentName);
                    cmd.Parameters.AddWithValue("@Designation", employevar.EmDesignationName);
                    cmd.Parameters.AddWithValue("@Project", employevar.UnitName);
                    cmd.Parameters.AddWithValue("@SalaryType", employevar.ItemType);
                    cmd.Parameters.AddWithValue("@SalaryTypeId", employevar.Itemid);
                    cmd.Parameters.AddWithValue("@Amount", employevar.ItemCharge);
                    cmd.Parameters.AddWithValue("@Year", employevar.Year);
                    cmd.Parameters.AddWithValue("@Month", employevar.MonthName);
                    cmd.Parameters.AddWithValue("@UserName", employevar.EmUserName);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }

                string query1 = @"Delete from tbl_HR_GLO_EMPLOYEE_DLS Where EmpId=" + aModel.ElementAt(0).EmId + "";
                var cmd1 = new SqlCommand(query1, Con, _trans);
                cmd1.Parameters.Clear();
                cmd1.ExecuteNonQuery();

                foreach (var employevar in aModel)
                {
                    const string query2 = @"INSERT INTO tbl_HR_GLO_EMPLOYEE_DLS (EmpId, SalaryType, SalaryTypeId, Amount) VALUES (@EmpId, @SalaryType, @SalaryTypeId, @Amount)";
                    var cmd2 = new SqlCommand(query2, Con, _trans);
                    cmd2.Parameters.Clear();
                    cmd2.Parameters.AddWithValue("@EmpId", employevar.EmId);
                    cmd2.Parameters.AddWithValue("@SalaryType", employevar.ItemType);
                    cmd2.Parameters.AddWithValue("@SalaryTypeId", employevar.Itemid);
                    cmd2.Parameters.AddWithValue("@Amount", employevar.ItemCharge);
                    cmd2.ExecuteNonQuery();
                }
                _trans.Commit();
                Con.Close();
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
    }
}