using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006FF RID: 1791
	public class NameValueSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x06003728 RID: 14120 RVA: 0x000EAB6C File Offset: 0x000E9B6C
		public object Create(object parent, object context, XmlNode section)
		{
			return NameValueSectionHandler.CreateStatic(parent, section, this.KeyAttributeName, this.ValueAttributeName);
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x000EAB81 File Offset: 0x000E9B81
		internal static object CreateStatic(object parent, XmlNode section)
		{
			return NameValueSectionHandler.CreateStatic(parent, section, "key", "value");
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x000EAB94 File Offset: 0x000E9B94
		internal static object CreateStatic(object parent, XmlNode section, string keyAttriuteName, string valueAttributeName)
		{
			ReadOnlyNameValueCollection readOnlyNameValueCollection;
			if (parent == null)
			{
				readOnlyNameValueCollection = new ReadOnlyNameValueCollection(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				ReadOnlyNameValueCollection value = (ReadOnlyNameValueCollection)parent;
				readOnlyNameValueCollection = new ReadOnlyNameValueCollection(value);
			}
			HandlerBase.CheckForUnrecognizedAttributes(section);
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					if (xmlNode.Name == "add")
					{
						string name = HandlerBase.RemoveRequiredAttribute(xmlNode, keyAttriuteName);
						string value2 = HandlerBase.RemoveRequiredAttribute(xmlNode, valueAttributeName, true);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection[name] = value2;
					}
					else if (xmlNode.Name == "remove")
					{
						string name2 = HandlerBase.RemoveRequiredAttribute(xmlNode, keyAttriuteName);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection.Remove(name2);
					}
					else if (xmlNode.Name.Equals("clear"))
					{
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						readOnlyNameValueCollection.Clear();
					}
					else
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
				}
			}
			readOnlyNameValueCollection.SetReadOnly();
			return readOnlyNameValueCollection;
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x0600372B RID: 14123 RVA: 0x000EACB0 File Offset: 0x000E9CB0
		protected virtual string KeyAttributeName
		{
			get
			{
				return "key";
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x0600372C RID: 14124 RVA: 0x000EACB7 File Offset: 0x000E9CB7
		protected virtual string ValueAttributeName
		{
			get
			{
				return "value";
			}
		}

		// Token: 0x040031AA RID: 12714
		private const string defaultKeyAttribute = "key";

		// Token: 0x040031AB RID: 12715
		private const string defaultValueAttribute = "value";
	}
}
