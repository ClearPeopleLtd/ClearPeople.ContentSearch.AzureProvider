using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedDog.Search.Model;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.WebControls;

namespace AzureProvider
{
    public class DefaulAzureDocumentTypeMapper : DefaultDocumentMapper<SearchQueryRecord>
    {
        public DefaulAzureDocumentTypeMapper()
        {
        }

        protected override IEnumerable<string> GetDocumentFieldNames(SearchQueryRecord document)
        {
            return null;
        }

        [Obsolete]
        protected override void ReadDocumentFields<TElement>(SearchQueryRecord document, IEnumerable<string> fieldNames, DocumentTypeMapInfo documentTypeMapInfo, IEnumerable<IFieldQueryTranslator> virtualFieldProcessors, TElement result)
        {
        }

        protected override IDictionary<string, object> ReadDocumentFields(SearchQueryRecord document, IEnumerable<string> fieldNames, IEnumerable<IFieldQueryTranslator> virtualFieldProcessors)
        {
            Assert.ArgumentNotNull(document, "document");
            IDictionary<string, object> strs = new Dictionary<string, object>();
            strs = document.Properties;
            if (virtualFieldProcessors != null)
            {
                strs = virtualFieldProcessors.Aggregate<IFieldQueryTranslator, IDictionary<string, object>>(strs, (IDictionary<string, object> current, IFieldQueryTranslator processor) => processor.TranslateFieldResult(current, this.index.FieldNameTranslator));
            }
            return strs;
        }
    }
}
