using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DydelfGame;

public partial class EndGameWindow : Window
{
    public EndGameWindow(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
    }

    private void OK_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}