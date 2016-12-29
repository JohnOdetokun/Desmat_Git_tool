using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitVersionTool
{
    class ExtractAllCommits : ICommitFilter
    {
        private RepositoryInformation repo;
        public ExtractAllCommits(RepositoryInformation repo)
        {
            this.repo = repo;
        }

        public List<Commit> ExtractCommits()
        {
            return repo.Log.ToList<Commit>();
        }

        public void ValidateArgs(){}
    }
}
