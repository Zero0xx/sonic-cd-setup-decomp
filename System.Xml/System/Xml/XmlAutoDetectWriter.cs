using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200006C RID: 108
	internal class XmlAutoDetectWriter : XmlRawWriter, IRemovableWriter
	{
		// Token: 0x060003CC RID: 972 RVA: 0x00012013 File Offset: 0x00011013
		private XmlAutoDetectWriter(XmlWriterSettings writerSettings, Encoding encoding)
		{
			this.writerSettings = writerSettings.Clone();
			this.writerSettings.Encoding = encoding;
			this.writerSettings.ReadOnly = true;
			this.eventCache = new XmlEventCache(string.Empty, true);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00012050 File Offset: 0x00011050
		public XmlAutoDetectWriter(TextWriter textWriter, XmlWriterSettings writerSettings) : this(writerSettings, textWriter.Encoding)
		{
			this.textWriter = textWriter;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00012066 File Offset: 0x00011066
		public XmlAutoDetectWriter(Stream strm, Encoding encoding, XmlWriterSettings writerSettings) : this(writerSettings, encoding)
		{
			this.strm = strm;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00012077 File Offset: 0x00011077
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0001207F File Offset: 0x0001107F
		public OnRemoveWriter OnRemoveWriterEvent
		{
			get
			{
				return this.onRemove;
			}
			set
			{
				this.onRemove = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00012088 File Offset: 0x00011088
		public override XmlWriterSettings Settings
		{
			get
			{
				return this.writerSettings;
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00012090 File Offset: 0x00011090
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000120A9 File Offset: 0x000110A9
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (this.wrapped == null)
			{
				if (ns.Length == 0 && XmlAutoDetectWriter.IsHtmlTag(localName))
				{
					this.CreateWrappedWriter(XmlOutputMethod.Html);
				}
				else
				{
					this.CreateWrappedWriter(XmlOutputMethod.Xml);
				}
			}
			this.wrapped.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000120E1 File Offset: 0x000110E1
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000120F8 File Offset: 0x000110F8
		public override void WriteEndAttribute()
		{
			this.wrapped.WriteEndAttribute();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00012105 File Offset: 0x00011105
		public override void WriteCData(string text)
		{
			if (this.TextBlockCreatesWriter(text))
			{
				this.wrapped.WriteCData(text);
				return;
			}
			this.eventCache.WriteCData(text);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00012129 File Offset: 0x00011129
		public override void WriteComment(string text)
		{
			if (this.wrapped == null)
			{
				this.eventCache.WriteComment(text);
				return;
			}
			this.wrapped.WriteComment(text);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001214C File Offset: 0x0001114C
		public override void WriteProcessingInstruction(string name, string text)
		{
			if (this.wrapped == null)
			{
				this.eventCache.WriteProcessingInstruction(name, text);
				return;
			}
			this.wrapped.WriteProcessingInstruction(name, text);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00012171 File Offset: 0x00011171
		public override void WriteWhitespace(string ws)
		{
			if (this.wrapped == null)
			{
				this.eventCache.WriteWhitespace(ws);
				return;
			}
			this.wrapped.WriteWhitespace(ws);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00012194 File Offset: 0x00011194
		public override void WriteString(string text)
		{
			if (this.TextBlockCreatesWriter(text))
			{
				this.wrapped.WriteString(text);
				return;
			}
			this.eventCache.WriteString(text);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000121B8 File Offset: 0x000111B8
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count));
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000121C8 File Offset: 0x000111C8
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.WriteRaw(new string(buffer, index, count));
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000121D8 File Offset: 0x000111D8
		public override void WriteRaw(string data)
		{
			if (this.TextBlockCreatesWriter(data))
			{
				this.wrapped.WriteRaw(data);
				return;
			}
			this.eventCache.WriteRaw(data);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000121FC File Offset: 0x000111FC
		public override void WriteEntityRef(string name)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteEntityRef(name);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00012211 File Offset: 0x00011211
		public override void WriteCharEntity(char ch)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteCharEntity(ch);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00012226 File Offset: 0x00011226
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001223C File Offset: 0x0001123C
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteBase64(buffer, index, count);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00012253 File Offset: 0x00011253
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteBinHex(buffer, index, count);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001226A File Offset: 0x0001126A
		public override void Close()
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.Close();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001227E File Offset: 0x0001127E
		public override void Flush()
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.Flush();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00012292 File Offset: 0x00011292
		public override void WriteValue(object value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000122A7 File Offset: 0x000112A7
		public override void WriteValue(string value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000122BC File Offset: 0x000112BC
		public override void WriteValue(bool value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000122D1 File Offset: 0x000112D1
		public override void WriteValue(DateTime value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000122E6 File Offset: 0x000112E6
		public override void WriteValue(double value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000122FB File Offset: 0x000112FB
		public override void WriteValue(float value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00012310 File Offset: 0x00011310
		public override void WriteValue(decimal value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00012325 File Offset: 0x00011325
		public override void WriteValue(int value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001233A File Offset: 0x0001133A
		public override void WriteValue(long value)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteValue(value);
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0001234F File Offset: 0x0001134F
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00012357 File Offset: 0x00011357
		internal override IXmlNamespaceResolver NamespaceResolver
		{
			get
			{
				return this.resolver;
			}
			set
			{
				this.resolver = value;
				if (this.wrapped == null)
				{
					this.eventCache.NamespaceResolver = value;
					return;
				}
				this.wrapped.NamespaceResolver = value;
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00012381 File Offset: 0x00011381
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteXmlDeclaration(standalone);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00012396 File Offset: 0x00011396
		internal override void WriteXmlDeclaration(string xmldecl)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteXmlDeclaration(xmldecl);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000123AB File Offset: 0x000113AB
		internal override void StartElementContent()
		{
			this.wrapped.StartElementContent();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000123B8 File Offset: 0x000113B8
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			this.wrapped.WriteEndElement(prefix, localName, ns);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000123C8 File Offset: 0x000113C8
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.wrapped.WriteFullEndElement(prefix, localName, ns);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000123D8 File Offset: 0x000113D8
		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
			this.EnsureWrappedWriter(XmlOutputMethod.Xml);
			this.wrapped.WriteNamespaceDeclaration(prefix, ns);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000123F0 File Offset: 0x000113F0
		private static bool IsHtmlTag(string tagName)
		{
			return tagName.Length == 4 && (tagName[0] == 'H' || tagName[0] == 'h') && (tagName[1] == 'T' || tagName[1] == 't') && (tagName[2] == 'M' || tagName[2] == 'm') && (tagName[3] == 'L' || tagName[3] == 'l');
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00012469 File Offset: 0x00011469
		private void EnsureWrappedWriter(XmlOutputMethod outMethod)
		{
			if (this.wrapped == null)
			{
				this.CreateWrappedWriter(outMethod);
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001247C File Offset: 0x0001147C
		private bool TextBlockCreatesWriter(string textBlock)
		{
			if (this.wrapped == null)
			{
				if (XmlCharType.Instance.IsOnlyWhitespace(textBlock))
				{
					return false;
				}
				this.CreateWrappedWriter(XmlOutputMethod.Xml);
			}
			return true;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000124AC File Offset: 0x000114AC
		private void CreateWrappedWriter(XmlOutputMethod outMethod)
		{
			this.writerSettings.ReadOnly = false;
			this.writerSettings.OutputMethod = outMethod;
			if (outMethod == XmlOutputMethod.Html && this.writerSettings.InternalIndent == TriState.Unknown)
			{
				this.writerSettings.Indent = true;
			}
			this.writerSettings.ReadOnly = true;
			if (this.textWriter != null)
			{
				this.wrapped = ((XmlWellFormedWriter)XmlWriter.Create(this.textWriter, this.writerSettings)).RawWriter;
			}
			else
			{
				this.wrapped = ((XmlWellFormedWriter)XmlWriter.Create(this.strm, this.writerSettings)).RawWriter;
			}
			this.eventCache.EndEvents();
			this.eventCache.EventsToWriter(this.wrapped);
			if (this.onRemove != null)
			{
				this.onRemove(this.wrapped);
			}
		}

		// Token: 0x040005D9 RID: 1497
		private XmlRawWriter wrapped;

		// Token: 0x040005DA RID: 1498
		private OnRemoveWriter onRemove;

		// Token: 0x040005DB RID: 1499
		private XmlWriterSettings writerSettings;

		// Token: 0x040005DC RID: 1500
		private XmlEventCache eventCache;

		// Token: 0x040005DD RID: 1501
		private TextWriter textWriter;

		// Token: 0x040005DE RID: 1502
		private Stream strm;
	}
}
