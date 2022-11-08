using System;

namespace System.Xml
{
	// Token: 0x0200005A RID: 90
	internal class IncrementalReadCharsDecoder : IncrementalReadDecoder
	{
		// Token: 0x06000342 RID: 834 RVA: 0x00010E12 File Offset: 0x0000FE12
		internal IncrementalReadCharsDecoder()
		{
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00010E1A File Offset: 0x0000FE1A
		internal override int DecodedCount
		{
			get
			{
				return this.curIndex - this.startIndex;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00010E29 File Offset: 0x0000FE29
		internal override bool IsFull
		{
			get
			{
				return this.curIndex == this.endIndex;
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00010E3C File Offset: 0x0000FE3C
		internal override int Decode(char[] chars, int startPos, int len)
		{
			int num = this.endIndex - this.curIndex;
			if (num > len)
			{
				num = len;
			}
			Buffer.BlockCopy(chars, startPos * 2, this.buffer, this.curIndex * 2, num * 2);
			this.curIndex += num;
			return num;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00010E88 File Offset: 0x0000FE88
		internal override int Decode(string str, int startPos, int len)
		{
			int num = this.endIndex - this.curIndex;
			if (num > len)
			{
				num = len;
			}
			str.CopyTo(startPos, this.buffer, this.curIndex, num);
			this.curIndex += num;
			return num;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00010ECC File Offset: 0x0000FECC
		internal override void Reset()
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00010ECE File Offset: 0x0000FECE
		internal override void SetNextOutputBuffer(Array buffer, int index, int count)
		{
			this.buffer = (char[])buffer;
			this.startIndex = index;
			this.curIndex = index;
			this.endIndex = index + count;
		}

		// Token: 0x04000585 RID: 1413
		private char[] buffer;

		// Token: 0x04000586 RID: 1414
		private int startIndex;

		// Token: 0x04000587 RID: 1415
		private int curIndex;

		// Token: 0x04000588 RID: 1416
		private int endIndex;
	}
}
