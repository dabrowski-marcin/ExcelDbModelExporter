using System;
using System.Text.RegularExpressions;

namespace ExcelDbModelExporter
{
    public enum Types
    {
        Bool,
        Decimal,
        Double,
        Int,
        String,
        DateTime
    }

    public static class ClassCreator
    {
        public const string Class = "class";
        public const string PublicAccessor = "public";
        public const string PrivateAccessor = "private";
        public const string AutoProperty = "{ get; set; }";
        private const string Indentation = "    ";


        public static string CreatePublicNamedProperty(Types type, string name, string defaultValue)
        {
            var lowerType = type.ToString().ToLowerInvariant();
            string efAnnotations;
            var propertyString = $"{Indentation}{PublicAccessor} {lowerType} {name} {AutoProperty}\n";

            switch (type)
            {
                case Types.String:
                    efAnnotations = NvarcharStringAttribute(name)

            }

            if (!String.IsNullOrEmpty(optionalArguments))
            {
                return $"{optionalArguments}\n{propertyString}";
            }

            return propertyString;
        }

        private static string NvarcharStringAttribute(string type)
        {
            Regex regex = new Regex(@"(?:\w*)([\([\d]*\))");
            var length = StripParenthesis(regex.Match(type).Groups[1].Value);
            return $"{Indentation}[Column(TypeName = \"NVARCHAR\")]\n{Indentation}[StringLength({length})]";
        }

        private static string RequiredStringAttribute(string defaultValue)
        {
            if (defaultValue.Contains("not null") || defaultValue.Contains("notnull"))
            {
                return $"{Indentation}[Required]";
            }

            return string.Empty;
        }

        private static string StripParenthesis(string phrase)
        {
            return phrase.Replace("(", string.Empty).Replace(")", string.Empty);
        }
    }
}