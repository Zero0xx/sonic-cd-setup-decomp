using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x02000447 RID: 1095
	internal static class Fusion
	{
		// Token: 0x06002C73 RID: 11379 RVA: 0x00096D50 File Offset: 0x00095D50
		public static void ReadCache(ArrayList alAssems, string name, uint nFlag)
		{
			IAssemblyEnum assemblyEnum = null;
			IAssemblyName aName = null;
			IAssemblyName pName = null;
			IApplicationContext pAppCtx = null;
			int num;
			if (name != null)
			{
				num = Win32Native.CreateAssemblyNameObject(out pName, name, 1U, IntPtr.Zero);
				if (num != 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
			}
			num = Win32Native.CreateAssemblyEnum(out assemblyEnum, pAppCtx, pName, nFlag, IntPtr.Zero);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			for (;;)
			{
				num = assemblyEnum.GetNextAssembly(out pAppCtx, out aName, 0U);
				if (num != 0)
				{
					break;
				}
				string displayName = Fusion.GetDisplayName(aName, 0U);
				if (displayName != null)
				{
					alAssems.Add(displayName);
				}
			}
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
				return;
			}
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x00096DD8 File Offset: 0x00095DD8
		private unsafe static string GetDisplayName(IAssemblyName aName, uint dwDisplayFlags)
		{
			uint num = 0U;
			string result = null;
			aName.GetDisplayName((IntPtr)0, ref num, dwDisplayFlags);
			if (num > 0U)
			{
				IntPtr intPtr = (IntPtr)0;
				byte[] array = new byte[(num + 1U) * 2U];
				fixed (byte* ptr = array)
				{
					intPtr = new IntPtr((void*)ptr);
					aName.GetDisplayName(intPtr, ref num, dwDisplayFlags);
					result = Marshal.PtrToStringUni(intPtr);
				}
			}
			return result;
		}
	}
}
