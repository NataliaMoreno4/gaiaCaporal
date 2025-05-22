Notiflix.Notify.init({
    width: '280px',
    position: 'right-top', // 'right-top' - 'right-bottom' - 'left-top' - 'left-bottom' - 'center-top' - 'center-bottom' - 'center-center'
    distance: '10px',
    opacity: 1,
    borderRadius: '5px',
    rtl: false,
    timeout: 3000,
    messageMaxLength: 110,
    backOverlay: false,
    backOverlayColor: 'rgba(0,0,0,0.5)',
    plainText: true,
    showOnlyTheLastOne: false,
    clickToClose: false,
    pauseOnHover: true,

    ID: 'NotiflixNotify',
    className: 'notiflix-notify',
    zindex: 4001,
    fontFamily: 'Quicksand',
    fontSize: '14px',
    cssAnimation: true,
    cssAnimationDuration: 400,
    cssAnimationStyle: 'fade', // 'fade' - 'zoom' - 'from-right' - 'from-top' - 'from-bottom' - 'from-left'
    closeButton: false,
    useIcon: true,
    useFontAwesome: false,
    fontAwesomeIconStyle: 'basic', // 'basic' - 'shadow'
    fontAwesomeIconSize: '34px',

    success: {
        background: '#32c682',
        textColor: '#fff',
        childClassName: 'notiflix-notify-success',
        notiflixIconColor: 'rgba(0,0,0,0.2)',
        fontAwesomeClassName: 'fas fa-check-circle',
        fontAwesomeIconColor: 'rgba(0,0,0,0.2)',
        backOverlayColor: 'rgba(50,198,130,0.2)',
    },

    failure: {
        background: '#ff5549',
        textColor: '#fff',
        childClassName: 'notiflix-notify-failure',
        notiflixIconColor: 'rgba(0,0,0,0.2)',
        fontAwesomeClassName: 'fas fa-times-circle',
        fontAwesomeIconColor: 'rgba(0,0,0,0.2)',
        backOverlayColor: 'rgba(255,85,73,0.2)',
    },

    warning: {
        background: '#eebf31',
        textColor: '#fff',
        childClassName: 'notiflix-notify-warning',
        notiflixIconColor: 'rgba(0,0,0,0.2)',
        fontAwesomeClassName: 'fas fa-exclamation-circle',
        fontAwesomeIconColor: 'rgba(0,0,0,0.2)',
        backOverlayColor: 'rgba(238,191,49,0.2)',
    },

    info: {
        background: '#26c0d3',
        textColor: '#fff',
        childClassName: 'notiflix-notify-info',
        notiflixIconColor: 'rgba(0,0,0,0.2)',
        fontAwesomeClassName: 'fas fa-info-circle',
        fontAwesomeIconColor: 'rgba(0,0,0,0.2)',
        backOverlayColor: 'rgba(38,192,211,0.2)',
    },
});

Notiflix.Report.init({
    className: 'notiflix-report',
    width: '320px',
    backgroundColor: '#f8f8f8',
    borderRadius: '25px',
    rtl: false,
    zindex: 4002,
    backOverlay: true,
    backOverlayColor: 'rgba(0,0,0,0.5)',
    backOverlayClickToClose: false,
    fontFamily: 'Quicksand',
    svgSize: '110px',
    plainText: true,
    titleFontSize: '18px',
    titleMaxLength: 34,
    messageFontSize: '15px',
    messageMaxLength: 400,
    buttonFontSize: '14px',
    buttonMaxLength: 34,
    cssAnimation: true,
    cssAnimationDuration: 360,
    cssAnimationStyle: 'fade', // 'fade' - 'zoom'

    success: {
        svgColor: '#32c682',
        titleColor: '#1e1e1e',
        messageColor: '#242424',
        buttonBackground: '#32c682',
        buttonColor: '#fff',
        backOverlayColor: 'rgba(50,198,130,0.2)',
    },

    failure: {
        svgColor: '#ff5549',
        titleColor: '#1e1e1e',
        messageColor: '#242424',
        buttonBackground: '#ff5549',
        buttonColor: '#fff',
        backOverlayColor: 'rgba(255,85,73,0.2)',
    },

    warning: {
        svgColor: '#eebf31',
        titleColor: '#1e1e1e',
        messageColor: '#242424',
        buttonBackground: '#eebf31',
        buttonColor: '#fff',
        backOverlayColor: 'rgba(238,191,49,0.2)',
    },

    info: {
        svgColor: '#26c0d3',
        titleColor: '#1e1e1e',
        messageColor: '#242424',
        buttonBackground: '#26c0d3',
        buttonColor: '#fff',
        backOverlayColor: 'rgba(38,192,211,0.2)',
    },
});

