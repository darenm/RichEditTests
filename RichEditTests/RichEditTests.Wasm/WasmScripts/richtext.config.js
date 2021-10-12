var defaultStyle;
var IsReadOnly = false;
var Text = "";
var Height = 0;
var IsDarkMode = false;
var notifyViewModel;
var notifyUrlViewModel;
var uniqueContainerID;

function initAllControls(isReadOnly, text, height, isDarkMode) {
    configRichTextBoxForWASM(jQuery);

    IsReadOnly = isReadOnly;
    Text = text;
    Height = height;
    IsDarkMode = isDarkMode;

    $(document).on("click", ".richText-editor a", function (event) {
        event.preventDefault();
        notifyUrlViewModel($(this).attr('href'));
    });

    $('.content').richText({
        id: "richTextMessage",
        // uploads
        imageUpload: false,
        fileUpload: false,

        // media
        videoEmbed: false,
        // tables
        //table: false,

        // code
        removeStyles: false,
        code: false
    });

    defaultStyle = {
        bodyColor: $("body").css("background-color"),
        richTextBackgroundColor: $(".richText").css("background-color"),
        richTextColor: $(".richText").css("color"),
        richTextBorder: $(".richText").css("border"),

        richTextToolBarColor: $(".richText-toolbar").css("backgroundColor"),
        richTextToolBarBorder: $(".richText-toolbar").css("border-bottom"),

        richTextBtnBorder: $(".richText-btn").css("border-right"),
        richTextBtnHoverIn: "#EFEFEF",
        richTextBtnHoverOut: "#EFEFEF",

        richTextBorderRight: $(".richText .richText-undo, .richText .richText-redo").css("border-right"),

        richTextEditorColor: $(".richText-editor").css("background-color"),
        richTextEditorBorder: $(".richText-editor").css("border-left"),

        richTextDropDownBackground: $(".richText-dropdown").css("background-color"),
        richTextDropDownColor: $(".richText-dropdown").css("color"),
        richTextDropDownBorder: $(".richText-dropdown").css("border"),

        richTextBorderButtom: $(".richText .richText-toolbar ul li a .richText-dropdown-outer ul.richText-dropdown li a").css("border-bottom"),
        richTextBorderRight: $(".richText-toolbar ul li a").css("border-right"),

        richTextDropDownClose: $(".richText-dropdown-close").css("background-color"),
        richTextDropDownCloseColor: $(".richText-dropdown-close").css("color"),

        richTextControlBackground: $(".richText .richText-form input[type='text'], .richText .richText-form input[type='file'], .richText .richText-form input[type='number'], .richText .richText-form select").css("background-color"),
        richTextControlColor: $(".richText .richText-form input[type='text'], .richText .richText-form input[type='file'], .richText .richText-form input[type='number'], .richText .richText-form select").css("color")
    }


    $(".richText-editor").css("pointer-events", "all");
    $(".richText-toolbar").css("pointer-events", "all");

    var enableDarkMode = function () {

        var black = "#000000";
        var toolbarColor = "#1C1C1E";
        var textColor = "#CCCCCC";

        $("body").css("background-color", black);

        $(".richText").css("background-color", black);
        $(".richText").css("color", textColor);
        $(".richText").css("border", textColor);

        $(".richText-toolbar").css("backgroundColor", toolbarColor);
        $(".richText-toolbar").css("border-bottom", textColor);
        $(".richText-btn").css("border-right", textColor);
        $(".richText-btn").hover(function () {
            $(this).css("background-color", black);
        }, function () {
            $(this).css("background-color", toolbarColor);
        });
        $(".richText-btn").css("background-color", toolbarColor);

        $(".richText .richText-undo, .richText .richText-redo").css("border-right", toolbarColor);

        $(".richText-editor").css("background-color", black);
        $(".richText-editor").css("border-left", toolbarColor + " 2px solid");

        $(".richText-dropdown").css("background-color", black);
        $(".richText-dropdown").css("color", textColor);
        $(".richText-dropdown").css("border", toolbarColor + " 1px solid");

        $(".richText .richText-toolbar ul li a .richText-dropdown-outer ul.richText-dropdown li a").css("border-bottom", toolbarColor + " 1px solid");
        $(".richText-toolbar ul li a").css("border-right", toolbarColor + " 1px solid");

        $(".richText-dropdown-close").css("background-color", toolbarColor);
        $(".richText-dropdown-close").css("color", textColor);

        $(".richText .richText-form input[type='text'], .richText .richText-form input[type='file'], .richText .richText-form input[type='number'], .richText .richText-form select").css("background-color", toolbarColor);
        $(".richText .richText-form input[type='text'], .richText .richText-form input[type='file'], .richText .richText-form input[type='number'], .richText .richText-form select").css("color", textColor);

    }

    var disableDarkMode = function () {

        $("body").css("background-color", defaultStyle.bodyColor);

        $(".richText").css("background-color", defaultStyle.richTextBackgroundColor);
        $(".richText").css("color", defaultStyle.richTextColor);
        $(".richText").css("border", defaultStyle.richTextBorder);

        $(".richText-toolbar").css("backgroundColor", defaultStyle.richTextToolBarColor);
        $(".richText-toolbar").css("border-bottom", defaultStyle.richTextToolBarBorder);
        $(".richText-btn").css("border-right", defaultStyle.richTextBtnBorder);
        $(".richText-btn").hover(function () {
            $(this).css("background-color", defaultStyle.richTextBtnHoverIn);
        }, function () {
            $(this).css("background-color", defaultStyle.richTextBtnHoverOut);
        });
        $(".richText-btn").css("background-color", defaultStyle.richTextBtnHoverIn);

        $(".richText .richText-undo, .richText .richText-redo").css("border-right", defaultStyle.richTextBorderRight);

        $(".richText-editor").css("background-color", defaultStyle.richTextEditorColor);
        $(".richText-editor").css("border-left", defaultStyle.richTextEditorBorder);

        $(".richText-dropdown").css("background-color", defaultStyle.richTextDropDownBackground);
        $(".richText-dropdown").css("color", defaultStyle.richTextDropDownColor);
        $(".richText-dropdown").css("border", defaultStyle.richTextDropDownBorder);

        $(".richText .richText-toolbar ul li a .richText-dropdown-outer ul.richText-dropdown li a").css("border-bottom", defaultStyle.richTextBorderButtom);
        $(".richText-toolbar ul li a").css("border-right", defaultStyle.richTextBorderRight);

        $(".richText-dropdown-close").css("background-color", defaultStyle.richTextDropDownClose);
        $(".richText-dropdown-close").css("color", defaultStyle.richTextDropDownCloseColor);

        $(".richText .richText-form input[type='text'], .richText .richText-form input[type='file'], .richText .richText-form input[type='number'], .richText .richText-form select").css("background-color", defaultStyle.richTextControlBackground);
        $(".richText .richText-form input[type='text'], .richText .richText-form input[type='file'], .richText .richText-form input[type='number'], .richText .richText-form select").css("color", defaultStyle.richTextControlColor);

    }

    notifyViewModel = function () {
        var _htmlElement = document.getElementById("containerRichText");
        _htmlElement.dispatchEvent(new CustomEvent("complexEvent", { bubbles: true, detail: contentValue }));
    }

    notifyUrlViewModel = function (url) {
        var _htmlElement = document.getElementById("containerRichText");
        _htmlElement.dispatchEvent(new CustomEvent("urlEvent", { bubbles: true, detail: url }));
    }

    function setControl(isReadOnly, text, height, isDarkMode) {
        try {
            var i = 0;
            while ($(".richText-editor").length === 0 && i < 100) {
                i++
            }
            var content = $(".richText-editor");
            var toolbar = $(".richText-toolbar");
            content.attr('contenteditable', 'true');
            content.css("height", (height - 120).toString());
            toolbar.css("min-height", "40px");
            toolbar.css('display', 'block');

            //SetEditable
            //if (isReadOnly) {
            //    content.attr('contenteditable', 'false');
            //    toolbar.css('display', 'none');
            //    content.css("height", (height).toString());
            //}

            //if (isDarkMode) {
            //    enableDarkMode();
            //} else {
            //    disableDarkMode();
            //}

            //SetValue
            content.html(text)
        }
        catch (err) {
            console.log('asdasd' + err);
            //invokeCSharpAction(err)
        }

    };
}