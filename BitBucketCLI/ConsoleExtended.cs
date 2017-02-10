using System;

namespace BitBucketCLI {
    public class ConsoleExtended {
        public void WriteLine() {
            Console.WriteLine();
        }

        public void WriteLine(string content) {
            Console.WriteLine(content);
        }

        public void WriteLine(string format, params string[] args) {
            WriteLine(string.Format(format, args));
        }

        public void WriteLine(ConsoleColor color, string content) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            WriteLine(content);
            Console.ForegroundColor = old;
        }

        public void WriteLine(ConsoleColor color, string format, params string[] args) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            WriteLine(format, args);
            Console.ForegroundColor = old;
        }

        public void Write(string content) {
            Console.Write(content);
        }

        public void Write(string format, params string[] args) {
            Console.Write(string.Format(format, args));
        }

        public void Write(ConsoleColor color, string content) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Write(content);
            Console.ForegroundColor = old;
        }

        public void Write(ConsoleColor color, string format, params string[] args) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Write(format, args);
            Console.ForegroundColor = old;
        }
    }
}
