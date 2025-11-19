var app = angular.module('Homeapp', []);
app.controller('InspectionController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Home/')

    //localStorage.setItem('Search', "done");

    //$http.get(localStorage.getItem('URLIndex') + 'GetProductList').then(function (i) {
    //    debugger
    //    $scope.GetProductList = i.data;
    //},
    //function (error) {
    //    alert(error);
    //    $scope.GetProductList = error;
    //});

    $http.get(localStorage.getItem('URLIndex') + 'GetQuestionList').then(function (i) {
        debugger
        $scope.GetQuestionList = i.data;

        // Initialize the current index
        $scope.currentIndex = 0;
        // Display the first col element
        displayNextCol();
    },
    function (error) {
        alert(error);
        $scope.GetQuestionList = error;
    });


    // Array to store question IDs and their respective responses
    var questionID = [];
    var reponce_arr = new Array();
    var Comment_arr = new Array();
    // Function to display the next col element


    var per = 0; 
    function displayNextCol() {
        debugger;
        // Check if index is within bounds
        if ($scope.currentIndex >= 0 && $scope.currentIndex < $scope.GetQuestionList.length) {

            let num = $scope.GetQuestionList.length;
            let progressvalue = 100 / num; 
            // Create HTML for the col element
            var html = `
            <div class="col">
                <h4 id="h2-container" class="my-4 mx-3 fw-bold text-center txt-color">Q: ${$scope.GetQuestionList[$scope.currentIndex].QuestionText}</h4>
                <div class="radio-tile-group">
                    <div class="input-container">
                        <input id="Not OK" type="radio" name="responce${$scope.GetQuestionList[$scope.currentIndex].QuestionID}" value="Not OK">
                        <div class="radio-tile">
                            <i class="fa fa-times-circle" aria-hidden="true"></i>
                            <label for="Not OK">Not OK</label>
                        </div>
                    </div>
                    <div class="input-container">
                        <input id="OK" type="radio" name="responce${$scope.GetQuestionList[$scope.currentIndex].QuestionID}" value="OK">
                        <div class="radio-tile">
                            <i class="fa fa-check-circle" aria-hidden="true"></i>
                            <label for="OK">OK</label>
                        </div>
                    </div>
                </div>
                   <div class="row d-flex justify-content-center ">
                      <div class="col-8 ">
                        <div class="form-group mt-2  ">
                            <label>Comment :</label>
                            <textarea rows="4" id="Comment${$scope.GetQuestionList[$scope.currentIndex].QuestionID}" class="form-control no-resize" placeholder="Please type what you want..."></textarea>
                        </div>
                     </div>
                  </div>
            </div>
        `;

            //var progresshtml = `
            // <div class="progress-bar" role="progressbar" aria-label="Example 2px high" style="width:${per}%;"
            //             aria-valuenow=" 25" aria-valuemin="0" aria-valuemax="100"></div>
            //`;
             var progresshtml = `
             <div class="progress-bar progress-bar-striped progress-bar-animated bg-success" style="width:${per}%;"></div>
            `;
            
            per +=  progressvalue;
            // Update the HTML content of the container
            document.getElementById("Qcontainer").innerHTML = html; 
            document.getElementById("progressbar").innerHTML = progresshtml; 

        } else {
            // No more data to display
            //alert("No more data available");

            // Save question IDs and their respective responses
            //saveResponses();

            localStorage.setItem('QID', JSON.stringify(questionID));
            localStorage.setItem('responce', JSON.stringify(reponce_arr));
            localStorage.setItem('Comments', JSON.stringify(Comment_arr));
            $scope.ResponceInsertionFunction();

        }
    }

    // Function to display the next col element
    $scope.nextCol = function () {
        debugger
        if ($("input[name='responce" + $scope.GetQuestionList[$scope.currentIndex].QuestionID + "']").is(':checked')) {

            questionID.push($scope.GetQuestionList[$scope.currentIndex].QuestionID);
            $("input[name='responce" + $scope.GetQuestionList[$scope.currentIndex].QuestionID + "']:checked").each(function () {
                reponce_arr.push($(this).val());
            });
            var comment = $("#Comment" + $scope.GetQuestionList[$scope.currentIndex].QuestionID).val();
            Comment_arr.push(comment);


            // Increment the index
            $scope.currentIndex++;
            // Display the next col element
            displayNextCol();
        }
        else {
            alert('Please Select option...');
        }
    };

    // Function to insert or update responses
    $scope.ResponceInsertionFunction = function () {
        debugger
        $('.page-loader-wrapper').fadeIn();

        var ServUrl = window.location.pathname;
        var urlsplit = ServUrl.split('/');
        var product_ID = urlsplit[3].replace(/-/g, ' ');

        //I correct questionID name to QID
        const QID = JSON.parse(localStorage.getItem('QID'));
        const ResponseID = JSON.parse(localStorage.getItem('responce'));
        const ResponseComments = JSON.parse(localStorage.getItem('Comments'));

        var data = new FormData;
      
        data.append("Q_SerialNumber", product_ID);

        var arrayLength = QID.length;
        for (var i = 0; i < arrayLength; i++) {
            data.append("Q_ResponseID", QID[i]);
        }
        var arrayLength = ResponseID.length;
        for (var i = 0; i < arrayLength; i++) {
            data.append("Response", ResponseID[i]);
        }
        var arrayLength = ResponseComments.length;
        for (var i = 0; i < arrayLength; i++) {
            data.append("Comment", ResponseComments[i]);
        }

        $.ajax({
            type: "Post",
            url: localStorage.getItem('URLIndex') + 'ResponseInsertionAndUpdation',
            data: data,
            contentType: false,
            processData: false,
            success: function (result) {
                debugger;
                $('.page-loader-wrapper').fadeOut();
                if (result == "Saved") {
                    $("#inspectionsaveHeading").text('Blaze Inspect Pro');
                    $("#inspectionsaveModalBodyId").html("Your Inpection Report has been saved ..!");
                    $("#inspectionsaveModalID").modal('show');
                    //alert("Your Inpection Report has been saved!..");
                    //window.close();
                } else {
                    alert(result);
                }
            },
            error: function (xhr, status, error) {
                        $('.page-loader-wrapper').fadeOut();
                var errorMessage = xhr.status + ': ' + xhr.statusText;
                alert('Error - ' + errorMessage);
            }
        });
    };


    //$scope.asdadaResponceInsertionFunction = function () {
    //    debugger
    //    var Question_ID = $("#Question_ID").val();

    //    var selectedValue = $('input[name="gender"]:checked').val();

    //    var test = new Array();
    //    $("input[name='Checkbox']:checked").each(function () {
    //        //var values = $(this).val();
    //        //test += values;
    //        test.push($(this).val());
    //    });
    //    //alert(test);
    //    localStorage.setItem('Email_DeleteID', JSON.stringify(test));

    //    const ID = JSON.parse(localStorage.getItem('Email_DeleteID'));

    //    if (Question_ID == "") {
    //        $("#SerialNumber_error_ID").html("<strong>Serial Number </strong> is a Mandatory, Please Put it..");
    //        $("#Question_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else if (Location_ID == "") {
    //        $("#Location_ID_error_id").html("<strong>Location </strong> is a Mandatory, Please Put it..");
    //        $("#Location_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
    //    }
    //    else {
    //        debugger
    //        var data = new FormData;

    //        var arrayLength = ID.length;
    //        for (var i = 0; i < arrayLength; i++) {
    //            data.append("checkbox", ID[i]);
    //        }

    //        $.ajax({
    //            type: "Post",
    //            url: localStorage.getItem('URLIndex') + 'ProductInsertionAndUpdation',
    //            data: data,
    //            contentType: false,
    //            processData: false,
    //            success: function (result) {
    //                debugger
    //                if (result == "Saved") {
    //                    $(".imageLoadinng").hide();
    //                    //location.reload(true);

    //                    $("#Remarks").get(0).value = "";

    //                }
    //                else {
    //                    //$(".imageLoadinng").hide();
    //                    //$("#EmptyModalID").mod
    //                    //$("#modeltextId").html(result);
    //                    //$("#EmptyModalID").modal('show');
    //                    alert(result);
    //                    //location.reload();
    //                }
    //            },
    //            error: function (xhr, status, error) {
    //                //$(".loader").hide();
    //                $(".imageLoadinng").hide();
    //                var errorMessage = xhr.status + ': ' + xhr.statusText
    //                alert('Error - ' + errorMessage);
    //            }
    //        })
    //    }
    //}


    //Product insert

});




