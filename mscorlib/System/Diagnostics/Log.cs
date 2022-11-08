using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Diagnostics
{
	// Token: 0x020002C2 RID: 706
	internal static class Log
	{
		// Token: 0x06001B4C RID: 6988 RVA: 0x0004716C File Offset: 0x0004616C
		static Log()
		{
			Log.GlobalSwitch.MinimumLevel = LoggingLevels.ErrorLevel;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000471C8 File Offset: 0x000461C8
		public static void AddOnLogMessage(LogMessageEventHandler handler)
		{
			lock (Log.locker)
			{
				Log._LogMessageEventHandler = (LogMessageEventHandler)Delegate.Combine(Log._LogMessageEventHandler, handler);
			}
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00047210 File Offset: 0x00046210
		public static void RemoveOnLogMessage(LogMessageEventHandler handler)
		{
			lock (Log.locker)
			{
				Log._LogMessageEventHandler = (LogMessageEventHandler)Delegate.Remove(Log._LogMessageEventHandler, handler);
			}
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x00047258 File Offset: 0x00046258
		public static void AddOnLogSwitchLevel(LogSwitchLevelHandler handler)
		{
			lock (Log.locker)
			{
				Log._LogSwitchLevelHandler = (LogSwitchLevelHandler)Delegate.Combine(Log._LogSwitchLevelHandler, handler);
			}
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x000472A0 File Offset: 0x000462A0
		public static void RemoveOnLogSwitchLevel(LogSwitchLevelHandler handler)
		{
			lock (Log.locker)
			{
				Log._LogSwitchLevelHandler = (LogSwitchLevelHandler)Delegate.Remove(Log._LogSwitchLevelHandler, handler);
			}
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000472E8 File Offset: 0x000462E8
		internal static void InvokeLogSwitchLevelHandlers(LogSwitch ls, LoggingLevels newLevel)
		{
			LogSwitchLevelHandler logSwitchLevelHandler = Log._LogSwitchLevelHandler;
			if (logSwitchLevelHandler != null)
			{
				logSwitchLevelHandler(ls, newLevel);
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001B52 RID: 6994 RVA: 0x00047306 File Offset: 0x00046306
		// (set) Token: 0x06001B53 RID: 6995 RVA: 0x0004730D File Offset: 0x0004630D
		public static bool IsConsoleEnabled
		{
			get
			{
				return Log.m_fConsoleDeviceEnabled;
			}
			set
			{
				Log.m_fConsoleDeviceEnabled = value;
			}
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x00047318 File Offset: 0x00046318
		public static void AddStream(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (Log.m_iStreamArraySize <= Log.m_iNumOfStreamDevices)
			{
				Stream[] array = new Stream[Log.m_iStreamArraySize + 4];
				if (Log.m_iNumOfStreamDevices > 0)
				{
					Array.Copy(Log.m_rgStream, array, Log.m_iNumOfStreamDevices);
				}
				Log.m_iStreamArraySize += 4;
				Log.m_rgStream = array;
			}
			Log.m_rgStream[Log.m_iNumOfStreamDevices++] = stream;
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0004738A File Offset: 0x0004638A
		public static void LogMessage(LoggingLevels level, string message)
		{
			Log.LogMessage(level, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x00047398 File Offset: 0x00046398
		public static void LogMessage(LoggingLevels level, LogSwitch logswitch, string message)
		{
			if (logswitch == null)
			{
				throw new ArgumentNullException("LogSwitch");
			}
			if (level < LoggingLevels.TraceLevel0)
			{
				throw new ArgumentOutOfRangeException("level", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (logswitch.CheckLevel(level))
			{
				Debugger.Log((int)level, logswitch.strName, message);
				if (Log.m_fConsoleDeviceEnabled)
				{
					Console.Write(message);
				}
				for (int i = 0; i < Log.m_iNumOfStreamDevices; i++)
				{
					StreamWriter streamWriter = new StreamWriter(Log.m_rgStream[i]);
					streamWriter.Write(message);
					streamWriter.Flush();
				}
			}
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x00047419 File Offset: 0x00046419
		public static void Trace(LogSwitch logswitch, string message)
		{
			Log.LogMessage(LoggingLevels.TraceLevel0, logswitch, message);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x00047424 File Offset: 0x00046424
		public static void Trace(string switchname, string message)
		{
			LogSwitch @switch = LogSwitch.GetSwitch(switchname);
			Log.LogMessage(LoggingLevels.TraceLevel0, @switch, message);
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x00047440 File Offset: 0x00046440
		public static void Trace(string message)
		{
			Log.LogMessage(LoggingLevels.TraceLevel0, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0004744E File Offset: 0x0004644E
		public static void Status(LogSwitch logswitch, string message)
		{
			Log.LogMessage(LoggingLevels.StatusLevel0, logswitch, message);
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0004745C File Offset: 0x0004645C
		public static void Status(string switchname, string message)
		{
			LogSwitch @switch = LogSwitch.GetSwitch(switchname);
			Log.LogMessage(LoggingLevels.StatusLevel0, @switch, message);
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x00047479 File Offset: 0x00046479
		public static void Status(string message)
		{
			Log.LogMessage(LoggingLevels.StatusLevel0, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x00047488 File Offset: 0x00046488
		public static void Warning(LogSwitch logswitch, string message)
		{
			Log.LogMessage(LoggingLevels.WarningLevel, logswitch, message);
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x00047494 File Offset: 0x00046494
		public static void Warning(string switchname, string message)
		{
			LogSwitch @switch = LogSwitch.GetSwitch(switchname);
			Log.LogMessage(LoggingLevels.WarningLevel, @switch, message);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x000474B1 File Offset: 0x000464B1
		public static void Warning(string message)
		{
			Log.LogMessage(LoggingLevels.WarningLevel, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x000474C0 File Offset: 0x000464C0
		public static void Error(LogSwitch logswitch, string message)
		{
			Log.LogMessage(LoggingLevels.ErrorLevel, logswitch, message);
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000474CC File Offset: 0x000464CC
		public static void Error(string switchname, string message)
		{
			LogSwitch @switch = LogSwitch.GetSwitch(switchname);
			Log.LogMessage(LoggingLevels.ErrorLevel, @switch, message);
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000474E9 File Offset: 0x000464E9
		public static void Error(string message)
		{
			Log.LogMessage(LoggingLevels.ErrorLevel, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000474F8 File Offset: 0x000464F8
		public static void Panic(string message)
		{
			Log.LogMessage(LoggingLevels.PanicLevel, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B64 RID: 7012
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddLogSwitch(LogSwitch logSwitch);

		// Token: 0x06001B65 RID: 7013
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ModifyLogSwitch(int iNewLevel, string strSwitchName, string strParentName);

		// Token: 0x04000A6C RID: 2668
		internal static Hashtable m_Hashtable = new Hashtable();

		// Token: 0x04000A6D RID: 2669
		private static bool m_fConsoleDeviceEnabled = false;

		// Token: 0x04000A6E RID: 2670
		private static Stream[] m_rgStream = null;

		// Token: 0x04000A6F RID: 2671
		private static int m_iNumOfStreamDevices = 0;

		// Token: 0x04000A70 RID: 2672
		private static int m_iStreamArraySize = 0;

		// Token: 0x04000A71 RID: 2673
		internal static int iNumOfSwitches;

		// Token: 0x04000A72 RID: 2674
		private static LogMessageEventHandler _LogMessageEventHandler;

		// Token: 0x04000A73 RID: 2675
		private static LogSwitchLevelHandler _LogSwitchLevelHandler;

		// Token: 0x04000A74 RID: 2676
		private static object locker = new object();

		// Token: 0x04000A75 RID: 2677
		public static readonly LogSwitch GlobalSwitch = new LogSwitch("Global", "Global Switch for this log");
	}
}
