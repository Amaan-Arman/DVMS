var app = angular.module('Homeapp', []);
app.controller('Settingscontroller', function ($scope, $http, SignalRService) {

    SignalRService.init($scope); // Initialize SignalR

    localStorage.setItem('URLIndex', '/Login/')

    //$http.get(localStorage.getItem('URLIndex') + 'GetNotification').then(function (i) {
    //    $scope.GetNotification = i.data;
    //},
    //    function (error) {
    //        alert(error);
    //        $scope.GetNotification = error;
    //    });
    //$http.get(localStorage.getItem('URLIndex') + 'GetMessage').then(function (i) {
    //    $scope.GetMessage = i.data;
    //},
    //    function (error) {
    //        alert(error);
    //        $scope.GetMessage = error;
    //    });

    function showWithTitleMessage(msg) {
        swal("Alert !", msg);
    }

    //usercredential 
    $scope.MemberCredentialFunction = function () {
        $('.page-loader-wrapper').fadeIn();
        //debugger

        var user_name_id = $("#user_name_id").val();
        var email_id = $("#email_id").val();
        var login_id = $("#login_id").val();
        var password_id = $("#password_id").val();
        var user_mobileNo_id = $("#user_mobileNo_id").val();
        var loginType_id = $("#loginType_id").val();
        var pattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

        var emp_pic = $("#User_img").get(0).files;
        var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];

        if (user_name_id == "") {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("User Name is a Mandatory Requirement, Please Put it.. !");
        }
        else if (login_id == "") {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("Login ID is a Mandatory Requirement, Please Put it.. !");
        }
        //else if (password_id == "") {
        //                $('.page-loader-wrapper').fadeOut();
        //    showWithTitleMessage("Password is a Mandatory Requirement, Please Put it.. !");
        //}
        else if (email_id == "") {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("Email is a Mandatory Requirement, Please Put it.. !");
        }
        else if (!email_id.match(pattern)) {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("Enter Valid Email address.. !");
        }
        else if (user_mobileNo_id == "") {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("Phone is a Mandatory Requirement, Please Put it.. !");
        }
        else if (user_mobileNo_id.length != 11) {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("Enter Valid Phone number.. ! example 03052285844");
        }
        else if (loginType_id == "") {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("Login type is a Mandatory Requirement, Please Put it.. !");
        }
        else if (emp_pic.length == 0) {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("Please upload Image!");
        }
        else if ($.inArray($("#User_img").val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            $('.page-loader-wrapper').fadeOut();
            showWithTitleMessage("Invalid format!");
        }
        else {
            $('.page-loader-wrapper').fadeOut();
            var login_type = $("#loginType_id").find("option:selected").text();

            var data = new FormData;
            data.append("user_name", user_name_id);
            data.append("login_id", login_id);
            data.append("email", email_id);
            data.append("user_mobileNo", user_mobileNo_id);
            data.append("login_type_id", loginType_id);
            data.append("login_type", login_type);
            data.append("emp_pic", emp_pic[0]);

            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'setEditMemberCredential',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result == "Saved") {
                        //debugger
                        $('.page-loader-wrapper').fadeOut();
                        location.reload();
                        $http.get(localStorage.getItem('URLIndex') + 'GetMemberUserAccess').then(function (f) {
                            $scope.GetMemberUserAccess = f.data;
                        },
                            function (error) {
                                alert(error)
                                $scope.GetMemberUserAccess = error;
                            });
                    }
                    else if (result == "DataBaseError") {
                        $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Database Connectivity Error</strong>, Please check  application database connection...");

                    }
                    else if (result == "NetworkError") {
                        $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
                    }
                    else if (result == "ExceptionError") {
                        $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Exception Error</strong>, Please Check the Error Log...");
                    }
                    else {
                        $('.page-loader-wrapper').fadeOut();
                        alert("Session has been expired, Please login again...")
                    }
                },
                error: function (xhr, status, error) {
                    $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })
        }
    }
  
    $scope.RightsFunction = function (moduleID, column_name, btnid, userid) {
        //debugger
        var user = $(userid).val();
        if (user == "") {
            //$(errorid).html("User is a Mandatory Requirement, Please Put it.")
            //$(btnid).prop("checked", false);
            showWithTitleMessage("Please select User.. !");
        }
        else {
            //var a = $(btnid).is(':checked');
            //var result = 0;
            if (btnid == 0) {
                result = 1;
            }
            else if (btnid == 1) {
                result = 0;
            }
            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'Rights',
                data: { user: user, moduleID: moduleID, column_name: column_name, result: result },
                success: function (result) {
                    //debugger
                    if (result == 'Saved') {
                        //$http.get(localStorage.getItem('URLIndex') + "GetRight?filter_id=" + a).then(function (d) {
                        //    //debugger
                        //    $scope.GetRight = d.data;
                        //},
                        //function (error) {
                        //    $scope.GetRight = error;
                        //});
                    }
                    else {
                        alert(result);
                    }
                },
                error: function (xhr, status, error) {
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })
        }
    }
    $(function () {
        $("#user_id").change(function () {
            //debugger
            var a = $("#user_id").val();
            $http.get(localStorage.getItem('URLIndex') + "GetRight?filter_id=" + a).then(function (d) {
                //debugger
                $scope.GetRight = d.data;
            },
                function (error) {
                    $scope.GetRight = error;
                });
        })
    });


    //Delete Funtion
    $scope.UserDeleteFunction = function () {
        //debugger
        var ID = new Array();
        $("input[name='Checkbox']:checked").each(function () {
            ID.push($(this).val());
        });
        if (ID == "") {
            showWithTitleMessage("Please select User.. !");
        }
        else {
            showConfirmMessages(ID);
        }
    }
    function showConfirmMessages(ID) {
        swal({
            title: "Are you sure?",
            text: "You want to delete this User",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#dc3545",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        }, function () {
            //debugger

            var data = new FormData;
            for (var j = 0; j < ID.length; j++) {
                data.append("checkbox", ID[j]);
            }
            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'DeleteCredentialMember',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    $(".loader").hide();
                    //debugger
                    if (result == 'Saved') {
                        swal("Deleted!", "User has been deleted.", "success");

                        $http.get(localStorage.getItem('URLIndex') + 'GetMemberUserAccess').then(function (d) {
                            //debugger
                            $scope.GetMemberUserAccess = d.data;
                        },
                            function (error) {
                                alert(error)
                                $scope.GetMemberUserAccess = error;
                            });
                    }
                    else if (result == "DataBaseError") {
                        $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Database Connectivity Error</strong>, Please check  application database connection...");

                    }
                    else if (result == "NetworkError") {
                        $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
                    }
                    else if (result == "ExceptionError") {
                        $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Exception Error</strong>, Please Check the Error Log...");
                    }
                    else {
                        $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage(result);
                    }
                },
                error: function (xhr, status, error) {
                    $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            });

        });
    }
    //Delete Funtion
    $scope.SetNewPassword = function () {
        //debugger
        var currentPassword = $("#current").val();
        var NewPassword = $("#newpassword").val();
        var ConPassword = $("#connewpassword").val();
        if (currentPassword == "") {
            showWithTitleMessage("Enter Current Passowrd..!");
        }
        else if (NewPassword == "") {
            showWithTitleMessage("Enter New Passowrd..!");
        }
        else if (ConPassword == "") {
            showWithTitleMessage("Enter Confirm Passowrd..!");
        }
        else if (NewPassword != ConPassword) {
            showWithTitleMessage("Password do not match..!");
        }
        else {
            //debugger

            //function showConfirmMessage()
            (function () {
                swal({
                    title: "Are you sure?",
                    text: "You want to Change the Password",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#dc3545",
                    confirmButtonText: "Yes, change it!",
                    closeOnConfirm: false
                }, function () {

                    //debugger
                    var data = new FormData;
                    data.append("password", currentPassword);
                    data.append("NewPassword", NewPassword);
                    //debugger
                    $.ajax({
                        type: "POST",
                        url: localStorage.getItem('URLIndex') + 'SetChangePassword',
                        data: data,
                        contentType: false,
                        processData: false,
                        success: function (result) {
                            //debugger
                            if (result == "Saved") {
                                swal("changed!", "Your password has been change.", "success");
                                $("#current").get(0).value = "";
                                $("#newpassword").get(0).value = "";
                                $("#connewpassword").get(0).value = "";
                            }
                            else if (result == "passwordError") {
                                showWithTitleMessage("Invalid Current Password..!");
                            }
                            else if (result == "DataBaseError") {
                                showWithTitleMessage("Database Connectivity Error, Please check  application database connection...");
                            }
                            else if (result == "NetworkError") {
                                showWithTitleMessage(" Internet Connectivity Error, Please check the Internet connection...");
                            }
                            else if (result == "ExceptionError") {
                                showWithTitleMessage("Exception Error, Please Check the Error Log.....");
                            }
                            else {
                                showWithTitleMessage(result);
                            }
                        },
                        error: function (xhr, status, error) {
                            $(".imageLoadinng").hide();
                            var errorMessage = xhr.status + ': ' + xhr.statusText
                            alert('Error - ' + errorMessage);
                        }
                    })

                });
            }

            )();

        }
    }
    //usercredential


    //old not in use usercredential
    $scope.EditCredentialMemberFunction = function (ID, modelText, modelID) {
        //debugger
        $(".imageLoadinng").show();
        localStorage.setItem('memberupdate_ID', ID);
        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'EditMemberCredential',
            data: { ID: ID },
            success: function (result) {
                // //debugger
                $(".imageLoadinng").hide();

                $(modelText).html(result);
                $(modelID).modal('show');
            },
            error: function (xhr, status, error) {
                $(".imageLoadinng").hide();
                var errorMessage = xhr.status + ': ' + xhr.statusText
                alert('Error - ' + errorMessage);
            }
        })
    }
    $scope.SetEditCredentialMemberFunction = function (modelID) {
        $(".imageLoadinng").show();

        //debugger
        var user_credential_id = localStorage.getItem('memberupdate_ID');
        var user_name_id = $("#user_name_id_Edit").val();
        var login_id = $("#login_id_Edit").val();
        var email_id = $("#email_id_Edit").val();
        var user_mobileNo_id = $("#user_mobileNo_id_Edit").val();
        //var branch_id = $("#branch_id_Edit").val();
        var loginType_id = $("#loginType_id_Edit").val();
        //var department_id = $("#department_id_Edit").val();
        var pattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

        if (user_name_id == "") {
            $(".imageLoadinng").hide();
            //$(modelText).html("<strong>User Name</strong> is a Mandatory Requirement, Please Put it..");
            //$(modelID).modal('show');
            $("#user_name_id_Edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $("#user_name_id_errormessage_Edit").html("<strong>User Name</strong> is a Mandatory Requirement, Please Put it");
        }
        else if (login_id == "") {
            $(".imageLoadinng").hide();
            //$(modelText).html("<strong>Login ID</strong> is a Mandatory Requirement, Please Put it..");
            //$(modelID).modal('show');
            $("#login_id_Edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $("#login_id_errormessage_Edit").html("<strong>Login Id</strong> is a Mandatory Requirement, Please Put it");
        }

        else if (email_id == "") {
            $(".imageLoadinng").hide();
            //$(modelText).html("<strong>Email</strong> is a Mandatory Requirement, Please Put it..");
            //$(modelID).modal('show');
            $("#email_id_Edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $("#email_id_errormessage_Edit").html("<strong>Email</strong> is a Mandatory Requirement, Please Put it");
        }
        else if (!email_id.match(pattern)) {
            $(".imageLoadinng").hide();
            //$(modelText).html("<strong>Email</strong> is a Mandatory Requirement, Please Put it..");
            //$(modelID).modal('show');
            $("#email_id_Edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $("#email_id_errormessage_Edit").html("Invalid email format...");
        }
        else if (user_mobileNo_id == "") {
            $(".imageLoadinng").hide();
            //$(modelText).html("<strong>Mobile #</strong> is a Mandatory Requirement, Please Put it..");
            //$(modelID).modal('show');
            $("#user_mobileNo_id_Edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $("#user_mobileNo_id_errormessage_Edit").html("<strong>Mobile #</strong> is a Mandatory Requirement, Please Put it");
        }
        else if (user_mobileNo_id.length != 11) {
            $(".imageLoadinng").hide();
            //$(modelText).html("<strong>Mobile #</strong> is a Mandatory Requirement, Please Put it..");
            //$(modelID).modal('show');
            $("#user_mobileNo_id_Edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $("#user_mobileNo_id_errormessage_Edit").html("<strong>11 Digit Mobile #</strong> is a Mandatory Requirement, Please Put it");
        }
        else if (loginType_id == "") {
            $(".imageLoadinng").hide();
            //$(modelText).html("<strong>Login Type</strong> is a Mandatory Requirement, Please Put it..");
            //$(modelID).modal('show');
            $("#loginType_id_Edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $("#loginType_id_errormessage_Edit").html("<strong>Login Type</strong> is a Mandatory Requirement, Please Put it");
        }
        else {
            //debugger
            var login_type = $("#loginType_id_Edit").find("option:selected").text();

            var data = new FormData;
            data.append("user_credential_id", user_credential_id);
            data.append("user_name", user_name_id);
            data.append("login_id", login_id);
            data.append("email", email_id);
            data.append("user_mobileNo", user_mobileNo_id);
            data.append("login_type", login_type);
            data.append("login_type_id", loginType_id);
            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'SetEditMemberCredential',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result == "Saved") {
                        $(".imageLoadinng").hide();
                        //$(modelID).modal('show');
                        $(modelID).modal('hide');
                        $http.get(localStorage.getItem('URLIndex') + 'GetMemberUserAccess').then(function (f) {
                            //debugger
                            $scope.GetMemberUserAccess = f.data;
                        },
                            function (error) {
                                alert(error)
                                $scope.GetMemberUserAccess = error;
                            });
                        //$(modelID).modal('hide');
                    }
                    else {
                        $(".imageLoadinng").hide();
                        //location.reload();
                        alert("Session has been expired, Please login again...")
                    }
                },
                error: function (xhr, status, error) {
                    $(".imageLoadinng").hide();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })

        }

    }
    $scope.DeleteApprovalMemberFunction = function (ID, name, modelText, modelID) {
        //debugger

        $(".imageLoadinng").show();
        localStorage.setItem('UserCredentialID_DeleteID', ID);

        $(modelText).html("Are you sure ? do you want to delete <strong>" + name + "</strong> user...");
        $(modelID).modal('show');
        $(".imageLoadinng").hide();
        //var ID = $(id).val();
    }
    $scope.DeleteComfirmMemberFunction = function (modelID) {
        //debugger
        //  $(".loader").show();
        $(".imageLoadinng").show();
        var ID = localStorage.getItem('UserCredentialID_DeleteID');
        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'DeleteCredentialMember',
            data: { ID: ID },
            success: function (result) {
                $(".imageLoadinng").hide();
                if (result == "Saved") {
                    ////debugger
                    //$(modelText).html(result);
                    $(modelID).modal('hide');
                    $(".imageLoadinng").hide();

                    $http.get(localStorage.getItem('URLIndex') + 'GetMemberUserAccess').then(function (f) {
                        //debugger
                        $scope.GetMemberUserAccess = f.data;
                    },
                        function (error) {
                            alert(error)
                            $scope.GetMemberUserAccess = error;
                        });
                }
                else {
                    $(".imageLoadinng").hide();
                    alert("Session has been expired, Please login again...")
                    //location.reload();
                }
            },
            error: function (xhr, status, error) {
                $(".imageLoadinng").hide();
                var errorMessage = xhr.status + ': ' + xhr.statusText
                alert('Error - ' + errorMessage);
            }
        })

    }
    $scope.PasswordResentFunction = function (formID, modelID, modelText) {

        $(".imageLoadinng").show();

        //debugger

        var user_credential_id = $("#user_credential_id").val();
        if (user_credential_id == "") {
            $(".imageLoadinng").hide();
            $(modelText).html("<strong>User </strong> is a Mandatory Requirement, Please Put it..");
            $(modelID).modal('show');
            $("#user_credential_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (user_credential_id == null) {
            $(".imageLoadinng").hide();
            $(modelText).html("<strong>User</strong> is a Mandatory Requirement, Please Put it..");
            $(modelID).modal('show');
            $("#user_credential_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }

        else {
            //debugger
            // var clientName = $("#user_credential_id").find("option:selected").text();
            var data = new FormData;
            data.append("user_credential_id", user_credential_id);
            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'setPasswordResent',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result == "Saved") {
                        $("#user_credential_id")[0].value = 0;
                        //$("#PasswordResetSucessfullyModelID").modal('show');
                        $("#ChangeSucessfullyModelID").modal('show');
                        $(".imageLoadinng").hide();
                    }
                    else {
                        $(".imageLoadinng").hide();
                        //location.reload();
                        alert("Session has been expired, Please login again...")
                    }
                },
                error: function (xhr, status, error) {
                    $(".imageLoadinng").hide();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })

        }
    }
    $scope.ChangePicuteFunction = function () {

        $(".imageLoadinng").show();

        //debugger
        var files = $("#Employee_Picture_id").get(0).files;

        if (files.length == 0) {
            $(".imageLoadinng").hide();
            $(modelText).html("<strong>Picture</strong> is a Mandatory Requirement, Please Put it..");
            $(modelID).modal('show');
        }
        else {
            var data = new FormData;
            for (var i = 0; i < files.length; i++) {
                data.append("emp_pic", files[i]);
            }
            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'setChangePicture',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result == "Saved") {
                        //debugger
                        $(".imageLoadinng").hide();
                        location.reload();
                        //$("#ChangeSucessfullyModelID").modal('show');
                    }
                    else {
                        $(".imageLoadinng").hide();
                        //location.reload();
                        alert("Session has been expired, Please login again...")
                    }
                },
                error: function (xhr, status, error) {
                    $(".imageLoadinng").hide();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })

        }
    }
    //old not in use usercredential
});




