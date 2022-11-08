using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006D8 RID: 1752
	internal class DispatchChannelSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x06003F0C RID: 16140 RVA: 0x000D7F55 File Offset: 0x000D6F55
		internal DispatchChannelSink()
		{
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x000D7F5D File Offset: 0x000D6F5D
		public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			if (requestMsg == null)
			{
				throw new ArgumentNullException("requestMsg", Environment.GetResourceString("Remoting_Channel_DispatchSinkMessageMissing"));
			}
			if (requestStream != null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Channel_DispatchSinkWantsNullRequestStream"));
			}
			responseHeaders = null;
			responseStream = null;
			return ChannelServices.DispatchMessage(sinkStack, requestMsg, out responseMsg);
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x000D7F9C File Offset: 0x000D6F9C
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x000D7FA3 File Offset: 0x000D6FA3
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003F10 RID: 16144 RVA: 0x000D7FAA File Offset: 0x000D6FAA
		public IServerChannelSink NextChannelSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003F11 RID: 16145 RVA: 0x000D7FAD File Offset: 0x000D6FAD
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}
	}
}
