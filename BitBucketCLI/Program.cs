using Atlassian.Stash;
using Atlassian.Stash.Helpers;
using System;

namespace BitBucketCLI {
    class Program {
        public static RequestOptions DefaultRequestOptions = new RequestOptions() { Limit = 5000 };

        private static string _server;
        private static string _user;
        private static string _password;
       
        public static StashClient ConnectToBitBucket() {
            var client = new StashClient(_server, _user, _password);
            return client;
        }

        static int Main(string[] args) {
            CredentialStorage credentialStore = new CredentialStorage();
            if (!credentialStore.TryGetCredentials(out _server, out _user, out _password)) {
                Console.Out.WriteLine("No server and login configuration found.");
                Console.Out.Write("Please enter Bitbucket server url: ");
                _server = Console.ReadLine();
                Console.Out.Write("Please enter Bitbucket user: ");
                _user = Console.ReadLine();
                Console.Out.Write("Please enter user password: ");
                _password = Console.ReadLine();
                credentialStore.StoreCredentials(_server, _user, _password);
            }
            
            RootModule rootModule = new RootModule();
            int result = rootModule.RunWithArgs(args);
            return result;
        }
    }
}
