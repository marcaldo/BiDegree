var imageInterop = imageInterop || {};

var queue = [];
var dotNetObjRef;
var displayTime;

imageInterop.runQueue = function (displayQueue, interval, netObjRef) {
    dotNetObjRef = netObjRef;
    displayTime = interval;
    queue = displayQueue;

    //if (queue.length === 1) {
    //    queue.push(queue[0]);
    //}

    console.table(queue);

    imgTop = document.getElementById("imgTop");
    imgBottom = document.getElementById("imgBottom");

    imgTop.setAttribute("style", "display:none");
    imgBottom.setAttribute("style", "display:none");
    imgBottom.className = "transparent";

    imgTop.setAttribute("src", queue[0].sourceUrl);
    imgBottom.setAttribute("src", queue[1].sourceUrl);

    setTimeout(() => {
        let object_fit = "cover";

        imgTop.onload = function () {
            if ((this.height / this.width) > 1) {
                object_fit = "contain";
            }

            this.setAttribute("style", "display:'';object-fit:" + object_fit);
        }

        imgBottom.onload = function () {
            if ((this.height / this.width) > 1) {
                object_fit = "contain";
            }

            this.setAttribute("style", "display:'';object-fit:" + object_fit);
        }

        queue.splice(0, 2);

        setInterval(this.transition, displayTime);

    }, 500);

}

imageInterop.transition = function () {
    const delayToLoad = 3000;

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
                imgBottom.setAttribute("src", queue[0].sourceUrl);

                let object_fit = "cover";
                imgBottom.onload = function () {
                    if ((imgBottom.height / imgBottom.width) > 1) {
                        object_fit = "contain";
                    }

                    this.setAttribute("style", "object-fit:" + object_fit);
                }

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

                let object_fit = "cover";
                imgTop.onload = function () {
                    if ((imgTop.height / imgTop.width) > 1) {
                        object_fit = "contain";
                    }

                    this.setAttribute("style", "object-fit:" + object_fit);
                }

                queue.splice(0, 1);
            }
        }, delayToLoad);
    }


}
