// the arrow image that will follow the cursor
const arrow = document.getElementById("arrow-img");
// app div containing the entire generated blazor stuff
// listen for mutations in that div
const appContainer = document.getElementById("app");

let svg = document.getElementById("svg");

// config for mutation observer
// observes attribute changes, node deletions/insertions in the entire subtree of the app div
let config = { attributes: false, childList: true, subtree: true };

// if mutation occurs, call reassign
let observer = new MutationObserver(() => {
    reappend();
});

function reappend() {
    if (svg == null) {
        svg = document.getElementById("svg");
    } else {
        // temporarily disconnect observer to avoid infinite loop caused by appending the arrow
        observer.disconnect();
        svg.appendChild(arrow);
        observer.observe(appContainer, config);
        console.log("reappended");
    }
}

// check for performance impact
window.addEventListener("mousemove", (event) => {
    track(event);
});

function track(event) {
    // track arrow position to mouse position
    coords = convertToSvgCoords2(event.x, event.y);
    let width = arrow.getAttribute("width");
    arrow.setAttribute("x", coords.x - parseInt(width) / 2);
    arrow.setAttribute("y", coords.y - parseInt(width) / 2);
}

// start looking for mutations
observer.observe(appContainer, config);
