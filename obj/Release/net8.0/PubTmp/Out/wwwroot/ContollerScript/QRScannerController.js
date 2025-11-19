// Define AngularJS module
var app = angular.module('ScannerApp', []);
// Define AngularJS controller
app.controller('QRScannerController', function ($scope, $http, SignalRService) {
    SignalRService.init($scope); // Initialize SignalR

    localStorage.setItem('URLIndex', '/Admin/')

    localStorage.setItem('cnicfront', "");
    localStorage.setItem('cnicback', "");

    //$scope.qrCodeData = ''; // Initialize variable to store QR code data
    var scanner; // Declare scanner variable
    $scope.stopScanning = function () {
        scanner.stop(); // Stop scanning after successful scan
    };
    function showMessage(msg) {
        swal(msg);
    }
    function showSuccessMessage(msg,msg2) {
        swal(msg, msg2, "success");
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

    $scope.employees = [];
    $scope.loadEmployees = function () {
        if ($scope.selectedCompany) {
            $http.get(localStorage.getItem('URLIndex') + 'GetEmployeesByCompany', {
                params: { companyId: $scope.selectedCompany }
            }).then(function (response) {
                debugger
                $scope.employees = response.data;
            }, function (error) {
                console.error("Error fetching employees:", error);
            });
        } else {
            $scope.employees = [];
        }
    };


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
                    GuestFullName:GuestName,
                    VisitDate:DateTime,
                    Invitation_ID:invitationID
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
                        if (result == "Saved") {
                            showSuccessMessage("Welcome," + GuestName + "", "You will be attended by" + HostName + "");
                        }
                        else if (result == "Checkout") {
                            showSuccessMessage("Check Out Complete","Thanks for visiting");
                        }
                        else if (result == "Invitationexpired") {
                            showErrorMessage("Your invitation is has been expired :(");
                        }
                        else if (result == "InvalidInvitation") {
                            showErrorMessage("Invalid invitation :(");
                        }
                        else{
                            showErrorMessage("Error - "+result);
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

    $scope.stopCamera = function () {
        if (stream) {
            let tracks = stream.getTracks();
            tracks.forEach(track => track.stop());
            document.getElementById('video').srcObject = null;
            document.getElementById('videoback').srcObject = null;
        }
    };

    //$scope.OpenCamerafront = function () {
    //    $("#modalcamerafront").modal('show');

    //    navigator.mediaDevices.getUserMedia({ video: true })
    //        .then(function (mediaStream) {
    //            stream = mediaStream;
    //            const video = document.getElementById('video');
    //            video.srcObject = stream;

    //        })
    //        .catch(function (err) {
    //            alert("Error accessing webcam: " + err);
    //        });
  
    //};

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
    $scope.cnicfront = function () {
        debugger
        $('.page-loader-wrapper').fadeIn();
        const video = document.getElementById('video');
        const canvas = document.getElementById('canvas');
        const snapshot = document.getElementById('snapshot');

        //const context = canvas.getContext('2d');
        //// Mirror the image before drawing
        //context.translate(canvas.width, 0);
        //context.scale(-1, 1);
        //context.drawImage(video, 0, 0, canvas.width, canvas.height);
        canvas.getContext('2d').drawImage(video, 0, 0, canvas.width, canvas.height);

        $("#modalcamerafront").modal('hide');
        $("#btntakefront").hide();
        
        snapshot.style.display = 'block';

        const imageDataF = canvas.toDataURL('image/png');
        snapshot.src = imageDataF;
        snapshot.style.display = 'block';

        if (stream) {
            let tracks = stream.getTracks();
            tracks.forEach(track => track.stop());
            document.getElementById('video').srcObject = null;
        }
        localStorage.setItem('cnicfront', imageDataF)
    }
    $scope.cnicback = function () {
        debugger
        const videoback = document.getElementById('videoback');
        const canvasback = document.getElementById('canvasback');
        const snapshotback = document.getElementById('snapshotback');

        //const context = canvasback.getContext('2d');
        //// Mirror the image before drawing
        //context.translate(canvasback.width, 0);
        //context.scale(-1, 1);
        //context.drawImage(videoback, 0, 0, canvasback.width, canvasback.height);
        canvasback.getContext('2d').drawImage(videoback, 0, 0, canvasback.width, canvasback.height);

        $("#modalcameraback").modal('hide');
        $("#btntakecnicback").hide();

        snapshotback.style.display = 'block';

        const imageDataB = canvasback.toDataURL('image/png');
        snapshotback.src = imageDataB;
        snapshotback.style.display = 'block';

        if (stream) {
            let tracks = stream.getTracks();
            tracks.forEach(track => track.stop());
            document.getElementById('videoback').srcObject = null;
        }
        localStorage.setItem('cnicback', imageDataB)
    }
    $scope.InsertVisitor = function () {
        debugger
        var visitorName = $("#visitorName").val();
        var GenderID = $("#GenderID").val();
        var company_Id = $("#company_Id").val();
        var Department_Id = $("#Department_Id").val();
        var Employee_Id = $("#Employee_Id").val();
        var visitorPurpose = $("#visitorPurpose").val();

        if (localStorage.getItem('cnicfront') == "") {
            showMessage("cnic front is required.");
            return;
        } 
        //else if (localStorage.getItem('cnicback') == "") {
        //    showMessage("cnic back is required.");
        //    return;
        //} 
        else if (!visitorName) {
            showMessage("Name is required.");
            return;
        } else if (!GenderID) {
            showMessage("Email is required.");
            return;
        } else if (!company_Id) {
            showMessage("Phone is required.");
            return;
        } else if (!Department_Id) {
            showMessage("Phone is required.");
            return;
        } else if (!Employee_Id) {
            showMessage("Phone is required.");
            return;
        } else if (!visitorPurpose) {
            showMessage("CNIC is required.");
            return;
        }
        var data = {
            VisitorFullName: visitorName,
            Gender: $("#GenderID").find("option:selected").text(),
            Company_name: $("#company_Id").find("option:selected").text(),
            Department_name: $("#Department_Id").find("option:selected").text(),
            Employee_id: Employee_Id,
            Employee_name: $("#Employee_Id").find("option:selected").text(),
            VisitPurpose: visitorPurpose,
            CNICfront: localStorage.getItem('cnicfront'),
            //CNICback: localStorage.getItem('cnicback')
        };
        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'VisitorInsertion',
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (result) {
                debugger
                if (result === "Saved") {
                    $("#Visitor").modal('hide');
                    $('.page-loader-wrapper').fadeOut();

                    localStorage.setItem('cnicfront', "");
                    document.getElementById('snapshot').display = 'none';
                    //$("#btntakefront").show();

                    //localStorage.setItem('cnicback', "");
                    //document.getElementById('snapshotback').display = 'none';
                    //$("#btntakecnicback").show();

                    $("#visitorName").get(0).value = "";
                    $("#GenderID").get(0).value = "";
                    $("#company_Id").get(0).value = "";
                    $("#Department_Id").get(0).value = "";
                    $("#Employee_Id").get(0).value = "";
                    $("#visitorPurpose").get(0).value = "";
                    showSuccessMessage(" Registration Successful!", "Thank you for registering. Please wait while we notify the host of your arrival.");
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

    }


    let stream = null;
    let captured = false;
    let prevGray = null;
    let stableStart = null;

    let currentDeviceIndex = 0;
    let videoDevices = [];

    const video = document.getElementById("video");
    const overlay = document.getElementById("overlay");
    const ctx = overlay.getContext("2d");

    // Switch camera
    function switchCamera() {
        currentDeviceIndex = (currentDeviceIndex + 1) % videoDevices.length;
        startCamera(videoDevices[currentDeviceIndex].deviceId);
    }

    $scope.OpenCamerafront = function () {
        $("#modalcamerafront").modal('show');
        //video = document.getElementById('video');
        //overlay = document.getElementById("overlay");
        //ctx = overlay.getContext("2d");
        const switchBtn = document.getElementById('switchCamera');

        navigator.mediaDevices.enumerateDevices()
            .then(devices => {
                videoDevices = devices.filter(device => device.kind === 'videoinput');
                if (videoDevices.length > 0) {
                    startCamera(videoDevices[currentDeviceIndex].deviceId);
                }
                switchBtn.style.display = (videoDevices.length > 1) ? 'block' : 'none';
            });
    };

    // =========================
    // Camera start
    // =========================
    function startCamera(deviceId) {
        if (stream) {
            stream.getTracks().forEach(track => track.stop()); // stop old stream
        }

        navigator.mediaDevices.getUserMedia({
            video: { deviceId: { ideal: deviceId } }
        })
            .then(mediaStream => {
                stream = mediaStream;
                video.srcObject = stream;

                video.onloadedmetadata = () => {
                    const track = stream.getVideoTracks()[0];
                    const label = track.label.toLowerCase();

                    // flip only if front camera
                    if (label.includes("front") || label.includes("user") || label.includes("fixed")) {
                        video.style.transform = "scaleX(-1)";
                    } else {
                        video.style.transform = "scaleX(1)";
                    }

                    video.play();
                    drawGuide("red");        // initial guide frame
                };

                video.oncanplay = () => {
                    requestAnimationFrame(processFrame);
                };
            })
            .catch(err => {
                console.error("Error starting camera:", err.name, err.message);
                alert("Error starting camera: " + err.name + " - " + err.message);
            });
    }


    // =========================
    // Frame processing loop
    // =========================
    function processFrame() {
        requestAnimationFrame(processFrame);

        if (captured) return;
        if (video.videoWidth === 0 || video.videoHeight === 0) return;

        try {
            // ✅ Always use video.videoWidth / video.videoHeight
            let src = new cv.Mat(video.height, video.width, cv.CV_8UC4);
            let cap = new cv.VideoCapture(video);

            cap.read(src);

            // ✅ Sync overlay with video
            if (overlay.width !== video.width || overlay.height !== video.width) {
                overlay.width = video.width;
                overlay.height = video.height;
            }

            // Convert to grayscale
            let gray = new cv.Mat();
            cv.cvtColor(src, gray, cv.COLOR_RGBA2GRAY);

            // --- Quality & stability checks ---
            let sharpness = getSharpness(src);
            let motion = prevGray ? getMotion(prevGray, gray) : 1;
            let validShape = validateCNICShape(src);

            if (sharpness > 120 && motion < 0.02 && validShape) {
                if (!stableStart) stableStart = Date.now();

                if (Date.now() - stableStart > 1500) {
                    drawGuide("green");
                    showGuideMessage("✅ CNIC detected, capturing...", "lime");
                    captureCNIC(src);
                    captured = true;
                } else {
                    drawGuide("yellow");
                    showGuideMessage("⏳ Hold steady, CNIC detected", "orange");
                }
            } else {
                stableStart = null;
                drawGuide("red");

                if (!validShape) {
                    showGuideMessage("📷 Place CNIC fully inside the box", "yellow");
                } else if (sharpness <= 120) {
                    showGuideMessage("🔍 CNIC blurry, hold steady", "yellow");
                } else if (motion >= 0.02) {
                    showGuideMessage("✋ Don’t move CNIC", "yellow");
                }
            }

            // Store prevGray
            if (prevGray) prevGray.delete();
            prevGray = gray;

            src.delete();
        } catch (err) {
            console.error("processFrame error:", err);
        }
    }


    // =========================
    // Draw guide frame
    // =========================
    function drawGuide(color) {
        ctx.clearRect(0, 0, overlay.width, overlay.height);
        ctx.strokeStyle = color;
        ctx.lineWidth = 3;
        ctx.setLineDash([10, 6]);

        let { gx, gy, gw, gh } = getGuideBox();
        ctx.strokeRect(gx, gy, gw, gh);
    }
    function getGuideBox() {
        let gw = overlay.width * 0.65;
        let gh = overlay.height * 0.42;
        let gx = Math.max(0, (overlay.width - gw) / 2);
        let gy = Math.max(0, (overlay.height - gh) / 2);
        return { gx, gy, gw, gh };
    }

    // Utility: Sharpness check
    function getSharpness(mat) {
        let gray = new cv.Mat();
        cv.cvtColor(mat, gray, cv.COLOR_RGBA2GRAY, 0);
        let lap = new cv.Mat();
        cv.Laplacian(gray, lap, cv.CV_64F);
        let mean = new cv.Mat(), stddev = new cv.Mat();
        cv.meanStdDev(lap, mean, stddev);
        let variance = stddev.doubleAt(0, 0) ** 2;
        gray.delete(); lap.delete(); mean.delete(); stddev.delete();
        return variance; // >120 is sharp enough
    }
    function showGuideMessage(msg, color = "yellow") {
        let guideText = document.getElementById("guideText");
        guideText.innerText = msg;
        guideText.style.color = color;
    }

    // Utility: Motion check 
    function getMotion(prevGray, gray) {
        let diff = new cv.Mat();
        cv.absdiff(prevGray, gray, diff);
        let thresh = new cv.Mat();
        cv.threshold(diff, thresh, 25, 255, cv.THRESH_BINARY);
        let nonZero = cv.countNonZero(thresh);
        let ratio = nonZero / (gray.rows * gray.cols);
        diff.delete(); thresh.delete();
        return ratio; // <0.02 means stable 
    }
    // =========================
    // CNIC Shape validation
    // =========================
    function validateCNICShape(src) {
        try {
            let { gx, gy, gw, gh } = getGuideBox();

            // ✅ Ensure ROI is inside bounds
            if (gx < 0 || gy < 0 || gx + gw > src.cols || gy + gh > src.rows) {
                return false;
            }

            let roi = src.roi(new cv.Rect(gx, gy, gw, gh));

            let gray = new cv.Mat();
            cv.cvtColor(roi, gray, cv.COLOR_RGBA2GRAY, 0);
            cv.GaussianBlur(gray, gray, new cv.Size(5, 5), 0);

            let edges = new cv.Mat();
            cv.Canny(gray, edges, 75, 200);

            let contours = new cv.MatVector();
            let hierarchy = new cv.Mat();
            cv.findContours(edges, contours, hierarchy, cv.RETR_EXTERNAL, cv.CHAIN_APPROX_SIMPLE);

            let valid = false;
            for (let i = 0; i < contours.size(); i++) {
                let cnt = contours.get(i);
                let peri = cv.arcLength(cnt, true);
                let approx = new cv.Mat();
                cv.approxPolyDP(cnt, approx, 0.02 * peri, true);

                if (approx.rows === 4) { // ✅ Card = rectangle
                    let area = cv.contourArea(approx);
                    let rect = cv.boundingRect(approx);

                    // ✅ Area + aspect ratio check
                    let aspect = rect.width / rect.height;
                    if (area > 5000 && aspect > 1.3 && aspect < 1.8) {
                        valid = true;
                    }
                }

                approx.delete();
                cnt.delete();
            }

            // cleanup
            gray.delete(); edges.delete(); contours.delete(); hierarchy.delete(); roi.delete();
            return valid;
        } catch (err) {
            console.warn("validateCNICShape error:", err);
            return false;
        }
    }

    // =========================
    // Capture CNIC
    // =========================
    function captureCNIC(src) {
        let { gx, gy, gw, gh } = getGuideBox();

        // ✅ ROI check
        if (gx < 0 || gy < 0 || gx + gw > src.cols || gy + gh > src.rows) {
            console.warn("Capture skipped: ROI out of bounds");
            return;
        }
        let card = src.roi(new cv.Rect(gx, gy, gw, gh));

        let canvas = document.createElement("canvas");
        canvas.width = gw;
        canvas.height = gh;
        cv.imshow(canvas, card);

        let dataUrl = canvas.toDataURL("image/jpeg", 0.9);
        document.getElementById("snapshot").src = dataUrl;

        console.log("✅ CNIC captured:", dataUrl);
        $("#modalcamerafront").modal('hide');

        localStorage.setItem('cnicfront', dataUrl);

        if (stream) {
            let tracks = stream.getTracks();
            tracks.forEach(track => track.stop());
            document.getElementById('video').srcObject = null;
        }

        card.delete();
        setTimeout(() => {
            captured = false;   // restart after 2 seconds
            stableStart = null; // reset stability timer
        }, 2000);
    }

});