using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Schema;
using MS.Internal.Xml.XPath;

namespace System.Xml.XPath
{
	// Token: 0x020000B9 RID: 185
	[DebuggerDisplay("{debuggerDisplayProxy}")]
	public abstract class XPathNavigator : XPathItem, ICloneable, IXPathNavigable, IXmlNamespaceResolver
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x00030A5B File Offset: 0x0002FA5B
		public override string ToString()
		{
			return this.Value;
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00030A63 File Offset: 0x0002FA63
		public sealed override bool IsNode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00030A68 File Offset: 0x0002FA68
		public override XmlSchemaType XmlType
		{
			get
			{
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo == null || schemaInfo.Validity != XmlSchemaValidity.Valid)
				{
					return null;
				}
				XmlSchemaType memberType = schemaInfo.MemberType;
				if (memberType != null)
				{
					return memberType;
				}
				return schemaInfo.SchemaType;
			}
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00030A9C File Offset: 0x0002FA9C
		public virtual void SetValue(string value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00030AA4 File Offset: 0x0002FAA4
		public override object TypedValue
		{
			get
			{
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					if (schemaInfo.Validity == XmlSchemaValidity.Valid)
					{
						XmlSchemaType xmlSchemaType = schemaInfo.MemberType;
						if (xmlSchemaType == null)
						{
							xmlSchemaType = schemaInfo.SchemaType;
						}
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return xmlSchemaType.ValueConverter.ChangeType(this.Value, datatype.ValueType, this);
							}
						}
					}
					else
					{
						XmlSchemaType xmlSchemaType = schemaInfo.SchemaType;
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return xmlSchemaType.ValueConverter.ChangeType(datatype.ParseValue(this.Value, this.NameTable, this), datatype.ValueType, this);
							}
						}
					}
				}
				return this.Value;
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00030B3C File Offset: 0x0002FB3C
		public virtual void SetTypedValue(object typedValue)
		{
			if (typedValue == null)
			{
				throw new ArgumentNullException("typedValue");
			}
			switch (this.NodeType)
			{
			case XPathNodeType.Element:
			case XPathNodeType.Attribute:
			{
				string text = null;
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					XmlSchemaType schemaType = schemaInfo.SchemaType;
					if (schemaType != null)
					{
						text = schemaType.ValueConverter.ToString(typedValue, this);
						XmlSchemaDatatype datatype = schemaType.Datatype;
						if (datatype != null)
						{
							datatype.ParseValue(text, this.NameTable, this);
						}
					}
				}
				if (text == null)
				{
					text = XmlUntypedConverter.Untyped.ToString(typedValue, this);
				}
				this.SetValue(text);
				return;
			}
			default:
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00030BD4 File Offset: 0x0002FBD4
		public override Type ValueType
		{
			get
			{
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					if (schemaInfo.Validity == XmlSchemaValidity.Valid)
					{
						XmlSchemaType xmlSchemaType = schemaInfo.MemberType;
						if (xmlSchemaType == null)
						{
							xmlSchemaType = schemaInfo.SchemaType;
						}
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return datatype.ValueType;
							}
						}
					}
					else
					{
						XmlSchemaType xmlSchemaType = schemaInfo.SchemaType;
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return datatype.ValueType;
							}
						}
					}
				}
				return typeof(string);
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00030C40 File Offset: 0x0002FC40
		public override bool ValueAsBoolean
		{
			get
			{
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					if (schemaInfo.Validity == XmlSchemaValidity.Valid)
					{
						XmlSchemaType xmlSchemaType = schemaInfo.MemberType;
						if (xmlSchemaType == null)
						{
							xmlSchemaType = schemaInfo.SchemaType;
						}
						if (xmlSchemaType != null)
						{
							return xmlSchemaType.ValueConverter.ToBoolean(this.Value);
						}
					}
					else
					{
						XmlSchemaType xmlSchemaType = schemaInfo.SchemaType;
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return xmlSchemaType.ValueConverter.ToBoolean(datatype.ParseValue(this.Value, this.NameTable, this));
							}
						}
					}
				}
				return XmlUntypedConverter.Untyped.ToBoolean(this.Value);
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00030CCC File Offset: 0x0002FCCC
		public override DateTime ValueAsDateTime
		{
			get
			{
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					if (schemaInfo.Validity == XmlSchemaValidity.Valid)
					{
						XmlSchemaType xmlSchemaType = schemaInfo.MemberType;
						if (xmlSchemaType == null)
						{
							xmlSchemaType = schemaInfo.SchemaType;
						}
						if (xmlSchemaType != null)
						{
							return xmlSchemaType.ValueConverter.ToDateTime(this.Value);
						}
					}
					else
					{
						XmlSchemaType xmlSchemaType = schemaInfo.SchemaType;
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return xmlSchemaType.ValueConverter.ToDateTime(datatype.ParseValue(this.Value, this.NameTable, this));
							}
						}
					}
				}
				return XmlUntypedConverter.Untyped.ToDateTime(this.Value);
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x00030D58 File Offset: 0x0002FD58
		public override double ValueAsDouble
		{
			get
			{
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					if (schemaInfo.Validity == XmlSchemaValidity.Valid)
					{
						XmlSchemaType xmlSchemaType = schemaInfo.MemberType;
						if (xmlSchemaType == null)
						{
							xmlSchemaType = schemaInfo.SchemaType;
						}
						if (xmlSchemaType != null)
						{
							return xmlSchemaType.ValueConverter.ToDouble(this.Value);
						}
					}
					else
					{
						XmlSchemaType xmlSchemaType = schemaInfo.SchemaType;
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return xmlSchemaType.ValueConverter.ToDouble(datatype.ParseValue(this.Value, this.NameTable, this));
							}
						}
					}
				}
				return XmlUntypedConverter.Untyped.ToDouble(this.Value);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x00030DE4 File Offset: 0x0002FDE4
		public override int ValueAsInt
		{
			get
			{
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					if (schemaInfo.Validity == XmlSchemaValidity.Valid)
					{
						XmlSchemaType xmlSchemaType = schemaInfo.MemberType;
						if (xmlSchemaType == null)
						{
							xmlSchemaType = schemaInfo.SchemaType;
						}
						if (xmlSchemaType != null)
						{
							return xmlSchemaType.ValueConverter.ToInt32(this.Value);
						}
					}
					else
					{
						XmlSchemaType xmlSchemaType = schemaInfo.SchemaType;
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return xmlSchemaType.ValueConverter.ToInt32(datatype.ParseValue(this.Value, this.NameTable, this));
							}
						}
					}
				}
				return XmlUntypedConverter.Untyped.ToInt32(this.Value);
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00030E70 File Offset: 0x0002FE70
		public override long ValueAsLong
		{
			get
			{
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					if (schemaInfo.Validity == XmlSchemaValidity.Valid)
					{
						XmlSchemaType xmlSchemaType = schemaInfo.MemberType;
						if (xmlSchemaType == null)
						{
							xmlSchemaType = schemaInfo.SchemaType;
						}
						if (xmlSchemaType != null)
						{
							return xmlSchemaType.ValueConverter.ToInt64(this.Value);
						}
					}
					else
					{
						XmlSchemaType xmlSchemaType = schemaInfo.SchemaType;
						if (xmlSchemaType != null)
						{
							XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
							if (datatype != null)
							{
								return xmlSchemaType.ValueConverter.ToInt64(datatype.ParseValue(this.Value, this.NameTable, this));
							}
						}
					}
				}
				return XmlUntypedConverter.Untyped.ToInt64(this.Value);
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00030EFC File Offset: 0x0002FEFC
		public override object ValueAs(Type returnType, IXmlNamespaceResolver nsResolver)
		{
			if (nsResolver == null)
			{
				nsResolver = this;
			}
			IXmlSchemaInfo schemaInfo = this.SchemaInfo;
			if (schemaInfo != null)
			{
				if (schemaInfo.Validity == XmlSchemaValidity.Valid)
				{
					XmlSchemaType xmlSchemaType = schemaInfo.MemberType;
					if (xmlSchemaType == null)
					{
						xmlSchemaType = schemaInfo.SchemaType;
					}
					if (xmlSchemaType != null)
					{
						return xmlSchemaType.ValueConverter.ChangeType(this.Value, returnType, nsResolver);
					}
				}
				else
				{
					XmlSchemaType xmlSchemaType = schemaInfo.SchemaType;
					if (xmlSchemaType != null)
					{
						XmlSchemaDatatype datatype = xmlSchemaType.Datatype;
						if (datatype != null)
						{
							return xmlSchemaType.ValueConverter.ChangeType(datatype.ParseValue(this.Value, this.NameTable, nsResolver), returnType, nsResolver);
						}
					}
				}
			}
			return XmlUntypedConverter.Untyped.ChangeType(this.Value, returnType, nsResolver);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00030F91 File Offset: 0x0002FF91
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00030F99 File Offset: 0x0002FF99
		public virtual XPathNavigator CreateNavigator()
		{
			return this.Clone();
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000A7A RID: 2682
		public abstract XmlNameTable NameTable { get; }

		// Token: 0x06000A7B RID: 2683 RVA: 0x00030FA4 File Offset: 0x0002FFA4
		public virtual string LookupNamespace(string prefix)
		{
			if (prefix == null)
			{
				return null;
			}
			if (this.NodeType != XPathNodeType.Element)
			{
				XPathNavigator xpathNavigator = this.Clone();
				if (xpathNavigator.MoveToParent())
				{
					return xpathNavigator.LookupNamespace(prefix);
				}
			}
			else if (this.MoveToNamespace(prefix))
			{
				string value = this.Value;
				this.MoveToParent();
				return value;
			}
			if (prefix.Length == 0)
			{
				return string.Empty;
			}
			if (prefix == "xml")
			{
				return "http://www.w3.org/XML/1998/namespace";
			}
			if (prefix == "xmlns")
			{
				return "http://www.w3.org/2000/xmlns/";
			}
			return null;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00031024 File Offset: 0x00030024
		public virtual string LookupPrefix(string namespaceURI)
		{
			if (namespaceURI == null)
			{
				return null;
			}
			XPathNavigator xpathNavigator = this.Clone();
			if (this.NodeType != XPathNodeType.Element)
			{
				if (xpathNavigator.MoveToParent())
				{
					return xpathNavigator.LookupPrefix(namespaceURI);
				}
			}
			else if (xpathNavigator.MoveToFirstNamespace(XPathNamespaceScope.All))
			{
				while (!(namespaceURI == xpathNavigator.Value))
				{
					if (!xpathNavigator.MoveToNextNamespace(XPathNamespaceScope.All))
					{
						goto IL_4C;
					}
				}
				return xpathNavigator.LocalName;
			}
			IL_4C:
			if (namespaceURI == this.LookupNamespace(string.Empty))
			{
				return string.Empty;
			}
			if (namespaceURI == "http://www.w3.org/XML/1998/namespace")
			{
				return "xml";
			}
			if (namespaceURI == "http://www.w3.org/2000/xmlns/")
			{
				return "xmlns";
			}
			return null;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x000310C0 File Offset: 0x000300C0
		public virtual IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			XPathNodeType nodeType = this.NodeType;
			if ((nodeType != XPathNodeType.Element && scope != XmlNamespaceScope.Local) || nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.Namespace)
			{
				XPathNavigator xpathNavigator = this.Clone();
				if (xpathNavigator.MoveToParent())
				{
					return xpathNavigator.GetNamespacesInScope(scope);
				}
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (scope == XmlNamespaceScope.All)
			{
				dictionary["xml"] = "http://www.w3.org/XML/1998/namespace";
			}
			if (this.MoveToFirstNamespace((XPathNamespaceScope)scope))
			{
				do
				{
					string localName = this.LocalName;
					string value = this.Value;
					if (localName.Length != 0 || value.Length != 0 || scope == XmlNamespaceScope.Local)
					{
						dictionary[localName] = value;
					}
				}
				while (this.MoveToNextNamespace((XPathNamespaceScope)scope));
				this.MoveToParent();
			}
			return dictionary;
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0003115B File Offset: 0x0003015B
		public static IEqualityComparer NavigatorComparer
		{
			get
			{
				return XPathNavigator.comparer;
			}
		}

		// Token: 0x06000A7F RID: 2687
		public abstract XPathNavigator Clone();

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000A80 RID: 2688
		public abstract XPathNodeType NodeType { get; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000A81 RID: 2689
		public abstract string LocalName { get; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000A82 RID: 2690
		public abstract string Name { get; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000A83 RID: 2691
		public abstract string NamespaceURI { get; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000A84 RID: 2692
		public abstract string Prefix { get; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000A85 RID: 2693
		public abstract string BaseURI { get; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000A86 RID: 2694
		public abstract bool IsEmptyElement { get; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00031164 File Offset: 0x00030164
		public virtual string XmlLang
		{
			get
			{
				XPathNavigator xpathNavigator = this.Clone();
				while (!xpathNavigator.MoveToAttribute("lang", "http://www.w3.org/XML/1998/namespace"))
				{
					if (!xpathNavigator.MoveToParent())
					{
						return string.Empty;
					}
				}
				return xpathNavigator.Value;
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x000311A0 File Offset: 0x000301A0
		public virtual XmlReader ReadSubtree()
		{
			switch (this.NodeType)
			{
			case XPathNodeType.Root:
			case XPathNodeType.Element:
				return this.CreateReader();
			default:
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			}
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x000311D8 File Offset: 0x000301D8
		public virtual void WriteSubtree(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteNode(this, true);
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x000311F0 File Offset: 0x000301F0
		public virtual object UnderlyingObject
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x000311F3 File Offset: 0x000301F3
		public virtual bool HasAttributes
		{
			get
			{
				if (!this.MoveToFirstAttribute())
				{
					return false;
				}
				this.MoveToParent();
				return true;
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00031208 File Offset: 0x00030208
		public virtual string GetAttribute(string localName, string namespaceURI)
		{
			if (!this.MoveToAttribute(localName, namespaceURI))
			{
				return "";
			}
			string value = this.Value;
			this.MoveToParent();
			return value;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00031234 File Offset: 0x00030234
		public virtual bool MoveToAttribute(string localName, string namespaceURI)
		{
			if (this.MoveToFirstAttribute())
			{
				while (!(localName == this.LocalName) || !(namespaceURI == this.NamespaceURI))
				{
					if (!this.MoveToNextAttribute())
					{
						this.MoveToParent();
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000A8E RID: 2702
		public abstract bool MoveToFirstAttribute();

		// Token: 0x06000A8F RID: 2703
		public abstract bool MoveToNextAttribute();

		// Token: 0x06000A90 RID: 2704 RVA: 0x0003126C File Offset: 0x0003026C
		public virtual string GetNamespace(string name)
		{
			if (this.MoveToNamespace(name))
			{
				string value = this.Value;
				this.MoveToParent();
				return value;
			}
			if (name == "xml")
			{
				return "http://www.w3.org/XML/1998/namespace";
			}
			if (name == "xmlns")
			{
				return "http://www.w3.org/2000/xmlns/";
			}
			return string.Empty;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x000312BD File Offset: 0x000302BD
		public virtual bool MoveToNamespace(string name)
		{
			if (this.MoveToFirstNamespace(XPathNamespaceScope.All))
			{
				while (!(name == this.LocalName))
				{
					if (!this.MoveToNextNamespace(XPathNamespaceScope.All))
					{
						this.MoveToParent();
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000A92 RID: 2706
		public abstract bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope);

		// Token: 0x06000A93 RID: 2707
		public abstract bool MoveToNextNamespace(XPathNamespaceScope namespaceScope);

		// Token: 0x06000A94 RID: 2708 RVA: 0x000312E9 File Offset: 0x000302E9
		public bool MoveToFirstNamespace()
		{
			return this.MoveToFirstNamespace(XPathNamespaceScope.All);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000312F2 File Offset: 0x000302F2
		public bool MoveToNextNamespace()
		{
			return this.MoveToNextNamespace(XPathNamespaceScope.All);
		}

		// Token: 0x06000A96 RID: 2710
		public abstract bool MoveToNext();

		// Token: 0x06000A97 RID: 2711
		public abstract bool MoveToPrevious();

		// Token: 0x06000A98 RID: 2712 RVA: 0x000312FC File Offset: 0x000302FC
		public virtual bool MoveToFirst()
		{
			switch (this.NodeType)
			{
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
				return false;
			default:
				return this.MoveToParent() && this.MoveToFirstChild();
			}
		}

		// Token: 0x06000A99 RID: 2713
		public abstract bool MoveToFirstChild();

		// Token: 0x06000A9A RID: 2714
		public abstract bool MoveToParent();

		// Token: 0x06000A9B RID: 2715 RVA: 0x00031334 File Offset: 0x00030334
		public virtual void MoveToRoot()
		{
			while (this.MoveToParent())
			{
			}
		}

		// Token: 0x06000A9C RID: 2716
		public abstract bool MoveTo(XPathNavigator other);

		// Token: 0x06000A9D RID: 2717
		public abstract bool MoveToId(string id);

		// Token: 0x06000A9E RID: 2718 RVA: 0x00031340 File Offset: 0x00030340
		public virtual bool MoveToChild(string localName, string namespaceURI)
		{
			if (this.MoveToFirstChild())
			{
				while (this.NodeType != XPathNodeType.Element || !(localName == this.LocalName) || !(namespaceURI == this.NamespaceURI))
				{
					if (!this.MoveToNext())
					{
						this.MoveToParent();
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0003138C File Offset: 0x0003038C
		public virtual bool MoveToChild(XPathNodeType type)
		{
			if (this.MoveToFirstChild())
			{
				int contentKindMask = XPathNavigator.GetContentKindMask(type);
				while ((1 << (int)this.NodeType & contentKindMask) == 0)
				{
					if (!this.MoveToNext())
					{
						this.MoveToParent();
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000313C9 File Offset: 0x000303C9
		public virtual bool MoveToFollowing(string localName, string namespaceURI)
		{
			return this.MoveToFollowing(localName, namespaceURI, null);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000313D4 File Offset: 0x000303D4
		public virtual bool MoveToFollowing(string localName, string namespaceURI, XPathNavigator end)
		{
			XPathNavigator other = this.Clone();
			if (end != null)
			{
				switch (end.NodeType)
				{
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
					end = end.Clone();
					end.MoveToNonDescendant();
					break;
				}
			}
			switch (this.NodeType)
			{
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
				if (!this.MoveToParent())
				{
					return false;
				}
				break;
			}
			for (;;)
			{
				if (!this.MoveToFirstChild())
				{
					while (!this.MoveToNext())
					{
						if (!this.MoveToParent())
						{
							goto Block_6;
						}
					}
				}
				if (end != null && this.IsSamePosition(end))
				{
					goto Block_8;
				}
				if (this.NodeType == XPathNodeType.Element && !(localName != this.LocalName) && !(namespaceURI != this.NamespaceURI))
				{
					return true;
				}
			}
			Block_6:
			this.MoveTo(other);
			return false;
			Block_8:
			this.MoveTo(other);
			return false;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00031494 File Offset: 0x00030494
		public virtual bool MoveToFollowing(XPathNodeType type)
		{
			return this.MoveToFollowing(type, null);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000314A0 File Offset: 0x000304A0
		public virtual bool MoveToFollowing(XPathNodeType type, XPathNavigator end)
		{
			XPathNavigator other = this.Clone();
			int contentKindMask = XPathNavigator.GetContentKindMask(type);
			if (end != null)
			{
				switch (end.NodeType)
				{
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
					end = end.Clone();
					end.MoveToNonDescendant();
					break;
				}
			}
			switch (this.NodeType)
			{
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
				if (!this.MoveToParent())
				{
					return false;
				}
				break;
			}
			for (;;)
			{
				if (!this.MoveToFirstChild())
				{
					while (!this.MoveToNext())
					{
						if (!this.MoveToParent())
						{
							goto Block_6;
						}
					}
				}
				if (end != null && this.IsSamePosition(end))
				{
					goto Block_8;
				}
				if ((1 << (int)this.NodeType & contentKindMask) != 0)
				{
					return true;
				}
			}
			Block_6:
			this.MoveTo(other);
			return false;
			Block_8:
			this.MoveTo(other);
			return false;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00031554 File Offset: 0x00030554
		public virtual bool MoveToNext(string localName, string namespaceURI)
		{
			XPathNavigator other = this.Clone();
			while (this.MoveToNext())
			{
				if (this.NodeType == XPathNodeType.Element && localName == this.LocalName && namespaceURI == this.NamespaceURI)
				{
					return true;
				}
			}
			this.MoveTo(other);
			return false;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000315A4 File Offset: 0x000305A4
		public virtual bool MoveToNext(XPathNodeType type)
		{
			XPathNavigator other = this.Clone();
			int contentKindMask = XPathNavigator.GetContentKindMask(type);
			while (this.MoveToNext())
			{
				if ((1 << (int)this.NodeType & contentKindMask) != 0)
				{
					return true;
				}
			}
			this.MoveTo(other);
			return false;
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x000315E3 File Offset: 0x000305E3
		public virtual bool HasChildren
		{
			get
			{
				if (this.MoveToFirstChild())
				{
					this.MoveToParent();
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000AA7 RID: 2727
		public abstract bool IsSamePosition(XPathNavigator other);

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000315F7 File Offset: 0x000305F7
		public virtual bool IsDescendant(XPathNavigator nav)
		{
			if (nav != null)
			{
				nav = nav.Clone();
				while (nav.MoveToParent())
				{
					if (nav.IsSamePosition(this))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0003161C File Offset: 0x0003061C
		public virtual XmlNodeOrder ComparePosition(XPathNavigator nav)
		{
			if (nav == null)
			{
				return XmlNodeOrder.Unknown;
			}
			if (this.IsSamePosition(nav))
			{
				return XmlNodeOrder.Same;
			}
			XPathNavigator xpathNavigator = this.Clone();
			XPathNavigator xpathNavigator2 = nav.Clone();
			int i = XPathNavigator.GetDepth(xpathNavigator.Clone());
			int j = XPathNavigator.GetDepth(xpathNavigator2.Clone());
			if (i > j)
			{
				while (i > j)
				{
					xpathNavigator.MoveToParent();
					i--;
				}
				if (xpathNavigator.IsSamePosition(xpathNavigator2))
				{
					return XmlNodeOrder.After;
				}
			}
			if (j > i)
			{
				while (j > i)
				{
					xpathNavigator2.MoveToParent();
					j--;
				}
				if (xpathNavigator.IsSamePosition(xpathNavigator2))
				{
					return XmlNodeOrder.Before;
				}
			}
			XPathNavigator xpathNavigator3 = xpathNavigator.Clone();
			XPathNavigator xpathNavigator4 = xpathNavigator2.Clone();
			while (xpathNavigator3.MoveToParent() && xpathNavigator4.MoveToParent())
			{
				if (xpathNavigator3.IsSamePosition(xpathNavigator4))
				{
					xpathNavigator.GetType().ToString() != "Microsoft.VisualStudio.Modeling.StoreNavigator";
					return this.CompareSiblings(xpathNavigator, xpathNavigator2);
				}
				xpathNavigator.MoveToParent();
				xpathNavigator2.MoveToParent();
			}
			return XmlNodeOrder.Unknown;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x000316FC File Offset: 0x000306FC
		public virtual IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this as IXmlSchemaInfo;
			}
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00031704 File Offset: 0x00030704
		public virtual bool CheckValidity(XmlSchemaSet schemas, ValidationEventHandler validationEventHandler)
		{
			XmlSchemaType xmlSchemaType = null;
			XmlSchemaElement xmlSchemaElement = null;
			XmlSchemaAttribute xmlSchemaAttribute = null;
			switch (this.NodeType)
			{
			case XPathNodeType.Root:
				if (schemas == null)
				{
					throw new InvalidOperationException(Res.GetString("XPathDocument_MissingSchemas"));
				}
				xmlSchemaType = null;
				break;
			case XPathNodeType.Element:
			{
				if (schemas == null)
				{
					throw new InvalidOperationException(Res.GetString("XPathDocument_MissingSchemas"));
				}
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					xmlSchemaType = schemaInfo.SchemaType;
					xmlSchemaElement = schemaInfo.SchemaElement;
				}
				if (xmlSchemaType == null && xmlSchemaElement == null)
				{
					throw new InvalidOperationException(Res.GetString("XPathDocument_NotEnoughSchemaInfo", null));
				}
				break;
			}
			case XPathNodeType.Attribute:
			{
				if (schemas == null)
				{
					throw new InvalidOperationException(Res.GetString("XPathDocument_MissingSchemas"));
				}
				IXmlSchemaInfo schemaInfo = this.SchemaInfo;
				if (schemaInfo != null)
				{
					xmlSchemaType = schemaInfo.SchemaType;
					xmlSchemaAttribute = schemaInfo.SchemaAttribute;
				}
				if (xmlSchemaType == null && xmlSchemaAttribute == null)
				{
					throw new InvalidOperationException(Res.GetString("XPathDocument_NotEnoughSchemaInfo", null));
				}
				break;
			}
			default:
				throw new InvalidOperationException(Res.GetString("XPathDocument_ValidateInvalidNodeType", null));
			}
			XmlReader xmlReader = this.CreateReader();
			XPathNavigator.CheckValidityHelper checkValidityHelper = new XPathNavigator.CheckValidityHelper(validationEventHandler, xmlReader as XPathNavigatorReader);
			validationEventHandler = new ValidationEventHandler(checkValidityHelper.ValidationCallback);
			XmlReader validatingReader = this.GetValidatingReader(xmlReader, schemas, validationEventHandler, xmlSchemaType, xmlSchemaElement, xmlSchemaAttribute);
			while (validatingReader.Read())
			{
			}
			return checkValidityHelper.IsValid;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0003182C File Offset: 0x0003082C
		private XmlReader GetValidatingReader(XmlReader reader, XmlSchemaSet schemas, ValidationEventHandler validationEvent, XmlSchemaType schemaType, XmlSchemaElement schemaElement, XmlSchemaAttribute schemaAttribute)
		{
			if (schemaAttribute != null)
			{
				return schemaAttribute.Validate(reader, null, schemas, validationEvent);
			}
			if (schemaElement != null)
			{
				return schemaElement.Validate(reader, null, schemas, validationEvent);
			}
			if (schemaType != null)
			{
				return schemaType.Validate(reader, null, schemas, validationEvent);
			}
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			xmlReaderSettings.Schemas = schemas;
			xmlReaderSettings.ValidationEventHandler += validationEvent;
			return XmlReader.Create(reader, xmlReaderSettings);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00031892 File Offset: 0x00030892
		public virtual XPathExpression Compile(string xpath)
		{
			return XPathExpression.Compile(xpath);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0003189A File Offset: 0x0003089A
		public virtual XPathNavigator SelectSingleNode(string xpath)
		{
			return this.SelectSingleNode(XPathExpression.Compile(xpath));
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000318A8 File Offset: 0x000308A8
		public virtual XPathNavigator SelectSingleNode(string xpath, IXmlNamespaceResolver resolver)
		{
			return this.SelectSingleNode(XPathExpression.Compile(xpath, resolver));
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000318B8 File Offset: 0x000308B8
		public virtual XPathNavigator SelectSingleNode(XPathExpression expression)
		{
			XPathNodeIterator xpathNodeIterator = this.Select(expression);
			if (xpathNodeIterator.MoveNext())
			{
				return xpathNodeIterator.Current;
			}
			return null;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000318DD File Offset: 0x000308DD
		public virtual XPathNodeIterator Select(string xpath)
		{
			return this.Select(XPathExpression.Compile(xpath));
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000318EB File Offset: 0x000308EB
		public virtual XPathNodeIterator Select(string xpath, IXmlNamespaceResolver resolver)
		{
			return this.Select(XPathExpression.Compile(xpath, resolver));
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000318FC File Offset: 0x000308FC
		public virtual XPathNodeIterator Select(XPathExpression expr)
		{
			XPathNodeIterator xpathNodeIterator = this.Evaluate(expr) as XPathNodeIterator;
			if (xpathNodeIterator == null)
			{
				throw XPathException.Create("Xp_NodeSetExpected");
			}
			return xpathNodeIterator;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00031925 File Offset: 0x00030925
		public virtual object Evaluate(string xpath)
		{
			return this.Evaluate(XPathExpression.Compile(xpath), null);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00031934 File Offset: 0x00030934
		public virtual object Evaluate(string xpath, IXmlNamespaceResolver resolver)
		{
			return this.Evaluate(XPathExpression.Compile(xpath, resolver));
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00031943 File Offset: 0x00030943
		public virtual object Evaluate(XPathExpression expr)
		{
			return this.Evaluate(expr, null);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00031950 File Offset: 0x00030950
		public virtual object Evaluate(XPathExpression expr, XPathNodeIterator context)
		{
			CompiledXpathExpr compiledXpathExpr = expr as CompiledXpathExpr;
			if (compiledXpathExpr == null)
			{
				throw XPathException.Create("Xp_BadQueryObject");
			}
			Query query = Query.Clone(compiledXpathExpr.QueryTree);
			query.Reset();
			if (context == null)
			{
				context = new XPathSingletonIterator(this.Clone(), true);
			}
			object obj = query.Evaluate(context);
			if (obj is XPathNodeIterator)
			{
				return new XPathSelectionIterator(context.Current, query);
			}
			return obj;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000319B4 File Offset: 0x000309B4
		public virtual bool Matches(XPathExpression expr)
		{
			CompiledXpathExpr compiledXpathExpr = expr as CompiledXpathExpr;
			if (compiledXpathExpr == null)
			{
				throw XPathException.Create("Xp_BadQueryObject");
			}
			Query query = Query.Clone(compiledXpathExpr.QueryTree);
			bool result;
			try
			{
				result = (query.MatchNode(this) != null);
			}
			catch (XPathException)
			{
				throw XPathException.Create("Xp_InvalidPattern", compiledXpathExpr.Expression);
			}
			return result;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00031A18 File Offset: 0x00030A18
		public virtual bool Matches(string xpath)
		{
			return this.Matches(XPathNavigator.CompileMatchPattern(xpath));
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00031A26 File Offset: 0x00030A26
		public virtual XPathNodeIterator SelectChildren(XPathNodeType type)
		{
			return new XPathChildIterator(this.Clone(), type);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00031A34 File Offset: 0x00030A34
		public virtual XPathNodeIterator SelectChildren(string name, string namespaceURI)
		{
			return new XPathChildIterator(this.Clone(), name, namespaceURI);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00031A43 File Offset: 0x00030A43
		public virtual XPathNodeIterator SelectAncestors(XPathNodeType type, bool matchSelf)
		{
			return new XPathAncestorIterator(this.Clone(), type, matchSelf);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00031A52 File Offset: 0x00030A52
		public virtual XPathNodeIterator SelectAncestors(string name, string namespaceURI, bool matchSelf)
		{
			return new XPathAncestorIterator(this.Clone(), name, namespaceURI, matchSelf);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00031A62 File Offset: 0x00030A62
		public virtual XPathNodeIterator SelectDescendants(XPathNodeType type, bool matchSelf)
		{
			return new XPathDescendantIterator(this.Clone(), type, matchSelf);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00031A71 File Offset: 0x00030A71
		public virtual XPathNodeIterator SelectDescendants(string name, string namespaceURI, bool matchSelf)
		{
			return new XPathDescendantIterator(this.Clone(), name, namespaceURI, matchSelf);
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00031A81 File Offset: 0x00030A81
		public virtual bool CanEdit
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00031A84 File Offset: 0x00030A84
		public virtual XmlWriter PrependChild()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00031A8B File Offset: 0x00030A8B
		public virtual XmlWriter AppendChild()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00031A92 File Offset: 0x00030A92
		public virtual XmlWriter InsertAfter()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00031A99 File Offset: 0x00030A99
		public virtual XmlWriter InsertBefore()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00031AA0 File Offset: 0x00030AA0
		public virtual XmlWriter CreateAttributes()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00031AA7 File Offset: 0x00030AA7
		public virtual XmlWriter ReplaceRange(XPathNavigator lastSiblingToReplace)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00031AB0 File Offset: 0x00030AB0
		public virtual void ReplaceSelf(string newNode)
		{
			XmlReader newNode2 = this.CreateContextReader(newNode, false);
			this.ReplaceSelf(newNode2);
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00031AD0 File Offset: 0x00030AD0
		public virtual void ReplaceSelf(XmlReader newNode)
		{
			if (newNode == null)
			{
				throw new ArgumentNullException("newNode");
			}
			XPathNodeType nodeType = this.NodeType;
			if (nodeType == XPathNodeType.Root || nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.Namespace)
			{
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			}
			XmlWriter xmlWriter = this.ReplaceRange(this);
			this.BuildSubtree(newNode, xmlWriter);
			xmlWriter.Close();
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00031B24 File Offset: 0x00030B24
		public virtual void ReplaceSelf(XPathNavigator newNode)
		{
			if (newNode == null)
			{
				throw new ArgumentNullException("newNode");
			}
			XmlReader newNode2 = newNode.CreateReader();
			this.ReplaceSelf(newNode2);
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00031B50 File Offset: 0x00030B50
		// (set) Token: 0x06000ACB RID: 2763 RVA: 0x00031C3C File Offset: 0x00030C3C
		public virtual string OuterXml
		{
			get
			{
				if (this.NodeType == XPathNodeType.Attribute)
				{
					return this.Name + "=\"" + this.Value + "\"";
				}
				if (this.NodeType != XPathNodeType.Namespace)
				{
					StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
					XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
					{
						Indent = true,
						OmitXmlDeclaration = true,
						ConformanceLevel = ConformanceLevel.Auto
					});
					try
					{
						xmlWriter.WriteNode(this, true);
					}
					finally
					{
						xmlWriter.Close();
					}
					return stringWriter.ToString();
				}
				if (this.LocalName.Length == 0)
				{
					return "xmlns=\"" + this.Value + "\"";
				}
				return string.Concat(new string[]
				{
					"xmlns:",
					this.LocalName,
					"=\"",
					this.Value,
					"\""
				});
			}
			set
			{
				this.ReplaceSelf(value);
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x00031C48 File Offset: 0x00030C48
		// (set) Token: 0x06000ACD RID: 2765 RVA: 0x00031CF0 File Offset: 0x00030CF0
		public virtual string InnerXml
		{
			get
			{
				switch (this.NodeType)
				{
				case XPathNodeType.Root:
				case XPathNodeType.Element:
				{
					StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
					XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
					{
						Indent = true,
						OmitXmlDeclaration = true,
						ConformanceLevel = ConformanceLevel.Auto
					});
					try
					{
						if (this.MoveToFirstChild())
						{
							do
							{
								xmlWriter.WriteNode(this, true);
							}
							while (this.MoveToNext());
							this.MoveToParent();
						}
					}
					finally
					{
						xmlWriter.Close();
					}
					return stringWriter.ToString();
				}
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
					return this.Value;
				default:
					return string.Empty;
				}
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				switch (this.NodeType)
				{
				case XPathNodeType.Root:
				case XPathNodeType.Element:
				{
					XPathNavigator xpathNavigator = this.CreateNavigator();
					while (xpathNavigator.MoveToFirstChild())
					{
						xpathNavigator.DeleteSelf();
					}
					if (value.Length != 0)
					{
						xpathNavigator.AppendChild(value);
						return;
					}
					return;
				}
				case XPathNodeType.Attribute:
					this.SetValue(value);
					return;
				default:
					throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
				}
			}
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00031D68 File Offset: 0x00030D68
		public virtual void AppendChild(string newChild)
		{
			XmlReader newChild2 = this.CreateContextReader(newChild, true);
			this.AppendChild(newChild2);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00031D88 File Offset: 0x00030D88
		public virtual void AppendChild(XmlReader newChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			XmlWriter xmlWriter = this.AppendChild();
			this.BuildSubtree(newChild, xmlWriter);
			xmlWriter.Close();
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00031DB8 File Offset: 0x00030DB8
		public virtual void AppendChild(XPathNavigator newChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			if (!this.IsValidChildType(newChild.NodeType))
			{
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			}
			XmlReader newChild2 = newChild.CreateReader();
			this.AppendChild(newChild2);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00031E00 File Offset: 0x00030E00
		public virtual void PrependChild(string newChild)
		{
			XmlReader newChild2 = this.CreateContextReader(newChild, true);
			this.PrependChild(newChild2);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00031E20 File Offset: 0x00030E20
		public virtual void PrependChild(XmlReader newChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			XmlWriter xmlWriter = this.PrependChild();
			this.BuildSubtree(newChild, xmlWriter);
			xmlWriter.Close();
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00031E50 File Offset: 0x00030E50
		public virtual void PrependChild(XPathNavigator newChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			if (!this.IsValidChildType(newChild.NodeType))
			{
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			}
			XmlReader newChild2 = newChild.CreateReader();
			this.PrependChild(newChild2);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00031E98 File Offset: 0x00030E98
		public virtual void InsertBefore(string newSibling)
		{
			XmlReader newSibling2 = this.CreateContextReader(newSibling, false);
			this.InsertBefore(newSibling2);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00031EB8 File Offset: 0x00030EB8
		public virtual void InsertBefore(XmlReader newSibling)
		{
			if (newSibling == null)
			{
				throw new ArgumentNullException("newSibling");
			}
			XmlWriter xmlWriter = this.InsertBefore();
			this.BuildSubtree(newSibling, xmlWriter);
			xmlWriter.Close();
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00031EE8 File Offset: 0x00030EE8
		public virtual void InsertBefore(XPathNavigator newSibling)
		{
			if (newSibling == null)
			{
				throw new ArgumentNullException("newSibling");
			}
			if (!this.IsValidSiblingType(newSibling.NodeType))
			{
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			}
			XmlReader newSibling2 = newSibling.CreateReader();
			this.InsertBefore(newSibling2);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00031F30 File Offset: 0x00030F30
		public virtual void InsertAfter(string newSibling)
		{
			XmlReader newSibling2 = this.CreateContextReader(newSibling, false);
			this.InsertAfter(newSibling2);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00031F50 File Offset: 0x00030F50
		public virtual void InsertAfter(XmlReader newSibling)
		{
			if (newSibling == null)
			{
				throw new ArgumentNullException("newSibling");
			}
			XmlWriter xmlWriter = this.InsertAfter();
			this.BuildSubtree(newSibling, xmlWriter);
			xmlWriter.Close();
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00031F80 File Offset: 0x00030F80
		public virtual void InsertAfter(XPathNavigator newSibling)
		{
			if (newSibling == null)
			{
				throw new ArgumentNullException("newSibling");
			}
			if (!this.IsValidSiblingType(newSibling.NodeType))
			{
				throw new InvalidOperationException(Res.GetString("Xpn_BadPosition"));
			}
			XmlReader newSibling2 = newSibling.CreateReader();
			this.InsertAfter(newSibling2);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00031FC7 File Offset: 0x00030FC7
		public virtual void DeleteRange(XPathNavigator lastSiblingToDelete)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00031FCE File Offset: 0x00030FCE
		public virtual void DeleteSelf()
		{
			this.DeleteRange(this);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00031FD8 File Offset: 0x00030FD8
		public virtual void PrependChildElement(string prefix, string localName, string namespaceURI, string value)
		{
			XmlWriter xmlWriter = this.PrependChild();
			xmlWriter.WriteStartElement(prefix, localName, namespaceURI);
			if (value != null)
			{
				xmlWriter.WriteString(value);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00032010 File Offset: 0x00031010
		public virtual void AppendChildElement(string prefix, string localName, string namespaceURI, string value)
		{
			XmlWriter xmlWriter = this.AppendChild();
			xmlWriter.WriteStartElement(prefix, localName, namespaceURI);
			if (value != null)
			{
				xmlWriter.WriteString(value);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00032048 File Offset: 0x00031048
		public virtual void InsertElementBefore(string prefix, string localName, string namespaceURI, string value)
		{
			XmlWriter xmlWriter = this.InsertBefore();
			xmlWriter.WriteStartElement(prefix, localName, namespaceURI);
			if (value != null)
			{
				xmlWriter.WriteString(value);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00032080 File Offset: 0x00031080
		public virtual void InsertElementAfter(string prefix, string localName, string namespaceURI, string value)
		{
			XmlWriter xmlWriter = this.InsertAfter();
			xmlWriter.WriteStartElement(prefix, localName, namespaceURI);
			if (value != null)
			{
				xmlWriter.WriteString(value);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000320B8 File Offset: 0x000310B8
		public virtual void CreateAttribute(string prefix, string localName, string namespaceURI, string value)
		{
			XmlWriter xmlWriter = this.CreateAttributes();
			xmlWriter.WriteStartAttribute(prefix, localName, namespaceURI);
			if (value != null)
			{
				xmlWriter.WriteString(value);
			}
			xmlWriter.WriteEndAttribute();
			xmlWriter.Close();
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000320F0 File Offset: 0x000310F0
		internal bool MoveToPrevious(string localName, string namespaceURI)
		{
			XPathNavigator other = this.Clone();
			localName = ((localName != null) ? this.NameTable.Get(localName) : null);
			while (this.MoveToPrevious())
			{
				if (this.NodeType == XPathNodeType.Element && localName == this.LocalName && namespaceURI == this.NamespaceURI)
				{
					return true;
				}
			}
			this.MoveTo(other);
			return false;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00032150 File Offset: 0x00031150
		internal bool MoveToPrevious(XPathNodeType type)
		{
			XPathNavigator other = this.Clone();
			int contentKindMask = XPathNavigator.GetContentKindMask(type);
			while (this.MoveToPrevious())
			{
				if ((1 << (int)this.NodeType & contentKindMask) != 0)
				{
					return true;
				}
			}
			this.MoveTo(other);
			return false;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00032190 File Offset: 0x00031190
		internal bool MoveToNonDescendant()
		{
			if (this.NodeType == XPathNodeType.Root)
			{
				return false;
			}
			if (this.MoveToNext())
			{
				return true;
			}
			XPathNavigator xpathNavigator = this.Clone();
			if (!this.MoveToParent())
			{
				return false;
			}
			switch (xpathNavigator.NodeType)
			{
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
				if (this.MoveToFirstChild())
				{
					return true;
				}
				break;
			}
			while (!this.MoveToNext())
			{
				if (!this.MoveToParent())
				{
					this.MoveTo(xpathNavigator);
					return false;
				}
			}
			return true;
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00032200 File Offset: 0x00031200
		internal uint IndexInParent
		{
			get
			{
				XPathNavigator xpathNavigator = this.Clone();
				uint num = 0U;
				switch (this.NodeType)
				{
				case XPathNodeType.Attribute:
					while (xpathNavigator.MoveToNextAttribute())
					{
						num += 1U;
					}
					break;
				case XPathNodeType.Namespace:
					while (xpathNavigator.MoveToNextNamespace())
					{
						num += 1U;
					}
					break;
				default:
					while (xpathNavigator.MoveToNext())
					{
						num += 1U;
					}
					break;
				}
				return num;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00032258 File Offset: 0x00031258
		internal virtual string UniqueId
		{
			get
			{
				XPathNavigator xpathNavigator = this.Clone();
				BufferBuilder bufferBuilder = new BufferBuilder();
				bufferBuilder.Append(XPathNavigator.NodeTypeLetter[(int)this.NodeType]);
				for (;;)
				{
					uint num = xpathNavigator.IndexInParent;
					if (!xpathNavigator.MoveToParent())
					{
						break;
					}
					if (num <= 31U)
					{
						bufferBuilder.Append(XPathNavigator.UniqueIdTbl[(int)((UIntPtr)num)]);
					}
					else
					{
						bufferBuilder.Append('0');
						do
						{
							bufferBuilder.Append(XPathNavigator.UniqueIdTbl[(int)((UIntPtr)(num & 31U))]);
							num >>= 5;
						}
						while (num != 0U);
						bufferBuilder.Append('0');
					}
				}
				return bufferBuilder.ToString();
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x000322D8 File Offset: 0x000312D8
		private static XPathExpression CompileMatchPattern(string xpath)
		{
			bool needContext;
			Query query = new QueryBuilder().BuildPatternQuery(xpath, out needContext);
			return new CompiledXpathExpr(query, xpath, needContext);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x000322FC File Offset: 0x000312FC
		private static int GetDepth(XPathNavigator nav)
		{
			int num = 0;
			while (nav.MoveToParent())
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0003231C File Offset: 0x0003131C
		private XmlNodeOrder CompareSiblings(XPathNavigator n1, XPathNavigator n2)
		{
			int num = 0;
			switch (n1.NodeType)
			{
			case XPathNodeType.Attribute:
				num++;
				break;
			case XPathNodeType.Namespace:
				break;
			default:
				num += 2;
				break;
			}
			switch (n2.NodeType)
			{
			case XPathNodeType.Attribute:
				num--;
				if (num == 0)
				{
					while (n1.MoveToNextAttribute())
					{
						if (n1.IsSamePosition(n2))
						{
							return XmlNodeOrder.Before;
						}
					}
				}
				break;
			case XPathNodeType.Namespace:
				if (num == 0)
				{
					while (n1.MoveToNextNamespace())
					{
						if (n1.IsSamePosition(n2))
						{
							return XmlNodeOrder.Before;
						}
					}
				}
				break;
			default:
				num -= 2;
				if (num == 0)
				{
					while (n1.MoveToNext())
					{
						if (n1.IsSamePosition(n2))
						{
							return XmlNodeOrder.Before;
						}
					}
				}
				break;
			}
			if (num >= 0)
			{
				return XmlNodeOrder.After;
			}
			return XmlNodeOrder.Before;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000323C4 File Offset: 0x000313C4
		internal static XmlNamespaceManager GetNamespaces(IXmlNamespaceResolver resolver)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
			IDictionary<string, string> namespacesInScope = resolver.GetNamespacesInScope(XmlNamespaceScope.All);
			foreach (KeyValuePair<string, string> keyValuePair in namespacesInScope)
			{
				if (keyValuePair.Key != "xmlns")
				{
					xmlNamespaceManager.AddNamespace(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return xmlNamespaceManager;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00032440 File Offset: 0x00031440
		internal static int GetContentKindMask(XPathNodeType type)
		{
			return XPathNavigator.ContentKindMasks[(int)type];
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00032449 File Offset: 0x00031449
		internal static int GetKindMask(XPathNodeType type)
		{
			if (type == XPathNodeType.All)
			{
				return int.MaxValue;
			}
			if (type == XPathNodeType.Text)
			{
				return 112;
			}
			return 1 << (int)type;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00032463 File Offset: 0x00031463
		internal static bool IsText(XPathNodeType type)
		{
			return (1 << (int)type & 112) != 0;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00032474 File Offset: 0x00031474
		private bool IsValidChildType(XPathNodeType type)
		{
			switch (this.NodeType)
			{
			case XPathNodeType.Root:
				switch (type)
				{
				case XPathNodeType.Element:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					return true;
				}
				break;
			case XPathNodeType.Element:
				switch (type)
				{
				case XPathNodeType.Element:
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x000324F8 File Offset: 0x000314F8
		private bool IsValidSiblingType(XPathNodeType type)
		{
			switch (this.NodeType)
			{
			case XPathNodeType.Element:
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				switch (type)
				{
				case XPathNodeType.Element:
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00032565 File Offset: 0x00031565
		private XmlReader CreateReader()
		{
			return XPathNavigatorReader.Create(this);
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00032570 File Offset: 0x00031570
		private XmlReader CreateContextReader(string xml, bool fromCurrentNode)
		{
			if (xml == null)
			{
				throw new ArgumentNullException("xml");
			}
			XPathNavigator xpathNavigator = this.CreateNavigator();
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(this.NameTable);
			if (!fromCurrentNode)
			{
				xpathNavigator.MoveToParent();
			}
			if (xpathNavigator.MoveToFirstNamespace(XPathNamespaceScope.All))
			{
				do
				{
					xmlNamespaceManager.AddNamespace(xpathNavigator.LocalName, xpathNavigator.Value);
				}
				while (xpathNavigator.MoveToNextNamespace(XPathNamespaceScope.All));
			}
			XmlParserContext context = new XmlParserContext(this.NameTable, xmlNamespaceManager, null, XmlSpace.Default);
			return new XmlTextReader(xml, XmlNodeType.Element, context)
			{
				WhitespaceHandling = WhitespaceHandling.Significant
			};
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000325EC File Offset: 0x000315EC
		internal void BuildSubtree(XmlReader reader, XmlWriter writer)
		{
			string text = "http://www.w3.org/2000/xmlns/";
			ReadState readState = reader.ReadState;
			if (readState != ReadState.Initial && readState != ReadState.Interactive)
			{
				throw new ArgumentException(Res.GetString("Xml_InvalidOperation"), "reader");
			}
			int num = 0;
			if (readState == ReadState.Initial)
			{
				if (!reader.Read())
				{
					return;
				}
				num++;
			}
			do
			{
				switch (reader.NodeType)
				{
				case XmlNodeType.Element:
				{
					writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
					bool isEmptyElement = reader.IsEmptyElement;
					while (reader.MoveToNextAttribute())
					{
						if (reader.NamespaceURI == text)
						{
							if (reader.Prefix.Length == 0)
							{
								writer.WriteAttributeString("", "xmlns", text, reader.Value);
							}
							else
							{
								writer.WriteAttributeString("xmlns", reader.LocalName, text, reader.Value);
							}
						}
						else
						{
							writer.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
							writer.WriteString(reader.Value);
							writer.WriteEndAttribute();
						}
					}
					reader.MoveToElement();
					if (isEmptyElement)
					{
						writer.WriteEndElement();
					}
					else
					{
						num++;
					}
					break;
				}
				case XmlNodeType.Attribute:
					if (reader.NamespaceURI == text)
					{
						if (reader.Prefix.Length == 0)
						{
							writer.WriteAttributeString("", "xmlns", text, reader.Value);
						}
						else
						{
							writer.WriteAttributeString("xmlns", reader.LocalName, text, reader.Value);
						}
					}
					else
					{
						writer.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
						writer.WriteString(reader.Value);
						writer.WriteEndAttribute();
					}
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					writer.WriteString(reader.Value);
					break;
				case XmlNodeType.EntityReference:
					reader.ResolveEntity();
					break;
				case XmlNodeType.ProcessingInstruction:
					writer.WriteProcessingInstruction(reader.LocalName, reader.Value);
					break;
				case XmlNodeType.Comment:
					writer.WriteComment(reader.Value);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					writer.WriteString(reader.Value);
					break;
				case XmlNodeType.EndElement:
					writer.WriteFullEndElement();
					num--;
					break;
				}
			}
			while (reader.Read() && num > 0);
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0003282A File Offset: 0x0003182A
		private object debuggerDisplayProxy
		{
			get
			{
				return new XPathNavigator.DebuggerDisplayProxy(this);
			}
		}

		// Token: 0x040008CB RID: 2251
		internal const int AllMask = 2147483647;

		// Token: 0x040008CC RID: 2252
		internal const int NoAttrNmspMask = 2147483635;

		// Token: 0x040008CD RID: 2253
		internal const int TextMask = 112;

		// Token: 0x040008CE RID: 2254
		internal static readonly XPathNavigatorKeyComparer comparer = new XPathNavigatorKeyComparer();

		// Token: 0x040008CF RID: 2255
		internal static readonly char[] NodeTypeLetter = new char[]
		{
			'R',
			'E',
			'A',
			'N',
			'T',
			'S',
			'W',
			'P',
			'C',
			'X'
		};

		// Token: 0x040008D0 RID: 2256
		internal static readonly char[] UniqueIdTbl = new char[]
		{
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6'
		};

		// Token: 0x040008D1 RID: 2257
		internal static readonly int[] ContentKindMasks = new int[]
		{
			1,
			2,
			0,
			0,
			112,
			32,
			64,
			128,
			256,
			2147483635
		};

		// Token: 0x020000BA RID: 186
		private class CheckValidityHelper
		{
			// Token: 0x06000AF5 RID: 2805 RVA: 0x0003291C File Offset: 0x0003191C
			internal CheckValidityHelper(ValidationEventHandler nextEventHandler, XPathNavigatorReader reader)
			{
				this.isValid = true;
				this.nextEventHandler = nextEventHandler;
				this.reader = reader;
			}

			// Token: 0x06000AF6 RID: 2806 RVA: 0x0003293C File Offset: 0x0003193C
			internal void ValidationCallback(object sender, ValidationEventArgs args)
			{
				if (args.Severity == XmlSeverityType.Error)
				{
					this.isValid = false;
				}
				XmlSchemaValidationException ex = args.Exception as XmlSchemaValidationException;
				if (ex != null && this.reader != null)
				{
					ex.SetSourceObject(this.reader.UnderlyingObject);
				}
				if (this.nextEventHandler != null)
				{
					this.nextEventHandler(sender, args);
					return;
				}
				if (ex != null && args.Severity == XmlSeverityType.Error)
				{
					throw ex;
				}
			}

			// Token: 0x17000261 RID: 609
			// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x000329A3 File Offset: 0x000319A3
			internal bool IsValid
			{
				get
				{
					return this.isValid;
				}
			}

			// Token: 0x040008D2 RID: 2258
			private bool isValid;

			// Token: 0x040008D3 RID: 2259
			private ValidationEventHandler nextEventHandler;

			// Token: 0x040008D4 RID: 2260
			private XPathNavigatorReader reader;
		}

		// Token: 0x020000BB RID: 187
		[DebuggerDisplay("{ToString()}")]
		internal struct DebuggerDisplayProxy
		{
			// Token: 0x06000AF8 RID: 2808 RVA: 0x000329AB File Offset: 0x000319AB
			public DebuggerDisplayProxy(XPathNavigator nav)
			{
				this.nav = nav;
			}

			// Token: 0x06000AF9 RID: 2809 RVA: 0x000329B4 File Offset: 0x000319B4
			public override string ToString()
			{
				string text = this.nav.NodeType.ToString();
				switch (this.nav.NodeType)
				{
				case XPathNodeType.Element:
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						", Name=\"",
						this.nav.Name,
						'"'
					});
					break;
				}
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
				case XPathNodeType.ProcessingInstruction:
				{
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						", Name=\"",
						this.nav.Name,
						'"'
					});
					object obj3 = text;
					text = string.Concat(new object[]
					{
						obj3,
						", Value=\"",
						XmlConvert.EscapeValueForDebuggerDisplay(this.nav.Value),
						'"'
					});
					break;
				}
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.Comment:
				{
					object obj4 = text;
					text = string.Concat(new object[]
					{
						obj4,
						", Value=\"",
						XmlConvert.EscapeValueForDebuggerDisplay(this.nav.Value),
						'"'
					});
					break;
				}
				}
				return text;
			}

			// Token: 0x040008D5 RID: 2261
			private XPathNavigator nav;
		}
	}
}
