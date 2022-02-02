var setDocumentTitle = function (title) {
    document.title = title;
};

var setFocus = (element) => {
    element.focus();
};

var startRandomGenerator = function (dotNetObject) {
    let text = "Hello .NET from JavaScript";
    return setInterval(() => {
        console.log("JS: Generated " + text);
        dotNetObject.invokeMethodAsync("AddText", text);
    }, 1000);
};

var stopRandomGenerator = function (handle) {
    clearInterval(handle);
}