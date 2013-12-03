using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Collections.Specialized;

namespace DataAccessLayer
{
    public class BusinessLogicException : Exception
    {
        public NameValueCollection logicErrors = new NameValueCollection();

        public BusinessLogicException(List<String> errors)
        {
            string xmlErrorsFileLoc = ConfigurationManager.AppSettings["ERROR_XML_LOCATION"];
            XDocument errorDoc = XDocument.Load(xmlErrorsFileLoc);
            foreach (string errorName in errors)
            {
                logicErrors.Add(errorName, GetErrorMessageFromErrorName(errorDoc, errorName));
            }
        }

        public string GetErrorMessageFromErrorName(XDocument errorDoc, string errorName)
        {
            var error = (from err in errorDoc.Descendants("error")
                        where err.Element("name").Value == errorName
                        select err).SingleOrDefault();

            return error.Element("displayText").Value;
        }

        public void RegisterWithModel(ModelStateDictionary modelState)
        {
            foreach(string errorKey in logicErrors)
            {
                foreach(string errorValue in logicErrors.GetValues(errorKey))
                {
                    modelState.AddModelError(errorKey, errorValue);
                }
            }

        }
    }
}
