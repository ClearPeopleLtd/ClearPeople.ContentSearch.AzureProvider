using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;

namespace AzureProvider
{
    public class AzureSearchConfiguration : ContentSearchConfiguration
    {        
        new public virtual void AddIndex(ISearchIndex index)
        {
            Indexes[index.Name] = index;     
            index.Configuration = new ProviderIndexConfiguration();
            index.Configuration.DocumentOptions = new DocumentBuilderOptions();
            index.Configuration.IndexAllFields = true;
            
            index.Initialize();            
        }
    }
}
