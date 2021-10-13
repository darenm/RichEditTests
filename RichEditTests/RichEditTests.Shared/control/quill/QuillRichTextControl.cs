using System;
using System.Collections.Generic;
using System.Text;
//using kahua.host.uno.utility;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System.Security.Cryptography;
//using kahua.ktree.control.text.richtext;
#if __WASM__
using Uno.Foundation;
using Uno.Extensions;
using Uno.UI.Runtime.WebAssembly;
#endif

namespace RichEditTests.control.quill
{
    public class QuillRichTextControl :  Control
    {
        private readonly string _containerID;

        public bool JSRichTextIsReadOnly { get; set; }
        public int JSRichTextHeight { get; set; }
        public bool JSRichTextDarkMode { get; set; }
        public event EventHandler<string> LinkInvoked = null;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
           "Value", typeof(string), typeof(QuillRichTextControl), new PropertyMetadata(null));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
#if __WASM__
        public QuillRichTextControl()
        {
            _containerID = $"quillRichText{CalculateNonce()}";
            this.SetHtmlContent($"<div id='{_containerID}' class='page-wrapper box-content'>Pointer Out</div>");
            Loaded += RichTextControl_Loaded;
        }


        private void RichTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            string reformatString = string.Empty;
            if (Value != null)
            {
                reformatString = Value.ToString().Replace("\"", "\'").Replace("\n", "").Replace("\r", "").Replace("\t", "");
            }
            var javascript = $"initQuillInstance({JSRichTextIsReadOnly.ToString().ToLower()}, \"{reformatString}\", {JSRichTextHeight}, {JSRichTextDarkMode.ToString().ToLower()}, \"{_containerID}\");";
            var result = this.ExecuteJavascript(javascript);
            //if (!JSRichTextIsReadOnly)
            //{
            //    this.RegisterHtmlCustomEventHandler("complexEvent", OnComplexEvent, isDetailJson: false);
            //}

            //this.RegisterHtmlCustomEventHandler("urlEvent", OnUrlEvent, isDetailJson: false);
        }

        private void OnComplexEvent(object sender, HtmlCustomEventArgs e)
        {
            Value = e.Detail;
        }

        private void OnUrlEvent(object sender, HtmlCustomEventArgs e)
        {
            LinkInvoked?.Invoke(this, e.Detail);
        }
#endif

        private static string CalculateNonce()
        {
            //Allocate a buffer
            var ByteArray = new byte[20];
            //Generate a cryptographically random set of bytes
            using (var Rnd = RandomNumberGenerator.Create())
            {
                Rnd.GetBytes(ByteArray);
            }
            //Base64 encode and then return
            return Convert.ToBase64String(ByteArray);
        }

    }
}
