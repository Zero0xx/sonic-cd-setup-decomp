using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000068 RID: 104
	internal class TextUtf8RawTextWriter : XmlUtf8RawTextWriter
	{
		// Token: 0x06000399 RID: 921 RVA: 0x00011D49 File Offset: 0x00010D49
		public TextUtf8RawTextWriter(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput) : base(stream, encoding, settings, closeOutput)
		{
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00011D56 File Offset: 0x00010D56
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00011D58 File Offset: 0x00010D58
		internal override void WriteXmlDeclaration(string xmldecl)
		{
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00011D5A File Offset: 0x00010D5A
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00011D5C File Offset: 0x00010D5C
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00011D5E File Offset: 0x00010D5E
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00011D60 File Offset: 0x00010D60
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00011D62 File Offset: 0x00010D62
		internal override void StartElementContent()
		{
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00011D64 File Offset: 0x00010D64
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.inAttributeValue = true;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00011D6D File Offset: 0x00010D6D
		public override void WriteEndAttribute()
		{
			this.inAttributeValue = false;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00011D76 File Offset: 0x00010D76
		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00011D78 File Offset: 0x00010D78
		public override void WriteCData(string text)
		{
			base.WriteRaw(text);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00011D81 File Offset: 0x00010D81
		public override void WriteComment(string text)
		{
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00011D83 File Offset: 0x00010D83
		public override void WriteProcessingInstruction(string name, string text)
		{
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00011D85 File Offset: 0x00010D85
		public override void WriteEntityRef(string name)
		{
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00011D87 File Offset: 0x00010D87
		public override void WriteCharEntity(char ch)
		{
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00011D89 File Offset: 0x00010D89
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00011D8B File Offset: 0x00010D8B
		public override void WriteWhitespace(string ws)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(ws);
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00011D9C File Offset: 0x00010D9C
		public override void WriteString(string textBlock)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(textBlock);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00011DAD File Offset: 0x00010DAD
		public override void WriteChars(char[] buffer, int index, int count)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(buffer, index, count);
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00011DC0 File Offset: 0x00010DC0
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(buffer, index, count);
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00011DD3 File Offset: 0x00010DD3
		public override void WriteRaw(string data)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(data);
			}
		}
	}
}
