using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Chatlogs.ConsoleHelper;

namespace Chatlogs {
    class Program {
        private const string localDir = "chatlogs_ex";

        static void Main(string[] args) {

            // Print initialisation screen.

            Console.WriteLine(Properties.Resources.Chatlogs);
            Console.WriteLine("Copying logs from: " + LogData.GetLogDir());
            Console.Write("Search >> ");

            // Get search value.

            string searchValue = Console.ReadLine();

            // Decompress the logs.

            LogData ld = new LogData() {
                LocalDir = localDir
            };

            Update("Copying logs to " + localDir, ConsoleColor.Yellow);
            ld.DecompressLogs();

            // Read the logs.

            var found = new List<ChatlogEntry>();

            foreach (string file in Directory.EnumerateFiles(localDir, "*.log")) {
                Update("Searching " + file + "...", ConsoleColor.Cyan);

                using (FileStream fs = File.Open(file, FileMode.Open))
                using (StreamReader sr = new StreamReader(fs)) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        var entry =
                            ChatlogEntry.FromLine(line, new FileInfo(file).Name);

                        bool contains = entry.Message.IndexOf(searchValue,
                            StringComparison.OrdinalIgnoreCase) >= 0;

                        if (contains) {
                            Console.WriteLine(Trim(entry.ToString(), 73));
                            found.Add(entry);
                        }
                    }
                }
            }

            // Save the results.

            int count = found.Count;
            Update($"Found {count} results.", count == 0 ?
                ConsoleColor.Red : ConsoleColor.Yellow);

            Console.Write("Do you want to save the search results (Y/N)? ");
            bool save = Console.ReadKey().Key == ConsoleKey.Y;

            if (save) {
                ld.Save(found);
            }
        }
    }
}
