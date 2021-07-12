var imageInterop = imageInterop || {};

var queue = [];
var dotNetObjRef;
var displayTime;

const fit_cover = "cover";
const fit_contain = "contain";

imageInterop.runQueue = function (displayQueue, interval, netObjRef) {
    dotNetObjRef = netObjRef;
    displayTime = interval;
    queue = displayQueue;

    //if (queue.length === 1) {
    //    queue.push(queue[0]);
    //}

    console.table(queue);

    let imgTop = document.getElementById("imgTop");
    let imgBottom = document.getElementById("imgBottom");

    imgTop.setAttribute("style", "display:none");
    imgBottom.setAttribute("style", "display:none");
    imgBottom.className = "transparent";

    imgTop.setAttribute("src", queue[0].sourceUrl);
    imgBottom.setAttribute("src", queue[1].sourceUrl);

    setTimeout(() => {

        imgTop.onload = function () {
            let object_fit = this.height > this.width ? fit_contain : fit_cover;
            console.log("iT " + queue[0].title + " fit " + object_fit + " height " + imgTop.height + " width " + imgTop.width + " portrait " + (imgTop.height > imgTop.width));
            this.setAttribute("style", "display:'';object-fit:" + object_fit);
        }

        imgBottom.onload = function () {
            let object_fit = this.height > this.width ? fit_contain : fit_cover;
            console.log("iB " + queue[1].title + " fit " + object_fit + " height " + imgBottom.height + " width " + imgBottom.width + " portratit " + (imgBottom.height > imgBottom.width));
            this.setAttribute("style", "display:'';object-fit:" + object_fit);
        }

        queue.splice(0, 2);

        setInterval(this.transition, displayTime);

    }, 500);

}

imageInterop.transition = function () {
    const delayToLoad = 3000;

    let imgTop = document.getElementById("imgTop");
    let imgBottom = document.getElementById("imgBottom");

    if (queue.length === 0) {
        setTimeout(() => {
            window.location = window.location.href;
            //dotNetObjRef.invokeMethodAsync("Reload");
        }, displayTime);
    }

    if (imgTop.className === "transparent") {

        imgBottom.className = "transparent";
        imgTop.className = "";

        setTimeout(() => {
            if (queue.length > 0) {
                let title = queue[0].title;


                imgBottom.onload = function () {
                    let object_fit = imgBottom.height > imgBottom.width ? fit_contain : fit_cover;
                    imgBottom.setAttribute("style", "object-fit:" + object_fit);
                    console.log("iB " + title + " fit " + object_fit + " height " + imgBottom.height + " width " + imgBottom.width + " portratit " + (imgBottom.height > imgBottom.width));

                }

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
                let title = queue[0].title;


                imgTop.onload = function () {
                    let object_fit = imgTop.height > imgTop.width ? fit_contain : fit_cover;
                    imgTop.setAttribute("style", "object-fit:" + object_fit);
                    console.log("iT " + title + " fit " + object_fit + " height " + imgTop.height + " width " + imgTop.width + " portrait " + (imgTop.height > imgTop.width));
                }

                imgTop.setAttribute("src", queue[0].sourceUrl);

                queue.splice(0, 1);
            }
        }, delayToLoad);
    }


}
