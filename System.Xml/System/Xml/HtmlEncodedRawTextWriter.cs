using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000053 RID: 83
	internal class HtmlEncodedRawTextWriter : XmlEncodedRawTextWriter
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x0000C6EC File Offset: 0x0000B6EC
		public HtmlEncodedRawTextWriter(TextWriter writer, XmlWriterSettings settings) : base(writer, settings)
		{
			this.Init(settings);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000C6FD File Offset: 0x0000B6FD
		public HtmlEncodedRawTextWriter(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput) : base(stream, encoding, settings, closeOutput)
		{
			this.Init(settings);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000C711 File Offset: 0x0000B711
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000C713 File Offset: 0x0000B713
		internal override void WriteXmlDeclaration(string xmldecl)
		{
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C718 File Offset: 0x0000B718
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				base.ChangeTextContentMark(false);
			}
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
				this.bufChars[this.bufPos++] = '"';
			}
			else if (sysid != null)
			{
				base.RawText(" SYSTEM \"");
				base.RawText(sysid);
				this.bufChars[this.bufPos++] = '"';
			}
			else
			{
				this.bufChars[this.bufPos++] = ' ';
			}
			if (subset != null)
			{
				this.bufChars[this.bufPos++] = '[';
				base.RawText(subset);
				this.bufChars[this.bufPos++] = ']';
			}
			this.bufChars[this.bufPos++] = '>';
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C85C File Offset: 0x0000B85C
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.elementScope.Push((byte)this.currentElementProperties);
			if (ns.Length == 0)
			{
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				this.currentElementProperties = (ElementProperties)HtmlEncodedRawTextWriter.elementPropertySearch.FindCaseInsensitiveString(localName);
				this.bufChars[this.bufPos++] = '<';
				base.RawText(localName);
				this.attrEndPos = this.bufPos;
				return;
			}
			this.currentElementProperties = ElementProperties.HAS_NS;
			base.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C8F0 File Offset: 0x0000B8F0
		internal override void StartElementContent()
		{
			this.bufChars[this.bufPos++] = '>';
			this.contentPos = this.bufPos;
			if ((this.currentElementProperties & ElementProperties.HEAD) != ElementProperties.DEFAULT)
			{
				this.WriteMetaElement();
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C934 File Offset: 0x0000B934
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				if ((this.currentElementProperties & ElementProperties.EMPTY) == ElementProperties.DEFAULT)
				{
					this.bufChars[this.bufPos++] = '<';
					this.bufChars[this.bufPos++] = '/';
					base.RawText(localName);
					this.bufChars[this.bufPos++] = '>';
				}
			}
			else
			{
				base.WriteEndElement(prefix, localName, ns);
			}
			this.currentElementProperties = (ElementProperties)this.elementScope.Pop();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000C9DC File Offset: 0x0000B9DC
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				if ((this.currentElementProperties & ElementProperties.EMPTY) == ElementProperties.DEFAULT)
				{
					this.bufChars[this.bufPos++] = '<';
					this.bufChars[this.bufPos++] = '/';
					base.RawText(localName);
					this.bufChars[this.bufPos++] = '>';
				}
			}
			else
			{
				base.WriteFullEndElement(prefix, localName, ns);
			}
			this.currentElementProperties = (ElementProperties)this.elementScope.Pop();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000CA84 File Offset: 0x0000BA84
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				if (this.attrEndPos == this.bufPos)
				{
					this.bufChars[this.bufPos++] = ' ';
				}
				base.RawText(localName);
				if ((this.currentElementProperties & (ElementProperties)7U) != ElementProperties.DEFAULT)
				{
					this.currentAttributeProperties = (AttributeProperties)((ElementProperties)HtmlEncodedRawTextWriter.attributePropertySearch.FindCaseInsensitiveString(localName) & this.currentElementProperties);
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
				this.bufChars[this.bufPos++] = '=';
				this.bufChars[this.bufPos++] = '"';
			}
			else
			{
				base.WriteStartAttribute(prefix, localName, ns);
				this.currentAttributeProperties = AttributeProperties.DEFAULT;
			}
			this.inAttributeValue = true;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000CB6C File Offset: 0x0000BB6C
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
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				this.bufChars[this.bufPos++] = '"';
			}
			this.inAttributeValue = false;
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000CBEC File Offset: 0x0000BBEC
		public override void WriteProcessingInstruction(string target, string text)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				base.ChangeTextContentMark(false);
			}
			this.bufChars[this.bufPos++] = '<';
			this.bufChars[this.bufPos++] = '?';
			base.RawText(target);
			this.bufChars[this.bufPos++] = ' ';
			base.WriteCommentOrPi(text, 63);
			this.bufChars[this.bufPos++] = '>';
			if (this.bufPos > this.bufLen)
			{
				this.FlushBuffer();
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000CC9C File Offset: 0x0000BC9C
		public unsafe override void WriteString(string text)
		{
			if (this.trackTextContent && !this.inTextContent)
			{
				base.ChangeTextContentMark(true);
			}
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

		// Token: 0x060002CE RID: 718 RVA: 0x0000CCF6 File Offset: 0x0000BCF6
		public override void WriteEntityRef(string name)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000CD07 File Offset: 0x0000BD07
		public override void WriteCharEntity(char ch)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000CD18 File Offset: 0x0000BD18
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000CD2C File Offset: 0x0000BD2C
		public unsafe override void WriteChars(char[] buffer, int index, int count)
		{
			if (this.trackTextContent && !this.inTextContent)
			{
				base.ChangeTextContentMark(true);
			}
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

		// Token: 0x060002D2 RID: 722 RVA: 0x0000CD84 File Offset: 0x0000BD84
		private void Init(XmlWriterSettings settings)
		{
			if (HtmlEncodedRawTextWriter.elementPropertySearch == null)
			{
				HtmlEncodedRawTextWriter.attributePropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlAttributes);
				HtmlEncodedRawTextWriter.elementPropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlElements);
			}
			this.elementScope = new ByteStack(10);
			this.uriEscapingBuffer = new byte[5];
			this.currentElementProperties = ElementProperties.DEFAULT;
			this.mediaType = settings.MediaType;
			this.doNotEscapeUriAttributes = settings.DoNotEscapeUriAttributes;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000CDF0 File Offset: 0x0000BDF0
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

		// Token: 0x060002D4 RID: 724 RVA: 0x0000CE59 File Offset: 0x0000BE59
		protected unsafe void WriteHtmlElementTextBlock(char* pSrc, char* pSrcEnd)
		{
			if ((this.currentElementProperties & ElementProperties.NO_ENTITIES) != ElementProperties.DEFAULT)
			{
				base.RawText(pSrc, pSrcEnd);
				return;
			}
			base.WriteElementTextBlock(pSrc, pSrcEnd);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000CE78 File Offset: 0x0000BE78
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

		// Token: 0x060002D6 RID: 726 RVA: 0x0000CEE0 File Offset: 0x0000BEE0
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
			fixed (char* bufChars = this.bufChars)
			{
				char* ptr = bufChars + this.bufPos;
				char c = '\0';
				for (;;)
				{
					char* ptr2 = ptr + (long)(pSrcEnd - pSrc) * 2L / 2L;
					if (ptr2 != bufChars + this.bufLen)
					{
						ptr2 = bufChars + this.bufLen;
					}
					while (ptr < ptr2 && (this.xmlCharType.charProperties[c = *pSrc] & 128) != 0)
					{
						*(ptr++) = c;
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
								goto IL_15E;
							case '\n':
								ptr = XmlEncodedRawTextWriter.LineFeedEntity(ptr);
								goto IL_18A;
							case '\v':
							case '\f':
								break;
							case '\r':
								ptr = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr);
								goto IL_18A;
							default:
								if (c2 == '"')
								{
									ptr = XmlEncodedRawTextWriter.QuoteEntity(ptr);
									goto IL_18A;
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
									ptr = XmlEncodedRawTextWriter.AmpEntity(ptr);
									goto IL_18A;
								}
								*(ptr++) = c;
								goto IL_18A;
							case '\'':
								goto IL_15E;
							default:
								switch (c2)
								{
								case '<':
								case '>':
									goto IL_15E;
								}
								break;
							}
						}
						base.EncodeChar(ref pSrc, pSrcEnd, ref ptr);
						continue;
						IL_18A:
						pSrc++;
						continue;
						IL_15E:
						*(ptr++) = c;
						goto IL_18A;
					}
					this.bufPos = (int)((long)(ptr - bufChars));
					this.FlushBuffer();
					ptr = bufChars + 1;
				}
				this.bufPos = (int)((long)(ptr - bufChars));
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D094 File Offset: 0x0000C094
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
			fixed (char* bufChars = this.bufChars)
			{
				char* ptr = bufChars + this.bufPos;
				char c = '\0';
				for (;;)
				{
					char* ptr2 = ptr + (long)(pSrcEnd - pSrc) * 2L / 2L;
					if (ptr2 != bufChars + this.bufLen)
					{
						ptr2 = bufChars + this.bufLen;
					}
					while (ptr < ptr2 && (this.xmlCharType.charProperties[c = *pSrc] & 128) != 0 && c < '\u0080')
					{
						*(ptr++) = c;
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
								goto IL_175;
							case '\n':
								ptr = XmlEncodedRawTextWriter.LineFeedEntity(ptr);
								goto IL_21C;
							case '\v':
							case '\f':
								break;
							case '\r':
								ptr = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr);
								goto IL_21C;
							default:
								if (c2 == '"')
								{
									ptr = XmlEncodedRawTextWriter.QuoteEntity(ptr);
									goto IL_21C;
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
									ptr = XmlEncodedRawTextWriter.AmpEntity(ptr);
									goto IL_21C;
								}
								*(ptr++) = c;
								goto IL_21C;
							case '\'':
								goto IL_175;
							default:
								switch (c2)
								{
								case '<':
								case '>':
									goto IL_175;
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
								*(ptr++) = '%';
								*(ptr++) = "0123456789ABCDEF"[*ptr4 >> 4];
								*(ptr++) = "0123456789ABCDEF"[(int)(*ptr4 & 15)];
								ptr4++;
							}
						}
						continue;
						IL_21C:
						pSrc++;
						continue;
						IL_175:
						*(ptr++) = c;
						goto IL_21C;
					}
					this.bufPos = (int)((long)(ptr - bufChars));
					this.FlushBuffer();
					ptr = bufChars + 1;
				}
				this.bufPos = (int)((long)(ptr - bufChars));
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D2DC File Offset: 0x0000C2DC
		private void OutputRestAmps()
		{
			this.bufChars[this.bufPos++] = 'a';
			this.bufChars[this.bufPos++] = 'm';
			this.bufChars[this.bufPos++] = 'p';
			this.bufChars[this.bufPos++] = ';';
		}

		// Token: 0x0400054E RID: 1358
		private const int StackIncrement = 10;

		// Token: 0x0400054F RID: 1359
		protected ByteStack elementScope;

		// Token: 0x04000550 RID: 1360
		protected ElementProperties currentElementProperties;

		// Token: 0x04000551 RID: 1361
		private AttributeProperties currentAttributeProperties;

		// Token: 0x04000552 RID: 1362
		private bool endsWithAmpersand;

		// Token: 0x04000553 RID: 1363
		private byte[] uriEscapingBuffer;

		// Token: 0x04000554 RID: 1364
		private string mediaType;

		// Token: 0x04000555 RID: 1365
		private bool doNotEscapeUriAttributes;

		// Token: 0x04000556 RID: 1366
		protected static TernaryTreeReadOnly elementPropertySearch;

		// Token: 0x04000557 RID: 1367
		protected static TernaryTreeReadOnly attributePropertySearch;
	}
}
