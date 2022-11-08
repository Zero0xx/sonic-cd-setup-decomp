using System;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200004C RID: 76
	internal class CharEntityEncoderFallback : EncoderFallback
	{
		// Token: 0x0600020E RID: 526 RVA: 0x00009218 File Offset: 0x00008218
		internal CharEntityEncoderFallback()
		{
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00009220 File Offset: 0x00008220
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			if (this.fallbackBuffer == null)
			{
				this.fallbackBuffer = new CharEntityEncoderFallbackBuffer(this);
			}
			return this.fallbackBuffer;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000923C File Offset: 0x0000823C
		public override int MaxCharCount
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00009240 File Offset: 0x00008240
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00009248 File Offset: 0x00008248
		internal int StartOffset
		{
			get
			{
				return this.startOffset;
			}
			set
			{
				this.startOffset = value;
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009251 File Offset: 0x00008251
		internal void Reset(int[] textContentMarks, int endMarkPos)
		{
			this.textContentMarks = textContentMarks;
			this.endMarkPos = endMarkPos;
			this.curMarkPos = 0;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009268 File Offset: 0x00008268
		internal bool CanReplaceAt(int index)
		{
			int num = this.curMarkPos;
			int num2 = this.startOffset + index;
			while (num < this.endMarkPos && num2 >= this.textContentMarks[num + 1])
			{
				num++;
			}
			this.curMarkPos = num;
			return (num & 1) != 0;
		}

		// Token: 0x04000514 RID: 1300
		private CharEntityEncoderFallbackBuffer fallbackBuffer;

		// Token: 0x04000515 RID: 1301
		private int[] textContentMarks;

		// Token: 0x04000516 RID: 1302
		private int endMarkPos;

		// Token: 0x04000517 RID: 1303
		private int curMarkPos;

		// Token: 0x04000518 RID: 1304
		private int startOffset;
	}
}
