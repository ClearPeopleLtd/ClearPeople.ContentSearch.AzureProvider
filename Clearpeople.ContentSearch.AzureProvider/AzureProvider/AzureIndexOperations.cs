using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.Data.Items;
using IndexOperation = RedDog.Search.Model.IndexOperation;

namespace AzureProvider
{
    public class AzureIndexOperations : IIndexOperations
    {
        public void Add(IIndexable indexable, IProviderUpdateContext context, ProviderIndexConfiguration indexConfiguration)
        {
            
        }

        public void Delete(IIndexableUniqueId indexableUniqueId, IProviderUpdateContext context)
        {
            
        }

        public void Delete(IIndexableId id, IProviderUpdateContext context)
        {
            
        }

        public void Delete(IIndexable indexable, IProviderUpdateContext context)
        {
            
        }

        public void Update(IIndexable indexable, IProviderUpdateContext context, ProviderIndexConfiguration indexConfiguration)
        {
            var doc = GetDocument(indexable);
            context.UpdateDocument(doc, null,new DefaultDocumentMapperFactoryRuleExecutionContext());
        }
        protected virtual Dictionary<string, object> GetDocument(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);
            var doc = new Dictionary<string, object>();
            doc.Add("id",item.ID.ToShortID().ToString());
            doc.Add("name", item.Name);
            doc.Add("path", item.Paths.Path);
            return doc;
        }
        
    }
}
