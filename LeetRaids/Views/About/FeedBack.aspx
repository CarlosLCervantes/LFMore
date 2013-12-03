<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Modal.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FeedBack
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
    <script type="text/javascript">
        function validate() {
            var errors = new Array();

            if ($("#Subject").val().length == 0) {
                errors.push("Please enter a subject.<br />");
            }
            
            if ($("#Message").val().length == 0) {
                errors.push("Please enter a message.<br />");
            }

            if (errors.length == 0) {
                $("#errors").hide();
                document.forms[0].submit();
            }
            else {
                $("#errors").html(errors.join(""));
                $("#errors").show();
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Submit Feedback</h2>
    
    <p>Your opinion is important to us. If you have any suggestions or problems we want to hear from you. Please fill out the form below and click send to contact us.</p>
    <p id="errors" class="errorMsg" style="display:none; margin-bottom:10px"></p>
    <% Html.BeginForm(); %>
        <strong>Subject:</strong> <input id="Subject" name="Subject" type="text" class="textboxLarge" style="width:370px;" maxlength="100" />
        <br /><br />
        <strong>Your Message:</strong> <span class="sideNote"><em>(1000 Character Max)</em></span>
        <br />
        <textarea id="Message" style="margin-top:5px" name="Message" type="text" maxlength="1000" rows="10" cols="50"></textarea>
        
        <a href="javascript:void(0)" style="display:block; margin:15px auto; text-align:center" onclick="validate();"><img src="../../Content/Images/Buttons/btnSend.png" /></a>
    <% Html.EndForm(); %>

</asp:Content>
