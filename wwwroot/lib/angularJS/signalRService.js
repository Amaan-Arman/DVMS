app.factory('signalRService', function ($rootScope) {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    connection.start().then(function () {
        console.log("SignalR Connected");
    }).catch(function (err) {
        console.error("Error while starting connection: " + err.toString());
    });

    return {
        sendMessage: function (userId, user_name, message, type) {
            connection.invoke("SendMessage", userId, user_name, message, type)
                .catch(function (err) {
                    console.error(err.toString());
                });
        },

        onReceiveMessage: function (callback) {
            connection.on("broadcastMessage", function (userId, user_name, message, type) {
                $rootScope.$apply(function () {
                    callback(userId, user_name, message, type);
                });
            });
        }
    };
});
