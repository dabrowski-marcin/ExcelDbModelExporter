using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelDbModelExporter
{
    public class ModelCreator
    {
        private ExcelReader _excelReader;
        private readonly string Indentation = "    ";

        public ModelCreator()
        {
            _excelReader = new ExcelReader();
        }

        public void CreateAndSaveModels(string path = @"C:/temp/models")
        {
            var models = CreateClassStringBuilders();
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Unable to create directory {path}");
                    throw;
                }
            }

            foreach (var stringbuilder in models)
            {

                using (var writer = new StreamWriter(Path.Combine(path, stringbuilder.Key)))
                {
                    writer.Write(stringbuilder.Value.ToString());
                }
            }
        }

        public Dictionary<string, StringBuilder> CreateClassStringBuilders()
        {
            var data = _excelReader.ReadDatabaseEntries();
            Dictionary<string, StringBuilder> stringBuilders = new Dictionary<string, StringBuilder>();
            
            foreach (var databaseEntry in data)
            {
                var name = databaseEntry.TableName + ".cs";
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"public class {databaseEntry.TableName}");
                sb.AppendLine("{");
                sb.AppendLine($"{Indentation}public {databaseEntry.TableName}()");
                sb.AppendLine($"{Indentation}" + "{");
                foreach (var member in databaseEntry.TableMembers)
                {
                    var memberValue = GetDefaultValue(member.DefaultValue);
                    if (!string.IsNullOrEmpty(memberValue) && !string.IsNullOrEmpty(member.Name))
                    {
                        sb.AppendLine($"{Indentation}{Indentation}this.{member.Name} = {memberValue};");
                    }
                }
                sb.AppendLine($"{Indentation}" + "}\n");


                foreach (var memeber in databaseEntry.TableMembers)
                {
                    var member = CreateMember(memeber.Name, memeber.TypeAndAttributes, memeber.DefaultValue);
                    if (!string.IsNullOrEmpty(member))
                    {
                        sb.AppendLine(member);
                    }
                }
                sb.AppendLine("}");
                stringBuilders.Add(name, sb);

                Console.WriteLine(sb.ToString());
            }

            return stringBuilders;
        }

        private string GetDefaultValue(string value)
        {
            if (value.Contains("\"\"") || value.Contains("„”")) 
            {
                return "string.Empty";
            }

            if (value.Contains("not null") || value.Contains("notnull") || string.IsNullOrEmpty(value) || value.Contains("getdate"))
            {
                return string.Empty;
            }

            return value;
        }

        private string CreateMember(string name, string type, string defaultValue)
        {
            var lowerType = type.ToLowerInvariant();
            string additionalAttributes = RequiredStringAttribute(defaultValue.ToLowerInvariant() + "\n");


            if (lowerType.Contains("string"))
            {
                additionalAttributes += NvarcharStringAttribute(type);
                return ClassCreator.CreatePublicNamedProperty(Types.String, name, additionalAttributes);
            }

            if (lowerType.Contains("int"))
            {
                return ClassCreator.CreatePublicNamedProperty(Types.Int, name, additionalAttributes);
            }

            if (lowerType.Contains("bool"))
            {
                return ClassCreator.CreatePublicNamedProperty(Types.Bool, name, additionalAttributes);
            }

            if (lowerType.Contains("decimal"))
            {
                additionalAttributes += DecimalStringAttribute(type);
                return ClassCreator.CreatePublicNamedProperty(Types.Decimal, name, additionalAttributes);
            }

            if (lowerType.Contains("datetime") || lowerType.Contains("date"))
            {
                return ClassCreator.CreatePublicNamedProperty(Types.DateTime, "DateTime", additionalAttributes);
            }

            return String.Empty;
        }

        private string NvarcharStringAttribute(string type)
        {
            Regex regex = new Regex(@"(?:\w*)([\([\d]*\))");
            var length = StripParenthesis(regex.Match(type).Groups[1].Value);
            return $"{Indentation}[Column(TypeName = \"NVARCHAR({length})\")]\n{Indentation}[StringLength({length})]";
        }

        private string DecimalStringAttribute(string type)
        {
            Regex regex = new Regex(@"(?:\w*)(.*)");
            var length = StripParenthesis(regex.Match(type).Groups[1].Value).Replace(" ", string.Empty);
            return $"{Indentation}[Column(TypeName = \"decimal({length})\")]";
        }

        private string RequiredStringAttribute(string defaultValue)
        {
            if (defaultValue.Contains("not null") || defaultValue.Contains("notnull"))
            {
                return $"{Indentation}[Required]";
            }

            return string.Empty;
        }

        private string StripParenthesis(string phrase)
        {
            return phrase.Replace("(", string.Empty).Replace(")", string.Empty);
        }
    }
}