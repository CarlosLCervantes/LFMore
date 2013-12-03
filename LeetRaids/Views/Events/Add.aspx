<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AddEventModel>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">LFMore - Add Event</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
<script src="../../Content/Scripts/PageScripts/AddEvent.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Validation.js" type="text/javascript"></script>
    <script type="text/javascript">
        bindEventTypes(0);
        defaultEventDate = '<%= ((DateTime)ViewData["Date"]).ToString("MM/dd/yyyy") %>'

        function formVal() {
            if (confirmEvent()) {
                addEvent($("#hdEventTypeID").val());
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    <h2>Add a New Event</h2>
    <p>Here you can configure and add a new event to LFMore. Once you have added the event you can Manage it and invite your friends.
    First, select one of the games that you have added to your account. You will be presented with a list of common event types for the game.
    Then, drill down to the specific event you want and then fill in the specics.</p>
    <p>You may also want to restrict the quantity of certain roles you will accept.</p>
    <select id="games" style="margin:10px auto; display:block;" onchange="bindEventTypes($('#games :selected').val())">
        <option value="-1">Select a Game</option>
        <%foreach (Game game in Model.Games)
          {%>
            <option value="<%= game.GameID %>"><%= game.GameName%></option>
        <%} %>
    </select>
    
    <div id="eventSelection">
        
    </div>
    <input type="hidden" id="hdEventTypeID" name="EventTypeID" />
    <br class='clear' />
    
    <% Html.RenderPartial("Events/RoleRestrictions", Model.RolesInfo, new ViewDataDictionary() { { "ReadOnly", false } }); %>
    
    <a href="javascript:void(0)"><img id="btnAddEvent" style="display:none;" src="../../Content/Images/Buttons/btnCreateEvent.png" onclick="formVal()" /></a>
    <div style='display:none' id='errors' title='Input Issues'></div>
    </form>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
