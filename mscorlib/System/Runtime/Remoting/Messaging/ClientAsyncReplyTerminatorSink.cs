using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020007A6 RID: 1958
	internal class ClientAsyncReplyTerminatorSink : IMessageSink
	{
		// Token: 0x060045A7 RID: 17831 RVA: 0x000ECF37 File Offset: 0x000EBF37
		internal ClientAsyncReplyTerminatorSink(IMessageSink nextSink)
		{
			this._nextSink = nextSink;
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x000ECF48 File Offset: 0x000EBF48
		public virtual IMessage SyncProcessMessage(IMessage replyMsg)
		{
			Guid id = Guid.Empty;
			if (RemotingServices.CORProfilerTrackRemotingCookie())
			{
				object obj = replyMsg.Properties["CORProfilerCookie"];
				if (obj != null)
				{
					id = (Guid)obj;
				}
			}
			RemotingServices.CORProfilerRemotingClientReceivingReply(id, true);
			return this._nextSink.SyncProcessMessage(replyMsg);
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x000ECF90 File Offset: 0x000EBF90
		public virtual IMessageCtrl AsyncProcessMessage(IMessage replyMsg, IMessageSink replySink)
		{
			return null;
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x060045AA RID: 17834 RVA: 0x000ECF93 File Offset: 0x000EBF93
		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x040022A0 RID: 8864
		internal IMessageSink _nextSink;
	}
}
