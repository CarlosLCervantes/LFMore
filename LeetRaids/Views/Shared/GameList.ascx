<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Game>>" %>
<%@ Import Namespace="DataAccessLayer" %>
<%@ Import Namespace="LeetRaids.Controllers" %>
<%@ Import Namespace="LeetRaids.Models" %>

<select id="games" class="largeDDL">
    <option value="-1">Select a Game</option>
    <%foreach (Game game in Model)
      {%>
        <option value="<%= game.GameID %>"><%= game.GameName%></option>
    <%} %>
</select>