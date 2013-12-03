var eventTypes;
var selectGameID;
var selectedChar;
var roles;
var defaultEventDate;

var dialogeActive;
var characterDialogActive;

function bindEventTypes(gameID) {
    $('#btnAddEvent').hide();
    selectGameID = gameID;
    if (gameID != -1) {
        $.getJSON("/Events/GetAllEventGroupsWithSubGroups?gameID=" + gameID, displayGroupTypes);
    }
    else {
        alert('no game selected');
    }
    loadRoles(gameID);
}

function displayGroupTypes(eventGroupTypes){
    var sb = new Array();

    sb.push("<p>Select the type of Event to create</p>");
    var estHeight = 0;
    for (var group = 0; group < eventGroupTypes.length; group++) {
        var eventGroupBoxID = "eventGroupBox" + eventGroupTypes[group].EventGroup.Name;
        sb.push("<div id='" + eventGroupBoxID + "' class='eventGroupBox'>");
        sb.push("<p>" + eventGroupTypes[group].EventGroup.Name + "</p>");
        sb.push("<ul class='eventSubGroupBox'>");
        var grpEstHeight = 0;
        for (var subGrp = 0; subGrp < eventGroupTypes[group].SubGroups.length; subGrp++) {
            sb.push("<a href='javascript:void(0);' onclick='selectSubGroup(" + eventGroupTypes[group].SubGroups[subGrp].EventGroupSubTypeID + ", \"" + eventGroupTypes[group].SubGroups[subGrp].Name + "\")'><li>" + eventGroupTypes[group].SubGroups[subGrp].Name + "</li></a>");
            grpEstHeight += 10;
            if (grpEstHeight > estHeight) {
                estHeight = grpEstHeight;
            }
        }
        sb.push("</ul>");
        sb.push("</div>");
    }

    sb.push();
    var output = sb.join("");
    $("#eventSelection").empty();
    //output = output.replace("[[height]]", (estHeight + 100) + "px")
    $("#eventSelection").append(output);
}

function selectSubGroup(subGrpID, subGrpName) {
    $.getJSON("/Events/GetAllEventTypesBySubGrpIDAJAX?subGrpID=" + subGrpID, function(cbEventTypes) {
        eventTypes = cbEventTypes;
        $("#eventSelection").css("height", "330px");
        var sb = new Array();
        sb.push("<p>Select the type of Event to create</p>");
        sb.push("<p class='breadCrumb'><a href='javascript:void(0);' onclick='bindEventTypes(" + selectGameID + ")'>Events</a> >> " + subGrpName + "</p>");
        sb.push("<div id='divEventTypes' class='eventTypeBox'>");
        sb.push("<select size='10' id='lbEventTypes' name='eventTypeID' onchange='showEventTypeDetails($(\"#lbEventTypes\"))'>");
        for (var i = 0; i < cbEventTypes.length; i++) {

            sb.push("<option value='" + cbEventTypes[i].EventTypeID + "' >" + cbEventTypes[i].EventTypeName + "</option>");

        }
        sb.push("</select>");
        sb.push("</div>");
        sb.push("<div id='divDetails' class='eventTypeBox'> </div>");
        sb.push("<div id='divImage' class='eventTypeBox'> </div>");
        sb.push("<br class='clear' />");
        sb.push("<a href='javascript:void(0);'><img id='imgEventSelectionConfirm' style='display:none;' onclick='selectEvent($(\"#lbEventTypes\").val(), $(\"#lbEventTypes option:selected\").text()," + subGrpID + ",\"" + subGrpName + "\")' src='../../Content/Images/Buttons/btnSelect.png' /></a>");
        $("#eventSelection").empty();
        $("#eventSelection").append(sb.join(""));
    });
}

