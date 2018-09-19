namespace ExcelDbModelExporter
{
    public class TableMember
    {
        public string Name;
        public string TypeAndAttributes;
        public string DefaultValue;

        public TableMember(string name, string typeAndAttributes, string defaultValue)
        {
            Name = name;
            TypeAndAttributes = typeAndAttributes;
            DefaultValue = defaultValue;
        }
    }
}