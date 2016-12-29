﻿using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool
{
    class ExtractBuildChangeCommits : ICommitFilter
    {
        private RepositoryInformation repo;
        private string[] args;
        string newVersion;
        string oldVersion;
        List<Tag> versionTags;
        DateTimeOffset newVersionDate;
        DateTimeOffset oldVersionDate;

        public ExtractBuildChangeCommits(RepositoryInformation repo, string[] args)
        {
            this.repo = repo;
            this.args = args;
            ValidateArgs();
            newVersion = args[4];
            oldVersion = DecrementVersion(newVersion);
            versionTags = GetVersionTags(repo.Tags.ToList());
            newVersionDate = GetVersionDateTimeOffset(newVersion, versionTags);
            oldVersionDate = GetVersionDateTimeOffset(oldVersion, versionTags);
        }

        public List<Commit> ExtractCommits()
        {
            List<Commit> inRangeCommits = new List<Commit>();
            foreach (Commit commit in repo.Log.ToList<Commit>())
            {
                DateTime commitDate = commit.Committer.When.DateTime;
                if (IsInRange(oldVersionDate.DateTime, newVersionDate.DateTime, commitDate))
                {
                    inRangeCommits.Add(commit);
                }
            }
            return inRangeCommits;
        }

        public void ValidateArgs()
        {
            if (args[4] == null)
            {
                throw new ArgumentNullException("Version argument not provided, please run program again and version number arg.");
            }
        }

        private bool IsInRange(DateTime rangeBegin, DateTime rangeEnd, DateTime date)
        {
            if (date > rangeBegin && date < rangeEnd)
            {
                return true;
            }
            return false;
        }

        private DateTimeOffset GetVersionDateTimeOffset(string version, List<Tag> versionTag)
        {
            foreach (Tag tag in versionTag)
            {
                if (tag.FriendlyName.Contains(version))
                {
                    Commit commit = GetCommitOfTag(tag);
                    return commit.Committer.When;
                }
            }
            return new DateTimeOffset();
        }

        private Commit GetCommitOfTag(Tag tag)
        {
            foreach (Commit commit in repo.Log.ToList())
            {
                if (tag.PeeledTarget.Id == commit.Id)
                {
                    return commit;
                }
            }
            return null;
        }

        private List<Tag> GetVersionTags(List<Tag> rawTagList)
        {
            List<Tag> versionTagList = new List<Tag>();
            foreach (Tag tag in rawTagList)
            {
                if (tag.FriendlyName.Contains("version"))
                {
                    versionTagList.Add(tag);                    
                }
            }
            return versionTagList;
        }

        private string DecrementVersion(string version)
        {
            string[] majorMinor = version.Split('.');
            string minor = (Int32.Parse(majorMinor[1]) - 1).ToString();
            return majorMinor[0] + "." + minor;
        }
    }
}