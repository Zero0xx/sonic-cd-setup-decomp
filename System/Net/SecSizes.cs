using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000541 RID: 1345
	[StructLayout(LayoutKind.Sequential)]
	internal class SecSizes
	{
		// Token: 0x0600290D RID: 10509 RVA: 0x000AB5B0 File Offset: 0x000AA5B0
		internal unsafe SecSizes(byte[] memory)
		{
			checked
			{
				fixed (void* ptr = memory)
				{
					IntPtr ptr2 = new IntPtr(ptr);
					try
					{
						this.MaxToken = (int)((uint)Marshal.ReadInt32(ptr2));
						this.MaxSignature = (int)((uint)Marshal.ReadInt32(ptr2, 4));
						this.BlockSize = (int)((uint)Marshal.ReadInt32(ptr2, 8));
						this.SecurityTrailer = (int)((uint)Marshal.ReadInt32(ptr2, 12));
					}
					catch (OverflowException)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x040027D8 RID: 10200
		public readonly int MaxToken;

		// Token: 0x040027D9 RID: 10201
		public readonly int MaxSignature;

		// Token: 0x040027DA RID: 10202
		public readonly int BlockSize;

		// Token: 0x040027DB RID: 10203
		public readonly int SecurityTrailer;

		// Token: 0x040027DC RID: 10204
		public static readonly int SizeOf = Marshal.SizeOf(typeof(SecSizes));
	}
}
