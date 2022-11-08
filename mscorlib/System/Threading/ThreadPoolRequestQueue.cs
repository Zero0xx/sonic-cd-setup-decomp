using System;
using System.Runtime.CompilerServices;

namespace System.Threading
{
	// Token: 0x02000170 RID: 368
	internal sealed class ThreadPoolRequestQueue
	{
		// Token: 0x060013AB RID: 5035 RVA: 0x00035580 File Offset: 0x00034580
		public ThreadPoolRequestQueue()
		{
			this.tpSync = new object();
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00035594 File Offset: 0x00034594
		public uint EnQueue(_ThreadPoolWaitCallback tpcallBack)
		{
			uint result = 0U;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(this.tpSync);
					flag = true;
				}
				catch (Exception)
				{
				}
				if (flag)
				{
					if (this.tpCount == 0U)
					{
						flag2 = ThreadPool.SetAppDomainRequestActive();
					}
					this.tpCount += 1U;
					result = this.tpCount;
					if (this.tpHead == null)
					{
						this.tpHead = tpcallBack;
						this.tpTail = tpcallBack;
					}
					else
					{
						this.tpTail._next = tpcallBack;
						this.tpTail = tpcallBack;
					}
					Monitor.Exit(this.tpSync);
					if (flag2)
					{
						ThreadPool.SetNativeTpEvent();
					}
				}
			}
			return result;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00035648 File Offset: 0x00034648
		public _ThreadPoolWaitCallback DeQueue()
		{
			bool flag = false;
			_ThreadPoolWaitCallback result = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(this.tpSync);
					flag = true;
				}
				catch (Exception)
				{
				}
				if (flag)
				{
					_ThreadPoolWaitCallback threadPoolWaitCallback = this.tpHead;
					if (threadPoolWaitCallback != null)
					{
						result = threadPoolWaitCallback;
						this.tpHead = threadPoolWaitCallback._next;
						this.tpCount -= 1U;
						if (this.tpCount == 0U)
						{
							this.tpTail = null;
							ThreadPool.ClearAppDomainRequestActive();
						}
					}
					Monitor.Exit(this.tpSync);
				}
			}
			return result;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x000356E0 File Offset: 0x000346E0
		public uint GetQueueCount()
		{
			return this.tpCount;
		}

		// Token: 0x040006B4 RID: 1716
		private _ThreadPoolWaitCallback tpHead;

		// Token: 0x040006B5 RID: 1717
		private _ThreadPoolWaitCallback tpTail;

		// Token: 0x040006B6 RID: 1718
		private object tpSync;

		// Token: 0x040006B7 RID: 1719
		private uint tpCount;
	}
}
