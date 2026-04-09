using System;

namespace EmployeesApp;

[Serializable]
public class Employee
{
    public int Id { get; set; }
    public string Imie { get; set; } = "";
    public string Nazwisko { get; set; } = "";
    public int Wiek { get; set; }
    public string Stanowisko { get; set; } = "";

    public Employee() { }
}