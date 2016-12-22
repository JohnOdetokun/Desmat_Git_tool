using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    public class FileOutWrite
    {
        string fileWriteName;
        string fileWritePath;
        List<RepositoryInformation.CommitFormat> commitList;
        RepositoryInformation repo;

        public FileOutWrite(string fileWriteName
            , string fileWritePath
            , List<RepositoryInformation.CommitFormat> commitList
            , RepositoryInformation repo)
        {
            this.fileWriteName = fileWriteName;
            this.fileWritePath = fileWritePath;
            this.commitList = commitList;
            this.repo = repo;
        }

        public void setList(List<RepositoryInformation.CommitFormat> list)
        {
            commitList = list;
        }

        public void writeFiles()
        {
            Console.WriteLine("Writing to chosen file...");
            writeCommitFormat(fileWriteName, fileWritePath, commitList);
            CSVwriteCommitFormat(fileWriteName, fileWritePath, commitList);
            HTMLwriteCommitFormat(fileWriteName, fileWritePath, commitList, repo);
            Console.WriteLine("Go to \"" + fileWritePath + "\" to view .csv, .txt and .htm files with name: \"" + fileWriteName + "\"\nPRESS ENTER TO EXIT");
            Console.ReadLine();
        }

        private bool writeCommitFormat(string filename, string fileWritePath, List<RepositoryInformation.CommitFormat> commits)
        {
            try
            {
                using (System.IO.StreamWriter file1 = new System.IO.StreamWriter(fileWritePath + "\\" + filename + ".txt"))
                {
                    string dateFormat = "dddd, MMM dd yyyy HH:mm:ss";
                    foreach (RepositoryInformation.CommitFormat commit in commits)
                    {
                        file1.WriteLine("Author: {0}{1}Committer: {2}{3}Message: {4}{5}Date and Time: {6}{7}",
                            commit.author, Environment.NewLine, commit.committer, Environment.NewLine, commit.message, Environment.NewLine,
                            commit.dateTimeOffset.ToString(dateFormat), Environment.NewLine);

                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
                return false;
            }
        }

        private bool CSVwriteCommitFormat(string fileName, string fileWritePath, List<RepositoryInformation.CommitFormat> formatCommitList)
        {
            try
            {
                string dateFormat = "dddd, MMM dd yyyy HH:mm:ss";
                using (StreamWriter sw = new StreamWriter(fileWritePath + "\\" + fileName + ".csv"))
                {
                    sw.WriteLine("Author, Committer, Message, DateTime");
                    foreach (RepositoryInformation.CommitFormat commit in formatCommitList)
                    {
                        sw.WriteLine(String.Format("{0},{1},{2},{3}", commit.author.Replace(',', ';'), commit.committer.Replace(',', ';'),
                            commit.message.Replace(',', ';'), commit.dateTimeOffset.ToString(dateFormat).Replace(',', ';')));
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        private bool HTMLwriteCommitFormat(string fileName, string fileWritePath, List<RepositoryInformation.CommitFormat> commitList, RepositoryInformation repo)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileWritePath + "\\" + fileName + ".htm"))
                {
                    file.WriteLine("<table class=\"table table - bordered table - hover table - condensed\">");
                    file.WriteLine("<thead>");
                    file.WriteLine("<tr><th title=\"Field #1\">Author</th>");
                    file.WriteLine("<th title=\"Field #2\">Committer</th>");
                    file.WriteLine("<th title=\"Field #3\">Message</th>");
                    file.WriteLine("<th title=\"Field #4\">Link to Assembla</th>");
                    file.WriteLine("<th title=\"Field #5\">DateTime</th> ");
                    file.WriteLine("</tr></thead>");
                    file.WriteLine("<tbody>");
                    string dateFormat = "dddd, MMM dd yyyy HH:mm:ss";
                    foreach (RepositoryInformation.CommitFormat commit in commitList)
                    {
                        file.WriteLine("<tr>");
                        file.WriteLine("<td>" + commit.author + "</td>");
                        file.WriteLine("<td>" + commit.committer + "</td>");
                        file.WriteLine("<td>" + commit.message + "</td>");
                        bool isValidTicket = repo.checkTicket(commit.message);
                        if (isValidTicket)
                        {
                            int ticketNumber = repo.getTicketNumber(commit.message);
                            file.WriteLine("<td class=\"formtext\"><a href= \"https://entelect.assembla.com/spaces/deloitte-desmat/tickets/realtime_list?ticket=" + ticketNumber + "\">Go to ticket in assembla</a></td>");
                        }
                        else
                        {
                            file.WriteLine("<td></td>");
                        }
                        file.WriteLine("<td>" + commit.dateTimeOffset.ToString(dateFormat) + "</td>");
                        file.WriteLine("</tr>");
                    }
                    file.WriteLine("</tbody></table>");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return false;
            }
        }
    }
}
