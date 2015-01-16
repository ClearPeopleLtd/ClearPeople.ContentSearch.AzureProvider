using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedDog.Search;
using RedDog.Search.Http;
using RedDog.Search.Model;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.ContentSearch.Events;
using Sitecore.ContentSearch.Maintenance.Strategies;
using Sitecore.Eventing;
using Sitecore.Events;
using Sitecore.StringExtensions;

namespace AzureProvider
{
    public class AzureSearchIndex : AbstractSearchIndex ,ISearchIndex
    {
        public IndexManagementClient ManagementClient
        {
            get { return _managementClient; }
        }
        private IndexManagementClient _managementClient;
        public IndexQueryClient QueryClient
        {
            get { return _queryClient; }
        }

        private AzureFieldTranslator fieldNameTranslator;
        private IndexQueryClient _queryClient;
        private AzureSearchIndexSummary summary;
        private string key = "CDAC986F23CE5EFBD577044A040B0850";
        private string servicename = "cpsearch";
        private AzureIndexSchema _schema;
        
        public AzureSearchIndex(string name)
        {
            this._name = name;
            //Crawlers = new List<IProviderCrawler>();
            this.Strategies = new List<IIndexUpdateStrategy>();
            var connection = ApiConnection.Create(servicename, key);
            _managementClient = new IndexManagementClient(connection);
            _queryClient = new IndexQueryClient(connection);
            this._schema = new AzureIndexSchema(this);
            
            
        }
        public void AddCrawler(IProviderCrawler crawler)
        {         
            crawler.Initialize(this);
            this.Crawlers.Add(crawler);
        }

        public override void AddStrategy(Sitecore.ContentSearch.Maintenance.Strategies.IIndexUpdateStrategy strategy)
        {
            strategy.Initialize(this);
            Strategies.Add(strategy);
        }


        public override ProviderIndexConfiguration Configuration
        {
            get;
            set;
        }
        
        public List<IIndexUpdateStrategy> Strategies { get; private set; }

        public void ExcludeTemplate(string templateid)
        {

            if (Configuration.DocumentOptions.ExcludedFields != null && !string.IsNullOrEmpty(templateid) && !Configuration.DocumentOptions.ExcludedFields.Contains(templateid))
            {
                Configuration.DocumentOptions.ExcludedFields.Add(templateid);
            }
        }

        public override IProviderDeleteContext CreateDeleteContext()
        {
            return null;
        }

        public override IProviderSearchContext CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions options = Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck)
        {
            return new AzureSearchContext(this, options);
        }

        public override IProviderUpdateContext CreateUpdateContext()
        {
            return new AzureUpdateContext(this);
        }

        public override void Delete(IIndexableUniqueId indexableUniqueId, IndexingOptions indexingOptions)
        {
            
        }

        public override void Delete(IIndexableUniqueId indexableUniqueId)
        {
            
        }

        public override void Delete(IIndexableId indexableId, IndexingOptions indexingOptions)
        {
            
        }

        public override void Delete(IIndexableId indexableId)
        {
            using (var context = this.CreateUpdateContext())
            {
                foreach (var crawler in this.Crawlers)
                {
                    crawler.Delete(context, indexableId);
                }
                context.Commit();
            }
        }

        public override AbstractFieldNameTranslator FieldNameTranslator
        {
            get
            {
                return this.fieldNameTranslator;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.fieldNameTranslator = value;
            }
        }

        public IndexingState IndexingState
        {
            get { return IndexingState.Stopped; }
        }

        public override void Initialize()
        {
            var exists = Exists();
            if (!(exists.IsCompleted && exists.Result.IsSuccess))
                CreateAsync();
        }

        //public Sitecore.ContentSearch.Abstractions.IObjectLocator Locator { get; set; }

        private string _name;
        public override string Name
        {
            get { return this._name; }
        }

        public override IIndexOperations Operations { get{return new AzureIndexOperations();}  }

        public void PauseIndexing()
        {
            
        }

        public override Sitecore.ContentSearch.Maintenance.IIndexPropertyStore PropertyStore { get; set; }

        public override void Rebuild(IndexingOptions indexingOptions)
        {
            
        }

        public override void Rebuild()
        {
            Event.RaiseEvent("indexing:start", new object[] { this.Name, true });
            var event2 = new IndexingStartedEvent
            {
                IndexName = this.Name,
                FullRebuild = true
            };
            EventManager.QueueEvent<IndexingStartedEvent>(event2);
            this.Reset();
            this.Create();
            this.DoRebuild();
            Event.RaiseEvent("indexing:end", new object[] { this.Name, true });
            var event3 = new IndexingFinishedEvent
            {
                IndexName = this.Name,
                FullRebuild = true
            };
            EventManager.QueueEvent<IndexingFinishedEvent>(event3);
        }
        protected virtual void DoRebuild()
        {
            using (var context = this.CreateUpdateContext())
            {
                foreach (var crawler in this.Crawlers)
                {
                    crawler.RebuildFromRoot(context,IndexingOptions.Default);
                }
                context.Optimize();
                context.Commit();
            }
        }

        public override Task RebuildAsync(IndexingOptions indexingOptions, System.Threading.CancellationToken cancellationToken)
        {
            return null;
        }

        public override void Refresh(IIndexable indexableStartingPoint, IndexingOptions indexingOptions)
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

        public override void Refresh(IIndexable indexableStartingPoint)
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

        public override Task RefreshAsync(IIndexable indexableStartingPoint, IndexingOptions indexingOptions, System.Threading.CancellationToken cancellationToken)
        {
            return null;
        }

        public async void CreateAsync()
        {
            var result2 = await ManagementClient.CreateIndexAsync(new Index(Name)
                        .WithStringField("_id", opt =>
                            opt.IsKey().IsRetrievable())
                        .WithStringField("_name", opt =>
                            opt.IsRetrievable().IsSearchable())
                        .WithStringField("_path", opt =>
                            opt.IsRetrievable().IsSearchable())
                        );
        }
        public void Create()
        {
            var indexdefinition = new Index(Name);            

            ManagementClient.CreateIndexAsync(new Index(Name)
                        .WithStringField("_id", opt =>
                            opt.IsKey().IsRetrievable())
                        .WithStringField("_name", opt =>
                            opt.IsRetrievable().IsSearchable())
                        .WithStringField("_path", opt =>
                            opt.IsRetrievable().IsSearchable())
                        );
        }

        private void BuildSchema()
        {
            
        }

        private async Task<IApiResponse<Index>> Exists()
        {
            var result = await ManagementClient.GetIndexAsync(this.Name);
            return result;
        }
        public override void Reset()
        {
            var result = _managementClient.DeleteIndexAsync(this.Name);            
        }

        public  void ResumeIndexing()
        {
            
        }

        public override ISearchIndexSchema Schema
        {
            get { return null; }
        }



        public override ISearchIndexSummary Summary { get { return this.summary; } }

        

        public override void Update(IEnumerable<IIndexableUniqueId> indexableUniqueIds, IndexingOptions indexingOptions)
        {
            
        }

        public override void Update(IEnumerable<IIndexableUniqueId> indexableUniqueIds)
        {
            
        }

        public override void Update(IIndexableUniqueId indexableUniqueId, IndexingOptions indexingOptions)
        {
            
        }

        public override void Update(IIndexableUniqueId indexableUniqueId)
        {
            using (var context = this.CreateUpdateContext())
            {
                foreach (var crawler in this.Crawlers)
                {
                    crawler.Update(context, indexableUniqueId);
                }
                context.Commit();
            }
        }
    }
}
