using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Options;
using Atlassian.Stash.Entities;

namespace BitBucketCLI.Repositories {
    public class RepositoryList : Action {
        public string Project { get; set; }

        public override OptionSet CreateOptions() {
            return new OptionSet() {
                 { "project=", "project", v => Project = v },
            };
        }

        public override void Execute() {
            var repositories = ListRepositories(Project);
            WriteRepositories(repositories);
        }

        private void WriteRepositories(IEnumerable<Repository> repositories) {
            foreach (var repository in repositories) {
                Console.WriteLine("Name: " + repository.Name);
                Console.WriteLine("\tURL: " + repository.Links.Self[0].Href);
            }
        }

        public static IEnumerable<Repository> ListRepositories(string project) {
            var bitbucket = Program.ConnectToBitBucket();
            var repositoriesTask = bitbucket.Repositories.Get(project, Program.DefaultRequestOptions);
            var repositories = repositoriesTask.Result.Values;
            return repositories;
        }

        public override bool ValidateParameters() {
            return !string.IsNullOrEmpty(Project);
        }
    }
}
