using System.Data.SqlTypes;

namespace DVMS.Models
{
    public class login
    {
        //public List<login> SearchServiceListcancle { get; set; }
        public int user_credential_id { get; set; }
        public string login_id { get; set; }
        public string Password { get; set; }
        public string login_type { get; set; }
        public string user_name { get; set; } 
        public string Email { get; set; }
        public string user_mobileNo { get; set; }
        public int company { get; set; }
        public string department { get; set; }
        public string user_img { get; set; }
        public IFormFile user_pic { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string active { get; set; }


        public int module_id { get; set; }
        public string module_name { get; set; }
        public int can_read { get; set; }
        public int can_create { get; set; }
        public int can_delete { get; set; }
        public int can_update { get; set; }
        public int can_print { get; set; }
        public int can_report { get; set; }

    }
}
