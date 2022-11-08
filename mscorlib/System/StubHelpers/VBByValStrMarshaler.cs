using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32;

namespace System.StubHelpers
{
	// Token: 0x0200011B RID: 283
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static class VBByValStrMarshaler
	{
		// Token: 0x06001031 RID: 4145 RVA: 0x0002E0C4 File Offset: 0x0002D0C4
		internal unsafe static IntPtr ConvertToNative(string strManaged, bool fBestFit, bool fThrowOnUnmappableChar, ref int cch)
		{
			if (strManaged == null)
			{
				return IntPtr.Zero;
			}
			cch = strManaged.Length;
			StubHelpers.CheckStringLength(cch);
			int cb = 4 + (cch + 1) * 2;
			byte* ptr = (byte*)((void*)Win32Native.CoTaskMemAlloc(cb));
			int* ptr2 = (int*)ptr;
			ptr += 4;
			if (cch == 0)
			{
				*ptr = 0;
				*ptr2 = 0;
			}
			else
			{
				byte[] array = AnsiCharMarshaler.DoAnsiConversion(strManaged, fBestFit, fThrowOnUnmappableChar);
				int num = array.Length;
				Buffer.memcpy(array, 0, ptr, 0, num);
				ptr[num] = 0;
				*ptr2 = num;
			}
			return new IntPtr((void*)ptr);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0002E13A File Offset: 0x0002D13A
		internal unsafe static string ConvertToManaged(IntPtr pNative, int cch)
		{
			if (IntPtr.Zero == pNative)
			{
				return null;
			}
			return new string((sbyte*)((void*)pNative), 0, cch);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0002E158 File Offset: 0x0002D158
		internal static void ClearNative(IntPtr pNative)
		{
			if (IntPtr.Zero != pNative)
			{
				Win32Native.CoTaskMemFree((IntPtr)((long)pNative - 4L));
			}
		}
	}
}
