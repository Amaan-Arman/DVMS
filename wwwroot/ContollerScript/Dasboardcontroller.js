var app = angular.module('Homeapp', []);
app.controller('Dasboardcontroller', function ($scope, $http, SignalRService) {
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
    $http.get(localStorage.getItem('URLIndex') + 'GetMessage')
        .then(function (i) {
            $scope.GetMessage = i.data;
        }, function (error) {
            alert(error);
            $scope.GetMessage = error;
        });
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
    $http.get(localStorage.getItem('URLIndex') + 'GetTopvisitor').then(function (i) {
        $scope.GetTopvisitor = i.data;
    },
        function (error) {
            alert(error);
            $scope.GetTopvisitor = error;
        });
    $http.get(localStorage.getItem('URLIndex') + 'GetTopguest').then(function (i) {
        $scope.GetTopguest = i.data;
    },
        function (error) {
            alert(error);
            $scope.GetTopguest = error;
        });

    $scope.DetailFuntion = function (status) {
        $('.page-loader-wrapper').fadeIn();
        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'DetailFuntion',
            data: { status: status },
            success: function (result) {
                debugger
                $('.page-loader-wrapper').fadeOut();

                $("#inspectionreportHeading").text("Product Detail");
                $("#inspectionreportModalBodyId").html(result);
                $("#inspectionreportModalID").modal('show');
            },
            error: function (xhr, status, error) {
                debugger
                $('.page-loader-wrapper').fadeOut();
                var errorMessage = xhr.status + ': ' + xhr.statusText
                alert('Error - ' + errorMessage);
            }
        })
    }

    $http.get(localStorage.getItem('URLIndex') + 'FloorWiseData').then(function (i) {
        debugger;
        $scope.FloorWiseData = i.data;

        // Extract floor numbers, guest counts, and visitor counts
        var floors = [];
        var guestCounts = [];
        var visitorCounts = [];

        angular.forEach($scope.FloorWiseData, function (row) {
            floors.push( row.floor_no);   // X-axis label
            guestCounts.push(row.gusetCheckIn);   // Guest data
            visitorCounts.push(row.visitorCheckIn); // Visitor data
        });

        // Update chart options dynamically
        var options = {
            series: [
                { name: 'Guests', data: guestCounts},
                { name: 'Visitors', data: visitorCounts }
            ],
            chart: {
                type: 'bar',
                height: 350
            },
            plotOptions: {
                bar: {
                    horizontal: false,
                    columnWidth: '55%',
                    borderRadius: 5,
                    borderRadiusApplication: 'end'
                },
            },
            dataLabels: {
                enabled: true,
                style: {
                    colors: ["#304758"]
                }
            },
            stroke: {
                show: true,
                width: 2,
                colors: ['transparent']
            },
            xaxis: {
                categories: floors   // Dynamic floor labels
                //categories: ['1st', '2nd', '3rd', '4th', '5th', '6th', '7th', '8th', '9th', '10th', '11th', '12th', '13th', '14th', '15th'],
            },
            yaxis: {
                title: { text: 'Count' }
            },
            fill: { opacity: 1 },
            tooltip: {
                y: {
                    formatter: function (val) {
                        return val + "";
                    }
                }
            }
        };

        var chart = new ApexCharts(document.querySelector("#chart"), options);
        chart.render();

    }, function (error) {
        alert(error);
        $scope.FloorWiseData = error;
    });

    $scope.MarkRead = function (type) {
        $('.page-loader-wrapper').fadeIn();
        debugger
        var data = {
            notificationType: type,
            updateIDs: []
        };
        if (type == "msg") {
            for (var j = 0; j < $scope.GetMessage.length; j++) {
                data.updateIDs.push($scope.GetMessage[j].receiver_user_id);
            }
        } else {
            for (var j = 0; j < $scope.GetNotification.length; j++) {
                data.updateIDs.push($scope.GetNotification[j].receiver_user_id);
            }
        }
        $.ajax(
            {
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'MarkasRead',
                data: JSON.stringify(data),
                contentType: "application/json",
                success: function (status) {
                    $('.page-loader-wrapper').fadeOut();
                    //debugger
                    if (status == 'Saved') {
                        $http.get(localStorage.getItem('URLIndex') + 'GetNotification').then(function (i) {
                            $scope.GetNotification = i.data;
                        },
                            function (error) {
                                alert(error);
                                $scope.GetNotification = error;
                            });
                        $http.get(localStorage.getItem('URLIndex') + 'GetMessage').then(function (i) {
                            $scope.GetMessage = i.data;
                        },
                            function (error) {
                                alert(error);
                                $scope.GetMessage = error;
                            });
                    }
                    else {
                        showError(status);
                    }
                },
                error: function (xhr, status, error) {
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    showMessage(errorMessage);
                }
            });
    }


    //var options = {
    //    series: [{
    //        name: 'Guests',
    //        data: [44, 55, 57, 56, 61, 58, 63, 60, 66, 56, 61, 58, 63, 60, 66]
    //    }, {
    //        name: 'Visitors',
    //        data: [76, 85, 99, 98, 87, 105, 91, 100, 94, 56, 61, 58, 63, 60, 66]
    //    }],
    //    chart: {
    //        type: 'bar',
    //        height: 350
    //    },
    //    plotOptions: {
    //        bar: {
    //            horizontal: false,
    //            columnWidth: '55%',
    //            borderRadius: 5,
    //            borderRadiusApplication: 'end'
    //        },
    //    },
    //    dataLabels: {
    //        enabled: true
    //    },
    //    stroke: {
    //        show: true,
    //        width: 2,
    //        colors: ['transparent']
    //    },
    //    xaxis: {
    //        categories: ['1st', '2nd', '3rd', '4th', '5th', '6th', '7th', '8th', '9th', '10th', '11th', '12th', '13th', '14th', '15th'],

    //    },
    //    yaxis: {
    //        title: {
    //            text: 'Floors'
    //        }
    //    },
    //    fill: {
    //        opacity: 1
    //    },
    //    tooltip: {
    //        y: {
    //            formatter: function (val) {
    //                return "" + val + ""
    //            }
    //        }
    //    }
    //};

    //var chart = new ApexCharts(document.querySelector("#chart"), options);
    //chart.render();


});