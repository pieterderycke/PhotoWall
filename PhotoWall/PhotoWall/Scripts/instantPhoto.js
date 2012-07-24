/// <reference path="jquery-1.7.2.js" />
/// <reference path="jquery.webcam.js" />
/// <reference path="knockout-2.1.0.debug.js" />

$(function () {
    var isDialogFirstTime = true;

    $('#videoWallOptions').dialog({
        width: 500,
        draggable: false,
        resizable: false,
        autoOpen: false,
        modal: true,
        open: function (event, ui) {
            if (isDialogFirstTime) {
                $('video').webcam({
                    onError: function (e) {
                        alert('Access Denied by the user!');
                    }
                });

                isDialogFirstTime = false;
            }
        }
    });

    // Create the MVVM Model
    function VideoWallViewModel() {
        var self = this;

        self.users = ko.observableArray();
        self.handleAfterUserAdded = function (element, data) {
            $(element).hoverMorph();

        };
    };

    ko.applyBindings(new VideoWallViewModel());

    // Create the video hub
    var videoHub = $.connection.videoHub;

    videoHub.display = function (userName, imageUrl) {
        var viewModel = ko.contextFor($('#videoWall')[0]).$root;

        var index = findUserIndex(viewModel, userName);

        if (index != undefined) {
            // Refresh the corresponding video wall item
            var videoWallItem = $('.videoWallItem img').eq(index);
            videoWallItem.attr('src', videoWallItem.attr('src') + '&timeStamp=' + new Date().getTime());
        }
        else {
            // New user
            viewModel.users.push({ name: userName, image: imageUrl });
        }
    };

    function findUserIndex(viewModel, userName) {
        for (var i = 0; i < viewModel.users().length; i++) {
            if (viewModel.users()[i].name == userName) {
                return i;
            }
        }
    }

    $.post("/Home/GetPhotos", undefined, function (data) {
        var viewModel = ko.contextFor($('#videoWall')[0]).$root;

        for (var i = 0; i < data.length; i++) {
            var photoInformation = data[i];

            viewModel.users.push({ name: photoInformation.UserName, image: photoInformation.PhotoUrl });
        }
    });

    $('#takePhoto').click(function () {
        sendSnapshot();
    });

    $('#joinVideoWall').click(function (e) {
        $('#videoWallOptions').dialog('open');

        e.preventDefault();
    });

    function sendSnapshot() {
        var photoCanvas = $('#photoCanvas')[0];
        var context = photoCanvas.getContext('2d');
        var video = $('video')[0];
        var userName = $('#userNameField').val();

        // Take a photo of the video stream
        photoCanvas.height = video.videoHeight;
        photoCanvas.width = video.videoWidth;
        context.drawImage(video, 0, 0);

        // Send to the backend
        videoHub.send(userName, photoCanvas.toDataURL('image/jpeg'));
    };

    // Start the connection
    $.connection.hub.start();
});