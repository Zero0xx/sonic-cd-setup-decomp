using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000057 RID: 87
	internal class HtmlUtf8RawTextWriter : XmlUtf8RawTextWriter
	{
		// Token: 0x0600031B RID: 795 RVA: 0x0000FFB4 File Offset: 0x0000EFB4
		public HtmlUtf8RawTextWriter(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput) : base(stream, encoding, settings, closeOutput)
		{
			this.Init(settings);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000FFC8 File Offset: 0x0000EFC8
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000FFCA File Offset: 0x0000EFCA
		internal override void WriteXmlDeclaration(string xmldecl)
		{
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000FFCC File Offset: 0x0000EFCC
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			base.RawText("<!DOCTYPE ");
			if (name == "HTML")
			{
				base.RawText("HTML");
			}
			else
			{
				base.RawText("html");
			}
			if (pubid != null)
			{
				base.RawText(" PUBLIC \"");
				base.RawText(pubid);
				if (sysid != null)
				{
					base.RawText("\" \"");
					base.RawText(sysid);
				}
				this.bufBytes[this.bufPos++] = 34;
			}
			else if (sysid != null)
			{
				base.RawText(" SYSTEM \"");
				base.RawText(sysid);
				this.bufBytes[this.bufPos++] = 34;
			}
			else
			{
				this.bufBytes[this.bufPos++] = 32;
			}
			if (subset != null)
			{
				this.bufBytes[this.bufPos++] = 91;
				base.RawText(subset);
				this.bufBytes[this.bufPos++] = 93;
			}
			this.bufBytes[this.bufPos++] = 62;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000100F8 File Offset: 0x0000F0F8
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.elementScope.Push((byte)this.currentElementProperties);
			if (ns.Length == 0)
			{
				this.currentElementProperties = (ElementProperties)HtmlUtf8RawTextWriter.elementPropertySearch.FindCaseInsensitiveString(localName);
				this.bufBytes[this.bufPos++] = 60;
				base.RawText(localName);
				this.attrEndPos = this.bufPos;
				return;
			}
			this.currentElementProperties = ElementProperties.HAS_NS;
			base.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00010174 File Offset: 0x0000F174
		internal override void StartElementContent()
		{
			this.bufBytes[this.bufPos++] = 62;
			this.contentPos = this.bufPos;
			if ((this.currentElementProperties & ElementProperties.HEAD) != ElementProperties.DEFAULT)
			{
				this.WriteMetaElement();
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000101B8 File Offset: 0x0000F1B8
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if ((this.currentElementProperties & ElementProperties.EMPTY) == ElementProperties.DEFAULT)
				{
					this.bufBytes[this.bufPos++] = 60;
					this.bufBytes[this.bufPos++] = 47;
					base.RawText(localName);
					this.bufBytes[this.bufPos++] = 62;
				}
			}
			else
			{
				base.WriteEndElement(prefix, localName, ns);
			}
			this.currentElementProperties = (ElementProperties)this.elementScope.Pop();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00010248 File Offset: 0x0000F248
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if ((this.currentElementProperties & ElementProperties.EMPTY) == ElementProperties.DEFAULT)
				{
					this.bufBytes[this.bufPos++] = 60;
					this.bufBytes[this.bufPos++] = 47;
					base.RawText(localName);
					this.bufBytes[this.bufPos++] = 62;
				}
			}
			else
			{
				base.WriteFullEndElement(prefix, localName, ns);
			}
			this.currentElementProperties = (ElementProperties)this.elementScope.Pop();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000102D8 File Offset: 0x0000F2D8
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if (this.attrEndPos == this.bufPos)
				{
					this.bufBytes[this.bufPos++] = 32;
				}
				base.RawText(localName);
				if ((this.currentElementProperties & (ElementProperties)7U) != ElementProperties.DEFAULT)
				{
					this.currentAttributeProperties = (AttributeProperties)((ElementProperties)HtmlUtf8RawTextWriter.attributePropertySearch.FindCaseInsensitiveString(localName) & this.currentElementProperties);
					if ((this.currentAttributeProperties & AttributeProperties.BOOLEAN) != AttributeProperties.DEFAULT)
					{
						this.inAttributeValue = true;
						return;
					}
				}
				else
				{
					this.currentAttributeProperties = AttributeProperties.DEFAULT;
				}
				this.bufBytes[this.bufPos++] = 61;
				this.bufBytes[this.bufPos++] = 34;
			}
			else
			{
				base.WriteStartAttribute(prefix, localName, ns);
				this.currentAttributeProperties = AttributeProperties.DEFAULT;
			}
			this.inAttributeValue = true;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000103A8 File Offset: 0x0000F3A8
		public override void WriteEndAttribute()
		{
			if ((this.currentAttributeProperties & AttributeProperties.BOOLEAN) != AttributeProperties.DEFAULT)
			{
				this.attrEndPos = this.bufPos;
			}
			else
			{
				if (this.endsWithAmpersand)
				{
					this.OutputRestAmps();
					this.endsWithAmpersand = false;
				}
				this.bufBytes[this.bufPos++] = 34;
			}
			this.inAttributeValue = false;
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00010410 File Offset: 0x0000F410
		public override void WriteProcessingInstruction(string target, string text)
		{
			this.bufBytes[this.bufPos++] = 60;
			this.bufBytes[this.bufPos++] = 63;
			base.RawText(target);
			this.bufBytes[this.bufPos++] = 32;
			base.WriteCommentOrPi(text, 63);
			this.bufBytes[this.bufPos++] = 62;
			if (this.bufPos > this.bufLen)
			{
				this.FlushBuffer();
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000104AC File Offset: 0x0000F4AC
		public unsafe override void WriteString(string text)
		{
			fixed (char* ptr = text)
			{
				char* pSrcEnd = ptr + text.Length;
				if (this.inAttributeValue)
				{
					this.WriteHtmlAttributeTextBlock(ptr, pSrcEnd);
				}
				else
				{
					this.WriteHtmlElementTextBlock(ptr, pSrcEnd);
				}
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000104EF File Offset: 0x0000F4EF
		public override void WriteEntityRef(string name)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00010500 File Offset: 0x0000F500
		public override void WriteCharEntity(char ch)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00010511 File Offset: 0x0000F511
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00010524 File Offset: 0x0000F524
		public unsafe override void WriteChars(char[] buffer, int index, int count)
		{
			fixed (char* ptr = &buffer[index])
			{
				if (this.inAttributeValue)
				{
					base.WriteAttributeTextBlock(ptr, ptr + count);
				}
				else
				{
					base.WriteElementTextBlock(ptr, ptr + count);
				}
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00010564 File Offset: 0x0000F564
		private void Init(XmlWriterSettings settings)
		{
			if (HtmlUtf8RawTextWriter.elementPropertySearch == null)
			{
				HtmlUtf8RawTextWriter.attributePropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlAttributes);
				HtmlUtf8RawTextWriter.elementPropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlElements);
			}
			this.elementScope = new ByteStack(10);
			this.uriEscapingBuffer = new byte[5];
			this.currentElementProperties = ElementProperties.DEFAULT;
			this.mediaType = settings.MediaType;
			this.doNotEscapeUriAttributes = settings.DoNotEscapeUriAttributes;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000105D0 File Offset: 0x0000F5D0
		protected void WriteMetaElement()
		{
			base.RawText("<META http-equiv=\"Content-Type\"");
			if (this.mediaType == null)
			{
				this.mediaType = "text/html";
			}
			base.RawText(" content=\"");
			base.RawText(this.mediaType);
			base.RawText("; charset=");
			base.RawText(this.encoding.WebName);
			base.RawText("\">");
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00010639 File Offset: 0x0000F639
		protected unsafe void WriteHtmlElementTextBlock(char* pSrc, char* pSrcEnd)
		{
			if ((this.currentElementProperties & ElementProperties.NO_ENTITIES) != ElementProperties.DEFAULT)
			{
				base.RawText(pSrc, pSrcEnd);
				return;
			}
			base.WriteElementTextBlock(pSrc, pSrcEnd);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00010658 File Offset: 0x0000F658
		protected unsafe void WriteHtmlAttributeTextBlock(char* pSrc, char* pSrcEnd)
		{
			if ((this.currentAttributeProperties & (AttributeProperties)7U) != AttributeProperties.DEFAULT)
			{
				if ((this.currentAttributeProperties & AttributeProperties.BOOLEAN) != AttributeProperties.DEFAULT)
				{
					return;
				}
				if ((this.currentAttributeProperties & (AttributeProperties)5U) != AttributeProperties.DEFAULT && !this.doNotEscapeUriAttributes)
				{
					this.WriteUriAttributeText(pSrc, pSrcEnd);
					return;
				}
				this.WriteHtmlAttributeText(pSrc, pSrcEnd);
				return;
			}
			else
			{
				if ((this.currentElementProperties & ElementProperties.HAS_NS) != ElementProperties.DEFAULT)
				{
					base.WriteAttributeTextBlock(pSrc, pSrcEnd);
					return;
				}
				this.WriteHtmlAttributeText(pSrc, pSrcEnd);
				return;
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000106C0 File Offset: 0x0000F6C0
		private unsafe void WriteHtmlAttributeText(char* pSrc, char* pSrcEnd)
		{
			if (this.endsWithAmpersand)
			{
				if ((long)(pSrcEnd - pSrc) > 0L && *pSrc != '{')
				{
					this.OutputRestAmps();
				}
				this.endsWithAmpersand = false;
			}
			fixed (byte* bufBytes = this.bufBytes)
			{
				byte* ptr = bufBytes + this.bufPos;
				char c = '\0';
				for (;;)
				{
					byte* ptr2 = ptr + (long)(pSrcEnd - pSrc);
					if (ptr2 != bufBytes + this.bufLen)
					{
						ptr2 = bufBytes + this.bufLen;
					}
					while (ptr < ptr2 && (this.xmlCharType.charProperties[c = *pSrc] & 128) != 0 && c <= '\u007f')
					{
						*(ptr++) = (byte)c;
						pSrc++;
					}
					if (pSrc >= pSrcEnd)
					{
						break;
					}
					if (ptr < ptr2)
					{
						char c2 = c;
						if (c2 <= '"')
						{
							switch (c2)
							{
							case '\t':
								goto IL_159;
							case '\n':
								ptr = XmlUtf8RawTextWriter.LineFeedEntity(ptr);
								goto IL_186;
							case '\v':
							case '\f':
								break;
							case '\r':
								ptr = XmlUtf8RawTextWriter.CarriageReturnEntity(ptr);
								goto IL_186;
							default:
								if (c2 == '"')
								{
									ptr = XmlUtf8RawTextWriter.QuoteEntity(ptr);
									goto IL_186;
								}
								break;
							}
						}
						else
						{
							switch (c2)
							{
							case '&':
								if (pSrc + 1 == pSrcEnd)
								{
									this.endsWithAmpersand = true;
								}
								else if (pSrc[1] != '{')
								{
									ptr = XmlUtf8RawTextWriter.AmpEntity(ptr);
									goto IL_186;
								}
								*(ptr++) = (byte)c;
								goto IL_186;
							case '\'':
								goto IL_159;
							default:
								switch (c2)
								{
								case '<':
								case '>':
									goto IL_159;
								}
								break;
							}
						}
						base.EncodeChar(ref pSrc, pSrcEnd, ref ptr);
						continue;
						IL_186:
						pSrc++;
						continue;
						IL_159:
						*(ptr++) = (byte)c;
						goto IL_186;
					}
					this.bufPos = (int)((long)(ptr - bufBytes));
					this.FlushBuffer();
					ptr = bufBytes + 1;
				}
				this.bufPos = (int)((long)(ptr - bufBytes));
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00010870 File Offset: 0x0000F870
		private unsafe void WriteUriAttributeText(char* pSrc, char* pSrcEnd)
		{
			if (this.endsWithAmpersand)
			{
				if ((long)(pSrcEnd - pSrc) > 0L && *pSrc != '{')
				{
					this.OutputRestAmps();
				}
				this.endsWithAmpersand = false;
			}
			fixed (byte* bufBytes = this.bufBytes)
			{
				byte* ptr = bufBytes + this.bufPos;
				char c = '\0';
				for (;;)
				{
					byte* ptr2 = ptr + (long)(pSrcEnd - pSrc);
					if (ptr2 != bufBytes + this.bufLen)
					{
						ptr2 = bufBytes + this.bufLen;
					}
					while (ptr < ptr2 && (this.xmlCharType.charProperties[c = *pSrc] & 128) != 0 && c < '\u0080')
					{
						*(ptr++) = (byte)c;
						pSrc++;
					}
					if (pSrc >= pSrcEnd)
					{
						break;
					}
					if (ptr < ptr2)
					{
						char c2 = c;
						if (c2 <= '"')
						{
							switch (c2)
							{
							case '\t':
								goto IL_168;
							case '\n':
								ptr = XmlUtf8RawTextWriter.LineFeedEntity(ptr);
								goto IL_212;
							case '\v':
							case '\f':
								break;
							case '\r':
								ptr = XmlUtf8RawTextWriter.CarriageReturnEntity(ptr);
								goto IL_212;
							default:
								if (c2 == '"')
								{
									ptr = XmlUtf8RawTextWriter.QuoteEntity(ptr);
									goto IL_212;
								}
								break;
							}
						}
						else
						{
							switch (c2)
							{
							case '&':
								if (pSrc + 1 == pSrcEnd)
								{
									this.endsWithAmpersand = true;
								}
								else if (pSrc[1] != '{')
								{
									ptr = XmlUtf8RawTextWriter.AmpEntity(ptr);
									goto IL_212;
								}
								*(ptr++) = (byte)c;
								goto IL_212;
							case '\'':
								goto IL_168;
							default:
								switch (c2)
								{
								case '<':
								case '>':
									goto IL_168;
								}
								break;
							}
						}
						fixed (byte* ptr3 = this.uriEscapingBuffer)
						{
							byte* ptr4 = ptr3;
							byte* ptr5 = ptr4;
							XmlUtf8RawTextWriter.CharToUTF8(ref pSrc, pSrcEnd, ref ptr5);
							while (ptr4 < ptr5)
							{
								*(ptr++) = 37;
								*(ptr++) = (byte)"0123456789ABCDEF"[*ptr4 >> 4];
								*(ptr++) = (byte)"0123456789ABCDEF"[(int)(*ptr4 & 15)];
								ptr4++;
							}
						}
						continue;
						IL_212:
						pSrc++;
						continue;
						IL_168:
						*(ptr++) = (byte)c;
						goto IL_212;
					}
					this.bufPos = (int)((long)(ptr - bufBytes));
					this.FlushBuffer();
					ptr = bufBytes + 1;
				}
				this.bufPos = (int)((long)(ptr - bufBytes));
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00010AAC File Offset: 0x0000FAAC
		private void OutputRestAmps()
		{
			this.bufBytes[this.bufPos++] = 97;
			this.bufBytes[this.bufPos++] = 109;
			this.bufBytes[this.bufPos++] = 112;
			this.bufBytes[this.bufPos++] = 59;
		}

		// Token: 0x04000577 RID: 1399
		private const int StackIncrement = 10;

		// Token: 0x04000578 RID: 1400
		protected ByteStack elementScope;

		// Token: 0x04000579 RID: 1401
		protected ElementProperties currentElementProperties;

		// Token: 0x0400057A RID: 1402
		private AttributeProperties currentAttributeProperties;

		// Token: 0x0400057B RID: 1403
		private bool endsWithAmpersand;

		// Token: 0x0400057C RID: 1404
		private byte[] uriEscapingBuffer;

		// Token: 0x0400057D RID: 1405
		private string mediaType;

		// Token: 0x0400057E RID: 1406
		private bool doNotEscapeUriAttributes;

		// Token: 0x0400057F RID: 1407
		protected static TernaryTreeReadOnly elementPropertySearch;

		// Token: 0x04000580 RID: 1408
		protected static TernaryTreeReadOnly attributePropertySearch;
	}
}
