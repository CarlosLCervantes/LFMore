using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace LeetRaids.Tests
{
    public class TestValues
    {
        protected XDocument testValues;

        public TestValues(string dataSourceXml)
        {
            testValues = XDocument.Load(dataSourceXml);
        }
    }

    public class RegistrationTestValues : TestValues
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public RegistrationTestValues(string dataSourceXml) : base(dataSourceXml)
        {
            //userName = (from elm in testValues.Descendants("TestValues").Descendants("Register")
            //            select String.Concat(elm.Element("Base").Value, elm.Element("Count").Value)).SingleOrDefault();

            email = (from elm in testValues.Descendants("TestValues").Descendants("Email")
                       select elm.Element("Base").Value + elm.Element("Count").Value + "@" + elm.Element("Domain").Value).SingleOrDefault();

            //email = (from elm in testValues.Descendants("Email")
            //         select String.Concat(elm.Element("Base").Value, elm.Element("Count").Value, elm.Element("Domain").Value)).SingleOrDefault();
        }

    }
}
