using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using RedDog.Search.Model;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Abstractions;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Methods;
using Sitecore.ContentSearch.Pipelines.IndexingFilters;
using Sitecore.ContentSearch.Security;
using Sitecore.ContentSearch.Utilities;

namespace AzureProvider
{
    public struct AzureSearchResults<TElement>
    {
        private readonly AzureSearchContext context;

        public readonly SearchQueryResult searchResults;

        private readonly AzureIndexConfiguration solrIndexConfiguration;

        private readonly IIndexDocumentPropertyMapper<Dictionary<string,object>> mapper;

        private readonly SelectMethod selectMethod;

        private readonly IEnumerable<IExecutionContext> executionContexts;

        private readonly IEnumerable<IFieldQueryTranslator> virtualFieldProcessors;

        private readonly int numberFound;

        public int NumberFound
        {
            get
            {
                return this.numberFound;
            }
        }

        public AzureSearchResults(AzureSearchContext context, SearchQueryResult searchResults, SelectMethod selectMethod, IEnumerable<IExecutionContext> executionContexts, IEnumerable<IFieldQueryTranslator> virtualFieldProcessors)
        {
            OverrideExecutionContext<IIndexDocumentPropertyMapper<Dictionary<string, object>>> overrideExecutionContext;
            object overrideObject;
            this.context = context;
            this.solrIndexConfiguration = (AzureIndexConfiguration)this.context.Index.Configuration;
            this.selectMethod = selectMethod;
            this.virtualFieldProcessors = virtualFieldProcessors;
            this.executionContexts = executionContexts;
            this.numberFound = searchResults.Count;
            this.searchResults = AzureSearchResults<TElement>.ApplySecurity(searchResults, context.SecurityOptions, context.Index.Locator.GetInstance<ICorePipeline>(), context.Index.Locator.GetInstance<IAccessRight>(), ref this.numberFound);
            if (this.executionContexts != null)
            {
                overrideExecutionContext = this.executionContexts.FirstOrDefault<IExecutionContext>((IExecutionContext c) => c is OverrideExecutionContext<IIndexDocumentPropertyMapper<Dictionary<string, object>>>) as OverrideExecutionContext<IIndexDocumentPropertyMapper<Dictionary<string, object>>>;
            }
            else
            {
                overrideExecutionContext = null;
            }
            OverrideExecutionContext<IIndexDocumentPropertyMapper<Dictionary<string, object>>> overrideExecutionContext1 = overrideExecutionContext;
            if (overrideExecutionContext1 != null)
            {
                overrideObject = overrideExecutionContext1.OverrideObject;
            }
            else
            {
                overrideObject = null;
            }
            if (overrideObject == null)
            {
                overrideObject = this.solrIndexConfiguration.IndexDocumentPropertyMapper;
            }
            this.mapper = (IIndexDocumentPropertyMapper<Dictionary<string,object>>)overrideObject;
        }

        public bool Any()
        {
            return this.numberFound > 0;
        }

        private static SearchQueryResult ApplySecurity(SearchQueryResult solrQueryResults, SearchSecurityOptions options, ICorePipeline pipeline, IAccessRight accessRight, ref int numberFound)
        {
            object obj;
            object obj1;
            if (!options.HasFlag(SearchSecurityOptions.DisableSecurityCheck))
            {
                List<SearchQueryRecord> filteredrecord = new List<SearchQueryRecord>();
                foreach (SearchQueryRecord strs in solrQueryResults.Records.Where(x=>x != null))                    
                {
                    if (!strs.Properties.TryGetValue("_uniqueid", out obj))
                    {
                        filteredrecord.Add(strs);
                    }
                    else
                    {
                        strs.Properties.TryGetValue("_datasource", out obj1);
                        if (OutboundIndexFilterPipeline.CheckItemSecurity(pipeline, accessRight, new OutboundIndexFilterArgs((string)obj, (string)obj1)))
                        {
                            filteredrecord.Add(strs);
                        }
                    }
                    
                    solrQueryResults.Records = filteredrecord;
                    solrQueryResults.Count = numberFound = filteredrecord.Count;
                }                
            }
            return solrQueryResults;
        }

        public long Count()
        {
            return (long)this.numberFound;
        }

