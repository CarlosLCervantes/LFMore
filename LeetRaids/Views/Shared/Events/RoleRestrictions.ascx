<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<EventRoleInfo>>" %>
<%@ Import Namespace="LeetRaids.Models" %>

<% bool readOnly = (ViewData["ReadOnly"] != null) ? Convert.ToBoolean(ViewData.Eval("ReadOnly")) : false; %>

<div id="divNewEventRoleRestrictions" class="colapseContainer">
    <table class="colapseSectionHeader">
        <tr>
            <td><h4>Role Restrictions</h4></td>
            <td class="colapseShowHide"><a href='javascript:void(0)' onclick="showHide($('#divNewEventRoleRestrictionsContainer'), this)">Hide/Show</a></td>
        </tr>
    </table>
    <div id="divNewEventRoleRestrictionsContainer" style="display: none;">
        <p>You may choose to define a set amount of places in your event for specific role types. The suggested number of roles per type has been pre-populated for you. Feel free to enter your own numbers. You may also clear the values indicating any number of roles can join your raid.</p>
        <p>Once the number of roles you have identified for your event is reached. No more members of that role type will be allowed to join.</p>
        <%-- TODO: Remove World of Warcraft HardCodes --%>
        <%
            var healerRestriction = Model.Where(e => e.Role.RoleID == 2).SingleOrDefault().RoleRestriction;
            string healerRestrictionCount = (healerRestriction != null) ? healerRestriction.Quantity.ToString() : "";
            var tankRestriction = Model.Where(e => e.Role.RoleID == 3).SingleOrDefault().RoleRestriction;
            string tankRestrictionCount = (tankRestriction != null) ? tankRestriction.Quantity.ToString() : "";
            var dmgRestriction = Model.Where(e => e.Role.RoleID == 1).SingleOrDefault().RoleRestriction;
            string dmgRestrictionCount = (dmgRestriction != null) ? dmgRestriction.Quantity.ToString() : "";
        %>
        <table id="tblNewEventRoleRestrictions">
            <tr>
                <td>Healers:

                    <% if (readOnly)
                       { %>
                            <strong><%= (String.IsNullOrEmpty(healerRestrictionCount) || healerRestrictionCount == "0") ? "N/A" : healerRestrictionCount%></strong>
                    <% }
                       else
                       {%>
                        <input type="text" name="HealerRestriction" id="txtHealerRestriction" value="<%= healerRestrictionCount %>" />
                    <%}%>
                </td>
                <td>Tank:
                    <% if (readOnly)
                       { %>
                            <strong><%= (String.IsNullOrEmpty(tankRestrictionCount) || tankRestrictionCount == "0") ? "N/A" : tankRestrictionCount%></strong>
                    <% }
                       else
                       {%>
                        <input type="text" name="TankRestriction" id="txtTankRestriction" value="<%= tankRestrictionCount %>" />
                    <%}%>
                </td>
                <td>DPS:
                       <% if (readOnly)
                       { %>
                            <strong><%= (String.IsNullOrEmpty(dmgRestrictionCount) || dmgRestrictionCount == "0") ? "N/A" : dmgRestrictionCount%></strong>
                    <% }
                       else
                       {%>
                        <input type="text" name="DamageRestriction" id="txtDPSRestriction" value="<%= dmgRestrictionCount %>" />
                    <%}%>
                
                </td>
            </tr>
        </table>
        
        <% if (!readOnly){ %> <a href="javascript:void(0)" onclick="clearEventRestrictions()"><img style="margin:10px auto; display:block;" src="../../../Content/Images/Buttons/btnReset_medium.png" /></a> <%} %>
        
    </div>
</div>
