
using Newtonsoft.Json;
using System.Net;
using System.Threading;
using RestSharp;
using Microcharts.Abstracts;
using Microsoft.Maui.Controls;

//using static Android.Icu.Text.CaseMap;

namespace Binance;

public partial class MainPage : ContentPage
{



    private Timer timer;
    private static readonly string Key = "42e3486c6ce449cca63ff0636b7d85d4";

    public class BitcoinPrice
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }



    }

    public MainPage()
	{
		InitializeComponent();
        CryptoPriceView();
        NewsView();
        timer = new Timer(MyFunction, null, 0, 60000);
        WebView webView = new WebView();
        webView.Source = new UrlWebViewSource { Url = "https://ru.tradingview.com/chart/" };


        FrameGrafic.Content = webView;
        webView.WidthRequest = 540;
        webView.HeightRequest = 480;
        webView.Margin = 0;
        webView.HorizontalOptions = LayoutOptions.FillAndExpand;
        webView.VerticalOptions = LayoutOptions.FillAndExpand;





    }
    int LastPriceBTC = 0;
    int LastPriceETH = 0;
    int lastPriceLTC = 0;

    int NewsNumber = 0;

    [Obsolete]

    private async void MyFunction(object state)
    {
        await Device.InvokeOnMainThreadAsync(() =>
        {
            using (var webClient = new WebClient())
            {
                var jsonBTC = webClient.DownloadString("https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT");
                var jsonETH = webClient.DownloadString("https://api.binance.com/api/v3/ticker/price?symbol=ETHUSDT");
                var jsonLTC = webClient.DownloadString("https://api.binance.com/api/v3/ticker/price?symbol=LTCUSDT");

                var bitcoinPrice = JsonConvert.DeserializeObject<BitcoinPrice>(jsonBTC);
                var etheriumPrice = JsonConvert.DeserializeObject<BitcoinPrice>(jsonETH);
                var ltcPrice = JsonConvert.DeserializeObject<BitcoinPrice>(jsonLTC);

                BTCprice.Text = Convert.ToString(Math.Round(bitcoinPrice.Price, 1)) + "$";
                ETHprice.Text = Convert.ToString(Math.Round(etheriumPrice.Price, 1)) + "$";
                LTCprice.Text = Convert.ToString(Math.Round(ltcPrice.Price, 1)) + "$";

                if (Convert.ToInt32(bitcoinPrice.Price) > LastPriceBTC)
                {
                    StrelkaBTC.Source = "strelkaV.png";
                }
                else { StrelkaBTC.Source = "strelkad.png"; }

                if (Convert.ToInt32(etheriumPrice.Price) > LastPriceETH)
                {
                    StrelkaETH.Source = "strelkaV.png";
                }
                else { StrelkaETH.Source = "strelkad.png"; }

                if (Convert.ToInt32(ltcPrice.Price) > lastPriceLTC)
                {
                    StrelkaLTC.Source = "strelkaV.png";
                }
                else { StrelkaLTC.Source = "strelkad.png"; }

                LastPriceBTC = Convert.ToInt32(bitcoinPrice.Price);
                LastPriceETH = Convert.ToInt32(etheriumPrice.Price);
                lastPriceLTC = Convert.ToInt32(ltcPrice.Price);
            }
        });
    }


    private void CryptoPriceView(){
        using (var webClient = new WebClient())
        {
            var jsonBTC = webClient.DownloadString("https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT");
            var jsonETH = webClient.DownloadString("https://api.binance.com/api/v3/ticker/price?symbol=ETHUSDT");
            var jsonLTC = webClient.DownloadString("https://api.binance.com/api/v3/ticker/price?symbol=LTCUSDT");

            var bitcoinPrice = JsonConvert.DeserializeObject<BitcoinPrice>(jsonBTC);
            var etheriumPrice = JsonConvert.DeserializeObject<BitcoinPrice>(jsonETH);
            var ltcPrice = JsonConvert.DeserializeObject<BitcoinPrice>(jsonLTC);




            BTCprice.Text = Convert.ToString(Math.Round(bitcoinPrice.Price, 1))+"$";
            ETHprice.Text = Convert.ToString(Math.Round(etheriumPrice.Price, 1))+"$";
            LTCprice.Text = Convert.ToString(Math.Round(ltcPrice.Price, 1)) + "$";
        }
    }
 
    private void NewsView()
    {
        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", "Криптовалюта");
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            NewsNumber++;
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }
    }
   public  string ActualNews = "Криптовалюта";
    public void OnButtonBackClicked(object sender, EventArgs e)
    {
        if (NewsNumber != 0)
        {
            NewsNumber--;
        }
        
        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", ActualNews);
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }
    }

    public void OnButtonNextClicked(object sender, EventArgs e)
    {
        NewsNumber++;
        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", ActualNews);
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }
    }
    private void OnButtonNews1Clicked(object sender, EventArgs e)
    {
        ActualNews = News1.Text;
        News1.Margin = new Thickness(5,0);

        News2.Margin = new Thickness(5, 5);
        News3.Margin = new Thickness(5, 5);
        News4.Margin = new Thickness(5, 5);
        News5.Margin = new Thickness(5, 5);
        News6.Margin = new Thickness(5, 5);

        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", "Криптовалюта");
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            NewsNumber++;
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }

    }
    private void OnButtonNews2Clicked(object sender, EventArgs e)
    {
        ActualNews = News2.Text;
        News1.Margin = new Thickness(5, 5);

        News2.Margin  = new Thickness(5,0);

        News3.Margin = new Thickness(5, 5);
        News4.Margin = new Thickness(5, 5);
        News5.Margin = new Thickness(5, 5);
        News6.Margin = new Thickness(5, 5);

        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", "Биржы");
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            NewsNumber++;
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }
    }
    private void OnButtonNews3Clicked(object sender, EventArgs e)
    {
        ActualNews = News3.Text;
        News1.Margin = new Thickness(5, 5);

        News2.Margin = new Thickness(5, 5);

        News3.Margin = new Thickness(5, 0);
        News4.Margin = new Thickness(5, 5);
        News5.Margin = new Thickness(5, 5);
        News6.Margin = new Thickness(5, 5);


        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", "Блокчейн");
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            NewsNumber++;
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }
    }
    private void OnButtonNews4Clicked(object sender, EventArgs e)
    {
        ActualNews = News4.Text;
        News1.Margin = new Thickness(5, 5);

        News2.Margin = new Thickness(5, 5);

        News3.Margin = new Thickness(5, 5);
        News4.Margin = new Thickness(5, 0);
        News5.Margin = new Thickness(5, 5);
        News6.Margin = new Thickness(5, 5);

        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", "Биткоин");
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            NewsNumber++;
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }
    }
    private void OnButtonNews5Clicked(object sender, EventArgs e)
    {
        ActualNews = News5.Text;
        News1.Margin = new Thickness(5, 5);

        News2.Margin = new Thickness(5, 5);

        News3.Margin = new Thickness(5, 5);
        News4.Margin = new Thickness(5, 5);
        News5.Margin = new Thickness(5, 0);
        News6.Margin = new Thickness(5, 5);

        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", "NFT");
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            NewsNumber++;
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }
    }
    private void OnButtonNews6Clicked(object sender, EventArgs e)
    {
        ActualNews = News6.Text;
        News1.Margin = new Thickness(5, 5);

        News2.Margin = new Thickness(5, 5);

        News3.Margin = new Thickness(5, 5);
        News4.Margin = new Thickness(5, 5);
        News5.Margin = new Thickness(5, 5);
        News6.Margin = new Thickness(5, 0);


        RestClient client = new RestClient("https://newsapi.org/v2/everything");

        RestRequest request = new RestRequest($"https://newsapi.org/v2/everything", Method.Get);
        request.AddHeader("X-Api-Key", Key);
        request.AddParameter("q", "IDO");
        RestResponse response = client.Execute(request);
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

        for (int i = NewsNumber; i < myDeserializedClass.articles.Count; i++)
        {
            NewsNumber++;
            Title.Text = myDeserializedClass.articles[i].title;
            Text1.Text = myDeserializedClass.articles[i].description;
            Image.Source = myDeserializedClass.articles[i].urlToImage;
            DataOfpublication.Text = Convert.ToString(myDeserializedClass.articles[i].publishedAt);
            break;

        }
    }


    public void OnButtonNextClicked()
    {

    }

    private void GoWallet(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PageWallet());
    }

    private void GoRobot(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PageRobot());
    }
    private void GoKolendar(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PageKolendar());
    }

    private void GoSettings(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PageSettings());
    }

    private void GoTrade(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PageTrade());
    }
}

