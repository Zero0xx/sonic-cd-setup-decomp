using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B3 RID: 1715
	internal class ServerAsyncReplyTerminatorSink : IMessageSink
	{
		// Token: 0x06003DEC RID: 15852 RVA: 0x000D3D58 File Offset: 0x000D2D58
		internal ServerAsyncReplyTerminatorSink(IMessageSink nextSink)
		{
			this._nextSink = nextSink;
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x000D3D68 File Offset: 0x000D2D68
		public virtual IMessage SyncProcessMessage(IMessage replyMsg)
		{
			Guid guid;
			RemotingServices.CORProfilerRemotingServerSendingReply(out guid, true);
			if (RemotingServices.CORProfilerTrackRemotingCookie())
			{
				replyMsg.Properties["CORProfilerCookie"] = guid;
			}
			return this._nextSink.SyncProcessMessage(replyMsg);
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x000D3DA6 File Offset: 0x000D2DA6
		public virtual IMessageCtrl AsyncProcessMessage(IMessage replyMsg, IMessageSink replySink)
		{
			return null;
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06003DEF RID: 15855 RVA: 0x000D3DA9 File Offset: 0x000D2DA9
		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x04001F98 RID: 8088
		internal IMessageSink _nextSink;
	}
}
