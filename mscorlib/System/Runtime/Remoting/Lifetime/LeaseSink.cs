using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x0200070A RID: 1802
	internal class LeaseSink : IMessageSink
	{
		// Token: 0x0600400E RID: 16398 RVA: 0x000DA26A File Offset: 0x000D926A
		public LeaseSink(Lease lease, IMessageSink nextSink)
		{
			this.lease = lease;
			this.nextSink = nextSink;
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x000DA280 File Offset: 0x000D9280
		public IMessage SyncProcessMessage(IMessage msg)
		{
			this.lease.RenewOnCall();
			return this.nextSink.SyncProcessMessage(msg);
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x000DA299 File Offset: 0x000D9299
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			this.lease.RenewOnCall();
			return this.nextSink.AsyncProcessMessage(msg, replySink);
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06004011 RID: 16401 RVA: 0x000DA2B3 File Offset: 0x000D92B3
		public IMessageSink NextSink
		{
			get
			{
				return this.nextSink;
			}
		}

		// Token: 0x04002055 RID: 8277
		private Lease lease;

		// Token: 0x04002056 RID: 8278
		private IMessageSink nextSink;
	}
}
