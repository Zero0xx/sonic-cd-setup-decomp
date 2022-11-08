using System;

namespace System.Threading
{
	// Token: 0x0200016A RID: 362
	internal class ThreadHelper
	{
		// Token: 0x06001309 RID: 4873 RVA: 0x000347CF File Offset: 0x000337CF
		internal ThreadHelper(Delegate start)
		{
			this._start = start;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000347DE File Offset: 0x000337DE
		internal void SetExecutionContextHelper(ExecutionContext ec)
		{
			this._executionContext = ec;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000347E8 File Offset: 0x000337E8
		internal static void ThreadStart_Context(object state)
		{
			ThreadHelper threadHelper = (ThreadHelper)state;
			if (threadHelper._start is ThreadStart)
			{
				((ThreadStart)threadHelper._start)();
				return;
			}
			((ParameterizedThreadStart)threadHelper._start)(threadHelper._startArg);
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00034830 File Offset: 0x00033830
		internal void ThreadStart(object obj)
		{
			this._startArg = obj;
			if (this._executionContext != null)
			{
				ExecutionContext.Run(this._executionContext, ThreadHelper._ccb, this);
				return;
			}
			((ParameterizedThreadStart)this._start)(obj);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00034864 File Offset: 0x00033864
		internal void ThreadStart()
		{
			if (this._executionContext != null)
			{
				ExecutionContext.Run(this._executionContext, ThreadHelper._ccb, this);
				return;
			}
			((ThreadStart)this._start)();
		}

		// Token: 0x04000697 RID: 1687
		private Delegate _start;

		// Token: 0x04000698 RID: 1688
		private object _startArg;

		// Token: 0x04000699 RID: 1689
		private ExecutionContext _executionContext;

		// Token: 0x0400069A RID: 1690
		internal static ContextCallback _ccb = new ContextCallback(ThreadHelper.ThreadStart_Context);
	}
}
