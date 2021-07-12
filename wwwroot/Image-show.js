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

    let item_top = queue[0];
    let item_bottom = queue[1];

    imgTop.setAttribute("src", item_top.sourceUrl);
    imgBottom.setAttribute("src", item_bottom.sourceUrl);

    setTimeout(() => {

        imgTop.onload = function () {
            let object_fit = item_top.portrait ? fit_contain : fit_cover;
            console.log("iT " + item_top.title + " fit " + object_fit + " w,h " + imgTop.width + "x" + imgTop.height + " portrait " + (imgTop.width < imgTop.height) + " " + item_top.portrait);
            this.setAttribute("style", "display:'';object-fit:" + object_fit);
        }

        imgBottom.onload = function () {
            let object_fit = item_bottom ? fit_contain : fit_cover;
            console.log("iB " + item_bottom.title + " fit " + object_fit + " w,h " + imgBottom.width + "x" + imgBottom.height + " portratit " + (imgBottom.width < imgBottom.height) + " " + item_bottom.portrait);
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

    let item = queue[0];

    if (imgTop.className === "transparent") {

        imgBottom.className = "transparent";
        imgTop.className = "";

        setTimeout(() => {
            if (queue.length > 0) {

                imgBottom.onload = function () {
                    let object_fit = item.portrait ? fit_contain : fit_cover;
                    imgBottom.setAttribute("style", "object-fit:" + object_fit);
                    console.log("iB " + item.title + " fit " + object_fit + " w,h " + imgBottom.width + "x" + imgBottom.height + " portratit " + (imgBottom.width < imgBottom.height) + " " + item.portrait);

                }

                imgBottom.setAttribute("src", item.sourceUrl);

                queue.splice(0, 1);
            }
        }, delayToLoad);

    }
    else {

        imgTop.className = "transparent";
        imgBottom.className = "";

        setTimeout(() => {
            if (queue.length > 0) {

                imgTop.onload = function () {
                    let object_fit = item.portrait ? fit_contain : fit_cover;
                    imgTop.setAttribute("style", "object-fit:" + object_fit);
                    console.log("iT " + item.title + " fit " + object_fit + " w,h " + imgTop.width + "x" + imgTop.height + " portrait " + (imgTop.width < imgTop.height) + " " + item.portrait);
                }

                imgTop.setAttribute("src", item.sourceUrl);

                queue.splice(0, 1);
            }
        }, delayToLoad);
    }


}
