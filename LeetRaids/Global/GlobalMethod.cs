using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessLayer;

public static class GlobalMethod
{
    public static Member MemberInfo()
    {
        HttpContext context = System.Web.HttpContext.Current;
        Member mem = null;
        //mem = new Member(){MemberID = 3, Email = "Test2@Test.com",Password = "123456",CreateDT = new DateTime(2009, 11, 7, 14, 0, 0),PlayTimeEnd = new TimeSpan(1, 1, 1),PlayTimeStart = new TimeSpan(1, 1, 1),TimeZone = "-8.0", UserName="Test2"};
        if (context.Session[GlobalConst.SESSION_MEMBER] != null)
        {
            mem = context.Session[GlobalConst.SESSION_MEMBER] as Member;
        }
        return mem;
    }

    public static Event CurrentEvent()
    {
        HttpContext context = System.Web.HttpContext.Current;
        Event curEvent = null;
        //mem = new Member(){MemberID = 3, Email = "Test2@Test.com",Password = "123456",CreateDT = new DateTime(2009, 11, 7, 14, 0, 0),PlayTimeEnd = new TimeSpan(1, 1, 1),PlayTimeStart = new TimeSpan(1, 1, 1),TimeZone = "-8.0"};
        if (context.Session[GlobalConst.SESSION_CURRENT_EVENT] != null)
        {
            curEvent = context.Session[GlobalConst.SESSION_CURRENT_EVENT] as Event;
        }
        return curEvent;
    }


    public static void RedirectHome()
    {
        HttpContext.Current.Response.Redirect("/Home");
    }

    public static void RedirectBack()
    {
        HttpContext context = HttpContext.Current;
        string previousAction = context.Session[GlobalConst.SESSION_CURRENT_ACTION] as string;

        bool isRequestedPageThePreviousAction = (context.Request.Url.AbsolutePath == previousAction); //Doing this to make sure user never gets into endless redirect loop
        
        if (!String.IsNullOrEmpty(previousAction) && !isRequestedPageThePreviousAction)
        {
            HttpContext.Current.Response.Redirect(previousAction);
        }
        else
        {
            //Redirect Home
            GlobalMethod.RedirectHome();
        }
    }

    public static bool UserLoggedIn()
    {
        return (MemberInfo() != null);
    }

    public static string JSEncode(string input)
    {
        input = input.Replace("'", @"\'");
        input = input.Replace("\"", @"\""");

        return input;
    }
}
