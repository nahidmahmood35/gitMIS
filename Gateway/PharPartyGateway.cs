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
    public class PharPartyGateway : DbConnection
    {

        public Task<string> Save(PharCompanyModel aModel)
        {
            try
            {
                string partyId = GetPartyId();
                const string query = @"INSERT INTO tbl_PHAR_COMPANY (Code,Name,Address,Phone,OpeningBalance,Status,EntryDate,UserName) 
										VALUES (@Code,@Name,@Address,@Phone,@OpeningBalance,@Status,@EntryDate,@UserName)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Code", partyId);
              //  cmd.Parameters.AddWithValue("@Id", aModel.CompanyId);
                cmd.Parameters.AddWithValue("@Name", aModel.ComName);
                cmd.Parameters.AddWithValue("@Address", aModel.Address);
                cmd.Parameters.AddWithValue("@Phone", aModel.Phone);
                cmd.Parameters.AddWithValue("@OpeningBalance", aModel.OpeningBal);
                cmd.Parameters.AddWithValue("@Status", aModel.ComStatus);
                cmd.Parameters.AddWithValue("@EntryDate", aModel.ComEntryDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);           
       
                cmd.ExecuteNonQuery();
                Con.Close();
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



        public Task<string> Update(PharCompanyModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"UPDATE tbl_PHAR_COMPANY 
								SET Name=@Name,Address=@Address,Phone=@Phone,OpeningBalance=@OpeningBalance,Status=@Status,EntryDate=@EntryDate,UserName=@UserName
								WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", aModel.CompanyId);
                cmd.Parameters.AddWithValue("@Name", aModel.ComName);
                cmd.Parameters.AddWithValue("@Address", aModel.Address);
                cmd.Parameters.AddWithValue("@Phone", aModel.Phone);
                cmd.Parameters.AddWithValue("@OpeningBalance", aModel.OpeningBal);
                cmd.Parameters.AddWithValue("@Status", aModel.ComStatus);
                cmd.Parameters.AddWithValue("@EntryDate", aModel.ComEntryDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);   
                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult("Update successful");
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


//        public List<PartyInfoModel> GetPartyInfoList(int param)
//        {
//            try
//            {
//                var lists = new List<PartyInfoModel>();
//                string query = "";

//                if (param != 0)
//                {
//                    query = @"SELECT IdNo,PartyId,PartyName,isnull(Address,0) Address,isnull(ContactNo,0) ContactNo,OpeningBal,TrDate,UserName,EntryTime,ShowPC,PStatus,isnull(VendorFor,0)  VendorFor,isnull(Ratio,0)Ratio
//							From PartyInfo WHERE IdNo=@Param";
//                }
//                else
//                {
//                    query = @"SELECT IdNo,PartyId,PartyName,isnull(Address,0) Address,isnull(ContactNo,0) ContactNo,OpeningBal,TrDate,UserName,EntryTime,ShowPC,PStatus,isnull(VendorFor,0)  VendorFor,isnull(Ratio,0)Ratio
//							From PartyInfo";
//                }
//                Con.Open();
//                var cmd = new SqlCommand(query, Con);
//                cmd.Parameters.Clear();
//                cmd.Parameters.AddWithValue("@Param", param);
//                var rdr = cmd.ExecuteReader();

//                while (rdr.Read())
//                {
//                    lists.Add(new PartyInfoModel()
//                    {
//                        IdNo = Convert.ToInt32(rdr["IdNo"]),
//                        PartyId = rdr["PartyId"].ToString(),
//                        PartyName = rdr["PartyName"].ToString(),
//                        Address = rdr["Address"].ToString(),
//                        ContactNo = rdr["ContactNo"].ToString(),
//                        OpeningBal = Convert.ToDouble(rdr["OpeningBal"]),
//                        TrDate = Convert.ToDateTime(rdr["TrDate"]),
//                        UserName = rdr["UserName"].ToString(),
//                        EntryTime = rdr["EntryTime"].ToString(),
//                        ShowPC = Convert.ToDouble(rdr["ShowPC"]),
//                        PStatus = rdr["PStatus"].ToString(),
//                        VendorFor = rdr["VendorFor"].ToString(),
//                        Ratio = Convert.ToDouble(rdr["Ratio"]),

//                    });
//                }
//                rdr.Close();
//                Con.Close();
//                return lists;
//            }
//            catch (Exception exception)
//            {
//                if (Con.State == ConnectionState.Open)
//                {
//                    Con.Close();
//                }
//                return new List<PartyInfoModel>();
//            }
//        }





        public List<PharCompanyModel> GetPartyInfoListByName(string searchString)
        {
            try 
            {
                var lists = new List<PharCompanyModel>();
                string query = "";
                string condition = "";

                    if (searchString != "0") { condition = " WHERE (Name+Code) LIKE '%' + '" + searchString + "' + '%' "; }

                    query = @"SELECT Id,Code,Name,isnull(Address,0) Address,isnull(Phone,0) Phone,OpeningBalance,EntryDate,UserName,Status
							From tbl_PHAR_COMPANY " + condition + " ";
                
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
              //  cmd.Parameters.AddWithValue("@Param", searchString);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new PharCompanyModel()
                    {
                        CompanyId = Convert.ToInt32(rdr["Id"]),
                        ComCode = rdr["Code"].ToString(),
                        ComName = rdr["Name"].ToString(),
                        Address = rdr["Address"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        OpeningBal = Convert.ToDouble(rdr["OpeningBalance"]),
                        ComEntryDate = Convert.ToDateTime(rdr["EntryDate"]),
                        UserName = rdr["UserName"].ToString(),
                        ComStatus = Convert.ToInt32(rdr["Status"]),
                   

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
                return new List<PharCompanyModel>();
            }
        }





        public string GetPartyId()
        {
            int maxId = Convert.ToInt32(GetMaxId("tbl_PHAR_COMPANY", "Id"));
            string partyId = Convert.ToString(maxId);
            partyId = "S0" + maxId;
            return partyId;

        }


    }
}