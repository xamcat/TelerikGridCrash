using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TelerikMauiGridResizeCrash;

public partial class DataGridView : ContentView, IToolView
{
	public DataGridView()
	{
		InitializeComponent();
        this.BindingContext = new StocksViewModel();
    }
}



public class StocksViewModel : NotifyPropertyChangedBase
{
    public ObservableCollection<StockSummaryViewModel> Stocks { get; } = new ObservableCollection<StockSummaryViewModel>();

    private string _someTestProperty = "Initial value";
    public string SomeTestProperty { get => _someTestProperty; set => RaiseAndUpdate(ref _someTestProperty, value); }

    public StocksViewModel()
    {
        Init();
    }

    public async Task Init()
    {
        GenerateStocks();
        RefreshStocksAsync();
        UpdateSomeTestPropertyPeriodically();
    }

    private void GenerateStocks()
    {
        for (int i = 0; i < 1000; i++)
        {
            Stocks.Add(new StockSummaryViewModel(new StockSummary($"Stock {i}")));
        }
    }

    private async Task RefreshStocksAsync()
    {
        var random = new Random();
        while (true)
        {
            await Task.Delay(10);
            for (var i = 0; i < Stocks.Count; i++)
            {
                Stocks[i].NetChange = Math.Round(random.NextDouble(), 2);
                Stocks[i].NetChange = Math.Round(random.NextDouble(), 2);
                Stocks[i].PercentageChange = Math.Round(random.NextDouble(), 2);
                Stocks[i].YearHigh = Math.Round(random.NextDouble(), 2);
                Stocks[i].YearLow = Math.Round(random.NextDouble(), 2);
                Stocks[i].OpenPrice = Math.Round(random.NextDouble(), 2);
                Stocks[i].PreviousClose = Math.Round(random.NextDouble(), 2);
                Stocks[i].TradeSize = Math.Round(random.NextDouble(), 2);
                Stocks[i].AverageVolume10Day = Math.Round(random.NextDouble(), 2);
                Stocks[i].AverageVolume90Day = Math.Round(random.NextDouble(), 2);
                Stocks[i].BlockTradeCumulativeVolume = Math.Round(random.NextDouble(), 2);
                Stocks[i].PeRatio = Math.Round(random.NextDouble(), 2);
                Stocks[i].Eps = Math.Round(random.NextDouble(), 2);
                Stocks[i].DividentYield = Math.Round(random.NextDouble(), 2);
                Stocks[i].AskPrice = Math.Round(random.NextDouble(), 2);
                Stocks[i].BidPrice = Math.Round(random.NextDouble(), 2);
                Stocks[i].HighPrice = Math.Round(random.NextDouble(), 2);
                Stocks[i].LowPrice = Math.Round(random.NextDouble(), 2);
                Stocks[i].AskPrice = Math.Round(random.NextDouble(), 2);
                Stocks[i].CumulativeVolume = Math.Round(random.NextDouble(), 2);
            }
        }
    }

    private async Task UpdateSomeTestPropertyPeriodically()
    {
        while (true)
        {
            await Task.Delay(1000);
            SomeTestProperty = $"New value: {Environment.TickCount}";
        }
    }
}


public class StockSummaryViewModel : NotifyPropertyChangedBase
{
    private double _netChange;
    private double _percentageChange;
    private double _yearHigh;
    private double _yearLow;
    private double _openPrice;
    private double _previousClose;
    private double _tradeSize;
    private double _averageVolume10Day;
    private double _averageVolume90Day;
    private double _blockTradeCount;
    private double _blockTradeCumulativeVolume;
    private double _peRatio;
    private double _eps;
    private double _dividentYield;
    private double _askPrice;
    private double _bidPrice;
    private double _highPrice;
    private double _lowPrice;
    private double _lastPrice;
    private double _cumulativeVolume;

