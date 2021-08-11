function playVideo(v, start) {
    var video = document.getElementById(v);

    video.load();

    video.muted = start;

    if (start) {

        var playPromise = video.play();

        if (playPromise !== undefined) {
            playPromise.then(_ => {
            })
                .catch(error => {
                });
        }
    }
};

