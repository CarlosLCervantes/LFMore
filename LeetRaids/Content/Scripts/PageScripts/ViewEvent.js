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
                sb.push("<tr>");
                sb.push("<td><image src='../../Content/Images/ClassIcons/" + roster[i].ImageLocation + "' /></td>"); //Class
                sb.push("<td>" + roster[i].CharacterName + "</td>"); //Name
                sb.push("<td>" + roster[i].Status + "</td>"); //Status
                sb.push("<td>" + roster[i].RoleName + "</td>"); //Role
                sb.push("<td>" + roster[i].LVL + "</td>"); //Lvl
                sb.push("<td><a title'" + roster[i].Note + "'>Note</a></td>"); //Note
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


//var statusDialogActive;

//function showUpdateStatus(eventID, charID) {
//    var statusDialog = $("#updateStatusDialog");
//    if (statusDialogActive) {
//        statusDialog.dialog('open');
//    }
//    else {
//        statusDialog.dialog({
//            bgiframe: true,
//            resizable: false,
//            width: 600,
//            //height: 140,
//            modal: true,
//            overlay: {
//                backgroundColor: '#000',
//                opacity: 0.5
//            },
//            buttons: {
//                'Update': function() {
//                    updateAttendeeStatus(eventID, charID, statusDialog.find("option:selected").val());
//                    $(this).dialog('close');
//                },
//                Cancel: function() {
//                    $(this).dialog('close');
//                }
//            }
//        });
//        statusDialogActive = true;
//    }
//}

//function updateAttendeeStatus(eventID, charID, statusID) {
//    $.getJSON("/Events/UpdateAttendeeStatusAJAX?eventID=" + eventID + "&charID=" + charID + "&statusID=" + statusID, function(response) {
//        if (response.success) {
//            alert("Status successfully changed");
//        }
//        else {

//        }
//    });
//}


function updateDisplayAfterLeavingEvent(aTarget) {
    window.location = window.location;
}