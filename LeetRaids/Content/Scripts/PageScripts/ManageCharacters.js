window.onload = load;

/*================Global Variables==============*/
var classes;
var roles;
var factions;
var servers;
var curGameID;

function load() {
    //alert("hi");
    ddlMemGames = $("#ddlMemGames");
    displayMemberGames();
}

function displayMemberGames() {
    $.getJSON("/Account/GetMembersGames", cbDisplayMemberGames);
}

function cbDisplayMemberGames(games) {
    //alert(games.length);
    //alert(games);
    ddlMemGames.empty();
    var sb = new Array();
    sb.push("<option value='-1'>Select A Game</option>");
    if (games && games.length > 0) {
        for (var i = 0; i < games.length; i++) {
            sb.push("<option value='" + games[i].GameID + "'>");
            sb.push(games[i].GameName);
            sb.push("</option>");
        }
    }
    else {
        sb.push("<option value='-1'>No Games Added</option>");
    }
    
    ddlMemGames.html(sb.join(""));
}

function addNewGame(gameID, gameName) {
    $.getJSON("/Account/InsertNewMemberGameAJAX/" + gameID, function(response) {
        var msg = $("#addGameMsg");
        if (response.success) {
            msg.text(gameName + " was successfully added. Select the game from the drop down to the left to add characters.");
            displayMemberGames();
        }
        else {
            //msg.text("The game \"" + gameName + "\" couldn't be added to your account.");
        }
    });
}

function selectGame(gameID) {
    if (gameID != -1 && gameID.length > 0) {
        curGameID = gameID;
        $.getJSON("/Game/GetGameDetails/" + gameID, displayGame);
        //loadClasses(gameID);
        //loadRoles(gameID);
        //loadFactions(gameID);
        //loadServers(gameID);
        setAddCharButton(gameID); //exists on the page level
        $("#gameContainer").show();
        displayCharacters(gameID);
    }
    
}

function displayGame(gameInfo)
{
    $("#imgGameLogo").attr("src", "../../Content/Images/Games/" + gameInfo.ImageName);
}

function addCharacter(gameID) {
    //Load up Form Vars
    var charName = $("#txtName").val();
    var classID = $("#ddlClasses :selected").val();
    var factionID = $("#ddlFactionID :selected").val();
    var level = $("#txtLevel").val();

    $.getJSON("/Account/InsertNewCharacterAJAX/?gameID=" + gameID + "&charName=" + charName + "&classID=" + classID + "&factionID=" + factionID + "&level=" + level,
        function(success) {
            if (success) {
                $("#lightboxOverlay").hide();
                displayCharacters(gameID);
            }
            else {
                //alert("shit got fucked up");
            }
        }
    );
}

function delChar(charID) {
    $.getJSON("/Account/DeleteCharacterAJAX?charID=" + charID, function(response) {
        if (response.success) {
            
            displayCharacters(curGameID);
        }
        else {
            alert(response.errorMsg);
        }
    });
}

function displayCharacters(gameID) {
    $.getJSON("/Account/GetAllMemberCharactersByGameAJAX/" + gameID, cbDisplayCharacters);
}

function cbDisplayCharacters(characters) {
    $("#divMgmtCharNextSteps").show();
    //Gota Add Server Support
    var sb = new Array();
    //Make Header
    if (characters.length > 0) {
        sb.push("<thead><tr>");
        sb.push("<th>Name</th>");
        sb.push("<th>Class</th>");
        sb.push("<th>Server</th>");
        sb.push("<th>Faction</th>");
        sb.push("<th></th>");
        sb.push("<th></th>");
        sb.push("</tr></thead>");

        //Make Body
        sb.push("<tbody>");
        for (var i = 0; i < characters.length; i++) {
            var rowClassName = ((i % 2) == 0) ? "charListRow" : "charListRow charListRowAlt";
            sb.push("<tr class='" + rowClassName + "'>");
            sb.push("<td>" + characters[i].CharacterName + "</td>");
            sb.push("<td><img src='../../Content/Images/ClassIcons/" + characters[i].ImageLocation + "' /></td>");
            sb.push("<td>" + characters[i].ServerName + "</td>");
            sb.push("<td>" + characters[i].FactionName + "</td>");
            sb.push("<td><a class='lnkEditChar altLink' href='/Account/EditCharacter/" + characters[i].CharacterID + "'>Edit</a></td>");
            sb.push("<td><a class='altLink' href='javascript:void(0);' onclick='delChar(" + characters[i].CharacterID + ")' >Delete</a></td>");
            sb.push("</tr>");
        }
        sb.push("</tbody>");
    }
    else {
        sb.push("<tr><td colspan='4'>You have no characters registerd for this game</td></tr>");
    }

    $("#tblCharacterList").empty();
    $("#tblCharacterList").append(sb.join(""));

    $(".lnkEditChar").fancybox({
        'type': 'iframe',
        'width': 650,
        'height': 400,
        'hideOnOverlayClick': false,
        'hideOnContentClick': false
    });
}

