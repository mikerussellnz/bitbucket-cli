using System;
using System.Collections.Generic;

namespace BitBucketCLI.Commits {
    public class CommitModule : CLIModule {

        protected override Dictionary<string, CLIItem> GetItems() {
            return new Dictionary<string, CLIItem>() {
              { "list", new CommitList() }
           };
        }

    }
}
