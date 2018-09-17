using System.Collections.Generic;

namespace ExcelDbModelExporter
{
    public class DatabaseEntry
    {
        public string TableName;
        public readonly List<TableMember> TableMembers = new List<TableMember>();

        public DatabaseEntry(string tableName)
        {
            this.TableName = tableName;
        }

        public void AddTableMember(TableMember member)
        {
            TableMembers.Add(member);
        }
    }
}