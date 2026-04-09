using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using Avalonia.Controls.Models;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.Json;

namespace EmployeesApp;

public partial class MainWindow : Window
{
    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        Employees.Add(new Employee
        {
            Id = 1,
            Imie = "Jan",
            Nazwisko = "Kowalski",
            Wiek = 35,
            Stanowisko = "Szef",
        });

        Employees.Add(new Employee
        {
            Id = 2,
            Imie = "Maria",
            Nazwisko = "Kowalska",
            Wiek = 25,
            Stanowisko = "Żona Szefa",
        });

        Employees.Add(new Employee
        {
            Id = 3,
            Imie = "Karolina",
            Nazwisko = "Brzęczyszczykiewicz",
            Wiek = 30,
            Stanowisko = "Programista",
        });
    }

    private async void AddEmployee_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddEmployeeWindow();
        await window.ShowDialog(this);

        if (window.IsConfirmed)
        {
            int newId = Employees.Count > 0 ? Employees.Max(x => x.Id) + 1 : 1;

            Employees.Add(new Employee
            {
                Id = newId,
                Imie = window.EmployeeImie,
                Nazwisko = window.EmployeeNazwisko,
                Wiek = window.EmployeeWiek,
                Stanowisko = window.EmployeeStanowisko
            });
        }
    }

    private void RemoveEmployee_Click(object? sender, RoutedEventArgs e)
    {
        var selected = EmployeesDataGrid.SelectedItem as Employee;

        if (selected != null)
        {
            Employees.Remove(selected);
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

            foreach (var line in File.ReadAllLines(path))
            {
                var parts = line.Split(',');

                Employees.Add(new Employee
                {
                    Id = int.Parse(parts[0]),
                    Imie = parts[1],
                    Nazwisko = parts[2],
                    Wiek = int.Parse(parts[3]),
                    Stanowisko = parts[4]
                });
            }
        }
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));

            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, lista);
            }
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
            var lista = (List<Employee>?)serializer.Deserialize(reader);
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