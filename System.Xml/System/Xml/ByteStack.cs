using System;

namespace System.Xml
{
	// Token: 0x02000016 RID: 22
	internal class ByteStack
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000037DC File Offset: 0x000027DC
		public ByteStack(int growthRate)
		{
			this.growthRate = growthRate;
			this.top = 0;
			this.stack = new byte[growthRate];
			this.size = growthRate;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003808 File Offset: 0x00002808
		public void Push(byte data)
		{
			if (this.size == this.top)
			{
				byte[] dst = new byte[this.size + this.growthRate];
				if (this.top > 0)
				{
					Buffer.BlockCopy(this.stack, 0, dst, 0, this.top);
				}
				this.stack = dst;
				this.size += this.growthRate;
			}
			this.stack[this.top++] = data;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003888 File Offset: 0x00002888
		public byte Pop()
		{
			if (this.top > 0)
			{
				return this.stack[--this.top];
			}
			return 0;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000038B8 File Offset: 0x000028B8
		public byte Peek()
		{
			if (this.top > 0)
			{
				return this.stack[this.top - 1];
			}
			return 0;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000038D4 File Offset: 0x000028D4
		public int Length
		{
			get
			{
				return this.top;
			}
		}

		// Token: 0x0400046E RID: 1134
		private byte[] stack;

		// Token: 0x0400046F RID: 1135
		private int growthRate;

		// Token: 0x04000470 RID: 1136
		private int top;

		// Token: 0x04000471 RID: 1137
		private int size;
	}
}
