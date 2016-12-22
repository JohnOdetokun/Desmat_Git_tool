using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    class Driver
    {
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
                RepositoryInformation checkRepo = RepositoryInformation.GetRepositoryInformationForPath(path);
                Data data = new Data(path);
                if (checkRepo != null)
                {
                    Console.WriteLine("Gathering commit info...");
                    List<RepositoryInformation.CommitFormat> commitList = checkRepo.getCommitList();
                    Console.WriteLine("Commit info gathered");
                    FileOutWrite fileWriter = new FileOutWrite(fileWriteName, fileWritePath, commitList, checkRepo);
                    switch (option)
                    {
                        case "-displayAll":
                            fileWriter.setList(commitList);
                            fileWriter.writeFiles();
                            break;
                        case "-displayBetweenTwoDates":
                            if (args[4] != null && args[5] != null)
                            {
                                string[] date1info = args[4].Split('-');
                                string[] date2info = args[5].Split('-');
                                DateTime date1 = new DateTime(int.Parse(date1info[2]), int.Parse(date1info[1]), int.Parse(date1info[0]));
                                DateTime date2 = new DateTime(int.Parse(date2info[2]), int.Parse(date2info[1]), int.Parse(date2info[0]));
                                fileWriter.setList(checkRepo.getInRangeCommitList(commitList, date1, date2));
                            }
                            else if (args[5] != null)
                            {
                                string[] dateInfo = args[4].Split('-');
                                DateTime date = new DateTime(int.Parse(dateInfo[2]), int.Parse(dateInfo[1]), int.Parse(dateInfo[0]));
                                fileWriter.setList(checkRepo.getDateCommitList(commitList, date));
                            }
                            else
                            {
                                throw new ArgumentNullException("Date(s) not provided");
                            }
                            fileWriter.writeFiles();
                            break;
                        case "-searchPhrase":
                            if (args[4] != null)
                            {
                                string phrase = args[4];
                                fileWriter.setList(checkRepo.getPhraseSpecificCommitList(commitList, phrase));
                            }
                            else
                            {
                                throw new ArgumentNullException("phrase error");
                            }
                            fileWriter.writeFiles();
                            break;
                        case "-displayGroupedTickets":
                            fileWriter.setList(checkRepo.getGroupCommitList(commitList));
                            fileWriter.writeFiles();
                            break;
                        case "-displayChangesBetweenLastTwoBuilds":
                            if (args[4] != null)
                            {
                                string newVersion = args[4];
                                fileWriter.setList(checkRepo.getVersionDiffCommitList(commitList, newVersion));
                            }
                            else
                            {
                                throw new ArgumentNullException("version number error");
                            }
                            fileWriter.writeFiles();
                            break;
                        default:
                            throw new ArgumentNullException("Unknown request");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
