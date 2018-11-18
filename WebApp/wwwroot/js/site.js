
var connection = new signalR.HubConnectionBuilder().withUrl("/updateSportDataHub").configureLogging(signalR.LogLevel.Information).build();
connection.start().catch(err => console.error(err.toString()));

connection.on("DeleteMatchesMethodName", function (matchesIDs) {
    for (var i = 0; i < matchesIDs.length; i++) {
        $("#match_" + matchesIDs[i]).remove();

        console.log(matchesIDs[i]);
    }
    
});

function requestHtml(url, parentSelector) {
    $.ajax({
        url: url,
        type: "Get",
        dataType: "json",
        contentType: "application/json",
        success: function (data) {

            var result = JSON.parse(data);

            for (var i = 0; i < result.length; i++) {
                $("#" + parentSelector + result[i].ParentContainerID + ">.panel>.panel-body").html(result[i].DataHtml);
            }
        }
    });
}

connection.on("AddNewEventsMethodName", function (cacheKey) {

    requestHtml("/Home/GetEventHtml?cachKey=" + cacheKey, "sport");
});



