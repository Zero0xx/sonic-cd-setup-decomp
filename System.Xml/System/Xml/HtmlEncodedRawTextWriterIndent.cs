using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000054 RID: 84
	internal class HtmlEncodedRawTextWriterIndent : HtmlEncodedRawTextWriter
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x0000D351 File Offset: 0x0000C351
		public HtmlEncodedRawTextWriterIndent(TextWriter writer, XmlWriterSettings settings) : base(writer, settings)
		{
			this.Init(settings);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000D362 File Offset: 0x0000C362
		public HtmlEncodedRawTextWriterIndent(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput) : base(stream, encoding, settings, closeOutput)
		{
			this.Init(settings);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000D376 File Offset: 0x0000C376
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			base.WriteDocType(name, pubid, sysid, subset);
			this.endBlockPos = this.bufPos;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000D390 File Offset: 0x0000C390
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				base.ChangeTextContentMark(false);
			}
			this.elementScope.Push((byte)this.currentElementProperties);
			if (ns.Length == 0)
			{
				this.currentElementProperties = (ElementProperties)HtmlEncodedRawTextWriter.elementPropertySearch.FindCaseInsensitiveString(localName);
				if (this.endBlockPos == this.bufPos && (this.currentElementProperties & ElementProperties.BLOCK_WS) != ElementProperties.DEFAULT)
				{
					this.WriteIndent();
				}
				this.indentLevel++;
				this.bufChars[this.bufPos++] = '<';
			}
			else
			{
				this.currentElementProperties = (ElementProperties)192U;
				if (this.endBlockPos == this.bufPos)
				{
					this.WriteIndent();
				}
				this.indentLevel++;
				this.bufChars[this.bufPos++] = '<';
				if (prefix.Length != 0)
				{
					base.RawText(prefix);
					this.bufChars[this.bufPos++] = ':';
				}
			}
			base.RawText(localName);
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000D4AC File Offset: 0x0000C4AC
		internal override void StartElementContent()
		{
			this.bufChars[this.bufPos++] = '>';
			this.contentPos = this.bufPos;
			if ((this.currentElementProperties & ElementProperties.HEAD) != ElementProperties.DEFAULT)
			{
				this.WriteIndent();
				base.WriteMetaElement();
				this.endBlockPos = this.bufPos;
				return;
			}
			if ((this.currentElementProperties & ElementProperties.BLOCK_WS) != ElementProperties.DEFAULT)
			{
				this.endBlockPos = this.bufPos;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000D51C File Offset: 0x0000C51C
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			this.indentLevel--;
			bool flag = (this.currentElementProperties & ElementProperties.BLOCK_WS) != ElementProperties.DEFAULT;
			if (flag && this.endBlockPos == this.bufPos && this.contentPos != this.bufPos)
			{
				this.WriteIndent();
			}
			base.WriteEndElement(prefix, localName, ns);
			this.contentPos = 0;
			if (flag)
			{
				this.endBlockPos = this.bufPos;
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000D58C File Offset: 0x0000C58C
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (this.newLineOnAttributes)
			{
				base.RawText(this.newLineChars);
				this.indentLevel++;
				this.WriteIndent();
				this.indentLevel--;
			}
			base.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000D5D8 File Offset: 0x0000C5D8
		protected override void FlushBuffer()
		{
			this.endBlockPos = ((this.endBlockPos == this.bufPos) ? 1 : 0);
			base.FlushBuffer();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000D5F8 File Offset: 0x0000C5F8
		private void Init(XmlWriterSettings settings)
		{
			this.indentLevel = 0;
			this.indentChars = settings.IndentChars;
			this.newLineOnAttributes = settings.NewLineOnAttributes;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000D61C File Offset: 0x0000C61C
		private void WriteIndent()
		{
			base.RawText(this.newLineChars);
			for (int i = this.indentLevel; i > 0; i--)
			{
				base.RawText(this.indentChars);
			}
		}

		// Token: 0x04000558 RID: 1368
		private int indentLevel;

		// Token: 0x04000559 RID: 1369
		private int endBlockPos;

		// Token: 0x0400055A RID: 1370
		private string indentChars;

		// Token: 0x0400055B RID: 1371
		private bool newLineOnAttributes;
	}
}
