using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Schema;
using System.Xml.XmlConfiguration;

namespace System.Xml
{
	// Token: 0x02000085 RID: 133
	internal class XmlTextReaderImpl : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
	{
		// Token: 0x06000675 RID: 1653 RVA: 0x0001986C File Offset: 0x0001886C
		internal XmlTextReaderImpl()
		{
			this.curNode = new XmlTextReaderImpl.NodeData();
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.NoData;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000198D8 File Offset: 0x000188D8
		internal XmlTextReaderImpl(XmlNameTable nt)
		{
			this.v1Compat = true;
			this.outerReader = this;
			this.nameTable = nt;
			nt.Add(string.Empty);
			this.xmlResolver = new XmlUrlResolver();
			this.Xml = nt.Add("xml");
			this.XmlNs = nt.Add("xmlns");
			this.nodes = new XmlTextReaderImpl.NodeData[8];
			this.nodes[0] = new XmlTextReaderImpl.NodeData();
			this.curNode = this.nodes[0];
			this.stringBuilder = new BufferBuilder();
			this.xmlContext = new XmlTextReaderImpl.XmlContext();
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractiveXmlDecl;
			this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
			this.entityHandling = EntityHandling.ExpandCharEntities;
			this.whitespaceHandling = WhitespaceHandling.All;
			this.closeInput = true;
			this.maxCharactersInDocument = 0L;
			this.maxCharactersFromEntities = 10000000L;
			this.charactersInDocument = 0L;
			this.charactersFromEntities = 0L;
			this.ps.lineNo = 1;
			this.ps.lineStartPos = -1;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00019A1C File Offset: 0x00018A1C
		private XmlTextReaderImpl(XmlResolver resolver, XmlReaderSettings settings, XmlParserContext context)
		{
			this.v1Compat = false;
			this.outerReader = this;
			this.xmlContext = new XmlTextReaderImpl.XmlContext();
			XmlNameTable xmlNameTable = settings.NameTable;
			if (context == null)
			{
				if (xmlNameTable == null)
				{
					xmlNameTable = new NameTable();
				}
				else
				{
					this.nameTableFromSettings = true;
				}
				this.nameTable = xmlNameTable;
				this.namespaceManager = new XmlNamespaceManager(xmlNameTable);
			}
			else
			{
				this.SetupFromParserContext(context, settings);
				xmlNameTable = this.nameTable;
			}
			xmlNameTable.Add(string.Empty);
			this.Xml = xmlNameTable.Add("xml");
			this.XmlNs = xmlNameTable.Add("xmlns");
			this.xmlResolver = resolver;
			this.nodes = new XmlTextReaderImpl.NodeData[8];
			this.nodes[0] = new XmlTextReaderImpl.NodeData();
			this.curNode = this.nodes[0];
			this.stringBuilder = new BufferBuilder();
			this.entityHandling = EntityHandling.ExpandEntities;
			this.xmlResolverIsSet = settings.IsXmlResolverSet;
			this.whitespaceHandling = (settings.IgnoreWhitespace ? WhitespaceHandling.Significant : WhitespaceHandling.All);
			this.normalize = true;
			this.ignorePIs = settings.IgnoreProcessingInstructions;
			this.ignoreComments = settings.IgnoreComments;
			this.checkCharacters = settings.CheckCharacters;
			this.lineNumberOffset = settings.LineNumberOffset;
			this.linePositionOffset = settings.LinePositionOffset;
			this.ps.lineNo = this.lineNumberOffset + 1;
			this.ps.lineStartPos = -this.linePositionOffset - 1;
			this.curNode.SetLineInfo(this.ps.LineNo - 1, this.ps.LinePos - 1);
			this.prohibitDtd = settings.ProhibitDtd;
			this.maxCharactersInDocument = settings.MaxCharactersInDocument;
			this.maxCharactersFromEntities = settings.MaxCharactersFromEntities;
			this.charactersInDocument = 0L;
			this.charactersFromEntities = 0L;
			this.fragmentParserContext = context;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractiveXmlDecl;
			this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
			switch (settings.ConformanceLevel)
			{
			case ConformanceLevel.Auto:
				this.fragmentType = XmlNodeType.None;
				this.fragment = true;
				return;
			case ConformanceLevel.Fragment:
				this.fragmentType = XmlNodeType.Element;
				this.fragment = true;
				return;
			}
			this.fragmentType = XmlNodeType.Document;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00019C72 File Offset: 0x00018C72
		internal XmlTextReaderImpl(Stream input) : this(string.Empty, input, new NameTable())
		{
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00019C85 File Offset: 0x00018C85
		internal XmlTextReaderImpl(Stream input, XmlNameTable nt) : this(string.Empty, input, nt)
		{
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00019C94 File Offset: 0x00018C94
		internal XmlTextReaderImpl(string url, Stream input) : this(url, input, new NameTable())
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00019CA4 File Offset: 0x00018CA4
		internal XmlTextReaderImpl(string url, Stream input, XmlNameTable nt) : this(nt)
		{
			this.namespaceManager = new XmlNamespaceManager(nt);
			if (url == null || url.Length == 0)
			{
				this.InitStreamInput(input, null);
			}
			else
			{
				this.InitStreamInput(url, input, null);
			}
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00019D04 File Offset: 0x00018D04
		internal XmlTextReaderImpl(TextReader input) : this(string.Empty, input, new NameTable())
		{
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00019D17 File Offset: 0x00018D17
		internal XmlTextReaderImpl(TextReader input, XmlNameTable nt) : this(string.Empty, input, nt)
		{
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00019D26 File Offset: 0x00018D26
		internal XmlTextReaderImpl(string url, TextReader input) : this(url, input, new NameTable())
		{
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00019D38 File Offset: 0x00018D38
		internal XmlTextReaderImpl(string url, TextReader input, XmlNameTable nt) : this(nt)
		{
			this.namespaceManager = new XmlNamespaceManager(nt);
			this.reportedBaseUri = ((url != null) ? url : string.Empty);
			this.InitTextReaderInput(this.reportedBaseUri, input);
			this.reportedEncoding = this.ps.encoding;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00019D88 File Offset: 0x00018D88
		internal XmlTextReaderImpl(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context) : this((context != null && context.NameTable != null) ? context.NameTable : new NameTable())
		{
			Encoding encoding = (context != null) ? context.Encoding : null;
			if (context == null || context.BaseURI == null || context.BaseURI.Length == 0)
			{
				this.InitStreamInput(xmlFragment, encoding);
			}
			else
			{
				this.InitStreamInput(this.xmlResolver.ResolveUri(null, context.BaseURI), xmlFragment, encoding);
			}
			this.InitFragmentReader(fragType, context, false);
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00019E28 File Offset: 0x00018E28
		internal XmlTextReaderImpl(string xmlFragment, XmlNodeType fragType, XmlParserContext context) : this((context == null || context.NameTable == null) ? new NameTable() : context.NameTable)
		{
			if (context == null)
			{
				this.InitStringInput(string.Empty, Encoding.Unicode, xmlFragment);
			}
			else
			{
				this.reportedBaseUri = context.BaseURI;
				this.InitStringInput(context.BaseURI, Encoding.Unicode, xmlFragment);
			}
			this.InitFragmentReader(fragType, context, false);
			this.reportedEncoding = this.ps.encoding;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00019EA4 File Offset: 0x00018EA4
		internal XmlTextReaderImpl(string xmlFragment, XmlParserContext context) : this((context == null || context.NameTable == null) ? new NameTable() : context.NameTable)
		{
			this.InitStringInput((context == null) ? string.Empty : context.BaseURI, Encoding.Unicode, "<?xml " + xmlFragment + "?>");
			this.InitFragmentReader(XmlNodeType.XmlDeclaration, context, true);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00019F04 File Offset: 0x00018F04
		public XmlTextReaderImpl(string url) : this(url, new NameTable())
		{
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00019F14 File Offset: 0x00018F14
		public XmlTextReaderImpl(string url, XmlNameTable nt) : this(nt)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (url.Length == 0)
			{
				throw new ArgumentException(Res.GetString("Xml_EmptyUrl"), "url");
			}
			this.namespaceManager = new XmlNamespaceManager(nt);
			this.compressedStack = CompressedStack.Capture();
			this.url = url;
			this.ps.baseUri = this.xmlResolver.ResolveUri(null, url);
			this.ps.baseUriStr = this.ps.baseUri.ToString();
			this.reportedBaseUri = this.ps.baseUriStr;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.OpenUrl;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00019FBC File Offset: 0x00018FBC
		internal XmlTextReaderImpl(Stream stream, byte[] bytes, int byteCount, XmlReaderSettings settings, Uri baseUri, string baseUriStr, XmlParserContext context, bool closeInput) : this(settings.GetXmlResolver(), settings, context)
		{
			Encoding encoding = null;
			if (context != null)
			{
				if (context.BaseURI != null && context.BaseURI.Length > 0 && !this.UriEqual(baseUri, baseUriStr, context.BaseURI, settings.GetXmlResolver()))
				{
					if (baseUriStr.Length > 0)
					{
						this.Throw("Xml_DoubleBaseUri");
					}
					baseUriStr = context.BaseURI;
				}
				encoding = context.Encoding;
			}
			this.InitStreamInput(baseUri, baseUriStr, stream, bytes, byteCount, encoding);
			this.closeInput = closeInput;
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
			if (context != null && context.HasDtdInfo)
			{
				if (this.prohibitDtd)
				{
					this.ThrowWithoutLineInfo("Xml_DtdIsProhibitedEx", string.Empty);
				}
				this.ParseDtdFromParserContext();
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001A09C File Offset: 0x0001909C
		internal XmlTextReaderImpl(TextReader input, XmlReaderSettings settings, string baseUriStr, XmlParserContext context) : this(settings.GetXmlResolver(), settings, context)
		{
			if (context != null && context.BaseURI != null)
			{
				baseUriStr = context.BaseURI;
			}
			this.InitTextReaderInput(baseUriStr, input);
			this.closeInput = settings.CloseInput;
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
			if (context != null && context.HasDtdInfo)
			{
				if (this.prohibitDtd)
				{
					this.ThrowWithoutLineInfo("Xml_DtdIsProhibitedEx", string.Empty);
				}
				this.ParseDtdFromParserContext();
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001A12F File Offset: 0x0001912F
		internal XmlTextReaderImpl(string xmlFragment, XmlParserContext context, XmlReaderSettings settings) : this(null, settings, context)
		{
			this.InitStringInput(string.Empty, Encoding.Unicode, xmlFragment);
			this.reportedBaseUri = this.ps.baseUriStr;
			this.reportedEncoding = this.ps.encoding;
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0001A170 File Offset: 0x00019170
		public override XmlReaderSettings Settings
		{
			get
			{
				if (this.v1Compat)
				{
					return null;
				}
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				if (this.nameTableFromSettings)
				{
					xmlReaderSettings.NameTable = this.nameTable;
				}
				XmlNodeType xmlNodeType = this.fragmentType;
				switch (xmlNodeType)
				{
				case XmlNodeType.None:
					break;
				case XmlNodeType.Element:
					xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
					goto IL_57;
				default:
					if (xmlNodeType == XmlNodeType.Document)
					{
						xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
						goto IL_57;
					}
					break;
				}
				xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
				IL_57:
				xmlReaderSettings.CheckCharacters = this.checkCharacters;
				xmlReaderSettings.LineNumberOffset = this.lineNumberOffset;
				xmlReaderSettings.LinePositionOffset = this.linePositionOffset;
				xmlReaderSettings.IgnoreWhitespace = (this.whitespaceHandling == WhitespaceHandling.Significant);
				xmlReaderSettings.IgnoreProcessingInstructions = this.ignorePIs;
				xmlReaderSettings.IgnoreComments = this.ignoreComments;
				xmlReaderSettings.ProhibitDtd = this.prohibitDtd;
				xmlReaderSettings.MaxCharactersInDocument = this.maxCharactersInDocument;
				xmlReaderSettings.MaxCharactersFromEntities = this.maxCharactersFromEntities;
				xmlReaderSettings.ReadOnly = true;
				return xmlReaderSettings;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001A24B File Offset: 0x0001924B
		public override XmlNodeType NodeType
		{
			get
			{
				return this.curNode.type;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001A258 File Offset: 0x00019258
		public override string Name
		{
			get
			{
				return this.curNode.GetNameWPrefix(this.nameTable);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001A26B File Offset: 0x0001926B
		public override string LocalName
		{
			get
			{
				return this.curNode.localName;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0001A278 File Offset: 0x00019278
		public override string NamespaceURI
		{
			get
			{
				return this.curNode.ns;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001A285 File Offset: 0x00019285
		public override string Prefix
		{
			get
			{
				return this.curNode.prefix;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0001A292 File Offset: 0x00019292
		public override bool HasValue
		{
			get
			{
				return XmlReader.HasValueInternal(this.curNode.type);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0001A2A4 File Offset: 0x000192A4
		public override string Value
		{
			get
			{
				if (this.parsingFunction >= XmlTextReaderImpl.ParsingFunction.PartialTextValue)
				{
					if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.PartialTextValue)
					{
						this.FinishPartialValue();
						this.parsingFunction = this.nextParsingFunction;
					}
					else
					{
						this.FinishOtherValueIterator();
					}
				}
				return this.curNode.StringValue;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x0001A2DF File Offset: 0x000192DF
		public override int Depth
		{
			get
			{
				return this.curNode.depth;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0001A2EC File Offset: 0x000192EC
		public override string BaseURI
		{
			get
			{
				return this.reportedBaseUri;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0001A2F4 File Offset: 0x000192F4
		public override bool IsEmptyElement
		{
			get
			{
				return this.curNode.IsEmptyElement;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0001A301 File Offset: 0x00019301
		public override bool IsDefault
		{
			get
			{
				return this.curNode.IsDefaultAttribute;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0001A30E File Offset: 0x0001930E
		public override char QuoteChar
		{
			get
			{
				if (this.curNode.type != XmlNodeType.Attribute)
				{
					return '"';
				}
				return this.curNode.quoteChar;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001A32C File Offset: 0x0001932C
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.xmlContext.xmlSpace;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0001A339 File Offset: 0x00019339
		public override string XmlLang
		{
			get
			{
				return this.xmlContext.xmlLang;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001A346 File Offset: 0x00019346
		public override ReadState ReadState
		{
			get
			{
				return this.readState;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001A34E File Offset: 0x0001934E
		public override bool EOF
		{
			get
			{
				return this.parsingFunction == XmlTextReaderImpl.ParsingFunction.Eof;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001A35A File Offset: 0x0001935A
		public override XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001A362 File Offset: 0x00019362
		public override bool CanResolveEntity
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001A365 File Offset: 0x00019365
		public override int AttributeCount
		{
			get
			{
				return this.attrCount;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001A370 File Offset: 0x00019370
		public override string GetAttribute(string name)
		{
			int num;
			if (name.IndexOf(':') == -1)
			{
				num = this.GetIndexOfAttributeWithoutPrefix(name);
			}
			else
			{
				num = this.GetIndexOfAttributeWithPrefix(name);
			}
			if (num < 0)
			{
				return null;
			}
			return this.nodes[num].StringValue;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001A3B0 File Offset: 0x000193B0
		public override string GetAttribute(string localName, string namespaceURI)
		{
			namespaceURI = ((namespaceURI == null) ? string.Empty : this.nameTable.Get(namespaceURI));
			localName = this.nameTable.Get(localName);
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				if (Ref.Equal(this.nodes[i].localName, localName) && Ref.Equal(this.nodes[i].ns, namespaceURI))
				{
					return this.nodes[i].StringValue;
				}
			}
			return null;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001A43D File Offset: 0x0001943D
		public override string GetAttribute(int i)
		{
			if (i < 0 || i >= this.attrCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			return this.nodes[this.index + i + 1].StringValue;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001A470 File Offset: 0x00019470
		public override bool MoveToAttribute(string name)
		{
			int num;
			if (name.IndexOf(':') == -1)
			{
				num = this.GetIndexOfAttributeWithoutPrefix(name);
			}
			else
			{
				num = this.GetIndexOfAttributeWithPrefix(name);
			}
			if (num >= 0)
			{
				if (this.InAttributeValueIterator)
				{
					this.FinishAttributeValueIterator();
				}
				this.curAttrIndex = num - this.index - 1;
				this.curNode = this.nodes[num];
				return true;
			}
			return false;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001A4D0 File Offset: 0x000194D0
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			namespaceURI = ((namespaceURI == null) ? string.Empty : this.nameTable.Get(namespaceURI));
			localName = this.nameTable.Get(localName);
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				if (Ref.Equal(this.nodes[i].localName, localName) && Ref.Equal(this.nodes[i].ns, namespaceURI))
				{
					this.curAttrIndex = i - this.index - 1;
					this.curNode = this.nodes[i];
					if (this.InAttributeValueIterator)
					{
						this.FinishAttributeValueIterator();
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001A580 File Offset: 0x00019580
		public override void MoveToAttribute(int i)
		{
			if (i < 0 || i >= this.attrCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			if (this.InAttributeValueIterator)
			{
				this.FinishAttributeValueIterator();
			}
			this.curAttrIndex = i;
			this.curNode = this.nodes[this.index + 1 + this.curAttrIndex];
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001A5D6 File Offset: 0x000195D6
		public override bool MoveToFirstAttribute()
		{
			if (this.attrCount == 0)
			{
				return false;
			}
			if (this.InAttributeValueIterator)
			{
				this.FinishAttributeValueIterator();
			}
			this.curAttrIndex = 0;
			this.curNode = this.nodes[this.index + 1];
			return true;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001A610 File Offset: 0x00019610
		public override bool MoveToNextAttribute()
		{
			if (this.curAttrIndex + 1 < this.attrCount)
			{
				if (this.InAttributeValueIterator)
				{
					this.FinishAttributeValueIterator();
				}
				this.curNode = this.nodes[this.index + 1 + ++this.curAttrIndex];
				return true;
			}
			return false;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001A665 File Offset: 0x00019665
		public override bool MoveToElement()
		{
			if (this.InAttributeValueIterator)
			{
				this.FinishAttributeValueIterator();
			}
			else if (this.curNode.type != XmlNodeType.Attribute)
			{
				return false;
			}
			this.curAttrIndex = -1;
			this.curNode = this.nodes[this.index];
			return true;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001A6A4 File Offset: 0x000196A4
		public override bool Read()
		{
			for (;;)
			{
				switch (this.parsingFunction)
				{
				case XmlTextReaderImpl.ParsingFunction.ElementContent:
					goto IL_77;
				case XmlTextReaderImpl.ParsingFunction.NoData:
					goto IL_2D9;
				case XmlTextReaderImpl.ParsingFunction.OpenUrl:
					this.OpenUrl();
					break;
				case XmlTextReaderImpl.ParsingFunction.SwitchToInteractive:
					this.readState = ReadState.Interactive;
					this.parsingFunction = this.nextParsingFunction;
					continue;
				case XmlTextReaderImpl.ParsingFunction.SwitchToInteractiveXmlDecl:
					break;
				case XmlTextReaderImpl.ParsingFunction.DocumentContent:
					goto IL_7E;
				case XmlTextReaderImpl.ParsingFunction.MoveToElementContent:
					this.ResetAttributes();
					this.index++;
					this.curNode = this.AddNode(this.index, this.index);
					this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ElementContent;
					continue;
				case XmlTextReaderImpl.ParsingFunction.PopElementContext:
					this.PopElementContext();
					this.parsingFunction = this.nextParsingFunction;
					continue;
				case XmlTextReaderImpl.ParsingFunction.PopEmptyElementContext:
					this.curNode = this.nodes[this.index];
					this.curNode.IsEmptyElement = false;
					this.ResetAttributes();
					this.PopElementContext();
					this.parsingFunction = this.nextParsingFunction;
					continue;
				case XmlTextReaderImpl.ParsingFunction.ResetAttributesRootLevel:
					this.ResetAttributes();
					this.curNode = this.nodes[this.index];
					this.parsingFunction = ((this.index == 0) ? XmlTextReaderImpl.ParsingFunction.DocumentContent : XmlTextReaderImpl.ParsingFunction.ElementContent);
					continue;
				case XmlTextReaderImpl.ParsingFunction.Error:
				case XmlTextReaderImpl.ParsingFunction.Eof:
				case XmlTextReaderImpl.ParsingFunction.ReaderClosed:
					return false;
				case XmlTextReaderImpl.ParsingFunction.EntityReference:
					goto IL_1A5;
				case XmlTextReaderImpl.ParsingFunction.InIncrementalRead:
					goto IL_2B0;
				case XmlTextReaderImpl.ParsingFunction.FragmentAttribute:
					goto IL_2B8;
				case XmlTextReaderImpl.ParsingFunction.ReportEndEntity:
					goto IL_1B9;
				case XmlTextReaderImpl.ParsingFunction.AfterResolveEntityInContent:
					this.curNode = this.AddNode(this.index, this.index);
					this.reportedEncoding = this.ps.encoding;
					this.reportedBaseUri = this.ps.baseUriStr;
					this.parsingFunction = this.nextParsingFunction;
					continue;
				case XmlTextReaderImpl.ParsingFunction.AfterResolveEmptyEntityInContent:
					goto IL_218;
				case XmlTextReaderImpl.ParsingFunction.XmlDeclarationFragment:
					goto IL_2BF;
				case XmlTextReaderImpl.ParsingFunction.GoToEof:
					goto IL_2CF;
				case XmlTextReaderImpl.ParsingFunction.PartialTextValue:
					this.SkipPartialTextValue();
					continue;
				case XmlTextReaderImpl.ParsingFunction.InReadAttributeValue:
					this.FinishAttributeValueIterator();
					this.curNode = this.nodes[this.index];
					continue;
				case XmlTextReaderImpl.ParsingFunction.InReadValueChunk:
					this.FinishReadValueChunk();
					continue;
				case XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary:
					this.FinishReadContentAsBinary();
					continue;
				case XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary:
					this.FinishReadElementContentAsBinary();
					continue;
				default:
					continue;
				}
				this.readState = ReadState.Interactive;
				this.parsingFunction = this.nextParsingFunction;
				if (this.ParseXmlDeclaration(false))
				{
					goto Block_1;
				}
				this.reportedEncoding = this.ps.encoding;
			}
			IL_77:
			return this.ParseElementContent();
			IL_7E:
			return this.ParseDocumentContent();
			Block_1:
			this.reportedEncoding = this.ps.encoding;
			return true;
			IL_1A5:
			this.parsingFunction = this.nextParsingFunction;
			this.ParseEntityReference();
			return true;
			IL_1B9:
			this.SetupEndEntityNodeInContent();
			this.parsingFunction = this.nextParsingFunction;
			return true;
			IL_218:
			this.curNode = this.AddNode(this.index, this.index);
			this.curNode.SetValueNode(XmlNodeType.Text, string.Empty);
			this.curNode.SetLineInfo(this.ps.lineNo, this.ps.LinePos);
			this.reportedEncoding = this.ps.encoding;
			this.reportedBaseUri = this.ps.baseUriStr;
			this.parsingFunction = this.nextParsingFunction;
			return true;
			IL_2B0:
			this.FinishIncrementalRead();
			return true;
			IL_2B8:
			return this.ParseFragmentAttribute();
			IL_2BF:
			this.ParseXmlDeclarationFragment();
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.GoToEof;
			return true;
			IL_2CF:
			this.OnEof();
			return false;
			IL_2D9:
			this.ThrowWithoutLineInfo("Xml_MissingRoot");
			return false;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001A9C2 File Offset: 0x000199C2
		public override void Close()
		{
			this.Close(this.closeInput);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001A9D0 File Offset: 0x000199D0
		public override void Skip()
		{
			if (this.readState != ReadState.Interactive)
			{
				return;
			}
			if (this.InAttributeValueIterator)
			{
				this.FinishAttributeValueIterator();
				this.curNode = this.nodes[this.index];
			}
			else
			{
				XmlTextReaderImpl.ParsingFunction parsingFunction = this.parsingFunction;
				if (parsingFunction != XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
				{
					switch (parsingFunction)
					{
					case XmlTextReaderImpl.ParsingFunction.PartialTextValue:
						this.SkipPartialTextValue();
						break;
					case XmlTextReaderImpl.ParsingFunction.InReadValueChunk:
						this.FinishReadValueChunk();
						break;
					case XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary:
						this.FinishReadContentAsBinary();
						break;
					case XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary:
						this.FinishReadElementContentAsBinary();
						break;
					}
				}
				else
				{
					this.FinishIncrementalRead();
				}
			}
			switch (this.curNode.type)
			{
			case XmlNodeType.Element:
				break;
			case XmlNodeType.Attribute:
				this.outerReader.MoveToElement();
				break;
			default:
				goto IL_E4;
			}
			if (!this.curNode.IsEmptyElement)
			{
				int num = this.index;
				this.parsingMode = XmlTextReaderImpl.ParsingMode.SkipContent;
				while (this.outerReader.Read() && this.index > num)
				{
				}
				this.parsingMode = XmlTextReaderImpl.ParsingMode.Full;
			}
			IL_E4:
			this.outerReader.Read();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001AACD File Offset: 0x00019ACD
		public override string LookupNamespace(string prefix)
		{
			if (!this.supportNamespaces)
			{
				return null;
			}
			return this.namespaceManager.LookupNamespace(prefix);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001AAE8 File Offset: 0x00019AE8
		public override bool ReadAttributeValue()
		{
			if (this.parsingFunction != XmlTextReaderImpl.ParsingFunction.InReadAttributeValue)
			{
				if (this.curNode.type != XmlNodeType.Attribute)
				{
					return false;
				}
				if (this.readState != ReadState.Interactive || this.curAttrIndex < 0)
				{
					return false;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadValueChunk)
				{
					this.FinishReadValueChunk();
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
				{
					this.FinishReadContentAsBinary();
				}
				if (this.curNode.nextAttrValueChunk == null || this.entityHandling == EntityHandling.ExpandEntities)
				{
					XmlTextReaderImpl.NodeData nodeData = this.AddNode(this.index + this.attrCount + 1, this.curNode.depth + 1);
					nodeData.SetValueNode(XmlNodeType.Text, this.curNode.StringValue);
					nodeData.lineInfo = this.curNode.lineInfo2;
					nodeData.depth = this.curNode.depth + 1;
					nodeData.nextAttrValueChunk = null;
					this.curNode = nodeData;
				}
				else
				{
					this.curNode = this.curNode.nextAttrValueChunk;
					this.AddNode(this.index + this.attrCount + 1, this.index + 2);
					this.nodes[this.index + this.attrCount + 1] = this.curNode;
					this.fullAttrCleanup = true;
				}
				this.nextParsingFunction = this.parsingFunction;
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.InReadAttributeValue;
				this.attributeValueBaseEntityId = this.ps.entityId;
				return true;
			}
			else
			{
				if (this.ps.entityId != this.attributeValueBaseEntityId)
				{
					return this.ParseAttributeValueChunk();
				}
				if (this.curNode.nextAttrValueChunk != null)
				{
					this.curNode = this.curNode.nextAttrValueChunk;
					this.nodes[this.index + this.attrCount + 1] = this.curNode;
					return true;
				}
				return false;
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001AC98 File Offset: 0x00019C98
		public override void ResolveEntity()
		{
			if (this.curNode.type != XmlNodeType.EntityReference)
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadAttributeValue || this.parsingFunction == XmlTextReaderImpl.ParsingFunction.FragmentAttribute)
			{
				switch (this.HandleGeneralEntityReference(this.curNode.localName, true, true, this.curNode.LinePos))
				{
				case XmlTextReaderImpl.EntityType.Expanded:
				case XmlTextReaderImpl.EntityType.ExpandedInAttribute:
					if (this.ps.charsUsed - this.ps.charPos == 0)
					{
						this.emptyEntityInAttributeResolved = true;
						goto IL_157;
					}
					goto IL_157;
				case XmlTextReaderImpl.EntityType.FakeExpanded:
					this.emptyEntityInAttributeResolved = true;
					goto IL_157;
				}
				throw new XmlException("Xml_InternalError");
			}
			switch (this.HandleGeneralEntityReference(this.curNode.localName, false, true, this.curNode.LinePos))
			{
			case XmlTextReaderImpl.EntityType.Expanded:
			case XmlTextReaderImpl.EntityType.ExpandedInAttribute:
				this.nextParsingFunction = this.parsingFunction;
				if (this.ps.charsUsed - this.ps.charPos == 0 && !this.ps.entity.IsExternal)
				{
					this.parsingFunction = XmlTextReaderImpl.ParsingFunction.AfterResolveEmptyEntityInContent;
					goto IL_157;
				}
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.AfterResolveEntityInContent;
				goto IL_157;
			case XmlTextReaderImpl.EntityType.FakeExpanded:
				this.nextParsingFunction = this.parsingFunction;
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.AfterResolveEmptyEntityInContent;
				goto IL_157;
			}
			throw new XmlException("Xml_InternalError");
			IL_157:
			this.ps.entityResolvedManually = true;
			this.index++;
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001AE16 File Offset: 0x00019E16
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x0001AE1E File Offset: 0x00019E1E
		internal XmlReader OuterReader
		{
			get
			{
				return this.outerReader;
			}
			set
			{
				this.outerReader = value;
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001AE27 File Offset: 0x00019E27
		internal void MoveOffEntityReference()
		{
			if (this.outerReader.NodeType == XmlNodeType.EntityReference && this.parsingFunction == XmlTextReaderImpl.ParsingFunction.AfterResolveEntityInContent && !this.outerReader.Read())
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001AE5E File Offset: 0x00019E5E
		public override string ReadString()
		{
			this.MoveOffEntityReference();
			return base.ReadString();
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001AE6C File Offset: 0x00019E6C
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001AE70 File Offset: 0x00019E70
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
			{
				if (this.incReadDecoder == this.base64Decoder)
				{
					return this.ReadContentAsBinary(buffer, index, count);
				}
			}
			else
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary)
				{
					throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
				}
				if (!XmlReader.CanReadContentAs(this.curNode.type))
				{
					throw base.CreateReadContentAsException("ReadContentAsBase64");
				}
				if (!this.InitReadContentAsBinary())
				{
					return 0;
				}
			}
			this.InitBase64Decoder();
			return this.ReadContentAsBinary(buffer, index, count);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001AF3C File Offset: 0x00019F3C
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
			{
				if (this.incReadDecoder == this.binHexDecoder)
				{
					return this.ReadContentAsBinary(buffer, index, count);
				}
			}
			else
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary)
				{
					throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
				}
				if (!XmlReader.CanReadContentAs(this.curNode.type))
				{
					throw base.CreateReadContentAsException("ReadContentAsBinHex");
				}
				if (!this.InitReadContentAsBinary())
				{
					return 0;
				}
			}
			this.InitBinHexDecoder();
			return this.ReadContentAsBinary(buffer, index, count);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001B008 File Offset: 0x0001A008
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary)
			{
				if (this.incReadDecoder == this.base64Decoder)
				{
					return this.ReadElementContentAsBinary(buffer, index, count);
				}
			}
			else
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
				{
					throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
				}
				if (this.curNode.type != XmlNodeType.Element)
				{
					throw base.CreateReadElementContentAsException("ReadElementContentAsBinHex");
				}
				if (!this.InitReadElementContentAsBinary())
				{
					return 0;
				}
			}
			this.InitBase64Decoder();
			return this.ReadElementContentAsBinary(buffer, index, count);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001B0D0 File Offset: 0x0001A0D0
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary)
			{
				if (this.incReadDecoder == this.binHexDecoder)
				{
					return this.ReadElementContentAsBinary(buffer, index, count);
				}
			}
			else
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
				{
					throw new InvalidOperationException(Res.GetString("Xml_MixingBinaryContentMethods"));
				}
				if (this.curNode.type != XmlNodeType.Element)
				{
					throw base.CreateReadElementContentAsException("ReadElementContentAsBinHex");
				}
				if (!this.InitReadElementContentAsBinary())
				{
					return 0;
				}
			}
			this.InitBinHexDecoder();
			return this.ReadElementContentAsBinary(buffer, index, count);
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0001B196 File Offset: 0x0001A196
		public override bool CanReadValueChunk
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001B19C File Offset: 0x0001A19C
		public override int ReadValueChunk(char[] buffer, int index, int count)
		{
			if (!XmlReader.HasValueInternal(this.curNode.type))
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidReadValueChunk", new object[]
				{
					this.curNode.type
				}));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.parsingFunction != XmlTextReaderImpl.ParsingFunction.InReadValueChunk)
			{
				if (this.readState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.PartialTextValue)
				{
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue;
				}
				else
				{
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnCachedValue;
					this.nextNextParsingFunction = this.nextParsingFunction;
					this.nextParsingFunction = this.parsingFunction;
				}
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.InReadValueChunk;
				this.readValueOffset = 0;
			}
			if (count == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = this.curNode.CopyTo(this.readValueOffset, buffer, index + num, count - num);
			num += num2;
			this.readValueOffset += num2;
			if (num == count)
			{
				char c = buffer[index + count - 1];
				if (c >= '\ud800' && c <= '\udbff')
				{
					num--;
					this.readValueOffset--;
					if (num == 0)
					{
						this.Throw("Xml_NotEnoughSpaceForSurrogatePair");
					}
				}
				return num;
			}
			if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue)
			{
				this.curNode.SetValue(string.Empty);
				bool flag = false;
				int num3 = 0;
				int num4 = 0;
				while (num < count && !flag)
				{
					int num5 = 0;
					flag = this.ParseText(out num3, out num4, ref num5);
					int num6 = count - num;
					if (num6 > num4 - num3)
					{
						num6 = num4 - num3;
					}
					Buffer.BlockCopy(this.ps.chars, num3 * 2, buffer, (index + num) * 2, num6 * 2);
					num += num6;
					num3 += num6;
				}
				this.incReadState = (flag ? XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnCachedValue : XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue);
				if (num == count)
				{
					char c2 = buffer[index + count - 1];
					if (c2 >= '\ud800' && c2 <= '\udbff')
					{
						num--;
						num3--;
						if (num == 0)
						{
							this.Throw("Xml_NotEnoughSpaceForSurrogatePair");
						}
					}
				}
				this.readValueOffset = 0;
				this.curNode.SetValue(this.ps.chars, num3, num4 - num3);
			}
			return num;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001B3D8 File Offset: 0x0001A3D8
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0001B3DB File Offset: 0x0001A3DB
		public int LineNumber
		{
			get
			{
				return this.curNode.LineNo;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0001B3E8 File Offset: 0x0001A3E8
		public int LinePosition
		{
			get
			{
				return this.curNode.LinePos;
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001B3F5 File Offset: 0x0001A3F5
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.GetNamespacesInScope(scope);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001B3FE File Offset: 0x0001A3FE
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return this.LookupNamespace(prefix);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001B407 File Offset: 0x0001A407
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.LookupPrefix(namespaceName);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001B410 File Offset: 0x0001A410
		internal IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.namespaceManager.GetNamespacesInScope(scope);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001B41E File Offset: 0x0001A41E
		internal string LookupPrefix(string namespaceName)
		{
			return this.namespaceManager.LookupPrefix(namespaceName);
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001B42C File Offset: 0x0001A42C
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x0001B434 File Offset: 0x0001A434
		internal bool Namespaces
		{
			get
			{
				return this.supportNamespaces;
			}
			set
			{
				if (this.readState != ReadState.Initial)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				this.supportNamespaces = value;
				if (value)
				{
					if (this.namespaceManager is XmlTextReaderImpl.NoNamespaceManager)
					{
						if (this.fragment && this.fragmentParserContext != null && this.fragmentParserContext.NamespaceManager != null)
						{
							this.namespaceManager = this.fragmentParserContext.NamespaceManager;
						}
						else
						{
							this.namespaceManager = new XmlNamespaceManager(this.nameTable);
						}
					}
					this.xmlContext.defaultNamespace = this.namespaceManager.LookupNamespace(string.Empty);
					return;
				}
				if (!(this.namespaceManager is XmlTextReaderImpl.NoNamespaceManager))
				{
					this.namespaceManager = new XmlTextReaderImpl.NoNamespaceManager();
				}
				this.xmlContext.defaultNamespace = string.Empty;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0001B4F5 File Offset: 0x0001A4F5
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x0001B500 File Offset: 0x0001A500
		internal bool Normalization
		{
			get
			{
				return this.normalize;
			}
			set
			{
				if (this.readState == ReadState.Closed)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				this.normalize = value;
				if (this.ps.entity == null || this.ps.entity.IsExternal)
				{
					this.ps.eolNormalized = !value;
				}
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0001B55B File Offset: 0x0001A55B
		internal Encoding Encoding
		{
			get
			{
				if (this.readState != ReadState.Interactive)
				{
					return null;
				}
				return this.reportedEncoding;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0001B56E File Offset: 0x0001A56E
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0001B576 File Offset: 0x0001A576
		internal WhitespaceHandling WhitespaceHandling
		{
			get
			{
				return this.whitespaceHandling;
			}
			set
			{
				if (this.readState == ReadState.Closed)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				if (value > WhitespaceHandling.None)
				{
					throw new XmlException("Xml_WhitespaceHandling", string.Empty);
				}
				this.whitespaceHandling = value;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0001B5AC File Offset: 0x0001A5AC
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0001B5B4 File Offset: 0x0001A5B4
		internal bool ProhibitDtd
		{
			get
			{
				return this.prohibitDtd;
			}
			set
			{
				this.prohibitDtd = value;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0001B5BD File Offset: 0x0001A5BD
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x0001B5C5 File Offset: 0x0001A5C5
		internal EntityHandling EntityHandling
		{
			get
			{
				return this.entityHandling;
			}
			set
			{
				if (value != EntityHandling.ExpandEntities && value != EntityHandling.ExpandCharEntities)
				{
					throw new XmlException("Xml_EntityHandling", string.Empty);
				}
				this.entityHandling = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x0001B5E8 File Offset: 0x0001A5E8
		internal XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
				this.xmlResolverIsSet = true;
				this.ps.baseUri = null;
				for (int i = 0; i <= this.parsingStatesStackTop; i++)
				{
					this.parsingStatesStack[i].baseUri = null;
				}
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001B634 File Offset: 0x0001A634
		internal void ResetState()
		{
			if (this.fragment)
			{
				this.Throw(new InvalidOperationException(Res.GetString("Xml_InvalidResetStateCall")));
			}
			if (this.readState == ReadState.Initial)
			{
				return;
			}
			this.ResetAttributes();
			while (this.namespaceManager.PopScope())
			{
			}
			while (this.InEntity)
			{
				this.HandleEntityEnd(true);
			}
			this.readState = ReadState.Initial;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractiveXmlDecl;
			this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
			this.curNode = this.nodes[0];
			this.curNode.Clear(XmlNodeType.None);
			this.curNode.SetLineInfo(0, 0);
			this.index = 0;
			this.rootElementParsed = false;
			this.charactersInDocument = 0L;
			this.charactersFromEntities = 0L;
			this.afterResetState = true;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001B6F0 File Offset: 0x0001A6F0
		internal TextReader GetRemainder()
		{
			XmlTextReaderImpl.ParsingFunction parsingFunction = this.parsingFunction;
			if (parsingFunction != XmlTextReaderImpl.ParsingFunction.OpenUrl)
			{
				switch (parsingFunction)
				{
				case XmlTextReaderImpl.ParsingFunction.Eof:
				case XmlTextReaderImpl.ParsingFunction.ReaderClosed:
					return new StringReader(string.Empty);
				case XmlTextReaderImpl.ParsingFunction.InIncrementalRead:
					if (!this.InEntity)
					{
						this.stringBuilder.Append(this.ps.chars, this.incReadLeftStartPos, this.incReadLeftEndPos - this.incReadLeftStartPos);
					}
					break;
				}
			}
			else
			{
				this.OpenUrl();
			}
			while (this.InEntity)
			{
				this.HandleEntityEnd(true);
			}
			this.ps.appendMode = false;
			do
			{
				this.stringBuilder.Append(this.ps.chars, this.ps.charPos, this.ps.charsUsed - this.ps.charPos);
				this.ps.charPos = this.ps.charsUsed;
			}
			while (this.ReadData() != 0);
			this.OnEof();
			string s = this.stringBuilder.ToString();
			this.stringBuilder.Length = 0;
			return new StringReader(s);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001B800 File Offset: 0x0001A800
		internal int ReadChars(char[] buffer, int index, int count)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
			{
				if (this.incReadDecoder != this.readCharsDecoder)
				{
					if (this.readCharsDecoder == null)
					{
						this.readCharsDecoder = new IncrementalReadCharsDecoder();
					}
					this.readCharsDecoder.Reset();
					this.incReadDecoder = this.readCharsDecoder;
				}
				return this.IncrementalRead(buffer, index, count);
			}
			if (this.curNode.type != XmlNodeType.Element)
			{
				return 0;
			}
			if (this.curNode.IsEmptyElement)
			{
				this.outerReader.Read();
				return 0;
			}
			if (this.readCharsDecoder == null)
			{
				this.readCharsDecoder = new IncrementalReadCharsDecoder();
			}
			this.InitIncrementalRead(this.readCharsDecoder);
			return this.IncrementalRead(buffer, index, count);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001B8AC File Offset: 0x0001A8AC
		internal int ReadBase64(byte[] array, int offset, int len)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
			{
				if (this.incReadDecoder != this.base64Decoder)
				{
					this.InitBase64Decoder();
				}
				return this.IncrementalRead(array, offset, len);
			}
			if (this.curNode.type != XmlNodeType.Element)
			{
				return 0;
			}
			if (this.curNode.IsEmptyElement)
			{
				this.outerReader.Read();
				return 0;
			}
			if (this.base64Decoder == null)
			{
				this.base64Decoder = new Base64Decoder();
			}
			this.InitIncrementalRead(this.base64Decoder);
			return this.IncrementalRead(array, offset, len);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001B934 File Offset: 0x0001A934
		internal int ReadBinHex(byte[] array, int offset, int len)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
			{
				if (this.incReadDecoder != this.binHexDecoder)
				{
					this.InitBinHexDecoder();
				}
				return this.IncrementalRead(array, offset, len);
			}
			if (this.curNode.type != XmlNodeType.Element)
			{
				return 0;
			}
			if (this.curNode.IsEmptyElement)
			{
				this.outerReader.Read();
				return 0;
			}
			if (this.binHexDecoder == null)
			{
				this.binHexDecoder = new BinHexDecoder();
			}
			this.InitIncrementalRead(this.binHexDecoder);
			return this.IncrementalRead(array, offset, len);
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0001B9BC File Offset: 0x0001A9BC
		internal XmlNameTable DtdParserProxy_NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0001B9C4 File Offset: 0x0001A9C4
		internal XmlNamespaceManager DtdParserProxy_NamespaceManager
		{
			get
			{
				return this.namespaceManager;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0001B9CC File Offset: 0x0001A9CC
		internal bool DtdParserProxy_DtdValidation
		{
			get
			{
				return this.DtdValidation;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0001B9D4 File Offset: 0x0001A9D4
		internal bool DtdParserProxy_Normalization
		{
			get
			{
				return this.normalize;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001B9DC File Offset: 0x0001A9DC
		internal bool DtdParserProxy_Namespaces
		{
			get
			{
				return this.supportNamespaces;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0001B9E4 File Offset: 0x0001A9E4
		internal bool DtdParserProxy_V1CompatibilityMode
		{
			get
			{
				return this.v1Compat;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001B9EC File Offset: 0x0001A9EC
		internal Uri DtdParserProxy_BaseUri
		{
			get
			{
				if (this.ps.baseUriStr.Length > 0 && this.ps.baseUri == null && this.xmlResolver != null)
				{
					this.ps.baseUri = this.xmlResolver.ResolveUri(null, this.ps.baseUriStr);
				}
				return this.ps.baseUri;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0001BA54 File Offset: 0x0001AA54
		internal bool DtdParserProxy_IsEof
		{
			get
			{
				return this.ps.isEof;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0001BA61 File Offset: 0x0001AA61
		internal char[] DtdParserProxy_ParsingBuffer
		{
			get
			{
				return this.ps.chars;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0001BA6E File Offset: 0x0001AA6E
		internal int DtdParserProxy_ParsingBufferLength
		{
			get
			{
				return this.ps.charsUsed;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0001BA7B File Offset: 0x0001AA7B
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x0001BA88 File Offset: 0x0001AA88
		internal int DtdParserProxy_CurrentPosition
		{
			get
			{
				return this.ps.charPos;
			}
			set
			{
				this.ps.charPos = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001BA96 File Offset: 0x0001AA96
		internal int DtdParserProxy_EntityStackLength
		{
			get
			{
				return this.parsingStatesStackTop + 1;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001BAA0 File Offset: 0x0001AAA0
		internal bool DtdParserProxy_IsEntityEolNormalized
		{
			get
			{
				return this.ps.eolNormalized;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001BAAD File Offset: 0x0001AAAD
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x0001BAB5 File Offset: 0x0001AAB5
		internal ValidationEventHandler DtdParserProxy_EventHandler
		{
			get
			{
				return this.validationEventHandler;
			}
			set
			{
				this.validationEventHandler = value;
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001BABE File Offset: 0x0001AABE
		internal void DtdParserProxy_OnNewLine(int pos)
		{
			this.OnNewLine(pos);
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001BAC7 File Offset: 0x0001AAC7
		internal int DtdParserProxy_LineNo
		{
			get
			{
				return this.ps.LineNo;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001BAD4 File Offset: 0x0001AAD4
		internal int DtdParserProxy_LineStartPosition
		{
			get
			{
				return this.ps.lineStartPos;
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001BAE1 File Offset: 0x0001AAE1
		internal int DtdParserProxy_ReadData()
		{
			return this.ReadData();
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001BAE9 File Offset: 0x0001AAE9
		internal void DtdParserProxy_SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception)
		{
			if (this.DtdValidation)
			{
				this.SendValidationEvent(severity, exception);
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001BAFC File Offset: 0x0001AAFC
		internal int DtdParserProxy_ParseNumericCharRef(BufferBuilder internalSubsetBuilder)
		{
			XmlTextReaderImpl.EntityType entityType;
			return this.ParseNumericCharRef(true, internalSubsetBuilder, out entityType);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001BB13 File Offset: 0x0001AB13
		internal int DtdParserProxy_ParseNamedCharRef(bool expand, BufferBuilder internalSubsetBuilder)
		{
			return this.ParseNamedCharRef(expand, internalSubsetBuilder);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001BB20 File Offset: 0x0001AB20
		internal void DtdParserProxy_ParsePI(BufferBuilder sb)
		{
			if (sb == null)
			{
				XmlTextReaderImpl.ParsingMode parsingMode = this.parsingMode;
				this.parsingMode = XmlTextReaderImpl.ParsingMode.SkipNode;
				this.ParsePI(null);
				this.parsingMode = parsingMode;
				return;
			}
			this.ParsePI(sb);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001BB58 File Offset: 0x0001AB58
		internal void DtdParserProxy_ParseComment(BufferBuilder sb)
		{
			try
			{
				if (sb == null)
				{
					XmlTextReaderImpl.ParsingMode parsingMode = this.parsingMode;
					this.parsingMode = XmlTextReaderImpl.ParsingMode.SkipNode;
					this.ParseCDataOrComment(XmlNodeType.Comment);
					this.parsingMode = parsingMode;
				}
				else
				{
					XmlTextReaderImpl.NodeData nodeData = this.curNode;
					this.curNode = this.AddNode(this.index + this.attrCount + 1, this.index);
					this.ParseCDataOrComment(XmlNodeType.Comment);
					this.curNode.CopyTo(sb);
					this.curNode = nodeData;
				}
			}
			catch (XmlException ex)
			{
				if (!(ex.ResString == "Xml_UnexpectedEOF") || this.ps.entity == null)
				{
					throw;
				}
				this.SendValidationEvent(XmlSeverityType.Error, "Sch_ParEntityRefNesting", null, this.ps.LineNo, this.ps.LinePos);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001BC24 File Offset: 0x0001AC24
		private bool IsResolverNull
		{
			get
			{
				return this.xmlResolver == null || (XmlReaderSection.ProhibitDefaultUrlResolver && !this.xmlResolverIsSet);
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001BC42 File Offset: 0x0001AC42
		internal bool DtdParserProxy_PushEntity(SchemaEntity entity, int entityId)
		{
			if (entity.IsExternal)
			{
				return !this.IsResolverNull && this.PushExternalEntity(entity, entityId);
			}
			this.PushInternalEntity(entity, entityId);
			return true;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001BC68 File Offset: 0x0001AC68
		internal bool DtdParserProxy_PopEntity(out SchemaEntity oldEntity, out int newEntityId)
		{
			if (this.parsingStatesStackTop == -1)
			{
				oldEntity = null;
				newEntityId = -1;
				return false;
			}
			oldEntity = this.ps.entity;
			this.PopEntity();
			newEntityId = this.ps.entityId;
			return true;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001BC9C File Offset: 0x0001AC9C
		internal bool DtdParserProxy_PushExternalSubset(string systemId, string publicId)
		{
			if (this.IsResolverNull)
			{
				return false;
			}
			if (this.ps.baseUriStr.Length > 0 && this.ps.baseUri == null)
			{
				this.ps.baseUri = this.xmlResolver.ResolveUri(null, this.ps.baseUriStr);
			}
			Stream stream = null;
			Uri uri;
			if (publicId == null || publicId.Length == 0)
			{
				uri = this.xmlResolver.ResolveUri(this.ps.baseUri, systemId);
				try
				{
					stream = this.OpenStream(uri);
					goto IL_14A;
				}
				catch (Exception ex)
				{
					if (this.v1Compat)
					{
						throw;
					}
					this.Throw(new XmlException("Xml_ErrorOpeningExternalDtd", new string[]
					{
						uri.ToString(),
						ex.Message
					}, ex, 0, 0));
					goto IL_14A;
				}
			}
			try
			{
				uri = this.xmlResolver.ResolveUri(this.ps.baseUri, publicId);
				stream = this.OpenStream(uri);
			}
			catch (Exception)
			{
				uri = this.xmlResolver.ResolveUri(this.ps.baseUri, systemId);
				try
				{
					stream = this.OpenStream(uri);
				}
				catch (Exception ex2)
				{
					if (this.v1Compat)
					{
						throw;
					}
					this.Throw(new XmlException("Xml_ErrorOpeningExternalDtd", new string[]
					{
						uri.ToString(),
						ex2.Message
					}, ex2, 0, 0));
				}
			}
			IL_14A:
			if (stream == null)
			{
				this.ThrowWithoutLineInfo("Xml_CannotResolveExternalSubset", new string[]
				{
					(publicId != null) ? publicId : string.Empty,
					systemId
				});
			}
			this.PushParsingState();
			if (this.v1Compat)
			{
				this.InitStreamInput(uri, stream, null);
			}
			else
			{
				this.InitStreamInput(uri, stream, null);
			}
			this.ps.entity = null;
			this.ps.entityId = 0;
			int charPos = this.ps.charPos;
			if (this.v1Compat)
			{
				this.EatWhitespaces(null);
			}
			if (!this.ParseXmlDeclaration(true))
			{
				this.ps.charPos = charPos;
			}
			return true;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001BEB8 File Offset: 0x0001AEB8
		internal void DtdParserProxy_PushInternalDtd(string baseUri, string internalDtd)
		{
			this.PushParsingState();
			this.RegisterConsumedCharacters((long)internalDtd.Length, false);
			this.InitStringInput(baseUri, Encoding.Unicode, internalDtd);
			this.ps.entity = null;
			this.ps.entityId = 0;
			this.ps.eolNormalized = false;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001BF0A File Offset: 0x0001AF0A
		internal void DtdParserProxy_Throw(Exception e)
		{
			this.Throw(e);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001BF14 File Offset: 0x0001AF14
		internal void DtdParserProxy_OnSystemId(string systemId, LineInfo keywordLineInfo, LineInfo systemLiteralLineInfo)
		{
			XmlTextReaderImpl.NodeData nodeData = this.AddAttributeNoChecks("SYSTEM", this.index);
			nodeData.SetValue(systemId);
			nodeData.lineInfo = keywordLineInfo;
			nodeData.lineInfo2 = systemLiteralLineInfo;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001BF48 File Offset: 0x0001AF48
		internal void DtdParserProxy_OnPublicId(string publicId, LineInfo keywordLineInfo, LineInfo publicLiteralLineInfo)
		{
			XmlTextReaderImpl.NodeData nodeData = this.AddAttributeNoChecks("PUBLIC", this.index);
			nodeData.SetValue(publicId);
			nodeData.lineInfo = keywordLineInfo;
			nodeData.lineInfo2 = publicLiteralLineInfo;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001BF7C File Offset: 0x0001AF7C
		private void Throw(int pos, string res, string arg)
		{
			this.ps.charPos = pos;
			this.Throw(res, arg);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001BF92 File Offset: 0x0001AF92
		private void Throw(int pos, string res, string[] args)
		{
			this.ps.charPos = pos;
			this.Throw(res, args);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001BFA8 File Offset: 0x0001AFA8
		private void Throw(int pos, string res)
		{
			this.ps.charPos = pos;
			this.Throw(res, string.Empty);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001BFC2 File Offset: 0x0001AFC2
		private void Throw(string res)
		{
			this.Throw(res, string.Empty);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001BFD0 File Offset: 0x0001AFD0
		private void Throw(string res, int lineNo, int linePos)
		{
			this.Throw(new XmlException(res, string.Empty, lineNo, linePos, this.ps.baseUriStr));
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001BFF0 File Offset: 0x0001AFF0
		private void Throw(string res, string arg)
		{
			this.Throw(new XmlException(res, arg, this.ps.LineNo, this.ps.LinePos, this.ps.baseUriStr));
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001C020 File Offset: 0x0001B020
		private void Throw(string res, string arg, int lineNo, int linePos)
		{
			this.Throw(new XmlException(res, arg, lineNo, linePos, this.ps.baseUriStr));
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001C03D File Offset: 0x0001B03D
		private void Throw(string res, string[] args)
		{
			this.Throw(new XmlException(res, args, this.ps.LineNo, this.ps.LinePos, this.ps.baseUriStr));
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001C070 File Offset: 0x0001B070
		private void Throw(Exception e)
		{
			this.SetErrorState();
			XmlException ex = e as XmlException;
			if (ex != null)
			{
				this.curNode.SetLineInfo(ex.LineNumber, ex.LinePosition);
			}
			throw e;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001C0A5 File Offset: 0x0001B0A5
		private void ReThrow(Exception e, int lineNo, int linePos)
		{
			this.Throw(new XmlException(e.Message, null, lineNo, linePos, this.ps.baseUriStr));
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001C0C6 File Offset: 0x0001B0C6
		private void ThrowWithoutLineInfo(string res)
		{
			this.Throw(new XmlException(res, string.Empty, this.ps.baseUriStr));
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001C0E4 File Offset: 0x0001B0E4
		private void ThrowWithoutLineInfo(string res, string arg)
		{
			this.Throw(new XmlException(res, arg, this.ps.baseUriStr));
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001C0FE File Offset: 0x0001B0FE
		private void ThrowWithoutLineInfo(string res, string[] args)
		{
			this.Throw(new XmlException(res, args, this.ps.baseUriStr));
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001C118 File Offset: 0x0001B118
		private void ThrowInvalidChar(int pos, char invChar)
		{
			if (pos == 0 && this.curNode.type == XmlNodeType.None && this.ps.textReader != null && this.ps.charsUsed >= 2 && ((this.ps.chars[0] == '\u0001' && this.ps.chars[1] == '\u0004') || this.ps.chars[0] == 'ß' || this.ps.chars[1] == 'ÿ'))
			{
				this.Throw(pos, "Xml_BinaryXmlReadAsText", XmlException.BuildCharExceptionStr(invChar));
				return;
			}
			this.Throw(pos, "Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(invChar));
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001C1C0 File Offset: 0x0001B1C0
		private void SetErrorState()
		{
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.Error;
			this.readState = ReadState.Error;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001C1D1 File Offset: 0x0001B1D1
		private void SendValidationEvent(XmlSeverityType severity, string code, string arg, int lineNo, int linePos)
		{
			this.SendValidationEvent(severity, new XmlSchemaException(code, arg, this.ps.baseUriStr, lineNo, linePos));
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001C1F0 File Offset: 0x0001B1F0
		private void SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception)
		{
			if (this.validationEventHandler != null)
			{
				this.validationEventHandler(this, new ValidationEventArgs(exception, severity));
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001C20D File Offset: 0x0001B20D
		private bool InAttributeValueIterator
		{
			get
			{
				return this.attrCount > 0 && this.parsingFunction >= XmlTextReaderImpl.ParsingFunction.InReadAttributeValue;
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001C228 File Offset: 0x0001B228
		private void FinishAttributeValueIterator()
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadValueChunk)
			{
				this.FinishReadValueChunk();
			}
			else if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary)
			{
				this.FinishReadContentAsBinary();
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadAttributeValue)
			{
				while (this.ps.entityId != this.attributeValueBaseEntityId)
				{
					this.HandleEntityEnd(false);
				}
				this.parsingFunction = this.nextParsingFunction;
				this.nextParsingFunction = ((this.index > 0) ? XmlTextReaderImpl.ParsingFunction.ElementContent : XmlTextReaderImpl.ParsingFunction.DocumentContent);
				this.emptyEntityInAttributeResolved = false;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001C2A4 File Offset: 0x0001B2A4
		private bool DtdValidation
		{
			get
			{
				return this.validationEventHandler != null;
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001C2B2 File Offset: 0x0001B2B2
		private void InitStreamInput(Stream stream, Encoding encoding)
		{
			this.InitStreamInput(null, string.Empty, stream, null, 0, encoding);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001C2C4 File Offset: 0x0001B2C4
		private void InitStreamInput(string baseUriStr, Stream stream, Encoding encoding)
		{
			this.InitStreamInput(null, baseUriStr, stream, null, 0, encoding);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001C2D2 File Offset: 0x0001B2D2
		private void InitStreamInput(Uri baseUri, Stream stream, Encoding encoding)
		{
			this.InitStreamInput(baseUri, baseUri.ToString(), stream, null, 0, encoding);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001C2E5 File Offset: 0x0001B2E5
		private void InitStreamInput(Uri baseUri, string baseUriStr, Stream stream, Encoding encoding)
		{
			this.InitStreamInput(baseUri, baseUriStr, stream, null, 0, encoding);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001C2F4 File Offset: 0x0001B2F4
		private void InitStreamInput(Uri baseUri, string baseUriStr, Stream stream, byte[] bytes, int byteCount, Encoding encoding)
		{
			this.ps.stream = stream;
			this.ps.baseUri = baseUri;
			this.ps.baseUriStr = baseUriStr;
			int num;
			if (bytes != null)
			{
				this.ps.bytes = bytes;
				this.ps.bytesUsed = byteCount;
				num = this.ps.bytes.Length;
			}
			else
			{
				num = XmlReader.CalcBufferSize(stream);
				if (this.ps.bytes == null || this.ps.bytes.Length < num)
				{
					this.ps.bytes = new byte[num];
				}
			}
			if (this.ps.chars == null || this.ps.chars.Length < num + 1)
			{
				this.ps.chars = new char[num + 1];
			}
			this.ps.bytePos = 0;
			while (this.ps.bytesUsed < 4 && this.ps.bytes.Length - this.ps.bytesUsed > 0)
			{
				int num2 = stream.Read(this.ps.bytes, this.ps.bytesUsed, this.ps.bytes.Length - this.ps.bytesUsed);
				if (num2 == 0)
				{
					this.ps.isStreamEof = true;
					break;
				}
				this.ps.bytesUsed = this.ps.bytesUsed + num2;
			}
			if (encoding == null)
			{
				encoding = this.DetectEncoding();
			}
			this.SetupEncoding(encoding);
			byte[] preamble = this.ps.encoding.GetPreamble();
			int num3 = preamble.Length;
			int num4 = 0;
			while (num4 < num3 && num4 < this.ps.bytesUsed && this.ps.bytes[num4] == preamble[num4])
			{
				num4++;
			}
			if (num4 == num3)
			{
				this.ps.bytePos = num3;
			}
			this.documentStartBytePos = this.ps.bytePos;
			this.ps.eolNormalized = !this.normalize;
			this.ps.appendMode = true;
			this.ReadData();
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001C4F4 File Offset: 0x0001B4F4
		private void InitTextReaderInput(string baseUriStr, TextReader input)
		{
			this.ps.textReader = input;
			this.ps.baseUriStr = baseUriStr;
			this.ps.baseUri = null;
			if (this.ps.chars == null)
			{
				this.ps.chars = new char[4097];
			}
			this.ps.encoding = Encoding.Unicode;
			this.ps.eolNormalized = !this.normalize;
			this.ps.appendMode = true;
			this.ReadData();
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001C580 File Offset: 0x0001B580
		private void InitStringInput(string baseUriStr, Encoding originalEncoding, string str)
		{
			this.ps.baseUriStr = baseUriStr;
			this.ps.baseUri = null;
			int length = str.Length;
			this.ps.chars = new char[length + 1];
			str.CopyTo(0, this.ps.chars, 0, str.Length);
			this.ps.charsUsed = length;
			this.ps.chars[length] = '\0';
			this.ps.encoding = originalEncoding;
			this.ps.eolNormalized = !this.normalize;
			this.ps.isEof = true;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001C620 File Offset: 0x0001B620
		private void InitFragmentReader(XmlNodeType fragmentType, XmlParserContext parserContext, bool allowXmlDeclFragment)
		{
			this.fragmentParserContext = parserContext;
			if (parserContext != null)
			{
				if (parserContext.NamespaceManager != null)
				{
					this.namespaceManager = parserContext.NamespaceManager;
					this.xmlContext.defaultNamespace = this.namespaceManager.LookupNamespace(string.Empty);
				}
				else
				{
					this.namespaceManager = new XmlNamespaceManager(this.nameTable);
				}
				this.ps.baseUriStr = parserContext.BaseURI;
				this.ps.baseUri = null;
				this.xmlContext.xmlLang = parserContext.XmlLang;
				this.xmlContext.xmlSpace = parserContext.XmlSpace;
			}
			else
			{
				this.namespaceManager = new XmlNamespaceManager(this.nameTable);
				this.ps.baseUriStr = string.Empty;
				this.ps.baseUri = null;
			}
			this.reportedBaseUri = this.ps.baseUriStr;
			switch (fragmentType)
			{
			case XmlNodeType.Element:
				this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
				break;
			case XmlNodeType.Attribute:
				this.ps.appendMode = false;
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractive;
				this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.FragmentAttribute;
				break;
			default:
				if (fragmentType != XmlNodeType.Document)
				{
					if (fragmentType == XmlNodeType.XmlDeclaration)
					{
						if (allowXmlDeclFragment)
						{
							this.ps.appendMode = false;
							this.parsingFunction = XmlTextReaderImpl.ParsingFunction.SwitchToInteractive;
							this.nextParsingFunction = XmlTextReaderImpl.ParsingFunction.XmlDeclarationFragment;
							break;
						}
					}
					this.Throw("Xml_PartialContentNodeTypeNotSupportedEx", fragmentType.ToString());
					return;
				}
				break;
			}
			this.fragmentType = fragmentType;
			this.fragment = true;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001C784 File Offset: 0x0001B784
		private void OpenUrl()
		{
			XmlResolver xmlResolver;
			if (this.ps.baseUri != null)
			{
				xmlResolver = this.xmlResolver;
			}
			else
			{
				xmlResolver = ((this.xmlResolver == null) ? new XmlUrlResolver() : this.xmlResolver);
				this.ps.baseUri = xmlResolver.ResolveUri(null, this.url);
				this.ps.baseUriStr = this.ps.baseUri.ToString();
			}
			try
			{
				CompressedStack.Run(this.compressedStack, new ContextCallback(this.OpenUrlDelegate), xmlResolver);
			}
			catch
			{
				this.SetErrorState();
				throw;
			}
			if (this.ps.stream == null)
			{
				this.ThrowWithoutLineInfo("Xml_CannotResolveUrl", this.ps.baseUriStr);
			}
			this.InitStreamInput(this.ps.baseUri, this.ps.baseUriStr, this.ps.stream, null);
			this.reportedEncoding = this.ps.encoding;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001C888 File Offset: 0x0001B888
		private void OpenUrlDelegate(object xmlResolver)
		{
			this.ps.stream = (Stream)((XmlResolver)xmlResolver).GetEntity(this.ps.baseUri, null, typeof(Stream));
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001C8BC File Offset: 0x0001B8BC
		private Encoding DetectEncoding()
		{
			if (this.ps.bytesUsed < 2)
			{
				return null;
			}
			int num = (int)this.ps.bytes[0] << 8 | (int)this.ps.bytes[1];
			int num2 = (this.ps.bytesUsed >= 4) ? ((int)this.ps.bytes[2] << 8 | (int)this.ps.bytes[3]) : 0;
			int num3 = num;
			if (num3 <= 15360)
			{
				if (num3 != 0)
				{
					if (num3 != 60)
					{
						if (num3 == 15360)
						{
							int num4 = num2;
							if (num4 == 0)
							{
								return Ucs4Encoding.UCS4_Littleendian;
							}
							if (num4 == 16128)
							{
								return Encoding.Unicode;
							}
						}
					}
					else
					{
						int num5 = num2;
						if (num5 == 0)
						{
							return Ucs4Encoding.UCS4_3412;
						}
						if (num5 == 63)
						{
							return Encoding.BigEndianUnicode;
						}
					}
				}
				else
				{
					int num6 = num2;
					if (num6 <= 15360)
					{
						if (num6 == 60)
						{
							return Ucs4Encoding.UCS4_Bigendian;
						}
						if (num6 == 15360)
						{
							return Ucs4Encoding.UCS4_2143;
						}
					}
					else
					{
						if (num6 == 65279)
						{
							return Ucs4Encoding.UCS4_Bigendian;
						}
						if (num6 == 65534)
						{
							return Ucs4Encoding.UCS4_2143;
						}
					}
				}
			}
			else if (num3 <= 61371)
			{
				if (num3 != 19567)
				{
					if (num3 == 61371)
					{
						if ((num2 & 65280) == 48896)
						{
							return new UTF8Encoding(true, true);
						}
					}
				}
				else if (num2 == 42900)
				{
					this.Throw("Xml_UnknownEncoding", "ebcdic");
				}
			}
			else if (num3 != 65279)
			{
				if (num3 == 65534)
				{
					if (num2 == 0)
					{
						return Ucs4Encoding.UCS4_Littleendian;
					}
					return Encoding.Unicode;
				}
			}
			else
			{
				if (num2 == 0)
				{
					return Ucs4Encoding.UCS4_3412;
				}
				return Encoding.BigEndianUnicode;
			}
			return null;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001CA58 File Offset: 0x0001BA58
		private void SetupEncoding(Encoding encoding)
		{
			if (encoding == null)
			{
				this.ps.encoding = Encoding.UTF8;
				this.ps.decoder = new SafeAsciiDecoder();
				return;
			}
			this.ps.encoding = encoding;
			switch (this.ps.encoding.CodePage)
			{
			case 1200:
				this.ps.decoder = new UTF16Decoder(false);
				return;
			case 1201:
				this.ps.decoder = new UTF16Decoder(true);
				return;
			default:
				this.ps.decoder = encoding.GetDecoder();
				return;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001CAF4 File Offset: 0x0001BAF4
		private void SwitchEncoding(Encoding newEncoding)
		{
			if ((newEncoding.CodePage != this.ps.encoding.CodePage || this.ps.decoder is SafeAsciiDecoder) && !this.afterResetState)
			{
				this.UnDecodeChars();
				this.ps.appendMode = false;
				this.SetupEncoding(newEncoding);
				this.ReadData();
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001CB54 File Offset: 0x0001BB54
		private Encoding CheckEncoding(string newEncodingName)
		{
			if (this.ps.stream == null)
			{
				return this.ps.encoding;
			}
			if (string.Compare(newEncodingName, "ucs-2", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "utf-16", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "iso-10646-ucs-2", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "ucs-4", StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (this.ps.encoding.CodePage != Encoding.BigEndianUnicode.CodePage && this.ps.encoding.CodePage != Encoding.Unicode.CodePage && string.Compare(newEncodingName, "ucs-4", StringComparison.OrdinalIgnoreCase) != 0)
				{
					if (this.afterResetState)
					{
						this.Throw("Xml_EncodingSwitchAfterResetState", newEncodingName);
					}
					else
					{
						this.ThrowWithoutLineInfo("Xml_MissingByteOrderMark");
					}
				}
				return this.ps.encoding;
			}
			Encoding encoding = null;
			if (string.Compare(newEncodingName, "utf-8", StringComparison.OrdinalIgnoreCase) == 0)
			{
				encoding = new UTF8Encoding(true, true);
			}
			else
			{
				try
				{
					encoding = Encoding.GetEncoding(newEncodingName);
					if (encoding.CodePage == -1)
					{
						this.Throw("Xml_UnknownEncoding", newEncodingName);
					}
				}
				catch (NotSupportedException)
				{
					this.Throw("Xml_UnknownEncoding", newEncodingName);
				}
				catch (ArgumentException)
				{
					this.Throw("Xml_UnknownEncoding", newEncodingName);
				}
			}
			if (this.afterResetState && this.ps.encoding.CodePage != encoding.CodePage)
			{
				this.Throw("Xml_EncodingSwitchAfterResetState", newEncodingName);
			}
			return encoding;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001CCC4 File Offset: 0x0001BCC4
		private void UnDecodeChars()
		{
			if (this.maxCharactersInDocument > 0L)
			{
				this.charactersInDocument -= (long)(this.ps.charsUsed - this.ps.charPos);
			}
			if (this.maxCharactersFromEntities > 0L && this.InEntity)
			{
				this.charactersFromEntities -= (long)(this.ps.charsUsed - this.ps.charPos);
			}
			this.ps.bytePos = this.documentStartBytePos;
			if (this.ps.charPos > 0)
			{
				this.ps.bytePos = this.ps.bytePos + this.ps.encoding.GetByteCount(this.ps.chars, 0, this.ps.charPos);
			}
			this.ps.charsUsed = this.ps.charPos;
			this.ps.isEof = false;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001CDB1 File Offset: 0x0001BDB1
		private void SwitchEncodingToUTF8()
		{
			this.SwitchEncoding(new UTF8Encoding(true, true));
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001CDC0 File Offset: 0x0001BDC0
		private int ReadData()
		{
			if (this.ps.isEof)
			{
				return 0;
			}
			int num;
			if (this.ps.appendMode)
			{
				if (this.ps.charsUsed == this.ps.chars.Length - 1)
				{
					for (int i = 0; i < this.attrCount; i++)
					{
						this.nodes[this.index + i + 1].OnBufferInvalidated();
					}
					char[] array = new char[this.ps.chars.Length * 2];
					Buffer.BlockCopy(this.ps.chars, 0, array, 0, this.ps.chars.Length * 2);
					this.ps.chars = array;
				}
				if (this.ps.stream != null && this.ps.bytesUsed - this.ps.bytePos < 6 && this.ps.bytes.Length - this.ps.bytesUsed < 6)
				{
					byte[] array2 = new byte[this.ps.bytes.Length * 2];
					Buffer.BlockCopy(this.ps.bytes, 0, array2, 0, this.ps.bytesUsed);
					this.ps.bytes = array2;
				}
				num = this.ps.chars.Length - this.ps.charsUsed - 1;
				if (num > 80)
				{
					num = 80;
				}
			}
			else
			{
				int num2 = this.ps.chars.Length;
				if (num2 - this.ps.charsUsed <= num2 / 2)
				{
					for (int j = 0; j < this.attrCount; j++)
					{
						this.nodes[this.index + j + 1].OnBufferInvalidated();
					}
					int num3 = this.ps.charsUsed - this.ps.charPos;
					if (num3 < num2 - 1)
					{
						this.ps.lineStartPos = this.ps.lineStartPos - this.ps.charPos;
						if (num3 > 0)
						{
							Buffer.BlockCopy(this.ps.chars, this.ps.charPos * 2, this.ps.chars, 0, num3 * 2);
						}
						this.ps.charPos = 0;
						this.ps.charsUsed = num3;
					}
					else
					{
						char[] array3 = new char[this.ps.chars.Length * 2];
						Buffer.BlockCopy(this.ps.chars, 0, array3, 0, this.ps.chars.Length * 2);
						this.ps.chars = array3;
					}
				}
				if (this.ps.stream != null)
				{
					int num4 = this.ps.bytesUsed - this.ps.bytePos;
					if (num4 <= 128)
					{
						if (num4 == 0)
						{
							this.ps.bytesUsed = 0;
						}
						else
						{
							Buffer.BlockCopy(this.ps.bytes, this.ps.bytePos, this.ps.bytes, 0, num4);
							this.ps.bytesUsed = num4;
						}
						this.ps.bytePos = 0;
					}
				}
				num = this.ps.chars.Length - this.ps.charsUsed - 1;
			}
			if (this.ps.stream != null)
			{
				if (!this.ps.isStreamEof && this.ps.bytePos == this.ps.bytesUsed && this.ps.bytes.Length - this.ps.bytesUsed > 0)
				{
					int num5 = this.ps.stream.Read(this.ps.bytes, this.ps.bytesUsed, this.ps.bytes.Length - this.ps.bytesUsed);
					if (num5 == 0)
					{
						this.ps.isStreamEof = true;
					}
					this.ps.bytesUsed = this.ps.bytesUsed + num5;
				}
				int bytePos = this.ps.bytePos;
				num = this.GetChars(num);
				if (num == 0 && this.ps.bytePos != bytePos)
				{
					return this.ReadData();
				}
			}
			else if (this.ps.textReader != null)
			{
				num = this.ps.textReader.Read(this.ps.chars, this.ps.charsUsed, this.ps.chars.Length - this.ps.charsUsed - 1);
				this.ps.charsUsed = this.ps.charsUsed + num;
			}
			else
			{
				num = 0;
			}
			this.RegisterConsumedCharacters((long)num, this.InEntity);
			if (num == 0)
			{
				this.ps.isEof = true;
			}
			this.ps.chars[this.ps.charsUsed] = '\0';
			return num;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001D270 File Offset: 0x0001C270
		private int GetChars(int maxCharsCount)
		{
			int num = this.ps.bytesUsed - this.ps.bytePos;
			if (num == 0)
			{
				return 0;
			}
			int num2;
			try
			{
				bool flag;
				this.ps.decoder.Convert(this.ps.bytes, this.ps.bytePos, num, this.ps.chars, this.ps.charsUsed, maxCharsCount, false, out num, out num2, out flag);
			}
			catch (ArgumentException)
			{
				this.InvalidCharRecovery(ref num, out num2);
			}
			this.ps.bytePos = this.ps.bytePos + num;
			this.ps.charsUsed = this.ps.charsUsed + num2;
			return num2;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001D328 File Offset: 0x0001C328
		private void InvalidCharRecovery(ref int bytesCount, out int charsCount)
		{
			int num = 0;
			int i = 0;
			try
			{
				while (i < bytesCount)
				{
					int num2;
					int num3;
					bool flag;
					this.ps.decoder.Convert(this.ps.bytes, this.ps.bytePos + i, 1, this.ps.chars, this.ps.charsUsed + num, 1, false, out num2, out num3, out flag);
					num += num3;
					i += num2;
				}
			}
			catch (ArgumentException)
			{
			}
			if (num == 0)
			{
				this.Throw(this.ps.charsUsed, "Xml_InvalidCharInThisEncoding");
			}
			charsCount = num;
			bytesCount = i;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001D3C8 File Offset: 0x0001C3C8
		internal void Close(bool closeInput)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.ReaderClosed)
			{
				return;
			}
			while (this.InEntity)
			{
				this.PopParsingState();
			}
			this.ps.Close(closeInput);
			this.curNode = XmlTextReaderImpl.NodeData.None;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ReaderClosed;
			this.reportedEncoding = null;
			this.reportedBaseUri = string.Empty;
			this.readState = ReadState.Closed;
			this.fullAttrCleanup = false;
			this.ResetAttributes();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001D433 File Offset: 0x0001C433
		private void ShiftBuffer(int sourcePos, int destPos, int count)
		{
			Buffer.BlockCopy(this.ps.chars, sourcePos * 2, this.ps.chars, destPos * 2, count * 2);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001D45C File Offset: 0x0001C45C
		private unsafe bool ParseXmlDeclaration(bool isTextDecl)
		{
			while (this.ps.charsUsed - this.ps.charPos < 6)
			{
				if (this.ReadData() == 0)
				{
					IL_7EC:
					if (!isTextDecl)
					{
						this.parsingFunction = this.nextParsingFunction;
					}
					if (this.afterResetState)
					{
						int codePage = this.ps.encoding.CodePage;
						if (codePage != Encoding.UTF8.CodePage && codePage != Encoding.Unicode.CodePage && codePage != Encoding.BigEndianUnicode.CodePage && !(this.ps.encoding is Ucs4Encoding))
						{
							this.Throw("Xml_EncodingSwitchAfterResetState", (this.ps.encoding.GetByteCount("A") == 1) ? "UTF-8" : "UTF-16");
						}
					}
					if (this.ps.decoder is SafeAsciiDecoder)
					{
						this.SwitchEncodingToUTF8();
					}
					this.ps.appendMode = false;
					return false;
				}
			}
			if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, 5, "<?xml") && !this.xmlCharType.IsNameChar(this.ps.chars[this.ps.charPos + 5]))
			{
				if (!isTextDecl)
				{
					this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos + 2);
					this.curNode.SetNamedNode(XmlNodeType.XmlDeclaration, this.Xml);
				}
				this.ps.charPos = this.ps.charPos + 5;
				BufferBuilder bufferBuilder = isTextDecl ? new BufferBuilder() : this.stringBuilder;
				int num = 0;
				Encoding encoding = null;
				for (;;)
				{
					int length = bufferBuilder.Length;
					int num2 = this.EatWhitespaces((num == 0) ? null : bufferBuilder);
					if (this.ps.chars[this.ps.charPos] == '?')
					{
						bufferBuilder.Length = length;
						if (this.ps.chars[this.ps.charPos + 1] == '>')
						{
							break;
						}
						if (this.ps.charPos + 1 == this.ps.charsUsed)
						{
							goto IL_7C4;
						}
						this.ThrowUnexpectedToken("'>'");
					}
					if (num2 == 0 && num != 0)
					{
						this.ThrowUnexpectedToken("?>");
					}
					int num3 = this.ParseName();
					XmlTextReaderImpl.NodeData nodeData = null;
					char c = this.ps.chars[this.ps.charPos];
					if (c != 'e')
					{
						if (c != 's')
						{
							if (c != 'v' || !XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num3 - this.ps.charPos, "version") || num != 0)
							{
								goto IL_3BB;
							}
							if (!isTextDecl)
							{
								nodeData = this.AddAttributeNoChecks("version", 0);
							}
						}
						else
						{
							if (!XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num3 - this.ps.charPos, "standalone") || (num != 1 && num != 2) || isTextDecl)
							{
								goto IL_3BB;
							}
							if (!isTextDecl)
							{
								nodeData = this.AddAttributeNoChecks("standalone", 0);
							}
							num = 2;
						}
					}
					else
					{
						if (!XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num3 - this.ps.charPos, "encoding") || (num != 1 && (!isTextDecl || num != 0)))
						{
							goto IL_3BB;
						}
						if (!isTextDecl)
						{
							nodeData = this.AddAttributeNoChecks("encoding", 0);
						}
						num = 1;
					}
					IL_3D0:
					if (!isTextDecl)
					{
						nodeData.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
					}
					bufferBuilder.Append(this.ps.chars, this.ps.charPos, num3 - this.ps.charPos);
					this.ps.charPos = num3;
					if (this.ps.chars[this.ps.charPos] != '=')
					{
						this.EatWhitespaces(bufferBuilder);
						if (this.ps.chars[this.ps.charPos] != '=')
						{
							this.ThrowUnexpectedToken("=");
						}
					}
					bufferBuilder.Append('=');
					this.ps.charPos = this.ps.charPos + 1;
					char c2 = this.ps.chars[this.ps.charPos];
					if (c2 != '"' && c2 != '\'')
					{
						this.EatWhitespaces(bufferBuilder);
						c2 = this.ps.chars[this.ps.charPos];
						if (c2 != '"' && c2 != '\'')
						{
							this.ThrowUnexpectedToken("\"", "'");
						}
					}
					bufferBuilder.Append(c2);
					this.ps.charPos = this.ps.charPos + 1;
					if (!isTextDecl)
					{
						nodeData.quoteChar = c2;
						nodeData.SetLineInfo2(this.ps.LineNo, this.ps.LinePos);
					}
					int num4 = this.ps.charPos;
					char[] chars;
					for (;;)
					{
						chars = this.ps.chars;
						while ((this.xmlCharType.charProperties[chars[num4]] & 128) != 0)
						{
							num4++;
						}
						if (this.ps.chars[num4] == c2)
						{
							break;
						}
						if (num4 != this.ps.charsUsed)
						{
							goto IL_7AF;
						}
						if (this.ReadData() == 0)
						{
							goto Block_57;
						}
					}
					switch (num)
					{
					case 0:
						if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos, "1.0"))
						{
							if (!isTextDecl)
							{
								nodeData.SetValue(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos);
							}
							num = 1;
						}
						else
						{
							string arg = new string(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos);
							this.Throw("Xml_InvalidVersionNumber", arg);
						}
						break;
					case 1:
					{
						string text = new string(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos);
						encoding = this.CheckEncoding(text);
						if (!isTextDecl)
						{
							nodeData.SetValue(text);
						}
						num = 2;
						break;
					}
					case 2:
						if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos, "yes"))
						{
							this.standalone = true;
						}
						else if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos, "no"))
						{
							this.standalone = false;
						}
						else
						{
							this.Throw("Xml_InvalidXmlDecl", this.ps.LineNo, this.ps.LinePos - 1);
						}
						if (!isTextDecl)
						{
							nodeData.SetValue(this.ps.chars, this.ps.charPos, num4 - this.ps.charPos);
						}
						num = 3;
						break;
					}
					bufferBuilder.Append(chars, this.ps.charPos, num4 - this.ps.charPos);
					bufferBuilder.Append(c2);
					this.ps.charPos = num4 + 1;
					continue;
					Block_57:
					this.Throw("Xml_UnclosedQuote");
					goto IL_7C4;
					IL_7AF:
					this.Throw(isTextDecl ? "Xml_InvalidTextDecl" : "Xml_InvalidXmlDecl");
					goto IL_7C4;
					IL_3BB:
					this.Throw(isTextDecl ? "Xml_InvalidTextDecl" : "Xml_InvalidXmlDecl");
					goto IL_3D0;
					IL_7C4:
					if (this.ps.isEof || this.ReadData() == 0)
					{
						this.Throw("Xml_UnexpectedEOF1");
					}
				}
				if (num == 0)
				{
					this.Throw(isTextDecl ? "Xml_InvalidTextDecl" : "Xml_InvalidXmlDecl");
				}
				this.ps.charPos = this.ps.charPos + 2;
				if (!isTextDecl)
				{
					this.curNode.SetValue(bufferBuilder.ToString());
					bufferBuilder.Length = 0;
					this.nextParsingFunction = this.parsingFunction;
					this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ResetAttributesRootLevel;
				}
				if (encoding == null)
				{
					if (isTextDecl)
					{
						this.Throw("Xml_InvalidTextDecl");
					}
					if (this.afterResetState)
					{
						int codePage2 = this.ps.encoding.CodePage;
						if (codePage2 != Encoding.UTF8.CodePage && codePage2 != Encoding.Unicode.CodePage && codePage2 != Encoding.BigEndianUnicode.CodePage && !(this.ps.encoding is Ucs4Encoding))
						{
							this.Throw("Xml_EncodingSwitchAfterResetState", (this.ps.encoding.GetByteCount("A") == 1) ? "UTF-8" : "UTF-16");
						}
					}
					if (this.ps.decoder is SafeAsciiDecoder)
					{
						this.SwitchEncodingToUTF8();
					}
				}
				else
				{
					this.SwitchEncoding(encoding);
				}
				this.ps.appendMode = false;
				return true;
			}
			goto IL_7EC;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001DD10 File Offset: 0x0001CD10
		private bool ParseDocumentContent()
		{
			int num;
			for (;;)
			{
				bool flag = false;
				num = this.ps.charPos;
				char[] chars = this.ps.chars;
				if (chars[num] == '<')
				{
					flag = true;
					if (this.ps.charsUsed - num >= 4)
					{
						num++;
						char c = chars[num];
						if (c != '!')
						{
							if (c != '/')
							{
								if (c != '?')
								{
									goto IL_1CC;
								}
								this.ps.charPos = num + 1;
								if (this.ParsePI())
								{
									break;
								}
								continue;
							}
							else
							{
								this.Throw(num + 1, "Xml_UnexpectedEndTag");
							}
						}
						else
						{
							num++;
							if (this.ps.charsUsed - num >= 2)
							{
								if (chars[num] == '-')
								{
									if (chars[num + 1] == '-')
									{
										this.ps.charPos = num + 2;
										if (this.ParseComment())
										{
											return true;
										}
										continue;
									}
									else
									{
										this.ThrowUnexpectedToken(num + 1, "-");
									}
								}
								else if (chars[num] == '[')
								{
									if (this.fragmentType != XmlNodeType.Document)
									{
										num++;
										if (this.ps.charsUsed - num >= 6)
										{
											if (XmlConvert.StrEqual(chars, num, 6, "CDATA["))
											{
												goto Block_13;
											}
											this.ThrowUnexpectedToken(num, "CDATA[");
										}
									}
									else
									{
										this.Throw(this.ps.charPos, "Xml_InvalidRootData");
									}
								}
								else
								{
									if (this.fragmentType == XmlNodeType.Document || this.fragmentType == XmlNodeType.None)
									{
										goto IL_164;
									}
									if (this.ParseUnexpectedToken(num) == "DOCTYPE")
									{
										this.Throw("Xml_BadDTDLocation");
									}
									else
									{
										this.ThrowUnexpectedToken(num, "<!--", "<[CDATA[");
									}
								}
							}
						}
					}
				}
				else if (chars[num] == '&')
				{
					if (this.fragmentType != XmlNodeType.Document)
					{
						if (this.fragmentType == XmlNodeType.None)
						{
							this.fragmentType = XmlNodeType.Element;
						}
						int num2;
						switch (this.HandleEntityReference(false, XmlTextReaderImpl.EntityExpandType.OnlyGeneral, out num2))
						{
						case XmlTextReaderImpl.EntityType.CharacterDec:
						case XmlTextReaderImpl.EntityType.CharacterHex:
						case XmlTextReaderImpl.EntityType.CharacterNamed:
							if (this.ParseText())
							{
								return true;
							}
							continue;
						case XmlTextReaderImpl.EntityType.Unexpanded:
							goto IL_279;
						}
						chars = this.ps.chars;
						num = this.ps.charPos;
						continue;
					}
					this.Throw(num, "Xml_InvalidRootData");
				}
				else if (num != this.ps.charsUsed && (!this.v1Compat || chars[num] != '\0'))
				{
					if (this.fragmentType == XmlNodeType.Document)
					{
						if (this.ParseRootLevelWhitespace())
						{
							return true;
						}
					}
					else if (this.ParseText())
					{
						goto Block_30;
					}
					chars = this.ps.chars;
					num = this.ps.charPos;
					continue;
				}
				if (this.ReadData() != 0)
				{
					num = this.ps.charPos;
					num = this.ps.charPos;
					chars = this.ps.chars;
				}
				else
				{
					if (flag)
					{
						this.Throw("Xml_InvalidRootData");
					}
					if (!this.InEntity)
					{
						goto IL_374;
					}
					if (this.HandleEntityEnd(true))
					{
						goto Block_36;
					}
				}
			}
			return true;
			Block_13:
			this.ps.charPos = num + 6;
			this.ParseCData();
			if (this.fragmentType == XmlNodeType.None)
			{
				this.fragmentType = XmlNodeType.Element;
			}
			return true;
			IL_164:
			this.fragmentType = XmlNodeType.Document;
			this.ps.charPos = num;
			this.ParseDoctypeDecl();
			return true;
			IL_1CC:
			if (this.rootElementParsed)
			{
				if (this.fragmentType == XmlNodeType.Document)
				{
					this.Throw(num, "Xml_MultipleRoots");
				}
				if (this.fragmentType == XmlNodeType.None)
				{
					this.fragmentType = XmlNodeType.Element;
				}
			}
			this.ps.charPos = num;
			this.rootElementParsed = true;
			this.ParseElement();
			return true;
			IL_279:
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.EntityReference)
			{
				this.parsingFunction = this.nextParsingFunction;
			}
			this.ParseEntityReference();
			return true;
			Block_30:
			if (this.fragmentType == XmlNodeType.None && this.curNode.type == XmlNodeType.Text)
			{
				this.fragmentType = XmlNodeType.Element;
			}
			return true;
			Block_36:
			this.SetupEndEntityNodeInContent();
			return true;
			IL_374:
			if (!this.rootElementParsed && this.fragmentType == XmlNodeType.Document)
			{
				this.ThrowWithoutLineInfo("Xml_MissingRoot");
			}
			if (this.fragmentType == XmlNodeType.None)
			{
				this.fragmentType = (this.rootElementParsed ? XmlNodeType.Document : XmlNodeType.Element);
			}
			this.OnEof();
			return false;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001E0F0 File Offset: 0x0001D0F0
		private bool ParseElementContent()
		{
			int num;
			for (;;)
			{
				num = this.ps.charPos;
				char[] chars = this.ps.chars;
				char c = chars[num];
				if (c != '&')
				{
					if (c == '<')
					{
						char c2 = chars[num + 1];
						if (c2 != '!')
						{
							if (c2 == '/')
							{
								goto IL_13B;
							}
							if (c2 == '?')
							{
								this.ps.charPos = num + 2;
								if (this.ParsePI())
								{
									break;
								}
								continue;
							}
							else if (num + 1 != this.ps.charsUsed)
							{
								goto Block_14;
							}
						}
						else
						{
							num += 2;
							if (this.ps.charsUsed - num >= 2)
							{
								if (chars[num] == '-')
								{
									if (chars[num + 1] == '-')
									{
										this.ps.charPos = num + 2;
										if (this.ParseComment())
										{
											return true;
										}
										continue;
									}
									else
									{
										this.ThrowUnexpectedToken(num + 1, "-");
									}
								}
								else if (chars[num] == '[')
								{
									num++;
									if (this.ps.charsUsed - num >= 6)
									{
										if (XmlConvert.StrEqual(chars, num, 6, "CDATA["))
										{
											goto Block_12;
										}
										this.ThrowUnexpectedToken(num, "CDATA[");
									}
								}
								else if (this.ParseUnexpectedToken(num) == "DOCTYPE")
								{
									this.Throw("Xml_BadDTDLocation");
								}
								else
								{
									this.ThrowUnexpectedToken(num, "<!--", "<[CDATA[");
								}
							}
						}
					}
					else if (num != this.ps.charsUsed)
					{
						if (this.ParseText())
						{
							return true;
						}
						continue;
					}
					if (this.ReadData() == 0)
					{
						if (this.ps.charsUsed - this.ps.charPos != 0)
						{
							this.ThrowUnclosedElements();
						}
						if (!this.InEntity)
						{
							if (this.index == 0 && this.fragmentType != XmlNodeType.Document)
							{
								goto Block_22;
							}
							this.ThrowUnclosedElements();
						}
						if (this.HandleEntityEnd(true))
						{
							goto Block_23;
						}
					}
				}
				else if (this.ParseText())
				{
					return true;
				}
			}
			return true;
			Block_12:
			this.ps.charPos = num + 6;
			this.ParseCData();
			return true;
			IL_13B:
			this.ps.charPos = num + 2;
			this.ParseEndElement();
			return true;
			Block_14:
			this.ps.charPos = num + 1;
			this.ParseElement();
			return true;
			Block_22:
			this.OnEof();
			return false;
			Block_23:
			this.SetupEndEntityNodeInContent();
			return true;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001E304 File Offset: 0x0001D304
		private void ThrowUnclosedElements()
		{
			if (this.index == 0 && this.curNode.type != XmlNodeType.Element)
			{
				this.Throw(this.ps.charsUsed, "Xml_UnexpectedEOF1");
				return;
			}
			int i = (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead) ? this.index : (this.index - 1);
			this.stringBuilder.Length = 0;
			while (i >= 0)
			{
				XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
				if (nodeData.type == XmlNodeType.Element)
				{
					this.stringBuilder.Append(nodeData.GetNameWPrefix(this.nameTable));
					if (i > 0)
					{
						this.stringBuilder.Append(", ");
					}
					else
					{
						this.stringBuilder.Append(".");
					}
				}
				i--;
			}
			this.Throw(this.ps.charsUsed, "Xml_UnexpectedEOFInElementContent", this.stringBuilder.ToString());
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001E3E0 File Offset: 0x0001D3E0
		private unsafe void ParseElement()
		{
			int num = this.ps.charPos;
			char[] chars = this.ps.chars;
			int num2 = -1;
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			while ((this.xmlCharType.charProperties[chars[num]] & 4) != 0)
			{
				num++;
				for (;;)
				{
					if ((this.xmlCharType.charProperties[chars[num]] & 8) == 0)
					{
						if (chars[num] != ':')
						{
							goto IL_A3;
						}
						if (num2 == -1)
						{
							break;
						}
						if (this.supportNamespaces)
						{
							goto Block_5;
						}
						num++;
					}
					else
					{
						num++;
					}
				}
				num2 = num;
				num++;
				continue;
				Block_5:
				this.Throw(num, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(':'));
				break;
				IL_A3:
				if (num >= this.ps.charsUsed)
				{
					break;
				}
				IL_C6:
				this.namespaceManager.PushScope();
				if (num2 == -1 || !this.supportNamespaces)
				{
					this.curNode.SetNamedNode(XmlNodeType.Element, this.nameTable.Add(chars, this.ps.charPos, num - this.ps.charPos));
				}
				else
				{
					int charPos = this.ps.charPos;
					int num3 = num2 - charPos;
					if (num3 == this.lastPrefix.Length && XmlConvert.StrEqual(chars, charPos, num3, this.lastPrefix))
					{
						this.curNode.SetNamedNode(XmlNodeType.Element, this.nameTable.Add(chars, num2 + 1, num - num2 - 1), this.lastPrefix, null);
					}
					else
					{
						this.curNode.SetNamedNode(XmlNodeType.Element, this.nameTable.Add(chars, num2 + 1, num - num2 - 1), this.nameTable.Add(chars, this.ps.charPos, num3), null);
						this.lastPrefix = this.curNode.prefix;
					}
				}
				char c = chars[num];
				bool flag = (this.xmlCharType.charProperties[c] & 1) != 0;
				if (flag)
				{
					this.ps.charPos = num;
					this.ParseAttributes();
					return;
				}
				if (c == '>')
				{
					this.ps.charPos = num + 1;
					this.parsingFunction = XmlTextReaderImpl.ParsingFunction.MoveToElementContent;
				}
				else if (c == '/')
				{
					if (num + 1 == this.ps.charsUsed)
					{
						this.ps.charPos = num;
						if (this.ReadData() == 0)
						{
							this.Throw(num, "Xml_UnexpectedEOF", ">");
						}
						num = this.ps.charPos;
						chars = this.ps.chars;
					}
					if (chars[num + 1] == '>')
					{
						this.curNode.IsEmptyElement = true;
						this.nextParsingFunction = this.parsingFunction;
						this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PopEmptyElementContext;
						this.ps.charPos = num + 2;
					}
					else
					{
						this.ThrowUnexpectedToken(num, ">");
					}
				}
				else
				{
					this.Throw(num, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(c));
				}
				if (this.addDefaultAttributesAndNormalize)
				{
					this.AddDefaultAttributesAndNormalize();
				}
				this.ElementNamespaceLookup();
				return;
			}
			num = this.ParseQName(out num2);
			chars = this.ps.chars;
			goto IL_C6;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001E6BC File Offset: 0x0001D6BC
		private void AddDefaultAttributesAndNormalize()
		{
			this.qName.Init(this.curNode.localName, this.curNode.prefix);
			SchemaInfo dtdSchemaInfo = this.dtdParserProxy.DtdSchemaInfo;
			SchemaElementDecl schemaElementDecl;
			if ((schemaElementDecl = dtdSchemaInfo.GetElementDecl(this.qName)) == null && (schemaElementDecl = (SchemaElementDecl)dtdSchemaInfo.UndeclaredElementDecls[this.qName]) == null)
			{
				return;
			}
			if (this.normalize && schemaElementDecl.HasNonCDataAttribute)
			{
				for (int i = this.index + 1; i < this.index + 1 + this.attrCount; i++)
				{
					XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
					this.qName.Init(nodeData.localName, nodeData.prefix);
					SchemaAttDef attDef = schemaElementDecl.GetAttDef(this.qName);
					if (attDef != null && attDef.SchemaType.Datatype.TokenizedType != XmlTokenizedType.CDATA)
					{
						if (this.DtdValidation && this.standalone && attDef.IsDeclaredInExternal)
						{
							string stringValue = nodeData.StringValue;
							nodeData.TrimSpacesInValue();
							if (stringValue != nodeData.StringValue)
							{
								this.SendValidationEvent(XmlSeverityType.Error, "Sch_StandAloneNormalization", nodeData.GetNameWPrefix(this.nameTable), nodeData.LineNo, nodeData.LinePos);
							}
						}
						else
						{
							nodeData.TrimSpacesInValue();
						}
					}
				}
			}
			SchemaAttDef[] defaultAttDefs = schemaElementDecl.DefaultAttDefs;
			if (defaultAttDefs != null)
			{
				int num = this.attrCount;
				XmlTextReaderImpl.NodeData[] array = null;
				if (this.attrCount >= 250)
				{
					array = new XmlTextReaderImpl.NodeData[this.attrCount];
					Array.Copy(this.nodes, this.index + 1, array, 0, this.attrCount);
					Array.Sort(array, XmlTextReaderImpl.SchemaAttDefToNodeDataComparer.Instance);
				}
				foreach (SchemaAttDef schemaAttDef in defaultAttDefs)
				{
					if (this.AddDefaultAttribute(schemaAttDef, true, array) && this.DtdValidation && this.standalone && schemaAttDef.IsDeclaredInExternal)
					{
						this.SendValidationEvent(XmlSeverityType.Error, "Sch_UnSpecifiedDefaultAttributeInExternalStandalone", schemaAttDef.Name.Name, this.curNode.LineNo, this.curNode.LinePos);
					}
				}
				if (num == 0 && this.attrNeedNamespaceLookup)
				{
					this.AttributeNamespaceLookup();
					this.attrNeedNamespaceLookup = false;
				}
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001E8E8 File Offset: 0x0001D8E8
		private unsafe void ParseEndElement()
		{
			XmlTextReaderImpl.NodeData nodeData = this.nodes[this.index - 1];
			int length = nodeData.prefix.Length;
			int length2 = nodeData.localName.Length;
			while (this.ps.charsUsed - this.ps.charPos < length + length2 + 1 && this.ReadData() != 0)
			{
			}
			char[] chars = this.ps.chars;
			int num;
			if (nodeData.prefix.Length == 0)
			{
				if (!XmlConvert.StrEqual(chars, this.ps.charPos, length2, nodeData.localName))
				{
					this.ThrowTagMismatch(nodeData);
				}
				num = length2;
			}
			else
			{
				int num2 = this.ps.charPos + length;
				if (!XmlConvert.StrEqual(chars, this.ps.charPos, length, nodeData.prefix) || chars[num2] != ':' || !XmlConvert.StrEqual(chars, num2 + 1, length2, nodeData.localName))
				{
					this.ThrowTagMismatch(nodeData);
				}
				num = length2 + length + 1;
			}
			int num3;
			for (;;)
			{
				num3 = this.ps.charPos + num;
				chars = this.ps.chars;
				if (num3 != this.ps.charsUsed)
				{
					if ((this.xmlCharType.charProperties[chars[num3]] & 8) != 0 || chars[num3] == ':')
					{
						this.ThrowTagMismatch(nodeData);
					}
					while ((this.xmlCharType.charProperties[chars[num3]] & 1) != 0)
					{
						num3++;
					}
					if (chars[num3] == '>')
					{
						break;
					}
					if (num3 != this.ps.charsUsed)
					{
						this.ThrowUnexpectedToken(num3, ">");
					}
				}
				if (this.ReadData() == 0)
				{
					this.ThrowUnclosedElements();
				}
			}
			this.index--;
			this.curNode = this.nodes[this.index];
			nodeData.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			nodeData.type = XmlNodeType.EndElement;
			this.ps.charPos = num3 + 1;
			this.nextParsingFunction = ((this.index > 0) ? this.parsingFunction : XmlTextReaderImpl.ParsingFunction.DocumentContent);
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PopElementContext;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001EAF8 File Offset: 0x0001DAF8
		private void ThrowTagMismatch(XmlTextReaderImpl.NodeData startTag)
		{
			if (startTag.type == XmlNodeType.Element)
			{
				int num2;
				int num = this.ParseQName(out num2);
				this.Throw("Xml_TagMismatch", new string[]
				{
					startTag.GetNameWPrefix(this.nameTable),
					startTag.lineInfo.lineNo.ToString(CultureInfo.InvariantCulture),
					new string(this.ps.chars, this.ps.charPos, num - this.ps.charPos)
				});
				return;
			}
			this.Throw("Xml_UnexpectedEndTag");
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001EB88 File Offset: 0x0001DB88
		private unsafe void ParseAttributes()
		{
			int num = this.ps.charPos;
			char[] chars = this.ps.chars;
			for (;;)
			{
				IL_1A:
				int num2 = 0;
				char c;
				while ((this.xmlCharType.charProperties[c = chars[num]] & 1) != 0)
				{
					if (c == '\n')
					{
						this.OnNewLine(num + 1);
						num2++;
					}
					else if (c == '\r')
					{
						if (chars[num + 1] == '\n')
						{
							this.OnNewLine(num + 2);
							num2++;
							num++;
						}
						else if (num + 1 != this.ps.charsUsed)
						{
							this.OnNewLine(num + 1);
							num2++;
						}
						else
						{
							this.ps.charPos = num;
							IL_42C:
							this.ps.lineNo = this.ps.lineNo - num2;
							if (this.ReadData() != 0)
							{
								num = this.ps.charPos;
								chars = this.ps.chars;
								goto IL_1A;
							}
							this.ThrowUnclosedElements();
							goto IL_1A;
						}
					}
					num++;
				}
				char c2;
				if ((this.xmlCharType.charProperties[c2 = chars[num]] & 4) == 0)
				{
					if (c2 == '>')
					{
						break;
					}
					if (c2 == '/')
					{
						if (num + 1 == this.ps.charsUsed)
						{
							goto IL_42C;
						}
						if (chars[num + 1] == '>')
						{
							goto Block_10;
						}
						this.ThrowUnexpectedToken(num + 1, ">");
					}
					else
					{
						if (num == this.ps.charsUsed)
						{
							goto IL_42C;
						}
						if (c2 != ':' || this.supportNamespaces)
						{
							this.Throw(num, "Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(c2));
						}
					}
				}
				if (num == this.ps.charPos)
				{
					this.Throw("Xml_ExpectingWhiteSpace", this.ParseUnexpectedToken());
				}
				this.ps.charPos = num;
				int linePos = this.ps.LinePos;
				int num3 = -1;
				num++;
				for (;;)
				{
					char c3;
					if ((this.xmlCharType.charProperties[c3 = chars[num]] & 8) == 0)
					{
						if (c3 != ':')
						{
							goto IL_23F;
						}
						if (num3 != -1)
						{
							if (this.supportNamespaces)
							{
								goto Block_17;
							}
							num++;
						}
						else
						{
							num3 = num;
							num++;
							if ((this.xmlCharType.charProperties[chars[num]] & 4) == 0)
							{
								goto IL_228;
							}
							num++;
						}
					}
					else
					{
						num++;
					}
				}
				IL_262:
				XmlTextReaderImpl.NodeData nodeData = this.AddAttribute(num, num3);
				nodeData.SetLineInfo(this.ps.LineNo, linePos);
				if (chars[num] != '=')
				{
					this.ps.charPos = num;
					this.EatWhitespaces(null);
					num = this.ps.charPos;
					if (chars[num] != '=')
					{
						this.ThrowUnexpectedToken("=");
					}
				}
				num++;
				char c4 = chars[num];
				if (c4 != '"' && c4 != '\'')
				{
					this.ps.charPos = num;
					this.EatWhitespaces(null);
					num = this.ps.charPos;
					c4 = chars[num];
					if (c4 != '"' && c4 != '\'')
					{
						this.ThrowUnexpectedToken("\"", "'");
					}
				}
				num++;
				this.ps.charPos = num;
				nodeData.quoteChar = c4;
				nodeData.SetLineInfo2(this.ps.LineNo, this.ps.LinePos);
				char c5;
				while ((this.xmlCharType.charProperties[c5 = chars[num]] & 128) != 0)
				{
					num++;
				}
				if (c5 == c4)
				{
					nodeData.SetValue(chars, this.ps.charPos, num - this.ps.charPos);
					num++;
					this.ps.charPos = num;
				}
				else
				{
					this.ParseAttributeValueSlow(num, c4, nodeData);
					num = this.ps.charPos;
					chars = this.ps.chars;
				}
				if (nodeData.prefix.Length == 0)
				{
					if (Ref.Equal(nodeData.localName, this.XmlNs))
					{
						this.OnDefaultNamespaceDecl(nodeData);
						continue;
					}
					continue;
				}
				else
				{
					if (Ref.Equal(nodeData.prefix, this.XmlNs))
					{
						this.OnNamespaceDecl(nodeData);
						continue;
					}
					if (Ref.Equal(nodeData.prefix, this.Xml))
					{
						this.OnXmlReservedAttribute(nodeData);
						continue;
					}
					continue;
				}
				Block_17:
				this.Throw(num, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(':'));
				goto IL_262;
				IL_228:
				num = this.ParseQName(out num3);
				chars = this.ps.chars;
				goto IL_262;
				IL_23F:
				if (num == this.ps.charsUsed)
				{
					num = this.ParseQName(out num3);
					chars = this.ps.chars;
					goto IL_262;
				}
				goto IL_262;
			}
			this.ps.charPos = num + 1;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.MoveToElementContent;
			goto IL_46F;
			Block_10:
			this.ps.charPos = num + 2;
			this.curNode.IsEmptyElement = true;
			this.nextParsingFunction = this.parsingFunction;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PopEmptyElementContext;
			IL_46F:
			if (this.addDefaultAttributesAndNormalize)
			{
				this.AddDefaultAttributesAndNormalize();
			}
			this.ElementNamespaceLookup();
			if (this.attrNeedNamespaceLookup)
			{
				this.AttributeNamespaceLookup();
				this.attrNeedNamespaceLookup = false;
			}
			if (this.attrDuplWalkCount >= 250)
			{
				this.AttributeDuplCheck();
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001F040 File Offset: 0x0001E040
		private void ElementNamespaceLookup()
		{
			if (this.curNode.prefix.Length == 0)
			{
				this.curNode.ns = this.xmlContext.defaultNamespace;
				return;
			}
			this.curNode.ns = this.LookupNamespace(this.curNode);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001F090 File Offset: 0x0001E090
		private void AttributeNamespaceLookup()
		{
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
				if (nodeData.type == XmlNodeType.Attribute && nodeData.prefix.Length > 0)
				{
					nodeData.ns = this.LookupNamespace(nodeData);
				}
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001F0EC File Offset: 0x0001E0EC
		private void AttributeDuplCheck()
		{
			if (this.attrCount < 250)
			{
				for (int i = this.index + 1; i < this.index + 1 + this.attrCount; i++)
				{
					XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
					for (int j = i + 1; j < this.index + 1 + this.attrCount; j++)
					{
						if (Ref.Equal(nodeData.localName, this.nodes[j].localName) && Ref.Equal(nodeData.ns, this.nodes[j].ns))
						{
							this.Throw("Xml_DupAttributeName", this.nodes[j].GetNameWPrefix(this.nameTable), this.nodes[j].LineNo, this.nodes[j].LinePos);
						}
					}
				}
				return;
			}
			if (this.attrDuplSortingArray == null || this.attrDuplSortingArray.Length < this.attrCount)
			{
				this.attrDuplSortingArray = new XmlTextReaderImpl.NodeData[this.attrCount];
			}
			Array.Copy(this.nodes, this.index + 1, this.attrDuplSortingArray, 0, this.attrCount);
			Array.Sort<XmlTextReaderImpl.NodeData>(this.attrDuplSortingArray, 0, this.attrCount);
			XmlTextReaderImpl.NodeData nodeData2 = this.attrDuplSortingArray[0];
			for (int k = 1; k < this.attrCount; k++)
			{
				XmlTextReaderImpl.NodeData nodeData3 = this.attrDuplSortingArray[k];
				if (Ref.Equal(nodeData2.localName, nodeData3.localName) && Ref.Equal(nodeData2.ns, nodeData3.ns))
				{
					this.Throw("Xml_DupAttributeName", nodeData3.GetNameWPrefix(this.nameTable), nodeData3.LineNo, nodeData3.LinePos);
				}
				nodeData2 = nodeData3;
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001F29C File Offset: 0x0001E29C
		private void OnDefaultNamespaceDecl(XmlTextReaderImpl.NodeData attr)
		{
			if (!this.supportNamespaces)
			{
				return;
			}
			string text = this.nameTable.Add(attr.StringValue);
			attr.ns = this.nameTable.Add("http://www.w3.org/2000/xmlns/");
			if (!this.curNode.xmlContextPushed)
			{
				this.PushXmlContext();
			}
			this.xmlContext.defaultNamespace = text;
			this.AddNamespace(string.Empty, text, attr);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001F308 File Offset: 0x0001E308
		private void OnNamespaceDecl(XmlTextReaderImpl.NodeData attr)
		{
			if (!this.supportNamespaces)
			{
				return;
			}
			string text = this.nameTable.Add(attr.StringValue);
			if (text.Length == 0)
			{
				this.Throw("Xml_BadNamespaceDecl", attr.lineInfo2.lineNo, attr.lineInfo2.linePos - 1);
			}
			this.AddNamespace(attr.localName, text, attr);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001F36C File Offset: 0x0001E36C
		private void OnXmlReservedAttribute(XmlTextReaderImpl.NodeData attr)
		{
			string localName;
			if ((localName = attr.localName) != null)
			{
				if (localName == "space")
				{
					if (!this.curNode.xmlContextPushed)
					{
						this.PushXmlContext();
					}
					string a;
					if ((a = XmlConvert.TrimString(attr.StringValue)) != null)
					{
						if (a == "preserve")
						{
							this.xmlContext.xmlSpace = XmlSpace.Preserve;
							return;
						}
						if (a == "default")
						{
							this.xmlContext.xmlSpace = XmlSpace.Default;
							return;
						}
					}
					this.Throw("Xml_InvalidXmlSpace", attr.StringValue, attr.lineInfo.lineNo, attr.lineInfo.linePos);
					return;
				}
				if (!(localName == "lang"))
				{
					return;
				}
				if (!this.curNode.xmlContextPushed)
				{
					this.PushXmlContext();
				}
				this.xmlContext.xmlLang = attr.StringValue;
			}
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001F448 File Offset: 0x0001E448
		private unsafe void ParseAttributeValueSlow(int curPos, char quoteChar, XmlTextReaderImpl.NodeData attr)
		{
			int num = curPos;
			char[] chars = this.ps.chars;
			int entityId = this.ps.entityId;
			int num2 = 0;
			LineInfo lineInfo = new LineInfo(this.ps.lineNo, this.ps.LinePos);
			XmlTextReaderImpl.NodeData nodeData = null;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[chars[num]] & 128) == 0)
				{
					if (num - this.ps.charPos > 0)
					{
						this.stringBuilder.Append(chars, this.ps.charPos, num - this.ps.charPos);
						this.ps.charPos = num;
					}
					if (chars[num] == quoteChar && entityId == this.ps.entityId)
					{
						goto IL_654;
					}
					char c = chars[num];
					if (c <= '"')
					{
						switch (c)
						{
						case '\t':
							num++;
							if (this.normalize)
							{
								this.stringBuilder.Append(' ');
								this.ps.charPos = this.ps.charPos + 1;
								continue;
							}
							continue;
						case '\n':
							num++;
							this.OnNewLine(num);
							if (this.normalize)
							{
								this.stringBuilder.Append(' ');
								this.ps.charPos = this.ps.charPos + 1;
								continue;
							}
							continue;
						case '\v':
						case '\f':
							goto IL_500;
						case '\r':
							if (chars[num + 1] == '\n')
							{
								num += 2;
								if (this.normalize)
								{
									this.stringBuilder.Append(this.ps.eolNormalized ? "  " : " ");
									this.ps.charPos = num;
								}
							}
							else
							{
								if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
								{
									goto IL_55F;
								}
								num++;
								if (this.normalize)
								{
									this.stringBuilder.Append(' ');
									this.ps.charPos = num;
								}
							}
							this.OnNewLine(num);
							continue;
						default:
							if (c != '"')
							{
								goto IL_500;
							}
							break;
						}
					}
					else
					{
						switch (c)
						{
						case '&':
						{
							if (num - this.ps.charPos > 0)
							{
								this.stringBuilder.Append(chars, this.ps.charPos, num - this.ps.charPos);
							}
							this.ps.charPos = num;
							int entityId2 = this.ps.entityId;
							LineInfo lineInfo2 = new LineInfo(this.ps.lineNo, this.ps.LinePos + 1);
							switch (this.HandleEntityReference(true, XmlTextReaderImpl.EntityExpandType.All, out num))
							{
							case XmlTextReaderImpl.EntityType.CharacterDec:
							case XmlTextReaderImpl.EntityType.CharacterHex:
							case XmlTextReaderImpl.EntityType.CharacterNamed:
								break;
							case XmlTextReaderImpl.EntityType.Expanded:
							case XmlTextReaderImpl.EntityType.Skipped:
								goto IL_4E3;
							case XmlTextReaderImpl.EntityType.ExpandedInAttribute:
								if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full && entityId2 == entityId)
								{
									int num3 = this.stringBuilder.Length - num2;
									if (num3 > 0)
									{
										XmlTextReaderImpl.NodeData nodeData2 = new XmlTextReaderImpl.NodeData();
										nodeData2.lineInfo = lineInfo;
										nodeData2.depth = attr.depth + 1;
										nodeData2.SetValueNode(XmlNodeType.Text, this.stringBuilder.ToString(num2, num3));
										this.AddAttributeChunkToList(attr, nodeData2, ref nodeData);
									}
									XmlTextReaderImpl.NodeData nodeData3 = new XmlTextReaderImpl.NodeData();
									nodeData3.lineInfo = lineInfo2;
									nodeData3.depth = attr.depth + 1;
									nodeData3.SetNamedNode(XmlNodeType.EntityReference, this.ps.entity.Name.Name);
									this.AddAttributeChunkToList(attr, nodeData3, ref nodeData);
									this.fullAttrCleanup = true;
								}
								num = this.ps.charPos;
								break;
							case XmlTextReaderImpl.EntityType.Unexpanded:
								if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full && this.ps.entityId == entityId)
								{
									int num4 = this.stringBuilder.Length - num2;
									if (num4 > 0)
									{
										XmlTextReaderImpl.NodeData nodeData4 = new XmlTextReaderImpl.NodeData();
										nodeData4.lineInfo = lineInfo;
										nodeData4.depth = attr.depth + 1;
										nodeData4.SetValueNode(XmlNodeType.Text, this.stringBuilder.ToString(num2, num4));
										this.AddAttributeChunkToList(attr, nodeData4, ref nodeData);
									}
									this.ps.charPos = this.ps.charPos + 1;
									string text = this.ParseEntityName();
									XmlTextReaderImpl.NodeData nodeData5 = new XmlTextReaderImpl.NodeData();
									nodeData5.lineInfo = lineInfo2;
									nodeData5.depth = attr.depth + 1;
									nodeData5.SetNamedNode(XmlNodeType.EntityReference, text);
									this.AddAttributeChunkToList(attr, nodeData5, ref nodeData);
									this.stringBuilder.Append('&');
									this.stringBuilder.Append(text);
									this.stringBuilder.Append(';');
									num2 = this.stringBuilder.Length;
									lineInfo.Set(this.ps.LineNo, this.ps.LinePos);
									this.fullAttrCleanup = true;
								}
								else
								{
									this.ps.charPos = this.ps.charPos + 1;
									this.ParseEntityName();
								}
								num = this.ps.charPos;
								break;
							default:
								goto IL_4E3;
							}
							IL_4EF:
							chars = this.ps.chars;
							continue;
							IL_4E3:
							num = this.ps.charPos;
							goto IL_4EF;
						}
						case '\'':
							break;
						default:
							switch (c)
							{
							case '<':
								this.Throw(num, "Xml_BadAttributeChar", XmlException.BuildCharExceptionStr('<'));
								goto IL_55F;
							case '=':
								goto IL_500;
							case '>':
								break;
							default:
								goto IL_500;
							}
							break;
						}
					}
					num++;
					continue;
					IL_500:
					if (num != this.ps.charsUsed)
					{
						char c2 = chars[num];
						if (c2 >= '\ud800' && c2 <= '\udbff')
						{
							if (num + 1 == this.ps.charsUsed)
							{
								goto IL_55F;
							}
							num++;
							if (chars[num] >= '\udc00' && chars[num] <= '\udfff')
							{
								num++;
								continue;
							}
						}
						this.ThrowInvalidChar(num, c2);
					}
					IL_55F:
					if (this.ReadData() == 0)
					{
						if (this.ps.charsUsed - this.ps.charPos > 0)
						{
							if (this.ps.chars[this.ps.charPos] != '\r')
							{
								this.Throw("Xml_UnexpectedEOF1");
							}
						}
						else
						{
							if (!this.InEntity)
							{
								if (this.fragmentType == XmlNodeType.Attribute)
								{
									break;
								}
								this.Throw("Xml_UnclosedQuote");
							}
							if (this.HandleEntityEnd(true))
							{
								this.Throw("Xml_InternalError");
							}
							if (entityId == this.ps.entityId)
							{
								num2 = this.stringBuilder.Length;
								lineInfo.Set(this.ps.LineNo, this.ps.LinePos);
							}
						}
					}
					num = this.ps.charPos;
					chars = this.ps.chars;
				}
				else
				{
					num++;
				}
			}
			if (entityId != this.ps.entityId)
			{
				this.Throw("Xml_EntityRefNesting");
			}
			IL_654:
			if (attr.nextAttrValueChunk != null)
			{
				int num5 = this.stringBuilder.Length - num2;
				if (num5 > 0)
				{
					XmlTextReaderImpl.NodeData nodeData6 = new XmlTextReaderImpl.NodeData();
					nodeData6.lineInfo = lineInfo;
					nodeData6.depth = attr.depth + 1;
					nodeData6.SetValueNode(XmlNodeType.Text, this.stringBuilder.ToString(num2, num5));
					this.AddAttributeChunkToList(attr, nodeData6, ref nodeData);
				}
			}
			this.ps.charPos = num + 1;
			attr.SetValue(this.stringBuilder.ToString());
			this.stringBuilder.Length = 0;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001FB30 File Offset: 0x0001EB30
		private void AddAttributeChunkToList(XmlTextReaderImpl.NodeData attr, XmlTextReaderImpl.NodeData chunk, ref XmlTextReaderImpl.NodeData lastChunk)
		{
			if (lastChunk == null)
			{
				lastChunk = chunk;
				attr.nextAttrValueChunk = chunk;
				return;
			}
			lastChunk.nextAttrValueChunk = chunk;
			lastChunk = chunk;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001FB4C File Offset: 0x0001EB4C
		private bool ParseText()
		{
			int num = 0;
			if (this.parsingMode != XmlTextReaderImpl.ParsingMode.Full)
			{
				int num2;
				int num3;
				while (!this.ParseText(out num2, out num3, ref num))
				{
				}
			}
			else
			{
				this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
				int num2;
				int num3;
				if (this.ParseText(out num2, out num3, ref num))
				{
					if (num3 - num2 != 0)
					{
						XmlNodeType textNodeType = this.GetTextNodeType(num);
						if (textNodeType != XmlNodeType.None)
						{
							this.curNode.SetValueNode(textNodeType, this.ps.chars, num2, num3 - num2);
							return true;
						}
					}
				}
				else if (this.v1Compat)
				{
					do
					{
						this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
					}
					while (!this.ParseText(out num2, out num3, ref num));
					this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
					XmlNodeType textNodeType2 = this.GetTextNodeType(num);
					if (textNodeType2 != XmlNodeType.None)
					{
						this.curNode.SetValueNode(textNodeType2, this.stringBuilder.ToString());
						this.stringBuilder.Length = 0;
						return true;
					}
					this.stringBuilder.Length = 0;
				}
				else
				{
					if (num > 32)
					{
						this.curNode.SetValueNode(XmlNodeType.Text, this.ps.chars, num2, num3 - num2);
						this.nextParsingFunction = this.parsingFunction;
						this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PartialTextValue;
						return true;
					}
					this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
					bool flag;
					do
					{
						flag = this.ParseText(out num2, out num3, ref num);
						this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
					}
					while (!flag && num <= 32 && this.stringBuilder.Length < 4096);
					XmlNodeType xmlNodeType = (this.stringBuilder.Length < 4096) ? this.GetTextNodeType(num) : XmlNodeType.Text;
					if (xmlNodeType != XmlNodeType.None)
					{
						this.curNode.SetValueNode(xmlNodeType, this.stringBuilder.ToString());
						this.stringBuilder.Length = 0;
						if (!flag)
						{
							this.nextParsingFunction = this.parsingFunction;
							this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PartialTextValue;
						}
						return true;
					}
					this.stringBuilder.Length = 0;
					if (!flag)
					{
						while (!this.ParseText(out num2, out num3, ref num))
						{
						}
					}
				}
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.ReportEndEntity)
			{
				this.SetupEndEntityNodeInContent();
				this.parsingFunction = this.nextParsingFunction;
				return true;
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.EntityReference)
			{
				this.parsingFunction = this.nextNextParsingFunction;
				this.ParseEntityReference();
				return true;
			}
			return false;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001FDBC File Offset: 0x0001EDBC
		private unsafe bool ParseText(out int startPos, out int endPos, ref int outOrChars)
		{
			char[] chars = this.ps.chars;
			int num = this.ps.charPos;
			int num2 = 0;
			int num3 = -1;
			int num4 = outOrChars;
			char c;
			int num7;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[c = chars[num]] & 64) == 0)
				{
					char c2 = c;
					if (c2 <= '&')
					{
						switch (c2)
						{
						case '\t':
							num++;
							continue;
						case '\n':
							num++;
							this.OnNewLine(num);
							continue;
						case '\v':
						case '\f':
							break;
						case '\r':
							if (chars[num + 1] == '\n')
							{
								if (!this.ps.eolNormalized && this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
								{
									if (num - this.ps.charPos > 0)
									{
										if (num2 == 0)
										{
											num2 = 1;
											num3 = num;
										}
										else
										{
											this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
											num3 = num - num2;
											num2++;
										}
									}
									else
									{
										this.ps.charPos = this.ps.charPos + 1;
									}
								}
								num += 2;
							}
							else
							{
								if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
								{
									goto IL_36A;
								}
								if (!this.ps.eolNormalized)
								{
									chars[num] = '\n';
								}
								num++;
							}
							this.OnNewLine(num);
							continue;
						default:
							if (c2 == '&')
							{
								int num6;
								XmlTextReaderImpl.EntityType entityType;
								int num5;
								if ((num5 = this.ParseCharRefInline(num, out num6, out entityType)) > 0)
								{
									if (num2 > 0)
									{
										this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
									}
									num3 = num - num2;
									num2 += num5 - num - num6;
									num = num5;
									if (!this.xmlCharType.IsWhiteSpace(chars[num5 - num6]) || (this.v1Compat && entityType == XmlTextReaderImpl.EntityType.CharacterDec))
									{
										num4 |= 255;
										continue;
									}
									continue;
								}
								else
								{
									if (num > this.ps.charPos)
									{
										goto IL_415;
									}
									switch (this.HandleEntityReference(false, XmlTextReaderImpl.EntityExpandType.All, out num))
									{
									case XmlTextReaderImpl.EntityType.CharacterDec:
										if (!this.v1Compat)
										{
											goto IL_229;
										}
										num4 |= 255;
										break;
									case XmlTextReaderImpl.EntityType.CharacterHex:
									case XmlTextReaderImpl.EntityType.CharacterNamed:
										goto IL_229;
									case XmlTextReaderImpl.EntityType.Expanded:
									case XmlTextReaderImpl.EntityType.ExpandedInAttribute:
									case XmlTextReaderImpl.EntityType.Skipped:
										goto IL_251;
									case XmlTextReaderImpl.EntityType.Unexpanded:
										goto IL_1FC;
									default:
										goto IL_251;
									}
									IL_25D:
									chars = this.ps.chars;
									continue;
									IL_251:
									num = this.ps.charPos;
									goto IL_25D;
									IL_229:
									if (!this.xmlCharType.IsWhiteSpace(this.ps.chars[num - 1]))
									{
										num4 |= 255;
										goto IL_25D;
									}
									goto IL_25D;
								}
							}
							break;
						}
					}
					else
					{
						if (c2 == '<')
						{
							goto IL_415;
						}
						if (c2 == ']')
						{
							if (this.ps.charsUsed - num >= 3 || this.ps.isEof)
							{
								if (chars[num + 1] == ']' && chars[num + 2] == '>')
								{
									this.Throw(num, "Xml_CDATAEndInText");
								}
								num4 |= 93;
								num++;
								continue;
							}
							goto IL_36A;
						}
					}
					if (num != this.ps.charsUsed)
					{
						char c3 = chars[num];
						if (c3 >= '\ud800' && c3 <= '\udbff')
						{
							if (num + 1 == this.ps.charsUsed)
							{
								goto IL_36A;
							}
							num++;
							if (chars[num] >= '\udc00' && chars[num] <= '\udfff')
							{
								num++;
								num4 |= (int)c3;
								continue;
							}
						}
						num7 = num - this.ps.charPos;
						if (this.ZeroEndingStream(num))
						{
							goto Block_31;
						}
						this.ThrowInvalidChar(this.ps.charPos + num7, c3);
					}
					IL_36A:
					if (num > this.ps.charPos)
					{
						goto IL_415;
					}
					if (this.ReadData() == 0)
					{
						if (this.ps.charsUsed - this.ps.charPos > 0)
						{
							if (this.ps.chars[this.ps.charPos] != '\r')
							{
								this.Throw("Xml_UnexpectedEOF1");
							}
						}
						else
						{
							if (!this.InEntity)
							{
								goto IL_409;
							}
							if (this.HandleEntityEnd(true))
							{
								goto Block_37;
							}
						}
					}
					num = this.ps.charPos;
					chars = this.ps.chars;
				}
				else
				{
					num4 |= (int)c;
					num++;
				}
			}
			IL_1FC:
			this.nextParsingFunction = this.parsingFunction;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.EntityReference;
			goto IL_409;
			Block_31:
			chars = this.ps.chars;
			num = this.ps.charPos + num7;
			goto IL_415;
			Block_37:
			this.nextParsingFunction = this.parsingFunction;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ReportEndEntity;
			IL_409:
			startPos = (endPos = num);
			return true;
			IL_415:
			if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full && num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
			}
			startPos = this.ps.charPos;
			endPos = num - num2;
			this.ps.charPos = num;
			outOrChars = num4;
			return c == '<';
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00020224 File Offset: 0x0001F224
		private void FinishPartialValue()
		{
			this.curNode.CopyTo(this.readValueOffset, this.stringBuilder);
			int num = 0;
			int num2;
			int num3;
			while (!this.ParseText(out num2, out num3, ref num))
			{
				this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
			}
			this.stringBuilder.Append(this.ps.chars, num2, num3 - num2);
			this.curNode.SetValue(this.stringBuilder.ToString());
			this.stringBuilder.Length = 0;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000202B0 File Offset: 0x0001F2B0
		private void FinishOtherValueIterator()
		{
			switch (this.parsingFunction)
			{
			case XmlTextReaderImpl.ParsingFunction.InReadAttributeValue:
				break;
			case XmlTextReaderImpl.ParsingFunction.InReadValueChunk:
				if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue)
				{
					this.FinishPartialValue();
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnCachedValue;
					return;
				}
				if (this.readValueOffset > 0)
				{
					this.curNode.SetValue(this.curNode.StringValue.Substring(this.readValueOffset));
					this.readValueOffset = 0;
					return;
				}
				break;
			case XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary:
			case XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary:
				switch (this.incReadState)
				{
				case XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnCachedValue:
					if (this.readValueOffset > 0)
					{
						this.curNode.SetValue(this.curNode.StringValue.Substring(this.readValueOffset));
						this.readValueOffset = 0;
						return;
					}
					break;
				case XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue:
					this.FinishPartialValue();
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnCachedValue;
					return;
				case XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_End:
					this.curNode.SetValue(string.Empty);
					break;
				default:
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0002039C File Offset: 0x0001F39C
		private void SkipPartialTextValue()
		{
			int num = 0;
			this.parsingFunction = this.nextParsingFunction;
			int num2;
			int num3;
			while (!this.ParseText(out num2, out num3, ref num))
			{
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x000203C5 File Offset: 0x0001F3C5
		private void FinishReadValueChunk()
		{
			this.readValueOffset = 0;
			if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadValueChunk_OnPartialValue)
			{
				this.SkipPartialTextValue();
				return;
			}
			this.parsingFunction = this.nextParsingFunction;
			this.nextParsingFunction = this.nextNextParsingFunction;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000203F8 File Offset: 0x0001F3F8
		private void FinishReadContentAsBinary()
		{
			this.readValueOffset = 0;
			if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue)
			{
				this.SkipPartialTextValue();
			}
			else
			{
				this.parsingFunction = this.nextParsingFunction;
				this.nextParsingFunction = this.nextNextParsingFunction;
			}
			if (this.incReadState != XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_End)
			{
				while (this.MoveToNextContentNode(true))
				{
				}
			}
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0002044C File Offset: 0x0001F44C
		private void FinishReadElementContentAsBinary()
		{
			this.FinishReadContentAsBinary();
			if (this.curNode.type != XmlNodeType.EndElement)
			{
				this.Throw("Xml_InvalidNodeType", this.curNode.type.ToString());
			}
			this.outerReader.Read();
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0002049C File Offset: 0x0001F49C
		private bool ParseRootLevelWhitespace()
		{
			XmlNodeType whitespaceType = this.GetWhitespaceType();
			if (whitespaceType == XmlNodeType.None)
			{
				this.EatWhitespaces(null);
				if (this.ps.chars[this.ps.charPos] == '<' || this.ps.charsUsed - this.ps.charPos == 0 || this.ZeroEndingStream(this.ps.charPos))
				{
					return false;
				}
			}
			else
			{
				this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
				this.EatWhitespaces(this.stringBuilder);
				if (this.ps.chars[this.ps.charPos] == '<' || this.ps.charsUsed - this.ps.charPos == 0 || this.ZeroEndingStream(this.ps.charPos))
				{
					if (this.stringBuilder.Length > 0)
					{
						this.curNode.SetValueNode(whitespaceType, this.stringBuilder.ToString());
						this.stringBuilder.Length = 0;
						return true;
					}
					return false;
				}
			}
			if (this.xmlCharType.IsCharData(this.ps.chars[this.ps.charPos]))
			{
				this.Throw("Xml_InvalidRootData");
			}
			else
			{
				this.ThrowInvalidChar(this.ps.charPos, this.ps.chars[this.ps.charPos]);
			}
			return false;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0002060C File Offset: 0x0001F60C
		private void ParseEntityReference()
		{
			this.ps.charPos = this.ps.charPos + 1;
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			this.curNode.SetNamedNode(XmlNodeType.EntityReference, this.ParseEntityName());
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00020660 File Offset: 0x0001F660
		private XmlTextReaderImpl.EntityType HandleEntityReference(bool isInAttributeValue, XmlTextReaderImpl.EntityExpandType expandType, out int charRefEndPos)
		{
			if (this.ps.charPos + 1 == this.ps.charsUsed && this.ReadData() == 0)
			{
				this.Throw("Xml_UnexpectedEOF1");
			}
			if (this.ps.chars[this.ps.charPos + 1] == '#')
			{
				XmlTextReaderImpl.EntityType result;
				charRefEndPos = this.ParseNumericCharRef(expandType != XmlTextReaderImpl.EntityExpandType.OnlyGeneral, null, out result);
				return result;
			}
			charRefEndPos = this.ParseNamedCharRef(expandType != XmlTextReaderImpl.EntityExpandType.OnlyGeneral, null);
			if (charRefEndPos >= 0)
			{
				return XmlTextReaderImpl.EntityType.CharacterNamed;
			}
			if (expandType != XmlTextReaderImpl.EntityExpandType.OnlyCharacter && (this.entityHandling == EntityHandling.ExpandEntities || (isInAttributeValue && this.validatingReaderCompatFlag)))
			{
				this.ps.charPos = this.ps.charPos + 1;
				int linePos = this.ps.LinePos;
				int num;
				try
				{
					num = this.ParseName();
				}
				catch (XmlException)
				{
					this.Throw("Xml_ErrorParsingEntityName", this.ps.LineNo, linePos);
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				if (this.ps.chars[num] != ';')
				{
					this.ThrowUnexpectedToken(num, ";");
				}
				int linePos2 = this.ps.LinePos;
				string name = this.nameTable.Add(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
				this.ps.charPos = num + 1;
				charRefEndPos = -1;
				XmlTextReaderImpl.EntityType result2 = this.HandleGeneralEntityReference(name, isInAttributeValue, false, linePos2);
				this.reportedBaseUri = this.ps.baseUriStr;
				this.reportedEncoding = this.ps.encoding;
				return result2;
			}
			return XmlTextReaderImpl.EntityType.Unexpanded;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000207FC File Offset: 0x0001F7FC
		private XmlTextReaderImpl.EntityType HandleGeneralEntityReference(string name, bool isInAttributeValue, bool pushFakeEntityIfNullResolver, int entityStartLinePos)
		{
			SchemaEntity schemaEntity = null;
			XmlQualifiedName key = new XmlQualifiedName(name);
			if (this.dtdParserProxy == null && this.fragmentParserContext != null && this.fragmentParserContext.HasDtdInfo && !this.prohibitDtd)
			{
				this.ParseDtdFromParserContext();
			}
			if (this.dtdParserProxy == null || (schemaEntity = (SchemaEntity)this.dtdParserProxy.DtdSchemaInfo.GeneralEntities[key]) == null)
			{
				if (this.disableUndeclaredEntityCheck)
				{
					schemaEntity = new SchemaEntity(new XmlQualifiedName(name), false);
					schemaEntity.Text = string.Empty;
				}
				else
				{
					this.Throw("Xml_UndeclaredEntity", name, this.ps.LineNo, entityStartLinePos);
				}
			}
			if (schemaEntity.IsProcessed)
			{
				this.Throw("Xml_RecursiveGenEntity", name, this.ps.LineNo, entityStartLinePos);
			}
			if (!schemaEntity.NData.IsEmpty)
			{
				if (this.disableUndeclaredEntityCheck)
				{
					schemaEntity = new SchemaEntity(new XmlQualifiedName(name), false);
					schemaEntity.Text = string.Empty;
				}
				else
				{
					this.Throw("Xml_UnparsedEntityRef", name, this.ps.LineNo, entityStartLinePos);
				}
			}
			if (this.standalone && schemaEntity.DeclaredInExternal)
			{
				this.Throw("Xml_ExternalEntityInStandAloneDocument", schemaEntity.Name.Name, this.ps.LineNo, entityStartLinePos);
			}
			if (schemaEntity.IsExternal)
			{
				if (isInAttributeValue)
				{
					this.Throw("Xml_ExternalEntityInAttValue", name, this.ps.LineNo, entityStartLinePos);
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				if (this.parsingMode == XmlTextReaderImpl.ParsingMode.SkipContent)
				{
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				if (this.IsResolverNull)
				{
					if (pushFakeEntityIfNullResolver)
					{
						this.PushExternalEntity(schemaEntity, ++this.nextEntityId);
						this.curNode.entityId = this.ps.entityId;
						return XmlTextReaderImpl.EntityType.FakeExpanded;
					}
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				else
				{
					this.PushExternalEntity(schemaEntity, ++this.nextEntityId);
					this.curNode.entityId = this.ps.entityId;
					if (!isInAttributeValue || !this.validatingReaderCompatFlag)
					{
						return XmlTextReaderImpl.EntityType.Expanded;
					}
					return XmlTextReaderImpl.EntityType.ExpandedInAttribute;
				}
			}
			else
			{
				if (this.parsingMode == XmlTextReaderImpl.ParsingMode.SkipContent)
				{
					return XmlTextReaderImpl.EntityType.Skipped;
				}
				int entityId = this.nextEntityId++;
				this.PushInternalEntity(schemaEntity, entityId);
				this.curNode.entityId = entityId;
				if (!isInAttributeValue || !this.validatingReaderCompatFlag)
				{
					return XmlTextReaderImpl.EntityType.Expanded;
				}
				return XmlTextReaderImpl.EntityType.ExpandedInAttribute;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x00020A2E File Offset: 0x0001FA2E
		private bool InEntity
		{
			get
			{
				return this.parsingStatesStackTop >= 0;
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00020A3C File Offset: 0x0001FA3C
		private bool HandleEntityEnd(bool checkEntityNesting)
		{
			if (this.parsingStatesStackTop == -1)
			{
				this.Throw("Xml_InternalError");
			}
			if (this.ps.entityResolvedManually)
			{
				this.index--;
				if (checkEntityNesting && this.ps.entityId != this.nodes[this.index].entityId)
				{
					this.Throw("Xml_IncompleteEntity");
				}
				this.lastEntity = this.ps.entity;
				this.PopEntity();
				this.curNode.entityId = this.ps.entityId;
				return true;
			}
			if (checkEntityNesting && this.ps.entityId != this.nodes[this.index].entityId)
			{
				this.Throw("Xml_IncompleteEntity");
			}
			this.PopEntity();
			this.curNode.entityId = this.ps.entityId;
			this.reportedEncoding = this.ps.encoding;
			this.reportedBaseUri = this.ps.baseUriStr;
			return false;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00020B40 File Offset: 0x0001FB40
		private void SetupEndEntityNodeInContent()
		{
			this.reportedEncoding = this.ps.encoding;
			this.reportedBaseUri = this.ps.baseUriStr;
			this.curNode = this.nodes[this.index];
			this.curNode.SetNamedNode(XmlNodeType.EndEntity, this.lastEntity.Name.Name);
			this.curNode.lineInfo.Set(this.ps.lineNo, this.ps.LinePos - 1);
			if (this.index == 0 && this.parsingFunction == XmlTextReaderImpl.ParsingFunction.ElementContent)
			{
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.DocumentContent;
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00020BE0 File Offset: 0x0001FBE0
		private void SetupEndEntityNodeInAttribute()
		{
			this.curNode = this.nodes[this.index + this.attrCount + 1];
			XmlTextReaderImpl.NodeData nodeData = this.curNode;
			nodeData.lineInfo.linePos = nodeData.lineInfo.linePos + this.curNode.localName.Length;
			this.curNode.type = XmlNodeType.EndEntity;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00020C3D File Offset: 0x0001FC3D
		private bool ParsePI()
		{
			return this.ParsePI(null);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00020C48 File Offset: 0x0001FC48
		private bool ParsePI(BufferBuilder piInDtdStringBuilder)
		{
			if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
			{
				this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			}
			int num = this.ParseName();
			string text = this.nameTable.Add(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
			if (string.Compare(text, "xml", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.Throw(text.Equals("xml") ? "Xml_XmlDeclNotFirst" : "Xml_InvalidPIName", text);
			}
			this.ps.charPos = num;
			if (piInDtdStringBuilder == null)
			{
				if (!this.ignorePIs && this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
				{
					this.curNode.SetNamedNode(XmlNodeType.ProcessingInstruction, text);
				}
			}
			else
			{
				piInDtdStringBuilder.Append(text);
			}
			char c = this.ps.chars[this.ps.charPos];
			if (this.EatWhitespaces(piInDtdStringBuilder) == 0)
			{
				if (this.ps.charsUsed - this.ps.charPos < 2)
				{
					this.ReadData();
				}
				if (c != '?' || this.ps.chars[this.ps.charPos + 1] != '>')
				{
					this.Throw("Xml_BadNameChar", XmlException.BuildCharExceptionStr(c));
				}
			}
			int num2;
			int num3;
			if (this.ParsePIValue(out num2, out num3))
			{
				if (piInDtdStringBuilder == null)
				{
					if (this.ignorePIs)
					{
						return false;
					}
					if (this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
					{
						this.curNode.SetValue(this.ps.chars, num2, num3 - num2);
					}
				}
				else
				{
					piInDtdStringBuilder.Append(this.ps.chars, num2, num3 - num2);
				}
			}
			else
			{
				BufferBuilder bufferBuilder;
				if (piInDtdStringBuilder == null)
				{
					if (this.ignorePIs || this.parsingMode != XmlTextReaderImpl.ParsingMode.Full)
					{
						while (!this.ParsePIValue(out num2, out num3))
						{
						}
						return false;
					}
					bufferBuilder = this.stringBuilder;
				}
				else
				{
					bufferBuilder = piInDtdStringBuilder;
				}
				do
				{
					bufferBuilder.Append(this.ps.chars, num2, num3 - num2);
				}
				while (!this.ParsePIValue(out num2, out num3));
				bufferBuilder.Append(this.ps.chars, num2, num3 - num2);
				if (piInDtdStringBuilder == null)
				{
					this.curNode.SetValue(this.stringBuilder.ToString());
					this.stringBuilder.Length = 0;
				}
			}
			return true;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00020E7C File Offset: 0x0001FE7C
		private unsafe bool ParsePIValue(out int outStartPos, out int outEndPos)
		{
			if (this.ps.charsUsed - this.ps.charPos < 2 && this.ReadData() == 0)
			{
				this.Throw(this.ps.charsUsed, "Xml_UnexpectedEOF", "PI");
			}
			int num = this.ps.charPos;
			char[] chars = this.ps.chars;
			int num2 = 0;
			int num3 = -1;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[chars[num]] & 64) == 0 || chars[num] == '?')
				{
					char c = chars[num];
					if (c <= '&')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
							num++;
							this.OnNewLine(num);
							continue;
						case '\v':
						case '\f':
							goto IL_1F2;
						case '\r':
							if (chars[num + 1] == '\n')
							{
								if (!this.ps.eolNormalized && this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
								{
									if (num - this.ps.charPos > 0)
									{
										if (num2 == 0)
										{
											num2 = 1;
											num3 = num;
										}
										else
										{
											this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
											num3 = num - num2;
											num2++;
										}
									}
									else
									{
										this.ps.charPos = this.ps.charPos + 1;
									}
								}
								num += 2;
							}
							else
							{
								if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
								{
									goto IL_256;
								}
								if (!this.ps.eolNormalized)
								{
									chars[num] = '\n';
								}
								num++;
							}
							this.OnNewLine(num);
							continue;
						default:
							if (c != '&')
							{
								goto IL_1F2;
							}
							break;
						}
					}
					else if (c != '<')
					{
						if (c != '?')
						{
							if (c != ']')
							{
								goto IL_1F2;
							}
						}
						else
						{
							if (chars[num + 1] == '>')
							{
								break;
							}
							if (num + 1 != this.ps.charsUsed)
							{
								num++;
								continue;
							}
							goto IL_256;
						}
					}
					num++;
					continue;
					IL_1F2:
					if (num == this.ps.charsUsed)
					{
						goto IL_256;
					}
					char c2 = chars[num];
					if (c2 >= '\ud800' && c2 <= '\udbff')
					{
						if (num + 1 == this.ps.charsUsed)
						{
							goto IL_256;
						}
						num++;
						if (chars[num] >= '\udc00' && chars[num] <= '\udfff')
						{
							num++;
							continue;
						}
					}
					this.ThrowInvalidChar(num, c2);
				}
				else
				{
					num++;
				}
			}
			if (num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
				outEndPos = num - num2;
			}
			else
			{
				outEndPos = num;
			}
			outStartPos = this.ps.charPos;
			this.ps.charPos = num + 2;
			return true;
			IL_256:
			if (num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
				outEndPos = num - num2;
			}
			else
			{
				outEndPos = num;
			}
			outStartPos = this.ps.charPos;
			this.ps.charPos = num;
			return false;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00021118 File Offset: 0x00020118
		private bool ParseComment()
		{
			if (this.ignoreComments)
			{
				XmlTextReaderImpl.ParsingMode parsingMode = this.parsingMode;
				this.parsingMode = XmlTextReaderImpl.ParsingMode.SkipNode;
				this.ParseCDataOrComment(XmlNodeType.Comment);
				this.parsingMode = parsingMode;
				return false;
			}
			this.ParseCDataOrComment(XmlNodeType.Comment);
			return true;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00021153 File Offset: 0x00020153
		private void ParseCData()
		{
			this.ParseCDataOrComment(XmlNodeType.CDATA);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0002115C File Offset: 0x0002015C
		private void ParseCDataOrComment(XmlNodeType type)
		{
			int num;
			int num2;
			if (this.parsingMode != XmlTextReaderImpl.ParsingMode.Full)
			{
				while (!this.ParseCDataOrComment(type, out num, out num2))
				{
				}
				return;
			}
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			if (this.ParseCDataOrComment(type, out num, out num2))
			{
				this.curNode.SetValueNode(type, this.ps.chars, num, num2 - num);
				return;
			}
			do
			{
				this.stringBuilder.Append(this.ps.chars, num, num2 - num);
			}
			while (!this.ParseCDataOrComment(type, out num, out num2));
			this.stringBuilder.Append(this.ps.chars, num, num2 - num);
			this.curNode.SetValueNode(type, this.stringBuilder.ToString());
			this.stringBuilder.Length = 0;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00021230 File Offset: 0x00020230
		private unsafe bool ParseCDataOrComment(XmlNodeType type, out int outStartPos, out int outEndPos)
		{
			if (this.ps.charsUsed - this.ps.charPos < 3 && this.ReadData() == 0)
			{
				this.Throw("Xml_UnexpectedEOF", (type == XmlNodeType.Comment) ? "Comment" : "CDATA");
			}
			int num = this.ps.charPos;
			char[] chars = this.ps.chars;
			int num2 = 0;
			int num3 = -1;
			char c = (type == XmlNodeType.Comment) ? '-' : ']';
			char c3;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[chars[num]] & 64) == 0 || chars[num] == c)
				{
					if (chars[num] == c)
					{
						if (chars[num + 1] == c)
						{
							if (chars[num + 2] == '>')
							{
								break;
							}
							if (num + 2 == this.ps.charsUsed)
							{
								goto IL_28F;
							}
							if (type == XmlNodeType.Comment)
							{
								this.Throw(num, "Xml_InvalidCommentChars");
							}
						}
						else if (num + 1 == this.ps.charsUsed)
						{
							goto IL_28F;
						}
						num++;
					}
					else
					{
						char c2 = chars[num];
						if (c2 <= '&')
						{
							switch (c2)
							{
							case '\t':
								break;
							case '\n':
								num++;
								this.OnNewLine(num);
								continue;
							case '\v':
							case '\f':
								goto IL_230;
							case '\r':
								if (chars[num + 1] == '\n')
								{
									if (!this.ps.eolNormalized && this.parsingMode == XmlTextReaderImpl.ParsingMode.Full)
									{
										if (num - this.ps.charPos > 0)
										{
											if (num2 == 0)
											{
												num2 = 1;
												num3 = num;
											}
											else
											{
												this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
												num3 = num - num2;
												num2++;
											}
										}
										else
										{
											this.ps.charPos = this.ps.charPos + 1;
										}
									}
									num += 2;
								}
								else
								{
									if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
									{
										goto IL_28F;
									}
									if (!this.ps.eolNormalized)
									{
										chars[num] = '\n';
									}
									num++;
								}
								this.OnNewLine(num);
								continue;
							default:
								if (c2 != '&')
								{
									goto IL_230;
								}
								break;
							}
						}
						else if (c2 != '<' && c2 != ']')
						{
							goto IL_230;
						}
						num++;
						continue;
						IL_230:
						if (num == this.ps.charsUsed)
						{
							goto IL_28F;
						}
						c3 = chars[num];
						if (c3 < '\ud800' || c3 > '\udbff')
						{
							goto IL_286;
						}
						if (num + 1 == this.ps.charsUsed)
						{
							goto IL_28F;
						}
						num++;
						if (chars[num] < '\udc00' || chars[num] > '\udfff')
						{
							goto IL_286;
						}
						num++;
					}
				}
				else
				{
					num++;
				}
			}
			if (num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
				outEndPos = num - num2;
			}
			else
			{
				outEndPos = num;
			}
			outStartPos = this.ps.charPos;
			this.ps.charPos = num + 3;
			return true;
			IL_286:
			this.ThrowInvalidChar(num, c3);
			IL_28F:
			if (num2 > 0)
			{
				this.ShiftBuffer(num3 + num2, num3, num - num3 - num2);
				outEndPos = num - num2;
			}
			else
			{
				outEndPos = num;
			}
			outStartPos = this.ps.charPos;
			this.ps.charPos = num;
			return false;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00021504 File Offset: 0x00020504
		private void ParseDoctypeDecl()
		{
			if (this.prohibitDtd)
			{
				this.ThrowWithoutLineInfo(this.v1Compat ? "Xml_DtdIsProhibited" : "Xml_DtdIsProhibitedEx", string.Empty);
			}
			while (this.ps.charsUsed - this.ps.charPos < 8)
			{
				if (this.ReadData() == 0)
				{
					this.Throw("Xml_UnexpectedEOF", "DOCTYPE");
				}
			}
			if (!XmlConvert.StrEqual(this.ps.chars, this.ps.charPos, 7, "DOCTYPE"))
			{
				this.ThrowUnexpectedToken((!this.rootElementParsed && this.dtdParserProxy == null) ? "DOCTYPE" : "<!--");
			}
			if (!this.xmlCharType.IsWhiteSpace(this.ps.chars[this.ps.charPos + 7]))
			{
				this.Throw("Xml_ExpectingWhiteSpace", this.ParseUnexpectedToken(this.ps.charPos + 7));
			}
			if (this.dtdParserProxy != null)
			{
				this.Throw(this.ps.charPos - 2, "Xml_MultipleDTDsProvided");
			}
			if (this.rootElementParsed)
			{
				this.Throw(this.ps.charPos - 2, "Xml_DtdAfterRootElement");
			}
			this.ps.charPos = this.ps.charPos + 8;
			this.EatWhitespaces(null);
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			this.dtdParserProxy = new XmlTextReaderImpl.DtdParserProxy(this);
			this.dtdParserProxy.Parse(true);
			SchemaInfo dtdSchemaInfo = this.dtdParserProxy.DtdSchemaInfo;
			if ((this.validatingReaderCompatFlag || !this.v1Compat) && (dtdSchemaInfo.HasDefaultAttributes || dtdSchemaInfo.HasNonCDataAttributes))
			{
				this.addDefaultAttributesAndNormalize = true;
				this.qName = new XmlQualifiedName();
			}
			this.curNode.SetNamedNode(XmlNodeType.DocumentType, dtdSchemaInfo.DocTypeName.ToString());
			this.curNode.SetValue(this.dtdParserProxy.InternalDtdSubset);
			this.nextParsingFunction = this.parsingFunction;
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.ResetAttributesRootLevel;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0002170C File Offset: 0x0002070C
		private int EatWhitespaces(BufferBuilder sb)
		{
			int num = this.ps.charPos;
			int num2 = 0;
			char[] chars = this.ps.chars;
			for (;;)
			{
				char c = chars[num];
				switch (c)
				{
				case '\t':
					break;
				case '\n':
					num++;
					this.OnNewLine(num);
					continue;
				case '\v':
				case '\f':
					goto IL_F9;
				case '\r':
					if (chars[num + 1] == '\n')
					{
						int num3 = num - this.ps.charPos;
						if (sb != null && !this.ps.eolNormalized)
						{
							if (num3 > 0)
							{
								sb.Append(chars, this.ps.charPos, num3);
								num2 += num3;
							}
							this.ps.charPos = num + 1;
						}
						num += 2;
					}
					else
					{
						if (num + 1 >= this.ps.charsUsed && !this.ps.isEof)
						{
							goto IL_14F;
						}
						if (!this.ps.eolNormalized)
						{
							chars[num] = '\n';
						}
						num++;
					}
					this.OnNewLine(num);
					continue;
				default:
					if (c != ' ')
					{
						goto IL_F9;
					}
					break;
				}
				num++;
				continue;
				IL_14F:
				int num4 = num - this.ps.charPos;
				if (num4 > 0)
				{
					if (sb != null)
					{
						sb.Append(this.ps.chars, this.ps.charPos, num4);
					}
					this.ps.charPos = num;
					num2 += num4;
				}
				if (this.ReadData() == 0)
				{
					if (this.ps.charsUsed - this.ps.charPos == 0)
					{
						return num2;
					}
					if (this.ps.chars[this.ps.charPos] != '\r')
					{
						this.Throw("Xml_UnexpectedEOF1");
					}
				}
				num = this.ps.charPos;
				chars = this.ps.chars;
				continue;
				IL_F9:
				if (num != this.ps.charsUsed)
				{
					break;
				}
				goto IL_14F;
			}
			int num5 = num - this.ps.charPos;
			if (num5 > 0)
			{
				if (sb != null)
				{
					sb.Append(this.ps.chars, this.ps.charPos, num5);
				}
				this.ps.charPos = num;
				num2 += num5;
			}
			return num2;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00021913 File Offset: 0x00020913
		private int ParseCharRefInline(int startPos, out int charCount, out XmlTextReaderImpl.EntityType entityType)
		{
			if (this.ps.chars[startPos + 1] == '#')
			{
				return this.ParseNumericCharRefInline(startPos, true, null, out charCount, out entityType);
			}
			charCount = 1;
			entityType = XmlTextReaderImpl.EntityType.CharacterNamed;
			return this.ParseNamedCharRefInline(startPos, true, null);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00021944 File Offset: 0x00020944
		private int ParseNumericCharRef(bool expand, BufferBuilder internalSubsetBuilder, out XmlTextReaderImpl.EntityType entityType)
		{
			int num3;
			int num;
			for (;;)
			{
				int num2;
				num = (num2 = this.ParseNumericCharRefInline(this.ps.charPos, expand, internalSubsetBuilder, out num3, out entityType));
				if (num2 != -2)
				{
					break;
				}
				if (this.ReadData() == 0)
				{
					this.Throw("Xml_UnexpectedEOF");
				}
			}
			if (expand)
			{
				this.ps.charPos = num - num3;
			}
			return num;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00021998 File Offset: 0x00020998
		private int ParseNumericCharRefInline(int startPos, bool expand, BufferBuilder internalSubsetBuilder, out int charCount, out XmlTextReaderImpl.EntityType entityType)
		{
			int num = 0;
			char[] chars = this.ps.chars;
			int num2 = startPos + 2;
			charCount = 0;
			string res;
			if (chars[num2] == 'x')
			{
				num2++;
				res = "Xml_BadHexEntity";
				for (;;)
				{
					char c = chars[num2];
					if (c >= '0' && c <= '9')
					{
						num = num * 16 + (int)c - 48;
					}
					else if (c >= 'a' && c <= 'f')
					{
						num = num * 16 + 10 + (int)c - 97;
					}
					else
					{
						if (c < 'A' || c > 'F')
						{
							break;
						}
						num = num * 16 + 10 + (int)c - 65;
					}
					num2++;
				}
				entityType = XmlTextReaderImpl.EntityType.CharacterHex;
			}
			else
			{
				if (num2 >= this.ps.charsUsed)
				{
					entityType = XmlTextReaderImpl.EntityType.Unexpanded;
					return -2;
				}
				res = "Xml_BadDecimalEntity";
				while (chars[num2] >= '0' && chars[num2] <= '9')
				{
					num = num * 10 + (int)chars[num2] - 48;
					num2++;
				}
				entityType = XmlTextReaderImpl.EntityType.CharacterDec;
			}
			if (chars[num2] != ';')
			{
				if (num2 == this.ps.charsUsed)
				{
					return -2;
				}
				this.Throw(num2, res);
			}
			if (num <= 65535)
			{
				char c2 = (char)num;
				if ((!this.xmlCharType.IsCharData(c2) || (c2 >= '\udc00' && c2 <= '\udeff')) && ((this.v1Compat && this.normalize) || (!this.v1Compat && this.checkCharacters)))
				{
					this.ThrowInvalidChar((this.ps.chars[this.ps.charPos + 2] == 'x') ? (this.ps.charPos + 3) : (this.ps.charPos + 2), c2);
				}
				if (expand)
				{
					if (internalSubsetBuilder != null)
					{
						internalSubsetBuilder.Append(this.ps.chars, this.ps.charPos, num2 - this.ps.charPos + 1);
					}
					chars[num2] = c2;
				}
				charCount = 1;
				return num2 + 1;
			}
			int num3 = num - 65536;
			int num4 = 56320 + num3 % 1024;
			int num5 = 55296 + num3 / 1024;
			if (this.normalize)
			{
				char c3 = (char)num5;
				if (c3 >= '\ud800' && c3 <= '\udbff')
				{
					c3 = (char)num4;
					if (c3 >= '\udc00' && c3 <= '\udfff')
					{
						goto IL_259;
					}
				}
				this.ThrowInvalidChar((this.ps.chars[this.ps.charPos + 2] == 'x') ? (this.ps.charPos + 3) : (this.ps.charPos + 2), (char)num);
			}
			IL_259:
			if (expand)
			{
				if (internalSubsetBuilder != null)
				{
					internalSubsetBuilder.Append(this.ps.chars, this.ps.charPos, num2 - this.ps.charPos + 1);
				}
				chars[num2 - 1] = (char)num5;
				chars[num2] = (char)num4;
			}
			charCount = 2;
			return num2 + 1;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00021C44 File Offset: 0x00020C44
		private int ParseNamedCharRef(bool expand, BufferBuilder internalSubsetBuilder)
		{
			int num;
			for (;;)
			{
				switch (num = this.ParseNamedCharRefInline(this.ps.charPos, expand, internalSubsetBuilder))
				{
				case -2:
					if (this.ReadData() == 0)
					{
						return -1;
					}
					continue;
				case -1:
					return -1;
				}
				break;
			}
			if (expand)
			{
				this.ps.charPos = num - 1;
			}
			return num;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00021C98 File Offset: 0x00020C98
		private int ParseNamedCharRefInline(int startPos, bool expand, BufferBuilder internalSubsetBuilder)
		{
			int num = startPos + 1;
			char[] chars = this.ps.chars;
			char c = chars[num];
			if (c <= 'g')
			{
				if (c != 'a')
				{
					if (c == 'g')
					{
						if (this.ps.charsUsed - num < 3)
						{
							return -2;
						}
						if (chars[num + 1] == 't' && chars[num + 2] == ';')
						{
							num += 3;
							char c2 = '>';
							goto IL_175;
						}
						return -1;
					}
				}
				else
				{
					num++;
					if (chars[num] == 'm')
					{
						if (this.ps.charsUsed - num < 3)
						{
							return -2;
						}
						if (chars[num + 1] == 'p' && chars[num + 2] == ';')
						{
							num += 3;
							char c2 = '&';
							goto IL_175;
						}
						return -1;
					}
					else if (chars[num] == 'p')
					{
						if (this.ps.charsUsed - num < 4)
						{
							return -2;
						}
						if (chars[num + 1] == 'o' && chars[num + 2] == 's' && chars[num + 3] == ';')
						{
							num += 4;
							char c2 = '\'';
							goto IL_175;
						}
						return -1;
					}
					else
					{
						if (num < this.ps.charsUsed)
						{
							return -1;
						}
						return -2;
					}
				}
			}
			else if (c != 'l')
			{
				if (c == 'q')
				{
					if (this.ps.charsUsed - num < 5)
					{
						return -2;
					}
					if (chars[num + 1] == 'u' && chars[num + 2] == 'o' && chars[num + 3] == 't' && chars[num + 4] == ';')
					{
						num += 5;
						char c2 = '"';
						goto IL_175;
					}
					return -1;
				}
			}
			else
			{
				if (this.ps.charsUsed - num < 3)
				{
					return -2;
				}
				if (chars[num + 1] == 't' && chars[num + 2] == ';')
				{
					num += 3;
					char c2 = '<';
					goto IL_175;
				}
				return -1;
			}
			return -1;
			IL_175:
			if (expand)
			{
				if (internalSubsetBuilder != null)
				{
					internalSubsetBuilder.Append(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
				}
				char c2;
				this.ps.chars[num - 1] = c2;
			}
			return num;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00021E5C File Offset: 0x00020E5C
		private int ParseName()
		{
			int num;
			return this.ParseQName(false, 0, out num);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00021E73 File Offset: 0x00020E73
		private int ParseQName(out int colonPos)
		{
			return this.ParseQName(true, 0, out colonPos);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00021E80 File Offset: 0x00020E80
		private unsafe int ParseQName(bool isQName, int startOffset, out int colonPos)
		{
			int num = -1;
			int num2 = this.ps.charPos + startOffset;
			for (;;)
			{
				char[] chars = this.ps.chars;
				if ((this.xmlCharType.charProperties[chars[num2]] & 4) == 0)
				{
					if (num2 == this.ps.charsUsed)
					{
						if (this.ReadDataInName(ref num2))
						{
							continue;
						}
						this.Throw(num2, "Xml_UnexpectedEOF", "Name");
					}
					if (chars[num2] != ':' || this.supportNamespaces)
					{
						this.Throw(num2, "Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(chars[num2]));
					}
				}
				num2++;
				for (;;)
				{
					if ((this.xmlCharType.charProperties[chars[num2]] & 8) == 0)
					{
						if (chars[num2] == ':')
						{
							break;
						}
						if (num2 != this.ps.charsUsed)
						{
							goto IL_111;
						}
						if (!this.ReadDataInName(ref num2))
						{
							goto IL_100;
						}
						chars = this.ps.chars;
					}
					else
					{
						num2++;
					}
				}
				if ((num != -1 || !isQName) && this.supportNamespaces)
				{
					this.Throw(num2, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(':'));
				}
				num = num2 - this.ps.charPos;
				num2++;
			}
			IL_100:
			this.Throw(num2, "Xml_UnexpectedEOF", "Name");
			IL_111:
			colonPos = ((num == -1) ? -1 : (this.ps.charPos + num));
			return num2;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00021FB8 File Offset: 0x00020FB8
		private bool ReadDataInName(ref int pos)
		{
			int num = pos - this.ps.charPos;
			bool result = this.ReadData() != 0;
			pos = this.ps.charPos + num;
			return result;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00021FF4 File Offset: 0x00020FF4
		private string ParseEntityName()
		{
			int num;
			try
			{
				num = this.ParseName();
			}
			catch (XmlException)
			{
				this.Throw("Xml_ErrorParsingEntityName");
				return null;
			}
			if (this.ps.chars[num] != ';')
			{
				this.Throw("Xml_ErrorParsingEntityName");
			}
			string result = this.nameTable.Add(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
			this.ps.charPos = num + 1;
			return result;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00022088 File Offset: 0x00021088
		private XmlTextReaderImpl.NodeData AddNode(int nodeIndex, int nodeDepth)
		{
			XmlTextReaderImpl.NodeData nodeData = this.nodes[nodeIndex];
			if (nodeData != null)
			{
				nodeData.depth = nodeDepth;
				return nodeData;
			}
			return this.AllocNode(nodeIndex, nodeDepth);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000220B4 File Offset: 0x000210B4
		private XmlTextReaderImpl.NodeData AllocNode(int nodeIndex, int nodeDepth)
		{
			if (nodeIndex >= this.nodes.Length - 1)
			{
				XmlTextReaderImpl.NodeData[] destinationArray = new XmlTextReaderImpl.NodeData[this.nodes.Length * 2];
				Array.Copy(this.nodes, 0, destinationArray, 0, this.nodes.Length);
				this.nodes = destinationArray;
			}
			XmlTextReaderImpl.NodeData nodeData = this.nodes[nodeIndex];
			if (nodeData == null)
			{
				nodeData = new XmlTextReaderImpl.NodeData();
				this.nodes[nodeIndex] = nodeData;
			}
			nodeData.depth = nodeDepth;
			return nodeData;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00022120 File Offset: 0x00021120
		private XmlTextReaderImpl.NodeData AddAttributeNoChecks(string name, int attrDepth)
		{
			XmlTextReaderImpl.NodeData nodeData = this.AddNode(this.index + this.attrCount + 1, attrDepth);
			nodeData.SetNamedNode(XmlNodeType.Attribute, this.nameTable.Add(name));
			this.attrCount++;
			return nodeData;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00022168 File Offset: 0x00021168
		private XmlTextReaderImpl.NodeData AddAttribute(int endNamePos, int colonPos)
		{
			if (colonPos == -1 || !this.supportNamespaces)
			{
				string text = this.nameTable.Add(this.ps.chars, this.ps.charPos, endNamePos - this.ps.charPos);
				return this.AddAttribute(text, string.Empty, text);
			}
			this.attrNeedNamespaceLookup = true;
			int charPos = this.ps.charPos;
			int num = colonPos - charPos;
			if (num == this.lastPrefix.Length && XmlConvert.StrEqual(this.ps.chars, charPos, num, this.lastPrefix))
			{
				return this.AddAttribute(this.nameTable.Add(this.ps.chars, colonPos + 1, endNamePos - colonPos - 1), this.lastPrefix, null);
			}
			string prefix = this.nameTable.Add(this.ps.chars, charPos, num);
			this.lastPrefix = prefix;
			return this.AddAttribute(this.nameTable.Add(this.ps.chars, colonPos + 1, endNamePos - colonPos - 1), prefix, null);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00022270 File Offset: 0x00021270
		private XmlTextReaderImpl.NodeData AddAttribute(string localName, string prefix, string nameWPrefix)
		{
			XmlTextReaderImpl.NodeData nodeData = this.AddNode(this.index + this.attrCount + 1, this.index + 1);
			nodeData.SetNamedNode(XmlNodeType.Attribute, localName, prefix, nameWPrefix);
			int num = 1 << (int)localName[0];
			if ((this.attrHashtable & num) == 0)
			{
				this.attrHashtable |= num;
			}
			else if (this.attrDuplWalkCount < 250)
			{
				this.attrDuplWalkCount++;
				for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
				{
					XmlTextReaderImpl.NodeData nodeData2 = this.nodes[i];
					if (Ref.Equal(nodeData2.localName, nodeData.localName))
					{
						this.attrDuplWalkCount = 250;
						break;
					}
				}
			}
			this.attrCount++;
			return nodeData;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00022343 File Offset: 0x00021343
		private void PopElementContext()
		{
			this.namespaceManager.PopScope();
			if (this.curNode.xmlContextPushed)
			{
				this.PopXmlContext();
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00022364 File Offset: 0x00021364
		private void OnNewLine(int pos)
		{
			this.ps.lineNo = this.ps.lineNo + 1;
			this.ps.lineStartPos = pos - 1;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00022388 File Offset: 0x00021388
		private void OnEof()
		{
			this.curNode = this.nodes[0];
			this.curNode.Clear(XmlNodeType.None);
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.Eof;
			this.readState = ReadState.EndOfFile;
			this.reportedEncoding = null;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000223E8 File Offset: 0x000213E8
		private string LookupNamespace(XmlTextReaderImpl.NodeData node)
		{
			string text = this.namespaceManager.LookupNamespace(node.prefix);
			if (text != null)
			{
				return text;
			}
			this.Throw("Xml_UnknownNs", node.prefix, node.LineNo, node.LinePos);
			return null;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0002242C File Offset: 0x0002142C
		private void AddNamespace(string prefix, string uri, XmlTextReaderImpl.NodeData attr)
		{
			if (uri == "http://www.w3.org/2000/xmlns/")
			{
				if (Ref.Equal(prefix, this.XmlNs))
				{
					this.Throw("Xml_XmlnsPrefix", attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
				}
				else
				{
					this.Throw("Xml_NamespaceDeclXmlXmlns", prefix, attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
				}
			}
			else if (uri == "http://www.w3.org/XML/1998/namespace" && !Ref.Equal(prefix, this.Xml) && !this.v1Compat)
			{
				this.Throw("Xml_NamespaceDeclXmlXmlns", prefix, attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
			}
			if (uri.Length == 0 && prefix.Length > 0)
			{
				this.Throw("Xml_BadNamespaceDecl", attr.lineInfo.lineNo, attr.lineInfo.linePos);
			}
			try
			{
				this.namespaceManager.AddNamespace(prefix, uri);
			}
			catch (ArgumentException e)
			{
				this.ReThrow(e, attr.lineInfo.lineNo, attr.lineInfo.linePos);
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00022554 File Offset: 0x00021554
		private void ResetAttributes()
		{
			if (this.fullAttrCleanup)
			{
				this.FullAttributeCleanup();
			}
			this.curAttrIndex = -1;
			this.attrCount = 0;
			this.attrHashtable = 0;
			this.attrDuplWalkCount = 0;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00022580 File Offset: 0x00021580
		private void FullAttributeCleanup()
		{
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				XmlTextReaderImpl.NodeData nodeData = this.nodes[i];
				nodeData.nextAttrValueChunk = null;
				nodeData.IsDefaultAttribute = false;
			}
			this.fullAttrCleanup = false;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x000225CC File Offset: 0x000215CC
		private void PushXmlContext()
		{
			this.xmlContext = new XmlTextReaderImpl.XmlContext(this.xmlContext);
			this.curNode.xmlContextPushed = true;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x000225EB File Offset: 0x000215EB
		private void PopXmlContext()
		{
			this.xmlContext = this.xmlContext.previousContext;
			this.curNode.xmlContextPushed = false;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0002260A File Offset: 0x0002160A
		private XmlNodeType GetWhitespaceType()
		{
			if (this.whitespaceHandling != WhitespaceHandling.None)
			{
				if (this.xmlContext.xmlSpace == XmlSpace.Preserve)
				{
					return XmlNodeType.SignificantWhitespace;
				}
				if (this.whitespaceHandling == WhitespaceHandling.All)
				{
					return XmlNodeType.Whitespace;
				}
			}
			return XmlNodeType.None;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00022632 File Offset: 0x00021632
		private XmlNodeType GetTextNodeType(int orChars)
		{
			if (orChars > 32)
			{
				return XmlNodeType.Text;
			}
			return this.GetWhitespaceType();
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00022644 File Offset: 0x00021644
		private bool PushExternalEntity(SchemaEntity entity, int entityId)
		{
			if (!this.IsResolverNull)
			{
				Uri baseUri = (entity.BaseURI.Length > 0) ? this.xmlResolver.ResolveUri(null, entity.BaseURI) : null;
				Uri uri = this.xmlResolver.ResolveUri(baseUri, entity.Url);
				Stream stream = null;
				try
				{
					stream = this.OpenStream(uri);
				}
				catch (Exception ex)
				{
					if (this.v1Compat)
					{
						throw;
					}
					this.Throw(new XmlException("Xml_ErrorOpeningExternalEntity", new string[]
					{
						uri.ToString(),
						ex.Message
					}, ex, 0, 0));
				}
				if (stream == null)
				{
					this.Throw("Xml_CannotResolveEntity", entity.Name.Name);
				}
				this.PushParsingState();
				if (this.v1Compat)
				{
					this.InitStreamInput(uri, stream, null);
				}
				else
				{
					this.InitStreamInput(uri, stream, null);
				}
				this.ps.entity = entity;
				this.ps.entityId = entityId;
				entity.IsProcessed = true;
				int charPos = this.ps.charPos;
				if (this.v1Compat)
				{
					this.EatWhitespaces(null);
				}
				if (!this.ParseXmlDeclaration(true))
				{
					this.ps.charPos = charPos;
				}
				return true;
			}
			Encoding encoding = this.ps.encoding;
			this.PushParsingState();
			this.InitStringInput(entity.Url, encoding, string.Empty);
			this.ps.entity = entity;
			this.ps.entityId = entityId;
			this.RegisterConsumedCharacters(0L, true);
			return false;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000227C4 File Offset: 0x000217C4
		private void PushInternalEntity(SchemaEntity entity, int entityId)
		{
			Encoding encoding = this.ps.encoding;
			this.PushParsingState();
			this.InitStringInput((entity.DeclaredURI != null) ? entity.DeclaredURI : string.Empty, encoding, entity.Text);
			this.ps.entity = entity;
			this.ps.entityId = entityId;
			this.ps.lineNo = entity.Line;
			this.ps.lineStartPos = -entity.Pos - 1;
			this.ps.eolNormalized = true;
			entity.IsProcessed = true;
			this.RegisterConsumedCharacters((long)entity.Text.Length, true);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00022868 File Offset: 0x00021868
		private void PopEntity()
		{
			if (this.ps.entity != null)
			{
				this.ps.entity.IsProcessed = false;
			}
			if (this.ps.stream != null)
			{
				this.ps.stream.Close();
			}
			this.PopParsingState();
			this.curNode.entityId = this.ps.entityId;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x000228CC File Offset: 0x000218CC
		private void PushParsingState()
		{
			if (this.parsingStatesStack == null)
			{
				this.parsingStatesStack = new XmlTextReaderImpl.ParsingState[2];
			}
			else if (this.parsingStatesStackTop + 1 == this.parsingStatesStack.Length)
			{
				XmlTextReaderImpl.ParsingState[] destinationArray = new XmlTextReaderImpl.ParsingState[this.parsingStatesStack.Length * 2];
				Array.Copy(this.parsingStatesStack, 0, destinationArray, 0, this.parsingStatesStack.Length);
				this.parsingStatesStack = destinationArray;
			}
			this.parsingStatesStackTop++;
			this.parsingStatesStack[this.parsingStatesStackTop] = this.ps;
			this.ps.Clear();
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00022964 File Offset: 0x00021964
		private void PopParsingState()
		{
			this.ps.Close(true);
			this.ps = this.parsingStatesStack[this.parsingStatesStackTop--];
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000229A4 File Offset: 0x000219A4
		private void InitIncrementalRead(IncrementalReadDecoder decoder)
		{
			this.ResetAttributes();
			decoder.Reset();
			this.incReadDecoder = decoder;
			this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
			this.incReadDepth = 1;
			this.incReadLeftStartPos = this.ps.charPos;
			this.incReadLeftEndPos = this.ps.charPos;
			this.incReadLineInfo.Set(this.ps.LineNo, this.ps.LinePos);
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.InIncrementalRead;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00022A20 File Offset: 0x00021A20
		private int IncrementalRead(Array array, int index, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException((this.incReadDecoder is IncrementalReadCharsDecoder) ? "buffer" : "array");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException((this.incReadDecoder is IncrementalReadCharsDecoder) ? "count" : "len");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException((this.incReadDecoder is IncrementalReadCharsDecoder) ? "index" : "offset");
			}
			if (array.Length - index < count)
			{
				throw new ArgumentException((this.incReadDecoder is IncrementalReadCharsDecoder) ? "count" : "len");
			}
			if (count == 0)
			{
				return 0;
			}
			this.curNode.lineInfo = this.incReadLineInfo;
			this.incReadDecoder.SetNextOutputBuffer(array, index, count);
			this.IncrementalRead();
			return this.incReadDecoder.DecodedCount;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00022AF8 File Offset: 0x00021AF8
		private unsafe int IncrementalRead()
		{
			int num = 0;
			int num3;
			int num4;
			int num5;
			int num7;
			for (;;)
			{
				int num2 = this.incReadLeftEndPos - this.incReadLeftStartPos;
				if (num2 > 0)
				{
					try
					{
						num3 = this.incReadDecoder.Decode(this.ps.chars, this.incReadLeftStartPos, num2);
					}
					catch (XmlException e)
					{
						this.ReThrow(e, this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
						return 0;
					}
					if (num3 < num2)
					{
						break;
					}
					this.incReadLeftStartPos = 0;
					this.incReadLeftEndPos = 0;
					this.incReadLineInfo.linePos = this.incReadLineInfo.linePos + num3;
					if (this.incReadDecoder.IsFull)
					{
						return num3;
					}
				}
				num4 = 0;
				num5 = 0;
				int num10;
				for (;;)
				{
					switch (this.incReadState)
					{
					case XmlTextReaderImpl.IncrementalReadState.Text:
					case XmlTextReaderImpl.IncrementalReadState.StartTag:
					case XmlTextReaderImpl.IncrementalReadState.Attributes:
					case XmlTextReaderImpl.IncrementalReadState.AttributeValue:
						goto IL_1E2;
					case XmlTextReaderImpl.IncrementalReadState.PI:
						if (this.ParsePIValue(out num4, out num5))
						{
							this.ps.charPos = this.ps.charPos - 2;
							this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
						}
						break;
					case XmlTextReaderImpl.IncrementalReadState.CDATA:
						if (this.ParseCDataOrComment(XmlNodeType.CDATA, out num4, out num5))
						{
							this.ps.charPos = this.ps.charPos - 3;
							this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
						}
						break;
					case XmlTextReaderImpl.IncrementalReadState.Comment:
						if (this.ParseCDataOrComment(XmlNodeType.Comment, out num4, out num5))
						{
							this.ps.charPos = this.ps.charPos - 3;
							this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
						}
						break;
					case XmlTextReaderImpl.IncrementalReadState.ReadData:
						if (this.ReadData() == 0)
						{
							this.ThrowUnclosedElements();
						}
						this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
						num4 = this.ps.charPos;
						num5 = num4;
						goto IL_1E2;
					case XmlTextReaderImpl.IncrementalReadState.EndElement:
						goto IL_182;
					case XmlTextReaderImpl.IncrementalReadState.End:
						return num;
					default:
						goto IL_1E2;
					}
					IL_6DB:
					int num6 = num5 - num4;
					if (num6 <= 0)
					{
						continue;
					}
					try
					{
						num7 = this.incReadDecoder.Decode(this.ps.chars, num4, num6);
					}
					catch (XmlException e2)
					{
						this.ReThrow(e2, this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
						return 0;
					}
					num += num7;
					if (this.incReadDecoder.IsFull)
					{
						goto Block_51;
					}
					continue;
					IL_1E2:
					char[] chars = this.ps.chars;
					num4 = this.ps.charPos;
					num5 = num4;
					int num8;
					for (;;)
					{
						this.incReadLineInfo.Set(this.ps.LineNo, this.ps.LinePos);
						if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.Attributes)
						{
							char c;
							while ((this.xmlCharType.charProperties[c = chars[num5]] & 128) != 0)
							{
								if (c == '/')
								{
									break;
								}
								num5++;
							}
						}
						else
						{
							while ((this.xmlCharType.charProperties[chars[num5]] & 128) != 0)
							{
								num5++;
							}
						}
						if (chars[num5] == '&' || chars[num5] == '\t')
						{
							num5++;
						}
						else
						{
							if (num5 - num4 > 0)
							{
								break;
							}
							char c2 = chars[num5];
							if (c2 <= '"')
							{
								if (c2 == '\n')
								{
									num5++;
									this.OnNewLine(num5);
									continue;
								}
								if (c2 == '\r')
								{
									if (chars[num5 + 1] == '\n')
									{
										num5 += 2;
									}
									else
									{
										if (num5 + 1 >= this.ps.charsUsed)
										{
											goto IL_6C7;
										}
										num5++;
									}
									this.OnNewLine(num5);
									continue;
								}
								if (c2 != '"')
								{
									goto IL_6AD;
								}
							}
							else if (c2 != '\'')
							{
								if (c2 == '/')
								{
									if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.Attributes)
									{
										if (this.ps.charsUsed - num5 < 2)
										{
											goto IL_6C7;
										}
										if (chars[num5 + 1] == '>')
										{
											this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
											this.incReadDepth--;
										}
									}
									num5++;
									continue;
								}
								switch (c2)
								{
								case '<':
								{
									if (this.incReadState != XmlTextReaderImpl.IncrementalReadState.Text)
									{
										num5++;
										continue;
									}
									if (this.ps.charsUsed - num5 < 2)
									{
										goto IL_6C7;
									}
									char c3 = chars[num5 + 1];
									if (c3 != '!')
									{
										if (c3 != '/')
										{
											if (c3 == '?')
											{
												goto Block_29;
											}
											int num9;
											num8 = this.ParseQName(true, 1, out num9);
											if (XmlConvert.StrEqual(this.ps.chars, this.ps.charPos + 1, num8 - this.ps.charPos - 1, this.curNode.localName) && (this.ps.chars[num8] == '>' || this.ps.chars[num8] == '/' || this.xmlCharType.IsWhiteSpace(this.ps.chars[num8])))
											{
												goto IL_5B1;
											}
											num5 = num8;
											num4 = this.ps.charPos;
											chars = this.ps.chars;
											continue;
										}
										else
										{
											int num11;
											num10 = this.ParseQName(true, 2, out num11);
											if (!XmlConvert.StrEqual(chars, this.ps.charPos + 2, num10 - this.ps.charPos - 2, this.curNode.GetNameWPrefix(this.nameTable)) || (this.ps.chars[num10] != '>' && !this.xmlCharType.IsWhiteSpace(this.ps.chars[num10])))
											{
												num5 = num10;
												continue;
											}
											if (--this.incReadDepth > 0)
											{
												num5 = num10 + 1;
												continue;
											}
											goto IL_4AE;
										}
									}
									else
									{
										if (this.ps.charsUsed - num5 < 4)
										{
											goto IL_6C7;
										}
										if (chars[num5 + 2] == '-' && chars[num5 + 3] == '-')
										{
											goto Block_32;
										}
										if (this.ps.charsUsed - num5 < 9)
										{
											goto IL_6C7;
										}
										if (XmlConvert.StrEqual(chars, num5 + 2, 7, "[CDATA["))
										{
											goto Block_34;
										}
										continue;
									}
									break;
								}
								case '=':
									goto IL_6AD;
								case '>':
									if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.Attributes)
									{
										this.incReadState = XmlTextReaderImpl.IncrementalReadState.Text;
									}
									num5++;
									continue;
								default:
									goto IL_6AD;
								}
							}
							switch (this.incReadState)
							{
							case XmlTextReaderImpl.IncrementalReadState.Attributes:
								this.curNode.quoteChar = chars[num5];
								this.incReadState = XmlTextReaderImpl.IncrementalReadState.AttributeValue;
								break;
							case XmlTextReaderImpl.IncrementalReadState.AttributeValue:
								if (chars[num5] == this.curNode.quoteChar)
								{
									this.incReadState = XmlTextReaderImpl.IncrementalReadState.Attributes;
								}
								break;
							}
							num5++;
							continue;
							IL_6AD:
							if (num5 == this.ps.charsUsed)
							{
								goto IL_6C7;
							}
							num5++;
						}
					}
					IL_6CE:
					this.ps.charPos = num5;
					goto IL_6DB;
					IL_6C7:
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadData;
					goto IL_6CE;
					IL_5B1:
					this.incReadDepth++;
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.Attributes;
					num5 = num8;
					goto IL_6CE;
					Block_34:
					num5 += 9;
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.CDATA;
					goto IL_6CE;
					Block_32:
					num5 += 4;
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.Comment;
					goto IL_6CE;
					Block_29:
					num5 += 2;
					this.incReadState = XmlTextReaderImpl.IncrementalReadState.PI;
					goto IL_6CE;
				}
				IL_4AE:
				this.ps.charPos = num10;
				if (this.xmlCharType.IsWhiteSpace(this.ps.chars[num10]))
				{
					this.EatWhitespaces(null);
				}
				if (this.ps.chars[this.ps.charPos] != '>')
				{
					this.ThrowUnexpectedToken(">");
				}
				this.ps.charPos = this.ps.charPos + 1;
				this.incReadState = XmlTextReaderImpl.IncrementalReadState.EndElement;
			}
			this.incReadLeftStartPos += num3;
			this.incReadLineInfo.linePos = this.incReadLineInfo.linePos + num3;
			return num3;
			IL_182:
			this.parsingFunction = XmlTextReaderImpl.ParsingFunction.PopElementContext;
			this.nextParsingFunction = ((this.index > 0 || this.fragmentType != XmlNodeType.Document) ? XmlTextReaderImpl.ParsingFunction.ElementContent : XmlTextReaderImpl.ParsingFunction.DocumentContent);
			this.outerReader.Read();
			this.incReadState = XmlTextReaderImpl.IncrementalReadState.End;
			return num;
			Block_51:
			this.incReadLeftStartPos = num4 + num7;
			this.incReadLeftEndPos = num5;
			this.incReadLineInfo.linePos = this.incReadLineInfo.linePos + num7;
			return num;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00023290 File Offset: 0x00022290
		private void FinishIncrementalRead()
		{
			this.incReadDecoder = new IncrementalReadDummyDecoder();
			this.IncrementalRead();
			this.incReadDecoder = null;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000232AC File Offset: 0x000222AC
		private bool ParseFragmentAttribute()
		{
			if (this.curNode.type == XmlNodeType.None)
			{
				this.curNode.type = XmlNodeType.Attribute;
				this.curAttrIndex = 0;
				this.ParseAttributeValueSlow(this.ps.charPos, ' ', this.curNode);
			}
			else
			{
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.InReadAttributeValue;
			}
			if (this.ReadAttributeValue())
			{
				this.parsingFunction = XmlTextReaderImpl.ParsingFunction.FragmentAttribute;
				return true;
			}
			this.OnEof();
			return false;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00023318 File Offset: 0x00022318
		private unsafe bool ParseAttributeValueChunk()
		{
			char[] chars = this.ps.chars;
			int num = this.ps.charPos;
			this.curNode = this.AddNode(this.index + this.attrCount + 1, this.index + 2);
			this.curNode.SetLineInfo(this.ps.LineNo, this.ps.LinePos);
			if (this.emptyEntityInAttributeResolved)
			{
				this.curNode.SetValueNode(XmlNodeType.Text, string.Empty);
				this.emptyEntityInAttributeResolved = false;
				return true;
			}
			for (;;)
			{
				if ((this.xmlCharType.charProperties[chars[num]] & 128) == 0)
				{
					char c = chars[num];
					if (c <= '"')
					{
						switch (c)
						{
						case '\t':
						case '\n':
							if (this.normalize)
							{
								chars[num] = ' ';
							}
							num++;
							continue;
						case '\v':
						case '\f':
							goto IL_258;
						case '\r':
							num++;
							continue;
						default:
							if (c != '"')
							{
								goto IL_258;
							}
							break;
						}
					}
					else
					{
						switch (c)
						{
						case '&':
							if (num - this.ps.charPos > 0)
							{
								this.stringBuilder.Append(chars, this.ps.charPos, num - this.ps.charPos);
							}
							this.ps.charPos = num;
							switch (this.HandleEntityReference(true, XmlTextReaderImpl.EntityExpandType.OnlyCharacter, out num))
							{
							case XmlTextReaderImpl.EntityType.CharacterDec:
							case XmlTextReaderImpl.EntityType.CharacterHex:
							case XmlTextReaderImpl.EntityType.CharacterNamed:
								chars = this.ps.chars;
								if (this.normalize && this.xmlCharType.IsWhiteSpace(chars[this.ps.charPos]) && num - this.ps.charPos == 1)
								{
									chars[this.ps.charPos] = ' ';
								}
								break;
							case XmlTextReaderImpl.EntityType.Unexpanded:
								goto IL_1F8;
							}
							chars = this.ps.chars;
							continue;
						case '\'':
							break;
						default:
							switch (c)
							{
							case '<':
								this.Throw(num, "Xml_BadAttributeChar", XmlException.BuildCharExceptionStr('<'));
								goto IL_2B3;
							case '=':
								goto IL_258;
							case '>':
								break;
							default:
								goto IL_258;
							}
							break;
						}
					}
					num++;
					continue;
					IL_258:
					if (num != this.ps.charsUsed)
					{
						char c2 = chars[num];
						if (c2 >= '\ud800' && c2 <= '\udbff')
						{
							if (num + 1 == this.ps.charsUsed)
							{
								goto IL_2B3;
							}
							num++;
							if (chars[num] >= '\udc00' && chars[num] <= '\udfff')
							{
								num++;
								continue;
							}
						}
						this.ThrowInvalidChar(num, c2);
					}
					IL_2B3:
					if (num - this.ps.charPos > 0)
					{
						this.stringBuilder.Append(chars, this.ps.charPos, num - this.ps.charPos);
						this.ps.charPos = num;
					}
					if (this.ReadData() == 0)
					{
						if (this.stringBuilder.Length > 0)
						{
							goto IL_337;
						}
						if (this.HandleEntityEnd(false))
						{
							goto Block_24;
						}
					}
					num = this.ps.charPos;
					chars = this.ps.chars;
				}
				else
				{
					num++;
				}
			}
			IL_1F8:
			if (this.stringBuilder.Length == 0)
			{
				XmlTextReaderImpl.NodeData nodeData = this.curNode;
				nodeData.lineInfo.linePos = nodeData.lineInfo.linePos + 1;
				this.ps.charPos = this.ps.charPos + 1;
				this.curNode.SetNamedNode(XmlNodeType.EntityReference, this.ParseEntityName());
				return true;
			}
			goto IL_337;
			Block_24:
			this.SetupEndEntityNodeInAttribute();
			return true;
			IL_337:
			if (num - this.ps.charPos > 0)
			{
				this.stringBuilder.Append(chars, this.ps.charPos, num - this.ps.charPos);
				this.ps.charPos = num;
			}
			this.curNode.SetValueNode(XmlNodeType.Text, this.stringBuilder.ToString());
			this.stringBuilder.Length = 0;
			return true;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x000236C0 File Offset: 0x000226C0
		private void ParseXmlDeclarationFragment()
		{
			try
			{
				this.ParseXmlDeclaration(false);
			}
			catch (XmlException ex)
			{
				this.ReThrow(ex, ex.LineNumber, ex.LinePosition - 6);
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00023700 File Offset: 0x00022700
		private void ThrowUnexpectedToken(int pos, string expectedToken)
		{
			this.ThrowUnexpectedToken(pos, expectedToken, null);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002370B File Offset: 0x0002270B
		private void ThrowUnexpectedToken(string expectedToken1)
		{
			this.ThrowUnexpectedToken(expectedToken1, null);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00023715 File Offset: 0x00022715
		private void ThrowUnexpectedToken(int pos, string expectedToken1, string expectedToken2)
		{
			this.ps.charPos = pos;
			this.ThrowUnexpectedToken(expectedToken1, expectedToken2);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0002372C File Offset: 0x0002272C
		private void ThrowUnexpectedToken(string expectedToken1, string expectedToken2)
		{
			string text = this.ParseUnexpectedToken();
			if (expectedToken2 != null)
			{
				this.Throw("Xml_UnexpectedTokens2", new string[]
				{
					text,
					expectedToken1,
					expectedToken2
				});
				return;
			}
			this.Throw("Xml_UnexpectedTokenEx", new string[]
			{
				text,
				expectedToken1
			});
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002377E File Offset: 0x0002277E
		private string ParseUnexpectedToken(int pos)
		{
			this.ps.charPos = pos;
			return this.ParseUnexpectedToken();
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00023794 File Offset: 0x00022794
		private string ParseUnexpectedToken()
		{
			if (this.xmlCharType.IsNCNameChar(this.ps.chars[this.ps.charPos]))
			{
				int num = this.ps.charPos + 1;
				while (this.xmlCharType.IsNCNameChar(this.ps.chars[num]))
				{
					num++;
				}
				return new string(this.ps.chars, this.ps.charPos, num - this.ps.charPos);
			}
			return new string(this.ps.chars, this.ps.charPos, 1);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00023838 File Offset: 0x00022838
		private int GetIndexOfAttributeWithoutPrefix(string name)
		{
			name = this.nameTable.Get(name);
			if (name == null)
			{
				return -1;
			}
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				if (Ref.Equal(this.nodes[i].localName, name) && this.nodes[i].prefix.Length == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000238A8 File Offset: 0x000228A8
		private int GetIndexOfAttributeWithPrefix(string name)
		{
			name = this.nameTable.Add(name);
			if (name == null)
			{
				return -1;
			}
			for (int i = this.index + 1; i < this.index + this.attrCount + 1; i++)
			{
				if (Ref.Equal(this.nodes[i].GetNameWPrefix(this.nameTable), name))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00023907 File Offset: 0x00022907
		private Stream OpenStream(Uri uri)
		{
			return (Stream)this.xmlResolver.GetEntity(uri, null, typeof(Stream));
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00023928 File Offset: 0x00022928
		private bool ZeroEndingStream(int pos)
		{
			if (this.v1Compat && pos == this.ps.charsUsed - 1 && this.ps.chars[pos] == '\0' && this.ReadData() == 0 && this.ps.isStreamEof)
			{
				this.ps.charsUsed = this.ps.charsUsed - 1;
				return true;
			}
			return false;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00023988 File Offset: 0x00022988
		private void ParseDtdFromParserContext()
		{
			this.dtdParserProxy = new XmlTextReaderImpl.DtdParserProxy(this.fragmentParserContext.BaseURI, this.fragmentParserContext.DocTypeName, this.fragmentParserContext.PublicId, this.fragmentParserContext.SystemId, this.fragmentParserContext.InternalSubset, this);
			this.dtdParserProxy.Parse(false);
			SchemaInfo dtdSchemaInfo = this.dtdParserProxy.DtdSchemaInfo;
			if ((this.validatingReaderCompatFlag || !this.v1Compat) && (dtdSchemaInfo.HasDefaultAttributes || dtdSchemaInfo.HasNonCDataAttributes))
			{
				this.addDefaultAttributesAndNormalize = true;
				this.qName = new XmlQualifiedName();
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00023A24 File Offset: 0x00022A24
		private bool InitReadContentAsBinary()
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InReadValueChunk)
			{
				throw new InvalidOperationException(Res.GetString("Xml_MixingReadValueChunkWithBinary"));
			}
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.InIncrementalRead)
			{
				throw new InvalidOperationException(Res.GetString("Xml_MixingV1StreamingWithV2Binary"));
			}
			if (!XmlReader.IsTextualNode(this.curNode.type) && !this.MoveToNextContentNode(false))
			{
				return false;
			}
			this.SetupReadContentAsBinaryState(XmlTextReaderImpl.ParsingFunction.InReadContentAsBinary);
			this.incReadLineInfo.Set(this.curNode.LineNo, this.curNode.LinePos);
			return true;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00023AAC File Offset: 0x00022AAC
		private bool InitReadElementContentAsBinary()
		{
			bool isEmptyElement = this.curNode.IsEmptyElement;
			this.outerReader.Read();
			if (isEmptyElement)
			{
				return false;
			}
			if (!this.MoveToNextContentNode(false))
			{
				if (this.curNode.type != XmlNodeType.EndElement)
				{
					this.Throw("Xml_InvalidNodeType", this.curNode.type.ToString());
				}
				this.outerReader.Read();
				return false;
			}
			this.SetupReadContentAsBinaryState(XmlTextReaderImpl.ParsingFunction.InReadElementContentAsBinary);
			this.incReadLineInfo.Set(this.curNode.LineNo, this.curNode.LinePos);
			return true;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00023B48 File Offset: 0x00022B48
		private bool MoveToNextContentNode(bool moveIfOnContentNode)
		{
			for (;;)
			{
				switch (this.curNode.type)
				{
				case XmlNodeType.Attribute:
					goto IL_52;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					if (!moveIfOnContentNode)
					{
						return true;
					}
					goto IL_6B;
				case XmlNodeType.EntityReference:
					this.outerReader.ResolveEntity();
					goto IL_6B;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.EndEntity:
					goto IL_6B;
				}
				break;
				IL_6B:
				moveIfOnContentNode = false;
				if (!this.outerReader.Read())
				{
					return false;
				}
			}
			return false;
			IL_52:
			return !moveIfOnContentNode;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00023BD4 File Offset: 0x00022BD4
		private void SetupReadContentAsBinaryState(XmlTextReaderImpl.ParsingFunction inReadBinaryFunction)
		{
			if (this.parsingFunction == XmlTextReaderImpl.ParsingFunction.PartialTextValue)
			{
				this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue;
			}
			else
			{
				this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnCachedValue;
				this.nextNextParsingFunction = this.nextParsingFunction;
				this.nextParsingFunction = this.parsingFunction;
			}
			this.readValueOffset = 0;
			this.parsingFunction = inReadBinaryFunction;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00023C24 File Offset: 0x00022C24
		private void SetupFromParserContext(XmlParserContext context, XmlReaderSettings settings)
		{
			XmlNameTable xmlNameTable = settings.NameTable;
			this.nameTableFromSettings = (xmlNameTable != null);
			if (context.NamespaceManager != null)
			{
				if (xmlNameTable != null && xmlNameTable != context.NamespaceManager.NameTable)
				{
					throw new XmlException("Xml_NametableMismatch");
				}
				this.namespaceManager = context.NamespaceManager;
				this.xmlContext.defaultNamespace = this.namespaceManager.LookupNamespace(string.Empty);
				xmlNameTable = this.namespaceManager.NameTable;
			}
			else if (context.NameTable != null)
			{
				if (xmlNameTable != null && xmlNameTable != context.NameTable)
				{
					throw new XmlException("Xml_NametableMismatch");
				}
				xmlNameTable = context.NameTable;
			}
			else if (xmlNameTable == null)
			{
				xmlNameTable = new NameTable();
			}
			this.nameTable = xmlNameTable;
			if (this.namespaceManager == null)
			{
				this.namespaceManager = new XmlNamespaceManager(xmlNameTable);
			}
			this.xmlContext.xmlSpace = context.XmlSpace;
			this.xmlContext.xmlLang = context.XmlLang;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00023D0C File Offset: 0x00022D0C
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x00023D24 File Offset: 0x00022D24
		internal SchemaInfo DtdSchemaInfo
		{
			get
			{
				if (this.dtdParserProxy != null)
				{
					return this.dtdParserProxy.DtdSchemaInfo;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.dtdParserProxy = new XmlTextReaderImpl.DtdParserProxy(this, value);
					if ((this.validatingReaderCompatFlag || !this.v1Compat) && (value.HasDefaultAttributes || value.HasNonCDataAttributes))
					{
						this.addDefaultAttributesAndNormalize = true;
						this.qName = new XmlQualifiedName();
						return;
					}
				}
				else
				{
					this.dtdParserProxy = null;
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00023D7B File Offset: 0x00022D7B
		internal bool XmlValidatingReaderCompatibilityMode
		{
			set
			{
				this.validatingReaderCompatFlag = value;
				if (value)
				{
					this.nameTable.Add("http://www.w3.org/2001/XMLSchema");
					this.nameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
					this.nameTable.Add("urn:schemas-microsoft-com:datatypes");
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x00023DBA File Offset: 0x00022DBA
		internal ValidationEventHandler ValidationEventHandler
		{
			set
			{
				this.validationEventHandler = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x00023DC3 File Offset: 0x00022DC3
		internal XmlNodeType FragmentType
		{
			get
			{
				return this.fragmentType;
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00023DCB File Offset: 0x00022DCB
		internal void ChangeCurrentNodeType(XmlNodeType newNodeType)
		{
			this.curNode.type = newNodeType;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00023DD9 File Offset: 0x00022DD9
		internal XmlResolver GetResolver()
		{
			if (this.IsResolverNull)
			{
				return null;
			}
			return this.xmlResolver;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00023DEB File Offset: 0x00022DEB
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x00023DF8 File Offset: 0x00022DF8
		internal object InternalSchemaType
		{
			get
			{
				return this.curNode.schemaType;
			}
			set
			{
				this.curNode.schemaType = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00023E06 File Offset: 0x00022E06
		// (set) Token: 0x06000783 RID: 1923 RVA: 0x00023E13 File Offset: 0x00022E13
		internal object InternalTypedValue
		{
			get
			{
				return this.curNode.typedValue;
			}
			set
			{
				this.curNode.typedValue = value;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x00023E21 File Offset: 0x00022E21
		internal bool StandAlone
		{
			get
			{
				return this.standalone;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x00023E29 File Offset: 0x00022E29
		internal override XmlNamespaceManager NamespaceManager
		{
			get
			{
				return this.namespaceManager;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x00023E31 File Offset: 0x00022E31
		internal bool V1Compat
		{
			get
			{
				return this.v1Compat;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00023E39 File Offset: 0x00022E39
		internal ConformanceLevel V1ComformanceLevel
		{
			get
			{
				if (this.fragmentType != XmlNodeType.Element)
				{
					return ConformanceLevel.Document;
				}
				return ConformanceLevel.Fragment;
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00023E47 File Offset: 0x00022E47
		internal bool AddDefaultAttribute(SchemaAttDef attrDef, bool definedInDtd)
		{
			return this.AddDefaultAttribute(attrDef, definedInDtd, null);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00023E54 File Offset: 0x00022E54
		private bool AddDefaultAttribute(SchemaAttDef attrDef, bool definedInDtd, XmlTextReaderImpl.NodeData[] nameSortedNodeData)
		{
			string text = attrDef.Name.Name;
			string text2 = attrDef.Prefix;
			string text3 = attrDef.Name.Namespace;
			if (definedInDtd)
			{
				if (text2.Length > 0)
				{
					this.attrNeedNamespaceLookup = true;
				}
			}
			else
			{
				text3 = this.nameTable.Add(text3);
				if (text2.Length == 0 && text3.Length > 0)
				{
					text2 = this.namespaceManager.LookupPrefix(text3);
					if (text2 == null)
					{
						text2 = string.Empty;
					}
				}
			}
			text = this.nameTable.Add(text);
			text2 = this.nameTable.Add(text2);
			if (definedInDtd && nameSortedNodeData != null)
			{
				if (Array.BinarySearch(nameSortedNodeData, attrDef, XmlTextReaderImpl.SchemaAttDefToNodeDataComparer.Instance) >= 0)
				{
					return false;
				}
			}
			else
			{
				for (int i = this.index + 1; i < this.index + 1 + this.attrCount; i++)
				{
					if (this.nodes[i].localName == text && (this.nodes[i].prefix == text2 || (this.nodes[i].ns == text3 && text3 != null)))
					{
						return false;
					}
				}
			}
			if (definedInDtd && this.DtdValidation && !attrDef.DefaultValueChecked)
			{
				attrDef.CheckDefaultValue(this.dtdParserProxy.DtdSchemaInfo, this.dtdParserProxy);
			}
			XmlTextReaderImpl.NodeData nodeData = this.AddAttribute(text, text2, (text2.Length > 0) ? null : text);
			if (!definedInDtd)
			{
				nodeData.ns = text3;
			}
			nodeData.SetValue(attrDef.DefaultValueExpanded);
			nodeData.IsDefaultAttribute = true;
			nodeData.schemaType = ((attrDef.SchemaType == null) ? attrDef.Datatype : attrDef.SchemaType);
			nodeData.typedValue = attrDef.DefaultValueTyped;
			nodeData.lineInfo.Set(attrDef.LineNum, attrDef.LinePos);
			nodeData.lineInfo2.Set(attrDef.ValueLineNum, attrDef.ValueLinePos);
			if (nodeData.prefix.Length == 0)
			{
				if (Ref.Equal(nodeData.localName, this.XmlNs))
				{
					this.OnDefaultNamespaceDecl(nodeData);
					if (!definedInDtd && this.nodes[this.index].prefix.Length == 0)
					{
						this.nodes[this.index].ns = this.xmlContext.defaultNamespace;
					}
				}
			}
			else if (Ref.Equal(nodeData.prefix, this.XmlNs))
			{
				this.OnNamespaceDecl(nodeData);
				if (!definedInDtd)
				{
					string localName = nodeData.localName;
					for (int j = this.index; j < this.index + this.attrCount + 1; j++)
					{
						if (this.nodes[j].prefix.Equals(localName))
						{
							this.nodes[j].ns = this.namespaceManager.LookupNamespace(localName);
						}
					}
				}
			}
			else if (attrDef.Reserved != SchemaAttDef.Reserve.None)
			{
				this.OnXmlReservedAttribute(nodeData);
			}
			this.fullAttrCleanup = true;
			return true;
		}

		// Token: 0x17000158 RID: 344
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x00024113 File Offset: 0x00023113
		internal bool DisableUndeclaredEntityCheck
		{
			set
			{
				this.disableUndeclaredEntityCheck = value;
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002411C File Offset: 0x0002311C
		private int ReadContentAsBinary(byte[] buffer, int index, int count)
		{
			if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_End)
			{
				return 0;
			}
			this.incReadDecoder.SetNextOutputBuffer(buffer, index, count);
			int num;
			int num2;
			int num3;
			XmlTextReaderImpl.ParsingFunction inReadBinaryFunction;
			for (;;)
			{
				num = 0;
				try
				{
					num = this.curNode.CopyToBinary(this.incReadDecoder, this.readValueOffset);
				}
				catch (XmlException e)
				{
					this.curNode.AdjustLineInfo(this.readValueOffset, this.ps.eolNormalized, ref this.incReadLineInfo);
					this.ReThrow(e, this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
				}
				this.readValueOffset += num;
				if (this.incReadDecoder.IsFull)
				{
					break;
				}
				if (this.incReadState == XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue)
				{
					this.curNode.SetValue(string.Empty);
					bool flag = false;
					num2 = 0;
					num3 = 0;
					while (!this.incReadDecoder.IsFull && !flag)
					{
						int num4 = 0;
						this.incReadLineInfo.Set(this.ps.LineNo, this.ps.LinePos);
						flag = this.ParseText(out num2, out num3, ref num4);
						try
						{
							num = this.incReadDecoder.Decode(this.ps.chars, num2, num3 - num2);
						}
						catch (XmlException e2)
						{
							this.ReThrow(e2, this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
						}
						num2 += num;
					}
					this.incReadState = (flag ? XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnCachedValue : XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_OnPartialValue);
					this.readValueOffset = 0;
					if (this.incReadDecoder.IsFull)
					{
						goto Block_8;
					}
				}
				inReadBinaryFunction = this.parsingFunction;
				this.parsingFunction = this.nextParsingFunction;
				this.nextParsingFunction = this.nextNextParsingFunction;
				if (!this.MoveToNextContentNode(true))
				{
					goto Block_9;
				}
				this.SetupReadContentAsBinaryState(inReadBinaryFunction);
				this.incReadLineInfo.Set(this.curNode.LineNo, this.curNode.LinePos);
			}
			return this.incReadDecoder.DecodedCount;
			Block_8:
			this.curNode.SetValue(this.ps.chars, num2, num3 - num2);
			XmlTextReaderImpl.AdjustLineInfo(this.ps.chars, num2 - num, num2, this.ps.eolNormalized, ref this.incReadLineInfo);
			this.curNode.SetLineInfo(this.incReadLineInfo.lineNo, this.incReadLineInfo.linePos);
			return this.incReadDecoder.DecodedCount;
			Block_9:
			this.SetupReadContentAsBinaryState(inReadBinaryFunction);
			this.incReadState = XmlTextReaderImpl.IncrementalReadState.ReadContentAsBinary_End;
			return this.incReadDecoder.DecodedCount;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00024394 File Offset: 0x00023394
		private int ReadElementContentAsBinary(byte[] buffer, int index, int count)
		{
			if (count == 0)
			{
				return 0;
			}
			int num = this.ReadContentAsBinary(buffer, index, count);
			if (num > 0)
			{
				return num;
			}
			if (this.curNode.type != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.curNode.type.ToString(), this);
			}
			this.parsingFunction = this.nextParsingFunction;
			this.nextParsingFunction = this.nextNextParsingFunction;
			this.outerReader.Read();
			return 0;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0002440B File Offset: 0x0002340B
		private void InitBase64Decoder()
		{
			if (this.base64Decoder == null)
			{
				this.base64Decoder = new Base64Decoder();
			}
			else
			{
				this.base64Decoder.Reset();
			}
			this.incReadDecoder = this.base64Decoder;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00024439 File Offset: 0x00023439
		private void InitBinHexDecoder()
		{
			if (this.binHexDecoder == null)
			{
				this.binHexDecoder = new BinHexDecoder();
			}
			else
			{
				this.binHexDecoder.Reset();
			}
			this.incReadDecoder = this.binHexDecoder;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00024468 File Offset: 0x00023468
		private bool UriEqual(Uri uri1, string uri1Str, string uri2Str, XmlResolver resolver)
		{
			if (uri1 == null || resolver == null)
			{
				return uri1Str == uri2Str;
			}
			Uri obj = resolver.ResolveUri(null, uri2Str);
			return uri1.Equals(obj);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002449C File Offset: 0x0002349C
		private void RegisterConsumedCharacters(long characters, bool inEntityReference)
		{
			if (this.maxCharactersInDocument > 0L)
			{
				long num = this.charactersInDocument + characters;
				if (num < this.charactersInDocument)
				{
					this.ThrowWithoutLineInfo("XmlSerializeErrorDetails", new string[]
					{
						"MaxCharactersInDocument",
						""
					});
				}
				else
				{
					this.charactersInDocument = num;
				}
				if (this.charactersInDocument > this.maxCharactersInDocument)
				{
					this.ThrowWithoutLineInfo("XmlSerializeErrorDetails", new string[]
					{
						"MaxCharactersInDocument",
						""
					});
				}
			}
			if (this.maxCharactersFromEntities > 0L && inEntityReference)
			{
				long num2 = this.charactersFromEntities + characters;
				if (num2 < this.charactersFromEntities)
				{
					this.ThrowWithoutLineInfo("XmlSerializeErrorDetails", new string[]
					{
						"MaxCharactersFromEntities",
						""
					});
				}
				else
				{
					this.charactersFromEntities = num2;
				}
				if (this.charactersFromEntities > this.maxCharactersFromEntities && XmlTextReaderSection.LimitCharactersFromEntities)
				{
					this.ThrowWithoutLineInfo("XmlSerializeErrorDetails", new string[]
					{
						"MaxCharactersFromEntities",
						""
					});
				}
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000245B0 File Offset: 0x000235B0
		internal static void AdjustLineInfo(char[] chars, int startPos, int endPos, bool isNormalized, ref LineInfo lineInfo)
		{
			int num = -1;
			for (int i = startPos; i < endPos; i++)
			{
				char c = chars[i];
				if (c != '\n')
				{
					if (c == '\r')
					{
						if (!isNormalized)
						{
							lineInfo.lineNo++;
							num = i;
							if (i + 1 < endPos && chars[i + 1] == '\n')
							{
								i++;
								num++;
							}
						}
					}
				}
				else
				{
					lineInfo.lineNo++;
					num = i;
				}
			}
			if (num >= 0)
			{
				lineInfo.linePos = endPos - num;
			}
		}

		// Token: 0x04000681 RID: 1665
		private const int MaxBytesToMove = 128;

		// Token: 0x04000682 RID: 1666
		private const int ApproxXmlDeclLength = 80;

		// Token: 0x04000683 RID: 1667
		private const int NodesInitialSize = 8;

		// Token: 0x04000684 RID: 1668
		private const int InitialAttributesCount = 4;

		// Token: 0x04000685 RID: 1669
		private const int InitialParsingStateStackSize = 2;

		// Token: 0x04000686 RID: 1670
		private const int InitialParsingStatesDepth = 2;

		// Token: 0x04000687 RID: 1671
		private const int DtdChidrenInitialSize = 2;

		// Token: 0x04000688 RID: 1672
		private const int MaxByteSequenceLen = 6;

		// Token: 0x04000689 RID: 1673
		private const int MaxAttrDuplWalkCount = 250;

		// Token: 0x0400068A RID: 1674
		private const int MinWhitespaceLookahedCount = 4096;

		// Token: 0x0400068B RID: 1675
		private const string XmlDeclarationBegining = "<?xml";

		// Token: 0x0400068C RID: 1676
		internal const int SurHighStart = 55296;

		// Token: 0x0400068D RID: 1677
		internal const int SurHighEnd = 56319;

		// Token: 0x0400068E RID: 1678
		internal const int SurLowStart = 56320;

		// Token: 0x0400068F RID: 1679
		internal const int SurLowEnd = 57343;

		// Token: 0x04000690 RID: 1680
		private XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x04000691 RID: 1681
		private XmlTextReaderImpl.ParsingState ps;

		// Token: 0x04000692 RID: 1682
		private XmlTextReaderImpl.ParsingFunction parsingFunction;

		// Token: 0x04000693 RID: 1683
		private XmlTextReaderImpl.ParsingFunction nextParsingFunction;

		// Token: 0x04000694 RID: 1684
		private XmlTextReaderImpl.ParsingFunction nextNextParsingFunction;

		// Token: 0x04000695 RID: 1685
		private XmlTextReaderImpl.NodeData[] nodes;

		// Token: 0x04000696 RID: 1686
		private XmlTextReaderImpl.NodeData curNode;

		// Token: 0x04000697 RID: 1687
		private int index;

		// Token: 0x04000698 RID: 1688
		private int curAttrIndex = -1;

		// Token: 0x04000699 RID: 1689
		private int attrCount;

		// Token: 0x0400069A RID: 1690
		private int attrHashtable;

		// Token: 0x0400069B RID: 1691
		private int attrDuplWalkCount;

		// Token: 0x0400069C RID: 1692
		private bool attrNeedNamespaceLookup;

		// Token: 0x0400069D RID: 1693
		private bool fullAttrCleanup;

		// Token: 0x0400069E RID: 1694
		private XmlTextReaderImpl.NodeData[] attrDuplSortingArray;

		// Token: 0x0400069F RID: 1695
		private XmlNameTable nameTable;

		// Token: 0x040006A0 RID: 1696
		private bool nameTableFromSettings;

		// Token: 0x040006A1 RID: 1697
		private XmlResolver xmlResolver;

		// Token: 0x040006A2 RID: 1698
		private string url = string.Empty;

		// Token: 0x040006A3 RID: 1699
		private CompressedStack compressedStack;

		// Token: 0x040006A4 RID: 1700
		private bool normalize;

		// Token: 0x040006A5 RID: 1701
		private bool supportNamespaces = true;

		// Token: 0x040006A6 RID: 1702
		private WhitespaceHandling whitespaceHandling;

		// Token: 0x040006A7 RID: 1703
		private bool prohibitDtd;

		// Token: 0x040006A8 RID: 1704
		private EntityHandling entityHandling;

		// Token: 0x040006A9 RID: 1705
		private bool ignorePIs;

		// Token: 0x040006AA RID: 1706
		private bool ignoreComments;

		// Token: 0x040006AB RID: 1707
		private bool checkCharacters;

		// Token: 0x040006AC RID: 1708
		private int lineNumberOffset;

		// Token: 0x040006AD RID: 1709
		private int linePositionOffset;

		// Token: 0x040006AE RID: 1710
		private bool closeInput;

		// Token: 0x040006AF RID: 1711
		private long maxCharactersInDocument;

		// Token: 0x040006B0 RID: 1712
		private long maxCharactersFromEntities;

		// Token: 0x040006B1 RID: 1713
		private bool v1Compat;

		// Token: 0x040006B2 RID: 1714
		private XmlNamespaceManager namespaceManager;

		// Token: 0x040006B3 RID: 1715
		private string lastPrefix = string.Empty;

		// Token: 0x040006B4 RID: 1716
		private XmlTextReaderImpl.XmlContext xmlContext;

		// Token: 0x040006B5 RID: 1717
		private XmlTextReaderImpl.ParsingState[] parsingStatesStack;

		// Token: 0x040006B6 RID: 1718
		private int parsingStatesStackTop = -1;

		// Token: 0x040006B7 RID: 1719
		private string reportedBaseUri;

		// Token: 0x040006B8 RID: 1720
		private Encoding reportedEncoding;

		// Token: 0x040006B9 RID: 1721
		private XmlTextReaderImpl.DtdParserProxy dtdParserProxy;

		// Token: 0x040006BA RID: 1722
		private XmlNodeType fragmentType = XmlNodeType.Document;

		// Token: 0x040006BB RID: 1723
		private bool fragment;

		// Token: 0x040006BC RID: 1724
		private XmlParserContext fragmentParserContext;

		// Token: 0x040006BD RID: 1725
		private IncrementalReadDecoder incReadDecoder;

		// Token: 0x040006BE RID: 1726
		private XmlTextReaderImpl.IncrementalReadState incReadState;

		// Token: 0x040006BF RID: 1727
		private int incReadDepth;

		// Token: 0x040006C0 RID: 1728
		private int incReadLeftStartPos;

		// Token: 0x040006C1 RID: 1729
		private int incReadLeftEndPos;

		// Token: 0x040006C2 RID: 1730
		private LineInfo incReadLineInfo;

		// Token: 0x040006C3 RID: 1731
		private IncrementalReadCharsDecoder readCharsDecoder;

		// Token: 0x040006C4 RID: 1732
		private BinHexDecoder binHexDecoder;

		// Token: 0x040006C5 RID: 1733
		private Base64Decoder base64Decoder;

		// Token: 0x040006C6 RID: 1734
		private int attributeValueBaseEntityId;

		// Token: 0x040006C7 RID: 1735
		private bool emptyEntityInAttributeResolved;

		// Token: 0x040006C8 RID: 1736
		private ValidationEventHandler validationEventHandler;

		// Token: 0x040006C9 RID: 1737
		private bool validatingReaderCompatFlag;

		// Token: 0x040006CA RID: 1738
		private bool addDefaultAttributesAndNormalize;

		// Token: 0x040006CB RID: 1739
		private XmlQualifiedName qName;

		// Token: 0x040006CC RID: 1740
		private BufferBuilder stringBuilder;

		// Token: 0x040006CD RID: 1741
		private bool rootElementParsed;

		// Token: 0x040006CE RID: 1742
		private bool standalone;

		// Token: 0x040006CF RID: 1743
		private int nextEntityId = 1;

		// Token: 0x040006D0 RID: 1744
		private XmlTextReaderImpl.ParsingMode parsingMode;

		// Token: 0x040006D1 RID: 1745
		private ReadState readState;

		// Token: 0x040006D2 RID: 1746
		private SchemaEntity lastEntity;

		// Token: 0x040006D3 RID: 1747
		private bool afterResetState;

		// Token: 0x040006D4 RID: 1748
		private int documentStartBytePos;

		// Token: 0x040006D5 RID: 1749
		private int readValueOffset;

		// Token: 0x040006D6 RID: 1750
		private long charactersInDocument;

		// Token: 0x040006D7 RID: 1751
		private long charactersFromEntities;

		// Token: 0x040006D8 RID: 1752
		private bool disableUndeclaredEntityCheck;

		// Token: 0x040006D9 RID: 1753
		private XmlReader outerReader;

		// Token: 0x040006DA RID: 1754
		private bool xmlResolverIsSet;

		// Token: 0x040006DB RID: 1755
		private string Xml;

		// Token: 0x040006DC RID: 1756
		private string XmlNs;

		// Token: 0x02000086 RID: 134
		private enum ParsingFunction
		{
			// Token: 0x040006DE RID: 1758
			ElementContent,
			// Token: 0x040006DF RID: 1759
			NoData,
			// Token: 0x040006E0 RID: 1760
			OpenUrl,
			// Token: 0x040006E1 RID: 1761
			SwitchToInteractive,
			// Token: 0x040006E2 RID: 1762
			SwitchToInteractiveXmlDecl,
			// Token: 0x040006E3 RID: 1763
			DocumentContent,
			// Token: 0x040006E4 RID: 1764
			MoveToElementContent,
			// Token: 0x040006E5 RID: 1765
			PopElementContext,
			// Token: 0x040006E6 RID: 1766
			PopEmptyElementContext,
			// Token: 0x040006E7 RID: 1767
			ResetAttributesRootLevel,
			// Token: 0x040006E8 RID: 1768
			Error,
			// Token: 0x040006E9 RID: 1769
			Eof,
			// Token: 0x040006EA RID: 1770
			ReaderClosed,
			// Token: 0x040006EB RID: 1771
			EntityReference,
			// Token: 0x040006EC RID: 1772
			InIncrementalRead,
			// Token: 0x040006ED RID: 1773
			FragmentAttribute,
			// Token: 0x040006EE RID: 1774
			ReportEndEntity,
			// Token: 0x040006EF RID: 1775
			AfterResolveEntityInContent,
			// Token: 0x040006F0 RID: 1776
			AfterResolveEmptyEntityInContent,
			// Token: 0x040006F1 RID: 1777
			XmlDeclarationFragment,
			// Token: 0x040006F2 RID: 1778
			GoToEof,
			// Token: 0x040006F3 RID: 1779
			PartialTextValue,
			// Token: 0x040006F4 RID: 1780
			InReadAttributeValue,
			// Token: 0x040006F5 RID: 1781
			InReadValueChunk,
			// Token: 0x040006F6 RID: 1782
			InReadContentAsBinary,
			// Token: 0x040006F7 RID: 1783
			InReadElementContentAsBinary
		}

		// Token: 0x02000087 RID: 135
		private enum ParsingMode
		{
			// Token: 0x040006F9 RID: 1785
			Full,
			// Token: 0x040006FA RID: 1786
			SkipNode,
			// Token: 0x040006FB RID: 1787
			SkipContent
		}

		// Token: 0x02000088 RID: 136
		private enum EntityType
		{
			// Token: 0x040006FD RID: 1789
			CharacterDec,
			// Token: 0x040006FE RID: 1790
			CharacterHex,
			// Token: 0x040006FF RID: 1791
			CharacterNamed,
			// Token: 0x04000700 RID: 1792
			Expanded,
			// Token: 0x04000701 RID: 1793
			ExpandedInAttribute,
			// Token: 0x04000702 RID: 1794
			Skipped,
			// Token: 0x04000703 RID: 1795
			Unexpanded,
			// Token: 0x04000704 RID: 1796
			FakeExpanded
		}

		// Token: 0x02000089 RID: 137
		private enum EntityExpandType
		{
			// Token: 0x04000706 RID: 1798
			OnlyGeneral,
			// Token: 0x04000707 RID: 1799
			OnlyCharacter,
			// Token: 0x04000708 RID: 1800
			All
		}

		// Token: 0x0200008A RID: 138
		private enum IncrementalReadState
		{
			// Token: 0x0400070A RID: 1802
			Text,
			// Token: 0x0400070B RID: 1803
			StartTag,
			// Token: 0x0400070C RID: 1804
			PI,
			// Token: 0x0400070D RID: 1805
			CDATA,
			// Token: 0x0400070E RID: 1806
			Comment,
			// Token: 0x0400070F RID: 1807
			Attributes,
			// Token: 0x04000710 RID: 1808
			AttributeValue,
			// Token: 0x04000711 RID: 1809
			ReadData,
			// Token: 0x04000712 RID: 1810
			EndElement,
			// Token: 0x04000713 RID: 1811
			End,
			// Token: 0x04000714 RID: 1812
			ReadValueChunk_OnCachedValue,
			// Token: 0x04000715 RID: 1813
			ReadValueChunk_OnPartialValue,
			// Token: 0x04000716 RID: 1814
			ReadContentAsBinary_OnCachedValue,
			// Token: 0x04000717 RID: 1815
			ReadContentAsBinary_OnPartialValue,
			// Token: 0x04000718 RID: 1816
			ReadContentAsBinary_End
		}

		// Token: 0x0200008B RID: 139
		private struct ParsingState
		{
			// Token: 0x06000792 RID: 1938 RVA: 0x00024628 File Offset: 0x00023628
			internal void Clear()
			{
				this.chars = null;
				this.charPos = 0;
				this.charsUsed = 0;
				this.encoding = null;
				this.stream = null;
				this.decoder = null;
				this.bytes = null;
				this.bytePos = 0;
				this.bytesUsed = 0;
				this.textReader = null;
				this.lineNo = 1;
				this.lineStartPos = -1;
				this.baseUriStr = string.Empty;
				this.baseUri = null;
				this.isEof = false;
				this.isStreamEof = false;
				this.eolNormalized = true;
				this.entityResolvedManually = false;
			}

			// Token: 0x06000793 RID: 1939 RVA: 0x000246B7 File Offset: 0x000236B7
			internal void Close(bool closeInput)
			{
				if (closeInput)
				{
					if (this.stream != null)
					{
						this.stream.Close();
						return;
					}
					if (this.textReader != null)
					{
						this.textReader.Close();
					}
				}
			}

			// Token: 0x17000159 RID: 345
			// (get) Token: 0x06000794 RID: 1940 RVA: 0x000246E3 File Offset: 0x000236E3
			internal int LineNo
			{
				get
				{
					return this.lineNo;
				}
			}

			// Token: 0x1700015A RID: 346
			// (get) Token: 0x06000795 RID: 1941 RVA: 0x000246EB File Offset: 0x000236EB
			internal int LinePos
			{
				get
				{
					return this.charPos - this.lineStartPos;
				}
			}

			// Token: 0x04000719 RID: 1817
			internal char[] chars;

			// Token: 0x0400071A RID: 1818
			internal int charPos;

			// Token: 0x0400071B RID: 1819
			internal int charsUsed;

			// Token: 0x0400071C RID: 1820
			internal Encoding encoding;

			// Token: 0x0400071D RID: 1821
			internal bool appendMode;

			// Token: 0x0400071E RID: 1822
			internal Stream stream;

			// Token: 0x0400071F RID: 1823
			internal Decoder decoder;

			// Token: 0x04000720 RID: 1824
			internal byte[] bytes;

			// Token: 0x04000721 RID: 1825
			internal int bytePos;

			// Token: 0x04000722 RID: 1826
			internal int bytesUsed;

			// Token: 0x04000723 RID: 1827
			internal TextReader textReader;

			// Token: 0x04000724 RID: 1828
			internal int lineNo;

			// Token: 0x04000725 RID: 1829
			internal int lineStartPos;

			// Token: 0x04000726 RID: 1830
			internal string baseUriStr;

			// Token: 0x04000727 RID: 1831
			internal Uri baseUri;

			// Token: 0x04000728 RID: 1832
			internal bool isEof;

			// Token: 0x04000729 RID: 1833
			internal bool isStreamEof;

			// Token: 0x0400072A RID: 1834
			internal SchemaEntity entity;

			// Token: 0x0400072B RID: 1835
			internal int entityId;

			// Token: 0x0400072C RID: 1836
			internal bool eolNormalized;

			// Token: 0x0400072D RID: 1837
			internal bool entityResolvedManually;
		}

		// Token: 0x0200008C RID: 140
		private class XmlContext
		{
			// Token: 0x06000796 RID: 1942 RVA: 0x000246FA File Offset: 0x000236FA
			internal XmlContext()
			{
				this.xmlSpace = XmlSpace.None;
				this.xmlLang = string.Empty;
				this.defaultNamespace = string.Empty;
				this.previousContext = null;
			}

			// Token: 0x06000797 RID: 1943 RVA: 0x00024726 File Offset: 0x00023726
			internal XmlContext(XmlTextReaderImpl.XmlContext previousContext)
			{
				this.xmlSpace = previousContext.xmlSpace;
				this.xmlLang = previousContext.xmlLang;
				this.defaultNamespace = previousContext.defaultNamespace;
				this.previousContext = previousContext;
			}

			// Token: 0x0400072E RID: 1838
			internal XmlSpace xmlSpace;

			// Token: 0x0400072F RID: 1839
			internal string xmlLang;

			// Token: 0x04000730 RID: 1840
			internal string defaultNamespace;

			// Token: 0x04000731 RID: 1841
			internal XmlTextReaderImpl.XmlContext previousContext;
		}

		// Token: 0x0200008D RID: 141
		private class NoNamespaceManager : XmlNamespaceManager
		{
			// Token: 0x1700015B RID: 347
			// (get) Token: 0x06000799 RID: 1945 RVA: 0x00024761 File Offset: 0x00023761
			public override string DefaultNamespace
			{
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x0600079A RID: 1946 RVA: 0x00024768 File Offset: 0x00023768
			public override void PushScope()
			{
			}

			// Token: 0x0600079B RID: 1947 RVA: 0x0002476A File Offset: 0x0002376A
			public override bool PopScope()
			{
				return false;
			}

			// Token: 0x0600079C RID: 1948 RVA: 0x0002476D File Offset: 0x0002376D
			public override void AddNamespace(string prefix, string uri)
			{
			}

			// Token: 0x0600079D RID: 1949 RVA: 0x0002476F File Offset: 0x0002376F
			public override void RemoveNamespace(string prefix, string uri)
			{
			}

			// Token: 0x0600079E RID: 1950 RVA: 0x00024771 File Offset: 0x00023771
			public override IEnumerator GetEnumerator()
			{
				return null;
			}

			// Token: 0x0600079F RID: 1951 RVA: 0x00024774 File Offset: 0x00023774
			public override IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
			{
				return null;
			}

			// Token: 0x060007A0 RID: 1952 RVA: 0x00024777 File Offset: 0x00023777
			public override string LookupNamespace(string prefix)
			{
				return string.Empty;
			}

			// Token: 0x060007A1 RID: 1953 RVA: 0x0002477E File Offset: 0x0002377E
			public override string LookupPrefix(string uri)
			{
				return null;
			}

			// Token: 0x060007A2 RID: 1954 RVA: 0x00024781 File Offset: 0x00023781
			public override bool HasNamespace(string prefix)
			{
				return false;
			}
		}

		// Token: 0x0200008F RID: 143
		internal class DtdParserProxy : IDtdParserAdapter
		{
			// Token: 0x060007C3 RID: 1987 RVA: 0x00024784 File Offset: 0x00023784
			internal DtdParserProxy(XmlTextReaderImpl reader)
			{
				this.reader = reader;
				this.dtdParser = new DtdParser(this);
			}

			// Token: 0x060007C4 RID: 1988 RVA: 0x0002479F File Offset: 0x0002379F
			internal DtdParserProxy(XmlTextReaderImpl reader, SchemaInfo schemaInfo)
			{
				this.reader = reader;
				this.schemaInfo = schemaInfo;
			}

			// Token: 0x060007C5 RID: 1989 RVA: 0x000247B5 File Offset: 0x000237B5
			internal DtdParserProxy(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, XmlTextReaderImpl reader)
			{
				this.reader = reader;
				this.dtdParser = new DtdParser(baseUri, docTypeName, publicId, systemId, internalSubset, this);
			}

			// Token: 0x060007C6 RID: 1990 RVA: 0x000247D8 File Offset: 0x000237D8
			internal void Parse(bool saveInternalSubset)
			{
				if (this.dtdParser == null)
				{
					throw new InvalidOperationException();
				}
				this.dtdParser.Parse(saveInternalSubset);
			}

			// Token: 0x1700016C RID: 364
			// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000247F4 File Offset: 0x000237F4
			internal SchemaInfo DtdSchemaInfo
			{
				get
				{
					if (this.dtdParser == null)
					{
						return this.schemaInfo;
					}
					return this.dtdParser.SchemaInfo;
				}
			}

			// Token: 0x1700016D RID: 365
			// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00024810 File Offset: 0x00023810
			internal string InternalDtdSubset
			{
				get
				{
					if (this.dtdParser == null)
					{
						throw new InvalidOperationException();
					}
					return this.dtdParser.InternalSubset;
				}
			}

			// Token: 0x1700016E RID: 366
			// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0002482B File Offset: 0x0002382B
			XmlNameTable IDtdParserAdapter.NameTable
			{
				get
				{
					return this.reader.DtdParserProxy_NameTable;
				}
			}

			// Token: 0x1700016F RID: 367
			// (get) Token: 0x060007CA RID: 1994 RVA: 0x00024838 File Offset: 0x00023838
			XmlNamespaceManager IDtdParserAdapter.NamespaceManager
			{
				get
				{
					return this.reader.DtdParserProxy_NamespaceManager;
				}
			}

			// Token: 0x17000170 RID: 368
			// (get) Token: 0x060007CB RID: 1995 RVA: 0x00024845 File Offset: 0x00023845
			bool IDtdParserAdapter.DtdValidation
			{
				get
				{
					return this.reader.DtdParserProxy_DtdValidation;
				}
			}

			// Token: 0x17000171 RID: 369
			// (get) Token: 0x060007CC RID: 1996 RVA: 0x00024852 File Offset: 0x00023852
			bool IDtdParserAdapter.Normalization
			{
				get
				{
					return this.reader.DtdParserProxy_Normalization;
				}
			}

			// Token: 0x17000172 RID: 370
			// (get) Token: 0x060007CD RID: 1997 RVA: 0x0002485F File Offset: 0x0002385F
			bool IDtdParserAdapter.Namespaces
			{
				get
				{
					return this.reader.DtdParserProxy_Namespaces;
				}
			}

			// Token: 0x17000173 RID: 371
			// (get) Token: 0x060007CE RID: 1998 RVA: 0x0002486C File Offset: 0x0002386C
			bool IDtdParserAdapter.V1CompatibilityMode
			{
				get
				{
					return this.reader.DtdParserProxy_V1CompatibilityMode;
				}
			}

			// Token: 0x17000174 RID: 372
			// (get) Token: 0x060007CF RID: 1999 RVA: 0x00024879 File Offset: 0x00023879
			Uri IDtdParserAdapter.BaseUri
			{
				get
				{
					return this.reader.DtdParserProxy_BaseUri;
				}
			}

			// Token: 0x17000175 RID: 373
			// (get) Token: 0x060007D0 RID: 2000 RVA: 0x00024886 File Offset: 0x00023886
			bool IDtdParserAdapter.IsEof
			{
				get
				{
					return this.reader.DtdParserProxy_IsEof;
				}
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00024893 File Offset: 0x00023893
			char[] IDtdParserAdapter.ParsingBuffer
			{
				get
				{
					return this.reader.DtdParserProxy_ParsingBuffer;
				}
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x060007D2 RID: 2002 RVA: 0x000248A0 File Offset: 0x000238A0
			int IDtdParserAdapter.ParsingBufferLength
			{
				get
				{
					return this.reader.DtdParserProxy_ParsingBufferLength;
				}
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x060007D3 RID: 2003 RVA: 0x000248AD File Offset: 0x000238AD
			// (set) Token: 0x060007D4 RID: 2004 RVA: 0x000248BA File Offset: 0x000238BA
			int IDtdParserAdapter.CurrentPosition
			{
				get
				{
					return this.reader.DtdParserProxy_CurrentPosition;
				}
				set
				{
					this.reader.DtdParserProxy_CurrentPosition = value;
				}
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x060007D5 RID: 2005 RVA: 0x000248C8 File Offset: 0x000238C8
			int IDtdParserAdapter.EntityStackLength
			{
				get
				{
					return this.reader.DtdParserProxy_EntityStackLength;
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x060007D6 RID: 2006 RVA: 0x000248D5 File Offset: 0x000238D5
			bool IDtdParserAdapter.IsEntityEolNormalized
			{
				get
				{
					return this.reader.DtdParserProxy_IsEntityEolNormalized;
				}
			}

			// Token: 0x1700017B RID: 379
			// (get) Token: 0x060007D7 RID: 2007 RVA: 0x000248E2 File Offset: 0x000238E2
			// (set) Token: 0x060007D8 RID: 2008 RVA: 0x000248EF File Offset: 0x000238EF
			ValidationEventHandler IDtdParserAdapter.EventHandler
			{
				get
				{
					return this.reader.DtdParserProxy_EventHandler;
				}
				set
				{
					this.reader.DtdParserProxy_EventHandler = value;
				}
			}

			// Token: 0x060007D9 RID: 2009 RVA: 0x000248FD File Offset: 0x000238FD
			void IDtdParserAdapter.OnNewLine(int pos)
			{
				this.reader.DtdParserProxy_OnNewLine(pos);
			}

			// Token: 0x1700017C RID: 380
			// (get) Token: 0x060007DA RID: 2010 RVA: 0x0002490B File Offset: 0x0002390B
			int IDtdParserAdapter.LineNo
			{
				get
				{
					return this.reader.DtdParserProxy_LineNo;
				}
			}

			// Token: 0x1700017D RID: 381
			// (get) Token: 0x060007DB RID: 2011 RVA: 0x00024918 File Offset: 0x00023918
			int IDtdParserAdapter.LineStartPosition
			{
				get
				{
					return this.reader.DtdParserProxy_LineStartPosition;
				}
			}

			// Token: 0x060007DC RID: 2012 RVA: 0x00024925 File Offset: 0x00023925
			int IDtdParserAdapter.ReadData()
			{
				return this.reader.DtdParserProxy_ReadData();
			}

			// Token: 0x060007DD RID: 2013 RVA: 0x00024932 File Offset: 0x00023932
			void IDtdParserAdapter.SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception)
			{
				this.reader.DtdParserProxy_SendValidationEvent(severity, exception);
			}

			// Token: 0x060007DE RID: 2014 RVA: 0x00024941 File Offset: 0x00023941
			int IDtdParserAdapter.ParseNumericCharRef(BufferBuilder internalSubsetBuilder)
			{
				return this.reader.DtdParserProxy_ParseNumericCharRef(internalSubsetBuilder);
			}

			// Token: 0x060007DF RID: 2015 RVA: 0x0002494F File Offset: 0x0002394F
			int IDtdParserAdapter.ParseNamedCharRef(bool expand, BufferBuilder internalSubsetBuilder)
			{
				return this.reader.DtdParserProxy_ParseNamedCharRef(expand, internalSubsetBuilder);
			}

			// Token: 0x060007E0 RID: 2016 RVA: 0x0002495E File Offset: 0x0002395E
			void IDtdParserAdapter.ParsePI(BufferBuilder sb)
			{
				this.reader.DtdParserProxy_ParsePI(sb);
			}

			// Token: 0x060007E1 RID: 2017 RVA: 0x0002496C File Offset: 0x0002396C
			void IDtdParserAdapter.ParseComment(BufferBuilder sb)
			{
				this.reader.DtdParserProxy_ParseComment(sb);
			}

			// Token: 0x060007E2 RID: 2018 RVA: 0x0002497A File Offset: 0x0002397A
			bool IDtdParserAdapter.PushEntity(SchemaEntity entity, int entityId)
			{
				return this.reader.DtdParserProxy_PushEntity(entity, entityId);
			}

			// Token: 0x060007E3 RID: 2019 RVA: 0x00024989 File Offset: 0x00023989
			bool IDtdParserAdapter.PopEntity(out SchemaEntity oldEntity, out int newEntityId)
			{
				return this.reader.DtdParserProxy_PopEntity(out oldEntity, out newEntityId);
			}

			// Token: 0x060007E4 RID: 2020 RVA: 0x00024998 File Offset: 0x00023998
			bool IDtdParserAdapter.PushExternalSubset(string systemId, string publicId)
			{
				return this.reader.DtdParserProxy_PushExternalSubset(systemId, publicId);
			}

			// Token: 0x060007E5 RID: 2021 RVA: 0x000249A7 File Offset: 0x000239A7
			void IDtdParserAdapter.PushInternalDtd(string baseUri, string internalDtd)
			{
				this.reader.DtdParserProxy_PushInternalDtd(baseUri, internalDtd);
			}

			// Token: 0x060007E6 RID: 2022 RVA: 0x000249B6 File Offset: 0x000239B6
			void IDtdParserAdapter.Throw(Exception e)
			{
				this.reader.DtdParserProxy_Throw(e);
			}

			// Token: 0x060007E7 RID: 2023 RVA: 0x000249C4 File Offset: 0x000239C4
			void IDtdParserAdapter.OnSystemId(string systemId, LineInfo keywordLineInfo, LineInfo systemLiteralLineInfo)
			{
				this.reader.DtdParserProxy_OnSystemId(systemId, keywordLineInfo, systemLiteralLineInfo);
			}

			// Token: 0x060007E8 RID: 2024 RVA: 0x000249D4 File Offset: 0x000239D4
			void IDtdParserAdapter.OnPublicId(string publicId, LineInfo keywordLineInfo, LineInfo publicLiteralLineInfo)
			{
				this.reader.DtdParserProxy_OnPublicId(publicId, keywordLineInfo, publicLiteralLineInfo);
			}

			// Token: 0x04000732 RID: 1842
			private XmlTextReaderImpl reader;

			// Token: 0x04000733 RID: 1843
			private DtdParser dtdParser;

			// Token: 0x04000734 RID: 1844
			private SchemaInfo schemaInfo;
		}

		// Token: 0x02000090 RID: 144
		private class NodeData : IComparable
		{
			// Token: 0x1700017E RID: 382
			// (get) Token: 0x060007E9 RID: 2025 RVA: 0x000249E4 File Offset: 0x000239E4
			internal static XmlTextReaderImpl.NodeData None
			{
				get
				{
					if (XmlTextReaderImpl.NodeData.s_None == null)
					{
						XmlTextReaderImpl.NodeData.s_None = new XmlTextReaderImpl.NodeData();
					}
					return XmlTextReaderImpl.NodeData.s_None;
				}
			}

			// Token: 0x060007EA RID: 2026 RVA: 0x000249FC File Offset: 0x000239FC
			internal NodeData()
			{
				this.Clear(XmlNodeType.None);
				this.xmlContextPushed = false;
			}

			// Token: 0x1700017F RID: 383
			// (get) Token: 0x060007EB RID: 2027 RVA: 0x00024A12 File Offset: 0x00023A12
			internal int LineNo
			{
				get
				{
					return this.lineInfo.lineNo;
				}
			}

			// Token: 0x17000180 RID: 384
			// (get) Token: 0x060007EC RID: 2028 RVA: 0x00024A1F File Offset: 0x00023A1F
			internal int LinePos
			{
				get
				{
					return this.lineInfo.linePos;
				}
			}

			// Token: 0x17000181 RID: 385
			// (get) Token: 0x060007ED RID: 2029 RVA: 0x00024A2C File Offset: 0x00023A2C
			// (set) Token: 0x060007EE RID: 2030 RVA: 0x00024A3F File Offset: 0x00023A3F
			internal bool IsEmptyElement
			{
				get
				{
					return this.type == XmlNodeType.Element && this.isEmptyOrDefault;
				}
				set
				{
					this.isEmptyOrDefault = value;
				}
			}

			// Token: 0x17000182 RID: 386
			// (get) Token: 0x060007EF RID: 2031 RVA: 0x00024A48 File Offset: 0x00023A48
			// (set) Token: 0x060007F0 RID: 2032 RVA: 0x00024A5B File Offset: 0x00023A5B
			internal bool IsDefaultAttribute
			{
				get
				{
					return this.type == XmlNodeType.Attribute && this.isEmptyOrDefault;
				}
				set
				{
					this.isEmptyOrDefault = value;
				}
			}

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00024A64 File Offset: 0x00023A64
			internal bool ValueBuffered
			{
				get
				{
					return this.value == null;
				}
			}

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00024A6F File Offset: 0x00023A6F
			internal string StringValue
			{
				get
				{
					if (this.value == null)
					{
						this.value = new string(this.chars, this.valueStartPos, this.valueLength);
					}
					return this.value;
				}
			}

			// Token: 0x060007F3 RID: 2035 RVA: 0x00024A9C File Offset: 0x00023A9C
			internal void TrimSpacesInValue()
			{
				if (this.ValueBuffered)
				{
					XmlComplianceUtil.StripSpaces(this.chars, this.valueStartPos, ref this.valueLength);
					return;
				}
				this.value = XmlComplianceUtil.StripSpaces(this.value);
			}

			// Token: 0x060007F4 RID: 2036 RVA: 0x00024ACF File Offset: 0x00023ACF
			internal void Clear(XmlNodeType type)
			{
				this.type = type;
				this.ClearName();
				this.value = string.Empty;
				this.valueStartPos = -1;
				this.nameWPrefix = string.Empty;
				this.schemaType = null;
				this.typedValue = null;
			}

			// Token: 0x060007F5 RID: 2037 RVA: 0x00024B09 File Offset: 0x00023B09
			internal void ClearName()
			{
				this.localName = string.Empty;
				this.prefix = string.Empty;
				this.ns = string.Empty;
				this.nameWPrefix = string.Empty;
			}

			// Token: 0x060007F6 RID: 2038 RVA: 0x00024B37 File Offset: 0x00023B37
			internal void SetLineInfo(int lineNo, int linePos)
			{
				this.lineInfo.Set(lineNo, linePos);
			}

			// Token: 0x060007F7 RID: 2039 RVA: 0x00024B46 File Offset: 0x00023B46
			internal void SetLineInfo2(int lineNo, int linePos)
			{
				this.lineInfo2.Set(lineNo, linePos);
			}

			// Token: 0x060007F8 RID: 2040 RVA: 0x00024B55 File Offset: 0x00023B55
			internal void SetValueNode(XmlNodeType type, string value)
			{
				this.type = type;
				this.ClearName();
				this.value = value;
				this.valueStartPos = -1;
			}

			// Token: 0x060007F9 RID: 2041 RVA: 0x00024B72 File Offset: 0x00023B72
			internal void SetValueNode(XmlNodeType type, char[] chars, int startPos, int len)
			{
				this.type = type;
				this.ClearName();
				this.value = null;
				this.chars = chars;
				this.valueStartPos = startPos;
				this.valueLength = len;
			}

			// Token: 0x060007FA RID: 2042 RVA: 0x00024B9E File Offset: 0x00023B9E
			internal void SetNamedNode(XmlNodeType type, string localName)
			{
				this.SetNamedNode(type, localName, string.Empty, localName);
			}

			// Token: 0x060007FB RID: 2043 RVA: 0x00024BAE File Offset: 0x00023BAE
			internal void SetNamedNode(XmlNodeType type, string localName, string prefix, string nameWPrefix)
			{
				this.type = type;
				this.localName = localName;
				this.prefix = prefix;
				this.nameWPrefix = nameWPrefix;
				this.ns = string.Empty;
				this.value = string.Empty;
				this.valueStartPos = -1;
			}

			// Token: 0x060007FC RID: 2044 RVA: 0x00024BEA File Offset: 0x00023BEA
			internal void SetValue(string value)
			{
				this.valueStartPos = -1;
				this.value = value;
			}

			// Token: 0x060007FD RID: 2045 RVA: 0x00024BFA File Offset: 0x00023BFA
			internal void SetValue(char[] chars, int startPos, int len)
			{
				this.value = null;
				this.chars = chars;
				this.valueStartPos = startPos;
				this.valueLength = len;
			}

			// Token: 0x060007FE RID: 2046 RVA: 0x00024C18 File Offset: 0x00023C18
			internal void OnBufferInvalidated()
			{
				if (this.value == null)
				{
					this.value = new string(this.chars, this.valueStartPos, this.valueLength);
				}
				this.valueStartPos = -1;
			}

			// Token: 0x060007FF RID: 2047 RVA: 0x00024C46 File Offset: 0x00023C46
			internal void CopyTo(BufferBuilder sb)
			{
				this.CopyTo(0, sb);
			}

			// Token: 0x06000800 RID: 2048 RVA: 0x00024C50 File Offset: 0x00023C50
			internal void CopyTo(int valueOffset, BufferBuilder sb)
			{
				if (this.value == null)
				{
					sb.Append(this.chars, this.valueStartPos + valueOffset, this.valueLength - valueOffset);
					return;
				}
				if (valueOffset <= 0)
				{
					sb.Append(this.value);
					return;
				}
				sb.Append(this.value, valueOffset, this.value.Length - valueOffset);
			}

			// Token: 0x06000801 RID: 2049 RVA: 0x00024CB0 File Offset: 0x00023CB0
			internal int CopyTo(int valueOffset, char[] buffer, int offset, int length)
			{
				if (this.value == null)
				{
					int num = this.valueLength - valueOffset;
					if (num > length)
					{
						num = length;
					}
					Buffer.BlockCopy(this.chars, (this.valueStartPos + valueOffset) * 2, buffer, offset * 2, num * 2);
					return num;
				}
				int num2 = this.value.Length - valueOffset;
				if (num2 > length)
				{
					num2 = length;
				}
				this.value.CopyTo(valueOffset, buffer, offset, num2);
				return num2;
			}

			// Token: 0x06000802 RID: 2050 RVA: 0x00024D1C File Offset: 0x00023D1C
			internal int CopyToBinary(IncrementalReadDecoder decoder, int valueOffset)
			{
				if (this.value == null)
				{
					return decoder.Decode(this.chars, this.valueStartPos + valueOffset, this.valueLength - valueOffset);
				}
				return decoder.Decode(this.value, valueOffset, this.value.Length - valueOffset);
			}

			// Token: 0x06000803 RID: 2051 RVA: 0x00024D68 File Offset: 0x00023D68
			internal void AdjustLineInfo(int valueOffset, bool isNormalized, ref LineInfo lineInfo)
			{
				if (valueOffset == 0)
				{
					return;
				}
				if (this.valueStartPos != -1)
				{
					XmlTextReaderImpl.AdjustLineInfo(this.chars, this.valueStartPos, this.valueStartPos + valueOffset, isNormalized, ref lineInfo);
					return;
				}
				char[] array = this.value.ToCharArray(0, valueOffset);
				XmlTextReaderImpl.AdjustLineInfo(array, 0, array.Length, isNormalized, ref lineInfo);
			}

			// Token: 0x06000804 RID: 2052 RVA: 0x00024DB8 File Offset: 0x00023DB8
			internal string GetNameWPrefix(XmlNameTable nt)
			{
				if (this.nameWPrefix != null)
				{
					return this.nameWPrefix;
				}
				return this.CreateNameWPrefix(nt);
			}

			// Token: 0x06000805 RID: 2053 RVA: 0x00024DD0 File Offset: 0x00023DD0
			internal string CreateNameWPrefix(XmlNameTable nt)
			{
				if (this.prefix.Length == 0)
				{
					this.nameWPrefix = this.localName;
				}
				else
				{
					this.nameWPrefix = nt.Add(this.prefix + ":" + this.localName);
				}
				return this.nameWPrefix;
			}

			// Token: 0x06000806 RID: 2054 RVA: 0x00024E20 File Offset: 0x00023E20
			int IComparable.CompareTo(object obj)
			{
				XmlTextReaderImpl.NodeData nodeData = obj as XmlTextReaderImpl.NodeData;
				if (nodeData == null)
				{
					return this.GetHashCode().CompareTo(nodeData.GetHashCode());
				}
				if (!Ref.Equal(this.localName, nodeData.localName))
				{
					return string.CompareOrdinal(this.localName, nodeData.localName);
				}
				if (Ref.Equal(this.ns, nodeData.ns))
				{
					return 0;
				}
				return string.CompareOrdinal(this.ns, nodeData.ns);
			}

			// Token: 0x04000735 RID: 1845
			private static XmlTextReaderImpl.NodeData s_None;

			// Token: 0x04000736 RID: 1846
			internal XmlNodeType type;

			// Token: 0x04000737 RID: 1847
			internal string localName;

			// Token: 0x04000738 RID: 1848
			internal string prefix;

			// Token: 0x04000739 RID: 1849
			internal string ns;

			// Token: 0x0400073A RID: 1850
			internal string nameWPrefix;

			// Token: 0x0400073B RID: 1851
			private string value;

			// Token: 0x0400073C RID: 1852
			private char[] chars;

			// Token: 0x0400073D RID: 1853
			private int valueStartPos;

			// Token: 0x0400073E RID: 1854
			private int valueLength;

			// Token: 0x0400073F RID: 1855
			internal LineInfo lineInfo;

			// Token: 0x04000740 RID: 1856
			internal LineInfo lineInfo2;

			// Token: 0x04000741 RID: 1857
			internal char quoteChar;

			// Token: 0x04000742 RID: 1858
			internal int depth;

			// Token: 0x04000743 RID: 1859
			private bool isEmptyOrDefault;

			// Token: 0x04000744 RID: 1860
			internal int entityId;

			// Token: 0x04000745 RID: 1861
			internal bool xmlContextPushed;

			// Token: 0x04000746 RID: 1862
			internal XmlTextReaderImpl.NodeData nextAttrValueChunk;

			// Token: 0x04000747 RID: 1863
			internal object schemaType;

			// Token: 0x04000748 RID: 1864
			internal object typedValue;
		}

		// Token: 0x02000091 RID: 145
		private class SchemaAttDefToNodeDataComparer : IComparer
		{
			// Token: 0x17000185 RID: 389
			// (get) Token: 0x06000807 RID: 2055 RVA: 0x00024E97 File Offset: 0x00023E97
			internal static IComparer Instance
			{
				get
				{
					return XmlTextReaderImpl.SchemaAttDefToNodeDataComparer.s_instance;
				}
			}

			// Token: 0x06000808 RID: 2056 RVA: 0x00024EA0 File Offset: 0x00023EA0
			public int Compare(object x, object y)
			{
				if (x == null)
				{
					if (y != null)
					{
						return -1;
					}
					return 0;
				}
				else
				{
					if (y == null)
					{
						return 1;
					}
					XmlTextReaderImpl.NodeData nodeData = x as XmlTextReaderImpl.NodeData;
					string strA;
					string prefix;
					if (nodeData != null)
					{
						strA = nodeData.localName;
						prefix = nodeData.prefix;
					}
					else
					{
						SchemaAttDef schemaAttDef = x as SchemaAttDef;
						if (schemaAttDef == null)
						{
							throw new XmlException("Xml_DefaultException", string.Empty);
						}
						strA = schemaAttDef.Name.Name;
						prefix = schemaAttDef.Prefix;
					}
					nodeData = (y as XmlTextReaderImpl.NodeData);
					string strB;
					string prefix2;
					if (nodeData != null)
					{
						strB = nodeData.localName;
						prefix2 = nodeData.prefix;
					}
					else
					{
						SchemaAttDef schemaAttDef2 = y as SchemaAttDef;
						if (schemaAttDef2 == null)
						{
							throw new XmlException("Xml_DefaultException", string.Empty);
						}
						strB = schemaAttDef2.Name.Name;
						prefix2 = schemaAttDef2.Prefix;
					}
					int num = string.Compare(strA, strB, StringComparison.Ordinal);
					if (num != 0)
					{
						return num;
					}
					return string.Compare(prefix, prefix2, StringComparison.Ordinal);
				}
			}

			// Token: 0x04000749 RID: 1865
			private static IComparer s_instance = new XmlTextReaderImpl.SchemaAttDefToNodeDataComparer();
		}
	}
}
