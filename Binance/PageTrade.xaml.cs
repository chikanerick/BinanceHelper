using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Binance;

public partial class PageTrade : ContentPage
{
    private ObservableCollection<Cryptocurrency> _cryptocurrencies = new ObservableCollection<Cryptocurrency>();
    public ObservableCollection<Cryptocurrency> Cryptocurrencies
    {
        get { return _cryptocurrencies; }
        set { _cryptocurrencies = value; }
    }

    public PageTrade()
	{


        


        InitializeComponent();
        LoadCryptocurrencies();


    }


    private async void LoadCryptocurrencies()
    {
        double pr;
        double percent24h;
        string apiKey = "ca5759b6-720b-4fc6-a149-7c123b7f4bed";
        string url = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?limit=30&convert=USD";

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        dynamic result = JsonConvert.DeserializeObject(json);

        foreach (var coin in result.data)
        {
            pr = coin.quote.USD.price;

            percent24h = coin.quote.USD.percent_change_24h;


            Cryptocurrency cryptocurrency = new Cryptocurrency();
            cryptocurrency.Name = coin.name;
            cryptocurrency.LogoUrl = "https://s2.coinmarketcap.com/static/img/coins/64x64/" + coin.id + ".png";
            cryptocurrency.Price = Math.Round(pr, 5) ;
            cryptocurrency.PercentChange24h = Math.Round(percent24h, 3); ;
            Cryptocurrencies.Add(cryptocurrency);


            
            
        }

        MyCollectionView.ItemsSource = Cryptocurrencies;
    }
}
