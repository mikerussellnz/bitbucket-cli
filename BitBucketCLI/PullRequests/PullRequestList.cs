using Atlassian.Stash;
using Atlassian.Stash.Entities;
using Atlassian.Stash.Helpers;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBucketCLI.PullRequests {
    public class PullRequestList : Action {
        private string User = null;
        private List<string> Repositories = new List<string>();
        private PullRequestState State = PullRequestState.OPEN;

        public override OptionSet CreateOptions() {
           return new OptionSet() {
               { "repository=", "repository (multiple allowed)", v => Repositories.Add(v) },
               { "user=", "filter to user", v => User = v },
               { "state=", "filter to state (default is open)", v => State = (PullRequestState)Enum.Parse(typeof(PullRequestState), v.ToUpper()) }
            };
        }

        public override bool ValidateParameters() {
            if (Repositories.Count == 0) {
                return false;
            }
            return true;
        }

        public override void Execute() {
            var pullRequests = GetPullRequests(Repositories, State, User);
            WritePullRequests(pullRequests);
        }

        private static Dictionary<string, List<PullRequest>> GetPullRequests(List<string> repositories, PullRequestState state = PullRequestState.OPEN, string filterToUser = null) {
            StashClient bitbucket = Program.ConnectToBitBucket();

            Dictionary<string, Task<ResponseWrapper<PullRequest>>> tasksToWaitFor = repositories
                .ToDictionary(r => r, r => {
                    string project;
                    string repo;
                    Utils.SplitRepositoryReference(r, out project, out repo);
                    return bitbucket.PullRequests.Get(project, repo, Program.DefaultRequestOptions, state: state);
                });
                      
            var pullRequests = tasksToWaitFor.ToDictionary(
                r => r.Key,
                t => t.Value.Result.Values.Where(
                      pr => filterToUser == null ||
                      (pr.Author?.User?.EmailAddress?.EqualsIgnoreCase(filterToUser) ?? false)
                   ).ToList()
                );

            return pullRequests;
        }

        private void WritePullRequests(Dictionary<string, List<PullRequest>> pullRequests) {
            ConsoleExtended console = new ConsoleExtended();

            bool firstRepo = true;
            foreach (var kvp in pullRequests) {
                if (firstRepo) firstRepo = false; else console.WriteLine("\n");
                console.WriteLine(ConsoleColor.Green, "Repository: " + kvp.Key);

                bool firstPr = true;
                if (kvp.Value.Count > 0) {
                    foreach (var pr in kvp.Value) {
                        if (firstPr) firstPr = false; else console.WriteLine();
                        WritePullRequest(console, pr);
                    }
                } else {
                    console.WriteLine("<No Pull Requests>");
                }
            }
        }

        private void WritePullRequest(ConsoleExtended console, PullRequest pr) {
            console.WriteLine(ConsoleColor.Yellow, "Request: {0} <{1}>", pr.Id, pr.Links.Self[0].Href.ToString());
            console.WriteLine("Created: " + DateTimeUtils.FromEpochMilliseconds(pr.CreatedDate));
            console.WriteLine("Author: {0} <{1}>", 
                (pr.Author?.User?.DisplayName ?? "None"), 
                (pr.Author?.User?.EmailAddress ?? "None"));

            console.WriteLine("Status: " + pr.State);
            console.WriteLine("Branches: {0} -> {1}", 
                (pr.FromRef?.DisplayId ?? "None"),
                (pr.ToRef?.DisplayId ?? "None"));

            foreach (var reviewer in pr.Reviewers) {
                console.Write("Reviewer: {0} <{1}> - ",
                    (reviewer.User?.DisplayName ?? "None"),
                    (reviewer.User?.EmailAddress ?? "None"));

                ConsoleColor statusColor = ConsoleColor.Gray;
                switch (reviewer.Status) {
                    case "APPROVED":
                        statusColor = ConsoleColor.Green;
                        break;
                    case "UNAPPROVED":
                        statusColor = ConsoleColor.Yellow;
                        break;
                }
                console.WriteLine(statusColor, reviewer.Status);
            }
            console.WriteLine();
            console.WriteLine(pr.Description ?? "<No Description>");
        }
    }
}
