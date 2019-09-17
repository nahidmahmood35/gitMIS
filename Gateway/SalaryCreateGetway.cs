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
    public class SalaryCreateGetway : DbConnection
    {
        private SqlTransaction _trans;
        public Task<string> Save(EmployeeModel aModel)
        {
            try
            {
              string FirstDayMonth = aModel.AttYear.ToString()+"-"+aModel.MonthId.ToString().PadLeft(2,'0') +"-"+"01";
              Con.Open();
              _trans = Con.BeginTransaction();
                //-----------------------------------------------------------------------------------------old
              //var lists2 = new List<EmployeeModel>();
              ////const string query5 = @"Select DISTINCT EmpId,LoanTypeId,Sum(LoanAmt-PaidAmount) AS 'RestLoanAmount'  from tbl_HR_LOAN_LEDGER GROUP BY EmpId,LoanTypeId HAVING Sum(LoanAmt-PaidAmount)=0";
              //  const string query5 = @"Select DISTINCT EmpId,LoanTypeId,Sum(LoanAmt-PaidAmount) AS 'RestLoanAmount'  from tbl_HR_LOAN_LEDGER GROUP BY EmpId,LoanTypeId";
              //var cmd5 = new SqlCommand(query5, Con, _trans);
              //SqlDataReader rdr2 = cmd5.ExecuteReader();
              //while (rdr2.Read())
              //{
              //    lists2.Add(new EmployeeModel()
              //    {

              //        EmId = Convert.ToInt32(rdr2["EmpId"]),
              //        LoanTypeId = Convert.ToInt32(rdr2["LoanTypeId"]),
              //        RestLoanAmount = Convert.ToDouble(rdr2["RestLoanAmount"]),
                    
              //    });
              //}
              //rdr2.Close();
              //foreach (var EmployeeDetalsUpdate in lists2)
              //{
              //    const string query6 = @"update tbl_HR_GLO_EMPLOYEE_DLS SET Amount=@Amount Where EmpId=@EmpId AND SalaryTypeId=@SalaryTypeId";
              //    var cmd6 = new SqlCommand(query6, Con, _trans);
              //    cmd6.Parameters.Clear();
              //    cmd6.Parameters.AddWithValue("@EmpId", EmployeeDetalsUpdate.EmId);
              //    cmd6.Parameters.AddWithValue("@Amount", EmployeeDetalsUpdate.RestLoanAmount);
              //    cmd6.Parameters.AddWithValue("@SalaryTypeId", EmployeeDetalsUpdate.LoanTypeId);
              //    cmd6.ExecuteNonQuery();
              //  }

               //-------------------------------------------------------------------------------------------old
              var lists = new List<EmployeeModel>();
              const string query2 = @"Select EmpId,LoanTypeId,LoanNo,Sum(LoanAmt-PaidAmount) AS 'RestLoanAmount',InstallmentSize,LoanAndAdvance from tbl_HR_LOAN_LEDGER GROUP BY EmpId,LoanTypeId,LoanNo,InstallmentSize,LoanAndAdvance HAVING Sum(LoanAmt-PaidAmount)>0";
              var cmd2 = new SqlCommand(query2, Con, _trans);
              SqlDataReader rdr = cmd2.ExecuteReader();

              while (rdr.Read())
              {
                  lists.Add(new EmployeeModel()
                  {

                      EmId = Convert.ToInt32(rdr["EmpId"]),
                      LoanTypeId = Convert.ToInt32(rdr["LoanTypeId"]),
                      LoanNo = Convert.ToInt32(rdr["LoanNo"]),
                      RestLoanAmount = Convert.ToDouble(rdr["RestLoanAmount"]),
                      InstallmentSize = Convert.ToDouble(rdr["InstallmentSize"]),
                      LoanAndAdvance = Convert.ToInt32(rdr["LoanAndAdvance"]),
                  });
              }
              rdr.Close();

              //-------------------------------------------------------------------------------------------

              foreach (var loanLedger in lists)
              {
                  string trNo = GetTrNo("TrNo", "tbl_HR_LOAN_LEDGER", _trans);
                  if (loanLedger.RestLoanAmount > loanLedger.InstallmentSize)
                  {
                      const string query3 = @"INSERT INTO tbl_HR_LOAN_LEDGER (EmpId,TrNo,TrDate,MonthId,AttYear,LoanTypeId,LoanNo,LoanAmt,InstallmentSize,PaidAmount,ExcemptAmount,LoanAndAdvance,LoanStatus,UserName,EntryDate) VALUES ( @EmpId,@TrNo,@TrDate,@MonthId,@AttYear,@LoanTypeId,@LoanNo,@LoanAmt,@InstallmentSize,@PaidAmount,@ExcemptAmount,@LoanAndAdvance,@LoanStatus,@UserName,@EntryDate)";
                      var cmd3 = new SqlCommand(query3, Con, _trans);
                      cmd3.Parameters.Clear();
                      cmd3.Parameters.AddWithValue("@EmpId", loanLedger.EmId);
                      cmd3.Parameters.AddWithValue("@TrNo", trNo);
                      cmd3.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                      cmd3.Parameters.AddWithValue("@MonthId", DateTime.Now.ToString("MM"));
                      cmd3.Parameters.AddWithValue("@AttYear", DateTime.Now.ToString("yyyy"));
                      cmd3.Parameters.AddWithValue("@LoanTypeId", loanLedger.LoanTypeId);
                      cmd3.Parameters.AddWithValue("@LoanNo", loanLedger.LoanNo);
                      cmd3.Parameters.AddWithValue("@LoanAmt", 0);
                      cmd3.Parameters.AddWithValue("@InstallmentSize", loanLedger.InstallmentSize);
                      cmd3.Parameters.AddWithValue("@PaidAmount", loanLedger.InstallmentSize);
                      cmd3.Parameters.AddWithValue("@ExcemptAmount",0);
                      cmd3.Parameters.AddWithValue("@LoanAndAdvance", loanLedger.LoanAndAdvance);
                      cmd3.Parameters.AddWithValue("@LoanStatus", "payment");
                      cmd3.Parameters.AddWithValue("@UserName", "NotDevelopment");
                      cmd3.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                      cmd3.ExecuteNonQuery();

                      const string query4 = @"update tbl_HR_GLO_EMPLOYEE_DLS SET Amount=@Amount Where EmpId=@EmpId AND SalaryTypeId=@SalaryTypeId";
                      var cmd4 = new SqlCommand(query4, Con, _trans);
                      cmd4.Parameters.Clear();
                      cmd4.Parameters.AddWithValue("@EmpId", loanLedger.EmId);
                      cmd4.Parameters.AddWithValue("@Amount", loanLedger.InstallmentSize);
                      cmd4.Parameters.AddWithValue("@SalaryTypeId", loanLedger.LoanTypeId);
                      cmd4.ExecuteNonQuery();

                  }
                  else
                  {
                      const string query3 = @"INSERT INTO tbl_HR_LOAN_LEDGER (EmpId,TrNo,TrDate,MonthId,AttYear,LoanTypeId,LoanNo,LoanAmt,InstallmentSize,PaidAmount,ExcemptAmount,LoanAndAdvance,LoanStatus,UserName,EntryDate) VALUES ( @EmpId,@TrNo,@TrDate,@MonthId,@AttYear,@LoanTypeId,@LoanNo,@LoanAmt,@InstallmentSize,@PaidAmount,@ExcemptAmount,@LoanAndAdvance,@LoanStatus,@UserName,@EntryDate)";
                      var cmd3 = new SqlCommand(query3, Con, _trans);
                      cmd3.Parameters.Clear();
                      cmd3.Parameters.AddWithValue("@EmpId", loanLedger.EmId);
                      cmd3.Parameters.AddWithValue("@TrNo", trNo);
                      cmd3.Parameters.AddWithValue("@TrDate", DateTime.Now.ToString("yyyy-MM-dd"));
                      cmd3.Parameters.AddWithValue("@MonthId", DateTime.Now.ToString("MM"));
                      cmd3.Parameters.AddWithValue("@AttYear", DateTime.Now.ToString("yyyy"));
                      cmd3.Parameters.AddWithValue("@LoanTypeId", loanLedger.LoanTypeId);
                      cmd3.Parameters.AddWithValue("@LoanNo", loanLedger.LoanNo);
                      cmd3.Parameters.AddWithValue("@LoanAmt", 0);
                      cmd3.Parameters.AddWithValue("@InstallmentSize", loanLedger.RestLoanAmount);
                      cmd3.Parameters.AddWithValue("@PaidAmount", loanLedger.InstallmentSize);
                      cmd3.Parameters.AddWithValue("@ExcemptAmount", 0);
                      cmd3.Parameters.AddWithValue("@LoanAndAdvance", loanLedger.LoanAndAdvance);
                      cmd3.Parameters.AddWithValue("@LoanStatus", "payment");
                      cmd3.Parameters.AddWithValue("@UserName", "NotDevelopment");
                      cmd3.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
                      cmd3.ExecuteNonQuery();

                      const string query4 = @"update tbl_HR_GLO_EMPLOYEE_DLS SET Amount=@Amount Where EmpId=@EmpId AND SalaryTypeId=@SalaryTypeId";
                      var cmd4 = new SqlCommand(query4, Con, _trans);
                      cmd4.Parameters.Clear();
                      cmd4.Parameters.AddWithValue("@EmpId", loanLedger.EmId);
                      cmd4.Parameters.AddWithValue("@Amount", 0);
                      cmd4.Parameters.AddWithValue("@SalaryTypeId", loanLedger.LoanTypeId);
                      cmd4.ExecuteNonQuery();
                  }
                 
              }
            //-------------------------------------------------------------------------------------------------------------------------
//              const string query = @"INSERT INTO tbl_HR_GLO_PAYREGISTER (MonthId,Year,EntryDate,FirstDate,SalaryCustomId,Amount,EmpId,SalaryType,SalaryId) 
//                                       SELECT @MonthId,@AttYear,@EntryDate,@FirstDate,b.CustomId,a.Amount,a.EmpId,a.SalaryType,a.SalaryTypeId 
//                                        from tbl_HR_GLO_EMPLOYEE_DLS AS a inner join tbl_HR_GLO_SALARY_TYPE AS b ON a.SalaryTypeId = b.Id";
                const string query =
                    @"INSERT INTO tbl_HR_GLO_PAYREGISTER (MonthId,Year,EntryDate,FirstDate,PaymentAmountBankPc,PaymentAmountCashPc,SalaryCustomId,Amount,EmpId,SalaryType,SalaryId,ProjectId,DeparmentId,DesignationId,EmployeeBankAccountNo,EmployeeBankId,EmployeeBankBranchId) 
                                       	
																		  
                         SELECT @MonthId,@AttYear,@EntryDate,@FirstDate,c.PaymentAmountBankPc,c.PaymentAmountCashPc,b.CustomId,a.Amount,a.EmpId,a.SalaryType,a.SalaryTypeId,c.ProjectId,c.DeparmentId,c.DesignationId,c.EmployeeBankAccountNo,c.EmployeeBankId,c.EmployeeBankBranchId
                        from tbl_HR_GLO_EMPLOYEE_DLS AS a inner join tbl_HR_GLO_SALARY_TYPE AS b ON a.SalaryTypeId = b.Id
                        inner join tbl_HR_GLO_EMPLOYEE AS c ON a.EmpId = c.Id	
																		  															  
                        union All 

                        SELECT @MonthId,@AttYear,@EntryDate,@FirstDate,c.PaymentAmountBankPc,c.PaymentAmountCashPc,N'মোট বেতন',SUM(a.Amount),a.EmpId,a.SalaryType,'5000',c.ProjectId,c.DeparmentId,c.DesignationId,c.EmployeeBankAccountNo,c.EmployeeBankId,c.EmployeeBankBranchId
                        from tbl_HR_GLO_EMPLOYEE_DLS AS a inner join tbl_HR_GLO_SALARY_TYPE AS b ON a.SalaryTypeId = b.Id
                          inner join tbl_HR_GLO_EMPLOYEE AS c ON a.EmpId = c.Id	
                          where a.SalaryType = 'e'
                        group by a.EmpId,a.SalaryType,c.ProjectId,c.DeparmentId,c.DesignationId,c.EmployeeBankAccountNo,c.EmployeeBankId,c.EmployeeBankBranchId,c.PaymentAmountBankPc,c.PaymentAmountCashPc
                        union All 
                        SELECT @MonthId,@AttYear,@EntryDate,@FirstDate,c.PaymentAmountBankPc,c.PaymentAmountCashPc,N'মোট কর্তন',SUM(a.Amount),a.EmpId,a.SalaryType,'10000',c.ProjectId,c.DeparmentId,c.DesignationId,c.EmployeeBankAccountNo,c.EmployeeBankId,c.EmployeeBankBranchId
                        from tbl_HR_GLO_EMPLOYEE_DLS AS a inner join tbl_HR_GLO_SALARY_TYPE AS b ON a.SalaryTypeId = b.Id
                          inner join tbl_HR_GLO_EMPLOYEE AS c ON a.EmpId = c.Id	
                          where a.SalaryType = 'd'
                        group by a.EmpId,a.SalaryType,c.ProjectId,c.DeparmentId,c.DesignationId,c.EmployeeBankAccountNo,c.EmployeeBankId,c.EmployeeBankBranchId,c.PaymentAmountBankPc,c.PaymentAmountCashPc
                        union All
                        SELECT @MonthId,@AttYear,@EntryDate,@FirstDate,c.PaymentAmountBankPc,c.PaymentAmountCashPc,N'মোট প্রাপ্তি',(SELECT SUM(Amount) FROM tbl_HR_GLO_EMPLOYEE_DLS WHERE SalaryType = 'e' AND EmpId=a.EmpId )-SUM(a.Amount),a.EmpId,'','15000',c.ProjectId,c.DeparmentId,c.DesignationId,c.EmployeeBankAccountNo,c.EmployeeBankId,c.EmployeeBankBranchId
                        from tbl_HR_GLO_EMPLOYEE_DLS AS a inner join tbl_HR_GLO_SALARY_TYPE AS b ON a.SalaryTypeId = b.Id
                          inner join tbl_HR_GLO_EMPLOYEE AS c ON a.EmpId = c.Id	
                          where a.SalaryType = 'd'
                        group by a.EmpId,c.ProjectId,c.DeparmentId,c.DesignationId,c.EmployeeBankAccountNo,c.EmployeeBankId,c.EmployeeBankBranchId,c.PaymentAmountBankPc,c.PaymentAmountCashPc";
                
               // _trans = Con.BeginTransaction();
                var cmd = new SqlCommand(query, Con, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@MonthId", aModel.MonthId);
                cmd.Parameters.AddWithValue("@AttYear", aModel.AttYear);
                cmd.Parameters.AddWithValue("@EntryDate", DateTime.Today.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@FirstDate", FirstDayMonth);
           
                cmd.ExecuteNonQuery();
               



                _trans.Commit();
                Con.Close();
                return Task.FromResult<string>("Save uccess");
                
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