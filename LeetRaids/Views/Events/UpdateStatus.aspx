<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Modal.Master" Inherits="System.Web.Mvc.ViewPage<UpdateStatusModel>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UpdateStatus
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
    <script type="text/javascript">
        function valForm() {
            var valid = true;
            var errors = new Array();
            if($("input[name='SelectedCharacter']:checked").length == 0) {
                valid = false;
                errors.push("Please select a character <br/>");    
            }

            if ($("input[name='Role']:checked").length == 0) {
                valid = false;
                errors.push("Please select a role <br/>");
            }
            else {
                if ($("input[name='SelectedCharacter']:checked").parent().prev("td").find("[name='Role']:checked").length == 0) {
                    valid = false;
                    errors.push("Select a Role for the right character <br/>");
                }
            }

            if (valid) {
                $("#frmJoinEvent").submit();
            }
            else {
                $("#erroMsg").html(errors.join(""));
            }
        }        
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<form id="frmJoinEvent" method="post" action="/Events/UpdateStatus">
    <h2>Update Status</h2>
    <p>
        Has your situation changed? Use this form to update your status for the selected event. You can also change your character or role anytime.
        Most MMO events are restricted to ccertain parameters such as the server your character is on. So don't be alarmed if some of the options on this form are locked.
    </p>
    <p>
        Note: Even though you sign up for an event the event creator may choose to update your status to stand-by or remove you from the event.
    </p>
    
    <p id="erroMsg" class="errorMsg"></p>
    <%= Html.ValidationSummary() %>
    
   <% if (Model.RestrictedRoles.Count() > 0)
       { %>
            <div id="roleRestrictionsWarning">
                <strong>Role Restrictions:</strong>
                The event creator has specified restrictions on certain roles. You will not be able to update your role to any of these roles. If you are already performing a restricted Role then you will not be effected. The following roles are full:
                <ul>
                    <% foreach (Role role in Model.RestrictedRoles)
                       { %>
                            <%= "<li>" + role.RoleName + "</li>" %>
                    <% } %>
                </ul>
            </div>
    <% } %>

    <% Html.RenderPartial("Characters/CharacterList", Model.UserCharacters, new ViewDataDictionary() { {"SelectedCharacter", Model.SelectedCharacter}, {"SelectedRole", Model.SelectedRole} }); %>
    <p class="note">Displaying only characters on the same server as the event</p>
    
    <table style="margin:15px auto;">
        <tr>
            <td>
                What is your status for this event?
                <select name="AttendeeStatus" id="ddlStatus" >
                    <option value="2">Accept</option>
                    <option value="3">Tentative</option>
                    <option value="4">Decline</option>
                    <option value="5">Standby</option>
                </select>
            </td>
            <td style="vertical-align:middle">
                Note: (50 char Max) <br /> <%= Html.TextArea("Note", Model.Note, new { rows = "7", cols = "30", maxlength = "50" })%>
            </td>
        </tr>
    </table>
   
    
    <a href="javascript:void(0);" onclick="valForm();"><img alt="Update Your Status" src="../../Content/Images/Buttons/btnUpdateStatus.png" style="margin: 15px auto; display:block;" /></a>
   
</form>
</asp:Content>
