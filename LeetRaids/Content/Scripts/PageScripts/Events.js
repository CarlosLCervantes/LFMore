function showDailyEventsLightbox(day, month, year) {
    $("#lightboxOverlay").show();

    var sb = new Array();
    sb.push("<h2>Daily Events</h2>");
    sb.push("<h3>November 10, 2009</h3>");
    sb.push("<div>");
    sb.push("To add a new event on this day click the 'Add Event' Button <a href='/Events/Add/'><img src='../../Content/Images/Buttons/btnAddGame.png' /></a>");
    //TODO: The date which is currently being opperated on needs to persist to the add event page. So that the date doesnt need to be re-selected.
    sb.push("</div>");
    sb.push("<table id='tblDaysEvents'>");
    sb.push("<thead><tr>");
    sb.push("<th>Name</th>");
    sb.push("<th>Type</th>");
    sb.push("<th>Players</th>");
    sb.push("<th>Organizer</th>");
    sb.push("</tr></thead>");
    sb.push("<tbody id='tbDaysEvents'>");
    sb.push("</tbody>");
    $.getJSON("/Events/GetMembersEventsByDayAJAX?day=" + day + "&month=" + month + "&year=" + year, populatePlayersEvents)
    sb.push("</table>");
    sb.push("<table id='eventInfoTable' cellspacing='0' cellpadding='0'>");
    sb.push("</table>");
    sb.push("<a href='javascript:void(0);' onclick='$(\"#lightboxOverlay\").hide()'>close</a>");

    $("#lightboxContainer").empty();
    $("#lightboxContainer").append(sb.join(""));
}

function populatePlayersEvents(events) {
    var sb = new Array();
    for (var i = 0; i < events.length; i++) {
        sb.push("<tr>");
        sb.push("<td style='width:200px'>" + events[i].EventName + "</td>");
        sb.push("<td style='width:150px'>" + events[i].EventTypeName + "</td>");
        sb.push("<td style='width:50px'>" + events[i].Players + "</td>");
        sb.push("<td style='width:100px'>Sindrome</td>");
        if (true) {
            sb.push("<td style='width:50px'><a href='/Events/Manage/" + events[i].EventID + "'>Manage</a></td>");
        }
        sb.push("</tr>");
    }

    $("#tbDaysEvents").empty();
    $("#tbDaysEvents").append(sb.join(''));

}