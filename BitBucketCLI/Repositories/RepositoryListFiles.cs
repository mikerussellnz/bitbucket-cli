using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Options;

namespace BitBucketCLI.Repositories {
    class RepositoryListFiles : Action {
        public string Repository { get; set; }

        public override OptionSet CreateOptions() {
            return new OptionSet() {
                { "repository=", "repository", v => Repository = v }
            };
        }

        public override void Execute() {
            var files = ListFiles(Repository);
            WriteFiles(files);
        }

        private void WriteFiles(IEnumerable<string> files) {
            foreach (var file in files) {
                Console.WriteLine(file);
            }
        }

        public IEnumerable<string> ListFiles(string repository) {
            var bitbucket = Program.ConnectToBitBucket();
            string project;
            string repo;
            Utils.SplitRepositoryReference(repository, out project, out repo);
            var filesTask = bitbucket.Repositories.GetFiles(project, repo, Program.DefaultRequestOptions);
            var files = filesTask.Result.Values;
            return files;
        }

        public override bool ValidateParameters() {
            return !string.IsNullOrEmpty(Repository);
        }
    }
}
