var clock;
var hour12;
function startTime(element, isHour12) {

	let timeString = new Date().toLocaleTimeString([], { hour: 'numeric', hour12: hour12, minute: 'numeric', second: 'numeric' });
	timeString = timeString.replace("AM", "<span class='ampm'>AM</span>").replace("PM", "<span class='ampm'>PM</span>");

	console.log("++++++++++++++ + " + hour12);

	element.innerHTML = timeString;
	clock = setTimeout(startTime.bind(null, element, hour12), 3000);
}
function stopTime() {
	clearTimeout(clock);
}


function log(obj) { console.log(obj); }