// https://www.w3schools.com/html/tryit.asp?filename=tryhtml5_geolocation

// Called from C# index.razor
function getCurrentLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(returnPosition);
    } else {
        console.log("Geolocation is not supported by this browser.");
    }
}

// Call the C# function "GetWeatherCaller"
function returnPosition(position) {
    DotNet.invokeMethodAsync("BiDegree", "GetWeatherCaller", position.coords.latitude, position.coords.longitude);
}