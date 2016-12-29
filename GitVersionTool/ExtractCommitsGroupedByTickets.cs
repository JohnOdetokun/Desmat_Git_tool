using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    class ExtractCommitsGroupedByTickets : ICommitFilter
    {
        private RepositoryInformation repo;

        public ExtractCommitsGroupedByTickets(RepositoryInformation repo)
        {
            this.repo = repo;
        }

        public List<Commit> ExtractCommits()
        {
            Dictionary<int, List<Commit>> ticketDictionary = GetTicketDictionary();
            List<Commit> groupCommitList = new List<Commit>();
            List<int> keys = ticketDictionary.Keys.ToList<int>();
            keys.Sort();
            keys.Reverse();
            foreach (var key in keys)
            {
                foreach (Commit commit in ticketDictionary[key])
                {
                    groupCommitList.Add(commit);
                }
            }
            return groupCommitList;
        }

        public void ValidateArgs(){}

        private Dictionary<int, List<Commit>> GetTicketDictionary()
        {
            List<Commit> commitList = repo.Log.ToList<Commit>();
            Dictionary<int, List<Commit>> ticketDictionary = new Dictionary<int, List<Commit>>();
            foreach (Commit commit in commitList)
            {
                TicketCommit ticket = new TicketCommit(commit.MessageShort);
                List<int> ticketnumberList = ticket.GetTicketNumberList();
                foreach(int ticketNumber in ticketnumberList)
                {
                    if (ticketDictionary.ContainsKey(ticketNumber))
                    {
                        List<Commit> list = ticketDictionary[ticketNumber];
                        if (list.Contains(commit) == false)
                        {
                            list.Add(commit);
                        }
                    }
                    else
                    {
                        List<Commit> list = new List<Commit>();
                        list.Add(commit);
                        ticketDictionary.Add(ticketNumber, list);
                    }
                }
            }
            return ticketDictionary;
        }
    }
}
