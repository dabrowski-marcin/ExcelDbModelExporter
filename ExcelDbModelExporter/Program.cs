using System;
using System.Drawing;

namespace ExcelDbModelExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Console.WriteLine("===== ExcelDbModelExporter =====\n\n\n");
            var posStart = new Point(Console.CursorLeft, Console.CursorTop);
            Console.Write("Save destination: ");
            var posEnd = new Point(Console.CursorLeft, Console.CursorTop);
            posEnd = ConsoleOperations.Write("C:/temp/Models", posEnd ,ConsoleColor.Green);

            Console.Write("\n");

            Console.WriteLine("1. Open excel file and create C# classes.");
            Console.WriteLine("2. Set path for models folder.");
            Console.WriteLine("3. Exit.");

            var key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    // ConsoleOperations.Clear(posStart, posEnd);
                    ModelCreator m = new ModelCreator();
                    m.CreateAndSaveModels();
                    break;
                case ConsoleKey.D2:
                    ConsoleOperations.Clear(posStart, posEnd);
                    break;
                case ConsoleKey.D3:
                    Environment.Exit(0);
                    break;
                default:
                    throw new Exception();
            }

            Console.ReadKey();

        }
    }
}
