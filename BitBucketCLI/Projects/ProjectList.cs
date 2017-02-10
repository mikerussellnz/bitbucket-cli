using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Options;
using Atlassian.Stash.Entities;

namespace BitBucketCLI.Projects {
    public class ProjectList : Action {
        public override OptionSet CreateOptions() {
            return new OptionSet();
        }

        public override void Execute() {
            var projects = ListProjects();
            WriteProjects(projects);
        }

        private void WriteProjects(IEnumerable<Project> projects) {
            foreach (var project in projects) {
                Console.WriteLine("Name: " + project.Name);
                Console.WriteLine("\tKey: " + project.Key);
                Console.WriteLine("\tURL: " + project.Links.Self[0].Href);
                Console.WriteLine("\tDescription: " + project.Description);
            }
        }

        public static IEnumerable<Project> ListProjects() {
            var bitbucket = Program.ConnectToBitBucket();
            var projectsTask = bitbucket.Projects.Get(Program.DefaultRequestOptions);
            var projects = projectsTask.Result.Values;
            return projects;
        }

        public override bool ValidateParameters() {
            return true;
        }
    }
}
