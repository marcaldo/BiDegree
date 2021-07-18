var imageInterop = imageInterop || {};

var queue = [];
var dotNetObjRef;
var displayTime;

const fit_cover = "cover";
const fit_contain = "contain";

const itemType = {
    isImage: 0,
    isVideo: 1,
    isWeather: 2
}

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
    let videoTop = document.getElementById("videoTop");
    let videoBottom = document.getElementById("videoBottom");

    imgBottom.className = "transparent";
    videoBottom.localName = "transparent";

    let item_top = queue[0];
    let item_bottom = queue[1];

    //if (item_bottom.itemType === itemType.isVideo) {
    //    let videoBottomSrc = document.getElementById("videoBottomSrc");   // document.createElement("source");
    //    videoBottomSrc.setAttribute("src", item_bottom.sourceUrl);
    //    videoBottom.appendChild(videoBottomSrc);
    //    videoBottom.play();
    //}
    //else {
        imgBottom.setAttribute("src", item_bottom.sourceUrl);
    //}

    //if (item_top.itemType === itemType.isVideo) {
    //    let videoTopSrc = document.getElementById("videoTopSrc");  // document.createElement("source");
    //    videoTopSrc.setAttribute("src", item_top.sourceUrl);
    //    videoTop.appendChild(videoTopSrc);
    //    videoTop.play();
    //}
    //else {
        imgTop.setAttribute("src", item_top.sourceUrl);
    //}


    setTimeout(() => {
        imgTop.onload = function () {

            //videoTop.oncanplaythrough = function () {
            //    if (item_top.itemType === itemType.isVideo) {
            //        this.play();
            //    }
            //}

            //videoBottom.oncanplaythrough = function () {
            //    if (item_bottom.itemType === itemType.isVideo) {
            //        this.play();
            //    }
            //}


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
    //let videoTop = document.getElementById("videoTop");
    //let videoBottom = document.getElementById("videoBottom");
    //let videoTopSrc = document.getElementById("videoTopSrc");
    //let videoBottomSrc = document.getElementById("videoBottomSrc");

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

        //videoBottom.className = "transparent";
        //videoTop.className = "";

        setTimeout(() => {
            if (queue.length > 0) {

                //videoBottom.oncanplaythrough = function () {
                //    this.setAttribute("style", "display:''");
                //    videoTop.setAttribute("style", "display:none");
                //}

                imgBottom.onload = function () {

                    object_fit = imageInterop.fit_orientation(this);
                    imgBottom.setAttribute("style", "object-fit:" + object_fit);

                    item.portrait = object_fit == fit_contain;
                    imageInterop.debug_log(item);
                }

                imgBottom.setAttribute("src", item.sourceUrl);
                //videoBottomSrc.setAttribute("src", item.sourceUrl);
                //videoBottom.play();

                queue.splice(0, 1);
            }
        }, delayToLoad);

    }
    else {

        imgTop.className = "transparent";
        imgBottom.className = "";

        //videoTop.className = "transparent";
        //videoBottom.className = "";

        setTimeout(() => {
            if (queue.length > 0) {

                //videoTop.oncanplaythrough = function () {
                //    this.setAttribute("style", "display:''");
                //    videoBottomSrc.setAttribute("style", "display:none");
                //}

                imgTop.onload = function () {
                    object_fit = imageInterop.fit_orientation(this);
                    imgTop.setAttribute("style", "object-fit:" + object_fit);

                    item.portrait = object_fit == fit_contain;
                    imageInterop.debug_log(item);
                }

                imgTop.setAttribute("src", item.sourceUrl);
                //videoTopSrc.setAttribute("src", item.sourceUrl);
                //videoTop.play();

                queue.splice(0, 1);
            }
        }, delayToLoad);
    }

}

imageInterop.fit_orientation = function (img) {

    if (img.naturalWidth > img.naturalHeight) { return fit_cover; }
    return fit_contain;
}

var itemCount = 0;
imageInterop.debug_log = function (item) {

    if (!item) { return };

    let debugInfo = document.getElementById("debugInfo");

    if (debugInfo === null) { return };

    let orientation = item.portrait ? "portrait" : "landscape";

    let tr = document.createElement("tr");
    let itemTypeIcon = item.itemType === itemType.isVideo
        ? "<svg xmlns='http://www.w3.org/2000/svg' width='16' height='16' fill='currentColor' class='bi bi-film' viewBox='0 0 16 16'><path d='M0 1a1 1 0 0 1 1-1h14a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H1a1 1 0 0 1-1-1V1zm4 0v6h8V1H4zm8 8H4v6h8V9zM1 1v2h2V1H1zm2 3H1v2h2V4zM1 7v2h2V7H1zm2 3H1v2h2v-2zm-2 3v2h2v-2H1zM15 1h-2v2h2V1zm-2 3v2h2V4h-2zm2 3h-2v2h2V7zm-2 3v2h2v-2h-2zm2 3h-2v2h2v-2z'/></svg>"
        : "<svg xmlns='http://www.w3.org/2000/svg' width='16' height='16' fill='currentColor' class='bi bi-image' viewBox='0 0 16 16'><path d='M6.002 5.5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0z'/><path d='M2.002 1a2 2 0 0 0-2 2v10a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V3a2 2 0 0 0-2-2h-12zm12 1a1 1 0 0 1 1 1v6.5l-3.777-1.947a.5.5 0 0 0-.577.093l-3.71 3.71-2.66-1.772a.5.5 0 0 0-.63.062L1.002 12V3a1 1 0 0 1 1-1h12z'/></svg>"
    tr.innerHTML = `<td>${++itemCount}</td><td style='text-align:center'>${itemTypeIcon}</td><td><span class="action"></span>${item.title}</td><td>${item.width}</td><td>${item.height}</td><td>${orientation}</td><td>${item.fileSize / 1024}</td>`;
    debugInfo.appendChild(tr);

}

