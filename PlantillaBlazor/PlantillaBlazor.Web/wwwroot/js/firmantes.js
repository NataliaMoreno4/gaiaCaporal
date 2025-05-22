//Ubicación de espacios de firmas
export function agregarFirmaFirmante(id_firmante, descripcion_firmante, id) {
    var id_firma = "firma_" + id_firmante + "_" + id;

    var canvas = document.getElementById('signature-canvas');
    var ctx = canvas.getContext('2d');

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Configurar el estilo del texto
    ctx.font = '30px Arial';
    ctx.fillStyle = 'black';

    // Agregar texto al canvas
    ctx.fillText(descripcion_firmante, 0, canvas.height);

    var dataURL = canvas.toDataURL('image/png');

    var count = $("[data-element-count]").length + 1;

    var pagina = $('#pdf-page-text').val();
    agregarFirma(id_firma, pagina, count, 99, 199, 208, 115, dataURL, id_firmante);


    $("#" + id_firma).draggable({ scroll: !0, containment: "parent" }).attr({
        "data-element-count": count,
    });
}

function agregarFirma(id_firma, pagina, count, height, width, left, top, src, id_firmante) {

    var html = `<div class="sign-image" id="${id_firma}" ui-draggable ui-draggable-handle" data-element-count="${count-1}" style="height: ${height}px; width: ${width}px; left: ${left}px; top: ${top}px;" data-id-firmante="${id_firmante}" data-height="${height}px">
            <div class="sign-ifc">
                <button type="button" class="btn btn-default btn-xs delete">×</button>
                <button type="button" class="btn btn-default btn-xs sign-minus">-</button> 
                <button type="button" class="btn btn-default btn-xs sign-plus">+</button>
            </div>
            <img src="${src}" alt="" >
        </div>`;


    $('#editor-page-' + pagina).append(html);
}

function agregarFirmaNoControls(id_firma, pagina, count, height, width, left, top, src, id_firmante) {

    var html = `<div class="sign-image" id="${id_firma}" ui-draggable ui-draggable-handle" data-element-count="${count-1}" style="height: ${height}px; width: ${width}px; left: ${left}px; top: ${top}px;" data-id-firmante="${id_firmante}" data-height="${height}px">
            <div class="sign-ifc">
                <button type="button" class="btn btn-default btn-xs delete">×</button>
            </div>
            <img src="${src}" alt="" >
        </div>`;


    $('#editor-page-' + pagina).append(html);
}

export function limpiarFirmas() {
    $('.sign-image:not([data-box-type])').each(function (index, element) {
        $(element).remove();
    });
}

export function getCantidadFirmas() {
    return $('.sign-image:not([data-box-type])').length;
}

export function prepararFirma(id_firma, pagina, count, height_real, width_real, x, y, src, id_firmante) {
    var e = $('#editor-page-' + pagina);

    var s = (height_real / e.attr("data-viewport-height")) *e.attr("data-height"),
        o = (width_real / e.attr("data-viewport-width")) * e.attr("data-width");

    var top = (y / e.attr("data-viewport-height")) * e.attr("data-height");
    var left = (x / e.attr("data-viewport-width")) * e.attr("data-width");

    var height_final = s;
    var width_final = o;

    agregarFirmaNoControls(id_firma, pagina, count, height_final, width_final, left, top, src, id_firmante);
}



export function getFirmas() {
    var jsonData = [];

    $('.editor-page').each(function (index, element) {
        // Encontrar elementos con la clase "sign-image" dentro de cada "editor-page"

        var numero_pagina = $(element).data('page');

        var signImages = $(element).find('.sign-image:not([data-box-type])');

        // Iterar sobre los elementos "sign-image" y mostrar la información
        signImages.each(function (signIndex, signElement) {

            var n = $(signElement).parent(),
                r = parseFloat($(signElement).css("top")) / n.attr("data-height"),
                a = parseFloat($(signElement).css("left")) / n.attr("data-width"),
                s = parseFloat($(signElement).css("height")) / n.attr("data-height"),
                o = parseFloat($(signElement).css("width")) / n.attr("data-width");

            var height_t = s * n.attr("data-viewport-height");
            var width_t = o * n.attr("data-viewport-width");
            var x_t = a * n.attr("data-viewport-width");
            var y_t = r * n.attr("data-viewport-height");

            jsonData.push({
                NumeroPagina: numero_pagina,
                Contenido: "",
                Height: height_t,
                Width: width_t,
                CoordenadaX: x_t,
                CoordenadaY: y_t
            });
        });
    });

    // Convertir el objeto a una cadena JSON
    var jsonString = JSON.stringify(jsonData, null, 2);

    return jsonString;
}

