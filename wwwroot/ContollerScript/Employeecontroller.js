var app = angular.module('Homeapp', []);
app.controller('Employeecontroller', function ($scope, $http, SignalRService) {
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

    $http.get(localStorage.getItem('URLIndex') + 'GetEmployeeList').then(function (i) {
        debugger
        $scope.GetEmployeeList = i.data;
    },
    function (error) {
        alert(error);
        $scope.GetEmployeeList = error;
        });

    $scope.SetEmployeeFunction = function (type) {
        debugger
        var employee_LoginID = $("#employee_loginID").val();
        var employeeName = $("#employeeName").val();
        var employeeEmail = $("#employeeemail").val();
        var employeePhone = $("#employeephone").val();
        var companyName = $("#companyName").val();
        var employee_Department = $("#employee_department").val();
        var employeeRole = $("#employeeRole").val();

        if (!employee_LoginID) {
            showMessage("Name is required.");
            return;
        }
        else if (!employeeName) {
            showMessage("Email is required.");
            return;
        }
        else if (!employeeEmail) {
            showMessage("Phone is required.");
            return;
        }
        else if (!employeePhone) {
            showMessage("CNIC is required.");
            return;
        }
        else if (type == "superadmin" && !companyName) {
            showMessage("company is required.");
            return;
        }
        else if (!employee_Department) {
            showMessage("Department is required.");
            return;
        }
        else if (!employeeRole) {
            showMessage("Role is required.");
            return;
        }
        $('.page-loader-wrapper').fadeIn();
        if (type == "superadmin") {
            var companyID = companyName;
        } else {
            var companyID = "";
        }
        var data = {
            employee_loginID: employee_LoginID,
            employeeName: employeeName,
            email: employeeEmail,
            phone: employeePhone,
            department: $("#employee_department").find("option:selected").text(),
            role: $("#employeeRole").find("option:selected").text(),
            companyID: companyID
        };
        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'SetEmployee',
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (result) {
                if (result === "Saved") {
                    $('.page-loader-wrapper').fadeOut();
                    $("#employee_loginID").get(0).value = "";
                    $("#employeeName").get(0).value = "";
                    $("#employeeemail").get(0).value = "";
                    $("#employeephone").get(0).value = "";
                    $("#employee_department").get(0).value = "";
                    $("#employeeRole").get(0).value = "";

                    $http.get(localStorage.getItem('URLIndex') + 'GetEmployeeList').then(function (i) {
                        debugger
                        $scope.GetEmployeeList = i.data;
                    },
                        function (error) {
                            alert(error);
                            $scope.GetEmployeeList = error;
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

});