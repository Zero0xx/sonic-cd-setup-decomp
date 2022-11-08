using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x0200076B RID: 1899
	[ComVisible(true)]
	[Guid("82840BE1-D273-11D2-B94A-00600893B17A")]
	[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class PerformanceCounterManager : ICollectData
	{
		// Token: 0x06003A76 RID: 14966 RVA: 0x000F8FF6 File Offset: 0x000F7FF6
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public PerformanceCounterManager()
		{
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x000F8FFE File Offset: 0x000F7FFE
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		void ICollectData.CollectData(int callIdx, IntPtr valueNamePtr, IntPtr dataPtr, int totalBytes, out IntPtr res)
		{
			res = (IntPtr)(-1);
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x000F900D File Offset: 0x000F800D
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		void ICollectData.CloseData()
		{
		}
	}
}
