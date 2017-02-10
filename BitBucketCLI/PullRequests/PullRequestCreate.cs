using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mono.Options;
using Atlassian.Stash;
using Atlassian.Stash.Entities;

namespace BitBucketCLI.PullRequests {
    public class PullRequestCreate : Action {
        public List<string> Reviewers { get; private set; }
        public string Repository { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public PullRequestCreate() {
            Reviewers = new List<string>();
        }

        public override OptionSet CreateOptions() {
            return new OptionSet() {
                { "repository=", "repository", v => Repository = v },
                { "from=", "pull from", v => From = v },
                { "to=", "pull to", v => To = v },
                { "title=", "title", v => Title = v },
                { "description=", "description", v => Description = v },
                { "reviewer=", "reviewer (multiple allowed)", v => Reviewers.Add(v) }
            };
        }

        public override bool ValidateParameters() {
            if (string.IsNullOrEmpty(Repository) ||
                string.IsNullOrEmpty(From) ||
                string.IsNullOrEmpty(To) ||
                string.IsNullOrEmpty(Title) ||
                string.IsNullOrEmpty(Description)) {
                return false;
            }
            return true;
        }

        public override void Execute() {
            CreatePullRequest(Repository, From, To, Title, Description, Reviewers);
        }

        private static PullRequest CreatePullRequest(string repository, string from, string to, string title, string description, List<string> reviewers) {
            StashClient bitbucket = Program.ConnectToBitBucket();

            Console.WriteLine("Creating pull request in: " + repository);
            Console.WriteLine("From: " + from);
            Console.WriteLine("To: " + to);
            Console.WriteLine("Title: " + title);
            Console.WriteLine("Description: " + description);

            string project;
            string repo;
            Utils.SplitRepositoryReference(repository, out project, out repo);

            var reviewAuthors = reviewers.Select(r => new AuthorWrapper() {
                User = new Author() {
                    Name = r
                }
            }).ToArray();

            var repoType = new Repository() {
                Slug = repo,
                Project = new Project() {
                    Key = project
                }
            };

            Task<PullRequest> task = bitbucket.PullRequests.Create(project, repo, new PullRequest() {
                Title = title,
                Description = description,
                State = PullRequestState.OPEN,
                Closed = false,
                FromRef = new Ref() {
                    Id = "refs/heads/" + from,
                    Repository = repoType
                },
                ToRef = new Ref() {
                    Id = "refs/heads/" + to,
                    Repository = repoType
                },
                Locked = false,
                Reviewers = reviewAuthors,
                Links = new Links() {
                    Self = null
                }
            });
                      
            return task.Result;
        }
    }
}
