using System;

namespace System.Xml
{
	// Token: 0x0200000D RID: 13
	internal abstract class Base64Encoder
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000025DD File Offset: 0x000015DD
		internal Base64Encoder()
		{
			this.charsLine = new char[76];
		}

		// Token: 0x06000024 RID: 36
		internal abstract void WriteChars(char[] chars, int index, int count);

		// Token: 0x06000025 RID: 37 RVA: 0x000025F4 File Offset: 0x000015F4
		internal void Encode(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.leftOverBytesCount > 0)
			{
				int num = this.leftOverBytesCount;
				while (num < 3 && count > 0)
				{
					this.leftOverBytes[num++] = buffer[index++];
					count--;
				}
				if (count == 0 && num < 3)
				{
					this.leftOverBytesCount = num;
					return;
				}
				int count2 = Convert.ToBase64CharArray(this.leftOverBytes, 0, 3, this.charsLine, 0);
				this.WriteChars(this.charsLine, 0, count2);
			}
			this.leftOverBytesCount = count % 3;
			if (this.leftOverBytesCount > 0)
			{
				count -= this.leftOverBytesCount;
				if (this.leftOverBytes == null)
				{
					this.leftOverBytes = new byte[3];
				}
				for (int i = 0; i < this.leftOverBytesCount; i++)
				{
					this.leftOverBytes[i] = buffer[index + count + i];
				}
			}
			int num2 = index + count;
			int num3 = 57;
			while (index < num2)
			{
				if (index + num3 > num2)
				{
					num3 = num2 - index;
				}
				int count3 = Convert.ToBase64CharArray(buffer, index, num3, this.charsLine, 0);
				this.WriteChars(this.charsLine, 0, count3);
				index += num3;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002738 File Offset: 0x00001738
		internal void Flush()
		{
			if (this.leftOverBytesCount > 0)
			{
				int count = Convert.ToBase64CharArray(this.leftOverBytes, 0, this.leftOverBytesCount, this.charsLine, 0);
				this.WriteChars(this.charsLine, 0, count);
				this.leftOverBytesCount = 0;
			}
		}

		// Token: 0x0400044B RID: 1099
		internal const int Base64LineSize = 76;

		// Token: 0x0400044C RID: 1100
		internal const int LineSizeInBytes = 57;

		// Token: 0x0400044D RID: 1101
		private byte[] leftOverBytes;

		// Token: 0x0400044E RID: 1102
		private int leftOverBytesCount;

		// Token: 0x0400044F RID: 1103
		private char[] charsLine;
	}
}
