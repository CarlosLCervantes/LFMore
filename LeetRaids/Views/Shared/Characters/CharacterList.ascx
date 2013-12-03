<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CompleteCharacterData>>" %>
<%@ Import Namespace="DataAccessLayer" %>

<div id="divCharacterListContainer">
    <% int selectedCharID = (ViewData.Eval("SelectedCharacter") != null) ? Convert.ToInt32(ViewData.Eval("SelectedCharacter")) : 0;  %>
    <% int selectedRoleID = (ViewData.Eval("SelectedRole") != null) ? Convert.ToInt32(ViewData.Eval("SelectedRole")) : 0; %>
    <p style="margin:5px 10px;">Your World of Warcraft Characters:</p>
    <% if (Model.Count() > 0)
       {%>
        <table id='tblCharacterList' cellpadding='0' cellspacing='0'>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Class</th>
                    <th>Server</th>
                    <th>Faction</th>
                    <th>Role</th>
                    <th>Select</th>
                </tr>
            </thead>
            <tbody>
                <% int rowCount = 0; %>
                <% foreach (CompleteCharacterData memChar in Model)
                   { %>
                    <% string rowClass = ((rowCount % 2) == 0) ? "charListRow" : "charListRow charListRowAlt"; %>
                    <tr class="<%= rowClass  %>">
                    <td> <%= memChar.CharacterName %> </td>
                    <td> <%= memChar.ClassName %> </td>
                    <td> <%= memChar.ServerName %> </td>
                    <td> <%= memChar.FactionName %> </td>
                    <td>
                        <% foreach (Role role in memChar.Roles) { %>
                           <% if (memChar.CharacterID == selectedCharID && role.RoleID == selectedRoleID)
                              { %>
                                    <input type="radio" name="Role" value="<%= role.RoleID %>" checked="true" /> <%= role.RoleName %> |
                           <% }
                              else
                              {%>
                                    <input type="radio" name="Role" value="<%= role.RoleID %>" /> <%= role.RoleName %> |
                            <%} %>
                           
                        <% } %>
                    
                    </td>
                    <td>
                        <% if (true)
                           {%> 
                                <% if (memChar.CharacterID == selectedCharID)
                                   { %>
                                        <input type="radio" name="SelectedCharacter" value="<%= memChar.CharacterID %>" checked="true" /> 
                                <% }
                                   else
                                   {%>
                                        <input type="radio" name="SelectedCharacter" value="<%= memChar.CharacterID %>" /> 
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
    <%} %>
    <% else{ %>
        <p style="text-align:center;">Couldn't find any characters</p>
    <%} %>
</div>

