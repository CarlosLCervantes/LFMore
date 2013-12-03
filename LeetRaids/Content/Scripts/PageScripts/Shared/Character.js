function displayCharacters(gameID) {
    $.getJSON("/Account/GetAllMemberCharactersByGameAJAX/" + gameID, function(characters) {
        var sb = new Array();
        if (characters.length > 0) {
            sb.push("<table id='tblCharacterList'>");
            sb.push("<thead><tr>");
            sb.push("<th>Name</th>");
            sb.push("<th>Class</th>");
            sb.push("<th>Server</th>");
            sb.push("<th>Faction</th>");
            sb.push("</tr></thead>");
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