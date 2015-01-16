using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToAzure;
using RedDog.Search.Model;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Abstractions;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Pipelines.QueryGlobalFilters;
using Sitecore.ContentSearch.Security;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Diagnostics;

namespace AzureProvider
{
    public class AzureSearchContext : IProviderSearchContext, IDisposable
    {
        private readonly AzureSearchIndex index;
        private readonly SearchSecurityOptions securityOptions;
        private readonly IContentSearchConfigurationSettings contentSearchSettings;
        //private ISettings settings;

        public AzureSearchContext(AzureSearchIndex index, SearchSecurityOptions securityOptions = SearchSecurityOptions.EnableSecurityCheck)
        {
            Assert.ArgumentNotNull(index, "index");
            this.index = index;
            this.securityOptions = securityOptions;
            contentSearchSettings = this.index.Locator.GetInstance<IContentSearchConfigurationSettings>();
        }
        public void Dispose()
        {
            
        }

        public IQueryable<TItem> GetQueryable<TItem>(params Sitecore.ContentSearch.Linq.Common.IExecutionContext[] executionContexts)
        {            
            var linqToSolrIndex = new LinqToAzureIndex<TItem>(this, executionContexts);
            if (this.contentSearchSettings.EnableSearchDebug())
            {
                ((IHasTraceWriter)linqToSolrIndex).TraceWriter = new LoggingTraceWriter(SearchLog.Log);
            }
            QueryGlobalFiltersArgs queryGlobalFiltersArg = new QueryGlobalFiltersArgs(linqToSolrIndex.GetQueryable(), typeof(TItem), executionContexts.ToList<IExecutionContext>());
            this.Index.Locator.GetInstance<ICorePipeline>().Run("contentSearch.getGlobalLinqFilters", queryGlobalFiltersArg);
            return (IQueryable<TItem>)queryGlobalFiltersArg.Query;
            
            return null;
        }

        public IQueryable<TItem> GetQueryable<TItem>(Sitecore.ContentSearch.Linq.Common.IExecutionContext executionContext)
        {
            return this.GetQueryable<TItem>(new IExecutionContext[] { executionContext });
        }

        public IQueryable<TItem> GetQueryable<TItem>()
        {
            return this.GetQueryable<TItem>(new IExecutionContext[0]);
        }

        public IEnumerable<SearchIndexTerm> GetTermsByFieldName(string fieldName, string prefix)
        {
            throw new NotImplementedException();
        }

        public ISearchIndex Index
        {
            get { return this.index; }
        }

        public Sitecore.ContentSearch.Security.SearchSecurityOptions SecurityOptions
        {
            get { return this.securityOptions; }
        }
    }
}
