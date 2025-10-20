namespace DVMS.Models
{
    public class Company
    {
        public int companyID { get; set; }
        public string companyName { get; set; }
        public string floorno { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string date_time { get; set; }
        public string tottalguest { get; set; }
        public string tottalvisitor { get; set; }
        public string image { get; set; }
        public IFormFile logo { get; set; }

    }
}
