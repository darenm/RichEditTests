using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace RichEditTests.control.quill
{
    public sealed partial class QuillRichTextFieldControlView : UserControl
    {
        private bool _isPreview = false;
        private int _heightRichText = 300;
        private bool _isDarkMode = false;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
           "Value", typeof(string), typeof(QuillRichTextFieldControlView), new PropertyMetadata(null));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public QuillRichTextFieldControlView()
        {
            this.InitializeComponent();

            Loaded += QuillRichTextFieldControlView_Loaded;
            Unloaded += QuillRichTextFieldControlView_Unloaded;

            DataContextChanged += QuillRichTextFieldControlView_DataContextChanged;
        }

        private void QuillRichTextFieldControlView_DataContextChanged(DependencyObject sender, DataContextChangedEventArgs args)
        {
            //if (args.NewValue is RichTextViewModel richTextViewModel && _richTextViewModels == null)
            //{
            //    _richTextViewModels = richTextViewModel;
            //    var dynamic = richTextViewModel.GetAncestor<DynamicViewViewModel>();
            //    if (dynamic?.Name.Contains("Preview") == true)
            //    {
            //        _isPreview = true;
            //    }
            //    var dataContextBinding = new Binding
            //    {
            //        Path = new PropertyPath("DataValue"),
            //        Mode = BindingMode.TwoWay,
            //        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            //        Source = _richTextViewModels
            //    };
            //    this.SetBinding(QuillRichTextFieldControlView.ValueProperty, dataContextBinding);

            //}
            //else if(args.NewValue is RichTextComponent richTextComponents && _richTextComponents == null)
            //{
            //    _richTextComponents = richTextComponents;

            //    if(!_richTextComponents.IsEditable)
            //    {
            //        _isPreview = true;
            //    }

            //    var dataContextBinding = new Binding
            //    {
            //        Path = new PropertyPath("Value"),
            //        Mode = BindingMode.TwoWay,
            //        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            //        Source = _richTextComponents
            //    };
            //    this.SetBinding(QuillRichTextFieldControlView.ValueProperty, dataContextBinding);
            //}
        }

        private void QuillRichTextFieldControlView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
#if __WASM__
            var dataContextBinding = new Binding
            {
                Path = new PropertyPath("Value"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Source = this
            };
            QuillRichTextControl QuillRichTextControl = new QuillRichTextControl();
            QuillRichTextControl.Height = 300;
            QuillRichTextControl.JSRichTextIsReadOnly = _isPreview;
            QuillRichTextControl.JSRichTextHeight = _heightRichText;
            QuillRichTextControl.JSRichTextDarkMode = _isDarkMode;
            QuillRichTextControl.SetBinding(QuillRichTextControl.ValueProperty, dataContextBinding);
            QuillRichTextControl.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            QuillRichTextControl.LinkInvoked += QuillRichTextControl_LinkInvoked;
            QuillRichTextControl.IsEnabled = true;
            container.Children.Add(QuillRichTextControl);
#else
            WebView webView = new WebView();
            webView.Source = new System.Uri("ms-appx-web:///control/richtext/page/index.html");
            webView.Height = 300;
            webView.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            container.Children.Add(webView);
            webView.LoadCompleted += WebView_LoadCompleted;
            webView.ScriptNotify += WebView_ScriptNotify;
#endif
        }

        private void QuillRichTextControl_LinkInvoked(object sender, string e)
        {
            //_richTextViewModels?.InvokeLinkCommand.Execute(e);
            //_richTextComponents?.InvokeLinkCommand.Execute(e);
        }

        private void WebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            executeJSRequestAsync(sender as WebView);
        }

        public async void executeJSRequestAsync(WebView webView)
        {
            string reformatString = string.Empty;
            if (Value != null)
            {
                reformatString = Value.ToString().Replace("\"", "\'").Replace("\n", "").Replace("\r", "").Replace("\t", "");
            }

            var parameters = new string[] { _isPreview.ToString().ToLower(), reformatString, _heightRichText.ToString(), _isDarkMode.ToString().ToLower() };

            await webView.InvokeScriptAsync("setControl", parameters);
        }

        private void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (Value != null)
            {
                Value = e.Value;
            }
        }

        private void QuillRichTextFieldControlView_Unloaded(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            //_richTextViewModels = null;
            //_richTextComponents = null;
            _isPreview = false;
            _isDarkMode = false;
        }
    }
}

