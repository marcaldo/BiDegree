var queue;

function runQueue(displayQueue, displayTime) {
    queue = displayQueue;

    imgTop = document.getElementById("imgTop");
    imgBottom = document.getElementById("imgBottom");

    imgTop.setAttribute("style", "display:none");
    imgBottom.setAttribute("style", "display:none");
    imgBottom.className = "transparent";

    imgTop.setAttribute("src", queue[0].sourceUrl);

    imgBottom.setAttribute("src", queue[1].sourceUrl);

    setTimeout(() => {

        imgTop.setAttribute("style", "display:''");
        imgBottom.setAttribute("style", "display:''");

        queue.splice(0, 2);

        setInterval(transition, displayTime);

    }, 500);

}

function transition() {
    const loadingDelay = 4000;

    queue.splice(0, 1);

    if (imgTop.className === "transparent") {

        imgBottom.className = "transparent";
        imgTop.className = "";

        setTimeout(() => {
            imgBottom.setAttribute("src", queue[0].sourceUrl);
        }, loadingDelay);

    }
    else {

        imgTop.className = "transparent";
        imgBottom.className = "";

        setTimeout(() => {
            imgTop.setAttribute("src", queue[0].sourceUrl);
        }, loadingDelay);
    }
}
