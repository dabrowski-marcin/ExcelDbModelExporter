using System;

namespace ExcelDbModelExporter
{
    public class LanguageRules
    {
        public static PluralityForm GetPluralityForm(string phrase)
        {
            return IsPlural(phrase);
        }

        private static bool IsIrregular(string phrase)
        {
            return EndsWithIes(phrase);
        }

        private static PluralityForm IsPlural(string phrase)
        {
            var splitWordArray = StringHelpers.SplitOnCapitalLetters(phrase).Split(' ');
            PluralityForm pluralityForm = PluralityForm.NotPlural;

            for (int i = 0; i < splitWordArray.Length; i++)
            {
                if (IsIrregular(phrase))
                {
                    pluralityForm = PluralityForm.IrregularIes;
                }
                else if (splitWordArray[i].EndsWith('s'))
                {
                    pluralityForm = PluralityForm.Standard;
                }
            }

            return pluralityForm;
        }


        private static bool EndsWithIes(string phrase)
        {
            if (phrase.EndsWith("ies", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }

    public enum PluralityForm
    {
        NotPlural,
        Standard,
        IrregularIes
    }
}