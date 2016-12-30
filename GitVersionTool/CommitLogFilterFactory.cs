using System;

namespace GitVersionTool
{
    class CommitLogFilterFactory
    {
        private const string All = "-All";
        private const string InRange = "-BetweenTwoDates";
        private const string Search = "-MessagePhraseSearch";
        private const string GroupedTickets = "-GroupByTickets";
        private const string LatestBuildChanges = "-BetweenLastTwoBuilds";
        string option;
        string path;
        RepositoryInformation repo;
        string[] args;

        public CommitLogFilterFactory(string [] args)
        {
            this.args = args;

            path = args[0];
            option = args[3];
            ValidateArgs();
            repo = RepositoryInformation.GetRepositoryInformationForPath(path);
        }

        public ICommitFilter GetFilter()
        {
            switch (option)
            {
                case All:
                    return new ExtractAllCommits(repo);
                case InRange:
                    return new ExtractCommitsBetweenTwoDates(repo, args);
                case Search:
                    return new ExtractCommitsWithPhrase(repo, args);
                case GroupedTickets:
                    return new ExtractCommitsGroupedByTickets(repo);
                case LatestBuildChanges:
                    return new ExtractBuildChangeCommits(repo, args);
                default:
                    throw new ArgumentNullException("Invalid option");
            }
        }

        public void ValidateArgs()
        {
            if (path == null || option == null)
            {
                throw new ArgumentNullException("Incorrect/Insufficient args");
            }
        }
    }
}
