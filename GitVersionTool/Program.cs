using LibGit2Sharp;
using System;
using System.Collections.Generic;

namespace GitVersionTool
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Program begin");
            var commitLogFilter = (new CommitLogFilterFactory(args)).GetFilter();
            List<Commit> filteredList = commitLogFilter.ExtractCommits();
            WriteCommitsToFile write = new WriteCommitsToFile(args);
            IBuildOutputString<Commit>[] fileTypes = { new BuildCSVOutputString(), new BuildHTMOutputString(), new BuildTXTOutputString() };
            write.WriteMultipleFileTypes(fileTypes, filteredList);
            Console.WriteLine("Program end");
        }
    }
}
