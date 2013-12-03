<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    LFMore - Join, Schedule, Manage MMO Groups
</asp:Content>
<asp:Content ID="metaContent" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Description" content="LFM allows you easily Schedule, Manage, and Join groups or raids for MMO games for Free. We support many Popular MMOs including World of Wacraft and Aion. " />
</asp:Content>
<asp:Content ID="indexMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="indexMainContent">
        <h1>Manage your groups with exceptional accuracy.</h1>
        <div class="divOverviewPoint">
            <img alt="Event Calendar" src="../../Content/Images/calendar_medium.jpg" />
            <p>Schedule and Manage all your events for many popular MMO Games all in one place from any computer.</p>
        </div>
        <br class="clear" />
        <div class="divOverviewPoint">
            <img alt="Group of players" src="../../Content/Images/group_small.jpg" />
            <p>See group overviews, filter attendees by various criteria, contact any player, and more all with a few clicks.</p>
        </div>
        <br class="clear" />
        <div class="divOverviewPoint">
            <img alt="Dragon Raid Boss" style="width:55px;" src="../../Content/Images/dragonHead_medium.png" />
            <p>Built in support for MMO specific events makes it easier to manage your Raids, Dungeon Crawls, PvP Events, etc.</p>
        </div>
        <br class="clear" />
        <div class="divOverviewPoint">
            <img alt="Magnifying Glass" src="../../Content/Images/magnifyingGlass_medium.jpg" />
            <p>Find groups of players from our registry. Stop sitting around and spamming LFM/LFG!</p>
        </div>
        <br class="clear" />
        <h2>Creating, Managing, and Joining a group has never been easier!</h2>
        <a href="/Account/Register"><img class="callToAction" alt="Register Now" src="../../Content/Images/Buttons/btnRegister.png" /></a>
    </div>
</asp:Content>
<asp:Content ID="indexSideContent" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
<%--    <div class="squareAd">
    </div>--%>
</asp:Content>
