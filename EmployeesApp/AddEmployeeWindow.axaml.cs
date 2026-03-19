using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EmployeesApp;

public partial class AddEmployeeWindow : Window
{
    public string EmployeeImie { get; set; }
    public string EmployeeNazwisko { get; set; }
    public int EmployeeWiek { get; set; }
    public string EmployeeStanowisko { get; set; }
    public bool IsConfirmed { get; set; } = false;

    public AddEmployeeWindow()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object? sender, RoutedEventArgs e)
    {
        EmployeeImie = ImieTextBox.Text;
        EmployeeNazwisko = NazwiskoTextBox.Text;
        EmployeeWiek = int.Parse(WiekTextBox.Text);
        EmployeeStanowisko = StanowiskoComboBox.SelectedItem?.ToString();

        IsConfirmed = true;
        Close();
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        IsConfirmed = false;
        Close();
    }
}