//function editCharInfo(link) {
//    $("#hdlnkFancyOverview").attr("href", "/Events/EventsOverview?date=" + date);

//    if (BrowserDetect.browser == BrowserTypes.IE) {
//        document.getElementById('hdlnkFancyOverview').click(); //Dont Use Jquery click, its not working.
//    }
//    else if (BrowserDetect.browser == BrowserTypes.Firefox) {
//        var theEvent = document.createEvent("MouseEvent");
//        theEvent.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
//        var element = document.getElementById('hdlnkFancyOverview');
//        element.dispatchEvent(theEvent);
//    }
//    else if (BrowserDetect.browser == BrowserTypes.Chrome) {
//        var theEvent = document.createEvent("MouseEvent");
//        theEvent.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
//        var element = document.getElementById('hdlnkFancyOverview');
//        element.dispatchEvent(theEvent);
//    }
//    else {
//        var theEvent = document.createEvent("MouseEvent");
//        theEvent.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
//        var element = document.getElementById('hdlnkFancyOverview');
//        element.dispatchEvent(theEvent);
//    }

//    return false;
//}

//function loadClasses(gameID) {
//    $.getJSON("/Game/GetAllClassesByGameID/" + gameID,
//        function(classesData) {
//            classes = classesData;
//        }
//    );
//}

//function loadRoles(gameID){
//    $.getJSON("/Game/GetAllRolesByGameID/" + gameID,
//        function(rolesData) {
//            roles = rolesData;
//        }
//    );
//}
//    

//function loadFactions(gameID) {
//    $.getJSON("/Game/GetAllFactionsByGameID/" + gameID,
//        function(factionData) {
//            factions = factionData;
//        }
//    );
//}

//function loadServers(gameID) {
//    $.getJSON("/Game/GetAllServersByGameID/" + gameID,
//        function(serverData) {
//            servers = serverData;
//        }
//    );
//}


/*=====================================Modals================================*/
//function showAddCharacterLB(gameID) {
//    alert('this shoudlnt be happening');
//    $("#lightboxOverlay").show();

//    var sb = new Array();
//    sb.push("<form id='formAddChar'>");
//    sb.push("<h2>Add a Character</h2>");
//    sb.push("<table id='tblAddCharacter'>");
//    sb.push("<tr>");
//    sb.push("<td colspan='2'>Name: <input name='CharacterName' id='txtName' type='text' onblur=\"validate($(this), 'singleName');\" /></td>");
//    sb.push("<td>Level: <input name='LVL' id='txtLevel' type='text' onblur=\"validate($(this), 'number');\" /></td>");
//    sb.push("</tr>");
//    sb.push("<td>");
//    sb.push("Server: <select id='ddlServer' name='ServerID'>");
//    for (var i = 0; i < servers.length; i++) {
//        sb.push("<option value='" + servers[i].ServerID + "'>" + servers[i].Name + "</option>");
//    }
//    sb.push("</select></td>");
//    sb.push("<td>Class: <select name='ClassID' id='ddlClasses'>");
//    for (var i = 0; i < classes.length; i++) {
//        sb.push("<option value='" + classes[i].ClassID + "'>" + classes[i].ClassName + "</option>");
//    }
//    sb.push("</select></td>");
//    sb.push("<td>");
//    sb.push("Faction: <select name='FactionID' id='ddlFactionID'>");
//    for (var i = 0; i < factions.length; i++) {
//        sb.push("<option value='" + factions[i].FactionID + "'>" + factions[i].FactionName + "</option>");
//    }
//    sb.push("</select></td>");
//    sb.push("</tr>");
//    sb.push("<tr id='rowRoles'>");
//    for (var i = 0; i < roles.length; i++) {
//        sb.push("<td>" + roles[i].RoleName + "<input name='RoleID' type='checkbox' value='" + roles[i].RoleID + "' /></td>");
//    }
//    sb.push("</tr>");
//    sb.push("</table>");
//    sb.push("<div id='modalAction'>");
//    sb.push("<a href='javascript:void(0)' onclick='addCharacter(" + gameID + ")'><img style='display:block;' src='../../Content/Images/Buttons/btnAddCharacter2.png' /></a>");
//    sb.push("<a href='javascript:void(0)' onclick='$(\"#lightboxOverlay\").hide()'>or cancel</a>");
//    sb.push("<p class='error' style='display:none;'></p>");
//    sb.push("</div>");
//    sb.push("<input name='GameID' type='hidden' value='" + gameID + "' />");
//    sb.push("</form>");
//    
//    $("#lightboxContainer").empty();
//    $("#lightboxContainer").append(sb.join(""));
//}