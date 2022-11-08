using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Security
{
	// Token: 0x0200068A RID: 1674
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeBSTRHandle : SafePointer
	{
		// Token: 0x06003C7B RID: 15483 RVA: 0x000CEE68 File Offset: 0x000CDE68
		internal SafeBSTRHandle() : base(true)
		{
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x000CEE74 File Offset: 0x000CDE74
		internal static SafeBSTRHandle Allocate(string src, uint len)
		{
			SafeBSTRHandle safeBSTRHandle = SafeBSTRHandle.SysAllocStringLen(src, len);
			safeBSTRHandle.Initialize((ulong)(len * 2U));
			return safeBSTRHandle;
		}

		// Token: 0x06003C7D RID: 15485
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
		private static extern SafeBSTRHandle SysAllocStringLen(string src, uint len);

		// Token: 0x06003C7E RID: 15486 RVA: 0x000CEE94 File Offset: 0x000CDE94
		protected override bool ReleaseHandle()
		{
			Win32Native.ZeroMemory(this.handle, (uint)(Win32Native.SysStringLen(this.handle) * 2));
			Win32Native.SysFreeString(this.handle);
			return true;
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x000CEEBC File Offset: 0x000CDEBC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal unsafe void ClearBuffer()
		{
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.AcquirePointer(ref ptr);
				Win32Native.ZeroMemory((IntPtr)((void*)ptr), (uint)(Win32Native.SysStringLen((IntPtr)((void*)ptr)) * 2));
			}
			finally
			{
				if (ptr != null)
				{
					base.ReleasePointer();
				}
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06003C80 RID: 15488 RVA: 0x000CEF10 File Offset: 0x000CDF10
		internal unsafe int Length
		{
			get
			{
				byte* ptr = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				int result;
				try
				{
					base.AcquirePointer(ref ptr);
					result = Win32Native.SysStringLen((IntPtr)((void*)ptr));
				}
				finally
				{
					if (ptr != null)
					{
						base.ReleasePointer();
					}
				}
				return result;
			}
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x000CEF58 File Offset: 0x000CDF58
		internal unsafe static void Copy(SafeBSTRHandle source, SafeBSTRHandle target)
		{
			byte* ptr = null;
			byte* ptr2 = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				source.AcquirePointer(ref ptr);
				target.AcquirePointer(ref ptr2);
				Buffer.memcpyimpl(ptr, ptr2, Win32Native.SysStringLen((IntPtr)((void*)ptr)) * 2);
			}
			finally
			{
				if (ptr != null)
				{
					source.ReleasePointer();
				}
				if (ptr2 != null)
				{
					target.ReleasePointer();
				}
			}
		}
	}
}
