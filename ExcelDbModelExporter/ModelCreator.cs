using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelDbModelExporter
{
    public class ModelCreator
    {
        private const string tempPath = @"C:\Users\Marcin\Downloads\SunWave_Tabele.xls";
        private const char DatasheetNameSeparator = '-';
        private const int IndentationDepth = 4;

        private ExcelReader _excelReader;

        public void load()
        {
            _excelReader = new ExcelReader();
            CreateClassStringBuilders();
        }

        public List<StringBuilder> CreateClassStringBuilders()
        {
            var data = _excelReader.ReadDatabaseEntries();
            List<StringBuilder> stringBuilders = new List<StringBuilder>();
            
            foreach (var databaseEntry in data)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"public class {databaseEntry.TableName}\n");
                sb.Append("{\n");
                foreach (var memeber in databaseEntry.TableMembers)
                {
                    sb.Append(CreateMember(memeber.Name, memeber.TypeAndAttributes));
                }
                sb.Append("}");
                stringBuilders.Add(sb);

                Console.WriteLine(sb.ToString());
            }

            return stringBuilders;
        }

        private string CreateMember(string name, string type)
        {
            var lowerType = type.ToLowerInvariant();

            if (lowerType.Contains("string"))
            {
                var efsa = EntityFrameworkStringAttribute(type);
                return $"{efsa}    public string {name};\n\n";
            }

            if (lowerType.Contains("int"))
            {
                return $"    public int {name};\n\n";
            }

            if (lowerType.Contains("bool"))
            {
                return $"    public bool {name};\n\n";
            }

            if (lowerType.Contains("decimal"))
            {
                return $"    public decimal {name};\n\n";
            }

            return String.Empty;
        }

        private string EntityFrameworkStringAttribute(string type)
        {
            Regex regex = new Regex(@"(?:\w*)([\([\d]*\))");
            var length = StripParenthesis(regex.Match(type).Groups[1].Value);
            return $"    [Column(TypeName = \"NVARCHAR\")]\n    [StringLength({length})]\n";
        }

        private string StripParenthesis(string phrase)
        {
            return phrase.Replace("(", string.Empty).Replace(")", string.Empty);
        }
    }
}