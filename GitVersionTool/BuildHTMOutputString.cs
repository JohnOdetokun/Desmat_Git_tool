using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    class BuildHTMOutputString : IBuildOutputString<Commit>
    {
        StringBuilder outputStringBuilder = new StringBuilder();
        string dateFormat = "dddd, MMM dd yyyy HH:mm:ss";

        public string BuildString(List<Commit> filteredList)
        {
            outputStringBuilder.AppendLine("<table class=\"table table - bordered table - hover table - condensed\">");
            outputStringBuilder.AppendLine("<thead>");
            outputStringBuilder.AppendLine("<tr><th title=\"Field #1\">Author</th>");
            outputStringBuilder.AppendLine("<th title=\"Field #2\">Committer</th>");
            outputStringBuilder.AppendLine("<th title=\"Field #3\">Message</th>");
            outputStringBuilder.AppendLine("<th title=\"Field #4\">Link to Assembla</th>");
            outputStringBuilder.AppendLine("<th title=\"Field #5\">DateTime</th> ");
            outputStringBuilder.AppendLine("</tr></thead>");
            outputStringBuilder.AppendLine("<tbody>");
            foreach (Commit commit in filteredList)
            {
                outputStringBuilder.AppendLine("<tr>");
                outputStringBuilder.AppendLine("<td>" + commit.Author.ToString() + "</td>");
                outputStringBuilder.AppendLine("<td>" + commit.Committer.ToString() + "</td>");
                outputStringBuilder.AppendLine("<td>" + commit.MessageShort + "</td>");

                TicketCommit ticket = new TicketCommit(commit.MessageShort);
                List<int> ticketNumberList = ticket.GetTicketNumberList();
                bool isCommit = ticketNumberList.ToArray<int>().Length != 0;
                if (isCommit)
                {
                    outputStringBuilder.AppendLine("<td class=\"formtext\">" + AssemblaTicektLink(ticketNumberList) + "</td>");
                }
                else
                {
                    outputStringBuilder.AppendLine("<td></td>");
                }
                outputStringBuilder.AppendLine("<td>" + commit.Committer.When.ToString(dateFormat) + "</td>");
                outputStringBuilder.AppendLine("</tr>");
            }
            outputStringBuilder.AppendLine("</tbody></table>");
            return outputStringBuilder.ToString();
        }

        public string AssemblaTicektLink(List<int> ticketList)
        {
            StringBuilder ticketLink = new StringBuilder();
            if(ticketList.Count != 0)
            {
                foreach (int ticketNumber in ticketList)
                {
                    ticketLink.AppendLine("<a href = \"https://entelect.assembla.com/spaces/deloitte-desmat/tickets/realtime_list?ticket=" + ticketNumber + "\">Go to ticket " +ticketNumber + " in assembla</a>, ");
                }
            }
            else
            {
                return "";
            }
            string stringOfLinks = ticketLink.ToString();
            return stringOfLinks.Remove(stringOfLinks.Length -2);
        }

        public string Extension()
        {
            return "htm";
        }




    }
}
