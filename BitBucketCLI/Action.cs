using Mono.Options;
using System;
using System.Collections.Generic;

namespace BitBucketCLI {
    public abstract class Action : CLIItem {
        public abstract OptionSet CreateOptions();

        public virtual int RunWithArgs(string[] args) {
            var options = CreateOptions();

            bool help = false;
            options.Add("h|?|help", "display usage", v => help = v != null);

            List<string> extra;
            try {
                extra = options.Parse(args);
            } catch (OptionException e) {
                Console.Write("{0}: ", Utils.GetExecutableName());
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", Utils.GetExecutableName());
                return -1;
            }

            if (help) {
                ShowHelp(options);
                return 0;
            }

            if (!ValidateParameters()) {
                ShowHelp(options);
                return 0;
            }

            Execute();
            return 0;
        }

        public abstract bool ValidateParameters();

        public abstract void Execute();

        private void ShowHelp(OptionSet p) {
            Console.WriteLine("Usage: {0} module action [options]+", Utils.GetExecutableName());
            Console.WriteLine("Author: <Michael Russell> mikerussellnz@gmail.com");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
