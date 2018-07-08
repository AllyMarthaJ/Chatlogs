using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chatlogs {
    class ChatlogEntry {
        public string Date { get; set; }
        public int Date_ID { get; set; }
        public string Time { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }

        public ChatlogEntry() { }
        public ChatlogEntry(string date, int date_id, string time, string source, string message) {
            this.Date = date;
            this.Date_ID = date_id;
            this.Time = time;
            this.Source = source;
            this.Message = message;
        }

        public static ChatlogEntry FromLine(string line, string filename) {
            try {
                var l = Regex.Match(line, @"(?:\[(.*)\])? (?:\[(.*)\])?: (.*)").Groups;
                var d = Regex.Match(filename, @"(.*)-(.*?).log").Groups;
                return new ChatlogEntry(
                        d[1].Value.Replace('-', '/'), Int32.Parse(d[2].Value),
                        l[1].Value, l[2].Value, l[3].Value
                    );
            } catch {
                string error = "An error occurred. " +
                    "The line wasn't in a format recognised by the program.";
                string na = "N/a";
                return new ChatlogEntry(na, -1, na, na, error);
            }
        }

        public override string ToString() {
            return $"({Date} #{Date_ID} at {Time})\r\n\t{Message}";
        }
    }
}
