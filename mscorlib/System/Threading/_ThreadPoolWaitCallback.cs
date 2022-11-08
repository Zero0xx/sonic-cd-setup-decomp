using System;

namespace System.Threading
{
	// Token: 0x02000175 RID: 373
	internal class _ThreadPoolWaitCallback
	{
		// Token: 0x060013C5 RID: 5061 RVA: 0x0003591C File Offset: 0x0003491C
		internal static void WaitCallback_Context(object state)
		{
			_ThreadPoolWaitCallback threadPoolWaitCallback = (_ThreadPoolWaitCallback)state;
			threadPoolWaitCallback._waitCallback(threadPoolWaitCallback._state);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00035941 File Offset: 0x00034941
		internal _ThreadPoolWaitCallback(WaitCallback waitCallback, object state, bool compressStack, ref StackCrawlMark stackMark)
		{
			this._waitCallback = waitCallback;
			this._state = state;
			if (compressStack && !ExecutionContext.IsFlowSuppressed())
			{
				this._executionContext = ExecutionContext.Capture(ref stackMark);
				ExecutionContext.ClearSyncContext(this._executionContext);
			}
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0003597C File Offset: 0x0003497C
		internal static void PerformWaitCallback(object state)
		{
			int tickCount = Environment.TickCount;
			for (;;)
			{
				_ThreadPoolWaitCallback threadPoolWaitCallback = ThreadPoolGlobals.tpQueue.DeQueue();
				if (threadPoolWaitCallback == null)
				{
					break;
				}
				ThreadPool.CompleteThreadPoolRequest(ThreadPoolGlobals.tpQueue.GetQueueCount());
				_ThreadPoolWaitCallback.PerformWaitCallbackInternal(threadPoolWaitCallback);
				int tickCount2 = Environment.TickCount;
				int num = tickCount2 - tickCount;
				if ((long)num > (long)((ulong)ThreadPoolGlobals.tpQuantum) && ThreadPool.ShouldReturnToVm())
				{
					return;
				}
			}
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x000359D4 File Offset: 0x000349D4
		internal static void PerformWaitCallbackInternal(_ThreadPoolWaitCallback tpWaitCallBack)
		{
			if (tpWaitCallBack._executionContext == null)
			{
				WaitCallback waitCallback = tpWaitCallBack._waitCallback;
				waitCallback(tpWaitCallBack._state);
				return;
			}
			ExecutionContext.Run(tpWaitCallBack._executionContext, _ThreadPoolWaitCallback._ccb, tpWaitCallBack);
		}

		// Token: 0x040006BE RID: 1726
		private WaitCallback _waitCallback;

		// Token: 0x040006BF RID: 1727
		private ExecutionContext _executionContext;

		// Token: 0x040006C0 RID: 1728
		private object _state;

		// Token: 0x040006C1 RID: 1729
		protected internal _ThreadPoolWaitCallback _next;

		// Token: 0x040006C2 RID: 1730
		internal static ContextCallback _ccb = new ContextCallback(_ThreadPoolWaitCallback.WaitCallback_Context);
	}
}
