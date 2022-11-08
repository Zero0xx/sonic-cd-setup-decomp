using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32;

namespace System.StubHelpers
{
	// Token: 0x0200011A RID: 282
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static class BSTRMarshaler
	{
		// Token: 0x0600102E RID: 4142 RVA: 0x0002E075 File Offset: 0x0002D075
		internal static IntPtr ConvertToNative(string strManaged)
		{
			if (strManaged == null)
			{
				return IntPtr.Zero;
			}
			return Win32Native.SysAllocStringLen(strManaged, strManaged.Length);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0002E08C File Offset: 0x0002D08C
		internal unsafe static string ConvertToManaged(IntPtr bstr)
		{
			if (IntPtr.Zero == bstr)
			{
				return null;
			}
			return new string((char*)((void*)bstr), 0, Win32Native.SysStringLen(bstr));
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0002E0AF File Offset: 0x0002D0AF
		internal static void ClearNative(IntPtr pNative)
		{
			if (IntPtr.Zero != pNative)
			{
				Win32Native.SysFreeString(pNative);
			}
		}
	}
}
