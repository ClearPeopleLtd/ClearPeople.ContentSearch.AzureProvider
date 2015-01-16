using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LinqToAzure;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Abstractions;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Indexing;
using Sitecore.ContentSearch.Linq.Parsing;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Diagnostics;

namespace AzureProvider
{
    public class LinqToAzureIndex<TItem> : Index<TItem, AzureCompositeQuery>
    {
         private AzureIndexparameters parameters;
         private QueryMapper<AzureCompositeQuery> queryMapper;
        private readonly AzureSearchContext context;

        private readonly string cultureCode;

        //private readonly ISettings settings;

        private readonly IContentSearchConfigurationSettings contentSearchSettings;

        private readonly ICorePipeline pipeline;
        public AzureIndexparameters Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public override TResult Execute<TResult>(AzureCompositeQuery query)
        {
            return default(TResult);
        }

        public override IEnumerable<TElement> FindElements<TElement>(AzureCompositeQuery query)
        {
            SearchLog.Log.Debug("Executing query: " + query.Expression);
            var index = _context.Index as AzureSearchIndex;
            Assert.IsNotNull(index, "context.Index is not an instance of XmlIndex");
            //var doc = new XmlDocument();
            //doc.Load(index.IndexFilePath);
            //var nodes = doc.SelectNodes(query.Expression);
            //if (nodes != null)
            //{
            //    foreach (XmlNode node in nodes)
            //    {
            yield return default(TElement);
            //  }
            //}
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
            InitializeLinqToAzureIndex(new AzureIndexparameters(context.Index.Configuration.IndexFieldStorageValueFormatter, context.Index.Configuration.VirtualFieldProcessors, context.Index.FieldNameTranslator, executionContext));
            this.context = context;
            //this.settings = context.Index.Locator.GetInstance<ISettings>();
            this.contentSearchSettings = context.Index.Locator.GetInstance<IContentSearchConfigurationSettings>();
            this.pipeline = context.Index.Locator.GetInstance<ICorePipeline>();
            CultureExecutionContext cultureExecutionContext = ((IEnumerable<IExecutionContext>)Parameters.ExecutionContexts).FirstOrDefault<IExecutionContext>((IExecutionContext c) => c is CultureExecutionContext) as CultureExecutionContext;
            //CultureInfo cultureInfo = (cultureExecutionContext == null ? CultureInfo.GetCultureInfo(this.settings.DefaultLanguage()) : cultureExecutionContext.Culture);
            //this.cultureCode = cultureInfo.TwoLetterISOLanguageName;
            //((AzureFieldTranslator)Parameters.FieldNameTranslator).AddCultureContext(cultureInfo);
        }

        private readonly AzureQueryMapper _mapper;

        protected override QueryMapper<AzureCompositeQuery> QueryMapper
        {
            get { return _mapper; }
        }

        private readonly AzureQueryOptimizer _queryOptimizer;

        protected override IQueryOptimizer QueryOptimizer
        {
            get { return _queryOptimizer; }
        }

        private readonly FieldNameTranslator _fieldNameTranslator;

        protected override FieldNameTranslator FieldNameTranslator
        {
            get { return _fieldNameTranslator; }
        }

        private readonly AzureSearchContext _context;
        private readonly AzureIndexConfiguration _configuration;

        protected override IIndexValueFormatter ValueFormatter
        {
            get { return this.parameters.ValueFormatter; }
        }
    }
}
