(function () {
    const head = document.getElementsByTagName("head")[0];

    // Load Quill CSS from CDN
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = "//cdn.quilljs.com/1.3.6/quill.snow.css";
    head.appendChild(link);
})();

require([`${config.uno_app_base}/quill.src.js`], q => {
    window.quill = q;
    if (window.quill) {
        console.log('Quill loaded');
    }
});


