<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DataAccessLayer.Member>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - Login
</asp:Content>
<asp:Content ID="metaContent" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Description" content="Login to LFM to resume using our great tools." />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="../../Content/Scripts/Validation.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Login</h2>
    
    <p>Login to your LFMore account using your user name and password</p>
    
    <% Html.BeginForm("Login", "Account"); %>
    
    <table id="tblLogin">
        <tr>
            <td>User Name:</td>
            <td><%= Html.TextBox("UserName", null, new { onblur = "val_el($(this).val(), isNotEmpty, 'valUserName', 'static')" })%></td>
            <td><span class="valMsg" id="valUserName">Check User Name</span></td>
        </tr>
        <tr>
            <td>Password:</td>
            <td><%= Html.Password("Password", null, new { onblur = "val_el($(this).val(), isNotEmpty, 'valPassword', 'static')" })%></td>
            <td><span class="valMsg" id="valPassword">Check Password</span></td>
        </tr>
    </table>
   
    <br />
   
    <input id="btnLogin" style="height:25px; width:80px;" type="submit" value="Login" />
    
    <%  bool loginFailure = (TempData["LoginFailure"] != null) ? Convert.ToBoolean(TempData["LoginFailure"]) : false;
        if (loginFailure) {%>
            <p class="error">Your User Name or Password does not match</p>
        <%} %>
        
    <p><strong>Don't have an account?</strong> <%= Html.ActionLink("Click here to register an account", "Register", null, new { @class = "altLink" })%></p>
        
    <% Html.EndForm(); %>
    

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
