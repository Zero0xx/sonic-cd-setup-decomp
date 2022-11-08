using System;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020007A0 RID: 1952
	[Serializable]
	internal class EnvoyTerminatorSink : InternalSink, IMessageSink
	{
		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x000EC830 File Offset: 0x000EB830
		internal static IMessageSink MessageSink
		{
			get
			{
				if (EnvoyTerminatorSink.messageSink == null)
				{
					EnvoyTerminatorSink envoyTerminatorSink = new EnvoyTerminatorSink();
					lock (EnvoyTerminatorSink.staticSyncObject)
					{
						if (EnvoyTerminatorSink.messageSink == null)
						{
							EnvoyTerminatorSink.messageSink = envoyTerminatorSink;
						}
					}
				}
				return EnvoyTerminatorSink.messageSink;
			}
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x000EC884 File Offset: 0x000EB884
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			if (message != null)
			{
				return message;
			}
			return Thread.CurrentContext.GetClientContextChain().SyncProcessMessage(reqMsg);
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x000EC8B0 File Offset: 0x000EB8B0
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			IMessageCtrl result = null;
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			if (message != null)
			{
				if (replySink != null)
				{
					replySink.SyncProcessMessage(message);
				}
			}
			else
			{
				result = Thread.CurrentContext.GetClientContextChain().AsyncProcessMessage(reqMsg, replySink);
			}
			return result;
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06004587 RID: 17799 RVA: 0x000EC8E9 File Offset: 0x000EB8E9
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04002295 RID: 8853
		private static EnvoyTerminatorSink messageSink;

		// Token: 0x04002296 RID: 8854
		private static object staticSyncObject = new object();
	}
}
