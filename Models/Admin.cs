namespace DVMS.Models
{
    public class Admin
    {

        public int GusetCheckIn { get; set; }
        public string GusetCheckInTime { get; set; }
        public int GusetCheckOut { get; set; }
        public string GusetCheckOutTime { get; set; }
        public int VisitorCheckIn { get; set; }
        public string VisitorCheckInTime { get; set; }
        public int VisitorCheckOut { get; set; }
        public string VisitorCheckOutTime { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public string HostName { get; set; }

        //InvitationId
        public int InvitationId { get; set; }
        public int Hostid { get; set; }
        public int CompanyId { get; set; }
        public string VisitPurpose { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string Status { get; set; }
        public string QRCode { get; set; }
        //public string CheckIn { get; set; }
        //public string CheckOut { get; set; }
        public string Invitation_ID { get; set; }


        //Guest
        public int GuestId { get; set; }
        public string GuestFullName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string GuestCNIC { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        //Visitor
        public int VisitorId { get; set; }
        public string VisitorFullName { get; set; }
        public string Gender { get; set; }
        public int Company_id { get; set; }
        public string Company_name{ get; set; }

        public int Floor_no { get; set; }
        public string num { get; set; }

        public int Department_id { get; set; }
        public string Department_name { get; set; }
        public int Employee_id { get; set; }
        public string Employee_name { get; set; }
        public string CNICfront { get; set; }
        public string CNICback { get; set; }

    }
}
