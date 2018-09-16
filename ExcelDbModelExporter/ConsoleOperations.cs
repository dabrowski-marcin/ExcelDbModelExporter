using System;
using System.Drawing;
using System.Net;
using System.Text;

namespace ExcelDbModelExporter
{
    public class ConsoleOperations
    {
        private const string EmptyLineFill = " ";

        public static void Clear(Point cursorStartPt, Point cursorEndPt)
        {
            var lineLength = cursorEndPt.X - cursorStartPt.X;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lineLength; i++)
            {
                sb.Append(EmptyLineFill);
            }
            Console.SetCursorPosition(cursorStartPt.X, cursorEndPt.Y);
            Console.Write(sb.ToString());
            Console.SetCursorPosition(cursorStartPt.X, cursorEndPt.Y);

        }

        public static Point Write(string text, Point cursorEndPt)
        {
            return WriteInternal(text, cursorEndPt);
        }

        public static Point Write(string text, Point cursorEndPt, ConsoleColor textColor)
        {
            return WriteInternal(text, cursorEndPt, textColor);
        }

        private static Point WriteInternal(string text, Point cursorEndPt, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(cursorEndPt.X, cursorEndPt.Y);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
            return new Point(Console.CursorLeft, Console.CursorTop);
        }
    }
}