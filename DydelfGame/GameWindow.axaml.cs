using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace DydelfGame;

public partial class GameWindow : Window
{
    private int _width, _height, _time, _dydelfy, _szopy, _krokodyle;
    private int _dydelfyLeft;
    private int _secondsLeft;
    private DispatcherTimer _timer;
    private Button[,] _buttons;
    private string[,] _board;
    private bool _gameOver = false;

    public GameWindow(int width, int height, int time, int dydelfy, int szopy, int krokodyle)
    {
        InitializeComponent();

        _width = width;
        _height = height;
        _time = time;
        _secondsLeft = time;
        _dydelfy = dydelfy;
        _dydelfyLeft = dydelfy;
        _szopy = szopy;
        _krokodyle = krokodyle;

        _buttons = new Button[height, width];
        _board = new string[height, width];

        GenerateBoard();
        BuildGrid();
        StartTimer();
    }

    private void GenerateBoard()
    {
        // Wypełnij pustymi polami
        for (int r = 0; r < _height; r++)
            for (int c = 0; c < _width; c++)
                _board[r, c] = "empty";

        var rng = new Random();
        PlaceRandom("dydelf", _dydelfy, rng);
        PlaceRandom("szop", _szopy, rng);
        PlaceRandom("krokodyl", _krokodyle, rng);
    }

    private void PlaceRandom(string type, int count, Random rng)
    {
        int placed = 0;
        while (placed < count)
        {
            int r = rng.Next(_height);
            int c = rng.Next(_width);
            if (_board[r, c] == "empty")
            {
                _board[r, c] = type;
                placed++;
            }
        }
    }

    private void BuildGrid()
    {
        GameGrid.RowDefinitions.Clear();
        GameGrid.ColumnDefinitions.Clear();

        for (int r = 0; r < _height; r++)
            GameGrid.RowDefinitions.Add(new RowDefinition(60, GridUnitType.Pixel));
        for (int c = 0; c < _width; c++)
            GameGrid.ColumnDefinitions.Add(new ColumnDefinition(60, GridUnitType.Pixel));

        for (int r = 0; r < _height; r++)
        {
            for (int c = 0; c < _width; c++)
            {
                var btn = new Button
                {
                    Content = "🗑️",
                    FontSize = 20,
                    Width = 55,
                    Height = 55,
                    Background = Brushes.Gray,
                    Tag = $"{r},{c}"
                };
                btn.Click += Cell_Click;
                Grid.SetRow(btn, r);
                Grid.SetColumn(btn, c);
                GameGrid.Children.Add(btn);
                _buttons[r, c] = btn;
            }
        }
    }

    private async void Cell_Click(object? sender, RoutedEventArgs e)
    {
        if (_gameOver) return;

        var btn = sender as Button;
        var parts = btn!.Tag!.ToString()!.Split(',');
        int r = int.Parse(parts[0]);
        int c = int.Parse(parts[1]);

        string content = _board[r, c];

        if (content == "empty")
        {
            btn.Content = "⬜";
            btn.IsEnabled = false;
            btn.Background = Brushes.LightGray;
        }
        else if (content == "dydelf")
        {
            btn.Content = "🐾";
            btn.Background = Brushes.LightGreen;
            btn.IsEnabled = false;
            _board[r, c] = "found";
            _dydelfyLeft--;

            if (_dydelfyLeft == 0)
            {
                EndGame(true);
            }
        }
        else if (content == "szop")
        {
            btn.Content = "🦝";
            btn.Background = Brushes.Orange;
            btn.IsEnabled = false;
            _board[r, c] = "found";

            // Szop zamyka siebie i sąsiadów po 2 sekundach
            await System.Threading.Tasks.Task.Delay(2000);
            CoverNeighbors(r, c);
        }
        else if (content == "krokodyl")
        {
            btn.Content = "🐊";
            btn.Background = Brushes.Red;
            _board[r, c] = "krokodyl_active";

            // Gracz ma 2 sekundy na ponowne kliknięcie
            await System.Threading.Tasks.Task.Delay(2000);

            if (_board[r, c] == "krokodyl_active")
            {
                EndGame(false, "Krokodyl Cię zjadł! 🐊");
            }
        }
        else if (content == "krokodyl_active")
        {
            // Gracz zdążył kliknąć
            btn.Content = "✅";
            btn.Background = Brushes.LightGreen;
            btn.IsEnabled = false;
            _board[r, c] = "found";
        }
    }

    private void CoverNeighbors(int r, int c)
    {
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                int nr = r + dr;
                int nc = c + dc;
                if (nr >= 0 && nr < _height && nc >= 0 && nc < _width)
                {
                    if (_board[nr, nc] != "found" && _board[nr, nc] != "krokodyl_active")
                    {
                        _buttons[nr, nc].Content = "🗑️";
                        _buttons[nr, nc].Background = Brushes.Gray;
                        _buttons[nr, nc].IsEnabled = true;
                        _board[nr, nc] = _board[nr, nc]; // zostawia zawartość
                    }
                }
            }
        }
    }

    private void StartTimer()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (s, e) =>
        {
            _secondsLeft--;
            TimerText.Text = $"Czas: {_secondsLeft}";

            if (_secondsLeft <= 0)
            {
                _timer.Stop();
                EndGame(false, "Czas minął! ⏰");
            }
        };
        _timer.Start();
    }

    private async void EndGame(bool won, string message = "")
    {
        if (_gameOver) return;
        _gameOver = true;
        _timer?.Stop();

        string msg = won ? "Wygrałeś! Znalazłeś wszystkie Dydelfy! 🎉" : message;

        var dialog = new EndGameWindow(msg);
        await dialog.ShowDialog(this);
        Close();
    }
}