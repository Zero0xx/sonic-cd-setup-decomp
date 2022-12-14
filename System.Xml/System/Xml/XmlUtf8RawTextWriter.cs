using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000056 RID: 86
	internal class XmlUtf8RawTextWriter : XmlRawWriter
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000DDF0 File Offset: 0x0000CDF0
		protected XmlUtf8RawTextWriter(XmlWriterSettings settings, bool closeOutput)
		{
			this.newLineHandling = settings.NewLineHandling;
			this.omitXmlDeclaration = settings.OmitXmlDeclaration;
			this.newLineChars = settings.NewLineChars;
			this.standalone = settings.Standalone;
			this.outputMethod = settings.OutputMethod;
			this.checkCharacters = settings.CheckCharacters;
			this.mergeCDataSections = settings.MergeCDataSections;
			this.closeOutput = closeOutput;
			if (this.checkCharacters && this.newLineHandling == NewLineHandling.Replace)
			{
				this.ValidateContentChars(this.newLineChars, "NewLineChars", false);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000DEA4 File Offset: 0x0000CEA4
		public XmlUtf8RawTextWriter(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput) : this(settings, closeOutput)
		{
			this.stream = stream;
			this.encoding = encoding;
			this.bufBytes = new byte[6176];
			if (!stream.CanSeek || stream.Position == 0L)
			{
				byte[] preamble = encoding.GetPreamble();
				if (preamble.Length != 0)
				{
					Buffer.BlockCopy(preamble, 0, this.bufBytes, 1, preamble.Length);
					this.bufPos += preamble.Length;
					this.textPos += preamble.Length;
				}
			}
			if (settings.AutoXmlDeclaration)
			{
				this.WriteXmlDeclaration(this.standalone);
				this.autoXmlDeclaration = true;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000DF44 File Offset: 0x0000CF44
		public override XmlWriterSettings Settings
		{
			get
			{
				return new XmlWriterSettings
				{
					Encoding = this.encoding,
					OmitXmlDeclaration = this.omitXmlDeclaration,
					NewLineHandling = this.newLineHandling,
					NewLineChars = this.newLineChars,
					CloseOutput = this.closeOutput,
					ConformanceLevel = ConformanceLevel.Auto,
					AutoXmlDeclaration = this.autoXmlDeclaration,
					Standalone = this.standalone,
					OutputMethod = this.outputMethod,
					CheckCharacters = this.checkCharacters,
					ReadOnly = true
				};
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000DFD4 File Offset: 0x0000CFD4
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
			if (!this.omitXmlDeclaration && !this.autoXmlDeclaration)
			{
				this.RawText("<?xml version=\"");
				this.RawText("1.0");
				if (this.encoding != null)
				{
					this.RawText("\" encoding=\"");
					this.RawText((this.encoding.CodePage == 1201) ? "UTF-16BE" : this.encoding.WebName);
				}
				if (standalone != XmlStandalone.Omit)
				{
					this.RawText("\" standalone=\"");
					this.RawText((standalone == XmlStandalone.Yes) ? "yes" : "no");
				}
				this.RawText("\"?>");
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000E079 File Offset: 0x0000D079
		internal override void WriteXmlDeclaration(string xmldecl)
		{
			if (!this.omitXmlDeclaration && !this.autoXmlDeclaration)
			{
				this.WriteProcessingInstruction("xml", xmldecl);
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000E098 File Offset: 0x0000D098
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this.RawText("<!DOCTYPE ");
			this.RawText(name);
			if (pubid != null)
			{
				this.RawText(" PUBLIC \"");
				this.RawText(pubid);
				this.RawText("\" \"");
				if (sysid != null)
				{
					this.RawText(sysid);
				}
				this.bufBytes[this.bufPos++] = 34;
			}
			else if (sysid != null)
			{
				this.RawText(" SYSTEM \"");
				this.RawText(sysid);
				this.bufBytes[this.bufPos++] = 34;
			}
			else
			{
				this.bufBytes[this.bufPos++] = 32;
			}
			if (subset != null)
			{
				this.bufBytes[this.bufPos++] = 91;
				this.RawText(subset);
				this.bufBytes[this.bufPos++] = 93;
			}
			this.bufBytes[this.bufPos++] = 62;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000E1A8 File Offset: 0x0000D1A8
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.bufBytes[this.bufPos++] = 60;
			if (prefix != null && prefix.Length != 0)
			{
				this.RawText(prefix);
				this.bufBytes[this.bufPos++] = 58;
			}
			this.RawText(localName);
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000E210 File Offset: 0x0000D210
		internal override void StartElementContent()
		{
			this.bufBytes[this.bufPos++] = 62;
			this.contentPos = this.bufPos;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000E244 File Offset: 0x0000D244
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			if (this.contentPos != this.bufPos)
			{
				this.bufBytes[this.bufPos++] = 60;
				this.bufBytes[this.bufPos++] = 47;
				if (prefix != null && prefix.Length != 0)
				{
					this.RawText(prefix);
					this.bufBytes[this.bufPos++] = 58;
				}
				this.RawText(localName);
				this.bufBytes[this.bufPos++] = 62;
				return;
			}
			this.bufPos--;
			this.bufBytes[this.bufPos++] = 32;
			this.bufBytes[this.bufPos++] = 47;
			this.bufBytes[this.bufPos++] = 62;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000E348 File Offset: 0x0000D348
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.bufBytes[this.bufPos++] = 60;
			this.bufBytes[this.bufPos++] = 47;
			if (prefix != null && prefix.Length != 0)
			{
				this.RawText(prefix);
				this.bufBytes[this.bufPos++] = 58;
			}
			this.RawText(localName);
			this.bufBytes[this.bufPos++] = 62;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000E3D8 File Offset: 0x0000D3D8
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (this.attrEndPos == this.bufPos)
			{
				this.bufBytes[this.bufPos++] = 32;
			}
			if (prefix != null && prefix.Length > 0)
			{
				this.RawText(prefix);
				this.bufBytes[this.bufPos++] = 58;
			}
			this.RawText(localName);
			this.bufBytes[this.bufPos++] = 61;
			this.bufBytes[this.bufPos++] = 34;
			this.inAttributeValue = true;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000E47C File Offset: 0x0000D47C
		public override void WriteEndAttribute()
		{
			this.bufBytes[this.bufPos++] = 34;
			this.inAttributeValue = false;
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000E4B8 File Offset: 0x0000D4B8
		internal override void WriteNamespaceDeclaration(string prefix, string namespaceName)
		{
			if (prefix.Length == 0)
			{
				this.RawText(" xmlns=\"");
			}
			else
			{
				this.RawText(" xmlns:");
				this.RawText(prefix);
				this.bufBytes[this.bufPos++] = 61;
				this.bufBytes[this.bufPos++] = 34;
			}
			this.inAttributeValue = true;
			this.WriteString(namespaceName);
			this.inAttributeValue = false;
			this.bufBytes[this.bufPos++] = 34;
			this.attrEndPos = this.bufPos;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000E55C File Offset: 0x0000D55C
		public override void WriteCData(string text)
		{
			if (this.mergeCDataSections && this.bufPos == this.cdataPos)
			{
				this.bufPos -= 3;
			}
			else
			{
				this.bufBytes[this.bufPos++] = 60;
				this.bufBytes[this.bufPos++] = 33;
				this.bufBytes[this.bufPos++] = 91;
				this.bufBytes[this.bufPos++] = 67;
				this.bufBytes[this.bufPos++] = 68;
				this.bufBytes[this.bufPos++] = 65;
				this.bufBytes[this.bufPos++] = 84;
				this.bufBytes[this.bufPos++] = 65;
				this.bufBytes[this.bufPos++] = 91;
			}
			this.WriteCDataSection(text);
			this.bufBytes[this.bufPos++] = 93;
			this.bufBytes[this.bufPos++] = 93;
			this.bufBytes[this.bufPos++] = 62;
			this.textPos = this.bufPos;
			this.cdataPos = this.bufPos;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000E6FC File Offset: 0x0000D6FC
		public override void WriteComment(string text)
		{
			this.bufBytes[this.bufPos++] = 60;
			this.bufBytes[this.bufPos++] = 33;
			this.bufBytes[this.bufPos++] = 45;
			this.bufBytes[this.bufPos++] = 45;
			this.WriteCommentOrPi(text, 45);
			this.bufBytes[this.bufPos++] = 45;
			this.bufBytes[this.bufPos++] = 45;
			this.bufBytes[this.bufPos++] = 62;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000E7D0 File Offset: 0x0000D7D0
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.bufBytes[this.bufPos++] = 60;
			this.bufBytes[this.bufPos++] = 63;
			this.RawText(name);
			if (text.Length > 0)
			{
				this.bufBytes[this.bufPos++] = 32;
				this.WriteCommentOrPi(text, 63);
			}
			this.bufBytes[this.bufPos++] = 63;
			this.bufBytes[this.bufPos++] = 62;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000E87C File Offset: 0x0000D87C
		public override void WriteEntityRef(string name)
		{
			this.bufBytes[this.bufPos++] = 38;
			this.RawText(name);
			this.bufBytes[this.bufPos++] = 59;
			if (this.bufPos > this.bufLen)
			{
				this.FlushBuffer();
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000E8E4 File Offset: 0x0000D8E4
		public override void WriteCharEntity(char ch)
		{
			int num = (int)ch;
			string s = num.ToString("X", NumberFormatInfo.InvariantInfo);
			if (this.checkCharacters && !this.xmlCharType.IsCharData(ch))
			{
				throw XmlConvert.CreateInvalidCharException(ch);
			}
			this.bufBytes[this.bufPos++] = 38;
			this.bufBytes[this.bufPos++] = 35;
			this.bufBytes[this.bufPos++] = 120;
			this.RawText(s);
			this.bufBytes[this.bufPos++] = 59;
			if (this.bufPos > this.bufLen)
			{
				this.FlushBuffer();
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000E9B8 File Offset: 0x0000D9B8
		public unsafe override void WriteWhitespace(string ws)
		{
			fixed (char* ptr = ws)
			{
				char* pSrcEnd = ptr + ws.Length;
				if (this.inAttributeValue)
				{
					this.WriteAttributeTextBlock(ptr, pSrcEnd);
				}
				else
				{
					this.WriteElementTextBlock(ptr, pSrcEnd);
				}
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000E9FC File Offset: 0x0000D9FC
		public unsafe override void WriteString(string text)
		{
			fixed (char* ptr = text)
			{
				char* pSrcEnd = ptr + text.Length;
				if (this.inAttributeValue)
				{
					this.WriteAttributeTextBlock(ptr, pSrcEnd);
				}
				else
				{
					this.WriteElementTextBlock(ptr, pSrcEnd);
				}
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000EA40 File Offset: 0x0000DA40
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			int num = (int)(lowChar - '\udc00') | (int)((int)(highChar - '\ud800') << 10) + 65536;
			this.bufBytes[this.bufPos++] = 38;
			this.bufBytes[this.bufPos++] = 35;
			this.bufBytes[this.bufPos++] = 120;
			this.RawText(num.ToString("X", NumberFormatInfo.InvariantInfo));
			this.bufBytes[this.bufPos++] = 59;
			this.textPos = this.bufPos;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000EAF4 File Offset: 0x0000DAF4
		public unsafe override void WriteChars(char[] buffer, int index, int count)
		{
			fixed (char* ptr = &buffer[index])
			{
				if (this.inAttributeValue)
				{
					this.WriteAttributeTextBlock(ptr, ptr + count);
				}
				else
				{
					this.WriteElementTextBlock(ptr, ptr + count);
				}
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000EB34 File Offset: 0x0000DB34
		public unsafe override void WriteRaw(char[] buffer, int index, int count)
		{
			fixed (char* ptr = &buffer[index])
			{
				this.WriteRawWithCharChecking(ptr, ptr + count);
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000EB68 File Offset: 0x0000DB68
		public unsafe override void WriteRaw(string data)
		{
			fixed (char* ptr = data)
			{
				this.WriteRawWithCharChecking(ptr, ptr + data.Length);
			}
			this.textPos = this.bufPos;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000EBA4 File Offset: 0x0000DBA4
		public override void Close()
		{
			this.FlushBuffer();
			this.FlushEncoder();
			this.writeToNull = true;
			if (this.stream != null)
			{
				this.stream.Flush();
				if (this.closeOutput)
				{
					this.stream.Close();
				}
				this.stream = null;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000EBF1 File Offset: 0x0000DBF1
		public override void Flush()
		{
			this.FlushBuffer();
			this.FlushEncoder();
			if (this.stream != null)
			{
				this.stream.Flush();
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000EC14 File Offset: 0x0000DC14
		protected virtual void FlushBuffer()
		{
			try
			{
				if (!this.writeToNull)
				{
					this.stream.Write(this.bufBytes, 1, this.bufPos - 1);
				}
			}
			catch
			{
				this.writeToNull = true;
				throw;
			}
			finally
			{
				this.bufBytes[0] = this.bufBytes[this.bufPos - 1];
				if (XmlUtf8RawTextWriter.IsSurrogateByte(this.bufBytes[0]))
				{
					this.bufBytes[1] = this.bufBytes[this.bufPos];
					this.bufBytes[2] = this.bufBytes[this.bufPos + 1];
					this.bufBytes[3] = this.bufBytes[this.bufPos + 2];
				}
				this.textPos = ((this.textPos == this.bufPos) ? 1 : 0);
				this.attrEndPos = ((this.attrEndPos == this.bufPos) ? 1 : 0);
				this.contentPos = 0;
				this.cdataPos = 0;
				this.bufPos = 1;
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000ED20 File Offset: 0x0000DD20
		private void FlushEncoder()
		{
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000ED24 File Offset: 0x0000DD24
		protected unsafe void WriteAttributeTextBlock(char* pSrc, char* pSrcEnd)
		{
			fixed (byte* ptr = this.bufBytes)
			{
				byte* ptr2 = ptr + this.bufPos;
				int num = 0;
				for (;;)
				{
					byte* ptr3 = ptr2 + (long)(pSrcEnd - pSrc);
					if (ptr3 != ptr + this.bufLen)
					{
						ptr3 = ptr + this.bufLen;
					}
					while (ptr2 < ptr3 && (this.xmlCharType.charProperties[num = (int)(*pSrc)] & 128) != 0 && num <= 127)
					{
						*ptr2 = (byte)num;
						ptr2++;
						pSrc++;
					}
					if (pSrc >= pSrcEnd)
					{
						break;
					}
					if (ptr2 >= ptr3)
					{
						this.bufPos = (int)((long)(ptr2 - ptr));
						this.FlushBuffer();
						ptr2 = ptr + 1;
					}
					else
					{
						int num2 = num;
						if (num2 <= 34)
						{
							switch (num2)
							{
							case 9:
								if (this.newLineHandling == NewLineHandling.None)
								{
									*ptr2 = (byte)num;
									ptr2++;
									goto IL_1FD;
								}
								ptr2 = XmlUtf8RawTextWriter.TabEntity(ptr2);
								goto IL_1FD;
							case 10:
								if (this.newLineHandling == NewLineHandling.None)
								{
									*ptr2 = (byte)num;
									ptr2++;
									goto IL_1FD;
								}
								ptr2 = XmlUtf8RawTextWriter.LineFeedEntity(ptr2);
								goto IL_1FD;
							case 11:
							case 12:
								break;
							case 13:
								if (this.newLineHandling == NewLineHandling.None)
								{
									*ptr2 = (byte)num;
									ptr2++;
									goto IL_1FD;
								}
								ptr2 = XmlUtf8RawTextWriter.CarriageReturnEntity(ptr2);
								goto IL_1FD;
							default:
								if (num2 == 34)
								{
									ptr2 = XmlUtf8RawTextWriter.QuoteEntity(ptr2);
									goto IL_1FD;
								}
								break;
							}
						}
						else
						{
							switch (num2)
							{
							case 38:
								ptr2 = XmlUtf8RawTextWriter.AmpEntity(ptr2);
								goto IL_1FD;
							case 39:
								*ptr2 = (byte)num;
								ptr2++;
								goto IL_1FD;
							default:
								switch (num2)
								{
								case 60:
									ptr2 = XmlUtf8RawTextWriter.LtEntity(ptr2);
									goto IL_1FD;
								case 62:
									ptr2 = XmlUtf8RawTextWriter.GtEntity(ptr2);
									goto IL_1FD;
								}
								break;
							}
						}
						if (XmlUtf8RawTextWriter.InRange(num, 55296, 57343))
						{
							ptr2 = XmlUtf8RawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, ptr2);
							pSrc += 2;
							continue;
						}
						if (num <= 127 || num >= 65534)
						{
							ptr2 = this.InvalidXmlChar(num, ptr2, true);
							pSrc++;
							continue;
						}
						ptr2 = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(num, ptr2);
						pSrc++;
						continue;
						IL_1FD:
						pSrc++;
					}
				}
				this.bufPos = (int)((long)(ptr2 - ptr));
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000EF4C File Offset: 0x0000DF4C
		protected unsafe void WriteElementTextBlock(char* pSrc, char* pSrcEnd)
		{
			fixed (byte* ptr = this.bufBytes)
			{
				byte* ptr2 = ptr + this.bufPos;
				int num = 0;
				for (;;)
				{
					byte* ptr3 = ptr2 + (long)(pSrcEnd - pSrc);
					if (ptr3 != ptr + this.bufLen)
					{
						ptr3 = ptr + this.bufLen;
					}
					while (ptr2 < ptr3 && (this.xmlCharType.charProperties[num = (int)(*pSrc)] & 128) != 0 && num <= 127)
					{
						*ptr2 = (byte)num;
						ptr2++;
						pSrc++;
					}
					if (pSrc >= pSrcEnd)
					{
						break;
					}
					if (ptr2 < ptr3)
					{
						int num2 = num;
						if (num2 <= 34)
						{
							switch (num2)
							{
							case 9:
								goto IL_128;
							case 10:
								if (this.newLineHandling == NewLineHandling.Replace)
								{
									ptr2 = this.WriteNewLine(ptr2);
									goto IL_201;
								}
								*ptr2 = (byte)num;
								ptr2++;
								goto IL_201;
							case 11:
							case 12:
								break;
							case 13:
								switch (this.newLineHandling)
								{
								case NewLineHandling.Replace:
									if (pSrc[1] == '\n')
									{
										pSrc++;
									}
									ptr2 = this.WriteNewLine(ptr2);
									goto IL_201;
								case NewLineHandling.Entitize:
									ptr2 = XmlUtf8RawTextWriter.CarriageReturnEntity(ptr2);
									goto IL_201;
								case NewLineHandling.None:
									*ptr2 = (byte)num;
									ptr2++;
									goto IL_201;
								default:
									goto IL_201;
								}
								break;
							default:
								if (num2 == 34)
								{
									goto IL_128;
								}
								break;
							}
						}
						else
						{
							switch (num2)
							{
							case 38:
								ptr2 = XmlUtf8RawTextWriter.AmpEntity(ptr2);
								goto IL_201;
							case 39:
								goto IL_128;
							default:
								switch (num2)
								{
								case 60:
									ptr2 = XmlUtf8RawTextWriter.LtEntity(ptr2);
									goto IL_201;
								case 62:
									ptr2 = XmlUtf8RawTextWriter.GtEntity(ptr2);
									goto IL_201;
								}
								break;
							}
						}
						if (XmlUtf8RawTextWriter.InRange(num, 55296, 57343))
						{
							ptr2 = XmlUtf8RawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, ptr2);
							pSrc += 2;
							continue;
						}
						if (num <= 127 || num >= 65534)
						{
							ptr2 = this.InvalidXmlChar(num, ptr2, true);
							pSrc++;
							continue;
						}
						ptr2 = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(num, ptr2);
						pSrc++;
						continue;
						IL_201:
						pSrc++;
						continue;
						IL_128:
						*ptr2 = (byte)num;
						ptr2++;
						goto IL_201;
					}
					this.bufPos = (int)((long)(ptr2 - ptr));
					this.FlushBuffer();
					ptr2 = ptr + 1;
				}
				this.bufPos = (int)((long)(ptr2 - ptr));
				this.textPos = this.bufPos;
				this.contentPos = 0;
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000F18C File Offset: 0x0000E18C
		protected unsafe void RawText(string s)
		{
			fixed (char* ptr = s)
			{
				this.RawText(ptr, ptr + s.Length);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000F1BC File Offset: 0x0000E1BC
		protected unsafe void RawText(char* pSrcBegin, char* pSrcEnd)
		{
			fixed (byte* ptr = this.bufBytes)
			{
				byte* ptr2 = ptr + this.bufPos;
				char* ptr3 = pSrcBegin;
				int num = 0;
				for (;;)
				{
					byte* ptr4 = ptr2 + (long)(pSrcEnd - ptr3);
					if (ptr4 != ptr + this.bufLen)
					{
						ptr4 = ptr + this.bufLen;
					}
					while (ptr2 < ptr4 && (num = (int)(*ptr3)) <= 127)
					{
						ptr3++;
						*ptr2 = (byte)num;
						ptr2++;
					}
					if (ptr3 >= pSrcEnd)
					{
						break;
					}
					if (ptr2 >= ptr4)
					{
						this.bufPos = (int)((long)(ptr2 - ptr));
						this.FlushBuffer();
						ptr2 = ptr + 1;
					}
					else if (XmlUtf8RawTextWriter.InRange(num, 55296, 57343))
					{
						ptr2 = XmlUtf8RawTextWriter.EncodeSurrogate(ptr3, pSrcEnd, ptr2);
						ptr3 += 2;
					}
					else if (num <= 127 || num >= 65534)
					{
						ptr2 = this.InvalidXmlChar(num, ptr2, false);
						ptr3++;
					}
					else
					{
						ptr2 = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(num, ptr2);
						ptr3++;
					}
				}
				this.bufPos = (int)((long)(ptr2 - ptr));
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000F2C4 File Offset: 0x0000E2C4
		protected unsafe void WriteRawWithCharChecking(char* pSrcBegin, char* pSrcEnd)
		{
			fixed (byte* ptr = this.bufBytes)
			{
				char* ptr2 = pSrcBegin;
				byte* ptr3 = ptr + this.bufPos;
				int num = 0;
				for (;;)
				{
					byte* ptr4 = ptr3 + (long)(pSrcEnd - ptr2);
					if (ptr4 != ptr + this.bufLen)
					{
						ptr4 = ptr + this.bufLen;
					}
					while (ptr3 < ptr4 && (this.xmlCharType.charProperties[num = (int)(*ptr2)] & 64) != 0 && num <= 127)
					{
						*ptr3 = (byte)num;
						ptr3++;
						ptr2++;
					}
					if (ptr2 >= pSrcEnd)
					{
						break;
					}
					if (ptr3 < ptr4)
					{
						int num2 = num;
						if (num2 <= 38)
						{
							switch (num2)
							{
							case 9:
								goto IL_E3;
							case 10:
								if (this.newLineHandling == NewLineHandling.Replace)
								{
									ptr3 = this.WriteNewLine(ptr3);
									goto IL_194;
								}
								*ptr3 = (byte)num;
								ptr3++;
								goto IL_194;
							case 11:
							case 12:
								break;
							case 13:
								if (this.newLineHandling == NewLineHandling.Replace)
								{
									if (ptr2[1] == '\n')
									{
										ptr2++;
									}
									ptr3 = this.WriteNewLine(ptr3);
									goto IL_194;
								}
								*ptr3 = (byte)num;
								ptr3++;
								goto IL_194;
							default:
								if (num2 == 38)
								{
									goto IL_E3;
								}
								break;
							}
						}
						else if (num2 == 60 || num2 == 93)
						{
							goto IL_E3;
						}
						if (XmlUtf8RawTextWriter.InRange(num, 55296, 57343))
						{
							ptr3 = XmlUtf8RawTextWriter.EncodeSurrogate(ptr2, pSrcEnd, ptr3);
							ptr2 += 2;
							continue;
						}
						if (num <= 127 || num >= 65534)
						{
							ptr3 = this.InvalidXmlChar(num, ptr3, false);
							ptr2++;
							continue;
						}
						ptr3 = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(num, ptr3);
						ptr2++;
						continue;
						IL_194:
						ptr2++;
						continue;
						IL_E3:
						*ptr3 = (byte)num;
						ptr3++;
						goto IL_194;
					}
					this.bufPos = (int)((long)(ptr3 - ptr));
					this.FlushBuffer();
					ptr3 = ptr + 1;
				}
				this.bufPos = (int)((long)(ptr3 - ptr));
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000F480 File Offset: 0x0000E480
		protected unsafe void WriteCommentOrPi(string text, int stopChar)
		{
			fixed (char* ptr = text)
			{
				fixed (byte* ptr2 = this.bufBytes)
				{
					char* ptr3 = ptr;
					char* ptr4 = ptr + text.Length;
					byte* ptr5 = ptr2 + this.bufPos;
					int num = 0;
					for (;;)
					{
						byte* ptr6 = ptr5 + (long)(ptr4 - ptr3);
						if (ptr6 != ptr2 + this.bufLen)
						{
							ptr6 = ptr2 + this.bufLen;
						}
						while (ptr5 < ptr6 && (this.xmlCharType.charProperties[num = (int)(*ptr3)] & 64) != 0 && num != stopChar && num <= 127)
						{
							*ptr5 = (byte)num;
							ptr5++;
							ptr3++;
						}
						if (ptr3 >= ptr4)
						{
							break;
						}
						if (ptr5 < ptr6)
						{
							int num2 = num;
							if (num2 <= 45)
							{
								switch (num2)
								{
								case 9:
									goto IL_210;
								case 10:
									if (this.newLineHandling == NewLineHandling.Replace)
									{
										ptr5 = this.WriteNewLine(ptr5);
										goto IL_282;
									}
									*ptr5 = (byte)num;
									ptr5++;
									goto IL_282;
								case 11:
								case 12:
									break;
								case 13:
									if (this.newLineHandling == NewLineHandling.Replace)
									{
										if (ptr3[1] == '\n')
										{
											ptr3++;
										}
										ptr5 = this.WriteNewLine(ptr5);
										goto IL_282;
									}
									*ptr5 = (byte)num;
									ptr5++;
									goto IL_282;
								default:
									if (num2 == 38)
									{
										goto IL_210;
									}
									if (num2 == 45)
									{
										*ptr5 = 45;
										ptr5++;
										if (num == stopChar && (ptr3 + 1 == ptr4 || ptr3[1] == '-'))
										{
											*ptr5 = 32;
											ptr5++;
											goto IL_282;
										}
										goto IL_282;
									}
									break;
								}
							}
							else
							{
								if (num2 == 60)
								{
									goto IL_210;
								}
								if (num2 != 63)
								{
									if (num2 == 93)
									{
										*ptr5 = 93;
										ptr5++;
										goto IL_282;
									}
								}
								else
								{
									*ptr5 = 63;
									ptr5++;
									if (num == stopChar && ptr3 + 1 < ptr4 && ptr3[1] == '>')
									{
										*ptr5 = 32;
										ptr5++;
										goto IL_282;
									}
									goto IL_282;
								}
							}
							if (XmlUtf8RawTextWriter.InRange(num, 55296, 57343))
							{
								ptr5 = XmlUtf8RawTextWriter.EncodeSurrogate(ptr3, ptr4, ptr5);
								ptr3 += 2;
								continue;
							}
							if (num <= 127 || num >= 65534)
							{
								ptr5 = this.InvalidXmlChar(num, ptr5, false);
								ptr3++;
								continue;
							}
							ptr5 = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(num, ptr5);
							ptr3++;
							continue;
							IL_282:
							ptr3++;
							continue;
							IL_210:
							*ptr5 = (byte)num;
							ptr5++;
							goto IL_282;
						}
						this.bufPos = (int)((long)(ptr5 - ptr2));
						this.FlushBuffer();
						ptr5 = ptr2 + 1;
					}
					this.bufPos = (int)((long)(ptr5 - ptr2));
				}
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000F730 File Offset: 0x0000E730
		protected unsafe void WriteCDataSection(string text)
		{
			fixed (char* ptr = text)
			{
				fixed (byte* ptr2 = this.bufBytes)
				{
					char* ptr3 = ptr;
					char* ptr4 = ptr + text.Length;
					byte* ptr5 = ptr2 + this.bufPos;
					int num = 0;
					for (;;)
					{
						byte* ptr6 = ptr5 + (long)(ptr4 - ptr3);
						if (ptr6 != ptr2 + this.bufLen)
						{
							ptr6 = ptr2 + this.bufLen;
						}
						while (ptr5 < ptr6 && (this.xmlCharType.charProperties[num = (int)(*ptr3)] & 128) != 0 && num != 93 && num <= 127)
						{
							*ptr5 = (byte)num;
							ptr5++;
							ptr3++;
						}
						if (ptr3 >= ptr4)
						{
							break;
						}
						if (ptr5 < ptr6)
						{
							int num2 = num;
							if (num2 <= 34)
							{
								switch (num2)
								{
								case 9:
									goto IL_203;
								case 10:
									if (this.newLineHandling == NewLineHandling.Replace)
									{
										ptr5 = this.WriteNewLine(ptr5);
										goto IL_275;
									}
									*ptr5 = (byte)num;
									ptr5++;
									goto IL_275;
								case 11:
								case 12:
									break;
								case 13:
									if (this.newLineHandling == NewLineHandling.Replace)
									{
										if (ptr3[1] == '\n')
										{
											ptr3++;
										}
										ptr5 = this.WriteNewLine(ptr5);
										goto IL_275;
									}
									*ptr5 = (byte)num;
									ptr5++;
									goto IL_275;
								default:
									if (num2 == 34)
									{
										goto IL_203;
									}
									break;
								}
							}
							else
							{
								switch (num2)
								{
								case 38:
								case 39:
									goto IL_203;
								default:
									switch (num2)
									{
									case 60:
										goto IL_203;
									case 61:
										break;
									case 62:
										if (this.hadDoubleBracket && ptr5[-1] == 93)
										{
											ptr5 = XmlUtf8RawTextWriter.RawEndCData(ptr5);
											ptr5 = XmlUtf8RawTextWriter.RawStartCData(ptr5);
										}
										*ptr5 = 62;
										ptr5++;
										goto IL_275;
									default:
										if (num2 == 93)
										{
											if (ptr5[-1] == 93)
											{
												this.hadDoubleBracket = true;
											}
											else
											{
												this.hadDoubleBracket = false;
											}
											*ptr5 = 93;
											ptr5++;
											goto IL_275;
										}
										break;
									}
									break;
								}
							}
							if (XmlUtf8RawTextWriter.InRange(num, 55296, 57343))
							{
								ptr5 = XmlUtf8RawTextWriter.EncodeSurrogate(ptr3, ptr4, ptr5);
								ptr3 += 2;
								continue;
							}
							if (num <= 127 || num >= 65534)
							{
								ptr5 = this.InvalidXmlChar(num, ptr5, false);
								ptr3++;
								continue;
							}
							ptr5 = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(num, ptr5);
							ptr3++;
							continue;
							IL_275:
							ptr3++;
							continue;
							IL_203:
							*ptr5 = (byte)num;
							ptr5++;
							goto IL_275;
						}
						this.bufPos = (int)((long)(ptr5 - ptr2));
						this.FlushBuffer();
						ptr5 = ptr2 + 1;
					}
					this.bufPos = (int)((long)(ptr5 - ptr2));
				}
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000F9D1 File Offset: 0x0000E9D1
		private static bool IsSurrogateByte(byte b)
		{
			return (b & 248) == 240;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000F9E4 File Offset: 0x0000E9E4
		private unsafe static byte* EncodeSurrogate(char* pSrc, char* pSrcEnd, byte* pDst)
		{
			int num = (int)(*pSrc);
			if (num > 56319)
			{
				throw XmlConvert.CreateInvalidHighSurrogateCharException((char)num);
			}
			if (pSrc + 1 >= pSrcEnd)
			{
				throw new ArgumentException(Res.GetString("Xml_InvalidSurrogateMissingLowChar"));
			}
			int num2 = (int)pSrc[1];
			if (num2 >= 56320)
			{
				num = num2 + (num << 10) + -56613888;
				*pDst = (byte)(240 | num >> 18);
				pDst[1] = (byte)(128 | (num >> 12 & 63));
				pDst[2] = (byte)(128 | (num >> 6 & 63));
				pDst[3] = (byte)(128 | (num & 63));
				pDst += 4;
				return pDst;
			}
			throw XmlConvert.CreateInvalidSurrogatePairException((char)num2, (char)num);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000FA8C File Offset: 0x0000EA8C
		private unsafe byte* InvalidXmlChar(int ch, byte* pDst, bool entitize)
		{
			if (this.checkCharacters)
			{
				throw XmlConvert.CreateInvalidCharException((char)ch);
			}
			if (entitize)
			{
				return XmlUtf8RawTextWriter.CharEntity(pDst, (char)ch);
			}
			if (ch < 128)
			{
				*pDst = (byte)ch;
				pDst++;
			}
			else
			{
				pDst = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(ch, pDst);
			}
			return pDst;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000FAC8 File Offset: 0x0000EAC8
		internal unsafe void EncodeChar(ref char* pSrc, char* pSrcEnd, ref byte* pDst)
		{
			int num = (int)(*pSrc);
			if (XmlUtf8RawTextWriter.InRange(num, 55296, 57343))
			{
				pDst = XmlUtf8RawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, pDst);
				pSrc += (IntPtr)4;
				return;
			}
			if (num <= 127 || num >= 65534)
			{
				pDst = this.InvalidXmlChar(num, pDst, false);
				pSrc += (IntPtr)2;
				return;
			}
			pDst = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(num, pDst);
			pSrc += (IntPtr)2;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000FB34 File Offset: 0x0000EB34
		internal unsafe static byte* EncodeMultibyteUTF8(int ch, byte* pDst)
		{
			if (ch < 2048)
			{
				*pDst = (byte)(-64 | ch >> 6);
			}
			else
			{
				*pDst = (byte)(-32 | ch >> 12);
				pDst++;
				*pDst = (byte)(-128 | (ch >> 6 & 63));
			}
			pDst++;
			*pDst = (byte)(128 | (ch & 63));
			return pDst + 1;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000FB88 File Offset: 0x0000EB88
		internal unsafe static void CharToUTF8(ref char* pSrc, char* pSrcEnd, ref byte* pDst)
		{
			int num = (int)(*pSrc);
			if (num <= 127)
			{
				*pDst = (byte)num;
				pDst += (IntPtr)1;
				pSrc += (IntPtr)2;
				return;
			}
			if (num >= 55296 && num <= 57343)
			{
				pDst = XmlUtf8RawTextWriter.EncodeSurrogate(pSrc, pSrcEnd, pDst);
				pSrc += (IntPtr)4;
				return;
			}
			pDst = XmlUtf8RawTextWriter.EncodeMultibyteUTF8(num, pDst);
			pSrc += (IntPtr)2;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000FBE8 File Offset: 0x0000EBE8
		protected unsafe byte* WriteNewLine(byte* pDst)
		{
			fixed (byte* ptr = this.bufBytes)
			{
				this.bufPos = (int)((long)(pDst - ptr));
				this.RawText(this.newLineChars);
				return ptr + this.bufPos;
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000FC38 File Offset: 0x0000EC38
		protected unsafe static byte* LtEntity(byte* pDst)
		{
			*pDst = 38;
			pDst[1] = 108;
			pDst[2] = 116;
			pDst[3] = 59;
			return pDst + 4;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000FC57 File Offset: 0x0000EC57
		protected unsafe static byte* GtEntity(byte* pDst)
		{
			*pDst = 38;
			pDst[1] = 103;
			pDst[2] = 116;
			pDst[3] = 59;
			return pDst + 4;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000FC76 File Offset: 0x0000EC76
		protected unsafe static byte* AmpEntity(byte* pDst)
		{
			*pDst = 38;
			pDst[1] = 97;
			pDst[2] = 109;
			pDst[3] = 112;
			pDst[4] = 59;
			return pDst + 5;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000FC9C File Offset: 0x0000EC9C
		protected unsafe static byte* QuoteEntity(byte* pDst)
		{
			*pDst = 38;
			pDst[1] = 113;
			pDst[2] = 117;
			pDst[3] = 111;
			pDst[4] = 116;
			pDst[5] = 59;
			return pDst + 6;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000FCC9 File Offset: 0x0000ECC9
		protected unsafe static byte* TabEntity(byte* pDst)
		{
			*pDst = 38;
			pDst[1] = 35;
			pDst[2] = 120;
			pDst[3] = 57;
			pDst[4] = 59;
			return pDst + 5;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000FCEF File Offset: 0x0000ECEF
		protected unsafe static byte* LineFeedEntity(byte* pDst)
		{
			*pDst = 38;
			pDst[1] = 35;
			pDst[2] = 120;
			pDst[3] = 65;
			pDst[4] = 59;
			return pDst + 5;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000FD15 File Offset: 0x0000ED15
		protected unsafe static byte* CarriageReturnEntity(byte* pDst)
		{
			*pDst = 38;
			pDst[1] = 35;
			pDst[2] = 120;
			pDst[3] = 68;
			pDst[4] = 59;
			return pDst + 5;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000FD3C File Offset: 0x0000ED3C
		private unsafe static byte* CharEntity(byte* pDst, char ch)
		{
			int num = (int)ch;
			string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
			*pDst = 38;
			pDst[1] = 35;
			pDst[2] = 120;
			pDst += 3;
			fixed (char* ptr = text)
			{
				char* ptr2 = ptr;
				while ((*(pDst++) = (byte)(*(ptr2++))) != 0)
				{
				}
			}
			pDst[-1] = 59;
			return pDst;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000FDAC File Offset: 0x0000EDAC
		protected unsafe static byte* RawStartCData(byte* pDst)
		{
			*pDst = 60;
			pDst[1] = 33;
			pDst[2] = 91;
			pDst[3] = 67;
			pDst[4] = 68;
			pDst[5] = 65;
			pDst[6] = 84;
			pDst[7] = 65;
			pDst[8] = 91;
			return pDst + 9;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000FDFA File Offset: 0x0000EDFA
		protected unsafe static byte* RawEndCData(byte* pDst)
		{
			*pDst = 93;
			pDst[1] = 93;
			pDst[2] = 62;
			return pDst + 3;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000FE12 File Offset: 0x0000EE12
		private static bool InRange(int ch, int start, int end)
		{
			return ch - start <= end - start;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000FE20 File Offset: 0x0000EE20
		protected void ValidateContentChars(string chars, string propertyName, bool allowOnlyWhitespace)
		{
			if (!allowOnlyWhitespace)
			{
				for (int i = 0; i < chars.Length; i++)
				{
					if (!this.xmlCharType.IsTextChar(chars[i]))
					{
						char c = chars[i];
						if (c <= '&')
						{
							switch (c)
							{
							case '\t':
							case '\n':
							case '\r':
								goto IL_152;
							case '\v':
							case '\f':
								goto IL_A7;
							default:
								if (c != '&')
								{
									goto IL_A7;
								}
								break;
							}
						}
						else if (c != '<' && c != ']')
						{
							goto IL_A7;
						}
						string @string = Res.GetString("Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(chars[i]));
						goto IL_163;
						IL_A7:
						if (chars[i] >= '\ud800' && chars[i] <= '\udbff')
						{
							if (i + 1 < chars.Length && chars[i + 1] >= '\udc00' && chars[i + 1] <= '\udfff')
							{
								i++;
								goto IL_152;
							}
							@string = Res.GetString("Xml_InvalidSurrogateMissingLowChar");
						}
						else
						{
							if (chars[i] < '\udc00' || chars[i] > '\udfff')
							{
								goto IL_152;
							}
							@string = Res.GetString("Xml_InvalidSurrogateHighChar", new object[]
							{
								((uint)chars[i]).ToString("X", CultureInfo.InvariantCulture)
							});
						}
						IL_163:
						throw new ArgumentException(Res.GetString("Xml_InvalidCharsInIndent", new string[]
						{
							propertyName,
							@string
						}));
					}
					IL_152:;
				}
				return;
			}
			if (!this.xmlCharType.IsOnlyWhitespace(chars))
			{
				throw new ArgumentException(Res.GetString("Xml_IndentCharsNotWhitespace", new object[]
				{
					propertyName
				}));
			}
		}

		// Token: 0x0400055E RID: 1374
		private const int BUFSIZE = 6144;

		// Token: 0x0400055F RID: 1375
		private const int OVERFLOW = 32;

		// Token: 0x04000560 RID: 1376
		private const int INIT_MARKS_COUNT = 64;

		// Token: 0x04000561 RID: 1377
		protected byte[] bufBytes;

		// Token: 0x04000562 RID: 1378
		protected Stream stream;

		// Token: 0x04000563 RID: 1379
		protected Encoding encoding;

		// Token: 0x04000564 RID: 1380
		protected XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x04000565 RID: 1381
		protected int bufPos = 1;

		// Token: 0x04000566 RID: 1382
		protected int textPos = 1;

		// Token: 0x04000567 RID: 1383
		protected int contentPos;

		// Token: 0x04000568 RID: 1384
		protected int cdataPos;

		// Token: 0x04000569 RID: 1385
		protected int attrEndPos;

		// Token: 0x0400056A RID: 1386
		protected int bufLen = 6144;

		// Token: 0x0400056B RID: 1387
		protected bool writeToNull;

		// Token: 0x0400056C RID: 1388
		protected bool hadDoubleBracket;

		// Token: 0x0400056D RID: 1389
		protected bool inAttributeValue;

		// Token: 0x0400056E RID: 1390
		protected NewLineHandling newLineHandling;

		// Token: 0x0400056F RID: 1391
		protected bool closeOutput;

		// Token: 0x04000570 RID: 1392
		protected bool omitXmlDeclaration;

		// Token: 0x04000571 RID: 1393
		protected bool autoXmlDeclaration;

		// Token: 0x04000572 RID: 1394
		protected string newLineChars;

		// Token: 0x04000573 RID: 1395
		protected XmlStandalone standalone;

		// Token: 0x04000574 RID: 1396
		protected XmlOutputMethod outputMethod;

		// Token: 0x04000575 RID: 1397
		protected bool checkCharacters;

		// Token: 0x04000576 RID: 1398
		protected bool mergeCDataSections;
	}
}
