using System;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x02000051 RID: 81
	internal abstract class XmlRawWriter : XmlWriter
	{
		// Token: 0x06000263 RID: 611 RVA: 0x00009F33 File Offset: 0x00008F33
		public override void WriteStartDocument()
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009F44 File Offset: 0x00008F44
		public override void WriteStartDocument(bool standalone)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009F55 File Offset: 0x00008F55
		public override void WriteEndDocument()
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009F66 File Offset: 0x00008F66
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009F68 File Offset: 0x00008F68
		public override void WriteEndElement()
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009F79 File Offset: 0x00008F79
		public override void WriteFullEndElement()
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009F8A File Offset: 0x00008F8A
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			if (this.base64Encoder == null)
			{
				this.base64Encoder = new XmlRawWriterBase64Encoder(this);
			}
			this.base64Encoder.Encode(buffer, index, count);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009FAE File Offset: 0x00008FAE
		public override string LookupPrefix(string ns)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00009FBF File Offset: 0x00008FBF
		public override WriteState WriteState
		{
			get
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00009FD0 File Offset: 0x00008FD0
		public override XmlSpace XmlSpace
		{
			get
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00009FE1 File Offset: 0x00008FE1
		public override string XmlLang
		{
			get
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009FF2 File Offset: 0x00008FF2
		public override void WriteNmToken(string name)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A003 File Offset: 0x00009003
		public override void WriteName(string name)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000A014 File Offset: 0x00009014
		public override void WriteQualifiedName(string localName, string ns)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000A025 File Offset: 0x00009025
		public override void WriteCData(string text)
		{
			this.WriteString(text);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000A030 File Offset: 0x00009030
		public override void WriteCharEntity(char ch)
		{
			this.WriteString(new string(new char[]
			{
				ch
			}));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000A054 File Offset: 0x00009054
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.WriteString(new string(new char[]
			{
				lowChar,
				highChar
			}));
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000A07C File Offset: 0x0000907C
		public override void WriteWhitespace(string ws)
		{
			this.WriteString(ws);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A085 File Offset: 0x00009085
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count));
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A095 File Offset: 0x00009095
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count));
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A0A5 File Offset: 0x000090A5
		public override void WriteRaw(string data)
		{
			this.WriteString(data);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A0AE File Offset: 0x000090AE
		public override void WriteValue(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value, this.resolver));
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A0D5 File Offset: 0x000090D5
		public override void WriteValue(string value)
		{
			this.WriteString(value);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A0DE File Offset: 0x000090DE
		public override void WriteAttributes(XmlReader reader, bool defattr)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A0EF File Offset: 0x000090EF
		public override void WriteNode(XmlReader reader, bool defattr)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A100 File Offset: 0x00009100
		public override void WriteNode(XPathNavigator navigator, bool defattr)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000A111 File Offset: 0x00009111
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000A119 File Offset: 0x00009119
		internal virtual IXmlNamespaceResolver NamespaceResolver
		{
			get
			{
				return this.resolver;
			}
			set
			{
				this.resolver = value;
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A122 File Offset: 0x00009122
		internal virtual void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A124 File Offset: 0x00009124
		internal virtual void WriteXmlDeclaration(string xmldecl)
		{
		}

		// Token: 0x06000281 RID: 641
		internal abstract void StartElementContent();

		// Token: 0x06000282 RID: 642 RVA: 0x0000A126 File Offset: 0x00009126
		internal virtual void OnRootElement(ConformanceLevel conformanceLevel)
		{
		}

		// Token: 0x06000283 RID: 643
		internal abstract void WriteEndElement(string prefix, string localName, string ns);

		// Token: 0x06000284 RID: 644 RVA: 0x0000A128 File Offset: 0x00009128
		internal virtual void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.WriteEndElement(prefix, localName, ns);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A133 File Offset: 0x00009133
		internal virtual void WriteQualifiedName(string prefix, string localName, string ns)
		{
			if (prefix.Length != 0)
			{
				this.WriteString(prefix);
				this.WriteString(":");
			}
			this.WriteString(localName);
		}

		// Token: 0x06000286 RID: 646
		internal abstract void WriteNamespaceDeclaration(string prefix, string ns);

		// Token: 0x06000287 RID: 647 RVA: 0x0000A156 File Offset: 0x00009156
		internal virtual void WriteEndBase64()
		{
			this.base64Encoder.Flush();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000A163 File Offset: 0x00009163
		internal virtual void Close(WriteState currentState)
		{
			this.Close();
		}

		// Token: 0x04000525 RID: 1317
		internal const int SurHighStart = 55296;

		// Token: 0x04000526 RID: 1318
		internal const int SurHighEnd = 56319;

		// Token: 0x04000527 RID: 1319
		internal const int SurLowStart = 56320;

		// Token: 0x04000528 RID: 1320
		internal const int SurLowEnd = 57343;

		// Token: 0x04000529 RID: 1321
		internal const int SurMask = 64512;

		// Token: 0x0400052A RID: 1322
		protected XmlRawWriterBase64Encoder base64Encoder;

		// Token: 0x0400052B RID: 1323
		protected IXmlNamespaceResolver resolver;
	}
}
