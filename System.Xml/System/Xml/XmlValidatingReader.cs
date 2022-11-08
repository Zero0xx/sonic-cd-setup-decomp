using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x0200009B RID: 155
	[Obsolete("Use XmlReader created by XmlReader.Create() method using appropriate XmlReaderSettings instead. http://go.microsoft.com/fwlink/?linkid=14202")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class XmlValidatingReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x000278A6 File Offset: 0x000268A6
		public XmlValidatingReader(XmlReader reader)
		{
			this.impl = new XmlValidatingReaderImpl(reader);
			this.impl.OuterReader = this;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000278C6 File Offset: 0x000268C6
		public XmlValidatingReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
		{
			this.impl = new XmlValidatingReaderImpl(xmlFragment, fragType, context);
			this.impl.OuterReader = this;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000278E8 File Offset: 0x000268E8
		public XmlValidatingReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
		{
			this.impl = new XmlValidatingReaderImpl(xmlFragment, fragType, context);
			this.impl.OuterReader = this;
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0002790A File Offset: 0x0002690A
		public override XmlReaderSettings Settings
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0002790D File Offset: 0x0002690D
		public override XmlNodeType NodeType
		{
			get
			{
				return this.impl.NodeType;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x0002791A File Offset: 0x0002691A
		public override string Name
		{
			get
			{
				return this.impl.Name;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x00027927 File Offset: 0x00026927
		public override string LocalName
		{
			get
			{
				return this.impl.LocalName;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00027934 File Offset: 0x00026934
		public override string NamespaceURI
		{
			get
			{
				return this.impl.NamespaceURI;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x00027941 File Offset: 0x00026941
		public override string Prefix
		{
			get
			{
				return this.impl.Prefix;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0002794E File Offset: 0x0002694E
		public override bool HasValue
		{
			get
			{
				return this.impl.HasValue;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0002795B File Offset: 0x0002695B
		public override string Value
		{
			get
			{
				return this.impl.Value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00027968 File Offset: 0x00026968
		public override int Depth
		{
			get
			{
				return this.impl.Depth;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00027975 File Offset: 0x00026975
		public override string BaseURI
		{
			get
			{
				return this.impl.BaseURI;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00027982 File Offset: 0x00026982
		public override bool IsEmptyElement
		{
			get
			{
				return this.impl.IsEmptyElement;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0002798F File Offset: 0x0002698F
		public override bool IsDefault
		{
			get
			{
				return this.impl.IsDefault;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0002799C File Offset: 0x0002699C
		public override char QuoteChar
		{
			get
			{
				return this.impl.QuoteChar;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x000279A9 File Offset: 0x000269A9
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.impl.XmlSpace;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x000279B6 File Offset: 0x000269B6
		public override string XmlLang
		{
			get
			{
				return this.impl.XmlLang;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x000279C3 File Offset: 0x000269C3
		public override int AttributeCount
		{
			get
			{
				return this.impl.AttributeCount;
			}
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000279D0 File Offset: 0x000269D0
		public override string GetAttribute(string name)
		{
			return this.impl.GetAttribute(name);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000279DE File Offset: 0x000269DE
		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this.impl.GetAttribute(localName, namespaceURI);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x000279ED File Offset: 0x000269ED
		public override string GetAttribute(int i)
		{
			return this.impl.GetAttribute(i);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000279FB File Offset: 0x000269FB
		public override bool MoveToAttribute(string name)
		{
			return this.impl.MoveToAttribute(name);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00027A09 File Offset: 0x00026A09
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			return this.impl.MoveToAttribute(localName, namespaceURI);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00027A18 File Offset: 0x00026A18
		public override void MoveToAttribute(int i)
		{
			this.impl.MoveToAttribute(i);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00027A26 File Offset: 0x00026A26
		public override bool MoveToFirstAttribute()
		{
			return this.impl.MoveToFirstAttribute();
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00027A33 File Offset: 0x00026A33
		public override bool MoveToNextAttribute()
		{
			return this.impl.MoveToNextAttribute();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00027A40 File Offset: 0x00026A40
		public override bool MoveToElement()
		{
			return this.impl.MoveToElement();
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00027A4D File Offset: 0x00026A4D
		public override bool ReadAttributeValue()
		{
			return this.impl.ReadAttributeValue();
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00027A5A File Offset: 0x00026A5A
		public override bool Read()
		{
			return this.impl.Read();
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x00027A67 File Offset: 0x00026A67
		public override bool EOF
		{
			get
			{
				return this.impl.EOF;
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00027A74 File Offset: 0x00026A74
		public override void Close()
		{
			this.impl.Close();
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x00027A81 File Offset: 0x00026A81
		public override ReadState ReadState
		{
			get
			{
				return this.impl.ReadState;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00027A8E File Offset: 0x00026A8E
		public override XmlNameTable NameTable
		{
			get
			{
				return this.impl.NameTable;
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00027A9C File Offset: 0x00026A9C
		public override string LookupNamespace(string prefix)
		{
			string text = this.impl.LookupNamespace(prefix);
			if (text != null && text.Length == 0)
			{
				text = null;
			}
			return text;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00027AC4 File Offset: 0x00026AC4
		public override bool CanResolveEntity
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00027AC7 File Offset: 0x00026AC7
		public override void ResolveEntity()
		{
			this.impl.ResolveEntity();
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00027AD4 File Offset: 0x00026AD4
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00027AD7 File Offset: 0x00026AD7
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			return this.impl.ReadContentAsBase64(buffer, index, count);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00027AE7 File Offset: 0x00026AE7
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			return this.impl.ReadElementContentAsBase64(buffer, index, count);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00027AF7 File Offset: 0x00026AF7
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			return this.impl.ReadContentAsBinHex(buffer, index, count);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00027B07 File Offset: 0x00026B07
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			return this.impl.ReadElementContentAsBinHex(buffer, index, count);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00027B17 File Offset: 0x00026B17
		public override string ReadString()
		{
			this.impl.MoveOffEntityReference();
			return base.ReadString();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00027B2A File Offset: 0x00026B2A
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00027B2D File Offset: 0x00026B2D
		public int LineNumber
		{
			get
			{
				return this.impl.LineNumber;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00027B3A File Offset: 0x00026B3A
		public int LinePosition
		{
			get
			{
				return this.impl.LinePosition;
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00027B47 File Offset: 0x00026B47
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.impl.GetNamespacesInScope(scope);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00027B55 File Offset: 0x00026B55
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return this.impl.LookupNamespace(prefix);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00027B63 File Offset: 0x00026B63
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.impl.LookupPrefix(namespaceName);
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600089A RID: 2202 RVA: 0x00027B71 File Offset: 0x00026B71
		// (remove) Token: 0x0600089B RID: 2203 RVA: 0x00027B7F File Offset: 0x00026B7F
		public event ValidationEventHandler ValidationEventHandler
		{
			add
			{
				this.impl.ValidationEventHandler += value;
			}
			remove
			{
				this.impl.ValidationEventHandler -= value;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00027B8D File Offset: 0x00026B8D
		public object SchemaType
		{
			get
			{
				return this.impl.SchemaType;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x00027B9A File Offset: 0x00026B9A
		public XmlReader Reader
		{
			get
			{
				return this.impl.Reader;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00027BA7 File Offset: 0x00026BA7
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00027BB4 File Offset: 0x00026BB4
		public ValidationType ValidationType
		{
			get
			{
				return this.impl.ValidationType;
			}
			set
			{
				this.impl.ValidationType = value;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00027BC2 File Offset: 0x00026BC2
		public XmlSchemaCollection Schemas
		{
			get
			{
				return this.impl.Schemas;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00027BCF File Offset: 0x00026BCF
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x00027BDC File Offset: 0x00026BDC
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

		// Token: 0x170001AC RID: 428
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00027BEA File Offset: 0x00026BEA
		public XmlResolver XmlResolver
		{
			set
			{
				this.impl.XmlResolver = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00027BF8 File Offset: 0x00026BF8
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00027C05 File Offset: 0x00026C05
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

		// Token: 0x060008A6 RID: 2214 RVA: 0x00027C13 File Offset: 0x00026C13
		public object ReadTypedValue()
		{
			return this.impl.ReadTypedValue();
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x00027C20 File Offset: 0x00026C20
		public Encoding Encoding
		{
			get
			{
				return this.impl.Encoding;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00027C2D File Offset: 0x00026C2D
		internal XmlValidatingReaderImpl Impl
		{
			get
			{
				return this.impl;
			}
		}

		// Token: 0x040007A2 RID: 1954
		private XmlValidatingReaderImpl impl;
	}
}
