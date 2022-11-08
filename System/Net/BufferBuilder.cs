using System;

namespace System.Net
{
	// Token: 0x02000680 RID: 1664
	internal class BufferBuilder
	{
		// Token: 0x06003384 RID: 13188 RVA: 0x000D9AA8 File Offset: 0x000D8AA8
		internal BufferBuilder() : this(256)
		{
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x000D9AB5 File Offset: 0x000D8AB5
		internal BufferBuilder(int initialSize)
		{
			this.buffer = new byte[initialSize];
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x000D9ACC File Offset: 0x000D8ACC
		private void EnsureBuffer(int count)
		{
			if (count > this.buffer.Length - this.offset)
			{
				byte[] dst = new byte[(this.buffer.Length * 2 > this.buffer.Length + count) ? (this.buffer.Length * 2) : (this.buffer.Length + count)];
				Buffer.BlockCopy(this.buffer, 0, dst, 0, this.offset);
				this.buffer = dst;
			}
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x000D9B38 File Offset: 0x000D8B38
		internal void Append(byte value)
		{
			this.EnsureBuffer(1);
			this.buffer[this.offset++] = value;
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x000D9B65 File Offset: 0x000D8B65
		internal void Append(byte[] value)
		{
			this.Append(value, 0, value.Length);
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x000D9B72 File Offset: 0x000D8B72
		internal void Append(byte[] value, int offset, int count)
		{
			this.EnsureBuffer(count);
			Buffer.BlockCopy(value, offset, this.buffer, this.offset, count);
			this.offset += count;
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x000D9B9D File Offset: 0x000D8B9D
		internal void Append(string value)
		{
			this.Append(value, 0, value.Length);
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x000D9BB0 File Offset: 0x000D8BB0
		internal void Append(string value, int offset, int count)
		{
			this.EnsureBuffer(count);
			for (int i = 0; i < count; i++)
			{
				char c = value[offset + i];
				if (c > 'ÿ')
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				this.buffer[this.offset + i] = (byte)c;
			}
			this.offset += count;
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x0600338C RID: 13196 RVA: 0x000D9C11 File Offset: 0x000D8C11
		internal int Length
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x000D9C19 File Offset: 0x000D8C19
		internal byte[] GetBuffer()
		{
			return this.buffer;
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x000D9C21 File Offset: 0x000D8C21
		internal void Reset()
		{
			this.offset = 0;
		}

		// Token: 0x04002F99 RID: 12185
		private byte[] buffer;

		// Token: 0x04002F9A RID: 12186
		private int offset;
	}
}
