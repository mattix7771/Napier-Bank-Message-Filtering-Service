using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Napier_Bank_Message_Filtering_Service_NEW
{
    class Message
    {
        private char type;
        private string id;
        private string sender;
        private string text;

        //For emails
        private string subject;

        //For SIR
        private DateTime sir_date;
        private string sort_code;
        private string noi;

        public Message() { }
        public Message(char type, string id, string sender, string text)
        {
            this.type = type;
            this.id = id;
            this.sender = sender;
            this.text = text;
        }

        
        public char Type {set { type = value; } get { return type; } }
        public string Id { set { id = value; } get { return id; } }
        public string Sender { set { sender = value; } get { return sender; } }
        public string Text { set { text = value; } get { return text; } }
        public string Subject { set { subject = value; } get { return subject; } }
        public DateTime Sir_date { set { sir_date = value; } get { return sir_date; } }
        public string Sort_code { set { sort_code = value; } get { return sort_code; } }
        public string NOI { set { noi = value; } get { return noi; } }

    }
}
