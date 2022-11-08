using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.StubHelpers
{
	// Token: 0x0200011C RID: 284
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static class AnsiBSTRMarshaler
	{
		// Token: 0x06001034 RID: 4148 RVA: 0x0002E17C File Offset: 0x0002D17C
		internal unsafe static IntPtr ConvertToNative(int flags, string strManaged)
		{
			if (strManaged == null)
			{
				return IntPtr.Zero;
			}
			int length = strManaged.Length;
			StubHelpers.CheckStringLength(length);
			byte[] array = null;
			int num = 0;
			if (length > 0)
			{
				array = AnsiCharMarshaler.DoAnsiConversion(strManaged, 0 != (flags & 255), 0 != flags >> 8);
				num = array.Length;
			}
			int num2 = (length + 2) * Marshal.SystemMaxDBCSCharSize + 4;
			num2 = Math.Max(num2, num + 2 + 4);
			byte* ptr = (byte*)((void*)Win32Native.CoTaskMemAlloc(num2)) + 4;
			Buffer.memcpy(array, 0, ptr, 0, num);
			ptr[num] = 0;
			ptr[num + 1] = 0;
			*(int*)(ptr + -4) = num;
			return (IntPtr)((void*)ptr);
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0002E218 File Offset: 0x0002D218
		internal unsafe static string ConvertToManaged(IntPtr bstr)
		{
			if (IntPtr.Zero == bstr)
			{
				return null;
			}
			return new string((sbyte*)((void*)bstr));
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0002E234 File Offset: 0x0002D234
		internal static void ClearNative(IntPtr pNative)
		{
			if (IntPtr.Zero != pNative)
			{
				Win32Native.CoTaskMemFree((IntPtr)((long)pNative - 4L));
			}
		}
	}
}
