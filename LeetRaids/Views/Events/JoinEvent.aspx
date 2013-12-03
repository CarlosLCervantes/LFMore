<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Modal.Master" Inherits="System.Web.Mvc.ViewPage<JoinEventModel>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	JoinEvent
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
            
            if($("input[name='Role']:checked").length == 0)
            {
                valid = false;
                errors.push("Please select a role <br/>");
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
<form id="frmJoinEvent" method="post" action="/Events/JoinEvent">
    <h2>JoinEvent</h2>
    <p>
        Feel like joining this event? You canÅc as long as you meet the requirements the creator has set up. Also, you can always contact the event creator via the View Event page and ask him to let you in.
        Most MMO events are restricted to ccertain parameters such as the server your character is on. So don't be alarmed if some of the options on this form are locked.
    </p>
    <p>Note: Even though you sign up for an event the event creator may choose to update your status to stand-by or remove you from the event.</p>
    
    <p id="erroMsg" class="errorMsg"></p>
    <%= Html.ValidationSummary() %>
    
    <% if (Model.RestrictedRoles.Count() > 0)
       { %>
            <div id="roleRestrictionsWarning">
                <strong>Role Restrictions:</strong>
                The event creator has specified restrictions on certain roles. You will not be able to perform these roles in this event. The following roles are full:
                <ul>
                    <% foreach (Role role in Model.RestrictedRoles)
                       { %>
                            <%= "<li>" + role.RoleName + "</li>" %>
                    <% } %>
                </ul>
            </div>
    <% } %>
    
    <% Html.RenderPartial("Characters/CharacterList", Model.UserCharacters , new ViewDataDictionary() { { "ReadOnly", false } }); %>
    <% if(!String.IsNullOrEmpty(Model.CantJoinEventReason)){ %>
        <p class="fyiText" style="text-align:center;"><%= Model.CantJoinEventReason %></p>
    <%} %>
    <%--<p class="note">Displaying only characters on the same server as the event</p>--%>
    
    <table style="margin:15px auto;">
        <tr>
            <td>
                What is your status for this event?
                <% if (!Model.IsEventFull)
                   {%>
                    <select name="AttendeeStatus" id="ddlStatus" >
                        <option value="2">Accept</option>
                        <option value="3">Tentative</option>
                        <option value="4">Decline</option>
                    </select>
                <%} 
                   else{%>
                    <select name="AttendeeStatus" id="Select1" >
                        <option value="5">Standby</option>
                    </select>
                    <br />
                    This event is full. You can only join this event as a standby player. If a spot opens up the organizer may bump you to an accepted status.
                   <%} %>
                   
            </td>
            <td style="vertical-align:middle">
                Note: (50 char Max) <br /> <%= Html.TextArea("Note", Model.Note, new { rows = "7", cols = "30", maxlength = "50" })%>
            </td>
        </tr>
    </table>
   
    
    <a href="javascript:void(0);" onclick="valForm();"><img alt="Join This Event" src="../../Content/Images/Buttons/btnJoinEvent.png" style="margin: 15px auto; display:block;" /></a>
   
</form>
</asp:Content>
