using System;
using System.Globalization;
using System.IO;

namespace System.Xml
{
	// Token: 0x02000083 RID: 131
	internal class XmlTextEncoder
	{
		// Token: 0x0600060E RID: 1550 RVA: 0x00018863 File Offset: 0x00017863
		internal XmlTextEncoder(TextWriter textWriter)
		{
			this.textWriter = textWriter;
			this.quoteChar = '"';
			this.xmlCharType = XmlCharType.Instance;
		}

		// Token: 0x170000F6 RID: 246
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x00018885 File Offset: 0x00017885
		internal char QuoteChar
		{
			set
			{
				this.quoteChar = value;
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001888E File Offset: 0x0001788E
		internal void StartAttribute(bool cacheAttrValue)
		{
			this.inAttribute = true;
			this.cacheAttrValue = cacheAttrValue;
			if (cacheAttrValue)
			{
				if (this.attrValue == null)
				{
					this.attrValue = new BufferBuilder();
					return;
				}
				this.attrValue.Clear();
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000188C0 File Offset: 0x000178C0
		internal void EndAttribute()
		{
			if (this.cacheAttrValue)
			{
				this.attrValue.Clear();
			}
			this.inAttribute = false;
			this.cacheAttrValue = false;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x000188E3 File Offset: 0x000178E3
		internal string AttributeValue
		{
			get
			{
				if (this.cacheAttrValue)
				{
					return this.attrValue.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00018900 File Offset: 0x00017900
		internal void WriteSurrogateChar(char lowChar, char highChar)
		{
			if (lowChar < '\udc00' || lowChar > '\udfff' || highChar < '\ud800' || highChar > '\udbff')
			{
				throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
			}
			this.textWriter.Write(highChar);
			this.textWriter.Write(lowChar);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00018950 File Offset: 0x00017950
		internal unsafe void Write(char[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (0 > offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (0 > count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > array.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(array, offset, count);
			}
			int num = offset + count;
			int num2 = offset;
			char c = '\0';
			for (;;)
			{
				int num3 = num2;
				while (num2 < num && (this.xmlCharType.charProperties[c = array[num2]] & 128) != 0)
				{
					num2++;
				}
				if (num3 < num2)
				{
					this.textWriter.Write(array, num3, num2 - num3);
				}
				if (num2 == num)
				{
					break;
				}
				char c2 = c;
				if (c2 <= '"')
				{
					switch (c2)
					{
					case '\t':
						this.textWriter.Write(c);
						break;
					case '\n':
					case '\r':
						if (this.inAttribute)
						{
							this.WriteCharEntityImpl(c);
						}
						else
						{
							this.textWriter.Write(c);
						}
						break;
					case '\v':
					case '\f':
						goto IL_1C4;
					default:
						if (c2 != '"')
						{
							goto IL_1C4;
						}
						if (this.inAttribute && this.quoteChar == c)
						{
							this.WriteEntityRefImpl("quot");
						}
						else
						{
							this.textWriter.Write('"');
						}
						break;
					}
				}
				else
				{
					switch (c2)
					{
					case '&':
						this.WriteEntityRefImpl("amp");
						break;
					case '\'':
						if (this.inAttribute && this.quoteChar == c)
						{
							this.WriteEntityRefImpl("apos");
						}
						else
						{
							this.textWriter.Write('\'');
						}
						break;
					default:
						switch (c2)
						{
						case '<':
							this.WriteEntityRefImpl("lt");
							break;
						case '=':
							goto IL_1C4;
						case '>':
							this.WriteEntityRefImpl("gt");
							break;
						default:
							goto IL_1C4;
						}
						break;
					}
				}
				IL_218:
				num2++;
				continue;
				IL_1C4:
				if (c >= '\ud800' && c <= '\udbff')
				{
					if (num2 + 1 < num)
					{
						this.WriteSurrogateChar(array[++num2], c);
						goto IL_218;
					}
					goto IL_1EA;
				}
				else
				{
					if (c >= '\udc00' && c <= '\udfff')
					{
						goto Block_23;
					}
					this.WriteCharEntityImpl(c);
					goto IL_218;
				}
			}
			return;
			IL_1EA:
			throw new ArgumentException(Res.GetString("Xml_SurrogatePairSplit"));
			Block_23:
			throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00018B80 File Offset: 0x00017B80
		internal unsafe void Write(char ch)
		{
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(ch);
			}
			bool flag = (this.xmlCharType.charProperties[ch] & 128) != 0;
			if (flag)
			{
				this.textWriter.Write(ch);
				return;
			}
			if (ch <= '"')
			{
				switch (ch)
				{
				case '\t':
					this.textWriter.Write(ch);
					return;
				case '\n':
				case '\r':
					if (this.inAttribute)
					{
						this.WriteCharEntityImpl(ch);
						return;
					}
					this.textWriter.Write(ch);
					return;
				case '\v':
				case '\f':
					break;
				default:
					if (ch == '"')
					{
						if (this.inAttribute && this.quoteChar == ch)
						{
							this.WriteEntityRefImpl("quot");
							return;
						}
						this.textWriter.Write('"');
						return;
					}
					break;
				}
			}
			else
			{
				switch (ch)
				{
				case '&':
					this.WriteEntityRefImpl("amp");
					return;
				case '\'':
					if (this.inAttribute && this.quoteChar == ch)
					{
						this.WriteEntityRefImpl("apos");
						return;
					}
					this.textWriter.Write('\'');
					return;
				default:
					switch (ch)
					{
					case '<':
						this.WriteEntityRefImpl("lt");
						return;
					case '>':
						this.WriteEntityRefImpl("gt");
						return;
					}
					break;
				}
			}
			if (ch >= '\ud800' && ch <= '\udbff')
			{
				throw new ArgumentException(Res.GetString("Xml_InvalidSurrogateMissingLowChar"));
			}
			if (ch >= '\udc00' && ch <= '\udfff')
			{
				throw XmlConvert.CreateInvalidHighSurrogateCharException(ch);
			}
			this.WriteCharEntityImpl(ch);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00018D0C File Offset: 0x00017D0C
		internal void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			if (lowChar < '\udc00' || lowChar > '\udfff' || highChar < '\ud800' || highChar > '\udbff')
			{
				throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
			}
			int num = (int)(lowChar - '\udc00') | (int)((int)(highChar - '\ud800') << 10) + 65536;
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(highChar);
				this.attrValue.Append(lowChar);
			}
			this.textWriter.Write("&#x");
			this.textWriter.Write(num.ToString("X", NumberFormatInfo.InvariantInfo));
			this.textWriter.Write(';');
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00018DB4 File Offset: 0x00017DB4
		internal unsafe void Write(string text)
		{
			if (text == null)
			{
				return;
			}
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(text);
			}
			int length = text.Length;
			int i = 0;
			int num = 0;
			char c = '\0';
			for (;;)
			{
				if (i >= length || (this.xmlCharType.charProperties[c = text[i]] & 128) == 0)
				{
					if (i == length)
					{
						break;
					}
					if (this.inAttribute)
					{
						if (c != '\t')
						{
							goto IL_90;
						}
						i++;
					}
					else
					{
						if (c != '\t' && c != '\n' && c != '\r' && c != '"' && c != '\'')
						{
							goto IL_90;
						}
						i++;
					}
				}
				else
				{
					i++;
				}
			}
			this.textWriter.Write(text);
			return;
			IL_90:
			char[] helperBuffer = new char[256];
			for (;;)
			{
				if (num < i)
				{
					this.WriteStringFragment(text, num, i - num, helperBuffer);
				}
				if (i == length)
				{
					break;
				}
				char c2 = c;
				if (c2 <= '"')
				{
					switch (c2)
					{
					case '\t':
						this.textWriter.Write(c);
						break;
					case '\n':
					case '\r':
						if (this.inAttribute)
						{
							this.WriteCharEntityImpl(c);
						}
						else
						{
							this.textWriter.Write(c);
						}
						break;
					case '\v':
					case '\f':
						goto IL_1DA;
					default:
						if (c2 != '"')
						{
							goto IL_1DA;
						}
						if (this.inAttribute && this.quoteChar == c)
						{
							this.WriteEntityRefImpl("quot");
						}
						else
						{
							this.textWriter.Write('"');
						}
						break;
					}
				}
				else
				{
					switch (c2)
					{
					case '&':
						this.WriteEntityRefImpl("amp");
						break;
					case '\'':
						if (this.inAttribute && this.quoteChar == c)
						{
							this.WriteEntityRefImpl("apos");
						}
						else
						{
							this.textWriter.Write('\'');
						}
						break;
					default:
						switch (c2)
						{
						case '<':
							this.WriteEntityRefImpl("lt");
							break;
						case '=':
							goto IL_1DA;
						case '>':
							this.WriteEntityRefImpl("gt");
							break;
						default:
							goto IL_1DA;
						}
						break;
					}
				}
				IL_230:
				i++;
				num = i;
				while (i < length)
				{
					if ((this.xmlCharType.charProperties[c = text[i]] & 128) == 0)
					{
						break;
					}
					i++;
				}
				continue;
				IL_1DA:
				if (c >= '\ud800' && c <= '\udbff')
				{
					if (i + 1 < length)
					{
						this.WriteSurrogateChar(text[++i], c);
						goto IL_230;
					}
					goto IL_204;
				}
				else
				{
					if (c >= '\udc00' && c <= '\udfff')
					{
						goto Block_27;
					}
					this.WriteCharEntityImpl(c);
					goto IL_230;
				}
			}
			return;
			IL_204:
			throw XmlConvert.CreateInvalidSurrogatePairException(text[i], c);
			Block_27:
			throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00019028 File Offset: 0x00018028
		internal unsafe void WriteRawWithSurrogateChecking(string text)
		{
			if (text == null)
			{
				return;
			}
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(text);
			}
			int length = text.Length;
			int num = 0;
			char c = '\0';
			char c2;
			for (;;)
			{
				if (num >= length || ((this.xmlCharType.charProperties[c = text[num]] & 16) == 0 && c >= ' '))
				{
					if (num == length)
					{
						goto IL_BF;
					}
					if (c >= '\ud800' && c <= '\udbff')
					{
						if (num + 1 >= length)
						{
							goto IL_8F;
						}
						c2 = text[num + 1];
						if (c2 < '\udc00' || c2 > '\udfff')
						{
							break;
						}
						num += 2;
					}
					else
					{
						if (c >= '\udc00' && c <= '\udfff')
						{
							goto Block_12;
						}
						num++;
					}
				}
				else
				{
					num++;
				}
			}
			throw XmlConvert.CreateInvalidSurrogatePairException(c2, c);
			IL_8F:
			throw new ArgumentException(Res.GetString("Xml_InvalidSurrogateMissingLowChar"));
			Block_12:
			throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
			IL_BF:
			this.textWriter.Write(text);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00019100 File Offset: 0x00018100
		internal void WriteRaw(string value)
		{
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(value);
			}
			this.textWriter.Write(value);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00019124 File Offset: 0x00018124
		internal void WriteRaw(char[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (0 > count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (0 > offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count > array.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(array, offset, count);
			}
			this.textWriter.Write(array, offset, count);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00019194 File Offset: 0x00018194
		internal void WriteCharEntity(char ch)
		{
			if (ch >= '\ud800' && ch <= '\udfff')
			{
				throw new ArgumentException(Res.GetString("Xml_InvalidSurrogateMissingLowChar"));
			}
			int num = (int)ch;
			string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
			if (this.cacheAttrValue)
			{
				this.attrValue.Append("&#x");
				this.attrValue.Append(text);
				this.attrValue.Append(';');
			}
			this.WriteCharEntityImpl(text);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001920D File Offset: 0x0001820D
		internal void WriteEntityRef(string name)
		{
			if (this.cacheAttrValue)
			{
				this.attrValue.Append('&');
				this.attrValue.Append(name);
				this.attrValue.Append(';');
			}
			this.WriteEntityRefImpl(name);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00019244 File Offset: 0x00018244
		internal void Flush()
		{
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00019248 File Offset: 0x00018248
		private void WriteStringFragment(string str, int offset, int count, char[] helperBuffer)
		{
			int num = helperBuffer.Length;
			while (count > 0)
			{
				int num2 = count;
				if (num2 > num)
				{
					num2 = num;
				}
				str.CopyTo(offset, helperBuffer, 0, num2);
				this.textWriter.Write(helperBuffer, 0, num2);
				offset += num2;
				count -= num2;
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001928C File Offset: 0x0001828C
		private void WriteCharEntityImpl(char ch)
		{
			int num = (int)ch;
			this.WriteCharEntityImpl(num.ToString("X", NumberFormatInfo.InvariantInfo));
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000192B2 File Offset: 0x000182B2
		private void WriteCharEntityImpl(string strVal)
		{
			this.textWriter.Write("&#x");
			this.textWriter.Write(strVal);
			this.textWriter.Write(';');
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000192DD File Offset: 0x000182DD
		private void WriteEntityRefImpl(string name)
		{
			this.textWriter.Write('&');
			this.textWriter.Write(name);
			this.textWriter.Write(';');
		}

		// Token: 0x04000676 RID: 1654
		private const int SurHighStart = 55296;

		// Token: 0x04000677 RID: 1655
		private const int SurHighEnd = 56319;

		// Token: 0x04000678 RID: 1656
		private const int SurLowStart = 56320;

		// Token: 0x04000679 RID: 1657
		private const int SurLowEnd = 57343;

		// Token: 0x0400067A RID: 1658
		private TextWriter textWriter;

		// Token: 0x0400067B RID: 1659
		private bool inAttribute;

		// Token: 0x0400067C RID: 1660
		private char quoteChar;

		// Token: 0x0400067D RID: 1661
		private BufferBuilder attrValue;

		// Token: 0x0400067E RID: 1662
		private bool cacheAttrValue;

		// Token: 0x0400067F RID: 1663
		private XmlCharType xmlCharType;
	}
}
