using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitBucketCLI.Projects {
    public class ProjectModule : CLIModule {
        protected override Dictionary<string, CLIItem> GetItems() {
            return new Dictionary<string, CLIItem>() {
               { "list", new ProjectList() }
           };
        }
    }
}
