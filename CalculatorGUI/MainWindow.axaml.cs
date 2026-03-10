using Avalonia.Controls;
using Avalonia.Interactivity;
using CalculatorLibrary;

namespace CalculatorGUI;

public partial class MainWindow : Window
{
    Calculator calc = new Calculator();

    public MainWindow()
    {
        InitializeComponent();
    }

    double GetInput1() => double.Parse(Input1.Text);
    double GetInput2() => double.Parse(Input2.Text);

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        ResultText.Text = calc.Add(GetInput1(), GetInput2()).ToString();
    }

    private void Sub_Click(object sender, RoutedEventArgs e)
    {
        ResultText.Text = calc.Subtract(GetInput1(), GetInput2()).ToString();
    }

    private void Mul_Click(object sender, RoutedEventArgs e)
    {
        ResultText.Text = calc.Multiply(GetInput1(), GetInput2()).ToString();
    }

    private void Div_Click(object sender, RoutedEventArgs e)
    {
        ResultText.Text = calc.Divide(GetInput1(), GetInput2()).ToString();
    }
}