Notiflix.Confirm.init({
    className: 'notiflix-confirm',
    width: '300px',
    zindex: 4003,
    position: 'center', // 'center' - 'center-top' - 'center-bottom' - 'right-top' - 'right-center' - 'right-bottom' - 'left-top' - 'left-center' - 'left-bottom'
    distance: '10px',
    backgroundColor: '#f8f8f8',
    borderRadius: '25px',
    backOverlay: true,
    backOverlayColor: 'rgba(0,0,0,0.5)',
    rtl: false,
    fontFamily: 'Quicksand',
    cssAnimation: true,
    cssAnimationDuration: 300,
    cssAnimationStyle: 'fade', // 'zoom' - 'fade'
    plainText: true,

    titleColor: '#9E1C64',
    titleFontSize: '18px',
    titleMaxLength: 34,

    messageColor: '#1e1e1e',
    messageFontSize: '16px',
    messageMaxLength: 300,

    buttonsFontSize: '15px',
    buttonsMaxLength: 34,
    okButtonColor: '#f8f8f8',
    okButtonBackground: '#9E1C64',
    cancelButtonColor: '#f8f8f8',
    cancelButtonBackground: '#a9a9a9',
});

Notiflix.Loading.init({
    className: 'notiflix-loading',
    zindex: 4000,
    backgroundColor: 'rgba(0,0,0,0.8)',
    rtl: false,
    fontFamily: 'Quicksand',
    cssAnimation: true,
    cssAnimationDuration: 400,
    clickToClose: false,
    customSvgUrl: null,
    customSvgCode: null,
    svgSize: '80px',
    svgColor: '#9E1C64',
    messageID: 'NotiflixLoadingMessage',
    messageFontSize: '16px',
    messageMaxLength: 34,
    messageColor: '#dcdcdc',
});

Notiflix.Block.init({
    querySelectorLimit: 200,
    className: 'notiflix-block',
    position: 'absolute',
    zindex: 1000,
    backgroundColor: 'rgba(255,255,255,0.9)',
    rtl: false,
    fontFamily: 'Quicksand',
    cssAnimation: true,
    cssAnimationDuration: 300,
    svgSize: '45px',
    svgColor: '#383838',
    messageFontSize: '16px',
    messageMaxLength: 34,
    messageColor: '#383838',
});

//Notify

function NotiflixNotifyAlert(message, type) {
    switch (type) {
        case "success":
            Notiflix.Notify.success(message);
            break;
        case "failure":
            Notiflix.Notify.failure(message);
            break;
        case "warning":
            Notiflix.Notify.warning(message);
            break;
        case "info":
            Notiflix.Notify.info(message);
            break;
    }
}

//Report

function NotiflixReportAlert(title, message, buttonText, width, svgSize, type, plainText) {
    switch (type) {
        case "success":
            Notiflix.Report.success(
                title,
                message,
                buttonText,
                {
                    width: width,
                    svgSize: svgSize,
                    plainText: plainText
                },
            );
            break;
        case "failure":
            Notiflix.Report.failure(
                title,
                message,
                buttonText,
                {
                    width: width,
                    svgSize: svgSize,
                    plainText: plainText
                },
            );
            break;
        case "warning":
            Notiflix.Report.warning(
                title,
                message,
                buttonText,
                {
                    width: width,
                    svgSize: svgSize,
                    plainText: plainText
                },
            );
            break;
        case "info":
            Notiflix.Report.info(
                title,
                message,
                buttonText,
                {
                    width: width,
                    svgSize: svgSize,
                    plainText: plainText
                },
            );
            break;
    }

    var notiflixElement = document.querySelector(".notiflix-report");
    notiflixElement.classList.add(type);
}

