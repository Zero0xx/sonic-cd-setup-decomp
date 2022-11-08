using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000424 RID: 1060
	internal class Logging
	{
		// Token: 0x06002114 RID: 8468 RVA: 0x000827E9 File Offset: 0x000817E9
		private Logging()
		{
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002115 RID: 8469 RVA: 0x000827F4 File Offset: 0x000817F4
		private static object InternalSyncObject
		{
			get
			{
				if (Logging.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref Logging.s_InternalSyncObject, value, null);
				}
				return Logging.s_InternalSyncObject;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002116 RID: 8470 RVA: 0x00082820 File Offset: 0x00081820
		internal static bool On
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				return Logging.s_LoggingEnabled;
			}
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00082833 File Offset: 0x00081833
		internal static bool IsVerbose(TraceSource traceSource)
		{
			return Logging.ValidateSettings(traceSource, TraceEventType.Verbose);
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002118 RID: 8472 RVA: 0x0008283D File Offset: 0x0008183D
		internal static TraceSource Web
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_WebTraceSource;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002119 RID: 8473 RVA: 0x00082859 File Offset: 0x00081859
		internal static TraceSource HttpListener
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_HttpListenerTraceSource;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x00082875 File Offset: 0x00081875
		internal static TraceSource Sockets
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_SocketsTraceSource;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x00082891 File Offset: 0x00081891
		internal static TraceSource RequestCache
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_CacheTraceSource;
			}
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000828B0 File Offset: 0x000818B0
		private static bool GetUseProtocolTextSetting(TraceSource traceSource)
		{
			bool result = false;
			if (traceSource.Attributes["tracemode"] == "protocolonly")
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x000828E0 File Offset: 0x000818E0
		private static int GetMaxDumpSizeSetting(TraceSource traceSource)
		{
			int result = 1024;
			if (traceSource.Attributes.ContainsKey("maxdatasize"))
			{
				try
				{
					result = int.Parse(traceSource.Attributes["maxdatasize"], NumberFormatInfo.InvariantInfo);
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					traceSource.Attributes["maxdatasize"] = result.ToString(NumberFormatInfo.InvariantInfo);
				}
			}
			return result;
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0008296C File Offset: 0x0008196C
		private static void InitializeLogging()
		{
			lock (Logging.InternalSyncObject)
			{
				if (!Logging.s_LoggingInitialized)
				{
					bool flag = false;
					Logging.s_WebTraceSource = new Logging.NclTraceSource("System.Net");
					Logging.s_HttpListenerTraceSource = new Logging.NclTraceSource("System.Net.HttpListener");
					Logging.s_SocketsTraceSource = new Logging.NclTraceSource("System.Net.Sockets");
					Logging.s_CacheTraceSource = new Logging.NclTraceSource("System.Net.Cache");
					if (Logging.s_WebTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_HttpListenerTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_SocketsTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_CacheTraceSource.Switch.ShouldTrace(TraceEventType.Critical))
					{
						flag = true;
						AppDomain currentDomain = AppDomain.CurrentDomain;
						currentDomain.UnhandledException += Logging.UnhandledExceptionHandler;
						currentDomain.DomainUnload += Logging.AppDomainUnloadEvent;
						currentDomain.ProcessExit += Logging.ProcessExitEvent;
					}
					Logging.s_LoggingEnabled = flag;
					Logging.s_LoggingInitialized = true;
				}
			}
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x00082A78 File Offset: 0x00081A78
		private static void Close()
		{
			if (Logging.s_WebTraceSource != null)
			{
				Logging.s_WebTraceSource.Close();
			}
			if (Logging.s_HttpListenerTraceSource != null)
			{
				Logging.s_HttpListenerTraceSource.Close();
			}
			if (Logging.s_SocketsTraceSource != null)
			{
				Logging.s_SocketsTraceSource.Close();
			}
			if (Logging.s_CacheTraceSource != null)
			{
				Logging.s_CacheTraceSource.Close();
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00082ACC File Offset: 0x00081ACC
		private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;
			Logging.Exception(Logging.Web, sender, "UnhandledExceptionHandler", e);
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00082AF6 File Offset: 0x00081AF6
		private static void ProcessExitEvent(object sender, EventArgs e)
		{
			Logging.Close();
			Logging.s_AppDomainShutdown = true;
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00082B03 File Offset: 0x00081B03
		private static void AppDomainUnloadEvent(object sender, EventArgs e)
		{
			Logging.Close();
			Logging.s_AppDomainShutdown = true;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00082B10 File Offset: 0x00081B10
		private static bool ValidateSettings(TraceSource traceSource, TraceEventType traceLevel)
		{
			if (!Logging.s_LoggingEnabled)
			{
				return false;
			}
			if (!Logging.s_LoggingInitialized)
			{
				Logging.InitializeLogging();
			}
			return traceSource != null && traceSource.Switch.ShouldTrace(traceLevel) && !Logging.s_AppDomainShutdown;
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00082B44 File Offset: 0x00081B44
		private static string GetObjectName(object obj)
		{
			string text = obj.ToString();
			string result;
			try
			{
				if (!(obj is Uri))
				{
					int num = text.LastIndexOf('.') + 1;
					result = text.Substring(num, text.Length - num);
				}
				else
				{
					result = text;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00082BB4 File Offset: 0x00081BB4
		internal static uint GetThreadId()
		{
			uint num = UnsafeNclNativeMethods.GetCurrentThreadId();
			if (num == 0U)
			{
				num = (uint)Thread.CurrentThread.GetHashCode();
			}
			return num;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x00082BD8 File Offset: 0x00081BD8
		internal static void PrintLine(TraceSource traceSource, TraceEventType eventType, int id, string msg)
		{
			string str = "[" + Logging.GetThreadId().ToString("d4", CultureInfo.InvariantCulture) + "] ";
			traceSource.TraceEvent(eventType, id, str + msg);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x00082C1C File Offset: 0x00081C1C
		internal static void Associate(TraceSource traceSource, object objA, object objB)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string str = Logging.GetObjectName(objA) + "#" + ValidationHelper.HashString(objA);
			string str2 = Logging.GetObjectName(objB) + "#" + ValidationHelper.HashString(objB);
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, "Associating " + str + " with " + str2);
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x00082C7A File Offset: 0x00081C7A
		internal static void Enter(TraceSource traceSource, object obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, param);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x00082CA4 File Offset: 0x00081CA4
		internal static void Enter(TraceSource traceSource, object obj, string method, object paramObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, paramObject);
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x00082CD0 File Offset: 0x00081CD0
		internal static void Enter(TraceSource traceSource, string obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, string.Concat(new string[]
			{
				obj,
				"::",
				method,
				"(",
				param,
				")"
			}));
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00082D20 File Offset: 0x00081D20
		internal static void Enter(TraceSource traceSource, string obj, string method, object paramObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = "";
			if (paramObject != null)
			{
				text = Logging.GetObjectName(paramObject) + "#" + ValidationHelper.HashString(paramObject);
			}
			Logging.Enter(traceSource, string.Concat(new string[]
			{
				obj,
				"::",
				method,
				"(",
				text,
				")"
			}));
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x00082D8E File Offset: 0x00081D8E
		internal static void Enter(TraceSource traceSource, string method, string parameters)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, method + "(" + parameters + ")");
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00082DB1 File Offset: 0x00081DB1
		internal static void Enter(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, msg);
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x00082DC8 File Offset: 0x00081DC8
		internal static void Exit(TraceSource traceSource, object obj, string method, object retObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string retValue = "";
			if (retObject != null)
			{
				retValue = Logging.GetObjectName(retObject) + "#" + ValidationHelper.HashString(retObject);
			}
			Logging.Exit(traceSource, obj, method, retValue);
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x00082E08 File Offset: 0x00081E08
		internal static void Exit(TraceSource traceSource, string obj, string method, object retObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string retValue = "";
			if (retObject != null)
			{
				retValue = Logging.GetObjectName(retObject) + "#" + ValidationHelper.HashString(retObject);
			}
			Logging.Exit(traceSource, obj, method, retValue);
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00082E48 File Offset: 0x00081E48
		internal static void Exit(TraceSource traceSource, object obj, string method, string retValue)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Exit(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, retValue);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x00082E74 File Offset: 0x00081E74
		internal static void Exit(TraceSource traceSource, string obj, string method, string retValue)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			if (!ValidationHelper.IsBlankString(retValue))
			{
				retValue = "\t-> " + retValue;
			}
			Logging.Exit(traceSource, string.Concat(new string[]
			{
				obj,
				"::",
				method,
				"() ",
				retValue
			}));
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00082ECF File Offset: 0x00081ECF
		internal static void Exit(TraceSource traceSource, string method, string parameters)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Exit(traceSource, method + "() " + parameters);
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00082EED File Offset: 0x00081EED
		internal static void Exit(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "Exiting " + msg);
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00082F10 File Offset: 0x00081F10
		internal static void Exception(TraceSource traceSource, object obj, string method, Exception e)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			string str = string.Concat(new string[]
			{
				"Exception in the ",
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				" - "
			});
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, str + e.Message);
			if (!ValidationHelper.IsBlankString(e.StackTrace))
			{
				Logging.PrintLine(traceSource, TraceEventType.Error, 0, e.StackTrace);
			}
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x00082F9A File Offset: 0x00081F9A
		internal static void PrintInfo(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, msg);
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00082FB0 File Offset: 0x00081FB0
		internal static void PrintInfo(TraceSource traceSource, object obj, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				" - ",
				msg
			}));
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00083004 File Offset: 0x00082004
		internal static void PrintInfo(TraceSource traceSource, object obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"(",
				param,
				")"
			}));
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x0008306A File Offset: 0x0008206A
		internal static void PrintWarning(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Warning))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Warning, 0, msg);
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00083080 File Offset: 0x00082080
		internal static void PrintWarning(TraceSource traceSource, object obj, string method, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Warning))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Warning, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"() - ",
				msg
			}));
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000830DE File Offset: 0x000820DE
		internal static void PrintError(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, msg);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000830F4 File Offset: 0x000820F4
		internal static void PrintError(TraceSource traceSource, object obj, string method, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"() - ",
				msg
			}));
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x00083154 File Offset: 0x00082154
		internal static void Dump(TraceSource traceSource, object obj, string method, IntPtr bufferPtr, int length)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Verbose) || bufferPtr == IntPtr.Zero || length < 0)
			{
				return;
			}
			byte[] array = new byte[length];
			Marshal.Copy(bufferPtr, array, 0, length);
			Logging.Dump(traceSource, obj, method, array, 0, length);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0008319C File Offset: 0x0008219C
		internal static void Dump(TraceSource traceSource, object obj, string method, byte[] buffer, int offset, int length)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Verbose))
			{
				return;
			}
			if (buffer == null)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "(null)");
				return;
			}
			if (offset > buffer.Length)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "(offset out of range)");
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, string.Concat(new string[]
			{
				"Data from ",
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method
			}));
			int maxDumpSizeSetting = Logging.GetMaxDumpSizeSetting(traceSource);
			if (length > maxDumpSizeSetting)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, string.Concat(new string[]
				{
					"(printing ",
					maxDumpSizeSetting.ToString(NumberFormatInfo.InvariantInfo),
					" out of ",
					length.ToString(NumberFormatInfo.InvariantInfo),
					")"
				}));
				length = maxDumpSizeSetting;
			}
			if (length < 0 || length > buffer.Length - offset)
			{
				length = buffer.Length - offset;
			}
			if (Logging.GetUseProtocolTextSetting(traceSource))
			{
				string msg = "<<" + WebHeaderCollection.HeaderEncoding.GetString(buffer, offset, length) + ">>";
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, msg);
				return;
			}
			do
			{
				int num = Math.Min(length, 16);
				string text = string.Format(CultureInfo.CurrentCulture, "{0:X8} : ", new object[]
				{
					offset
				});
				for (int i = 0; i < num; i++)
				{
					text = text + string.Format(CultureInfo.CurrentCulture, "{0:X2}", new object[]
					{
						buffer[offset + i]
					}) + ((i == 7) ? '-' : ' ');
				}
				for (int j = num; j < 16; j++)
				{
					text += "   ";
				}
				text += ": ";
				for (int k = 0; k < num; k++)
				{
					text += (char)((buffer[offset + k] < 32 || buffer[offset + k] > 126) ? 46 : buffer[offset + k]);
				}
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, text);
				offset += num;
				length -= num;
			}
			while (length > 0);
		}

		// Token: 0x0400215D RID: 8541
		private const int DefaultMaxDumpSize = 1024;

		// Token: 0x0400215E RID: 8542
		private const bool DefaultUseProtocolTextOnly = false;

		// Token: 0x0400215F RID: 8543
		private const string AttributeNameMaxSize = "maxdatasize";

		// Token: 0x04002160 RID: 8544
		private const string AttributeNameTraceMode = "tracemode";

		// Token: 0x04002161 RID: 8545
		private const string AttributeValueProtocolOnly = "protocolonly";

		// Token: 0x04002162 RID: 8546
		private const string TraceSourceWebName = "System.Net";

		// Token: 0x04002163 RID: 8547
		private const string TraceSourceHttpListenerName = "System.Net.HttpListener";

		// Token: 0x04002164 RID: 8548
		private const string TraceSourceSocketsName = "System.Net.Sockets";

		// Token: 0x04002165 RID: 8549
		private const string TraceSourceCacheName = "System.Net.Cache";

		// Token: 0x04002166 RID: 8550
		private static bool s_LoggingEnabled = true;

		// Token: 0x04002167 RID: 8551
		private static bool s_LoggingInitialized;

		// Token: 0x04002168 RID: 8552
		private static bool s_AppDomainShutdown;

		// Token: 0x04002169 RID: 8553
		private static readonly string[] SupportedAttributes = new string[]
		{
			"maxdatasize",
			"tracemode"
		};

		// Token: 0x0400216A RID: 8554
		private static TraceSource s_WebTraceSource;

		// Token: 0x0400216B RID: 8555
		private static TraceSource s_HttpListenerTraceSource;

		// Token: 0x0400216C RID: 8556
		private static TraceSource s_SocketsTraceSource;

		// Token: 0x0400216D RID: 8557
		private static TraceSource s_CacheTraceSource;

		// Token: 0x0400216E RID: 8558
		private static object s_InternalSyncObject;

		// Token: 0x02000425 RID: 1061
		private class NclTraceSource : TraceSource
		{
			// Token: 0x0600213F RID: 8511 RVA: 0x00083404 File Offset: 0x00082404
			internal NclTraceSource(string name) : base(name)
			{
			}

			// Token: 0x06002140 RID: 8512 RVA: 0x0008340D File Offset: 0x0008240D
			protected internal override string[] GetSupportedAttributes()
			{
				return Logging.SupportedAttributes;
			}
		}
	}
}
