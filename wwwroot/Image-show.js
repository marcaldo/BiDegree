var imageInterop = imageInterop || {};

var queue = [];
var dotNetObjRef;
var displayTime;

const _c = {
    fit_cover: "cover",
    fit_contain: "contain"
}

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

    imgTop.setAttribute("style", "display:none");
    imgBottom.setAttribute("style", "display:none");
    imgBottom.className = "transparent";

    let item_top = queue[0];
    let item_bottom = queue[1];

    imgTop.setAttribute("src", item_top.sourceUrl);

    console.log("1.imgTop loading: " + item_top.title);

    imgBottom.setAttribute("src", item_bottom.sourceUrl);

    console.log("2.imgBottom loading: " + item_bottom.title);


    setTimeout(() => {
        imgTop.onload = function () {

            let object_fit = imageInterop.fit_orientation(this);
            this.setAttribute("style", "display:'';object-fit:" + object_fit);

            console.log("3.imgTop visible: " + item_top.title);


            item_top.portrait = object_fit == _c.fit_contain;
            imageInterop.debug_log(item_top);
        }

        imgBottom.onload = function () {

            let object_fit = imageInterop.fit_orientation(this);
            this.setAttribute("style", "display:'';object-fit:" + object_fit);

            console.log("4.imgBottom visible: " + item_bottom.title);

            item_bottom.portrait = object_fit == _c.fit_contain;
            imageInterop.debug_log(item_bottom);
        }

        queue.splice(0, 2);

        setInterval(this.transition, displayTime);

    }, 500);

}

imageInterop.transition = function () {

    const delayToLoad = 3000;
    let waitToReload = displayTime - delayToLoad;

    let imgTop = document.getElementById("imgTop");
    let imgBottom = document.getElementById("imgBottom");

    if (queue.length === 0) {
        setTimeout(() => {
            window.location = window.location.href;
            ////dotnetobjref.invokemethodasync("reload");
        }, waitToReload);
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

                    item.portrait = object_fit == _c.fit_contain;
                    imageInterop.debug_log(item);
                }

                imgBottom.setAttribute("src", item.sourceUrl);

                console.log("5.imgBottom loading: " + item.title);


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

                    item.portrait = object_fit == _c.fit_contain;
                    imageInterop.debug_log(item);
                }

                imgTop.setAttribute("src", item.sourceUrl);

                console.log("6.imgBottom loading: " + item.title);


                queue.splice(0, 1);
            }
        }, delayToLoad);
    }

}

imageInterop.fit_orientation = function (img) {

    if (img.naturalWidth > img.naturalHeight) { return _c.fit_cover; }
    return _c.fit_contain;
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

