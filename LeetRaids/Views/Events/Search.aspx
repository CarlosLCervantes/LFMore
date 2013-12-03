<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EventSearchModel>" %>
<%@ Import Namespace="LeetRaids.Models" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Register Src="~/Views/Shared/GameList.ascx" TagName="GameList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="../../Content/Scripts/Validation.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/PageScripts/SearchEvents.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            var gameID = <%= Model.EventSearchParams.GameID %>;
            if(gameID > 0){
                var serverID = "<%= Model.EventSearchParams.ServerID %>";
                popServers(gameID, serverID);
            }
            $("#txteventdatestart").datepicker();
            $("#txteventdateend").datepicker();
        });
        
        function valForm(){
            if($("#ddlGames").find("option:selected").val() < 1){
                $("#erroMsg").html("Please Select a Game");
                return false;
            }
            
            if($("#txtSearchTerm").val() == ""){
                $("#erroMsg").html("Please Enter a Search Term");
                return false;
            }
            
            document.forms[0].submit();
        }
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="StylesContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Search Events</h2>
    <p>Need a group? Search for LFMore events that have been created on your server. Stop waiting around spaming "LFG" and plan ahead.</p>
<div class="parentContainer">
    <h4 class='containerHeaderText'>Search Event</h4>
    <form id='frmEventSearch' method="post" action="/Events/Search">
    <div id="divSearchEvents">
        <div id='eventSearchBox'>
           <%-- <uc1:GameList ID="ucGameList" runat="server" />--%>
            <%--<select id="ddlGames" class="eventSearchGames"></select>--%>
            <%= Html.DropDownList("EventSearchParams.GameID", new SelectList(Model.Games, "GameID", "GameName", Model.EventSearchParams.GameID), new { @id = "ddlGames", onChange = "gameSelected($(this))" })%>
            <%= Html.TextBox("EventSearchParams.SearchTerm", Model.EventSearchParams.SearchTerm, new { @id = "txtSearchTerm"})%> <!--<input type="text" id="txtSearchTerm" /> -->
            <!--<a href="javascript:void(0);" onclick="documents.forms[0].submit()">Search</a>-->
            <input type="button" value="Search" onclick="valForm();" />
            <em class="smallNote">Search for Events by Name, Event Type, or Event Creator</em>
            <p id="erroMsg" class="errorMsg"></p>
            <%= Html.CheckBox("EventSearchParams.ShowFullEvents", false, new { @id = "cbShowFullEvents" }) %>
            <!--<input type="checkbox" name="showFullEvents" id="cbShowFullEvents" />--> <label id="lblShowFullEvents" for="cbShowFullEvents"> Show Event that are Full</label>
            <a href="javascript:void();" onclick="$('#filters').toggle();">Display Filters</a>
        </div>

        <div id="filters" style="display:none;">
            <span class='sectionHeader'>General Filters:</span>
            <table id="tblGlobalFilters">
                <tr>
                    <td>Event Name</td>
                    <td class='inputFieldsFirst'><%= Html.TextBox("EventSearchParams.Eventname", Model.EventSearchParams.EventName, new {@id = "txtEventNameFilter"}) %></td>
                    <td>Event Creator</td>
                    <td class='inputFieldsSecond'><%= Html.TextBox("EventSearchParams.EventCreatorsCharName", Model.EventSearchParams.EventCreatorsCharName, new {@id = "txtEventCreatorFilter"}) %></td>
                </tr>
                <tr>
                    <td>Date</td>
                    <td>
                        <%= Html.TextBox("EventSearchParams.EventStartDate", Model.EventSearchParams.EventStartDate, new { @id = "txtEventDateStart", @class = "dateBox", onblur = "val_el($(this).val(), isDate, 'valMsgStartDate', 'dynamic')" })%>
                        <span id="valMsgStartDate" class="valMsgDynamic">*</span>
                        ---- <%= Html.TextBox("EventSearchParams.EventEndDate", Model.EventSearchParams.EventEndDate, new { @id = "txtEventDateEnd", @class = "dateBox", onblur = "val_el($(this).val(), isDate, 'valMsgEndDate', 'dynamic')" })%>
                        <span id="valMsgEndDate" class="valMsgDynamic">*</span>
                    </td>
                    <td>Server</td>
                    <td>
                        <select name="EventSearchParams.ServerID" id='ddlServers' disabled="disabled">
                            <option>No Game Selected</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>Time</td>
                    <td>
                        <input name="startHour" id='txtEventTimeStartHour' class="timeBox" value='<%= ViewData.Eval("StartHour") %>' /> : 
                        <input name="startMinute" id='txtEventTimestartMinute' class="timeBox" value='<%= ViewData.Eval("StartMinute") %>' />
                        <select name="startMeridian" id='ddlEventTimeStartMeridian' class='selectMeridian'>
                            <%= "<option " + ((ViewData.Eval("StartMeridian").ToString() == "PM") ? "selected" : "") + " value='PM'>PM</option>" %>
                            <%= "<option " + ((ViewData.Eval("StartMeridian").ToString() == "AM") ? "selected" : "") + " value='AM'>AM</option>"%>
                        </select> 
                        ---
                        <input name="endHour" id='txtEventTimeEndHour' class="timeBox" value='<%= ViewData.Eval("EndHour") %>' /> : 
                        <input name="endMinute" id='txtEventTimeEndMinute' class="timeBox" value='<%= ViewData.Eval("EndMinute") %>' />
                        <select name="endMeridian" id='ddlEventTimeEndMeridian' class='selectMeridian'>
                            <%= "<option " + ((ViewData.Eval("EndMeridian").ToString() == "PM") ? "selected" : "") + " value='PM'>PM</option>"%>
                            <%= "<option " + ((ViewData.Eval("EndMeridian").ToString() == "AM") ? "selected" : "") + " value='AM'>AM</option>"%>
                        </select> 
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <span class='sectionHeader'>Game Specific:</span>
            <table id="tblGameSpecificFilters">
                <tr>
                    <td>Event Group</td>
                    <td>
                        <select id='ddlEventGroups' disabled="disabled" onchange='popEventSubGroups($(this))'>
                            <option>No Game Selected</option>
                        </select>
                    </td>
                    <td>Event Sub Groups</td>
                    <td>
                        <select id='ddlEventSubGroups' disabled="disabled" onchange="popEventTypes($(this))">
                            <option>No Event Group Selected</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>Event Type</td>
                    <td>
                         <select id='ddlEventTypes' disabled="disabled">
                            <option>No Event Sub Group Selected</option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</div>

