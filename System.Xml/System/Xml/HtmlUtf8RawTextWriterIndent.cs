using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000058 RID: 88
	internal class HtmlUtf8RawTextWriterIndent : HtmlUtf8RawTextWriter
	{
		// Token: 0x06000332 RID: 818 RVA: 0x00010B21 File Offset: 0x0000FB21
		public HtmlUtf8RawTextWriterIndent(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput) : base(stream, encoding, settings, closeOutput)
		{
			this.Init(settings);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00010B35 File Offset: 0x0000FB35
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			base.WriteDocType(name, pubid, sysid, subset);
			this.endBlockPos = this.bufPos;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00010B50 File Offset: 0x0000FB50
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.elementScope.Push((byte)this.currentElementProperties);
			if (ns.Length == 0)
			{
				this.currentElementProperties = (ElementProperties)HtmlUtf8RawTextWriter.elementPropertySearch.FindCaseInsensitiveString(localName);
				if (this.endBlockPos == this.bufPos && (this.currentElementProperties & ElementProperties.BLOCK_WS) != ElementProperties.DEFAULT)
				{
					this.WriteIndent();
				}
				this.indentLevel++;
				this.bufBytes[this.bufPos++] = 60;
			}
			else
			{
				this.currentElementProperties = (ElementProperties)192U;
				if (this.endBlockPos == this.bufPos)
				{
					this.WriteIndent();
				}
				this.indentLevel++;
				this.bufBytes[this.bufPos++] = 60;
				if (prefix.Length != 0)
				{
					base.RawText(prefix);
					this.bufBytes[this.bufPos++] = 58;
				}
			}
			base.RawText(localName);
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00010C54 File Offset: 0x0000FC54
		internal override void StartElementContent()
		{
			this.bufBytes[this.bufPos++] = 62;
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

		// Token: 0x06000336 RID: 822 RVA: 0x00010CC4 File Offset: 0x0000FCC4
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

		// Token: 0x06000337 RID: 823 RVA: 0x00010D34 File Offset: 0x0000FD34
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

		// Token: 0x06000338 RID: 824 RVA: 0x00010D80 File Offset: 0x0000FD80
		protected override void FlushBuffer()
		{
			this.endBlockPos = ((this.endBlockPos == this.bufPos) ? 1 : 0);
			base.FlushBuffer();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00010DA0 File Offset: 0x0000FDA0
		private void Init(XmlWriterSettings settings)
		{
			this.indentLevel = 0;
			this.indentChars = settings.IndentChars;
			this.newLineOnAttributes = settings.NewLineOnAttributes;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00010DC4 File Offset: 0x0000FDC4
		private void WriteIndent()
		{
			base.RawText(this.newLineChars);
			for (int i = this.indentLevel; i > 0; i--)
			{
				base.RawText(this.indentChars);
			}
		}

		// Token: 0x04000581 RID: 1409
		private int indentLevel;

		// Token: 0x04000582 RID: 1410
		private int endBlockPos;

		// Token: 0x04000583 RID: 1411
		private string indentChars;

		// Token: 0x04000584 RID: 1412
		private bool newLineOnAttributes;
	}
}
