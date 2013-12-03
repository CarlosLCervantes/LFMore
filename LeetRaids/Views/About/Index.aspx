<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - About Us
</asp:Content>

<asp:Content ID="metaContent" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Description" content="About the Site, Team, and our Goals." />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About LFMore</h2>
    <p>LFMore is provided as a free service for MMO gamers. LFMore was created by MMO gamers for MMO gamers. We seek to enhance and empower players to be the best they can be.</p>
    <h3>The Site:</h3>
    <p>LFMore.com is designed to support very complex operations for any MMO game not just mega hits (IE. World of Warcraft). We are dedicated to providing users with the 
    best user experience through engaging User Interfaces. Also, expect to see more new and innovative features in future.</p>
    <h3>Our Goals:</h3>
    <ul id="goalsList">
        <li>
            <strong>Save Time:</strong>
            <p>MMO games have a tendency to require a large investment of time in order to progress through the game. LFMore provides you 
            with a way to set your group up for success before you even zone into your Raid, Dungeon, PvP encounter. You can also join groups 
            which have been created on the platform and save yourself even more time.</p>
        </li>
        <li>
            <strong>Accessibility (Cloud Computing):</strong>
            <p>Planning ahead is a great time saver. With LFMore you can plan and manage future gaming events from work, school, or anywhere with internet access.</p>
        </li>
        <li>
            <strong>Global Management:</strong>
            <p>If you decide to start playing a new MMO you can still use LFMore just like usual. We can support any MMO game not just Mega Hits.</p>
        </li>
        <li>
            <strong>Innovation:</strong>
            <p>We are constantly designing, developing, and implementing new features. Please contact us if you would like to share a new feature or idea with the community.</p>
        </li>
    </ul>
    <h3>The Team:</h3>
    <p>
    Between everyone on the LFMore team we have played dozens of MMO games. Each person has a very deep understanding of how MMO games work and could be labeled a 
    specialist in at least a few. Each of us knows the amount of dedication an MMO can take and is proud to be a part of providing quality solutions to our fellow gamers.
    </p>

	 




	

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
