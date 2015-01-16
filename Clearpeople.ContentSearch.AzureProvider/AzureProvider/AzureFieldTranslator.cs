using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Security;

namespace AzureProvider
{
   public class AzureFieldTranslator:AbstractFieldNameTranslator
    {
       //private readonly AzureFieldMap fieldMap;

       // private readonly AzureIndexSchema schema;

       // private readonly AzureSearchIndex index;

       // private readonly string defaultCultureCode;

       // private string currentCultureCode;

       // private readonly Dictionary<string, IEnumerable<string>> typeFieldMap = new Dictionary<string, IEnumerable<string>>();

        //private ISettings settings;


        //public AzureFieldTranslator(AzureSearchIndex azureSearchIndex)
        //{
        //    this.index = azureSearchIndex;
        //    this.fieldMap = azureSearchIndex.Configuration.FieldMap as AzureFieldMap;
        //    this.schema = azureSearchIndex.Schema as AzureIndexSchema;
        //    //this.settings = azureSearchIndex.Locator.GetInstance<ISettings>();
        //    //this.defaultCultureCode = this.settings.DefaultLanguage();
        //    this.currentCultureCode = defaultCultureCode = this.defaultCultureCode;
        //}

        public override string GetIndexFieldName(MemberInfo member)
        {
            var attribute = base.GetMemberAttribute(member);
            if (attribute != null)
            {
                return this.GetIndexFieldName(attribute.GetIndexFieldName(member.Name));
            }
            return this.GetIndexFieldName(member.Name);
        }
        public override Dictionary<string, List<string>> MapDocumentFieldsToType(Type type, MappingTargetType target, IEnumerable<string> documentFieldNames)
        {
            return MapDocumentFieldsToType(type,documentFieldNames);
        }
        public Dictionary<string, List<string>> MapDocumentFieldsToType(Type type, IEnumerable<string> documentFieldNames)
        {
            var map = documentFieldNames.ToDictionary(name => name, name => this.GetTypeFieldNames(name).ToList());
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                var attribute = this.GetMemberAttribute(property);
                if (attribute == null)
                {
                    continue;
                }
                var indexFieldName = this.GetIndexFieldName(attribute.GetIndexFieldName(property.Name));
                if (map.ContainsKey(indexFieldName))
                {
                    map[indexFieldName].Add(attribute.GetTypeFieldName(property.Name));
                }
            }
            return map;
        }
        public override IEnumerable<string> GetTypeFieldNames(string fieldName)
        {
            yield return fieldName;
        }

       // public void AddCultureContext(CultureInfo culture)
       // {
       //     if (culture != null)
       //     {
       //         this.currentCultureCode = culture.TwoLetterISOLanguageName;
       //     }
       //     if (this.currentCultureCode == "iv")
       //     {
       //         this.currentCultureCode = this.defaultCultureCode;
       //     }
       // }

       // public override string GetIndexFieldName(MemberInfo member)
       // {
       //     return this.GetIndexFieldName(member, null);
       // }

       // public string GetIndexFieldName(MemberInfo member, CultureInfo culture)
       // {
       //     string name = member.Name;
       //     IIndexFieldNameFormatterAttribute indexFieldNameFormatterAttribute = base.GetIndexFieldNameFormatterAttribute(member);
       //     if (indexFieldNameFormatterAttribute != null)
       //     {
       //         name = indexFieldNameFormatterAttribute.GetIndexFieldName(member.Name);
       //     }
       //     Type propertyType = ((PropertyInfo)member).PropertyType;
       //     return this.ProcessFieldName(name, propertyType, culture);
       // }

       // public override string GetIndexFieldName(string fieldName, Type returnType)
       // {
       //     return this.GetIndexFieldName(fieldName, returnType);
       // }

       // public string GetIndexFieldName(string fieldName, Type returnType, CultureInfo culture)
       // {
       //     return this.ProcessFieldName(fieldName, returnType, culture);
       // }

