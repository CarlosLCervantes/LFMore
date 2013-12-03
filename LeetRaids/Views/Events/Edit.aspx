<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EditEventModel>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Models" %>
<%@ Register src="../Shared/Events/RoleRestrictions.ascx" tagname="RoleRestrictions" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	LFMore - Edit Event
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="../../Content/Scripts/Validation.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/PageScripts/AddEvent.js" type="text/javascript"></script>
    <script type="text/javascript">
        selectGameID = <%= Model.GameID %>;
        $.getJSON("/Game/GetAllRolesByGameID/" + selectGameID,
            function(rolesData) {
                roles = rolesData;
                selectEvent(<%= Model.EventTypeInfo.EventTypeID %> ,'<%= GlobalMethod.JSEncode(Model.EventTypeInfo.EventTypeName) %>', <%= Model.EventSubTypeInfo.EventGroupSubTypeID %>, '<%= GlobalMethod.JSEncode(Model.EventSubTypeInfo.Name) %>');
                geteventInfo(<%= Model.EventInfo.EventID %>);
            }
        );
        
        function formVal(){
            if (confirmEvent()) {
                document.forms[0].submit();
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<form id="form1" runat="server" action="/Events/Edit" method="post">
    <h2>Edit Event</h2>
    <p>
        You can edit your event information here. The current information is all ready for you to change. Once you are done click the Edit Event button.
    </p>
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
    
    
    
    <!--<table id="tblClassRestrictions">
        <tr>
        </tr>
    </table>-->

    <a href="javascript:void(0)"><img alt="Edit this Event" style="display: block; text-align:center; margin:auto;" src="../../Content/Images/Buttons/btnEditEvent.png" onclick="formVal()" /></a>
    <div style='display:none' id='errors' title='Input Issues'></div>
    <input type="hidden" name="Char" id="hdChar" value='<%= ViewData.Eval("CharacterID") %>' />
    
</form>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
