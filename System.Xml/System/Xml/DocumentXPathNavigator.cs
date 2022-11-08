using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000BC RID: 188
	internal sealed class DocumentXPathNavigator : XPathNavigator, IHasXmlNode
	{
		// Token: 0x06000AFA RID: 2810 RVA: 0x00032B0A File Offset: 0x00031B0A
		public DocumentXPathNavigator(XmlDocument document, XmlNode node)
		{
			this.document = document;
			this.ResetPosition(node);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00032B20 File Offset: 0x00031B20
		public DocumentXPathNavigator(DocumentXPathNavigator other)
		{
			this.document = other.document;
			this.source = other.source;
			this.attributeIndex = other.attributeIndex;
			this.namespaceParent = other.namespaceParent;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00032B58 File Offset: 0x00031B58
		public override XPathNavigator Clone()
		{
			return new DocumentXPathNavigator(this);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00032B60 File Offset: 0x00031B60
		public override void SetValue(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			XmlNode xmlNode = this.source;
			switch (xmlNode.NodeType)
			{
			case XmlNodeType.Element:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
				break;
			case XmlNodeType.Attribute:
				if (!((XmlAttribute)xmlNode).IsNamespace)
				{
					xmlNode.InnerText = value;
					return;
				}
				goto IL_B8;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
			{
				this.CalibrateText();
				xmlNode = this.source;
				XmlNode xmlNode2 = this.TextEnd(xmlNode);
				if (xmlNode != xmlNode2)
				{
					if (xmlNode.IsReadOnly)
					{
						throw new InvalidOperationException(Res.GetString("Xdom_Node_Modify_ReadOnly"));
					}
					DocumentXPathNavigator.DeleteToFollowingSibling(xmlNode.NextSibling, xmlNode2);
				}
				break;
			}
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentType:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
				goto IL_B8;
			default:
				goto IL_B8;
			}
			xmlNode.InnerText = value;
			return;
			IL_B8:
			throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00032C34 File Offset: 0x00031C34
		public override XmlNameTable NameTable
		{
			get
			{
				return this.document.NameTable;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00032C41 File Offset: 0x00031C41
		public override XPathNodeType NodeType
		{
			get
			{
				this.CalibrateText();
				return this.source.XPNodeType;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00032C54 File Offset: 0x00031C54
		public override string LocalName
		{
			get
			{
				return this.source.XPLocalName;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00032C64 File Offset: 0x00031C64
		public override string NamespaceURI
		{
			get
			{
				XmlAttribute xmlAttribute = this.source as XmlAttribute;
				if (xmlAttribute != null && xmlAttribute.IsNamespace)
				{
					return string.Empty;
				}
				return this.source.NamespaceURI;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00032C9C File Offset: 0x00031C9C
		public override string Name
		{
			get
			{
				XmlNodeType nodeType = this.source.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					break;
				case XmlNodeType.Attribute:
				{
					if (!((XmlAttribute)this.source).IsNamespace)
					{
						return this.source.Name;
					}
					string localName = this.source.LocalName;
					if (object.Equals(localName, this.document.strXmlns))
					{
						return string.Empty;
					}
					return localName;
				}
				default:
					if (nodeType != XmlNodeType.ProcessingInstruction)
					{
						return string.Empty;
					}
					break;
				}
				return this.source.Name;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00032D20 File Offset: 0x00031D20
		public override string Prefix
		{
			get
			{
				XmlAttribute xmlAttribute = this.source as XmlAttribute;
				if (xmlAttribute != null && xmlAttribute.IsNamespace)
				{
					return string.Empty;
				}
				return this.source.Prefix;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00032D58 File Offset: 0x00031D58
		public override string Value
		{
			get
			{
				XmlNodeType nodeType = this.source.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					break;
				case XmlNodeType.Attribute:
					goto IL_61;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					goto IL_5A;
				default:
					switch (nodeType)
					{
					case XmlNodeType.Document:
						return this.ValueDocument;
					case XmlNodeType.DocumentType:
					case XmlNodeType.Notation:
						goto IL_61;
					case XmlNodeType.DocumentFragment:
						break;
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						goto IL_5A;
					default:
						goto IL_61;
					}
					break;
				}
				return this.source.InnerText;
				IL_5A:
				return this.ValueText;
				IL_61:
				return this.source.Value;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00032DD4 File Offset: 0x00031DD4
		private string ValueDocument
		{
			get
			{
				XmlElement documentElement = this.document.DocumentElement;
				if (documentElement != null)
				{
					return documentElement.InnerText;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00032DFC File Offset: 0x00031DFC
		private string ValueText
		{
			get
			{
				this.CalibrateText();
				string text = this.source.Value;
				XmlNode xmlNode = this.NextSibling(this.source);
				if (xmlNode != null && xmlNode.IsText)
				{
					StringBuilder stringBuilder = new StringBuilder(text);
					do
					{
						stringBuilder.Append(xmlNode.Value);
						xmlNode = this.NextSibling(xmlNode);
					}
					while (xmlNode != null && xmlNode.IsText);
					text = stringBuilder.ToString();
				}
				return text;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00032E62 File Offset: 0x00031E62
		public override string BaseURI
		{
			get
			{
				return this.source.BaseURI;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00032E70 File Offset: 0x00031E70
		public override bool IsEmptyElement
		{
			get
			{
				XmlElement xmlElement = this.source as XmlElement;
				return xmlElement != null && xmlElement.IsEmpty;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00032E94 File Offset: 0x00031E94
		public override string XmlLang
		{
			get
			{
				return this.source.XmlLang;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00032EA1 File Offset: 0x00031EA1
		public override object UnderlyingObject
		{
			get
			{
				this.CalibrateText();
				return this.source;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00032EB0 File Offset: 0x00031EB0
		public override bool HasAttributes
		{
			get
			{
				XmlElement xmlElement = this.source as XmlElement;
				if (xmlElement != null && xmlElement.HasAttributes)
				{
					XmlAttributeCollection attributes = xmlElement.Attributes;
					for (int i = 0; i < attributes.Count; i++)
					{
						XmlAttribute xmlAttribute = attributes[i];
						if (!xmlAttribute.IsNamespace)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00032EFF File Offset: 0x00031EFF
		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this.source.GetXPAttribute(localName, namespaceURI);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00032F10 File Offset: 0x00031F10
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			XmlElement xmlElement = this.source as XmlElement;
			if (xmlElement != null && xmlElement.HasAttributes)
			{
				XmlAttributeCollection attributes = xmlElement.Attributes;
				int i = 0;
				while (i < attributes.Count)
				{
					XmlAttribute xmlAttribute = attributes[i];
					if (xmlAttribute.LocalName == localName && xmlAttribute.NamespaceURI == namespaceURI)
					{
						if (!xmlAttribute.IsNamespace)
						{
							this.source = xmlAttribute;
							this.attributeIndex = i;
							return true;
						}
						return false;
					}
					else
					{
						i++;
					}
				}
			}
			return false;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00032F8C File Offset: 0x00031F8C
		public override bool MoveToFirstAttribute()
		{
			XmlElement xmlElement = this.source as XmlElement;
			if (xmlElement != null && xmlElement.HasAttributes)
			{
				XmlAttributeCollection attributes = xmlElement.Attributes;
				for (int i = 0; i < attributes.Count; i++)
				{
					XmlAttribute xmlAttribute = attributes[i];
					if (!xmlAttribute.IsNamespace)
					{
						this.source = xmlAttribute;
						this.attributeIndex = i;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00032FEC File Offset: 0x00031FEC
		public override bool MoveToNextAttribute()
		{
			XmlAttribute xmlAttribute = this.source as XmlAttribute;
			if (xmlAttribute == null || xmlAttribute.IsNamespace)
			{
				return false;
			}
			XmlAttributeCollection xmlAttributeCollection;
			if (!DocumentXPathNavigator.CheckAttributePosition(xmlAttribute, out xmlAttributeCollection, this.attributeIndex) && !DocumentXPathNavigator.ResetAttributePosition(xmlAttribute, xmlAttributeCollection, out this.attributeIndex))
			{
				return false;
			}
			for (int i = this.attributeIndex + 1; i < xmlAttributeCollection.Count; i++)
			{
				xmlAttribute = xmlAttributeCollection[i];
				if (!xmlAttribute.IsNamespace)
				{
					this.source = xmlAttribute;
					this.attributeIndex = i;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0003306C File Offset: 0x0003206C
		public override string GetNamespace(string name)
		{
			XmlNode xmlNode = this.source;
			while (xmlNode != null && xmlNode.NodeType != XmlNodeType.Element)
			{
				XmlAttribute xmlAttribute = xmlNode as XmlAttribute;
				if (xmlAttribute != null)
				{
					xmlNode = xmlAttribute.OwnerElement;
				}
				else
				{
					xmlNode = xmlNode.ParentNode;
				}
			}
			XmlElement xmlElement = xmlNode as XmlElement;
			if (xmlElement != null)
			{
				string localName;
				if (name != null && name.Length != 0)
				{
					localName = name;
				}
				else
				{
					localName = this.document.strXmlns;
				}
				string strReservedXmlns = this.document.strReservedXmlns;
				XmlAttribute attributeNode;
				for (;;)
				{
					attributeNode = xmlElement.GetAttributeNode(localName, strReservedXmlns);
					if (attributeNode != null)
					{
						break;
					}
					xmlElement = (xmlElement.ParentNode as XmlElement);
					if (xmlElement == null)
					{
						goto IL_87;
					}
				}
				return attributeNode.Value;
			}
			IL_87:
			if (name == this.document.strXml)
			{
				return this.document.strReservedXml;
			}
			if (name == this.document.strXmlns)
			{
				return this.document.strReservedXmlns;
			}
			return string.Empty;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00033144 File Offset: 0x00032144
		public override bool MoveToNamespace(string name)
		{
			if (name == this.document.strXmlns)
			{
				return false;
			}
			XmlElement xmlElement = this.source as XmlElement;
			if (xmlElement != null)
			{
				string localName;
				if (name != null && name.Length != 0)
				{
					localName = name;
				}
				else
				{
					localName = this.document.strXmlns;
				}
				string strReservedXmlns = this.document.strReservedXmlns;
				XmlAttribute attributeNode;
				for (;;)
				{
					attributeNode = xmlElement.GetAttributeNode(localName, strReservedXmlns);
					if (attributeNode != null)
					{
						break;
					}
					xmlElement = (xmlElement.ParentNode as XmlElement);
					if (xmlElement == null)
					{
						goto Block_6;
					}
				}
				this.namespaceParent = (XmlElement)this.source;
				this.source = attributeNode;
				return true;
				Block_6:
				if (name == this.document.strXml)
				{
					this.namespaceParent = (XmlElement)this.source;
					this.source = this.document.NamespaceXml;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0003320C File Offset: 0x0003220C
		public override bool MoveToFirstNamespace(XPathNamespaceScope scope)
		{
			XmlElement xmlElement = this.source as XmlElement;
			if (xmlElement == null)
			{
				return false;
			}
			int maxValue = int.MaxValue;
			switch (scope)
			{
			case XPathNamespaceScope.All:
			{
				XmlAttributeCollection attributes = xmlElement.Attributes;
				if (!DocumentXPathNavigator.MoveToFirstNamespaceGlobal(ref attributes, ref maxValue))
				{
					this.source = this.document.NamespaceXml;
				}
				else
				{
					this.source = attributes[maxValue];
					this.attributeIndex = maxValue;
				}
				this.namespaceParent = xmlElement;
				break;
			}
			case XPathNamespaceScope.ExcludeXml:
			{
				XmlAttributeCollection attributes = xmlElement.Attributes;
				if (!DocumentXPathNavigator.MoveToFirstNamespaceGlobal(ref attributes, ref maxValue))
				{
					return false;
				}
				XmlAttribute xmlAttribute = attributes[maxValue];
				while (object.Equals(xmlAttribute.LocalName, this.document.strXml))
				{
					if (!DocumentXPathNavigator.MoveToNextNamespaceGlobal(ref attributes, ref maxValue))
					{
						return false;
					}
					xmlAttribute = attributes[maxValue];
				}
				this.source = xmlAttribute;
				this.attributeIndex = maxValue;
				this.namespaceParent = xmlElement;
				break;
			}
			case XPathNamespaceScope.Local:
			{
				if (!xmlElement.HasAttributes)
				{
					return false;
				}
				XmlAttributeCollection attributes = xmlElement.Attributes;
				if (!DocumentXPathNavigator.MoveToFirstNamespaceLocal(attributes, ref maxValue))
				{
					return false;
				}
				this.source = attributes[maxValue];
				this.attributeIndex = maxValue;
				this.namespaceParent = xmlElement;
				break;
			}
			default:
				return false;
			}
			return true;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00033330 File Offset: 0x00032330
		private static bool MoveToFirstNamespaceLocal(XmlAttributeCollection attributes, ref int index)
		{
			for (int i = attributes.Count - 1; i >= 0; i--)
			{
				XmlAttribute xmlAttribute = attributes[i];
				if (xmlAttribute.IsNamespace)
				{
					index = i;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00033368 File Offset: 0x00032368
		private static bool MoveToFirstNamespaceGlobal(ref XmlAttributeCollection attributes, ref int index)
		{
			if (DocumentXPathNavigator.MoveToFirstNamespaceLocal(attributes, ref index))
			{
				return true;
			}
			for (XmlElement xmlElement = attributes.parent.ParentNode as XmlElement; xmlElement != null; xmlElement = (xmlElement.ParentNode as XmlElement))
			{
				if (xmlElement.HasAttributes)
				{
					attributes = xmlElement.Attributes;
					if (DocumentXPathNavigator.MoveToFirstNamespaceLocal(attributes, ref index))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x000333C4 File Offset: 0x000323C4
		public override bool MoveToNextNamespace(XPathNamespaceScope scope)
		{
			XmlAttribute xmlAttribute = this.source as XmlAttribute;
			if (xmlAttribute == null || !xmlAttribute.IsNamespace)
			{
				return false;
			}
			int num = this.attributeIndex;
			XmlAttributeCollection xmlAttributeCollection;
			if (!DocumentXPathNavigator.CheckAttributePosition(xmlAttribute, out xmlAttributeCollection, num) && !DocumentXPathNavigator.ResetAttributePosition(xmlAttribute, xmlAttributeCollection, out num))
			{
				return false;
			}
			switch (scope)
			{
			case XPathNamespaceScope.All:
				while (DocumentXPathNavigator.MoveToNextNamespaceGlobal(ref xmlAttributeCollection, ref num))
				{
					xmlAttribute = xmlAttributeCollection[num];
					if (!this.PathHasDuplicateNamespace(xmlAttribute.OwnerElement, this.namespaceParent, xmlAttribute.LocalName))
					{
						this.source = xmlAttribute;
						this.attributeIndex = num;
						return true;
					}
				}
				if (this.PathHasDuplicateNamespace(null, this.namespaceParent, this.document.strXml))
				{
					return false;
				}
				this.source = this.document.NamespaceXml;
				return true;
			case XPathNamespaceScope.ExcludeXml:
				while (DocumentXPathNavigator.MoveToNextNamespaceGlobal(ref xmlAttributeCollection, ref num))
				{
					xmlAttribute = xmlAttributeCollection[num];
					string localName = xmlAttribute.LocalName;
					if (!this.PathHasDuplicateNamespace(xmlAttribute.OwnerElement, this.namespaceParent, localName) && !object.Equals(localName, this.document.strXml))
					{
						this.source = xmlAttribute;
						this.attributeIndex = num;
						return true;
					}
				}
				return false;
			case XPathNamespaceScope.Local:
				if (xmlAttribute.OwnerElement != this.namespaceParent)
				{
					return false;
				}
				if (!DocumentXPathNavigator.MoveToNextNamespaceLocal(xmlAttributeCollection, ref num))
				{
					return false;
				}
				this.source = xmlAttributeCollection[num];
				this.attributeIndex = num;
				break;
			default:
				return false;
			}
			return true;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0003351C File Offset: 0x0003251C
		private static bool MoveToNextNamespaceLocal(XmlAttributeCollection attributes, ref int index)
		{
			for (int i = index - 1; i >= 0; i--)
			{
				XmlAttribute xmlAttribute = attributes[i];
				if (xmlAttribute.IsNamespace)
				{
					index = i;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00033550 File Offset: 0x00032550
		private static bool MoveToNextNamespaceGlobal(ref XmlAttributeCollection attributes, ref int index)
		{
			if (DocumentXPathNavigator.MoveToNextNamespaceLocal(attributes, ref index))
			{
				return true;
			}
			for (XmlElement xmlElement = attributes.parent.ParentNode as XmlElement; xmlElement != null; xmlElement = (xmlElement.ParentNode as XmlElement))
			{
				if (xmlElement.HasAttributes)
				{
					attributes = xmlElement.Attributes;
					if (DocumentXPathNavigator.MoveToFirstNamespaceLocal(attributes, ref index))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x000335AC File Offset: 0x000325AC
		private bool PathHasDuplicateNamespace(XmlElement top, XmlElement bottom, string localName)
		{
			string strReservedXmlns = this.document.strReservedXmlns;
			while (bottom != null && bottom != top)
			{
				XmlAttribute attributeNode = bottom.GetAttributeNode(localName, strReservedXmlns);
				if (attributeNode != null)
				{
					return true;
				}
				bottom = (bottom.ParentNode as XmlElement);
			}
			return false;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x000335EC File Offset: 0x000325EC
		public override bool MoveToNext()
		{
			XmlNode xmlNode = this.NextSibling(this.source);
			if (xmlNode == null)
			{
				return false;
			}
			if (xmlNode.IsText && this.source.IsText)
			{
				xmlNode = this.NextSibling(this.TextEnd(xmlNode));
				if (xmlNode == null)
				{
					return false;
				}
			}
			XmlNode parent = this.ParentNode(xmlNode);
			while (!DocumentXPathNavigator.IsValidChild(parent, xmlNode))
			{
				xmlNode = this.NextSibling(xmlNode);
				if (xmlNode == null)
				{
					return false;
				}
			}
			this.source = xmlNode;
			return true;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0003365C File Offset: 0x0003265C
		public override bool MoveToPrevious()
		{
			XmlNode xmlNode = this.PreviousSibling(this.source);
			if (xmlNode == null)
			{
				return false;
			}
			if (xmlNode.IsText)
			{
				if (this.source.IsText)
				{
					xmlNode = this.PreviousSibling(this.TextStart(xmlNode));
					if (xmlNode == null)
					{
						return false;
					}
				}
				else
				{
					xmlNode = this.TextStart(xmlNode);
				}
			}
			XmlNode parent = this.ParentNode(xmlNode);
			while (!DocumentXPathNavigator.IsValidChild(parent, xmlNode))
			{
				xmlNode = this.PreviousSibling(xmlNode);
				if (xmlNode == null)
				{
					return false;
				}
			}
			this.source = xmlNode;
			return true;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x000336D4 File Offset: 0x000326D4
		public override bool MoveToFirst()
		{
			if (this.source.NodeType == XmlNodeType.Attribute)
			{
				return false;
			}
			XmlNode xmlNode = this.ParentNode(this.source);
			if (xmlNode == null)
			{
				return false;
			}
			XmlNode xmlNode2 = this.FirstChild(xmlNode);
			while (!DocumentXPathNavigator.IsValidChild(xmlNode, xmlNode2))
			{
				xmlNode2 = this.NextSibling(xmlNode2);
				if (xmlNode2 == null)
				{
					return false;
				}
			}
			this.source = xmlNode2;
			return true;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0003372C File Offset: 0x0003272C
		public override bool MoveToFirstChild()
		{
			XmlNodeType nodeType = this.source.NodeType;
			XmlNode xmlNode;
			if (nodeType != XmlNodeType.Element)
			{
				switch (nodeType)
				{
				case XmlNodeType.Document:
				case XmlNodeType.DocumentFragment:
					xmlNode = this.FirstChild(this.source);
					if (xmlNode == null)
					{
						return false;
					}
					while (!DocumentXPathNavigator.IsValidChild(this.source, xmlNode))
					{
						xmlNode = this.NextSibling(xmlNode);
						if (xmlNode == null)
						{
							return false;
						}
					}
					goto IL_6A;
				}
				return false;
			}
			xmlNode = this.FirstChild(this.source);
			if (xmlNode == null)
			{
				return false;
			}
			IL_6A:
			this.source = xmlNode;
			return true;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x000337AC File Offset: 0x000327AC
		public override bool MoveToParent()
		{
			XmlNode xmlNode = this.ParentNode(this.source);
			if (xmlNode != null)
			{
				this.source = xmlNode;
				return true;
			}
			XmlAttribute xmlAttribute = this.source as XmlAttribute;
			if (xmlAttribute != null)
			{
				xmlNode = (xmlAttribute.IsNamespace ? this.namespaceParent : xmlAttribute.OwnerElement);
				if (xmlNode != null)
				{
					this.source = xmlNode;
					this.namespaceParent = null;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0003380C File Offset: 0x0003280C
		public override void MoveToRoot()
		{
			for (;;)
			{
				XmlNode xmlNode = this.source.ParentNode;
				if (xmlNode == null)
				{
					XmlAttribute xmlAttribute = this.source as XmlAttribute;
					if (xmlAttribute == null)
					{
						break;
					}
					xmlNode = (xmlAttribute.IsNamespace ? this.namespaceParent : xmlAttribute.OwnerElement);
					if (xmlNode == null)
					{
						break;
					}
				}
				this.source = xmlNode;
			}
			this.namespaceParent = null;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00033864 File Offset: 0x00032864
		public override bool MoveTo(XPathNavigator other)
		{
			DocumentXPathNavigator documentXPathNavigator = other as DocumentXPathNavigator;
			if (documentXPathNavigator != null && this.document == documentXPathNavigator.document)
			{
				this.source = documentXPathNavigator.source;
				this.attributeIndex = documentXPathNavigator.attributeIndex;
				this.namespaceParent = documentXPathNavigator.namespaceParent;
				return true;
			}
			return false;
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x000338B0 File Offset: 0x000328B0
		public override bool MoveToId(string id)
		{
			XmlElement elementById = this.document.GetElementById(id);
			if (elementById != null)
			{
				this.source = elementById;
				this.namespaceParent = null;
				return true;
			}
			return false;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x000338E0 File Offset: 0x000328E0
		public override bool MoveToChild(string localName, string namespaceUri)
		{
			if (this.source.NodeType == XmlNodeType.Attribute)
			{
				return false;
			}
			XmlNode xmlNode = this.FirstChild(this.source);
			if (xmlNode != null)
			{
				while (xmlNode.NodeType != XmlNodeType.Element || !(xmlNode.LocalName == localName) || !(xmlNode.NamespaceURI == namespaceUri))
				{
					xmlNode = this.NextSibling(xmlNode);
					if (xmlNode == null)
					{
						return false;
					}
				}
				this.source = xmlNode;
				return true;
			}
			return false;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00033948 File Offset: 0x00032948
		public override bool MoveToChild(XPathNodeType type)
		{
			if (this.source.NodeType == XmlNodeType.Attribute)
			{
				return false;
			}
			XmlNode xmlNode = this.FirstChild(this.source);
			if (xmlNode != null)
			{
				int contentKindMask = XPathNavigator.GetContentKindMask(type);
				if (contentKindMask == 0)
				{
					return false;
				}
				while ((1 << (int)xmlNode.XPNodeType & contentKindMask) == 0)
				{
					xmlNode = this.NextSibling(xmlNode);
					if (xmlNode == null)
					{
						return false;
					}
				}
				this.source = xmlNode;
				return true;
			}
			return false;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x000339A8 File Offset: 0x000329A8
		public override bool MoveToFollowing(string localName, string namespaceUri, XPathNavigator end)
		{
			XmlNode xmlNode = null;
			DocumentXPathNavigator documentXPathNavigator = end as DocumentXPathNavigator;
			if (documentXPathNavigator != null)
			{
				if (this.document != documentXPathNavigator.document)
				{
					return false;
				}
				XmlNodeType nodeType = documentXPathNavigator.source.NodeType;
				if (nodeType == XmlNodeType.Attribute)
				{
					documentXPathNavigator = (DocumentXPathNavigator)documentXPathNavigator.Clone();
					if (!documentXPathNavigator.MoveToNonDescendant())
					{
						return false;
					}
				}
				xmlNode = documentXPathNavigator.source;
			}
			XmlNode xmlNode2 = this.source;
			if (xmlNode2.NodeType == XmlNodeType.Attribute)
			{
				xmlNode2 = ((XmlAttribute)xmlNode2).OwnerElement;
				if (xmlNode2 == null)
				{
					return false;
				}
			}
			for (;;)
			{
				XmlNode firstChild = xmlNode2.FirstChild;
				if (firstChild != null)
				{
					xmlNode2 = firstChild;
				}
				else
				{
					XmlNode nextSibling;
					for (;;)
					{
						nextSibling = xmlNode2.NextSibling;
						if (nextSibling != null)
						{
							break;
						}
						XmlNode parentNode = xmlNode2.ParentNode;
						if (parentNode == null)
						{
							return false;
						}
						xmlNode2 = parentNode;
					}
					xmlNode2 = nextSibling;
				}
				if (xmlNode2 == xmlNode)
				{
					return false;
				}
				if (xmlNode2.NodeType == XmlNodeType.Element && !(xmlNode2.LocalName != localName) && !(xmlNode2.NamespaceURI != namespaceUri))
				{
					goto Block_13;
				}
			}
			return false;
			Block_13:
			this.source = xmlNode2;
			return true;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00033A88 File Offset: 0x00032A88
		public override bool MoveToFollowing(XPathNodeType type, XPathNavigator end)
		{
			XmlNode xmlNode = null;
			DocumentXPathNavigator documentXPathNavigator = end as DocumentXPathNavigator;
			if (documentXPathNavigator != null)
			{
				if (this.document != documentXPathNavigator.document)
				{
					return false;
				}
				XmlNodeType nodeType = documentXPathNavigator.source.NodeType;
				if (nodeType == XmlNodeType.Attribute)
				{
					documentXPathNavigator = (DocumentXPathNavigator)documentXPathNavigator.Clone();
					if (!documentXPathNavigator.MoveToNonDescendant())
					{
						return false;
					}
				}
				xmlNode = documentXPathNavigator.source;
			}
			int contentKindMask = XPathNavigator.GetContentKindMask(type);
			if (contentKindMask == 0)
			{
				return false;
			}
			XmlNode xmlNode2 = this.source;
			XmlNodeType nodeType2 = xmlNode2.NodeType;
			switch (nodeType2)
			{
			case XmlNodeType.Attribute:
				xmlNode2 = ((XmlAttribute)xmlNode2).OwnerElement;
				if (xmlNode2 == null)
				{
					return false;
				}
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				goto IL_A0;
			default:
				switch (nodeType2)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					goto IL_A0;
				}
				break;
			}
			for (;;)
			{
				IL_A8:
				XmlNode firstChild = xmlNode2.FirstChild;
				if (firstChild != null)
				{
					xmlNode2 = firstChild;
				}
				else
				{
					XmlNode nextSibling;
					for (;;)
					{
						nextSibling = xmlNode2.NextSibling;
						if (nextSibling != null)
						{
							break;
						}
						XmlNode parentNode = xmlNode2.ParentNode;
						if (parentNode == null)
						{
							return false;
						}
						xmlNode2 = parentNode;
					}
					xmlNode2 = nextSibling;
				}
				if (xmlNode2 == xmlNode)
				{
					return false;
				}
				if ((1 << (int)xmlNode2.XPNodeType & contentKindMask) != 0)
				{
					goto Block_13;
				}
			}
			return false;
			Block_13:
			this.source = xmlNode2;
			return true;
			IL_A0:
			xmlNode2 = this.TextEnd(xmlNode2);
			goto IL_A8;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00033B90 File Offset: 0x00032B90
		public override bool MoveToNext(string localName, string namespaceUri)
		{
			XmlNode xmlNode = this.NextSibling(this.source);
			if (xmlNode == null)
			{
				return false;
			}
			while (xmlNode.NodeType != XmlNodeType.Element || !(xmlNode.LocalName == localName) || !(xmlNode.NamespaceURI == namespaceUri))
			{
				xmlNode = this.NextSibling(xmlNode);
				if (xmlNode == null)
				{
					return false;
				}
			}
			this.source = xmlNode;
			return true;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00033BEC File Offset: 0x00032BEC
		public override bool MoveToNext(XPathNodeType type)
		{
			XmlNode xmlNode = this.NextSibling(this.source);
			if (xmlNode == null)
			{
				return false;
			}
			if (xmlNode.IsText && this.source.IsText)
			{
				xmlNode = this.NextSibling(this.TextEnd(xmlNode));
				if (xmlNode == null)
				{
					return false;
				}
			}
			int contentKindMask = XPathNavigator.GetContentKindMask(type);
			if (contentKindMask == 0)
			{
				return false;
			}
			while ((1 << (int)xmlNode.XPNodeType & contentKindMask) == 0)
			{
				xmlNode = this.NextSibling(xmlNode);
				if (xmlNode == null)
				{
					return false;
				}
			}
			this.source = xmlNode;
			return true;
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00033C64 File Offset: 0x00032C64
		public override bool HasChildren
		{
			get
			{
				XmlNodeType nodeType = this.source.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					switch (nodeType)
					{
					case XmlNodeType.Document:
					case XmlNodeType.DocumentFragment:
					{
						XmlNode xmlNode = this.FirstChild(this.source);
						if (xmlNode == null)
						{
							return false;
						}
						while (!DocumentXPathNavigator.IsValidChild(this.source, xmlNode))
						{
							xmlNode = this.NextSibling(xmlNode);
							if (xmlNode == null)
							{
								return false;
							}
						}
						return true;
					}
					}
					return false;
				}
				return this.FirstChild(this.source) != null;
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00033CDC File Offset: 0x00032CDC
		public override bool IsSamePosition(XPathNavigator other)
		{
			DocumentXPathNavigator documentXPathNavigator = other as DocumentXPathNavigator;
			if (documentXPathNavigator != null)
			{
				this.CalibrateText();
				documentXPathNavigator.CalibrateText();
				return this.source == documentXPathNavigator.source && this.namespaceParent == documentXPathNavigator.namespaceParent;
			}
			return false;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00033D20 File Offset: 0x00032D20
		public override bool IsDescendant(XPathNavigator other)
		{
			DocumentXPathNavigator documentXPathNavigator = other as DocumentXPathNavigator;
			return documentXPathNavigator != null && DocumentXPathNavigator.IsDescendant(this.source, documentXPathNavigator.source);
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00033D4A File Offset: 0x00032D4A
		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this.source.SchemaInfo;
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00033D58 File Offset: 0x00032D58
		public override bool CheckValidity(XmlSchemaSet schemas, ValidationEventHandler validationEventHandler)
		{
			XmlDocument xmlDocument;
			if (this.source.NodeType == XmlNodeType.Document)
			{
				xmlDocument = (XmlDocument)this.source;
			}
			else
			{
				xmlDocument = this.source.OwnerDocument;
				if (schemas != null)
				{
					throw new ArgumentException(Res.GetString("XPathDocument_SchemaSetNotAllowed", null));
				}
			}
			if (schemas == null && xmlDocument != null)
			{
				schemas = xmlDocument.Schemas;
			}
			if (schemas == null || schemas.Count == 0)
			{
				throw new InvalidOperationException(Res.GetString("XmlDocument_NoSchemaInfo"));
			}
			return new DocumentSchemaValidator(xmlDocument, schemas, validationEventHandler)
			{
				PsviAugmentation = false
			}.Validate(this.source);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00033DE8 File Offset: 0x00032DE8
		private static XmlNode OwnerNode(XmlNode node)
		{
			XmlNode parentNode = node.ParentNode;
			if (parentNode != null)
			{
				return parentNode;
			}
			XmlAttribute xmlAttribute = node as XmlAttribute;
			if (xmlAttribute != null)
			{
				return xmlAttribute.OwnerElement;
			}
			return null;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00033E14 File Offset: 0x00032E14
		private static int GetDepth(XmlNode node)
		{
			int num = 0;
			for (XmlNode node2 = DocumentXPathNavigator.OwnerNode(node); node2 != null; node2 = DocumentXPathNavigator.OwnerNode(node2))
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00033E3C File Offset: 0x00032E3C
		private XmlNodeOrder Compare(XmlNode node1, XmlNode node2)
		{
			if (node1.XPNodeType == XPathNodeType.Attribute)
			{
				if (node2.XPNodeType == XPathNodeType.Attribute)
				{
					XmlElement ownerElement = ((XmlAttribute)node1).OwnerElement;
					if (ownerElement.HasAttributes)
					{
						XmlAttributeCollection attributes = ownerElement.Attributes;
						for (int i = 0; i < attributes.Count; i++)
						{
							XmlAttribute xmlAttribute = attributes[i];
							if (xmlAttribute == node1)
							{
								return XmlNodeOrder.Before;
							}
							if (xmlAttribute == node2)
							{
								return XmlNodeOrder.After;
							}
						}
					}
					return XmlNodeOrder.Unknown;
				}
				return XmlNodeOrder.Before;
			}
			else
			{
				if (node2.XPNodeType == XPathNodeType.Attribute)
				{
					return XmlNodeOrder.After;
				}
				XmlNode nextSibling = node1.NextSibling;
				while (nextSibling != null && nextSibling != node2)
				{
					nextSibling = nextSibling.NextSibling;
				}
				if (nextSibling == null)
				{
					return XmlNodeOrder.After;
				}
				return XmlNodeOrder.Before;
			}
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00033ED0 File Offset: 0x00032ED0
		public override XmlNodeOrder ComparePosition(XPathNavigator other)
		{
			DocumentXPathNavigator documentXPathNavigator = other as DocumentXPathNavigator;
			if (documentXPathNavigator == null)
			{
				return XmlNodeOrder.Unknown;
			}
			this.CalibrateText();
			documentXPathNavigator.CalibrateText();
			if (this.source == documentXPathNavigator.source && this.namespaceParent == documentXPathNavigator.namespaceParent)
			{
				return XmlNodeOrder.Same;
			}
			if (this.namespaceParent != null || documentXPathNavigator.namespaceParent != null)
			{
				return base.ComparePosition(other);
			}
			XmlNode xmlNode = this.source;
			XmlNode xmlNode2 = documentXPathNavigator.source;
			XmlNode xmlNode3 = DocumentXPathNavigator.OwnerNode(xmlNode);
			XmlNode xmlNode4 = DocumentXPathNavigator.OwnerNode(xmlNode2);
			if (xmlNode3 != xmlNode4)
			{
				int num = DocumentXPathNavigator.GetDepth(xmlNode);
				int num2 = DocumentXPathNavigator.GetDepth(xmlNode2);
				if (num2 > num)
				{
					while (xmlNode2 != null && num2 > num)
					{
						xmlNode2 = DocumentXPathNavigator.OwnerNode(xmlNode2);
						num2--;
					}
					if (xmlNode == xmlNode2)
					{
						return XmlNodeOrder.Before;
					}
					xmlNode4 = DocumentXPathNavigator.OwnerNode(xmlNode2);
				}
				else if (num > num2)
				{
					while (xmlNode != null && num > num2)
					{
						xmlNode = DocumentXPathNavigator.OwnerNode(xmlNode);
						num--;
					}
					if (xmlNode == xmlNode2)
					{
						return XmlNodeOrder.After;
					}
					xmlNode3 = DocumentXPathNavigator.OwnerNode(xmlNode);
				}
				while (xmlNode3 != null && xmlNode4 != null)
				{
					if (xmlNode3 == xmlNode4)
					{
						return this.Compare(xmlNode, xmlNode2);
					}
					xmlNode = xmlNode3;
					xmlNode2 = xmlNode4;
					xmlNode3 = DocumentXPathNavigator.OwnerNode(xmlNode);
					xmlNode4 = DocumentXPathNavigator.OwnerNode(xmlNode2);
				}
				return XmlNodeOrder.Unknown;
			}
			if (xmlNode3 == null)
			{
				return XmlNodeOrder.Unknown;
			}
			return this.Compare(xmlNode, xmlNode2);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00033FF0 File Offset: 0x00032FF0
		XmlNode IHasXmlNode.GetNode()
		{
			return this.source;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00033FF8 File Offset: 0x00032FF8
		public override XPathNodeIterator SelectDescendants(string localName, string namespaceURI, bool matchSelf)
		{
			string text = this.document.NameTable.Get(namespaceURI);
			if (text == null || this.source.NodeType == XmlNodeType.Attribute)
			{
				return new DocumentXPathNodeIterator_Empty(this);
			}
			string text2 = this.document.NameTable.Get(localName);
			if (text2 == null)
			{
				return new DocumentXPathNodeIterator_Empty(this);
			}
			if (text2.Length == 0)
			{
				if (matchSelf)
				{
					return new DocumentXPathNodeIterator_ElemChildren_AndSelf_NoLocalName(this, text);
				}
				return new DocumentXPathNodeIterator_ElemChildren_NoLocalName(this, text);
			}
			else
			{
				if (matchSelf)
				{
					return new DocumentXPathNodeIterator_ElemChildren_AndSelf(this, text2, text);
				}
				return new DocumentXPathNodeIterator_ElemChildren(this, text2, text);
			}
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0003407C File Offset: 0x0003307C
		public override XPathNodeIterator SelectDescendants(XPathNodeType nt, bool includeSelf)
		{
			if (nt != XPathNodeType.Element)
			{
				return base.SelectDescendants(nt, includeSelf);
			}
			XmlNodeType nodeType = this.source.NodeType;
			if (nodeType != XmlNodeType.Document && nodeType != XmlNodeType.Element)
			{
				return new DocumentXPathNodeIterator_Empty(this);
			}
			if (includeSelf)
			{
				return new DocumentXPathNodeIterator_AllElemChildren_AndSelf(this);
			}
			return new DocumentXPathNodeIterator_AllElemChildren(this);
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x000340C2 File Offset: 0x000330C2
		public override bool CanEdit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x000340C8 File Offset: 0x000330C8
		public override XmlWriter PrependChild()
		{
			XmlNodeType nodeType = this.source.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				switch (nodeType)
				{
				case XmlNodeType.Document:
				case XmlNodeType.DocumentFragment:
					break;
				default:
					throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
				}
			}
			DocumentXmlWriter documentXmlWriter = new DocumentXmlWriter(DocumentXmlWriterType.PrependChild, this.source, this.document);
			documentXmlWriter.NamespaceManager = DocumentXPathNavigator.GetNamespaceManager(this.source, this.document);
			return new XmlWellFormedWriter(documentXmlWriter, documentXmlWriter.Settings);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00034140 File Offset: 0x00033140
		public override XmlWriter AppendChild()
		{
			XmlNodeType nodeType = this.source.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				switch (nodeType)
				{
				case XmlNodeType.Document:
				case XmlNodeType.DocumentFragment:
					break;
				default:
					throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
				}
			}
			DocumentXmlWriter documentXmlWriter = new DocumentXmlWriter(DocumentXmlWriterType.AppendChild, this.source, this.document);
			documentXmlWriter.NamespaceManager = DocumentXPathNavigator.GetNamespaceManager(this.source, this.document);
			return new XmlWellFormedWriter(documentXmlWriter, documentXmlWriter.Settings);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000341B8 File Offset: 0x000331B8
		public override XmlWriter InsertAfter()
		{
			XmlNode xmlNode = this.source;
			switch (xmlNode.NodeType)
			{
			case XmlNodeType.Attribute:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				xmlNode = this.TextEnd(xmlNode);
				break;
			}
			DocumentXmlWriter documentXmlWriter = new DocumentXmlWriter(DocumentXmlWriterType.InsertSiblingAfter, xmlNode, this.document);
			documentXmlWriter.NamespaceManager = DocumentXPathNavigator.GetNamespaceManager(xmlNode.ParentNode, this.document);
			return new XmlWellFormedWriter(documentXmlWriter, documentXmlWriter.Settings);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0003425C File Offset: 0x0003325C
		public override XmlWriter InsertBefore()
		{
			switch (this.source.NodeType)
			{
			case XmlNodeType.Attribute:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				this.CalibrateText();
				break;
			}
			DocumentXmlWriter documentXmlWriter = new DocumentXmlWriter(DocumentXmlWriterType.InsertSiblingBefore, this.source, this.document);
			documentXmlWriter.NamespaceManager = DocumentXPathNavigator.GetNamespaceManager(this.source.ParentNode, this.document);
			return new XmlWellFormedWriter(documentXmlWriter, documentXmlWriter.Settings);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00034304 File Offset: 0x00033304
		public override XmlWriter CreateAttributes()
		{
			if (this.source.NodeType != XmlNodeType.Element)
			{
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			}
			DocumentXmlWriter documentXmlWriter = new DocumentXmlWriter(DocumentXmlWriterType.AppendAttribute, this.source, this.document);
			documentXmlWriter.NamespaceManager = DocumentXPathNavigator.GetNamespaceManager(this.source, this.document);
			return new XmlWellFormedWriter(documentXmlWriter, documentXmlWriter.Settings);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00034368 File Offset: 0x00033368
		public override XmlWriter ReplaceRange(XPathNavigator lastSiblingToReplace)
		{
			DocumentXPathNavigator documentXPathNavigator = lastSiblingToReplace as DocumentXPathNavigator;
			if (documentXPathNavigator != null)
			{
				this.CalibrateText();
				documentXPathNavigator.CalibrateText();
				XmlNode xmlNode = this.source;
				XmlNode xmlNode2 = documentXPathNavigator.source;
				if (xmlNode == xmlNode2)
				{
					switch (xmlNode.NodeType)
					{
					case XmlNodeType.Attribute:
					case XmlNodeType.Document:
					case XmlNodeType.DocumentFragment:
						throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						xmlNode2 = documentXPathNavigator.TextEnd(xmlNode2);
						break;
					}
				}
				else
				{
					if (xmlNode2.IsText)
					{
						xmlNode2 = documentXPathNavigator.TextEnd(xmlNode2);
					}
					if (!DocumentXPathNavigator.IsFollowingSibling(xmlNode, xmlNode2))
					{
						throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
					}
				}
				DocumentXmlWriter documentXmlWriter = new DocumentXmlWriter(DocumentXmlWriterType.ReplaceToFollowingSibling, xmlNode, this.document);
				documentXmlWriter.NamespaceManager = DocumentXPathNavigator.GetNamespaceManager(xmlNode.ParentNode, this.document);
				documentXmlWriter.Navigator = this;
				documentXmlWriter.EndNode = xmlNode2;
				return new XmlWellFormedWriter(documentXmlWriter, documentXmlWriter.Settings);
			}
			if (lastSiblingToReplace == null)
			{
				throw new ArgumentNullException("lastSiblingToReplace");
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0003447C File Offset: 0x0003347C
		public override void DeleteRange(XPathNavigator lastSiblingToDelete)
		{
			DocumentXPathNavigator documentXPathNavigator = lastSiblingToDelete as DocumentXPathNavigator;
			if (documentXPathNavigator != null)
			{
				this.CalibrateText();
				documentXPathNavigator.CalibrateText();
				XmlNode xmlNode = this.source;
				XmlNode xmlNode2 = documentXPathNavigator.source;
				if (xmlNode == xmlNode2)
				{
					XmlNode xmlNode3;
					switch (xmlNode.NodeType)
					{
					case XmlNodeType.Element:
					case XmlNodeType.ProcessingInstruction:
					case XmlNodeType.Comment:
						break;
					case XmlNodeType.Attribute:
					{
						XmlAttribute xmlAttribute = (XmlAttribute)xmlNode;
						if (xmlAttribute.IsNamespace)
						{
							goto IL_E1;
						}
						xmlNode3 = DocumentXPathNavigator.OwnerNode(xmlAttribute);
						DocumentXPathNavigator.DeleteAttribute(xmlAttribute, this.attributeIndex);
						if (xmlNode3 != null)
						{
							this.ResetPosition(xmlNode3);
							return;
						}
						return;
					}
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						xmlNode2 = documentXPathNavigator.TextEnd(xmlNode2);
						break;
					case XmlNodeType.EntityReference:
					case XmlNodeType.Entity:
					case XmlNodeType.Document:
					case XmlNodeType.DocumentType:
					case XmlNodeType.DocumentFragment:
					case XmlNodeType.Notation:
						goto IL_E1;
					default:
						goto IL_E1;
					}
					xmlNode3 = DocumentXPathNavigator.OwnerNode(xmlNode);
					DocumentXPathNavigator.DeleteToFollowingSibling(xmlNode, xmlNode2);
					if (xmlNode3 != null)
					{
						this.ResetPosition(xmlNode3);
						return;
					}
					return;
					IL_E1:
					throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
				}
				if (xmlNode2.IsText)
				{
					xmlNode2 = documentXPathNavigator.TextEnd(xmlNode2);
				}
				if (!DocumentXPathNavigator.IsFollowingSibling(xmlNode, xmlNode2))
				{
					throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
				}
				XmlNode xmlNode4 = DocumentXPathNavigator.OwnerNode(xmlNode);
				DocumentXPathNavigator.DeleteToFollowingSibling(xmlNode, xmlNode2);
				if (xmlNode4 != null)
				{
					this.ResetPosition(xmlNode4);
				}
				return;
			}
			if (lastSiblingToDelete == null)
			{
				throw new ArgumentNullException("lastSiblingToDelete");
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000345C0 File Offset: 0x000335C0
		public override void DeleteSelf()
		{
			XmlNode xmlNode = this.source;
			XmlNode end = xmlNode;
			XmlNode xmlNode2;
			switch (xmlNode.NodeType)
			{
			case XmlNodeType.Element:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
				break;
			case XmlNodeType.Attribute:
			{
				XmlAttribute xmlAttribute = (XmlAttribute)xmlNode;
				if (xmlAttribute.IsNamespace)
				{
					goto IL_AF;
				}
				xmlNode2 = DocumentXPathNavigator.OwnerNode(xmlAttribute);
				DocumentXPathNavigator.DeleteAttribute(xmlAttribute, this.attributeIndex);
				if (xmlNode2 != null)
				{
					this.ResetPosition(xmlNode2);
					return;
				}
				return;
			}
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				this.CalibrateText();
				xmlNode = this.source;
				end = this.TextEnd(xmlNode);
				break;
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentType:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
				goto IL_AF;
			default:
				goto IL_AF;
			}
			xmlNode2 = DocumentXPathNavigator.OwnerNode(xmlNode);
			DocumentXPathNavigator.DeleteToFollowingSibling(xmlNode, end);
			if (xmlNode2 != null)
			{
				this.ResetPosition(xmlNode2);
				return;
			}
			return;
			IL_AF:
			throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0003468C File Offset: 0x0003368C
		private static void DeleteAttribute(XmlAttribute attribute, int index)
		{
			XmlAttributeCollection xmlAttributeCollection;
			if (!DocumentXPathNavigator.CheckAttributePosition(attribute, out xmlAttributeCollection, index) && !DocumentXPathNavigator.ResetAttributePosition(attribute, xmlAttributeCollection, out index))
			{
				throw new InvalidOperationException(Res.GetString("Xpn_MissingParent"));
			}
			if (attribute.IsReadOnly)
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Node_Modify_ReadOnly"));
			}
			xmlAttributeCollection.RemoveAt(index);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x000346E0 File Offset: 0x000336E0
		internal static void DeleteToFollowingSibling(XmlNode node, XmlNode end)
		{
			XmlNode parentNode = node.ParentNode;
			if (parentNode == null)
			{
				throw new InvalidOperationException(Res.GetString("Xpn_MissingParent"));
			}
			if (node.IsReadOnly || end.IsReadOnly)
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Node_Modify_ReadOnly"));
			}
			while (node != end)
			{
				XmlNode oldChild = node;
				node = node.NextSibling;
				parentNode.RemoveChild(oldChild);
			}
			parentNode.RemoveChild(node);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00034748 File Offset: 0x00033748
		private static XmlNamespaceManager GetNamespaceManager(XmlNode node, XmlDocument document)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(document.NameTable);
			List<XmlElement> list = new List<XmlElement>();
			while (node != null)
			{
				XmlElement xmlElement = node as XmlElement;
				if (xmlElement != null && xmlElement.HasAttributes)
				{
					list.Add(xmlElement);
				}
				node = node.ParentNode;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				xmlNamespaceManager.PushScope();
				XmlAttributeCollection attributes = list[i].Attributes;
				for (int j = 0; j < attributes.Count; j++)
				{
					XmlAttribute xmlAttribute = attributes[j];
					if (xmlAttribute.IsNamespace)
					{
						string prefix = (xmlAttribute.Prefix.Length == 0) ? string.Empty : xmlAttribute.LocalName;
						xmlNamespaceManager.AddNamespace(prefix, xmlAttribute.Value);
					}
				}
			}
			return xmlNamespaceManager;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0003480C File Offset: 0x0003380C
		internal void ResetPosition(XmlNode node)
		{
			this.source = node;
			XmlAttribute xmlAttribute = node as XmlAttribute;
			if (xmlAttribute != null)
			{
				XmlElement ownerElement = xmlAttribute.OwnerElement;
				if (ownerElement != null)
				{
					DocumentXPathNavigator.ResetAttributePosition(xmlAttribute, ownerElement.Attributes, out this.attributeIndex);
					if (xmlAttribute.IsNamespace)
					{
						this.namespaceParent = ownerElement;
					}
				}
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00034858 File Offset: 0x00033858
		private static bool ResetAttributePosition(XmlAttribute attribute, XmlAttributeCollection attributes, out int index)
		{
			if (attributes != null)
			{
				for (int i = 0; i < attributes.Count; i++)
				{
					if (attribute == attributes[i])
					{
						index = i;
						return true;
					}
				}
			}
			index = 0;
			return false;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003488C File Offset: 0x0003388C
		private static bool CheckAttributePosition(XmlAttribute attribute, out XmlAttributeCollection attributes, int index)
		{
			XmlElement ownerElement = attribute.OwnerElement;
			if (ownerElement != null)
			{
				attributes = ownerElement.Attributes;
				if (index >= 0 && index < attributes.Count && attribute == attributes[index])
				{
					return true;
				}
			}
			else
			{
				attributes = null;
			}
			return false;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000348CC File Offset: 0x000338CC
		private void CalibrateText()
		{
			for (XmlNode node = this.PreviousText(this.source); node != null; node = this.PreviousText(node))
			{
				this.ResetPosition(node);
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x000348FC File Offset: 0x000338FC
		private XmlNode ParentNode(XmlNode node)
		{
			XmlNode parentNode = node.ParentNode;
			if (!this.document.HasEntityReferences)
			{
				return parentNode;
			}
			return this.ParentNodeTail(parentNode);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00034926 File Offset: 0x00033926
		private XmlNode ParentNodeTail(XmlNode parent)
		{
			while (parent != null && parent.NodeType == XmlNodeType.EntityReference)
			{
				parent = parent.ParentNode;
			}
			return parent;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00034940 File Offset: 0x00033940
		private XmlNode FirstChild(XmlNode node)
		{
			XmlNode firstChild = node.FirstChild;
			if (!this.document.HasEntityReferences)
			{
				return firstChild;
			}
			return this.FirstChildTail(firstChild);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0003496A File Offset: 0x0003396A
		private XmlNode FirstChildTail(XmlNode child)
		{
			while (child != null && child.NodeType == XmlNodeType.EntityReference)
			{
				child = child.FirstChild;
			}
			return child;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00034984 File Offset: 0x00033984
		private XmlNode NextSibling(XmlNode node)
		{
			XmlNode nextSibling = node.NextSibling;
			if (!this.document.HasEntityReferences)
			{
				return nextSibling;
			}
			return this.NextSiblingTail(node, nextSibling);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x000349AF File Offset: 0x000339AF
		private XmlNode NextSiblingTail(XmlNode node, XmlNode sibling)
		{
			while (sibling == null)
			{
				node = node.ParentNode;
				if (node == null || node.NodeType != XmlNodeType.EntityReference)
				{
					return null;
				}
				sibling = node.NextSibling;
			}
			while (sibling != null && sibling.NodeType == XmlNodeType.EntityReference)
			{
				sibling = sibling.FirstChild;
			}
			return sibling;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x000349EC File Offset: 0x000339EC
		private XmlNode PreviousSibling(XmlNode node)
		{
			XmlNode previousSibling = node.PreviousSibling;
			if (!this.document.HasEntityReferences)
			{
				return previousSibling;
			}
			return this.PreviousSiblingTail(node, previousSibling);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00034A17 File Offset: 0x00033A17
		private XmlNode PreviousSiblingTail(XmlNode node, XmlNode sibling)
		{
			while (sibling == null)
			{
				node = node.ParentNode;
				if (node == null || node.NodeType != XmlNodeType.EntityReference)
				{
					return null;
				}
				sibling = node.PreviousSibling;
			}
			while (sibling != null && sibling.NodeType == XmlNodeType.EntityReference)
			{
				sibling = sibling.LastChild;
			}
			return sibling;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00034A54 File Offset: 0x00033A54
		private XmlNode PreviousText(XmlNode node)
		{
			XmlNode previousText = node.PreviousText;
			if (!this.document.HasEntityReferences)
			{
				return previousText;
			}
			return this.PreviousTextTail(node, previousText);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00034A80 File Offset: 0x00033A80
		private XmlNode PreviousTextTail(XmlNode node, XmlNode text)
		{
			if (text != null)
			{
				return text;
			}
			if (!node.IsText)
			{
				return null;
			}
			XmlNode xmlNode;
			for (xmlNode = node.PreviousSibling; xmlNode == null; xmlNode = node.PreviousSibling)
			{
				node = node.ParentNode;
				if (node == null || node.NodeType != XmlNodeType.EntityReference)
				{
					return null;
				}
			}
			while (xmlNode != null)
			{
				XmlNodeType nodeType = xmlNode.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					break;
				case XmlNodeType.EntityReference:
					xmlNode = xmlNode.LastChild;
					continue;
				default:
					switch (nodeType)
					{
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						break;
					default:
						return null;
					}
					break;
				}
				return xmlNode;
			}
			return null;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00034B06 File Offset: 0x00033B06
		internal static bool IsFollowingSibling(XmlNode left, XmlNode right)
		{
			do
			{
				left = left.NextSibling;
				if (left == null)
				{
					return false;
				}
			}
			while (left != right);
			return true;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00034B1C File Offset: 0x00033B1C
		private static bool IsDescendant(XmlNode top, XmlNode bottom)
		{
			do
			{
				XmlNode xmlNode = bottom.ParentNode;
				if (xmlNode == null)
				{
					XmlAttribute xmlAttribute = bottom as XmlAttribute;
					if (xmlAttribute == null)
					{
						return false;
					}
					xmlNode = xmlAttribute.OwnerElement;
					if (xmlNode == null)
					{
						return false;
					}
				}
				bottom = xmlNode;
			}
			while (top != bottom);
			return true;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00034B54 File Offset: 0x00033B54
		private static bool IsValidChild(XmlNode parent, XmlNode child)
		{
			XmlNodeType nodeType = parent.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				switch (nodeType)
				{
				case XmlNodeType.Document:
				{
					XmlNodeType nodeType2 = child.NodeType;
					if (nodeType2 != XmlNodeType.Element)
					{
						switch (nodeType2)
						{
						case XmlNodeType.ProcessingInstruction:
						case XmlNodeType.Comment:
							break;
						default:
							return false;
						}
					}
					return true;
				}
				case XmlNodeType.DocumentFragment:
				{
					XmlNodeType nodeType3 = child.NodeType;
					switch (nodeType3)
					{
					case XmlNodeType.Element:
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
					case XmlNodeType.ProcessingInstruction:
					case XmlNodeType.Comment:
						break;
					case XmlNodeType.Attribute:
					case XmlNodeType.EntityReference:
					case XmlNodeType.Entity:
						return false;
					default:
						switch (nodeType3)
						{
						case XmlNodeType.Whitespace:
						case XmlNodeType.SignificantWhitespace:
							break;
						default:
							return false;
						}
						break;
					}
					return true;
				}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00034BEC File Offset: 0x00033BEC
		private XmlNode TextStart(XmlNode node)
		{
			XmlNode result;
			do
			{
				result = node;
				node = this.PreviousSibling(node);
			}
			while (node != null && node.IsText);
			return result;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00034C10 File Offset: 0x00033C10
		private XmlNode TextEnd(XmlNode node)
		{
			XmlNode result;
			do
			{
				result = node;
				node = this.NextSibling(node);
			}
			while (node != null && node.IsText);
			return result;
		}

		// Token: 0x040008D6 RID: 2262
		private XmlDocument document;

		// Token: 0x040008D7 RID: 2263
		private XmlNode source;

		// Token: 0x040008D8 RID: 2264
		private int attributeIndex;

		// Token: 0x040008D9 RID: 2265
		private XmlElement namespaceParent;
	}
}
