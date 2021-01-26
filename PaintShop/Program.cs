using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaintShop
{
    class Program
    {
        static string _filePath = string.Empty;
        static char[] colorCodes = new char[2] { 'G', 'H' };
        static void Main(string[] args)
        {
            GetFile();
        }

        static void GetFile()
        {
            Console.WriteLine("Enter the file path.");
            _filePath = Console.ReadLine();

            if (!File.Exists(_filePath))
            {
                Console.WriteLine("File does not existing at given address.");
                AskQuestion();
            }
            else
            {
                FileParser fileParser = new FileParser();
                Solver solver = new Solver();
                var parsedFileObject = fileParser.ParseFile(_filePath);
                string solution = solver.Solve(parsedFileObject);

                Console.WriteLine(solution);
                AskQuestion();
            }
        }

        static void AskQuestion()
        {
            Console.WriteLine("\nPress 'Enter' to continue ans 'Esc' to close\n");
            ConsoleKeyInfo consoleKey = Console.ReadKey();

            if (consoleKey.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Good bye...");
                return;
            }
            else if (consoleKey.Key == ConsoleKey.Enter)
            {
                _filePath = string.Empty;

                GetFile();
            }
            else
            {
                AskQuestion();
            }
        }
    }
}
