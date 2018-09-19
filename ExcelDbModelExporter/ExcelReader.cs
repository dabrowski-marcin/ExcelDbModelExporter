using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ExcelDbModelExporter
{
    public class ExcelReader : IDatabaseSchemeReader
    {
        private const string tempPath = @"C:\Users\marcindx\Downloads\SunWave_Tabele.xls";
        private const char DatasheetNameSeparator = '-';
        public readonly List<DatabaseEntry> DatabaseEntries = new List<DatabaseEntry>();
        private readonly DataSet DataSet;

        private int NameColumnId;
        private int TypeColumnId;
        private int DefaultValueColumnId;

        private const string FieldNameColIdentifier = "NazwaEN";
        private const string TypeNameColIdentifier = "Typ";
        private const string DefaultValueColIdentifier = "Default";
    

        public ExcelReader(string path = tempPath)
        {
            DataSet = ReadDataSetFromFile();
            LoadData();
        }

        public List<DatabaseEntry> ReadDatabaseEntries()
        {
            return DatabaseEntries;
        }

        private void LoadData()
        {
            foreach (DataTable table in DataSet.Tables)
            {
                var tableName = ReadClassNameFromDatasheet(table.TableName);
                var form = LanguageRules.GetPluralityForm(tableName);
                var tableNameSingularized = StringHelpers.SingularizePhrase(form, tableName);

                DatabaseEntry entry = new DatabaseEntry(tableNameSingularized);

                foreach (DataColumn column in table.Columns)
                {
                    if (column.ColumnName.Contains(FieldNameColIdentifier))
                    {
                        NameColumnId = column.Ordinal;
                    }
                    if (column.ColumnName.Contains(TypeNameColIdentifier))
                    {
                        TypeColumnId = column.Ordinal;
                    }
                    if (column.ColumnName.Contains(DefaultValueColIdentifier))
                    {
                        DefaultValueColumnId = column.Ordinal;
                    }
                }

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var name = table.Rows[i][NameColumnId].ToString();
                    var type = table.Rows[i][TypeColumnId].ToString();
                    var defaultValue = table.Rows[i][DefaultValueColumnId].ToString();

                    entry.AddTableMember(new TableMember(name, type, defaultValue));
                }

                DatabaseEntries.Add(entry);
            }
        }


        private DataSet ReadDataSetFromFile()
        {
            using (var stream = File.Open(tempPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration
                {

                    FallbackEncoding = Encoding.GetEncoding(1252)
                }))

                {
                    return reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    });
                }
            }
        }

        private static string ReadClassNameFromDatasheet(string datasheetName)
        {
            var output = datasheetName.Split(DatasheetNameSeparator);
            return output[0];
        }

    }

    public interface IDatabaseSchemeReader
    {
        List<DatabaseEntry> ReadDatabaseEntries();
    }
}