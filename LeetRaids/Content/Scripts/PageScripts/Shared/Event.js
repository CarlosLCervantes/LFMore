var leaveEventDialogActive;

/*Leave Event Functionality*/
//Requires a modal div setup on the page.
//Requires a definition for the updateDisplayAfterLeavingEvent function on the page
function initializeLeaveEvent(aTarget, evtID, evtName) {
    $("#leaveEventDialog").find('.dialogEventDetails').html(evtName);
    if (leaveEventDialogActive) {
        $("#leaveEventDialog").dialog('open');
    }
    else {
        $("#leaveEventDialog").dialog({
            bgiframe: true,
            resizable: false,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Leave this event?': function() {
                    leaveEvent(aTarget, evtID);
                    $(this).dialog('close');
                },
                Cancel: function() {
                    $(this).dialog('close');
                }
            }
        });
        leaveEventDialogActive = true;
    }
}

function leaveEvent(aTarget, evtID) {
    $.getJSON("/Events/LeaveEventAJAX?eventID=" + evtID, function(response) {
        if (response.success) {
            updateDisplayAfterLeavingEvent(aTarget);
        }
        else {
            //message
        }
    });
}
