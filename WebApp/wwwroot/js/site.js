
var connection = new signalR.HubConnectionBuilder().withUrl("/updateSportDataHub").configureLogging(signalR.LogLevel.Information).build();
connection.start().catch(err => console.error(err.toString()));


function requestHtml(url, parentSelector) {
    $.ajax({
        url: url,
        type: "Get",
        dataType: "json",
        contentType: "application/json",
        success: function (data) {

            var result = JSON.parse(data);

            for (var i = 0; i < result.length; i++) {
                $("#" + parentSelector + result[i].ParentContainerID + ">.panel>.panel-body").append(result[i].DataHtml);
            }
        }
    });
}


connection.on("DeleteMatchesMethodName", function (matchesIDs)
{
  for (var i = 0; i < matchesIDs.length; i++)
  {
    $("#match_" + matchesIDs[i]).remove();

    console.log(matchesIDs[i]);
  }

});

connection.on("AddNewEventsMethodName", function (cacheKey) {

    requestHtml("/Home/GetEventHtml?cachKey=" + cacheKey, "sport_");
});

connection.on("AddNewMatchesMethodName", function (cacheKey)
{

  requestHtml("/Home/GetEventHtml?cachKey=" + cacheKey, "event_");
});

connection.on("AddNewBetsMethodName", function (cacheKey)
{

  requestHtml("/Home/GetEventHtml?cachKey=" + cacheKey, "match_");
});

connection.on("AddNewOddsMethodName", function (cacheKey)
{

  requestHtml("/Home/GetEventHtml?cachKey=" + cacheKey, "bet_");
});



