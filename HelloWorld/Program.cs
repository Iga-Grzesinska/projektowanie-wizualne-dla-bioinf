// See https://aka.ms/new-console-template for more information
using System;
using CalculatorLibrary;

class Program
{
    static void Main()
    {
	Console.WriteLine("Hello, World!");
	Console.WriteLine("Pierwszy tekst");
	Console.WriteLine("Drugi tekst");
	Console.WriteLine("Trzeci tekst");
	Console.WriteLine("Test kalkulatora:");

        Calculator calc = new Calculator();

        Console.WriteLine("2 + 3 = " + calc.Add(2,3));
        Console.WriteLine("5 - 2 = " + calc.Subtract(5,2));
        Console.WriteLine("4 * 3 = " + calc.Multiply(4,3));
        Console.WriteLine("10 / 2 = " + calc.Divide(10,2));
    }
}
