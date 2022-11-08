using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000067 RID: 103
	internal class TextEncodedRawTextWriter : XmlEncodedRawTextWriter
	{
		// Token: 0x06000382 RID: 898 RVA: 0x00011CA4 File Offset: 0x00010CA4
		public TextEncodedRawTextWriter(TextWriter writer, XmlWriterSettings settings) : base(writer, settings)
		{
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00011CAE File Offset: 0x00010CAE
		public TextEncodedRawTextWriter(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput) : base(stream, encoding, settings, closeOutput)
		{
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00011CBB File Offset: 0x00010CBB
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00011CBD File Offset: 0x00010CBD
		internal override void WriteXmlDeclaration(string xmldecl)
		{
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00011CBF File Offset: 0x00010CBF
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00011CC1 File Offset: 0x00010CC1
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00011CC3 File Offset: 0x00010CC3
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00011CC5 File Offset: 0x00010CC5
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00011CC7 File Offset: 0x00010CC7
		internal override void StartElementContent()
		{
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00011CC9 File Offset: 0x00010CC9
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.inAttributeValue = true;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00011CD2 File Offset: 0x00010CD2
		public override void WriteEndAttribute()
		{
			this.inAttributeValue = false;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00011CDB File Offset: 0x00010CDB
		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00011CDD File Offset: 0x00010CDD
		public override void WriteCData(string text)
		{
			base.WriteRaw(text);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00011CE6 File Offset: 0x00010CE6
		public override void WriteComment(string text)
		{
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00011CE8 File Offset: 0x00010CE8
		public override void WriteProcessingInstruction(string name, string text)
		{
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00011CEA File Offset: 0x00010CEA
		public override void WriteEntityRef(string name)
		{
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00011CEC File Offset: 0x00010CEC
		public override void WriteCharEntity(char ch)
		{
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00011CEE File Offset: 0x00010CEE
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00011CF0 File Offset: 0x00010CF0
		public override void WriteWhitespace(string ws)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(ws);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00011D01 File Offset: 0x00010D01
		public override void WriteString(string textBlock)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(textBlock);
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00011D12 File Offset: 0x00010D12
		public override void WriteChars(char[] buffer, int index, int count)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(buffer, index, count);
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00011D25 File Offset: 0x00010D25
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(buffer, index, count);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00011D38 File Offset: 0x00010D38
		public override void WriteRaw(string data)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(data);
			}
		}
	}
}
