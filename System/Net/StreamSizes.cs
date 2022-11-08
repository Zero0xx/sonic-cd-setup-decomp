using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000540 RID: 1344
	[StructLayout(LayoutKind.Sequential)]
	internal class StreamSizes
	{
		// Token: 0x0600290B RID: 10507 RVA: 0x000AB504 File Offset: 0x000AA504
		internal unsafe StreamSizes(byte[] memory)
		{
			checked
			{
				fixed (void* ptr = memory)
				{
					IntPtr ptr2 = new IntPtr(ptr);
					try
					{
						this.header = (int)((uint)Marshal.ReadInt32(ptr2));
						this.trailer = (int)((uint)Marshal.ReadInt32(ptr2, 4));
						this.maximumMessage = (int)((uint)Marshal.ReadInt32(ptr2, 8));
						this.buffersCount = (int)((uint)Marshal.ReadInt32(ptr2, 12));
						this.blockSize = (int)((uint)Marshal.ReadInt32(ptr2, 16));
					}
					catch (OverflowException)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x040027D2 RID: 10194
		public int header;

		// Token: 0x040027D3 RID: 10195
		public int trailer;

		// Token: 0x040027D4 RID: 10196
		public int maximumMessage;

		// Token: 0x040027D5 RID: 10197
		public int buffersCount;

		// Token: 0x040027D6 RID: 10198
		public int blockSize;

		// Token: 0x040027D7 RID: 10199
		public static readonly int SizeOf = Marshal.SizeOf(typeof(StreamSizes));
	}
}
