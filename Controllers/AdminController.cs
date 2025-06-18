using DVMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder.Core;
using SixLabors.ImageSharp;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.SignalR;
using DVMS.Hubs;
using System.Reflection;
using Microsoft.AspNetCore.Hosting.Server;
using SixLabors.ImageSharp.Formats;
using System.ComponentModel.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


namespace DVMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly CrudClass Cc;
        public AdminController(IConfiguration configuration)
        {
            Cc = new CrudClass(configuration);
            //_hubContext = hubContext;
            //_env = env;
        }
        //, IHubContext<ChatHub> hubContext, IWebHostEnvironment env
        //private readonly IHubContext<ChatHub> _hubContext;
        //private readonly IWebHostEnvironment _env;

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



        public IActionResult inviteguest()
        {
            //    var Userid = HttpContext.Session.GetInt32("user_credential_id").ToString();
            var Userid = "1";
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
                    GetGuestList = Cc.SelectAdmin("GetGuestList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
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
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetGuestList);
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
                
                var userId = "1";

                string field = "FullName  ,Email , Phone ,CNIC, UserID";
                string values = "'" + request.GuestFullName + "','" + request.GuestEmail + "', '" + request.GuestPhone + "', '" + request.GuestCNIC + "', '" + userId + "'";
                string Status =  Cc.InsertionMethodStatus("SetGeust", field, values);

                //await SendNotification(request.userID.ToString(), userName, "message_txt", "Offer");
                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }

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

                //var userId = HttpContext.Session.GetInt32("user_credential_id")?.ToString();
                //var userName = HttpContext.Session.GetString("user_name");
                //var CompanyId = HttpContext.Session.GetString("CompanyId");
                var userId = "1";
                var userName = "Amman";
                var CompanyId = "1";

                string invitationId = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/logo.png"); // update if needed
                string logoPath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/logoname.png"); // update if needed

                string timeFormatted = DateTime.ParseExact(request.VisitTime, "HH:mm", null).ToString("hh:mm tt");


                // 1. Generate QR code with logo
                Bitmap qrImage;
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrData = qrGenerator.CreateQrCode(
                        $"InvitationID; {invitationId}, Visitor; {request.GuestFullName}, Time; {request.VisitDate};{timeFormatted}, Host; {userName}",
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
                    graphics.DrawString($"Visitor: {request.GuestFullName}", font, Brushes.Black, new System.Drawing.PointF(50, 150));
                    graphics.DrawString($"Host: {userName}", font, Brushes.Black, new System.Drawing.PointF(50, 190));
                    graphics.DrawString($"Visit Time: {request.VisitDate:dd MMM yyyy} {timeFormatted}", font, Brushes.Black, new System.Drawing.PointF(50, 230));
                    graphics.DrawString($"Purpose: {request.VisitPurpose}", font, Brushes.Black, new System.Drawing.PointF(50, 270));

                    // Draw QR code
                    graphics.DrawImage(qrImage, new System.Drawing.Rectangle(150, 320, 300, 300));

                    // Draw Invitation ID at bottom center
                    var idFont = new Font("Arial", 14, FontStyle.Bold);
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

                return Json(Status);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }


        [HttpPost]
        public JsonResult ValidateInvitation([FromBody] Admin request)
        {
            string Status = "";
            List<Admin> GetGuestList = new List<Admin>();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        GetGuestList = Cc.SelectAdmin("ValidateInvitation", request.Invitation_ID.ToString().Trim(), request.VisitDate, "0000-00-00", "0000-00-00").ToList();

                        Status = GetGuestList[0].Status;
                        var InvitationId = GetGuestList[0].InvitationId;

                        if (Status.ToString().Trim().Equals("GuestVerified"))
                        {
                            string field = "InvitationId , CheckInTime ,Invitation_ID";
                            string values = $"'{InvitationId}', '{DateTime.Now:yyyyMMddHHmmss}', '{request.Invitation_ID.ToString()}'";
                            Status = Cc.InsertionMethodStatus("SetAccessLogs", field, values);
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
    }
}