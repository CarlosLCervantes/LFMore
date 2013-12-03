<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LeetRaids.Models.SearchCharactersModel>" %>

<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Models" %>

<script type="text/javascript">
    enforceEventRestrictions = <%= Model.EnforceEventRestrictions.ToString().ToLower() %>;
</script>

<h3>Search Characters</h3>
<table id="tblCharacterSearchFilters">
    <tr>
        <td>Name:</td>
        <td class="colFirst"><input type="text" name="CharacterName" id="CharacterName" /></td>
        <td>Role:</td>
        <td class="colSecond">
            <select id="RoleID" name="RoleID">
                <option value="-1">Any</option>
                <%foreach(Role role in Model.Roles) 
                  {%>
                  <%= "<option value='" + role.RoleID + "'>" + role.RoleName + "</option>" %>
                <%} %>
            </select>
        </td>
        <td>Server:</td>
        <td class="colLast">
            <select id="ServerID" name="ServerID">
                <%= (Model.Servers.Count > 1) ? "<option value='-1'>Any</option>" : String.Empty %>
                <%foreach(Server server in Model.Servers) 
                  {%>
                  <%= "<option value='" + server.ServerID + "'>" + server.Name + "</option>" %>
                <%} %>
            </select>
        </td>
    </tr>
    <tr>
        <td>Class:</td>
        <td>
            <select id="ClassID" name="ClassID">
                <option value="-1">Any</option>
                <%foreach(Class clss in Model.Classes) 
                  {%>
                  <%= "<option value='" + clss.ClassID + "'>" + clss.ClassName + "</option>"%>
                <%} %>
            </select>
        </td>
        <td>Level:</td>
        <td><input id="LVL" name="LVL" /></td>
        <td>Faction</td>
        <td>
            <select id="ddlFactions" name="FactionID">
                <%= (Model.Factions.Count > 1) ? "<option value='-1'>Any</option>" : String.Empty %>
                <%foreach(Faction faction in Model.Factions) 
                  {%>
                  <%= "<option value='" + faction.FactionID + "'>" + faction.FactionName + "</option>"%>
                <%} %>
            </select>
        </td>
    </tr>
</table>
<%= (ViewData.Eval("FilterDisclaimer") != null) ? "<p class='smallNote'>" + ViewData.Eval("FilterDisclaimer") + "</p>" : "" %>
<p id="message" style="font-weight:bold; text-align:center;">No Characters Listed. Start a search to see characters.</p>


<table id="tblCharSearch" class="tblCharacterList" style="display:none">
    <thead>
        <tr>
            <%= Model.EnforceEventRestrictions ? "<th></th>" : "" %>
            <th>Name</th>
            <th>Class</th>
            <th>Roles</th>
            <th>LvL</th>
            <!--<th>GearScore</th>-->
            <th></th>
        </tr>
    </thead>
    <tbody>
    </tbody>
</table>

<p style="margin:15px auto; text-align:center">
    <a href="javascript:void(0)" style="margin-right: 50px" onclick="searchCharacters()"><img src="../../Content/Images/Buttons/btnSearch.png" /></a>
    <a href="javascript:void(0)" onclick="clearFields()"><img src="../../Content/Images/Buttons/btnClear.png" /></a>
</p>

<div style='display:none' id='confirmAttributes' title='Confirm'></div>