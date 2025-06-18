// Define AngularJS module
var app = angular.module('ScannerApp', []);

// Define AngularJS controller
app.controller('QRScannerController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Admin/')

    //$scope.qrCodeData = ''; // Initialize variable to store QR code data
    var scanner; // Declare scanner variable
    $scope.stopScanning = function () {
        scanner.stop(); // Stop scanning after successful scan
    };
    function showSuccessMessage(msg,msg2) {
        swal(msg, msg2, "success");
    } 
    function showErrorMessage(msg) {
        swal("failed", msg, "error");
    } 
    // Function to start scanning
    $scope.startScanning = function () {
        $("#staticBackdrop").modal('show');
        scanner = new Instascan.Scanner({ video: document.getElementById('videoguest') });
        scanner.addListener('scan', function (content) {
            debugger
            //Split the string by delimiters to extract information
            var parts = content.split(",");
            var invitationID = parts[0].split(";")[1];
            var GuestName = parts[1].split(";")[1];
            var DateTime = parts[2].split(";")[1];
            var HostName = parts[3].split(";")[1];
            var validate = parts[0].split(";")[0];
            if (validate == "InvitationID") {
                //localStorage.setItem('ProductserialNumber', serialNumber)
                var data = {
                    GuestFullName: GuestName,
                    VisitDate: DateTime,
                    Invitation_ID: invitationID
                };
                $.ajax({
                    type: "POST",
                    url: localStorage.getItem('URLIndex') + 'ValidateInvitation',
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    success: function (result) {
                        debugger
                        $("#staticBackdrop").modal('hide');
                        scanner.stop();
                        if (result = "Saved") {
                            showSuccessMessage("Welcome," + GuestFullName + "", "You will be attended by" + HostName + "");
                        }
                        else if (result = "Invitationexpired") {
                            showErrorMessage("Your invitation is has been expired :(");
                        }
                        else if (result = "InvalidInvitation") {
                            showErrorMessage("Invalid invitation :(");
                        }
                    },
                    error: function (xhr, status, error) {
                        $("#staticBackdrop").modal('hide');
                        scanner.stop();
                        var errorMessage = xhr.status + ': ' + xhr.statusText
                        alert('Error - ' + errorMessage);
                    }
                });
            } else {
                $("#staticBackdrop").modal('hide');
                scanner.stop();
                showErrorMessage("Invalid invitation card..!");
            }
            //$scope.$apply(function () {
            //    $scope.qrCodeData = serialNumber; // Update scope variable with scanned QR code data
            //});
            //scanner.stop(); // Stop scanning after successful scan
        });



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
    };



    let stream = null;
    $scope.OpenCamerafront = function () {
        $("#modalcamerafront").modal('show');
      
        navigator.mediaDevices.getUserMedia({ video: true })
        .then(function (mediaStream) {
            stream = mediaStream;
            const video = document.getElementById('video');
            video.srcObject = stream;
        })
        .catch(function (err) {
            alert("Error accessing webcam: " + err);
        });
    };
    $scope.OpenCameraback = function () {
        $("#modalcameraback").modal('show');

        navigator.mediaDevices.getUserMedia({ video: true })
            .then(function (mediaStream) {
                stream = mediaStream;
                const videoback = document.getElementById('videoback');
                videoback.srcObject = stream;
            })
            .catch(function (err) {
                alert("Error accessing webcam: " + err);
            });
    };
    $scope.stopCamera= function () {
        if (stream) {
            let tracks = stream.getTracks();
            tracks.forEach(track => track.stop());
            document.getElementById('video').srcObject = null;
            document.getElementById('videoback').srcObject = null;
        }
    };
    $scope.cnicfront = function () {
        debugger
        const video = document.getElementById('video');
        const canvas = document.getElementById('canvas');
        const snapshot = document.getElementById('snapshot');

        const context = canvas.getContext('2d');
        // Mirror the image before drawing
        context.translate(canvas.width, 0);
        context.scale(-1, 1);
        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        $("#modalcamerafront").modal('hide');
        $("#btntakefront").hide();
        
        snapshot.style.display = 'block';

        const imageData = canvas.toDataURL('image/png');
        snapshot.src = imageData;
        snapshot.style.display = 'block';

        if (stream) {
            let tracks = stream.getTracks();
            tracks.forEach(track => track.stop());
            document.getElementById('video').srcObject = null;
        }

        // Send to server
        //fetch('/Photo/SaveIDImage', {
        //    method: 'POST',
        //    headers: { 'Content-Type': 'application/json' },
        //    body: JSON.stringify({ base64Image: imageData })
        //})
        //    .then(res => res.json())
        //    .then(data => {
        //        alert(data.status === "success" ? "Image saved!" : "Failed to save.");
        //});
    }
    $scope.cnicback = function () {
        debugger
        const videoback = document.getElementById('videoback');
        const canvasback = document.getElementById('canvasback');
        const snapshotback = document.getElementById('snapshotback');

        const context = canvasback.getContext('2d');
        // Mirror the image before drawing
        context.translate(canvasback.width, 0);
        context.scale(-1, 1);
        context.drawImage(videoback, 0, 0, canvasback.width, canvasback.height);

        $("#modalcameraback").modal('hide');
        $("#btntakecnicback").hide();

        snapshotback.style.display = 'block';

        const imageData = canvasback.toDataURL('image/png');
        snapshotback.src = imageData;
        snapshotback.style.display = 'block';

        if (stream) {
            let tracks = stream.getTracks();
            tracks.forEach(track => track.stop());
            document.getElementById('videoback').srcObject = null;
        }

        // Send to server
        //fetch('/Photo/SaveIDImage', {
        //    method: 'POST',
        //    headers: { 'Content-Type': 'application/json' },
        //    body: JSON.stringify({ base64Image: imageData })
        //})
        //    .then(res => res.json())
        //    .then(data => {
        //        alert(data.status === "success" ? "Image saved!" : "Failed to save.");
        //});


    }

});