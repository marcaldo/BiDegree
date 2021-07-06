var isEven = true;
var image1, image1;
var queue1, queue2;
var item1, item2;
var i = 0;

function RunQueues(q1, q2, displayTime) {

    displayTime = 5000;             // TODO: REMOVE

    image1 = document.getElementById("image1");
    image2 = document.getElementById("image2");

    queue1 = q1;
    queue2 = q2;

     SetDisplay();

    setInterval(SetDisplay, displayTime);

}

function SetDisplay() {

    if (i % 2 == 0) {

    }


    item1 = queue1[0];
    item2 = queue2[0];

    if (isFirst) {
        image1.setAttribute("src", item1.sourceUrl);
        image2.setAttribute("src", item2.sourceUrl);

        queue1.splice(0, 1);
        queue2.splice(0, 1);
    }

    console.log("display " + ++cont);
    console.log(item1.sourceUrl);
    console.log(item2.sourceUrl);

    DoTransition();


}

function DoTransition() {

    if (isFirst) {
        image1.setAttribute("style", "display:");
        image2.setAttribute("style", "display:none");
    }
    else {
        image1.setAttribute("style", "display:none");
        image2.setAttribute("style", "display:");
    }


}