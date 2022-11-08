using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;

namespace System.Net
{
	// Token: 0x020004EA RID: 1258
	internal static class GlobalLog
	{
		// Token: 0x0600272A RID: 10026 RVA: 0x000A2162 File Offset: 0x000A1162
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		private static BaseLoggingObject LoggingInitialize()
		{
			return new BaseLoggingObject();
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x000A2169 File Offset: 0x000A1169
		internal static ThreadKinds CurrentThreadKind
		{
			get
			{
				return ThreadKinds.Unknown;
			}
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000A216C File Offset: 0x000A116C
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		internal static void SetThreadSource(ThreadKinds source)
		{
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000A216E File Offset: 0x000A116E
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		internal static void ThreadContract(ThreadKinds kind, string errorMsg)
		{
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000A2170 File Offset: 0x000A1170
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		internal static void ThreadContract(ThreadKinds kind, ThreadKinds allowedSources, string errorMsg)
		{
			if ((kind & ThreadKinds.SourceMask) != ThreadKinds.Unknown || (allowedSources & ThreadKinds.SourceMask) != allowedSources)
			{
				throw new InternalException();
			}
			ThreadKinds currentThreadKind = GlobalLog.CurrentThreadKind;
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000A2191 File Offset: 0x000A1191
		[Conditional("TRAVE")]
		public static void AddToArray(string msg)
		{
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000A2193 File Offset: 0x000A1193
		[Conditional("TRAVE")]
		public static void Ignore(object msg)
		{
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000A2195 File Offset: 0x000A1195
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		[Conditional("TRAVE")]
		public static void Print(string msg)
		{
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000A2197 File Offset: 0x000A1197
		[Conditional("TRAVE")]
		public static void PrintHex(string msg, object value)
		{
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000A2199 File Offset: 0x000A1199
		[Conditional("TRAVE")]
		public static void Enter(string func)
		{
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000A219B File Offset: 0x000A119B
		[Conditional("TRAVE")]
		public static void Enter(string func, string parms)
		{
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000A21A0 File Offset: 0x000A11A0
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		[Conditional("DEBUG")]
		[Conditional("_FORCE_ASSERTS")]
		public static void Assert(bool condition, string messageFormat, params object[] data)
		{
			if (!condition)
			{
				string text = string.Format(CultureInfo.InvariantCulture, messageFormat, data);
				int num = text.IndexOf('|');
				if (num == -1)
				{
					return;
				}
				int length = text.Length;
			}
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x000A21D2 File Offset: 0x000A11D2
		[Conditional("DEBUG")]
		[Conditional("_FORCE_ASSERTS")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		public static void Assert(string message)
		{
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000A21D4 File Offset: 0x000A11D4
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		[Conditional("_FORCE_ASSERTS")]
		public static void Assert(string message, string detailMessage)
		{
			try
			{
				GlobalLog.Logobject.DumpArray(false);
			}
			finally
			{
				UnsafeNclNativeMethods.DebugBreak();
				Debugger.Break();
			}
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000A220C File Offset: 0x000A120C
		[Conditional("TRAVE")]
		public static void LeaveException(string func, Exception exception)
		{
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000A220E File Offset: 0x000A120E
		[Conditional("TRAVE")]
		public static void Leave(string func)
		{
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000A2210 File Offset: 0x000A1210
		[Conditional("TRAVE")]
		public static void Leave(string func, string result)
		{
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000A2212 File Offset: 0x000A1212
		[Conditional("TRAVE")]
		public static void Leave(string func, int returnval)
		{
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000A2214 File Offset: 0x000A1214
		[Conditional("TRAVE")]
		public static void Leave(string func, bool returnval)
		{
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000A2216 File Offset: 0x000A1216
		[Conditional("TRAVE")]
		public static void DumpArray()
		{
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000A2218 File Offset: 0x000A1218
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer)
		{
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000A221A File Offset: 0x000A121A
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer, int length)
		{
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000A221C File Offset: 0x000A121C
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer, int offset, int length)
		{
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000A221E File Offset: 0x000A121E
		[Conditional("TRAVE")]
		public static void Dump(IntPtr buffer, int offset, int length)
		{
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000A2220 File Offset: 0x000A1220
		[Conditional("DEBUG")]
		internal static void DebugAddRequest(HttpWebRequest request, Connection connection, int flags)
		{
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000A2222 File Offset: 0x000A1222
		[Conditional("DEBUG")]
		internal static void DebugRemoveRequest(HttpWebRequest request)
		{
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000A2224 File Offset: 0x000A1224
		[Conditional("DEBUG")]
		internal static void DebugUpdateRequest(HttpWebRequest request, Connection connection, int flags)
		{
		}

		// Token: 0x040026AE RID: 9902
		private static BaseLoggingObject Logobject = GlobalLog.LoggingInitialize();
	}
}
