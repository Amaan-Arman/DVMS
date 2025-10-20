using Microsoft.Data.SqlClient;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Net;
using DVMS.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Mail;
using System.Drawing;
using Azure.Core;


namespace DVMS.Models
{
    public class CrudClass
    {
        private readonly string _connectionString;

        // Inject IConfiguration to access connection string
        public CrudClass(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DVMSConnection");
        }

        public string InsertionMethodStatus(string status, string Field, string Values)
        {
            string db_status = "SP Not Work";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SPInsertion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Field", Field.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Values", Values.ToString().Trim());
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            db_status = ToString(rdr[0].ToString().Trim());
                        }
                        rdr.Close();
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return db_status.ToString().Trim();
        }
        public string UpdationMethodReturn(string status, string Values, string id)
        {
            string db_status = "SP Not Work";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SPUpdation", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Values", Values.ToString().Trim());
                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();


                        while (rdr.Read())
                        {
                            db_status = ToString(rdr[0].ToString().Trim());
                        }
                        rdr.Close();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return db_status.ToString().Trim();
        }


        //public List<Home> SelectHome(string status, string id, string start_date, string end_date, string datetime)
        //{
        //    try
        //    {
        //        List<Home> lst = new List<Home>();
        //        using (SqlConnection con = new SqlConnection(_connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("SpSelection", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
        //                cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
        //                cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
        //                cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
        //                cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
        //                con.Open();
        //                SqlDataReader rdr = cmd.ExecuteReader();

        //                if (status.Equals("SearchServiceList"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.UserImg = ToString(rdr[1].ToString()).ToLower();
        //                        p.Name = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
        //                        p.ServiceID = ToInt32(ToString(rdr[3].ToString()).ToLower());
        //                        p.ServiceName = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
        //                        p.CustomPrice = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
        //                        p.latitude = rdr[6].ToString();
        //                        p.longitude = rdr[7].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }

        //                con.Close();
        //            }
        //        }
        //        return lst;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error in SelectHome: " + ex.Message);
        //    }
        //}

        public List<Admin> SelectAdmin(string status, string id,string datetime, string start_date, string end_date)
        {
            try
            {
                List<Admin> lst = new List<Admin>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (status.Equals("CheckInOutList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.GusetCheckIn = ToInt32(rdr[0].ToString());
                                p.GusetCheckOut = ToInt32(rdr[1].ToString());
                                p.VisitorCheckIn = ToInt32(rdr[2].ToString());
                                p.VisitorCheckOut = ToInt32(rdr[3].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("Topvisitor"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.VisitorId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.VisitorFullName = ToString(rdr[1].ToString());
                                p.HostName = ToString(rdr[2].ToString());
                                p.Floor_no = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.VisitPurpose = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("Topguest"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.InvitationId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.Invitation_ID = ToString(rdr[1].ToString());
                                p.GuestFullName = ToString(rdr[2].ToString());
                                p.HostName = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.Floor_no = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("FloorWiseData"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.CompanyId = ToInt32(rdr[0].ToString());
                                p.Floor_no = ToString(rdr[1].ToString());
                                p.GusetCheckIn = ToInt32(rdr[2].ToString());
                                p.VisitorCheckIn = ToInt32(rdr[3].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("GuestSearchList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.InvitationId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.Invitation_ID = ToString(rdr[1].ToString());
                                p.GuestFullName = ToString(rdr[2].ToString());
                                p.HostName = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.Floor_no = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("GuestOutSearchList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.InvitationId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.Invitation_ID = ToString(rdr[1].ToString());
                                p.GuestFullName = ToString(rdr[2].ToString());
                                p.HostName = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.Floor_no = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("GuestCheckInList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.InvitationId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.Invitation_ID = ToString(rdr[1].ToString());
                                p.GuestFullName = ToString(rdr[2].ToString());
                                p.HostName = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.Floor_no = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("GuestCheckOutList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.InvitationId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.Invitation_ID = ToString(rdr[1].ToString());
                                p.GuestFullName = ToString(rdr[2].ToString());
                                p.HostName = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.Floor_no = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }


                        if (status.Equals("VisitorSearchList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.VisitorId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.VisitorFullName = ToString(rdr[1].ToString());
                                p.HostName = ToString(rdr[2].ToString());
                                p.Floor_no = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.VisitPurpose = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("VisitorOutSearchList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.VisitorId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.VisitorFullName = ToString(rdr[1].ToString());
                                p.HostName = ToString(rdr[2].ToString());
                                p.Floor_no = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.VisitPurpose = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("VisitorCheckInList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.VisitorId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.VisitorFullName = ToString(rdr[1].ToString());
                                p.HostName = ToString(rdr[2].ToString());
                                p.Floor_no = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.VisitPurpose = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("VisitorCheckOutList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.VisitorId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.VisitorFullName = ToString(rdr[1].ToString());
                                p.HostName = ToString(rdr[2].ToString());
                                p.Floor_no = ToString(rdr[3].ToString());
                                p.Company_name = ToString(rdr[4].ToString());
                                p.VisitPurpose = ToString(rdr[5].ToString());
                                p.GusetCheckInTime = ToString(rdr[6].ToString());
                                p.GusetCheckOutTime = ToString(rdr[7].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("ValidateInvitation"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.Status = ToString(rdr[0].ToString()); 
                                p.InvitationId = ToInt32(rdr[1].ToString());
                                p.Hostid = ToInt32(rdr[2].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("GetInvitationList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.Invitation_ID = ToString(rdr[0].ToString()); //(int) rdr[0];
                                p.GuestFullName = ToString(rdr[1].ToString());
                                p.VisitPurpose = ToString(rdr[2].ToString());
                                p.Status = ToString(rdr[3].ToString());
                                p.VisitDate = ToString(rdr[4].ToString());
                                p.VisitTime = ToString(rdr[5].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }


                        if (status.Equals("GetGuestList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.GuestId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.GuestFullName = ToString(rdr[1].ToString());
                                p.GuestEmail = ToString(rdr[2].ToString()); 
                                p.GuestPhone = ToString(rdr[3].ToString());
                                p.GuestCNIC =ToString(rdr[4].ToString());
                                p.Date = ToString(rdr[5].ToString());
                              
                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("GetGuestEmail"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.GuestId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.GuestEmail= ToString(rdr[1].ToString());
                                p.GuestPhone= ToString(rdr[2].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("GetGuestListID"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.GuestId = ToInt32(rdr[0].ToString());
                                p.GuestFullName = ToString(rdr[1].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("GetCompanyList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.Company_id = ToInt32(rdr[0].ToString());
                                p.Company_name = ToString(rdr[1].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("GetDepartmentList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.Department_id = ToInt32(rdr[0].ToString());
                                p.Department_name = ToString(rdr[1].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("GetEmployeeList"))
                        {
                            while (rdr.Read())
                            {
                                Admin p = new Admin();
                                p.Employee_id = ToInt32(rdr[0].ToString());
                                p.Employee_name = ToString(rdr[1].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        

                        con.Close();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SelectHome: " + ex.Message);
            }
        }

        public List<Employee> SelectEmployee(string status, string id, string datetime, string start_date, string end_date)
        {
            try
            {
                List<Employee> lst = new List<Employee>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();

                      
                        if (status.Equals("EmployeeList"))
                        {
                            while (rdr.Read())
                            {
                                Employee p = new Employee();
                                p.employeeId = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.employeeName = ToString(rdr[1].ToString());
                                p.email = ToString(rdr[2].ToString());
                                p.phone = ToString(rdr[3].ToString());
                                p.role = ToString(rdr[4].ToString());
                                p.department = ToString(rdr[5].ToString());
                                p.date_time = ToString(rdr[6].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        con.Close();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SelectHome: " + ex.Message);
            }
        }
        public List<Company> SelectCompany(string status, string id, string datetime, string start_date, string end_date)
        {
            try
            {
                List<Company> lst = new List<Company>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (status.Equals("CompanyList"))
                        {
                            while (rdr.Read())
                            {
                                Company p = new Company();
                                p.companyID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.companyName= ToString(rdr[1].ToString());
                                p.floorno = ToString(rdr[2].ToString());
                                p.email = ToString(rdr[3].ToString());
                                p.phone = ToString(rdr[4].ToString());
                                p.date_time = ToString(rdr[5].ToString());
                                p.image = ToString(rdr[6].ToString());
                                p.tottalguest = ToString(rdr[7].ToString());
                                p.tottalvisitor = ToString(rdr[8].ToString());

                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        con.Close();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SelectHome: " + ex.Message);
            }
        }

        public List<Chat> SelectChatNotificaion(string status, string id, string start_date, string end_date, string datetime)
        {
            try
            {
                List<Chat> lst = new List<Chat>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (status.Equals("NotificationList"))
                        {
                            while (rdr.Read())
                            {
                                Chat p = new Chat();
                                p.receiver_user_id = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.messege_title = ToString(rdr[1].ToString()); //(int) rdr[0];
                                p.messege_text = ToString(rdr[2].ToString()); //(int) rdr[0];
                                p.isread = ToString(rdr[3].ToString()); //(int) rdr[0];
                                p.date = ToString(rdr[4].ToString()); //(int) rdr[0];
                                p.time = ToString(rdr[5].ToString()); //(int) rdr[0];
                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("MessageList"))
                        {
                            while (rdr.Read())
                            {
                                Chat p = new Chat();
                                p.user_id = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.user_img = ToString(rdr[1].ToString()); //(int) rdr[0];
                                p.receiver_user_id = ToInt32(rdr[2].ToString()); //(int) rdr[0];
                                p.messege_title = ToString(rdr[3].ToString()); //(int) rdr[0];
                                p.messege_text = ToString(rdr[4].ToString()); //(int) rdr[0];
                                p.isread = ToString(rdr[5].ToString()); //(int) rdr[0];
                                p.date = ToString(rdr[6].ToString());
                                p.time = ToString(rdr[7].ToString());
                                p.day = ToString(rdr[8].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("UserList"))
                        {
                            while (rdr.Read())
                            {
                                Chat p = new Chat();
                                p.user_id = ToInt32(rdr[0].ToString());
                                p.user_name = ToString(rdr[1].ToString());
                                p.login_type = ToString(rdr[2].ToString());
                                p.user_img = ToString(rdr[3].ToString());
                                p.messege_text = ToString(rdr[4].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("Userdata"))
                        {
                            while (rdr.Read())
                            {
                                Chat p = new Chat();
                                p.user_id = ToInt32(rdr[0].ToString());
                                p.user_name = ToString(rdr[1].ToString());
                                p.login_type = ToString(rdr[2].ToString());
                                p.user_img = ToString(rdr[3].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        if (status.Equals("Chatbox"))
                        {
                            while (rdr.Read())
                            {
                                Chat p = new Chat();
                                p.user_id = ToInt32(rdr[0].ToString());
                                p.user_img = ToString(rdr[1].ToString());
                                p.user_name = ToString(rdr[2].ToString());
                                p.Chat_id = ToInt32(rdr[3].ToString());
                                p.sender_user_id = ToInt32(rdr[4].ToString());
                                p.receiver_user_id = ToInt32(rdr[5].ToString());
                                p.messege_title = ToString(rdr[6].ToString());
                                p.messege_text = ToString(rdr[7].ToString());
                                p.attachmentread = ToString(rdr[8].ToString());
                                p.date = ToString(rdr[9].ToString());
                                p.time = ToString(rdr[10].ToString());
                                p.day = ToString(rdr[11].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        con.Close();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Login System
        public string LoginVerification(string status, string LoginID, string Password)
        {
            string checker = "";
          
            try
            {

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SPLogin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.Trim().ToString());
                        cmd.Parameters.AddWithValue("@LoginID", LoginID.Trim().ToString());
                        cmd.Parameters.AddWithValue("@Password", Password.Trim().ToString());
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();

                    

                        if (status.Equals("ForgetPasswordVerification"))
                        {
                            while (rdr.Read())
                            {
                                checker = ToString(rdr[0].ToString().Trim());
                            }
                            rdr.Close();
                        }
                        if (status.Equals("ResetPasswordEmail"))
                        {
                            while (rdr.Read())
                            {
                                checker = ToString(rdr[0].ToString().Trim());
                            }
                            rdr.Close();
                        }
                        if (status.Equals("ResetPasswordlogin_id"))
                        {
                            while (rdr.Read())
                            {
                                checker = ToString(rdr[0].ToString().Trim());
                            }
                            rdr.Close();
                        }
                        if (status.Equals("ResetPassworduser_name"))
                        {
                            while (rdr.Read())
                            {
                                checker = ToString(rdr[0].ToString().Trim());
                            }
                            rdr.Close();
                        }
                        if (status.Equals("AdministratorSide"))
                        {
                            while (rdr.Read())
                            {
                                checker = ToString(rdr[0].ToString().Trim());
                            }
                            rdr.Close();
                        }
                        if (status.Equals("verification"))
                        {
                            while (rdr.Read())
                            {
                                checker = ToString(rdr[0].ToString().Trim());
                            }
                            rdr.Close();
                        }
                        if (status.Equals("UserRegistration"))
                        {
                            while (rdr.Read())
                            {
                                checker = ToString(rdr[0].ToString().Trim());
                            }
                            rdr.Close();
                        }
                        if (status.Equals("otpverify"))
                        {
                            while (rdr.Read())
                            {
                                checker = ToString(rdr[0].ToString().Trim());
                            }
                            rdr.Close();
                        }


                        con.Close();
                    }
                }
                return checker.ToString().Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<login> loginSession(string status, string LoginID, string Password)
        {
            try
            {
                List<login> lst = new List<login>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SPLogin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.Trim().ToString());
                        cmd.Parameters.AddWithValue("@LoginID", LoginID.Trim().ToString());
                        cmd.Parameters.AddWithValue("@Password", Password.Trim().ToString());

                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (status.Equals("UserList"))
                        {
                            while (rdr.Read())
                            {
                                login i = new login();
                                i.user_credential_id = ToInt32(rdr[0].ToString());
                                i.user_name = ToString(rdr[1].ToString());
                                lst.Add(i);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("EmployeeList"))
                        {
                            while (rdr.Read())
                            {
                                login p = new login();
                                p.user_credential_id = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.user_name = ToString(rdr[1].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("AdministratorSideVerified"))
                        {
                            while (rdr.Read())
                            {
                                login bo = new login();
                                bo.user_credential_id = ToInt32(rdr[0].ToString().Trim());
                                bo.user_name = FirstCharToUpper(ToString(rdr[1].ToString().Trim()).ToLower());
                                bo.Email = ToString(rdr[2].ToString().Trim());
                                bo.user_mobileNo = ToString(rdr[3].ToString().Trim());
                                bo.login_type = ToString(rdr[4].ToString().Trim());
                                bo.department = ToString(rdr[5].ToString().Trim());
                                bo.user_img = ToString(rdr[6].ToString().Trim());

                                bo.module_name = ToString(rdr[7].ToString().Trim());
                                bo.company = ToInt32(rdr[8].ToString().Trim());
                                bo.can_read = ToInt32(rdr[9].ToString().Trim());
                                bo.can_create = ToInt32(rdr[10].ToString().Trim());
                                bo.can_delete = ToInt32(rdr[11].ToString().Trim());
                                bo.can_update = ToInt32(rdr[12].ToString().Trim());
                                bo.can_print = ToInt32(rdr[13].ToString().Trim());
                                bo.can_report = ToInt32(rdr[14].ToString().Trim());


                                lst.Add(bo);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("GetRightList"))
                        {
                            while (rdr.Read())
                            {
                                login bo = new login();
                                bo.user_credential_id = ToInt32(rdr[0].ToString().Trim());
                                bo.user_name = FirstCharToUpper(ToString(rdr[1].ToString().Trim()).ToLower());
                                bo.module_id = ToInt32(rdr[2].ToString().Trim());
                                bo.module_name = ToString(rdr[3].ToString().Trim()).ToLower();
                                bo.can_read = ToInt32(rdr[4].ToString().Trim());
                                bo.can_create = ToInt32(rdr[5].ToString().Trim());
                                bo.can_delete = ToInt32(rdr[6].ToString().Trim());
                                bo.can_update = ToInt32(rdr[7].ToString().Trim());
                                bo.can_print = ToInt32(rdr[8].ToString().Trim());
                                bo.can_report = ToInt32(rdr[9].ToString().Trim());

                                lst.Add(bo);
                            }
                            rdr.Close();
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string BackOfficeInsertion(string status, string field, string value, string module_permission_id, string id, string column_name, string result)
        {
            string db_status = "SP Not Work";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SPRightsInsertion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status.Trim().ToString());
                        cmd.Parameters.AddWithValue("@field", field.Trim().ToString());
                        cmd.Parameters.AddWithValue("@value", value.Trim().ToString());
                        cmd.Parameters.AddWithValue("@module_permission_id", module_permission_id.Trim().ToString());
                        cmd.Parameters.AddWithValue("@id", id.Trim().ToString());
                        cmd.Parameters.AddWithValue("@column_name", column_name.Trim().ToString());
                        cmd.Parameters.AddWithValue("@result", result.ToString().Trim());
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            db_status = ToString(rdr[0].ToString().Trim());
                        }
                        rdr.Close();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return db_status.ToString().Trim();
        }
        //Login System
        public void CredentialSendEmail(string to_email, string name, string LoginId, string password)
        {
            string subject = "Credential Alert";
            string mailBodyhtml = "Dear " + name +
              ",<br><br>The Digital Visitor Management system Credentials: " +
              "<br><br>URL : Link <br><br>User ID: <strong>" + LoginId + "</strong>" +
              "<br>Password: <strong>" + password + "</strong>" +
              "<br><br>Thank You" +
              "<br><br><br><br><strong>The Digital Visitor Management system</strong><br><strong>Digital Business Solutions Provider</strong><br>";

            // Correctly set the sender's email and name
            var msg = new MailMessage
            {
                From = new MailAddress("amaanarman99@gmail.com", "The Inspection Pro"), // Correct sender format
                Subject = subject,
                Body = mailBodyhtml,
                IsBodyHtml = true
            };
            msg.To.Add(to_email); // Add the recipient email address

            // Configure SMTP client
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("amaanarman99@gmail.com", "ynmw ques ukae qmab"); // Replace with App Password
                smtpClient.EnableSsl = true; // Enable SSL

                // Send the email
                smtpClient.Send(msg);
            }

            // Clear variables (optional, handled by GC)
            mailBodyhtml = null;
            subject = null;
            msg.Dispose();
        }

        //public void InvitationSendEmail(string to_email, string hostname, string guestname, Bitmap image,string date, string time)
        //{
        //    string subject = "Guest Invitation from " + hostname;

        //    string mailBodyhtml = $@"
        //    <p>Dear {guestname},</p>

        //    <p>You are warmly invited by <strong>{hostname}</strong> to visit our facility.</p>

        //    <p>
        //    <strong>Guest Name:</strong> {guestname}<br>
        //    <strong>Host:</strong> {hostname}<br>
        //    <strong>Date & Time:</strong>{date} {time} <br>
        //    <strong>Location:</strong> Karchi, Pakistan<br>
        //    </p>

        //    <p>
        //    For your convenience, we have attached your QR code/entry pass below. 
        //    Kindly present this at the reception desk upon arrival.
        //    </p>

        //    <p>
        //    <img src='cid:GuestPassImage' alt='Guest QR Code' /><br>
        //    </p>

        //    <p>We look forward to welcoming you!</p>

        //    <br><br>
        //    Best Regards,<br>
        //    <strong>The Inspection Pro Team</strong><br>
        //    Digital Business Solutions Provider
        //    ";

        //    var msg = new MailMessage
        //    {
        //        From = new MailAddress("amaanarman99@gmail.com", "The Inspection Pro"),
        //        Subject = subject,
        //        IsBodyHtml = true
        //    };
        //    msg.To.Add(to_email);

        //    // Convert Bitmap to MemoryStream for inline image
        //    using (var ms = new MemoryStream())
        //    {
        //        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //        ms.Position = 0;

        //        // Attach inline image
        //        var inlineImage = new LinkedResource(ms, "image/png")
        //        {
        //            ContentId = "GuestPassImage",
        //            TransferEncoding = System.Net.Mime.TransferEncoding.Base64
        //        };

        //        var altView = AlternateView.CreateAlternateViewFromString(mailBodyhtml, null, "text/html");
        //        altView.LinkedResources.Add(inlineImage);
        //        msg.AlternateViews.Add(altView);
        //    }

        //    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
        //    {
        //        smtpClient.Credentials = new NetworkCredential("amaanarman99@gmail.com", "ynmw ques ukae qmab"); // App Password
        //        smtpClient.EnableSsl = true;
        //        smtpClient.Send(msg);
        //    }
        //    msg.Dispose();
        //}
        public void InvitationSendEmail(string to_email, string hostname, string guestname, Bitmap image, string date, string time)
        {
            string subject = "Guest Invitation from " + hostname;

            string mailBodyhtml = $@"
            <p>Dear {guestname},</p>

            <p>You are warmly invited by <strong>{hostname}</strong> to visit our facility.</p>

            <p>
            <strong>Guest Name:</strong> {guestname}<br>
            <strong>Host:</strong> {hostname}<br>
            <strong>Date & Time:</strong> {date} {time}<br>
            <strong>Location:</strong> Karachi, Pakistan<br>
            </p>

            <p>
            For your convenience, we have attached your QR code/entry pass below. 
            Kindly present this at the reception desk upon arrival.
            </p>

            <p>
            <img src='cid:GuestPassImage' alt='Guest QR Code' /><br>
            </p>

            <p>We look forward to welcoming you!</p>

            <br><br>
            Best Regards,<br>
            <strong>The Inspection Pro Team</strong><br>
            Digital Business Solutions Provider
            ";

            var msg = new MailMessage
            {
                From = new MailAddress("amaanarman99@gmail.com", "The Inspection Pro"),
                Subject = subject,
                IsBodyHtml = true
            };
            msg.To.Add(to_email);

            // Convert Bitmap to byte array first (avoid stream disposal issues)
            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageBytes = ms.ToArray();
            }

            // Create new MemoryStream from byte array (kept alive until send finishes)
            var msImage = new MemoryStream(imageBytes);

            var inlineImage = new LinkedResource(msImage, "image/png")
            {
                ContentId = "GuestPassImage",
                TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            };

            var altView = AlternateView.CreateAlternateViewFromString(mailBodyhtml, null, "text/html");
            altView.LinkedResources.Add(inlineImage);
            msg.AlternateViews.Add(altView);

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("amaanarman99@gmail.com", "ynmw ques ukae qmab"); // App Password
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);
            }

            msg.Dispose();
            msImage.Dispose();
        }


        public string opt_num()
        {
            int length = 4;
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public bool DatabaseConnectionCheck()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        public bool CheckForInternetConnection()
        {
            //try
            //{
            //    using (var client = new WebClient())
            //    using (client.OpenRead("http://clients3.google.com/generate_204"))
            //    {
            //        return true;
            //    }
            //}
            //catch
            //{
            //    return false;
            //}
            return true;
        }


        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            Random generator = new Random();
            System.String r = generator.Next(min, max).ToString("D6");
            return ToInt32(r);
        }
        public string Generatepassword()
        {
            int length = 6;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public string Suffix(int integer)
        {
            switch (integer % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }
            switch (integer % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
        public string FirstCharToUpper(string value)
        {
            char[] array = value.ToCharArray();
            char index = 'a';
            // Handle the first letter in the string.  
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.  
            // ... Uppercase the lowercase letters following spaces.  
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
                if (array[i - 1] == '(')
                {
                    index = array[i - 1];
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
                //if(index == '(' && array[i - 1] != ')')
                //{
                //    if (char.IsLower(array[i]))
                //    {
                //        array[i] = char.ToUpper(array[i]);
                //    }
                //}

            }
            return new string(array);
        }
        public string ToString(string value)
        {

            if (value == null)
            {
                return "---";
            }
            else if (value == "")
            {
                return "---";
            }
            return value;

        }
        public int ToInt32(string value)
        {

            if (value == null)
            {
                return 0;
            }
            else if (value == "")
            {
                return 0;
            }
            return (int)Convert.ToDouble(value);

        }
        public System.Boolean ToBoolean(string value)
        {

            if (value == null)
            {
                return false;
            }
            else if (value == "")
            {
                return false;
            }
            else if (value == "0")
            {
                return Convert.ToBoolean(ToInt32(value));
            }
            else if (value == "1")
            {
                return Convert.ToBoolean(ToInt32(value));
            }
            return Convert.ToBoolean(value);

        }
        public DateTime ToDate(string date)
        {
            if (date == "")
            {
                return Convert.ToDateTime("0000-00-00");
            }
            if (date == null)
            {
                return Convert.ToDateTime("0000-00-00");
            }
            return Convert.ToDateTime(date);
        }
        public double ConvertBytesToMB(long bytes)
        {
            return bytes / (1024.0 * 1024.0); // Divide by 1024 * 1024 to convert to MB
        }

        //public void WriteEventLog(string message)
        //{
        //    StreamWriter sw = null;
        //    try
        //    {
        //        //string path = Server.MapPath("/BranchAttendance/");
        //        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/ErrorLog"));
        //        if (!exists)
        //        {
        //            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/ErrorLog"));
        //        }
        //        HttpContext.Current.Server.MapPath("/ErrorLog/");
        //        string targetPath = HttpContext.Current.Server.MapPath("/ErrorLog/");// @"E:\Transferfiles\";
        //        string date = DateTime.Now.ToString("dd-MMM-yyyy");
        //        sw = new StreamWriter(targetPath + date + ".txt", true);
        //        sw.WriteLine(DateTime.Now.ToString() + " : " + message);
        //        sw.Flush();
        //        sw.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}


        //public string NParenthesis(decimal value)
        //{
        //    if(value<-1)
        //    {
        //        return "(" + value + ")";
        //    }
        //    return value.ToString();
        //}             
    }
}