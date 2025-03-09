using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace microsoft_hackathon_roi_calculator.Mobile.View;

public class Dashboard : ContentPage
{
    ObservableCollection<SalesData> data = new();
    Random _random = new();
    private void ModifyData()
    {
        Debug.WriteLine("modificando data");

        if (data.Count > 0)
        {
            var lastItem = data[data.Count - 1];
            lastItem.Revenue = _random.Next(5000, 10000);
            lastItem.Expenses = _random.Next(3000, 5000);

            Debug.WriteLine(lastItem.Revenue.ToString());
        }

        Content = CreateGraphics();
    }

    public Dashboard()
    {

        data = new ObservableCollection<SalesData>
        {
            new SalesData("Jan", 5000, 3000),
            new SalesData("Fev", 6000, 3200),
            new SalesData("Mar", 5500, 3100),
            new SalesData("Abr", 7000, 3500),
            new SalesData("Mai", 8000, 4000),
            new SalesData("Jun", 7500, 3800)
        };

        var label = new Label
        {
            Text = "Ola mundo",

        };

     

        Content = CreateGraphics();


        var timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(2);
        timer.Tick += (s, e) => ModifyData();
        timer.Start();
    }

    Grid CreateGraphics()
    {
        // Gráfico de Colunas (Receita Mensal)
        var columnChart = new SfCartesianChart
        {
            Margin = 10,
            Title = new Label { Text = "Receita Mensal", FontSize = 16, HorizontalOptions = LayoutOptions.Center },
            XAxes = { new CategoryAxis() },
            YAxes = { new NumericalAxis() },
            Series =
                    {
                        new ColumnSeries
                        {
                            ItemsSource = data,
                            XBindingPath = nameof(SalesData.Month),
                            YBindingPath = nameof(SalesData.Revenue),
                            EnableTooltip = true
                        }
                    },
        };

        // Gráfico de Linhas (Despesas Mensais)
        var lineChart = new SfCartesianChart
        {
            Margin = 10,
            Title = new Label { Text = "Despesas Mensais", FontSize = 16, HorizontalOptions = LayoutOptions.Center },
            XAxes = { new CategoryAxis() },
            YAxes = { new NumericalAxis() },
            Series =
                    {
                        new LineSeries
                        {
                            ItemsSource = data,
                            XBindingPath = nameof(SalesData.Month),
                            YBindingPath = nameof(SalesData.Expenses),
                            EnableTooltip = true
                        }
                    }
        };

        // Gráfico Circular (Distribuição de Receita)
        var circularChart = new SfCircularChart
        {
            Margin = 10,
            Title = new Label { Text = "Distribuição de Receita", FontSize = 16, HorizontalOptions = LayoutOptions.Center },
            Series =
                    {
                        new PieSeries
                        {
                            ItemsSource = data,
                            XBindingPath = nameof(SalesData.Month),
                            YBindingPath = nameof(SalesData.Revenue),
                            EnableTooltip = true,
                            ExplodeOnTouch = true
                        }
                    }
        };


        var grid = new Grid
        {
            RowDefinitions = { new RowDefinition(), new RowDefinition(), new RowDefinition() },
            ColumnDefinitions = { new ColumnDefinition(), new ColumnDefinition() },

            Children = {
                circularChart,
                lineChart,
                columnChart
            }
        };

        grid.SetRow(circularChart, 0);
        grid.SetColumn(circularChart, 0);

        grid.SetRow(columnChart, 1);
        grid.SetColumn(circularChart, 0);

        grid.SetRow(lineChart, 1);
        grid.SetColumn(lineChart, 1);

        return grid;
    }
}

// Modelo de dados
public class SalesData : INotifyPropertyChanged
{
    private string _month;
    private double _revenue;
    private double _expenses;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Month
    {
        get => _month;
        set => SetProperty(ref _month, value);
    }

    public double Revenue
    {
        get => _revenue;
        set => SetProperty(ref _revenue, value);
    }

    public double Expenses
    {
        get => _expenses;
        set => SetProperty(ref _expenses, value);
    }


    public SalesData(string month, double revenue, double expenses)
    {
        Month = month;
        Revenue = revenue;
        Expenses = expenses;
    }


    protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}