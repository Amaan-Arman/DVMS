app.factory('SignalRService', function ($http) {
    var connection = null;

    return {
        init: function ($scope) {
            // Build connection to hub
            connection = new signalR.HubConnectionBuilder()
                .withUrl("/chatHub")   // hub endpoint in Core
                .withAutomaticReconnect()
                .build();

            // Start connection
            connection.start()
                .then(function () {
                    console.log("SignalR connected (Core).");
                })
                .catch(function (err) {
                    console.error("Error while starting connection: " + err.toString());
                });

            // Register handlers
            connection.on("broadcastMessage", function (userId, user_name, message, type) {
                if (type === "message") {
                    $http.get(localStorage.getItem('URLIndex') + 'GetMessage')
                        .then(function (i) {
                            $scope.GetMessage = i.data;
                        }, function (error) {
                            alert(error);
                            $scope.GetMessage = error;
                        });

                    if (localStorage.getItem('Getchatmsg') !== "") {
                        $http.get(localStorage.getItem('URLIndex') + 'GetChatbox?ID=' + localStorage.getItem('Getchatmsg'))
                            .then(function (i) {
                                $scope.GetChatbox = i.data;
                            }, function (error) {
                                alert(error);
                                $scope.GetChatbox = error;
                            });
                    }
                }
                else if (type === "notification") {
                    $http.get(localStorage.getItem('URLIndex') + 'GetNotification')
                        .then(function (response) {
                            $scope.GetNotification = response.data;
                        }, function (error) {
                            alert(error);
                            $scope.GetNotification = error;
                        });
                    $http.get(localStorage.getItem('URLIndex') + 'CheckInOutList').then(function (i) {
                        $scope.CheckInOutList = i.data;
                    },
                        function (error) {
                            alert(error);
                            $scope.CheckInOutList = error;
                        });
                }
                else {
                    console.log("undefined type: " + type);
                }
            });
        },

        // Send message in Core
        sendMessage: function (user_id, user_name, msg) {
            if (connection) {
                connection.invoke("SendMessage", user_id, user_name, msg, "message")
                    .catch(function (err) {
                        console.error(err.toString());
                    });
            } else {
                console.warn("SignalR connection not ready yet.");
            }
        }
    };
});
//app.factory('signalRService', function ($rootScope) {
//    var connection = new signalR.HubConnectionBuilder()
//        .withUrl("/chatHub")
//        .build();

//    connection.start().then(function () {
//        console.log("SignalR Connected");
//    }).catch(function (err) {
//        console.error("Error while starting connection: " + err.toString());
//    });

//    return {
//        sendMessage: function (userId, user_name, message, type) {
//            connection.invoke("SendMessage", userId, user_name, message, type)
//                .catch(function (err) {
//                    console.error(err.toString());
//                });
//        },

//        onReceiveMessage: function (callback) {
//            connection.on("broadcastMessage", function (userId, user_name, message, type) {
//                $rootScope.$apply(function () {
//                    callback(userId, user_name, message, type);
//                });
//            });
//        }
//    };
//});