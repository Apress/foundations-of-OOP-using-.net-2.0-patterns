using System;

namespace NHibernate.Examples.QuickStart {
    public class User {
        private string _id;
        private string _userName;
        private string _password;
        private string _emailAddress;
         
        
        public User() {
        }
        
        public User( string id, string username, string password, string emailaddress) {
            _id = id;
            _userName = username;
            _password =password;
            _emailAddress = emailaddress;
        }
        public string Id {
            get { return _id; }
            internal set { _id = value; }
        }
        
        public string UserName {
            get { return _userName; }
            internal set { _userName = value; }
        }
        
        public string Password {
            get { return _password; }
            internal set { _password = value; }
        }
        
        public string EmailAddress {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }
        
    }
}
