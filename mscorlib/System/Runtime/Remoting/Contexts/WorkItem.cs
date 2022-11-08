using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200079C RID: 1948
	internal class WorkItem
	{
		// Token: 0x0600456D RID: 17773 RVA: 0x000EC492 File Offset: 0x000EB492
		internal WorkItem(IMessage reqMsg, IMessageSink nextSink, IMessageSink replySink)
		{
			this._reqMsg = reqMsg;
			this._replyMsg = null;
			this._nextSink = nextSink;
			this._replySink = replySink;
			this._ctx = Thread.CurrentContext;
			this._callCtx = CallContext.GetLogicalCallContext();
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x000EC4CC File Offset: 0x000EB4CC
		internal virtual void SetWaiting()
		{
			this._flags |= 1;
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x000EC4DC File Offset: 0x000EB4DC
		internal virtual bool IsWaiting()
		{
			return (this._flags & 1) == 1;
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x000EC4E9 File Offset: 0x000EB4E9
		internal virtual void SetSignaled()
		{
			this._flags |= 2;
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x000EC4F9 File Offset: 0x000EB4F9
		internal virtual bool IsSignaled()
		{
			return (this._flags & 2) == 2;
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x000EC506 File Offset: 0x000EB506
		internal virtual void SetAsync()
		{
			this._flags |= 4;
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x000EC516 File Offset: 0x000EB516
		internal virtual bool IsAsync()
		{
			return (this._flags & 4) == 4;
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x000EC523 File Offset: 0x000EB523
		internal virtual void SetDummy()
		{
			this._flags |= 8;
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x000EC533 File Offset: 0x000EB533
		internal virtual bool IsDummy()
		{
			return (this._flags & 8) == 8;
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x000EC540 File Offset: 0x000EB540
		internal static object ExecuteCallback(object[] args)
		{
			WorkItem workItem = (WorkItem)args[0];
			if (workItem.IsAsync())
			{
				workItem._nextSink.AsyncProcessMessage(workItem._reqMsg, workItem._replySink);
			}
			else if (workItem._nextSink != null)
			{
				workItem._replyMsg = workItem._nextSink.SyncProcessMessage(workItem._reqMsg);
			}
			return null;
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x000EC598 File Offset: 0x000EB598
		internal virtual void Execute()
		{
			Thread.CurrentThread.InternalCrossContextCallback(this._ctx, WorkItem._xctxDel, new object[]
			{
				this
			});
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06004578 RID: 17784 RVA: 0x000EC5C7 File Offset: 0x000EB5C7
		internal virtual IMessage ReplyMessage
		{
			get
			{
				return this._replyMsg;
			}
		}

		// Token: 0x04002280 RID: 8832
		private const int FLG_WAITING = 1;

		// Token: 0x04002281 RID: 8833
		private const int FLG_SIGNALED = 2;

		// Token: 0x04002282 RID: 8834
		private const int FLG_ASYNC = 4;

		// Token: 0x04002283 RID: 8835
		private const int FLG_DUMMY = 8;

		// Token: 0x04002284 RID: 8836
		internal int _flags;

		// Token: 0x04002285 RID: 8837
		internal IMessage _reqMsg;

		// Token: 0x04002286 RID: 8838
		internal IMessageSink _nextSink;

		// Token: 0x04002287 RID: 8839
		internal IMessageSink _replySink;

		// Token: 0x04002288 RID: 8840
		internal IMessage _replyMsg;

		// Token: 0x04002289 RID: 8841
		internal Context _ctx;

		// Token: 0x0400228A RID: 8842
		internal LogicalCallContext _callCtx;

		// Token: 0x0400228B RID: 8843
		internal static InternalCrossContextDelegate _xctxDel = new InternalCrossContextDelegate(WorkItem.ExecuteCallback);
	}
}
