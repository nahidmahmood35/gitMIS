using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class OpdGateway:DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(OpdModel aMdl)
        {
            try
            {
                Con.Open();

                _trans = Con.BeginTransaction();

                string regNo = GetAutoIncrementNumberFromStoreProcedure(1,_trans);
                string query = @"INSERT INTO tbl_PATIENT_REGISTRATION (PtRegNo, Name, MobileNo, DateOfBirth, GenderId, FatherName, MotherName, SpouseName, Address, ReligionId, Area, Occupation, BloodGroupId, NationalIdNo, PassportNo, IntroducerId, IntroducerName, BranchId, UserName) 
                                OUTPUT INSERTED.ID VALUES (@PtRegNo, @Name, @MobileNo, @DateOfBirth, @GenderId, @FatherName, @MotherName, @SpouseName, @Address, @ReligionId, @Area, @Occupation, @BloodGroupId, @NationalIdNo, @PassportNo, @IntroducerId, @IntroducerName, @BranchId, @UserName)";
              
                var cmd = new SqlCommand(query, Con,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PtRegNo", regNo);
                cmd.Parameters.AddWithValue("@Name", aMdl.PtName);
                cmd.Parameters.AddWithValue("@MobileNo", aMdl.PtMobileNo);
                cmd.Parameters.AddWithValue("@DateOfBirth", aMdl.PtDob.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@GenderId", aMdl.PtGendeId);
                cmd.Parameters.AddWithValue("@FatherName", "N/A");
                cmd.Parameters.AddWithValue("@MotherName", "N/A");
                cmd.Parameters.AddWithValue("@SpouseName", aMdl.PtSpooseName);
                cmd.Parameters.AddWithValue("@Address", "N/A");
                cmd.Parameters.AddWithValue("@ReligionId", 0);
                cmd.Parameters.AddWithValue("@Occupation", "N/A");
                cmd.Parameters.AddWithValue("@BloodGroupId", 0);
                cmd.Parameters.AddWithValue("@NationalIdNo", "N/A");
                cmd.Parameters.AddWithValue("@PassportNo", "N/A");
                cmd.Parameters.AddWithValue("@IntroducerId", 0);
                cmd.Parameters.AddWithValue("@IntroducerName", "N/A");
                cmd.Parameters.AddWithValue("@Area", "N/A");
                cmd.Parameters.AddWithValue("@UserName", aMdl.UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aMdl.UserName,_trans));

                var ptId=(int)cmd.ExecuteScalar();


                string invNo = GetAutoIncrementNumberFromStoreProcedure(5, _trans);
                query = @"INSERT INTO tbl_INVOICE_MASTER (RegId, PatientStatus, InvoiceNo, InvoiceDate, Age,DiscountPcOrTk, TotalAmt, AdvanceAmt, PaidAmount,  SubSubPnoId,  Remarks,CardNo,ChqNo,  UserName, BranchId) 
                            OUTPUT INSERTED.ID VALUES (@RegId,@PatientStatus, @InvoiceNo, @InvoiceDate, @Age,  @DiscountPcOrTk, @TotalAmt, @AdvanceAmt, @PaidAmount,   @SubSubPnoId, @Remarks,@CardNo, @ChqNo, @UserName, @BranchId)";

                cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@RegId", ptId);
                cmd.Parameters.AddWithValue("@PatientStatus", "Outdoor");
                cmd.Parameters.AddWithValue("@InvoiceNo", invNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Age", aMdl.PtAgeDetail);
                cmd.Parameters.AddWithValue("@DiscountPcOrTk", " ");
                cmd.Parameters.AddWithValue("@TotalAmt", aMdl.TotalAmount);
                cmd.Parameters.AddWithValue("@AdvanceAmt", aMdl.TotalAmount);
                cmd.Parameters.AddWithValue("@PaidAmount", aMdl.TotalAmount);
                cmd.Parameters.AddWithValue("@SubSubPnoId", 78);
                cmd.Parameters.AddWithValue("@Remarks", "Opd");
                cmd.Parameters.AddWithValue("@CardNo", "N/A");
                cmd.Parameters.AddWithValue("@ChqNo", "N/A");
                cmd.Parameters.AddWithValue("@UserName", aMdl.UserName);
                cmd.Parameters.AddWithValue("@BranchId", GetBranchIdByuserNameOpenCon(aMdl.UserName, _trans));
                var invmasterId = (int)cmd.ExecuteScalar();
                aMdl.InvMasterId = invmasterId;
                query = @"INSERT INTO tbl_INVOICE_DETAIL (InvMasterId, ItemId,  Quantity,Charge) 
                                OUTPUT INSERTED.ID VALUES (@InvMasterId, @ItemId, @Quantity, @Charge)";
                cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvMasterId", invmasterId);
                cmd.Parameters.AddWithValue("@ItemId", 1723);
                cmd.Parameters.AddWithValue("@Quantity", 1);
                cmd.Parameters.AddWithValue("@Charge", aMdl.TotalAmount);
                cmd.ExecuteNonQuery();

                query = @"INSERT INTO tbl_OPD_REF_INFO (InvMasterId, RoomNo,  DepartmentId) 
                                OUTPUT INSERTED.ID VALUES (@InvMasterId, @RoomNo, @DepartmentId)";
                cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@InvMasterId", invmasterId);
                cmd.Parameters.AddWithValue("@RoomNo", aMdl.RoomNo);
                cmd.Parameters.AddWithValue("@DepartmentId", aMdl.DepartmentId);
                cmd.ExecuteNonQuery();
                InsertIntoInvoiceLedger(ptId, 0, invmasterId, invNo, DateTime.Now, aMdl.TotalAmount, 0, aMdl.TotalAmount, 0, 78, "Sales And Collection",aMdl.UserName, _trans);


                _trans.Commit();
                Con.Close();
                return Task.FromResult<string>("Save Successful");
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    Con.Close();
                }
                return Task.FromResult<string>(exception.Message);
            }
        }
    }
}