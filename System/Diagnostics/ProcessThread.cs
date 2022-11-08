using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x02000788 RID: 1928
	[Designer("System.Diagnostics.Design.ProcessThreadDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[HostProtection(SecurityAction.LinkDemand, SelfAffectingProcessMgmt = true, SelfAffectingThreading = true)]
	public class ProcessThread : Component
	{
		// Token: 0x06003B8D RID: 15245 RVA: 0x000FDB62 File Offset: 0x000FCB62
		internal ProcessThread(bool isRemoteMachine, ThreadInfo threadInfo)
		{
			this.isRemoteMachine = isRemoteMachine;
			this.threadInfo = threadInfo;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06003B8E RID: 15246 RVA: 0x000FDB7E File Offset: 0x000FCB7E
		[MonitoringDescription("ThreadBasePriority")]
		public int BasePriority
		{
			get
			{
				return this.threadInfo.basePriority;
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06003B8F RID: 15247 RVA: 0x000FDB8B File Offset: 0x000FCB8B
		[MonitoringDescription("ThreadCurrentPriority")]
		public int CurrentPriority
		{
			get
			{
				return this.threadInfo.currentPriority;
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06003B90 RID: 15248 RVA: 0x000FDB98 File Offset: 0x000FCB98
		[MonitoringDescription("ThreadId")]
		public int Id
		{
			get
			{
				return this.threadInfo.threadId;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (set) Token: 0x06003B91 RID: 15249 RVA: 0x000FDBA8 File Offset: 0x000FCBA8
		[Browsable(false)]
		public int IdealProcessor
		{
			set
			{
				SafeThreadHandle handle = null;
				try
				{
					handle = this.OpenThreadHandle(32);
					if (NativeMethods.SetThreadIdealProcessor(handle, value) < 0)
					{
						throw new Win32Exception();
					}
				}
				finally
				{
					ProcessThread.CloseThreadHandle(handle);
				}
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06003B92 RID: 15250 RVA: 0x000FDBEC File Offset: 0x000FCBEC
		// (set) Token: 0x06003B93 RID: 15251 RVA: 0x000FDC50 File Offset: 0x000FCC50
		[MonitoringDescription("ThreadPriorityBoostEnabled")]
		public bool PriorityBoostEnabled
		{
			get
			{
				if (!this.havePriorityBoostEnabled)
				{
					SafeThreadHandle handle = null;
					try
					{
						handle = this.OpenThreadHandle(64);
						bool flag = false;
						if (!NativeMethods.GetThreadPriorityBoost(handle, out flag))
						{
							throw new Win32Exception();
						}
						this.priorityBoostEnabled = !flag;
						this.havePriorityBoostEnabled = true;
					}
					finally
					{
						ProcessThread.CloseThreadHandle(handle);
					}
				}
				return this.priorityBoostEnabled;
			}
			set
			{
				SafeThreadHandle handle = null;
				try
				{
					handle = this.OpenThreadHandle(32);
					if (!NativeMethods.SetThreadPriorityBoost(handle, !value))
					{
						throw new Win32Exception();
					}
					this.priorityBoostEnabled = value;
					this.havePriorityBoostEnabled = true;
				}
				finally
				{
					ProcessThread.CloseThreadHandle(handle);
				}
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06003B94 RID: 15252 RVA: 0x000FDCA4 File Offset: 0x000FCCA4
		// (set) Token: 0x06003B95 RID: 15253 RVA: 0x000FDD08 File Offset: 0x000FCD08
		[MonitoringDescription("ThreadPriorityLevel")]
		public ThreadPriorityLevel PriorityLevel
		{
			get
			{
				if (!this.havePriorityLevel)
				{
					SafeThreadHandle handle = null;
					try
					{
						handle = this.OpenThreadHandle(64);
						int threadPriority = NativeMethods.GetThreadPriority(handle);
						if (threadPriority == 2147483647)
						{
							throw new Win32Exception();
						}
						this.priorityLevel = (ThreadPriorityLevel)threadPriority;
						this.havePriorityLevel = true;
					}
					finally
					{
						ProcessThread.CloseThreadHandle(handle);
					}
				}
				return this.priorityLevel;
			}
			set
			{
				SafeThreadHandle handle = null;
				try
				{
					handle = this.OpenThreadHandle(32);
					if (!NativeMethods.SetThreadPriority(handle, (int)value))
					{
						throw new Win32Exception();
					}
					this.priorityLevel = value;
				}
				finally
				{
					ProcessThread.CloseThreadHandle(handle);
				}
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06003B96 RID: 15254 RVA: 0x000FDD50 File Offset: 0x000FCD50
		[MonitoringDescription("ThreadPrivilegedProcessorTime")]
		public TimeSpan PrivilegedProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().PrivilegedProcessorTime;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06003B97 RID: 15255 RVA: 0x000FDD64 File Offset: 0x000FCD64
		[MonitoringDescription("ThreadStartAddress")]
		public IntPtr StartAddress
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.threadInfo.startAddress;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003B98 RID: 15256 RVA: 0x000FDD78 File Offset: 0x000FCD78
		[MonitoringDescription("ThreadStartTime")]
		public DateTime StartTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().StartTime;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06003B99 RID: 15257 RVA: 0x000FDD8C File Offset: 0x000FCD8C
		[MonitoringDescription("ThreadThreadState")]
		public ThreadState ThreadState
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.threadInfo.threadState;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003B9A RID: 15258 RVA: 0x000FDDA0 File Offset: 0x000FCDA0
		[MonitoringDescription("ThreadTotalProcessorTime")]
		public TimeSpan TotalProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().TotalProcessorTime;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003B9B RID: 15259 RVA: 0x000FDDB4 File Offset: 0x000FCDB4
		[MonitoringDescription("ThreadUserProcessorTime")]
		public TimeSpan UserProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().UserProcessorTime;
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06003B9C RID: 15260 RVA: 0x000FDDC8 File Offset: 0x000FCDC8
		[MonitoringDescription("ThreadWaitReason")]
		public ThreadWaitReason WaitReason
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				if (this.threadInfo.threadState != ThreadState.Wait)
				{
					throw new InvalidOperationException(SR.GetString("WaitReasonUnavailable"));
				}
				return this.threadInfo.threadWaitReason;
			}
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x000FDDFA File Offset: 0x000FCDFA
		private static void CloseThreadHandle(SafeThreadHandle handle)
		{
			if (handle != null)
			{
				handle.Close();
			}
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x000FDE08 File Offset: 0x000FCE08
		private void EnsureState(ProcessThread.State state)
		{
			if ((state & ProcessThread.State.IsLocal) != (ProcessThread.State)0 && this.isRemoteMachine)
			{
				throw new NotSupportedException(SR.GetString("NotSupportedRemoteThread"));
			}
			if ((state & ProcessThread.State.IsNt) != (ProcessThread.State)0 && Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x000FDE54 File Offset: 0x000FCE54
		private SafeThreadHandle OpenThreadHandle(int access)
		{
			this.EnsureState(ProcessThread.State.IsLocal);
			return ProcessManager.OpenThread(this.threadInfo.threadId, access);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x000FDE6E File Offset: 0x000FCE6E
		public void ResetIdealProcessor()
		{
			this.IdealProcessor = 32;
		}

		// Token: 0x17000E09 RID: 3593
		// (set) Token: 0x06003BA1 RID: 15265 RVA: 0x000FDE78 File Offset: 0x000FCE78
		[Browsable(false)]
		public IntPtr ProcessorAffinity
		{
			set
			{
				SafeThreadHandle handle = null;
				try
				{
					handle = this.OpenThreadHandle(96);
					if (NativeMethods.SetThreadAffinityMask(handle, new HandleRef(this, value)) == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
				finally
				{
					ProcessThread.CloseThreadHandle(handle);
				}
			}
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x000FDEC8 File Offset: 0x000FCEC8
		private ProcessThreadTimes GetThreadTimes()
		{
			ProcessThreadTimes processThreadTimes = new ProcessThreadTimes();
			SafeThreadHandle handle = null;
			try
			{
				handle = this.OpenThreadHandle(64);
				if (!NativeMethods.GetThreadTimes(handle, out processThreadTimes.create, out processThreadTimes.exit, out processThreadTimes.kernel, out processThreadTimes.user))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				ProcessThread.CloseThreadHandle(handle);
			}
			return processThreadTimes;
		}

		// Token: 0x04003437 RID: 13367
		private ThreadInfo threadInfo;

		// Token: 0x04003438 RID: 13368
		private bool isRemoteMachine;

		// Token: 0x04003439 RID: 13369
		private bool priorityBoostEnabled;

		// Token: 0x0400343A RID: 13370
		private bool havePriorityBoostEnabled;

		// Token: 0x0400343B RID: 13371
		private ThreadPriorityLevel priorityLevel;

		// Token: 0x0400343C RID: 13372
		private bool havePriorityLevel;

		// Token: 0x02000789 RID: 1929
		private enum State
		{
			// Token: 0x0400343E RID: 13374
			IsLocal = 2,
			// Token: 0x0400343F RID: 13375
			IsNt = 4
		}
	}
}
