using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x02000772 RID: 1906
	[Designer("System.Diagnostics.Design.ProcessDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("Exited")]
	[DefaultProperty("StartInfo")]
	[MonitoringDescription("ProcessDesc")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true, Synchronization = true, ExternalProcessMgmt = true, SelfAffectingProcessMgmt = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class Process : Component
	{
		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06003A9F RID: 15007 RVA: 0x000F9467 File Offset: 0x000F8467
		// (remove) Token: 0x06003AA0 RID: 15008 RVA: 0x000F9480 File Offset: 0x000F8480
		[MonitoringDescription("ProcessAssociated")]
		[Browsable(true)]
		public event DataReceivedEventHandler OutputDataReceived;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06003AA1 RID: 15009 RVA: 0x000F9499 File Offset: 0x000F8499
		// (remove) Token: 0x06003AA2 RID: 15010 RVA: 0x000F94B2 File Offset: 0x000F84B2
		[Browsable(true)]
		[MonitoringDescription("ProcessAssociated")]
		public event DataReceivedEventHandler ErrorDataReceived;

		// Token: 0x06003AA3 RID: 15011 RVA: 0x000F94CB File Offset: 0x000F84CB
		public Process()
		{
			this.machineName = ".";
			this.outputStreamReadMode = Process.StreamReadMode.undefined;
			this.errorStreamReadMode = Process.StreamReadMode.undefined;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x000F94EC File Offset: 0x000F84EC
		private Process(string machineName, bool isRemoteMachine, int processId, ProcessInfo processInfo)
		{
			this.processInfo = processInfo;
			this.machineName = machineName;
			this.isRemoteMachine = isRemoteMachine;
			this.processId = processId;
			this.haveProcessId = true;
			this.outputStreamReadMode = Process.StreamReadMode.undefined;
			this.errorStreamReadMode = Process.StreamReadMode.undefined;
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06003AA5 RID: 15013 RVA: 0x000F9526 File Offset: 0x000F8526
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessAssociated")]
		[Browsable(false)]
		private bool Associated
		{
			get
			{
				return this.haveProcessId || this.haveProcessHandle;
			}
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x000F9538 File Offset: 0x000F8538
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessBasePriority")]
		public int BasePriority
		{
			get
			{
				this.EnsureState(Process.State.HaveProcessInfo);
				return this.processInfo.basePriority;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06003AA7 RID: 15015 RVA: 0x000F954C File Offset: 0x000F854C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("ProcessExitCode")]
		public int ExitCode
		{
			get
			{
				this.EnsureState(Process.State.Exited);
				return this.exitCode;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x000F955C File Offset: 0x000F855C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("ProcessTerminated")]
		public bool HasExited
		{
			get
			{
				if (!this.exited)
				{
					this.EnsureState(Process.State.Associated);
					SafeProcessHandle safeProcessHandle = null;
					try
					{
						safeProcessHandle = this.GetProcessHandle(1049600, false);
						int num;
						if (safeProcessHandle.IsInvalid)
						{
							this.exited = true;
						}
						else if (NativeMethods.GetExitCodeProcess(safeProcessHandle, out num) && num != 259)
						{
							this.exited = true;
							this.exitCode = num;
						}
						else
						{
							if (!this.signaled)
							{
								ProcessWaitHandle processWaitHandle = null;
								try
								{
									processWaitHandle = new ProcessWaitHandle(safeProcessHandle);
									this.signaled = processWaitHandle.WaitOne(0, false);
								}
								finally
								{
									if (processWaitHandle != null)
									{
										processWaitHandle.Close();
									}
								}
							}
							if (this.signaled)
							{
								if (!NativeMethods.GetExitCodeProcess(safeProcessHandle, out num))
								{
									throw new Win32Exception();
								}
								this.exited = true;
								this.exitCode = num;
							}
						}
					}
					finally
					{
						this.ReleaseProcessHandle(safeProcessHandle);
					}
					if (this.exited)
					{
						this.RaiseOnExited();
					}
				}
				return this.exited;
			}
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x000F964C File Offset: 0x000F864C
		private ProcessThreadTimes GetProcessTimes()
		{
			ProcessThreadTimes processThreadTimes = new ProcessThreadTimes();
			SafeProcessHandle safeProcessHandle = null;
			try
			{
				safeProcessHandle = this.GetProcessHandle(1024, false);
				if (safeProcessHandle.IsInvalid)
				{
					throw new InvalidOperationException(SR.GetString("ProcessHasExited", new object[]
					{
						this.processId.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (!NativeMethods.GetProcessTimes(safeProcessHandle, out processThreadTimes.create, out processThreadTimes.exit, out processThreadTimes.kernel, out processThreadTimes.user))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				this.ReleaseProcessHandle(safeProcessHandle);
			}
			return processThreadTimes;
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06003AAA RID: 15018 RVA: 0x000F96E4 File Offset: 0x000F86E4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessExitTime")]
		public DateTime ExitTime
		{
			get
			{
				if (!this.haveExitTime)
				{
					this.EnsureState((Process.State)20);
					this.exitTime = this.GetProcessTimes().ExitTime;
					this.haveExitTime = true;
				}
				return this.exitTime;
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06003AAB RID: 15019 RVA: 0x000F9714 File Offset: 0x000F8714
		[MonitoringDescription("ProcessHandle")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IntPtr Handle
		{
			get
			{
				this.EnsureState(Process.State.Associated);
				return this.OpenProcessHandle().DangerousGetHandle();
			}
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06003AAC RID: 15020 RVA: 0x000F9729 File Offset: 0x000F8729
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessHandleCount")]
		public int HandleCount
		{
			get
			{
				this.EnsureState(Process.State.HaveProcessInfo);
				return this.processInfo.handleCount;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06003AAD RID: 15021 RVA: 0x000F973D File Offset: 0x000F873D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessId")]
		public int Id
		{
			get
			{
				this.EnsureState(Process.State.HaveId);
				return this.processId;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003AAE RID: 15022 RVA: 0x000F974C File Offset: 0x000F874C
		[MonitoringDescription("ProcessMachineName")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string MachineName
		{
			get
			{
				this.EnsureState(Process.State.Associated);
				return this.machineName;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06003AAF RID: 15023 RVA: 0x000F975C File Offset: 0x000F875C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessMainWindowHandle")]
		public IntPtr MainWindowHandle
		{
			get
			{
				if (!this.haveMainWindow)
				{
					this.EnsureState((Process.State)10);
					this.mainWindowHandle = ProcessManager.GetMainWindowHandle(this.processInfo);
					this.haveMainWindow = true;
				}
				return this.mainWindowHandle;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06003AB0 RID: 15024 RVA: 0x000F978C File Offset: 0x000F878C
		[MonitoringDescription("ProcessMainWindowTitle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string MainWindowTitle
		{
			get
			{
				if (this.mainWindowTitle == null)
				{
					IntPtr intPtr = this.MainWindowHandle;
					if (intPtr == (IntPtr)0)
					{
						this.mainWindowTitle = string.Empty;
					}
					else
					{
						int capacity = NativeMethods.GetWindowTextLength(new HandleRef(this, intPtr)) * 2;
						StringBuilder stringBuilder = new StringBuilder(capacity);
						NativeMethods.GetWindowText(new HandleRef(this, intPtr), stringBuilder, stringBuilder.Capacity);
						this.mainWindowTitle = stringBuilder.ToString();
					}
				}
				return this.mainWindowTitle;
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x000F9800 File Offset: 0x000F8800
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("ProcessMainModule")]
		public ProcessModule MainModule
		{
			get
			{
				if (this.OperatingSystem.Platform == PlatformID.Win32NT)
				{
					this.EnsureState((Process.State)3);
					ModuleInfo firstModuleInfo = NtProcessManager.GetFirstModuleInfo(this.processId);
					return new ProcessModule(firstModuleInfo);
				}
				ProcessModuleCollection processModuleCollection = this.Modules;
				this.EnsureState(Process.State.HaveProcessInfo);
				foreach (object obj in processModuleCollection)
				{
					ProcessModule processModule = (ProcessModule)obj;
					if (processModule.moduleInfo.Id == this.processInfo.mainModuleId)
					{
						return processModule;
					}
				}
				return null;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x000F98AC File Offset: 0x000F88AC
		// (set) Token: 0x06003AB3 RID: 15027 RVA: 0x000F98BA File Offset: 0x000F88BA
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessMaxWorkingSet")]
		public IntPtr MaxWorkingSet
		{
			get
			{
				this.EnsureWorkingSetLimits();
				return this.maxWorkingSet;
			}
			set
			{
				this.SetWorkingSetLimits(null, value);
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06003AB4 RID: 15028 RVA: 0x000F98C9 File Offset: 0x000F88C9
		// (set) Token: 0x06003AB5 RID: 15029 RVA: 0x000F98D7 File Offset: 0x000F88D7
		[MonitoringDescription("ProcessMinWorkingSet")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IntPtr MinWorkingSet
		{
			get
			{
				this.EnsureWorkingSetLimits();
				return this.minWorkingSet;
			}
			set
			{
				this.SetWorkingSetLimits(value, null);
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06003AB6 RID: 15030 RVA: 0x000F98E8 File Offset: 0x000F88E8
		[MonitoringDescription("ProcessModules")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ProcessModuleCollection Modules
		{
			get
			{
				if (this.modules == null)
				{
					this.EnsureState((Process.State)3);
					ModuleInfo[] moduleInfos = ProcessManager.GetModuleInfos(this.processId);
					ProcessModule[] array = new ProcessModule[moduleInfos.Length];
					for (int i = 0; i < moduleInfos.Length; i++)
					{
						array[i] = new ProcessModule(moduleInfos[i]);
					}
					ProcessModuleCollection processModuleCollection = new ProcessModuleCollection(array);
					this.modules = processModuleCollection;
				}
				return this.modules;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x000F9946 File Offset: 0x000F8946
		[MonitoringDescription("ProcessNonpagedSystemMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.NonpagedSystemMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int NonpagedSystemMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.poolNonpagedBytes;
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x000F995C File Offset: 0x000F895C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessNonpagedSystemMemorySize")]
		public long NonpagedSystemMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.poolNonpagedBytes;
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x000F9971 File Offset: 0x000F8971
		[MonitoringDescription("ProcessPagedMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PagedMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PagedMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.pageFileBytes;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06003ABA RID: 15034 RVA: 0x000F9987 File Offset: 0x000F8987
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessPagedMemorySize")]
		public long PagedMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.pageFileBytes;
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06003ABB RID: 15035 RVA: 0x000F999C File Offset: 0x000F899C
		[MonitoringDescription("ProcessPagedSystemMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PagedSystemMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PagedSystemMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.poolPagedBytes;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06003ABC RID: 15036 RVA: 0x000F99B2 File Offset: 0x000F89B2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessPagedSystemMemorySize")]
		public long PagedSystemMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.poolPagedBytes;
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06003ABD RID: 15037 RVA: 0x000F99C7 File Offset: 0x000F89C7
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PeakPagedMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[MonitoringDescription("ProcessPeakPagedMemorySize")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PeakPagedMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.pageFileBytesPeak;
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x000F99DD File Offset: 0x000F89DD
		[ComVisible(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessPeakPagedMemorySize")]
		public long PeakPagedMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.pageFileBytesPeak;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06003ABF RID: 15039 RVA: 0x000F99F2 File Offset: 0x000F89F2
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PeakWorkingSet64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[MonitoringDescription("ProcessPeakWorkingSet")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PeakWorkingSet
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.workingSetPeak;
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06003AC0 RID: 15040 RVA: 0x000F9A08 File Offset: 0x000F8A08
		[ComVisible(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessPeakWorkingSet")]
		public long PeakWorkingSet64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.workingSetPeak;
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x000F9A1D File Offset: 0x000F8A1D
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PeakVirtualMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[MonitoringDescription("ProcessPeakVirtualMemorySize")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PeakVirtualMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.virtualBytesPeak;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x000F9A33 File Offset: 0x000F8A33
		[ComVisible(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessPeakVirtualMemorySize")]
		public long PeakVirtualMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.virtualBytesPeak;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06003AC3 RID: 15043 RVA: 0x000F9A48 File Offset: 0x000F8A48
		private OperatingSystem OperatingSystem
		{
			get
			{
				if (this.operatingSystem == null)
				{
					this.operatingSystem = Environment.OSVersion;
				}
				return this.operatingSystem;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06003AC4 RID: 15044 RVA: 0x000F9A64 File Offset: 0x000F8A64
		// (set) Token: 0x06003AC5 RID: 15045 RVA: 0x000F9AD4 File Offset: 0x000F8AD4
		[MonitoringDescription("ProcessPriorityBoostEnabled")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PriorityBoostEnabled
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				if (!this.havePriorityBoostEnabled)
				{
					SafeProcessHandle handle = null;
					try
					{
						handle = this.GetProcessHandle(1024);
						bool flag = false;
						if (!NativeMethods.GetProcessPriorityBoost(handle, out flag))
						{
							throw new Win32Exception();
						}
						this.priorityBoostEnabled = !flag;
						this.havePriorityBoostEnabled = true;
					}
					finally
					{
						this.ReleaseProcessHandle(handle);
					}
				}
				return this.priorityBoostEnabled;
			}
			set
			{
				this.EnsureState(Process.State.IsNt);
				SafeProcessHandle handle = null;
				try
				{
					handle = this.GetProcessHandle(512);
					if (!NativeMethods.SetProcessPriorityBoost(handle, !value))
					{
						throw new Win32Exception();
					}
					this.priorityBoostEnabled = value;
					this.havePriorityBoostEnabled = true;
				}
				finally
				{
					this.ReleaseProcessHandle(handle);
				}
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x000F9B30 File Offset: 0x000F8B30
		// (set) Token: 0x06003AC7 RID: 15047 RVA: 0x000F9B94 File Offset: 0x000F8B94
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessPriorityClass")]
		public ProcessPriorityClass PriorityClass
		{
			get
			{
				if (!this.havePriorityClass)
				{
					SafeProcessHandle handle = null;
					try
					{
						handle = this.GetProcessHandle(1024);
						int num = NativeMethods.GetPriorityClass(handle);
						if (num == 0)
						{
							throw new Win32Exception();
						}
						this.priorityClass = (ProcessPriorityClass)num;
						this.havePriorityClass = true;
					}
					finally
					{
						this.ReleaseProcessHandle(handle);
					}
				}
				return this.priorityClass;
			}
			set
			{
				if (!Enum.IsDefined(typeof(ProcessPriorityClass), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ProcessPriorityClass));
				}
				if ((value & (ProcessPriorityClass)49152) != (ProcessPriorityClass)0 && (this.OperatingSystem.Platform != PlatformID.Win32NT || this.OperatingSystem.Version.Major < 5))
				{
					throw new PlatformNotSupportedException(SR.GetString("PriorityClassNotSupported"), null);
				}
				SafeProcessHandle handle = null;
				try
				{
					handle = this.GetProcessHandle(512);
					if (!NativeMethods.SetPriorityClass(handle, (int)value))
					{
						throw new Win32Exception();
					}
					this.priorityClass = value;
					this.havePriorityClass = true;
				}
				finally
				{
					this.ReleaseProcessHandle(handle);
				}
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x000F9C50 File Offset: 0x000F8C50
		[MonitoringDescription("ProcessPrivateMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PrivateMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PrivateMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.privateBytes;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06003AC9 RID: 15049 RVA: 0x000F9C66 File Offset: 0x000F8C66
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessPrivateMemorySize")]
		public long PrivateMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.privateBytes;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x000F9C7B File Offset: 0x000F8C7B
		[MonitoringDescription("ProcessPrivilegedProcessorTime")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TimeSpan PrivilegedProcessorTime
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				return this.GetProcessTimes().PrivilegedProcessorTime;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06003ACB RID: 15051 RVA: 0x000F9C90 File Offset: 0x000F8C90
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessProcessName")]
		public string ProcessName
		{
			get
			{
				this.EnsureState(Process.State.HaveProcessInfo);
				string processName = this.processInfo.processName;
				if (processName.Length == 15 && ProcessManager.IsNt && ProcessManager.IsOSOlderThanXP && !this.isRemoteMachine)
				{
					try
					{
						string moduleName = this.MainModule.ModuleName;
						if (moduleName != null)
						{
							this.processInfo.processName = Path.ChangeExtension(Path.GetFileName(moduleName), null);
						}
					}
					catch (Exception)
					{
					}
				}
				return this.processInfo.processName;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x000F9D18 File Offset: 0x000F8D18
		// (set) Token: 0x06003ACD RID: 15053 RVA: 0x000F9D7C File Offset: 0x000F8D7C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessProcessorAffinity")]
		public IntPtr ProcessorAffinity
		{
			get
			{
				if (!this.haveProcessorAffinity)
				{
					SafeProcessHandle handle = null;
					try
					{
						handle = this.GetProcessHandle(1024);
						IntPtr intPtr;
						IntPtr intPtr2;
						if (!NativeMethods.GetProcessAffinityMask(handle, out intPtr, out intPtr2))
						{
							throw new Win32Exception();
						}
						this.processorAffinity = intPtr;
					}
					finally
					{
						this.ReleaseProcessHandle(handle);
					}
					this.haveProcessorAffinity = true;
				}
				return this.processorAffinity;
			}
			set
			{
				SafeProcessHandle handle = null;
				try
				{
					handle = this.GetProcessHandle(512);
					if (!NativeMethods.SetProcessAffinityMask(handle, value))
					{
						throw new Win32Exception();
					}
					this.processorAffinity = value;
					this.haveProcessorAffinity = true;
				}
				finally
				{
					this.ReleaseProcessHandle(handle);
				}
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x000F9DD0 File Offset: 0x000F8DD0
		[MonitoringDescription("ProcessResponding")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Responding
		{
			get
			{
				if (!this.haveResponding)
				{
					IntPtr intPtr = this.MainWindowHandle;
					if (intPtr == (IntPtr)0)
					{
						this.responding = true;
					}
					else
					{
						IntPtr intPtr2;
						this.responding = (NativeMethods.SendMessageTimeout(new HandleRef(this, intPtr), 0, IntPtr.Zero, IntPtr.Zero, 2, 5000, out intPtr2) != (IntPtr)0);
					}
				}
				return this.responding;
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003ACF RID: 15055 RVA: 0x000F9E39 File Offset: 0x000F8E39
		[MonitoringDescription("ProcessSessionId")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SessionId
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.sessionId;
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x000F9E4E File Offset: 0x000F8E4E
		// (set) Token: 0x06003AD1 RID: 15057 RVA: 0x000F9E6A File Offset: 0x000F8E6A
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[MonitoringDescription("ProcessStartInfo")]
		public ProcessStartInfo StartInfo
		{
			get
			{
				if (this.startInfo == null)
				{
					this.startInfo = new ProcessStartInfo(this);
				}
				return this.startInfo;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.startInfo = value;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x000F9E81 File Offset: 0x000F8E81
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessStartTime")]
		public DateTime StartTime
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				return this.GetProcessTimes().StartTime;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06003AD3 RID: 15059 RVA: 0x000F9E98 File Offset: 0x000F8E98
		// (set) Token: 0x06003AD4 RID: 15060 RVA: 0x000F9EF2 File Offset: 0x000F8EF2
		[MonitoringDescription("ProcessSynchronizingObject")]
		[Browsable(false)]
		[DefaultValue(null)]
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				if (this.synchronizingObject == null && base.DesignMode)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						object rootComponent = designerHost.RootComponent;
						if (rootComponent != null && rootComponent is ISynchronizeInvoke)
						{
							this.synchronizingObject = (ISynchronizeInvoke)rootComponent;
						}
					}
				}
				return this.synchronizingObject;
			}
			set
			{
				this.synchronizingObject = value;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06003AD5 RID: 15061 RVA: 0x000F9EFC File Offset: 0x000F8EFC
		[MonitoringDescription("ProcessThreads")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ProcessThreadCollection Threads
		{
			get
			{
				if (this.threads == null)
				{
					this.EnsureState(Process.State.HaveProcessInfo);
					int count = this.processInfo.threadInfoList.Count;
					ProcessThread[] array = new ProcessThread[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = new ProcessThread(this.isRemoteMachine, (ThreadInfo)this.processInfo.threadInfoList[i]);
					}
					ProcessThreadCollection processThreadCollection = new ProcessThreadCollection(array);
					this.threads = processThreadCollection;
				}
				return this.threads;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x000F9F74 File Offset: 0x000F8F74
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessTotalProcessorTime")]
		public TimeSpan TotalProcessorTime
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				return this.GetProcessTimes().TotalProcessorTime;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x000F9F88 File Offset: 0x000F8F88
		[MonitoringDescription("ProcessUserProcessorTime")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TimeSpan UserProcessorTime
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				return this.GetProcessTimes().UserProcessorTime;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x000F9F9C File Offset: 0x000F8F9C
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.VirtualMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessVirtualMemorySize")]
		public int VirtualMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.virtualBytes;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x000F9FB2 File Offset: 0x000F8FB2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessVirtualMemorySize")]
		public long VirtualMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.virtualBytes;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06003ADA RID: 15066 RVA: 0x000F9FC7 File Offset: 0x000F8FC7
		// (set) Token: 0x06003ADB RID: 15067 RVA: 0x000F9FCF File Offset: 0x000F8FCF
		[DefaultValue(false)]
		[MonitoringDescription("ProcessEnableRaisingEvents")]
		[Browsable(false)]
		public bool EnableRaisingEvents
		{
			get
			{
				return this.watchForExit;
			}
			set
			{
				if (value != this.watchForExit)
				{
					if (this.Associated)
					{
						if (value)
						{
							this.OpenProcessHandle();
							this.EnsureWatchingForExit();
						}
						else
						{
							this.StopWatchingForExit();
						}
					}
					this.watchForExit = value;
				}
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x000FA001 File Offset: 0x000F9001
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessStandardInput")]
		[Browsable(false)]
		public StreamWriter StandardInput
		{
			get
			{
				if (this.standardInput == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardIn"));
				}
				return this.standardInput;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x000FA024 File Offset: 0x000F9024
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("ProcessStandardOutput")]
		public StreamReader StandardOutput
		{
			get
			{
				if (this.standardOutput == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardOut"));
				}
				if (this.outputStreamReadMode == Process.StreamReadMode.undefined)
				{
					this.outputStreamReadMode = Process.StreamReadMode.syncMode;
				}
				else if (this.outputStreamReadMode != Process.StreamReadMode.syncMode)
				{
					throw new InvalidOperationException(SR.GetString("CantMixSyncAsyncOperation"));
				}
				return this.standardOutput;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x000FA07C File Offset: 0x000F907C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessStandardError")]
		[Browsable(false)]
		public StreamReader StandardError
		{
			get
			{
				if (this.standardError == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardError"));
				}
				if (this.errorStreamReadMode == Process.StreamReadMode.undefined)
				{
					this.errorStreamReadMode = Process.StreamReadMode.syncMode;
				}
				else if (this.errorStreamReadMode != Process.StreamReadMode.syncMode)
				{
					throw new InvalidOperationException(SR.GetString("CantMixSyncAsyncOperation"));
				}
				return this.standardError;
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000FA0D1 File Offset: 0x000F90D1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.WorkingSet64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[MonitoringDescription("ProcessWorkingSet")]
		public int WorkingSet
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.workingSet;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x000FA0E7 File Offset: 0x000F90E7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessWorkingSet")]
		public long WorkingSet64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.workingSet;
			}
		}

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06003AE1 RID: 15073 RVA: 0x000FA0FC File Offset: 0x000F90FC
		// (remove) Token: 0x06003AE2 RID: 15074 RVA: 0x000FA115 File Offset: 0x000F9115
		[Category("Behavior")]
		[MonitoringDescription("ProcessExited")]
		public event EventHandler Exited
		{
			add
			{
				this.onExited = (EventHandler)Delegate.Combine(this.onExited, value);
			}
			remove
			{
				this.onExited = (EventHandler)Delegate.Remove(this.onExited, value);
			}
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x000FA130 File Offset: 0x000F9130
		public bool CloseMainWindow()
		{
			IntPtr intPtr = this.MainWindowHandle;
			if (intPtr == (IntPtr)0)
			{
				return false;
			}
			int windowLong = NativeMethods.GetWindowLong(new HandleRef(this, intPtr), -16);
			if ((windowLong & 134217728) != 0)
			{
				return false;
			}
			NativeMethods.PostMessage(new HandleRef(this, intPtr), 16, IntPtr.Zero, IntPtr.Zero);
			return true;
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x000FA188 File Offset: 0x000F9188
		private void ReleaseProcessHandle(SafeProcessHandle handle)
		{
			if (handle == null)
			{
				return;
			}
			if (this.haveProcessHandle && handle == this.m_processHandle)
			{
				return;
			}
			handle.Close();
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x000FA1A6 File Offset: 0x000F91A6
		private void CompletionCallback(object context, bool wasSignaled)
		{
			this.StopWatchingForExit();
			this.RaiseOnExited();
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x000FA1B4 File Offset: 0x000F91B4
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.Close();
				}
				this.disposed = true;
				base.Dispose(disposing);
			}
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x000FA1D8 File Offset: 0x000F91D8
		public void Close()
		{
			if (this.Associated)
			{
				if (this.haveProcessHandle)
				{
					this.StopWatchingForExit();
					this.m_processHandle.Close();
					this.m_processHandle = null;
					this.haveProcessHandle = false;
				}
				this.haveProcessId = false;
				this.isRemoteMachine = false;
				this.machineName = ".";
				this.raisedOnExited = false;
				this.standardOutput = null;
				this.standardInput = null;
				this.standardError = null;
				this.Refresh();
			}
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x000FA250 File Offset: 0x000F9250
		private void EnsureState(Process.State state)
		{
			if ((state & Process.State.IsWin2k) != (Process.State)0 && (this.OperatingSystem.Platform != PlatformID.Win32NT || this.OperatingSystem.Version.Major < 5))
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2kRequired"));
			}
			if ((state & Process.State.IsNt) != (Process.State)0 && this.OperatingSystem.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
			if ((state & Process.State.Associated) != (Process.State)0 && !this.Associated)
			{
				throw new InvalidOperationException(SR.GetString("NoAssociatedProcess"));
			}
			if ((state & Process.State.HaveId) != (Process.State)0 && !this.haveProcessId)
			{
				if (!this.haveProcessHandle)
				{
					this.EnsureState(Process.State.Associated);
					throw new InvalidOperationException(SR.GetString("ProcessIdRequired"));
				}
				this.SetProcessId(ProcessManager.GetProcessIdFromHandle(this.m_processHandle));
			}
			if ((state & Process.State.IsLocal) != (Process.State)0 && this.isRemoteMachine)
			{
				throw new NotSupportedException(SR.GetString("NotSupportedRemote"));
			}
			if ((state & Process.State.HaveProcessInfo) != (Process.State)0 && this.processInfo == null)
			{
				if ((state & Process.State.HaveId) == (Process.State)0)
				{
					this.EnsureState(Process.State.HaveId);
				}
				ProcessInfo[] processInfos = ProcessManager.GetProcessInfos(this.machineName);
				for (int i = 0; i < processInfos.Length; i++)
				{
					if (processInfos[i].processId == this.processId)
					{
						this.processInfo = processInfos[i];
						break;
					}
				}
				if (this.processInfo == null)
				{
					throw new InvalidOperationException(SR.GetString("NoProcessInfo"));
				}
			}
			if ((state & Process.State.Exited) != (Process.State)0)
			{
				if (!this.HasExited)
				{
					throw new InvalidOperationException(SR.GetString("WaitTillExit"));
				}
				if (!this.haveProcessHandle)
				{
					throw new InvalidOperationException(SR.GetString("NoProcessHandle"));
				}
			}
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x000FA3D0 File Offset: 0x000F93D0
		private void EnsureWatchingForExit()
		{
			if (!this.watchingForExit)
			{
				lock (this)
				{
					if (!this.watchingForExit)
					{
						this.watchingForExit = true;
						try
						{
							this.waitHandle = new ProcessWaitHandle(this.m_processHandle);
							this.registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(this.waitHandle, new WaitOrTimerCallback(this.CompletionCallback), null, -1, true);
						}
						catch
						{
							this.watchingForExit = false;
							throw;
						}
					}
				}
			}
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x000FA460 File Offset: 0x000F9460
		private void EnsureWorkingSetLimits()
		{
			this.EnsureState(Process.State.IsNt);
			if (!this.haveWorkingSetLimits)
			{
				SafeProcessHandle handle = null;
				try
				{
					handle = this.GetProcessHandle(1024);
					IntPtr intPtr;
					IntPtr intPtr2;
					if (!NativeMethods.GetProcessWorkingSetSize(handle, out intPtr, out intPtr2))
					{
						throw new Win32Exception();
					}
					this.minWorkingSet = intPtr;
					this.maxWorkingSet = intPtr2;
					this.haveWorkingSetLimits = true;
				}
				finally
				{
					this.ReleaseProcessHandle(handle);
				}
			}
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x000FA4CC File Offset: 0x000F94CC
		public static void EnterDebugMode()
		{
			if (ProcessManager.IsNt)
			{
				Process.SetPrivilege("SeDebugPrivilege", 2);
			}
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x000FA4E0 File Offset: 0x000F94E0
		private static void SetPrivilege(string privilegeName, int attrib)
		{
			IntPtr handle = (IntPtr)0;
			NativeMethods.LUID luid = default(NativeMethods.LUID);
			IntPtr currentProcess = NativeMethods.GetCurrentProcess();
			if (!NativeMethods.OpenProcessToken(new HandleRef(null, currentProcess), 32, out handle))
			{
				throw new Win32Exception();
			}
			try
			{
				if (!NativeMethods.LookupPrivilegeValue(null, privilegeName, out luid))
				{
					throw new Win32Exception();
				}
				NativeMethods.TokenPrivileges tokenPrivileges = new NativeMethods.TokenPrivileges();
				tokenPrivileges.Luid = luid;
				tokenPrivileges.Attributes = attrib;
				NativeMethods.AdjustTokenPrivileges(new HandleRef(null, handle), false, tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero);
				if (Marshal.GetLastWin32Error() != 0)
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				SafeNativeMethods.CloseHandle(new HandleRef(null, handle));
			}
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x000FA588 File Offset: 0x000F9588
		public static void LeaveDebugMode()
		{
			if (ProcessManager.IsNt)
			{
				Process.SetPrivilege("SeDebugPrivilege", 0);
			}
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x000FA59C File Offset: 0x000F959C
		public static Process GetProcessById(int processId, string machineName)
		{
			if (!ProcessManager.IsProcessRunning(processId, machineName))
			{
				throw new ArgumentException(SR.GetString("MissingProccess", new object[]
				{
					processId.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return new Process(machineName, ProcessManager.IsRemoteMachine(machineName), processId, null);
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x000FA5E7 File Offset: 0x000F95E7
		public static Process GetProcessById(int processId)
		{
			return Process.GetProcessById(processId, ".");
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x000FA5F4 File Offset: 0x000F95F4
		public static Process[] GetProcessesByName(string processName)
		{
			return Process.GetProcessesByName(processName, ".");
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x000FA604 File Offset: 0x000F9604
		public static Process[] GetProcessesByName(string processName, string machineName)
		{
			if (processName == null)
			{
				processName = string.Empty;
			}
			Process[] processes = Process.GetProcesses(machineName);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < processes.Length; i++)
			{
				if (string.Equals(processName, processes[i].ProcessName, StringComparison.OrdinalIgnoreCase))
				{
					arrayList.Add(processes[i]);
				}
			}
			Process[] array = new Process[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x000FA666 File Offset: 0x000F9666
		public static Process[] GetProcesses()
		{
			return Process.GetProcesses(".");
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x000FA674 File Offset: 0x000F9674
		public static Process[] GetProcesses(string machineName)
		{
			bool flag = ProcessManager.IsRemoteMachine(machineName);
			ProcessInfo[] processInfos = ProcessManager.GetProcessInfos(machineName);
			Process[] array = new Process[processInfos.Length];
			for (int i = 0; i < processInfos.Length; i++)
			{
				ProcessInfo processInfo = processInfos[i];
				array[i] = new Process(machineName, flag, processInfo.processId, processInfo);
			}
			return array;
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x000FA6BF File Offset: 0x000F96BF
		public static Process GetCurrentProcess()
		{
			return new Process(".", false, NativeMethods.GetCurrentProcessId(), null);
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x000FA6D4 File Offset: 0x000F96D4
		protected void OnExited()
		{
			EventHandler eventHandler = this.onExited;
			if (eventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(eventHandler, new object[]
					{
						this,
						EventArgs.Empty
					});
					return;
				}
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x000FA730 File Offset: 0x000F9730
		private SafeProcessHandle GetProcessHandle(int access, bool throwIfExited)
		{
			if (this.haveProcessHandle)
			{
				if (throwIfExited)
				{
					ProcessWaitHandle processWaitHandle = null;
					try
					{
						processWaitHandle = new ProcessWaitHandle(this.m_processHandle);
						if (processWaitHandle.WaitOne(0, false))
						{
							if (this.haveProcessId)
							{
								throw new InvalidOperationException(SR.GetString("ProcessHasExited", new object[]
								{
									this.processId.ToString(CultureInfo.CurrentCulture)
								}));
							}
							throw new InvalidOperationException(SR.GetString("ProcessHasExitedNoId"));
						}
					}
					finally
					{
						if (processWaitHandle != null)
						{
							processWaitHandle.Close();
						}
					}
				}
				return this.m_processHandle;
			}
			this.EnsureState((Process.State)3);
			SafeProcessHandle safeProcessHandle = SafeProcessHandle.InvalidHandle;
			safeProcessHandle = ProcessManager.OpenProcess(this.processId, access, throwIfExited);
			if (throwIfExited && (access & 1024) != 0 && NativeMethods.GetExitCodeProcess(safeProcessHandle, out this.exitCode) && this.exitCode != 259)
			{
				throw new InvalidOperationException(SR.GetString("ProcessHasExited", new object[]
				{
					this.processId.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return safeProcessHandle;
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x000FA834 File Offset: 0x000F9834
		private SafeProcessHandle GetProcessHandle(int access)
		{
			return this.GetProcessHandle(access, true);
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x000FA83E File Offset: 0x000F983E
		private SafeProcessHandle OpenProcessHandle()
		{
			if (!this.haveProcessHandle)
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				this.SetProcessHandle(this.GetProcessHandle(2035711));
			}
			return this.m_processHandle;
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x000FA878 File Offset: 0x000F9878
		private void RaiseOnExited()
		{
			if (!this.raisedOnExited)
			{
				lock (this)
				{
					if (!this.raisedOnExited)
					{
						this.raisedOnExited = true;
						this.OnExited();
					}
				}
			}
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x000FA8C4 File Offset: 0x000F98C4
		public void Refresh()
		{
			this.processInfo = null;
			this.threads = null;
			this.modules = null;
			this.mainWindowTitle = null;
			this.exited = false;
			this.signaled = false;
			this.haveMainWindow = false;
			this.haveWorkingSetLimits = false;
			this.haveProcessorAffinity = false;
			this.havePriorityClass = false;
			this.haveExitTime = false;
			this.haveResponding = false;
			this.havePriorityBoostEnabled = false;
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x000FA92C File Offset: 0x000F992C
		private void SetProcessHandle(SafeProcessHandle processHandle)
		{
			this.m_processHandle = processHandle;
			this.haveProcessHandle = true;
			if (this.watchForExit)
			{
				this.EnsureWatchingForExit();
			}
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x000FA94A File Offset: 0x000F994A
		private void SetProcessId(int processId)
		{
			this.processId = processId;
			this.haveProcessId = true;
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x000FA95C File Offset: 0x000F995C
		private void SetWorkingSetLimits(object newMin, object newMax)
		{
			this.EnsureState(Process.State.IsNt);
			SafeProcessHandle handle = null;
			try
			{
				handle = this.GetProcessHandle(1280);
				IntPtr intPtr;
				IntPtr intPtr2;
				if (!NativeMethods.GetProcessWorkingSetSize(handle, out intPtr, out intPtr2))
				{
					throw new Win32Exception();
				}
				if (newMin != null)
				{
					intPtr = (IntPtr)newMin;
				}
				if (newMax != null)
				{
					intPtr2 = (IntPtr)newMax;
				}
				if ((long)intPtr > (long)intPtr2)
				{
					if (newMin != null)
					{
						throw new ArgumentException(SR.GetString("BadMinWorkset"));
					}
					throw new ArgumentException(SR.GetString("BadMaxWorkset"));
				}
				else
				{
					if (!NativeMethods.SetProcessWorkingSetSize(handle, intPtr, intPtr2))
					{
						throw new Win32Exception();
					}
					if (!NativeMethods.GetProcessWorkingSetSize(handle, out intPtr, out intPtr2))
					{
						throw new Win32Exception();
					}
					this.minWorkingSet = intPtr;
					this.maxWorkingSet = intPtr2;
					this.haveWorkingSetLimits = true;
				}
			}
			finally
			{
				this.ReleaseProcessHandle(handle);
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x000FAA28 File Offset: 0x000F9A28
		public bool Start()
		{
			this.Close();
			ProcessStartInfo processStartInfo = this.StartInfo;
			if (processStartInfo.FileName.Length == 0)
			{
				throw new InvalidOperationException(SR.GetString("FileNameMissing"));
			}
			if (processStartInfo.UseShellExecute)
			{
				return this.StartWithShellExecuteEx(processStartInfo);
			}
			return this.StartWithCreateProcess(processStartInfo);
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x000FAA78 File Offset: 0x000F9A78
		private static void CreatePipeWithSecurityAttributes(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, NativeMethods.SECURITY_ATTRIBUTES lpPipeAttributes, int nSize)
		{
			bool flag = NativeMethods.CreatePipe(out hReadPipe, out hWritePipe, lpPipeAttributes, nSize);
			if (!flag || hReadPipe.IsInvalid || hWritePipe.IsInvalid)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x000FAAAC File Offset: 0x000F9AAC
		private void CreatePipe(out SafeFileHandle parentHandle, out SafeFileHandle childHandle, bool parentInputs)
		{
			NativeMethods.SECURITY_ATTRIBUTES security_ATTRIBUTES = new NativeMethods.SECURITY_ATTRIBUTES();
			security_ATTRIBUTES.bInheritHandle = true;
			SafeFileHandle safeFileHandle = null;
			try
			{
				if (parentInputs)
				{
					Process.CreatePipeWithSecurityAttributes(out childHandle, out safeFileHandle, security_ATTRIBUTES, 0);
				}
				else
				{
					Process.CreatePipeWithSecurityAttributes(out safeFileHandle, out childHandle, security_ATTRIBUTES, 0);
				}
				if (!NativeMethods.DuplicateHandle(new HandleRef(this, NativeMethods.GetCurrentProcess()), safeFileHandle, new HandleRef(this, NativeMethods.GetCurrentProcess()), out parentHandle, 0, false, 2))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				if (safeFileHandle != null && !safeFileHandle.IsInvalid)
				{
					safeFileHandle.Close();
				}
			}
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x000FAB30 File Offset: 0x000F9B30
		private static StringBuilder BuildCommandLine(string executableFileName, string arguments)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = executableFileName.Trim();
			bool flag = text.StartsWith("\"", StringComparison.Ordinal) && text.EndsWith("\"", StringComparison.Ordinal);
			if (!flag)
			{
				stringBuilder.Append("\"");
			}
			stringBuilder.Append(text);
			if (!flag)
			{
				stringBuilder.Append("\"");
			}
			if (!string.IsNullOrEmpty(arguments))
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(arguments);
			}
			return stringBuilder;
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x000FABAC File Offset: 0x000F9BAC
		private bool StartWithCreateProcess(ProcessStartInfo startInfo)
		{
			if (startInfo.StandardOutputEncoding != null && !startInfo.RedirectStandardOutput)
			{
				throw new InvalidOperationException(SR.GetString("StandardOutputEncodingNotAllowed"));
			}
			if (startInfo.StandardErrorEncoding != null && !startInfo.RedirectStandardError)
			{
				throw new InvalidOperationException(SR.GetString("StandardErrorEncodingNotAllowed"));
			}
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			StringBuilder stringBuilder = Process.BuildCommandLine(startInfo.FileName, startInfo.Arguments);
			NativeMethods.STARTUPINFO startupinfo = new NativeMethods.STARTUPINFO();
			SafeNativeMethods.PROCESS_INFORMATION process_INFORMATION = new SafeNativeMethods.PROCESS_INFORMATION();
			SafeProcessHandle safeProcessHandle = new SafeProcessHandle();
			SafeThreadHandle safeThreadHandle = new SafeThreadHandle();
			int num = 0;
			SafeFileHandle handle = null;
			SafeFileHandle handle2 = null;
			SafeFileHandle handle3 = null;
			GCHandle gchandle = default(GCHandle);
			try
			{
				if (startInfo.RedirectStandardInput || startInfo.RedirectStandardOutput || startInfo.RedirectStandardError)
				{
					if (startInfo.RedirectStandardInput)
					{
						this.CreatePipe(out handle, out startupinfo.hStdInput, true);
					}
					else
					{
						startupinfo.hStdInput = new SafeFileHandle(NativeMethods.GetStdHandle(-10), false);
					}
					if (startInfo.RedirectStandardOutput)
					{
						this.CreatePipe(out handle2, out startupinfo.hStdOutput, false);
					}
					else
					{
						startupinfo.hStdOutput = new SafeFileHandle(NativeMethods.GetStdHandle(-11), false);
					}
					if (startInfo.RedirectStandardError)
					{
						this.CreatePipe(out handle3, out startupinfo.hStdError, false);
					}
					else
					{
						startupinfo.hStdError = new SafeFileHandle(NativeMethods.GetStdHandle(-12), false);
					}
					startupinfo.dwFlags = 256;
				}
				int num2 = 0;
				if (startInfo.CreateNoWindow)
				{
					num2 |= 134217728;
				}
				IntPtr intPtr = (IntPtr)0;
				if (startInfo.environmentVariables != null)
				{
					bool unicode = false;
					if (ProcessManager.IsNt)
					{
						num2 |= 1024;
						unicode = true;
					}
					byte[] value = EnvironmentBlock.ToByteArray(startInfo.environmentVariables, unicode);
					gchandle = GCHandle.Alloc(value, GCHandleType.Pinned);
					intPtr = gchandle.AddrOfPinnedObject();
				}
				string text = startInfo.WorkingDirectory;
				if (text == string.Empty)
				{
					text = Environment.CurrentDirectory;
				}
				bool flag;
				if (startInfo.UserName.Length != 0)
				{
					NativeMethods.LogonFlags logonFlags = (NativeMethods.LogonFlags)0;
					if (startInfo.LoadUserProfile)
					{
						logonFlags = NativeMethods.LogonFlags.LOGON_WITH_PROFILE;
					}
					IntPtr intPtr2 = IntPtr.Zero;
					try
					{
						if (startInfo.Password == null)
						{
							intPtr2 = Marshal.StringToCoTaskMemUni(string.Empty);
						}
						else
						{
							intPtr2 = Marshal.SecureStringToCoTaskMemUnicode(startInfo.Password);
						}
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
						}
						finally
						{
							flag = NativeMethods.CreateProcessWithLogonW(startInfo.UserName, startInfo.Domain, intPtr2, logonFlags, null, stringBuilder, num2, intPtr, text, startupinfo, process_INFORMATION);
							if (!flag)
							{
								num = Marshal.GetLastWin32Error();
							}
							if (process_INFORMATION.hProcess != (IntPtr)0 && process_INFORMATION.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
							{
								safeProcessHandle.InitialSetHandle(process_INFORMATION.hProcess);
							}
							if (process_INFORMATION.hThread != (IntPtr)0 && process_INFORMATION.hThread != NativeMethods.INVALID_HANDLE_VALUE)
							{
								safeThreadHandle.InitialSetHandle(process_INFORMATION.hThread);
							}
						}
						if (flag)
						{
							goto IL_395;
						}
						if (num == 193)
						{
							throw new Win32Exception(num, SR.GetString("InvalidApplication"));
						}
						throw new Win32Exception(num);
					}
					finally
					{
						if (intPtr2 != IntPtr.Zero)
						{
							Marshal.ZeroFreeCoTaskMemUnicode(intPtr2);
						}
					}
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					flag = NativeMethods.CreateProcess(null, stringBuilder, null, null, true, num2, intPtr, text, startupinfo, process_INFORMATION);
					if (!flag)
					{
						num = Marshal.GetLastWin32Error();
					}
					if (process_INFORMATION.hProcess != (IntPtr)0 && process_INFORMATION.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
					{
						safeProcessHandle.InitialSetHandle(process_INFORMATION.hProcess);
					}
					if (process_INFORMATION.hThread != (IntPtr)0 && process_INFORMATION.hThread != NativeMethods.INVALID_HANDLE_VALUE)
					{
						safeThreadHandle.InitialSetHandle(process_INFORMATION.hThread);
					}
				}
				if (!flag)
				{
					if (num == 193)
					{
						throw new Win32Exception(num, SR.GetString("InvalidApplication"));
					}
					throw new Win32Exception(num);
				}
				IL_395:;
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				startupinfo.Dispose();
			}
			if (startInfo.RedirectStandardInput)
			{
				this.standardInput = new StreamWriter(new FileStream(handle, FileAccess.Write, 4096, false), Encoding.GetEncoding(NativeMethods.GetConsoleCP()), 4096);
				this.standardInput.AutoFlush = true;
			}
			if (startInfo.RedirectStandardOutput)
			{
				Encoding encoding = (startInfo.StandardOutputEncoding != null) ? startInfo.StandardOutputEncoding : Encoding.GetEncoding(NativeMethods.GetConsoleOutputCP());
				this.standardOutput = new StreamReader(new FileStream(handle2, FileAccess.Read, 4096, false), encoding, true, 4096);
			}
			if (startInfo.RedirectStandardError)
			{
				Encoding encoding2 = (startInfo.StandardErrorEncoding != null) ? startInfo.StandardErrorEncoding : Encoding.GetEncoding(NativeMethods.GetConsoleOutputCP());
				this.standardError = new StreamReader(new FileStream(handle3, FileAccess.Read, 4096, false), encoding2, true, 4096);
			}
			bool result = false;
			if (!safeProcessHandle.IsInvalid)
			{
				this.SetProcessHandle(safeProcessHandle);
				this.SetProcessId(process_INFORMATION.dwProcessId);
				safeThreadHandle.Close();
				result = true;
			}
			return result;
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x000FB0BC File Offset: 0x000FA0BC
		private bool StartWithShellExecuteEx(ProcessStartInfo startInfo)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (!string.IsNullOrEmpty(startInfo.UserName) || startInfo.Password != null)
			{
				throw new InvalidOperationException(SR.GetString("CantStartAsUser"));
			}
			if (startInfo.RedirectStandardInput || startInfo.RedirectStandardOutput || startInfo.RedirectStandardError)
			{
				throw new InvalidOperationException(SR.GetString("CantRedirectStreams"));
			}
			if (startInfo.StandardErrorEncoding != null)
			{
				throw new InvalidOperationException(SR.GetString("StandardErrorEncodingNotAllowed"));
			}
			if (startInfo.StandardOutputEncoding != null)
			{
				throw new InvalidOperationException(SR.GetString("StandardOutputEncodingNotAllowed"));
			}
			if (startInfo.environmentVariables != null)
			{
				throw new InvalidOperationException(SR.GetString("CantUseEnvVars"));
			}
			NativeMethods.ShellExecuteInfo shellExecuteInfo = new NativeMethods.ShellExecuteInfo();
			shellExecuteInfo.fMask = 64;
			if (startInfo.ErrorDialog)
			{
				shellExecuteInfo.hwnd = startInfo.ErrorDialogParentHandle;
			}
			else
			{
				shellExecuteInfo.fMask |= 1024;
			}
			switch (startInfo.WindowStyle)
			{
			case ProcessWindowStyle.Hidden:
				shellExecuteInfo.nShow = 0;
				break;
			case ProcessWindowStyle.Minimized:
				shellExecuteInfo.nShow = 2;
				break;
			case ProcessWindowStyle.Maximized:
				shellExecuteInfo.nShow = 3;
				break;
			default:
				shellExecuteInfo.nShow = 1;
				break;
			}
			try
			{
				if (startInfo.FileName.Length != 0)
				{
					shellExecuteInfo.lpFile = Marshal.StringToHGlobalAuto(startInfo.FileName);
				}
				if (startInfo.Verb.Length != 0)
				{
					shellExecuteInfo.lpVerb = Marshal.StringToHGlobalAuto(startInfo.Verb);
				}
				if (startInfo.Arguments.Length != 0)
				{
					shellExecuteInfo.lpParameters = Marshal.StringToHGlobalAuto(startInfo.Arguments);
				}
				if (startInfo.WorkingDirectory.Length != 0)
				{
					shellExecuteInfo.lpDirectory = Marshal.StringToHGlobalAuto(startInfo.WorkingDirectory);
				}
				shellExecuteInfo.fMask |= 256;
				ShellExecuteHelper shellExecuteHelper = new ShellExecuteHelper(shellExecuteInfo);
				if (!shellExecuteHelper.ShellExecuteOnSTAThread())
				{
					int num = shellExecuteHelper.ErrorCode;
					if (num == 0)
					{
						long num2 = (long)shellExecuteInfo.hInstApp;
						if (num2 <= 8L)
						{
							if (num2 < 2L)
							{
								goto IL_276;
							}
							switch ((int)(num2 - 2L))
							{
							case 0:
								num = 2;
								goto IL_282;
							case 1:
								num = 3;
								goto IL_282;
							case 2:
							case 4:
							case 5:
								goto IL_276;
							case 3:
								num = 5;
								goto IL_282;
							case 6:
								num = 8;
								goto IL_282;
							}
						}
						if (num2 <= 32L && num2 >= 26L)
						{
							switch ((int)(num2 - 26L))
							{
							case 0:
								num = 32;
								goto IL_282;
							case 2:
							case 3:
							case 4:
								num = 1156;
								goto IL_282;
							case 5:
								num = 1155;
								goto IL_282;
							case 6:
								num = 1157;
								goto IL_282;
							}
						}
						IL_276:
						num = (int)shellExecuteInfo.hInstApp;
					}
					IL_282:
					throw new Win32Exception(num);
				}
			}
			finally
			{
				if (shellExecuteInfo.lpFile != (IntPtr)0)
				{
					Marshal.FreeHGlobal(shellExecuteInfo.lpFile);
				}
				if (shellExecuteInfo.lpVerb != (IntPtr)0)
				{
					Marshal.FreeHGlobal(shellExecuteInfo.lpVerb);
				}
				if (shellExecuteInfo.lpParameters != (IntPtr)0)
				{
					Marshal.FreeHGlobal(shellExecuteInfo.lpParameters);
				}
				if (shellExecuteInfo.lpDirectory != (IntPtr)0)
				{
					Marshal.FreeHGlobal(shellExecuteInfo.lpDirectory);
				}
			}
			if (shellExecuteInfo.hProcess != (IntPtr)0)
			{
				SafeProcessHandle processHandle = new SafeProcessHandle(shellExecuteInfo.hProcess);
				this.SetProcessHandle(processHandle);
				return true;
			}
			return false;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x000FB414 File Offset: 0x000FA414
		public static Process Start(string fileName, string userName, SecureString password, string domain)
		{
			return Process.Start(new ProcessStartInfo(fileName)
			{
				UserName = userName,
				Password = password,
				Domain = domain,
				UseShellExecute = false
			});
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x000FB44C File Offset: 0x000FA44C
		public static Process Start(string fileName, string arguments, string userName, SecureString password, string domain)
		{
			return Process.Start(new ProcessStartInfo(fileName, arguments)
			{
				UserName = userName,
				Password = password,
				Domain = domain,
				UseShellExecute = false
			});
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000FB484 File Offset: 0x000FA484
		public static Process Start(string fileName)
		{
			return Process.Start(new ProcessStartInfo(fileName));
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x000FB491 File Offset: 0x000FA491
		public static Process Start(string fileName, string arguments)
		{
			return Process.Start(new ProcessStartInfo(fileName, arguments));
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x000FB4A0 File Offset: 0x000FA4A0
		public static Process Start(ProcessStartInfo startInfo)
		{
			Process process = new Process();
			if (startInfo == null)
			{
				throw new ArgumentNullException("startInfo");
			}
			process.StartInfo = startInfo;
			if (process.Start())
			{
				return process;
			}
			return null;
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x000FB4D4 File Offset: 0x000FA4D4
		public void Kill()
		{
			SafeProcessHandle safeProcessHandle = null;
			try
			{
				safeProcessHandle = this.GetProcessHandle(1);
				if (!NativeMethods.TerminateProcess(safeProcessHandle, -1))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				this.ReleaseProcessHandle(safeProcessHandle);
			}
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x000FB514 File Offset: 0x000FA514
		private void StopWatchingForExit()
		{
			if (this.watchingForExit)
			{
				lock (this)
				{
					if (this.watchingForExit)
					{
						this.watchingForExit = false;
						this.registeredWaitHandle.Unregister(null);
						this.waitHandle.Close();
						this.waitHandle = null;
						this.registeredWaitHandle = null;
					}
				}
			}
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x000FB580 File Offset: 0x000FA580
		public override string ToString()
		{
			if (!this.Associated)
			{
				return base.ToString();
			}
			string text = string.Empty;
			try
			{
				text = this.ProcessName;
			}
			catch (PlatformNotSupportedException)
			{
			}
			if (text.Length != 0)
			{
				return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", new object[]
				{
					base.ToString(),
					text
				});
			}
			return base.ToString();
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x000FB5F4 File Offset: 0x000FA5F4
		public bool WaitForExit(int milliseconds)
		{
			SafeProcessHandle safeProcessHandle = null;
			ProcessWaitHandle processWaitHandle = null;
			bool flag;
			try
			{
				safeProcessHandle = this.GetProcessHandle(1048576, false);
				if (safeProcessHandle.IsInvalid)
				{
					flag = true;
				}
				else
				{
					processWaitHandle = new ProcessWaitHandle(safeProcessHandle);
					if (processWaitHandle.WaitOne(milliseconds, false))
					{
						flag = true;
						this.signaled = true;
					}
					else
					{
						flag = false;
						this.signaled = false;
					}
				}
			}
			finally
			{
				if (processWaitHandle != null)
				{
					processWaitHandle.Close();
				}
				if (this.output != null && milliseconds == 2147483647)
				{
					this.output.WaitUtilEOF();
				}
				if (this.error != null && milliseconds == 2147483647)
				{
					this.error.WaitUtilEOF();
				}
				this.ReleaseProcessHandle(safeProcessHandle);
			}
			if (flag && this.watchForExit)
			{
				this.RaiseOnExited();
			}
			return flag;
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x000FB6B0 File Offset: 0x000FA6B0
		public void WaitForExit()
		{
			this.WaitForExit(int.MaxValue);
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x000FB6C0 File Offset: 0x000FA6C0
		public bool WaitForInputIdle(int milliseconds)
		{
			SafeProcessHandle handle = null;
			bool result;
			try
			{
				handle = this.GetProcessHandle(1049600);
				int num = NativeMethods.WaitForInputIdle(handle, milliseconds);
				int num2 = num;
				switch (num2)
				{
				case -1:
					break;
				case 0:
					result = true;
					goto IL_4A;
				default:
					if (num2 == 258)
					{
						result = false;
						goto IL_4A;
					}
					break;
				}
				throw new InvalidOperationException(SR.GetString("InputIdleUnkownError"));
				IL_4A:;
			}
			finally
			{
				this.ReleaseProcessHandle(handle);
			}
			return result;
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x000FB734 File Offset: 0x000FA734
		public bool WaitForInputIdle()
		{
			return this.WaitForInputIdle(int.MaxValue);
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x000FB744 File Offset: 0x000FA744
		[ComVisible(false)]
		public void BeginOutputReadLine()
		{
			if (this.outputStreamReadMode == Process.StreamReadMode.undefined)
			{
				this.outputStreamReadMode = Process.StreamReadMode.asyncMode;
			}
			else if (this.outputStreamReadMode != Process.StreamReadMode.asyncMode)
			{
				throw new InvalidOperationException(SR.GetString("CantMixSyncAsyncOperation"));
			}
			if (this.pendingOutputRead)
			{
				throw new InvalidOperationException(SR.GetString("PendingAsyncOperation"));
			}
			this.pendingOutputRead = true;
			if (this.output == null)
			{
				if (this.standardOutput == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardOut"));
				}
				Stream baseStream = this.standardOutput.BaseStream;
				this.output = new AsyncStreamReader(this, baseStream, new UserCallBack(this.OutputReadNotifyUser), this.standardOutput.CurrentEncoding);
			}
			this.output.BeginReadLine();
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x000FB7F8 File Offset: 0x000FA7F8
		[ComVisible(false)]
		public void BeginErrorReadLine()
		{
			if (this.errorStreamReadMode == Process.StreamReadMode.undefined)
			{
				this.errorStreamReadMode = Process.StreamReadMode.asyncMode;
			}
			else if (this.errorStreamReadMode != Process.StreamReadMode.asyncMode)
			{
				throw new InvalidOperationException(SR.GetString("CantMixSyncAsyncOperation"));
			}
			if (this.pendingErrorRead)
			{
				throw new InvalidOperationException(SR.GetString("PendingAsyncOperation"));
			}
			this.pendingErrorRead = true;
			if (this.error == null)
			{
				if (this.standardError == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardError"));
				}
				Stream baseStream = this.standardError.BaseStream;
				this.error = new AsyncStreamReader(this, baseStream, new UserCallBack(this.ErrorReadNotifyUser), this.standardError.CurrentEncoding);
			}
			this.error.BeginReadLine();
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x000FB8A9 File Offset: 0x000FA8A9
		[ComVisible(false)]
		public void CancelOutputRead()
		{
			if (this.output != null)
			{
				this.output.CancelOperation();
				this.pendingOutputRead = false;
				return;
			}
			throw new InvalidOperationException(SR.GetString("NoAsyncOperation"));
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x000FB8D7 File Offset: 0x000FA8D7
		[ComVisible(false)]
		public void CancelErrorRead()
		{
			if (this.error != null)
			{
				this.error.CancelOperation();
				this.pendingErrorRead = false;
				return;
			}
			throw new InvalidOperationException(SR.GetString("NoAsyncOperation"));
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x000FB908 File Offset: 0x000FA908
		internal void OutputReadNotifyUser(string data)
		{
			DataReceivedEventHandler outputDataReceived = this.OutputDataReceived;
			if (outputDataReceived != null)
			{
				DataReceivedEventArgs dataReceivedEventArgs = new DataReceivedEventArgs(data);
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.Invoke(outputDataReceived, new object[]
					{
						this,
						dataReceivedEventArgs
					});
					return;
				}
				outputDataReceived(this, dataReceivedEventArgs);
			}
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x000FB964 File Offset: 0x000FA964
		internal void ErrorReadNotifyUser(string data)
		{
			DataReceivedEventHandler errorDataReceived = this.ErrorDataReceived;
			if (errorDataReceived != null)
			{
				DataReceivedEventArgs dataReceivedEventArgs = new DataReceivedEventArgs(data);
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.Invoke(errorDataReceived, new object[]
					{
						this,
						dataReceivedEventArgs
					});
					return;
				}
				errorDataReceived(this, dataReceivedEventArgs);
			}
		}

		// Token: 0x04003369 RID: 13161
		private bool haveProcessId;

		// Token: 0x0400336A RID: 13162
		private int processId;

		// Token: 0x0400336B RID: 13163
		private bool haveProcessHandle;

		// Token: 0x0400336C RID: 13164
		private SafeProcessHandle m_processHandle;

		// Token: 0x0400336D RID: 13165
		private bool isRemoteMachine;

		// Token: 0x0400336E RID: 13166
		private string machineName;

		// Token: 0x0400336F RID: 13167
		private ProcessInfo processInfo;

		// Token: 0x04003370 RID: 13168
		private ProcessThreadCollection threads;

		// Token: 0x04003371 RID: 13169
		private ProcessModuleCollection modules;

		// Token: 0x04003372 RID: 13170
		private bool haveMainWindow;

		// Token: 0x04003373 RID: 13171
		private IntPtr mainWindowHandle;

		// Token: 0x04003374 RID: 13172
		private string mainWindowTitle;

		// Token: 0x04003375 RID: 13173
		private bool haveWorkingSetLimits;

		// Token: 0x04003376 RID: 13174
		private IntPtr minWorkingSet;

		// Token: 0x04003377 RID: 13175
		private IntPtr maxWorkingSet;

		// Token: 0x04003378 RID: 13176
		private bool haveProcessorAffinity;

		// Token: 0x04003379 RID: 13177
		private IntPtr processorAffinity;

		// Token: 0x0400337A RID: 13178
		private bool havePriorityClass;

		// Token: 0x0400337B RID: 13179
		private ProcessPriorityClass priorityClass;

		// Token: 0x0400337C RID: 13180
		private ProcessStartInfo startInfo;

		// Token: 0x0400337D RID: 13181
		private bool watchForExit;

		// Token: 0x0400337E RID: 13182
		private bool watchingForExit;

		// Token: 0x0400337F RID: 13183
		private EventHandler onExited;

		// Token: 0x04003380 RID: 13184
		private bool exited;

		// Token: 0x04003381 RID: 13185
		private int exitCode;

		// Token: 0x04003382 RID: 13186
		private bool signaled;

		// Token: 0x04003383 RID: 13187
		private DateTime exitTime;

		// Token: 0x04003384 RID: 13188
		private bool haveExitTime;

		// Token: 0x04003385 RID: 13189
		private bool responding;

		// Token: 0x04003386 RID: 13190
		private bool haveResponding;

		// Token: 0x04003387 RID: 13191
		private bool priorityBoostEnabled;

		// Token: 0x04003388 RID: 13192
		private bool havePriorityBoostEnabled;

		// Token: 0x04003389 RID: 13193
		private bool raisedOnExited;

		// Token: 0x0400338A RID: 13194
		private RegisteredWaitHandle registeredWaitHandle;

		// Token: 0x0400338B RID: 13195
		private WaitHandle waitHandle;

		// Token: 0x0400338C RID: 13196
		private ISynchronizeInvoke synchronizingObject;

		// Token: 0x0400338D RID: 13197
		private StreamReader standardOutput;

		// Token: 0x0400338E RID: 13198
		private StreamWriter standardInput;

		// Token: 0x0400338F RID: 13199
		private StreamReader standardError;

		// Token: 0x04003390 RID: 13200
		private OperatingSystem operatingSystem;

		// Token: 0x04003391 RID: 13201
		private bool disposed;

		// Token: 0x04003392 RID: 13202
		private Process.StreamReadMode outputStreamReadMode;

		// Token: 0x04003393 RID: 13203
		private Process.StreamReadMode errorStreamReadMode;

		// Token: 0x04003396 RID: 13206
		internal AsyncStreamReader output;

		// Token: 0x04003397 RID: 13207
		internal AsyncStreamReader error;

		// Token: 0x04003398 RID: 13208
		internal bool pendingOutputRead;

		// Token: 0x04003399 RID: 13209
		internal bool pendingErrorRead;

		// Token: 0x0400339A RID: 13210
		private static SafeFileHandle InvalidPipeHandle = new SafeFileHandle(IntPtr.Zero, false);

		// Token: 0x0400339B RID: 13211
		internal static TraceSwitch processTracing = null;

		// Token: 0x02000773 RID: 1907
		private enum StreamReadMode
		{
			// Token: 0x0400339D RID: 13213
			undefined,
			// Token: 0x0400339E RID: 13214
			syncMode,
			// Token: 0x0400339F RID: 13215
			asyncMode
		}

		// Token: 0x02000774 RID: 1908
		private enum State
		{
			// Token: 0x040033A1 RID: 13217
			HaveId = 1,
			// Token: 0x040033A2 RID: 13218
			IsLocal,
			// Token: 0x040033A3 RID: 13219
			IsNt = 4,
			// Token: 0x040033A4 RID: 13220
			HaveProcessInfo = 8,
			// Token: 0x040033A5 RID: 13221
			Exited = 16,
			// Token: 0x040033A6 RID: 13222
			Associated = 32,
			// Token: 0x040033A7 RID: 13223
			IsWin2k = 64,
			// Token: 0x040033A8 RID: 13224
			HaveNtProcessInfo = 12
		}
	}
}
