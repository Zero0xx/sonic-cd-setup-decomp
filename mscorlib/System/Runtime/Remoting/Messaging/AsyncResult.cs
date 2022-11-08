using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006A6 RID: 1702
	[ComVisible(true)]
	public class AsyncResult : IAsyncResult, IMessageSink
	{
		// Token: 0x06003D71 RID: 15729 RVA: 0x000D2361 File Offset: 0x000D1361
		internal AsyncResult(Message m)
		{
			m.GetAsyncBeginInfo(out this._acbd, out this._asyncState);
			this._asyncDelegate = (Delegate)m.GetThisPtr();
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06003D72 RID: 15730 RVA: 0x000D238C File Offset: 0x000D138C
		public virtual bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06003D73 RID: 15731 RVA: 0x000D2394 File Offset: 0x000D1394
		public virtual object AsyncDelegate
		{
			get
			{
				return this._asyncDelegate;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06003D74 RID: 15732 RVA: 0x000D239C File Offset: 0x000D139C
		public virtual object AsyncState
		{
			get
			{
				return this._asyncState;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06003D75 RID: 15733 RVA: 0x000D23A4 File Offset: 0x000D13A4
		public virtual bool CompletedSynchronously
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06003D76 RID: 15734 RVA: 0x000D23A7 File Offset: 0x000D13A7
		// (set) Token: 0x06003D77 RID: 15735 RVA: 0x000D23AF File Offset: 0x000D13AF
		public bool EndInvokeCalled
		{
			get
			{
				return this._endInvokeCalled;
			}
			set
			{
				this._endInvokeCalled = value;
			}
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x000D23B8 File Offset: 0x000D13B8
		private void FaultInWaitHandle()
		{
			lock (this)
			{
				if (this._AsyncWaitHandle == null)
				{
					this._AsyncWaitHandle = new ManualResetEvent(this._isCompleted);
				}
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06003D79 RID: 15737 RVA: 0x000D2400 File Offset: 0x000D1400
		public virtual WaitHandle AsyncWaitHandle
		{
			get
			{
				this.FaultInWaitHandle();
				return this._AsyncWaitHandle;
			}
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x000D240E File Offset: 0x000D140E
		public virtual void SetMessageCtrl(IMessageCtrl mc)
		{
			this._mc = mc;
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x000D2418 File Offset: 0x000D1418
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			if (msg == null)
			{
				this._replyMsg = new ReturnMessage(new RemotingException(Environment.GetResourceString("Remoting_NullMessage")), new ErrorMessage());
			}
			else if (!(msg is IMethodReturnMessage))
			{
				this._replyMsg = new ReturnMessage(new RemotingException(Environment.GetResourceString("Remoting_Message_BadType")), new ErrorMessage());
			}
			else
			{
				this._replyMsg = msg;
			}
			lock (this)
			{
				this._isCompleted = true;
				if (this._AsyncWaitHandle != null)
				{
					this._AsyncWaitHandle.Set();
				}
			}
			if (this._acbd != null)
			{
				this._acbd(this);
			}
			return null;
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x000D24CC File Offset: 0x000D14CC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x000D24DD File Offset: 0x000D14DD
		public IMessageSink NextSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return null;
			}
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x000D24E0 File Offset: 0x000D14E0
		public virtual IMessage GetReplyMessage()
		{
			return this._replyMsg;
		}

		// Token: 0x04001F6F RID: 8047
		private IMessageCtrl _mc;

		// Token: 0x04001F70 RID: 8048
		private AsyncCallback _acbd;

		// Token: 0x04001F71 RID: 8049
		private IMessage _replyMsg;

		// Token: 0x04001F72 RID: 8050
		private bool _isCompleted;

		// Token: 0x04001F73 RID: 8051
		private bool _endInvokeCalled;

		// Token: 0x04001F74 RID: 8052
		private ManualResetEvent _AsyncWaitHandle;

		// Token: 0x04001F75 RID: 8053
		private Delegate _asyncDelegate;

		// Token: 0x04001F76 RID: 8054
		private object _asyncState;
	}
}
