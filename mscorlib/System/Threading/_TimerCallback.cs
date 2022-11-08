using System;

namespace System.Threading
{
	// Token: 0x02000180 RID: 384
	internal class _TimerCallback
	{
		// Token: 0x06001406 RID: 5126 RVA: 0x00035F78 File Offset: 0x00034F78
		internal static void TimerCallback_Context(object state)
		{
			_TimerCallback timerCallback = (_TimerCallback)state;
			timerCallback._timerCallback(timerCallback._state);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00035F9D File Offset: 0x00034F9D
		internal _TimerCallback(TimerCallback timerCallback, object state, ref StackCrawlMark stackMark)
		{
			this._timerCallback = timerCallback;
			this._state = state;
			if (!ExecutionContext.IsFlowSuppressed())
			{
				this._executionContext = ExecutionContext.Capture(ref stackMark);
				ExecutionContext.ClearSyncContext(this._executionContext);
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00035FD4 File Offset: 0x00034FD4
		internal static void PerformTimerCallback(object state)
		{
			_TimerCallback timerCallback = (_TimerCallback)state;
			if (timerCallback._executionContext == null)
			{
				TimerCallback timerCallback2 = timerCallback._timerCallback;
				timerCallback2(timerCallback._state);
				return;
			}
			ExecutionContext.Run(timerCallback._executionContext.CreateCopy(), _TimerCallback._ccb, timerCallback);
		}

		// Token: 0x040006DA RID: 1754
		private TimerCallback _timerCallback;

		// Token: 0x040006DB RID: 1755
		private ExecutionContext _executionContext;

		// Token: 0x040006DC RID: 1756
		private object _state;

		// Token: 0x040006DD RID: 1757
		internal static ContextCallback _ccb = new ContextCallback(_TimerCallback.TimerCallback_Context);
	}
}
