using System;
using System.Collections.Generic;

namespace BitBucketCLI.Branches {
    public class BranchModule : CLIModule {

        protected override Dictionary<string, CLIItem> GetItems() {
            return new Dictionary<string, CLIItem>() {
                { "create", new BranchCreate() },
                { "list", new BranchList() }
           };
        }
    }
}
