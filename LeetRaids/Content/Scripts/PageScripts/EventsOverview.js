var deactivateDialogActive;

function initializeDeactivate(aTarget, evtID, evtName) {
    $("#deactivateEventDialog").find('.dialogEventDetails').html(evtName);
    if (deactivateDialogActive) {
        $("#deactivateEventDialog").dialog('open');
    }
    else {
        $("#deactivateEventDialog").dialog({
            bgiframe: true,
            resizable: false,
            width: 600,
            //height: 140,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Deactivate this event?': function() {
                    deactivate(aTarget, evtID);
                },
                Cancel: function() {
                    $(this).dialog('close');
                }
            }
        });
        deactivateDialogActive = true;
    }
}

function deactivate(aTarget, evtID) {
    var tblTarget = aTarget.parents('table');
    $.getJSON("/Events/DeactivateEventAJAX/" + evtID, function(response) {
        if (response.success) {
            tblTarget.attr("class", "eventInfoTable inactive");
            tblTarget.find("[class='button edit']").empty();
            tblTarget.find("[class='button delete']").empty();
        }
        else {
            //message
        }
    });
}

function updateDisplayAfterLeavingEvent(aTarget) {
//    var tblTarget = aTarget.parents('table');
//    tblTarget.attr("class", "eventInfoTable inactive");
//    tblTarget.find("[class='button edit']").empty();
    //    tblTarget.find("[class='button delete']").empty();
    window.location = window.location;
}