<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RaidCalendar>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Controllers" %>
<%@ Import Namespace="LeetRaids.Models" %>

<asp:Content ID="eventCalendarTitle" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore -  Event Calendar
</asp:Content>

<asp:Content ID="eventCalendarStylesOverride" ContentPlaceHolderID="StylesContent" runat="server">
	<style type="text/css">
	    #mainContent {width:805px; }
        #sideContent {width:160px; }
	</style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript" src="../../Content/Scripts/PageScripts/Events.js"></script>
    <script src="../../Content/Scripts/Validation.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="eventCalendarMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>The Event Calendar helps you keep track of what is going on.</h3>
    <p>
        This is an overview of all your events for a given month. To view a more in depth list of what is going on in a particular day click on a day in the calendar. 
        Administrative features are also accesed by selecting a day on the calendar.
    </p>
    
    <table id="eventCalendar" cellpadding="0" cellspacing="0">
    <thead>
        <tr>
            <th><%= Html.ActionLink("<<Prev", "DisplayMonth", new { monthNum = (Model.MonthNumber - 1), year = Model.Year })%></th>
            <th colspan="5"><%= Model.MonthName %> - <%= Model.Year %></th>
            <th><%= Html.ActionLink("Next>>", "DisplayMonth", new { monthNum = (Model.MonthNumber + 1), year = Model.Year })%></th>
        </tr>
        <tr>
            <th>Sunday </th>
            <th>Monday </th>
            <th>Tuesday </th>
            <th>Wednesday </th>
            <th>Thursday </th>
            <th>Friday </th>
            <th>Saturday </th>
        </tr>
    </thead>
    <%int count = 1, dayCount = 1; %>
    <%while(dayCount < (Model.DaysInMonth + 1))
      {%>
        <%= "<tr>" %>
        <%for (int c = 0; c < 7; c++)
          { %>
            <%if (count >= Model.calendarMonthStartIndex && count <= Model.calendarMonthEndIndex)
              {%>
                <%= "<td class='dayOfMonth' onclick='viewOverview($(this), \"" + String.Format("{0}-{1}-{2}", Model.MonthNumber, dayCount, Model.Year) + "\")'>"%>
                <%= "<span>" + dayCount + "</span>"%>
                <% Event[] daysEvents = Model.Events.Where(evt => evt.Date.Day == dayCount).ToArray(); %>
                <% int numEvents = daysEvents.Count(); %>
                <% int numEventsToDisplay = (numEvents <= 3) ? numEvents : 2; %>
                <% for (int eventNum = 0; eventNum < numEventsToDisplay; eventNum++)
                   {%>
                    <% if (daysEvents[eventNum] != null)
                       {%>
                        <%= "<p class='eventInfo'>" + daysEvents[eventNum].EventName.SubStrMax(30) + "</p>"%>
                    <%} %>
                <%} %>
                <% if (numEvents > 3) {%> <%= "<p class='hiddingEventInfo'>" + String.Format("Hiding {0} other events <br/ > Click to show", (numEvents - 3)) + "</p>" %> <%}  %>
                <%= "</td>"%>
                <% dayCount++; %>
            <%}
              else{ %>
                <%= "<td class='notDayOfMonth'></td>"%>
            <%} 
            count++; %>
        <%} %>
        <%= "</tr>" %>
    <%} %>
    </table>
    
    <a id="hdlnkFancyOverview" class="hdlnkDayModal" style="display:none" href="">Click me</a>
    
    <script type="text/javascript">
        $(".hdlnkDayModal").fancybox({
            "type": "iframe",
            'width': 900,
            'height': 600,
            'hideOnOverlayClick': false,
            'hideOnContentClick': false,
            'scrolling': 'no'
        });
        //<%= "<a class='hdlnkDayModal' href='/Events/EventsOverview?date=" + String.Format("{0}-{1}-{2}", dayCount, Model.MonthNumber, Model.Year) + "'>pewpew</a>"%>
        function viewOverview(td, date) {
            $("#hdlnkFancyOverview").attr("href", "/Events/EventsOverview?date=" + date);

            if (BrowserDetect.browser == BrowserTypes.IE) {
                document.getElementById('hdlnkFancyOverview').click(); //Dont Use Jquery click, its not working.
            }
            else if (BrowserDetect.browser == BrowserTypes.Firefox) {
                var theEvent = document.createEvent("MouseEvent");
                theEvent.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                var element = document.getElementById('hdlnkFancyOverview');
                element.dispatchEvent(theEvent);
            }
            else if (BrowserDetect.browser == BrowserTypes.Chrome) {
                var theEvent = document.createEvent("MouseEvent");
                theEvent.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                var element = document.getElementById('hdlnkFancyOverview');
                element.dispatchEvent(theEvent);
            }
            else {
                var theEvent = document.createEvent("MouseEvent");
                theEvent.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                var element = document.getElementById('hdlnkFancyOverview');
                element.dispatchEvent(theEvent);
            }
            
            return false;
        }
      
        //$(".dayOfMonth").click(function() { alert($(this).length); $(this).children(".hdlnkDayModal").trigger("click"); return true; })
    </script> 
    
<%--    <table id="eventInfoTable" cellspacing="0" cellpadding="0">
    <tr>
        <td rowspan="3" class="thumb"><img src="../../Content/Images/DungeonThumbs/naxxthumb.jpg" /></td>
        <td><strong>Name:</strong></td>
        <td><strong>Date:</strong></td>
        <td class="button" style="vertical-align:bottom;"><img src="../../Content/Images/Buttons/btnEdit.png" /></td>
    </tr>
    <tr>
        <td><strong>Players:</strong></td>
        <td></td>
        <td class="button"><img src="../../Content/Images/Buttons/btnManage.png" /></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td class="button" style="vertical-align:top;"><img src="../../Content/Images/Buttons/btnDelete.png" /></td>
    </tr>
    </table>--%>
</asp:Content>

<asp:Content ID="eventCalendarSideContent" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
    <%--<img src="../../Content/Images/SampleAds/Vertical_FreeRealms.png" />--%>
</asp:Content>
