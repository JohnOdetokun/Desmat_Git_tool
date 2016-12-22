using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    public class RepositoryInformation : IDisposable
    {
        private bool _disposed;
        private readonly Repository _repo;

        public static RepositoryInformation GetRepositoryInformationForPath(string path)
        {
            if (LibGit2Sharp.Repository.IsValid(path))
            {
                return new RepositoryInformation(path);
            }
            return null;
        }

        public string CommitHash
        {
            get
            {
                return _repo.Head.Tip.Sha;
            }
        }

        public string BranchName
        {
            get
            {
                return _repo.Head.FriendlyName;
            }
        }

        public string TrackedBranchName
        {
            get
            {
                return _repo.Head.IsTracking ? _repo.Head.TrackedBranch.FriendlyName : String.Empty;
            }
        }

        public bool HasUnpushedCommits
        {
            get
            {
                return _repo.Head.TrackingDetails.AheadBy > 0;
            }
        }

        public bool HasUncommittedChanges
        {
            get
            {
                return _repo.RetrieveStatus().Any(s => s.State != FileStatus.Ignored);
            }
        }

        public IEnumerable<Commit> Log
        {
            get
            {
                return _repo.Head.Commits;
            }
        }

        public List<CommitFormat> getCommitList()
        {
            List<Commit> rawCommitList = Log.ToList();
            List<CommitFormat> commitList = new List<CommitFormat>();
            foreach (Commit commit in rawCommitList)
            {
                CommitFormat temp;
                temp.author = commit.Author.ToString();
                temp.committer = commit.Committer.ToString();
                temp.message = commit.MessageShort;
                temp.dateTimeOffset = commit.Committer.When;
                commitList.Add(temp);
            }
            return commitList;
        }

        public IEnumerable<Tag> Tags
        {
            get
            {
                return _repo.Tags;
            }
        }

        private static IEnumerable<Tag> AssignedTags(Commit commit, Dictionary<ObjectId, List<Tag>> tags)
        {
            if (!tags.ContainsKey(commit.Id))
            {
                return Enumerable.Empty<Tag>();
            }

            return tags[commit.Id];
        }

        private static Dictionary<ObjectId, List<Tag>> TagsPerPeeledCommitId(Repository repo)
        {
            var tagsPerPeeledCommitId = new Dictionary<ObjectId, List<Tag>>();

            foreach (Tag tag in repo.Tags)
            {
                GitObject peeledTarget = tag.PeeledTarget;

                if (!(peeledTarget is Commit))
                {
                    // We're not interested by Tags pointing at Blobs or Trees
                    continue;
                }

                ObjectId commitId = peeledTarget.Id;

                if (!tagsPerPeeledCommitId.ContainsKey(commitId))
                {
                    // A Commit may be pointed at by more than one Tag
                    tagsPerPeeledCommitId.Add(commitId, new List<Tag>());
                }

                tagsPerPeeledCommitId[commitId].Add(tag);
            }

            return tagsPerPeeledCommitId;
        }

        public void displayTagCommitLinks()
        {
            // Build up a cached dictionary of all the tags that point to a commit
            var dic = TagsPerPeeledCommitId(_repo);

            // Let's enumerate all the reachable commits (similarly to `git log --all`)
            foreach (Commit commit in _repo.Commits)
            {
                foreach (var tags in AssignedTags(commit, dic))
                {
                    Console.WriteLine("Tag {0} points at {1}", tags.FriendlyName, commit.Id);
                    Console.Read();
                }
            }
            Console.Read();
        }

        public bool checkTicket(string message)
        {
            char[] messageArray = message.ToCharArray();
            // message begins with "# "
            if (messageArray[0] == '#' && Char.IsNumber(messageArray[1]))
            {
                return true;
            }
            // message begins with "re #"
            else if (messageArray[0] == 'r' && messageArray[1] == 'e' && messageArray[3] == '#' && Char.IsNumber(messageArray[4]))
            {
                return true;
            }
            return false;
        }

        public int getTicketNumber(string message)
        {
            StringBuilder strTicketNumber = new StringBuilder();
            char[] messageChars = message.ToCharArray();
            int ticketNumber = 0;
            foreach (char numberChar in messageChars)
            {
                if (numberChar == '#' || numberChar == ' ' || numberChar == 'r' || numberChar == 'e')
                {
                    continue;
                }
                else if (Char.IsNumber(numberChar))
                {
                    strTicketNumber.Append(numberChar);
                }
                else
                {
                    break;
                }
            }
            ticketNumber = int.Parse(strTicketNumber.ToString());
            return ticketNumber;
        }

        private Dictionary<int, List<CommitFormat>> getTicketDictionary(List<CommitFormat> commitList)
        {
            Dictionary<int, List<CommitFormat>> ticketDictionary = new Dictionary<int, List<CommitFormat>>();
            foreach (CommitFormat commit in commitList)
            {
                if (checkTicket(commit.message))
                {
                    if (ticketDictionary.ContainsKey(getTicketNumber(commit.message)))
                    {
                        List<CommitFormat> list = ticketDictionary[getTicketNumber(commit.message)];
                        if (list.Contains(commit) == false)
                        {
                            list.Add(commit);
                        }

                    }
                    else
                    {
                        List<CommitFormat> list = new List<CommitFormat>();
                        list.Add(commit);
                        ticketDictionary.Add(getTicketNumber(commit.message), list);

                    }
                }
            }

            return ticketDictionary;
        }

        private bool isInRange(DateTime rangeBegin, DateTime rangeEnd, DateTime date)
        {
            if (date > rangeBegin && date < rangeEnd)
            {
                return true;
            }
            return false;
        }

        public List<CommitFormat> getInRangeCommitList(List<CommitFormat> commitList, DateTime rangeBegin, DateTime rangeEnd)
        {
            List<CommitFormat> inRangeCommits = new List<CommitFormat>();
            foreach (CommitFormat commit in commitList)
            {
                DateTime commitDate = commit.dateTimeOffset.DateTime;
                if (isInRange(rangeBegin, rangeEnd, commitDate))
                {
                    inRangeCommits.Add(commit);
                }
            }
            return inRangeCommits;
        }

        public List<CommitFormat> getDateCommitList(List<CommitFormat> commitList, DateTime date)
        {
            List<CommitFormat> inRangeCommits = new List<CommitFormat>();
            foreach (CommitFormat commit in commitList)
            {
                DateTime commitDate = commit.dateTimeOffset.DateTime;
                string commitDateString = commitDate.Date.ToShortDateString();
                string dateString = date.Date.ToShortDateString();
                if (commitDateString.Equals(dateString))
                {
                    inRangeCommits.Add(commit);
                }
            }
            return inRangeCommits;
        }

        public List<CommitFormat> getPhraseSpecificCommitList(List<CommitFormat> commitList, string phrase)
        {
            List<CommitFormat> filteredCommitList = new List<CommitFormat>();
            foreach (CommitFormat commit in commitList)
            {
                if (commit.message.Contains(phrase))
                {
                    filteredCommitList.Add(commit);
                }
            }
            return filteredCommitList;
        }


        public List<CommitFormat> getGroupCommitList(List<CommitFormat> commitList)
        {
            Dictionary<int, List<CommitFormat>> ticketDictionary = getTicketDictionary(commitList);
            List<CommitFormat> groupCommitList = new List<CommitFormat>();
            foreach (var dictionaryItem in ticketDictionary)
            {
                foreach (CommitFormat commit in dictionaryItem.Value)
                {
                    groupCommitList.Add(commit);
                }
            }
            return groupCommitList;
        }

        public List<CommitFormat> getVersionDiffCommitList(List<CommitFormat> commitList, string newVersion)
        {
            List<Tag> rawTagList = Tags.ToList();
            List<Tag> versionTags = getVersionTags(rawTagList);
            Console.WriteLine("version number {0}", newVersion);
            string oldVersion = decrementVersion(newVersion);
            DateTimeOffset newVersionDate = getVersionDateTimeOffset(newVersion, versionTags);
            Console.WriteLine("new Version DateTime: {0}", newVersionDate.ToString());
            DateTimeOffset oldVersionDate = getVersionDateTimeOffset(oldVersion, versionTags);
            Console.WriteLine("old Version DateTime: {0}", oldVersionDate.ToString());

            return getInRangeCommitList(commitList, oldVersionDate.DateTime, newVersionDate.DateTime);
        }

        private DateTimeOffset getVersionDateTimeOffset(String version, List<Tag> versionTag)
        {


            foreach (Tag tag in versionTag)
            {
                //Console.WriteLine("tag annotation: {0}", tag.FriendlyName);
                if (tag.FriendlyName.Contains(version))
                {

                    Commit commit = getCommitOfTag(tag);
                    return commit.Committer.When;

                }
            }
            return new DateTimeOffset();
        }

        public Commit getCommitOfTag(Tag tag)
        {
            foreach (Commit commit in Log)
            {
                if (tag.PeeledTarget.Id == commit.Id)
                {
                    return commit;
                }
            }
            return null;
        }

        private List<Tag> getVersionTags(List<Tag> rawTagList)
        {
            //Console.WriteLine("Number of tags: {0}", rawTagList.ToArray().Length);
            List<Tag> versionTagList = new List<Tag>();
            foreach (Tag tag in rawTagList)
            {

                //Console.WriteLine("tag annotation: {0}", tag.FriendlyName);
                if (tag.FriendlyName.Contains("version"))
                {
                    versionTagList.Add(tag);
                }
            }
            return versionTagList;
        }

        private string decrementVersion(string version)
        {
            string[] majorMinor = version.Split('.');
            string minor = (Int32.Parse(majorMinor[1]) - 1).ToString();
            return majorMinor[0] + "." + minor;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _repo.Dispose();
            }
        }

        private RepositoryInformation(string path)
        {
            _repo = new Repository(path);
        }

        public struct CommitFormat
        {
            public string author;
            public string committer;
            public string message;
            public DateTimeOffset dateTimeOffset;
        };


    }
}
