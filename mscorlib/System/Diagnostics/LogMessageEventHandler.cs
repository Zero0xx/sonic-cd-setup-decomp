using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020002C0 RID: 704
	// (Invoke) Token: 0x06001B45 RID: 6981
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	[Serializable]
	internal delegate void LogMessageEventHandler(LoggingLevels level, LogSwitch category, string message, StackTrace location);
}
