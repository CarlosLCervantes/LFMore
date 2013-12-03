function gameSelected(ddlGames) {
    var gameID = ddlGames.find("option:selected").val();
    if (gameID != -1) {
        popServers(gameID);
        popEventGroups(gameID);
        $("#ddlEventGroups").removeAttr("disabled");
    }
    else {
        $("#ddlServers").html("<option>No Game Selected</option>");
        $("#ddlServers").attr("disabled", "disabled");
        $("#ddlEventGroups").html("<option>No Game Selected</option>");
        $("#ddlEventGroups").attr("disabled", "disabled");
        $("#ddlEventSubGroups").html("<option>No Event Group Selected</option>");
        $("#ddlEventSubGroups").attr("disabled", "disabled");
        $("#ddlEventTypes").html("<option>No Event Sub Group Selected</option>");
        $("#ddlEventTypes").attr("disabled", "disabled");
    }


}

function popServers(gameID, serverID) {
    var ddlServers = $("#ddlServers");
    ddlServers.empty();
    $.getJSON("/Game/GetAllServersByGameIDAJAX/" + gameID, function(servers) {
        ddlServers.removeAttr("disabled");
        var sb = new Array();
        sb.push("<option>Select a Server</option>");
        for (var i = 0; i < servers.length; i++) {
            if (serverID && serverID.length > 0 && serverID == servers[i].ServerID) {
                sb.push("<option selected value='" + servers[i].ServerID + "'>" + servers[i].Name + "</option>");
            }
            else {
                sb.push("<option value='" + servers[i].ServerID + "'>" + servers[i].Name + "</option>");
            }
        }
        ddlServers.append(sb.join(''));
    });
}

function popEventGroups(gameID) {
    var ddlEventGroups = $("#ddlEventGroups");
    //ddlEventGroups.empty();
    $.getJSON("/Events/GetAllEventGroupsAJAX?gameID=" + gameID, function(eventGroups) {
        var sb = new Array();
        sb.push("<option value='-1'>Select a Group</option>");
        for (var i = 0; i < eventGroups.length; i++) {
            sb.push("<option value='" + eventGroups[i].EventGroupID + "'>" + eventGroups[i].Name + "</option>");
        }
        ddlEventGroups.html(sb.join(""));
    });
}

function popEventSubGroups(ddlEventGroups) {
    var groupID = ddlEventGroups.find("option:selected").val();
    if (groupID != -1) {
        var ddlEventSubGroups = $("#ddlEventSubGroups");
        ddlEventSubGroups.removeAttr("disabled");
        //ddlEventSubGroups.empty();
        $.getJSON("/Events/GetAllEventSubGroupsAJAX?groupID=" + groupID, function(eventSubGroups) {
            var sb = new Array();
            sb.push("<option value='-1'>Select a Sub Group</option>");
            for (var i = 0; i < eventSubGroups.length; i++) {
                sb.push("<option value='" + eventSubGroups[i].EventGroupSubTypeID + "'>" + eventSubGroups[i].Name + "</option>");
            }
            ddlEventSubGroups.html(sb.join(""));
        });
    }
}

function popEventTypes(ddlEventSubGroups) {
    var eventGroupSubTypeID = ddlEventSubGroups.find("option:selected").val();
    if (eventGroupSubTypeID != -1) {
        var ddlEventTypes = $("#ddlEventTypes");
        ddlEventTypes.removeAttr("disabled");
        //ddlEventTypes.empty();
        $.getJSON("/Events/GetAllEventTypesBySubGrpIDAJAX?subGrpID=" + eventGroupSubTypeID, function(eventTypes) {
            var sb = new Array();
            sb.push("<option value='-1'>Select an Event Type</option>");
            for (var i = 0; i < eventTypes.length; i++) {
                sb.push("<option value='" + eventTypes[i].EventTypeID + "'>" + eventTypes[i].EventTypeName + "</option>");
            }
            ddlEventTypes.html(sb.join(""));
        });
    }
    
}