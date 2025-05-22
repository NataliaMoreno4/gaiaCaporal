//RECAPTCHA
runCaptcha = function (actionName) {
    return new Promise((resolve, reject) => {
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdSzOIUAAAAALm0JDVCho1mJWvnP9bLsajJvC8M', { action: 'submit' }).then(function (token) {
                resolve(token);
            });
        });
    });
};

//SWEET ALERT
//https://sweetalert2.github.io/#usage

function displaySwal(titulo, mensaje, footer, type) {
    Swal.fire({
        icon: type,
        title: titulo,
        text: mensaje,
        footer: footer
    })
}

function displaySwalWithHtml(titulo, mensaje, footer, type) {
    Swal.fire({
        icon: type,
        title: titulo,
        html: mensaje,
        footer: footer
    })
}

function customConfirmSwal(titulo, mensaje, tipo,footer) {
    return new Promise((resolve) => {
        Swal.fire({
            title: titulo,
            text: mensaje,
            icon: tipo,
            footer: footer,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                resolve(true);
            }
            else {
                resolve(false);
            }
        });
    });
}

function customConfirmSuccessSwal(titulo, mensaje, tipo, footer) {
    return new Promise((resolve) => {
        Swal.fire({
            icon: tipo,
            title: titulo,
            text: mensaje,
            showCancelButton: false,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        }).then(function () {
            resolve(true);
        });
    });
}



function LoadingSwal(title, mensaje) {
    let timerInterval
    Swal.fire({
        title: title,
        html: mensaje,
        timer: 2000000000000000000000000,

        didOpen: () => {
            Swal.showLoading()

        },
        willClose: () => {

        }
    }).then((result) => {
        /* Read more about handling dismissals below */
        if (result.dismiss === Swal.DismissReason.timer) {
            
        }
    })
}

// loadScript: returns a promise that completes when the script loads
window.loadScript = function (scriptPath) {
    ////// check list - if already loaded we can ignore
    //if (loaded[scriptPath]) {
    //    console.log(scriptPath + " already loaded");
    //    // return 'empty' promise
    //    return new this.Promise(function (resolve, reject) {
    //        resolve();
    //    });
    //}

    return new Promise(function (resolve, reject) {
        // create JS library script element
        var script = document.createElement("script");
        script.src = scriptPath;
        script.type = "text/javascript";
        

        // flag as loading/loaded
        loaded[scriptPath] = true;

        // if the script returns okay, return resolve
        script.onload = function () {
            
            resolve(scriptPath);
        };

        // if it fails, return reject
        script.onerror = function () {
            console.log(scriptPath + " load failed");
            reject(scriptPath);
        }

        // scripts will load at end of body
        document["body"].appendChild(script);
    });
}
// store list of what scripts we've loaded
loaded = [];

function CloseModal(modalId) {
    $(modalId).modal('hide');
}

function OpenModal(modalId) {
    $(modalId).modal('show');
}

function clearInput(inputId) {
    var input = document.querySelector("#" + inputId);
    if (input) {
        input.value = "";
    }
}

function setInput(inputId, value) {
    var input = document.querySelector("#" + inputId);
    if (input) {
        input.value = value;
    }
}

function descargarArchivo(rutaArchivo) {
    window.open(rutaArchivo, '_blank');
}

function soloLetras(e) {
    var key = e.keyCode || e.which,
        tecla = String.fromCharCode(key).toLowerCase(),
        letras = " áéíóúabcdefghijklmnñopqrstuvwxyz",
        especiales = [8, 37, 39, 46],
        tecla_especial = false;

    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

function PreventPaste(input) {
    $(input).on('paste', function (e) {
        e.preventDefault();
    })
}

function format(input) {
    var num = input.value.replace(/\./g, '');

    if (!isNaN(num)) {
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
        num = num.split('').reverse().join('').replace(/^[\.]/, '');
        input.value = num;
    } else {
        input.value = input.value.replace(/[^\d\.]*/g, '');
    }
}

function addFormatListener(id) {
    const input = document.getElementById(id);
    input.addEventListener('input', function () {
        format(this);
    });
}

function mtdValidarDireccion(input) {
    var iChars = "!@$%^&*()+=[]\\\';,./{}|\":<>?¡“”";

    for (var i = 0; i < input.value.length; i++) {
        if (iChars.indexOf(input.value.charAt(i)) != -1) {
            alert("Este campo no puede contener caracteres especiales");
            input.value = "";
            retornador = false; return;
        }
    }

}

function agregarClase(elemento, clase) {
    document.querySelector(elemento).classList.add(clase);
}


function validarPorc(input) {
    var valor = input.value;

    if (valor > 100) {
        alert('Los porcentajes no pueden exceder el 100%');
        input.value = "";
    }
}

window.getIpAddress = () => {
    return fetch('/api/info/ipaddress')
        .then((response) => response.text())
        .then((data) => {
            return data
        })
}

function successToastr(mensaje) {
    toastr.success(mensaje, { timeOut: 5000 })
}

function warningToastr(mensaje) {
    toastr.warning(mensaje, { timeOut: 5000 })
}

function errorToastr(mensaje) {
    toastr.error(mensaje, { timeOut: 5000 })
}

var idotnetHelper;

function initializeInactivityTimer(dotnetHelper, maximoInactividad, alertaInactividad) {

    idotnetHelper = dotnetHelper;

    var timer;
    var timerAlerta;

    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        clearTimeout(timerAlerta);

        timer = setTimeout(logout, 1000 * 60 * maximoInactividad);
        timerAlerta = setTimeout(alertaLogout, 1000 * 60 * alertaInactividad);
    }

    function logout() {
        clearTimeout(timer);
        clearTimeout(timerAlerta);
        dotnetHelper.invokeMethodAsync("Logout");
    }

    function alertaLogout() {
        clearTimeout(timerAlerta);
        dotnetHelper.invokeMethodAsync("AlertaLogout");
    }
}

