using System;

namespace BitBucketCLI {
    public static class Utils {
        public static string GetExecutableName() {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        public static void SplitRepositoryReference(string repositoryString, out string project, out string repository) {
            string[] parts = repositoryString.Split('/');
            project = parts[0];
            repository = parts[1];
        }
    }
}
