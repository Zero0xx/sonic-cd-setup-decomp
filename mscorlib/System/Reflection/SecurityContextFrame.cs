using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System.Reflection
{
	// Token: 0x02000308 RID: 776
	internal struct SecurityContextFrame
	{
		// Token: 0x06001E62 RID: 7778
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Push(Assembly assembly);

		// Token: 0x06001E63 RID: 7779
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Pop();

		// Token: 0x04000B3D RID: 2877
		private IntPtr m_GSCookie;

		// Token: 0x04000B3E RID: 2878
		private IntPtr __VFN_table;

		// Token: 0x04000B3F RID: 2879
		private IntPtr m_Next;

		// Token: 0x04000B40 RID: 2880
		private IntPtr m_Assembly;
	}
}
