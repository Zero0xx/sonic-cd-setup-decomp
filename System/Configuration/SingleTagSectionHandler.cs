using System;
using System.Collections;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200071A RID: 1818
	public class SingleTagSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x0600379D RID: 14237 RVA: 0x000EBB4C File Offset: 0x000EAB4C
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable hashtable;
			if (parent == null)
			{
				hashtable = new Hashtable();
			}
			else
			{
				hashtable = new Hashtable((IDictionary)parent);
			}
			HandlerBase.CheckForChildNodes(section);
			foreach (object obj in section.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				hashtable[xmlAttribute.Name] = xmlAttribute.Value;
			}
			return hashtable;
		}
	}
}