       // public string GetIndexFieldName(string fieldName, string returnType)
       // {
       //     return this.GetIndexFieldName(fieldName, returnType);
       // }

       // public string GetIndexFieldName(string fieldName, string returnType, CultureInfo culture)
       // {
       //     return this.ProcessFieldName(fieldName, null, culture, returnType, false);
       // }

       // public override string GetIndexFieldName(string fieldName)
       // {
       //     return this.GetIndexFieldName(fieldName);
       // }

       // public string GetIndexFieldName(string fieldName, CultureInfo culture)
       // {
       //     return this.ProcessFieldName(fieldName, null, culture, null, false);
       // }

       // public string GetIndexFieldName(string fieldName, bool aggressiveResolver)
       // {
       //     return this.GetIndexFieldName(fieldName, aggressiveResolver);
       // }

       // public string GetIndexFieldName(string fieldName, bool aggressiveResolver, CultureInfo culture)
       // {
       //     return this.ProcessFieldName(fieldName, null, culture, null, aggressiveResolver);
       // }

       // public override IEnumerable<string> GetTypeFieldNames(string fieldName)
       // {
       //     if (this.typeFieldMap.ContainsKey(fieldName))
       //     {
       //         return this.typeFieldMap[fieldName];
       //     }
       //     List<string> strs = new List<string>();
       //     string str = this.StripKnownExtensions(fieldName);
       //     if (str == fieldName)
       //     {
       //         fieldName = str;
       //         if (fieldName.StartsWith("_", StringComparison.Ordinal))
       //         {
       //             strs.Add(fieldName);
       //         }
       //         else
       //         {
       //             strs.Add(fieldName.Replace("_", " ").Trim());
       //         }
       //     }
       //     else
       //     {
       //         strs.Add(str);
       //     }
       //     if (!this.typeFieldMap.ContainsKey(fieldName))
       //     {
       //         this.typeFieldMap[fieldName] = strs;
       //     }
       //     return this.typeFieldMap[fieldName];
       // }

       // public bool HasCulture(string fieldName)
       // {
       //     return false;
       //     //return (
       //     //    from culture in this.schema.AllCultures
       //     //    where fieldName.Length > culture.Length
       //     //    select culture).Any<string>((string i) => fieldName.EndsWith(i, StringComparison.Ordinal));
       // }

       // public override Dictionary<string, List<string>> MapDocumentFieldsToType(Type type, MappingTargetType target, IEnumerable<string> documentFieldNames)
       // {
       //     if (target == MappingTargetType.Indexer)
       //     {
       //         return this.MapDocumentFieldsToTypeIndexer(type, documentFieldNames);
       //     }
       //     if (target == MappingTargetType.Properties)
       //     {
       //         return this.MapDocumentFieldsToTypeProperties(type, documentFieldNames);
       //     }
       //     if (target != MappingTargetType.Anything)
       //     {
       //         throw new ArgumentException(string.Concat("Invalid mapping target type: ", target), "target");
       //     }
       //     Dictionary<string, List<string>> strs3 = new Dictionary<string, List<string>>();
       //     Action<Dictionary<string, List<string>>, Dictionary<string, List<string>>> action = (Dictionary<string, List<string>> result, Dictionary<string, List<string>> map) => {
       //         List<string> strs;
       //         foreach (KeyValuePair<string, List<string>> keyValuePair in map)
       //         {
       //             if (keyValuePair.Value == null || keyValuePair.Value.Count == 0)
       //             {
       //                 continue;
       //             }
       //             if (!result.TryGetValue(keyValuePair.Key, out strs))
       //             {
       //                 string key = keyValuePair.Key;
       //                 List<string> strs1 = new List<string>();
       //                 List<string> strs2 = strs1;
       //                 strs = strs1;
       //                 result[key] = strs2;
       //             }
       //             strs.AddRange(
       //                 from l in keyValuePair.Value
       //                 where !strs.Contains(l)
       //                 select l);
       //         }
       //     };
       //     action(strs3, this.MapDocumentFieldsToTypeIndexer(type, documentFieldNames));
       //     action(strs3, this.MapDocumentFieldsToTypeProperties(type, documentFieldNames));
       //     return strs3;
       // }

