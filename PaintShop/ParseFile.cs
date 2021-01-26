using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PaintShop
{

    public class CustomerPreference
    {
        public int Color { get; set; }
        public string Finish { get; set; }
        public bool IsReadOnly { get; set; }
    }
    public class ParsedFileObject
    {
        public ParsedFileObject()
        {
            CustomerPreferences = new List<List<CustomerPreference>>();
        }
        public int NumberColors { get; set; }
        public List<List<CustomerPreference>> CustomerPreferences { get; set; }
    }
    public class FileParser
    {
        public ParsedFileObject ParseFile(string filePath)
        {
            try
            {
                ParsedFileObject parsedFileObject;

                FileInfo fileInfo = new FileInfo(filePath);
                string fileLine;
                Regex regexNumber = new Regex(@"^\d$");
                int numberOfColors = 0;
                char[] result = new char[0];


                StreamReader file =
                    new StreamReader(filePath);
                Regex linePattern = new Regex(@"^[GM1-9 ]+$");
                List<string> customerLines = new List<string>();

                string colorLine = string.Empty;

                while ((fileLine = file.ReadLine()) != null)
                {
                    Console.WriteLine(fileLine);

                    if (regexNumber.IsMatch(fileLine))
                    {
                        colorLine = fileLine.Trim();
                    }

                    else if (!string.IsNullOrEmpty(fileLine) && linePattern.IsMatch(fileLine))
                    {
                        fileLine = fileLine.Trim();
                        if (!string.IsNullOrEmpty(fileLine))
                            customerLines.Add(fileLine);
                    }
                }

                file.Close();

                int.TryParse(colorLine, out numberOfColors);

                parsedFileObject = new ParsedFileObject();
                parsedFileObject.NumberColors = numberOfColors;

                List<CustomerPreference> customerPreferences;

                foreach (var line in customerLines)
                {
                    customerPreferences = new List<CustomerPreference>();
                    CustomerPreference customerPreference;

                    var splitLine = Regex.Split(line, @"\s+");
                    List<string> nonEmptyOptions = new List<string>();
                    foreach (var item in splitLine)
                    {
                        if (!string.IsNullOrEmpty(item[0].ToString()))
                        {
                            nonEmptyOptions.Add(item);
                        }
                    }

                    customerPreference = new CustomerPreference();

                    foreach (var item in nonEmptyOptions)
                    {
                        if (item == "G" || item == "M")
                        {
                            customerPreference.Finish = item;
                            customerPreference.IsReadOnly = false;
                            customerPreferences.Add(customerPreference);
                            customerPreference = new CustomerPreference();
                        }
                        else
                        {
                            customerPreference.Color = int.Parse(item);
                        }
                    }

                    parsedFileObject.CustomerPreferences.Add(customerPreferences);
                }
                return parsedFileObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}
