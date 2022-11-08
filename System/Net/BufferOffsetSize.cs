using System;

namespace System.Net
{
	// Token: 0x020004B5 RID: 1205
	internal class BufferOffsetSize
	{
		// Token: 0x0600254A RID: 9546 RVA: 0x00094CA8 File Offset: 0x00093CA8
		internal BufferOffsetSize(byte[] buffer, int offset, int size, bool copyBuffer)
		{
			if (copyBuffer)
			{
				byte[] array = new byte[size];
				System.Buffer.BlockCopy(buffer, offset, array, 0, size);
				offset = 0;
				buffer = array;
			}
			this.Buffer = buffer;
			this.Offset = offset;
			this.Size = size;
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x00094CEB File Offset: 0x00093CEB
		internal BufferOffsetSize(byte[] buffer, bool copyBuffer) : this(buffer, 0, buffer.Length, copyBuffer)
		{
		}

		// Token: 0x04002512 RID: 9490
		internal byte[] Buffer;

		// Token: 0x04002513 RID: 9491
		internal int Offset;

		// Token: 0x04002514 RID: 9492
		internal int Size;
	}
}
