using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace password_manager
{
    class Entry
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Site { get; set; }

        public Entry(string aSite,string aUsername, string aPassword)
        {
            Username = aUsername;
            Password= aPassword;
            Site = aSite;

        }

        public Entry()
        { }
    }
}
