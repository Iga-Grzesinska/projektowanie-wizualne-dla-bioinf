using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DydelfGame;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Start_Click(object? sender, RoutedEventArgs e)
    {
        int width = (int)WidthInput.Value!;
        int height = (int)HeightInput.Value!;
        int time = (int)TimeInput.Value!;
        int dydelfy = (int)DydelfInput.Value!;
        int szopy = (int)SzopInput.Value!;
        int krokodyle = (int)KrokodylInput.Value!;

        var gameWindow = new GameWindow(width, height, time, dydelfy, szopy, krokodyle);
        await gameWindow.ShowDialog(this);
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}