using System;
using System.Collections;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F4 RID: 1780
	public class DictionarySectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x060036E7 RID: 14055 RVA: 0x000E9BA4 File Offset: 0x000E8BA4
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable hashtable;
			if (parent == null)
			{
				hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				hashtable = (Hashtable)((Hashtable)parent).Clone();
			}
			HandlerBase.CheckForUnrecognizedAttributes(section);
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					if (xmlNode.Name == "add")
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						string key = HandlerBase.RemoveRequiredAttribute(xmlNode, this.KeyAttributeName);
						string text;
						if (this.ValueRequired)
						{
							text = HandlerBase.RemoveRequiredAttribute(xmlNode, this.ValueAttributeName);
						}
						else
						{
							text = HandlerBase.RemoveAttribute(xmlNode, this.ValueAttributeName);
						}
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						if (text == null)
						{
							text = "";
						}
						hashtable[key] = text;
					}
					else if (xmlNode.Name == "remove")
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						string key2 = HandlerBase.RemoveRequiredAttribute(xmlNode, this.KeyAttributeName);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						hashtable.Remove(key2);
					}
					else if (xmlNode.Name.Equals("clear"))
					{
						HandlerBase.CheckForChildNodes(xmlNode);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						hashtable.Clear();
					}
					else
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
				}
			}
			return hashtable;
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x060036E8 RID: 14056 RVA: 0x000E9CFC File Offset: 0x000E8CFC
		protected virtual string KeyAttributeName
		{
			get
			{
				return "key";
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060036E9 RID: 14057 RVA: 0x000E9D03 File Offset: 0x000E8D03
		protected virtual string ValueAttributeName
		{
			get
			{
				return "value";
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060036EA RID: 14058 RVA: 0x000E9D0A File Offset: 0x000E8D0A
		internal virtual bool ValueRequired
		{
			get
			{
				return false;
			}
		}
	}
}
