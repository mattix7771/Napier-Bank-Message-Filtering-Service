using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Napier_Bank_Message_Filtering_Service_NEW
{
    public class SMS : Message
    {
        private char type;
        private string id;
        private string sender;
        private string text;


        //Constructor for SMS
        public SMS() { }
        public SMS(char type, string id, string sender, string text)
        {
            this.type = type;
            this.id = id;
            this.sender = sender;
            this.text = text;
        }

        //Setters and Getters
        public char Type { set { type = value; } get { return type; } }
        public string Id { set { id = value; } get { return id; } }
        public string Sender { set { sender = value; } get { return sender; } }
        public string Text { set { text = value; } get { return text; } }
    }
}
