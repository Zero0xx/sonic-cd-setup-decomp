using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x02000736 RID: 1846
	[Serializable]
	internal sealed class EnvoyInfo : IEnvoyInfo
	{
		// Token: 0x0600420B RID: 16907 RVA: 0x000E0920 File Offset: 0x000DF920
		internal static IEnvoyInfo CreateEnvoyInfo(ServerIdentity serverID)
		{
			IEnvoyInfo result = null;
			if (serverID != null)
			{
				if (serverID.EnvoyChain == null)
				{
					serverID.RaceSetEnvoyChain(serverID.ServerContext.CreateEnvoyChain(serverID.TPOrObject));
				}
				if (!(serverID.EnvoyChain is EnvoyTerminatorSink))
				{
					result = new EnvoyInfo(serverID.EnvoyChain);
				}
			}
			return result;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x000E096E File Offset: 0x000DF96E
		private EnvoyInfo(IMessageSink sinks)
		{
			this.EnvoySinks = sinks;
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x000E097D File Offset: 0x000DF97D
		// (set) Token: 0x0600420E RID: 16910 RVA: 0x000E0985 File Offset: 0x000DF985
		public IMessageSink EnvoySinks
		{
			get
			{
				return this.envoySinks;
			}
			set
			{
				this.envoySinks = value;
			}
		}

		// Token: 0x0400211A RID: 8474
		private IMessageSink envoySinks;
	}
}
