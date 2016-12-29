using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

    }
}