<div class="parentContainer">
    <h4 class='containerHeaderText'>Event Search Results</h4>
    <div id="divEventSearchResults">
        <% foreach (EventOverview e in Model.Events) //Begin ForEach
            {%>
               <% bool eventExpired = (e.EventInfo.IsExpired() || !e.EventInfo.Active); %>
               <% string tblClassName = (eventExpired) ? "eventInfoTable inactive" : "eventInfoTable"; %>
                <table id="eventInfoTable" class="<%= tblClassName %>" cellspacing="0" cellpadding="0">
                    <tr>
                        <td rowspan="3" class="thumb">
                            <img alt="Event Image" style="width:150px;height:125px;" src="../../Content/Images/DungeonThumbs/<%= e.EventImageFile %>>" />
                        </td>
                        <td><strong>Name: <%= e.EventInfo.EventName%></strong></td>
                        <td><strong>Date: <%= e.EventInfo.Date.ToShortDateString()%></strong></td>
                        <td><strong>Time: <%= e.EventInfo.EventTime %></strong></td>
                        <td class="button" style="vertical-align: bottom;">
                            <a target="_parent" href='/Events/ViewEvent/<%= e.EventInfo.EventID %>'>
                                <img alt="View Event" src="../../Content/Images/Buttons/btnView_small.png" />
                            </a>   
                        </td>
                    </tr>
                    <tr>
                        <td><strong>Type: <%= e.EventTypeName%></strong></td>
                        <td><strong>Players: <%= e.CurrentAttendees%></strong></td>
                        <td>&nbsp;</td>
                        <td class="button edit">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                        <td class="button delete" style="vertical-align: top;">
                            &nbsp;
                        </td>
                    </tr>
                </table>
        <%} //End For Each%>
    </div>
</div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderAction("LoginPanel", "Account"); %>
</asp:Content>
