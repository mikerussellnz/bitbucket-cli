using System;
using System.Collections.Generic;

namespace BitBucketCLI.PullRequests {
    public class PullRequestModule : CLIModule {

        protected override Dictionary<string, CLIItem> GetItems() {
            return new Dictionary<string, CLIItem>() {
                { "create", new PullRequestCreate() },
                { "list", new PullRequestList() }
           };
        }
    }
}
