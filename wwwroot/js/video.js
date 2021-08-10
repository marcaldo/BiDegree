function playVideo(v, start) {
    var video = document.getElementById(v);

    video.load();

    video.muted = start;

    if (start) {
        video.play();
    }
};