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
                        DepluralizeWord(ReadClassNameFromDatasheet(name.TableName));
                    }
                }
            }
        }

        private static string ReadClassNameFromDatasheet(string datasheetName)
        {
            var output = datasheetName.Split(DatasheetNameSeparator);
            return output[0];
        }


    }
}