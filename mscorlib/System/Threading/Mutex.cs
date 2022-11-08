using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Threading
{
	// Token: 0x0200015E RID: 350
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public sealed class Mutex : WaitHandle
	{
		// Token: 0x060012A4 RID: 4772 RVA: 0x00033798 File Offset: 0x00032798
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Mutex(bool initiallyOwned, string name, out bool createdNew) : this(initiallyOwned, name, out createdNew, null)
		{
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x000338DC File Offset: 0x000328DC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public unsafe Mutex(bool initiallyOwned, string name, out bool createdNew, MutexSecurity mutexSecurity)
		{
			Mutex.<>c__DisplayClass1 CS$<>8__locals1 = new Mutex.<>c__DisplayClass1();
			CS$<>8__locals1.initiallyOwned = initiallyOwned2;
			CS$<>8__locals1.name = name2;
			base..ctor();
			bool initiallyOwned = initiallyOwned2;
			string name = name2;
			if (CS$<>8__locals1.name != null && 260 < CS$<>8__locals1.name.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_WaitHandleNameTooLong", new object[]
				{
					CS$<>8__locals1.name
				}));
			}
			Win32Native.SECURITY_ATTRIBUTES secAttrs = null;
			if (mutexSecurity != null)
			{
				secAttrs = new Win32Native.SECURITY_ATTRIBUTES();
				secAttrs.nLength = Marshal.SizeOf(secAttrs);
				byte[] securityDescriptorBinaryForm = mutexSecurity.GetSecurityDescriptorBinaryForm();
				byte* ptr = stackalloc byte[1 * securityDescriptorBinaryForm.Length];
				Buffer.memcpy(securityDescriptorBinaryForm, 0, ptr, 0, securityDescriptorBinaryForm.Length);
				secAttrs.pSecurityDescriptor = ptr;
			}
			SafeWaitHandle mutexHandle = null;
			bool newMutex = false;
			RuntimeHelpers.CleanupCode backoutCode = new RuntimeHelpers.CleanupCode(this.MutexCleanupCode);
			Mutex.MutexCleanupInfo cleanupInfo = new Mutex.MutexCleanupInfo(mutexHandle, false);
			RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(delegate(object userData)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					if (CS$<>8__locals1.initiallyOwned)
					{
						cleanupInfo.inCriticalRegion = true;
						Thread.BeginThreadAffinity();
						Thread.BeginCriticalRegion();
					}
				}
				int num = 0;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					num = Mutex.CreateMutexHandle(CS$<>8__locals1.initiallyOwned, CS$<>8__locals1.name, secAttrs, out mutexHandle);
				}
				if (mutexHandle.IsInvalid)
				{
					mutexHandle.SetHandleAsInvalid();
					if (CS$<>8__locals1.name != null && CS$<>8__locals1.name.Length != 0 && 6 == num)
					{
						throw new WaitHandleCannotBeOpenedException(Environment.GetResourceString("Threading.WaitHandleCannotBeOpenedException_InvalidHandle", new object[]
						{
							CS$<>8__locals1.name
						}));
					}
					__Error.WinIOError(num, CS$<>8__locals1.name);
				}
				newMutex = (num != 183);
				this.SetHandleInternal(mutexHandle);
				this.hasThreadAffinity = true;
			}, backoutCode, cleanupInfo);
			createdNew = newMutex;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00033A0C File Offset: 0x00032A0C
		[PrePrepareMethod]
		private void MutexCleanupCode(object userData, bool exceptionThrown)
		{
			Mutex.MutexCleanupInfo mutexCleanupInfo = (Mutex.MutexCleanupInfo)userData;
			if (!this.hasThreadAffinity)
			{
				if (mutexCleanupInfo.mutexHandle != null && !mutexCleanupInfo.mutexHandle.IsInvalid)
				{
					if (mutexCleanupInfo.inCriticalRegion)
					{
						Win32Native.ReleaseMutex(mutexCleanupInfo.mutexHandle);
					}
					mutexCleanupInfo.mutexHandle.Dispose();
				}
				if (mutexCleanupInfo.inCriticalRegion)
				{
					Thread.EndCriticalRegion();
					Thread.EndThreadAffinity();
				}
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00033A6E File Offset: 0x00032A6E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Mutex(bool initiallyOwned, string name) : this(initiallyOwned, name, out Mutex.dummyBool)
		{
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00033A7D File Offset: 0x00032A7D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Mutex(bool initiallyOwned) : this(initiallyOwned, null, out Mutex.dummyBool)
		{
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00033A8C File Offset: 0x00032A8C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Mutex() : this(false, null, out Mutex.dummyBool)
		{
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00033A9B File Offset: 0x00032A9B
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private Mutex(SafeWaitHandle handle)
		{
			base.SetHandleInternal(handle);
			this.hasThreadAffinity = true;
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00033AB1 File Offset: 0x00032AB1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Mutex OpenExisting(string name)
		{
			return Mutex.OpenExisting(name, MutexRights.Modify | MutexRights.Synchronize);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00033AC0 File Offset: 0x00032AC0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Mutex OpenExisting(string name, MutexRights rights)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", Environment.GetResourceString("ArgumentNull_WithParamName"));
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (260 < name.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_WaitHandleNameTooLong", new object[]
				{
					name
				}));
			}
			SafeWaitHandle safeWaitHandle = Win32Native.OpenMutex((int)rights, false, name);
			if (safeWaitHandle.IsInvalid)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (2 == lastWin32Error || 123 == lastWin32Error)
				{
					throw new WaitHandleCannotBeOpenedException();
				}
				if (name != null && name.Length != 0 && 6 == lastWin32Error)
				{
					throw new WaitHandleCannotBeOpenedException(Environment.GetResourceString("Threading.WaitHandleCannotBeOpenedException_InvalidHandle", new object[]
					{
						name
					}));
				}
				__Error.WinIOError(lastWin32Error, name);
			}
			return new Mutex(safeWaitHandle);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00033B8B File Offset: 0x00032B8B
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void ReleaseMutex()
		{
			if (Win32Native.ReleaseMutex(this.safeWaitHandle))
			{
				Thread.EndCriticalRegion();
				Thread.EndThreadAffinity();
				return;
			}
			throw new ApplicationException(Environment.GetResourceString("Arg_SynchronizationLockException"));
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00033BB4 File Offset: 0x00032BB4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private static int CreateMutexHandle(bool initiallyOwned, string name, Win32Native.SECURITY_ATTRIBUTES securityAttribute, out SafeWaitHandle mutexHandle)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			int num;
			do
			{
				flag2 = false;
				flag3 = false;
				mutexHandle = Win32Native.CreateMutex(securityAttribute, initiallyOwned, name);
				num = Marshal.GetLastWin32Error();
				if (!mutexHandle.IsInvalid || num != 5)
				{
					return num;
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						Thread.BeginThreadAffinity();
						flag = true;
					}
					mutexHandle = Win32Native.OpenMutex(1048577, false, name);
					if (!mutexHandle.IsInvalid)
					{
						num = 183;
						if (Environment.IsW2k3)
						{
							SafeWaitHandle safeWaitHandle = Win32Native.OpenMutex(1048577, false, name);
							if (!safeWaitHandle.IsInvalid)
							{
								RuntimeHelpers.PrepareConstrainedRegions();
								try
								{
									IntPtr intPtr = mutexHandle.DangerousGetHandle();
									IntPtr intPtr2 = safeWaitHandle.DangerousGetHandle();
									IntPtr[] array = new IntPtr[]
									{
										intPtr,
										intPtr2
									};
									uint num2 = Win32Native.WaitForMultipleObjects(2U, array, true, 0U);
									GC.KeepAlive(array);
									if (num2 == 4294967295U)
									{
										uint lastWin32Error = (uint)Marshal.GetLastWin32Error();
										if (lastWin32Error != 87U)
										{
											mutexHandle.Dispose();
											flag3 = true;
										}
									}
									else
									{
										flag2 = true;
										if (num2 >= 0U && num2 < 2U)
										{
											Win32Native.ReleaseMutex(mutexHandle);
											Win32Native.ReleaseMutex(safeWaitHandle);
										}
										else if (num2 >= 128U && num2 < 130U)
										{
											Win32Native.ReleaseMutex(mutexHandle);
											Win32Native.ReleaseMutex(safeWaitHandle);
										}
										mutexHandle.Dispose();
									}
									goto IL_15B;
								}
								finally
								{
									safeWaitHandle.Dispose();
								}
							}
							mutexHandle.Dispose();
							flag3 = true;
						}
					}
					else
					{
						num = Marshal.GetLastWin32Error();
					}
					IL_15B:;
				}
				finally
				{
					if (flag)
					{
						Thread.EndThreadAffinity();
					}
				}
			}
			while (flag2 || flag3 || num == 2);
			if (num == 0)
			{
				num = 183;
			}
			return num;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00033D90 File Offset: 0x00032D90
		public MutexSecurity GetAccessControl()
		{
			return new MutexSecurity(this.safeWaitHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00033D9F File Offset: 0x00032D9F
		public void SetAccessControl(MutexSecurity mutexSecurity)
		{
			if (mutexSecurity == null)
			{
				throw new ArgumentNullException("mutexSecurity");
			}
			mutexSecurity.Persist(this.safeWaitHandle);
		}

		// Token: 0x04000662 RID: 1634
		private const int WAIT_OBJECT_0 = 0;

		// Token: 0x04000663 RID: 1635
		private const int WAIT_ABANDONED_0 = 128;

		// Token: 0x04000664 RID: 1636
		private const uint WAIT_FAILED = 4294967295U;

		// Token: 0x04000665 RID: 1637
		private static bool dummyBool;

		// Token: 0x0200015F RID: 351
		internal class MutexCleanupInfo
		{
			// Token: 0x060012B1 RID: 4785 RVA: 0x00033DBB File Offset: 0x00032DBB
			internal MutexCleanupInfo(SafeWaitHandle mutexHandle, bool inCriticalRegion)
			{
				this.mutexHandle = mutexHandle;
				this.inCriticalRegion = inCriticalRegion;
			}

			// Token: 0x04000666 RID: 1638
			internal SafeWaitHandle mutexHandle;

			// Token: 0x04000667 RID: 1639
			internal bool inCriticalRegion;
		}
	}
}
