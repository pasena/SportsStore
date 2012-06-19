using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "pa.sena@gmail.com";
        public string MailFromAddress = "pa.sena@gmail.com";
        public bool UseSsl = true;
        public string Username = "paulo.sena";
        public string Password = "J21vitor11";
        public string Servername = "smtp.google.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\sports_store_email";
    }
}
