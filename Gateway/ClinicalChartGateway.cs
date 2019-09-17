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
    public class ClinicalChartGateway : DbConnection
    {
        public Task<string> Save(ClinicalChartModel mClinicalChart)
        {
            try
            {
                const string query = @"INSERT INTO tbl_CLINICAL_CHART (PCode,Description, Charge, ServiceCharge, ServiceChargePcOrTk, Vat, VatPcOrTk, LessAmount, RefFee, RefFeePcOrTk, ReportFee, ReportFeePcOrTk, CollectionFee, CollectionFeePcOrTk, OthersFee, OthersFeePcOrTk, ReportDeliverDuration, OutTest, CanChangePrice, ShowDoctorCode, CanGiveLess, ReportFileName, SubSubPnoId,IndoorBillGroupId, ReportGroupId, IsShow, DiscountGroupId, AccountReportId, UserName, BranchId,  EntryTime,IsAdjustAmt) 
                             VALUES (@PCode, @Description, @Charge, @ServiceCharge, @ServiceChargePcOrTk, @Vat, @VatPcOrTk, @LessAmount, @RefFee, @RefFeePcOrTk, @ReportFee, @ReportFeePcOrTk, @CollectionFee, @CollectionFeePcOrTk, @OthersFee, @OthersFeePcOrTk, @ReportDeliverDuration, @OutTest, @CanChangePrice, @ShowDoctorCode, @CanGiveLess, @ReportFileName, @SubSubPnoId, @IndoorBillGroupId, @ReportGroupId, @IsShow, @DiscountGroupId, @AccountReportId, @UserName, @BranchId,  @EntryTime,@IsAdjustAmt)";
                Con.Open();

                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PCode", mClinicalChart.PCode);
                cmd.Parameters.AddWithValue("@Description", mClinicalChart.Description);
                cmd.Parameters.AddWithValue("@Charge", mClinicalChart.Charge);
                cmd.Parameters.AddWithValue("@ServiceCharge", mClinicalChart.ServiceCharge);
                cmd.Parameters.AddWithValue("@ServiceChargePcOrTk", mClinicalChart.ServiceChargePcOrTkId);
                cmd.Parameters.AddWithValue("@Vat", mClinicalChart.Vat);
                cmd.Parameters.AddWithValue("@VatPcOrTk", mClinicalChart.VatPcOrTkId);
                cmd.Parameters.AddWithValue("@LessAmount", mClinicalChart.LessFixedAmount);
                cmd.Parameters.AddWithValue("@RefFee", mClinicalChart.ReferelFee);
                cmd.Parameters.AddWithValue("@RefFeePcOrTk", mClinicalChart.ReferelFeePcOrTkId);
                cmd.Parameters.AddWithValue("@ReportFee", mClinicalChart.ReportFee);
                cmd.Parameters.AddWithValue("@ReportFeePcOrTk", mClinicalChart.ReportFeePcOrTkId);
                cmd.Parameters.AddWithValue("@CollectionFee", mClinicalChart.CollectionFee);
                cmd.Parameters.AddWithValue("@CollectionFeePcOrTk", mClinicalChart.CollectionFeePcOrTkId);
                cmd.Parameters.AddWithValue("@OthersFee", mClinicalChart.OthersFee);
                cmd.Parameters.AddWithValue("@OthersFeePcOrTk", mClinicalChart.OthersFeePcOrTkId);
                cmd.Parameters.AddWithValue("@ReportDeliverDuration", mClinicalChart.DeliveryDuration);
                cmd.Parameters.AddWithValue("@OutTest", mClinicalChart.OutTest);
                cmd.Parameters.AddWithValue("@CanChangePrice", mClinicalChart.CanChangePrice);
                cmd.Parameters.AddWithValue("@ShowDoctorCode", mClinicalChart.ShowDoctorCode);
                cmd.Parameters.AddWithValue("@CanGiveLess", mClinicalChart.CanGiveLess);
                cmd.Parameters.AddWithValue("@ReportFileName", mClinicalChart.ReportFileName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SubSubPnoId", mClinicalChart.SubSubPnoId);
                cmd.Parameters.AddWithValue("@IndoorBillGroupId", mClinicalChart.InDoorBillGroupId);
                cmd.Parameters.AddWithValue("@ReportGroupId", mClinicalChart.ReportGroupId);
                cmd.Parameters.AddWithValue("@IsShow", mClinicalChart.IsShowId);
                cmd.Parameters.AddWithValue("@DiscountGroupId", mClinicalChart.DiscountGroupId);
                cmd.Parameters.AddWithValue("@AccountReportId", mClinicalChart.AccountReportGroupId);
                cmd.Parameters.AddWithValue("@UserName", mClinicalChart.UserName);
                cmd.Parameters.AddWithValue("@BranchId", ReturnFieldValueOpenCon("tbl_USER_BRANCH_INFO", "UserName='" + mClinicalChart.UserName + "'", "BranchId"));//System.Web.HttpContext.Current.Session["BranchId"]);
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@IsAdjustAmt", mClinicalChart.IsAdjustAmtId);
                


                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Save Successful");
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

        public Task<string> Update(ClinicalChartModel mClinicalChart)
        {

            try
            {
                const string query = @"UPDATE tbl_CLINICAL_CHART SET   PCode=@PCode,Description=@Description, Charge=@Charge, ServiceCharge=@ServiceCharge, ServiceChargePcOrTk=@ServiceChargePcOrTk, Vat=@Vat, VatPcOrTk=@VatPcOrTk, LessAmount=@LessAmount, RefFee=@RefFee, RefFeePcOrTk=@RefFeePcOrTk, ReportFee=@ReportFee, ReportFeePcOrTk=@ReportFeePcOrTk, CollectionFee=@CollectionFee, CollectionFeePcOrTk=@CollectionFeePcOrTk, OthersFee=@OthersFee, OthersFeePcOrTk=@OthersFeePcOrTk, ReportDeliverDuration=@ReportDeliverDuration, OutTest=@OutTest, CanChangePrice=@CanChangePrice, ShowDoctorCode=@ShowDoctorCode, CanGiveLess=@CanGiveLess, ReportFileName=@ReportFileName, SubSubPnoId=@SubSubPnoId,IndoorBillGroupId=@IndoorBillGroupId, ReportGroupId=@ReportGroupId, IsShow=@IsShow, DiscountGroupId=@DiscountGroupId, AccountReportId=@AccountReportId, UserName=@UserName, BranchId=@BranchId,EntryTime=@EntryTime,IsAdjustAmt =@IsAdjustAmt
                                        WHERE Id=@ItemId";
                Con.Open();

                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ItemId", mClinicalChart.ItemId);
                cmd.Parameters.AddWithValue("@PCode", mClinicalChart.PCode);
                cmd.Parameters.AddWithValue("@Description", mClinicalChart.Description);
                cmd.Parameters.AddWithValue("@Charge", mClinicalChart.Charge);
                cmd.Parameters.AddWithValue("@ServiceCharge", mClinicalChart.ServiceCharge);
                cmd.Parameters.AddWithValue("@ServiceChargePcOrTk", mClinicalChart.ServiceChargePcOrTkId);
                cmd.Parameters.AddWithValue("@Vat", mClinicalChart.Vat);
                cmd.Parameters.AddWithValue("@VatPcOrTk", mClinicalChart.VatPcOrTkId);
                cmd.Parameters.AddWithValue("@LessAmount", mClinicalChart.LessFixedAmount);
                cmd.Parameters.AddWithValue("@RefFee", mClinicalChart.ReferelFee);
                cmd.Parameters.AddWithValue("@RefFeePcOrTk", mClinicalChart.ReferelFeePcOrTkId);
                cmd.Parameters.AddWithValue("@ReportFee", mClinicalChart.ReportFee);
                cmd.Parameters.AddWithValue("@ReportFeePcOrTk", mClinicalChart.ReportFeePcOrTkId);
                cmd.Parameters.AddWithValue("@CollectionFee", mClinicalChart.CollectionFee);
                cmd.Parameters.AddWithValue("@CollectionFeePcOrTk", mClinicalChart.CollectionFeePcOrTkId);
                cmd.Parameters.AddWithValue("@OthersFee", mClinicalChart.OthersFee);
                cmd.Parameters.AddWithValue("@OthersFeePcOrTk", mClinicalChart.OthersFeePcOrTkId);
                cmd.Parameters.AddWithValue("@ReportDeliverDuration", mClinicalChart.DeliveryDuration);
                cmd.Parameters.AddWithValue("@OutTest", mClinicalChart.OutTest);
                cmd.Parameters.AddWithValue("@CanChangePrice", mClinicalChart.CanChangePrice);
                cmd.Parameters.AddWithValue("@ShowDoctorCode", mClinicalChart.ShowDoctorCode);
                cmd.Parameters.AddWithValue("@CanGiveLess", mClinicalChart.CanGiveLess);
                cmd.Parameters.AddWithValue("@ReportFileName", mClinicalChart.ReportFileName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SubSubPnoId", mClinicalChart.SubSubPnoId);
                cmd.Parameters.AddWithValue("@IndoorBillGroupId", mClinicalChart.InDoorBillGroupId);
                cmd.Parameters.AddWithValue("@ReportGroupId", mClinicalChart.ReportGroupId);
                cmd.Parameters.AddWithValue("@IsShow", mClinicalChart.IsShowId);
                cmd.Parameters.AddWithValue("@DiscountGroupId", mClinicalChart.DiscountGroupId);
                cmd.Parameters.AddWithValue("@AccountReportId", mClinicalChart.AccountReportGroupId);
                cmd.Parameters.AddWithValue("@UserName", mClinicalChart.UserName);
                cmd.Parameters.AddWithValue("@BranchId", ReturnFieldValueOpenCon("tbl_USER_BRANCH_INFO", "UserName='" + mClinicalChart.UserName + "'", "BranchId"));//System.Web.HttpContext.Current.Session["BranchId"]);
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@IsAdjustAmt", mClinicalChart.IsAdjustAmtId);

                cmd.ExecuteNonQuery();
                Con.Close();
                return Task.FromResult<string>("Update Successful");
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

        internal List<ClinicalChartModel> GetClinicalChartList(string searchString,int isShowAll)
        {
            try
            {
                string showCond = "";
                if (isShowAll!=1)
                {
                    showCond = "TOP 10";
                }



                var list = new List<ClinicalChartModel>();
                string query = @"SELECT " + showCond + " Id,Pcode,Description,Charge,ServiceCharge,ServiceChargePcOrTk,Vat,VatPcOrTk,LessAmount,RefFee,RefFeePcOrTk,ReportFee,ReportFeePcOrTk,CollectionFee,CollectionFeePcOrTk,OthersFee,OthersFeePcOrTk,ReportDeliverDuration,OutTest,CanChangePrice,ShowDoctorCode,CanGiveLess,ReportFileName,SubSubPnoId,IndoorBillGroupId,ReportGroupId,IsShow,DiscountGroupId,AccountReportId,UserName,BranchId,EntryDate,EntryTime,CASE WHEN RefFeePcOrTk = 'Tk.' THEN RefFee ELSE CAST(RefFee*0.01*Charge AS Money) END  AS MaxReferelAmount,IsAdjustAmt FROm tbl_clinical_chart WHERE (Pcode+Description) LIKE '%' + @Param + '%'";
                Con.Open();

                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Param", searchString);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ClinicalChartModel()
                    {
                        ItemId = Convert.ToInt32(rdr["Id"]),
                        PCode = rdr["Pcode"].ToString(),
                        Description = rdr["Description"].ToString(),
                        Charge = Convert.ToDouble(rdr["Charge"]),
                        ServiceCharge = Convert.ToDouble(rdr["ServiceCharge"]),
                        ServiceChargePcOrTkId = rdr["ServiceChargePcOrTk"].ToString(),
                        Vat = Convert.ToDouble(rdr["Vat"]),
                        VatPcOrTkId = rdr["VatPcOrTk"].ToString(),
                        LessFixedAmount = Convert.ToDouble(rdr["LessAmount"]),
                        ReferelFee = Convert.ToDouble(rdr["RefFee"]),
                        ReferelFeePcOrTkId = rdr["RefFeePcOrTk"].ToString(),
                        ReportFee = Convert.ToDouble(rdr["ReportFee"]),
                        ReportFeePcOrTkId = rdr["ReportFeePcOrTk"].ToString(),
                        CollectionFee = Convert.ToDouble(rdr["CollectionFee"]),
                        CollectionFeePcOrTkId = rdr["CollectionFeePcOrTk"].ToString(),
                        OthersFee = Convert.ToDouble(rdr["OthersFee"]),
                        OthersFeePcOrTkId = rdr["OthersFeePcOrTk"].ToString(),
                        DeliveryDuration = Convert.ToInt32(rdr["ReportDeliverDuration"]),
                        OutTest = Convert.ToInt32(rdr["OutTest"]),
                        CanChangePrice = Convert.ToInt32(rdr["CanChangePrice"]),
                        ShowDoctorCode = Convert.ToInt32(rdr["ShowDoctorCode"]),
                        CanGiveLess = Convert.ToInt32(rdr["CanGiveLess"]),
                        ReportFileName = rdr["ReportFileName"].ToString(),
                        SubSubPnoId = Convert.ToInt32(rdr["SubSubPnoId"]),
                        InDoorBillGroupId = Convert.ToInt32(rdr["InDoorBillGroupId"]),
                        ReportGroupId = Convert.ToInt32(rdr["ReportGroupId"]),
                        IsShowId = Convert.ToInt32(rdr["IsShow"]),
                        DiscountGroupId = Convert.ToInt32(rdr["DiscountGroupId"]),
                        AccountReportGroupId = Convert.ToInt32(rdr["AccountReportId"]),
                        UserName = rdr["UserName"].ToString(),
                        MaxRefFee = Convert.ToDouble(rdr["MaxReferelAmount"]),
                        IsAdjustAmtId = Convert.ToInt32(rdr["IsAdjustAmt"]),
                        
                    });
                }

                // ,,,,,,,,,,BranchId,EntryDate,EntryTime
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
                return null;
            }
        }
    }
}