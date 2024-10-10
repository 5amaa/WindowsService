using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceTutorial.DTOs
{
    public class SmsDTO
    {
        public int SmsId { get; set; } 
        public string PhoneNumber { get; set; } 
        public string SmsContent { get; set; }
    }
}
