namespace Binance;

public partial class PageKolendar : ContentPage
{
	public PageKolendar()
	{
		InitializeComponent();

        WebView webView = new WebView();
        webView.Source = new UrlWebViewSource { Url = "https://coindar.org/ru" };


        FrameKolendar.Content = webView;
        webView.WidthRequest = 740;
        webView.HeightRequest = 680;
        webView.Margin = 0;
        webView.HorizontalOptions = LayoutOptions.FillAndExpand;
        webView.VerticalOptions = LayoutOptions.FillAndExpand;
    }
}