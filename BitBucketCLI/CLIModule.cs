using Mono.Options;
using System;
using System.Collections.Generic;

namespace BitBucketCLI {
    public abstract class CLIModule : CLIItem {
        protected abstract Dictionary<string, CLIItem> GetItems();

        protected string GetName() {
            var typename = GetType().Name;
            return typename;
        }

        public virtual int RunWithArgs(string[] args) {
            if (args.Length == 0) {
                ShowHelp();
                return 0;
            }

            var help = false;
            OptionSet options = new OptionSet() {
                { "h|?|help", "display usage", v => help = v != null }
            };

            List<string> extra;
            try {
                extra = options.Parse(args);
            } catch (OptionException e) {
                Console.Write("{0}: ", Utils.GetExecutableName());
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", Utils.GetExecutableName());
                return -1;
            }
            var requestedItem = args[0];
            var parameters = new string[args.Length - 1];
            Array.Copy(args, 1, parameters, 0, args.Length - 1);

            var items = GetItems();

            CLIItem item;
            if (!items.TryGetValue(requestedItem, out item)) {
                if (help) {
                    ShowHelp();
                    return 0;
                }
                Console.WriteLine("No item for {0}", requestedItem);
                ShowHelp();
                return -1;
            }

            var result = item.RunWithArgs(parameters);
            return result;
        }

        protected virtual void ShowHelp() {
            Console.WriteLine("Usage: {0} <module> [options]+", Utils.GetExecutableName());
            Console.WriteLine("Author: <Michael Russell> mikerussellnz@gmail.com");
            Console.WriteLine("Selected Moudle: " + GetName());
            Console.WriteLine("Arguments:");
            Console.WriteLine("\tmodule\t\trequired - module, repeat to access sub-modules.");
            var items = GetItems();
            Console.WriteLine("\t\t\tavailable modules at this level:");
            foreach (var kvp in items) {
                Console.WriteLine("\t\t\t\t" + kvp.Key);
            }
        }
    }
}
