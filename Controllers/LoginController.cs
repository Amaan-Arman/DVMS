using Microsoft.AspNetCore.Mvc;
using DVMS.Models;

namespace DVMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly CrudClass Cc;
        private readonly IWebHostEnvironment _env;

        public LoginController(IConfiguration configuration, IWebHostEnvironment env)
        {
            Cc = new CrudClass(configuration);
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        //BeComeSeller
        public IActionResult BeComeSeller()
        {
            return View();
        }
        public JsonResult CheckEmail(login model)
        {
            string Status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        Status = Cc.LoginVerification("UserRegistration", model.Email.ToString().Trim(), "0");
                        if (Status.ToString().Trim().Equals("EmailAlreadyExist"))
                        {
                            return Json(Status);
                        }
                        else if (Status.ToString().Equals("EmailVerified"))
                        {
                            return Json(Status);
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError");
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
            return Json(Status);
        }

        public async Task<IActionResult> SetWorkerRegistration(login model)
        {
            string Status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        string folderPath = Path.Combine(_env.WebRootPath, "admin_assets", "images");

                        // Create directory if it doesn't exist
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        var user_pic = model.user_pic; // IFormFile
                        var front_cnic = model.front_cnic; // IFormFile
                        var back_cnic = model.back_cnic; // IFormFile

                        if (front_cnic != null && front_cnic.Length > 0 && back_cnic != null && back_cnic.Length > 0 && user_pic != null && user_pic.Length > 0)
                        {
                            string fileName1 = Path.GetFileName(front_cnic.FileName);
                            string fileName2 = Path.GetFileName(back_cnic.FileName);
                            string fileName3 = Path.GetFileName(user_pic.FileName);

                            string fullPath1 = Path.Combine(folderPath, fileName1);
                            string fullPath2 = Path.Combine(folderPath, fileName2);
                            string fullPath3 = Path.Combine(folderPath, fileName3);

                            using (var stream = new FileStream(fullPath1, FileMode.Create))
                            {
                                await front_cnic.CopyToAsync(stream);
                            }
                            using (var stream = new FileStream(fullPath2, FileMode.Create))
                            {
                                await back_cnic.CopyToAsync(stream);
                            }
                            using (var stream = new FileStream(fullPath3, FileMode.Create))
                            {
                                await user_pic.CopyToAsync(stream);
                            }
                            string field = "UserImg ,Name , Email ,PhoneNumber, City, cnic_front, cnic_back, Password, UserType,isdelete";
                            string values = "'" + fileName3 + "','" + model.user_name + "', '" + model.Email + "', '" + model.user_mobileNo + "', '" + model.city + "', '" + fileName1 + "', '" + fileName2 + "', '" + model.Password + "', 'Seller', 'true'";
                            Status = Cc.InsertionMethodStatus("SetUser", field, values);
                        }
                        else
                        {
                            return Json("File not Found...!");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError");
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
            return Json(Status);
        }

        //BeComeSeller

        public JsonResult SetUser(login model)
        {
            string Status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        Status = Cc.LoginVerification("UserRegistration", model.Email.ToString().Trim(), "0");
                        if (Status.ToString().Trim().Equals("EmailAlreadyExist"))
                        {
                            return Json(Status);
                        }
                        else if (Status.ToString().Equals("EmailVerified"))
                        {
                            model.otp_num = Cc.opt_num();
                            string field = "otp_num , email , otp_status ";
                            string values = "'" + model.otp_num + "' , '" + model.Email + "' , ' New ' ";
                            Status = Cc.InsertionMethodStatus("SetOTP", field, values);
                            if (Status == "Saved")
                            {
                                //Cc.OtpSend(model.otp_num, model.Email);
                            }
                        }
                        //if (Status.ToString().Trim().Equals("Invalid Email Id"))
                        //{
                        //    return Json(Status);
                        //}
                        //else if (Status.ToString().Equals("ClientSideVerified"))
                        //{
                        //    model.otp_num = Cc.opt_num();
                        //    string field = "otp_num , email , otp_status ";
                        //    string values = "'" + model.otp_num + "' , '" + model.Email + "' , ' New ' ";
                        //    Status = Cc.InsertionMethodStatus("SetOTP", field, values);
                        //    //Cc.OtpSend(model.otp_num, model.Email);
                        //}
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError");
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
            return Json(Status);
        }

        public JsonResult OPTVerify(login model)
        {
            string Status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        Status = Cc.LoginVerification("otpverify", model.Email.ToString().Trim(), model.otp_num.ToString().Trim());

                        if (Status.ToString().Trim().Equals("Invalid Otp Id"))
                        {
                            //string field = "email , otp_num , otp_status ";
                            //string values = " '" + model.Email + "' ,'" + model.Password + "' , ' Incorrect OTP ' ";
                            //Cc.InsertionMethodStatus("SetOTP", field, values);
                            return Json(Status);
                        }
                        else if (Status.ToString().Equals("OtpVerified"))
                        {
                            string field = "Name , Email , Password,UserType";
                            string values = "'" + model.user_name + "', '" + model.Email + "', '" + model.Password + "', 'User'";
                            Status = Cc.InsertionMethodStatus("SetUser", field, values);
                            if (Status == "Saved")
                            {
                                rightSessions("AdministratorSideVerified", model.Email.ToString().Trim(), model.Password.ToString().Trim());
                                Status = HttpContext.Session.GetString("login_type").ToString().Trim();
                            }
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError");
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
            return Json(Status);
        }

        public JsonResult ResendOTP(login model)
        {
            //List<Login> login = new List<Login>();
            string Status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        Status = Cc.LoginVerification("ClientSide", model.Email.ToString().Trim(), "0");

                        if (Status.ToString().Trim().Equals("Invalid Email Id"))
                        {
                            return Json(Status);
                        }
                        else if (Status.ToString().Equals("ClientSideVerified"))
                        {
                            model.otp_num = Cc.opt_num();

                            string field = "otp_num , email , otp_status ";
                            string values = "'" + model.otp_num + "' , '" + model.Email + "' , ' Resend ' ";
                            Cc.InsertionMethodStatus("SetOTP", field, values);
                            Status = "Saved";

                            //Cc.OtpSend(model.otp_num, model.Email);

                            //model.otp_num = Cc.opt_num();
                            //string values = " otp_num='" + model.otp_num + " , 'otp_status=' Resend '  ";
                            //Cc.UpdationnMethod("updateOTP", values, "'"+model.email.ToString().Trim()+"'");
                            //Status = "Saved";
                            //Cc.OtpSend(model.otp_num, model.email);
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError");
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
            return Json(Status);
        }


        public JsonResult LoginAndPassword(login model)
        {
            //List<Login> login = new List<Login>();
            string Status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        Status = Cc.LoginVerification("AdministratorSide", model.login_id.ToString().Trim(), model.Password.ToString().Trim());

                        if (Status.ToString().Trim().Equals("Invalid Login Id"))
                        {
                            return Json(Status);
                        }
                        else if (Status.ToString().Trim().Equals("Invalid Password Id"))
                        {
                            return Json(Status);
                        }
                        else if (Status.ToString().Equals("AdministratorSideVerified"))
                        {
                            rightSessions(Status, model.login_id.ToString().Trim(), model.Password.ToString().Trim());
                            logindata();
                            Status = HttpContext.Session.GetString("login_type").ToString().Trim();
                            //Status = Session["login_type"].ToString().Trim();
                        }
                        //if (model.login_id.ToString().Trim().Equals("Admin") && model.password.ToString().Trim().Equals("Admin"))
                        //{
                        //    Status = "Admin";
                        //}
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError");
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
            return Json(Status);
        }

        public void rightSessions(string status, string LoginID, string Password)
        {

            //employee_id             
            foreach (var item in Cc.loginSession(status, LoginID, Password))
            {
                HttpContext.Session.SetString("isonline", "--");
                HttpContext.Session.SetInt32("user_credential_id", item.user_credential_id);
                HttpContext.Session.SetString("user_name", item.user_name.ToString().Trim());
                HttpContext.Session.SetString("login_type", item.login_type.ToString().Trim());
                HttpContext.Session.SetString("user_mobileNo", item.user_mobileNo.ToString().Trim());
                HttpContext.Session.SetString("user_img", item.user_img.ToString().Trim());

            }
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            HttpContext.Session.SetString("Login", finalString);

        }
        public int logindata()
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                TempData["isonline"] = null;
                TempData["Login"] = null;
                TempData["user_credential_id"] = null;
                TempData["user_name"] = null;
                TempData["login_type"] = null;
                TempData["user_mobileNo"] = null;
                TempData["user_img"] = null;

                return 1;
            }
            else
            {
                TempData["isonline"] = HttpContext.Session.GetString("isonline").ToString().Trim();
                TempData["Login"] = HttpContext.Session.GetString("Login").ToString().Trim();
                TempData["user_credential_id"] = HttpContext.Session.GetInt32("user_credential_id").ToString().Trim();
                TempData["user_name"] = HttpContext.Session.GetString("user_name").ToString().Trim();
                TempData["login_type"] = HttpContext.Session.GetString("login_type").ToString().Trim();
                TempData["user_mobileNo"] = HttpContext.Session.GetString("user_mobileNo").ToString().Trim();
                TempData["user_img"] = HttpContext.Session.GetString("user_img").ToString().Trim();

                return 0;
            }
        }

        public ActionResult Logout()
        {
            string Status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        string values = null;
                        var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();

                        values = "isonline='0'";
                        Status = Cc.UpdationMethodReturn("updateUser", values, Userid.ToString());
                        if (Status == "Saved")
                        {
                            HttpContext.Session.Remove("isonline");
                            HttpContext.Session.Remove("user_credential_id");
                            HttpContext.Session.Remove("Login");
                            HttpContext.Session.Remove("user_name");
                            HttpContext.Session.Remove("login_type");
                            HttpContext.Session.Remove("user_mobileNo");
                            HttpContext.Session.Remove("user_img");

                            TempData["isonline"] = null;
                            TempData["Login"] = null;
                            TempData["user_credential_id"] = null;
                            TempData["user_name"] = null;
                            TempData["login_type"] = null;
                            TempData["user_mobileNo"] = null;
                            TempData["user_img"] = null;
                            return RedirectToAction("Index", "Login");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError");
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
            return RedirectToAction("Index", "Home");

        }
    }
}