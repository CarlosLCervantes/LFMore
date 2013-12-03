<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<InviteModel>" %>
<%@ Import Namespace="LeetRaids.Models" %>
<%@ Import Namespace="DataAccessLayer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - Invite Players
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="../../Content/Scripts/PageScripts/CharacterSearch.js" type="text/javascript"></script>
    <script type="text/javascript">
        function submitFriendInvite() {
            $("#frmFriendInvite").submit();
            //document.forms[1].submit();
        }

        function submitEmailInvite() {
            var e = $("#Email").val();
            $.getJSON("/Events/InviteViaEmailAJAX?e=" + e, function(response) {
                if (response.success) {
                    alert("An Email was send to your friend.");
                }
                else {
                    alert("There was an issue sending the email.");
                }
            })
            
            //alert($("#frmInviteByEmail").attr("name"))
        }
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">

    <h2>Invite Players</h2>
    <p>
        You can’t have a group without members. We offer a few different ways to invite people to your group. Each of the panels below provides you 
        with a different method for inviting players to your group. Players may also search your for your event and sign themselves up without you having to worry.
        Be sure to visit the Manage page often.
    </p>
    
    <%= Html.ActionLink("<< Return to Event Management", "Manage", new { id = Model.EventInfo.EventID }, new { @class = "backLink" })%>
    
    <%= ViewData.Eval("FriendAddSuccessMsg") %>
    <%= Html.ValidationSummary() %>
    
    <div class="colapseContainer">
        <table class="colapseSectionHeader">
            <tr>
                <td><h4>Invite a friend by E-mail</h4></td>
                <td class="colapseShowHide"><a href='javascript:void(0)' onclick="showHide($('#divInviteByEmail'), this)">Hide/Show</a></td>
            </tr>
        </table>
        <div id="divInviteByEmail" class="eventMgMtContentContainer">
            <p>Have a friend who isn't on LFMore yet? Or maybe you just want to get in touch the old fashoined way. 
               Well, enter the email of the person you would like to inivte and we will send them an E-mail with everything 
               they need to get on board.</p>
                <table id="tblInviteByEmail">
                    <tr>
                        <td>Email: </td>
                        <td><%= Html.TextBox("Email", null)  %> </td>
                        <%--<td><input type="submit" value="Send" style="height:30px; width:100px;" /></td>--%>
                        <td><a href="javascript:void(0)" onclick="submitEmailInvite()"><img alt="Send Invite" src="../../Content/Images/Buttons/btnSend.png" /></a></td>
                    </tr>
                </table>
        </div>
    </div>
    
    <div class="colapseContainer">
        <table class="colapseSectionHeader">
            <tr>
                <td><h4>Select from Friends List</h4></td>
                <td class="colapseShowHide"><a href='javascript:void(0)' onclick="showHide($('#divInviteFriendList'), this)">Hide/Show</a></td>
            </tr>
        </table>
        <div id="divInviteFriendList" class="eventMgMtContentContainer">
            <form name="frmFriendInvite" id="frmFriendInvite" method="post" action="InviteFriends">
                <p>Having friends can come in handy, especially when you are putting a raid together. Here's a list of everyone
                   on your friends list. We're gona ahead an marked the ones with Roles/Classes you need and that are on the 
                   same Server as the character you are hosting this event with.</p>
                   
                
                <% Html.RenderPartial("Account/FriendsList", Model.FriendsList,
                   new ViewDataDictionary() { { "GameFilter", true }, { "GameName", Model.Event_GameName }, { "ServerFilter", true }, { "ServerName", Model.Event_ServerName } }); %>
                   
                <a onclick="submitFriendInvite()" class="backLink" style="text-align:center; display:block;" href="javascript:void(0)">Invite Selected Friends</a>
            </form>
        </div>
    </div>
    
    <div class="colapseContainer">
        <table class="colapseSectionHeader">
            <tr>
                <td><h4>Search for Players</h4></td>
                <td class="colapseShowHide"><a href='javascript:void(0)' onclick="showHide($('#divSearchForCharacters'), this)">Hide/Show</a></td>
            </tr>
        </table>
        <div id="divSearchForCharacters" class="eventMgMtContentContainer">
            <p>Need to find someone new to raid with? Go ahead and search from the listing of Leet Raids members. If you
               find someone you need go ahead and invite them. Maybe you can add them to your friends list after.</p>
               
               <% Html.RenderAction("SearchCharacters", "Shared", new { serverID = Model.EventRestritions.ServerID, factionID = Model.EventRestritions.FactionID, evtRest = 1 }); %>
               <%-- Html.RenderPartial("Characters/SearchCharacters"); --%>
        </div>
    </div>
    
    </form>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
