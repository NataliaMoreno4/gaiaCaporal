const popup = document.querySelector(".popup"),
wifiIcon = document.querySelector(".icon i"),
popupTitle = document.querySelector(".popup .title"),
popupDesc = document.querySelector(".desc"),
reconnectBtn = document.querySelector(".reconnect");
overlay = document.querySelector(".disconnected-alert");

let isOnline = true, intervalId, timer = 10;

const checkConnection = async () => {
    try {
        const response = await fetch("https://jsonplaceholder.typicode.com/posts");
        isOnline = response.status >= 200 && response.status < 300;
    } catch (error) {
        isOnline = false; // If there is an error, the connection is considered offline
    }
    timer = 10;
    clearInterval(intervalId);
    handlePopup(isOnline);
}

const handlePopup = (status) => {
    if(status) {
        wifiIcon.className = "uil uil-wifi";
        popupTitle.innerText = "Conexion reestablecida";
        popupDesc.innerHTML = "Conexion reestablecida con el servidor";
        popup.classList.add("online");
        
        return setTimeout(() => overlay.classList.remove("show"), 1000);
    }
    wifiIcon.className = "uil uil-wifi-slash";
    popupTitle.innerText = "Conexion perdida con el servidor";
    popupDesc.innerHTML = "Se ha perdido la conexion con el servidor. Reintentando conexion...";
    popup.className = "popup show";
    overlay.classList.add("show");
}