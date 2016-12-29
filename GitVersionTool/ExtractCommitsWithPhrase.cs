using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitVersionTool
{
    class ExtractCommitsWithPhrase : ICommitFilter
    {
        private RepositoryInformation repo;
        private string[] args;
        string phrase;

        public ExtractCommitsWithPhrase(RepositoryInformation repo, string[] args)
        {
            this.repo = repo;
            this.args = args;
            ValidateArgs();
            phrase = args[4];
        }

        public List<Commit> ExtractCommits()
        {
            List<Commit> filteredCommitList = new List<Commit>();
            foreach (Commit commit in repo.Log.ToList<Commit>())
            {
                if (commit.MessageShort.Contains(phrase))
                {
                    filteredCommitList.Add(commit);
                }
            }
            return filteredCommitList;
        }

        public void ValidateArgs()
        {
            if (args[4] == null)
            {
                throw new ArgumentNullException("Phrase argument not provided, please run program again and enter phrase with correct fromat.");
            }
        }
    }
}
