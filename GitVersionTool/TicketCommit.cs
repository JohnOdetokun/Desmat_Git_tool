using LibGit2Sharp;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    public class TicketCommit
    {
        private List<int> ticketNumberList;
        private string pattern = @"(#)(\d+)";


        public TicketCommit(string MessageShort)
        {
            var ticketArray = Regex.Matches(MessageShort, pattern);
            ticketNumberList = ticketArray.Cast<Match>().Select(match => int.Parse(match.ToString().Substring(1))).ToList();
        }

        public List<int> GetTicketNumberList()
        {
            return ticketNumberList;
        }

    }
}
