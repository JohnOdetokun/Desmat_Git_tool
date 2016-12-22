using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    class Program
    {
        private const string displayAll = "-displayAll";
        private const string displayInRange = "-displayBetweenTwoDates";
        private const string displaySearch = "-searchPhrase";
        private const string displayGroupedTickets = "-displayGroupedTickets";
        private const string displayBuildChanges = "-displayChangesBetweenLastTwoBuilds";

        static void Main(string[] args)
        {
            string fileWriteName = "";
            string path = "";
            string fileWritePath = "";
            string option = "";
            try
            {
                Console.WriteLine("Attempt to initialize variables from cmd args");
                path = args[0];
                fileWriteName = args[1];
                fileWritePath = args[2];
                option = args[3];
                Console.WriteLine("Variables successfuly initialized from cmd args");
                RepositoryInformation repo = RepositoryInformation.GetRepositoryInformationForPath(path);
                if (repo != null)
                {
                    Console.WriteLine("Gathering commit info...");
                    List<RepositoryInformation.CommitFormat> commitList = repo.getCommitList();
                    Console.WriteLine("Commit info gathered");
                    FileOutWrite fileWriter = new FileOutWrite(fileWriteName, fileWritePath, commitList, repo);
                    var commitsToWrite = new List<RepositoryInformation.CommitFormat>();
                    commitsToWrite = GetCommitToWrite(args, option, repo, commitList);
                    fileWriter.setList(commitsToWrite);
                    fileWriter.writeFiles();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static List<RepositoryInformation.CommitFormat> GetCommitToWrite(string[] args, string option, RepositoryInformation repo, List<RepositoryInformation.CommitFormat> commitList)
        {
            /*
             
            var commitLogFilter = (new CommitLogFilterFactory(option, args)).GetFilter();
           var List<CommitFormats>  = commitLogFilter.ExtractCommits();
            
            =========================== 
            public interface IComittFilter{
            List<ComitFormat> ExtractCommits;
            }

            public class DisplayAllCommitExtractor:ICommitFilter{
            
            public ctor(args){}

            public List<CommitFormat> ExtactCommits{
             List<RepositoryInformation.CommitFormat> commitList = repo.getCommitList();
             return commitList;
            }
            }
            public class CommitLogFilterFactory{
            
            public CommitLogFilterFactory(string option, string[] args){
            this.option = option;
            this.args = args;
            }

            pubic IComittFilter GetFilter(){
            
            switch(option){
            case displayAll: return new DisplayAllCommitExtractr(args);
            }
            }
            }
             
             
             */


            //var commitLogFilter = (new CommitLogFilterFactory(option, args)).GetFilter();
            //var List< CommitFormats >  = commitLogFilter.ExtractCommits();

            List<RepositoryInformation.CommitFormat> commitsToWrite;
            switch (option)
            {
                case displayAll:
                    commitsToWrite = commitList;
                    break;
                case displayInRange:
                    var isDateRangeProvided = args[4] != null && args[5] != null;
                    if (isDateRangeProvided)
                    {
                        //  datetime.parseexact
                        string[] date1info = args[4].Split('-');
                        string[] date2info = args[5].Split('-');
                        DateTime date1 = new DateTime(int.Parse(date1info[2]), int.Parse(date1info[1]), int.Parse(date1info[0]));
                        DateTime date2 = new DateTime(int.Parse(date2info[2]), int.Parse(date2info[1]), int.Parse(date2info[0]));
                        commitsToWrite = repo.getInRangeCommitList(commitList, date1, date2);
                    }
                    else if (args[5] != null)
                    {
                        string[] dateInfo = args[4].Split('-');
                        DateTime date = new DateTime(int.Parse(dateInfo[2]), int.Parse(dateInfo[1]), int.Parse(dateInfo[0]));
                        commitsToWrite = repo.getDateCommitList(commitList, date);
                    }
                    else
                    {
                        throw new ArgumentNullException("Date(s) not provided");
                    }
                    break;
                case displaySearch:
                    if (args[4] != null)
                    {
                        string phrase = args[4];
                        commitsToWrite = repo.getPhraseSpecificCommitList(commitList, phrase);
                    }
                    else
                    {
                        throw new ArgumentNullException("phrase error");
                    }
                    break;
                case displayGroupedTickets:
                    commitsToWrite = repo.getGroupCommitList(commitList);
                    break;
                case displayBuildChanges:
                    if (args[4] != null)
                    {
                        string newVersion = args[4];
                        commitsToWrite = repo.getVersionDiffCommitList(commitList, newVersion);
                    }
                    else
                    {
                        throw new ArgumentNullException("version number error");
                    }
                    break;
                default:
                    throw new ArgumentNullException("Unknown request");

            }

            return commitsToWrite;
        }
    }
}
