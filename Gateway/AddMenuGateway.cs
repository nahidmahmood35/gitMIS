using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CrystalDecisions.Shared.Json;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class AddMenuGateway : DbConnection
    {
        public string Save(AddMenuModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"INSERT INTO tbl_TREE_MAIN (DepartmentName, MainMenuName, SubMenuName, MenuSerialNo) VALUES (@DepartmentName, @MainMenuName, @SubMenuName, @MenuSerialNo)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@DepartmentName", aModel.DepartmentName);
                cmd.Parameters.AddWithValue("@MainMenuName", aModel.MainMenuName);
                cmd.Parameters.AddWithValue("@SubMenuName", aModel.SubMenuName);
                cmd.Parameters.AddWithValue("@MenuSerialNo", aModel.SubMenuSlNo);
                cmd.Parameters.AddWithValue("@ControllerName", aModel.ControllerName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ViewName", aModel.ViewName ?? (object)DBNull.Value);
                    
                int rtn = cmd.ExecuteNonQuery();
                msg = rtn==1 ? "Saved Success" : "Saved Failed";
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
     
        public List<IdNameForDropdownModel> GetList(string param)
        {
            try
            {
                var lists = new List<IdNameForDropdownModel>();
                string query = "";
                switch (param)
                {
                    case"DepartmentName":
                        query = @"SELECT Distinct DepartmentName as Name From tbl_TREE_MAIN ";
                        break;
                    case "MainMenuName":
                        query = @"SELECT Distinct MainMenuName as Name From tbl_TREE_MAIN ";
                        break;
                    case "SubMenuName":
                        query = @"SELECT Distinct SubMenuName as Name From tbl_TREE_MAIN ";
                        break;
                    case "ControllerName":
                        query = @"SELECT Distinct ControllerName as Name From tbl_TREE_MAIN ";
                        break;
                }
          
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Name = rdr["Name"].ToString(),
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
                return new List<IdNameForDropdownModel>();
            }
        }
 
    }
}