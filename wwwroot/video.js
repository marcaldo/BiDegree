var video = document.getElementsByTagName("video");

function playVideo()
{
    setTimeout(function () {
        video[0].muted = true;
        video[0].play();
    }, 1000);
};