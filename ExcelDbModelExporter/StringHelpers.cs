using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelDbModelExporter
{
    public static class StringHelpers
    {
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

        public static string SingularizePhrase(PluralityForm form, string phrase)
        {
            switch (form)
            {
                case PluralityForm.IrregularIes:
                    return SingularizeIrregularIes(phrase);
                case PluralityForm.Standard:
                    return SingularizeStandard(phrase);
                case PluralityForm.NotPlural:
                    return phrase;
                default:
                    throw new Exception("An error has occurred during singularization of a phrase.");
                    
            }
        }

        private static string SingularizeStandard(string phrase)
        {
            var splitWordArray = SplitOnCapitalLetters(phrase).Split(' ');

            for (int i = 0; i < splitWordArray.Length; i++)
            {
                if (splitWordArray[i].EndsWith('s'))
                {
                    splitWordArray[i] = splitWordArray[i].Remove(splitWordArray[i].Length - 1);
                }
            }

            string s = String.Join(string.Empty, splitWordArray);

            return s;
        }

        private static string SingularizeIrregularIes(string phrase)
        {
            var splitWordArray = SplitOnCapitalLetters(phrase).Split(' ');

            for (int i = 0; i < splitWordArray.Length; i++)
            {
                if (splitWordArray[i].EndsWith("ies"))
                {
                    splitWordArray[i] = splitWordArray[i].Remove(splitWordArray[i].Length - 3);
                    splitWordArray[i] = splitWordArray[i] + "y";
                }
            }

            string s = String.Join(string.Empty, splitWordArray);

            return s;
        }
    }
}