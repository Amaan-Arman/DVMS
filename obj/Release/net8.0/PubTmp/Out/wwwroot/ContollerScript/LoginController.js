var app = angular.module('Homeapp', []);
app.controller('LoginController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Login/')

    //$scope.Registerblock = false;
    //$scope.OTPblock = true;

    //$scope.StepOne = true;
    //$scope.StepTwo = false;
    //$scope.Stepthree = true;

    //if (localStorage.getItem('FromSubmit') == "True") {
    //    $scope.$applyAsync(() => {
    //        $scope.StepOne = true;
    //        $scope.StepTwo = true;
    //        $scope.Stepthree = false;
    //    });
    //} else {
    //    $scope.$applyAsync(() => {
    //        $scope.StepOne = false;
    //        $scope.StepTwo = true;
    //        $scope.Stepthree = true;
    //    });
    //}

    $scope.LoginFunction = function (formId) {
        var login_id = $("#login_id").val();
        var Password_id = $("#Password_id").val();

        if (login_id == "") {
            $("#login_id_errormessage").html("Login id is a Mandatory Requirement, Please Put it..");
            $("#login_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $(".imageLoadinng").hide();
        }
        else if (Password_id == "") {
            $("#Password_id_errormessage").html("Password is a Mandatory Requirement, Please Put it..");
            $("#Password_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $(".imageLoadinng").hide();
        }
        else {
            var data = $(formId).serialize();

            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'LoginAndPassword',
                data: data,
                success: function (result) {
                    debugger
                    if (result == "Invalid Login Id") {
                        $("#login_id_errormessage").html("Invalid Login Id");
                        $("#login_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
                    }
                    else if (result == "Invalid Password Id") {
                        $("#Password_id_errormessage").html("Invalid Password");
                        $("#Password_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
                    }
                    else if (result == "Invalid Login and Password Id") {
                        $("#login_id_errormessage").html("Invalid Login Id");
                        $("#login_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
                        $("#Password_id_errormessage").html("Invalid Password");
                        $("#Password_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
                    }
                    else {
                        window.location.href = "/Admin/"
                    }
                },
                error: function (xhr, status, error) {
                    //$(".imageLoadinng").hide();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })
        }
    }

    //$scope.RegistrationFunction = function () {
    //    var Name_id = $("#user_name_id").val();
    //    var email_id = $("#Email_id").val();
    //    var Password_id = $("#Password_id").val();

    //    if (Name_id == "") {
    //        $("#Name_id_errormessage").html("Login id is a Mandatory Requirement, Please Put it..");
    //        $("#user_name_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else if (email_id == "") {
    //        $("#Email_id_errormessage").html("Password is a Mandatory Requirement, Please Put it..");
    //        $("#Email_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else if (Password_id == "") {
    //        $("#Password_id_errormessage").html("Password is a Mandatory Requirement, Please Put it..");
    //        $("#Password_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else {
    //        localStorage.setItem('Name_id', Name_id);
    //        localStorage.setItem('email_id', email_id);
    //        localStorage.setItem('Password_id', Password_id);
    //        var data = new FormData;
    //        data.append("Email", email_id);
    //        $.ajax({
    //            type: "POST",
    //            url: localStorage.getItem('URLIndex') + 'SetUser',
    //            data: data,
    //            contentType: false,
    //            processData: false,
    //            success: function (result) {
    //                if (result == "EmailAlreadyExist") {
    //                    $("#Email_id_errormessage").html("Email Already Exist");
    //                }
    //                else if (result == "Saved") {
    //                    $scope.$applyAsync(() => {
    //                        $scope.Registerblock = true;
    //                        $scope.OTPblock = false;
    //                    });
    //                }
    //            },
    //            error: function (xhr, status, error) {
    //                var errorMessage = xhr.status + ': ' + xhr.statusText
    //                alert('Error - ' + errorMessage);
    //            }
    //        })
    //    }
    //}
    //$scope.OTPFunction = function () {
    //    debugger
    //    var OPT_id = $("#otp_id").val();
    //    var Name_id = localStorage.getItem('Name_id');
    //    var email_id = localStorage.getItem('email_id');
    //    var Password_id = localStorage.getItem('Password_id');
    //    if (OPT_id == "") {
    //        $("#otp_num_id_errormessage").html("OPT is a Mandatory Requirement, Please Put it..");
    //        $("#otp_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else {
    //        var data = new FormData;
    //        data.append("otp_num", OPT_id);
    //        data.append("user_name", Name_id);
    //        data.append("Email", email_id);
    //        data.append("Password", Password_id);
    //        $.ajax({
    //            type: "POST",
    //            url: localStorage.getItem('URLIndex') + 'OPTVerify',
    //            data: data,
    //            contentType: false,
    //            processData: false,
    //            success: function (result) {
    //                debugger
    //                if (result == "Invalid Otp Id") {
    //                    $("#login_id_errormessage").html("Invalid Otp");
    //                    $("#login_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //                }
    //                else if (result == "Admin") {
    //                    window.location.href = "/Admin/"
    //                }
    //                else if (result == "User") {
    //                    window.location.href = "/Home/"
    //                }
    //                else {
    //                    alert("this error" + result);
    //                }
    //            },
    //            error: function (xhr, status, error) {
    //                var errorMessage = xhr.status + ': ' + xhr.statusText
    //                alert('Error - ' + errorMessage);
    //            }
    //        })
    //    }
    //}

    //$scope.CheckEmailFunction = function () {
    //    debugger
    //    var Name_id = $("#Re_user_name_id").val();
    //    var phone_id = $("#Re_phone_id").val();
    //    var email_id = $("#Re_Email_id").val();
    //    var city_id = $("#CitySelect").val();
    //    var Password_id = $("#rePassword_id").val();
    //    if (Name_id == "") {
    //        $("#Name_id_errormessage").html("Name id is a Mandatory Requirement, Please Put it..");
    //        $("#Re_user_name_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else if (phone_id == "") {
    //        $("#Phone_id_errormessage").html("Phone is a Mandatory Requirement, Please Put it..");
    //        $("#Re_phone_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else if (email_id == "") {
    //        $("#Email_id_errormessage").html("Email is a Mandatory Requirement, Please Put it..");
    //        $("#Re_Email_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else if (city_id == "Choose...") {
    //        $("#City_id_errormessage").html("City is a Mandatory Requirement, Please Put it..");
    //        $("#Re_city_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else if (Password_id == "") {
    //        $("#Re_Password_id_errormessage").html("Password is a Mandatory Requirement, Please Put it..");
    //        $("#rePassword_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //        return;
    //    }
    //    else {
    //        var data = new FormData;
    //        data.append("Email", email_id);
    //        $.ajax({
    //            type: "POST",
    //            url: localStorage.getItem('URLIndex') + 'CheckEmail',
    //            data: data,
    //            contentType: false,
    //            processData: false,
    //            success: function (result) {
    //                if (result == "EmailAlreadyExist") {
    //                    $("#Email_id_errormessage").html("Email Already Exist");
    //                }
    //                else if (result == "EmailVerified") {
    //                    $scope.$applyAsync(() => {
    //                        $scope.StepOne = true;
    //                        $scope.StepTwo = false;
    //                    });
    //                }
    //                else {
    //                    Console.log(result);
    //                }
    //            },
    //            error: function (xhr, status, error) {
    //                var errorMessage = xhr.status + ': ' + xhr.statusText
    //                alert('Error - ' + errorMessage);
    //            }
    //        })
    //     }
    // }
    //$scope.WorkerRegistrationFunction = function () {
    //    var Name_id = $("#Re_user_name_id").val();
    //    var phone_id = $("#Re_phone_id").val();
    //    var email_id = $("#Re_Email_id").val();
    //    //var city_id = $("#Re_city_id").val();
    //    var User_img = $("#User_img").get(0).files;
    //    var Front_CNIC_id = $("#Front_CNIC").get(0).files;
    //    var Back_CNIC_id = $("#Back_CNIC").get(0).files;
    //    var fileExtension = ['jpeg', 'jpg', 'png','pdf'];
    //    var Password_id = $("#rePassword_id").val();

    //    if (User_img.length == 0) {
    //        $("#User_img-text-danger").html("Please upload Image!");
    //        return;
    //    }
    //    else if ($.inArray($("#User_img").val().split('.').pop().toLowerCase(), fileExtension) == -1) {
    //        $("#User_img-text-danger").html("Invalid format!");
    //        return;
    //    }
    //    else if (Front_CNIC_id.length == 0) {
    //        $("#Front-text-danger").html("Please upload Image!");
    //        return;
    //    }
    //    else if ($.inArray($("#Front_CNIC").val().split('.').pop().toLowerCase(), fileExtension) == -1) {
    //        $("#Front-text-danger").html("Invalid format!");
    //        return;
    //    }
    //    else if (Back_CNIC_id.length == 0) {
    //        $("#Back-text-danger").html("Please upload Image!");
    //        return;
    //    }
    //    else if ($.inArray($("#Back_CNIC").val().split('.').pop().toLowerCase(), fileExtension) == -1) {
    //        $("#Back-text-danger").html("Invalid format!");
    //        return;
    //    }
       
    //    else {
    //        var data = new FormData;
    //        var city_id = $("#CitySelect").find("option:selected").text();

    //        data.append("user_name", Name_id);
    //        data.append("user_mobileNo", phone_id);
    //        data.append("Email", email_id);
    //        data.append("city", city_id);
    //        data.append("user_pic", User_img[0]);
    //        data.append("front_cnic", Front_CNIC_id[0]);
    //        data.append("back_cnic", Back_CNIC_id[0]);
    //        data.append("Password", Password_id);
    //        $.ajax({
    //            type: "POST",
    //            url: localStorage.getItem('URLIndex') + 'SetWorkerRegistration',
    //            data: data,
    //            contentType: false,
    //            processData: false,
    //            success: function (result) {
    //                if (result == "Saved") {
    //                    localStorage.setItem('FromSubmit', 'True')
    //                    $scope.$applyAsync(() => {
    //                        $scope.StepOne = true;
    //                        $scope.StepTwo = true;
    //                        $scope.Stepthree = false;
    //                    });
    //                }
    //                else {
    //                    console.log(result);
    //                }
    //            },
    //            error: function (xhr, status, error) {
    //                var errorMessage = xhr.status + ': ' + xhr.statusText
    //                alert('Error - ' + errorMessage);
    //            }
    //        })
    //    }
    //}

});