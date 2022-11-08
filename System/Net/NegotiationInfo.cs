using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000545 RID: 1349
	internal struct NegotiationInfo
	{
		// Token: 0x04002813 RID: 10259
		internal IntPtr PackageInfo;

		// Token: 0x04002814 RID: 10260
		internal uint NegotiationState;

		// Token: 0x04002815 RID: 10261
		internal static readonly int Size = Marshal.SizeOf(typeof(NegotiationInfo));

		// Token: 0x04002816 RID: 10262
		internal static readonly int NegotiationStateOffest = (int)Marshal.OffsetOf(typeof(NegotiationInfo), "NegotiationState");
	}
}
