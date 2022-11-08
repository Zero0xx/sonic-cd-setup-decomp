using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006CF RID: 1743
	[Serializable]
	internal class CrossAppDomainChannel : IChannelSender, IChannelReceiver, IChannel
	{
		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003ED4 RID: 16084 RVA: 0x000D769F File Offset: 0x000D669F
		// (set) Token: 0x06003ED5 RID: 16085 RVA: 0x000D76B5 File Offset: 0x000D66B5
		private static CrossAppDomainChannel gAppDomainChannel
		{
			get
			{
				return Thread.GetDomain().RemotingData.ChannelServicesData.xadmessageSink;
			}
			set
			{
				Thread.GetDomain().RemotingData.ChannelServicesData.xadmessageSink = value;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x000D76CC File Offset: 0x000D66CC
		internal static CrossAppDomainChannel AppDomainChannel
		{
			get
			{
				if (CrossAppDomainChannel.gAppDomainChannel == null)
				{
					CrossAppDomainChannel gAppDomainChannel = new CrossAppDomainChannel();
					lock (CrossAppDomainChannel.staticSyncObject)
					{
						if (CrossAppDomainChannel.gAppDomainChannel == null)
						{
							CrossAppDomainChannel.gAppDomainChannel = gAppDomainChannel;
						}
					}
				}
				return CrossAppDomainChannel.gAppDomainChannel;
			}
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x000D7720 File Offset: 0x000D6720
		internal static void RegisterChannel()
		{
			CrossAppDomainChannel appDomainChannel = CrossAppDomainChannel.AppDomainChannel;
			ChannelServices.RegisterChannelInternal(appDomainChannel, false);
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003ED8 RID: 16088 RVA: 0x000D773A File Offset: 0x000D673A
		public virtual string ChannelName
		{
			get
			{
				return "XAPPDMN";
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003ED9 RID: 16089 RVA: 0x000D7741 File Offset: 0x000D6741
		public virtual string ChannelURI
		{
			get
			{
				return "XAPPDMN_URI";
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003EDA RID: 16090 RVA: 0x000D7748 File Offset: 0x000D6748
		public virtual int ChannelPriority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x000D774C File Offset: 0x000D674C
		public string Parse(string url, out string objectURI)
		{
			objectURI = url;
			return null;
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003EDC RID: 16092 RVA: 0x000D7752 File Offset: 0x000D6752
		public virtual object ChannelData
		{
			get
			{
				return new CrossAppDomainData(Context.DefaultContext.InternalContextID, Thread.GetDomain().GetId(), Identity.ProcessGuid);
			}
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x000D7774 File Offset: 0x000D6774
		public virtual IMessageSink CreateMessageSink(string url, object data, out string objectURI)
		{
			objectURI = null;
			IMessageSink result = null;
			if (url != null && data == null)
			{
				if (url.StartsWith("XAPPDMN", StringComparison.Ordinal))
				{
					throw new RemotingException(Environment.GetResourceString("Remoting_AppDomains_NYI"));
				}
			}
			else
			{
				CrossAppDomainData crossAppDomainData = data as CrossAppDomainData;
				if (crossAppDomainData != null && crossAppDomainData.ProcessGuid.Equals(Identity.ProcessGuid))
				{
					result = CrossAppDomainSink.FindOrCreateSink(crossAppDomainData);
				}
			}
			return result;
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x000D77CE File Offset: 0x000D67CE
		public virtual string[] GetUrlsForUri(string objectURI)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x000D77DF File Offset: 0x000D67DF
		public virtual void StartListening(object data)
		{
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x000D77E1 File Offset: 0x000D67E1
		public virtual void StopListening(object data)
		{
		}

		// Token: 0x04001FF2 RID: 8178
		private const string _channelName = "XAPPDMN";

		// Token: 0x04001FF3 RID: 8179
		private const string _channelURI = "XAPPDMN_URI";

		// Token: 0x04001FF4 RID: 8180
		private static object staticSyncObject = new object();

		// Token: 0x04001FF5 RID: 8181
		private static PermissionSet s_fullTrust = new PermissionSet(PermissionState.Unrestricted);
	}
}