       // private Dictionary<string, List<string>> MapDocumentFieldsToTypeIndexer(Type type, IEnumerable<string> documentFieldNames)
       // {
       //     Dictionary<string, List<string>> dictionary = documentFieldNames.ToDictionary<string, string, List<string>>((string f) => f.ToLowerInvariant(), (string f) => this.GetTypeFieldNames(f).ToList<string>());
       //     foreach (PropertyInfo property in base.GetProperties(type))
       //     {
       //         string name = property.Name;
       //         string propertyType = property.PropertyType;
       //         IIndexFieldNameFormatterAttribute indexFieldNameFormatterAttribute = base.GetIndexFieldNameFormatterAttribute(property);
       //         string typeFieldName = property.Name;
       //         if (indexFieldNameFormatterAttribute != null)
       //         {
       //             name = indexFieldNameFormatterAttribute.GetIndexFieldName(name);
       //             typeFieldName = indexFieldNameFormatterAttribute.GetTypeFieldName(typeFieldName);
       //         }
       //         if (!this.schema.AllFieldNames.Contains(name))
       //         {
       //             AzureFieldConfiguration fieldConfiguration = this.fieldMap.GetFieldConfiguration(propertyType) as AzureFieldConfiguration;
       //             if (fieldConfiguration != null)
       //             {
       //                 name = fieldConfiguration.FormatFieldName(name, this.schema, this.currentCultureCode);
       //             }
       //         }
       //         if (!dictionary.ContainsKey(name))
       //         {
       //             continue;
       //         }
       //         dictionary[name].Add(typeFieldName);
       //     }
       //     return dictionary;
       // }

       // private Dictionary<string, List<string>> MapDocumentFieldsToTypeProperties(Type type, IEnumerable<string> documentFieldNames)
       // {
       //     string typeFieldName;
       //     string indexFieldName;
       //     Dictionary<string, List<string>> strs = new Dictionary<string, List<string>>();
       //     foreach (string documentFieldName in documentFieldNames)
       //     {
       //         strs.Add(documentFieldName.ToLowerInvariant(), this.GetTypeFieldNames(documentFieldName).ToList<string>());
       //     }
       //     Dictionary<string, List<string>> strs1 = new Dictionary<string, List<string>>();
       //     foreach (PropertyInfo property in base.GetProperties(type))
       //     {
       //         IIndexFieldNameFormatterAttribute indexFieldNameFormatterAttribute = base.GetIndexFieldNameFormatterAttribute(property);
       //         if (indexFieldNameFormatterAttribute != null)
       //         {
       //             typeFieldName = indexFieldNameFormatterAttribute.GetTypeFieldName(property.Name);
       //             indexFieldName = this.GetIndexFieldName(indexFieldNameFormatterAttribute.GetIndexFieldName(property.Name), property.PropertyType);
       //         }
       //         else
       //         {
       //             typeFieldName = property.Name;
       //             indexFieldName = this.GetIndexFieldName(typeFieldName, property.PropertyType);
       //         }
       //         if (!strs.ContainsKey(indexFieldName))
       //         {
       //             continue;
       //         }
       //         if (!strs1.ContainsKey(indexFieldName))
       //         {
       //             strs1[indexFieldName] = new List<string>();
       //         }
       //         strs1[indexFieldName].Add(typeFieldName);
       //     }
       //     return strs1;
       // }

