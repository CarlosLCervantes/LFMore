<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - Page Not Found
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Page Not Found</h2>
    
    <p>The page you requested could not be found.</p>
    
    <%= Html.ActionLink("<< Return To The Previous Page", "ReturnPrevious", "Shared", null, new { @class = "altLink" })%>
    <br /><br />
    <%= Html.ActionLink("<< Return To The Home Page", "ReturnHome", "Shared", null, new { @class = "altLink" })%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
