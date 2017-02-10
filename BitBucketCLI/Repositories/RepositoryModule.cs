using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitBucketCLI.Repositories {
    public class RepositoryModule : CLIModule {
        protected override Dictionary<string, CLIItem> GetItems() {
            return new Dictionary<string, CLIItem>() {
                { "list", new RepositoryList() },
                { "listfiles", new RepositoryListFiles() },
                { "getfile", new RepositoryGetFile() }
            };
        }
    }
}
