using System;
using System.Threading.Tasks;
using Mono.Options;
using Atlassian.Stash;
using Atlassian.Stash.Helpers;
using Atlassian.Stash.Entities;
using System.Collections.Generic;

namespace BitBucketCLI.Branches {
    public class BranchList : Action {
        public string Repository { get; set; }
        public string Filter { get; set; }
  
        public override OptionSet CreateOptions() {
            return new OptionSet() {
                 { "repository=", "repository", v => Repository = v },
                 { "filter=", "filter to branches matching", v => Filter = v }
            };
        }

        public override bool ValidateParameters() {
            if (string.IsNullOrEmpty(Repository)) { 
                return false;
            }
            return true;
        }

        public override void Execute() {
            var branches = ListBranches(Repository, Filter);
            WriteBranches(branches);
        }

        private void WriteBranches(IEnumerable<Branch> branches) {
            ConsoleExtended console = new ConsoleExtended();

            bool firstPr = true;
            foreach (var br in branches) {
                if (firstPr) firstPr = false; else console.WriteLine();
                WriteBranch(console, br);
            }
        }

        private void WriteBranch(ConsoleExtended console, Branch br) {
            console.WriteLine("Name: " + br.DisplayId);
            console.WriteLine("Latest Changeset: " + br.LatestChangeset);
        }

        private static IEnumerable<Branch> ListBranches(string repository, string filter = null) {
            StashClient bitbucket = Program.ConnectToBitBucket();
            string project;
            string repo;
            Utils.SplitRepositoryReference(repository, out project, out repo);
            Task<ResponseWrapper<Branch>> branchesTask;
            if (!string.IsNullOrEmpty(filter)) {
                branchesTask = bitbucket.Branches.Get(project, repo, filter, Program.DefaultRequestOptions);
            } else {
                branchesTask = bitbucket.Branches.Get(project, repo, Program.DefaultRequestOptions);
            }
            var branches = branchesTask.Result.Values;
            return branches;
        }
    }
}
