using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Napier_Bank_Message_Filtering_Service_NEW
{
    public abstract class Message
    {
        public char type;
        public string id;
        public string sender;
        public string text;

        //For emails
        public string subject;

        //For SIR
        public DateTime sir_date;
        public string sort_code;
        public string noi;

        public Message() { }
    }
}
