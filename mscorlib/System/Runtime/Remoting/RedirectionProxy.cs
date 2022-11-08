using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace System.Runtime.Remoting
{
	// Token: 0x0200073E RID: 1854
	internal class RedirectionProxy : MarshalByRefObject, IMessageSink
	{
		// Token: 0x06004265 RID: 16997 RVA: 0x000E1FB0 File Offset: 0x000E0FB0
		internal RedirectionProxy(MarshalByRefObject proxy, Type serverType)
		{
			this._proxy = proxy;
			this._realProxy = RemotingServices.GetRealProxy(this._proxy);
			this._serverType = serverType;
			this._objectMode = WellKnownObjectMode.Singleton;
		}

		// Token: 0x17000BA7 RID: 2983
		// (set) Token: 0x06004266 RID: 16998 RVA: 0x000E1FDE File Offset: 0x000E0FDE
		public WellKnownObjectMode ObjectMode
		{
			set
			{
				this._objectMode = value;
			}
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x000E1FE8 File Offset: 0x000E0FE8
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			IMessage result = null;
			try
			{
				msg.Properties["__Uri"] = this._realProxy.IdentityObject.URI;
				if (this._objectMode == WellKnownObjectMode.Singleton)
				{
					result = this._realProxy.Invoke(msg);
				}
				else
				{
					MarshalByRefObject proxy = (MarshalByRefObject)Activator.CreateInstance(this._serverType, true);
					RealProxy realProxy = RemotingServices.GetRealProxy(proxy);
					result = realProxy.Invoke(msg);
				}
			}
			catch (Exception e)
			{
				result = new ReturnMessage(e, msg as IMethodCallMessage);
			}
			return result;
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x000E2074 File Offset: 0x000E1074
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMessage msg2 = this.SyncProcessMessage(msg);
			if (replySink != null)
			{
				replySink.SyncProcessMessage(msg2);
			}
			return null;
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06004269 RID: 17001 RVA: 0x000E2097 File Offset: 0x000E1097
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400213F RID: 8511
		private MarshalByRefObject _proxy;

		// Token: 0x04002140 RID: 8512
		private RealProxy _realProxy;

		// Token: 0x04002141 RID: 8513
		private Type _serverType;

		// Token: 0x04002142 RID: 8514
		private WellKnownObjectMode _objectMode;
	}
}
