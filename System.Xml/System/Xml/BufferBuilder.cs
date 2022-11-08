using System;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000014 RID: 20
	internal class BufferBuilder
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002E14 File Offset: 0x00001E14
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002E1C File Offset: 0x00001E1C
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				if (value < 0 || value > this.length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value == 0)
				{
					this.Clear();
					return;
				}
				this.SetLength(value);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002E48 File Offset: 0x00001E48
		public void Append(char value)
		{
			if (this.length + 1 <= 65536)
			{
				if (this.stringBuilder == null)
				{
					this.stringBuilder = new StringBuilder();
				}
				this.stringBuilder.Append(value);
			}
			else
			{
				if (this.lastBuffer == null)
				{
					this.CreateBuffers();
				}
				if (this.lastBufferIndex == this.lastBuffer.Length)
				{
					this.AddBuffer();
				}
				this.lastBuffer[this.lastBufferIndex++] = value;
			}
			this.length++;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002ED1 File Offset: 0x00001ED1
		public void Append(char[] value)
		{
			this.Append(value, 0, value.Length);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002EE0 File Offset: 0x00001EE0
		public unsafe void Append(char[] value, int start, int count)
		{
			if (value == null)
			{
				if (start == 0 && count == 0)
				{
					return;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				if (count == 0)
				{
					return;
				}
				if (start < 0)
				{
					throw new ArgumentOutOfRangeException("start");
				}
				if (count < 0 || start + count > value.Length)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (this.length + count <= 65536)
				{
					if (this.stringBuilder == null)
					{
						this.stringBuilder = new StringBuilder((count < 16) ? 16 : count);
					}
					this.stringBuilder.Append(value, start, count);
					this.length += count;
					return;
				}
				fixed (char* ptr = &value[start])
				{
					this.AppendHelper(ptr, count);
				}
				return;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002F8A File Offset: 0x00001F8A
		public void Append(string value)
		{
			this.Append(value, 0, value.Length);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002F9C File Offset: 0x00001F9C
		public unsafe void Append(string value, int start, int count)
		{
			if (value == null)
			{
				if (start == 0 && count == 0)
				{
					return;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				if (count == 0)
				{
					return;
				}
				if (start < 0)
				{
					throw new ArgumentOutOfRangeException("start");
				}
				if (count < 0 || start + count > value.Length)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (this.length + count <= 65536)
				{
					if (this.stringBuilder == null)
					{
						this.stringBuilder = new StringBuilder(value, start, count, 0);
					}
					else
					{
						this.stringBuilder.Append(value, start, count);
					}
					this.length += count;
					return;
				}
				fixed (char* ptr = value)
				{
					this.AppendHelper(ptr + start, count);
				}
				return;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003050 File Offset: 0x00002050
		public void Clear()
		{
			if (this.length <= 65536)
			{
				if (this.stringBuilder != null)
				{
					this.stringBuilder.Length = 0;
				}
			}
			else
			{
				if (this.lastBuffer != null)
				{
					this.ClearBuffers();
				}
				this.stringBuilder = null;
			}
			this.length = 0;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000309C File Offset: 0x0000209C
		internal void ClearBuffers()
		{
			if (this.buffers != null)
			{
				for (int i = 0; i < this.buffersCount; i++)
				{
					this.Recycle(this.buffers[i]);
				}
				this.lastBuffer = null;
			}
			this.lastBufferIndex = 0;
			this.buffersCount = 0;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000030F0 File Offset: 0x000020F0
		public override string ToString()
		{
			string result;
			if (this.length <= 65536 || (this.buffersCount == 1 && this.lastBufferIndex == 0))
			{
				result = ((this.stringBuilder != null) ? this.stringBuilder.ToString() : string.Empty);
			}
			else
			{
				if (this.stringBuilder == null)
				{
					this.stringBuilder = new StringBuilder(this.length);
				}
				else
				{
					this.stringBuilder.Capacity = this.length;
				}
				int num = this.length - this.stringBuilder.Length;
				for (int i = 0; i < this.buffersCount - 1; i++)
				{
					char[] buffer = this.buffers[i].buffer;
					this.stringBuilder.Append(buffer, 0, buffer.Length);
					num -= buffer.Length;
				}
				this.stringBuilder.Append(this.buffers[this.buffersCount - 1].buffer, 0, num);
				this.ClearBuffers();
				result = this.stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000031F0 File Offset: 0x000021F0
		public string ToString(int startIndex, int len)
		{
			if (startIndex < 0 || startIndex >= this.length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (len < 0 || startIndex + len > this.length)
			{
				throw new ArgumentOutOfRangeException("len");
			}
			if (this.length > 65536 && (this.buffersCount != 1 || this.lastBufferIndex != 0))
			{
				StringBuilder stringBuilder = new StringBuilder(len);
				if (this.stringBuilder != null)
				{
					if (startIndex < this.stringBuilder.Length)
					{
						if (len < this.stringBuilder.Length)
						{
							return this.stringBuilder.ToString(startIndex, len);
						}
						stringBuilder.Append(this.stringBuilder.ToString(startIndex, this.stringBuilder.Length));
						startIndex = 0;
					}
					else
					{
						startIndex -= this.stringBuilder.Length;
					}
				}
				int num = 0;
				while (num < this.buffersCount && startIndex >= this.buffers[num].buffer.Length)
				{
					startIndex -= this.buffers[num].buffer.Length;
					num++;
				}
				if (num < this.buffersCount)
				{
					int num2 = len;
					while (num < this.buffersCount && num2 > 0)
					{
						char[] buffer = this.buffers[num].buffer;
						int num3 = (buffer.Length < num2) ? buffer.Length : num2;
						stringBuilder.Append(buffer, startIndex, num3);
						startIndex = 0;
						num2 -= num3;
						num++;
					}
				}
				return stringBuilder.ToString();
			}
			if (this.stringBuilder == null)
			{
				return string.Empty;
			}
			return this.stringBuilder.ToString(startIndex, len);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000336C File Offset: 0x0000236C
		private void CreateBuffers()
		{
			if (this.buffers == null)
			{
				this.lastBuffer = new char[65536];
				this.buffers = new BufferBuilder.Buffer[4];
				this.buffers[0].buffer = this.lastBuffer;
				this.buffersCount = 1;
				return;
			}
			this.AddBuffer();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000033C4 File Offset: 0x000023C4
		private unsafe void AppendHelper(char* pSource, int count)
		{
			if (this.lastBuffer == null)
			{
				this.CreateBuffers();
			}
			while (count > 0)
			{
				if (this.lastBufferIndex >= this.lastBuffer.Length)
				{
					this.AddBuffer();
				}
				int num = count;
				int num2 = this.lastBuffer.Length - this.lastBufferIndex;
				if (num2 < num)
				{
					num = num2;
				}
				fixed (char* ptr = &this.lastBuffer[this.lastBufferIndex])
				{
					BufferBuilder.wstrcpy(ptr, pSource, num);
				}
				pSource += num;
				this.length += num;
				this.lastBufferIndex += num;
				count -= num;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000345C File Offset: 0x0000245C
		private void AddBuffer()
		{
			if (this.buffersCount + 1 == this.buffers.Length)
			{
				BufferBuilder.Buffer[] destinationArray = new BufferBuilder.Buffer[this.buffers.Length * 2];
				Array.Copy(this.buffers, 0, destinationArray, 0, this.buffers.Length);
				this.buffers = destinationArray;
			}
			char[] array;
			if (this.buffers[this.buffersCount].recycledBuffer != null)
			{
				array = (char[])this.buffers[this.buffersCount].recycledBuffer.Target;
				if (array != null)
				{
					this.buffers[this.buffersCount].recycledBuffer.Target = null;
					goto IL_A4;
				}
			}
			array = new char[65536];
			IL_A4:
			this.lastBuffer = array;
			this.buffers[this.buffersCount++].buffer = array;
			this.lastBufferIndex = 0;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000353D File Offset: 0x0000253D
		private void Recycle(BufferBuilder.Buffer buf)
		{
			if (buf.recycledBuffer == null)
			{
				buf.recycledBuffer = new WeakReference(buf.buffer);
			}
			else
			{
				buf.recycledBuffer.Target = buf.buffer;
			}
			buf.buffer = null;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003578 File Offset: 0x00002578
		private void SetLength(int newLength)
		{
			if (newLength == this.length)
			{
				return;
			}
			if (this.length <= 65536)
			{
				this.stringBuilder.Length = newLength;
			}
			else
			{
				int num = newLength;
				int i = 0;
				while (i < this.buffersCount && num >= this.buffers[i].buffer.Length)
				{
					num -= this.buffers[i].buffer.Length;
					i++;
				}
				if (i < this.buffersCount)
				{
					this.lastBuffer = this.buffers[i].buffer;
					this.lastBufferIndex = num;
					i++;
					int num2 = i;
					while (i < this.buffersCount)
					{
						this.Recycle(this.buffers[i]);
						i++;
					}
					this.buffersCount = num2;
				}
			}
			this.length = newLength;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003650 File Offset: 0x00002650
		internal unsafe static void wstrcpy(char* dmem, char* smem, int charCount)
		{
			if (charCount > 0)
			{
				if (((dmem ^ smem) & 3) == 0)
				{
					while ((dmem & 3) != 0 && charCount > 0)
					{
						*dmem = *smem;
						dmem++;
						smem++;
						charCount--;
					}
					if (charCount >= 8)
					{
						charCount -= 8;
						do
						{
							*(int*)dmem = (int)(*(uint*)smem);
							*(int*)(dmem + 2) = (int)(*(uint*)(smem + 2));
							*(int*)(dmem + 4) = (int)(*(uint*)(smem + 4));
							*(int*)(dmem + 6) = (int)(*(uint*)(smem + 6));
							dmem += 8;
							smem += 8;
							charCount -= 8;
						}
						while (charCount >= 0);
					}
					if ((charCount & 4) != 0)
					{
						*(int*)dmem = (int)(*(uint*)smem);
						*(int*)(dmem + 2) = (int)(*(uint*)(smem + 2));
						dmem += 4;
						smem += 4;
					}
					if ((charCount & 2) != 0)
					{
						*(int*)dmem = (int)(*(uint*)smem);
						dmem += 2;
						smem += 2;
					}
				}
				else
				{
					if (charCount >= 8)
					{
						charCount -= 8;
						do
						{
							*dmem = *smem;
							dmem[1] = smem[1];
							dmem[2] = smem[2];
							dmem[3] = smem[3];
							dmem[4] = smem[4];
							dmem[5] = smem[5];
							dmem[6] = smem[6];
							dmem[7] = smem[7];
							dmem += 8;
							smem += 8;
							charCount -= 8;
						}
						while (charCount >= 0);
					}
					if ((charCount & 4) != 0)
					{
						*dmem = *smem;
						dmem[1] = smem[1];
						dmem[2] = smem[2];
						dmem[3] = smem[3];
						dmem += 4;
						smem += 4;
					}
					if ((charCount & 2) != 0)
					{
						*dmem = *smem;
						dmem[1] = smem[1];
						dmem += 2;
						smem += 2;
					}
				}
				if ((charCount & 1) != 0)
				{
					*dmem = *smem;
				}
			}
		}

		// Token: 0x04000462 RID: 1122
		private const int BufferSize = 65536;

		// Token: 0x04000463 RID: 1123
		private const int InitialBufferArrayLength = 4;

		// Token: 0x04000464 RID: 1124
		private const int MaxStringBuilderLength = 65536;

		// Token: 0x04000465 RID: 1125
		private const int DefaultSBCapacity = 16;

		// Token: 0x04000466 RID: 1126
		private StringBuilder stringBuilder;

		// Token: 0x04000467 RID: 1127
		private BufferBuilder.Buffer[] buffers;

		// Token: 0x04000468 RID: 1128
		private int buffersCount;

		// Token: 0x04000469 RID: 1129
		private char[] lastBuffer;

		// Token: 0x0400046A RID: 1130
		private int lastBufferIndex;

		// Token: 0x0400046B RID: 1131
		private int length;

		// Token: 0x02000015 RID: 21
		private struct Buffer
		{
			// Token: 0x0400046C RID: 1132
			internal char[] buffer;

			// Token: 0x0400046D RID: 1133
			internal WeakReference recycledBuffer;
		}
	}
}
