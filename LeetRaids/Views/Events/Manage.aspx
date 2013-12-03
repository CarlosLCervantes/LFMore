<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LeetRaids.Models.ManageEventModel>" %>
<%@ Import Namespace="LeetRaids.Models" %>

<asp:Content ID="eventManageTitle" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - Manage Event
</asp:Content>

<asp:Content ID="eventManageStyles" ContentPlaceHolderID="StylesContent" runat="server"></asp:Content>
<asp:Content ID="eventManageScripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="../../Content/Scripts/PageScripts/ManageEvent.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function() {
        load(<%= Model.EventInfo.EventID %>);

        ;}
        
        $(document).ready(function() {
        $('#lnkSearchCharacters').fancybox({
            "type": "iframe",
            'width': 750,
            'height': 700,
            'hideOnOverlayClick': false,
            'hideOnContentClick': false,
            'titleShow': false
            //,'onClosed': function() { window.location = window.location; }
        });
        
        });
        
    </script>
</asp:Content>

<asp:Content ID="eventManageMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    <h2>Event Management</h2>
    <p>
        Only the event creator can access this page. This page presents you with an comprehensive overview of your group and provides administrative functions.
        You can see a breakdown of the event information, classes, and roles in the event overview panel. The roster provides you with a list of all the characters in
        your group. To add more players to your event use the "Add a player" panel.
    </p>
    <p>Players may also search your for your event and sign themselves up without you having to seek them out. Be sure to visit this page often.</p>
    <div class="colapseContainer">
        <table class="colapseSectionHeader">
            <tr>
                <td><h4>Event Overview</h4></td>
                <td class="colapseShowHide"><a href='javascript:void(0)' onclick="showHide($('#eventSummary'), this)">Hide/Show</a></td>
            </tr>
        </table>
        <div id="eventSummary" class="eventMgMtContentContainer">
            <br />
            <div class="summaryMainInfoHalf"><h3>Name: </h3><span class="highlightText"><%= Model.EventInfo.EventName %></span></div>
            <div class="summaryMainInfo top"><h3>Date: </h3><span class="highlightText"><%= Model.EventInfo.Date.ToShortDateString() %></span></div>
            <div class="summaryMainInfo top"><h3>Time: </h3><span class="highlightText"><%= Model.EventInfo.GetDisplayTime() %></span></div>
            <div class="summaryMainInfo"><h3>Players: </h3><span class="highlightText"><%= Model.Attendees.Count() %>/<%= Model.EventTypeInfo.PlayerCount %></span></div>
            <div class="summaryMainInfo"><h3>Meetup: </h3><span class="highlightText"><%= Model.EventInfo.MeetupLocation %></span></div>
            <div class="summaryMainInfo"><h3>Server: </h3><span class="highlightText"><%= Model.ServerName %></span></div>
            <div class="summaryMainInfo"><h3>Zone: </h3><span class="highlightText"><%= Model.EventTypeInfo.EventTypeName %></span></div>
            
            
            <br class="clear" /><br /><br /><br />
            <h3 id="raidConfigHeader">Players/Raid Configuration</h3> <%--<a id="editConfig">Edit Confguration</a> --%>
            <%= Html.ActionLink("Edit Event Configuration", "Edit", new {id = Model.EventInfo.EventID}, new { @id = "editConfig"}) %>
            <br /><br />
            
            <p>
            <ul id="listRoleRestrictions">
                <li id="roleRestLabel"><span class="restrictionLabel">Role Restrictions: </span></li>
                <% foreach(EventRoleInfo role in Model.RolesInfo){ %>
                    <%= "<li><strong>" + role.Role.RoleName + "<strong>: " + role.RoleRestictionDisplay + "</li>"%>
                <%} %>
            </ul>
            </p>
