var app = angular.module('Homeapp', []);
app.controller('TrackguestContrller', function ($scope, $http, SignalRService) {
    SignalRService.init($scope); // Initialize SignalR

    localStorage.setItem('URLIndex', '/Admin/')
    function showMessage(msg) {
        swal(msg);
    }
    function showErrorMessage(msg) {
        swal("failed", msg, "error");
    } 
    function showError(errorCode) {
        switch (errorCode) {
            case "DataBaseError":
                showErrorMessage("<strong>Database Connectivity Error</strong>, Please check the application database connection...");
                break;
            case "NetworkError":
                showErrorMessage("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
                break;
            case "ExceptionError":
                showErrorMessage("<strong>Exception Error</strong>, Please Check the Error Log...");
                break;
            default:
                showErrorMessage(errorCode);
                break;
        }
    }

    $http.get(localStorage.getItem('URLIndex') + 'GuestCheckInList').then(function (i) {
        $scope.GuestCheckInList = i.data;
    },
    function (error) {
        alert(error);
        $scope.GuestCheckInList = error;
     });

    $http.get(localStorage.getItem('URLIndex') + 'GuestCheckOutList').then(function (i) {
        $scope.GuestCheckOutList = i.data;
    },
    function (error) {
        alert(error);
        $scope.GuestCheckOutList = error;
    });

    $scope.GuestSearchFunction = function (status) {
        debugger
        if (status == "Checkin") {
            var id = $("#filter_id").val();
            var start_date_id = $("#start_date_id").val();
            var end_date_id = $("#end_date_id").val();

            if (id == "" && start_date_id == "" && end_date_id == "") {
                showMessage("Select on Search Box or Data Arrange is a Mandatory Requirement, Please Put it..");
            }
            else if (start_date_id != "" && end_date_id == "" || end_date_id != "" && start_date_id == "") {
                showMessage("Select on Search Box or Data Arrange is a Mandatory Requirement, Please Put it..");
            }
            else {
                //localStorage.setItem('Search', "Search");
                //localStorage.setItem('id', id);
                //localStorage.setItem('start_date_id', start_date_id);
                //localStorage.setItem('end_date_id', end_date_id);
                $http.get(localStorage.getItem('URLIndex') + "GuestSearchList?id=" + id + "&start_date_id=" + start_date_id + "&end_date_id=" + end_date_id).then(function (cp) {
                    $scope.GuestCheckInList = cp.data;
                },
                    function (error) {
                        alert(error)
                        $scope.GuestCheckInList = error;
                    });
            }
        } else {
            var id_out = $("#filter_id_out").val();
            var start_date_id_out = $("#start_date_id_out").val();
            var end_date_id_out = $("#end_date_id_out").val();

            if (id_out == "" && start_date_id_out == "" && end_date_id_out == "") {
                showMessage("Select on Search Box or Data Arrange is a Mandatory Requirement, Please Put it..");
            }
            else if (start_date_id_out != "" && end_date_id_out == "" || end_date_id_out != "" && start_date_id_out == "") {
                showMessage("Select on Search Box or Data Arrange is a Mandatory Requirement, Please Put it..");
            }
            else {
                $http.get(localStorage.getItem('URLIndex') + "GuestOutSearchList?id=" + id_out + "&start_date_id=" + start_date_id_out + "&end_date_id=" + end_date_id_out).then(function (cp) {
                    $scope.GuestCheckOutList = cp.data;
                },
                    function (error) {
                        alert(error)
                        $scope.GuestCheckOutList = error;
                    });
            }
        }
      
    }

    $scope.Guestcheckout = function (GuestId) {
        Swal.fire({
            title: "Are you sure?",
            text: "Do you want to Check Out this Guest?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes "
        }).then((result) => {
            if (result.isConfirmed) {

                var data = {
                    Invitation_ID: GuestId,
                };
                $.ajax({
                    type: "POST",
                    url: localStorage.getItem('URLIndex') + 'SetGuestCheckOut',
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    success: function (result) {
                        if (result === "Saved") {
                            Swal.fire({
                                title: "success!",
                                text: "Guest has been Check Out..!",
                                icon: "success"
                            });
                        } else {
                            showMessage(result);
                        }
                    },
                    error: function (xhr) {
                        alert('Error - ' + xhr.status + ': ' + xhr.statusText);
                    }
                });
            }
            else {
                //Swal.fire({
                //    title: "Cancelled!",
                //    text: "Your Post file is safe.",
                //    icon: "error"
                //});
            }
        });

    }
});