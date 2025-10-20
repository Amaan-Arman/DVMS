var app = angular.module('Homeapp', []);
app.controller('companycontroller', function ($scope, $http, SignalRService) {
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

    $http.get(localStorage.getItem('URLIndex') + 'GetCompanyList').then(function (i) {
        debugger
        $scope.GetCompanyList = i.data;
    },
    function (error) {
        alert(error);
        $scope.GetCompanyList = error;
        });

    $scope.SetCompanyFunction = function (type) {
        debugger
        var companyName = $("#companyName").val();
        var company_floor = $("#company_floor").val();
        var companyemail = $("#companyemail").val();
        var companyphone = $("#companyphone").val();
        var Logo = $("#Logo")[0].files[0];

        
        if (!companyName) {
            showMessage("Name is required.");
            return;
        }
        else if (!company_floor) {
            showMessage("Email is required.");
            return;
        }
        else if (!companyemail) {
            showMessage("Phone is required.");
            return;
        }
        else if (!companyphone) {
            showMessage("CNIC is required.");
            return;
        }
        else if (!Logo) {
            showMessage("Logo is required.");
            return;
        }
        $('.page-loader-wrapper').fadeIn();
        if (type == "superadmin") {
            var companyID = companyName;
        } else {
            var companyID = "";
        }
        var formData = new FormData();
        formData.append("companyName", companyName);
        formData.append("floorno", $("#company_floor").find("option:selected").text());
        formData.append("email", companyemail);
        formData.append("phone", companyphone);
        formData.append("logo", Logo); 

        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'SetCompany',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result === "Saved") {
                    $('.page-loader-wrapper').fadeOut();
                    $("#companyName").get(0).value = "";
                    $("#company_floor").get(0).value = "";
                    $("#companyemail").get(0).value = "";
                    $("#companyphone").get(0).value = "";

                    $http.get(localStorage.getItem('URLIndex') + 'GetCompanyList').then(function (i) {
                        debugger
                        $scope.GetCompanyList = i.data;
                    },
                        function (error) {
                            alert(error);
                            $scope.GetCompanyList = error;
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