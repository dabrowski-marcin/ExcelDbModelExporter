namespace ExcelDbModelExporter
{
    public class TableMember
    {
        public string Name;
        public string TypeAndAttributes;

        public TableMember(string name, string typeAndAttributes)
        {
            Name = name;
            TypeAndAttributes = typeAndAttributes;
        }
    }
}