    public string Symbol { get; }
    public double NetChange { get => _netChange; set => RaiseAndUpdate(ref _netChange, value); }
    public double PercentageChange { get => _percentageChange; set => RaiseAndUpdate(ref _percentageChange, value); }
    public double YearHigh { get => _yearHigh; set => RaiseAndUpdate(ref _yearHigh, value); }
    public double YearLow { get => _yearLow; set => RaiseAndUpdate(ref _yearLow, value); }
    public double OpenPrice { get => _openPrice; set => RaiseAndUpdate(ref _openPrice, value); }
    public double PreviousClose { get => _previousClose; set => RaiseAndUpdate(ref _previousClose, value); }
    public double TradeSize { get => _tradeSize; set => RaiseAndUpdate(ref _tradeSize, value); }
    public double AverageVolume10Day { get => _averageVolume10Day; set => RaiseAndUpdate(ref _averageVolume10Day, value); }
    public double AverageVolume90Day { get => _averageVolume90Day; set => RaiseAndUpdate(ref _averageVolume90Day, value); }
    public double BlockTradeCount { get => _blockTradeCount; set => RaiseAndUpdate(ref _blockTradeCount, value); }
    public double BlockTradeCumulativeVolume { get => _blockTradeCumulativeVolume; set => RaiseAndUpdate(ref _blockTradeCumulativeVolume, value); }
    public double PeRatio { get => _peRatio; set => RaiseAndUpdate(ref _peRatio, value); }
    public double Eps { get => _eps; set => RaiseAndUpdate(ref _eps, value); }
    public double DividentYield { get => _dividentYield; set => RaiseAndUpdate(ref _dividentYield, value); }
    public double AskPrice { get => _askPrice; set => RaiseAndUpdate(ref _askPrice, value); }
    public double BidPrice { get => _bidPrice; set => RaiseAndUpdate(ref _bidPrice, value); }
    public double HighPrice { get => _highPrice; set => RaiseAndUpdate(ref _highPrice, value); }
    public double LowPrice { get => _lowPrice; set => RaiseAndUpdate(ref _lowPrice, value); }
    public double LastPrice { get => _lastPrice; set => RaiseAndUpdate(ref _lastPrice, value); }
    public double CumulativeVolume { get => _cumulativeVolume; set => RaiseAndUpdate(ref _cumulativeVolume, value); }

    public StockSummaryViewModel(StockSummary stockSummary)
    {
        Symbol = stockSummary.Symbol;
        Update(stockSummary);
    }

    public void Update(StockSummary stockSummary)
    {
        // We check has value to avoid overiding of previous value in case delta updates
        if (stockSummary.NetChange.HasValue)
        {
            NetChange = stockSummary.NetChange.Value;
        }
        if (stockSummary.PercentageChange.HasValue)
        {
            PercentageChange = stockSummary.PercentageChange.Value;
        }
        if (stockSummary.YearHigh.HasValue)
        {
            YearHigh = stockSummary.YearHigh.Value;
        }
        if (stockSummary.YearLow.HasValue)
        {
            YearLow = stockSummary.YearLow.Value;
        }
        if (stockSummary.OpenPrice.HasValue)
        {
            OpenPrice = stockSummary.OpenPrice.Value;
        }
        if (stockSummary.PreviousClose.HasValue)
        {
            PreviousClose = stockSummary.PreviousClose.Value;
        }
        if (stockSummary.TradeSize.HasValue)
        {
            TradeSize = stockSummary.TradeSize.Value;
        }
        if (stockSummary.AverageVolume10Day.HasValue)
        {
            AverageVolume10Day = stockSummary.AverageVolume10Day.Value;
        }
        if (stockSummary.AverageVolume90Day.HasValue)
        {
            AverageVolume90Day = stockSummary.AverageVolume90Day.Value;
        }
        if (stockSummary.BlockTradeCount.HasValue)
        {
            BlockTradeCount = stockSummary.BlockTradeCount.Value;
        }
        if (stockSummary.BlockTradeCumulativeVolume.HasValue)
        {
            BlockTradeCumulativeVolume = stockSummary.BlockTradeCumulativeVolume.Value;
        }
        if (stockSummary.PeRatio.HasValue)
        {
            PeRatio = stockSummary.PeRatio.Value;
        }
        if (stockSummary.Eps.HasValue)
        {
            Eps = stockSummary.Eps.Value;
        }
        if (stockSummary.DividentYield.HasValue)
        {
            DividentYield = stockSummary.DividentYield.Value;
        }
        if (stockSummary.AskPrice.HasValue)
        {
            AskPrice = stockSummary.AskPrice.Value;
        }
        if (stockSummary.BidPrice.HasValue)
        {
            BidPrice = stockSummary.BidPrice.Value;
        }
        if (stockSummary.HighPrice.HasValue)
        {
            HighPrice = stockSummary.HighPrice.Value;
        }
        if (stockSummary.LowPrice.HasValue)
        {
            LowPrice = stockSummary.LowPrice.Value;
        }
        if (stockSummary.LastPrice.HasValue)
        {
            LastPrice = stockSummary.LastPrice.Value;
        }
        if (stockSummary.CumulativeVolume.HasValue)
        {
            CumulativeVolume = stockSummary.CumulativeVolume.Value;
        }
    }
}

