using System;

namespace System.Xml
{
	// Token: 0x0200000C RID: 12
	internal class Base64Decoder : IncrementalReadDecoder
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002316 File Offset: 0x00001316
		internal override int DecodedCount
		{
			get
			{
				return this.curIndex - this.startIndex;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002325 File Offset: 0x00001325
		internal override bool IsFull
		{
			get
			{
				return this.curIndex == this.endIndex;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002338 File Offset: 0x00001338
		internal unsafe override int Decode(char[] chars, int startPos, int len)
		{
			if (len == 0)
			{
				return 0;
			}
			int result;
			int num;
			fixed (char* ptr = &chars[startPos])
			{
				fixed (byte* ptr2 = &this.buffer[this.curIndex])
				{
					this.Decode(ptr, ptr + len, ptr2, ptr2 + (this.endIndex - this.curIndex), out result, out num);
				}
			}
			this.curIndex += num;
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023A0 File Offset: 0x000013A0
		internal unsafe override int Decode(string str, int startPos, int len)
		{
			if (len == 0)
			{
				return 0;
			}
			int result;
			int num;
			fixed (char* ptr = str)
			{
				fixed (byte* ptr2 = &this.buffer[this.curIndex])
				{
					this.Decode(ptr + startPos, ptr + startPos + len, ptr2, ptr2 + (this.endIndex - this.curIndex), out result, out num);
				}
			}
			this.curIndex += num;
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002416 File Offset: 0x00001416
		internal override void Reset()
		{
			this.bitsFilled = 0;
			this.bits = 0;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002426 File Offset: 0x00001426
		internal override void SetNextOutputBuffer(Array buffer, int index, int count)
		{
			this.buffer = (byte[])buffer;
			this.startIndex = index;
			this.curIndex = index;
			this.endIndex = index + count;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000244C File Offset: 0x0000144C
		private static byte[] ConstructMapBase64()
		{
			byte[] array = new byte[123];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = byte.MaxValue;
			}
			for (int j = 0; j < Base64Decoder.CharsBase64.Length; j++)
			{
				array[(int)Base64Decoder.CharsBase64[j]] = (byte)j;
			}
			return array;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000249C File Offset: 0x0000149C
		private unsafe void Decode(char* pChars, char* pCharsEndPos, byte* pBytes, byte* pBytesEndPos, out int charsDecoded, out int bytesDecoded)
		{
			byte* ptr = pBytes;
			char* ptr2 = pChars;
			int num = this.bits;
			int num2 = this.bitsFilled;
			XmlCharType instance = XmlCharType.Instance;
			while (ptr2 < pCharsEndPos && ptr < pBytesEndPos)
			{
				char c = *ptr2;
				if (c == '=')
				{
					break;
				}
				ptr2++;
				if ((instance.charProperties[c] & 1) == 0)
				{
					int num3;
					if (c > 'z' || (num3 = (int)Base64Decoder.MapBase64[(int)c]) == 255)
					{
						throw new XmlException("Xml_InvalidBase64Value", new string(pChars, 0, (int)((long)(pCharsEndPos - pChars))));
					}
					num = (num << 6 | num3);
					num2 += 6;
					if (num2 >= 8)
					{
						*(ptr++) = (byte)(num >> num2 - 8 & 255);
						num2 -= 8;
						if (ptr == pBytesEndPos)
						{
							IL_F4:
							this.bits = num;
							this.bitsFilled = num2;
							bytesDecoded = (int)((long)(ptr - pBytes));
							charsDecoded = (int)((long)(ptr2 - pChars));
							return;
						}
					}
				}
			}
			if (ptr2 >= pCharsEndPos || *ptr2 != '=')
			{
				goto IL_F4;
			}
			num2 = 0;
			do
			{
				ptr2++;
			}
			while (ptr2 < pCharsEndPos && *ptr2 == '=');
			if (ptr2 < pCharsEndPos)
			{
				while ((instance.charProperties[*(ptr2++)] & 1) != 0)
				{
					if (ptr2 >= pCharsEndPos)
					{
						goto IL_F4;
					}
				}
				throw new XmlException("Xml_InvalidBase64Value", new string(pChars, 0, (int)((long)(pCharsEndPos - pChars))));
			}
			goto IL_F4;
		}

		// Token: 0x04000441 RID: 1089
		private const int MaxValidChar = 122;

		// Token: 0x04000442 RID: 1090
		private const byte Invalid = 255;

		// Token: 0x04000443 RID: 1091
		private byte[] buffer;

		// Token: 0x04000444 RID: 1092
		private int startIndex;

		// Token: 0x04000445 RID: 1093
		private int curIndex;

		// Token: 0x04000446 RID: 1094
		private int endIndex;

		// Token: 0x04000447 RID: 1095
		private int bits;

		// Token: 0x04000448 RID: 1096
		private int bitsFilled;

		// Token: 0x04000449 RID: 1097
		private static readonly string CharsBase64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		// Token: 0x0400044A RID: 1098
		private static readonly byte[] MapBase64 = Base64Decoder.ConstructMapBase64();
	}
}
