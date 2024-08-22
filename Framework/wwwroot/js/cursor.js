// the arrow image that will follow the cursor
let arrow = document.getElementById("arrow-img");

// app div containing the entire generated blazor stuff
// listen for mutations in that div
const appContainer = document.getElementById("app");

// config for mutation observer
// observes attribute changes, node deletions/insertions in the entire subtree of the app div
let config = { attributes: false, childList: true, subtree: true };

// if mutation occurs, call reassign
let observer = new MutationObserver(() => {
    reassign();
});

function reassign() {
    // for some reason, it needs to be reassigned every time
    // if (arrow == null) {
    arrow = document.getElementById("arrow-img");
    // }
}

// check for performance impact
window.addEventListener("mousemove", (event) => {
    track(event);
});

function track(event) {
    if (arrow == null) {
        return;
    }
    // track arrow position to mouse position
    coords = convertToSvgCoords2(event.x, event.y);
    let width = arrow.getAttribute("width");
    arrow.setAttribute("x", coords.x - parseInt(width) / 2);
    arrow.setAttribute("y", coords.y - parseInt(width) / 2);
}

observer.observe(appContainer, config);