function showEventTypeDetails(ddl) {
    var index = ddl[0].selectedIndex;

    var sbDetails = new Array();
    sbDetails.push("<p class='header'>Details</p>");
    sbDetails.push("<p class='subheader'>Players: " + eventTypes[index].PlayerCount + "</p>");
    sbDetails.push("<p class='subheader'>Location: " + eventTypes[index].Location + "</p>");
    if (eventTypes[index].SuggestedLvlRange && eventTypes[index].SuggestedLvlRange != "")
        sbDetails.push("<p class='subheader'>Suggested Lvl: " + eventTypes[index].SuggestedLvlRange + "</p>");
    else
        sbDetails.push("<p class='subheader'>Suggested Lvl: N/A </p>");
    sbDetails.push("<p class='header'>More Info:</p>");
    if (eventTypes[index].AdditionalInfoLink && eventTypes[index].AdditionalInfoLink != "")
        sbDetails.push("<a href='" + eventTypes[index].AdditionalInfoLink + "'>More Info Here</a>");
    $('#divDetails').empty();
    $('#divDetails').append(sbDetails.join(""))

    var sbImage = new Array();
    sbImage.push("<img src='../../Content/Images/DungeonThumbs/naxxthumb.jpg' />");
    $('#divImage').empty();
    $('#divImage').append(sbImage.join(""));

    $("#imgEventSelectionConfirm").css("display", "");
}

function selectEvent(eventTypeID, eventTypeName, subGrpID, subGrpName) {
    $("#btnAddEvent").show();
    $("#eventSelection").css("height", "330px");

    $("#hdEventTypeID").val(eventTypeID);

    var sb = new Array();
    sb.push("<p>Select the type of Event to create</p>");
    sb.push("<p class='breadCrumb'><a href='javascript:void(0);' onclick='bindEventTypes(" + selectGameID + ")'>Events</a> >> ");
    sb.push("<a href='javascript:void(0);' onclick='selectSubGroup(" + subGrpID + ",\"" + subGrpName + "\")'>" + subGrpName + "</a> >> ");
    sb.push(eventTypeName + "</p>");
    
    sb.push("<div id='divEventFormLeft'>");
    sb.push("<img src='../../Content/Images/DungeonThumbs/naxxthumb.jpg' />");
    sb.push("<br />");
    //sb.push("<a href='javascript:void(0);'><img id='imgEventSelectionConfirm' onclick='confirmEvent(" + eventTypeID + ")' src='../../Content/Images/Buttons/btnAddGame.png' /></a>");
    sb.push("</div>");

    sb.push("<table id='tblEventInfo'>");
    sb.push("<tr>");
    sb.push("<td>*Name:</td>");
    sb.push("<td><input type='text' id='txtEventName' name='EventName' maxlength='25' /><em class='sideNote'>(25 char Max)</em></td>");
    sb.push("</tr>");
    sb.push("<tr>");
    sb.push("<td>*Date:</td>");
    sb.push("<td><input type='text' id='txtDate' name='Date' value='" + defaultEventDate + "'/></td>");
    sb.push("</tr>");
    sb.push("<tr>");
    sb.push("<td>*Time:</td>");
    sb.push("<td>");
    sb.push("<select id='ddlHour'>");
    for (var i = 1; i <= 12; i++) {
        sb.push("<option value='" + i + "'>" + i + "</option>");
    }
    sb.push("</select>");
    sb.push("<select id='ddlMinute'>");
    sb.push("<option value='0'>00</option>");
    sb.push("<option value='15'>15</option>");
    sb.push("<option value='30'>30</option>");
    sb.push("<option value='45'>45</option>");
    sb.push("</select>");
    sb.push("<select id='ddlMeridian'>");
    sb.push("<option value='AM'>AM</option>");
    sb.push("<option value='PM'>PM</option>");
    sb.push("</select>");
    sb.push("</td>");
    sb.push("</tr>");

    sb.push("<tr>");
    sb.push("<td>*Character:</td>");
    sb.push("<td><span id='newEventCharacter'></span>");
    sb.push("<a href='javascript:void(0);' onclick='displayCharacters()'>Select</a></td>");
    sb.push("</tr>");

    sb.push("<tr id='rowRoles'>");
    sb.push("<td>*Role:</td><td id='tdRoles'>");
    sb.push("Select a character first.")
    //for (var i = 0; i < roles.length; i++) {
    //    sb.push(roles[i].RoleName + "<input name='RoleID' type='checkbox' value='" + roles[i].RoleID + "' />|");
    //}
    sb.push("</td></tr>");
    
    
    sb.push("<tr>");
    sb.push("<td>*Meetup at:</td>");
    sb.push("<td><input type='text' id='txtMeetupLocation' name='MeetupLocation' maxlength='15' /><em class='sideNote'>(15 char Max)</em></td>");
    sb.push("</tr>");
    sb.push("<tr>");
    sb.push("<td>Notes:<em class='sideNote'><br />(50 char Max)</em></td>");
    sb.push("<td><textarea rows='7' cols='30' id='txtNotes' name='Note' value='asd' maxlength='50' /></td>");
    sb.push("</tr>");
    sb.push("<tr><td colspan='2'>* denotes a required field</td><tr>");
    sb.push("</table>");

    $("#eventSelection").empty();
    $("#eventSelection").append(sb.join(""));

    $("#txtDate").datepicker();

}