public class StockSummary
{
    public string Symbol { get; }
    public double? NetChange { get; set; }
    public double? PercentageChange { get; set; }
    public double? YearHigh { get; set; }
    public double? YearLow { get; set; }
    public double? OpenPrice { get; set; }
    public double? PreviousClose { get; set; }
    public double? TradeSize { get; set; }
    public double? AverageVolume10Day { get; set; }
    public double? AverageVolume90Day { get; set; }
    public double? BlockTradeCount { get; set; }
    public double? BlockTradeCumulativeVolume { get; set; }
    public double? PeRatio { get; set; }
    public double? Eps { get; set; }
    public double? DividentYield { get; set; }
    public double? AskPrice { get; set; }
    public double? BidPrice { get; set; }
    public double? HighPrice { get; set; }
    public double? LowPrice { get; set; }
    public double? LastPrice { get; set; }
    public double? CumulativeVolume { get; set; }

    public StockSummary(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentNullException(nameof(symbol));

        Symbol = symbol;
    }
}

public class NotifyPropertyChangedBase : INotifyPropertyChanged
{
    readonly Dictionary<string, List<Action>> _propertyWatchers = new Dictionary<string, List<Action>>();

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises PropertyChanged after updating the backing property with the specified value.
    /// </summary>
    /// <returns><c>true</c>, if and update was raised, <c>false</c> otherwise.</returns>
    /// <param name="field">Field.</param>
    /// <param name="value">Value.</param>
    /// <param name="propertyName">Property name.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected bool RaiseAndUpdate<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
#pragma warning disable CS8604 // Possible null reference argument.
        Raise(propertyName);
#pragma warning restore CS8604 // Possible null reference argument.

        return true;
    }

    /// <summary>
    /// Raises PropertyChanged after updating the backing property with the specified value.
    /// </summary>
    /// <returns><c>true</c>, if and update was raised, <c>false</c> otherwise.</returns>
    /// <param name="shouldRaiseFunc">Func determining whether the backing property should be updated and PropertyChanged should be raised.</param>
    /// <param name="updateFieldAction">Action for updating the backing property.</param>
    /// <param name="propertyName">Property name.</param>
    protected bool RaiseAndUpdate(Func<bool> shouldRaiseFunc, Action updateFieldAction, [CallerMemberName] string propertyName = null)
    {
        if (!shouldRaiseFunc())
            return false;

        updateFieldAction();
        Raise(propertyName);

        return true;
    }

    /// <summary>
    /// Raises PropertyChanged for the a named property.
    /// </summary>
    /// <param name="propertyName">Property name.</param>
    protected void Raise(string propertyName)
    {
        if (!string.IsNullOrEmpty(propertyName) && PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        if (!_propertyWatchers.ContainsKey(propertyName))
            return;

        var watchers = _propertyWatchers[propertyName];

        foreach (Action watcher in watchers)
            watcher();
    }

    /// <summary>
    /// Watch a property and execute an action when it broadcasts a change notification.
    /// </summary>
    /// <param name="propertyName">Property name.</param>
    /// <param name="action">Action.</param>
    public void WatchProperty(string propertyName, Action action)
    {
        if (!_propertyWatchers.ContainsKey(propertyName))
        {
            _propertyWatchers[propertyName] = new List<Action>();
        }

        _propertyWatchers[propertyName].Add(action);
    }

    /// <summary>
    /// Clears all property watchers.
    /// </summary>
    public void ClearWatchers()
    {
        _propertyWatchers.Clear();
    }
}