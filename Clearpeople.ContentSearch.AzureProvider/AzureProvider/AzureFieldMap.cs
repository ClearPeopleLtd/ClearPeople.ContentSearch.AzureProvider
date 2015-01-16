using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Reflection;
using Sitecore.Xml;

namespace AzureProvider
{
    public class AzureFieldMap : IFieldMap
    {
        private readonly Dictionary<string, AzureFieldConfiguration> typeMap =
            new Dictionary<string, AzureFieldConfiguration>();

        private readonly Dictionary<string, AzureFieldConfiguration> fieldNameMap =
            new Dictionary<string, AzureFieldConfiguration>();

        private readonly Dictionary<string, AzureFieldConfiguration> fieldTypeNameMap =
            new Dictionary<string, AzureFieldConfiguration>();

        private readonly AzureFieldConfiguration AzureSearchFieldDefault = new AzureFieldConfiguration();

        public List<AzureFieldConfiguration> AvailableTypes { get; set; }

        public AzureFieldMap()
        {
            this.AvailableTypes = new List<AzureFieldConfiguration>();
        }

        public void AddFieldByFieldName(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            string attribute = XmlUtil.GetAttribute("fieldName", configNode);
            string str = XmlUtil.GetAttribute("returnType", configNode);
            if (attribute == null || str == null)
            {
                throw new ConfigurationException("Unable to process 'AddFieldByFieldName' config section.");
            }
            this.AddFieldByFieldName(attribute.ToLowerInvariant(), str.ToLowerInvariant());
        }


        public void AddFieldByFieldName(string fieldName, string returnType)
        {
            Assert.ArgumentNotNullOrEmpty(fieldName, "fieldName");
            Assert.ArgumentNotNullOrEmpty(returnType, "returnType");
            if (this.typeMap.Count == 0 || !this.typeMap.ContainsKey(returnType.ToLowerInvariant()))
            {
                return;
            }
            AzureFieldConfiguration AzureFieldConfiguration =
                new AzureFieldConfiguration(this.typeMap[returnType.ToLowerInvariant()]);
            this.fieldNameMap[fieldName.ToLowerInvariant()] = AzureFieldConfiguration;
        }

        public void AddFieldByFieldName(string fieldName, AzureFieldConfiguration setting)
        {
            this.fieldNameMap[fieldName.ToLowerInvariant()] = setting;
        }

        public void AddFieldByFieldTypeName(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            string attribute = XmlUtil.GetAttribute("fieldTypeName", configNode);
            string str = XmlUtil.GetAttribute("returnType", configNode);
            if (attribute == null || str == null)
            {
                throw new ConfigurationException("Unable to process 'AddFieldByFieldName' config section.");
            }
            char[] chrArray = new char[] {'|'};
            this.AddFieldByFieldTypeName(attribute.Split(chrArray, StringSplitOptions.RemoveEmptyEntries), str);
        }



        public void AddFieldByFieldTypeName(IEnumerable<string> fieldTypeNames, string returnType)
        {
            Assert.ArgumentNotNull(fieldTypeNames, "fieldTypeNames");
            Assert.ArgumentNotNullOrEmpty(returnType, "returnType");
            if (this.typeMap.Count == 0 || !this.typeMap.ContainsKey(returnType.ToLowerInvariant()))
            {
                return;
            }
            AzureFieldConfiguration AzureFieldConfiguration =
                new AzureFieldConfiguration(this.typeMap[returnType.ToLowerInvariant()]);
            foreach (string fieldTypeName in fieldTypeNames)
            {
                this.fieldTypeNameMap[fieldTypeName.ToLowerInvariant()] = AzureFieldConfiguration;
            }
        }

        public void AddTypeMatch(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            string str = XmlUtil.GetAttribute("settingType", configNode);
            string str1 = XmlUtil.GetAttribute("typeName", configNode);
            NameValueCollection attributes = XmlUtil.GetAttributes(configNode);
            if (str == null || attributes == null || str1 == null)
            {
                throw new ConfigurationException("Unable to process 'AddTypeMatch' config section.");
            }
            Type typeInfo = ReflectionUtil.GetTypeInfo(str);
            Dictionary<string, string> dictionary =
                attributes.Keys.Cast<string>()
                    .ToDictionary<string, string, string>((string attribute) => attribute,
                        (string attribute) => attributes[attribute]);
            this.AddTypeMatch(str1, typeInfo, dictionary);
        }

        public void AddTypeMatch(string typeName, Type settingType, IDictionary<string, string> attributes)
        {
            Assert.ArgumentNotNullOrEmpty(typeName, "typeName");
            Assert.ArgumentNotNull(settingType, "settingType");
            object[] objArray = new object[] {typeName, attributes, null};
            AzureFieldConfiguration AzureFieldConfiguration =
                (AzureFieldConfiguration) ReflectionUtility.CreateInstance(settingType, objArray);
            Assert.IsNotNull(AzureFieldConfiguration, string.Format("Unable to create : {0}", settingType));
            this.typeMap[typeName.ToLowerInvariant()] = AzureFieldConfiguration;
            this.AvailableTypes.Add(AzureFieldConfiguration);
        }

        public AbstractSearchFieldConfiguration GetFieldConfiguration(IIndexableDataField field)
        {
            AzureFieldConfiguration AzureFieldConfiguration;
            Assert.ArgumentNotNull(field, "field");
            if (this.fieldNameMap.TryGetValue(field.Name.ToLowerInvariant(), out AzureFieldConfiguration))
            {
                return AzureFieldConfiguration;
            }
            if (this.fieldTypeNameMap.TryGetValue(field.TypeKey.ToLowerInvariant(), out AzureFieldConfiguration))
            {
                return AzureFieldConfiguration;
            }
            return this.AzureSearchFieldDefault;
        }

        public AbstractSearchFieldConfiguration GetFieldConfiguration(string fieldName)
        {
            AzureFieldConfiguration AzureFieldConfiguration;
            Assert.ArgumentNotNull(fieldName, "fieldName");
            if (this.fieldNameMap.TryGetValue(fieldName.ToLowerInvariant(), out AzureFieldConfiguration))
            {
                return AzureFieldConfiguration;
            }
            return null;
        }

        public AbstractSearchFieldConfiguration GetFieldConfigurationByType(string returnType)
        {
            Assert.ArgumentNotNull(returnType, "returnType");
            AzureFieldConfiguration[] array = (
                from x in this.AvailableTypes
                where x.AzureField.Type == returnType
                select x).ToArray<AzureFieldConfiguration>();
            if (!array.Any<AzureFieldConfiguration>())
            {
                return null;
            }
            return array.First<AzureFieldConfiguration>();
        }

        public AbstractSearchFieldConfiguration GetFieldConfigurationByFieldTypeName(string fieldTypeName)
        {
            AzureFieldConfiguration AzureFieldConfiguration;
            Assert.ArgumentNotNull(fieldTypeName, "fieldTypeName");
            if (this.fieldTypeNameMap.TryGetValue(fieldTypeName.ToLowerInvariant(), out AzureFieldConfiguration))
            {
                return AzureFieldConfiguration;
            }
            return null;
        }

        public AbstractSearchFieldConfiguration GetFieldConfigurationByReturnType(string returnType)
        {
            AzureFieldConfiguration solrSearchFieldConfiguration;
            Assert.ArgumentNotNull(returnType, "returnType");
            if (this.typeMap.TryGetValue(returnType.ToLowerInvariant(), out solrSearchFieldConfiguration))
            {
                return solrSearchFieldConfiguration;
            }
            return null;
        }
    }
}
