using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;

namespace AzureProvider
{
    public class AzureSearchIndexSummary:ISearchIndexSummary
    {
        public string Directory
        {
            get { return null; }
        }

        public bool HasDeletions
        {
            get { return false; }
        }

        public bool IsClean
        {
            get { return false; }
        }

        public bool IsHealthy
        {
            get { return false; }
        }

        public bool IsMissingSegment
        {
            get { return false; }
        }

        public bool IsOptimized
        {
            get { return false; }
        }

        public DateTime LastUpdated { get; set; }

        public long? LastUpdatedTimestamp { get; set; }

        public int NumberOfBadSegments
        {
            get { return 1; }
        }

        public long NumberOfDocuments
        {
            get { return 1; }
        }

        public int NumberOfFields
        {
            get { return 1; }
        }

        public long NumberOfTerms
        {
            get { return 1; }
        }

        public bool OutOfDateIndex
        {
            get { return false; }
        }

        public IDictionary<string, string> UserData
        {
            get { return new ConcurrentDictionary<string, string>();}
        }
    }
}
