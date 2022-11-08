using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005DB RID: 1499
	public static class RuntimeHelpers
	{
		// Token: 0x060037B7 RID: 14263
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);

		// Token: 0x060037B8 RID: 14264
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectValue(object obj);

		// Token: 0x060037B9 RID: 14265
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _RunClassConstructor(IntPtr type);

		// Token: 0x060037BA RID: 14266 RVA: 0x000BB9FA File Offset: 0x000BA9FA
		public static void RunClassConstructor(RuntimeTypeHandle type)
		{
			RuntimeHelpers._RunClassConstructor(type.Value);
		}

		// Token: 0x060037BB RID: 14267
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _RunModuleConstructor(IntPtr module);

		// Token: 0x060037BC RID: 14268 RVA: 0x000BBA08 File Offset: 0x000BAA08
		public static void RunModuleConstructor(ModuleHandle module)
		{
			RuntimeHelpers._RunModuleConstructor(new IntPtr(module.Value));
		}

		// Token: 0x060037BD RID: 14269
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _PrepareMethod(IntPtr method, RuntimeTypeHandle[] instantiation);

		// Token: 0x060037BE RID: 14270
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _CompileMethod(IntPtr method);

		// Token: 0x060037BF RID: 14271 RVA: 0x000BBA1B File Offset: 0x000BAA1B
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void PrepareMethod(RuntimeMethodHandle method)
		{
			RuntimeHelpers._PrepareMethod(method.Value, null);
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x000BBA2A File Offset: 0x000BAA2A
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void PrepareMethod(RuntimeMethodHandle method, RuntimeTypeHandle[] instantiation)
		{
			RuntimeHelpers._PrepareMethod(method.Value, instantiation);
		}

		// Token: 0x060037C1 RID: 14273
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PrepareDelegate(Delegate d);

		// Token: 0x060037C2 RID: 14274 RVA: 0x000BBA39 File Offset: 0x000BAA39
		public static int GetHashCode(object o)
		{
			return object.InternalGetHashCode(o);
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x000BBA41 File Offset: 0x000BAA41
		public new static bool Equals(object o1, object o2)
		{
			return object.InternalEquals(o1, o2);
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x060037C4 RID: 14276 RVA: 0x000BBA4A File Offset: 0x000BAA4A
		public static int OffsetToStringData
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x060037C5 RID: 14277
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ProbeForSufficientStack();

		// Token: 0x060037C6 RID: 14278 RVA: 0x000BBA4E File Offset: 0x000BAA4E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void PrepareConstrainedRegions()
		{
			RuntimeHelpers.ProbeForSufficientStack();
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x000BBA55 File Offset: 0x000BAA55
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void PrepareConstrainedRegionsNoOP()
		{
		}

		// Token: 0x060037C8 RID: 14280
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExecuteCodeWithGuaranteedCleanup(RuntimeHelpers.TryCode code, RuntimeHelpers.CleanupCode backoutCode, object userData);

		// Token: 0x060037C9 RID: 14281 RVA: 0x000BBA57 File Offset: 0x000BAA57
		[PrePrepareMethod]
		internal static void ExecuteBackoutCodeHelper(object backoutCode, object userData, bool exceptionThrown)
		{
			((RuntimeHelpers.CleanupCode)backoutCode)(userData, exceptionThrown);
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x000BBA68 File Offset: 0x000BAA68
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void ExecuteCodeWithLock(object lockObject, RuntimeHelpers.TryCode code, object userState)
		{
			RuntimeHelpers.ExecuteWithLockHelper userData = new RuntimeHelpers.ExecuteWithLockHelper(lockObject, code, userState);
			RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(RuntimeHelpers.s_EnterMonitor, RuntimeHelpers.s_ExitMonitor, userData);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x000BBA90 File Offset: 0x000BAA90
		private static void EnterMonitorAndTryCode(object helper)
		{
			RuntimeHelpers.ExecuteWithLockHelper executeWithLockHelper = (RuntimeHelpers.ExecuteWithLockHelper)helper;
			Monitor.ReliableEnter(executeWithLockHelper.m_lockObject, ref executeWithLockHelper.m_tookLock);
			executeWithLockHelper.m_userCode(executeWithLockHelper.m_userState);
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x000BBAC8 File Offset: 0x000BAAC8
		[PrePrepareMethod]
		private static void ExitMonitorOnBackout(object helper, bool exceptionThrown)
		{
			RuntimeHelpers.ExecuteWithLockHelper executeWithLockHelper = (RuntimeHelpers.ExecuteWithLockHelper)helper;
			if (executeWithLockHelper.m_tookLock)
			{
				Monitor.Exit(executeWithLockHelper.m_lockObject);
			}
		}

		// Token: 0x04001CE0 RID: 7392
		private static RuntimeHelpers.TryCode s_EnterMonitor = new RuntimeHelpers.TryCode(RuntimeHelpers.EnterMonitorAndTryCode);

		// Token: 0x04001CE1 RID: 7393
		private static RuntimeHelpers.CleanupCode s_ExitMonitor = new RuntimeHelpers.CleanupCode(RuntimeHelpers.ExitMonitorOnBackout);

		// Token: 0x020005DC RID: 1500
		// (Invoke) Token: 0x060037CF RID: 14287
		public delegate void TryCode(object userData);

		// Token: 0x020005DD RID: 1501
		// (Invoke) Token: 0x060037D3 RID: 14291
		public delegate void CleanupCode(object userData, bool exceptionThrown);

		// Token: 0x020005DE RID: 1502
		private class ExecuteWithLockHelper
		{
			// Token: 0x060037D6 RID: 14294 RVA: 0x000BBB13 File Offset: 0x000BAB13
			internal ExecuteWithLockHelper(object lockObject, RuntimeHelpers.TryCode userCode, object userState)
			{
				this.m_lockObject = lockObject;
				this.m_userCode = userCode;
				this.m_userState = userState;
			}

			// Token: 0x04001CE2 RID: 7394
			internal object m_lockObject;

			// Token: 0x04001CE3 RID: 7395
			internal bool m_tookLock;

			// Token: 0x04001CE4 RID: 7396
			internal RuntimeHelpers.TryCode m_userCode;

			// Token: 0x04001CE5 RID: 7397
			internal object m_userState;
		}
	}
}
