using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    class BuildCSVOutputString : IBuildOutputString<Commit>
    {
        private StringBuilder outputStringBuilder = new StringBuilder();
        private string dateFormat = "dddd, MMM dd yyyy HH:mm:ss";

        public string BuildString(List<Commit> filteredList)
        {
            outputStringBuilder.AppendLine("Author, Committer, Message, DateTime");
            foreach (Commit commit in filteredList)
            {
                outputStringBuilder.AppendLine(String.Format("{0},{1},{2},{3}", commit.Author.ToString().Replace(',', ';'), commit.Committer.ToString().Replace(',', ';'),
                    commit.MessageShort.Replace(',', ';'), commit.Committer.When.ToString(dateFormat).Replace(',', ';')));
            }
            return outputStringBuilder.ToString();
        }

        public string Extension()
        {
            return "csv";
        }
    }
}