        public TElement ElementAt(int index)
        {
            if (index < 0 || index > this.searchResults.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return this.mapper.MapToType<TElement>(this.searchResults.Records.ElementAt(index).Properties, this.selectMethod, this.virtualFieldProcessors, this.executionContexts, this.context.SecurityOptions);
        }

        public TElement ElementAtOrDefault(int index)
        {
            if (index < 0 || index > this.searchResults.Count)
            {
                return default(TElement);
            }
            return this.mapper.MapToType<TElement>(this.searchResults.Records.ElementAt(index).Properties, this.selectMethod, this.virtualFieldProcessors, this.executionContexts, this.context.SecurityOptions);
        }

        public TElement First()
        {
            if (this.searchResults.Count < 1)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            return this.ElementAt(0);
        }

        public TElement FirstOrDefault()
        {
            if (this.searchResults.Count < 1)
            {
                return default(TElement);
            }
            return this.ElementAt(0);
        }

        //private ICollection<KeyValuePair<string, int>> Flatten(IEnumerable<Pivot> pivots, string parentName)
        //{
        //    HashSet<KeyValuePair<string, int>> keyValuePairs = new HashSet<KeyValuePair<string, int>>();
        //    foreach (Pivot pivot in pivots)
        //    {
        //        if (parentName != string.Empty)
        //        {
        //            keyValuePairs.Add(new KeyValuePair<string, int>(string.Concat(parentName, "/", pivot.Value), pivot.Count));
        //        }
        //        if (!pivot.HasChildPivots)
        //        {
        //            continue;
        //        }
        //        keyValuePairs.UnionWith(this.Flatten(pivot.ChildPivots, pivot.Value));
        //    }
        //    return keyValuePairs;
        //}

        public Dictionary<string, FacetResult[]> GetFacets()
        {
            return this.searchResults.Facets;
            //IDictionary<string, ICollection<KeyValuePair<string, int>>> facetFields = this.searchResults.f;
            //IDictionary<string, IList<Pivot>> facetPivots = this.searchResults.FacetPivots;
            //Dictionary<string, ICollection<KeyValuePair<string, int>>> dictionary = facetFields.ToDictionary<KeyValuePair<string, ICollection<KeyValuePair<string, int>>>, string, ICollection<KeyValuePair<string, int>>>((KeyValuePair<string, ICollection<KeyValuePair<string, int>>> x) => x.Key, (KeyValuePair<string, ICollection<KeyValuePair<string, int>>> x) => x.Value);
            //if (facetPivots.Count > 0)
            //{
            //    foreach (KeyValuePair<string, IList<Pivot>> facetPivot in facetPivots)
            //    {
            //        dictionary[facetPivot.Key] = this.Flatten(facetPivot.Value, string.Empty);
            //    }
            //}
            //return dictionary;
        }

        public IEnumerable<SearchHit<TElement>> GetSearchHits()
        {
            object obj;
            foreach (SearchQueryRecord strs in this.searchResults.Records)
            {
                float single = -1f;
                single = float.Parse(strs.Score.ToString());
                yield return new SearchHit<TElement>(single, this.mapper.MapToType<TElement>(strs.Properties, this.selectMethod, this.virtualFieldProcessors, this.executionContexts, this.context.SecurityOptions));
            }
        }

        public IEnumerable<TElement> GetSearchResults()
        {
            foreach (SearchQueryRecord strs in this.searchResults.Records)
            {
                yield return this.mapper.MapToType<TElement>(strs.Properties, this.selectMethod, this.virtualFieldProcessors, this.executionContexts, this.context.SecurityOptions);
            }
        }
        public SearchQueryResult GetSearchResult()
        {
            return this.searchResults;
        }
        
        public TElement Last()
        {
            if (this.searchResults.Count < 1)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            return this.ElementAt(this.searchResults.Count - 1);
        }

        public TElement LastOrDefault()
        {
            if (this.searchResults.Count < 1)
            {
                return default(TElement);
            }
            return this.ElementAt(this.searchResults.Count - 1);
        }

        public TElement Single()
        {
            if (this.Count() < (long)1)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            if (this.Count() > (long)1)
            {
                throw new InvalidOperationException("Sequence contains more than one element");
            }
            return this.mapper.MapToType<TElement>(this.searchResults.Records.ElementAt(0).Properties, this.selectMethod, this.virtualFieldProcessors, this.executionContexts, this.context.SecurityOptions);
        }

        public TElement SingleOrDefault()
        {
            if (this.Count() == (long)0)
            {
                return default(TElement);
            }
            if (this.Count() != (long)1)
            {
                throw new InvalidOperationException("Sequence contains more than one element");
            }
            return this.mapper.MapToType<TElement>(this.searchResults.Records.ElementAt(0).Properties, this.selectMethod, this.virtualFieldProcessors, this.context.SecurityOptions);
        }
    }
}
