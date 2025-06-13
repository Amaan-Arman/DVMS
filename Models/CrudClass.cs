using Microsoft.Data.SqlClient;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Net;
using DVMS.Models;


namespace DVMS.Models
{
    public class CrudClass
    {
        private readonly string _connectionString;

        // Inject IConfiguration to access connection string
        public CrudClass(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
        //                if (status.Equals("GetServicePost"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.userID = ToInt32(rdr[1].ToString()); //(int) rdr[0];
        //                        p.title = ToString(rdr[2].ToString());
        //                        p.description = ToString(rdr[3].ToString());
        //                        p.CustomPrice = ToString(rdr[4].ToString());
        //                        p.featuredImg = ToString(rdr[5].ToString());
        //                        p.date = rdr[6].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }
        //                if (status.Equals("GetPostDetailList"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.userID = ToInt32(rdr[1].ToString()); //(int) rdr[0];
        //                        p.UserImg = ToString(rdr[2].ToString());
        //                        p.Name = ToString(rdr[3].ToString());
        //                        p.tagline = ToString(rdr[4].ToString());
        //                        p.aboutme = ToString(rdr[5].ToString());
        //                        p.title = ToString(rdr[6].ToString());
        //                        p.description = ToString(rdr[7].ToString());
        //                        p.CustomPrice = ToString(rdr[8].ToString());
        //                        p.featuredImg = ToString(rdr[9].ToString());
        //                        p.date = rdr[10].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }
        //                if (status.Equals("OfferList"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.userID = ToInt32(rdr[1].ToString()); //(int) rdr[0];
        //                        p.UserImg = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
        //                        p.Name = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
        //                        p.ServiceID = ToInt32(rdr[4].ToString());
        //                        p.ServiceName = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
        //                        p.latitude = rdr[6].ToString();
        //                        p.longitude = rdr[7].ToString();
        //                        p.requestDate = rdr[8].ToString();
        //                        p.offerprice = rdr[9].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }
        //                if (status.Equals("TrackPostRequests"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.bookingID = ToInt32(rdr[0].ToString());
        //                        p.UserImg = rdr[1].ToString();
        //                        p.userID = ToInt32(rdr[2].ToString());
        //                        p.Name = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
        //                        p.ServiceName = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
        //                        p.dateofbooking = rdr[5].ToString();
        //                        p.address = rdr[6].ToString();
        //                        p.date = rdr[7].ToString();
        //                        p.Status = rdr[8].ToString();
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

        //public List<Home> SelectAdmin(string status, string id, string start_date, string end_date, string datetime)
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


        //                if (status.Equals("RequestList"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.userID = ToInt32(rdr[1].ToString()); //(int) rdr[0];
        //                        p.UserImg = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
        //                        p.Name = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
        //                        p.ServiceID = ToInt32(rdr[4].ToString());
        //                        p.ServiceName = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
        //                        p.latitude = rdr[6].ToString();
        //                        p.longitude = rdr[7].ToString();
        //                        p.date = rdr[8].ToString();
        //                        p.time = rdr[9].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }
        //                if (status.Equals("GetPostRequests"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.bookingID = ToInt32(rdr[0].ToString());
        //                        p.UserImg = rdr[1].ToString();
        //                        p.userID = ToInt32(rdr[2].ToString());
        //                        p.Name = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
        //                        p.ServiceName = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
        //                        p.dateofbooking = rdr[5].ToString();
        //                        p.address = rdr[6].ToString();
        //                        p.date = rdr[7].ToString();
        //                        p.Status = rdr[8].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }
        //                if (status.Equals("GetAcceptPost"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.bookingID = ToInt32(rdr[0].ToString());
        //                        p.UserImg = rdr[1].ToString();
        //                        p.userID = ToInt32(rdr[2].ToString());
        //                        p.Name = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
        //                        p.ServiceName = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
        //                        p.dateofbooking = rdr[5].ToString();
        //                        p.address = rdr[6].ToString();
        //                        p.date = rdr[7].ToString();
        //                        p.Status = rdr[8].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }

        //                if (status.Equals("userserviceList"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ServiceID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.ServiceName = ToString(rdr[1].ToString()).ToLower();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }
        //                if (status.Equals("serviceList"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ServiceID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.ServiceName = ToString(rdr[1].ToString()).ToLower();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }
        //                if (status.Equals("GetServicePostadmin"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.title = ToString(rdr[1].ToString());
        //                        p.description = ToString(rdr[2].ToString());
        //                        p.CustomPrice = ToString(rdr[3].ToString());
        //                        p.featuredImg = ToString(rdr[4].ToString());
        //                        p.date = rdr[5].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }

        //                if (status.Equals("HistoryList"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
        //                        p.userID = ToInt32(rdr[1].ToString()); //(int) rdr[0];
        //                        p.UserImg = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
        //                        p.Name = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
        //                        p.ServiceID = ToInt32(rdr[4].ToString());
        //                        p.ServiceName = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
        //                        p.latitude = rdr[6].ToString();
        //                        p.longitude = rdr[7].ToString();
        //                        p.requestDate = rdr[8].ToString();
        //                        p.CustomPrice = rdr[9].ToString();
        //                        p.Status = rdr[10].ToString();
        //                        lst.Add(p);
        //                    }
        //                    rdr.Close();
        //                }
        //                if (status.Equals("GetEarning"))
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        Home p = new Home();
        //                        p.todayEarning = rdr[0].ToString();
        //                        p.yesterdayEarning = rdr[1].ToString();
        //                        p.weeklyEarning = rdr[2].ToString();
        //                        p.monthlyEarning = rdr[3].ToString();
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


                        if (status.Equals("AdministratorSideVerified"))
                        {
                            while (rdr.Read())
                            {
                                login bo = new login();
                                bo.user_credential_id = ToInt32(rdr[0].ToString().Trim());
                                bo.user_name = FirstCharToUpper(ToString(rdr[1].ToString().Trim()).ToLower());
                                bo.login_type = ToString(rdr[2].ToString().Trim());
                                bo.user_mobileNo = ToString(rdr[3].ToString().Trim());
                                bo.user_img = ToString(rdr[4].ToString().Trim());
                                bo.isonline = ToBoolean(rdr[5].ToString().Trim());

                                lst.Add(bo);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("GetProfile"))
                        {
                            while (rdr.Read())
                            {
                                login p = new login();
                                p.user_credential_id = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.user_name = ToString(rdr[1].ToString());
                                p.user_img = ToString(rdr[2].ToString());
                                p.Email = rdr[3].ToString();
                                p.user_mobileNo = rdr[4].ToString();
                                p.tagline = ToString(rdr[5].ToString());
                                p.aboutme = ToString(rdr[6].ToString());
                                p.address = ToString(rdr[7].ToString());
                                p.city = ToString(rdr[8].ToString());
                                p.country = ToString(rdr[9].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("Getuserlist"))
                        {
                            while (rdr.Read())
                            {
                                login p = new login();
                                p.user_credential_id = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.user_name = ToString(rdr[1].ToString());
                                p.user_img = ToString(rdr[2].ToString());
                                p.Email = rdr[3].ToString();
                                p.user_mobileNo = rdr[4].ToString();
                                p.city = ToString(rdr[5].ToString());
                                p.cnic_f = ToString(rdr[6].ToString());
                                p.cnic_b = ToString(rdr[7].ToString());
                                p.date = ToString(rdr[8].ToString());
                                p.time = ToString(rdr[9].ToString());
                                p.active = ToString(rdr[10].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }
                        if (status.Equals("Getuserlistactive"))
                        {
                            while (rdr.Read())
                            {
                                login p = new login();
                                p.user_credential_id = ToInt32(rdr[0].ToString()); //(int) rdr[0];
                                p.user_name = ToString(rdr[1].ToString());
                                p.user_img = ToString(rdr[2].ToString());
                                p.Email = rdr[3].ToString();
                                p.user_mobileNo = rdr[4].ToString();
                                p.city = ToString(rdr[5].ToString());
                                p.cnic_f = ToString(rdr[6].ToString());
                                p.cnic_b = ToString(rdr[7].ToString());
                                p.date = ToString(rdr[8].ToString());
                                p.time = ToString(rdr[9].ToString());
                                p.active = ToString(rdr[10].ToString());
                                lst.Add(p);
                            }
                            rdr.Close();
                        }

                        //if (status.Equals("GetRightList"))
                        //{
                        //    while (rdr.Read())
                        //    {
                        //        login bo = new login();
                        //        bo.user_credential_id = ToInt32(rdr[0].ToString().Trim());
                        //        bo.user_name = FirstCharToUpper(ToString(rdr[1].ToString().Trim()).ToLower());
                        //        bo.module_id = ToInt32(rdr[2].ToString().Trim());
                        //        bo.module_name = ToString(rdr[3].ToString().Trim()).ToLower();
                        //        bo.can_read = ToInt32(rdr[4].ToString().Trim());
                        //        bo.can_create = ToInt32(rdr[5].ToString().Trim());
                        //        bo.can_delete = ToInt32(rdr[6].ToString().Trim());
                        //        bo.can_update = ToInt32(rdr[7].ToString().Trim());
                        //        bo.can_print = ToInt32(rdr[8].ToString().Trim());
                        //        bo.can_report = ToInt32(rdr[9].ToString().Trim());
                        //        lst.Add(bo);
                        //    }
                        //    rdr.Close();
                        //}
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
        //Login System

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
            String r = generator.Next(min, max).ToString("D6");
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
        public Boolean ToBoolean(string value)
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