using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBNews.Models
{
   public class ResponseModel
    {
       public string message { get; set; }
       public DateTime created_time { get; set; }
       public string id { get; set; }

    }
}
