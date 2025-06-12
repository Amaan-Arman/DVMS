//using DVMS.Models;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;


//namespace DVMS.Models
//{
//    public class CrudClass
//    {
//        string connectionStringSP = ConfigurationManager.ConnectionStrings["connectionStringSP"].ToString();

//        public string InsertionReturnMethod2(string status, string Field, string Values, int ID)
//        {
//            int id = 0;
//            try
//            {
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("add_to_cart", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Field", Field.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Values", Values.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@ID", ID.ToString().Trim());
//                        con.Open();

//                        SqlDataReader rdr = cmd.ExecuteReader();
//                        while (rdr.Read())
//                        {
//                            id = ToInt32(rdr[0].ToString());
//                        }

//                        rdr.Close();
//                        con.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return id.ToString();
//        }
//        public string InsertionReturnMethod(string status, string Field, string Values)
//        {
//            int id = 0;
//            try
//            {
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SPInsertion", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Field", Field.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Values", Values.ToString().Trim());
//                        con.Open();

//                        SqlDataReader rdr = cmd.ExecuteReader();
//                        while (rdr.Read())
//                        {
//                            id = ToInt32(rdr[0].ToString());
//                        }

//                        rdr.Close();
//                        con.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return id.ToString();
//        }
//        public string InsertionMethodStatus(string status, string Field, string Values)
//        {
//            string db_status = "SP Not Work";
//            try
//            {
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("Insertion", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Field", Field.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Values", Values.ToString().Trim());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        while (rdr.Read())
//                        {
//                            db_status = ToString(rdr[0].ToString().Trim());
//                        }
//                        rdr.Close();
//                        con.Close();
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return db_status.ToString().Trim();
//        }
//        public void InsertionMethod(string status, string Field, string values)
//        {
//            try
//            {
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("Insertion", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Field", Field.ToString());
//                        cmd.Parameters.AddWithValue("@values", values);


//                        con.Open();
//                        cmd.ExecuteNonQuery();
//                        con.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//        public void UpdationMethod(string status, string Values, string id)
//        {
//            try
//            {
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SPUpdation", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Values", Values.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
//                        con.Open();
//                        cmd.ExecuteNonQuery();
//                        con.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//        public string UpdationMethodReturn(string status, string Values, string id)
//        {
//            string db_status = "SP Not Work";
//            try
//            {
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SPUpdation", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@Values", Values.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();


//                        while (rdr.Read())
//                        {
//                            db_status = ToString(rdr[0].ToString().Trim());
//                        }
//                        rdr.Close();
//                        con.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return db_status.ToString().Trim();
//        }


//        //Selection
//        public List<Filemanager> SelectDocument(string status, string id, string start_date, string end_date, string datetime)
//        {
//            try
//            {
//                List<Filemanager> lst = new List<Filemanager>();
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        if (status.Equals("DocumentList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Filemanager p = new Filemanager();
//                                p.Doc_ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.user_id = ToInt32(rdr[1].ToString()); //(int) rdr[0];
//                                p.owner = ToString(rdr[2].ToString()); //(int) rdr[0];
//                                p.File_name = ToString(rdr[3].ToString());
//                                p.file_size = ToString(rdr[4].ToString());
//                                p.user_name = ToString(rdr[5].ToString());
//                                p.user_img = ToString(rdr[6].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }

//                        con.Close();
//                    }
//                }
//                return lst;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public List<Chat> SelectChatNotificaion(string status, string id, string start_date, string end_date, string datetime)
//        {
//            try
//            {
//                List<Chat> lst = new List<Chat>();
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        if (status.Equals("NotificationList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Chat p = new Chat();
//                                p.receiver_user_id = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.messege_title= ToString(rdr[1].ToString()); //(int) rdr[0];
//                                p.messege_text = ToString(rdr[2].ToString()); //(int) rdr[0];
//                                p.isread = ToString(rdr[3].ToString()); //(int) rdr[0];
//                                p.date = ToString(rdr[4].ToString()); //(int) rdr[0];
//                                p.time = ToString(rdr[5].ToString()); //(int) rdr[0];
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("MessageList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Chat p = new Chat();
//                                p.user_id = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.user_img = ToString(rdr[1].ToString()); //(int) rdr[0];
//                                p.receiver_user_id = ToInt32(rdr[2].ToString()); //(int) rdr[0];
//                                p.messege_title = ToString(rdr[3].ToString()); //(int) rdr[0];
//                                p.messege_text = ToString(rdr[4].ToString()); //(int) rdr[0];
//                                p.isread = ToString(rdr[5].ToString()); //(int) rdr[0];
//                                p.date = ToString(rdr[6].ToString());
//                                p.time = ToString(rdr[7].ToString());
//                                p.day = ToString(rdr[8].ToString());
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
                        
//                        if (status.Equals("UserList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Chat p = new Chat();
//                                p.user_id = ToInt32(rdr[0].ToString()); 
//                                p.user_name = ToString(rdr[1].ToString()); 
//                                p.login_type = ToString(rdr[2].ToString()); 
//                                p.user_img = ToString(rdr[3].ToString());
//                                p.messege_text = ToString(rdr[4].ToString());
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("Userdata"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Chat p = new Chat();
//                                p.user_id = ToInt32(rdr[0].ToString());
//                                p.user_name = ToString(rdr[1].ToString());
//                                p.login_type = ToString(rdr[2].ToString());
//                                p.user_img = ToString(rdr[3].ToString());
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
                        
//                        if (status.Equals("Chatbox"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Chat p = new Chat();
//                                p.user_id = ToInt32(rdr[0].ToString());
//                                p.user_img = ToString(rdr[1].ToString());
//                                p.user_name = ToString(rdr[2].ToString());
//                                p.Chat_id = ToInt32(rdr[3].ToString());
//                                p.sender_user_id = ToInt32(rdr[4].ToString());
//                                p.receiver_user_id = ToInt32(rdr[5].ToString());
//                                p.messege_title = ToString(rdr[6].ToString());
//                                p.messege_text = ToString(rdr[7].ToString());
//                                p.attachmentread = ToString(rdr[8].ToString());
//                                p.date = ToString(rdr[9].ToString());
//                                p.time = ToString(rdr[10].ToString());
//                                p.day = ToString(rdr[11].ToString());
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }

//                        con.Close();
//                    }
//                }
//                return lst;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public List<Product> SelectProduct(string status, string id, string start_date, string end_date, string datetime)
//        {
//            try
//            {
//                List<Product> lst = new List<Product>();
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        if (status.Equals("ProductList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.Extinguisher_ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.SerialNumber = FirstCharToUpper(ToString(rdr[1].ToString()).ToLower());
//                                p.Location_ = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Type = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
//                                p.Capacity = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
//                                p.Manufacturer = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
//                                p.Status = FirstCharToUpper(ToString(rdr[6].ToString()).ToLower());
//                                p.Remarks = FirstCharToUpper(ToString(rdr[7].ToString()).ToLower());
//                                p.Last_Inspection_Date = ToString(rdr[8].ToString());
//                                p.Date_of_Manufacture = ToString(rdr[9].ToString());
//                                p.Date = ToString(rdr[10].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("ProductSearchList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.Extinguisher_ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.SerialNumber = FirstCharToUpper(ToString(rdr[1].ToString()).ToLower());
//                                p.Location_ = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Type = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
//                                p.Capacity = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
//                                p.Manufacturer = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
//                                p.Status = FirstCharToUpper(ToString(rdr[6].ToString()).ToLower());
//                                p.Remarks = FirstCharToUpper(ToString(rdr[7].ToString()).ToLower());
//                                p.Last_Inspection_Date = ToString(rdr[8].ToString());
//                                p.Date_of_Manufacture = ToString(rdr[9].ToString());
//                                p.Date = ToString(rdr[10].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("ProductEdit"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.Extinguisher_ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.Location_ = FirstCharToUpper(ToString(rdr[1].ToString()).ToLower());
//                                p.Type = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Capacity = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
//                                p.Manufacturer = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
//                                //p.Status = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
//                                p.Remarks = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
//                                p.Date_of_Manufacture = ToString(rdr[6].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("ProductDetail"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.Extinguisher_ID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.SerialNumber = FirstCharToUpper(ToString(rdr[1].ToString()).ToLower());
//                                p.Location_ = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Type = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
//                                p.Capacity = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
//                                p.Manufacturer = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());
//                                p.Status = FirstCharToUpper(ToString(rdr[6].ToString()).ToLower());
//                                p.Remarks = FirstCharToUpper(ToString(rdr[7].ToString()).ToLower());
//                                p.Last_Inspection_Date = ToString(rdr[8].ToString());
//                                p.Date_of_Manufacture = ToString(rdr[9].ToString());
//                                p.Date = ToString(rdr[10].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("DetailpendingInspection"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.SerialNumber = FirstCharToUpper(ToString(rdr[0].ToString()).ToLower());
//                                p.Location_ = FirstCharToUpper(ToString(rdr[1].ToString()).ToLower());
//                                p.Type = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Capacity = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
//                                p.Manufacturer = FirstCharToUpper(ToString(rdr[4].ToString()).ToLower());
//                                p.Status = FirstCharToUpper(ToString(rdr[5].ToString()).ToLower());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
                        
//                        if (status.Equals("ReportList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.inspector_name = ToString(rdr[0].ToString());
//                                p.SerialNumber = ToString(rdr[1].ToString());
//                                p.Status = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Date = ToString(rdr[3].ToString());
//                                p.time = ToString(rdr[4].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("ReportSearchList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.inspector_name = ToString(rdr[0].ToString());
//                                p.SerialNumber = ToString(rdr[1].ToString());
//                                p.Status = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Date = ToString(rdr[3].ToString());
//                                p.time = ToString(rdr[4].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
                        
//                        if (status.Equals("totalproduct"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.totalproduct = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("totalinspector"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.totalinspector = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("pendingInspection"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.pendinginspection = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("InspectionHistory"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.inspector_name = ToString(rdr[0].ToString());
//                                p.SerialNumber = ToString(rdr[1].ToString());
//                                p.Status = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Date = ToString(rdr[3].ToString());
//                                p.time = ToString(rdr[4].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("InspectionStatus"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.green = ToInt32(rdr[0].ToString());
//                                p.orange = ToInt32(rdr[1].ToString());
//                                p.red = ToInt32(rdr[2].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("DocumentList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.inspector_name = ToString(rdr[0].ToString());
//                                p.SerialNumber = ToString(rdr[1].ToString());
//                                p.Status = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.Date = ToString(rdr[3].ToString());
//                                p.time = ToString(rdr[4].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }

//                        if (status.Equals("inspectionanalyse"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Product p = new Product();
//                                p.Date = ToString(rdr[0].ToString()); //(int) rdr[0];
//                                p.todayinspection = ToInt32(rdr[1].ToString()); //(int) rdr[0];

//                                //p.todayinspection = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                //p.yesterdayinspection = ToInt32(rdr[1].ToString()); //(int) rdr[0];
//                                //p.weeklyinspection = ToInt32(rdr[2].ToString()); //(int) rdr[0];
//                                //p.monthlyinspection = ToInt32(rdr[3].ToString()); //(int) rdr[0];
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        con.Close();
//                    }
//                }
//                return lst;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public List<Question> SelectQuestionandResponse(string status, string id, string start_date, string end_date, string datetime)
//        {
//            try
//            {
//                List<Question> lst = new List<Question>();
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        if (status.Equals("QuestionList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Question p = new Question();
//                                p.QuestionID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.QuestionText = FirstCharToUpper(ToString(rdr[1].ToString()).ToLower());
//                                p.first_opt = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.second_opt = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
//                                p.date = ToString(rdr[4].ToString());
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("QuestionEdit"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Question p = new Question();
//                                p.QuestionID = ToInt32(rdr[0].ToString()); //(int) rdr[0];
//                                p.QuestionText = FirstCharToUpper(ToString(rdr[1].ToString()).ToLower());
//                                p.first_opt = FirstCharToUpper(ToString(rdr[2].ToString()).ToLower());
//                                p.second_opt = FirstCharToUpper(ToString(rdr[3].ToString()).ToLower());
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("GetReport"))
//                        {
//                            while (rdr.Read())
//                            {
//                                Question p = new Question();
//                                p.QuestionText = ToString(rdr[0].ToString());
//                                p.ResponseText = ToString(rdr[1].ToString());
//                                p.CommentText = ToString(rdr[2].ToString());
//                                p.date = ToString(rdr[3].ToString());
//                                p.time = ToString(rdr[4].ToString());

//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }

//                        con.Close();
//                    }
//                }
//                return lst;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public List<VapidKeys> SelectSubscription(string status, string id, string start_date, string end_date, string datetime)
//        {
//            try
//            {
//                List<VapidKeys> lst = new List<VapidKeys>();
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SpSelection", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@start_date", start_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@end_date", end_date.ToString().Trim());
//                        cmd.Parameters.AddWithValue("@datetime", datetime.ToString().Trim());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        if (status.Equals("GetSubscription"))
//                        {
//                            while (rdr.Read())
//                            {
//                                VapidKeys p = new VapidKeys();
//                                p.Id = ToInt32(rdr[0].ToString()); 
//                                p.endpoint= ToString(rdr[1].ToString());
//                                p.p256DH = ToString(rdr[2].ToString());
//                                p.auth = ToString(rdr[3].ToString());
//                                p.PublicKey = ToString(rdr[4].ToString());
//                                p.PrivateKey = ToString(rdr[5].ToString());
//                                lst.Add(p);
//                            }
//                            rdr.Close();
//                        }
//                        con.Close();
//                    }
//                }
//                return lst;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        //Selection

//        //Login
//        public string LoginVerification(string status, string LoginID, string Password)
//        {
//            string checker = "";
//            try
//            {

//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SPLogin", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@LoginID", LoginID.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@Password", Password.Trim().ToString());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        if (status.Equals("ForgetPasswordVerification"))
//                        {
//                            while (rdr.Read())
//                            {
//                                checker = ToString(rdr[0].ToString().Trim());
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("ResetPasswordEmail"))
//                        {
//                            while (rdr.Read())
//                            {
//                                checker = ToString(rdr[0].ToString().Trim());
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("ResetPasswordlogin_id"))
//                        {
//                            while (rdr.Read())
//                            {
//                                checker = ToString(rdr[0].ToString().Trim());
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("ResetPassworduser_name"))
//                        {
//                            while (rdr.Read())
//                            {
//                                checker = ToString(rdr[0].ToString().Trim());
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("AdministratorSide"))
//                        {
//                            while (rdr.Read())
//                            {
//                                checker = ToString(rdr[0].ToString().Trim());
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("verification"))
//                        {
//                            while (rdr.Read())
//                            {
//                                checker = ToString(rdr[0].ToString().Trim());
//                            }
//                            rdr.Close();
//                        }
                        
//                        con.Close();
//                    }
//                }
//                return checker.ToString().Trim();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//        public List<login> loginSession(string status, string LoginID, string Password)
//        {
//            try
//            {
//                List<login> lst = new List<login>();
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SPLogin", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@LoginID", LoginID.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@Password", Password.Trim().ToString());

//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        if (status.Equals("CustomerSideVerified"))
//                        {
//                            while (rdr.Read())
//                            {
//                                login bo = new login();
//                                bo.user_credential_id = ToInt32(rdr[0].ToString().Trim());
//                                bo.user_name = FirstCharToUpper(ToString(rdr[1].ToString().Trim()).ToLower());
//                                bo.email = ToString(rdr[2].ToString().Trim()).ToLower();
//                                bo.password = ToString(rdr[3].ToString().Trim()).ToLower();
//                                bo.user_mobileNo = ToString(rdr[4].ToString().Trim());
//                                //bo.login_type = ToString(rdr[5].ToString());

//                                lst.Add(bo);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("LoginType"))
//                        {
//                            while (rdr.Read())
//                            {
//                                login i = new login();
//                                i.login_type_id = ToInt32(rdr[0].ToString());
//                                i.login_type = ToString(rdr[1].ToString());
//                                lst.Add(i);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("UserList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                login i = new login();
//                                i.user_credential_id = ToInt32(rdr[0].ToString());
//                                i.user_name = ToString(rdr[1].ToString());
//                                lst.Add(i);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("inspectorList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                login i = new login();
//                                i.user_credential_id = ToInt32(rdr[0].ToString());
//                                i.user_name = ToString(rdr[1].ToString());
//                                lst.Add(i);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("MemberUserAccess"))
//                        {
//                            while (rdr.Read())
//                            {
//                                login bo = new login();
//                                bo.user_credential_id = ToInt32(rdr[0].ToString().Trim());
//                                bo.user_name = FirstCharToUpper(ToString(rdr[1].ToString().Trim()).ToLower());
//                                bo.login_id = ToString(rdr[2].ToString().Trim()).ToLower();
//                                bo.email = ToString(rdr[3].ToString().Trim()).ToLower();
//                                bo.user_mobileNo = ToString(rdr[4].ToString().Trim());
//                                bo.login_type = ToString(rdr[5].ToString());
//                                bo.date = ToString(rdr[6].ToString());
//                                lst.Add(bo);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("AdministratorSideVerified"))
//                        {
//                            while (rdr.Read())
//                            {
//                                login bo = new login();
//                                bo.user_credential_id = ToInt32(rdr[0].ToString().Trim());
//                                bo.user_name = FirstCharToUpper(ToString(rdr[1].ToString().Trim()).ToLower());
//                                bo.password = ToString(rdr[2].ToString().Trim()).ToLower();
//                                bo.login_id = ToString(rdr[3].ToString().Trim()).ToLower();
//                                bo.user_mobileNo = ToString(rdr[4].ToString().Trim());
//                                bo.login_type = ToString(rdr[5].ToString().Trim());
//                                bo.user_img = ToString(rdr[6].ToString().Trim());

//                                bo.module_name = ToString(rdr[7].ToString().Trim());
//                                bo.can_read = ToInt32(rdr[8].ToString().Trim());
//                                bo.can_create = ToInt32(rdr[9].ToString().Trim());
//                                bo.can_delete = ToInt32(rdr[10].ToString().Trim());
//                                bo.can_update = ToInt32(rdr[11].ToString().Trim());
//                                bo.can_print = ToInt32(rdr[12].ToString().Trim());
//                                bo.can_report = ToInt32(rdr[13].ToString().Trim());


//                                lst.Add(bo);
//                            }
//                            rdr.Close();
//                        }
//                        if (status.Equals("GetRightList"))
//                        {
//                            while (rdr.Read())
//                            {
//                                login bo = new login();
//                                bo.user_credential_id = ToInt32(rdr[0].ToString().Trim());
//                                bo.user_name = FirstCharToUpper(ToString(rdr[1].ToString().Trim()).ToLower());
//                                bo.module_id = ToInt32(rdr[2].ToString().Trim());
//                                bo.module_name = ToString(rdr[3].ToString().Trim()).ToLower();
//                                bo.can_read = ToInt32(rdr[4].ToString().Trim());
//                                bo.can_create = ToInt32(rdr[5].ToString().Trim());
//                                bo.can_delete = ToInt32(rdr[6].ToString().Trim());
//                                bo.can_update = ToInt32(rdr[7].ToString().Trim());
//                                bo.can_print = ToInt32(rdr[8].ToString().Trim());
//                                bo.can_report = ToInt32(rdr[9].ToString().Trim());

//                                lst.Add(bo);
//                            }
//                            rdr.Close();
//                        }
//                        cmd.ExecuteNonQuery();
//                        con.Close();
//                    }
//                }
//                return lst;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public string BackOfficeInsertion(string status, string field, string value, string module_permission_id, string id, string column_name, string result)
//        {
//            string db_status = "SP Not Work";
//            try
//            {
//                using (SqlConnection con = new SqlConnection(connectionStringSP))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SPRightsInsertion", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@status", status.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@field", field.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@value", value.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@module_permission_id", module_permission_id.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@id", id.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@column_name", column_name.Trim().ToString());
//                        cmd.Parameters.AddWithValue("@result", result.ToString().Trim());
//                        con.Open();
//                        SqlDataReader rdr = cmd.ExecuteReader();

//                        while (rdr.Read())
//                        {
//                            db_status = ToString(rdr[0].ToString().Trim());
//                        }
//                        rdr.Close();
//                        con.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return db_status.ToString().Trim();
//        }
//        //Login

//        public Boolean DatabaseConnectionCheck()
//        {
//            using (SqlConnection connection = new SqlConnection(connectionStringSP))
//            {
//                try
//                {
//                    connection.Open();
//                    return true;
//                }
//                catch (SqlException)
//                {
//                    return false;
//                }
//            }
//        }
//        public bool CheckForInternetConnection()
//        {
//            try
//            {
//                using (var client = new WebClient())
//                using (client.OpenRead("http://clients3.google.com/generate_204"))
//                {
//                    return true;
//                }
//            }
//            catch
//            {
//                return false;
//            }
//            //return true;
//        }


//        // Generates a random number within a range.      
//        public int RandomNumber(int min, int max)
//        {
//            Random generator = new Random();
//            String r = generator.Next(min, max).ToString("D6");
//            return ToInt32(r);
//        }

//        public string Generatepassword()
//        {
//            int length = 6;
//            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
//            StringBuilder res = new StringBuilder();
//            Random rnd = new Random();
//            while (0 < length--)
//            {
//                res.Append(valid[rnd.Next(valid.Length)]);
//            }
//            return res.ToString();
//        }

//        public string Suffix(int integer)
//        {
//            switch (integer % 100)
//            {
//                case 11:
//                case 12:
//                case 13:
//                    return "th";
//            }
//            switch (integer % 10)
//            {
//                case 1:
//                    return "st";
//                case 2:
//                    return "nd";
//                case 3:
//                    return "rd";
//                default:
//                    return "th";
//            }
//        }

//        public string FirstCharToUpper(string value)
//        {
//            char[] array = value.ToCharArray();
//            char index = 'a';
//            // Handle the first letter in the string.  
//            if (array.Length >= 1)
//            {
//                if (char.IsLower(array[0]))
//                {
//                    array[0] = char.ToUpper(array[0]);
//                }
//            }
//            // Scan through the letters, checking for spaces.  
//            // ... Uppercase the lowercase letters following spaces.  
//            for (int i = 1; i < array.Length; i++)
//            {
//                if (array[i - 1] == ' ')
//                {
//                    if (char.IsLower(array[i]))
//                    {
//                        array[i] = char.ToUpper(array[i]);
//                    }
//                }
//                if (array[i - 1] == '(')
//                {
//                    index = array[i - 1];
//                    if (char.IsLower(array[i]))
//                    {
//                        array[i] = char.ToUpper(array[i]);
//                    }
//                }
//                //if(index == '(' && array[i - 1] != ')')
//                //{
//                //    if (char.IsLower(array[i]))
//                //    {
//                //        array[i] = char.ToUpper(array[i]);
//                //    }
//                //}

//            }
//            return new string(array);
//        }

//        public string ToString(string value)
//        {

//            if (value == null)
//            {
//                return "---";
//            }
//            else if (value == "")
//            {
//                return "---";
//            }
//            return value;

//        }

//        public int ToInt32(string value)
//        {

//            if (value == null)
//            {
//                return 0;
//            }
//            else if (value == "")
//            {
//                return 0;
//            }
//            return (int)Convert.ToDouble(value);

//        }

//        public Boolean ToBoolean(string value)
//        {

//            if (value == null)
//            {
//                return false;
//            }
//            else if (value == "")
//            {
//                return false;
//            }
//            else if (value == "0")
//            {
//                return Convert.ToBoolean(ToInt32(value));
//            }
//            else if (value == "1")
//            {
//                return Convert.ToBoolean(ToInt32(value));
//            }
//            return Convert.ToBoolean(value);

//        }

//        public DateTime ToDate(string date)
//        {
//            if (date == "")
//            {
//                return Convert.ToDateTime("0000-00-00");
//            }
//            if (date == null)
//            {
//                return Convert.ToDateTime("0000-00-00");
//            }
//            return Convert.ToDateTime(date);
//        }

//        public void WriteEventLog(string message)
//        {
//            StreamWriter sw = null;
//            try
//            {

//                //string path = Server.MapPath("/BranchAttendance/");
//                bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/ErrorLog"));

//                if (!exists)
//                {
//                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/ErrorLog"));
//                }
//                HttpContext.Current.Server.MapPath("/ErrorLog/");
//                string targetPath = HttpContext.Current.Server.MapPath("/ErrorLog/");// @"E:\Transferfiles\";
//                string date = DateTime.Now.ToString("dd-MMM-yyyy");
//                sw = new StreamWriter(targetPath + date + ".txt", true);
//                sw.WriteLine(DateTime.Now.ToString() + " : " + message);
//                sw.Flush();
//                sw.Close();
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        public double ConvertBytesToMB(long bytes)
//        {
//            return bytes / (1024.0 * 1024.0); // Divide by 1024 * 1024 to convert to MB
//        }


//        //public string NParenthesis(decimal value)
//        //{
//        //    if(value<-1)
//        //    {
//        //        return "(" + value + ")";
//        //    }
//        //    return value.ToString();
//        //}             
//    }
//}