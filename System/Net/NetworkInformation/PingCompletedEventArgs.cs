using System;
using System.ComponentModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000620 RID: 1568
	public class PingCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06003032 RID: 12338 RVA: 0x000D0020 File Offset: 0x000CF020
		internal PingCompletedEventArgs(PingReply reply, Exception error, bool cancelled, object userToken) : base(error, cancelled, userToken)
		{
			this.reply = reply;
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003033 RID: 12339 RVA: 0x000D0033 File Offset: 0x000CF033
		public PingReply Reply
		{
			get
			{
				return this.reply;
			}
		}

		// Token: 0x04002DEA RID: 11754
		private PingReply reply;
	}
}
