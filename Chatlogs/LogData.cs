﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatlogs {
    class LogData {
        public string LocalDir { get; set; } = "chatlogs_ex";
        public static string GetLogDir() {
            string appData = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);
            string logs = appData + @"\.minecraft\logs";

            return logs;
        }

        public void DecompressLogs() {
            if (!Directory.Exists(LocalDir))
                Directory.CreateDirectory(LocalDir);

            foreach (string file in Directory.EnumerateFiles(GetLogDir(), "*.gz")) {
                FileInfo fi = new FileInfo(file);

                Decompress(fi, LocalDir + @"\" + fi.Name.Remove(fi.Name.Length - 2));
            }
        }

        public void Decompress(FileInfo fileToDecompress, string fileName) {
            // Open original file for reading.
            using (FileStream originalFileStream = fileToDecompress.OpenRead()) {
                // Create new file for decompression.
                using (FileStream decompressedFileStream = File.Create(fileName)) {
                    // Open stream for decompression.
                    using (GZipStream decompressionStream =
                        new GZipStream(originalFileStream,
                        CompressionMode.Decompress)) {
                        // Copy to decompressed file.
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }

        public void Save(List<ChatlogEntry> logs) {
            string lastFile = "";
            StringBuilder builder = new StringBuilder();

            foreach (var e in logs) {
                string file = $"\r\n<---[ {e.Date} #{e.Date_ID} ]--->\r\n";
                if (file != lastFile) builder.AppendLine(file);

                builder.AppendLine(e.ToString());

                lastFile = file;
            }

            string fn = "search_results_" +
                DateTime.Now.ToString("yyyy-dd-M_HH-mm-ss") + ".txt";
            File.WriteAllText(fn, builder.ToString());
            Console.WriteLine();

            ConsoleHelper.Update("Saved results as " + fn, ConsoleColor.Cyan);
            Console.ReadKey();
        }
    }
}
