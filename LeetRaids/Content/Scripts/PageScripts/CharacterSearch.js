var dialogeActive;
var chars;
var enforceEventRestrictions;
function searchCharacters() {
    var qs = new Array();
    qs.push("charName=" + $("#CharacterName").val());
    qs.push("&classID=" + $("#ClassID :selected").val());
    qs.push("&roleID=" + $("#RoleID :selected").val());
    qs.push("&serverID=" + $("#ServerID :selected").val());
    qs.push("&minLvl=" + $("#LVL").val());
    qs.push("&maxLvl=" + $("#LVL").val());
    qs.push("&evtRest=" + ((enforceEventRestrictions) ? 1 : 0));
    
    $.getJSON("/Shared/SearchCharactersAJAX?" + qs.join(""), popCharacters);
}

function popCharacters(characters) {
    $("#tblCharSearch tbody").empty();
    var sb = new Array();
    chars = characters;
    for (var i = 0; i < characters.length; i++) {
        sb.push("<tr>");
        if (enforceEventRestrictions) {
            sb.push("<td><img class='evtRestrImg' src='../../../Content/Images/" + ((characters[i].Restriction.Restricted) ? 'crossmark_small.png' : 'checkmark_small.png') + "' />");
            //sb.push("<td>" + characters[i].Restriction + "</td>");
        }
        sb.push("<td><a href='javascript:void(0)'>" + characters[i].CharacterInfo.CharacterName + "</a></td>");
        sb.push("<td>" + characters[i].CharacterInfo.ClassName + "</td>");
        sb.push("<td>");
        for (var r = 0; r < characters[i].CharacterInfo.Roles.length; r++) {
            sb.push("<img style='length:20px;height:20px;' src='../../Content/Images/RoleIcons/" + characters[i].CharacterInfo.Roles[r].ImageLocation + "' />");  //Don't resize images, it takes too much time.
        }
        sb.push("</td>");
        sb.push("<td>" + characters[i].CharacterInfo.LVL + "</td>");
        //sb.push("<td>" + "GS HERE" + "</td>"); //GS needs to change color based on the value
//        sb.push("<td><a href='javascript:void(0)'>View</a></td>");
        if (characters[i].CharacterInfo.Roles.length > 1) {
            sb.push("<td>" + "<a href='javascript:void(0);' onclick='choosePlayerAttributes(" + characters[i].CharacterInfo.CharacterID + ", \"" + characters[i].CharacterInfo.CharacterName + "\", " + i + ", $(this))'>Add</a>" + "</td>");
        }
        else if (characters[i].CharacterInfo.Roles.length == 1) {
            sb.push("<td>" + "<a href='javascript:void(0);' onclick='addCharacterToEvent(" + characters[i].CharacterInfo.CharacterID + ", \"" + characters[i].CharacterInfo.CharacterName + "\", " + characters[i].CharacterInfo.Roles[0].RoleID + ", $(this))'>Add</a>" + "</td>");
        }
        
        sb.push("</tr>");
    }

    var msg = $("#message");
    if (characters.length >= 1) {
        $("#tblCharSearch").show();
        msg.text("Found " + characters.length + " characters.");
    }
    else {
        $("#tblCharSearch").hide();
        msg.text("No characters could be found that matched your criteria.");
    }

    $("#tblCharSearch tbody").append(sb.join(""));
}

function choosePlayerAttributes(charID, charName, index, sender)
{
    var sb = new Array();

    sb.push("<h3>Select a Role for " + charName + ":</h3>")
    for (var i = 0; i < chars[index].CharacterInfo.Roles.length; i++) {
        sb.push("<input type='radio' name='role' value='" + chars[index].CharacterInfo.Roles[i].RoleID + "' />");
        sb.push("<img style='length:20px;height:20px;' src='../../Content/Images/RoleIcons/" + chars[index].CharacterInfo.Roles[i].ImageLocation + "' />") //Resize
    }
    sb.push("<p id='errorMsg' class='error'></p>");
    $("#confirmAttributes").empty();
    $("#confirmAttributes").append(sb.join(""));
    
    if (dialogeActive) {
        $("#confirmAttributes").dialog('open');
    }
    else {
        $("#confirmAttributes").dialog({
            bgiframe: true,
            modal: false,
            buttons: {
                Ok: function() {
                if ($("input[@name='rdio']:checked").val() != null && $("input[@name='rdio']:checked").val() != "") {
                        addCharacterToEvent(charID, charName, $("input[@name='rdio']:checked").val(), sender)
                        $(this).dialog('close');
                    }
                    else {
                        $("#errorMsg").text("Please select a Role");
                    }
                },
                Cancel: function() {
                    $(this).dialog('close');
                }
            }
        });
        dialogeActive = true;
    }
}

function addCharacterToEvent(charID, charName, roleID, sender) {
    $.getJSON("/Events/AddCharacterToEventAJAX?charID=" + charID + "&roleID=" + roleID, function(response) {
        if (response.success) {
            alert(charName + " was successfully added to your event.");
            if (sender) {
                if (enforceEventRestrictions) {
                    sender.parents("tr").find(".evtRestrImg").attr("src", "../../../Content/Images/crossmark_small.png");
                }
            }
        }
        else {
            alert(charName + " could not be added to your event. " + response.errorMsg);
        }
    }); 
}

function clearFields() {
    $("#CharacterName").val("");
    $("#ClassID").attr("selectedIndex", 0);
    $("#RoleID").attr("selectedIndex", 0);
    $("#ServerID").attr("selectedIndex", 0);
    $("#LVL").val("");

}