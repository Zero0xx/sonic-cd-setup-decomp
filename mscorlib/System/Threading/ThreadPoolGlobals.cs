using System;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x0200016F RID: 367
	internal static class ThreadPoolGlobals
	{
		// Token: 0x060013A9 RID: 5033 RVA: 0x00035551 File Offset: 0x00034551
		[EnvironmentPermission(SecurityAction.Assert, Read = "NUMBER_OF_PROCESSORS")]
		internal static int GetProcessorCount()
		{
			return Environment.ProcessorCount;
		}

		// Token: 0x040006AF RID: 1711
		public static uint tpQuantum = 2U;

		// Token: 0x040006B0 RID: 1712
		public static int tpWarmupCount = ThreadPoolGlobals.GetProcessorCount() * 2;

		// Token: 0x040006B1 RID: 1713
		public static bool tpHosted = ThreadPool.IsThreadPoolHosted();

		// Token: 0x040006B2 RID: 1714
		public static bool vmTpInitialized;

		// Token: 0x040006B3 RID: 1715
		public static ThreadPoolRequestQueue tpQueue = new ThreadPoolRequestQueue();
	}
}
