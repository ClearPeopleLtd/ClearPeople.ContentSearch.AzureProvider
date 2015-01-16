using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedDog.Search.Model;
using Sitecore.ContentSearch;

namespace AzureProvider
{
    public class AzureIndexSchema:ISearchIndexSchema
    {
        private AzureSearchIndex _index;
        public ICollection<string> AllFieldNames
        {
            get { return null; }
        }
        public ICollection<IndexField> AllFields { get; set; }

        public AzureIndexSchema(AzureSearchIndex index)
        {
            this._index = index;
        }

        private void CheckAndAddField(IIndexable indexable, IIndexableDataField field)
        {
            //_index.Configuration.DocumentOptions.
            //var fieldConfig = base.Index.Configuration.FieldMap.GetFieldConfiguration(field) as AzureFieldConfiguration;
            //if (fieldConfig == null || !ShouldAddField(field, fieldConfig))
            //{
            //    return;
            //} 
            //string name = field.Name;
            //if (this.IsTemplate && this.Options.HasExcludedTemplateFields && (this.Options.ExcludedTemplateFields.Contains(name) || this.Options.ExcludedTemplateFields.Contains(field.Id.ToString())) || this.IsMedia && this.Options.HasExcludedMediaFields && this.Options.ExcludedMediaFields.Contains(field.Name) || this.Options.ExcludedFields.Contains(field.Id.ToString()))
            //    return;
            //if (this.Options.ExcludedFields.Contains(name))
            //    return;
            //try
            //{
            //    if (this.Options.IndexAllFields)
            //    {
            //        this.AddField(field);
            //    }
            //    else
            //    {
            //        if (!this.Options.IncludedFields.Contains(name) && !this.Options.IncludedFields.Contains(field.Id.ToString()))
            //            return;
            //        this.AddField(field);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    CrawlingLog.Log.Warn(string.Format("Could not add field {1} : {2} for indexable {0}", (object)indexable.UniqueId, field.Id, (object)field.Name), ex);
            //    if (!this.Settings.StopOnCrawlFieldError())
            //        return;
            //    throw;
            //}
        }
    }
}
