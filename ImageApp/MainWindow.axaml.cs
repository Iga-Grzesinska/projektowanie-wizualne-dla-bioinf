using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.IO;
using System;

namespace ImageApp;

public partial class MainWindow : Window
{
    private Bitmap? _currentImage;

    public MainWindow()
    {
        InitializeComponent();
    }

    // 🔹 LOAD IMAGE
    private async void LoadImage_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Title = "Wybierz obraz";
        dialog.Filters.Add(new FileDialogFilter
        {
            Name = "Images",
            Extensions = { "bmp", "png", "jpg" }
        });

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            _currentImage = new Bitmap(result[0]);
            MainImage.Source = _currentImage;
        }
    }

    // 🔹 ROTATE 90°
    private void Rotate90_Click(object sender, RoutedEventArgs e)
    {
        if (_currentImage == null) return;

        int w = _currentImage.PixelSize.Width;
        int h = _currentImage.PixelSize.Height;

        var rotated = new WriteableBitmap(
            new Avalonia.PixelSize(h, w),
            new Avalonia.Vector(96, 96),
            Avalonia.Platform.PixelFormat.Bgra8888);

        using (var fb1 = _currentImage.Lock())
        using (var fb2 = rotated.Lock())
        {
            unsafe
            {
                byte* src = (byte*)fb1.Address;
                byte* dst = (byte*)fb2.Address;

                int srcStride = fb1.RowBytes;
                int dstStride = fb2.RowBytes;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        byte* pixel = src + y * srcStride + x * 4;

                        int newX = h - 1 - y;
                        int newY = x;

                        byte* target = dst + newY * dstStride + newX * 4;

                        target[0] = pixel[0]; // B
                        target[1] = pixel[1]; // G
                        target[2] = pixel[2]; // R
                        target[3] = pixel[3]; // A
                    }
                }
            }
        }

        _currentImage = rotated;
        MainImage.Source = _currentImage;
    }
}