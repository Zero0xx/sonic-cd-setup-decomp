using System;
using System.Runtime.ConstrainedExecution;

namespace System.StubHelpers
{
	// Token: 0x0200011D RID: 285
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static class WSTRBufferMarshaler
	{
		// Token: 0x06001037 RID: 4151 RVA: 0x0002E256 File Offset: 0x0002D256
		internal static IntPtr ConvertToNative(string strManaged)
		{
			return IntPtr.Zero;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0002E25D File Offset: 0x0002D25D
		internal static string ConvertToManaged(IntPtr bstr)
		{
			return null;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0002E260 File Offset: 0x0002D260
		internal static void ClearNative(IntPtr pNative)
		{
		}
	}
}
