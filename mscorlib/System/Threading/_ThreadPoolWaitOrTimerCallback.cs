using System;

namespace System.Threading
{
	// Token: 0x02000176 RID: 374
	internal class _ThreadPoolWaitOrTimerCallback
	{
		// Token: 0x060013CA RID: 5066 RVA: 0x00035A21 File Offset: 0x00034A21
		internal _ThreadPoolWaitOrTimerCallback(WaitOrTimerCallback waitOrTimerCallback, object state, bool compressStack, ref StackCrawlMark stackMark)
		{
			this._waitOrTimerCallback = waitOrTimerCallback;
			this._state = state;
			if (compressStack && !ExecutionContext.IsFlowSuppressed())
			{
				this._executionContext = ExecutionContext.Capture(ref stackMark);
				ExecutionContext.ClearSyncContext(this._executionContext);
			}
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00035A59 File Offset: 0x00034A59
		private static void WaitOrTimerCallback_Context_t(object state)
		{
			_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context(state, true);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00035A62 File Offset: 0x00034A62
		private static void WaitOrTimerCallback_Context_f(object state)
		{
			_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context(state, false);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00035A6C File Offset: 0x00034A6C
		private static void WaitOrTimerCallback_Context(object state, bool timedOut)
		{
			_ThreadPoolWaitOrTimerCallback threadPoolWaitOrTimerCallback = (_ThreadPoolWaitOrTimerCallback)state;
			threadPoolWaitOrTimerCallback._waitOrTimerCallback(threadPoolWaitOrTimerCallback._state, timedOut);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00035A94 File Offset: 0x00034A94
		internal static void PerformWaitOrTimerCallback(object state, bool timedOut)
		{
			_ThreadPoolWaitOrTimerCallback threadPoolWaitOrTimerCallback = (_ThreadPoolWaitOrTimerCallback)state;
			if (threadPoolWaitOrTimerCallback._executionContext == null)
			{
				WaitOrTimerCallback waitOrTimerCallback = threadPoolWaitOrTimerCallback._waitOrTimerCallback;
				waitOrTimerCallback(threadPoolWaitOrTimerCallback._state, timedOut);
				return;
			}
			if (timedOut)
			{
				ExecutionContext.Run(threadPoolWaitOrTimerCallback._executionContext.CreateCopy(), _ThreadPoolWaitOrTimerCallback._ccbt, threadPoolWaitOrTimerCallback);
				return;
			}
			ExecutionContext.Run(threadPoolWaitOrTimerCallback._executionContext.CreateCopy(), _ThreadPoolWaitOrTimerCallback._ccbf, threadPoolWaitOrTimerCallback);
		}

		// Token: 0x040006C3 RID: 1731
		private WaitOrTimerCallback _waitOrTimerCallback;

		// Token: 0x040006C4 RID: 1732
		private ExecutionContext _executionContext;

		// Token: 0x040006C5 RID: 1733
		private object _state;

		// Token: 0x040006C6 RID: 1734
		private static ContextCallback _ccbt = new ContextCallback(_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context_t);

		// Token: 0x040006C7 RID: 1735
		private static ContextCallback _ccbf = new ContextCallback(_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context_f);
	}
}