       // private string ProcessFieldName(string fieldName,  CultureInfo culture, string returnTypeString = "", bool aggressiveResolver = false)
       // {
       //     string lowerInvariant = fieldName;
       //     lowerInvariant = lowerInvariant.Replace(" ", "_").ToLowerInvariant();
       //     string empty = string.Empty;
       //     empty = (culture != null ? culture.TwoLetterISOLanguageName : this.currentCultureCode);
       //     AzureFieldConfiguration fieldConfiguration = this.fieldMap.GetFieldConfiguration(lowerInvariant) as AzureFieldConfiguration;
       //     if (fieldConfiguration != null)
       //     {
       //         return fieldConfiguration.FormatFieldName(lowerInvariant, this.schema, empty);
       //     }
       //     if (this.schema.AllFieldNames.Contains(lowerInvariant))
       //     {
       //         return lowerInvariant;
       //     }
       //     AzureFieldConfiguration fieldConfigurationByReturnType = null;
            
       //     if (!string.IsNullOrEmpty(returnTypeString))
       //     {
       //         fieldConfigurationByReturnType = this.fieldMap.GetFieldConfigurationByReturnType(returnTypeString) as AzureFieldConfiguration;
       //     }
       //     if (fieldConfigurationByReturnType != null)
       //     {
       //         return fieldConfigurationByReturnType.FormatFieldName(lowerInvariant, this.schema, empty);
       //     }
       //     if (aggressiveResolver)
       //     {
       //         IQueryable<TemplateResolver> queryable = this.index.CreateSearchContext(SearchSecurityOptions.DisableSecurityCheck).GetQueryable<TemplateResolver>();
       //         IQueryable<TemplateResolver> templateResolvers = (
       //             from x in queryable
       //             where x.Name == lowerInvariant
       //             select x).Filter<TemplateResolver>((TemplateResolver x) => x.TemplateName == "Template field").Take<TemplateResolver>(1);
       //         if (templateResolvers.Any<TemplateResolver>())
       //         {
       //             AzureFieldConfiguration fieldConfigurationByFieldTypeName = this.fieldMap.GetFieldConfigurationByFieldTypeName(templateResolvers.First<TemplateResolver>().Type) as SolrSearchFieldConfiguration;
       //             if (fieldConfigurationByFieldTypeName != null)
       //             {
       //                 this.fieldMap.AddFieldByFieldName(lowerInvariant, fieldConfigurationByFieldTypeName);
       //                 return fieldConfigurationByFieldTypeName.FormatFieldName(lowerInvariant, this.schema, empty);
       //             }
       //         }
       //     }
       //     return lowerInvariant;
       // }

       // //public string StripKnownCultures(string fieldName)
       // //{
       // //    foreach (string allCulture in this.schema.AllCultures)
       // //    {
       // //        if (fieldName.Length <= allCulture.Length || !fieldName.EndsWith(allCulture, StringComparison.Ordinal))
       // //        {
       // //            continue;
       // //        }
       // //        fieldName = fieldName.Substring(0, fieldName.Length - allCulture.Length);
       // //    }
       // //    return fieldName;
       // //}

       // public string StripKnownExtensions(string fieldName)
       // {
       //     //fieldName = this.StripKnownCultures(fieldName);
       //     foreach (AzureFieldConfiguration availableType in this.fieldMap.AvailableTypes)
       //     {
       //         if (fieldName.StartsWith("_", StringComparison.Ordinal) && !fieldName.StartsWith("__", StringComparison.Ordinal))
       //         {
       //             break;
       //         }
       //         string str = availableType.FieldNameFormat.Replace("{0}", string.Empty);
       //         if (fieldName.EndsWith(str, StringComparison.Ordinal))
       //         {
       //             fieldName = fieldName.Substring(0, fieldName.Length - str.Length);
       //         }
       //         if (!fieldName.StartsWith(str, StringComparison.Ordinal))
       //         {
       //             continue;
       //         }
       //         fieldName = fieldName.Substring(str.Length, fieldName.Length);
       //     }
       //     return fieldName;
       // }

       //public string StripKnownExtensions(IEnumerable<string> fields)
       //{
       //    List<string> strs = new List<string>();
       //    foreach (string field in fields)
       //    {
       //        strs.Add(this.StripKnownExtensions(field));
       //    }
       //    return string.Join(",", strs);
       //}

       
    }
}