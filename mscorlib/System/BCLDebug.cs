using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System
{
	// Token: 0x02000043 RID: 67
	internal static class BCLDebug
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x0000FAF5 File Offset: 0x0000EAF5
		[Conditional("_DEBUG")]
		public static void Assert(bool condition, string message)
		{
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000FAF7 File Offset: 0x0000EAF7
		[Conditional("_LOGGING")]
		public static void Log(string message)
		{
			if (AppDomain.CurrentDomain.IsUnloadingForcedFinalize())
			{
				return;
			}
			if (!BCLDebug.m_registryChecked)
			{
				BCLDebug.CheckRegistry();
			}
			System.Diagnostics.Log.Trace(message);
			System.Diagnostics.Log.Trace(Environment.NewLine);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000FB24 File Offset: 0x0000EB24
		[Conditional("_LOGGING")]
		public static void Log(string switchName, string message)
		{
			if (AppDomain.CurrentDomain.IsUnloadingForcedFinalize())
			{
				return;
			}
			if (!BCLDebug.m_registryChecked)
			{
				BCLDebug.CheckRegistry();
			}
			try
			{
				LogSwitch @switch = LogSwitch.GetSwitch(switchName);
				if (@switch != null)
				{
					System.Diagnostics.Log.Trace(@switch, message);
					System.Diagnostics.Log.Trace(@switch, Environment.NewLine);
				}
			}
			catch
			{
				System.Diagnostics.Log.Trace("Exception thrown in logging." + Environment.NewLine);
				System.Diagnostics.Log.Trace("Switch was: " + ((switchName == null) ? "<null>" : switchName) + Environment.NewLine);
				System.Diagnostics.Log.Trace("Message was: " + ((message == null) ? "<null>" : message) + Environment.NewLine);
			}
		}

		// Token: 0x060003DC RID: 988
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRegistryLoggingValues(out bool loggingEnabled, out bool logToConsole, out int logLevel, out bool perfWarnings, out bool correctnessWarnings, out bool safeHandleStackTraces);

		// Token: 0x060003DD RID: 989 RVA: 0x0000FBD0 File Offset: 0x0000EBD0
		private static void CheckRegistry()
		{
			if (AppDomain.CurrentDomain.IsUnloadingForcedFinalize())
			{
				return;
			}
			if (BCLDebug.m_registryChecked)
			{
				return;
			}
			BCLDebug.m_registryChecked = true;
			bool flag;
			bool isConsoleEnabled;
			int num;
			int registryLoggingValues = BCLDebug.GetRegistryLoggingValues(out flag, out isConsoleEnabled, out num, out BCLDebug.m_perfWarnings, out BCLDebug.m_correctnessWarnings, out BCLDebug.m_safeHandleStackTraces);
			if (!flag)
			{
				BCLDebug.m_loggingNotEnabled = true;
			}
			if (flag && BCLDebug.levelConversions != null)
			{
				try
				{
					num = (int)BCLDebug.levelConversions[num];
					if (registryLoggingValues > 0)
					{
						for (int i = 0; i < BCLDebug.switches.Length; i++)
						{
							if ((BCLDebug.switches[i].value & registryLoggingValues) != 0)
							{
								LogSwitch logSwitch = new LogSwitch(BCLDebug.switches[i].name, BCLDebug.switches[i].name, System.Diagnostics.Log.GlobalSwitch);
								logSwitch.MinimumLevel = (LoggingLevels)num;
							}
						}
						System.Diagnostics.Log.GlobalSwitch.MinimumLevel = (LoggingLevels)num;
						System.Diagnostics.Log.IsConsoleEnabled = isConsoleEnabled;
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000FCC0 File Offset: 0x0000ECC0
		internal static bool CheckEnabled(string switchName)
		{
			if (AppDomain.CurrentDomain.IsUnloadingForcedFinalize())
			{
				return false;
			}
			if (!BCLDebug.m_registryChecked)
			{
				BCLDebug.CheckRegistry();
			}
			LogSwitch @switch = LogSwitch.GetSwitch(switchName);
			return @switch != null && @switch.MinimumLevel <= LoggingLevels.TraceLevel0;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000FCFF File Offset: 0x0000ECFF
		private static bool CheckEnabled(string switchName, LogLevel level, out LogSwitch logSwitch)
		{
			if (AppDomain.CurrentDomain.IsUnloadingForcedFinalize())
			{
				logSwitch = null;
				return false;
			}
			logSwitch = LogSwitch.GetSwitch(switchName);
			return logSwitch != null && logSwitch.MinimumLevel <= (LoggingLevels)level;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000FD30 File Offset: 0x0000ED30
		[Conditional("_LOGGING")]
		public static void Log(string switchName, LogLevel level, params object[] messages)
		{
			if (AppDomain.CurrentDomain.IsUnloadingForcedFinalize())
			{
				return;
			}
			if (!BCLDebug.m_registryChecked)
			{
				BCLDebug.CheckRegistry();
			}
			LogSwitch logswitch;
			if (!BCLDebug.CheckEnabled(switchName, level, out logswitch))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < messages.Length; i++)
			{
				string value;
				try
				{
					if (messages[i] == null)
					{
						value = "<null>";
					}
					else
					{
						value = messages[i].ToString();
					}
				}
				catch
				{
					value = "<unable to convert>";
				}
				stringBuilder.Append(value);
			}
			System.Diagnostics.Log.LogMessage((LoggingLevels)level, logswitch, stringBuilder.ToString());
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000FDBC File Offset: 0x0000EDBC
		[Conditional("_LOGGING")]
		public static void Trace(string switchName, params object[] messages)
		{
			if (BCLDebug.m_loggingNotEnabled)
			{
				return;
			}
			LogSwitch logswitch;
			if (!BCLDebug.CheckEnabled(switchName, LogLevel.Trace, out logswitch))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < messages.Length; i++)
			{
				string value;
				try
				{
					if (messages[i] == null)
					{
						value = "<null>";
					}
					else
					{
						value = messages[i].ToString();
					}
				}
				catch
				{
					value = "<unable to convert>";
				}
				stringBuilder.Append(value);
			}
			stringBuilder.Append(Environment.NewLine);
			System.Diagnostics.Log.LogMessage(LoggingLevels.TraceLevel0, logswitch, stringBuilder.ToString());
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000FE44 File Offset: 0x0000EE44
		[Conditional("_LOGGING")]
		public static void Trace(string switchName, string format, params object[] messages)
		{
			if (BCLDebug.m_loggingNotEnabled)
			{
				return;
			}
			LogSwitch logswitch;
			if (!BCLDebug.CheckEnabled(switchName, LogLevel.Trace, out logswitch))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(format, messages);
			stringBuilder.Append(Environment.NewLine);
			System.Diagnostics.Log.LogMessage(LoggingLevels.TraceLevel0, logswitch, stringBuilder.ToString());
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000FE90 File Offset: 0x0000EE90
		[Conditional("_LOGGING")]
		public static void DumpStack(string switchName)
		{
			if (!BCLDebug.m_registryChecked)
			{
				BCLDebug.CheckRegistry();
			}
			LogSwitch logswitch;
			if (!BCLDebug.CheckEnabled(switchName, LogLevel.Trace, out logswitch))
			{
				return;
			}
			StackTrace stackTrace = new StackTrace();
			System.Diagnostics.Log.LogMessage(LoggingLevels.TraceLevel0, logswitch, stackTrace.ToString());
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000FEC8 File Offset: 0x0000EEC8
		[Conditional("_DEBUG")]
		internal static void ConsoleError(string msg)
		{
			if (AppDomain.CurrentDomain.IsUnloadingForcedFinalize())
			{
				return;
			}
			if (BCLDebug.m_MakeConsoleErrorLoggingWork == null)
			{
				PermissionSet permissionSet = new PermissionSet();
				permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
				permissionSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, Path.GetFullPath(".")));
				BCLDebug.m_MakeConsoleErrorLoggingWork = permissionSet;
			}
			BCLDebug.m_MakeConsoleErrorLoggingWork.Assert();
			using (TextWriter textWriter = File.AppendText("ConsoleErrors.log"))
			{
				textWriter.WriteLine(msg);
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000FF54 File Offset: 0x0000EF54
		[Conditional("_DEBUG")]
		internal static void Perf(bool expr, string msg)
		{
			if (AppDomain.CurrentDomain.IsUnloadingForcedFinalize())
			{
				return;
			}
			if (!BCLDebug.m_registryChecked)
			{
				BCLDebug.CheckRegistry();
			}
			if (!BCLDebug.m_perfWarnings)
			{
				return;
			}
			System.Diagnostics.Assert.Check(expr, "BCL Perf Warning: Your perf may be less than perfect because...", msg);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000FF85 File Offset: 0x0000EF85
		[Conditional("_DEBUG")]
		internal static void Correctness(bool expr, string msg)
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000FF87 File Offset: 0x0000EF87
		internal static bool CorrectnessEnabled()
		{
			return false;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000FF8A File Offset: 0x0000EF8A
		internal static bool SafeHandleStackTracesEnabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000FF90 File Offset: 0x0000EF90
		// Note: this type is marked as 'beforefieldinit'.
		static BCLDebug()
		{
			LogLevel[] array = new LogLevel[11];
			array[0] = LogLevel.Panic;
			array[1] = LogLevel.Error;
			array[2] = LogLevel.Error;
			array[3] = LogLevel.Warning;
			array[4] = LogLevel.Warning;
			array[5] = LogLevel.Status;
			array[6] = LogLevel.Status;
			BCLDebug.levelConversions = array;
		}

		// Token: 0x04000179 RID: 377
		internal static bool m_registryChecked = false;

		// Token: 0x0400017A RID: 378
		internal static bool m_loggingNotEnabled = false;

		// Token: 0x0400017B RID: 379
		internal static bool m_perfWarnings;

		// Token: 0x0400017C RID: 380
		internal static bool m_correctnessWarnings;

		// Token: 0x0400017D RID: 381
		internal static bool m_safeHandleStackTraces;

		// Token: 0x0400017E RID: 382
		internal static PermissionSet m_MakeConsoleErrorLoggingWork;

		// Token: 0x0400017F RID: 383
		private static readonly SwitchStructure[] switches = new SwitchStructure[]
		{
			new SwitchStructure("NLS", 1),
			new SwitchStructure("SER", 2),
			new SwitchStructure("DYNIL", 4),
			new SwitchStructure("REMOTE", 8),
			new SwitchStructure("BINARY", 16),
			new SwitchStructure("SOAP", 32),
			new SwitchStructure("REMOTINGCHANNELS", 64),
			new SwitchStructure("CACHE", 128),
			new SwitchStructure("RESMGRFILEFORMAT", 256),
			new SwitchStructure("PERF", 512),
			new SwitchStructure("CORRECTNESS", 1024),
			new SwitchStructure("MEMORYFAILPOINT", 2048)
		};

		// Token: 0x04000180 RID: 384
		private static readonly LogLevel[] levelConversions;
	}
}
