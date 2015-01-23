using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LinqToAzure;
using RedDog.Search.Http;
using RedDog.Search.Model;
using Sitecore.Common;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Abstractions;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Indexing;
using Sitecore.ContentSearch.Linq.Methods;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Linq.Parsing;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Diagnostics;

namespace AzureProvider
{
    public class LinqToAzureIndex<TItem> : Index<TItem, AzureCompositeQuery>
    {
         private AzureIndexparameters parameters;
         private QueryMapper<AzureCompositeQuery> queryMapper;
        private AzureFieldTranslator fieldnameTranslator;
        private readonly AzureSearchContext context;

        private readonly string cultureCode;

        //private readonly ISettings settings;

        private readonly AzureIndexConfiguration contentSearchSettings;

        private readonly ICorePipeline pipeline;
        public AzureIndexparameters Parameters
        {
            get
            {
                return this.parameters;
            }
        }
        private bool DoExecuteSearch(AzureCompositeQuery query)
        {
            if (query.Methods.First<QueryMethod>().MethodType == QueryMethodType.GetFacets)
            {
                return false;
            }
            return true;
        }
        private TResult ApplyScalarMethods<TResult, TDocument>(AzureCompositeQuery query, AzureSearchResults<TDocument> processedResults, SearchQueryResult results)
        {
            object totalHits;
            QueryMethod queryMethod = query.Methods.First<QueryMethod>();
            switch (queryMethod.MethodType)
            {
                case QueryMethodType.All:
                    {
                        totalHits = true;
                        break;
                    }
                case QueryMethodType.Any:
                    {
                        totalHits = processedResults.Any();
                        break;
                    }
                case QueryMethodType.Count:
                    {
                        if (query.Methods.Any<QueryMethod>((QueryMethod m) =>
                        {
                            if (m.MethodType == QueryMethodType.Skip)
                            {
                                return true;
                            }
                            return m.MethodType == QueryMethodType.Take;
                        }))
                        {
                            totalHits = processedResults.Count();
                            break;
                        }
                        else
                        {
                            totalHits = results.Count;
                            break;
                        }
                    }
                case QueryMethodType.ElementAt:
                    {
                        if (!((ElementAtMethod)queryMethod).AllowDefaultValue)
                        {
                            totalHits = processedResults.ElementAt(((ElementAtMethod)queryMethod).Index);
                            break;
                        }
                        else
                        {
                            totalHits = processedResults.ElementAtOrDefault(((ElementAtMethod)queryMethod).Index);
                            break;
                        }
                    }
                case QueryMethodType.First:
                    {
                        if (!((FirstMethod)queryMethod).AllowDefaultValue)
                        {
                            totalHits = processedResults.First();
                            break;
                        }
                        else
                        {
                            totalHits = processedResults.FirstOrDefault();
                            break;
                        }
                    }
                case QueryMethodType.Last:
                    {
                        if (!((LastMethod)queryMethod).AllowDefaultValue)
                        {
                            totalHits = processedResults.Last();
                            break;
                        }
                        else
                        {
                            totalHits = processedResults.LastOrDefault();
                            break;
                        }
                    }
                case QueryMethodType.Single:
                    {
                        if (!((SingleMethod)queryMethod).AllowDefaultValue)
                        {
                            totalHits = processedResults.Single();
                            break;
                        }
                        else
                        {
                            totalHits = processedResults.SingleOrDefault();
                            break;
                        }
                    }
                case QueryMethodType.Max:
                case QueryMethodType.Min:
                case QueryMethodType.OrderBy:
                case QueryMethodType.Cast:
                case QueryMethodType.Skip:
                case QueryMethodType.Take:
                case QueryMethodType.Select:
                    {
                        throw new InvalidOperationException(string.Concat("Invalid query method: ", queryMethod.MethodType));
                    }
                case QueryMethodType.GetResults:
                {
                    totalHits = processedResults.searchResults.Records;
                        break;
                    }
                case QueryMethodType.GetFacets:
                    {
                        totalHits = processedResults.searchResults.Facets;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException(string.Concat("Invalid query method: ", queryMethod.MethodType));
                    }
            }
            return (TResult)Convert.ChangeType(totalHits, typeof(TResult));
        }
        private TResult ExecuteScalarMethod<TResult>(AzureCompositeQuery query)
        {
            QueryMethod queryMethod = query.Methods.First<QueryMethod>();
            if (queryMethod.MethodType != QueryMethodType.GetFacets)
            {
                throw new InvalidOperationException(string.Concat("Invalid query method: ", queryMethod.MethodType));
            }
            object obj = this.FindElements<TResult>(query);
            return (TResult)Convert.ChangeType(obj, typeof(TResult));
        }
        private AzureSearchResults<TElement> ApplySearchMethods<TElement>(AzureCompositeQuery query, SearchQueryResult searchHits)
        {
            List<QueryMethod> queryMethods = (query.Methods != null ? new List<QueryMethod>(query.Methods) : new List<QueryMethod>());
            queryMethods.Reverse();
            SelectMethod selectMethod = null;
            foreach (QueryMethod queryMethod in queryMethods)
            {
                if (queryMethod.MethodType != QueryMethodType.Select)
                {
                    continue;
                }
                selectMethod = (SelectMethod)queryMethod;
            }
            int num = 0;
            int length = (int)searchHits.Count - 1;
            return new AzureSearchResults<TElement>(this.context, searchHits,selectMethod, query.ExecutionContexts,query.VirtualFieldProcessors);
        }

