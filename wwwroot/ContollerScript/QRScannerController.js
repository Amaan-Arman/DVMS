// Define AngularJS module
var app = angular.module('ScannerApp', []);

// Define AngularJS controller
app.controller('QRScannerController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Admin/')

    //$scope.qrCodeData = ''; // Initialize variable to store QR code data
    var scanner; // Declare scanner variable

    //function showSuccessMessage(company_name,msg) {
    $scope.showSuccessMessage = function (company_name,msg) {
        swal(company_name, msg, "success");
    } 

    // Function to start scanning
    $scope.startScanning = function () {
        $("#staticBackdrop").modal('show');

        scanner = new Instascan.Scanner({ video: document.getElementById('video') });
        scanner.addListener('scan', function (content) {
            debugger
             //Split the string by delimiters to extract information
            var parts = content.split(":");
            var serialNumber = parts[2].split(";;")[0]; // Extract serial number
            var validate = parts[0].split(";;")[0]; // Extract serial number
            if (validate == "product_info") {
                localStorage.setItem('ProductserialNumber', serialNumber)

                $.ajax({
                    type: "POST",
                    url: localStorage.getItem('URLIndex') + 'ValidateGuest',
                    data: { ID: serialNumber },
                    success: function (result) {
                        debugger
                        scanner.stop();
                        if (result = "Success") {
                           
                        }
                        $("#ProductHeading").text('Product Detail');
                        $("#ProductDetailModalBodyId").html(result);
                        $("#ProductDetailModalID").modal('show');
                    },
                    error: function (xhr, status, error) {
                        scanner.stop(); 
                        var errorMessage = xhr.status + ': ' + xhr.statusText
                        alert('Error - ' + errorMessage);
                    }
                });
            } else {
                scanner.stop(); 
                alert('Invalid QR Code..!')
            }
           
            //$scope.$apply(function () {
            //    $scope.qrCodeData = serialNumber; // Update scope variable with scanned QR code data
            //});
            //scanner.stop(); // Stop scanning after successful scan

        });
        // Function to switch between cameras
        //$scope.switchCamera = function () {
        //    Instascan.Camera.getCameras().then(function (cameras) {
        //        if (cameras.length > 1) {
        //            // More than one camera is available
        //            // Switch to the next camera
        //            var cameraIndex = (scanner.cameraIndex + 1) % cameras.length;
        //            scanner.start(cameras[cameraIndex]); // Start the scanner with the new camera
        //        } else {
        //            console.error('Only one camera available.');
        //        }
        //    }).catch(function (e) {
        //        console.error(e);
        //    });
        //};

        var currentCameraIndex = 0; // Track the current camera index
        var cameras = [];
        Instascan.Camera.getCameras().then(function (availableCameras) {
            debugger
            if (availableCameras.length > 0) {
                cameras = availableCameras;

                // Automatically switch to a back camera (if available)
                cameras.forEach(function (camera, index) {
                    if (camera.name.toLowerCase().includes('back') || camera.name.toLowerCase().includes('environment')) {
                        currentCameraIndex = index;  // Set to back camera
                    }
                });

                scanner.start(cameras[currentCameraIndex]);
            } else {
                console.error('No cameras found.');
            }
        }).catch(function (e) {
            console.error(e);
        });
        $scope.switchCamera = function () {
            debugger
            if (cameras.length > 1) {
                currentCameraIndex = (currentCameraIndex + 1) % cameras.length;
                scanner.stop().then(function () {
                    scanner.start(cameras[currentCameraIndex]);
                }).catch(function (e) {
                    console.error('Error starting the camera: ', e);
                });
            } else {
                alert('No other camera available to switch.');
            }
        };
        //Instascan.Camera.getCameras().then(function (availableCameras) {
        //    if (availableCameras.length > 0) {
        //        cameras = availableCameras;
        //        scanner.start(cameras[currentCameraIndex]);
        //    } else {
        //        console.error('No cameras found.');
        //    }
        //}).catch(function (e) {
        //    console.error(e);
        //});

        //Instascan.Camera.getCameras().then(function (cameras) {
        //    if (cameras.length > 0) {
        //        scanner.start(cameras[1]); // Use the first available camera
        //    } else {
        //        //console.error('No cameras found.');
        //        alert('No cameras found');
        //    }
        //}).catch(function (e) {
        //    console.error(e);
        //});
    };

    $scope.stopScanning = function () {
        scanner.stop(); // Stop scanning after successful scan
    };

    $scope.StartInspectionFuntion = function () {
        debugger
        $('.page-loader-wrapper').fadeIn();

        window.open('Inspection_Form/' + localStorage.getItem('ProductserialNumber'), '_blank');
        window.close();

        $('.page-loader-wrapper').fadeOut();
    };

});



//product_info: S: 1213574956;
//var contentData = "WIFI:S:Zunairah wali;T:WPA;P:Zunairah wali;H:false;;";

// Split the string by delimiters to extract information
//var parts = content.split(":");
//var serialNumber = parts[2].split(";;")[0]; // Extract serial number