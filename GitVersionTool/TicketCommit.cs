using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace GitVersionTool
{
    public class TicketCommit
    {
        private List<int> ticketNumberList;
        private string pattern = @"(#)(\d+)";


        public TicketCommit(string MessageShort)
        {
            var ticketArray = Regex.Matches(MessageShort, pattern);

            ticketNumberList = ticketArray.Cast<Match>().Select(match => ConvertPatternMatchToTicketNumber(match.ToString())).ToList();
        }

        public List<int> GetTicketNumberList()
        {
            return ticketNumberList;
        }

        private int ConvertPatternMatchToTicketNumber(string patternMatch)
        {
            // Pattern format #ddd
            return int.Parse(patternMatch.Substring(1));
        }

    }
}
