using System;
using System.Collections.Generic;
using System.Text;

namespace Chap04.Webservice {
    public static class Factory {
        public static ITestServiceSoap Create() {
            TestService srvc = new TestService();
            return srvc;
        }
    }
}