function geteventInfo(evtID) {
    $.getJSON("/Events/GetEditEventInfoAJAX?evtID=" + evtID, popEventInfo);
}

function popEventInfo(evt) {
    $("#txtEventName").val(evt[0].EventName);

    $("#txtDate").val(evt[0].Date);

    $("#ddlHour").val(evt[0].EventHour);
    $("#ddlMinute").val(evt[0].EventMinute);
    $("#ddlMeridian").val(evt[0].EventMeridian);

    var roleString = "";
    if (evt[2].length > 0) {
        roleString = evt[2][0].RoleID;
        for (var r = 1; r < evt[2].length; r++) {
            roleString += "|" + evt[2][r].RoleID;
        }
    }
    selectCharacter(evt[1].CharID, evt[1].CharacterName, evt[4].ImageLocation, roleString);

    $("input[name=RoleID][value=" + evt[3] + "]").attr("checked", "true");

    $("#txtMeetupLocation").val(evt[0].MeetupLocation);

    $("#txtNotes").val(evt[0].Notes);
}

function confirmEvent() {
    var valid = false;
    //FORCE LENGTHS FOR TEXT FIELDS
    var errors = new Array();

    if (!isParagraph($("#txtEventName").val())) {
        errors.push("Please check the Event Name");
    }
    if (!isDate($("#txtDate").val())) {
        errors.push("Please check the date (format: mm/yy/dd)");
    }
    if (!isParagraph($("#txtMeetupLocation").val())) {
        errors.push("Please check meetup location");
    }
    if (!selectedChar || selectedChar == "") {
        errors.push("Please select a character");
    }

    if (errors.length > 0) {
        var sb = new Array();
        sb.push("<p>We found the following issues with your new Event Submission</p>");
        sb.push("<ul>");
        for (var i = 0; i < errors.length; i++) {
            sb.push("<li>" + errors[i] + "</li>");
        }
        sb.push("</ul>");

        $("#errors").empty();
        $("#errors").append(sb.join(""));
        $("#errors").show();

        if (dialogeActive) {
            $("#errors").dialog('open');
        }
        else {
            $("#errors").dialog({
                bgiframe: true,
                modal: false,
                buttons: {
                    Ok: function() {
                        $(this).dialog('close');
                        //$(this).dialog('hide');
                        //$(this).hide();
                    }
                }
            });
            dialogeActive = true;
        }

    }
    else { valid = true; }

    return valid;
}

function addEvent(eventTypeID) {
    //Activate Class/Role restrictions
    var requestString = "name=" + $("#txtEventName").val();
    requestString += "&date=" + $("#txtDate").val();
    //var time = convertMeridianTimeTo24Hour($("#ddlHour").val(), $("#ddlMinute").val(), $("#ddlMeridian").val());
    //requestString += "&time=" + time;
    requestString += "&timeHour=" + $("#ddlHour").val();
    requestString += "&timeMin=" + $("#ddlMinute").val();
    requestString += "&timeMeridian=" + $("#ddlMeridian").val();
    requestString += "&typeID=" + eventTypeID;
    requestString += "&charID=" + selectedChar;
    requestString += "&roleID=" + "1"; //$("#RoleID:checked").val();
    requestString += "&meetupLocation=" + $("#txtMeetupLocation").val();
    requestString += "&notes=" + $("#txtNotes").val(); //Test for capacity
    requestString += "&inHealRestriction=" + $("#txtHealerRestriction").val();
    requestString += "&inTankRestriction=" + $("#txtTankRestriction").val();
    requestString += "&inDamageRestriction=" + $("#txtDPSRestriction").val(); 
    $.getJSON("/Events/AddNewEventAJAX?" + requestString, function(response) {
        if (response) {
            alert("Your event was successfully added");
            window.location = "/Events/";
        }
        else {
            //ummm shit got fucked up.
        }
    });
}

