using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020007A4 RID: 1956
	internal class DisposeSink : IMessageSink
	{
		// Token: 0x0600459F RID: 17823 RVA: 0x000ECDEF File Offset: 0x000EBDEF
		internal DisposeSink(IDisposable iDis, IMessageSink replySink)
		{
			this._iDis = iDis;
			this._replySink = replySink;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x000ECE08 File Offset: 0x000EBE08
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage result = null;
			try
			{
				if (this._replySink != null)
				{
					result = this._replySink.SyncProcessMessage(reqMsg);
				}
			}
			finally
			{
				this._iDis.Dispose();
			}
			return result;
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x000ECE4C File Offset: 0x000EBE4C
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x060045A2 RID: 17826 RVA: 0x000ECE53 File Offset: 0x000EBE53
		public IMessageSink NextSink
		{
			get
			{
				return this._replySink;
			}
		}

		// Token: 0x0400229D RID: 8861
		private IDisposable _iDis;

		// Token: 0x0400229E RID: 8862
		private IMessageSink _replySink;
	}
}
