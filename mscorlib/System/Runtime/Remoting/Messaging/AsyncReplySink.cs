using System;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020007A2 RID: 1954
	internal class AsyncReplySink : IMessageSink
	{
		// Token: 0x06004593 RID: 17811 RVA: 0x000ECBBE File Offset: 0x000EBBBE
		internal AsyncReplySink(IMessageSink replySink, Context cliCtx)
		{
			this._replySink = replySink;
			this._cliCtx = cliCtx;
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x000ECBD4 File Offset: 0x000EBBD4
		internal static object SyncProcessMessageCallback(object[] args)
		{
			IMessage msg = (IMessage)args[0];
			IMessageSink messageSink = (IMessageSink)args[1];
			Thread.CurrentContext.NotifyDynamicSinks(msg, true, false, true, true);
			return messageSink.SyncProcessMessage(msg);
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x000ECC0C File Offset: 0x000EBC0C
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage result = null;
			if (this._replySink != null)
			{
				object[] args = new object[]
				{
					reqMsg,
					this._replySink
				};
				InternalCrossContextDelegate ftnToCall = new InternalCrossContextDelegate(AsyncReplySink.SyncProcessMessageCallback);
				result = (IMessage)Thread.CurrentThread.InternalCrossContextCallback(this._cliCtx, ftnToCall, args);
			}
			return result;
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x000ECC5F File Offset: 0x000EBC5F
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06004597 RID: 17815 RVA: 0x000ECC66 File Offset: 0x000EBC66
		public IMessageSink NextSink
		{
			get
			{
				return this._replySink;
			}
		}

		// Token: 0x04002299 RID: 8857
		private IMessageSink _replySink;

		// Token: 0x0400229A RID: 8858
		private Context _cliCtx;
	}
}
