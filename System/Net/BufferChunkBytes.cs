using System;

namespace System.Net
{
	// Token: 0x020004C5 RID: 1221
	internal struct BufferChunkBytes : IReadChunkBytes
	{
		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x0009609C File Offset: 0x0009509C
		// (set) Token: 0x060025AB RID: 9643 RVA: 0x000960D9 File Offset: 0x000950D9
		public int NextByte
		{
			get
			{
				if (this.Count != 0)
				{
					this.Count--;
					return (int)this.Buffer[this.Offset++];
				}
				return -1;
			}
			set
			{
				this.Count++;
				this.Offset--;
				this.Buffer[this.Offset] = (byte)value;
			}
		}

		// Token: 0x0400256D RID: 9581
		public byte[] Buffer;

		// Token: 0x0400256E RID: 9582
		public int Offset;

		// Token: 0x0400256F RID: 9583
		public int Count;
	}
}
