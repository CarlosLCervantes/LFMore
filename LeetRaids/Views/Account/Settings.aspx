<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AccountSettings>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Register Src="../Shared/DropDowns/ddlHours.ascx" TagName="ddlHours" TagPrefix="hours" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - Account Settings
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="../../Content/Scripts/Validation.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Account Settings</h2>
    <p>Here you can modify your profile and contact information. Please keep your information up to date so we can reach you when your events update. </p>
    
    <%= ViewData.Eval("Success") %>
    
    <% Html.BeginForm(); %>
    
    <%= Html.ValidationSummary() %>
    
    <div id="divProfileSettings" class="settingsContainer">
        <div class="parentContainer">
            <strong class='containerHeaderText'>Profile Settings</strong>
        </div>
        <table>
            <tr>
                <td>Email Address:</td>
                <td><%= Html.TextBox("Email", null, new { @class = "text", onfocus = "selectTxt(this)", onblur = "unSelectTxt(this); val_Email(this)" })%></td>
                <td><span id="valEmail" class="valMsg">Incorrect</span></td>
            </tr>
           <%-- <tr>
                <td>Play Time:</td>
                <td><hours:ddlHours ID="ddlHours1" name="PlayTimeStart" runat="server" /> to <hours:ddlHours ID="ddlHours2" name="PlayTimeEnd" runat="server" /></td>
            </tr>--%>
        </table>
   </div>
   
   <%= Html.SubmitButton("btnSubmit", "Update", new { @id = "btnSubmitSettings" })%>
   
   <% Html.EndForm(); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
