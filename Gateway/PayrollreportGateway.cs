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
    public class PayrollreportGateway : DbConnection
    {
        public List<SalaryCreateModel> GetEmployeelList(string searchString)
        {
            try
            {
                //condition = "WHERE (Code+Name+MobileNo) LIKE '%' + '" + searchString + "' + '%' "; 
                string msg = "", condition = "";
                var list = new List<SalaryCreateModel>();
                string query = "";
                if (searchString != "0") { condition = "WHERE (a.Code+a.Name+convert(nvarchar(255), a.id)) LIKE '%' + N'" + searchString + "' + '%' "; }
                query = @"Select DISTINCT a.Id,a.code,a.Name AS employeeName,c.BName as DepartmentName,d.BName AS DesigenationName
                           from tbl_HR_GLO_EMPLOYEE AS a inner join tbl_HR_GLO_PAYREGISTER AS b ON a.Id = b.EmpId
									   inner Join tbl_DEPARTMENT_HR AS c ON a.DeparmentId = c.Id
									   inner join tbl_DESIGNATION_HR AS d ON a.DesignationId = d.Id " + condition + "";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@Param", searchString);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new SalaryCreateModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        EmCode = rdr["Code"].ToString(),
                        EmName = rdr["employeeName"].ToString(),
                        DepartmentName = rdr["DepartmentName"].ToString(),
                        DesignationName = rdr["DesigenationName"].ToString(),
                        //DeparmentId = Convert.ToInt32(rdr["DeparmentId"])

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
                return new List<SalaryCreateModel>();
            }
        }
    }
}