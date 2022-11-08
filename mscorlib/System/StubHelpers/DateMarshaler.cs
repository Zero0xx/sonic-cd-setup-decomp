using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System.StubHelpers
{
	// Token: 0x02000120 RID: 288
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static class DateMarshaler
	{
		// Token: 0x06001040 RID: 4160
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern double ConvertToNative(DateTime managedDate);

		// Token: 0x06001041 RID: 4161
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern DateTime ConvertToManaged(double nativeDate);
	}
}
