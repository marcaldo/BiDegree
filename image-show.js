var imageInterop = imageInterop || {};

var queue = [];
var dotNetObjRef;
var displayTime;

imageInterop.runQueue = function (displayQueue, interval, netObjRef) {
    dotNetObjRef = netObjRef;
    queue = displayQueue;
    displayTime = interval;

    console.table(queue);

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

        setInterval(this.transition, displayTime);

    }, 500);

}

imageInterop.transition = function () {
    const delayToLoad = 3000;

    if (queue.length === 0) {
        setTimeout(() => {
            window.location = "/photos/";
            //dotNetObjRef.invokeMethodAsync("Reload");
        }, displayTime);
    }

    if (imgTop.className === "transparent") {

        imgBottom.className = "transparent";
        imgTop.className = "";

        setTimeout(() => {
            if (queue.length > 0) {
                imgBottom.setAttribute("src", queue[0].sourceUrl);
                queue.splice(0, 1);
            }
        }, delayToLoad);

    }
    else {

        imgTop.className = "transparent";
        imgBottom.className = "";

        setTimeout(() => {
            if (queue.length > 0) {
                imgTop.setAttribute("src", queue[0].sourceUrl);
                queue.splice(0, 1);
            }
        }, delayToLoad);
    }


}
