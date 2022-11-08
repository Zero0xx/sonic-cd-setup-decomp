using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006A7 RID: 1703
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public sealed class CallContext
	{
		// Token: 0x06003D7F RID: 15743 RVA: 0x000D24E8 File Offset: 0x000D14E8
		private CallContext()
		{
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x000D24F0 File Offset: 0x000D14F0
		internal static LogicalCallContext GetLogicalCallContext()
		{
			return Thread.CurrentThread.GetLogicalCallContext();
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x000D24FC File Offset: 0x000D14FC
		internal static LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			return Thread.CurrentThread.SetLogicalCallContext(callCtx);
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x000D2509 File Offset: 0x000D1509
		internal static LogicalCallContext SetLogicalCallContext(Thread currThread, LogicalCallContext callCtx)
		{
			return currThread.SetLogicalCallContext(callCtx);
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06003D83 RID: 15747 RVA: 0x000D2512 File Offset: 0x000D1512
		internal static CallContextSecurityData SecurityData
		{
			get
			{
				return Thread.CurrentThread.GetLogicalCallContext().SecurityData;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06003D84 RID: 15748 RVA: 0x000D2523 File Offset: 0x000D1523
		internal static CallContextRemotingData RemotingData
		{
			get
			{
				return Thread.CurrentThread.GetLogicalCallContext().RemotingData;
			}
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x000D2534 File Offset: 0x000D1534
		public static void FreeNamedDataSlot(string name)
		{
			Thread.CurrentThread.GetLogicalCallContext().FreeNamedDataSlot(name);
			Thread.CurrentThread.GetIllogicalCallContext().FreeNamedDataSlot(name);
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x000D2558 File Offset: 0x000D1558
		public static object LogicalGetData(string name)
		{
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			return logicalCallContext.GetData(name);
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x000D2578 File Offset: 0x000D1578
		private static object IllogicalGetData(string name)
		{
			IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
			return illogicalCallContext.GetData(name);
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06003D88 RID: 15752 RVA: 0x000D2598 File Offset: 0x000D1598
		// (set) Token: 0x06003D89 RID: 15753 RVA: 0x000D25B4 File Offset: 0x000D15B4
		internal static IPrincipal Principal
		{
			get
			{
				LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
				return logicalCallContext.Principal;
			}
			set
			{
				LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
				logicalCallContext.Principal = value;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x000D25D0 File Offset: 0x000D15D0
		// (set) Token: 0x06003D8B RID: 15755 RVA: 0x000D2600 File Offset: 0x000D1600
		public static object HostContext
		{
			get
			{
				IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
				object hostContext = illogicalCallContext.HostContext;
				if (hostContext == null)
				{
					LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
					hostContext = logicalCallContext.HostContext;
				}
				return hostContext;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set
			{
				if (value is ILogicalThreadAffinative)
				{
					IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
					illogicalCallContext.HostContext = null;
					LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
					logicalCallContext.HostContext = value;
					return;
				}
				LogicalCallContext logicalCallContext2 = CallContext.GetLogicalCallContext();
				logicalCallContext2.HostContext = null;
				IllogicalCallContext illogicalCallContext2 = Thread.CurrentThread.GetIllogicalCallContext();
				illogicalCallContext2.HostContext = value;
			}
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x000D2654 File Offset: 0x000D1654
		public static object GetData(string name)
		{
			object obj = CallContext.LogicalGetData(name);
			if (obj == null)
			{
				return CallContext.IllogicalGetData(name);
			}
			return obj;
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x000D2674 File Offset: 0x000D1674
		public static void SetData(string name, object data)
		{
			if (data is ILogicalThreadAffinative)
			{
				CallContext.LogicalSetData(name, data);
				return;
			}
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			logicalCallContext.FreeNamedDataSlot(name);
			IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
			illogicalCallContext.SetData(name, data);
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x000D26B8 File Offset: 0x000D16B8
		public static void LogicalSetData(string name, object data)
		{
			IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
			illogicalCallContext.FreeNamedDataSlot(name);
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			logicalCallContext.SetData(name, data);
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x000D26EC File Offset: 0x000D16EC
		public static Header[] GetHeaders()
		{
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			return logicalCallContext.InternalGetHeaders();
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x000D270C File Offset: 0x000D170C
		public static void SetHeaders(Header[] headers)
		{
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			logicalCallContext.InternalSetHeaders(headers);
		}
	}
}
