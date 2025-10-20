namespace DVMS.Models
{
    public class Filemanager
    {
        //question
        public int Doc_ID { get; set; }
        public int user_id { get; set; }
        //public Array shared_id { get; set; }
        public List<int> shared_id { get; set; }
        public string get_share_id { get; set; }
        public Array checkbox { get; set; }
        public string File_name { get; set; }
        public string user_name { get; set; }
        public string user_img { get; set; }
        public string file_size { get; set; }
        public string owner { get; set; }

        public string date { get; set; }
        public string time { get; set; }

        public string attachmentread { get; set; }
        public IFormFile[] attachment { get; set; }

    }
}
