using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x020008FF RID: 2303
	internal sealed class Privilege
	{
		// Token: 0x0600537E RID: 21374 RVA: 0x0012D2EC File Offset: 0x0012C2EC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private static Win32Native.LUID LuidFromPrivilege(string privilege)
		{
			Win32Native.LUID luid;
			luid.LowPart = 0U;
			luid.HighPart = 0U;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Privilege.privilegeLock.AcquireReaderLock(-1);
				if (Privilege.luids.Contains(privilege))
				{
					luid = (Win32Native.LUID)Privilege.luids[privilege];
					Privilege.privilegeLock.ReleaseReaderLock();
				}
				else
				{
					Privilege.privilegeLock.ReleaseReaderLock();
					if (!Win32Native.LookupPrivilegeValue(null, privilege, ref luid))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error == 8)
						{
							throw new OutOfMemoryException();
						}
						if (lastWin32Error == 5)
						{
							throw new UnauthorizedAccessException();
						}
						if (lastWin32Error == 1313)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPrivilegeName", new object[]
							{
								privilege
							}));
						}
						throw new InvalidOperationException();
					}
					else
					{
						Privilege.privilegeLock.AcquireWriterLock(-1);
					}
				}
			}
			finally
			{
				if (Privilege.privilegeLock.IsReaderLockHeld)
				{
					Privilege.privilegeLock.ReleaseReaderLock();
				}
				if (Privilege.privilegeLock.IsWriterLockHeld)
				{
					if (!Privilege.luids.Contains(privilege))
					{
						Privilege.luids[privilege] = luid;
						Privilege.privileges[luid] = privilege;
					}
					Privilege.privilegeLock.ReleaseWriterLock();
				}
			}
			return luid;
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x0012D418 File Offset: 0x0012C418
		public Privilege(string privilegeName)
		{
			if (!WindowsIdentity.RunningOnWin2K)
			{
				throw new NotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresNT"));
			}
			if (privilegeName == null)
			{
				throw new ArgumentNullException("privilegeName");
			}
			this.luid = Privilege.LuidFromPrivilege(privilegeName);
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x0012D468 File Offset: 0x0012C468
		~Privilege()
		{
			if (this.needToRevert)
			{
				this.Revert();
			}
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x0012D49C File Offset: 0x0012C49C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Enable()
		{
			this.ToggleState(true);
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06005382 RID: 21378 RVA: 0x0012D4A5 File Offset: 0x0012C4A5
		public bool NeedToRevert
		{
			get
			{
				return this.needToRevert;
			}
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x0012D4B0 File Offset: 0x0012C4B0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private void ToggleState(bool enable)
		{
			int num = 0;
			if (!this.currentThread.Equals(Thread.CurrentThread))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustBeSameThread"));
			}
			if (this.needToRevert)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustRevertPrivilege"));
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				try
				{
					this.tlsContents = (Thread.GetData(Privilege.tlsSlot) as Privilege.TlsContents);
					if (this.tlsContents == null)
					{
						this.tlsContents = new Privilege.TlsContents();
						Thread.SetData(Privilege.tlsSlot, this.tlsContents);
					}
					else
					{
						this.tlsContents.IncrementReferenceCount();
					}
					Win32Native.TOKEN_PRIVILEGE token_PRIVILEGE = default(Win32Native.TOKEN_PRIVILEGE);
					token_PRIVILEGE.PrivilegeCount = 1U;
					token_PRIVILEGE.Privilege.Luid = this.luid;
					token_PRIVILEGE.Privilege.Attributes = (enable ? 2U : 0U);
					Win32Native.TOKEN_PRIVILEGE token_PRIVILEGE2 = default(Win32Native.TOKEN_PRIVILEGE);
					uint num2 = 0U;
					if (!Win32Native.AdjustTokenPrivileges(this.tlsContents.ThreadHandle, false, ref token_PRIVILEGE, (uint)Marshal.SizeOf(token_PRIVILEGE2), ref token_PRIVILEGE2, ref num2))
					{
						num = Marshal.GetLastWin32Error();
					}
					else if (1300 == Marshal.GetLastWin32Error())
					{
						num = 1300;
					}
					else
					{
						this.initialState = ((token_PRIVILEGE2.Privilege.Attributes & 2U) != 0U);
						this.stateWasChanged = (this.initialState != enable);
						this.needToRevert = (this.tlsContents.IsImpersonating || this.stateWasChanged);
					}
				}
				finally
				{
					if (!this.needToRevert)
					{
						this.Reset();
					}
				}
			}
			if (num == 1300)
			{
				throw new PrivilegeNotHeldException(Privilege.privileges[this.luid] as string);
			}
			if (num == 8)
			{
				throw new OutOfMemoryException();
			}
			if (num == 5 || num == 1347)
			{
				throw new UnauthorizedAccessException();
			}
			if (num != 0)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x0012D6A4 File Offset: 0x0012C6A4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Revert()
		{
			int num = 0;
			if (!this.currentThread.Equals(Thread.CurrentThread))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustBeSameThread"));
			}
			if (!this.NeedToRevert)
			{
				return;
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				bool flag = true;
				try
				{
					if (this.stateWasChanged && (this.tlsContents.ReferenceCountValue > 1 || !this.tlsContents.IsImpersonating))
					{
						Win32Native.TOKEN_PRIVILEGE token_PRIVILEGE = default(Win32Native.TOKEN_PRIVILEGE);
						token_PRIVILEGE.PrivilegeCount = 1U;
						token_PRIVILEGE.Privilege.Luid = this.luid;
						token_PRIVILEGE.Privilege.Attributes = (this.initialState ? 2U : 0U);
						Win32Native.TOKEN_PRIVILEGE token_PRIVILEGE2 = default(Win32Native.TOKEN_PRIVILEGE);
						uint num2 = 0U;
						if (!Win32Native.AdjustTokenPrivileges(this.tlsContents.ThreadHandle, false, ref token_PRIVILEGE, (uint)Marshal.SizeOf(token_PRIVILEGE2), ref token_PRIVILEGE2, ref num2))
						{
							num = Marshal.GetLastWin32Error();
							flag = false;
						}
					}
				}
				finally
				{
					if (flag)
					{
						this.Reset();
					}
				}
			}
			if (num == 8)
			{
				throw new OutOfMemoryException();
			}
			if (num == 5)
			{
				throw new UnauthorizedAccessException();
			}
			if (num != 0)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06005385 RID: 21381 RVA: 0x0012D7C8 File Offset: 0x0012C7C8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void Reset()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.stateWasChanged = false;
				this.initialState = false;
				this.needToRevert = false;
				if (this.tlsContents != null && this.tlsContents.DecrementReferenceCount() == 0)
				{
					this.tlsContents = null;
					Thread.SetData(Privilege.tlsSlot, null);
				}
			}
		}

		// Token: 0x04002B1C RID: 11036
		public const string CreateToken = "SeCreateTokenPrivilege";

		// Token: 0x04002B1D RID: 11037
		public const string AssignPrimaryToken = "SeAssignPrimaryTokenPrivilege";

		// Token: 0x04002B1E RID: 11038
		public const string LockMemory = "SeLockMemoryPrivilege";

		// Token: 0x04002B1F RID: 11039
		public const string IncreaseQuota = "SeIncreaseQuotaPrivilege";

		// Token: 0x04002B20 RID: 11040
		public const string UnsolicitedInput = "SeUnsolicitedInputPrivilege";

		// Token: 0x04002B21 RID: 11041
		public const string MachineAccount = "SeMachineAccountPrivilege";

		// Token: 0x04002B22 RID: 11042
		public const string TrustedComputingBase = "SeTcbPrivilege";

		// Token: 0x04002B23 RID: 11043
		public const string Security = "SeSecurityPrivilege";

		// Token: 0x04002B24 RID: 11044
		public const string TakeOwnership = "SeTakeOwnershipPrivilege";

		// Token: 0x04002B25 RID: 11045
		public const string LoadDriver = "SeLoadDriverPrivilege";

		// Token: 0x04002B26 RID: 11046
		public const string SystemProfile = "SeSystemProfilePrivilege";

		// Token: 0x04002B27 RID: 11047
		public const string SystemTime = "SeSystemtimePrivilege";

		// Token: 0x04002B28 RID: 11048
		public const string ProfileSingleProcess = "SeProfileSingleProcessPrivilege";

		// Token: 0x04002B29 RID: 11049
		public const string IncreaseBasePriority = "SeIncreaseBasePriorityPrivilege";

		// Token: 0x04002B2A RID: 11050
		public const string CreatePageFile = "SeCreatePagefilePrivilege";

		// Token: 0x04002B2B RID: 11051
		public const string CreatePermanent = "SeCreatePermanentPrivilege";

		// Token: 0x04002B2C RID: 11052
		public const string Backup = "SeBackupPrivilege";

		// Token: 0x04002B2D RID: 11053
		public const string Restore = "SeRestorePrivilege";

		// Token: 0x04002B2E RID: 11054
		public const string Shutdown = "SeShutdownPrivilege";

		// Token: 0x04002B2F RID: 11055
		public const string Debug = "SeDebugPrivilege";

		// Token: 0x04002B30 RID: 11056
		public const string Audit = "SeAuditPrivilege";

		// Token: 0x04002B31 RID: 11057
		public const string SystemEnvironment = "SeSystemEnvironmentPrivilege";

		// Token: 0x04002B32 RID: 11058
		public const string ChangeNotify = "SeChangeNotifyPrivilege";

		// Token: 0x04002B33 RID: 11059
		public const string RemoteShutdown = "SeRemoteShutdownPrivilege";

		// Token: 0x04002B34 RID: 11060
		public const string Undock = "SeUndockPrivilege";

		// Token: 0x04002B35 RID: 11061
		public const string SyncAgent = "SeSyncAgentPrivilege";

		// Token: 0x04002B36 RID: 11062
		public const string EnableDelegation = "SeEnableDelegationPrivilege";

		// Token: 0x04002B37 RID: 11063
		public const string ManageVolume = "SeManageVolumePrivilege";

		// Token: 0x04002B38 RID: 11064
		public const string Impersonate = "SeImpersonatePrivilege";

		// Token: 0x04002B39 RID: 11065
		public const string CreateGlobal = "SeCreateGlobalPrivilege";

		// Token: 0x04002B3A RID: 11066
		public const string TrustedCredentialManagerAccess = "SeTrustedCredManAccessPrivilege";

		// Token: 0x04002B3B RID: 11067
		public const string ReserveProcessor = "SeReserveProcessorPrivilege";

		// Token: 0x04002B3C RID: 11068
		private static LocalDataStoreSlot tlsSlot = Thread.AllocateDataSlot();

		// Token: 0x04002B3D RID: 11069
		private static Hashtable privileges = new Hashtable();

		// Token: 0x04002B3E RID: 11070
		private static Hashtable luids = new Hashtable();

		// Token: 0x04002B3F RID: 11071
		private static ReaderWriterLock privilegeLock = new ReaderWriterLock();

		// Token: 0x04002B40 RID: 11072
		private bool needToRevert;

		// Token: 0x04002B41 RID: 11073
		private bool initialState;

		// Token: 0x04002B42 RID: 11074
		private bool stateWasChanged;

		// Token: 0x04002B43 RID: 11075
		private Win32Native.LUID luid;

		// Token: 0x04002B44 RID: 11076
		private readonly Thread currentThread = Thread.CurrentThread;

		// Token: 0x04002B45 RID: 11077
		private Privilege.TlsContents tlsContents;

		// Token: 0x02000900 RID: 2304
		private sealed class TlsContents : IDisposable
		{
			// Token: 0x06005387 RID: 21383 RVA: 0x0012D858 File Offset: 0x0012C858
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			public TlsContents()
			{
				int num = 0;
				int num2 = 0;
				bool flag = true;
				if (Privilege.TlsContents.processHandle.IsInvalid)
				{
					lock (Privilege.TlsContents.syncRoot)
					{
						if (Privilege.TlsContents.processHandle.IsInvalid && !Win32Native.OpenProcessToken(Win32Native.GetCurrentProcess(), TokenAccessLevels.Duplicate, ref Privilege.TlsContents.processHandle))
						{
							num2 = Marshal.GetLastWin32Error();
							flag = false;
						}
					}
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					try
					{
						SafeTokenHandle safeTokenHandle = this.threadHandle;
						num = Win32.OpenThreadToken(TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, WinSecurityContext.Process, out this.threadHandle);
						num &= 2147024895;
						if (num != 0)
						{
							if (flag)
							{
								this.threadHandle = safeTokenHandle;
								if (num != 1008)
								{
									flag = false;
								}
								if (flag)
								{
									num = 0;
									if (!Win32Native.DuplicateTokenEx(Privilege.TlsContents.processHandle, TokenAccessLevels.Impersonate | TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, IntPtr.Zero, Win32Native.SECURITY_IMPERSONATION_LEVEL.Impersonation, System.Security.Principal.TokenType.TokenImpersonation, ref this.threadHandle))
									{
										num = Marshal.GetLastWin32Error();
										flag = false;
									}
								}
								if (flag)
								{
									num = Win32.SetThreadToken(this.threadHandle);
									num &= 2147024895;
									if (num != 0)
									{
										flag = false;
									}
								}
								if (flag)
								{
									this.isImpersonating = true;
								}
							}
							else
							{
								num = num2;
							}
						}
						else
						{
							flag = true;
						}
					}
					finally
					{
						if (!flag)
						{
							this.Dispose();
						}
					}
				}
				if (num == 8)
				{
					throw new OutOfMemoryException();
				}
				if (num == 5 || num == 1347)
				{
					throw new UnauthorizedAccessException();
				}
				if (num != 0)
				{
					throw new InvalidOperationException();
				}
			}

			// Token: 0x06005388 RID: 21384 RVA: 0x0012D9C0 File Offset: 0x0012C9C0
			~TlsContents()
			{
				if (!this.disposed)
				{
					this.Dispose(false);
				}
			}

			// Token: 0x06005389 RID: 21385 RVA: 0x0012D9F8 File Offset: 0x0012C9F8
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x0600538A RID: 21386 RVA: 0x0012DA07 File Offset: 0x0012CA07
			private void Dispose(bool disposing)
			{
				if (this.disposed)
				{
					return;
				}
				if (disposing && this.threadHandle != null)
				{
					this.threadHandle.Dispose();
					this.threadHandle = null;
				}
				if (this.isImpersonating)
				{
					Win32.RevertToSelf();
				}
				this.disposed = true;
			}

			// Token: 0x0600538B RID: 21387 RVA: 0x0012DA44 File Offset: 0x0012CA44
			public void IncrementReferenceCount()
			{
				this.referenceCount++;
			}

			// Token: 0x0600538C RID: 21388 RVA: 0x0012DA54 File Offset: 0x0012CA54
			public int DecrementReferenceCount()
			{
				int num = --this.referenceCount;
				if (num == 0)
				{
					this.Dispose();
				}
				return num;
			}

			// Token: 0x17000E82 RID: 3714
			// (get) Token: 0x0600538D RID: 21389 RVA: 0x0012DA7D File Offset: 0x0012CA7D
			public int ReferenceCountValue
			{
				get
				{
					return this.referenceCount;
				}
			}

			// Token: 0x17000E83 RID: 3715
			// (get) Token: 0x0600538E RID: 21390 RVA: 0x0012DA85 File Offset: 0x0012CA85
			public SafeTokenHandle ThreadHandle
			{
				get
				{
					return this.threadHandle;
				}
			}

			// Token: 0x17000E84 RID: 3716
			// (get) Token: 0x0600538F RID: 21391 RVA: 0x0012DA8D File Offset: 0x0012CA8D
			public bool IsImpersonating
			{
				get
				{
					return this.isImpersonating;
				}
			}

			// Token: 0x04002B46 RID: 11078
			private bool disposed;

			// Token: 0x04002B47 RID: 11079
			private int referenceCount = 1;

			// Token: 0x04002B48 RID: 11080
			private SafeTokenHandle threadHandle = new SafeTokenHandle(IntPtr.Zero);

			// Token: 0x04002B49 RID: 11081
			private bool isImpersonating;

			// Token: 0x04002B4A RID: 11082
			private static SafeTokenHandle processHandle = new SafeTokenHandle(IntPtr.Zero);

			// Token: 0x04002B4B RID: 11083
			private static readonly object syncRoot = new object();
		}
	}
}
