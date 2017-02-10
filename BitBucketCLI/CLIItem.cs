using System;
using System.Collections.Generic;
using System.Linq;

namespace BitBucketCLI {
    public interface CLIItem {
        int RunWithArgs(string[] args);
    }
}
