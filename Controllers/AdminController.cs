using DVMS.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace DVMS.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult companies()
        {
            return View();
        }
        public IActionResult employee()
        {
            return View();
        }
        public IActionResult inviteguest()
        {
            return View();
        }
        public IActionResult trackguest()
        {
            return View();
        }
        public IActionResult trackvisitor()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }


        //public JsonResult CheckEmail(login model)
        //{
        //    string Status = "";
        //    try
        //    {
        //        if (Cc.CheckForInternetConnection() == true)
        //        {
        //            if (Cc.DatabaseConnectionCheck() == true)
        //            {
        //                Status = Cc.LoginVerification("UserRegistration", model.Email.ToString().Trim(), "0");
        //                if (Status.ToString().Trim().Equals("EmailAlreadyExist"))
        //                {
        //                    return Json(Status);
        //                }
        //                else if (Status.ToString().Equals("EmailVerified"))
        //                {
        //                    return Json(Status);
        //                }
        //            }
        //            else
        //            {
        //                TempData["DbVarification"] = "UnConnected";
        //                return Json("DataBaseError");
        //            }
        //        }
        //        else
        //        {
        //            TempData["NetVarification"] = "UnConnected";
        //            return Json("NetworkError");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.ToString());
        //    }
        //    return Json(Status);
        //}

        public IActionResult ValidateGuest([FromBody] Home request)
        {
            if (request == null)
            {
                return BadRequest("Invalid data received.");
            }
            //double lat = request.Lat;
            //double lng = request.Lng;

            string status = "";
            //string values = null;
            //var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
            //string name = HttpContext.Session.GetString("user_name").ToString();

            //values = "Price='" + request.offerprice + "',RequestDate='" + DateTime.Now + "'";
            //status = Cc.UpdationMethodReturn("updateRequest", values, request.ID.ToString());

            //Task<IActionResult> task = SendNotification(request.userID.ToString(), name, "message_txt", "Offer");
            return Json(status);
        }
    }
}