using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    interface ICommitFilter
    {
        List<Commit> ExtractCommits();
        void ValidateArgs();
    }
}
