using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000601 RID: 1537
	internal static class JitHelpers
	{
		// Token: 0x060037FC RID: 14332
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UnsafeSetArrayElement(object[] target, int index, object element);
	}
}
