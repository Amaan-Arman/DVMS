using Microsoft.AspNetCore.Mvc;
using DVMS.Models;
using Microsoft.AspNetCore.Http;

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

        public JsonResult Rights(string user, string moduleID, string column_name, string result)
        {
            string status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        string field = "" + column_name + " , user_id ,module_permission_id";
                        string values = "'" + result + "','" + user + "','" + moduleID + "'";
                        string value = column_name + "=" + result;
                        status = Cc.BackOfficeInsertion("Rights", field, values, moduleID, user, column_name, value);
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
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                //Cc.WriteEventLog(opl + " : " + ex.ToString());
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError");
            }
            return Json(status);
        }

        public JsonResult GetRight(string filter_id)
        {
            List<login> GetRight = new List<login>();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        GetRight = Cc.loginSession("GetRightList", "empty", filter_id).ToList();
                    }
                    else
                    {
                        TempData["NetVarification"] = "UnConnected";
                        return Json("NetworkError");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                    return Json("DataBaseError");
                }
            }
            catch (Exception ex)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                //Cc.WriteEventLog(opl + " : " + ex.ToString());
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError");
            }
            return Json(GetRight);
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
                        else if (Status.ToString().Trim().Equals("Invalid Login and Password Id"))
                        {
                            return Json(Status);
                        }
                        else if (Status.ToString().Equals("AdministratorSideVerified"))
                        {
                            rightSessions(Status, model.login_id.ToString().Trim(), model.Password.ToString().Trim());
                            logindata();
                            Status = HttpContext.Session.GetString("login_type").ToString().Trim();
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

        public void rightSessions(string status, string LoginID, string Password)
        {

            //employee_id             
            foreach (var item in Cc.loginSession(status, LoginID, Password))
            {
                HttpContext.Session.SetInt32("user_credential_id", item.user_credential_id);
                HttpContext.Session.SetString("user_name", item.user_name.ToString().Trim());
                HttpContext.Session.SetString("Email", item.Email.ToString().Trim());
                HttpContext.Session.SetString("user_mobileNo", item.user_mobileNo.ToString().Trim());
                HttpContext.Session.SetString("login_type", item.login_type.ToString().Trim());
                HttpContext.Session.SetString("department", item.department.ToString().Trim());
                HttpContext.Session.SetString("user_img", item.user_img.ToString().Trim());
                HttpContext.Session.SetInt32("company", item.company);
                
                // List of all permission types
                string[] permissions = { "can_read", "can_create", "can_delete", "can_update", "can_print", "can_report" };

                // Get module name (already trimmed)
                string moduleName = item.module_name.ToString().Trim();

                // Loop through each permission property
                foreach (var perm in permissions)
                {
                    // Use reflection to get the value of each permission from "item"
                    var value = (int)item.GetType().GetProperty(perm).GetValue(item, null);

                    // Store in session dynamically -> Example: "Company_can_read"
                    HttpContext.Session.SetInt32($"{moduleName}_{perm}", value);
                }

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
            if (HttpContext.Session.GetString("Login") != null)
            {
                TempData["user_credential_id"] = HttpContext.Session.GetInt32("user_credential_id").ToString().Trim();
                TempData["user_name"] = HttpContext.Session.GetString("user_name").ToString().Trim();
                TempData["Email"] = HttpContext.Session.GetString("Email").ToString().Trim();
                TempData["user_mobileNo"] = HttpContext.Session.GetString("user_mobileNo").ToString().Trim();
                TempData["login_type"] = HttpContext.Session.GetString("login_type").ToString().Trim();
                TempData["department"] = HttpContext.Session.GetString("department").ToString().Trim();
                TempData["user_img"] = HttpContext.Session.GetString("user_img").ToString().Trim();
                TempData["company"] = HttpContext.Session.GetInt32("company").ToString().Trim();

                string[] modules = { "Company", "UserAccess", "InviteGuest", "TrackGuest", "TrackVisitor", "FileManager", "Chat", "Setting" };
                string[] permissions = { "can_read", "can_create", "can_delete", "can_update", "can_print", "can_report" };

                foreach (var module in modules)
                {
                    foreach (var perm in permissions)
                    {
                        TempData[$"{module}_{perm}"] = HttpContext.Session.GetInt32($"{module}_{perm}").ToString();
                    }
                }
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["user_credential_id"] = null;
            TempData["user_name"] = null;
            TempData["Email"] = null;
            TempData["user_mobileNo"] = null;
            TempData["login_type"] = null;
            TempData["department"] = null;
            TempData["user_img"] = null;
            TempData["company"] = null;
            string[] modules = { "Company", "UserAccess", "InviteGuest", "TrackGuest", "TrackVisitor", "FileManager", "Chat", "Setting" };
            string[] permissions = { "can_read", "can_create", "can_delete", "can_update", "can_print", "can_report" };

            foreach (var module in modules)
            {
                foreach (var perm in permissions)
                {
                    TempData[$"{module}_{perm}"] = null;
                }
            }

            return RedirectToAction("Index", "Login");
        }
    }
}