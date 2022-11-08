using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000408 RID: 1032
	[StructLayout(LayoutKind.Sequential)]
	internal class SecurityBufferDescriptor
	{
		// Token: 0x060020B2 RID: 8370 RVA: 0x00080DF2 File Offset: 0x0007FDF2
		public SecurityBufferDescriptor(int count)
		{
			this.Version = 0;
			this.Count = count;
			this.UnmanagedPointer = null;
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00080E10 File Offset: 0x0007FE10
		[Conditional("TRAVE")]
		internal void DebugDump()
		{
		}

		// Token: 0x04002090 RID: 8336
		public readonly int Version;

		// Token: 0x04002091 RID: 8337
		public readonly int Count;

		// Token: 0x04002092 RID: 8338
		public unsafe void* UnmanagedPointer;
	}
}
