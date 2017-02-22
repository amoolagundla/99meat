using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _99meat.Models
{

    public class TokenData
    {
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }

    public class Meta
    {
        public string version { get; set; }
        public string request_id { get; set; }
        public int status { get; set; }
    }

    public class Datum
    {
        public bool valid { get; set; }
        public string token { get; set; }
        public string id { get; set; }
        public string app_id { get; set; }
        public DateTime? invalidated { get; set; }
        public string type { get; set; }
        public DateTime created { get; set; }
    }

}