<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<MemFriend>>" %>
<%--<%@ Import Namespace="LeetRaids.Models" %>--%>
<%@ Import Namespace="DataAccessLayer" %>

<div id="divCharacterListContainer">
    <%-- int selectedCharID = (ViewData.Eval("SelectedCharacter") != null) ? Convert.ToInt32(ViewData.Eval("SelectedCharacter")) : 0;  --%>
    <%-- int selectedRoleID = (ViewData.Eval("SelectedRole") != null) ? Convert.ToInt32(ViewData.Eval("SelectedRole")) : 0; --%>
    <p style="margin:5px 10px;">Your Friends List:</p>
    <% if (Model.Count() > 0)
       {%>
        <table id='tblCharacterList' cellpadding='0' cellspacing='0'>
            <thead>
                <tr>
                    <th></th>
                    <th>Name</th>
                    <th>Class</th>
                    <th>Roles</th>
                    <th>LVL</th>
                    <th>Select</th>
                    <!--<th>Server</th>
                    <th>Faction</th>-->
                </tr>
            </thead>
            <tbody>
                <% int rowCount = 0; %>
                <% foreach (MemFriend memFriend in Model)
                   { %>
                    <% string rowClass = ((rowCount % 2) == 0) ? "charListRow" : "charListRow charListRowAlt"; %>
                    <tr class="<%= rowClass  %>">
                    <td style="width:auto"> 
                        <img src='../../../Content/Images/<%= (memFriend.HighlightOnList) ? "checkmark_small.png" : "crossmark_small.png" %>' />
                    </td>
                    <td> <%= memFriend.CompleteCharData.CharacterName %> </td>
                    <td> <%= memFriend.CompleteCharData.ClassName %> </td>
                    <td>
                        <% foreach (Role role in memFriend.CompleteCharData.Roles)
                           { %>
                            <%= "<img style='width:17px;height:17px;' src='../../../Content/Images/RoleIcons/" + role.ImageLocation + "' />" %>
                        <% } %>
                    
                    </td>
                    <td> <%= memFriend.CompleteCharData.LVL %> </td>
                    <td>
                        <% if (true)
                           {%> 
                                <% if (false/*memChar.CharacterID == selectedCharID*/)
                                   { %>
                                        <input type="checkbox" name="SelectedCharacter" value="<%= memFriend.FriendCharacterID %>" checked="true" /> 
                                <% }
                                   else
                                   {%>
                                        <input type="checkbox" name="SelectedCharacter" value="<%= memFriend.FriendCharacterID %>" /> 
                                 <%} %>
                        <% }
                           else
                           {%>
                                <a>Select</a>
                        <% } %>
                    </td>
                    </tr>
                    <% rowCount++; %>
                <%} %>
            </tbody>
        </table>
        <% bool gameFilter = (ViewData.Eval("GameFilter") != null) ? Convert.ToBoolean(ViewData.Eval("GameFilter").ToString()) : false;%>
           <%if (gameFilter)
           { %>
            <p class="smallNote">Only Showing Friends for game <%= ViewData.Eval("GameName")%> </p>
        <% } %>
        <% bool serverFilter = (ViewData.Eval("ServerFilter") != null) ? Convert.ToBoolean(ViewData.Eval("ServerFilter").ToString()) : false;%>
        <%if (serverFilter)
           { %>
            <p class="smallNote">Only Showing Friends for server <%= ViewData.Eval("ServerName")%> </p>
        <% } %>
    <%} %>
    <% else{ %>
        <p style="text-align:center;">You have no friends :(</p>
    <%} %>
</div>