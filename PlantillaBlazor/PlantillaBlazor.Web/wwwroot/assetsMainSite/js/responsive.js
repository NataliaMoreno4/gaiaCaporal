
document.querySelector(".vertical-overlay").addEventListener("click", function () {
    document.body.classList.remove("vertical-sidebar-enable");
});

$(".nav-item .nav-link").not('[role="button"]').on("click", function () {
    document.body.classList.remove("vertical-sidebar-enable");
});

document.getElementById("topnav-hamburger-icon") && document.getElementById("topnav-hamburger-icon").addEventListener("click", O);

function O() {
    var e = document.documentElement.clientWidth;
    767 < e && document.querySelector(".hamburger-icon").classList.toggle("open"),
        "horizontal" === document.documentElement.getAttribute("data-layout") && (document.body.classList.contains("menu") ? document.body.classList.remove("menu") : document.body.classList.add("menu")),
        "vertical" === document.documentElement.getAttribute("data-layout") &&
        (e <= 1025 && 767 < e
            ? (document.body.classList.remove("vertical-sidebar-enable"),
                "sm" == document.documentElement.getAttribute("data-sidebar-size") ? document.documentElement.setAttribute("data-sidebar-size", "") : document.documentElement.setAttribute("data-sidebar-size", "sm"))
            : 1025 < e
                ? (document.body.classList.remove("vertical-sidebar-enable"),
                    "lg" == document.documentElement.getAttribute("data-sidebar-size") ? document.documentElement.setAttribute("data-sidebar-size", "sm") : document.documentElement.setAttribute("data-sidebar-size", "lg"))
                : e <= 767 && (document.body.classList.add("vertical-sidebar-enable"), document.documentElement.setAttribute("data-sidebar-size", "lg"))),
        "semibox" === document.documentElement.getAttribute("data-layout") &&
        (767 < e
            ? "show" == document.documentElement.getAttribute("data-sidebar-visibility")
                ? "lg" == document.documentElement.getAttribute("data-sidebar-size")
                    ? document.documentElement.setAttribute("data-sidebar-size", "sm")
                    : document.documentElement.setAttribute("data-sidebar-size", "lg")
                : (document.getElementById("sidebar-visibility-show").click(), document.documentElement.setAttribute("data-sidebar-size", document.documentElement.getAttribute("data-sidebar-size")))
            : e <= 767 && (document.body.classList.add("vertical-sidebar-enable"), document.documentElement.setAttribute("data-sidebar-size", "lg"))),
        "twocolumn" == document.documentElement.getAttribute("data-layout") && (document.body.classList.contains("twocolumn-panel") ? document.body.classList.remove("twocolumn-panel") : document.body.classList.add("twocolumn-panel"));
}