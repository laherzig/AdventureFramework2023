// define variables for all the pos-presets
// most likely to be null at initialization, but who knows
let leftElement = document.getElementById("pos-preset-left");
let rightElement = document.getElementById("pos-preset-right");
let topElement = document.getElementById("pos-preset-top");
let bottomElement = document.getElementById("pos-preset-bottom");

const arrow = document.getElementById("arrow-img");

// this is to check if the preset was shown before the DOM update.
// If it was, you don't have to add new event listeners
let leftWasShown = false;
let rightWasShown = false;
let topWasShown = false;
let bottomWasShown = false;

// app div containing the entire generated blazor stuff
// listen for mutations in that div
const appContainer = document.getElementById("app");

// if mutation occurs, call reassign
let observer = new MutationObserver(function (mutations) {
    reassignAlwaysTrack();
});

// check for performance impact
window.addEventListener("mousemove", (event) => {
    track(event);
});

function reassignAlwaysTrack() {
    /*
        hide arrow

        get all the presets

        if preset is not null:
            if preset wasn't shown before:
                attach event listeners
            if mouse is inside preset:
                show arrow


        event listeners:
        variant 1:
            mouseenter: rotate arrow, show arrow
            mouseleave: hide arrow

        variant 2:
            mouseenter: track arrow, rotate arrow, show arrow
            mouseleave: untrack arrow, hide arrow


    */
    arrow.style.display = "none";

    leftElement = document.getElementById("pos-preset-left");

    // if visible
    if (leftElement) {
        if (!leftWasShown) {
            leftElement.addEventListener("mouseenter", function () {
                arrow.style.display = "block";
                arrow.style.transform = "none";
            });
            leftElement.addEventListener("mouseleave", function () {
                arrow.style.display = "none";
            });
        }

        if (
            isPointInsideElement(
                mouse.absMousePos.x,
                mouse.absMousePos.y,
                leftElement
            )
        ) {
            arrow.style.display = "block";
            arrow.style.transform = "none";
        }
        leftWasShown = true;
    } else {
        leftWasShown = false;
    }

    rightElement = document.getElementById("pos-preset-right");

    if (rightElement) {
        if (!rightWasShown) {
            rightElement.addEventListener("mouseenter", function () {
                arrow.style.display = "block";
                arrow.style.transform = "rotate(180deg)";
            });
            rightElement.addEventListener("mouseleave", function () {
                arrow.style.display = "none";
            });
        }

        if (
            isPointInsideElement(
                mouse.absMousePos.x,
                mouse.absMousePos.y,
                rightElement
            )
        ) {
            arrow.style.display = "block";
            arrow.style.transform = "rotate(180deg)";
        }
        rightWasShown = true;
    } else {
        rightWasShown = false;
    }

    topElement = document.getElementById("pos-preset-top");

    if (topElement) {
        if (!topWasShown) {
            topElement.addEventListener("mouseenter", function () {
                arrow.style.display = "block";
                arrow.style.transform = "rotate(270deg)";
            });
            topElement.addEventListener("mouseleave", function () {
                arrow.style.display = "none";
            });
        }

        if (
            isPointInsideElement(
                mouse.absMousePos.x,
                mouse.absMousePos.y,
                topElement
            )
        ) {
            arrow.style.display = "block";
            arrow.style.transform = "rotate(90deg)";
        }
        topWasShown = true;
    } else {
        topWasShown = false;
    }

    bottomElement = document.getElementById("pos-preset-bottom");

    if (bottomElement) {
        if (!bottomWasShown) {
            bottomElement.addEventListener("mouseenter", function () {
                arrow.style.display = "block";
                arrow.style.transform = "rotate(270deg)";
            });
            bottomElement.addEventListener("mouseleave", function () {
                arrow.style.display = "none";
            });
        }

        if (
            isPointInsideElement(
                mouse.absMousePos.x,
                mouse.absMousePos.y,
                bottomElement
            )
        ) {
            arrow.style.display = "block";
            arrow.style.transform = "rotate(90deg)";
        }
        bottomWasShown = true;
    } else {
        bottomWasShown = false;
    }
}

function track(event) {
    // track arrow position to mouse position
    let width = window.getComputedStyle(arrow).width;
    arrow.style.left = event.x - parseInt(width) / 2 + "px";
    arrow.style.top = event.y - parseInt(width) / 2 + "px";
}

// check if a point is inside an element
// needed to un-display an images on a route event when no present exists anymore at that place
function isPointInsideElement(x, y, element) {
    // Get the bounding rectangle of the element
    const rect = element.getBoundingClientRect();

    // Check if the point is within the bounding rectangle
    return (
        x >= rect.left && x <= rect.right && y >= rect.top && y <= rect.bottom
    );
}

// config for mutation observer
// observes attribute changes, node deletions/insertions in the entire subtree of the app div
let config = { attributes: true, childList: true, subtree: true };

// start looking for mutations
observer.observe(appContainer, config);
