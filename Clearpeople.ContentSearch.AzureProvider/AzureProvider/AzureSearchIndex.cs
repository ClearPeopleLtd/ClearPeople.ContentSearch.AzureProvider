using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Diagnostics;

namespace AzureProvider
{
    public class AzureSearchIndex : ISearchIndex
    {
        public AzureSearchIndex(string name)
        {
            this._name = name;
            this.Crawlers = new List<IProviderCrawler>();
        }
        public void AddCrawler(IProviderCrawler crawler)
        {
            crawler.Initialize(this);
            this.Crawlers.Add(crawler);
        }

        public void AddStrategy(Sitecore.ContentSearch.Maintenance.Strategies.IIndexUpdateStrategy strategy)
        {
            
        }

        public ProviderIndexConfiguration Configuration { get; set; }

        public IList<IProviderCrawler> Crawlers { get; set; }

        public IProviderDeleteContext CreateDeleteContext()
        {
            return null;
        }

        public IProviderSearchContext CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions options = Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck)
        {
            return null;
        }

        public IProviderUpdateContext CreateUpdateContext()
        {
            return new AzureUpdateContext(this);
        }

        public void Delete(IIndexableUniqueId indexableUniqueId, IndexingOptions indexingOptions)
        {
            
        }

        public void Delete(IIndexableUniqueId indexableUniqueId)
        {
            
        }

        public void Delete(IIndexableId indexableId, IndexingOptions indexingOptions)
        {
            
        }

        public void Delete(IIndexableId indexableId)
        {
            
        }

        public AbstractFieldNameTranslator FieldNameTranslator
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }

        public IndexingState IndexingState
        {
            get { return IndexingState.Stopped; }
        }

        public void Initialize()
        {
            
        }

        public Sitecore.ContentSearch.Abstractions.IObjectLocator Locator
        {
            get { return null; }
        }

        private string _name;
        public string Name
        {
            get { return this._name; }
        }

        public IIndexOperations Operations
        {
            get { return null; }
        }

        public void PauseIndexing()
        {
            
        }

        public Sitecore.ContentSearch.Maintenance.IIndexPropertyStore PropertyStore { get; set; }

        public void Rebuild(IndexingOptions indexingOptions)
        {
            
        }

        public void Rebuild()
        {
            
        }

        public Task RebuildAsync(IndexingOptions indexingOptions, System.Threading.CancellationToken cancellationToken)
        {
            return null;
        }

        public void Refresh(IIndexable indexableStartingPoint, IndexingOptions indexingOptions)
        {
            using (var context = this.CreateUpdateContext())
            {
                foreach (var crawler in this.Crawlers)
                {
                    crawler.RefreshFromRoot(context, indexableStartingPoint, indexingOptions);
                }
                context.Optimize();
                context.Commit();
            }
        }

        public void Refresh(IIndexable indexableStartingPoint)
        {
            using (var context = this.CreateUpdateContext())
            {
                foreach (var crawler in this.Crawlers)
                {
                    crawler.RefreshFromRoot(context, indexableStartingPoint, IndexingOptions.ForcedIndexing);
                }
                context.Optimize();
                context.Commit();
            }
        }

        public Task RefreshAsync(IIndexable indexableStartingPoint, IndexingOptions indexingOptions, System.Threading.CancellationToken cancellationToken)
        {
            return null;
        }

        public void Reset()
        {
            
        }

        public void ResumeIndexing()
        {
            
        }

        public ISearchIndexSchema Schema
        {
            get { return null; }
        }

        public void StopIndexing()
        {
            
        }

        public ISearchIndexSummary Summary
        {
            get { return null; }
        }

        public void Update(IEnumerable<IndexableInfo> indexableInfo)
        {
            
        }

        public void Update(IEnumerable<IIndexableUniqueId> indexableUniqueIds, IndexingOptions indexingOptions)
        {
            
        }

        public void Update(IEnumerable<IIndexableUniqueId> indexableUniqueIds)
        {
            
        }

        public void Update(IIndexableUniqueId indexableUniqueId, IndexingOptions indexingOptions)
        {
            
        }

        public void Update(IIndexableUniqueId indexableUniqueId)
        {
            
        }
    }
}
