using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006CB RID: 1739
	internal class AsyncWorkItem : IMessageSink
	{
		// Token: 0x06003EC4 RID: 16068 RVA: 0x000D7522 File Offset: 0x000D6522
		internal AsyncWorkItem(IMessageSink replySink, Context oldCtx) : this(null, replySink, oldCtx, null)
		{
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x000D752E File Offset: 0x000D652E
		internal AsyncWorkItem(IMessage reqMsg, IMessageSink replySink, Context oldCtx, ServerIdentity srvID)
		{
			this._reqMsg = reqMsg;
			this._replySink = replySink;
			this._oldCtx = oldCtx;
			this._callCtx = CallContext.GetLogicalCallContext();
			this._srvID = srvID;
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x000D7560 File Offset: 0x000D6560
		internal static object SyncProcessMessageCallback(object[] args)
		{
			IMessageSink messageSink = (IMessageSink)args[0];
			IMessage msg = (IMessage)args[1];
			return messageSink.SyncProcessMessage(msg);
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x000D7588 File Offset: 0x000D6588
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			IMessage result = null;
			if (this._replySink != null)
			{
				Thread.CurrentContext.NotifyDynamicSinks(msg, false, false, true, true);
				object[] args = new object[]
				{
					this._replySink,
					msg
				};
				InternalCrossContextDelegate ftnToCall = new InternalCrossContextDelegate(AsyncWorkItem.SyncProcessMessageCallback);
				result = (IMessage)Thread.CurrentThread.InternalCrossContextCallback(this._oldCtx, ftnToCall, args);
			}
			return result;
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x000D75EB File Offset: 0x000D65EB
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x000D75FC File Offset: 0x000D65FC
		public IMessageSink NextSink
		{
			get
			{
				return this._replySink;
			}
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x000D7604 File Offset: 0x000D6604
		internal static object FinishAsyncWorkCallback(object[] args)
		{
			AsyncWorkItem asyncWorkItem = (AsyncWorkItem)args[0];
			Context serverContext = asyncWorkItem._srvID.ServerContext;
			LogicalCallContext logicalCallContext = CallContext.SetLogicalCallContext(asyncWorkItem._callCtx);
			serverContext.NotifyDynamicSinks(asyncWorkItem._reqMsg, false, true, true, true);
			serverContext.GetServerContextChain().AsyncProcessMessage(asyncWorkItem._reqMsg, asyncWorkItem);
			CallContext.SetLogicalCallContext(logicalCallContext);
			return null;
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x000D7660 File Offset: 0x000D6660
		internal virtual void FinishAsyncWork(object stateIgnored)
		{
			InternalCrossContextDelegate ftnToCall = new InternalCrossContextDelegate(AsyncWorkItem.FinishAsyncWorkCallback);
			object[] args = new object[]
			{
				this
			};
			Thread.CurrentThread.InternalCrossContextCallback(this._srvID.ServerContext, ftnToCall, args);
		}

		// Token: 0x04001FED RID: 8173
		private IMessageSink _replySink;

		// Token: 0x04001FEE RID: 8174
		private ServerIdentity _srvID;

		// Token: 0x04001FEF RID: 8175
		private Context _oldCtx;

		// Token: 0x04001FF0 RID: 8176
		private LogicalCallContext _callCtx;

		// Token: 0x04001FF1 RID: 8177
		private IMessage _reqMsg;
	}
}
