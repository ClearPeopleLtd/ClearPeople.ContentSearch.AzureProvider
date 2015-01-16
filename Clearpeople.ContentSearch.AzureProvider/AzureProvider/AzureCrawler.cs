using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Diagnostics;

namespace AzureProvider
{
    public class AzureCrawler : AbstractProviderCrawler
    {
        public override void Initialize(ISearchIndex index)
        {
            base.Initialize(index);
            var msg = string.Format("[Index={0}] Initializing AzureSearchCrawler. DB:{1} / Root:{2}", index.Name, base.Database, base.Root);
            CrawlingLog.Log.Info(msg, null);
        }
        public void Delete(IIndexableId id, IProviderUpdateContext context)
        {
            CrawlingLog.Log.Debug("Deleting ID " + id.Value.ToString());
            context.Delete(id);
        }

        //protected override T GetIndexable(IIndexableId indexableId, System.Globalization.CultureInfo culture)
        //{
        //    return default(T);
        //}

        //protected override IEnumerable<T> GetIndexableChildren(T parent)
        //{
        //    return (IEnumerable<T>) default(T);
        //}

        //protected override IEnumerable<IIndexableId> GetIndexableChildrenIds(T parent)
        //{
        //    return null;
        //}

        //public override T GetIndexableRoot()
        //{
        //    return default(T); ;
        //}

        //protected override T GetIndexable(IIndexableUniqueId indexableUniqueId)
        //{
        //    return default(T); ;
        //}

        //protected override T GetIndexableAndCheckDeletes(IIndexableUniqueId indexableUniqueId)
        //{
        //    return default(T); ;
        //}

        //protected override IEnumerable<IIndexableUniqueId> GetIndexablesToUpdateOnDelete(IIndexableUniqueId indexableUniqueId)
        //{
        //    return null;
        //}

        //protected override bool IndexUpdateNeedDelete(T indexable)
        //{
        //    return true;
        //}
    }
}
