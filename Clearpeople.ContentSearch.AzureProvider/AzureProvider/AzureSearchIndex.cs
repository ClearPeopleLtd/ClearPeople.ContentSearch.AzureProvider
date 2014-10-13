using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;

namespace AzureProvider
{
    public class AzureSearchIndex : ISearchIndex
    {
        public AzureSearchIndex(string name)
        {
            this._name = name;
        }
        public void AddCrawler(IProviderCrawler crawler)
        {
            crawler.Initialize(this);
            this.Crawlers.Add(crawler);
        }

        public void AddStrategy(Sitecore.ContentSearch.Maintenance.Strategies.IIndexUpdateStrategy strategy)
        {
            throw new NotImplementedException();
        }

        public ProviderIndexConfiguration Configuration
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<IProviderCrawler> Crawlers
        {
            get { throw new NotImplementedException(); }
        }

        public IProviderDeleteContext CreateDeleteContext()
        {
            throw new NotImplementedException();
        }

        public IProviderSearchContext CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions options = Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck)
        {
            throw new NotImplementedException();
        }

        public IProviderUpdateContext CreateUpdateContext()
        {
            throw new NotImplementedException();
        }

        public void Delete(IIndexableUniqueId indexableUniqueId, IndexingOptions indexingOptions)
        {
            throw new NotImplementedException();
        }

        public void Delete(IIndexableUniqueId indexableUniqueId)
        {
            throw new NotImplementedException();
        }

        public void Delete(IIndexableId indexableId, IndexingOptions indexingOptions)
        {
            throw new NotImplementedException();
        }

        public void Delete(IIndexableId indexableId)
        {
            throw new NotImplementedException();
        }

        public AbstractFieldNameTranslator FieldNameTranslator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IndexingState IndexingState
        {
            get { throw new NotImplementedException(); }
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public Sitecore.ContentSearch.Abstractions.IObjectLocator Locator
        {
            get { throw new NotImplementedException(); }
        }

        private string _name;
        public string Name
        {
            get { return this._name; }
        }

        public IIndexOperations Operations
        {
            get { throw new NotImplementedException(); }
        }

        public void PauseIndexing()
        {
            throw new NotImplementedException();
        }

        public Sitecore.ContentSearch.Maintenance.IIndexPropertyStore PropertyStore
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Rebuild(IndexingOptions indexingOptions)
        {
            throw new NotImplementedException();
        }

        public void Rebuild()
        {
            throw new NotImplementedException();
        }

        public Task RebuildAsync(IndexingOptions indexingOptions, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Refresh(IIndexable indexableStartingPoint, IndexingOptions indexingOptions)
        {
            throw new NotImplementedException();
        }

        public void Refresh(IIndexable indexableStartingPoint)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(IIndexable indexableStartingPoint, IndexingOptions indexingOptions, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void ResumeIndexing()
        {
            throw new NotImplementedException();
        }

        public ISearchIndexSchema Schema
        {
            get { throw new NotImplementedException(); }
        }

        public void StopIndexing()
        {
            throw new NotImplementedException();
        }

        public ISearchIndexSummary Summary
        {
            get { throw new NotImplementedException(); }
        }

        public void Update(IEnumerable<IndexableInfo> indexableInfo)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<IIndexableUniqueId> indexableUniqueIds, IndexingOptions indexingOptions)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<IIndexableUniqueId> indexableUniqueIds)
        {
            throw new NotImplementedException();
        }

        public void Update(IIndexableUniqueId indexableUniqueId, IndexingOptions indexingOptions)
        {
            throw new NotImplementedException();
        }

        public void Update(IIndexableUniqueId indexableUniqueId)
        {
            throw new NotImplementedException();
        }
    }
}
