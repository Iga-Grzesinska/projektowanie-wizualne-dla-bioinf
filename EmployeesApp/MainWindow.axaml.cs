using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

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
    }

    private void SaveXML_Click(object? sender, RoutedEventArgs e)
    {
        var lista = Employees.ToList();

        var serializer = new XmlSerializer(typeof(List<Employee>));
        using var writer = new StreamWriter("pracownicy.xml");
        serializer.Serialize(writer, lista);
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

    private void SaveCSV_Click(object? sender, RoutedEventArgs e)
    {
        using var writer = new StreamWriter("employees.csv");
        writer.WriteLine("Id,Imie,Nazwisko,Wiek,Stanowisko");
        foreach (var emp in Employees)
        {
            writer.WriteLine($"{emp.Id},{emp.Imie},{emp.Nazwisko},{emp.Wiek},{emp.Stanowisko}");
        }
    }

    private void LoadCSV_Click(object? sender, RoutedEventArgs e)
    {
        if (!File.Exists("employees.csv")) return;
        Employees.Clear();
        using var reader = new StreamReader("employees.csv");
        reader.ReadLine(); // skip header
        string? line;
        while ((line = reader.ReadLine()) != null)
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
