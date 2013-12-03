<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AddGameModel>" %>

<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Controllers" %>
<%@ Import Namespace="LeetRaids.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    LFMore - AddCharacters
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript" src="../../Content/Scripts/PageScripts/ManageCharacters.js"></script>
    <script src="../../Content/Scripts/Validation.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function setAddCharButton(gameID) {
            $("#addCharacterLink").attr("href", "/Account/AddCharacter?gameID=" + gameID);

            $("#addCharacterLink").fancybox({
                'width': 650,
                'height': 400,
                'hideOnOverlayClick': false,
                'hideOnContentClick': false,
                "type": "iframe",
                "titleShow": false,
                'onClosed': function() { displayCharacters(gameID);  } //function(){alert('suck my dick');}
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Characters</h2>
    <%if (TempData["UserJustRegistered"] != null && TempData["UserJustRegistered"].ToString().ToLower() == "true")
      { %>
        <%= "Thank you for registering with LFMore," %>
    <%} %>
    <p>
        In order to use the features of LFMore you will need to create the characters you have for each game you play. You can then use those characters to create, join, and
        manage events.
    </p>
    <p>
        First, add the games you play using the list of Avalable games. We are constantly adding support for new and existing MMO games.
    </p>
    <p>Once you have added your games you can select that game and begin Adding, Editing, and Deleting your characters.</p>
    <form id="addGame" method="post" action="AddGame">
    <table id="tblAddGame" class="addGamesTable">
        <tr>
            <td class="selectGameLabel">
                <span class="largeText">Available Games:</span>
            </td>
            <td class="selectedGameDDL">
                <% Html.RenderAction("GameList", "Shared"); %>
            </td>
            <td>
                <a href="javascript:void(0);" onclick="addNewGame($('#games :selected').val(), $('#games :selected').text())" ><img alt="Add Game" src="../../Content/Images/Buttons/btnAddGame.png" /></a>
            </td>
        </tr>
    </table>
    
    <table id="selectGameTable" class="addGamesTable">
        <tr>
            <td class="selectGameLabel">
                <span class="largeText">Your Games:</span>
            </td>
            <td class="selectedGameDDL">
                <select id="ddlMemGames" class="largeDDL" onchange="selectGame($('#ddlMemGames :selected').val())"></select>
            </td>
            <td>
                <span id="addGameMsg"></span>
            </td>
        </tr>
    </table>
    <div id="gameContainer" style="display:none">
        <img alt="Select Game Logo" id="imgGameLogo" />
        <a id="addCharacterLink" href="/Account/AddCharacter?gameID=0"><img alt="Add a Character" src="../../Content/Images/Buttons/btnAddCharacter2.png" /></a>
        <div id="divCharacterListContainer">
            <p style="margin:5px 10px;">Your World of Warcraft Characters:</p>
            <table id="tblCharacterList" cellpadding='0' cellspacing='0'>
               
            </table>
        </div>
    </div>
    
    <div id="divMgmtCharNextSteps" style="display:none">
        <% if (!String.IsNullOrEmpty(Model.PrevActionURL) && !Model.PrevActionURL.Contains("ManageCharacters"))
           { %>
            <a href="<%= Model.PrevActionURL %>" class="nextStep">I'm ready to go to the page I was on  >></a>
            <p>-OR-</p>
        <%} %>
        <a href="/Events/" class="nextStep">Go To My Event Calendar  >></a>
    </div>
    
    
    </form>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
