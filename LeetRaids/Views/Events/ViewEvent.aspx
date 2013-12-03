<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ViewEventModel>" %>
<%@ Import Namespace="LeetRaids.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - View Event
</asp:Content>

<asp:Content ID="eventManageStyles" ContentPlaceHolderID="StylesContent" runat="server"></asp:Content>
<asp:Content ID="eventManageScripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="../../Content/Scripts/PageScripts/Shared/Event.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/PageScripts/ViewEvent.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function() { 
        load(<%= Model.EventInfo.EventID %>);}

        $(document).ready(function() {
            if($(".iframe").length > 0)
            {
                $('.iframe').fancybox({
                    "type": "iframe",
                    'width': 750,
                    'height': 700,
                    'hideOnOverlayClick': false,
                    'hideOnContentClick': false,
                    'titleShow': false,
                    'onClosed': function() { window.location = window.location; }
                });
            }
        });
    </script>
</asp:Content>

<asp:Content ID="eventManageMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    <h2>Event Information</h2>
    <p>
        This page presents you with an comprehensive overview of your group and provides many essential functions such as updating your status and leaving the group.
        You can see a breakdown of the event information, classes, and roles in the event overview panel. The roster provides you with a list of all the characters in
        your group.
    </p>
    <div class="colapseContainer">
        <table class="colapseSectionHeader">
            <tr>
                <td><h4>Role Restrictions</h4></td>
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
            <h3 id="raidConfigHeader">Players/Raid Configuration</h3>
            <br /><br />
            
            <p>
                <ul id="listRoleRestrictions">
                    <li id="roleRestLabel"><span class="restrictionLabel">Role Restrictions: </span></li>
                    <% foreach(EventRoleInfo role in Model.RolesInfo){ %>
                        <%= "<li><strong>" + role.Role.RoleName + "<strong>: " + role.RoleRestictionDisplay + "</li>"%>
                    <%} %>
                </ul>
            </p>
            
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
                    </tr>
                </thead>
                <tbody id="rosterTableBody">
                    
                </tbody>
            </table>
            <div id="manageRoster">
                <a href="javascript:void(0);" onclick="alert('This feature coming soon!')" title="Contact Event Organizer">Contact Organizer</a>
                <% if (Model.IsUserAttendee)
                   { %>
                   <a class="iframe" href="/Events/UpdateStatus?gameID=<%= Model.EventInfo.GameID %>" title="Update your Status, Note, or Character">Update Your Status</a> 
                   <a href="javascript:void(0);" onclick="initializeLeaveEvent($(this), <%= Model.EventInfo.EventID %>,'<%= GlobalMethod.JSEncode(Model.EventInfo.EventName) %>')" title="Leave this Event">Leave Events</a> 
                <% } 
                   else
                   {%>
                    <a id="lnkJoin" class="iframe" href="/Events/JoinEvent?gameID=<%= Model.EventInfo.GameID %>" title="Join This Event">Join this Event</a> <%-- Only if you are not in this event --%>
                <% } %>
            </div>
        </div>
    </div>
    
<%--    <div class="colapseContainer">
        <table class="colapseSectionHeader">
            <tr>
                <td><h4>Add a Player</h4></td>
                <td><a href='javascript:void(0)' onclick="showHide($('#addPlayer'), this)">Hide/Show</a></td>
            </tr>
        </table>
        <div id="addPlayer" style="background-color:#131313">
            <p>Adding a player to your event is easy. Either search from the list of Leetraids members to find someone with the stuff you need. Or add someone from your friends list with one click.</p>
            <a href="/Character/SearchCharacters" class="iframe" ><img src="../../Content/Images/Buttons/btnAddCharacter2.png"  style="margin:6px 90px;" /></a>
            <img src="../../Content/Images/Buttons/btnAddCharacter2.png" style="margin:6px 90px;" />
        </div>
    </div>--%>

    <% Html.RenderPartial("Events/RoleRestrictions", Model.RolesInfo, new ViewDataDictionary() { { "ReadOnly", true} }); %>
    
    </form>
    
    <div id="leaveEventDialog" class="dialog" style="" title="Leave this Event?">
        <p>Are you sure you want to leave this event?</p>
        <p class='dialogEventDetails'></p>
    </div>
    
</asp:Content>

<asp:Content ID="eventManageSideContent" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
