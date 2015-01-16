using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Pipelines.IndexingFilters;
using Sitecore.Data.Items;
using Sitecore.Events;
using IndexOperation = RedDog.Search.Model.IndexOperation;

namespace AzureProvider
{
    public class AzureIndexOperations : IIndexOperations
    {
        public void Add(IIndexable indexable, IProviderUpdateContext context, ProviderIndexConfiguration indexConfiguration)
        {
            var doc = GetDocument(indexable,context);
            if (doc == null)
            {
                Event.RaiseEvent("indexing:excludedfromindex", new object[] { context.Index.Name, indexable.Id });
                return;
            }
            context.AddDocument(doc, new DefaultDocumentMapperFactoryRuleExecutionContext());
        }

        public void Delete(IIndexableUniqueId indexableUniqueId, IProviderUpdateContext context)
        {
            
        }

        public void Delete(IIndexableId id, IProviderUpdateContext context)
        {
            context.Delete(id);
        }

        public void Delete(IIndexable indexable, IProviderUpdateContext context)
        {
            
        }

        public void Update(IIndexable indexable, IProviderUpdateContext context, ProviderIndexConfiguration indexConfiguration)
        {
            var doc = GetDocument(indexable, context);
            if (doc == null)
            {
                Event.RaiseEvent("indexing:excludedfromindex", new object[] { context.Index.Name, indexable.Id });
                return;
            }
            context.UpdateDocument(doc, null,new DefaultDocumentMapperFactoryRuleExecutionContext());
        }
        protected virtual Dictionary<string, object> GetDocument(IIndexable indexable, IProviderUpdateContext context)
        {
            if (InboundIndexFilterPipeline.Run(new InboundIndexFilterArgs(indexable)))
            {
                return null;
            }
            var builder = new AzureDocumentBuilder(indexable, context);
            builder.AddItemFields();
            return builder.Document;            
        }
        
    }
}
