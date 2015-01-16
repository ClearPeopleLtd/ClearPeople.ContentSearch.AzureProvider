using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RedDog.Search.Model;
using Sitecore.ContentSearch;

namespace AzureProvider
{
    public class AzureFieldConfiguration : AbstractSearchFieldConfiguration
    {
        public string FieldNameFormat { get; set; }
        public AzureFieldConfiguration()
        {
        }

        public AzureFieldConfiguration(string fieldName, string fieldID, string fieldTypeName, IDictionary<string, string> attributes, XmlNode configNode)
        {
            this.FieldName = fieldName;
            this.FieldTypeName = fieldTypeName;
            this.FieldID = fieldID;
            Attributes = attributes;
            AzureField = new IndexField();
            Initialize(attributes);
        }

        [Obsolete("Use AbstractSearchFieldConfiguration(string, string, string, IDictionary<string, string>, XmlNode) constructor instead")]
        public AzureFieldConfiguration(string fieldName, string fieldTypeName, IDictionary<string, string> attributes, XmlNode configNode)
      : this(fieldName, (string) null, fieldTypeName, attributes, configNode)
    {
    }
        public AzureFieldConfiguration(AzureFieldConfiguration configuration)
            : this(configuration.FieldTypeName, configuration.FieldID, configuration.Attributes, null)
        {
        }

        private void Initialize(IDictionary<string, string> attributes)
        {
            FieldNameFormat = "{0}";
            foreach (var attribute in attributes)
            {
                bool tmpbool;
                switch (attribute.Key.ToLowerInvariant())
                {
                    case "type":
                        AzureField.Type = attribute.Value;
                        break;
                    case "searchable":
                        if (Boolean.TryParse(attribute.Value, out tmpbool))
                            AzureField.Searchable = tmpbool;
                        break;
                    case "filterable":
                        if (Boolean.TryParse(attribute.Value, out tmpbool))
                            AzureField.Filterable = tmpbool;
                        break;
                    case "sortable":
                        if (Boolean.TryParse(attribute.Value, out tmpbool))
                            AzureField.Sortable = tmpbool;
                        break;
                    case "facetable":
                        if (Boolean.TryParse(attribute.Value, out tmpbool))
                            AzureField.Facetable = tmpbool;
                        break;
                    case "Suggestions":
                        if (Boolean.TryParse(attribute.Value, out tmpbool))
                            AzureField.Suggestions = tmpbool;
                        break;
                    case "key":
                        if (Boolean.TryParse(attribute.Value, out tmpbool))
                            AzureField.Key = tmpbool;
                        break;
                    case "retrievable":
                        if (Boolean.TryParse(attribute.Value, out tmpbool))
                            AzureField.Retrievable = tmpbool;
                        break;
                    case "analyzer":
                        AzureField.Analyzer = attribute.Value;
                        break;
                    case "fieldnameformat":
                        FieldNameFormat = attribute.Value;
                        break;
                }
            }
        }        
        public IndexField AzureField { get; set; }
       public string FormatFieldName(string fieldName, ISearchIndexSchema schema, string cultureCode)
        {
            return this.FormatFieldName(fieldName, schema, cultureCode);
        }
                

        //public string FormatFieldName(string fieldName, ISearchIndexSchema schema, string cultureCode,
        //    string defaultCulture)
        //{
        //    fieldName = fieldName.Replace(" ", "_").ToLowerInvariant();
        //    string empty = string.Empty;
        //    string str = (!string.IsNullOrEmpty(cultureCode) ? cultureCode : defaultCulture);
        //    if (str == "iv")
        //    {
        //        str = defaultCulture;
        //    }
        //    if (!string.IsNullOrEmpty(cultureCode) && !string.IsNullOrEmpty(this.CultureFormat) &&
        //        !defaultCulture.StartsWith(str))
        //    {
        //        empty = this.CultureFormat;
        //    }
        //    string str1 = string.Concat(fieldName, string.Format(empty, string.Empty, str));
        //    if (schema.AllFieldNames.Contains(str1))
        //    {
        //        return str1;
        //    }
        //    return string.Format(string.Concat(this.FieldNameFormat, empty), fieldName, str);
        //}
    }
}
