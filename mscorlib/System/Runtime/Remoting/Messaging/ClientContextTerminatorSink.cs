using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020007A1 RID: 1953
	internal class ClientContextTerminatorSink : InternalSink, IMessageSink
	{
		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x0600458A RID: 17802 RVA: 0x000EC900 File Offset: 0x000EB900
		internal static IMessageSink MessageSink
		{
			get
			{
				if (ClientContextTerminatorSink.messageSink == null)
				{
					ClientContextTerminatorSink clientContextTerminatorSink = new ClientContextTerminatorSink();
					lock (ClientContextTerminatorSink.staticSyncObject)
					{
						if (ClientContextTerminatorSink.messageSink == null)
						{
							ClientContextTerminatorSink.messageSink = clientContextTerminatorSink;
						}
					}
				}
				return ClientContextTerminatorSink.messageSink;
			}
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x000EC954 File Offset: 0x000EB954
		internal static object SyncProcessMessageCallback(object[] args)
		{
			IMessage msg = (IMessage)args[0];
			IMessageSink messageSink = (IMessageSink)args[1];
			return messageSink.SyncProcessMessage(msg);
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x000EC97C File Offset: 0x000EB97C
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			if (message != null)
			{
				return message;
			}
			Context currentContext = Thread.CurrentContext;
			bool flag = currentContext.NotifyDynamicSinks(reqMsg, true, true, false, true);
			IMessage message2;
			if (reqMsg is IConstructionCallMessage)
			{
				message = currentContext.NotifyActivatorProperties(reqMsg, false);
				if (message != null)
				{
					return message;
				}
				message2 = ((IConstructionCallMessage)reqMsg).Activator.Activate((IConstructionCallMessage)reqMsg);
				message = currentContext.NotifyActivatorProperties(message2, false);
				if (message != null)
				{
					return message;
				}
			}
			else
			{
				ChannelServices.NotifyProfiler(reqMsg, RemotingProfilerEvent.ClientSend);
				object[] array = new object[2];
				object[] array2 = array;
				IMessageSink channelSink = this.GetChannelSink(reqMsg);
				array2[0] = reqMsg;
				array2[1] = channelSink;
				InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(ClientContextTerminatorSink.SyncProcessMessageCallback);
				if (channelSink != CrossContextChannel.MessageSink)
				{
					message2 = (IMessage)Thread.CurrentThread.InternalCrossContextCallback(Context.DefaultContext, internalCrossContextDelegate, array2);
				}
				else
				{
					message2 = (IMessage)internalCrossContextDelegate(array2);
				}
				ChannelServices.NotifyProfiler(message2, RemotingProfilerEvent.ClientReceive);
			}
			if (flag)
			{
				currentContext.NotifyDynamicSinks(reqMsg, true, false, false, true);
			}
			return message2;
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x000ECA64 File Offset: 0x000EBA64
		internal static object AsyncProcessMessageCallback(object[] args)
		{
			IMessage msg = (IMessage)args[0];
			IMessageSink replySink = (IMessageSink)args[1];
			IMessageSink messageSink = (IMessageSink)args[2];
			return messageSink.AsyncProcessMessage(msg, replySink);
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x000ECA94 File Offset: 0x000EBA94
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			IMessageCtrl result = null;
			if (message == null)
			{
				message = InternalSink.DisallowAsyncActivation(reqMsg);
			}
			if (message != null)
			{
				if (replySink != null)
				{
					replySink.SyncProcessMessage(message);
				}
			}
			else
			{
				if (RemotingServices.CORProfilerTrackRemotingAsync())
				{
					Guid guid;
					RemotingServices.CORProfilerRemotingClientSendingMessage(out guid, true);
					if (RemotingServices.CORProfilerTrackRemotingCookie())
					{
						reqMsg.Properties["CORProfilerCookie"] = guid;
					}
					if (replySink != null)
					{
						IMessageSink messageSink = new ClientAsyncReplyTerminatorSink(replySink);
						replySink = messageSink;
					}
				}
				Context currentContext = Thread.CurrentContext;
				currentContext.NotifyDynamicSinks(reqMsg, true, true, true, true);
				if (replySink != null)
				{
					replySink = new AsyncReplySink(replySink, currentContext);
				}
				object[] array = new object[3];
				object[] array2 = array;
				InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(ClientContextTerminatorSink.AsyncProcessMessageCallback);
				IMessageSink channelSink = this.GetChannelSink(reqMsg);
				array2[0] = reqMsg;
				array2[1] = replySink;
				array2[2] = channelSink;
				if (channelSink != CrossContextChannel.MessageSink)
				{
					result = (IMessageCtrl)Thread.CurrentThread.InternalCrossContextCallback(Context.DefaultContext, internalCrossContextDelegate, array2);
				}
				else
				{
					result = (IMessageCtrl)internalCrossContextDelegate(array2);
				}
			}
			return result;
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x0600458F RID: 17807 RVA: 0x000ECB8B File Offset: 0x000EBB8B
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x000ECB90 File Offset: 0x000EBB90
		private IMessageSink GetChannelSink(IMessage reqMsg)
		{
			Identity identity = InternalSink.GetIdentity(reqMsg);
			return identity.ChannelSink;
		}

		// Token: 0x04002297 RID: 8855
		private static ClientContextTerminatorSink messageSink;

		// Token: 0x04002298 RID: 8856
		private static object staticSyncObject = new object();
	}
}
