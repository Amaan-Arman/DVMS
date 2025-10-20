using DVMS.Hubs;
using DVMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using QRCoder.Core;
using System;
using System.Drawing;
using System.Reflection;
using static QRCoder.Core.PayloadGenerator;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace DVMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly CrudClass Cc;
        public AdminController(IConfiguration configuration, IHubContext<ChatHub> hubContext, IWebHostEnvironment env)
        {
            Cc = new CrudClass(configuration);
            _hubContext = hubContext;
            _env = env;
        }
        private readonly IHubContext<ChatHub> _hubContext;

        [HttpPost]
        public async Task<IActionResult> SendNotification(string userId, string name, string message_txt, string type)
        {
            await _hubContext.Clients.All.SendAsync("broadcastMessage", userId, name, message_txt, type);
            return Ok(new { message = "Notification sent!" });
        }


        public IActionResult Home()
        {
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        var Userid = HttpContext.Session.GetString("user_credential_id");

                        List<Admin> CompanyList = Cc.SelectAdmin("GetCompanyList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        ViewBag.CompanyListVB = new SelectList(CompanyList, "Company_id", "Company_name");

                        List<Admin> DepartmentList = Cc.SelectAdmin("GetDepartmentList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        ViewBag.DepartmentListVB = new SelectList(DepartmentList, "Department_id", "Department_name");

                        List<Admin> EmployeeList = Cc.SelectAdmin("GetEmployeeList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        ViewBag.EmployeeListVB = new SelectList(EmployeeList, "Employee_id", "Employee_name");
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                    return View();
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
                return View();
            }
        }
        public JsonResult GetEmployeesByCompany(int companyId)
        {
            List<Admin> GetEmployeesByCompany = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    GetEmployeesByCompany = Cc.SelectAdmin("GetEmployeeList", companyId.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
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
            return Json(GetEmployeesByCompany);
        }
        public IActionResult Index()
        {
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Login")))
                    {
                        int a = logindata();
                        if (a == 0)
                        {
                            var Userid = HttpContext.Session.GetString("user_credential_id");

                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");

                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                    return View();
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
                return View();
            }
        }

        //companies
        public IActionResult companies()
        {
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        int a = logindata();
                        if (a == 0)
                        {
                            var Userid = HttpContext.Session.GetString("user_credential_id");

                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");

                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                    return View();
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
                return View();
            }
        }
        [HttpPost]
        public IActionResult SetCompany(Company model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid data received.");
                }
                if (!Cc.CheckForInternetConnection())
                {
                    return Json("NetworkError");
                }
                if (!Cc.DatabaseConnectionCheck())
                {
                    return Json("DataBaseError");
                }
                var userId = HttpContext.Session.GetInt32("user_credential_id")?.ToString();
                var userName = HttpContext.Session.GetString("user_name");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Session expired. Please login again.");
                }
                string folderPath = "~/assets/images/companieslogo";

                // Create directory if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var picture = model.logo; // IFormFile
                string fileName = "";
                if (picture != null && picture.Length > 0)
                {
                    // Optional: Generate a unique file name
                    fileName= Path.GetFileName(picture.FileName);
                    string fullPath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                         picture.CopyToAsync(stream);
                    }
                }
                string field = "Name  ,FloorNumber , Email ,Phone ,img";
                string values = "'" + model.companyName + "','" + model.floorno + "', '" + model.email + "', '" + model.phone + "', '" + fileName + "'";
                string Status = Cc.InsertionMethodStatus("SetCompany", field, values);

                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
        [HttpGet]
        public JsonResult GetCompanyList()
        {
            List<Company> GetCompanyList = new List<Company>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var ID = HttpContext.Session.GetInt32("company").ToString().Trim();
                    GetCompanyList = Cc.SelectCompany("CompanyList", ID, "0000-00-00", "0000-00-00", "0000-00-00").ToList();
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
            return Json(GetCompanyList);
        }
        //companies

        //Dashboard
        [HttpGet]
        public JsonResult CheckInOutList()
        {
            List<Admin> CheckInOutList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        if (HttpContext.Session.GetString("login_type") == "superadmin")
                        {
                            CheckInOutList = Cc.SelectAdmin("CheckInOutList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        }
                        else
                        {
                            var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
                            CheckInOutList = Cc.SelectAdmin("CheckInOutList", Userid, "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        }
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(CheckInOutList);
        }
        public JsonResult FloorWiseData()
        {
            List<Admin> FloorWiseData = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        if (HttpContext.Session.GetString("login_type") == "manager")
                        {
                            FloorWiseData = Cc.SelectAdmin("FloorWiseData", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        }
                        else
                        {
                            var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
                            FloorWiseData = Cc.SelectAdmin("FloorWiseData", Userid, "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        }
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(FloorWiseData);
        }
        public JsonResult GetTopvisitor()
        {
            List<Admin> GetTopvisitor = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        if (HttpContext.Session.GetString("login_type") == "manager")
                        {
                            GetTopvisitor = Cc.SelectAdmin("Topvisitor", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        }
                        else
                        {
                            var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
                            GetTopvisitor = Cc.SelectAdmin("Topvisitor", Userid, "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        }
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetTopvisitor);
        }
        public JsonResult GetTopguest()
        {
            List<Admin> GetTopguest = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        if (HttpContext.Session.GetString("login_type") == "manager")
                        {
                            GetTopguest = Cc.SelectAdmin("Topguest", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        }
                        else
                        {
                            var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
                            GetTopguest = Cc.SelectAdmin("Topguest", Userid, "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                        }
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetTopguest);
        }
        public ActionResult DetailFuntion(string status)
        {
            try
            {
                if (!Cc.CheckForInternetConnection())
                {
                    return Json("NetworkError");
                }
                if (!Cc.DatabaseConnectionCheck())
                {
                    return Json("DataBaseError");
                }
                //var logintype = Session["login_type"];
                //var user_id = Session["user_credential_id"];
                switch (status)
                {
                    case "GuestCheckInList":
                        ViewBag.DetailFuntionVB = Cc.SelectAdmin("GuestCheckInList", "0", "0000-00-00", "0000-00-00", "0000-00-00");
                        break;
                    case "GuestCheckOutList":
                        ViewBag.DetailFuntionVB = Cc.SelectAdmin("GuestCheckOutList", "0", "0000-00-00", "0000-00-00", "0000-00-00");
                        break;
                    case "VisitorCheckInList":
                        ViewBag.DetailFuntionVB = Cc.SelectAdmin("VisitorCheckInList", "0", "0000-00-00", "0000-00-00", "0000-00-00");
                        break;
                    case "VisitorCheckOutList":
                        ViewBag.DetailFuntionVB = Cc.SelectAdmin("VisitorCheckOutList", "0", "0000-00-00", "0000-00-00", "0000-00-00");
                        break;
                        //default:
                        //    ViewBag.DetailFuntionVB = Cc.SelectAdmin("DetailpendingInspection", "0", "0000-00-00", "0000-00-00", "0000-00-00");
                        //    break;
                }
            }
            catch (Exception ex)
            {
                //Cc.WriteEventLog(ex.ToString());
                return Json(ex.ToString());
            }
            return PartialView("~/Views/Shared/PartialViewCheckInOutDetail.cshtml");
        }
        //Dashboard

        //Employee
        public IActionResult employee()
        {
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        int a = logindata();
                        if (a == 0)
                        {
                            var Userid = HttpContext.Session.GetString("user_credential_id");
                            List<Admin> CompanyList = Cc.SelectAdmin("GetCompanyList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            ViewBag.CompanyListVB = new SelectList(CompanyList, "Company_id", "Company_name");

                            List<Admin> DepartmentList = Cc.SelectAdmin("GetDepartmentList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            ViewBag.DepartmentListVB = new SelectList(DepartmentList, "Department_id", "Department_name");
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");

                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                    return View();
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
                return View();
            }
        }
        [HttpPost]
        public IActionResult SetEmployee([FromBody] Employee request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid data received.");
                }
                if (!Cc.CheckForInternetConnection())
                {
                    return Json("NetworkError");
                }
                if (!Cc.DatabaseConnectionCheck())
                {
                    return Json("DataBaseError");
                }
                var userId = HttpContext.Session.GetInt32("user_credential_id")?.ToString();
                var userName = HttpContext.Session.GetString("user_name");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Session expired. Please login again.");
                }
                var CompanyID = "";
                if (string.IsNullOrEmpty(request.companyID.ToString()))
                {
                     CompanyID = HttpContext.Session.GetInt32("company")?.ToString();
                }
                else
                {
                     CompanyID = request.companyID.ToString();
                }
                string Password = Cc.Generatepassword();
                string field = "FullName  ,Email , Phone ,Role, Department,login_id,CompanyId,PasswordHash";
                string values = "'" + request.employeeName + "','" + request.email + "', '" + request.phone+ "', '" + request.role+ "', '" + request.department+ "', '" + request.employee_loginID+ "', '" + CompanyID + "', '" + Password + "'";
                string Status = Cc.InsertionMethodStatus("SetEmployee", field, values);

                if (Status =="Saved")
                {
                    Cc.CredentialSendEmail(request.email, request.employeeName, request.employee_loginID, Password);
                }
                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
        [HttpGet]
        public JsonResult GetEmployeeList()
        {
            List<Employee> GetEmployeeList = new List<Employee>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var ID = HttpContext.Session.GetInt32("company").ToString().Trim();
                    GetEmployeeList = Cc.SelectEmployee("EmployeeList", ID, "0000-00-00", "0000-00-00", "0000-00-00").ToList();
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
            return Json(GetEmployeeList);
        }
        //Employee

        //trackguest
        [HttpPost]
        public IActionResult SetGuestCheckOut([FromBody] Admin request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid data received.");
                }
                if (!Cc.CheckForInternetConnection())
                {
                    return Json("NetworkError");
                }
                if (!Cc.DatabaseConnectionCheck())
                {
                    return Json("DataBaseError");
                }
                var userId = HttpContext.Session.GetInt32("user_credential_id")?.ToString();
                var userName = HttpContext.Session.GetString("user_name");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Session expired. Please login again.");
                }
                string values = "CheckOutTime='" + DateTime.Now + "'";
                string Status = Cc.UpdationMethodReturn("UpdateGuestCheckOUT", values, request.Invitation_ID.ToString());

                //if (Status == "Saved")
                //{
                //    Cc.CredentialSendEmail(request.email, request.employeeName, request.employee_loginID, Password);
                //}
                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
        public IActionResult trackguest()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GuestCheckInList()
        {
            List<Admin> GuestCheckInList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    string CompanyId = HttpContext.Session.GetInt32("company").ToString();

                    if (HttpContext.Session.GetString("login_type") == "superadmin")
                    {
                        GuestCheckInList = Cc.SelectAdmin("GuestCheckInList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    }
                    else
                    {
                     GuestCheckInList = Cc.SelectAdmin("GuestCheckInList", CompanyId.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
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
            return Json(GuestCheckInList);
        }
        [HttpGet]
        public JsonResult GuestCheckOutList()
        {
            List<Admin> GuestCheckOutList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    string CompanyId = HttpContext.Session.GetInt32("company").ToString();

                    if (HttpContext.Session.GetString("login_type") == "superadmin")
                    {
                        GuestCheckOutList = Cc.SelectAdmin("GuestCheckOutList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    }
                    else
                    {
                        GuestCheckOutList = Cc.SelectAdmin("GuestCheckOutList", CompanyId.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
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
            return Json(GuestCheckOutList);
        }
        [HttpGet]
        public JsonResult GuestSearchList(string id, string start_date_id, string end_date_id)
        {
            List<Admin> GuestSearchList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        if (string.IsNullOrEmpty(start_date_id) && string.IsNullOrEmpty(end_date_id))
                        {
                            GuestSearchList = Cc.SelectAdmin("GuestSearchList", id.ToString(), "0000-00-00", "", "" ).ToList();
                        }
                        else if (string.IsNullOrEmpty(id))
                        {
                            GuestSearchList = Cc.SelectAdmin("GuestSearchList", "", "0000-00-00", start_date_id.ToString(), end_date_id.ToString()).ToList();
                        }
                        else
                        {
                            GuestSearchList = Cc.SelectAdmin("GuestSearchList", id.ToString(), "0000-00-00", start_date_id.ToString(), end_date_id.ToString()).ToList();
                        }
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
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
            return Json(GuestSearchList);
        }

        [HttpGet]
        public JsonResult GuestOutSearchList(string id, string start_date_id, string end_date_id)
        {
            List<Admin> GuestOutSearchList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        if (string.IsNullOrEmpty(start_date_id) && string.IsNullOrEmpty(end_date_id))
                        {
                            GuestOutSearchList = Cc.SelectAdmin("GuestOutSearchList", id.ToString(), "0000-00-00", "", "").ToList();
                        }
                        else if (string.IsNullOrEmpty(id))
                        {
                            GuestOutSearchList = Cc.SelectAdmin("GuestOutSearchList", "", "0000-00-00", start_date_id.ToString(), end_date_id.ToString()).ToList();
                        }
                        else
                        {
                            GuestOutSearchList = Cc.SelectAdmin("GuestOutSearchList", id.ToString(), "0000-00-00", start_date_id.ToString(), end_date_id.ToString()).ToList();
                        }
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
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
            return Json(GuestOutSearchList);
        }
        //trackguest

        //trackvisitor
        [HttpPost]
        public IActionResult SetVisitorCheckOut([FromBody] Admin request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid data received.");
                }
                if (!Cc.CheckForInternetConnection())
                {
                    return Json("NetworkError");
                }
                if (!Cc.DatabaseConnectionCheck())
                {
                    return Json("DataBaseError");
                }
                var userId = HttpContext.Session.GetInt32("user_credential_id")?.ToString();
                var userName = HttpContext.Session.GetString("user_name");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Session expired. Please login again.");
                }

                string values = "CheckOutTime='" + DateTime.Now + "'";
                string Status = Cc.UpdationMethodReturn("UpdateAccessLogsCheckOUT", values, request.VisitorId.ToString());

                //if (Status == "Saved")
                //{
                //    Cc.CredentialSendEmail(request.email, request.employeeName, request.employee_loginID, Password);
                //}
                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
        public IActionResult trackvisitor()
        {
            return View();
        }
        [HttpGet]
        public JsonResult VisitorCheckOutList()
        {
            List<Admin> VisitorCheckOutList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    string CompanyId = HttpContext.Session.GetInt32("company").ToString();

                    if (HttpContext.Session.GetString("login_type") == "superadmin")
                    {
                        VisitorCheckOutList = Cc.SelectAdmin("VisitorCheckOutList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    }
                    else
                    {
                        VisitorCheckOutList = Cc.SelectAdmin("VisitorCheckOutList", CompanyId.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
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
            return Json(VisitorCheckOutList);
        }
        [HttpGet]
        public JsonResult VisitorCheckInList()
        {
            List<Admin> VisitorCheckInList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    string CompanyId = HttpContext.Session.GetInt32("company").ToString();

                    if (HttpContext.Session.GetString("login_type") == "superadmin")
                    {
                        VisitorCheckInList = Cc.SelectAdmin("VisitorCheckInList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    }
                    else
                    {
                        VisitorCheckInList = Cc.SelectAdmin("VisitorCheckInList", CompanyId.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
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
            return Json(VisitorCheckInList);
        }
        [HttpGet]
        public JsonResult VisitorSearchList(string id, string start_date_id, string end_date_id)
        {
            List<Admin> VisitorSearchList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        if (string.IsNullOrEmpty(start_date_id) && string.IsNullOrEmpty(end_date_id))
                        {
                            VisitorSearchList = Cc.SelectAdmin("VisitorSearchList", id.ToString(), "0000-00-00", "", "").ToList();
                        }
                        else if (string.IsNullOrEmpty(id))
                        {
                            VisitorSearchList = Cc.SelectAdmin("VisitorSearchList", "", "0000-00-00", start_date_id.ToString(), end_date_id.ToString()).ToList();
                        }
                        else
                        {
                            VisitorSearchList = Cc.SelectAdmin("VisitorSearchList", id.ToString(), "0000-00-00", start_date_id.ToString(), end_date_id.ToString()).ToList();
                        }
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
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
            return Json(VisitorSearchList);
        }

        [HttpGet]
        public JsonResult VisitorOutSearchList(string id, string start_date_id, string end_date_id)
        {
            List<Admin> VisitorOutSearchList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        if (string.IsNullOrEmpty(start_date_id) && string.IsNullOrEmpty(end_date_id))
                        {
                            VisitorOutSearchList = Cc.SelectAdmin("VisitorOutSearchList", id.ToString(), "0000-00-00", "", "").ToList();
                        }
                        else if (string.IsNullOrEmpty(id))
                        {
                            VisitorOutSearchList = Cc.SelectAdmin("VisitorOutSearchList", "", "0000-00-00", start_date_id.ToString(), end_date_id.ToString()).ToList();
                        }
                        else
                        {
                            VisitorOutSearchList = Cc.SelectAdmin("VisitorOutSearchList", id.ToString(), "0000-00-00", start_date_id.ToString(), end_date_id.ToString()).ToList();
                        }
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
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
            return Json(VisitorOutSearchList);
        }
        //trackvisitor


        //Invite guest
        public IActionResult inviteguest()
        {
            var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
            List <Admin> GuestList = Cc.SelectAdmin("GetGuestListID", Userid.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
            ViewBag.GuestListVB = new SelectList(GuestList, "GuestId", "GuestFullName");
            return View();
        }
        [HttpGet]
        public JsonResult GetGuestList()
        {
            List<Admin> GetGuestList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();

                    GetGuestList = Cc.SelectAdmin("GetGuestList", Userid, "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    //if (HttpContext.Session.GetString("Login") != null)
                    //{
                    //    //    var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
                    //}
                    //else
                    //{
                    //    RedirectToAction("Login", "Home");
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
            return Json(GetGuestList);
        }
        [HttpGet]
        public JsonResult GetInvitationList()
        {
            List<Admin> GetInvitationList = new List<Admin>();
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        GetInvitationList = Cc.SelectAdmin("GetInvitationList", Userid, "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    }
                    else
                    {
                        RedirectToAction("Login", "Home");
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
            return Json(GetInvitationList);
        }
        [HttpPost]
        public IActionResult SetGuset([FromBody] Admin request)
        {
            if (request == null){
                return BadRequest("Invalid data received.");
            }
            try
            {
                if (!Cc.CheckForInternetConnection()){
                    return Json("NetworkError");
                }
                if (!Cc.DatabaseConnectionCheck()){ 
                    return Json("DataBaseError"); 
                }
                //var userId = HttpContext.Session.GetInt32("user_credential_id")?.ToString();
                //var userName = HttpContext.Session.GetString("user_name");

                //if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                //    return Unauthorized("Session expired. Please login again.");
                
                var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();

                string field = "FullName  ,Email , Phone ,CNIC, UserID";
                string values = "'" + request.GuestFullName + "','" + request.GuestEmail + "', '" + request.GuestPhone + "', '" + request.GuestCNIC + "', '" + Userid + "'";
                string Status =  Cc.InsertionMethodStatus("SetGeust", field, values);

                //await SendNotification(request.userID.ToString(), userName, "message_txt", "Offer");
                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }

        //SetGuset
        [HttpPost]
        public IActionResult SetGusetInvitation([FromBody] Admin request)
        {
            if (request == null)
                return BadRequest("Invalid data received.");

            try
            {
                if (!Cc.CheckForInternetConnection())
                    return Json("NetworkError");

                if (!Cc.DatabaseConnectionCheck())
                    return Json("DataBaseError");

                var userId = HttpContext.Session.GetInt32("user_credential_id")?.ToString();
                var userName = HttpContext.Session.GetString("user_name");
                var CompanyId = HttpContext.Session.GetInt32("company");

                List<Admin> GetGuestEmail = new List<Admin>();

                GetGuestEmail = Cc.SelectAdmin("GetGuestEmail", request.GuestId.ToString().Trim(), "0", "0000-00-00", "0000-00-00").ToList();
                string Email = GetGuestEmail[0].GuestEmail;

                string invitationId = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/logo.png"); // update if needed
                string logoPath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/logoname.png"); // update if needed

                string timeFormatted = DateTime.ParseExact(request.VisitTime, "HH:mm", null).ToString("hh:mm tt");


                // 1. Generate QR code with logo
                Bitmap qrImage;
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrData = qrGenerator.CreateQrCode(
                        $"InvitationID; {invitationId}, Visitor; {request.GuestFullName}, Time;{request.VisitDate};{timeFormatted}, Host; {userName}",
                        QRCodeGenerator.ECCLevel.Q
                    );
                    var qrCode = new QRCode(qrData);
                    var rawQr = qrCode.GetGraphic(20, System.Drawing.Color.Black, System.Drawing.Color.White, true);

                    using (var logo = (Bitmap)System.Drawing.Image.FromFile(logoPath))
                    {
                        qrImage = new Bitmap(rawQr.Width, rawQr.Height);
                        using (var g = Graphics.FromImage(qrImage))
                        {
                            g.DrawImage(rawQr, 0, 0);
                            int logoSize = rawQr.Width / 5;
                            int logoPos = (rawQr.Width - logoSize) / 2;
                            g.DrawImage(logo, new System.Drawing.Rectangle(logoPos, logoPos, logoSize, logoSize));
                        }
                    }
                }

                // 2. Create invitation card
                Bitmap invitationImage = new Bitmap(600, 800);
                using (Graphics graphics = Graphics.FromImage(invitationImage))
                using (System.Drawing.Image logo = System.Drawing.Image.FromFile(logoPath2))
                {
                    graphics.Clear(System.Drawing.Color.White);

                    // Draw company logo
                    graphics.DrawImage(logo, new System.Drawing.Rectangle(50, 20, 451, 94));

                    // Draw text
                    var font = new Font("Arial", 18);
                    graphics.DrawString($"Guest : {request.GuestFullName}", font, Brushes.Black, new System.Drawing.PointF(50, 150));
                    graphics.DrawString($"Host : {userName}", font, Brushes.Black, new System.Drawing.PointF(50, 190));
                    graphics.DrawString($"Visit Time : {request.VisitDate:dd MMM yyyy} {timeFormatted}", font, Brushes.Black, new System.Drawing.PointF(50, 230));
                    graphics.DrawString($"Purpose : {request.VisitPurpose}", font, Brushes.Black, new System.Drawing.PointF(50, 270));

                    // Draw QR code
                    graphics.DrawImage(qrImage, new System.Drawing.Rectangle(150, 320, 300, 300));

                    // Draw Invitation ID at bottom center
                    var idFont = new Font("Arial", 14, System.Drawing.FontStyle.Bold);
                    var idText = $"Invitation ID: {invitationId}";
                    System.Drawing.SizeF idSize = graphics.MeasureString(idText, idFont);
                    float idX = (invitationImage.Width - idSize.Width) / 2;
                    float idY = invitationImage.Height - 50;
                    graphics.DrawString(idText, idFont, Brushes.Gray, new System.Drawing.PointF(idX, idY));
                }

                // 3. Convert image to Base64 string
                //string base64Image;
                using (var ms = new MemoryStream())
                {
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/Invitations", $"{invitationId}.png");
                    invitationImage.Save(savePath, ImageFormat.Png);

                    //invitationImage.Save(ms, ImageFormat.Png);
                    //base64Image = Convert.ToBase64String(ms.ToArray());
                }

                // 4. Insert into DB
                string field = "GuestId, HostId, CompanyId, VisitPurpose, VisitDate, VisitTime, Status, QRCode, Invitaion_ID";
                string values = $"'{request.GuestId}', '{userId}', '{CompanyId}', '{request.VisitPurpose}', '{request.VisitDate}', '{request.VisitTime}', 'Pending', '{invitationId}', '{invitationId}'";
                
                string Status = Cc.InsertionMethodStatus("SetInvitation", field, values);
                if (Status == "Saved")
                {
                    Cc.InvitationSendEmail(Email, request.GuestFullName, userName, invitationImage, request.VisitDate , timeFormatted);
                }
                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }

        //SetGusetInvitation
        [HttpPost]
        public JsonResult ValidateInvitation([FromBody] Admin request)
        {
            string Status = "";
            string Status2 = "";
            string Result = "";
            List<Admin> GetGuestList = new List<Admin>();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        GetGuestList = Cc.SelectAdmin("ValidateInvitation", request.Invitation_ID.ToString().Trim(),
                            request.VisitDate.ToString(), "0000-00-00", "0000-00-00").ToList();

                        Status = GetGuestList[0].Status;
                        var InvitationId = GetGuestList[0].InvitationId;


                        if (Status.ToString().Trim().Equals("GuestVerified"))
                        {
                            string field = "InvitationId , CheckInTime ,Invitation_ID,Type";
                            string values = $"'{InvitationId}', '{DateTime.Now}', '{request.Invitation_ID.ToString()}','Guest'";
                            Status2 = Cc.InsertionMethodStatus("SetAccessLogs", field, values);

                            var Userid = HttpContext.Session.GetInt32("user_credential_id");
                            var UserName = HttpContext.Session.GetString("user_name");
                            string message_title = "Alert !";
                            string message_txt = "Your guest " + request.GuestFullName + " has arrived at the reception.";

                            if (Status2 == "Saved")
                            {
                                string values2 = "Status='CheckedIn'";
                                Status2 = Cc.UpdationMethodReturn("UpdateInvitation", values2, InvitationId.ToString());
                            }
                            if (Status2 == "Saved")
                            {
                                string field2 = "receiver_user_id, sender_user_id , message_title, message_text";
                                string value2 = " '" + GetGuestList[0].Hostid + "', '" + Userid + "' , '" + message_title + "' , '" + message_txt + "' ";
                                Result = Cc.InsertionMethodStatus("setNotification", field2, value2);
                            }
                            if (Result == "Saved")
                            {
                                Task<IActionResult> task = SendNotification(InvitationId.ToString(),UserName.ToString(), message_txt, "notification");
                            }
                            return Json(Status2);
                        }
                        else if (Status.ToString().Equals("Checkout"))
                        {

                            string values2 = "Status='CheckedOut'";
                            Status2 = Cc.UpdationMethodReturn("UpdateInvitation", values2, InvitationId.ToString());

                            string values3 = "CheckOutTime='" + DateTime.Now + "'";
                            Status2 = Cc.UpdationMethodReturn("UpdateAccessLogs", values3, InvitationId.ToString());
                            if (Status2 == "Saved")
                            {
                                Task<IActionResult> task = SendNotification(InvitationId.ToString(), "UserName", "message", "notification");
                            }
                            return Json(Status);
                        }
                        else if (Status.ToString().Equals("InvalidInvitation"))
                        {
                            return Json(Status);
                        }
                        else if (Status.ToString().Equals("Invitationexpired"))
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

        //ValidateInvitation
        [HttpPost]
        public IActionResult VisitorInsertion([FromBody] Admin request)
        {

            if (request == null)
                return BadRequest("Invalid data received.");
            
            try{
                if (!Cc.CheckForInternetConnection())
                    return Json("NetworkError");

                if (!Cc.DatabaseConnectionCheck())
                    return Json("DataBaseError");
                string Status = "";
                string Result = "";

                var CNICfront = request.CNICfront.Replace("data:image/jpeg;base64,", "");
                var bytesF = Convert.FromBase64String(CNICfront);
                var fileNameF = $"IDCard_{DateTime.Now:yyyyMMddHHmmss}.png";
                var pathF = Path.Combine("wwwroot/assets/images/VisitorIDs", fileNameF);
                System.IO.File.WriteAllBytes(pathF, bytesF);

                //var CNICback = request.CNICback.Replace("data:image/png;base64,", "");
                //var bytesB = Convert.FromBase64String(CNICback);
                //var fileNameB = $"IDCard_{DateTime.Now:yyyyMMddHHmmss}.png";
                //var pathB = Path.Combine("wwwroot/assets/images/VisitorIDs", fileNameB);
                //System.IO.File.WriteAllBytes(pathB, bytesB);

                string field = "Name , Gender , CNICFront,  Company, Department, Employee,visitorPurpose,Employee_id";
                string values = $"'{request.VisitorFullName}', '{request.Gender}', '{fileNameF}', '{request.Company_name}', '{request.Department_name}', '{request.Employee_name}', '{request.VisitPurpose}', '{request.Employee_id}'";
                Status = Cc.InsertionMethodStatus("SetVisitor", field, values);

                var Userid = HttpContext.Session.GetInt32("user_credential_id");
                var UserName = HttpContext.Session.GetString("user_name");
                string message_title = "Alert !";
                string message_txt = "Walk-in visitor " + request.VisitorFullName + " has checked in to meet you. ";

                if (Status == "Saved")
                {
                    string field2 = "receiver_user_id, sender_user_id , message_title, message_text";
                    string value2 = " '" + request.Employee_id + "', '" + Userid + "' , '" + message_title + "' , '" + message_txt + "' ";
                    Result = Cc.InsertionMethodStatus("setNotification", field2, value2);
                }
                if (Result == "Saved")
                {
                    Task<IActionResult> task = SendNotification(Userid.ToString(), UserName.ToString(), message_txt, "notification");
                }

                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
        //Invite guest


        public JsonResult GetMessage()
        {
            List<Chat> GetMessage = new List<Chat>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = HttpContext.Session.GetInt32("user_credential_id");

                    GetMessage = Cc.SelectChatNotificaion("MessageList", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetMessage);
        }
        public JsonResult GetNotification()
        {
            List<Chat> GetNotification = new List<Chat>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = HttpContext.Session.GetInt32("user_credential_id");

                    GetNotification = Cc.SelectChatNotificaion("NotificationList", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetNotification);
        }


        public IActionResult ChatApp()
        {
            return View();
        }
        //public JsonResult GetList()
        //{
        //    List<Chat> GetList = new List<Chat>();

        //    if (Cc.CheckForInternetConnection() == true)
        //    {
        //        if (Cc.DatabaseConnectionCheck() == true)
        //        {
        //            var user_id = Session["user_credential_id"];

        //            GetList = Cc.SelectChatNotificaion("UserList", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
        //        }
        //        else
        //        {
        //            TempData["DbVarification"] = "UnConnected";
        //        }
        //    }
        //    else
        //    {
        //        TempData["NetVarification"] = "UnConnected";
        //    }
        //    return Json(GetList, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetuserData(string ID)
        //{
        //    List<Chat> GetuserData = new List<Chat>();

        //    if (Cc.CheckForInternetConnection() == true)
        //    {
        //        if (Cc.DatabaseConnectionCheck() == true)
        //        {
        //            GetuserData = Cc.SelectChatNotificaion("Userdata", ID.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
        //        }
        //        else
        //        {
        //            TempData["DbVarification"] = "UnConnected";
        //        }
        //    }
        //    else
        //    {
        //        TempData["NetVarification"] = "UnConnected";
        //    }
        //    return Json(GetuserData, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetChatbox(string ID)
        //{
        //    List<Chat> GetChatbox = new List<Chat>();

        //    if (Cc.CheckForInternetConnection() == true)
        //    {
        //        if (Cc.DatabaseConnectionCheck() == true)
        //        {
        //            var user_id = Session["user_credential_id"];
        //            var receiverID = ID;
        //            GetChatbox = Cc.SelectChatNotificaion("Chatbox", user_id.ToString(), "0000-00-00", "0000-00-00", receiverID.ToString()).ToList();
        //        }
        //        else
        //        {
        //            TempData["DbVarification"] = "UnConnected";
        //        }
        //    }
        //    else
        //    {
        //        TempData["NetVarification"] = "UnConnected";
        //    }
        //    return Json(GetChatbox, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult Sendmessage(Chat model)
        //{
        //    string status = "false";
        //    try
        //    {
        //        if (Cc.CheckForInternetConnection() == true)
        //        {
        //            if (Cc.DatabaseConnectionCheck() == true)
        //            {
        //                string id = Session["user_credential_id"].ToString();
        //                string name = Session["user_name"].ToString();

        //                if (model.attachment != null)
        //                {
        //                    int i = Cc.RandomNumber(10, 1000);
        //                    foreach (var items in model.attachment)
        //                    {
        //                        string DocumentationPath2 = "~/admin_assets/images/attachment";
        //                        bool exists2 = System.IO.Directory.Exists(Server.MapPath(DocumentationPath2));
        //                        if (!exists2)
        //                        {
        //                            System.IO.Directory.CreateDirectory(Server.MapPath(DocumentationPath2));
        //                        }
        //                        i++;
        //                        var picture = items;
        //                        var extensionitem3 = Path.GetExtension(items.FileName);
        //                        string img = Cc.RandomNumber(i, 90000) + extensionitem3;
        //                        picture.SaveAs(Server.MapPath(DocumentationPath2 + "/" + img));

        //                        string field = "receiver_user_id , sender_user_id, message_title, attachment";
        //                        string values = "'" + model.receiver_user_id + "','" + id + "','" + name + "' , '" + img + "' ";
        //                        status = Cc.InsertionMethodStatus("setNotification", field, values);

        //                    }
        //                }
        //                else
        //                {
        //                    string field = "receiver_user_id , sender_user_id, message_text , message_title";
        //                    string values = "'" + model.receiver_user_id + "','" + id + "','" + model.messege_text + "','" + name + "' ";
        //                    status = Cc.InsertionMethodStatus("setNotification", field, values);

        //                }
        //                if (status == "Saved")
        //                {
        //                    SendNotificationToAllUsers("New message", model.messege_text, model.receiver_user_id);
        //                    // Call SignalR hub to notify clients
        //                    var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        //                    context.Clients.All.broadcastMessage(model.receiver_user_id, name, model.messege_text, "message");
        //                }
        //            }
        //            else
        //            {
        //                TempData["DbVarification"] = "UnConnected";
        //                return Json("DataBaseError", JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            TempData["NetVarification"] = "UnConnected";
        //            return Json("NetworkError", JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Cc.WriteEventLog(ex.ToString());
        //        TempData["ExceptionError"] = "ExceptionError";
        //        return Json("ExceptionError", JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(status, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public IActionResult MarkasRead([FromBody] Chat model)
        {
            string result = "false";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (model == null || model.updateIDs == null || !model.updateIDs.Any())
                        {
                            return Json("InvalidData");
                        }
                        if (model.notificationType == "note")
                        {
                            foreach (var item in model.updateIDs)
                            {
                                string values = "isread='true'";
                                result = Cc.UpdationMethodReturn("updateNotication", values, item.ToString());
                            }
                        }
                        else if (model.notificationType == "msg")
                        {
                            foreach (var item in model.updateIDs)
                            {
                                string values = "isread='true'";
                                result = Cc.UpdationMethodReturn("updateMessage", values, item.ToString());
                            }
                        }
                    }
                    else
                    {
                        result = "DataBaseError";
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    result = "NetworkError";
                    TempData["NetVarification"] = "UnConnected";
                }

            }
            catch (Exception ex)
            {
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError");
            }
            return Json(result);
        }


        public IActionResult FileManager()
        {
            return View();
        }
        //public JsonResult SaveFile(Filemanager model)
        //{
        //    string status = "false";
        //    try
        //    {
        //        if (Cc.CheckForInternetConnection() == true)
        //        {
        //            if (Cc.DatabaseConnectionCheck() == true)
        //            {
        //                string id = Session["user_credential_id"].ToString();
        //                string name = Session["user_name"].ToString();

        //                foreach (var items in model.attachment)
        //                {
        //                    string DocumentationPath2 = "~/admin_assets/File/" + name + "";
        //                    bool exists2 = System.IO.Directory.Exists(Server.MapPath(DocumentationPath2));
        //                    if (!exists2)
        //                    {
        //                        System.IO.Directory.CreateDirectory(Server.MapPath(DocumentationPath2));
        //                    }
        //                    var picture = items;
        //                    //var extensionitem = Path.GetExtension(items.FileName);
        //                    //string img = Cc.RandomNumber(i, 90000) + extensionitem;
        //                    picture.SaveAs(Server.MapPath(DocumentationPath2 + "/" + items.FileName));

        //                    long bytes = items.ContentLength;
        //                    double fileSizeInMB = Cc.ConvertBytesToMB(bytes);

        //                    string field = "File_Name , File_size, User_id";
        //                    string values = "'" + items.FileName + "','" + fileSizeInMB + "','" + id + "' ";
        //                    status = Cc.InsertionMethodStatus("SetFile", field, values);

        //                }
        //            }
        //            else
        //            {
        //                TempData["DbVarification"] = "UnConnected";
        //                return Json("DataBaseError", JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            TempData["NetVarification"] = "UnConnected";
        //            return Json("NetworkError", JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Cc.WriteEventLog(ex.ToString());
        //        TempData["ExceptionError"] = "ExceptionError";
        //        return Json("ExceptionError", JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(status, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult ShareuserListView()
        //{
        //    login model = new login();
        //    try
        //    {
        //        if (Cc.CheckForInternetConnection() == true)
        //        {
        //            if (Cc.DatabaseConnectionCheck() == true)
        //            {
        //                List<login> InspectorList = Cc.loginSession("inspectorList", "0", "0");
        //                ViewBag.InspectorListVB = new SelectList(InspectorList, "user_credential_id", "user_name");
        //            }
        //            else
        //            {
        //                TempData["DbVarification"] = "UnConnected";
        //            }
        //        }
        //        else
        //        {
        //            TempData["NetVarification"] = "UnConnected";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Cc.WriteEventLog(ex.ToString());
        //        TempData["ExceptionError"] = "ExceptionError";
        //        return Json("ExceptionError", JsonRequestBehavior.AllowGet);
        //    }
        //    return PartialView("~/Views/PartialView/PartialViewUserList.cshtml", model);
        //}
        //public JsonResult SetShareFile(Filemanager model)
        //{
        //    string status = "";
        //    string values = null;
        //    try
        //    {
        //        if (Cc.CheckForInternetConnection() == true)
        //        {
        //            if (Cc.DatabaseConnectionCheck() == true)
        //            {
        //                if (model.checkbox.Length > 0)
        //                {
        //                    if (Session["user_name"] != null && Session["user_credential_id"] != null)
        //                    {
        //                        string name = Session["user_name"].ToString();

        //                        foreach (var items in model.checkbox)
        //                        {
        //                            string joinedIDs = string.Join(",", model.shared_id);

        //                            values = "Shared_id='" + joinedIDs + "'";

        //                            status = Cc.UpdationMethodReturn("updateDocument", values, items.ToString());
        //                        }
        //                        if (status == "Saved")
        //                        {
        //                            foreach (var items in model.shared_id)
        //                            {
        //                                string message_title = "Alert !";
        //                                string message_txt = " " + Session["user_name"] + "Share a file with you ";
        //                                string field = "receiver_user_id , message_title,message_text";
        //                                string value = " '" + items.ToString() + "' , '" + message_title + "' , '" + message_txt + "' ";
        //                                status = Cc.InsertionMethodStatus("setNotification", field, value);

        //                                SendNotificationToAllUsers("Alert", message_txt, Cc.ToInt32(items.ToString()));

        //                                // Call SignalR hub to notify clients
        //                                var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        //                                context.Clients.All.broadcastMessage(Cc.ToInt32(items.ToString()), name, message_txt, "notification");
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        status = "SessionDestory";
        //                    }
        //                }
        //                else
        //                {
        //                    status = "ExceptionError";
        //                }
        //            }
        //            else
        //            {
        //                TempData["DbVarification"] = "UnConnected";
        //                return Json("DataBaseError", JsonRequestBehavior.AllowGet);
        //                //status = "DbVarification";
        //            }
        //        }
        //        else
        //        {
        //            TempData["NetVarification"] = "UnConnected";
        //            return Json("NetworkError", JsonRequestBehavior.AllowGet);
        //            //status = "NetVarification";
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        //        //Cc.writeEventLog(opl + " : " + ex.ToString());
        //        //throw ex;
        //        TempData["ExceptionError"] = "ExceptionError";
        //        return Json("ExceptionError", JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(status, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetDocumentList()
        //{
        //    List<Filemanager> documentList = new List<Filemanager>();

        //    if (Cc.CheckForInternetConnection() == true)
        //    {
        //        if (Cc.DatabaseConnectionCheck() == true)
        //        {
        //            var user_id = Session["user_credential_id"];

        //            documentList = Cc.SelectDocument("DocumentList", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
        //        }
        //        else
        //        {
        //            TempData["DbVarification"] = "UnConnected";
        //        }
        //    }
        //    else
        //    {
        //        TempData["NetVarification"] = "UnConnected";
        //    }

        //    // Mock logic: Combine user images with same File_name
        //    var groupedResult = documentList
        //        .GroupBy(x => x.File_name)
        //        .Select(g => new
        //        {
        //            FileName = g.Key,
        //            Users = g.Select(u => new { u.user_name, u.user_img }).Distinct().ToList(),
        //            FileDetails = g.FirstOrDefault() // Get common file details (size, date, etc.)
        //        }).ToList();

        //    return Json(groupedResult, JsonRequestBehavior.AllowGet);
        //}

        public IActionResult Settings()
        {
            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    if (HttpContext.Session.GetString("Login") != null)
                    {
                        var Userid = HttpContext.Session.GetInt32("user_credential_id");
                        var ID = HttpContext.Session.GetInt32("company").ToString().Trim();

                        List<login> User_NameList = Cc.loginSession("EmployeeList", ID, "0").ToList();
                        ViewBag.User_NameVB = new SelectList(User_NameList, "user_credential_id", "user_name");
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                    return View();
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
                return View();
            }
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

    }
}