// Define AngularJS module
var app = angular.module('ScannerApp', []);
// Define AngularJS controller
app.controller('QRScannerController', function ($scope, $http, SignalRService, $timeout) {
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

    //InsertVisitor
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
    //visitor scanning
    $scope.OpenCameraFront = async function () {
        // Show Bootstrap modal
        $("#modalcamerafront").modal('show');

        // Angular-friendly delay instead of setTimeout
        await $timeout(150);

        // Start camera
        await $scope.startCamera();

        // When video metadata is loaded
        video.onloadedmetadata = function () {
            scanning = true;
            captureBtn.disabled = false;
            $scope.startRectDetection();   // restart rectangle detection
        };
    };


    // -----------------------------
    // DOM Elements
    // -----------------------------
    const video = document.getElementById('video');
    const captureBtn = document.getElementById('captureBtn');
    const previewModal = document.getElementById('previewModal');
    const previewImg = document.getElementById('previewImg');
    const retakeBtn = document.getElementById('retakeBtn');
    const confirmBtn = document.getElementById('confirmBtn');
    const flashBtn = document.getElementById('flashBtn');
    const stopBtn = document.getElementById('stopBtn');
    const procCanvas = document.getElementById('procCanvas');
    const cropCanvas = document.getElementById('cropCanvas');
    const ocrText = document.getElementById('ocrText');

    // -----------------------------
    // Variables
    // -----------------------------
    let stream = null;
    let track = null;
    let scanning = false;
    let videoDevices = [];
    let currentCamIndex = 0;
    let usingFrontCam = false;
    let cvReady = false;
    const STABLE_THRESHOLD = 12;
    let stableCount = 0;
    let lastFound = false;

    // -----------------------------
    // Confirm → Run OCR
    // -----------------------------
    $scope.confirm = async function () {
        previewModal.style.display = "none";
        $("#modalcamerafront").modal('hide');

        const imgData = cropCanvas.toDataURL("image/png");

        snapshot.src = imgData;
        snapshot.style.display = 'block';

        $scope.stopCamera();

        localStorage.setItem('cnicfront', imgData)
    };

    // -----------------------------
    // Initialize
    // -----------------------------
    $scope.init = function () {
        $scope.startDisabled = true;
        //$scope.captureDisabled = true;

        loadCameraDevices();
        waitForCvReady();
    };


    // -----------------------------
    // Load camera list
    // -----------------------------
    function loadCameraDevices() {
        navigator.mediaDevices.enumerateDevices().then(devices => {
            videoDevices = devices.filter(d => d.kind === "videoinput");
            if (videoDevices.length === 0) alert("No camera found!");
        });
    }

    // -----------------------------
    // Start camera
    // -----------------------------
    $scope.startCamera = async function () {
        try {
            if (videoDevices.length === 0) loadCameraDevices();
            debugger
            const deviceInfo = videoDevices[currentCamIndex];
            const deviceId = deviceInfo?.deviceId;

            usingFrontCam = deviceInfo?.label.toLowerCase().includes("front") || deviceInfo?.label.toLowerCase().includes("fixed");

            video.style.transform = usingFrontCam ? "scaleX(-1)" : "scaleX(1)";

            stream = await navigator.mediaDevices.getUserMedia({
                video: deviceId ? { deviceId: { exact: deviceId } } : { facingMode: "environment" }
            });

            video.srcObject = stream;
            track = stream.getVideoTracks()[0];

            await new Promise(res => {
                if (video.readyState >= 2) res();
                else video.onloadedmetadata = () => res();
            });

            // Give camera time to auto-focus
            await new Promise(res => setTimeout(res, 800));

            // Turn on continuous focus if supported
            try {
                await track.applyConstraints({ advanced: [{ focusMode: "continuous" }] });
            } catch (e) {
                console.error("errror in camera focusMode",e);
            }

            scanning = true;
            //$scope.captureDisabled = false;
            $scope.startDisabled = true;

            startRectDetection();
        }
        catch (e) {
            console.error(e);
            alert("Camera access denied.");
        }
    };

    // -----------------------------
    // Stop camera
    // -----------------------------
    $scope.stopCamera = function () {
        if (stream) {
            stream.getTracks().forEach(t => t.stop());
        }
        scanning = false;
        stream = null;
        track = null;
        video.pause();

        $scope.startDisabled = false;
        //$scope.captureDisabled = true;
    };

    // -----------------------------
    // Switch camera
    // -----------------------------
    $scope.switchCamera = async function () {
        if (videoDevices.length <= 1) {
            return alert("No other cameras found.");
        }
        $scope.stopCamera();

        currentCamIndex = (currentCamIndex + 1) % videoDevices.length;
        await $scope.startCamera();
    };

    // -----------------------------
    // Toggle Flash
    // -----------------------------
    $scope.toggleFlash = async function () {
        if (!track) return alert("Start camera first");

        const caps = track.getCapabilities();
        if (!caps.torch) return alert("Flash not supported");

        const torchState = !flashBtn.classList.contains("on");
        flashBtn.classList.toggle("on");

        await track.applyConstraints({
            advanced: [{ torch: torchState }]
        });

        flashBtn.textContent = torchState ? "⚡ On" : "⚡ Flash";
    };

    // -----------------------------
    // Manual Capture
    // -----------------------------
    $scope.capture = function () {
        if (!scanning) return;
        doCapture();
    };

    // capture on-demand (manual)
    function doCapture() {
        const crop = getOverlayCrop();

        cropCanvas.width = crop.w;
        cropCanvas.height = crop.h;

        const ctx = cropCanvas.getContext("2d");

        ctx.drawImage(
            video,
            crop.x, crop.y, crop.w, crop.h,
            0, 0, crop.w, crop.h
        );

        previewImg.src = cropCanvas.toDataURL("image/png");
        previewModal.style.display = "flex";

        scanning = false;
    }

    // -----------------------------
    // Preview → Retake
    // -----------------------------
    $scope.retake = function () {
        previewModal.style.display = 'none';
        scanning = true;
        if (cvReady) startRectDetection();
    };

    // -----------------------------
    // Rectangle Detection Loop
    // -----------------------------
    function startRectDetection() {
        if (!cvReady) return;

        stableCount = 0;
        lastFound = false;

        (function detectLoop() {
            if (!scanning) return;

            try {
                const src = videoFrameToMat();
                const gray = new cv.Mat();
                const blurred = new cv.Mat();
                const edges = new cv.Mat();

                cv.cvtColor(src, gray, cv.COLOR_RGBA2GRAY);
                cv.GaussianBlur(gray, blurred, new cv.Size(5, 5), 0);
                cv.Canny(blurred, edges, 60, 180);

                const contours = new cv.MatVector();
                const hierarchy = new cv.Mat();
                cv.findContours(edges, contours, hierarchy, cv.RETR_LIST, cv.CHAIN_APPROX_SIMPLE);

                let best = null;
                let maxArea = 0;

                for (let i = 0; i < contours.size(); i++) {
                    const cnt = contours.get(i);
                    const peri = cv.arcLength(cnt, true);
                    const approx = new cv.Mat();
                    cv.approxPolyDP(cnt, approx, 0.02 * peri, true);

                    const area = cv.contourArea(approx);
                    if (approx.rows === 4 && area > 15000 && area > maxArea) {
                        best = approx.clone();
                        maxArea = area;
                    }
                    approx.delete();
                    cnt.delete();
                }

                if (best) {
                    stableCount++;
                    if (stableCount >= STABLE_THRESHOLD) {
                        scanning = false;
                        performPerspectiveCrop(src, best);
                    }
                    best.delete();
                } else {
                    stableCount = 0;
                }

                // cleanup
                src.delete(); gray.delete(); blurred.delete(); edges.delete();
                contours.delete(); hierarchy.delete();
            }
            catch (err) {
                console.error(err);
            }

            if (scanning) requestAnimationFrame(detectLoop);
        })();
    }

    // -----------------------------
    // Crop & Show Preview
    // -----------------------------
    function performPerspectiveCrop(srcMat, contourMat) {
        try {
            const pts = [];
            for (let i = 0; i < contourMat.rows; i++) {
                const p = contourMat.intPtr(i, 0);
                pts.push({ x: p[0], y: p[1] });
            }

            const ordered = orderPoints(pts);

            const width = Math.max(
                dist(ordered.tl, ordered.tr),
                dist(ordered.bl, ordered.br)
            );

            const height = Math.max(
                dist(ordered.tr, ordered.br),
                dist(ordered.tl, ordered.bl)
            );

            const srcTri = cv.matFromArray(4, 1, cv.CV_32FC2, [
                ordered.tl.x, ordered.tl.y,
                ordered.tr.x, ordered.tr.y,
                ordered.br.x, ordered.br.y,
                ordered.bl.x, ordered.bl.y
            ]);

            const dstTri = cv.matFromArray(4, 1, cv.CV_32FC2, [
                0, 0,
                width, 0,
                width, height,
                0, height
            ]);

            const M = cv.getPerspectiveTransform(srcTri, dstTri);
            const dst = new cv.Mat();

            cv.warpPerspective(srcMat, dst, M, new cv.Size(width, height));
            if (dst.rows > dst.cols) {
                cv.rotate(dst, dst, cv.ROTATE_90_CLOCKWISE);
            }

            cv.imshow(cropCanvas, dst);
            previewImg.src = cropCanvas.toDataURL("image/png");
            previewModal.style.display = "flex";

            srcTri.delete(); dstTri.delete(); M.delete(); dst.delete();
        }
        catch (err) {
            console.error(err);
            doCapture();
        }
    }

    // -----------------------------
    // Utility functions
    // -----------------------------
    function dist(a, b) {
        return Math.hypot(a.x - b.x, a.y - b.y);
    }

    function orderPoints(pts) {
        const sum = pts.map(p => p.x + p.y);
        const diff = pts.map(p => p.x - p.y);

        return {
            tl: pts[sum.indexOf(Math.min(...sum))],
            br: pts[sum.indexOf(Math.max(...sum))],
            tr: pts[diff.indexOf(Math.min(...diff))],
            bl: pts[diff.indexOf(Math.max(...diff))]
        };
    }

    function videoFrameToMat() {
        const crop = getOverlayCrop();
        procCanvas.width = crop.w;
        procCanvas.height = crop.h;

        const ctx = procCanvas.getContext("2d");

        ctx.translate(crop.w, 0);
        ctx.scale(-1, 1);
        ctx.drawImage(video, crop.x, crop.y, crop.w, crop.h, 0, 0, crop.w, crop.h);
        ctx.setTransform(1, 0, 0, 1, 0, 0);

        return cv.imread(procCanvas);
    }

    function getOverlayCrop() {
        const box = document.getElementById("overlayBox").getBoundingClientRect();
        const rect = video.getBoundingClientRect();

        const scaleX = video.videoWidth / rect.width;
        const scaleY = video.videoHeight / rect.height;

        return {
            x: (box.left - rect.left) * scaleX,
            y: (box.top - rect.top) * scaleY,
            w: box.width * scaleX,
            h: box.height * scaleY
        };
    }

    // -----------------------------
    // Wait for OpenCV
    // -----------------------------
    function waitForCvReady() {
        if (typeof cv === "undefined") {
            return setTimeout(waitForCvReady, 200);
        }

        cv.onRuntimeInitialized = function () {
            cvReady = true;
            $scope.startDisabled = true;
            $scope.$apply();
            console.log("OpenCV Ready");
        };
    }

    // Init
    $scope.init();
});