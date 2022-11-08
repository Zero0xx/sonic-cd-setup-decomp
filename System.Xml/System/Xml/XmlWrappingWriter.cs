using System;

namespace System.Xml
{
	// Token: 0x02000073 RID: 115
	internal class XmlWrappingWriter : XmlWriter
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x00015068 File Offset: 0x00014068
		internal XmlWrappingWriter(XmlWriter baseWriter)
		{
			this.Writer = baseWriter;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00015077 File Offset: 0x00014077
		public override XmlWriterSettings Settings
		{
			get
			{
				return this.writer.Settings;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00015084 File Offset: 0x00014084
		public override WriteState WriteState
		{
			get
			{
				return this.writer.WriteState;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00015091 File Offset: 0x00014091
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.writer.XmlSpace;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001509E File Offset: 0x0001409E
		public override string XmlLang
		{
			get
			{
				return this.writer.XmlLang;
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000150AB File Offset: 0x000140AB
		public override void WriteStartDocument()
		{
			this.writer.WriteStartDocument();
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000150B8 File Offset: 0x000140B8
		public override void WriteStartDocument(bool standalone)
		{
			this.writer.WriteStartDocument(standalone);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000150C6 File Offset: 0x000140C6
		public override void WriteEndDocument()
		{
			this.writer.WriteEndDocument();
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000150D3 File Offset: 0x000140D3
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this.writer.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000150E5 File Offset: 0x000140E5
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.writer.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x000150F5 File Offset: 0x000140F5
		public override void WriteEndElement()
		{
			this.writer.WriteEndElement();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00015102 File Offset: 0x00014102
		public override void WriteFullEndElement()
		{
			this.writer.WriteFullEndElement();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001510F File Offset: 0x0001410F
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.writer.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001511F File Offset: 0x0001411F
		public override void WriteEndAttribute()
		{
			this.writer.WriteEndAttribute();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001512C File Offset: 0x0001412C
		public override void WriteCData(string text)
		{
			this.writer.WriteCData(text);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001513A File Offset: 0x0001413A
		public override void WriteComment(string text)
		{
			this.writer.WriteComment(text);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00015148 File Offset: 0x00014148
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.writer.WriteProcessingInstruction(name, text);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00015157 File Offset: 0x00014157
		public override void WriteEntityRef(string name)
		{
			this.writer.WriteEntityRef(name);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00015165 File Offset: 0x00014165
		public override void WriteCharEntity(char ch)
		{
			this.writer.WriteCharEntity(ch);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00015173 File Offset: 0x00014173
		public override void WriteWhitespace(string ws)
		{
			this.writer.WriteWhitespace(ws);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00015181 File Offset: 0x00014181
		public override void WriteString(string text)
		{
			this.writer.WriteString(text);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001518F File Offset: 0x0001418F
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.writer.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001519E File Offset: 0x0001419E
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.writer.WriteChars(buffer, index, count);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000151AE File Offset: 0x000141AE
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.writer.WriteRaw(buffer, index, count);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000151BE File Offset: 0x000141BE
		public override void WriteRaw(string data)
		{
			this.writer.WriteRaw(data);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000151CC File Offset: 0x000141CC
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			this.writer.WriteBase64(buffer, index, count);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000151DC File Offset: 0x000141DC
		public override void Close()
		{
			this.writer.Close();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000151E9 File Offset: 0x000141E9
		public override void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000151F6 File Offset: 0x000141F6
		public override string LookupPrefix(string ns)
		{
			return this.writer.LookupPrefix(ns);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00015204 File Offset: 0x00014204
		public override void WriteValue(object value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00015212 File Offset: 0x00014212
		public override void WriteValue(string value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00015220 File Offset: 0x00014220
		public override void WriteValue(bool value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001522E File Offset: 0x0001422E
		public override void WriteValue(DateTime value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001523C File Offset: 0x0001423C
		public override void WriteValue(double value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001524A File Offset: 0x0001424A
		public override void WriteValue(float value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00015258 File Offset: 0x00014258
		public override void WriteValue(decimal value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00015266 File Offset: 0x00014266
		public override void WriteValue(int value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00015274 File Offset: 0x00014274
		public override void WriteValue(long value)
		{
			this.writer.WriteValue(value);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00015282 File Offset: 0x00014282
		protected override void Dispose(bool disposing)
		{
			((IDisposable)this.writer).Dispose();
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001528F File Offset: 0x0001428F
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x00015297 File Offset: 0x00014297
		protected XmlWriter Writer
		{
			get
			{
				return this.writer;
			}
			set
			{
				this.writer = value;
			}
		}

		// Token: 0x040005F7 RID: 1527
		protected XmlWriter writer;
	}
}
