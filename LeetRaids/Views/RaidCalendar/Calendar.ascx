<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<RaidCalendar>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Controllers" %>
<%@ Import Namespace="LeetRaids.Models" %>

<table id="eventCalendar" cellpadding="0" cellspacing="0">
    <thead>
        <tr>
            <th><%= Html.ActionLink("<<Prev", "DisplayCalendarMonth", new { monthNum = (Model.MonthNumber - 1) })%></th>
            <th colspan="5"><%= Model.MonthName %></th>
            <th><%= Html.ActionLink("Next>>", "DisplayCalendarMonth", new { monthNum = (Model.MonthNumber + 1) })%></th>
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
    <%for (int r = 0; r < 5; r++)
      {%>
        <%= "<tr>" %>
        <%for (int c = 0; c < 7; c++)
          { %>
            <%if (count >= Model.calendarMonthStartIndex && count <= Model.calendarMonthEndIndex)
              {%>
                <%= "<td class='dayOfMonth'>" + dayCount + "</td>"%>
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
