using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.StubHelpers
{
	// Token: 0x02000119 RID: 281
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static class CSTRMarshaler
	{
		// Token: 0x0600102B RID: 4139 RVA: 0x0002DFC8 File Offset: 0x0002CFC8
		internal unsafe static IntPtr ConvertToNative(int flags, string strManaged)
		{
			if (strManaged == null)
			{
				return IntPtr.Zero;
			}
			int length = strManaged.Length;
			StubHelpers.CheckStringLength(length);
			byte[] array = AnsiCharMarshaler.DoAnsiConversion(strManaged, 0 != (flags & 255), 0 != flags >> 8);
			int num = array.Length;
			int num2 = (length + 2) * Marshal.SystemMaxDBCSCharSize;
			num2 = Math.Max(num2, num + 2);
			byte* ptr = (byte*)((void*)Win32Native.CoTaskMemAlloc(num2));
			Buffer.memcpy(array, 0, ptr, 0, num);
			ptr[num] = 0;
			ptr[num + 1] = 0;
			return (IntPtr)((void*)ptr);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0002E051 File Offset: 0x0002D051
		internal unsafe static string ConvertToManaged(IntPtr cstr)
		{
			if (IntPtr.Zero == cstr)
			{
				return null;
			}
			return new string((sbyte*)((void*)cstr));
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0002E06D File Offset: 0x0002D06D
		internal static void ClearNative(IntPtr pNative)
		{
			Win32Native.CoTaskMemFree(pNative);
		}
	}
}
