using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;

namespace AzureProvider
{
    public class TemplateResolver
    {
        [IndexField("_name")]
        public string Name
        {
            get;
            set;
        }

        [IndexField("_templatename")]
        public string TemplateName
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public TemplateResolver()
        {
        }
    }
}
