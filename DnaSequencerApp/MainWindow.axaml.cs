using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Text;

namespace DnaSequencerApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ButtonClicked(object? sender, RoutedEventArgs e)
    {
        string dna = InputBox.Text?.ToUpper() ?? "";
        var counts = Count4Mers(dna);

        var sb = new StringBuilder();
        foreach (var kv in counts)
            sb.AppendLine($"{kv.Key}: {kv.Value}");

        OutputBox.Text = sb.ToString();
    }

    private Dictionary<string, int> Count4Mers(string dna)
    {
        var dict = new Dictionary<string, int>();

        for (int i = 0; i <= dna.Length - 4; i++)
        {
            string mer = dna.Substring(i, 4);

            if (!IsValid(mer))
                continue;

            if (!dict.ContainsKey(mer))
                dict[mer] = 0;

            dict[mer]++;
        }

        return dict;
    }

    private bool IsValid(string mer)
    {
        foreach (char c in mer)
            if (c is not ('A' or 'C' or 'G' or 'T'))
                return false;
        return true;
    }
}
