using System;
using System.Collections.Generic;
using System.Text;
//using kahua.host.uno.utility;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
//using kahua.ktree.control.text.richtext;
#if __WASM__
using Uno.Foundation;
using Uno.Extensions;
using Uno.UI.Runtime.WebAssembly;
#endif

namespace kahua.host.uno.control.richtext
{
    public class RichTextControl : Control
    {
        public bool JSRichTextIsReadOnly { get; set; }
        public int JSRichTextHeight { get; set; }
        public bool JSRichTextDarkMode { get; set; }
        public event EventHandler<string> LinkInvoked = null;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
           "Value", typeof(string), typeof(RichTextControl), new PropertyMetadata(null));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
#if __WASM__
        public RichTextControl()
        {
            this.SetHtmlContent("<div id='containerRichText' class='page-wrapper box-content'><textarea class='content' name='example'></textarea></div>");
            Loaded += RichTextControl_Loaded;
        }

        private void RichTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            string reformatString = string.Empty;
            if (Value != null)
            {
                reformatString = Value.ToString().Replace("\"", "\'").Replace("\n", "").Replace("\r", "").Replace("\t", "");
            }
            var javascript = $"initAllControls({JSRichTextIsReadOnly.ToString().ToLower()}, \"{reformatString.ToString()}\", {JSRichTextHeight.ToString()}, {JSRichTextDarkMode.ToString().ToLower()});";
            this.ExecuteJavascript(javascript);
            if(!JSRichTextIsReadOnly)
            {
                this.RegisterHtmlCustomEventHandler("complexEvent", OnComplexEvent, isDetailJson: false);
            }

            this.RegisterHtmlCustomEventHandler("urlEvent", OnUrlEvent, isDetailJson: false);
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

    }

}