function convertMeridianTimeTo24Hour(hour, minute, meridian) {
    var time = hour + ":" + minute;
    if (meridian == "PM") {
        var hour24 = hour + 12;
        time = hour24 + ":" + minute;
    }
    else {
        if (hour.length < 2) {
            time = hour + ":" + minute;   
        }
    }

    return time;
}


function displayCharacters() {
    $.getJSON("/Account/GetAllMemberCharactersByGameAJAX/" + selectGameID, function(characters) {
        //Gota Add Server Support
        var sb = new Array();
        //Make Header
        if (characters.length > 0) {
            sb.push("<table id='tblCharacterList'>");
            sb.push("<thead><tr>");
            sb.push("<th>Name</th>");
            sb.push("<th>Class</th>");
            sb.push("<th>Server</th>");
            sb.push("<th>Faction</th>");
            sb.push("</tr></thead>");

            //Make Body
            sb.push("<tbody>");
            for (var i = 0; i < characters.length; i++) {
                sb.push("<tr>");
                sb.push("<td>" + characters[i].CharacterName + "</td>");
                sb.push("<td>" + characters[i].ClassName + "</td>");
                sb.push("<td>" + characters[i].ServerName + "</td>");
                sb.push("<td>" + characters[i].FactionName + "</td>");
                var roleString = "";
                if (characters[i].Roles.length > 0) {
                    roleString = characters[i].Roles[0].RoleID;
                    for (var r = 1; r < characters[i].Roles.length; r++) {
                        roleString += "|" + characters[i].Roles[r].RoleID;
                    }
                }
                sb.push("<td><a href='javascript:void(0);' onclick='selectCharacter(" + characters[i].CharacterID + ", \"" + characters[i].CharacterName + "\", \"" + characters[i].ImageLocation + "\",\"" + roleString + "\")'>Select</a></td>");
                sb.push("</tr>");
            }
            sb.push("</tbody>");
        }
        else {
            sb.push("<tr><td colspan='4'>You have no characters registerd for this game</td></tr>");
        }
        sb.push("</table>");
        sb.push("<br />");
        sb.push("<a href='javascript:void(0)' onclick='$(\"#lightboxOverlay\").hide()'>Close</a>");

        $("#lightboxContainer").empty();
        $("#lightboxOverlay").show();
        $("#lightboxContainer").append(sb.join(""));
    });
}

function selectCharacter(charID, charName, classImg, roleString) {
    selectedChar = charID;
    if ($("#hdChar").length > 0) {$("#hdChar").val(charID); }
    $("#newEventCharacter").empty();
    $("#newEventCharacter").append("<img src='../../Content/Images/ClassIcons/" + classImg + "' /> " + charName);
    $("#newEventCharacter").append("<input type='hidden' id='charID' name='charID' value='" + charID + "' />");

    //Make Roles That the Character Supports
    var roleIDs = (roleString + "").split("|");
    var sb = new Array();
    for (var i = 0; i < roleIDs.length; i++) {
        for (var r = 0; r < roles.length; r++) {
            if (("" + roles[r].RoleID) == roleIDs[i]) {
                sb.push(roles[r].RoleName + "<input name='RoleID' type='radio' value='" + roles[r].RoleID + "' />   ");
            }
        }
    }
    $("#tdRoles").html(sb.join(''));

    $("#lightboxOverlay").hide();
    $("#lightboxContainer").empty();
}


function loadRoles(gameID) {
    $.getJSON("/Game/GetAllRolesByGameID/" + gameID,
        function(rolesData) {
            roles = rolesData;
        }
    );
}

function clearEventRestrictions() {
    var roleboxes = $("#tblNewEventRoleRestrictions").find("input:text");
    roleboxes.val("");
}

