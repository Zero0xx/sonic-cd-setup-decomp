using System;

namespace System.Xml
{
	// Token: 0x02000010 RID: 16
	internal class BinHexDecoder : IncrementalReadDecoder
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000027BB File Offset: 0x000017BB
		internal override int DecodedCount
		{
			get
			{
				return this.curIndex - this.startIndex;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000027CA File Offset: 0x000017CA
		internal override bool IsFull
		{
			get
			{
				return this.curIndex == this.endIndex;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000027DC File Offset: 0x000017DC
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
					BinHexDecoder.Decode(ptr, ptr + len, ptr2, ptr2 + (this.endIndex - this.curIndex), ref this.hasHalfByteCached, ref this.cachedHalfByte, out result, out num);
				}
			}
			this.curIndex += num;
			return result;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002850 File Offset: 0x00001850
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
					BinHexDecoder.Decode(ptr + startPos, ptr + startPos + len, ptr2, ptr2 + (this.endIndex - this.curIndex), ref this.hasHalfByteCached, ref this.cachedHalfByte, out result, out num);
				}
			}
			this.curIndex += num;
			return result;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000028D1 File Offset: 0x000018D1
		internal override void Reset()
		{
			this.hasHalfByteCached = false;
			this.cachedHalfByte = 0;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000028E1 File Offset: 0x000018E1
		internal override void SetNextOutputBuffer(Array buffer, int index, int count)
		{
			this.buffer = (byte[])buffer;
			this.startIndex = index;
			this.curIndex = index;
			this.endIndex = index + count;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002908 File Offset: 0x00001908
		public unsafe static byte[] Decode(char[] chars, bool allowOddChars)
		{
			if (chars == null)
			{
				throw new ArgumentException("chars");
			}
			int num = chars.Length;
			if (num == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[(num + 1) / 2];
			bool flag = false;
			byte b = 0;
			int num3;
			fixed (char* ptr = &chars[0])
			{
				fixed (byte* ptr2 = &array[0])
				{
					int num2;
					BinHexDecoder.Decode(ptr, ptr + num, ptr2, ptr2 + array.Length, ref flag, ref b, out num2, out num3);
				}
			}
			if (flag && !allowOddChars)
			{
				throw new XmlException("Xml_InvalidBinHexValueOddCount", new string(chars));
			}
			if (num3 < array.Length)
			{
				byte[] array2 = new byte[num3];
				Buffer.BlockCopy(array, 0, array2, 0, num3);
				array = array2;
			}
			return array;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000029B4 File Offset: 0x000019B4
		private unsafe static void Decode(char* pChars, char* pCharsEndPos, byte* pBytes, byte* pBytesEndPos, ref bool hasHalfByteCached, ref byte cachedHalfByte, out int charsDecoded, out int bytesDecoded)
		{
			char* ptr = pChars;
			byte* ptr2 = pBytes;
			XmlCharType instance = XmlCharType.Instance;
			while (ptr < pCharsEndPos && ptr2 < pBytesEndPos)
			{
				char c = *(ptr++);
				byte b;
				if (c >= 'a' && c <= 'f')
				{
					b = (byte)(c - 'a' + '\n');
				}
				else if (c >= 'A' && c <= 'F')
				{
					b = (byte)(c - 'A' + '\n');
				}
				else if (c >= '0' && c <= '9')
				{
					b = (byte)(c - '0');
				}
				else
				{
					if ((instance.charProperties[c] & 1) == 0)
					{
						throw new XmlException("Xml_InvalidBinHexValue", new string(pChars, 0, (int)((long)(pCharsEndPos - pChars))));
					}
					continue;
				}
				if (hasHalfByteCached)
				{
					*(ptr2++) = (byte)(((int)cachedHalfByte << 4) + (int)b);
					hasHalfByteCached = false;
				}
				else
				{
					cachedHalfByte = b;
					hasHalfByteCached = true;
				}
			}
			bytesDecoded = (int)((long)(ptr2 - pBytes));
			charsDecoded = (int)((long)(ptr - pChars));
		}

		// Token: 0x04000452 RID: 1106
		private byte[] buffer;

		// Token: 0x04000453 RID: 1107
		private int startIndex;

		// Token: 0x04000454 RID: 1108
		private int curIndex;

		// Token: 0x04000455 RID: 1109
		private int endIndex;

		// Token: 0x04000456 RID: 1110
		private bool hasHalfByteCached;

		// Token: 0x04000457 RID: 1111
		private byte cachedHalfByte;
	}
}
