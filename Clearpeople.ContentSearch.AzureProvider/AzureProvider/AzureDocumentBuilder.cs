using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;

namespace AzureProvider
{
    public class AzureDocumentBuilder:AbstractDocumentBuilder<Dictionary<string, object>> 
    {
        public AzureDocumentBuilder(IIndexable indexable, IProviderUpdateContext context)
            : base(indexable, context)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);            
            this.Document.Add("_id",item.ID.ToShortID().ToString());
            this.Document.Add("_name", item.Name);
            this.Document.Add("_path", item.Paths.Path);
             
        }
        private bool ShouldAddField(IIndexableDataField field, AzureFieldConfiguration config)
        {
            if (!base.Index.Configuration.IndexAllFields)
            {
                if (config == null || (config.FieldName == null && config.FieldTypeName == null))
                {
                    return false;
                }
                if (field.Value == null)
                {
                    return false;
                }
            }
            return true;
        }
        public override void AddField(IIndexableDataField field)
        {
            var fieldConfig = base.Index.Configuration.FieldMap.GetFieldConfiguration(field) as AzureFieldConfiguration;
            if (fieldConfig == null || !ShouldAddField(field, fieldConfig))
            {
                return;
            }            
            var value = field.Value.ToString();            
            this.Document.Add(field.Name,value);
        }

        public override void AddBoost()
        {
            
        }

        public override void AddComputedIndexFields()
        {
            
        }

        public override void AddField(string fieldName, object fieldValue, bool append = false)
        {
            
        }

        public override void AddProviderCustomFields()
        {
            
        }
    }
}
