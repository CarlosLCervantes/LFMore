<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Modal.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EventOverview>>" %>
<%@ Import Namespace="LeetRaids.Models" %>
<%@ Import Namespace="DataAccessLayer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    EventsOverview
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
    <script src="../../Content/Scripts/PageScripts/Shared/Event.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/PageScripts/EventsOverview.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Events Overview - <%= ((DateTime)ViewData["Date"]).ToLongDateString() %></h2>
        <p>
            You are currently viewing events for the day of <%= ((DateTime)ViewData["Date"]).ToLongDateString() %>. Click on one of the buttons next the raid 
            information to get more detailed information. Or if you are the creator of an event, then you can also manage and edit these events.
        </p>
        <div class="parentContainer" id="evtOverviewContainer">
            <table class="containerHeader">
                <tr>
                    <td>Your Events for Today: </td>
                    <td style="text-align:right"><a target="_parent" style="color:White;font-weight:bold; margin-right:15px" href='/Events/Add?date=<%= ((DateTime)ViewData["Date"]).ToString("MM-dd-yyy") %>'>Add New Event</a> </td>
                </tr>
            </table>
            <%if (Model.Count() > 0)
              { %>
                <% foreach (EventOverview e in Model) //Begin ForEach
                   {%>
                   <% bool eventExpired = (e.EventInfo.IsExpired() || !e.EventInfo.Active); %>
                   <% string tblClassName = (eventExpired) ? "eventInfoTable inactive" : "eventInfoTable"; %>
                    <table id="eventInfoTable" class="<%= tblClassName %>" cellspacing="0" cellpadding="0">
                        <tr>
                            <td rowspan="3" class="thumb">
                            
                                <img alt="Event Image" style="width:150px;height:125px;" src="../../Content/Images/DungeonThumbs/<%= e.EventImageFile %>" />
                            </td>
                            <td><strong>Name: <%= e.EventInfo.EventName%></strong></td>
                            <td><strong>Date: <%= e.EventInfo.Date.ToShortDateString()%></strong></td>
                            <%--<td><strong>Time: <%= DataAccessLayer.Global.Global.Convert24HourTimeToMeridian(e.EventInfo.EventTime)%></strong></td>--%>
                            <td><strong>Time: <%= e.EventInfo.GetDisplayTime() %></strong></td>
                            <td class="button" style="vertical-align: bottom;">
                                <% if (e.UserIsCreator && !eventExpired)
                                   { %>
                                    <a target="_parent" href='/Events/Manage/<%= e.EventInfo.EventID %>'>
                                        <img alt="Manage Event" src="../../Content/Images/Buttons/btnManage.png" />
                                    </a>    
                                <% }
                                   else
                                   {%>
                                    <a target="_parent" href='/Events/ViewEvent/<%= e.EventInfo.EventID %>'>
                                        <img alt="View Event" src="../../Content/Images/Buttons/btnView_small.png" />
                                    </a>   
                                   <% }%>
                            </td>
                        </tr>
                        <tr>
                            <td><strong>Type: <%= e.EventTypeName%></strong></td>
                            <td><strong>Players: <%= e.CurrentAttendees%></strong></td>
                            <td></td>
                            <td class="button edit">
                                <% if (e.UserIsCreator && !eventExpired)
                                   { %>
                                    <a target="_parent" href='/Events/Edit/<%= e.EventInfo.EventID %>'>
                                        <img alt="Edit Event" src="../../Content/Images/Buttons/btnEdit.png" />
                                    </a>
                                <%} %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <strong>Give this link to friends for sign-ups:</strong><br />
                                <span><%= e.EventInfo.GenerateEventURL(HttpContext.Current) %></span>
                            </td>
                            <td class="button delete" style="vertical-align: top;">
                            <% if (eventExpired)
                               { %>
                                    <p>Event has expired</p>
                            <%} %>
                            <% else if (e.UserIsCreator && !eventExpired)
                               { %>
                                    <a alt="Deactivate Event" href="javascript:void(0);" onclick="initializeDeactivate($(this), <%= e.EventInfo.EventID %>, '<%= GlobalMethod.JSEncode(e.EventInfo.EventName) %>')"><img src="../../Content/Images/Buttons/btnDelete.png" /></a>
                            <%} %>
                            <% else if (!e.UserIsCreator && !eventExpired)
                               { %>
                                    <a alt="Leave Event" href="javascript:void(0);" onclick="initializeLeaveEvent($(this), <%= e.EventInfo.EventID %>, '<%= GlobalMethod.JSEncode(e.EventInfo.EventName) %>')"><img src="../../Content/Images/Buttons/btnLeave_small.png" /></a>
                            <%} %>
                            </td>
                        </tr>
                    </table>
                <%} //End For Each%>
            <%}
              else
              {%>

                <h2 style="margin:10px;text-align:center;display:block;">No Events Found For this Day</h2>
                <% = ((bool)ViewData["BadDate"]) ? "<h4 style='text-align:center;display:block;'>The date being search was invalid</h4>" : "" %>

              <%} %>
        </div>
        
        <div id="deactivateEventDialog" class="dialog" style="" title="Confirm Event Deactivation">
	        <p>Are you sure you want to deactivate this event?</p>
	        <p class='dialogEventDetails'></p>
	        <p>You will not be able to re-active this event once it goes inactive. If you deactive this event it will be stored for history puposes so you can view the event info later on.</p>
        </div>
        
        <div id="leaveEventDialog" class="dialog" style="height:350px;width:350px;" title="Leave this Event?">
	        <p>Are you sure you want to leave this event?</p>
	        <p class='dialogEventDetails' style="font-weight:bold;"></p>
        </div>
</asp:Content>
