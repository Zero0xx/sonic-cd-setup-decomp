using System;

namespace System.Net
{
	// Token: 0x02000530 RID: 1328
	internal class ScatterGatherBuffers
	{
		// Token: 0x0600289D RID: 10397 RVA: 0x000A7E46 File Offset: 0x000A6E46
		internal ScatterGatherBuffers()
		{
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000A7E59 File Offset: 0x000A6E59
		internal ScatterGatherBuffers(long totalSize)
		{
			if (totalSize > 0L)
			{
				this.currentChunk = this.AllocateMemoryChunk((totalSize > 2147483647L) ? int.MaxValue : ((int)totalSize));
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000A7E90 File Offset: 0x000A6E90
		internal BufferOffsetSize[] GetBuffers()
		{
			if (this.Empty)
			{
				return null;
			}
			BufferOffsetSize[] array = new BufferOffsetSize[this.chunkCount];
			int num = 0;
			for (ScatterGatherBuffers.MemoryChunk next = this.headChunk; next != null; next = next.Next)
			{
				array[num] = new BufferOffsetSize(next.Buffer, 0, next.FreeOffset, false);
				num++;
			}
			return array;
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x000A7EE3 File Offset: 0x000A6EE3
		private bool Empty
		{
			get
			{
				return this.headChunk == null || this.chunkCount == 0;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x000A7EF8 File Offset: 0x000A6EF8
		internal int Length
		{
			get
			{
				return this.totalLength;
			}
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000A7F00 File Offset: 0x000A6F00
		internal void Write(byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				int num = this.Empty ? 0 : (this.currentChunk.Buffer.Length - this.currentChunk.FreeOffset);
				if (num == 0)
				{
					ScatterGatherBuffers.MemoryChunk next = this.AllocateMemoryChunk(count);
					if (this.currentChunk != null)
					{
						this.currentChunk.Next = next;
					}
					this.currentChunk = next;
				}
				int num2 = (count < num) ? count : num;
				Buffer.BlockCopy(buffer, offset, this.currentChunk.Buffer, this.currentChunk.FreeOffset, num2);
				offset += num2;
				count -= num2;
				this.totalLength += num2;
				this.currentChunk.FreeOffset += num2;
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000A7FB8 File Offset: 0x000A6FB8
		private ScatterGatherBuffers.MemoryChunk AllocateMemoryChunk(int newSize)
		{
			if (newSize > this.nextChunkLength)
			{
				this.nextChunkLength = newSize;
			}
			ScatterGatherBuffers.MemoryChunk result = new ScatterGatherBuffers.MemoryChunk(this.nextChunkLength);
			if (this.Empty)
			{
				this.headChunk = result;
			}
			this.nextChunkLength *= 2;
			this.chunkCount++;
			return result;
		}

		// Token: 0x04002789 RID: 10121
		private ScatterGatherBuffers.MemoryChunk headChunk;

		// Token: 0x0400278A RID: 10122
		private ScatterGatherBuffers.MemoryChunk currentChunk;

		// Token: 0x0400278B RID: 10123
		private int nextChunkLength = 1024;

		// Token: 0x0400278C RID: 10124
		private int totalLength;

		// Token: 0x0400278D RID: 10125
		private int chunkCount;

		// Token: 0x02000531 RID: 1329
		private class MemoryChunk
		{
			// Token: 0x060028A4 RID: 10404 RVA: 0x000A800D File Offset: 0x000A700D
			internal MemoryChunk(int bufferSize)
			{
				this.Buffer = new byte[bufferSize];
			}

			// Token: 0x0400278E RID: 10126
			internal byte[] Buffer;

			// Token: 0x0400278F RID: 10127
			internal int FreeOffset;

			// Token: 0x04002790 RID: 10128
			internal ScatterGatherBuffers.MemoryChunk Next;
		}
	}
}
