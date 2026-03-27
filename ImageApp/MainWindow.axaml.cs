using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace ImageApp;

public partial class MainWindow : Window
{
    private WriteableBitmap? _currentImage;

    public MainWindow()
    {
        InitializeComponent();
    }

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
            var bmp = new Bitmap(result[0]);

            _currentImage = new WriteableBitmap(
                bmp.PixelSize,
                new Vector(96, 96),
                PixelFormat.Bgra8888);

            using (var src = bmp.Lock())
            using (var dst = _currentImage.Lock())
            {
                unsafe
                {
                    byte* s = (byte*)src.Address;
                    byte* d = (byte*)dst.Address;

                    int size = src.RowBytes * bmp.PixelSize.Height;

                    for (int i = 0; i < size; i++)
                        d[i] = s[i];
                }
            }

            MainImage.Source = _currentImage;
        }
    }

    private void Rotate90_Click(object sender, RoutedEventArgs e)
    {
        if (_currentImage == null) return;

        int w = _currentImage.PixelSize.Width;
        int h = _currentImage.PixelSize.Height;

        var rotated = new WriteableBitmap(
            new PixelSize(h, w),
            new Vector(96, 96),
            PixelFormat.Bgra8888);

        using (var src = _currentImage.Lock())
        using (var dst = rotated.Lock())
        {
            unsafe
            {
                byte* s = (byte*)src.Address;
                byte* d = (byte*)dst.Address;

                int srcStride = src.RowBytes;
                int dstStride = dst.RowBytes;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        byte* pixel = s + y * srcStride + x * 4;

                        int newX = h - 1 - y;
                        int newY = x;

                        byte* target = d + newY * dstStride + newX * 4;

                        target[0] = pixel[0];
                        target[1] = pixel[1];
                        target[2] = pixel[2];
                        target[3] = pixel[3];
                    }
                }
            }
        }

        _currentImage = rotated;
        MainImage.Source = _currentImage;
    }

    private void Invert_Click(object sender, RoutedEventArgs e)
    {
        if (_currentImage == null) return;

        var result = new WriteableBitmap(
            _currentImage.PixelSize,
            new Vector(96, 96),
            PixelFormat.Bgra8888);

        using (var src = _currentImage.Lock())
        using (var dst = result.Lock())
        {
            unsafe
            {
                byte* s = (byte*)src.Address;
                byte* d = (byte*)dst.Address;

                int size = src.RowBytes * _currentImage.PixelSize.Height;

                for (int i = 0; i < size; i += 4)
                {
                    d[i] = (byte)(255 - s[i]);
                    d[i + 1] = (byte)(255 - s[i + 1]);
                    d[i + 2] = (byte)(255 - s[i + 2]);
                    d[i + 3] = s[i + 3];
                }
            }
        }

        _currentImage = result;
        MainImage.Source = _currentImage;
    }

    private void Flip_Click(object sender, RoutedEventArgs e)
    {
        if (_currentImage == null) return;

        int w = _currentImage.PixelSize.Width;
        int h = _currentImage.PixelSize.Height;

        var result = new WriteableBitmap(
            _currentImage.PixelSize,
            new Vector(96, 96),
            PixelFormat.Bgra8888);

        using (var src = _currentImage.Lock())
        using (var dst = result.Lock())
        {
            unsafe
            {
                byte* s = (byte*)src.Address;
                byte* d = (byte*)dst.Address;

                int stride = src.RowBytes;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        byte* pixel = s + y * stride + x * 4;
                        byte* target = d + (h - 1 - y) * stride + x * 4;

                        target[0] = pixel[0];
                        target[1] = pixel[1];
                        target[2] = pixel[2];
                        target[3] = pixel[3];
                    }
                }
            }
        }

        _currentImage = result;
        MainImage.Source = _currentImage;
    }

    private void Green_Click(object sender, RoutedEventArgs e)
    {
        if (_currentImage == null) return;

        var result = new WriteableBitmap(
            _currentImage.PixelSize,
            new Vector(96, 96),
            PixelFormat.Bgra8888);

        using (var src = _currentImage.Lock())
        using (var dst = result.Lock())
        {
            unsafe
            {
                byte* s = (byte*)src.Address;
                byte* d = (byte*)dst.Address;

                int size = src.RowBytes * _currentImage.PixelSize.Height;

                for (int i = 0; i < size; i += 4)
                {
                    byte b = s[i];
                    byte g = s[i + 1];
                    byte r = s[i + 2];

                    bool isGreen = g > r && g > b;

                    if (isGreen)
                    {
                        d[i] = b;
                        d[i + 1] = g;
                        d[i + 2] = r;
                    }
                    else
                    {
                        d[i] = 0;
                        d[i + 1] = 0;
                        d[i + 2] = 0;
                    }

                    d[i + 3] = s[i + 3];
                }
            }
        }

        _currentImage = result;
        MainImage.Source = _currentImage;
    }
}