<%--            <table id="tblRoleOverview" cellpadding="0" cellspacing="0">
                <tr>
                    <td><strong>Role Restrictions:</strong></td>
                    <% foreach(EventRoleInfo role in Model.RolesInfo){ %>
                        <%= "<td>" + role.Role.RoleName + ": " + role.RoleRestictionDisplay + "</td>"%>
                    <%} %>
                </tr>
            </table>--%>
            
            <p><span id="classRestLabel" class="restrictionLabel">Class Overview</span> (Total Attending / Restriction)</p>
            
            <table id="classOverviewTable" cellpadding="0" cellspacing="0">
                <% int colCount = 0; %>
                <% for (int i = 0; i < Model.ClassInfo.Count(); i++)
                   { %>
                    <%if(colCount == 0){%>
                        <%= "<tr>" %>
                    <%} %>
                    <% colCount++; %>
                    <%= "<td class='tdImg'>" + "<img src='../../Content/Images/ClassIcons/" + Model.ClassInfo[i].Class.ImageLocation + "' /> </td>"%>
                    <%--<%= "<td>" + Model.AttnCharacters.Where(a => a.ClassID == Model.Classes[i].ClassID).Count() + "/" + "0" + "</td>" %>--%>
                    <%= "<td class='tdRestriction'>" + Model.ClassInfo[i].ClassRestrictionDisplay + "</td>" %>
                    <%if (colCount == 5 || i == Model.ClassInfo.Count() - 1)
                      {%>
                        <% colCount = 0; %>
                        <%= "</tr>" %>
                    <%} %>
                <% } %>
            </table>
            <br /><br />
            <h3>Give this link to friends for sign-up: </h3><span><%= Model.EventInfo.GenerateEventURL(HttpContext.Current) %></span>
        </div>
    </div>
    
    <div class="colapseContainer">
        <table class="colapseSectionHeader">
            <tr>
                <td><h4>Roster</h4></td>
                <td class="colapseShowHide"><a href='javascript:void(0)' onclick="showHide($('#rosterDiv'), this)">Hide/Show</a></td>
            </tr>
        </table>
        <div id="rosterDiv" class="eventMgMtContentContainer">
            <table id="rosterTable">
                <thead>
                    <tr>
                        <th>Class</th>
                        <th>Name</th>
                        <th>Status</th>
                        <th>Role</th>
                        <th>LvL</th>
                        <th>Note</th>
                        <th>Select</th>
                    </tr>
                </thead>
                <tbody id="rosterTableBody">
                    
                </tbody>
            </table>
            <div id="manageRoster">
                <a href="javascript:void(0);" onclick="alert('This feature coming soon!')" title="Contact all selected players">Contact Player(s)</a>
                <a href="javascript:void(0);" onclick="showUpdateStatus()" title="Update Status of all selected players">Update Player(s) Status</a>
                <a href="javascript:void(0);" onclick="showRemoveAttn()" title="Remove all selected players" >Delete Player(s)</a>
            </div>
        </div>
    </div>
    
    <div class="colapseContainer">
        <table class="colapseSectionHeader">
            <tr>
                <td><h4>Add a Player</h4></td>
                <td class="colapseShowHide"><a href='javascript:void(0)' onclick="showHide($('#addPlayer'), this)">Hide/Show</a></td>
            </tr>
        </table>
        <div id="addPlayer" class="eventMgMtContentContainer">
            <p>Adding a player to your event is easy. Either search from the list of LFMore members to find someone with the stuff you need. Or add someone from your friends list with one click.</p>
            <%= Html.ActionLink("Proceed to Invite Players", "Invite", new { }, new { style = "text-align:center;margin:auto;display:block;" })%>
            <%--<a id="lnkSearchCharacters" href="/Character/SearchCharacters" style="margin:6px 90px;" >Search Characters</a>
            <a id="lnkAddFriend" href="/Account/FriendsList" style="margin:6px 90px;" >Add a Friend</a>--%>
            <%--<img src="../../Content/Images/Buttons/btnAddCharacter2.png"  style="margin:6px 90px;" />--%>
            <%--<img src="../../Content/Images/Buttons/btnAddCharacter2.png" style="margin:6px 90px;" />--%>
        </div>
    </div>

    <% Html.RenderPartial("Events/RoleRestrictions", Model.RolesInfo, new ViewDataDictionary() { { "ReadOnly", true } }); %>
    
    
    </form>
    
    <div id="updateAttnStatusDialog" class="dialog" style="" title="Update the status of these Attendees?">
        <p>Select the new status for the Attendees selected:</p>
        
        <select name="AttendeeStatus" id="ddlStatus" >
            <option value="-1">Select a Status</option>
            <option value="2">Accept</option>
            <option value="3">Tentative</option>
            <option value="4">Decline</option>
            <option value="5">Standby</option>
        </select>
        
        <p id="updateStatusMsg" style="display:none" class="errorMsg">Please select a Status</p>
    </div>
    

    
    <div id="removeAttnDialog" class="dialog" style="" title="Remove these Attendees?">
        <p>Are you sure you want to remove the Attendees?</p>
    </div>
    
    
    

</asp:Content>

<asp:Content ID="eventManageSideContent" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
