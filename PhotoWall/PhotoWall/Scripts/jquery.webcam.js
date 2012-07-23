/// <reference path="jquery-1.7.2.js" />

(function ($, undefined) {
    $.fn.webcam = function (options) {
        var settings = $.extend({
            onError: function (e) { }
        }, options || {});

        if (!hasSupportForUserMedia())
            throw "Your browser does not support accessing the webcam of the computer.";

        var videoElements = this;

        navigator.webkitGetUserMedia({ video: true, audio: true }, function (localMediaStream) {
            var videoUrl = window.webkitURL.createObjectURL(localMediaStream);

            $(videoElements).each(function () {
                var video = $(this)[0];
                video.src = videoUrl;

                // Note: onloadedmetadata doesn't fire in Chrome when using it with getUserMedia.
                // See crbug.com/110938.     
                video.onloadedmetadata = function (e) {
                    // Ready to go. Do some stuff.
                };
            });
        }, settings.onError);
    };

    function hasSupportForUserMedia() {
        return navigator.webkitGetUserMedia != undefined || navigator.getUserMedia != undefined;
    }
})(jQuery);