<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="LeetRaids.Models" %>
<%@ Import Namespace="DataAccessLayer" %>

<div id="divLoginPanel">
    <%if (Convert.ToBoolean(ViewData.Eval("UserLoggedIn")))
      { %>
        <div id="loginInfo">
            <span class="loginStatus">Welcome, <%= GlobalMethod.MemberInfo().UserName%></span>
            <a href="/Account/Logout"><img style="vertical-align:middle" src="../../Content/Images/Buttons/logout_small.png" /></a>
        </div>
    <%}
      else
      {%>
<%--        <form id="frmLogin">--%>
            <table id="tblLoginPanel">
                <tr>
                    <td colspan="2">
                        <p style="font-size:1.15em;">Login to LFMore:</p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <a href="/Account/Login"><img alt="Login To Your Account" src="../../Content/Images/Buttons/login_small.png" /></a>
                    </td>
                    <td>
                        <a href="/Account/Register"><img alt="Register an Account" src="../../Content/Images/Buttons/joinNow.png" /></a>
                    </td>
                </tr>
            </table>
<%--        </form>--%>
    <%} %>
</div>

<%--                <tr>
                    <td>User Name:</td>
                    <td><%= Html.TextBox("UserName", null, new { /*onblur = "val_el($(this).val(), isNotEmpty, 'valUserName', 'static')"*/ })%></td>
                    <td><span class="valMsg" id="valUserName">Check User Name</span></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td><%= Html.TextBox("Password", null, new { /*onblur = "val_el($(this).val(), isNotEmpty, 'valPassword', 'static')"*/ })%></td>
                    <td><span class="valMsg" id="valPassword">Check Password</span></td>
                </tr>--%>