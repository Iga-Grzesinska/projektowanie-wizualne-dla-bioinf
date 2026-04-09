using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Avalonia.Controls.Models;
using System.Text.Json;

namespace EmployeesApp;

public partial class MainWindow : Window
{
    public ObservableCollection<Employee> Employees { get; set; } = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        // PODPINANIE EVENTÓW (Avalonia 11)
        AddBtn.Click += AddEmployee_Click;
        RemoveBtn.Click += RemoveEmployee_Click;
        SaveCsvBtn.Click += SaveCSV_Click;
        LoadCsvBtn.Click += LoadCSV_Click;
        SaveXmlBtn.Click += SaveXML_Click;
        LoadXmlBtn.Click += LoadXML_Click;
    }

    private async void SaveXML_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Zapisz jako XML",
            Filters = { new FileDialogFilter { Name = "XML", Extensions = { "xml" } } }
        };

        var path = await dialog.ShowAsync(this);

        if (path != null)
        {
            var lista = Employees.ToList();
            var serializer = new XmlSerializer(typeof(List<Employee>));
            using var writer = new StreamWriter(path);
            serializer.Serialize(writer, lista);
        }
    }

    private async void LoadXML_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Wczytaj XML",
            AllowMultiple = false,
            Filters = { new FileDialogFilter { Name = "XML", Extensions = { "xml" } } }
        };

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            var path = result[0];
            var serializer = new XmlSerializer(typeof(List<Employee>));
            using var reader = new StreamReader(path);
            var lista = (List<Employee>)serializer.Deserialize(reader);
            Employees.Clear();
            foreach (var emp in lista)
            {
                Employees.Add(emp);
            }
        }
    }

    private async void AddEmployee_Click(object? sender, RoutedEventArgs e)
    {
        var addWindow = new AddEmployeeWindow();
        await addWindow.ShowDialog(this);
        if (addWindow.IsConfirmed)
        {
            var newEmployee = new Employee
            {
                Id = Employees.Any() ? Employees.Max(emp => emp.Id) + 1 : 1,
                Imie = addWindow.EmployeeImie,
                Nazwisko = addWindow.EmployeeNazwisko,
                Wiek = addWindow.EmployeeWiek,
                Stanowisko = addWindow.EmployeeStanowisko
            };
            Employees.Add(newEmployee);
        }
    }

    private void RemoveEmployee_Click(object? sender, RoutedEventArgs e)
    {
        if (EmployeesDataGrid.SelectedItem is Employee selectedEmployee)
        {
            Employees.Remove(selectedEmployee);
        }
    }

    private async void SaveCSV_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Zapisz jako CSV",
            Filters = { new FileDialogFilter { Name = "CSV", Extensions = { "csv" } } }
        };

        var path = await dialog.ShowAsync(this);

        if (path != null)
        {
            using var writer = new StreamWriter(path);
            writer.WriteLine("Id,Imie,Nazwisko,Wiek,Stanowisko");
            foreach (var emp in Employees)
            {
                writer.WriteLine($"{emp.Id},{emp.Imie},{emp.Nazwisko},{emp.Wiek},{emp.Stanowisko}");
            }
        }
    }

    private async void LoadCSV_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Wczytaj CSV",
            AllowMultiple = false,
            Filters = { new FileDialogFilter { Name = "CSV", Extensions = { "csv" } } }
        };

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            var path = result[0];
            Employees.Clear();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines.Skip(1)) // skip header
            {
                var parts = line.Split(',');
                if (parts.Length == 5)
                {
                    var emp = new Employee
                    {
                        Id = int.Parse(parts[0]),
                        Imie = parts[1],
                        Nazwisko = parts[2],
                        Wiek = int.Parse(parts[3]),
                        Stanowisko = parts[4]
                    };
                    Employees.Add(emp);
                }
            }
        }
    }

    private async void SaveJSON_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Zapisz jako JSON",
            Filters = { new FileDialogFilter { Name = "JSON", Extensions = { "json" } } }
        };

        var path = await dialog.ShowAsync(this);

        if (path != null)
        {
            var lista = Employees.ToList();
            string jsonString = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, jsonString);
        }
    }

    private async void LoadJSON_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Wczytaj JSON",
            AllowMultiple = false,
            Filters = { new FileDialogFilter { Name = "JSON", Extensions = { "json" } } }
        };

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            var path = result[0];
            var jsonString = File.ReadAllText(path);
            var lista = JsonSerializer.Deserialize<List<Employee>>(jsonString);
            if (lista != null)
            {
                Employees.Clear();
                foreach (var emp in lista)
                {
                    Employees.Add(emp);
                }
            }
        }
    }
}
