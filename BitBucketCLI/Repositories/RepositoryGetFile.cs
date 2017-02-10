using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Options;

namespace BitBucketCLI.Repositories {
    public class RepositoryGetFile : Action {
        public string Repository { get; set; }
        public string File { get; set; }

        public override OptionSet CreateOptions() {
            return new OptionSet() {
                { "repository=", "repository", v => Repository = v },
                { "file=", "file", v => File = v }
            };
        }

        public override void Execute() {
            var file = GetFile(Repository, File);
            WriteFile(file);
        }

        private void WriteFile(List<string> fileContent) {
            foreach (var line in fileContent) {
                Console.WriteLine(line);
            }
        }

        private List<string> GetFile(string repository, string file) {
            var bitbucket = Program.ConnectToBitBucket();
            string project;
            string repo;
            Utils.SplitRepositoryReference(repository, out project, out repo);
            var fileTask = bitbucket.Repositories.GetFileContents(project, repo, file, Program.DefaultRequestOptions);
            return fileTask.Result.FileContents;
        }

        public override bool ValidateParameters() {
            return (!string.IsNullOrEmpty(Repository) && !string.IsNullOrEmpty(File));
        }
    }
}
