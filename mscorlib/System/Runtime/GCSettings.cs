using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace System.Runtime
{
	// Token: 0x02000610 RID: 1552
	public static class GCSettings
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003819 RID: 14361 RVA: 0x000BC1F8 File Offset: 0x000BB1F8
		// (set) Token: 0x0600381A RID: 14362 RVA: 0x000BC1FF File Offset: 0x000BB1FF
		public static GCLatencyMode LatencyMode
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return (GCLatencyMode)GC.nativeGetGCLatencyMode();
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
			set
			{
				if (value < GCLatencyMode.Batch || value > GCLatencyMode.LowLatency)
				{
					throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_Enum"));
				}
				GC.nativeSetGCLatencyMode((int)value);
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x0600381B RID: 14363 RVA: 0x000BC21F File Offset: 0x000BB21F
		public static bool IsServerGC
		{
			get
			{
				return GC.nativeIsServerGC();
			}
		}
	}
}
