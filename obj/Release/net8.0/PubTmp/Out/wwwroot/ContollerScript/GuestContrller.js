var app = angular.module('Homeapp', []);
app.controller('GuestContrller', function ($scope, $http, SignalRService) {

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

    $http.get(localStorage.getItem('URLIndex') + 'GetInvitationList').then(function (i) {
        debugger
        $scope.GetInvitationList = i.data;
    },
        function (error) {
            alert(error);
            $scope.GetInvitationList = error;
        });

    $http.get(localStorage.getItem('URLIndex') + 'GetGuestList').then(function (i) {
        debugger
        $scope.GetGuestList = i.data;
    },
    function (error) {
        alert(error);
        $scope.GetGuestList = error;
        });

    $scope.SetGusetFunction = function () {
        debugger
        var GuestFullNameID = $("#GuestFullName").val();
        var GuestEmailID = $("#GuestEmail").val();
        var GuestPhoneID = $("#GuestPhone").val();
        var GuestCNICID = $("#GuestCNIC").val();

        if (!GuestFullNameID) {
            showMessage("Name is required.");
            return;
        } else if (!GuestEmailID) {
            showMessage("Email is required.");
            return;
        } else if (!GuestPhoneID) {
            showMessage("Phone is required.");
            return;
        } else if (!GuestCNICID) {
            showMessage("CNIC is required.");
            return;
        }
        $('.page-loader-wrapper').fadeIn();
        var data = {
            GuestFullName: GuestFullNameID,
            GuestEmail: GuestEmailID,
            GuestPhone: GuestPhoneID,
            GuestCNIC: GuestCNICID
        };
        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'SetGuset',
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (result) {
                if (result === "Saved") {
                    $('.page-loader-wrapper').fadeOut();
                    $("#GuestFullName").get(0).value = "";
                    $("#GuestEmail").get(0).value = "";
                    $("#GuestPhone").get(0).value = "";
                    $("#GuestCNIC").get(0).value = "";

                    $http.get(localStorage.getItem('URLIndex') + 'GetGuestList').then(function (i) {
                        debugger
                        $scope.GetGuestList = i.data;
                    },
                        function (error) {
                            alert(error);
                            $scope.GetGuestList = error;
                        });
                } else {
                    $('.page-loader-wrapper').fadeOut();
                    showError(result);
                }
            },
            error: function (xhr) {
                $('.page-loader-wrapper').fadeOut();
                alert('Error - ' + xhr.status + ': ' + xhr.statusText);
            }
        });
    };
 
    $scope.SetGusetInvitationFunction = function () {
        Swal.fire({
            title: "Are you sure?",
            text: "Do you want to send the invitation?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes "
        }).then((result) => {
            if (result.isConfirmed) {
                $('.page-loader-wrapper').fadeIn();
                debugger

                var Guest_ID = $("#Guest_Id").val();
                var VisitPurpose_ID = $("#VisitPurposeID").val();
                var invite_Date = $("#inviteDate").val();
                var invite_Time = $("#inviteTime").val();

                if (!Guest_ID) {
                    showMessage("Guest Name is required.");
                    $('.page-loader-wrapper').fadeOut();
                    return;
                } else if (!VisitPurpose_ID) {
                    $('.page-loader-wrapper').fadeOut();
                    showMessage("Visit Purpose_ID is required.");
                    return;
                }
                else if (!invite_Date) {
                    $('.page-loader-wrapper').fadeOut();
                    showMessage("Date is required.");
                    return;
                } 
                else if (!invite_Time) {
                    $('.page-loader-wrapper').fadeOut();
                    showMessage("Time is required.");
                    return;
                }
               
                var data = {
                    GuestId: Guest_ID,
                    GuestFullName: $("#Guest_Id").find("option:selected").text(),
                    VisitPurpose: VisitPurpose_ID,
                    VisitDate: invite_Date,
                    VisitTime: invite_Time
                };
                $.ajax({
                    type: "POST",
                    url: localStorage.getItem('URLIndex') + 'SetGusetInvitation',
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    success: function (result) {
                        if (result === "Saved") {
                            $('.page-loader-wrapper').fadeOut();
                            Swal.fire({
                                title: "success!",
                                text: "Your Invitation has been send successfully..!",
                                icon: "success"
                            });
                            $("#Guest_Id").get(0).value = "";
                            $("#VisitPurposeID").get(0).value = "";
                            $("#inviteDate").get(0).value = "";
                            $("#inviteTime").get(0).value = "";

                        } else {
                            $('.page-loader-wrapper').fadeOut();
                            showMessage(result);
                        }
                    },
                    error: function (xhr) {
                        $('.page-loader-wrapper').fadeOut();
                        alert('Error - ' + xhr.status + ': ' + xhr.statusText);
                    }
                });
            }
            else {
               $('.page-loader-wrapper').fadeOut();
                //Swal.fire({
                //    title: "Cancelled!",
                //    text: "Your Post file is safe.",
                //    icon: "error"
                //});
            }
        });

    } 
});