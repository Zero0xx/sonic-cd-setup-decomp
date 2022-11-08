using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200079B RID: 1947
	internal class SynchronizedServerContextSink : InternalSink, IMessageSink
	{
		// Token: 0x06004568 RID: 17768 RVA: 0x000EC3E0 File Offset: 0x000EB3E0
		internal SynchronizedServerContextSink(SynchronizationAttribute prop, IMessageSink nextSink)
		{
			this._property = prop;
			this._nextSink = nextSink;
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x000EC3F8 File Offset: 0x000EB3F8
		~SynchronizedServerContextSink()
		{
			this._property.Dispose();
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x000EC42C File Offset: 0x000EB42C
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			WorkItem workItem = new WorkItem(reqMsg, this._nextSink, null);
			this._property.HandleWorkRequest(workItem);
			return workItem.ReplyMessage;
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x000EC45C File Offset: 0x000EB45C
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			WorkItem workItem = new WorkItem(reqMsg, this._nextSink, replySink);
			workItem.SetAsync();
			this._property.HandleWorkRequest(workItem);
			return null;
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x0600456C RID: 17772 RVA: 0x000EC48A File Offset: 0x000EB48A
		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x0400227E RID: 8830
		internal IMessageSink _nextSink;

		// Token: 0x0400227F RID: 8831
		internal SynchronizationAttribute _property;
	}
}
