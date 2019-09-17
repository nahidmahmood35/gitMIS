using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HospitalManagementApp_Api.Gateway.DB_Helper;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Gateway
{
    public class InvestigationChartGateway : DbConnection
    {




        ///////////////////////////SAVE
        

        public string Save(InvestigationChartModel aModel )
        {
            try
            {
                string msg = "";
                const string query = @"INSERT INTO InvestigationChart (DeptName,PCode,ShortDesc,LongDesc,Charge,TradePrice,Discount,DiscountStatus,NormalValue,LessAmount,RptType,UserName,Category,GroupId,GroupName,DeliveryDuration,SubDeptName,SubSubDeptName,VaqGroup,VaqName,VaqPrice,VaqVat,VaqComm,VaqStatus,TestType,RptCom,RptComPcorTk,ISShow,CollectionFee,CollFeeTkPC,MedicineFee,MedicineFeeTkPc,MedicineFeeName,ReportGroup) 
                VALUES (@DeptName,@PCode,@ShortDesc,@LongDesc,@Charge,@TradePrice,@Discount,@DiscountStatus,@NormalValue,@LessAmount,@RptType,@UserName,@Category,@GroupId,@GroupName,@DeliveryDuration,@SubDeptName,@SubSubDeptName,@VaqGroup,@VaqName,@VaqPrice,@VaqVat,@VaqComm,@VaqStatus,@TestType,@RptCom,@RptComPcorTk,@ISShow,@CollectionFee,@CollFeeTkPC,@MedicineFee,@MedicineFeeTkPc,@MedicineFeeName,@ReportGroup)";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                
                cmd.Parameters.AddWithValue("@DeptName", aModel.DeptName);
                cmd.Parameters.AddWithValue("@PCode", aModel.PCode);
                cmd.Parameters.AddWithValue("@ShortDesc", aModel.ShortDesc);
                cmd.Parameters.AddWithValue("@LongDesc", aModel.LongDesc);
                cmd.Parameters.AddWithValue("@Charge", aModel.Charge);
                cmd.Parameters.AddWithValue("@TradePrice", aModel.TradePrice);
                cmd.Parameters.AddWithValue("@Discount", aModel.Discount);
                cmd.Parameters.AddWithValue("@DiscountStatus", aModel.DiscountStatus);
                cmd.Parameters.AddWithValue("@NormalValue", aModel.NormalValue);
                cmd.Parameters.AddWithValue("@LessAmount", aModel.LessAmount);
                cmd.Parameters.AddWithValue("@RptType", aModel.RptType);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@Category", aModel.Category);
                cmd.Parameters.AddWithValue("@GroupId", aModel.GroupId);
                cmd.Parameters.AddWithValue("@GroupName", aModel.GroupName);
                cmd.Parameters.AddWithValue("@DeliveryDuration", aModel.DeliveryDuration);
                cmd.Parameters.AddWithValue("@SubDeptName", aModel.SubSubDeptName);
                cmd.Parameters.AddWithValue("@SubSubDeptName", aModel.SubSubDeptName);
                cmd.Parameters.AddWithValue("@VaqGroup", aModel.VaqGroup);
                cmd.Parameters.AddWithValue("@VaqName", aModel.VaqName);
                cmd.Parameters.AddWithValue("@VaqPrice", aModel.VaqPrice);
                cmd.Parameters.AddWithValue("@VaqVat", aModel.VaqVat);
                cmd.Parameters.AddWithValue("@VaqComm", aModel.VaqComm);
                cmd.Parameters.AddWithValue("@VaqStatus", aModel.VaqStatus);
                cmd.Parameters.AddWithValue("@TestType", aModel.TestType);
                cmd.Parameters.AddWithValue("@RptCom", aModel.RptCom);
                cmd.Parameters.AddWithValue("@RptComPcorTk", aModel.RptComPcorTk);
                cmd.Parameters.AddWithValue("@ISShow", aModel.ISShow);
                cmd.Parameters.AddWithValue("@CollectionFee", aModel.CollectionFee);
                cmd.Parameters.AddWithValue("@CollFeeTkPC", aModel.CollFeeTkPC);
                cmd.Parameters.AddWithValue("@MedicineFee", aModel.MedicineFee);
                cmd.Parameters.AddWithValue("@MedicineFeeTkPc", aModel.MedicineFeeTkPc);
                cmd.Parameters.AddWithValue("@MedicineFeeName", aModel.MedicineFeeName);
                cmd.Parameters.AddWithValue("@ReportGroup", aModel.ReportGroup);


                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Saved Success" : "Saved Failed";
                Con.Close();
                return msg;


            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }

                return exception.ToString();
            }


        }




        ///////////////////////////UPDATE


        public string Update(InvestigationChartModel aModel)
        {
            try
            {
                string msg = "";
                const string query = @"UPDATE InvestigationChart SET DeptName=@DeptName,PCode=@PCode,ShortDesc=@ShortDesc,LongDesc=@LongDesc,Charge=@Charge,TradePrice=@TradePrice,Discount=@Discount,DiscountStatus=@DiscountStatus,NormalValue=@NormalValue,LessAmount=@LessAmount,RptType=@RptType,UserName=@UserName,Category=@Category,GroupId=@GroupId,GroupName=@GroupName,DeliveryDuration=@DeliveryDuration,SubDeptName=@SubDeptName,SubSubDeptName=@SubSubDeptName,VaqGroup=@VaqGroup,VaqName=@VaqName,VaqPrice=@VaqPrice,VaqVat=@VaqVat,VaqComm=@VaqComm,VaqStatus=@VaqStatus,TestType=@TestType,RptCom=@RptCom,RptComPcorTk=@RptComPcorTk,ISShow=@ISShow,CollectionFee=@CollectionFee,CollFeeTkPC=@CollFeeTkPC,MedicineFee=@MedicineFee,MedicineFeeTkPc=@MedicineFeeTkPc,MedicineFeeName=@MedicineFeeName,ReportGroup=@ReportGroup WHERE Id=@Id";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", aModel.Id);
                cmd.Parameters.AddWithValue("@DeptName", aModel.DeptName);
                cmd.Parameters.AddWithValue("@PCode", aModel.PCode);
                cmd.Parameters.AddWithValue("@ShortDesc", aModel.ShortDesc);
                cmd.Parameters.AddWithValue("@LongDesc", aModel.LongDesc);
                cmd.Parameters.AddWithValue("@Charge", aModel.Charge);
                cmd.Parameters.AddWithValue("@TradePrice", aModel.TradePrice);
                cmd.Parameters.AddWithValue("@Discount", aModel.Discount);
                cmd.Parameters.AddWithValue("@DiscountStatus", aModel.DiscountStatus);
                cmd.Parameters.AddWithValue("@NormalValue", aModel.NormalValue);
                cmd.Parameters.AddWithValue("@LessAmount", aModel.LessAmount);
                cmd.Parameters.AddWithValue("@RptType", aModel.RptType);
                cmd.Parameters.AddWithValue("@UserName", aModel.UserName);
                cmd.Parameters.AddWithValue("@Category", aModel.Category);
                cmd.Parameters.AddWithValue("@GroupId", aModel.GroupId);
                cmd.Parameters.AddWithValue("@GroupName", aModel.GroupName);
                cmd.Parameters.AddWithValue("@DeliveryDuration", aModel.DeliveryDuration);
                cmd.Parameters.AddWithValue("@SubDeptName", aModel.SubSubDeptName);
                cmd.Parameters.AddWithValue("@SubSubDeptName", aModel.SubSubDeptName);
                cmd.Parameters.AddWithValue("@VaqGroup", aModel.VaqGroup);
                cmd.Parameters.AddWithValue("@VaqName", aModel.VaqName);
                cmd.Parameters.AddWithValue("@VaqPrice", aModel.VaqPrice);
                cmd.Parameters.AddWithValue("@VaqVat", aModel.VaqVat);
                cmd.Parameters.AddWithValue("@VaqComm", aModel.VaqComm);
                cmd.Parameters.AddWithValue("@VaqStatus", aModel.VaqStatus);
                cmd.Parameters.AddWithValue("@TestType", aModel.TestType);
                cmd.Parameters.AddWithValue("@RptCom", aModel.RptCom);
                cmd.Parameters.AddWithValue("@RptComPcorTk", aModel.RptComPcorTk);
                cmd.Parameters.AddWithValue("@ISShow", aModel.ISShow);
                cmd.Parameters.AddWithValue("@CollectionFee", aModel.CollectionFee);
                cmd.Parameters.AddWithValue("@CollFeeTkPC", aModel.CollFeeTkPC);
                cmd.Parameters.AddWithValue("@MedicineFee", aModel.MedicineFee);
                cmd.Parameters.AddWithValue("@MedicineFeeTkPc", aModel.MedicineFeeTkPc);
                cmd.Parameters.AddWithValue("@MedicineFeeName", aModel.MedicineFeeName);
                cmd.Parameters.AddWithValue("@ReportGroup", aModel.ReportGroup);

                int rtn = cmd.ExecuteNonQuery();
                msg = rtn == 1 ? "Update Success" : "Update Fail";
                Con.Close();
                return msg;



            }
            catch (Exception exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }


                return exception.ToString();
            }


        }



        ///////////////////////////LIST





        ///////////////////////////DELETE






    }
}