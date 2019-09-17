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
    public class PartyGateway : DbConnection
    {

        public Task<string> Save(PartyInfoModel aModel)
        {
            try
            {
                string partyId = GetPartyId();
                const string query = @"INSERT INTO PartyInfo (PartyId,PartyName,Address,ContactNo,OpeningBal,TrDate,UserName,EntryTime,ShowPC,PStatus,VendorFor,Ratio) 
										VALUES (@PartyId,@PartyName,@Address,@ContactNo,@OpeningBal,@TrDate,@UserName,@EntryTime,@ShowPC,@PStatus,@VendorFor,@Ratio)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PartyId", partyId);
                cmd.Parameters.AddWithValue("@PartyName", aModel.PartyName);
                cmd.Parameters.AddWithValue("@Address", aModel.Address);
                cmd.Parameters.AddWithValue("@ContactNo", aModel.ContactNo);
                cmd.Parameters.AddWithValue("@OpeningBal", aModel.OpeningBal);
                cmd.Parameters.AddWithValue("@TrDate", aModel.TrDate);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@EntryTime", aModel.EntryTime);
                cmd.Parameters.AddWithValue("@ShowPC", aModel.ShowPC);
                cmd.Parameters.AddWithValue("@PStatus", aModel.PStatus);
                cmd.Parameters.AddWithValue("@VendorFor", aModel.VendorFor);
                cmd.Parameters.AddWithValue("@Ratio", aModel.Ratio);
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



        public Task<string> Update(PartyInfoModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"UPDATE PartyInfo 
								SET IdNo=@IdNo,PartyName=@PartyName,Address=@Address,ContactNo=@ContactNo,OpeningBal=@OpeningBal,TrDate=@TrDate,PStatus=@PStatus,Ratio=@Ratio
								WHERE IdNo=@IdNo";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdNo", aModel.IdNo);
                //  cmd.Parameters.AddWithValue("@PartyId", aModel.PartyId);
                cmd.Parameters.AddWithValue("@PartyName", aModel.PartyName);
                cmd.Parameters.AddWithValue("@Address", aModel.Address);
                cmd.Parameters.AddWithValue("@ContactNo", aModel.ContactNo);
                cmd.Parameters.AddWithValue("@OpeningBal", aModel.OpeningBal);
                cmd.Parameters.AddWithValue("@TrDate", aModel.TrDate.ToString("yyyy-MM-dd"));
                // cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                // cmd.Parameters.AddWithValue("@EntryTime", aModel.EntryTime);
                // cmd.Parameters.AddWithValue("@ShowPC", aModel.ShowPC);
                cmd.Parameters.AddWithValue("@PStatus", aModel.PStatus);
                //  cmd.Parameters.AddWithValue("@VendorFor", aModel.VendorFor);
                cmd.Parameters.AddWithValue("@Ratio", aModel.Ratio);
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


        public List<PartyInfoModel> GetPartyInfoList(int param)
        {
            try
            {
                var lists = new List<PartyInfoModel>();
                string query = "";

                if (param != 0)
                {
                    query = @"SELECT IdNo,PartyId,PartyName,isnull(Address,0) Address,isnull(ContactNo,0) ContactNo,OpeningBal,TrDate,UserName,EntryTime,ShowPC,PStatus,isnull(VendorFor,0)  VendorFor,isnull(Ratio,0)Ratio
							From PartyInfo WHERE IdNo=@Param";
                }
                else
                {
                    query = @"SELECT IdNo,PartyId,PartyName,isnull(Address,0) Address,isnull(ContactNo,0) ContactNo,OpeningBal,TrDate,UserName,EntryTime,ShowPC,PStatus,isnull(VendorFor,0)  VendorFor,isnull(Ratio,0)Ratio
							From PartyInfo";
                }
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", param);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lists.Add(new PartyInfoModel()
                    {
                        IdNo = Convert.ToInt32(rdr["IdNo"]),
                        PartyId = rdr["PartyId"].ToString(),
                        PartyName = rdr["PartyName"].ToString(),
                        Address = rdr["Address"].ToString(),
                        ContactNo = rdr["ContactNo"].ToString(),
                        OpeningBal = Convert.ToDouble(rdr["OpeningBal"]),
                        TrDate = Convert.ToDateTime(rdr["TrDate"]),
                        UserName = rdr["UserName"].ToString(),
                        EntryTime = rdr["EntryTime"].ToString(),
                        ShowPC = Convert.ToDouble(rdr["ShowPC"]),
                        PStatus = rdr["PStatus"].ToString(),
                        VendorFor = rdr["VendorFor"].ToString(),
                        Ratio = Convert.ToDouble(rdr["Ratio"]),

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
                return new List<PartyInfoModel>();
            }
        }


        public string GetPartyId()
        {
            int maxId = Convert.ToInt32(GetMaxId("PartyInfo", "IdNo"));
            string partyId = Convert.ToString(maxId);
            partyId = "S0" + maxId;
            return partyId;

        }


    }
}