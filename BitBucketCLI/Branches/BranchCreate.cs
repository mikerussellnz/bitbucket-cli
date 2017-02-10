using Mono.Options;
using Atlassian.Stash;
using Atlassian.Stash.Entities;
using System;
using System.Threading.Tasks;

namespace BitBucketCLI.Branches {
    public class BranchCreate : Action {
        public string Repository { get; set; }
        public string Name { get; set; }
        public string From { get; set; }

        public override OptionSet CreateOptions() {
            return new OptionSet() {
                 { "repository=", "repository", v => Repository = v },
                 { "name=", "branch name", v => Name = v },
                 { "from=", "create branch from commit", v => From = v }
            };
        }

        public override bool ValidateParameters() {
            if (string.IsNullOrEmpty(Repository) ||
                string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(From)) {
                return false;
            }
            return true;
        }

        public override void Execute() {
             CreateBranch(Repository, Name, From);
        }

        private static Branch CreateBranch(string repository, string name, string from) {
            StashClient bitbucket = Program.ConnectToBitBucket();
            string project;
            string repo;
            Utils.SplitRepositoryReference(repository, out project, out repo);
            Task<Branch> task = bitbucket.Branches.Create(project, repo, new Branch() {
                Name = name,
                StartPoint = from
            });
            return task.Result;
        }
    }
}
