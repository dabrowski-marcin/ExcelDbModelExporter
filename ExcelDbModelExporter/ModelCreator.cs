using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;

namespace ExcelDbModelExporter
{
    public class ModelCreator
    {
        private const string tempPath = @"C:\Users\Marcin\Downloads\SunWave_Tabele.xls";
        private const char DatasheetNameSeparator = '-';

        private Dictionary<string, string> ExceptionsDictionary =
            new Dictionary<string, string>() {{"Commodities", "Commodity"}};

    public static void load()
        {
            using (var stream = File.Open(tempPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration
                {

                    // Gets or sets the encoding to use when the input XLS lacks a CodePage
                    // record, or when the input CSV lacks a BOM and does not parse as UTF8. 
                    // Default: cp1252. (XLS BIFF2-5 and CSV only)
                    FallbackEncoding = Encoding.GetEncoding(1252)
                }))

                {
                    var a = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    });

                    foreach (DataTable name in a.Tables)
                    {
                        Console.WriteLine(ReadClassNameFromDatasheet(name.TableName));
                        foreach (var f in name.Columns)
                        {
                            DepluralizeWord(f.ToString());
                            Console.WriteLine(f);
                        }
                    }
                }
            }
        }

        private static string ReadClassNameFromDatasheet(string datasheetName)
        {
            var output = datasheetName.Split(DatasheetNameSeparator);
            return output[0];
        }

        private static string DepluralizeWord(string datasheetName)
        {
            var splitWord = SplitOnCapitalLetters(datasheetName);c
            return ds;
        }

        public static string SplitOnCapitalLetters(string inputString)
        {
            List<char> cleanString = inputString.ToList();
            for (int i = 1; i < cleanString.Count; i++)
            {
                if (char.IsUpper(cleanString[i]))
                {
                    char[] temp = new char[cleanString.Count - i];
                    for (int j = 0; j < temp.Length; j++)
                    {
                        temp[j] = cleanString[j + i];
                    }
                    cleanString[i] = ' ';
                    cleanString.Add(' ');
                    int index = 0;
                    for (int j = i + 1; j < cleanString.Count; j++)
                    {
                        cleanString[j] = temp[index];
                        index++;
                    }
                    i++;
                }
            }
            return new string(cleanString.ToArray());
        }

    }
}