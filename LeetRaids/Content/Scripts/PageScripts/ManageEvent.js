var eventID;
var statusDialogActive;
var removeDialogActive;

function load(evtID) {
    eventID = evtID;
    populateRoster();
}

function populateRoster() {
    var sb = new Array();
    $.getJSON("/Events/GetRosterAJAX?eventID=" + eventID, function(roster) {
        if (roster.length > 0) {
            //alert(roster.length);
            for (var i = 0; i < roster.length; i++) {
                var cssClass = (roster[i].IsCreator) ? "creatorRow" : "";
                sb.push("<tr class='" + cssClass + "'>");
                sb.push("<td><image src='../../Content/Images/ClassIcons/" + roster[i].ImageLocation + "' /></td>"); //Class
                sb.push("<td>" + roster[i].CharacterName + "</td>"); //Name
                sb.push("<td>" + roster[i].Status + "</td>"); //Status
                sb.push("<td>" + roster[i].RoleName + "</td>"); //Role
                sb.push("<td>" + roster[i].LVL + "</td>"); //Lvl
                sb.push("<td><a title'" + roster[i].Note + "'>Note</a></td>"); //Note
                sb.push("<td><input type='checkbox' name='RosterChar' value='" + roster[i].CharacterID + "' /></td>");
                sb.push("</tr>");
            }
        }
        else {
            sb.push("<tr><td>There are currently no players signed up for this event.</td></tr>");
        }

        $("#rosterTableBody").empty();
        $("#rosterTableBody").append(sb.join(""));
    });
}

function showUpdateStatus() {
    var statusDialog = $("#updateAttnStatusDialog");
    if (statusDialogActive) {
        statusDialog.dialog('open');
    }
    else {
        statusDialog.dialog({
            bgiframe: true,
            resizable: false,
            width: 350,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Update': function() {
                    var statusID = statusDialog.find("option:selected").val();
                    if (statusID != -1) {
                        updateAttendeeStatus(statusID);
                        $("#updateStatusMsg").hide();
                        $(this).dialog('close');
                    }
                    else {
                        $("#updateStatusMsg").show();
                    }
                },
                Cancel: function() {
                    $(this).dialog('close');
                    $("#updateStatusMsg").hide();
                }
            }
        });
        statusDialogActive = true;
    }
}

function updateAttendeeStatus(statusID) {
    var chars = getSelectedCharsAsDelimitedString();

    if (chars && chars.length > 0) {
        $.getJSON("/Events/UpdateAttendeeStatusAsCreatorAJAX?statusID=" + statusID + "&chars=" + chars, function(response) {
            if (response.success) {
                alert("Status successfully changed");
                populateRoster();
            }
            else {

            }
        });
    }
    else {
        alert("You didn't select any characters to update. None where updated.");
    }
}

function showRemoveAttn() {
    var statusDialog = $("#removeAttnDialog");
    if (removeDialogActive) {
        statusDialog.dialog('open');
    }
    else {
        statusDialog.dialog({
            bgiframe: true,
            resizable: false,
            width: 350,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Remove': function() {
                    removeAttn();
                    $(this).dialog('close');
                },
                Cancel: function() {
                    $(this).dialog('close');
                }
            }
        });
        removeDialogActive = true;
    }
}

function removeAttn() {
    var chars = getSelectedCharsAsDelimitedString();

    if (chars && chars.length > 0) {
        $.getJSON("/Events/RemoveAttendeesAsCreator?chars=" + chars, function(response) {
            if (response.success) {
                alert("Attendees successfully removed");
                populateRoster();
            }
            else {

            }
        });
    }
    else {
        alert("You didn't select any characters to remove. None where updated.");
    }
}

function getSelectedCharsAsDelimitedString() {
    var selectedAttendeesFields = $("[name='RosterChar']:checked");

    if (selectedAttendeesFields && selectedAttendeesFields.length > 0) {
        var chars = selectedAttendeesFields[0].value; //Auto pop the first value
        for (var i = 1; i < selectedAttendeesFields.length; i++) {
            chars += "|" + selectedAttendeesFields[i].value;
        }
        return chars;
    }
    else {
        //alert("You didn't select any characters to update. None where updated.");
        return "";
    }
}