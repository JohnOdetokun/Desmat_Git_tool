using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    class GitAccess
    {
        private bool _disposed;
        private readonly Repository _repo;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _repo.Dispose();
            }
        }

        private GitAccess(string path)
        {
            _repo = new Repository(path);
        }

        public static GitAccess GetRepositoryInformationForPath(string path)
        {
            if (LibGit2Sharp.Repository.IsValid(path))
            {
                return new GitAccess(path);
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
    }
}
