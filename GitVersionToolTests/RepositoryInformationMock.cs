using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionToolTests
{
    class RepositoryInformationMock
    {
        public RepositoryInformationMock()
        {
            Repository repo = new Repository("");
            // Incomplete: Add commits to repo in order to do testing
            repo.Dispose();
        }
        
        private Commit CreateCommit(Repository repo, string message, string name, string email, DateTimeOffset when)
        {
            Signature author = new Signature(name, email, when);
            Signature commiter = author;
            Commit commit = repo.Commit(message, author, commiter);
            return commit;
        }
    }
}