function NotiflixReportAlertCallback(title, message, buttonText, width, svgSize, type, plainText) {

    return new Promise((resolve) => {
        switch (type) {
            case "success":
                Notiflix.Report.success(
                    title,
                    message,
                    buttonText,
                    function cb() {
                        resolve(true);
                    },
                    {
                        width: width,
                        svgSize: svgSize,
                        plainText: plainText
                    },
                );
                break;
            case "failure":
                Notiflix.Report.failure(
                    title,
                    message,
                    buttonText,
                    function cb() {
                        resolve(true);
                    },
                    {
                        width: width,
                        svgSize: svgSize,
                        plainText: plainText
                    },
                );
                break;
            case "warning":
                Notiflix.Report.warning(
                    title,
                    message,
                    buttonText,
                    function cb() {
                        resolve(true);
                    },
                    {
                        width: width,
                        svgSize: svgSize,
                        plainText: plainText
                    },
                );
                break;
            case "info":
                Notiflix.Report.info(
                    title,
                    message,
                    buttonText,
                    function cb() {
                        resolve(true);
                    },
                    {
                        width: width,
                        svgSize: svgSize,
                        plainText: plainText
                    },
                );
                break;
        }
        var notiflixElement = document.querySelector(".notiflix-report");
        notiflixElement.classList.add(type);
    });
}

//Confirm

function NotiflixConfirmShow(title, message, okButtonText, cancelButtonText, width) {
    return new Promise((resolve) => {
        Notiflix.Confirm.show(
            title,
            message,
            okButtonText,
            cancelButtonText,
            function okCb() {
                resolve(true);
            },
            function cancelCb() {
                resolve(false);
            },
            {
                width: width,
            },
        );
    });
}


function NotiflixConfirmPrompt(title, question, okButtonText, cancelButtonText, defaultAnswer, width) {
    return new Promise((resolve) => {
        Notiflix.Confirm.prompt(
            title,
            question,
            defaultAnswer,
            okButtonText,
            cancelButtonText,
            function okCb(clientAnswer) {
                resolve({ success: true, message: clientAnswer });
            },
            function cancelCb(clientAnswer) {
                resolve({ success: false, message: clientAnswer });
            },
            {
                width: width,
            },
        );
    });
}

//Loading

function NotiflixLoading(text, type, svgSize) {
    switch (type) {
        case "standard":
            Notiflix.Loading.standard(text);
            break;
        case "hourglass":
            Notiflix.Loading.hourglass(text);
            break;
        case "circle":
            Notiflix.Loading.circle(text);
            break;
        case "arrows":
            Notiflix.Loading.arrows(text);
            break;
        case "dots":
            Notiflix.Loading.dots(text);
            break;
        case "pulse":
            Notiflix.Loading.pulse(text);
            break;
    }
}

function NotiflixRemoveLoading() {
    Notiflix.Loading.remove();
}

//Block
function NotiflixBlock(selector, text, type) {
    switch (type) {
        case "standard":
            Notiflix.Block.standard(selector, text);
            break;
        case "hourglass":
            Notiflix.Block.hourglass(selector, text);
            break;
        case "circle":
            Notiflix.Block.circle(selector, text);
            break;
        case "arrows":
            Notiflix.Block.arrows(selector, text);
            break;
        case "dots":
            Notiflix.Block.dots(selector, text);
            break;
        case "pulse":
            Notiflix.Block.pulse(selector, text);
            break;
    }
}

function NotiflixRemoveBlock(selector) {
    Notiflix.Block.remove(selector);
}