        public override TResult Execute<TResult>(AzureCompositeQuery query)
        {
            if (!this.DoExecuteSearch(query))
            {
                return this.ExecuteScalarMethod<TResult>(query);
            }
            if (!typeof(TResult).IsGenericType || !(typeof(TResult).GetGenericTypeDefinition() == typeof(SearchResults<>)))
            {
                var topDoc = this.FindElements(query);
                AzureSearchResults<TResult> luceneSearchResult = this.ApplySearchMethods<TResult>(query, topDoc);
                return this.ApplyScalarMethods<TResult, TResult>(query, luceneSearchResult, topDoc);
            }
            var result = FindElements(query);
            Type genericArguments = typeof(TResult).GetGenericArguments()[0];
            MethodInfo method = base.GetType().GetMethod("ApplySearchMethods", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo methodInfo = method.MakeGenericMethod(new Type[] { genericArguments });
            MethodInfo method1 = base.GetType().GetMethod("ApplyScalarMethods", BindingFlags.Instance | BindingFlags.NonPublic);
            Type[] typeArray = new Type[] { typeof(TResult), genericArguments };
            MethodInfo methodInfo1 = method1.MakeGenericMethod(typeArray);
            object[] objArray = new object[] { query, result };
            object obj = methodInfo.Invoke(this, objArray);
            object[] objArray1 = new object[] { query, obj, result };
            return (TResult)methodInfo1.Invoke(this, objArray1);
        }
        public SearchQueryResult FindElements(AzureCompositeQuery query)
        {

            //    {
            var result = FindElementsAsync<SearchQueryResult>(query);
            result.Wait();
            if (result.Result.IsSuccess)
            {
                SelectMethod item;
                List<SelectMethod> list =
                    query.Methods.Where(m => m.MethodType == QueryMethodType.Select)
                        .Select(s => (SelectMethod)s)
                        .ToList<SelectMethod>();
                if (list.Count<SelectMethod>() == 1)
                {
                    item = list[0];
                }
                else
                {
                    item = null;
                }
                AzureSearchResults<SearchQueryResult> results = new AzureSearchResults<SearchQueryResult>(this.context, result.Result.Body,
                    item, query.ExecutionContexts, null);
                return results.searchResults;
            }
            else
            {
                return null;
            }
        }
        public override IEnumerable<TElement> FindElements<TElement>(AzureCompositeQuery query)
        {
            
            //    {
            var result = FindElementsAsync<TElement>(query);
            result.Wait();
            if (result.Result.IsSuccess)
            {
                SelectMethod item;
                List<SelectMethod> list =
                    query.Methods.Where(m => m.MethodType == QueryMethodType.Select)
                        .Select(s => (SelectMethod) s)
                        .ToList<SelectMethod>();
                if (list.Count<SelectMethod>() == 1)
                {
                    item = list[0];
                }
                else
                {
                    item = null;
                }
                AzureSearchResults<TElement> results = new AzureSearchResults<TElement>(this.context, result.Result.Body,
                    item, query.ExecutionContexts, null);
                return results.GetSearchResults();


            }
            else
            {
                return null;
            }          
        }
        private async Task<IApiResponse<SearchQueryResult>> FindElementsAsync<TElement>(AzureCompositeQuery query)
        {
            SearchLog.Log.Debug("Executing query: " + query.Expression);
            SelectMethod item;
            var index = this.context.Index as AzureSearchIndex;
            Assert.IsNotNull(index, "context.Index is not an instance of XmlIndex");
            return await index.QueryClient.SearchAsync(index.Name, query);

            
        } 

        //public LinqToAzureIndex(AzureSearchContext context) : this(context, null)
        //{
        //}
        public void InitializeLinqToAzureIndex(AzureIndexparameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            this.queryMapper = new AzureQueryMapper(parameters);
            this.parameters = parameters;
        }
        public LinqToAzureIndex(AzureSearchContext context, IExecutionContext executionContext): this(context, new IExecutionContext[] { executionContext })
        {
        }
        public LinqToAzureIndex(AzureSearchContext context, IExecutionContext[] executionContext)
        {
            Assert.ArgumentNotNull(context, "context");
            AzureFieldTranslator trans = (AzureFieldTranslator )context.Index.FieldNameTranslator ??
                                         context.Index.Locator.GetInstance<AzureFieldTranslator>();
            this.fieldnameTranslator = trans;
            this._queryOptimizer= new AzureQueryOptimizer();
            InitializeLinqToAzureIndex(new AzureIndexparameters(context.Index.Configuration.IndexFieldStorageValueFormatter, context.Index.Configuration.VirtualFieldProcessors, context.Index.FieldNameTranslator, executionContext));
            this.context = context;
            //this.settings = context.Index.Locator.GetInstance<ISettings>();
            this.contentSearchSettings = (AzureIndexConfiguration) context.Index.Configuration;
            this.pipeline = context.Index.Locator.GetInstance<ICorePipeline>();
            CultureExecutionContext cultureExecutionContext = ((IEnumerable<IExecutionContext>)Parameters.ExecutionContexts).FirstOrDefault<IExecutionContext>((IExecutionContext c) => c is CultureExecutionContext) as CultureExecutionContext;
            //CultureInfo cultureInfo = (cultureExecutionContext == null ? CultureInfo.GetCultureInfo(this.settings.DefaultLanguage()) : cultureExecutionContext.Culture);
            //this.cultureCode = cultureInfo.TwoLetterISOLanguageName;
            //((AzureFieldTranslator)Parameters.FieldNameTranslator).AddCultureContext(cultureInfo);
        }

        private readonly AzureQueryMapper _mapper;

        protected override QueryMapper<AzureCompositeQuery> QueryMapper
        {
            get { return queryMapper; }
        }

        private readonly AzureQueryOptimizer _queryOptimizer;

        protected override IQueryOptimizer QueryOptimizer
        {
            get { return _queryOptimizer; }
        }

        

        protected override FieldNameTranslator FieldNameTranslator
        {
            get { return fieldnameTranslator; }
        }

        private readonly AzureSearchContext _context;
        private readonly AzureIndexConfiguration _configuration;

        protected override IIndexValueFormatter ValueFormatter
        {
            get { return this.parameters.ValueFormatter; }
        }
    }
}
