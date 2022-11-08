using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xml.Schema;
using System.Xml.XPath;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B5 RID: 181
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XmlQueryRuntime
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x0002A038 File Offset: 0x00029038
		internal XmlQueryRuntime(XmlQueryStaticData data, object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, XmlSequenceWriter seqWrt)
		{
			string[] names = data.Names;
			Int32Pair[] array = data.Filters;
			WhitespaceRuleLookup wsRules = (data.WhitespaceRules != null && data.WhitespaceRules.Count != 0) ? new WhitespaceRuleLookup(data.WhitespaceRules) : null;
			this.ctxt = new XmlQueryContext(this, defaultDataSource, dataSources, argList, wsRules);
			this.xsltLib = null;
			this.earlyInfo = data.EarlyBound;
			this.earlyObjects = ((this.earlyInfo != null) ? new object[this.earlyInfo.Length] : null);
			this.globalNames = data.GlobalNames;
			this.globalValues = ((this.globalNames != null) ? new object[this.globalNames.Length] : null);
			this.nameTableQuery = this.ctxt.QueryNameTable;
			this.atomizedNames = null;
			if (names != null)
			{
				XmlNameTable defaultNameTable = this.ctxt.DefaultNameTable;
				this.atomizedNames = new string[names.Length];
				if (defaultNameTable != this.nameTableQuery && defaultNameTable != null)
				{
					for (int i = 0; i < names.Length; i++)
					{
						string text = defaultNameTable.Get(names[i]);
						this.atomizedNames[i] = this.nameTableQuery.Add(text ?? names[i]);
					}
				}
				else
				{
					for (int i = 0; i < names.Length; i++)
					{
						this.atomizedNames[i] = this.nameTableQuery.Add(names[i]);
					}
				}
			}
			this.filters = null;
			if (array != null)
			{
				this.filters = new XmlNavigatorFilter[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					this.filters[i] = XmlNavNameFilter.Create(this.atomizedNames[array[i].Left], this.atomizedNames[array[i].Right]);
				}
			}
			this.prefixMappingsList = data.PrefixMappingsList;
			this.types = data.Types;
			this.collations = data.Collations;
			this.docOrderCmp = new DocumentOrderComparer();
			this.indexes = null;
			this.stkOutput = new Stack<XmlQueryOutput>(16);
			this.output = new XmlQueryOutput(this, seqWrt);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002A236 File Offset: 0x00029236
		public string[] DebugGetGlobalNames()
		{
			return this.globalNames;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002A240 File Offset: 0x00029240
		public IList DebugGetGlobalValue(string name)
		{
			for (int i = 0; i < this.globalNames.Length; i++)
			{
				if (this.globalNames[i] == name)
				{
					return (IList)this.globalValues[i];
				}
			}
			return null;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002A280 File Offset: 0x00029280
		public void DebugSetGlobalValue(string name, object value)
		{
			for (int i = 0; i < this.globalNames.Length; i++)
			{
				if (this.globalNames[i] == name)
				{
					this.globalValues[i] = (IList<XPathItem>)XmlAnyListConverter.ItemList.ChangeType(value, typeof(XPathItem[]), null);
					return;
				}
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002A2D4 File Offset: 0x000292D4
		public object DebugGetXsltValue(IList seq)
		{
			if (seq != null && seq.Count == 1)
			{
				XPathItem xpathItem = seq[0] as XPathItem;
				if (xpathItem != null && !xpathItem.IsNode)
				{
					return xpathItem.TypedValue;
				}
				if (xpathItem is RtfNavigator)
				{
					return ((RtfNavigator)xpathItem).ToNavigator();
				}
			}
			return seq;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0002A321 File Offset: 0x00029321
		public XmlQueryContext ExternalContext
		{
			get
			{
				return this.ctxt;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0002A329 File Offset: 0x00029329
		public XsltLibrary XsltFunctions
		{
			get
			{
				if (this.xsltLib == null)
				{
					this.xsltLib = new XsltLibrary(this);
				}
				return this.xsltLib;
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0002A348 File Offset: 0x00029348
		public object GetEarlyBoundObject(int index)
		{
			object obj = this.earlyObjects[index];
			if (obj == null)
			{
				obj = this.earlyInfo[index].CreateObject();
				this.earlyObjects[index] = obj;
			}
			return obj;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0002A37C File Offset: 0x0002937C
		public bool EarlyBoundFunctionExists(string name, string namespaceUri)
		{
			if (this.earlyInfo == null)
			{
				return false;
			}
			for (int i = 0; i < this.earlyInfo.Length; i++)
			{
				if (namespaceUri == this.earlyInfo[i].NamespaceUri)
				{
					return new XmlExtensionFunction(name, namespaceUri, -1, this.earlyInfo[i].EarlyBoundType, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public).CanBind();
				}
			}
			return false;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0002A3D9 File Offset: 0x000293D9
		public bool IsGlobalComputed(int index)
		{
			return this.globalValues[index] != null;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0002A3E9 File Offset: 0x000293E9
		public object GetGlobalValue(int index)
		{
			return this.globalValues[index];
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0002A3F3 File Offset: 0x000293F3
		public void SetGlobalValue(int index, object value)
		{
			this.globalValues[index] = value;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0002A3FE File Offset: 0x000293FE
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTableQuery;
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0002A406 File Offset: 0x00029406
		public string GetAtomizedName(int index)
		{
			return this.atomizedNames[index];
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0002A410 File Offset: 0x00029410
		public XmlNavigatorFilter GetNameFilter(int index)
		{
			return this.filters[index];
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0002A41A File Offset: 0x0002941A
		public XmlNavigatorFilter GetTypeFilter(XPathNodeType nodeType)
		{
			if (nodeType == XPathNodeType.All)
			{
				return XmlNavNeverFilter.Create();
			}
			if (nodeType == XPathNodeType.Attribute)
			{
				return XmlNavAttrFilter.Create();
			}
			return XmlNavTypeFilter.Create(nodeType);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002A438 File Offset: 0x00029438
		public XmlQualifiedName ParseTagName(string tagName, int indexPrefixMappings)
		{
			string text;
			string name;
			string ns;
			this.ParseTagName(tagName, indexPrefixMappings, out text, out name, out ns);
			return new XmlQualifiedName(name, ns);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0002A45C File Offset: 0x0002945C
		public XmlQualifiedName ParseTagName(string tagName, string ns)
		{
			string text;
			string name;
			ValidateNames.ParseQNameThrow(tagName, out text, out name);
			return new XmlQualifiedName(name, ns);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002A47C File Offset: 0x0002947C
		internal void ParseTagName(string tagName, int idxPrefixMappings, out string prefix, out string localName, out string ns)
		{
			ValidateNames.ParseQNameThrow(tagName, out prefix, out localName);
			ns = null;
			foreach (StringPair stringPair in this.prefixMappingsList[idxPrefixMappings])
			{
				if (prefix == stringPair.Left)
				{
					ns = stringPair.Right;
					break;
				}
			}
			if (ns != null)
			{
				return;
			}
			if (prefix.Length == 0)
			{
				ns = "";
				return;
			}
			if (prefix.Equals("xml"))
			{
				ns = "http://www.w3.org/XML/1998/namespace";
				return;
			}
			if (prefix.Equals("xmlns"))
			{
				ns = "http://www.w3.org/2000/xmlns/";
				return;
			}
			throw new XslTransformException("Xslt_InvalidPrefix", new string[]
			{
				prefix
			});
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002A534 File Offset: 0x00029534
		public bool IsQNameEqual(XPathNavigator n1, XPathNavigator n2)
		{
			if (n1.NameTable == n2.NameTable)
			{
				return n1.LocalName == n2.LocalName && n1.NamespaceURI == n2.NamespaceURI;
			}
			return n1.LocalName == n2.LocalName && n1.NamespaceURI == n2.NamespaceURI;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0002A594 File Offset: 0x00029594
		public bool IsQNameEqual(XPathNavigator navigator, int indexLocalName, int indexNamespaceUri)
		{
			if (navigator.NameTable == this.nameTableQuery)
			{
				return this.GetAtomizedName(indexLocalName) == navigator.LocalName && this.GetAtomizedName(indexNamespaceUri) == navigator.NamespaceURI;
			}
			return this.GetAtomizedName(indexLocalName) == navigator.LocalName && this.GetAtomizedName(indexNamespaceUri) == navigator.NamespaceURI;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0002A5F8 File Offset: 0x000295F8
		internal XmlQueryType[] XmlTypes
		{
			get
			{
				return this.types;
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002A600 File Offset: 0x00029600
		internal XmlQueryType GetXmlType(int idxType)
		{
			return this.types[idxType];
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002A60A File Offset: 0x0002960A
		public object ChangeTypeXsltArgument(int indexType, object value, Type destinationType)
		{
			return this.ChangeTypeXsltArgument(this.GetXmlType(indexType), value, destinationType);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0002A61C File Offset: 0x0002961C
		internal object ChangeTypeXsltArgument(XmlQueryType xmlType, object value, Type destinationType)
		{
			XmlTypeCode typeCode = xmlType.TypeCode;
			switch (typeCode)
			{
			case XmlTypeCode.Item:
			{
				if (destinationType != XsltConvert.ObjectType)
				{
					throw new XslTransformException("Xslt_UnsupportedClrType", new string[]
					{
						destinationType.Name
					});
				}
				IList<XPathItem> list = (IList<XPathItem>)value;
				if (list.Count == 1)
				{
					XPathItem xpathItem = list[0];
					if (xpathItem.IsNode)
					{
						RtfNavigator rtfNavigator = xpathItem as RtfNavigator;
						if (rtfNavigator != null)
						{
							value = rtfNavigator.ToNavigator();
						}
						else
						{
							value = new XPathArrayIterator((IList)value);
						}
					}
					else
					{
						value = xpathItem.TypedValue;
					}
				}
				else
				{
					value = new XPathArrayIterator((IList)value);
				}
				break;
			}
			case XmlTypeCode.Node:
				if (destinationType == XsltConvert.XPathNodeIteratorType)
				{
					value = new XPathArrayIterator((IList)value);
				}
				else if (destinationType == XsltConvert.XPathNavigatorArrayType)
				{
					IList<XPathNavigator> list2 = (IList<XPathNavigator>)value;
					XPathNavigator[] array = new XPathNavigator[list2.Count];
					for (int i = 0; i < list2.Count; i++)
					{
						array[i] = list2[i];
					}
					value = array;
				}
				break;
			default:
				if (typeCode != XmlTypeCode.String)
				{
					if (typeCode == XmlTypeCode.Double)
					{
						if (destinationType != XsltConvert.DoubleType)
						{
							value = Convert.ChangeType(value, destinationType, CultureInfo.InvariantCulture);
						}
					}
				}
				else if (destinationType == XsltConvert.DateTimeType)
				{
					value = XsltConvert.ToDateTime((string)value);
				}
				break;
			}
			return value;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0002A777 File Offset: 0x00029777
		public object ChangeTypeXsltResult(int indexType, object value)
		{
			return this.ChangeTypeXsltResult(this.GetXmlType(indexType), value);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0002A788 File Offset: 0x00029788
		internal object ChangeTypeXsltResult(XmlQueryType xmlType, object value)
		{
			if (value == null)
			{
				throw new XslTransformException("Xslt_ItemNull", new string[]
				{
					string.Empty
				});
			}
			XmlTypeCode typeCode = xmlType.TypeCode;
			switch (typeCode)
			{
			case XmlTypeCode.Item:
			{
				Type type = value.GetType();
				XmlTypeCode typeCode2 = XsltConvert.InferXsltType(type).TypeCode;
				switch (typeCode2)
				{
				case XmlTypeCode.Item:
					if (value is XPathNodeIterator)
					{
						value = this.ChangeTypeXsltResult(XmlQueryTypeFactory.NodeS, value);
					}
					else
					{
						IXPathNavigable ixpathNavigable = value as IXPathNavigable;
						if (ixpathNavigable == null)
						{
							throw new XslTransformException("Xslt_UnsupportedClrType", new string[]
							{
								type.Name
							});
						}
						if (value is XPathNavigator)
						{
							value = new XmlQueryNodeSequence((XPathNavigator)value);
						}
						else
						{
							value = new XmlQueryNodeSequence(ixpathNavigable.CreateNavigator());
						}
					}
					break;
				case XmlTypeCode.Node:
					value = this.ChangeTypeXsltResult(XmlQueryTypeFactory.NodeS, value);
					break;
				default:
					switch (typeCode2)
					{
					case XmlTypeCode.String:
						if (type == XsltConvert.DateTimeType)
						{
							value = new XmlQueryItemSequence(new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), XsltConvert.ToString((DateTime)value)));
						}
						else
						{
							value = new XmlQueryItemSequence(new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String), value));
						}
						break;
					case XmlTypeCode.Boolean:
						value = new XmlQueryItemSequence(new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Boolean), value));
						break;
					case XmlTypeCode.Double:
						value = new XmlQueryItemSequence(new XmlAtomicValue(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Double), ((IConvertible)value).ToDouble(null)));
						break;
					}
					break;
				}
				break;
			}
			case XmlTypeCode.Node:
				if (!xmlType.IsSingleton)
				{
					XPathArrayIterator xpathArrayIterator = value as XPathArrayIterator;
					if (xpathArrayIterator != null && xpathArrayIterator.AsList is XmlQueryNodeSequence)
					{
						value = (xpathArrayIterator.AsList as XmlQueryNodeSequence);
					}
					else
					{
						XmlQueryNodeSequence xmlQueryNodeSequence = new XmlQueryNodeSequence();
						IList list = value as IList;
						if (list != null)
						{
							for (int i = 0; i < list.Count; i++)
							{
								xmlQueryNodeSequence.Add(XmlQueryRuntime.EnsureNavigator(list[i]));
							}
						}
						else
						{
							foreach (object value2 in ((IEnumerable)value))
							{
								xmlQueryNodeSequence.Add(XmlQueryRuntime.EnsureNavigator(value2));
							}
						}
						value = xmlQueryNodeSequence;
					}
					value = ((XmlQueryNodeSequence)value).DocOrderDistinct(this.docOrderCmp);
				}
				break;
			default:
				if (typeCode != XmlTypeCode.String)
				{
					if (typeCode == XmlTypeCode.Double)
					{
						if (value.GetType() != XsltConvert.DoubleType)
						{
							value = ((IConvertible)value).ToDouble(null);
						}
					}
				}
				else if (value.GetType() == XsltConvert.DateTimeType)
				{
					value = XsltConvert.ToString((DateTime)value);
				}
				break;
			}
			return value;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0002AA48 File Offset: 0x00029A48
		private static XPathNavigator EnsureNavigator(object value)
		{
			XPathNavigator xpathNavigator = value as XPathNavigator;
			if (xpathNavigator == null)
			{
				throw new XslTransformException("Xslt_ItemNull", new string[]
				{
					string.Empty
				});
			}
			return xpathNavigator;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0002AA7C File Offset: 0x00029A7C
		public bool MatchesXmlType(IList<XPathItem> seq, int indexType)
		{
			XmlQueryType xmlQueryType = this.GetXmlType(indexType);
			XmlQueryCardinality left;
			switch (seq.Count)
			{
			case 0:
				left = XmlQueryCardinality.Zero;
				break;
			case 1:
				left = XmlQueryCardinality.One;
				break;
			default:
				left = XmlQueryCardinality.More;
				break;
			}
			if (!(left <= xmlQueryType.Cardinality))
			{
				return false;
			}
			xmlQueryType = xmlQueryType.Prime;
			for (int i = 0; i < seq.Count; i++)
			{
				if (!this.CreateXmlType(seq[0]).IsSubtypeOf(xmlQueryType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0002AAFE File Offset: 0x00029AFE
		public bool MatchesXmlType(XPathItem item, int indexType)
		{
			return this.CreateXmlType(item).IsSubtypeOf(this.GetXmlType(indexType));
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0002AB13 File Offset: 0x00029B13
		public bool MatchesXmlType(IList<XPathItem> seq, XmlTypeCode code)
		{
			return seq.Count == 1 && this.MatchesXmlType(seq[0], code);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0002AB30 File Offset: 0x00029B30
		public bool MatchesXmlType(XPathItem item, XmlTypeCode code)
		{
			if (code > XmlTypeCode.AnyAtomicType)
			{
				return !item.IsNode && item.XmlType.TypeCode == code;
			}
			switch (code)
			{
			case XmlTypeCode.Item:
				return true;
			case XmlTypeCode.Node:
				return item.IsNode;
			default:
				if (code == XmlTypeCode.AnyAtomicType)
				{
					return !item.IsNode;
				}
				if (!item.IsNode)
				{
					return false;
				}
				switch (((XPathNavigator)item).NodeType)
				{
				case XPathNodeType.Root:
					return code == XmlTypeCode.Document;
				case XPathNodeType.Element:
					return code == XmlTypeCode.Element;
				case XPathNodeType.Attribute:
					return code == XmlTypeCode.Attribute;
				case XPathNodeType.Namespace:
					return code == XmlTypeCode.Namespace;
				case XPathNodeType.Text:
					return code == XmlTypeCode.Text;
				case XPathNodeType.SignificantWhitespace:
					return code == XmlTypeCode.Text;
				case XPathNodeType.Whitespace:
					return code == XmlTypeCode.Text;
				case XPathNodeType.ProcessingInstruction:
					return code == XmlTypeCode.ProcessingInstruction;
				case XPathNodeType.Comment:
					return code == XmlTypeCode.Comment;
				default:
					return false;
				}
				break;
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0002ABF8 File Offset: 0x00029BF8
		private XmlQueryType CreateXmlType(XPathItem item)
		{
			if (!item.IsNode)
			{
				return XmlQueryTypeFactory.Type((XmlSchemaSimpleType)item.XmlType, true);
			}
			RtfNavigator rtfNavigator = item as RtfNavigator;
			if (rtfNavigator != null)
			{
				return XmlQueryTypeFactory.Node;
			}
			XPathNavigator xpathNavigator = (XPathNavigator)item;
			switch (xpathNavigator.NodeType)
			{
			case XPathNodeType.Root:
			case XPathNodeType.Element:
				if (xpathNavigator.XmlType == null)
				{
					return XmlQueryTypeFactory.Type(xpathNavigator.NodeType, XmlQualifiedNameTest.New(xpathNavigator.LocalName, xpathNavigator.NamespaceURI), XmlSchemaComplexType.UntypedAnyType, false);
				}
				return XmlQueryTypeFactory.Type(xpathNavigator.NodeType, XmlQualifiedNameTest.New(xpathNavigator.LocalName, xpathNavigator.NamespaceURI), xpathNavigator.XmlType, xpathNavigator.SchemaInfo.SchemaElement.IsNillable);
			case XPathNodeType.Attribute:
				if (xpathNavigator.XmlType == null)
				{
					return XmlQueryTypeFactory.Type(xpathNavigator.NodeType, XmlQualifiedNameTest.New(xpathNavigator.LocalName, xpathNavigator.NamespaceURI), DatatypeImplementation.UntypedAtomicType, false);
				}
				return XmlQueryTypeFactory.Type(xpathNavigator.NodeType, XmlQualifiedNameTest.New(xpathNavigator.LocalName, xpathNavigator.NamespaceURI), xpathNavigator.XmlType, false);
			default:
				return XmlQueryTypeFactory.Type(xpathNavigator.NodeType, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.AnyType, false);
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002AD1A File Offset: 0x00029D1A
		public XmlCollation GetCollation(int index)
		{
			return this.collations[index];
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0002AD24 File Offset: 0x00029D24
		public XmlCollation CreateCollation(string collation)
		{
			return XmlCollation.Create(collation);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0002AD2C File Offset: 0x00029D2C
		public int ComparePosition(XPathNavigator navigatorThis, XPathNavigator navigatorThat)
		{
			return this.docOrderCmp.Compare(navigatorThis, navigatorThat);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002AD3C File Offset: 0x00029D3C
		public IList<XPathNavigator> DocOrderDistinct(IList<XPathNavigator> seq)
		{
			if (seq.Count <= 1)
			{
				return seq;
			}
			XmlQueryNodeSequence xmlQueryNodeSequence = (XmlQueryNodeSequence)seq;
			if (xmlQueryNodeSequence == null)
			{
				xmlQueryNodeSequence = new XmlQueryNodeSequence(seq);
			}
			return xmlQueryNodeSequence.DocOrderDistinct(this.docOrderCmp);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002AD74 File Offset: 0x00029D74
		public string GenerateId(XPathNavigator navigator)
		{
			return "ID" + this.docOrderCmp.GetDocumentIndex(navigator).ToString(CultureInfo.InvariantCulture) + navigator.UniqueId;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0002ADAC File Offset: 0x00029DAC
		public bool FindIndex(XPathNavigator context, int indexId, out XmlILIndex index)
		{
			XPathNavigator xpathNavigator = context.Clone();
			xpathNavigator.MoveToRoot();
			if (this.indexes != null && indexId < this.indexes.Length)
			{
				ArrayList arrayList = this.indexes[indexId];
				if (arrayList != null)
				{
					for (int i = 0; i < arrayList.Count; i += 2)
					{
						if (((XPathNavigator)arrayList[i]).IsSamePosition(xpathNavigator))
						{
							index = (XmlILIndex)arrayList[i + 1];
							return true;
						}
					}
				}
			}
			index = new XmlILIndex();
			return false;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002AE24 File Offset: 0x00029E24
		public void AddNewIndex(XPathNavigator context, int indexId, XmlILIndex index)
		{
			XPathNavigator xpathNavigator = context.Clone();
			xpathNavigator.MoveToRoot();
			if (this.indexes == null)
			{
				this.indexes = new ArrayList[indexId + 4];
			}
			else if (indexId >= this.indexes.Length)
			{
				ArrayList[] destinationArray = new ArrayList[indexId + 4];
				Array.Copy(this.indexes, 0, destinationArray, 0, this.indexes.Length);
				this.indexes = destinationArray;
			}
			ArrayList arrayList = this.indexes[indexId];
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				this.indexes[indexId] = arrayList;
			}
			arrayList.Add(xpathNavigator);
			arrayList.Add(index);
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0002AEB2 File Offset: 0x00029EB2
		public XmlQueryOutput Output
		{
			get
			{
				return this.output;
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0002AEBC File Offset: 0x00029EBC
		public void StartSequenceConstruction(out XmlQueryOutput output)
		{
			this.stkOutput.Push(this.output);
			output = (this.output = new XmlQueryOutput(this, new XmlCachedSequenceWriter()));
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0002AEF0 File Offset: 0x00029EF0
		public IList<XPathItem> EndSequenceConstruction(out XmlQueryOutput output)
		{
			IList<XPathItem> resultSequence = ((XmlCachedSequenceWriter)this.output.SequenceWriter).ResultSequence;
			output = (this.output = this.stkOutput.Pop());
			return resultSequence;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0002AF2C File Offset: 0x00029F2C
		public void StartRtfConstruction(string baseUri, out XmlQueryOutput output)
		{
			this.stkOutput.Push(this.output);
			output = (this.output = new XmlQueryOutput(this, new XmlEventCache(baseUri, true)));
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0002AF64 File Offset: 0x00029F64
		public XPathNavigator EndRtfConstruction(out XmlQueryOutput output)
		{
			XmlEventCache xmlEventCache = (XmlEventCache)this.output.Writer;
			output = (this.output = this.stkOutput.Pop());
			xmlEventCache.EndEvents();
			return new RtfTreeNavigator(xmlEventCache, this.nameTableQuery);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0002AFAA File Offset: 0x00029FAA
		public XPathNavigator TextRtfConstruction(string text, string baseUri)
		{
			return new RtfTextNavigator(text, baseUri);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002AFB3 File Offset: 0x00029FB3
		public void SendMessage(string message)
		{
			this.ctxt.OnXsltMessageEncountered(message);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0002AFC1 File Offset: 0x00029FC1
		public void ThrowException(string text)
		{
			throw new XslTransformException(text);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002AFC9 File Offset: 0x00029FC9
		internal static XPathNavigator SyncToNavigator(XPathNavigator navigatorThis, XPathNavigator navigatorThat)
		{
			if (navigatorThis == null || !navigatorThis.MoveTo(navigatorThat))
			{
				return navigatorThat.Clone();
			}
			return navigatorThis;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0002AFE0 File Offset: 0x00029FE0
		public static int OnCurrentNodeChanged(XPathNavigator currentNode)
		{
			IXmlLineInfo xmlLineInfo = currentNode as IXmlLineInfo;
			if (xmlLineInfo != null && (currentNode.NodeType != XPathNodeType.Namespace || !XmlQueryRuntime.IsInheritedNamespace(currentNode)))
			{
				XmlQueryRuntime.OnCurrentNodeChanged2(currentNode.BaseURI, xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
			}
			return 0;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0002B020 File Offset: 0x0002A020
		private static bool IsInheritedNamespace(XPathNavigator node)
		{
			XPathNavigator xpathNavigator = node.Clone();
			if (xpathNavigator.MoveToParent() && xpathNavigator.MoveToFirstNamespace(XPathNamespaceScope.Local))
			{
				while (xpathNavigator.LocalName != node.LocalName)
				{
					if (!xpathNavigator.MoveToNextNamespace(XPathNamespaceScope.Local))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0002B05F File Offset: 0x0002A05F
		private static void OnCurrentNodeChanged2(string baseUri, int lineNumber, int linePosition)
		{
		}

		// Token: 0x04000593 RID: 1427
		internal const BindingFlags EarlyBoundFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x04000594 RID: 1428
		internal const BindingFlags LateBoundFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x04000595 RID: 1429
		private XmlQueryContext ctxt;

		// Token: 0x04000596 RID: 1430
		private XsltLibrary xsltLib;

		// Token: 0x04000597 RID: 1431
		private EarlyBoundInfo[] earlyInfo;

		// Token: 0x04000598 RID: 1432
		private object[] earlyObjects;

		// Token: 0x04000599 RID: 1433
		private string[] globalNames;

		// Token: 0x0400059A RID: 1434
		private object[] globalValues;

		// Token: 0x0400059B RID: 1435
		private XmlNameTable nameTableQuery;

		// Token: 0x0400059C RID: 1436
		private string[] atomizedNames;

		// Token: 0x0400059D RID: 1437
		private XmlNavigatorFilter[] filters;

		// Token: 0x0400059E RID: 1438
		private StringPair[][] prefixMappingsList;

		// Token: 0x0400059F RID: 1439
		private XmlQueryType[] types;

		// Token: 0x040005A0 RID: 1440
		private XmlCollation[] collations;

		// Token: 0x040005A1 RID: 1441
		private DocumentOrderComparer docOrderCmp;

		// Token: 0x040005A2 RID: 1442
		private ArrayList[] indexes;

		// Token: 0x040005A3 RID: 1443
		private XmlQueryOutput output;

		// Token: 0x040005A4 RID: 1444
		private Stack<XmlQueryOutput> stkOutput;
	}
}
