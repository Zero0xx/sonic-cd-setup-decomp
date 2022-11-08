using System;

namespace System.Xml
{
	// Token: 0x02000013 RID: 19
	internal class BitStack
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002CCC File Offset: 0x00001CCC
		public BitStack()
		{
			this.curr = 1U;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002CDB File Offset: 0x00001CDB
		public void PushBit(bool bit)
		{
			if ((this.curr & 2147483648U) != 0U)
			{
				this.PushCurr();
			}
			this.curr = (this.curr << 1 | (bit ? 1U : 0U));
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002D08 File Offset: 0x00001D08
		public bool PopBit()
		{
			bool result = (this.curr & 1U) != 0U;
			this.curr >>= 1;
			if (this.curr == 1U)
			{
				this.PopCurr();
			}
			return result;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002D42 File Offset: 0x00001D42
		public bool PeekBit()
		{
			return (this.curr & 1U) != 0U;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002D52 File Offset: 0x00001D52
		public bool IsEmpty
		{
			get
			{
				return this.curr == 1U;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D60 File Offset: 0x00001D60
		private void PushCurr()
		{
			if (this.bitStack == null)
			{
				this.bitStack = new uint[16];
			}
			this.bitStack[this.stackPos++] = this.curr;
			this.curr = 1U;
			int num = this.bitStack.Length;
			if (this.stackPos >= num)
			{
				uint[] destinationArray = new uint[2 * num];
				Array.Copy(this.bitStack, destinationArray, num);
				this.bitStack = destinationArray;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002DD8 File Offset: 0x00001DD8
		private void PopCurr()
		{
			if (this.stackPos > 0)
			{
				this.curr = this.bitStack[--this.stackPos];
			}
		}

		// Token: 0x0400045F RID: 1119
		private uint[] bitStack;

		// Token: 0x04000460 RID: 1120
		private int stackPos;

		// Token: 0x04000461 RID: 1121
		private uint curr;
	}
}
