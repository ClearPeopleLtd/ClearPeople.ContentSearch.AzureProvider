using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sitecore.ContentSearch;
using Sitecore.Data.Fields;

namespace AzureProvider
{
    class AzureIndexConfiguration:ProviderIndexConfiguration
    {
        public AzureIndexConfiguration()
        {
            base.DocumentOptions = new DocumentBuilderOptions();
            
        }
        public IIndexDocumentPropertyMapper<Dictionary<string, object>> IndexDocumentPropertyMapper
        {
            get;
            set;
        }
        public void AddCustomField(XmlNode node)
        {
           
        }

    }
}
