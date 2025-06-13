namespace DVMS.Models
{
    public class Admin
    {
        public int ID { get; set; }
        public string Name { get; set; }
        
        //InvitationId
        public int InvitationId { get; set; }
        public int Hostid { get; set; }
        public int CompanyId { get; set; }
        public string VisitPurpose { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string Status { get; set; }
        public string QRCode { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
            
        //Guest
        public int GuestId { get; set; }
        public string GuestFullName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string GuestCNIC { get; set; }

    }
}
