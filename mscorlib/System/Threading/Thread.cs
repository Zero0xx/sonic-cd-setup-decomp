using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Threading
{
	// Token: 0x0200016B RID: 363
	[ComDefaultInterface(typeof(_Thread))]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class Thread : CriticalFinalizerObject, _Thread
	{
		// Token: 0x0600130F RID: 4879 RVA: 0x000348A3 File Offset: 0x000338A3
		public Thread(ThreadStart start)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			this.SetStartHelper(start, 0);
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x000348C1 File Offset: 0x000338C1
		public Thread(ThreadStart start, int maxStackSize)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			if (0 > maxStackSize)
			{
				throw new ArgumentOutOfRangeException("maxStackSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.SetStartHelper(start, maxStackSize);
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x000348F8 File Offset: 0x000338F8
		public Thread(ParameterizedThreadStart start)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			this.SetStartHelper(start, 0);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00034916 File Offset: 0x00033916
		public Thread(ParameterizedThreadStart start, int maxStackSize)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			if (0 > maxStackSize)
			{
				throw new ArgumentOutOfRangeException("maxStackSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.SetStartHelper(start, maxStackSize);
		}

		// Token: 0x06001313 RID: 4883
		[ComVisible(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern int GetHashCode();

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x0003494D File Offset: 0x0003394D
		public int ManagedThreadId
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.m_ManagedThreadId;
			}
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00034958 File Offset: 0x00033958
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Start()
		{
			this.StartupSetApartmentStateInternal();
			if (this.m_Delegate != null)
			{
				ThreadHelper threadHelper = (ThreadHelper)this.m_Delegate.Target;
				ExecutionContext executionContext = ExecutionContext.Capture();
				ExecutionContext.ClearSyncContext(executionContext);
				threadHelper.SetExecutionContextHelper(executionContext);
			}
			IPrincipal principal = CallContext.Principal;
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this.StartInternal(principal, ref stackCrawlMark);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x000349A9 File Offset: 0x000339A9
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public void Start(object parameter)
		{
			if (this.m_Delegate is ThreadStart)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ThreadWrongThreadStart"));
			}
			this.m_ThreadStartArg = parameter;
			this.Start();
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x000349D5 File Offset: 0x000339D5
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal ExecutionContext GetExecutionContextNoCreate()
		{
			return this.m_ExecutionContext;
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x000349DD File Offset: 0x000339DD
		public ExecutionContext ExecutionContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				if (this.m_ExecutionContext == null && this == Thread.CurrentThread)
				{
					this.m_ExecutionContext = new ExecutionContext();
					this.m_ExecutionContext.Thread = this;
				}
				return this.m_ExecutionContext;
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00034A0C File Offset: 0x00033A0C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void SetExecutionContext(ExecutionContext value)
		{
			this.m_ExecutionContext = value;
			if (value != null)
			{
				this.m_ExecutionContext.Thread = this;
			}
		}

		// Token: 0x0600131A RID: 4890
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StartInternal(IPrincipal principal, ref StackCrawlMark stackMark);

		// Token: 0x0600131B RID: 4891 RVA: 0x00034A24 File Offset: 0x00033A24
		[Obsolete("Thread.SetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000")]
		public void SetCompressedStack(CompressedStack stack)
		{
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ThreadAPIsNotSupported"));
		}

		// Token: 0x0600131C RID: 4892
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr SetAppDomainStack(SafeCompressedStackHandle csHandle);

		// Token: 0x0600131D RID: 4893
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RestoreAppDomainStack(IntPtr appDomainStack);

		// Token: 0x0600131E RID: 4894 RVA: 0x00034A35 File Offset: 0x00033A35
		[Obsolete("Thread.GetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000")]
		public CompressedStack GetCompressedStack()
		{
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ThreadAPIsNotSupported"));
		}

		// Token: 0x0600131F RID: 4895
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr InternalGetCurrentThread();

		// Token: 0x06001320 RID: 4896 RVA: 0x00034A46 File Offset: 0x00033A46
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Abort(object stateInfo)
		{
			this.AbortReason = stateInfo;
			this.AbortInternal();
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00034A55 File Offset: 0x00033A55
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Abort()
		{
			this.AbortInternal();
		}

		// Token: 0x06001322 RID: 4898
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AbortInternal();

		// Token: 0x06001323 RID: 4899 RVA: 0x00034A60 File Offset: 0x00033A60
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public static void ResetAbort()
		{
			Thread currentThread = Thread.CurrentThread;
			if ((currentThread.ThreadState & ThreadState.AbortRequested) == ThreadState.Running)
			{
				throw new ThreadStateException(Environment.GetResourceString("ThreadState_NoAbortRequested"));
			}
			currentThread.ResetAbortNative();
			currentThread.ClearAbortReason();
		}

		// Token: 0x06001324 RID: 4900
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResetAbortNative();

		// Token: 0x06001325 RID: 4901 RVA: 0x00034A9D File Offset: 0x00033A9D
		[Obsolete("Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Suspend()
		{
			this.SuspendInternal();
		}

		// Token: 0x06001326 RID: 4902
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SuspendInternal();

		// Token: 0x06001327 RID: 4903 RVA: 0x00034AA5 File Offset: 0x00033AA5
		[Obsolete("Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Resume()
		{
			this.ResumeInternal();
		}

		// Token: 0x06001328 RID: 4904
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResumeInternal();

		// Token: 0x06001329 RID: 4905 RVA: 0x00034AAD File Offset: 0x00033AAD
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Interrupt()
		{
			this.InterruptInternal();
		}

		// Token: 0x0600132A RID: 4906
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InterruptInternal();

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600132B RID: 4907 RVA: 0x00034AB5 File Offset: 0x00033AB5
		// (set) Token: 0x0600132C RID: 4908 RVA: 0x00034ABD File Offset: 0x00033ABD
		public ThreadPriority Priority
		{
			get
			{
				return (ThreadPriority)this.GetPriorityNative();
			}
			[HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true)]
			set
			{
				this.SetPriorityNative((int)value);
			}
		}

		// Token: 0x0600132D RID: 4909
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPriorityNative();

		// Token: 0x0600132E RID: 4910
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPriorityNative(int priority);

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600132F RID: 4911 RVA: 0x00034AC6 File Offset: 0x00033AC6
		public bool IsAlive
		{
			get
			{
				return this.IsAliveNative();
			}
		}

		// Token: 0x06001330 RID: 4912
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsAliveNative();

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06001331 RID: 4913 RVA: 0x00034ACE File Offset: 0x00033ACE
		public bool IsThreadPoolThread
		{
			get
			{
				return this.IsThreadpoolThreadNative();
			}
		}

		// Token: 0x06001332 RID: 4914
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsThreadpoolThreadNative();

		// Token: 0x06001333 RID: 4915
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void JoinInternal();

		// Token: 0x06001334 RID: 4916 RVA: 0x00034AD6 File Offset: 0x00033AD6
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public void Join()
		{
			this.JoinInternal();
		}

		// Token: 0x06001335 RID: 4917
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool JoinInternal(int millisecondsTimeout);

		// Token: 0x06001336 RID: 4918 RVA: 0x00034ADE File Offset: 0x00033ADE
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public bool Join(int millisecondsTimeout)
		{
			return this.JoinInternal(millisecondsTimeout);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00034AE8 File Offset: 0x00033AE8
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public bool Join(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return this.Join((int)num);
		}

		// Token: 0x06001338 RID: 4920
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SleepInternal(int millisecondsTimeout);

		// Token: 0x06001339 RID: 4921 RVA: 0x00034B29 File Offset: 0x00033B29
		public static void Sleep(int millisecondsTimeout)
		{
			Thread.SleepInternal(millisecondsTimeout);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00034B34 File Offset: 0x00033B34
		public static void Sleep(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			Thread.Sleep((int)num);
		}

		// Token: 0x0600133B RID: 4923
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SpinWaitInternal(int iterations);

		// Token: 0x0600133C RID: 4924 RVA: 0x00034B74 File Offset: 0x00033B74
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public static void SpinWait(int iterations)
		{
			Thread.SpinWaitInternal(iterations);
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x00034B7C File Offset: 0x00033B7C
		public static Thread CurrentThread
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				Thread thread = Thread.GetFastCurrentThreadNative();
				if (thread == null)
				{
					thread = Thread.GetCurrentThreadNative();
				}
				return thread;
			}
		}

		// Token: 0x0600133E RID: 4926
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Thread GetCurrentThreadNative();

		// Token: 0x0600133F RID: 4927
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Thread GetFastCurrentThreadNative();

		// Token: 0x06001340 RID: 4928 RVA: 0x00034B9C File Offset: 0x00033B9C
		private void SetStartHelper(Delegate start, int maxStackSize)
		{
			ThreadHelper @object = new ThreadHelper(start);
			if (start is ThreadStart)
			{
				this.SetStart(new ThreadStart(@object.ThreadStart), maxStackSize);
				return;
			}
			this.SetStart(new ParameterizedThreadStart(@object.ThreadStart), maxStackSize);
		}

		// Token: 0x06001341 RID: 4929
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetStart(Delegate start, int maxStackSize);

		// Token: 0x06001342 RID: 4930 RVA: 0x00034BE0 File Offset: 0x00033BE0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		~Thread()
		{
			this.InternalFinalize();
		}

		// Token: 0x06001343 RID: 4931
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalFinalize();

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x00034C0C File Offset: 0x00033C0C
		// (set) Token: 0x06001345 RID: 4933 RVA: 0x00034C14 File Offset: 0x00033C14
		public bool IsBackground
		{
			get
			{
				return this.IsBackgroundNative();
			}
			[HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true)]
			set
			{
				this.SetBackgroundNative(value);
			}
		}

		// Token: 0x06001346 RID: 4934
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsBackgroundNative();

		// Token: 0x06001347 RID: 4935
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetBackgroundNative(bool isBackground);

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x00034C1D File Offset: 0x00033C1D
		public ThreadState ThreadState
		{
			get
			{
				return (ThreadState)this.GetThreadStateNative();
			}
		}

		// Token: 0x06001349 RID: 4937
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetThreadStateNative();

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x00034C25 File Offset: 0x00033C25
		// (set) Token: 0x0600134B RID: 4939 RVA: 0x00034C2D File Offset: 0x00033C2D
		[Obsolete("The ApartmentState property has been deprecated.  Use GetApartmentState, SetApartmentState or TrySetApartmentState instead.", false)]
		public ApartmentState ApartmentState
		{
			get
			{
				return (ApartmentState)this.GetApartmentStateNative();
			}
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SelfAffectingThreading = true)]
			set
			{
				this.SetApartmentStateNative((int)value, true);
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00034C38 File Offset: 0x00033C38
		public ApartmentState GetApartmentState()
		{
			return (ApartmentState)this.GetApartmentStateNative();
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00034C40 File Offset: 0x00033C40
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SelfAffectingThreading = true)]
		public bool TrySetApartmentState(ApartmentState state)
		{
			return this.SetApartmentStateHelper(state, false);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00034C4C File Offset: 0x00033C4C
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SelfAffectingThreading = true)]
		public void SetApartmentState(ApartmentState state)
		{
			if (!this.SetApartmentStateHelper(state, true))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ApartmentStateSwitchFailed"));
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00034C78 File Offset: 0x00033C78
		private bool SetApartmentStateHelper(ApartmentState state, bool fireMDAOnMismatch)
		{
			ApartmentState apartmentState = (ApartmentState)this.SetApartmentStateNative((int)state, fireMDAOnMismatch);
			return (state == ApartmentState.Unknown && apartmentState == ApartmentState.MTA) || apartmentState == state;
		}

		// Token: 0x06001350 RID: 4944
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetApartmentStateNative();

		// Token: 0x06001351 RID: 4945
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int SetApartmentStateNative(int state, bool fireMDAOnMismatch);

		// Token: 0x06001352 RID: 4946
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int StartupSetApartmentStateInternal();

		// Token: 0x06001353 RID: 4947 RVA: 0x00034C9F File Offset: 0x00033C9F
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static LocalDataStoreSlot AllocateDataSlot()
		{
			return Thread.LocalDataStoreManager.AllocateDataSlot();
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00034CAB File Offset: 0x00033CAB
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			return Thread.LocalDataStoreManager.AllocateNamedDataSlot(name);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00034CB8 File Offset: 0x00033CB8
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			return Thread.LocalDataStoreManager.GetNamedDataSlot(name);
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00034CC5 File Offset: 0x00033CC5
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static void FreeNamedDataSlot(string name)
		{
			Thread.LocalDataStoreManager.FreeNamedDataSlot(name);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00034CD4 File Offset: 0x00033CD4
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static object GetData(LocalDataStoreSlot slot)
		{
			Thread.LocalDataStoreManager.ValidateSlot(slot);
			LocalDataStore domainLocalStore = Thread.GetDomainLocalStore();
			if (domainLocalStore == null)
			{
				return null;
			}
			return domainLocalStore.GetData(slot);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00034D00 File Offset: 0x00033D00
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static void SetData(LocalDataStoreSlot slot, object data)
		{
			LocalDataStore localDataStore = Thread.GetDomainLocalStore();
			if (localDataStore == null)
			{
				localDataStore = Thread.LocalDataStoreManager.CreateLocalDataStore();
				Thread.SetDomainLocalStore(localDataStore);
			}
			localDataStore.SetData(slot, data);
		}

		// Token: 0x06001359 RID: 4953
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LocalDataStore GetDomainLocalStore();

		// Token: 0x0600135A RID: 4954
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDomainLocalStore(LocalDataStore dls);

		// Token: 0x0600135B RID: 4955 RVA: 0x00034D2F File Offset: 0x00033D2F
		private static void RemoveDomainLocalStore(LocalDataStore dls)
		{
			if (dls != null)
			{
				Thread.LocalDataStoreManager.DeleteLocalDataStore(dls);
			}
		}

		// Token: 0x0600135C RID: 4956
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeGetSafeCulture(Thread t, int appDomainId, bool isUI, ref CultureInfo safeCulture);

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x00034D40 File Offset: 0x00033D40
		// (set) Token: 0x0600135E RID: 4958 RVA: 0x00034D78 File Offset: 0x00033D78
		public CultureInfo CurrentUICulture
		{
			get
			{
				if (this.m_CurrentUICulture == null)
				{
					return CultureInfo.UserDefaultUICulture;
				}
				CultureInfo cultureInfo = null;
				if (!Thread.nativeGetSafeCulture(this, Thread.GetDomainID(), true, ref cultureInfo) || cultureInfo == null)
				{
					return CultureInfo.UserDefaultUICulture;
				}
				return cultureInfo;
			}
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				CultureInfo.VerifyCultureName(value, true);
				if (!Thread.nativeSetThreadUILocale(value.LCID))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidResourceCultureName", new object[]
					{
						value.Name
					}));
				}
				value.StartCrossDomainTracking();
				this.m_CurrentUICulture = value;
			}
		}

		// Token: 0x0600135F RID: 4959
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeSetThreadUILocale(int LCID);

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x00034DD8 File Offset: 0x00033DD8
		// (set) Token: 0x06001361 RID: 4961 RVA: 0x00034E0F File Offset: 0x00033E0F
		public CultureInfo CurrentCulture
		{
			get
			{
				if (this.m_CurrentCulture == null)
				{
					return CultureInfo.UserDefaultCulture;
				}
				CultureInfo cultureInfo = null;
				if (!Thread.nativeGetSafeCulture(this, Thread.GetDomainID(), false, ref cultureInfo) || cultureInfo == null)
				{
					return CultureInfo.UserDefaultCulture;
				}
				return cultureInfo;
			}
			[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				CultureInfo.CheckNeutral(value);
				CultureInfo.nativeSetThreadLocale(value.LCID);
				value.StartCrossDomainTracking();
				this.m_CurrentCulture = value;
			}
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00034E40 File Offset: 0x00033E40
		private int ReserveSlot()
		{
			if (this.m_ThreadStaticsBuckets == null)
			{
				object[][] array = new object[1][];
				Thread.SetIsThreadStaticsArray(array);
				array[0] = new object[32];
				Thread.SetIsThreadStaticsArray(array[0]);
				int[] array2 = new int[array.Length * 32 / 32];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = -1;
				}
				array2[0] &= -2;
				array2[0] &= -3;
				this.m_ThreadStaticsBits = array2;
				this.m_ThreadStaticsBuckets = array;
				return 1;
			}
			int num = this.FindSlot();
			if (num == 0)
			{
				int num2 = this.m_ThreadStaticsBuckets.Length;
				int num3 = this.m_ThreadStaticsBits.Length;
				int num4 = this.m_ThreadStaticsBuckets.Length + 1;
				object[][] array3 = new object[num4][];
				Thread.SetIsThreadStaticsArray(array3);
				int num5 = num4 * 32 / 32;
				int[] array4 = new int[num5];
				Array.Copy(this.m_ThreadStaticsBuckets, array3, this.m_ThreadStaticsBuckets.Length);
				for (int j = num2; j < num4; j++)
				{
					array3[j] = new object[32];
					Thread.SetIsThreadStaticsArray(array3[j]);
				}
				Array.Copy(this.m_ThreadStaticsBits, array4, this.m_ThreadStaticsBits.Length);
				for (int k = num3; k < num5; k++)
				{
					array4[k] = -1;
				}
				array4[num3] &= -2;
				this.m_ThreadStaticsBits = array4;
				this.m_ThreadStaticsBuckets = array3;
				return num2 * 32;
			}
			return num;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00034FB8 File Offset: 0x00033FB8
		private int FindSlot()
		{
			int num = 0;
			bool flag = false;
			if (this.m_ThreadStaticsBits.Length != 0 && this.m_ThreadStaticsBits.Length != this.m_ThreadStaticsBuckets.Length * 32 / 32)
			{
				return 0;
			}
			int i;
			for (i = 0; i < this.m_ThreadStaticsBits.Length; i++)
			{
				int num2 = this.m_ThreadStaticsBits[i];
				if (num2 != 0)
				{
					if ((num2 & 65535) != 0)
					{
						num2 &= 65535;
					}
					else
					{
						num2 = (num2 >> 16 & 65535);
						num += 16;
					}
					if ((num2 & 255) != 0)
					{
						num2 &= 255;
					}
					else
					{
						num += 8;
						num2 = (num2 >> 8 & 255);
					}
					int j;
					for (j = 0; j < 8; j++)
					{
						if ((num2 & 1 << j) != 0)
						{
							flag = true;
							break;
						}
					}
					num += j;
					this.m_ThreadStaticsBits[i] &= ~(1 << num);
					break;
				}
			}
			if (flag)
			{
				num += 32 * i;
			}
			return num;
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x000350AB File Offset: 0x000340AB
		public static Context CurrentContext
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return Thread.CurrentThread.GetCurrentContextInternal();
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x000350B7 File Offset: 0x000340B7
		internal Context GetCurrentContextInternal()
		{
			if (this.m_Context == null)
			{
				this.m_Context = Context.DefaultContext;
			}
			return this.m_Context;
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x000350D2 File Offset: 0x000340D2
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		internal LogicalCallContext GetLogicalCallContext()
		{
			return this.ExecutionContext.LogicalCallContext;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x000350E0 File Offset: 0x000340E0
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			LogicalCallContext logicalCallContext = this.ExecutionContext.LogicalCallContext;
			this.ExecutionContext.LogicalCallContext = callCtx;
			return logicalCallContext;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00035106 File Offset: 0x00034106
		internal IllogicalCallContext GetIllogicalCallContext()
		{
			return this.ExecutionContext.IllogicalCallContext;
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x00035114 File Offset: 0x00034114
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x00035164 File Offset: 0x00034164
		public static IPrincipal CurrentPrincipal
		{
			get
			{
				IPrincipal result;
				lock (Thread.CurrentThread)
				{
					IPrincipal principal = CallContext.Principal;
					if (principal == null)
					{
						principal = Thread.GetDomain().GetThreadPrincipal();
						CallContext.Principal = principal;
					}
					result = principal;
				}
				return result;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
			set
			{
				CallContext.Principal = value;
			}
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0003516C File Offset: 0x0003416C
		private void SetPrincipalInternal(IPrincipal principal)
		{
			this.GetLogicalCallContext().SecurityData.Principal = principal;
		}

		// Token: 0x0600136C RID: 4972
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Context GetContextInternal(IntPtr id);

		// Token: 0x0600136D RID: 4973
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object InternalCrossContextCallback(Context ctx, IntPtr ctxID, int appDomainID, InternalCrossContextDelegate ftnToCall, object[] args);

		// Token: 0x0600136E RID: 4974 RVA: 0x0003517F File Offset: 0x0003417F
		internal object InternalCrossContextCallback(Context ctx, InternalCrossContextDelegate ftnToCall, object[] args)
		{
			return this.InternalCrossContextCallback(ctx, ctx.InternalContextID, 0, ftnToCall, args);
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00035191 File Offset: 0x00034191
		private static object CompleteCrossContextCallback(InternalCrossContextDelegate ftnToCall, object[] args)
		{
			return ftnToCall(args);
		}

		// Token: 0x06001370 RID: 4976
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain GetDomainInternal();

		// Token: 0x06001371 RID: 4977
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain GetFastDomainInternal();

		// Token: 0x06001372 RID: 4978 RVA: 0x0003519C File Offset: 0x0003419C
		public static AppDomain GetDomain()
		{
			if (Thread.CurrentThread.m_Context == null)
			{
				AppDomain appDomain = Thread.GetFastDomainInternal();
				if (appDomain == null)
				{
					appDomain = Thread.GetDomainInternal();
				}
				return appDomain;
			}
			return Thread.CurrentThread.m_Context.AppDomain;
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x000351D5 File Offset: 0x000341D5
		public static int GetDomainID()
		{
			return Thread.GetDomain().GetId();
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x000351E1 File Offset: 0x000341E1
		// (set) Token: 0x06001375 RID: 4981 RVA: 0x000351EC File Offset: 0x000341EC
		public string Name
		{
			get
			{
				return this.m_Name;
			}
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			set
			{
				lock (this)
				{
					if (this.m_Name != null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_WriteOnce"));
					}
					this.m_Name = value;
					Thread.InformThreadNameChangeEx(this, this.m_Name);
				}
			}
		}

		// Token: 0x06001376 RID: 4982
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InformThreadNameChangeEx(Thread t, string name);

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x00035248 File Offset: 0x00034248
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x00035284 File Offset: 0x00034284
		internal object AbortReason
		{
			get
			{
				object result = null;
				try
				{
					result = this.GetAbortReason();
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ExceptionStateCrossAppDomain"), innerException);
				}
				return result;
			}
			set
			{
				this.SetAbortReason(value);
			}
		}

		// Token: 0x06001379 RID: 4985
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginCriticalRegion();

		// Token: 0x0600137A RID: 4986
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndCriticalRegion();

		// Token: 0x0600137B RID: 4987
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginThreadAffinity();

		// Token: 0x0600137C RID: 4988
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndThreadAffinity();

		// Token: 0x0600137D RID: 4989 RVA: 0x00035290 File Offset: 0x00034290
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static byte VolatileRead(ref byte address)
		{
			byte result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000352A8 File Offset: 0x000342A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static short VolatileRead(ref short address)
		{
			short result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000352C0 File Offset: 0x000342C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static int VolatileRead(ref int address)
		{
			int result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000352D8 File Offset: 0x000342D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static long VolatileRead(ref long address)
		{
			long result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x000352F0 File Offset: 0x000342F0
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static sbyte VolatileRead(ref sbyte address)
		{
			sbyte result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00035308 File Offset: 0x00034308
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ushort VolatileRead(ref ushort address)
		{
			ushort result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00035320 File Offset: 0x00034320
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint VolatileRead(ref uint address)
		{
			uint result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00035338 File Offset: 0x00034338
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IntPtr VolatileRead(ref IntPtr address)
		{
			IntPtr result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00035354 File Offset: 0x00034354
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static UIntPtr VolatileRead(ref UIntPtr address)
		{
			UIntPtr result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00035370 File Offset: 0x00034370
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ulong VolatileRead(ref ulong address)
		{
			ulong result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00035388 File Offset: 0x00034388
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static float VolatileRead(ref float address)
		{
			float result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x000353A0 File Offset: 0x000343A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static double VolatileRead(ref double address)
		{
			double result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x000353B8 File Offset: 0x000343B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static object VolatileRead(ref object address)
		{
			object result = address;
			Thread.MemoryBarrier();
			return result;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000353CE File Offset: 0x000343CE
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref byte address, byte value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x000353D8 File Offset: 0x000343D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref short address, short value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x000353E2 File Offset: 0x000343E2
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref int address, int value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x000353EC File Offset: 0x000343EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref long address, long value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x000353F6 File Offset: 0x000343F6
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref sbyte address, sbyte value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00035400 File Offset: 0x00034400
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref ushort address, ushort value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0003540A File Offset: 0x0003440A
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref uint address, uint value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00035414 File Offset: 0x00034414
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref IntPtr address, IntPtr value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00035422 File Offset: 0x00034422
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref UIntPtr address, UIntPtr value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00035430 File Offset: 0x00034430
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref ulong address, ulong value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0003543A File Offset: 0x0003443A
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref float address, float value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00035444 File Offset: 0x00034444
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref double address, double value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0003544E File Offset: 0x0003444E
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref object address, object value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001397 RID: 5015
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemoryBarrier();

		// Token: 0x06001398 RID: 5016
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIsThreadStaticsArray(object o);

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x00035458 File Offset: 0x00034458
		private static LocalDataStoreMgr LocalDataStoreManager
		{
			get
			{
				if (Thread.s_LocalDataStoreMgr == null)
				{
					lock (Thread.s_SyncObject)
					{
						if (Thread.s_LocalDataStoreMgr == null)
						{
							Thread.s_LocalDataStoreMgr = new LocalDataStoreMgr();
						}
					}
				}
				return Thread.s_LocalDataStoreMgr;
			}
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x000354A8 File Offset: 0x000344A8
		void _Thread.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000354AF File Offset: 0x000344AF
		void _Thread.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000354B6 File Offset: 0x000344B6
		void _Thread.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x000354BD File Offset: 0x000344BD
		void _Thread.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600139E RID: 5022
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetAbortReason(object o);

		// Token: 0x0600139F RID: 5023
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetAbortReason();

		// Token: 0x060013A0 RID: 5024
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void ClearAbortReason();

		// Token: 0x0400069B RID: 1691
		private const int STATICS_BUCKET_SIZE = 32;

		// Token: 0x0400069C RID: 1692
		private Context m_Context;

		// Token: 0x0400069D RID: 1693
		private ExecutionContext m_ExecutionContext;

		// Token: 0x0400069E RID: 1694
		private string m_Name;

		// Token: 0x0400069F RID: 1695
		private Delegate m_Delegate;

		// Token: 0x040006A0 RID: 1696
		private object[][] m_ThreadStaticsBuckets;

		// Token: 0x040006A1 RID: 1697
		private int[] m_ThreadStaticsBits;

		// Token: 0x040006A2 RID: 1698
		private CultureInfo m_CurrentCulture;

		// Token: 0x040006A3 RID: 1699
		private CultureInfo m_CurrentUICulture;

		// Token: 0x040006A4 RID: 1700
		private object m_ThreadStartArg;

		// Token: 0x040006A5 RID: 1701
		private IntPtr DONT_USE_InternalThread;

		// Token: 0x040006A6 RID: 1702
		private int m_Priority;

		// Token: 0x040006A7 RID: 1703
		private int m_ManagedThreadId;

		// Token: 0x040006A8 RID: 1704
		private static LocalDataStoreMgr s_LocalDataStoreMgr = null;

		// Token: 0x040006A9 RID: 1705
		private static object s_SyncObject = new object();
	}
}
