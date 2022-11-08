using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200050D RID: 1293
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCloseHandle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600280B RID: 10251 RVA: 0x000A5456 File Offset: 0x000A4456
		private SafeCloseHandle()
		{
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000A545E File Offset: 0x000A445E
		internal IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000A5466 File Offset: 0x000A4466
		protected override bool ReleaseHandle()
		{
			return this.IsInvalid || Interlocked.Increment(ref this._disposed) != 1 || UnsafeNclNativeMethods.SafeNetHandles.CloseHandle(this.handle);
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000A548C File Offset: 0x000A448C
		internal static int GetSecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle safeHandle)
		{
			int result = -2146893055;
			bool flag = false;
			safeHandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				phContext.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					phContext.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					result = UnsafeNclNativeMethods.SafeNetHandles.QuerySecurityContextToken(ref phContext._handle, out safeHandle);
					phContext.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000A5504 File Offset: 0x000A4504
		internal static SafeCloseHandle CreateRequestQueueHandle()
		{
			SafeCloseHandle safeCloseHandle = null;
			uint num = UnsafeNclNativeMethods.SafeNetHandles.HttpCreateHttpHandle(out safeCloseHandle, 0U);
			if (safeCloseHandle != null && num != 0U)
			{
				safeCloseHandle.SetHandleAsInvalid();
				throw new HttpListenerException((int)num);
			}
			return safeCloseHandle;
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000A5530 File Offset: 0x000A4530
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Abort()
		{
			this.ReleaseHandle();
			base.SetHandleAsInvalid();
		}

		// Token: 0x04002758 RID: 10072
		private const string SECURITY = "security.dll";

		// Token: 0x04002759 RID: 10073
		private const string ADVAPI32 = "advapi32.dll";

		// Token: 0x0400275A RID: 10074
		private const string HTTPAPI = "httpapi.dll";

		// Token: 0x0400275B RID: 10075
		private int _disposed;
	}
}
