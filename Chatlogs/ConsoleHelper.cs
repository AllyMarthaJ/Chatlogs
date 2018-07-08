using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatlogs {
    class ConsoleHelper {
        public static void Update(string update, ConsoleColor color) {
            var clr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now.ToString("[hh:mm:ss] ") + update);
            Console.ForegroundColor = clr;
        }

        public static string Trim(string input, int length) {
            if (input.Length <= length + 3) return input;

            return input.Remove(length + 3) + "...";
        }
    }
}
