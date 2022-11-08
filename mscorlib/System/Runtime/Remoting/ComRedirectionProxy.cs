using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x0200073F RID: 1855
	internal class ComRedirectionProxy : MarshalByRefObject, IMessageSink
	{
		// Token: 0x0600426A RID: 17002 RVA: 0x000E209A File Offset: 0x000E109A
		internal ComRedirectionProxy(MarshalByRefObject comObject, Type serverType)
		{
			this._comObject = comObject;
			this._serverType = serverType;
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x000E20B0 File Offset: 0x000E10B0
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			IMethodCallMessage reqMsg = (IMethodCallMessage)msg;
			IMethodReturnMessage methodReturnMessage = RemotingServices.ExecuteMessage(this._comObject, reqMsg);
			if (methodReturnMessage != null)
			{
				COMException ex = methodReturnMessage.Exception as COMException;
				if (ex != null && (ex._HResult == -2147023174 || ex._HResult == -2147023169))
				{
					this._comObject = (MarshalByRefObject)Activator.CreateInstance(this._serverType, true);
					methodReturnMessage = RemotingServices.ExecuteMessage(this._comObject, reqMsg);
				}
			}
			return methodReturnMessage;
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x000E2124 File Offset: 0x000E1124
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMessage msg2 = this.SyncProcessMessage(msg);
			if (replySink != null)
			{
				replySink.SyncProcessMessage(msg2);
			}
			return null;
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600426D RID: 17005 RVA: 0x000E2147 File Offset: 0x000E1147
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04002143 RID: 8515
		private MarshalByRefObject _comObject;

		// Token: 0x04002144 RID: 8516
		private Type _serverType;
	}
}
