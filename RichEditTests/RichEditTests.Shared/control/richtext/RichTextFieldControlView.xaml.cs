//using kahua.host.uno.ui.controls.text.richtext;
//using kahua.host.uno.utility;
//using kahua.ktree.control.text.richtext;
//using kahua.ktree.hub.item.component;
//using kahua.ktree.view;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace kahua.host.uno.control.richtext
{
    public sealed partial class RichTextFieldControlView : UserControl
    {
        private bool _isPreview = false;
        private int _heightRichText = 300;
        private bool _isDarkMode = false;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
           "Value", typeof(string), typeof(RichTextFieldControlView), new PropertyMetadata(null));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        //private RichTextViewModel _richTextViewModels { get; set; }
        //public RichTextComponent _richTextComponents { get; set; }

        public RichTextFieldControlView()
        {
            this.InitializeComponent();
            Loaded += RichTextFieldControlView_Loaded;
            Unloaded += RichTextFieldControlView_Unloaded;

            DataContextChanged += RichTextFieldControlView_DataContextChanged;
        }

       

        private void RichTextFieldControlView_DataContextChanged(DependencyObject sender, DataContextChangedEventArgs args)
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
            //    this.SetBinding(RichTextFieldControlView.ValueProperty, dataContextBinding);

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
            //    this.SetBinding(RichTextFieldControlView.ValueProperty, dataContextBinding);
            //}
        }

        private void RichTextFieldControlView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
#if __WASM__
            var dataContextBinding = new Binding
            {
                Path = new PropertyPath("Value"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Source = this
            };
            RichTextControl richTextControl = new RichTextControl();
            richTextControl.Height = 300;
            richTextControl.JSRichTextIsReadOnly = _isPreview;
            richTextControl.JSRichTextHeight = _heightRichText;
            richTextControl.JSRichTextDarkMode = _isDarkMode;
            richTextControl.SetBinding(RichTextControl.ValueProperty, dataContextBinding);
            richTextControl.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            richTextControl.LinkInvoked += RichTextControl_LinkInvoked;
            container.Children.Add(richTextControl);
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

        private void RichTextControl_LinkInvoked(object sender, string e)
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
            if(Value != null)
            {
                Value = e.Value;
            }
        }

        private void RichTextFieldControlView_Unloaded(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            //_richTextViewModels = null;
            //_richTextComponents = null;
            _isPreview = false;
            _isDarkMode = false;
        }
    }
}