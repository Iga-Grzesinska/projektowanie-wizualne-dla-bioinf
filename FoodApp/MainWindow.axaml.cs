using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace FoodApp;

public partial class MainWindow : Window
{
    public ObservableCollection<string> Cart { get; set; } = new();

    public MainWindow()
    {
        InitializeComponent();
        CartList.ItemsSource = Cart;   // poprawione
    }

    private void OpenProductWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var win = new ProductWindow(Cart);
        win.Show();
    }

    private void OpenTransportWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var win = new TransportWindow();
        win.Cart = Cart;   // właściwość
        win.Show();
    }

    private void OpenPaymentWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var win = new PaymentWindow();
        win.SetCart(Cart);   // metoda
        win.Show();
    }
}
