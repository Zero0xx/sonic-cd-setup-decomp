using System;

namespace System.Net
{
	// Token: 0x02000410 RID: 1040
	internal class StreamChunkBytes : IReadChunkBytes
	{
		// Token: 0x060020BF RID: 8383 RVA: 0x00080F84 File Offset: 0x0007FF84
		public StreamChunkBytes(ConnectStream connectStream)
		{
			this.ChunkStream = connectStream;
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060020C0 RID: 8384 RVA: 0x00080F93 File Offset: 0x0007FF93
		// (set) Token: 0x060020C1 RID: 8385 RVA: 0x00080FB6 File Offset: 0x0007FFB6
		public int NextByte
		{
			get
			{
				if (this.HavePush)
				{
					this.HavePush = false;
					return (int)this.PushByte;
				}
				return this.ChunkStream.ReadSingleByte();
			}
			set
			{
				this.PushByte = (byte)value;
				this.HavePush = true;
			}
		}

		// Token: 0x040020CB RID: 8395
		public ConnectStream ChunkStream;

		// Token: 0x040020CC RID: 8396
		public int BytesRead;

		// Token: 0x040020CD RID: 8397
		public int TotalBytesRead;

		// Token: 0x040020CE RID: 8398
		private byte PushByte;

		// Token: 0x040020CF RID: 8399
		private bool HavePush;
	}
}