export function getElementosFirma() {
    var jsonData = [];

    $('.editor-page').each(function (index, element) {
        // Encontrar elementos con la clase "sign-image" dentro de cada "editor-page"

        var numero_pagina = $(element).data('page');

        //Firmas

        var signImages = $(element).find('.sign-image:not([data-box-type])');

        signImages.each(function (signIndex, signElement) {

            var imgElement = $(signElement).find('img');

            // Obtener el valor del atributo src
            var srcValue = imgElement.attr('src');


            var n = $(signElement).parent(),
                r = parseFloat($(signElement).css("top")) / n.attr("data-height"),
                a = parseFloat($(signElement).css("left")) / n.attr("data-width"),
                s = parseFloat($(signElement).css("height")) / n.attr("data-height"),
                o = parseFloat($(signElement).css("width")) / n.attr("data-width");

            var height_t = s * n.attr("data-viewport-height");
            var width_t = o * n.attr("data-viewport-width");
            var x_t = a * n.attr("data-viewport-width");
            var y_t = r * n.attr("data-viewport-height");

            jsonData.push({
                NumeroPagina: numero_pagina,
                Contenido: srcValue,
                Height: height_t,
                Width: width_t,
                CoordenadaX: x_t,
                CoordenadaY: y_t,
                Tipo: "firma"
            });
        });

        //Checks
        var checks = $(element).find('.sign-image[data-box-type="checkmark"]');
        checks.each(function (checkIndex, element) {

            var id_firmante = "";

            var imgElement = $(element).find('img');

            // Obtener el valor del atributo src
            var srcValue = imgElement.attr('src');

            var n = $(element).parent(),
                r = parseFloat($(element).css("top")) / n.attr("data-height"),
                a = parseFloat($(element).css("left")) / n.attr("data-width"),
                s = parseFloat($(element).css("height")) / n.attr("data-height"),
                o = parseFloat($(element).css("width")) / n.attr("data-width");

            var height_t = s * n.attr("data-viewport-height");
            var width_t = o * n.attr("data-viewport-width");
            var x_t = a * n.attr("data-viewport-width");
            var y_t = r * n.attr("data-viewport-height");

            jsonData.push({
                NumeroPagina: numero_pagina,
                Contenido: srcValue,
                Height: height_t,
                Width: width_t,
                CoordenadaX: x_t,
                CoordenadaY: y_t,
                Tipo: "check"
            });
        });

        //CrossMarks
        var crosses = $(element).find('.sign-image[data-box-type="crossmark"]');
        crosses.each(function (crossIndex, element) {

            var id_firmante = "";

            var imgElement = $(element).find('img');

            // Obtener el valor del atributo src
            var srcValue = imgElement.attr('src');

            var n = $(element).parent(),
                r = parseFloat($(element).css("top")) / n.attr("data-height"),
                a = parseFloat($(element).css("left")) / n.attr("data-width"),
                s = parseFloat($(element).css("height")) / n.attr("data-height"),
                o = parseFloat($(element).css("width")) / n.attr("data-width");

            var height_t = s * n.attr("data-viewport-height");
            var width_t = o * n.attr("data-viewport-width");
            var x_t = a * n.attr("data-viewport-width");
            var y_t = r * n.attr("data-viewport-height");

            jsonData.push({
                NumeroPagina: numero_pagina,
                Contenido: srcValue,
                Height: height_t,
                Width: width_t,
                CoordenadaX: x_t,
                CoordenadaY: y_t,
                Tipo: "cross"
            });
        });

        //Radio
        var radios = $(element).find('.sign-image[data-box-type="radio"]');
        radios.each(function (radioIndex, element) {

            var id_firmante = "";

            var imgElement = $(element).find('img');

            // Obtener el valor del atributo src
            var srcValue = imgElement.attr('src');

            var n = $(element).parent(),
                r = parseFloat($(element).css("top")) / n.attr("data-height"),
                a = parseFloat($(element).css("left")) / n.attr("data-width"),
                s = parseFloat($(element).css("height")) / n.attr("data-height"),
                o = parseFloat($(element).css("width")) / n.attr("data-width");

            var height_t = s * n.attr("data-viewport-height");
            var width_t = o * n.attr("data-viewport-width");
            var x_t = a * n.attr("data-viewport-width");
            var y_t = r * n.attr("data-viewport-height");

            jsonData.push({
                NumeroPagina: numero_pagina,
                Contenido: srcValue,
                Height: height_t,
                Width: width_t,
                CoordenadaX: x_t,
                CoordenadaY: y_t,
                Tipo: "radio"
            });
        });

        //Texto
        var texts = $(element).find('.sign-text');
        texts.each(function (textIndex, element) {

            var id_firmante = "";

            var textAreaElement = $(element).find('textarea');

            var fontSize = $(textAreaElement).css('font-size');
            var lineHeight = $(textAreaElement).css('line-height');
            var content = $(textAreaElement).val();

            var n = $(element).parent(),
                r = parseFloat($(element).css("top")) / n.attr("data-height"),
                a = parseFloat($(element).css("left")) / n.attr("data-width"),
                s = parseFloat($(element).css("height")) / n.attr("data-height"),
                o = parseFloat($(element).css("width")) / n.attr("data-width");

            var height_t = s * n.attr("data-viewport-height");
            var width_t = o * n.attr("data-viewport-width");
            var x_t = a * n.attr("data-viewport-width");
            var y_t = r * n.attr("data-viewport-height");
            var fs = (parseFloat(fontSize) / n.attr("data-height")) *
                n.attr("data-viewport-height");
            var lh = (parseFloat(lineHeight) / n.attr("data-height")) *
                n.attr("data-viewport-height");
            
            jsonData.push({
                NumeroPagina: numero_pagina,
                Height: height_t,
                Width: width_t,
                CoordenadaX: x_t,
                CoordenadaY: y_t,
                Tipo: "texto",
                Contenido: content,
                FontSize: fs,
                LineHeight: lh
            });
        });

    });

    // Convertir el objeto a una cadena JSON
    var jsonString = JSON.stringify(jsonData, null, 2);

    return jsonString;
}

function setSello() {
    
}