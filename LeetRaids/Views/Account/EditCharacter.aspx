<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Modal.Master" Inherits="System.Web.Mvc.ViewPage<EditCharacterModel>" %>

<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Controllers" %>
<%@ Import Namespace="LeetRaids.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    EditCharacter
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">

    <script src="../../Content/Scripts/PageScripts/ManageCharacters.js" type="text/javascript"></script>

    <script src="../../Content/Scripts/Validation.js" type="text/javascript"></script>

    <script type="text/javascript">
        function validateForm() {
            var valid = true;
            if (valid) { valid = validate($("#txtName"), 'singleName', 'valCharName'); }
            if (valid) { valid = validate($("#txtLevel"), 'number', 'valLevel'); }

            if (valid) {
                document.forms[0].submit();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id='formAddChar' action="/Account/EditCharacter" method="post">
        <h2>Edit Character</h2>
        <table id='tblAddCharacter'>
            <tr>
                <td colspan='2'>Name:
                <input name='CharacterName' id='txtName' type='text' value='<%= Model.CharInfo.CharacterName %>' onblur="validate($(this), 'singleName', 'valCharName');" />
                <span id="valCharName" class="valMsg">*</span> </td>
                <td>Level:
                <input name='LVL' id='txtLevel' type='text'  value='<%= Model.CharInfo.LVL %>' onblur="validate($(this), 'number', 'valLevel');" />
                <span id="valLevel" class="valMsg">*</span> </td>
            </tr>
            <tr>
                <td>Server:
                <select id='ddlServer' name='ServerID'>
                    <% foreach (Server s in Model.Servers)
                       {
                           if (s.ServerID == Model.CharInfo.ServerID)
                           {%>
                                <%= "<option selected='true' value='" + s.ServerID + "'>" + s.Name + "</option>"%>
                            <%}
                           else
                           {%>
                                <%= "<option value='" + s.ServerID + "'>" + s.Name + "</option>"%>
                            <%} %>
                    <%} %>
                </select>
                </td>
                <td>Class:
                <select name='ClassID' id='ddlClasses'>
                    <% foreach (Class c in Model.Classes)
                       { %>
                       <% if (c.ClassID == Model.CharInfo.ClassID)
                          {%>
                                <%= "<option selected='true' value='" + c.ClassID + "'>" + c.ClassName + "</option>"%>
                          <%}
                          else
                          {%>
                                <%= "<option value='" + c.ClassID + "'>" + c.ClassName + "</option>"%>
                          <%} %>
                    <%} %>
                </select>
                </td>
                <td>Faction:
                <select name='FactionID' id='ddlFactionID'>
                    <% foreach (Faction f in Model.Factions)
                       {%> 
                        <% if (f.FactionID == Model.CharInfo.FactionID)
                           {%>
                                <%= "<option selected='true' value='" + f.FactionID + "'>" + f.FactionName + "</option>"%>
                        <% }
                           else{%>
                                <%= "<option value='" + f.FactionID + "'>" + f.FactionName + "</option>"%>
                        <% } %>
                    <%} %>
                </select>
                </td>
            </tr>
            <tr id='rowRoles'>
                <% foreach (Role r in Model.Roles)
                   { %>
                <!--TODO: remove the inline style-->
                <%= "<td><img style='height:20px;length:20px' src='../../Content/Images/RoleIcons/" + r.ImageLocation + "' /><input name='RoleID' type='checkbox' value='" + r.RoleID + "' /></td>"%>
                <%} %>
            </tr>
        </table>
        <div id='modalAction'>
            <a href='javascript:void(0)' class="iframe" onclick="validateForm()">
                <img alt="Edit this Character" style='display: block; text-align:center; margin:auto;' src="../../Content/Images/Buttons/btnEditCharacter.png" />
            </a>
            <a href='javascript:void(0)' class="close_fancy" onclick="parent.$.fancybox.close();">or cancel</a>
            <p class='error' style='display: none;'></p>
        </div>
        <input name='CharacterID' type='hidden' value='<%= Model.CharInfo.CharacterID %>' />
        <input name='GameID' type='hidden' value='" + gameID + "' />
    </form>
</asp:Content>
