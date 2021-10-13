function initQuillInstance(isReadOnly, text, height, isDarkMode, containerID) {
    // Route Quill events following Uno's documentation
    // https://platform.uno/docs/articles/wasm-custom-events.html
    console.log('Quill Config Called');
    if (window.quill) {
        console.log('Quill available');
    } else {
        console.log('Quill not found');
        return;
    }

    var toolbarOptions = [
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
        ['blockquote', 'code-block'],

        [{ 'header': 1 }, { 'header': 2 }],               // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
        [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
        [{ 'direction': 'rtl' }],                         // text direction

        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        [{ 'font': [] }],
        [{ 'align': [] }],

        ['clean']                                         // remove formatting button
    ];

    var container = document.getElementById(containerID);
    var editor = new window.quill(container, {
        debug: "log",
        modules: {
            toolbar: toolbarOptions
        },
        placeholder: 'Compose an epic...',
        readOnly: false,
        theme: 'snow'
    });
    editor.enable();

    return editor;
}