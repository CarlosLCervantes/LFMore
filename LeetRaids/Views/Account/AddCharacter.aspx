<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Modal.Master" Inherits="System.Web.Mvc.ViewPage<AddCharacterModel>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Controllers" %>
<%@ Import Namespace="LeetRaids.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    AddCharacter
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
    <form id='formAddChar' action="/Account/AddCharacter" method="post">
    <h2>Add a Character</h2>
    <%= Html.ValidationSummary() %>
    
    <% int prevServerID = (Model.Character.ServerID != null && Model.Character.ServerID != 0) ? (int)Model.Character.ServerID : 1;  %>
    <% int prevClassID = (Model.Character.ServerID != null && Model.Character.ServerID != 0) ? (int)Model.Character.ServerID : 1;  %>
    <% int prevFactionID = (Model.Character.ServerID != null && Model.Character.ServerID != 0) ? (int)Model.Character.ServerID : 1;  %>
    <table id='tblAddCharacter'>
        <tr>
            <td colspan=''>
                Name:
                <input name='CharacterName' id='txtName' style="width:100px" type='text' value='<%= ViewData.Eval("Character.CharacterName")%>' onblur="validate($(this), 'singleName', 'valCharName');" />
                <span id="valCharName" class="valMsg">*</span>
            </td>
            <td>
                Faction:
                <select name='FactionID' id='ddlFactionID'>
                    <% foreach (Faction f in Model.Factions)
                       { %>
                       <%= "<option value='" + f.FactionID + "'" + ((f.FactionID == prevFactionID) ? "selected='true'" : "") + ">" + f.FactionName + "</option>"%>
                    <%} %>
                </select>

            </td>
            <td>
                Level:
                <input name='LVL' id='txtLevel' type='text' style="width:40px" value='<%= ViewData.Eval("Character.LVL")%>' onblur="validate($(this), 'number', 'valLevel');" />
                <span id="valLevel" class="valMsg">*</span>
            </td>
        </tr>
        <tr>
            <td>
                Server:
                <select id='ddlServer' name='ServerID'>
                    <% foreach (Server s in Model.Servers)
                       { %>
                            <%= "<option value='" + s.ServerID + "' " + ((s.ServerID == prevServerID) ? "selected='true'" : "") + ">" + s.Name + "</option>"%>
                    <%} %>
                </select>
            </td>
            <td>
                Class:
                <select name='ClassID' id='ddlClasses'>
                    <% foreach (Class c in Model.Classes)
                       { %>
                       <%= "<option value='" + c.ClassID + "'" + ((c.ClassID == prevClassID) ? "selected='true'" : "") + ">" + c.ClassName + "</option>"%>
                    <%} %>
                </select>
            </td>
            <td>
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
            <img alt="Add New Character" style='display: block; text-align:center; margin:auto;' src='../../Content/Images/Buttons/btnAddCharacter2.png' />
        </a>
        <a href='javascript:void(0)' onclick="parent.$.fancybox.close();" class="close_fancy">or cancel</a>
        <p class='error' style='display: none;'></p>
    </div>
    
    <input name='GameID' type='hidden' value='<%= Model.GameID %>' />
    </form>
</asp:Content>
