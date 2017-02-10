using BitBucketCLI.Branches;
using BitBucketCLI.Commits;
using BitBucketCLI.Projects;
using BitBucketCLI.PullRequests;
using BitBucketCLI.Repositories;
using System;
using System.Collections.Generic;

namespace BitBucketCLI {
    public class RootModule : CLIModule {

        protected override Dictionary<string, CLIItem> GetItems() {
            return new Dictionary<string, CLIItem>() {
                { "pullrequest", new PullRequestModule() },
                { "branch", new BranchModule() },
                { "commit", new CommitModule() },
                { "project", new ProjectModule() },
                { "repository", new RepositoryModule() }
            };
        }
    }
}
