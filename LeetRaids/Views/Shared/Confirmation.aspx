<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Modal.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Confirmation
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h4><%= TempData[GlobalConst.TEMPDATA_CONFIRMATION_MESSAGE] %></h4>
        <br />
        <br />
        <br />
        <br />
        <a href="javascript:void(0)" onclick="parent.$.fancybox.close();" style="margin-top:25px;" class="close_fancy">Close this Window</a>
</asp:Content>
