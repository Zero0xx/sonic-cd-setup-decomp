using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000D3 RID: 211
	internal static class Mda
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00023C77 File Offset: 0x00022C77
		internal static bool StreamWriterBufferMDAEnabled
		{
			get
			{
				if (Mda._streamWriterMDAState == Mda.MdaState.Unknown)
				{
					if (Mda.IsStreamWriterBufferedDataLostEnabled())
					{
						Mda._streamWriterMDAState = Mda.MdaState.Enabled;
					}
					else
					{
						Mda._streamWriterMDAState = Mda.MdaState.Disabled;
					}
				}
				return Mda._streamWriterMDAState == Mda.MdaState.Enabled;
			}
		}

		// Token: 0x06000BF6 RID: 3062
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void MemberInfoCacheCreation();

		// Token: 0x06000BF7 RID: 3063
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DateTimeInvalidLocalFormat();

		// Token: 0x06000BF8 RID: 3064
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StreamWriterBufferedDataLost(string text);

		// Token: 0x06000BF9 RID: 3065
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsStreamWriterBufferedDataLostEnabled();

		// Token: 0x06000BFA RID: 3066
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsInvalidGCHandleCookieProbeEnabled();

		// Token: 0x06000BFB RID: 3067
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void FireInvalidGCHandleCookieProbe(IntPtr cookie);

		// Token: 0x04000434 RID: 1076
		private static Mda.MdaState _streamWriterMDAState;

		// Token: 0x020000D4 RID: 212
		private enum MdaState
		{
			// Token: 0x04000436 RID: 1078
			Unknown,
			// Token: 0x04000437 RID: 1079
			Enabled,
			// Token: 0x04000438 RID: 1080
			Disabled
		}
	}
}
