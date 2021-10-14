using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using RichEditTests.control.quill;

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
            richTextControl.IsEnabled = true;
            container.Children.Add(richTextControl);
#else
            //WebView webView = new WebView();
            //webView.Source = new System.Uri("ms-appx-web:///control/richtext/page/index.html");
            //webView.Height = 300;
            //webView.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            //container.Children.Add(webView);
            //webView.LoadCompleted += WebView_LoadCompleted;
            //webView.ScriptNotify += WebView_ScriptNotify;
#endif
        }

        private void RichTextControl_LinkInvoked(object sender, string e)
        {
            //_richTextViewModels?.InvokeLinkCommand.Execute(e);
            //_richTextComponents?.InvokeLinkCommand.Execute(e);
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