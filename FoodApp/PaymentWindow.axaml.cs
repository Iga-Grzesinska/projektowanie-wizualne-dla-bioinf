using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace FoodApp;

public partial class PaymentWindow : Window
{
    private ObservableCollection<string> _cart;

    public PaymentWindow()
    {
        InitializeComponent();
    }

    public void SetCart(ObservableCollection<string> cart)
    {
        _cart = cart;
    }

    private void Pay(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string method = PayCash.IsChecked == true ? "Gotówka" :
                        PayCard.IsChecked == true ? "Karta" :
                        "Online";

        var msg = string.Join("\n", _cart);

        var dialog = new Window
        {
            Width = 300,
            Height = 200,
            Content = new TextBlock
            {
                Text = $"Zamówienie:\n{msg}\n\nPłatność: {method}",
                Margin = new Thickness(20)
            }
        };

        dialog.Show();
    }
}
