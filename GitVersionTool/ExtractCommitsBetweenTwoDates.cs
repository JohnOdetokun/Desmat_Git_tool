using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace GitVersionTool
{
    class ExtractCommitsBetweenTwoDates : ICommitFilter
    {
        private RepositoryInformation repo;
        private string[] args;
        DateTime rangeBegin;
        DateTime rangeEnd;
        public ExtractCommitsBetweenTwoDates(RepositoryInformation repo, string[] args)
        {
            this.repo = repo;
            this.args = args;
            rangeBegin = GetDateTime(args[4]);
            rangeEnd = GetDateTime(args[5]);
            ValidateArgs();
        }

        public List<Commit> ExtractCommits()
        {
            List<Commit> extractedCommits = new List<Commit>();
            foreach (Commit commit in repo.Log.ToList<Commit>())
            {
                DateTime commitDate = commit.Committer.When.DateTime;
                if (IsInRange(rangeBegin, rangeEnd, commitDate))
                {
                    extractedCommits.Add(commit);
                }
            }
            return extractedCommits;
        }

        private bool IsInRange(DateTime rangeBegin, DateTime rangeEnd, DateTime date)
        {
            if (date > rangeBegin && date < rangeEnd)
            {
                return true;
            }
            return false;
        }

        private DateTime GetDateTime(string dateString)
        {
            DateTime date;
            if(!DateTime.TryParse(dateString, out date))
            {
                throw new ArgumentNullException("Date format incorrect, please run program again and enter two dates with correct \"dd-mm-yyyy\" fromat.");
            }
            return date;
        }

        public void ValidateArgs()
        {

            if (rangeBegin == null || rangeEnd == null)
            {
                throw new ArgumentNullException("Date arguments not provided, please run program again and enter two dates with correct \"dd-mm-yyyy\" fromat.");
            }
        }
    }
}
