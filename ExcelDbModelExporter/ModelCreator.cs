using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelDbModelExporter
{
    public class ModelCreator
    {
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

                sb.AppendLine($"public class {databaseEntry.TableName}");
                sb.AppendLine("{");
                foreach (var memeber in databaseEntry.TableMembers)
                {
                    sb.AppendLine(CreateMember(memeber.Name, memeber.TypeAndAttributes, memeber.DefaultValue));
                }
                sb.AppendLine("}");
                stringBuilders.Add(sb);

                Console.WriteLine(sb.ToString());
            }

            return stringBuilders;
        }

        private string CreateMember(string name, string type, string defaultValue)
        {
            var lowerType = type.ToLowerInvariant();
            string additionalAttributes = ClassCreator.RequiredStringAttribute(defaultValue.ToLowerInvariant());

            if (lowerType.Contains("string"))
            {
                additionalAttributes += ClassCreator.NvarcharStringAttribute(type);
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
                return ClassCreator.CreatePublicNamedProperty(Types.Decimal, name, additionalAttributes);
            }

            if (lowerType.Contains("datetime"))
            {
                return ClassCreator.CreatePublicNamedProperty(Types.DateTime, name, additionalAttributes);
            }

            return String.Empty;
        }
    }
}