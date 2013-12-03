<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Modal.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - Submit Feedback
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Submit Feedback</h2>
    
    <p>Your opinion is important to us. If you have any suggestions or problems we want to hear from you. Please fill out the form below and click send to contact us.</p>
    
    Subject: <input id="txtSubject" type="text" maxlength="100" />
    
    Body: <span class="smallNote">(1000 Character Max)</span>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>
