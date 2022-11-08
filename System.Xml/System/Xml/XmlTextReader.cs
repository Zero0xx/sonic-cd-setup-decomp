using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000084 RID: 132
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class XmlTextReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x00019305 File Offset: 0x00018305
		protected XmlTextReader()
		{
			this.impl = new XmlTextReaderImpl();
			this.impl.OuterReader = this;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00019324 File Offset: 0x00018324
		protected XmlTextReader(XmlNameTable nt)
		{
			this.impl = new XmlTextReaderImpl(nt);
			this.impl.OuterReader = this;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00019344 File Offset: 0x00018344
		public XmlTextReader(Stream input)
		{
			this.impl = new XmlTextReaderImpl(input);
			this.impl.OuterReader = this;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00019364 File Offset: 0x00018364
		public XmlTextReader(string url, Stream input)
		{
			this.impl = new XmlTextReaderImpl(url, input);
			this.impl.OuterReader = this;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00019385 File Offset: 0x00018385
		public XmlTextReader(Stream input, XmlNameTable nt)
		{
			this.impl = new XmlTextReaderImpl(input, nt);
			this.impl.OuterReader = this;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000193A6 File Offset: 0x000183A6
		public XmlTextReader(string url, Stream input, XmlNameTable nt)
		{
			this.impl = new XmlTextReaderImpl(url, input, nt);
			this.impl.OuterReader = this;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000193C8 File Offset: 0x000183C8
		public XmlTextReader(TextReader input)
		{
			this.impl = new XmlTextReaderImpl(input);
			this.impl.OuterReader = this;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000193E8 File Offset: 0x000183E8
		public XmlTextReader(string url, TextReader input)
		{
			this.impl = new XmlTextReaderImpl(url, input);
			this.impl.OuterReader = this;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00019409 File Offset: 0x00018409
		public XmlTextReader(TextReader input, XmlNameTable nt)
		{
			this.impl = new XmlTextReaderImpl(input, nt);
			this.impl.OuterReader = this;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001942A File Offset: 0x0001842A
		public XmlTextReader(string url, TextReader input, XmlNameTable nt)
		{
			this.impl = new XmlTextReaderImpl(url, input, nt);
			this.impl.OuterReader = this;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001944C File Offset: 0x0001844C
		public XmlTextReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
		{
			this.impl = new XmlTextReaderImpl(xmlFragment, fragType, context);
			this.impl.OuterReader = this;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001946E File Offset: 0x0001846E
		public XmlTextReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
		{
			this.impl = new XmlTextReaderImpl(xmlFragment, fragType, context);
			this.impl.OuterReader = this;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00019490 File Offset: 0x00018490
		public XmlTextReader(string url)
		{
			this.impl = new XmlTextReaderImpl(url, new NameTable());
			this.impl.OuterReader = this;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000194B5 File Offset: 0x000184B5
		public XmlTextReader(string url, XmlNameTable nt)
		{
			this.impl = new XmlTextReaderImpl(url, nt);
			this.impl.OuterReader = this;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000194D6 File Offset: 0x000184D6
		public override XmlReaderSettings Settings
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x000194D9 File Offset: 0x000184D9
		public override XmlNodeType NodeType
		{
			get
			{
				return this.impl.NodeType;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x000194E6 File Offset: 0x000184E6
		public override string Name
		{
			get
			{
				return this.impl.Name;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000194F3 File Offset: 0x000184F3
		public override string LocalName
		{
			get
			{
				return this.impl.LocalName;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00019500 File Offset: 0x00018500
		public override string NamespaceURI
		{
			get
			{
				return this.impl.NamespaceURI;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0001950D File Offset: 0x0001850D
		public override string Prefix
		{
			get
			{
				return this.impl.Prefix;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0001951A File Offset: 0x0001851A
		public override bool HasValue
		{
			get
			{
				return this.impl.HasValue;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00019527 File Offset: 0x00018527
		public override string Value
		{
			get
			{
				return this.impl.Value;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00019534 File Offset: 0x00018534
		public override int Depth
		{
			get
			{
				return this.impl.Depth;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x00019541 File Offset: 0x00018541
		public override string BaseURI
		{
			get
			{
				return this.impl.BaseURI;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0001954E File Offset: 0x0001854E
		public override bool IsEmptyElement
		{
			get
			{
				return this.impl.IsEmptyElement;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x0001955B File Offset: 0x0001855B
		public override bool IsDefault
		{
			get
			{
				return this.impl.IsDefault;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00019568 File Offset: 0x00018568
		public override char QuoteChar
		{
			get
			{
				return this.impl.QuoteChar;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00019575 File Offset: 0x00018575
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.impl.XmlSpace;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00019582 File Offset: 0x00018582
		public override string XmlLang
		{
			get
			{
				return this.impl.XmlLang;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001958F File Offset: 0x0001858F
		public override int AttributeCount
		{
			get
			{
				return this.impl.AttributeCount;
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001959C File Offset: 0x0001859C
		public override string GetAttribute(string name)
		{
			return this.impl.GetAttribute(name);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000195AA File Offset: 0x000185AA
		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this.impl.GetAttribute(localName, namespaceURI);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000195B9 File Offset: 0x000185B9
		public override string GetAttribute(int i)
		{
			return this.impl.GetAttribute(i);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000195C7 File Offset: 0x000185C7
		public override bool MoveToAttribute(string name)
		{
			return this.impl.MoveToAttribute(name);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000195D5 File Offset: 0x000185D5
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			return this.impl.MoveToAttribute(localName, namespaceURI);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000195E4 File Offset: 0x000185E4
		public override void MoveToAttribute(int i)
		{
			this.impl.MoveToAttribute(i);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000195F2 File Offset: 0x000185F2
		public override bool MoveToFirstAttribute()
		{
			return this.impl.MoveToFirstAttribute();
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000195FF File Offset: 0x000185FF
		public override bool MoveToNextAttribute()
		{
			return this.impl.MoveToNextAttribute();
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001960C File Offset: 0x0001860C
		public override bool MoveToElement()
		{
			return this.impl.MoveToElement();
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00019619 File Offset: 0x00018619
		public override bool ReadAttributeValue()
		{
			return this.impl.ReadAttributeValue();
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00019626 File Offset: 0x00018626
		public override bool Read()
		{
			return this.impl.Read();
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00019633 File Offset: 0x00018633
		public override bool EOF
		{
			get
			{
				return this.impl.EOF;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00019640 File Offset: 0x00018640
		public override void Close()
		{
			this.impl.Close();
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001964D File Offset: 0x0001864D
		public override ReadState ReadState
		{
			get
			{
				return this.impl.ReadState;
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001965A File Offset: 0x0001865A
		public override void Skip()
		{
			this.impl.Skip();
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00019667 File Offset: 0x00018667
		public override XmlNameTable NameTable
		{
			get
			{
				return this.impl.NameTable;
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00019674 File Offset: 0x00018674
		public override string LookupNamespace(string prefix)
		{
			string text = this.impl.LookupNamespace(prefix);
			if (text != null && text.Length == 0)
			{
				text = null;
			}
			return text;
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x0001969C File Offset: 0x0001869C
		public override bool CanResolveEntity
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001969F File Offset: 0x0001869F
		public override void ResolveEntity()
		{
			this.impl.ResolveEntity();
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x000196AC File Offset: 0x000186AC
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000196AF File Offset: 0x000186AF
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			return this.impl.ReadContentAsBase64(buffer, index, count);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000196BF File Offset: 0x000186BF
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			return this.impl.ReadElementContentAsBase64(buffer, index, count);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000196CF File Offset: 0x000186CF
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			return this.impl.ReadContentAsBinHex(buffer, index, count);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x000196DF File Offset: 0x000186DF
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			return this.impl.ReadElementContentAsBinHex(buffer, index, count);
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x000196EF File Offset: 0x000186EF
		public override bool CanReadValueChunk
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000196F2 File Offset: 0x000186F2
		public override string ReadString()
		{
			this.impl.MoveOffEntityReference();
			return base.ReadString();
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00019705 File Offset: 0x00018705
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00019708 File Offset: 0x00018708
		public int LineNumber
		{
			get
			{
				return this.impl.LineNumber;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00019715 File Offset: 0x00018715
		public int LinePosition
		{
			get
			{
				return this.impl.LinePosition;
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00019722 File Offset: 0x00018722
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.impl.GetNamespacesInScope(scope);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00019730 File Offset: 0x00018730
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return this.impl.LookupNamespace(prefix);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001973E File Offset: 0x0001873E
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.impl.LookupPrefix(namespaceName);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001974C File Offset: 0x0001874C
		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.impl.GetNamespacesInScope(scope);
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001975A File Offset: 0x0001875A
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x00019767 File Offset: 0x00018767
		public bool Namespaces
		{
			get
			{
				return this.impl.Namespaces;
			}
			set
			{
				this.impl.Namespaces = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00019775 File Offset: 0x00018775
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00019782 File Offset: 0x00018782
		public bool Normalization
		{
			get
			{
				return this.impl.Normalization;
			}
			set
			{
				this.impl.Normalization = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00019790 File Offset: 0x00018790
		public Encoding Encoding
		{
			get
			{
				return this.impl.Encoding;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001979D File Offset: 0x0001879D
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x000197AA File Offset: 0x000187AA
		public WhitespaceHandling WhitespaceHandling
		{
			get
			{
				return this.impl.WhitespaceHandling;
			}
			set
			{
				this.impl.WhitespaceHandling = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x000197B8 File Offset: 0x000187B8
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x000197C5 File Offset: 0x000187C5
		public bool ProhibitDtd
		{
			get
			{
				return this.impl.ProhibitDtd;
			}
			set
			{
				this.impl.ProhibitDtd = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x000197D3 File Offset: 0x000187D3
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x000197E0 File Offset: 0x000187E0
		public EntityHandling EntityHandling
		{
			get
			{
				return this.impl.EntityHandling;
			}
			set
			{
				this.impl.EntityHandling = value;
			}
		}

		// Token: 0x17000116 RID: 278
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x000197EE File Offset: 0x000187EE
		public XmlResolver XmlResolver
		{
			set
			{
				this.impl.XmlResolver = value;
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000197FC File Offset: 0x000187FC
		public void ResetState()
		{
			this.impl.ResetState();
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00019809 File Offset: 0x00018809
		public TextReader GetRemainder()
		{
			return this.impl.GetRemainder();
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00019816 File Offset: 0x00018816
		public int ReadChars(char[] buffer, int index, int count)
		{
			return this.impl.ReadChars(buffer, index, count);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00019826 File Offset: 0x00018826
		public int ReadBase64(byte[] array, int offset, int len)
		{
			return this.impl.ReadBase64(array, offset, len);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00019836 File Offset: 0x00018836
		public int ReadBinHex(byte[] array, int offset, int len)
		{
			return this.impl.ReadBinHex(array, offset, len);
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00019846 File Offset: 0x00018846
		internal XmlTextReaderImpl Impl
		{
			get
			{
				return this.impl;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001984E File Offset: 0x0001884E
		internal override XmlNamespaceManager NamespaceManager
		{
			get
			{
				return this.impl.NamespaceManager;
			}
		}

		// Token: 0x17000119 RID: 281
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x0001985B File Offset: 0x0001885B
		internal bool XmlValidatingReaderCompatibilityMode
		{
			set
			{
				this.impl.XmlValidatingReaderCompatibilityMode = value;
			}
		}

		// Token: 0x04000680 RID: 1664
		private XmlTextReaderImpl impl;
	}
}
