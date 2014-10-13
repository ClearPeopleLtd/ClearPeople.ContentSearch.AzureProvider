using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Diagnostics;

namespace AzureProvider
{
    public class AzureCrawler<T> : HierarchicalDataCrawler<T> where T :IHierarchicalIndexable
    {
        public override void Initialize(ISearchIndex index)
        {
            base.Initialize(index);            
        }
        protected override T GetIndexable(IIndexableId indexableId, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<T> GetIndexableChildren(T parent)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IIndexableId> GetIndexableChildrenIds(T parent)
        {
            throw new NotImplementedException();
        }

        public override T GetIndexableRoot()
        {
            throw new NotImplementedException();
        }

        protected override T GetIndexable(IIndexableUniqueId indexableUniqueId)
        {
            throw new NotImplementedException();
        }

        protected override T GetIndexableAndCheckDeletes(IIndexableUniqueId indexableUniqueId)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IIndexableUniqueId> GetIndexablesToUpdateOnDelete(IIndexableUniqueId indexableUniqueId)
        {
            throw new NotImplementedException();
        }

        protected override bool IndexUpdateNeedDelete(T indexable)
        {
            throw new NotImplementedException();
        }
    }
}
