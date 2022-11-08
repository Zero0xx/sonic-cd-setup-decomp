using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
	// Token: 0x0200074B RID: 1867
	[MonitoringDescription("EventLogDesc")]
	[DefaultEvent("EntryWritten")]
	[InstallerType("System.Diagnostics.EventLogInstaller, System.Configuration.Install, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class EventLog : Component, ISupportInitialize
	{
		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x060038DF RID: 14559 RVA: 0x000F0140 File Offset: 0x000EF140
		private static object InternalSyncObject
		{
			get
			{
				if (EventLog.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref EventLog.s_InternalSyncObject, value, null);
				}
				return EventLog.s_InternalSyncObject;
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x060038E0 RID: 14560 RVA: 0x000F016C File Offset: 0x000EF16C
		private static bool SkipRegPatch
		{
			get
			{
				if (!EventLog.s_CheckedOsVersion)
				{
					OperatingSystem osversion = Environment.OSVersion;
					EventLog.s_SkipRegPatch = (osversion.Platform == PlatformID.Win32NT && osversion.Version.Major > 5);
					EventLog.s_CheckedOsVersion = true;
				}
				return EventLog.s_SkipRegPatch;
			}
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x000F01B0 File Offset: 0x000EF1B0
		public EventLog() : this("", ".", "")
		{
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x000F01C7 File Offset: 0x000EF1C7
		public EventLog(string logName) : this(logName, ".", "")
		{
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x000F01DA File Offset: 0x000EF1DA
		public EventLog(string logName, string machineName) : this(logName, machineName, "")
		{
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x000F01EC File Offset: 0x000EF1EC
		public EventLog(string logName, string machineName, string source)
		{
			if (logName == null)
			{
				throw new ArgumentNullException("logName");
			}
			if (!EventLog.ValidLogName(logName, true))
			{
				throw new ArgumentException(SR.GetString("BadLogName"));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"machineName",
					machineName
				}));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, machineName);
			eventLogPermission.Demand();
			this.machineName = machineName;
			this.logName = logName;
			this.sourceName = source;
			this.readHandle = null;
			this.writeHandle = null;
			this.boolFlags[2] = true;
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x060038E5 RID: 14565 RVA: 0x000F02A4 File Offset: 0x000EF2A4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("LogEntries")]
		public EventLogEntryCollection Entries
		{
			get
			{
				string text = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, text);
				eventLogPermission.Demand();
				if (this.entriesCollection == null)
				{
					this.entriesCollection = new EventLogEntryCollection(this);
				}
				return this.entriesCollection;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x060038E6 RID: 14566 RVA: 0x000F02E4 File Offset: 0x000EF2E4
		internal int EntryCount
		{
			get
			{
				if (!this.IsOpenForRead)
				{
					this.OpenForRead(this.machineName);
				}
				int result;
				if (!UnsafeNativeMethods.GetNumberOfEventLogRecords(this.readHandle, out result))
				{
					throw SharedUtils.CreateSafeWin32Exception();
				}
				return result;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x060038E7 RID: 14567 RVA: 0x000F031D File Offset: 0x000EF31D
		private bool IsOpen
		{
			get
			{
				return this.readHandle != null || this.writeHandle != null;
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x060038E8 RID: 14568 RVA: 0x000F0335 File Offset: 0x000EF335
		private bool IsOpenForRead
		{
			get
			{
				return this.readHandle != null;
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x060038E9 RID: 14569 RVA: 0x000F0343 File Offset: 0x000EF343
		private bool IsOpenForWrite
		{
			get
			{
				return this.writeHandle != null;
			}
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x000F0354 File Offset: 0x000EF354
		private static PermissionSet _GetAssertPermSet()
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			RegistryPermission perm = new RegistryPermission(PermissionState.Unrestricted);
			permissionSet.AddPermission(perm);
			EnvironmentPermission perm2 = new EnvironmentPermission(PermissionState.Unrestricted);
			permissionSet.AddPermission(perm2);
			return permissionSet;
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x060038EB RID: 14571 RVA: 0x000F0388 File Offset: 0x000EF388
		[Browsable(false)]
		public string LogDisplayName
		{
			get
			{
				if (this.logDisplayName == null)
				{
					string text = this.machineName;
					if (this.GetLogName(text) != null)
					{
						EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, text);
						eventLogPermission.Demand();
						SharedUtils.CheckEnvironment();
						PermissionSet permissionSet = EventLog._GetAssertPermSet();
						permissionSet.Assert();
						RegistryKey registryKey = null;
						try
						{
							registryKey = this.GetLogRegKey(text, false);
							if (registryKey == null)
							{
								throw new InvalidOperationException(SR.GetString("MissingLog", new object[]
								{
									this.GetLogName(text),
									text
								}));
							}
							string text2 = (string)registryKey.GetValue("DisplayNameFile");
							if (text2 == null)
							{
								this.logDisplayName = this.GetLogName(text);
							}
							else
							{
								int messageNum = (int)registryKey.GetValue("DisplayNameID");
								this.logDisplayName = this.FormatMessageWrapper(text2, (uint)messageNum, null);
								if (this.logDisplayName == null)
								{
									this.logDisplayName = this.GetLogName(text);
								}
							}
						}
						finally
						{
							if (registryKey != null)
							{
								registryKey.Close();
							}
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				return this.logDisplayName;
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x060038EC RID: 14572 RVA: 0x000F0490 File Offset: 0x000EF490
		// (set) Token: 0x060038ED RID: 14573 RVA: 0x000F049E File Offset: 0x000EF49E
		[RecommendedAsConfigurable(true)]
		[ReadOnly(true)]
		[MonitoringDescription("LogLog")]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.LogConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Log
		{
			get
			{
				return this.GetLogName(this.machineName);
			}
			set
			{
				this.SetLogName(this.machineName, value);
			}
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x000F04B0 File Offset: 0x000EF4B0
		private string GetLogName(string currentMachineName)
		{
			if ((this.logName == null || this.logName.Length == 0) && this.sourceName != null && this.sourceName.Length != 0)
			{
				this.logName = EventLog.LogNameFromSourceName(this.sourceName, currentMachineName);
			}
			return this.logName;
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x000F0500 File Offset: 0x000EF500
		private void SetLogName(string currentMachineName, string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!EventLog.ValidLogName(value, true))
			{
				throw new ArgumentException(SR.GetString("BadLogName"));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, currentMachineName);
			eventLogPermission.Demand();
			if (value == null)
			{
				value = string.Empty;
			}
			if (this.logName == null)
			{
				this.logName = value;
				return;
			}
			if (string.Compare(this.logName, value, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return;
			}
			this.logDisplayName = null;
			this.logName = value;
			if (this.IsOpen)
			{
				bool enableRaisingEvents = this.EnableRaisingEvents;
				this.Close(currentMachineName);
				this.EnableRaisingEvents = enableRaisingEvents;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x060038F0 RID: 14576 RVA: 0x000F0598 File Offset: 0x000EF598
		// (set) Token: 0x060038F1 RID: 14577 RVA: 0x000F05BC File Offset: 0x000EF5BC
		[RecommendedAsConfigurable(true)]
		[DefaultValue(".")]
		[ReadOnly(true)]
		[MonitoringDescription("LogMachineName")]
		public string MachineName
		{
			get
			{
				string result = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, result);
				eventLogPermission.Demand();
				return result;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[]
					{
						"MachineName",
						value
					}));
				}
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, value);
				eventLogPermission.Demand();
				string text = this.machineName;
				if (text != null)
				{
					if (string.Compare(text, value, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return;
					}
					this.boolFlags[32] = false;
					if (this.IsOpen)
					{
						this.Close(text);
					}
				}
				this.machineName = value;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x060038F2 RID: 14578 RVA: 0x000F0640 File Offset: 0x000EF640
		// (set) Token: 0x060038F3 RID: 14579 RVA: 0x000F068C File Offset: 0x000EF68C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[ComVisible(false)]
		public long MaximumKilobytes
		{
			get
			{
				string currentMachineName = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, currentMachineName);
				eventLogPermission.Demand();
				object logRegValue = this.GetLogRegValue(currentMachineName, "MaxSize");
				if (logRegValue != null)
				{
					int num = (int)logRegValue;
					return (long)((ulong)(num / 1024));
				}
				return 512L;
			}
			set
			{
				string currentMachineName = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, currentMachineName);
				eventLogPermission.Demand();
				if (value < 64L || value > 4194240L || value % 64L != 0L)
				{
					throw new ArgumentOutOfRangeException("MaximumKilobytes", SR.GetString("MaximumKilobytesOutOfRange"));
				}
				PermissionSet permissionSet = EventLog._GetAssertPermSet();
				permissionSet.Assert();
				long num = value * 1024L;
				int num2 = (int)num;
				using (RegistryKey logRegKey = this.GetLogRegKey(currentMachineName, true))
				{
					logRegKey.SetValue("MaxSize", num2, RegistryValueKind.DWord);
				}
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x060038F4 RID: 14580 RVA: 0x000F0734 File Offset: 0x000EF734
		internal Hashtable MessageLibraries
		{
			get
			{
				if (this.messageLibraries == null)
				{
					this.messageLibraries = new Hashtable(StringComparer.OrdinalIgnoreCase);
				}
				return this.messageLibraries;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x060038F5 RID: 14581 RVA: 0x000F0754 File Offset: 0x000EF754
		[ComVisible(false)]
		[Browsable(false)]
		public OverflowAction OverflowAction
		{
			get
			{
				string currentMachineName = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, currentMachineName);
				eventLogPermission.Demand();
				object logRegValue = this.GetLogRegValue(currentMachineName, "Retention");
				if (logRegValue == null)
				{
					return OverflowAction.OverwriteOlder;
				}
				int num = (int)logRegValue;
				if (num == 0)
				{
					return OverflowAction.OverwriteAsNeeded;
				}
				if (num == -1)
				{
					return OverflowAction.DoNotOverwrite;
				}
				return OverflowAction.OverwriteOlder;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x000F079C File Offset: 0x000EF79C
		[ComVisible(false)]
		[Browsable(false)]
		public int MinimumRetentionDays
		{
			get
			{
				string currentMachineName = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, currentMachineName);
				eventLogPermission.Demand();
				object logRegValue = this.GetLogRegValue(currentMachineName, "Retention");
				if (logRegValue == null)
				{
					return 7;
				}
				int num = (int)logRegValue;
				if (num == 0 || num == -1)
				{
					return num;
				}
				return (int)((double)num / 86400.0);
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x000F07F0 File Offset: 0x000EF7F0
		// (set) Token: 0x060038F8 RID: 14584 RVA: 0x000F0820 File Offset: 0x000EF820
		[Browsable(false)]
		[MonitoringDescription("LogMonitoring")]
		[DefaultValue(false)]
		public bool EnableRaisingEvents
		{
			get
			{
				string text = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, text);
				eventLogPermission.Demand();
				return this.boolFlags[8];
			}
			set
			{
				string currentMachineName = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, currentMachineName);
				eventLogPermission.Demand();
				if (base.DesignMode)
				{
					this.boolFlags[8] = value;
					return;
				}
				if (value)
				{
					this.StartRaisingEvents(currentMachineName, this.GetLogName(currentMachineName));
					return;
				}
				this.StopRaisingEvents(this.GetLogName(currentMachineName));
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000F0878 File Offset: 0x000EF878
		private int OldestEntryNumber
		{
			get
			{
				if (!this.IsOpenForRead)
				{
					this.OpenForRead(this.machineName);
				}
				int[] array = new int[1];
				if (!UnsafeNativeMethods.GetOldestEventLogRecord(this.readHandle, array))
				{
					throw SharedUtils.CreateSafeWin32Exception();
				}
				int num = array[0];
				if (num == 0)
				{
					num = 1;
				}
				return num;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x060038FA RID: 14586 RVA: 0x000F08C0 File Offset: 0x000EF8C0
		internal SafeEventLogReadHandle ReadHandle
		{
			get
			{
				if (!this.IsOpenForRead)
				{
					this.OpenForRead(this.machineName);
				}
				return this.readHandle;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x060038FB RID: 14587 RVA: 0x000F08DC File Offset: 0x000EF8DC
		// (set) Token: 0x060038FC RID: 14588 RVA: 0x000F094C File Offset: 0x000EF94C
		[Browsable(false)]
		[MonitoringDescription("LogSynchronizingObject")]
		[DefaultValue(null)]
		public ISynchronizeInvoke SynchronizingObject
		{
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
			get
			{
				string text = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, text);
				eventLogPermission.Demand();
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

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x000F0958 File Offset: 0x000EF958
		// (set) Token: 0x060038FE RID: 14590 RVA: 0x000F0984 File Offset: 0x000EF984
		[RecommendedAsConfigurable(true)]
		[ReadOnly(true)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[MonitoringDescription("LogSource")]
		[DefaultValue("")]
		public string Source
		{
			get
			{
				string text = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, text);
				eventLogPermission.Demand();
				return this.sourceName;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length + "SYSTEM\\CurrentControlSet\\Services\\EventLog".Length > 254)
				{
					throw new ArgumentException(SR.GetString("ParameterTooLong", new object[]
					{
						"source",
						254 - "SYSTEM\\CurrentControlSet\\Services\\EventLog".Length
					}));
				}
				string currentMachineName = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, currentMachineName);
				eventLogPermission.Demand();
				if (this.sourceName == null)
				{
					this.sourceName = value;
					return;
				}
				if (string.Compare(this.sourceName, value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return;
				}
				this.sourceName = value;
				if (this.IsOpen)
				{
					bool enableRaisingEvents = this.EnableRaisingEvents;
					this.Close(currentMachineName);
					this.EnableRaisingEvents = enableRaisingEvents;
				}
			}
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x000F0A48 File Offset: 0x000EFA48
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		private static void AddListenerComponent(EventLog component, string compMachineName, string compLogName)
		{
			lock (EventLog.InternalSyncObject)
			{
				EventLog.LogListeningInfo logListeningInfo = (EventLog.LogListeningInfo)EventLog.listenerInfos[compLogName];
				if (logListeningInfo != null)
				{
					logListeningInfo.listeningComponents.Add(component);
				}
				else
				{
					logListeningInfo = new EventLog.LogListeningInfo();
					logListeningInfo.listeningComponents.Add(component);
					logListeningInfo.handleOwner = new EventLog();
					logListeningInfo.handleOwner.MachineName = compMachineName;
					logListeningInfo.handleOwner.Log = compLogName;
					SafeEventHandle safeEventHandle = SafeEventHandle.CreateEvent(NativeMethods.NullHandleRef, false, false, null);
					if (safeEventHandle.IsInvalid)
					{
						Win32Exception innerException = null;
						if (Marshal.GetLastWin32Error() != 0)
						{
							innerException = SharedUtils.CreateSafeWin32Exception();
						}
						throw new InvalidOperationException(SR.GetString("NotifyCreateFailed"), innerException);
					}
					if (!UnsafeNativeMethods.NotifyChangeEventLog(logListeningInfo.handleOwner.ReadHandle, safeEventHandle))
					{
						throw new InvalidOperationException(SR.GetString("CantMonitorEventLog"), SharedUtils.CreateSafeWin32Exception());
					}
					logListeningInfo.waitHandle = new EventLog.EventLogWaitHandle(safeEventHandle);
					logListeningInfo.registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(logListeningInfo.waitHandle, new WaitOrTimerCallback(EventLog.StaticCompletionCallback), logListeningInfo, -1, false);
					EventLog.listenerInfos[compLogName] = logListeningInfo;
				}
			}
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06003900 RID: 14592 RVA: 0x000F0B70 File Offset: 0x000EFB70
		// (remove) Token: 0x06003901 RID: 14593 RVA: 0x000F0BAC File Offset: 0x000EFBAC
		[MonitoringDescription("LogEntryWritten")]
		public event EntryWrittenEventHandler EntryWritten
		{
			add
			{
				string text = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, text);
				eventLogPermission.Demand();
				this.onEntryWrittenHandler = (EntryWrittenEventHandler)Delegate.Combine(this.onEntryWrittenHandler, value);
			}
			remove
			{
				string text = this.machineName;
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, text);
				eventLogPermission.Demand();
				this.onEntryWrittenHandler = (EntryWrittenEventHandler)Delegate.Remove(this.onEntryWrittenHandler, value);
			}
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x000F0BE8 File Offset: 0x000EFBE8
		public void BeginInit()
		{
			string currentMachineName = this.machineName;
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, currentMachineName);
			eventLogPermission.Demand();
			if (this.boolFlags[4])
			{
				throw new InvalidOperationException(SR.GetString("InitTwice"));
			}
			this.boolFlags[4] = true;
			if (this.boolFlags[8])
			{
				this.StopListening(this.GetLogName(currentMachineName));
			}
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x000F0C54 File Offset: 0x000EFC54
		public void Clear()
		{
			string currentMachineName = this.machineName;
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, currentMachineName);
			eventLogPermission.Demand();
			if (!this.IsOpenForRead)
			{
				this.OpenForRead(currentMachineName);
			}
			if (!UnsafeNativeMethods.ClearEventLog(this.readHandle, NativeMethods.NullHandleRef))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != 2)
				{
					throw SharedUtils.CreateSafeWin32Exception();
				}
			}
			this.Reset(currentMachineName);
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x000F0CB1 File Offset: 0x000EFCB1
		public void Close()
		{
			this.Close(this.machineName);
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x000F0CC0 File Offset: 0x000EFCC0
		private void Close(string currentMachineName)
		{
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, currentMachineName);
			eventLogPermission.Demand();
			if (this.readHandle != null)
			{
				try
				{
					this.readHandle.Close();
				}
				catch (IOException)
				{
					throw SharedUtils.CreateSafeWin32Exception();
				}
				this.readHandle = null;
			}
			if (this.writeHandle != null)
			{
				try
				{
					this.writeHandle.Close();
				}
				catch (IOException)
				{
					throw SharedUtils.CreateSafeWin32Exception();
				}
				this.writeHandle = null;
			}
			if (this.boolFlags[8])
			{
				this.StopRaisingEvents(this.GetLogName(currentMachineName));
			}
			if (this.messageLibraries != null)
			{
				foreach (object obj in this.messageLibraries.Values)
				{
					SafeLibraryHandle safeLibraryHandle = (SafeLibraryHandle)obj;
					safeLibraryHandle.Close();
				}
				this.messageLibraries = null;
			}
			this.boolFlags[512] = false;
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x000F0DC8 File Offset: 0x000EFDC8
		private void CompletionCallback(object context)
		{
			if (this.boolFlags[256])
			{
				return;
			}
			lock (this)
			{
				if (this.boolFlags[1])
				{
					return;
				}
				this.boolFlags[1] = true;
			}
			int i = this.lastSeenCount;
			try
			{
				int oldestEntryNumber = this.OldestEntryNumber;
				int num = this.EntryCount + oldestEntryNumber;
				while (i < num)
				{
					while (i < num)
					{
						EventLogEntry entryWithOldest = this.GetEntryWithOldest(i);
						if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
						{
							this.SynchronizingObject.BeginInvoke(this.onEntryWrittenHandler, new object[]
							{
								this,
								new EntryWrittenEventArgs(entryWithOldest)
							});
						}
						else
						{
							this.onEntryWrittenHandler(this, new EntryWrittenEventArgs(entryWithOldest));
						}
						i++;
					}
					oldestEntryNumber = this.OldestEntryNumber;
					num = this.EntryCount + oldestEntryNumber;
				}
			}
			catch (Exception)
			{
			}
			catch
			{
			}
			try
			{
				int num2 = this.EntryCount + this.OldestEntryNumber;
				if (i > num2)
				{
					this.lastSeenCount = num2;
				}
				else
				{
					this.lastSeenCount = i;
				}
			}
			catch (Win32Exception)
			{
			}
			lock (this)
			{
				this.boolFlags[1] = false;
			}
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x000F0F40 File Offset: 0x000EFF40
		public static void CreateEventSource(string source, string logName)
		{
			EventLog.CreateEventSource(new EventSourceCreationData(source, logName, "."));
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x000F0F53 File Offset: 0x000EFF53
		[Obsolete("This method has been deprecated.  Please use System.Diagnostics.EventLog.CreateEventSource(EventSourceCreationData sourceData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static void CreateEventSource(string source, string logName, string machineName)
		{
			EventLog.CreateEventSource(new EventSourceCreationData(source, logName, machineName));
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x000F0F64 File Offset: 0x000EFF64
		public static void CreateEventSource(EventSourceCreationData sourceData)
		{
			if (sourceData == null)
			{
				throw new ArgumentNullException("sourceData");
			}
			string text = sourceData.LogName;
			string source = sourceData.Source;
			string text2 = sourceData.MachineName;
			if (!SyntaxCheck.CheckMachineName(text2))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"machineName",
					text2
				}));
			}
			if (text == null || text.Length == 0)
			{
				text = "Application";
			}
			if (!EventLog.ValidLogName(text, false))
			{
				throw new ArgumentException(SR.GetString("BadLogName"));
			}
			if (source == null || source.Length == 0)
			{
				throw new ArgumentException(SR.GetString("MissingParameter", new object[]
				{
					"source"
				}));
			}
			if (source.Length + "SYSTEM\\CurrentControlSet\\Services\\EventLog".Length > 254)
			{
				throw new ArgumentException(SR.GetString("ParameterTooLong", new object[]
				{
					"source",
					254 - "SYSTEM\\CurrentControlSet\\Services\\EventLog".Length
				}));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, text2);
			eventLogPermission.Demand();
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutex("netfxeventlog.1.0", ref mutex);
				if (EventLog.SourceExists(source, text2))
				{
					if (".".Equals(text2))
					{
						throw new ArgumentException(SR.GetString("LocalSourceAlreadyExists", new object[]
						{
							source
						}));
					}
					throw new ArgumentException(SR.GetString("SourceAlreadyExists", new object[]
					{
						source,
						text2
					}));
				}
				else
				{
					PermissionSet permissionSet = EventLog._GetAssertPermSet();
					permissionSet.Assert();
					RegistryKey registryKey = null;
					RegistryKey registryKey2 = null;
					RegistryKey registryKey3 = null;
					RegistryKey registryKey4 = null;
					RegistryKey registryKey5 = null;
					try
					{
						if (text2 == ".")
						{
							registryKey = Registry.LocalMachine;
						}
						else
						{
							registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, text2);
						}
						registryKey2 = registryKey.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\EventLog", true);
						if (registryKey2 == null)
						{
							if (!".".Equals(text2))
							{
								throw new InvalidOperationException(SR.GetString("RegKeyMissing", new object[]
								{
									"SYSTEM\\CurrentControlSet\\Services\\EventLog",
									text,
									source,
									text2
								}));
							}
							throw new InvalidOperationException(SR.GetString("LocalRegKeyMissing", new object[]
							{
								"SYSTEM\\CurrentControlSet\\Services\\EventLog",
								text,
								source
							}));
						}
						else
						{
							registryKey3 = registryKey2.OpenSubKey(text, true);
							if (registryKey3 == null && text.Length >= 8)
							{
								string strA = text.Substring(0, 8);
								if (string.Compare(strA, "AppEvent", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(strA, "SecEvent", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(strA, "SysEvent", StringComparison.OrdinalIgnoreCase) == 0)
								{
									throw new ArgumentException(SR.GetString("InvalidCustomerLogName", new object[]
									{
										text
									}));
								}
								string text3 = EventLog.FindSame8FirstCharsLog(registryKey2, text);
								if (text3 != null)
								{
									throw new ArgumentException(SR.GetString("DuplicateLogName", new object[]
									{
										text,
										text3
									}));
								}
							}
							bool flag = registryKey3 == null;
							if (flag)
							{
								if (EventLog.SourceExists(text, text2))
								{
									if (".".Equals(text2))
									{
										throw new ArgumentException(SR.GetString("LocalLogAlreadyExistsAsSource", new object[]
										{
											text
										}));
									}
									throw new ArgumentException(SR.GetString("LogAlreadyExistsAsSource", new object[]
									{
										text,
										text2
									}));
								}
								else
								{
									registryKey3 = registryKey2.CreateSubKey(text);
									if (!EventLog.SkipRegPatch)
									{
										registryKey3.SetValue("Sources", new string[]
										{
											text,
											source
										}, RegistryValueKind.MultiString);
									}
									EventLog.SetSpecialLogRegValues(registryKey3, text);
									registryKey4 = registryKey3.CreateSubKey(text);
									EventLog.SetSpecialSourceRegValues(registryKey4, sourceData);
								}
							}
							if (text != source)
							{
								if (!flag)
								{
									EventLog.SetSpecialLogRegValues(registryKey3, text);
									if (!EventLog.SkipRegPatch)
									{
										string[] array = registryKey3.GetValue("Sources") as string[];
										if (array == null)
										{
											registryKey3.SetValue("Sources", new string[]
											{
												text,
												source
											}, RegistryValueKind.MultiString);
										}
										else if (Array.IndexOf<string>(array, source) == -1)
										{
											string[] array2 = new string[array.Length + 1];
											Array.Copy(array, array2, array.Length);
											array2[array.Length] = source;
											registryKey3.SetValue("Sources", array2, RegistryValueKind.MultiString);
										}
									}
								}
								registryKey5 = registryKey3.CreateSubKey(source);
								EventLog.SetSpecialSourceRegValues(registryKey5, sourceData);
							}
						}
					}
					finally
					{
						if (registryKey != null)
						{
							registryKey.Close();
						}
						if (registryKey2 != null)
						{
							registryKey2.Close();
						}
						if (registryKey3 != null)
						{
							registryKey3.Flush();
							registryKey3.Close();
						}
						if (registryKey4 != null)
						{
							registryKey4.Flush();
							registryKey4.Close();
						}
						if (registryKey5 != null)
						{
							registryKey5.Flush();
							registryKey5.Close();
						}
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x000F144C File Offset: 0x000F044C
		public static void Delete(string logName)
		{
			EventLog.Delete(logName, ".");
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x000F145C File Offset: 0x000F045C
		public static void Delete(string logName, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameterFormat", new object[]
				{
					"machineName"
				}));
			}
			if (logName == null || logName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("NoLogName"));
			}
			if (!EventLog.ValidLogName(logName, false))
			{
				throw new InvalidOperationException(SR.GetString("BadLogName"));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			SharedUtils.CheckEnvironment();
			PermissionSet permissionSet = EventLog._GetAssertPermSet();
			permissionSet.Assert();
			RegistryKey registryKey = null;
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutex("netfxeventlog.1.0", ref mutex);
				try
				{
					registryKey = EventLog.GetEventLogRegKey(machineName, true);
					if (registryKey == null)
					{
						throw new InvalidOperationException(SR.GetString("RegKeyNoAccess", new object[]
						{
							"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\EventLog",
							machineName
						}));
					}
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(logName))
					{
						if (registryKey2 == null)
						{
							throw new InvalidOperationException(SR.GetString("MissingLog", new object[]
							{
								logName,
								machineName
							}));
						}
						EventLog eventLog = new EventLog();
						try
						{
							eventLog.Log = logName;
							eventLog.MachineName = machineName;
							eventLog.Clear();
						}
						finally
						{
							eventLog.Close();
						}
						string text = null;
						try
						{
							text = (string)registryKey2.GetValue("File");
						}
						catch
						{
						}
						if (text != null)
						{
							try
							{
								File.Delete(text);
							}
							catch
							{
							}
						}
					}
					registryKey.DeleteSubKeyTree(logName);
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Close();
					}
					CodeAccessPermission.RevertAssert();
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
				}
			}
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x000F1634 File Offset: 0x000F0634
		public static void DeleteEventSource(string source)
		{
			EventLog.DeleteEventSource(source, ".");
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x000F1644 File Offset: 0x000F0644
		public static void DeleteEventSource(string source, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"machineName",
					machineName
				}));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			SharedUtils.CheckEnvironment();
			PermissionSet permissionSet = EventLog._GetAssertPermSet();
			permissionSet.Assert();
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutex("netfxeventlog.1.0", ref mutex);
				RegistryKey registryKey = null;
				RegistryKey registryKey2;
				registryKey = (registryKey2 = EventLog.FindSourceRegistration(source, machineName, true));
				try
				{
					if (registryKey == null)
					{
						if (machineName == null)
						{
							throw new ArgumentException(SR.GetString("LocalSourceNotRegistered", new object[]
							{
								source
							}));
						}
						throw new ArgumentException(SR.GetString("SourceNotRegistered", new object[]
						{
							source,
							machineName,
							"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\EventLog"
						}));
					}
					else
					{
						string name = registryKey.Name;
						int num = name.LastIndexOf('\\');
						if (string.Compare(name, num + 1, source, 0, name.Length - num, StringComparison.Ordinal) == 0)
						{
							throw new InvalidOperationException(SR.GetString("CannotDeleteEqualSource", new object[]
							{
								source
							}));
						}
					}
				}
				finally
				{
					if (registryKey2 != null)
					{
						((IDisposable)registryKey2).Dispose();
					}
				}
				try
				{
					registryKey = EventLog.FindSourceRegistration(source, machineName, false);
					registryKey.DeleteSubKeyTree(source);
					if (!EventLog.SkipRegPatch)
					{
						string[] array = (string[])registryKey.GetValue("Sources");
						ArrayList arrayList = new ArrayList(array.Length - 1);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] != source)
							{
								arrayList.Add(array[i]);
							}
						}
						string[] array2 = new string[arrayList.Count];
						arrayList.CopyTo(array2);
						registryKey.SetValue("Sources", array2, RegistryValueKind.MultiString);
					}
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Flush();
						registryKey.Close();
					}
					CodeAccessPermission.RevertAssert();
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
				}
			}
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x000F1868 File Offset: 0x000F0868
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.IsOpen)
				{
					this.Close();
				}
			}
			else
			{
				if (this.readHandle != null)
				{
					this.readHandle.Close();
				}
				if (this.writeHandle != null)
				{
					this.writeHandle.Close();
				}
				this.messageLibraries = null;
			}
			this.boolFlags[256] = true;
			base.Dispose(disposing);
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x000F18D0 File Offset: 0x000F08D0
		public void EndInit()
		{
			string currentMachineName = this.machineName;
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, currentMachineName);
			eventLogPermission.Demand();
			this.boolFlags[4] = false;
			if (this.boolFlags[8])
			{
				this.StartListening(currentMachineName, this.GetLogName(currentMachineName));
			}
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x000F191C File Offset: 0x000F091C
		public static bool Exists(string logName)
		{
			return EventLog.Exists(logName, ".");
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x000F192C File Offset: 0x000F092C
		public static bool Exists(string logName, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameterFormat", new object[]
				{
					"machineName"
				}));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			if (logName == null || logName.Length == 0)
			{
				return false;
			}
			SharedUtils.CheckEnvironment();
			PermissionSet permissionSet = EventLog._GetAssertPermSet();
			permissionSet.Assert();
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			bool result;
			try
			{
				registryKey = EventLog.GetEventLogRegKey(machineName, false);
				if (registryKey == null)
				{
					result = false;
				}
				else
				{
					registryKey2 = registryKey.OpenSubKey(logName, false);
					result = (registryKey2 != null);
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x000F19E4 File Offset: 0x000F09E4
		private static string FindSame8FirstCharsLog(RegistryKey keyParent, string logName)
		{
			string strB = logName.Substring(0, 8);
			foreach (string text in keyParent.GetSubKeyNames())
			{
				if (text.Length >= 8 && string.Compare(text.Substring(0, 8), strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x000F1A30 File Offset: 0x000F0A30
		private static RegistryKey FindSourceRegistration(string source, string machineName, bool readOnly)
		{
			if (source != null && source.Length != 0)
			{
				SharedUtils.CheckEnvironment();
				PermissionSet permissionSet = EventLog._GetAssertPermSet();
				permissionSet.Assert();
				RegistryKey registryKey = null;
				try
				{
					registryKey = EventLog.GetEventLogRegKey(machineName, !readOnly);
					if (registryKey == null)
					{
						return null;
					}
					StringBuilder stringBuilder = null;
					string[] subKeyNames = registryKey.GetSubKeyNames();
					for (int i = 0; i < subKeyNames.Length; i++)
					{
						RegistryKey registryKey2 = null;
						try
						{
							RegistryKey registryKey3 = registryKey.OpenSubKey(subKeyNames[i], !readOnly);
							if (registryKey3 != null)
							{
								registryKey2 = registryKey3.OpenSubKey(source, !readOnly);
								if (registryKey2 != null)
								{
									return registryKey3;
								}
							}
						}
						catch (UnauthorizedAccessException)
						{
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(subKeyNames[i]);
							}
							else
							{
								stringBuilder.Append(", ");
								stringBuilder.Append(subKeyNames[i]);
							}
						}
						catch (SecurityException)
						{
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(subKeyNames[i]);
							}
							else
							{
								stringBuilder.Append(", ");
								stringBuilder.Append(subKeyNames[i]);
							}
						}
						finally
						{
							if (registryKey2 != null)
							{
								registryKey2.Close();
							}
						}
					}
					if (stringBuilder != null)
					{
						throw new SecurityException(SR.GetString("SomeLogsInaccessible", new object[]
						{
							stringBuilder.ToString()
						}));
					}
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Close();
					}
					CodeAccessPermission.RevertAssert();
				}
			}
			return null;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x000F1B9C File Offset: 0x000F0B9C
		internal string FormatMessageWrapper(string dllNameList, uint messageNum, string[] insertionStrings)
		{
			if (dllNameList == null)
			{
				return null;
			}
			if (insertionStrings == null)
			{
				insertionStrings = new string[0];
			}
			string[] array = dllNameList.Split(new char[]
			{
				';'
			});
			foreach (string text in array)
			{
				if (text != null && text.Length != 0)
				{
					SafeLibraryHandle safeLibraryHandle = null;
					if (this.IsOpen)
					{
						safeLibraryHandle = (this.MessageLibraries[text] as SafeLibraryHandle);
						if (safeLibraryHandle == null || safeLibraryHandle.IsInvalid)
						{
							safeLibraryHandle = SafeLibraryHandle.LoadLibraryEx(text, IntPtr.Zero, 2);
							this.MessageLibraries[text] = safeLibraryHandle;
						}
					}
					else
					{
						safeLibraryHandle = SafeLibraryHandle.LoadLibraryEx(text, IntPtr.Zero, 2);
					}
					if (!safeLibraryHandle.IsInvalid)
					{
						string text2 = null;
						try
						{
							text2 = EventLog.TryFormatMessage(safeLibraryHandle, messageNum, insertionStrings);
						}
						finally
						{
							if (!this.IsOpen)
							{
								safeLibraryHandle.Close();
							}
						}
						if (text2 != null)
						{
							return text2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x000F1C90 File Offset: 0x000F0C90
		internal EventLogEntry[] GetAllEntries()
		{
			string currentMachineName = this.machineName;
			if (!this.IsOpenForRead)
			{
				this.OpenForRead(currentMachineName);
			}
			EventLogEntry[] array = new EventLogEntry[this.EntryCount];
			int i = 0;
			int oldestEntryNumber = this.OldestEntryNumber;
			int[] array2 = new int[1];
			int[] array3 = new int[]
			{
				40000
			};
			int num = 0;
			while (i < array.Length)
			{
				byte[] array4 = new byte[40000];
				if (!UnsafeNativeMethods.ReadEventLog(this.readHandle, 6, oldestEntryNumber + i, array4, array4.Length, array2, array3))
				{
					num = Marshal.GetLastWin32Error();
					if (num != 122 && num != 1503)
					{
						break;
					}
					if (num == 1503)
					{
						this.Reset(currentMachineName);
					}
					else if (array3[0] > array4.Length)
					{
						array4 = new byte[array3[0]];
					}
					bool flag = UnsafeNativeMethods.ReadEventLog(this.readHandle, 6, oldestEntryNumber + i, array4, array4.Length, array2, array3);
					if (!flag)
					{
						break;
					}
					num = 0;
				}
				array[i] = new EventLogEntry(array4, 0, this);
				int num2 = EventLog.IntFrom(array4, 0);
				i++;
				while (num2 < array2[0] && i < array.Length)
				{
					array[i] = new EventLogEntry(array4, num2, this);
					num2 += EventLog.IntFrom(array4, num2);
					i++;
				}
			}
			if (i == array.Length)
			{
				return array;
			}
			if (num != 0)
			{
				throw new InvalidOperationException(SR.GetString("CantRetrieveEntries"), SharedUtils.CreateSafeWin32Exception(num));
			}
			throw new InvalidOperationException(SR.GetString("CantRetrieveEntries"));
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x000F1DFE File Offset: 0x000F0DFE
		public static EventLog[] GetEventLogs()
		{
			return EventLog.GetEventLogs(".");
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x000F1E0C File Offset: 0x000F0E0C
		public static EventLog[] GetEventLogs(string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"machineName",
					machineName
				}));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			SharedUtils.CheckEnvironment();
			string[] array = new string[0];
			PermissionSet permissionSet = EventLog._GetAssertPermSet();
			permissionSet.Assert();
			RegistryKey registryKey = null;
			try
			{
				registryKey = EventLog.GetEventLogRegKey(machineName, false);
				if (registryKey == null)
				{
					throw new InvalidOperationException(SR.GetString("RegKeyMissingShort", new object[]
					{
						"SYSTEM\\CurrentControlSet\\Services\\EventLog",
						machineName
					}));
				}
				array = registryKey.GetSubKeyNames();
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			if (EventLog.s_dontFilterRegKeys || machineName != ".")
			{
				EventLog[] array2 = new EventLog[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = new EventLog
					{
						Log = array[i],
						MachineName = machineName
					};
				}
				return array2;
			}
			List<EventLog> list = new List<EventLog>(array.Length);
			for (int j = 0; j < array.Length; j++)
			{
				EventLog eventLog = new EventLog();
				eventLog.Log = array[j];
				eventLog.MachineName = machineName;
				SafeEventLogReadHandle safeEventLogReadHandle = SafeEventLogReadHandle.OpenEventLog(machineName, array[j]);
				if (!safeEventLogReadHandle.IsInvalid)
				{
					safeEventLogReadHandle.Close();
					list.Add(eventLog);
				}
				else if (Marshal.GetLastWin32Error() != 87)
				{
					list.Add(eventLog);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x000F1F98 File Offset: 0x000F0F98
		private static bool GetDisableEventLogRegistryKeysFilteringSwitchValue()
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AppContext", false))
				{
					if (registryKey == null)
					{
						return false;
					}
					string text = registryKey.GetValue("Switch.System.Diagnostics.EventLog.DisableEventLogRegistryKeysFiltering", null) as string;
					return text != null && text.Equals("true", StringComparison.OrdinalIgnoreCase);
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x000F2018 File Offset: 0x000F1018
		private int GetCachedEntryPos(int entryIndex)
		{
			if (this.cache == null || (this.boolFlags[2] && entryIndex < this.firstCachedEntry) || (!this.boolFlags[2] && entryIndex > this.firstCachedEntry) || this.firstCachedEntry == -1)
			{
				return -1;
			}
			while (this.lastSeenEntry < entryIndex)
			{
				this.lastSeenEntry++;
				if (this.boolFlags[2])
				{
					this.lastSeenPos = this.GetNextEntryPos(this.lastSeenPos);
					if (this.lastSeenPos < this.bytesCached)
					{
						continue;
					}
				}
				else
				{
					this.lastSeenPos = this.GetPreviousEntryPos(this.lastSeenPos);
					if (this.lastSeenPos >= 0)
					{
						continue;
					}
				}
				IL_FE:
				while (this.lastSeenEntry > entryIndex)
				{
					this.lastSeenEntry--;
					if (this.boolFlags[2])
					{
						this.lastSeenPos = this.GetPreviousEntryPos(this.lastSeenPos);
						if (this.lastSeenPos < 0)
						{
							break;
						}
					}
					else
					{
						this.lastSeenPos = this.GetNextEntryPos(this.lastSeenPos);
						if (this.lastSeenPos >= this.bytesCached)
						{
							break;
						}
					}
				}
				if (this.lastSeenPos >= this.bytesCached)
				{
					this.lastSeenPos = this.GetPreviousEntryPos(this.lastSeenPos);
					if (this.boolFlags[2])
					{
						this.lastSeenEntry--;
					}
					else
					{
						this.lastSeenEntry++;
					}
					return -1;
				}
				if (this.lastSeenPos < 0)
				{
					this.lastSeenPos = 0;
					if (this.boolFlags[2])
					{
						this.lastSeenEntry++;
					}
					else
					{
						this.lastSeenEntry--;
					}
					return -1;
				}
				return this.lastSeenPos;
			}
			goto IL_FE;
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x000F21C0 File Offset: 0x000F11C0
		internal EventLogEntry GetEntryAt(int index)
		{
			EventLogEntry entryAtNoThrow = this.GetEntryAtNoThrow(index);
			if (entryAtNoThrow == null)
			{
				throw new ArgumentException(SR.GetString("IndexOutOfBounds", new object[]
				{
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return entryAtNoThrow;
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x000F2200 File Offset: 0x000F1200
		internal EventLogEntry GetEntryAtNoThrow(int index)
		{
			if (!this.IsOpenForRead)
			{
				this.OpenForRead(this.machineName);
			}
			if (index < 0 || index >= this.EntryCount)
			{
				return null;
			}
			index += this.OldestEntryNumber;
			return this.GetEntryWithOldest(index);
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x000F2238 File Offset: 0x000F1238
		private EventLogEntry GetEntryWithOldest(int index)
		{
			int cachedEntryPos = this.GetCachedEntryPos(index);
			if (cachedEntryPos >= 0)
			{
				return new EventLogEntry(this.cache, cachedEntryPos, this);
			}
			string currentMachineName = this.machineName;
			int dwReadFlags;
			if (this.GetCachedEntryPos(index + 1) < 0)
			{
				dwReadFlags = 6;
				this.boolFlags[2] = true;
			}
			else
			{
				dwReadFlags = 10;
				this.boolFlags[2] = false;
			}
			this.cache = new byte[40000];
			int[] array = new int[1];
			int[] array2 = new int[]
			{
				this.cache.Length
			};
			bool flag = UnsafeNativeMethods.ReadEventLog(this.readHandle, dwReadFlags, index, this.cache, this.cache.Length, array, array2);
			if (!flag)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 122 || lastWin32Error == 1503)
				{
					if (lastWin32Error == 1503)
					{
						byte[] array3 = this.cache;
						this.Reset(currentMachineName);
						this.cache = array3;
					}
					else if (array2[0] > this.cache.Length)
					{
						this.cache = new byte[array2[0]];
					}
					flag = UnsafeNativeMethods.ReadEventLog(this.readHandle, 6, index, this.cache, this.cache.Length, array, array2);
				}
				if (!flag)
				{
					throw new InvalidOperationException(SR.GetString("CantReadLogEntryAt", new object[]
					{
						index.ToString(CultureInfo.CurrentCulture)
					}), SharedUtils.CreateSafeWin32Exception());
				}
			}
			this.bytesCached = array[0];
			this.firstCachedEntry = index;
			this.lastSeenEntry = index;
			this.lastSeenPos = 0;
			return new EventLogEntry(this.cache, 0, this);
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x000F23C8 File Offset: 0x000F13C8
		internal static RegistryKey GetEventLogRegKey(string machine, bool writable)
		{
			RegistryKey registryKey = null;
			try
			{
				if (machine.Equals("."))
				{
					registryKey = Registry.LocalMachine;
				}
				else
				{
					registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machine);
				}
				if (registryKey != null)
				{
					return registryKey.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\EventLog", writable);
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return null;
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x000F242C File Offset: 0x000F142C
		private RegistryKey GetLogRegKey(string currentMachineName, bool writable)
		{
			string text = this.GetLogName(currentMachineName);
			if (!EventLog.ValidLogName(text, false))
			{
				throw new InvalidOperationException(SR.GetString("BadLogName"));
			}
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			try
			{
				registryKey = EventLog.GetEventLogRegKey(currentMachineName, false);
				if (registryKey == null)
				{
					throw new InvalidOperationException(SR.GetString("RegKeyMissingShort", new object[]
					{
						"SYSTEM\\CurrentControlSet\\Services\\EventLog",
						currentMachineName
					}));
				}
				registryKey2 = registryKey.OpenSubKey(text, writable);
				if (registryKey2 == null)
				{
					throw new InvalidOperationException(SR.GetString("MissingLog", new object[]
					{
						text,
						currentMachineName
					}));
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return registryKey2;
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x000F24DC File Offset: 0x000F14DC
		private object GetLogRegValue(string currentMachineName, string valuename)
		{
			PermissionSet permissionSet = EventLog._GetAssertPermSet();
			permissionSet.Assert();
			RegistryKey registryKey = null;
			object result;
			try
			{
				registryKey = this.GetLogRegKey(currentMachineName, false);
				if (registryKey == null)
				{
					throw new InvalidOperationException(SR.GetString("MissingLog", new object[]
					{
						this.GetLogName(currentMachineName),
						currentMachineName
					}));
				}
				object value = registryKey.GetValue(valuename);
				result = value;
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x000F255C File Offset: 0x000F155C
		private int GetNextEntryPos(int pos)
		{
			return pos + EventLog.IntFrom(this.cache, pos);
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x000F256C File Offset: 0x000F156C
		private int GetPreviousEntryPos(int pos)
		{
			return pos - EventLog.IntFrom(this.cache, pos - 4);
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x000F257E File Offset: 0x000F157E
		internal static string GetDllPath(string machineName)
		{
			return SharedUtils.GetLatestBuildDllDirectory(machineName) + "\\EventLogMessages.dll";
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x000F2590 File Offset: 0x000F1590
		private static int IntFrom(byte[] buf, int offset)
		{
			return (-16777216 & (int)buf[offset + 3] << 24) | (16711680 & (int)buf[offset + 2] << 16) | (65280 & (int)buf[offset + 1] << 8) | (int)(byte.MaxValue & buf[offset]);
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x000F25C7 File Offset: 0x000F15C7
		public static bool SourceExists(string source)
		{
			return EventLog.SourceExists(source, ".");
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x000F25D4 File Offset: 0x000F15D4
		public static bool SourceExists(string source, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"machineName",
					machineName
				}));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, machineName);
			eventLogPermission.Demand();
			bool result;
			using (RegistryKey registryKey = EventLog.FindSourceRegistration(source, machineName, true))
			{
				result = (registryKey != null);
			}
			return result;
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x000F264C File Offset: 0x000F164C
		public static string LogNameFromSourceName(string source, string machineName)
		{
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			string result;
			using (RegistryKey registryKey = EventLog.FindSourceRegistration(source, machineName, true))
			{
				if (registryKey == null)
				{
					result = "";
				}
				else
				{
					string name = registryKey.Name;
					int num = name.LastIndexOf('\\');
					result = name.Substring(num + 1);
				}
			}
			return result;
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x000F26B8 File Offset: 0x000F16B8
		[ComVisible(false)]
		public void ModifyOverflowPolicy(OverflowAction action, int retentionDays)
		{
			string currentMachineName = this.machineName;
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, currentMachineName);
			eventLogPermission.Demand();
			if (action < OverflowAction.DoNotOverwrite || action > OverflowAction.OverwriteOlder)
			{
				throw new InvalidEnumArgumentException("action", (int)action, typeof(OverflowAction));
			}
			long num = (long)action;
			if (action == OverflowAction.OverwriteOlder)
			{
				if (retentionDays < 1 || retentionDays > 365)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("RentionDaysOutOfRange"));
				}
				num = (long)retentionDays * 86400L;
			}
			PermissionSet permissionSet = EventLog._GetAssertPermSet();
			permissionSet.Assert();
			using (RegistryKey logRegKey = this.GetLogRegKey(currentMachineName, true))
			{
				logRegKey.SetValue("Retention", num, RegistryValueKind.DWord);
			}
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x000F2770 File Offset: 0x000F1770
		private void OpenForRead(string currentMachineName)
		{
			if (this.boolFlags[256])
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			string text = this.GetLogName(currentMachineName);
			if (text == null || text.Length == 0)
			{
				throw new ArgumentException(SR.GetString("MissingLogProperty"));
			}
			if (!EventLog.Exists(text, currentMachineName))
			{
				throw new InvalidOperationException(SR.GetString("LogDoesNotExists", new object[]
				{
					text,
					currentMachineName
				}));
			}
			SharedUtils.CheckEnvironment();
			this.lastSeenEntry = 0;
			this.lastSeenPos = 0;
			this.bytesCached = 0;
			this.firstCachedEntry = -1;
			this.readHandle = SafeEventLogReadHandle.OpenEventLog(currentMachineName, text);
			if (this.readHandle.IsInvalid)
			{
				Win32Exception innerException = null;
				if (Marshal.GetLastWin32Error() != 0)
				{
					innerException = SharedUtils.CreateSafeWin32Exception();
				}
				throw new InvalidOperationException(SR.GetString("CantOpenLog", new object[]
				{
					text.ToString(),
					currentMachineName
				}), innerException);
			}
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x000F285C File Offset: 0x000F185C
		private void OpenForWrite(string currentMachineName)
		{
			if (this.boolFlags[256])
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (this.sourceName == null || this.sourceName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("NeedSourceToOpen"));
			}
			SharedUtils.CheckEnvironment();
			this.writeHandle = SafeEventLogWriteHandle.RegisterEventSource(currentMachineName, this.sourceName);
			if (this.writeHandle.IsInvalid)
			{
				Win32Exception innerException = null;
				if (Marshal.GetLastWin32Error() != 0)
				{
					innerException = SharedUtils.CreateSafeWin32Exception();
				}
				throw new InvalidOperationException(SR.GetString("CantOpenLogAccess", new object[]
				{
					this.sourceName
				}), innerException);
			}
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x000F2908 File Offset: 0x000F1908
		[ComVisible(false)]
		public void RegisterDisplayName(string resourceFile, long resourceId)
		{
			string currentMachineName = this.machineName;
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, currentMachineName);
			eventLogPermission.Demand();
			PermissionSet permissionSet = EventLog._GetAssertPermSet();
			permissionSet.Assert();
			using (RegistryKey logRegKey = this.GetLogRegKey(currentMachineName, true))
			{
				logRegKey.SetValue("DisplayNameFile", resourceFile, RegistryValueKind.ExpandString);
				logRegKey.SetValue("DisplayNameID", resourceId, RegistryValueKind.DWord);
			}
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x000F297C File Offset: 0x000F197C
		private void Reset(string currentMachineName)
		{
			bool isOpenForRead = this.IsOpenForRead;
			bool isOpenForWrite = this.IsOpenForWrite;
			bool value = this.boolFlags[8];
			bool flag = this.boolFlags[16];
			this.Close(currentMachineName);
			this.cache = null;
			if (isOpenForRead)
			{
				this.OpenForRead(currentMachineName);
			}
			if (isOpenForWrite)
			{
				this.OpenForWrite(currentMachineName);
			}
			if (flag)
			{
				this.StartListening(currentMachineName, this.GetLogName(currentMachineName));
			}
			this.boolFlags[8] = value;
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x000F29F4 File Offset: 0x000F19F4
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		private static void RemoveListenerComponent(EventLog component, string compLogName)
		{
			lock (EventLog.InternalSyncObject)
			{
				EventLog.LogListeningInfo logListeningInfo = (EventLog.LogListeningInfo)EventLog.listenerInfos[compLogName];
				logListeningInfo.listeningComponents.Remove(component);
				if (logListeningInfo.listeningComponents.Count == 0)
				{
					logListeningInfo.handleOwner.Dispose();
					logListeningInfo.registeredWaitHandle.Unregister(logListeningInfo.waitHandle);
					logListeningInfo.waitHandle.Close();
					EventLog.listenerInfos[compLogName] = null;
				}
			}
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x000F2A88 File Offset: 0x000F1A88
		private static void SetSpecialLogRegValues(RegistryKey logKey, string logName)
		{
			if (logKey.GetValue("MaxSize") == null)
			{
				logKey.SetValue("MaxSize", 524288, RegistryValueKind.DWord);
			}
			if (logKey.GetValue("AutoBackupLogFiles") == null)
			{
				logKey.SetValue("AutoBackupLogFiles", 0, RegistryValueKind.DWord);
			}
			if (!EventLog.SkipRegPatch)
			{
				if (logKey.GetValue("Retention") == null)
				{
					logKey.SetValue("Retention", 604800, RegistryValueKind.DWord);
				}
				if (logKey.GetValue("File") == null)
				{
					string value;
					if (logName.Length > 8)
					{
						value = "%SystemRoot%\\System32\\config\\" + logName.Substring(0, 8) + ".evt";
					}
					else
					{
						value = "%SystemRoot%\\System32\\config\\" + logName + ".evt";
					}
					logKey.SetValue("File", value, RegistryValueKind.ExpandString);
				}
			}
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x000F2B50 File Offset: 0x000F1B50
		private static void SetSpecialSourceRegValues(RegistryKey sourceLogKey, EventSourceCreationData sourceData)
		{
			if (string.IsNullOrEmpty(sourceData.MessageResourceFile))
			{
				sourceLogKey.SetValue("EventMessageFile", EventLog.GetDllPath(sourceData.MachineName), RegistryValueKind.ExpandString);
			}
			else
			{
				sourceLogKey.SetValue("EventMessageFile", EventLog.FixupPath(sourceData.MessageResourceFile), RegistryValueKind.ExpandString);
			}
			if (!string.IsNullOrEmpty(sourceData.ParameterResourceFile))
			{
				sourceLogKey.SetValue("ParameterMessageFile", EventLog.FixupPath(sourceData.ParameterResourceFile), RegistryValueKind.ExpandString);
			}
			if (!string.IsNullOrEmpty(sourceData.CategoryResourceFile))
			{
				sourceLogKey.SetValue("CategoryMessageFile", EventLog.FixupPath(sourceData.CategoryResourceFile), RegistryValueKind.ExpandString);
				sourceLogKey.SetValue("CategoryCount", sourceData.CategoryCount, RegistryValueKind.DWord);
			}
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x000F2BF9 File Offset: 0x000F1BF9
		private static string FixupPath(string path)
		{
			if (path[0] == '%')
			{
				return path;
			}
			return Path.GetFullPath(path);
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x000F2C0E File Offset: 0x000F1C0E
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		private void StartListening(string currentMachineName, string currentLogName)
		{
			this.lastSeenCount = this.EntryCount + this.OldestEntryNumber;
			EventLog.AddListenerComponent(this, currentMachineName, currentLogName);
			this.boolFlags[16] = true;
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x000F2C39 File Offset: 0x000F1C39
		private void StartRaisingEvents(string currentMachineName, string currentLogName)
		{
			if (!this.boolFlags[4] && !this.boolFlags[8] && !base.DesignMode)
			{
				this.StartListening(currentMachineName, currentLogName);
			}
			this.boolFlags[8] = true;
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x000F2C74 File Offset: 0x000F1C74
		private static void StaticCompletionCallback(object context, bool wasSignaled)
		{
			EventLog.LogListeningInfo logListeningInfo = (EventLog.LogListeningInfo)context;
			EventLog[] array = (EventLog[])logListeningInfo.listeningComponents.ToArray(typeof(EventLog));
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					if (array[i] != null)
					{
						array[i].CompletionCallback(null);
					}
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x000F2CD4 File Offset: 0x000F1CD4
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		private void StopListening(string currentLogName)
		{
			EventLog.RemoveListenerComponent(this, currentLogName);
			this.boolFlags[16] = false;
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x000F2CEB File Offset: 0x000F1CEB
		private void StopRaisingEvents(string currentLogName)
		{
			if (!this.boolFlags[4] && this.boolFlags[8] && !base.DesignMode)
			{
				this.StopListening(currentLogName);
			}
			this.boolFlags[8] = false;
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x000F2D28 File Offset: 0x000F1D28
		internal static string TryFormatMessage(SafeLibraryHandle hModule, uint messageNum, string[] insertionStrings)
		{
			string text = null;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder(1024);
			int num2 = 10240;
			IntPtr[] array = new IntPtr[insertionStrings.Length];
			GCHandle[] array2 = new GCHandle[insertionStrings.Length];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			if (insertionStrings.Length == 0)
			{
				num2 |= 512;
			}
			try
			{
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = GCHandle.Alloc(insertionStrings[i], GCHandleType.Pinned);
					array[i] = array2[i].AddrOfPinnedObject();
				}
				int num3 = 122;
				while (num == 0 && num3 == 122)
				{
					num = SafeNativeMethods.FormatMessage(num2, hModule, messageNum, 0, stringBuilder, stringBuilder.Capacity, array);
					if (num == 0)
					{
						num3 = Marshal.GetLastWin32Error();
						if (num3 == 122)
						{
							stringBuilder.Capacity *= 2;
						}
					}
				}
			}
			catch
			{
				num = 0;
			}
			finally
			{
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j].IsAllocated)
					{
						array2[j].Free();
					}
				}
				gchandle.Free();
			}
			if (num > 0)
			{
				text = stringBuilder.ToString();
				if (text.Length > 1 && text[text.Length - 1] == '\n')
				{
					text = text.Substring(0, text.Length - 2);
				}
			}
			return text;
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x000F2E94 File Offset: 0x000F1E94
		private static bool CharIsPrintable(char c)
		{
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			return unicodeCategory != UnicodeCategory.Control || unicodeCategory == UnicodeCategory.Format || unicodeCategory == UnicodeCategory.LineSeparator || unicodeCategory == UnicodeCategory.ParagraphSeparator || unicodeCategory == UnicodeCategory.OtherNotAssigned;
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x000F2EC4 File Offset: 0x000F1EC4
		internal static bool ValidLogName(string logName, bool ignoreEmpty)
		{
			if (logName.Length == 0 && !ignoreEmpty)
			{
				return false;
			}
			foreach (char c in logName)
			{
				if (!EventLog.CharIsPrintable(c) || c == '\\' || c == '*' || c == '?')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x000F2F18 File Offset: 0x000F1F18
		private void VerifyAndCreateSource(string sourceName, string currentMachineName)
		{
			if (this.boolFlags[512])
			{
				return;
			}
			if (!EventLog.SourceExists(sourceName, currentMachineName))
			{
				Mutex mutex = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					SharedUtils.EnterMutex("netfxeventlog.1.0", ref mutex);
					if (!EventLog.SourceExists(sourceName, currentMachineName))
					{
						if (this.GetLogName(currentMachineName) == null)
						{
							this.SetLogName(currentMachineName, "Application");
						}
						EventLog.CreateEventSource(new EventSourceCreationData(sourceName, this.GetLogName(currentMachineName), currentMachineName));
						this.Reset(currentMachineName);
					}
					else
					{
						string text = EventLog.LogNameFromSourceName(sourceName, currentMachineName);
						string text2 = this.GetLogName(currentMachineName);
						if (text != null && text2 != null && string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw new ArgumentException(SR.GetString("LogSourceMismatch", new object[]
							{
								this.Source.ToString(),
								text2,
								text
							}));
						}
					}
					goto IL_128;
				}
				finally
				{
					if (mutex != null)
					{
						mutex.ReleaseMutex();
						mutex.Close();
					}
				}
			}
			string text3 = EventLog.LogNameFromSourceName(sourceName, currentMachineName);
			string text4 = this.GetLogName(currentMachineName);
			if (text3 != null && text4 != null && string.Compare(text3, text4, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("LogSourceMismatch", new object[]
				{
					this.Source.ToString(),
					text4,
					text3
				}));
			}
			IL_128:
			this.boolFlags[512] = true;
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x000F3070 File Offset: 0x000F2070
		public void WriteEntry(string message)
		{
			this.WriteEntry(message, EventLogEntryType.Information, 0, 0, null);
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x000F307D File Offset: 0x000F207D
		public static void WriteEntry(string source, string message)
		{
			EventLog.WriteEntry(source, message, EventLogEntryType.Information, 0, 0, null);
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x000F308A File Offset: 0x000F208A
		public void WriteEntry(string message, EventLogEntryType type)
		{
			this.WriteEntry(message, type, 0, 0, null);
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x000F3097 File Offset: 0x000F2097
		public static void WriteEntry(string source, string message, EventLogEntryType type)
		{
			EventLog.WriteEntry(source, message, type, 0, 0, null);
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x000F30A4 File Offset: 0x000F20A4
		public void WriteEntry(string message, EventLogEntryType type, int eventID)
		{
			this.WriteEntry(message, type, eventID, 0, null);
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x000F30B1 File Offset: 0x000F20B1
		public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID)
		{
			EventLog.WriteEntry(source, message, type, eventID, 0, null);
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x000F30BE File Offset: 0x000F20BE
		public void WriteEntry(string message, EventLogEntryType type, int eventID, short category)
		{
			this.WriteEntry(message, type, eventID, category, null);
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x000F30CC File Offset: 0x000F20CC
		public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category)
		{
			EventLog.WriteEntry(source, message, type, eventID, category, null);
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x000F30DC File Offset: 0x000F20DC
		public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			EventLog eventLog = new EventLog();
			try
			{
				eventLog.Source = source;
				eventLog.WriteEntry(message, type, eventID, category, rawData);
			}
			finally
			{
				eventLog.Dispose(true);
			}
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x000F3120 File Offset: 0x000F2120
		public void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			if (eventID < 0 || eventID > 65535)
			{
				throw new ArgumentException(SR.GetString("EventID", new object[]
				{
					eventID,
					0,
					65535
				}));
			}
			if (this.Source.Length == 0)
			{
				throw new ArgumentException(SR.GetString("NeedSourceToWrite"));
			}
			if (!Enum.IsDefined(typeof(EventLogEntryType), type))
			{
				throw new InvalidEnumArgumentException("type", (int)type, typeof(EventLogEntryType));
			}
			string currentMachineName = this.machineName;
			if (!this.boolFlags[32])
			{
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, currentMachineName);
				eventLogPermission.Demand();
				this.boolFlags[32] = true;
			}
			this.VerifyAndCreateSource(this.sourceName, currentMachineName);
			this.InternalWriteEvent((uint)eventID, (ushort)category, type, new string[]
			{
				message
			}, rawData, currentMachineName);
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x000F3214 File Offset: 0x000F2214
		[ComVisible(false)]
		public void WriteEvent(EventInstance instance, params object[] values)
		{
			this.WriteEvent(instance, null, values);
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x000F3220 File Offset: 0x000F2220
		[ComVisible(false)]
		public void WriteEvent(EventInstance instance, byte[] data, params object[] values)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (this.Source.Length == 0)
			{
				throw new ArgumentException(SR.GetString("NeedSourceToWrite"));
			}
			string currentMachineName = this.machineName;
			if (!this.boolFlags[32])
			{
				EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, currentMachineName);
				eventLogPermission.Demand();
				this.boolFlags[32] = true;
			}
			this.VerifyAndCreateSource(this.Source, currentMachineName);
			string[] array = null;
			if (values != null)
			{
				array = new string[values.Length];
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] != null)
					{
						array[i] = values[i].ToString();
					}
					else
					{
						array[i] = string.Empty;
					}
				}
			}
			this.InternalWriteEvent((uint)instance.InstanceId, (ushort)instance.CategoryId, instance.EntryType, array, data, currentMachineName);
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x000F32EC File Offset: 0x000F22EC
		public static void WriteEvent(string source, EventInstance instance, params object[] values)
		{
			using (EventLog eventLog = new EventLog())
			{
				eventLog.Source = source;
				eventLog.WriteEvent(instance, null, values);
			}
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x000F332C File Offset: 0x000F232C
		public static void WriteEvent(string source, EventInstance instance, byte[] data, params object[] values)
		{
			using (EventLog eventLog = new EventLog())
			{
				eventLog.Source = source;
				eventLog.WriteEvent(instance, data, values);
			}
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x000F336C File Offset: 0x000F236C
		private void InternalWriteEvent(uint eventID, ushort category, EventLogEntryType type, string[] strings, byte[] rawData, string currentMachineName)
		{
			if (strings == null)
			{
				strings = new string[0];
			}
			if (strings.Length >= 256)
			{
				throw new ArgumentException(SR.GetString("TooManyReplacementStrings"));
			}
			for (int i = 0; i < strings.Length; i++)
			{
				if (strings[i] == null)
				{
					strings[i] = string.Empty;
				}
				if (strings[i].Length > 32766)
				{
					throw new ArgumentException(SR.GetString("LogEntryTooLong"));
				}
			}
			if (rawData == null)
			{
				rawData = new byte[0];
			}
			if (this.Source.Length == 0)
			{
				throw new ArgumentException(SR.GetString("NeedSourceToWrite"));
			}
			if (!this.IsOpenForWrite)
			{
				this.OpenForWrite(currentMachineName);
			}
			IntPtr[] array = new IntPtr[strings.Length];
			GCHandle[] array2 = new GCHandle[strings.Length];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				for (int j = 0; j < strings.Length; j++)
				{
					array2[j] = GCHandle.Alloc(strings[j], GCHandleType.Pinned);
					array[j] = array2[j].AddrOfPinnedObject();
				}
				byte[] userSID = null;
				if (!UnsafeNativeMethods.ReportEvent(this.writeHandle, (short)type, category, eventID, userSID, (short)strings.Length, rawData.Length, new HandleRef(this, gchandle.AddrOfPinnedObject()), rawData))
				{
					throw SharedUtils.CreateSafeWin32Exception();
				}
			}
			finally
			{
				for (int k = 0; k < strings.Length; k++)
				{
					if (array2[k].IsAllocated)
					{
						array2[k].Free();
					}
				}
				gchandle.Free();
			}
		}

		// Token: 0x04003266 RID: 12902
		private const int BUF_SIZE = 40000;

		// Token: 0x04003267 RID: 12903
		private const string EventLogKey = "SYSTEM\\CurrentControlSet\\Services\\EventLog";

		// Token: 0x04003268 RID: 12904
		internal const string DllName = "EventLogMessages.dll";

		// Token: 0x04003269 RID: 12905
		private const string eventLogMutexName = "netfxeventlog.1.0";

		// Token: 0x0400326A RID: 12906
		private const int SecondsPerDay = 86400;

		// Token: 0x0400326B RID: 12907
		private const int DefaultMaxSize = 524288;

		// Token: 0x0400326C RID: 12908
		private const int DefaultRetention = 604800;

		// Token: 0x0400326D RID: 12909
		private const int Flag_notifying = 1;

		// Token: 0x0400326E RID: 12910
		private const int Flag_forwards = 2;

		// Token: 0x0400326F RID: 12911
		private const int Flag_initializing = 4;

		// Token: 0x04003270 RID: 12912
		private const int Flag_monitoring = 8;

		// Token: 0x04003271 RID: 12913
		private const int Flag_registeredAsListener = 16;

		// Token: 0x04003272 RID: 12914
		private const int Flag_writeGranted = 32;

		// Token: 0x04003273 RID: 12915
		private const int Flag_disposed = 256;

		// Token: 0x04003274 RID: 12916
		private const int Flag_sourceVerified = 512;

		// Token: 0x04003275 RID: 12917
		private EventLogEntryCollection entriesCollection;

		// Token: 0x04003276 RID: 12918
		private string logName;

		// Token: 0x04003277 RID: 12919
		private int lastSeenCount;

		// Token: 0x04003278 RID: 12920
		private string machineName;

		// Token: 0x04003279 RID: 12921
		private EntryWrittenEventHandler onEntryWrittenHandler;

		// Token: 0x0400327A RID: 12922
		private SafeEventLogReadHandle readHandle;

		// Token: 0x0400327B RID: 12923
		private string sourceName;

		// Token: 0x0400327C RID: 12924
		private SafeEventLogWriteHandle writeHandle;

		// Token: 0x0400327D RID: 12925
		private string logDisplayName;

		// Token: 0x0400327E RID: 12926
		private int bytesCached;

		// Token: 0x0400327F RID: 12927
		private byte[] cache;

		// Token: 0x04003280 RID: 12928
		private int firstCachedEntry = -1;

		// Token: 0x04003281 RID: 12929
		private int lastSeenEntry;

		// Token: 0x04003282 RID: 12930
		private int lastSeenPos;

		// Token: 0x04003283 RID: 12931
		private ISynchronizeInvoke synchronizingObject;

		// Token: 0x04003284 RID: 12932
		private BitVector32 boolFlags = default(BitVector32);

		// Token: 0x04003285 RID: 12933
		private Hashtable messageLibraries;

		// Token: 0x04003286 RID: 12934
		private static Hashtable listenerInfos = new Hashtable(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04003287 RID: 12935
		private static object s_InternalSyncObject;

		// Token: 0x04003288 RID: 12936
		private static bool s_CheckedOsVersion;

		// Token: 0x04003289 RID: 12937
		private static bool s_SkipRegPatch;

		// Token: 0x0400328A RID: 12938
		private static readonly bool s_dontFilterRegKeys = EventLog.GetDisableEventLogRegistryKeysFilteringSwitchValue();

		// Token: 0x0200074C RID: 1868
		private class LogListeningInfo
		{
			// Token: 0x0400328B RID: 12939
			public EventLog handleOwner;

			// Token: 0x0400328C RID: 12940
			public RegisteredWaitHandle registeredWaitHandle;

			// Token: 0x0400328D RID: 12941
			public WaitHandle waitHandle;

			// Token: 0x0400328E RID: 12942
			public ArrayList listeningComponents = new ArrayList();
		}

		// Token: 0x0200074D RID: 1869
		private class EventLogWaitHandle : WaitHandle
		{
			// Token: 0x0600394A RID: 14666 RVA: 0x000F352A File Offset: 0x000F252A
			public EventLogWaitHandle(SafeEventHandle eventLogNativeHandle)
			{
				base.SafeWaitHandle = new SafeWaitHandle(eventLogNativeHandle.DangerousGetHandle(), true);
				eventLogNativeHandle.SetHandleAsInvalid();
			}
		}
	}
}
