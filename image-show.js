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
            let object_fit = imageInterop.fit_orientation(this);
            this.setAttribute("style", "display:'';object-fit:" + object_fit);

            item_top.portrait = object_fit == fit_contain;
            imageInterop.debug_log(item_top);
        }

        imgBottom.onload = function () {
            let object_fit = imageInterop.fit_orientation(this);
            this.setAttribute("style", "display:'';object-fit:" + object_fit);

            item_bottom.portrait = object_fit == fit_contain;
            imageInterop.debug_log(item_bottom);
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
    let object_fit;

    if (imgTop.className === "transparent") {

        imgBottom.className = "transparent";
        imgTop.className = "";

        setTimeout(() => {
            if (queue.length > 0) {

                imgBottom.onload = function () {

                    object_fit = imageInterop.fit_orientation(this);
                    imgBottom.setAttribute("style", "object-fit:" + object_fit);

                    item.portrait = object_fit == fit_contain;
                    imageInterop.debug_log(item);
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
                    object_fit = imageInterop.fit_orientation(this);
                    imgTop.setAttribute("style", "object-fit:" + object_fit);

                    item.portrait = object_fit == fit_contain;
                    imageInterop.debug_log(item);
                }

                imgTop.setAttribute("src", item.sourceUrl);

                queue.splice(0, 1);
            }
        }, delayToLoad);
    }

}

imageInterop.fit_orientation = function (img) {

    if (img.naturalWidth > img.naturalHeight) { return fit_cover; }
    return fit_contain;
}

imageInterop.debug_log = function (item) {

    if (!item) { return };

    let debugInfo = document.getElementById("debugInfo");

    if (debugInfo === null) { return };

    let orientation = item.portrait ? "portrait" : "landscape";

    let tr = document.createElement("tr");
    tr.innerHTML = `<td><span class="action"></span>${item.title}</td><td>${item.width}</td><td>${item.height}</td><td>${orientation}</td><td>${item.fileSize / 1024}</td>`;
    debugInfo.appendChild(tr);

}

