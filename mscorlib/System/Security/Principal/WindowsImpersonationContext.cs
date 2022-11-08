using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x020004D2 RID: 1234
	[ComVisible(true)]
	public class WindowsImpersonationContext : IDisposable
	{
		// Token: 0x06003120 RID: 12576 RVA: 0x000A8B6C File Offset: 0x000A7B6C
		private WindowsImpersonationContext()
		{
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x000A8B80 File Offset: 0x000A7B80
		internal WindowsImpersonationContext(SafeTokenHandle safeTokenHandle, WindowsIdentity wi, bool isImpersonating, FrameSecurityDescriptor fsd)
		{
			if (WindowsIdentity.RunningOnWin2K)
			{
				if (safeTokenHandle.IsInvalid)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidImpersonationToken"));
				}
				if (isImpersonating)
				{
					if (!Win32Native.DuplicateHandle(Win32Native.GetCurrentProcess(), safeTokenHandle, Win32Native.GetCurrentProcess(), ref this.m_safeTokenHandle, 0U, true, 2U))
					{
						throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
					}
					this.m_wi = wi;
				}
				this.m_fsd = fsd;
			}
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x000A8BFC File Offset: 0x000A7BFC
		public void Undo()
		{
			if (!WindowsIdentity.RunningOnWin2K)
			{
				return;
			}
			if (this.m_safeTokenHandle.IsInvalid)
			{
				int num = Win32.RevertToSelf();
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
			}
			else
			{
				int num = Win32.RevertToSelf();
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
				num = Win32.ImpersonateLoggedOnUser(this.m_safeTokenHandle);
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
			}
			WindowsIdentity.UpdateThreadWI(this.m_wi);
			if (this.m_fsd != null)
			{
				this.m_fsd.SetTokenHandles(null, null);
			}
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x000A8C88 File Offset: 0x000A7C88
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool UndoNoThrow()
		{
			bool result = false;
			try
			{
				if (!WindowsIdentity.RunningOnWin2K)
				{
					return true;
				}
				int num;
				if (this.m_safeTokenHandle.IsInvalid)
				{
					num = Win32.RevertToSelf();
				}
				else
				{
					num = Win32.RevertToSelf();
					if (num >= 0)
					{
						num = Win32.ImpersonateLoggedOnUser(this.m_safeTokenHandle);
					}
				}
				result = (num >= 0);
				if (this.m_fsd != null)
				{
					this.m_fsd.SetTokenHandles(null, null);
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x000A8D08 File Offset: 0x000A7D08
		[ComVisible(false)]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.m_safeTokenHandle != null && !this.m_safeTokenHandle.IsClosed)
			{
				this.Undo();
				this.m_safeTokenHandle.Dispose();
			}
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x000A8D33 File Offset: 0x000A7D33
		[ComVisible(false)]
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x040018CD RID: 6349
		private SafeTokenHandle m_safeTokenHandle = SafeTokenHandle.InvalidHandle;

		// Token: 0x040018CE RID: 6350
		private WindowsIdentity m_wi;

		// Token: 0x040018CF RID: 6351
		private FrameSecurityDescriptor m_fsd;
	}
}
