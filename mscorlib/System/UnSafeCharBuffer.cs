using System;

namespace System
{
	// Token: 0x02000139 RID: 313
	internal struct UnSafeCharBuffer
	{
		// Token: 0x0600114D RID: 4429 RVA: 0x0002FE80 File Offset: 0x0002EE80
		public unsafe UnSafeCharBuffer(char* buffer, int bufferSize)
		{
			this.m_buffer = buffer;
			this.m_totalSize = bufferSize;
			this.m_length = 0;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0002FE98 File Offset: 0x0002EE98
		public unsafe void AppendString(string stringToAppend)
		{
			if (string.IsNullOrEmpty(stringToAppend))
			{
				return;
			}
			if (this.m_totalSize - this.m_length < stringToAppend.Length)
			{
				throw new IndexOutOfRangeException();
			}
			fixed (char* src = stringToAppend)
			{
				Buffer.memcpyimpl((byte*)src, (byte*)(this.m_buffer + this.m_length), stringToAppend.Length * 2);
			}
			this.m_length += stringToAppend.Length;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x0002FF0A File Offset: 0x0002EF0A
		public int Length
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x040005DB RID: 1499
		private unsafe char* m_buffer;

		// Token: 0x040005DC RID: 1500
		private int m_totalSize;

		// Token: 0x040005DD RID: 1501
		private int m_length;
	}
}
