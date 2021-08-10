var dotNetObjRef;

export function clock(netObjRef) {
    dotNetObjRef = netObjRef;
    setInterval(clock_elapsed, 1000);
}

function clock_elapsed() {
    dotNetObjRef.invokeMethodAsync("Timer_elapsed");
}