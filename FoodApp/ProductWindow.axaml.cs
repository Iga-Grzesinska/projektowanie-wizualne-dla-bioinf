using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace FoodApp;

public partial class ProductWindow : Window
{
    private ObservableCollection<string> _cart;

    public ProductWindow(ObservableCollection<string> cart)
    {
        InitializeComponent();
        _cart = cart;

        ProductList.Items = new string[]
        {
            "Pizza 25 zł",
            "Burger 18 zł",
            "Frytki 8 zł",
            "Sałatka 12 zł"
        };
    }

	private void AddProduct(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ProductList.SelectedItem is string item)
        {
            if (ExtraSauce.IsChecked == true)
                item += " + sos";

            _cart.Add(item);
        }

        Close();
    }
}
