var dotNetObjRef;

export function clock(netObjRef) {
    dotNetObjRef = netObjRef;
    setInterval(clock_elapsed, 3000);
}

function clock_elapsed() {
    dotNetObjRef.invokeMethodAsync("Timer_elapsed");
}