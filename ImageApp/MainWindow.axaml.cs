using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.IO;

namespace ImageApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void LoadImage_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Title = "Wybierz obraz";
        dialog.Filters.Add(new FileDialogFilter()
        {
            Name = "Obrazy",
            Extensions = { "bmp", "png", "jpg" }
        });

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            var filePath = result[0];

            using (var stream = File.OpenRead(filePath))
            {
                MainImage.Source = new Bitmap(stream);
            }
        }
    }
}