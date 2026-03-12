using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace FoodApp;

public partial class TransportWindow : Window
{
    public ObservableCollection<string> Cart { get; set; }

    public TransportWindow()
    {
        InitializeComponent();
    }

    private void Save(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Czy rower?
        if (Bike.IsChecked == true)
            Cart.Add("Transport: dostawa rowerem +5 zł");

        // Czy samochód?
        if (Car.IsChecked == true)
            Cart.Add("Transport: dostawa samochodem +10 zł");

        // Czy ekspres?
        if (Express.IsChecked == true)
            Cart.Add("Transport: dostawa ekspresowa +15 zł");

        // Czy odbiór osobisty?
        if (Pickup.IsChecked == true)
            Cart.Add("Transport: odbiór osobisty (0 zł)");

        Close();
    }
}
