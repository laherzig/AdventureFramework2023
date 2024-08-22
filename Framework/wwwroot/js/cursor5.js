// this code was made in assumption that this library was being used:
// https://rouninmedia.github.io/axe/axe.js

// the arrow image that will follow the cursor
const arrow = document.getElementById("arrow-img");

// check for performance impact
window.addEventListener("mousemove", (event) => {
    track(event);
});

function track(event) {
    // track arrow position to mouse position
    let width = window.getComputedStyle(arrow).width;
    arrow.style.left = event.x - parseInt(width) / 2 + "px";
    arrow.style.top = event.y - parseInt(width) / 2 + "px";
}
