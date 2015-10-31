using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelEmailTemplate
    {
        public Double EmailTemplateNo { get; set; }
        public String EmailTemplate { get; set; }
        public String EmailPurpose { get; set; }
        public String EmailSubjectEN { get; set; }
        public String EmailSubjectAR { get; set; }
        public String EmailEN { get; set; }
        public String EmailAR { get; set; }
        public String EmailTextEN { get; set; }
        public String EmailTextAR { get; set; }
        public String EmailSMTP { get; set; }
        public String EmailUserName { get; set; }
        public String Emailpassword { get; set; }
        public String EmailSender { get; set; }
    }
}
