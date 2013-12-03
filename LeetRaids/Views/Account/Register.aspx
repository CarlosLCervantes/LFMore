<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DataAccessLayer.Member>" %>

<%@ Register Src="../Shared/vucStateList.ascx" TagName="vucStateList" TagPrefix="uc1" %>
<%@ Register Src="~/Views/Shared/DropDowns/ddlTimeZones.ascx" TagName="ddlTimeZones"
    TagPrefix="ucTimeZone" %>
<%@ Register Src="../Shared/DropDowns/ddlHours.ascx" TagName="ddlHours" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    LFMore - Register
</asp:Content>
<asp:Content ID="metaContent" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Description" content="Register with LFM and start using our time saving tools for free." />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript" src="../../Content/Scripts/Validation.js"></script>
    <script type="text/javascript">
        function valForm() {
            var valid = true;

            var txtUserName = document.getElementById("UserName");
            if (!val_userName(txtUserName)) {
                valid = false;
            }

            var txtEmail = document.getElementById("Email");
            if (!val_Email(txtEmail)) {
                valid = false;
            }

            var txtPassword = document.getElementById("Password");
            if (val_IsNotEmpty(txtPassword, 'valPassword')) {
                alert("3");
                valid = false;
            }

            var txtPassConfirm = document.getElementById("ConfirmPassword");
            if (!val_PasswordConfirm('Password', txtPassConfirm)) {
                valid = false;
            }

            if (!$("#AgreedToTerms").attr("checked")) {
                $("#valTerms").css("visibility", "visible");
                valid = false;
            }
            else {
                $("#valTerms").css("visibility", "hidden");
            }

            if (valid) {
                document.forms[0].submit();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register</h2>
    <p>
        By creating an account with LFMore you are able to signup, schedule, and manage MMO events(Raids, PvP, etc.). You can then invite your friends and guild members to 
        join the event. With LFMore it is easy to find and cooridinate players to make sure your event is successful before you even log in.
    </p>
    <form id="form1" runat="server" method="post" action="Register">
    <%= Html.ValidationSummary() %>
    
    <table id="register">
        <tr>
            <td class="tdLeft">*UserName</td>
            <td>
                <%= Html.TextBox("UserName", null, new { @class = "text", onfocus= "selectTxt(this)", onblur = "unSelectTxt(this); val_userName(this)" })%>
                <span id="valUserName" class="valMsg">*</span>
            </td>
        </tr>
        <tr>
            <td colspan="2"><em class="smallNote">Username must be between 5-15 characters</em></td>
        </tr>
        <tr>
            <td class="tdLeft">*Email Address</td>
            <td>
                <%= Html.TextBox("Email", null, new { @class = "text", onfocus= "selectTxt(this)", onblur = "unSelectTxt(this); val_Email(this)"  })%>
                <span id="valEmail" class="valMsg">Incorrect</span>
            </td>
        </tr>
        <tr>
            <td class="tdLeft">*Password</td>
            <td>
                <!-- Remove ability for validaiton to accept spaces-->
                <%= Html.Password("Password", null, new { @class = "text", onfocus = "selectTxt(this)", onblur = "unSelectTxt(this); val_IsNotEmpty(this, 'valPassword')" })%>
                <span id="valPassword" class="valMsg">Can't be empty</span>
            </td>
        </tr>
        <tr>
            <td colspan="2"><em class="smallNote">Password must be between 6-14 characters</em></td>
        </tr>
        <tr>
            <td class="tdLeft">*ConfirmPassword</td>
            <td>
                <%= Html.Password("ConfirmPassword", null, new { @class = "text", onfocus = "selectTxt(this)", onblur = "unSelectTxt(this); val_PasswordConfirm('Password', this)" })%>
                <span id="valPasswordConfirm" class="valMsg">Passwords Must Match</span>
            </td>
        </tr>
        <tr>
            <td class="tdLeft">TimeZone</td>
            <td><ucTimeZone:ddlTimeZones name="TimeZone" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLeft">PlayTime</td>
            <td><uc2:ddlHours ID="ddlHours1" name="PlayTimeStart" runat="server" /> to<uc2:ddlHours ID="ddlHours2" name="PlayTimeStart" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2">
                *I agree to the <a class="altLink" href="/About/TermsOfUse" target="_blank">Terms of Use</a> and <a class="altLink" href="/About/PrivacyPolicy" target="_blank">Privacy Policy</a> <%= Html.CheckBox("AgreedToTerms", false, new {}) %>
                <span id="valTerms" class="valMsg">You must Agree to the Terms</span>
            </td>
        </tr>
        <tr>
            <td></td>
            <td style="text-align: right;">
                <input id="btnNext" class="submit" type="button" value="Next" onclick="valForm()" />
            </td>
        </tr>
    </table>
    <p style="text-align:center;">* Denotes a required field</p>
    </form>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