function downloadURI(uri, name) {
    var link = document.createElement("a");
    link.download = name;
    link.href = uri;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}

function getActualUser() {
    try {
        return idotnetHelper.invokeMethodAsync("GetActualUserName");
    }
    catch (e) {
        return "";
    }

}


// Crear un objeto con todos los datos que se pueden obtener
var datosCliente = {
    userAgent: navigator.userAgent,
    navegador: navigator.appName,
    versionNavegador: navigator.appVersion,
    plataformaNavegador: navigator.platform,
    idioma: navigator.language,
    cookiesHabilitadas: navigator.cookieEnabled,
    resolucionPantalla: {
        ancho: window.screen.width,
        alto: window.screen.height
    },
    profundidadColor: window.screen.colorDepth,
    sistemaOperativo: {
        nombre: navigator.platform,
    },
    sesion: "",
    actualUser: "",
    url: window.location.href,
    ubicacion: null, // Se actualizará con la ubicación obtenida

    // Obtener la ubicación del cliente
    obtenerUbicacion: function () {
        datosCliente.url = window.location.href;
        navigator.geolocation.getCurrentPosition(function (posicion) {
            var latitud = posicion.coords.latitude;
            var longitud = posicion.coords.longitude;
            datosCliente.ubicacion = {
                latitud: latitud,
                longitud: longitud
            };
            enviarDatos();
        },
            function (error) {
                if (error.code == error.PERMISSION_DENIED) {
                    enviarDatos();
                }
            });
    }
};

async function enviarDatos() {
    if (datosCliente.actualUser == "") {
        var usuario = await getActualUser();
        datosCliente.actualUser = usuario;
    }
    fetch('/api/info/sendClientInfo', { // Reemplaza 'URL_DEL_METODO_API' con la URL correcta
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(datosCliente)
    })
        .then(function (response) {
            if (response.ok) {
                // La solicitud fue exitosa
            } else {
                // La solicitud falló
                throw new Error('Error en la solicitud: ' + response.status);
            }
        })
        .catch(function (error) {
            // Manejar errores de conexión o respuesta
            console.error(error);
        });
}

// Llamar a la función para obtener la ubicación y enviar los datos
datosCliente.obtenerUbicacion();

function getClientInfo() {
    datosCliente.obtenerUbicacion();
}

function numberOnly(id) {
    var element = document.getElementById(id);
    element.value = element.value.replace(/[^0-9]/gi, "");
}

function soloNumeros(event) {
    const keyCode = event.keyCode ? event.keyCode : event.which;

    // Permitir: backspace, delete, tab, escape, enter y flechas
    if ([8, 9, 27, 13, 37, 38, 39, 40].indexOf(keyCode) !== -1 ||
        // Permitir Ctrl+A, Ctrl+C, Ctrl+V, Ctrl+X
        (keyCode === 65 && (event.ctrlKey === true || event.metaKey === true)) ||
        (keyCode === 67 && (event.ctrlKey === true || event.metaKey === true)) ||
        (keyCode === 86 && (event.ctrlKey === true || event.metaKey === true)) ||
        (keyCode === 88 && (event.ctrlKey === true || event.metaKey === true))) {
        return;
    }

    // Asegurarse de que es un número (números normales y del teclado numérico)
    if ((keyCode < 48 || keyCode > 57) && (keyCode < 96 || keyCode > 105)) {
        event.preventDefault();
    }
}

function addSoloNumerosListener(id) {
    const input = document.getElementById(id);
    input.addEventListener('keydown', soloNumeros);
}

function abrirPopUp(nombreOverlay, nombrePopup) {
    var overlay1 = document.getElementById(nombreOverlay),
        popup1 = document.getElementById(nombrePopup);
    overlay1.classList.add('active');
    popup1.classList.toggle("show");
}

function cerrarPopUp(nombreOverlay, nombrePopup) {
    var overlay1 = document.getElementById(nombreOverlay),
        popup1 = document.getElementById(nombrePopup);
    overlay1.classList.remove('active');
    popup1.classList.toggle("show");
}
