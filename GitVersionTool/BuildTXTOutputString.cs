using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    class BuildTXTOutputString : IBuildOutputString<Commit>
    {
        private StringBuilder outputStringBuilder = new StringBuilder();
        private string dateFormat = "dddd, MMM dd yyyy HH:mm:ss";

        public string BuildString(List<Commit> filteredList)
        {
            foreach (Commit commit in filteredList)
            {
                outputStringBuilder.AppendFormat("Author: {0}{1}Committer: {2}{3}Message: {4}{5}Date and Time: {6}{7}",
                    commit.Author.ToString(), Environment.NewLine, commit.Committer.ToString(), Environment.NewLine, commit.MessageShort, Environment.NewLine,
                    commit.Committer.When.ToString(dateFormat), Environment.NewLine);
            }
            return outputStringBuilder.ToString();
        }

        public string Extension()
        {
            return "txt";
        }
    }
}
