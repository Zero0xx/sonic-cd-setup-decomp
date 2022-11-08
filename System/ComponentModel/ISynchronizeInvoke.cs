using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000103 RID: 259
	public interface ISynchronizeInvoke
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600082E RID: 2094
		bool InvokeRequired { get; }

		// Token: 0x0600082F RID: 2095
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		IAsyncResult BeginInvoke(Delegate method, object[] args);

		// Token: 0x06000830 RID: 2096
		object EndInvoke(IAsyncResult result);

		// Token: 0x06000831 RID: 2097
		object Invoke(Delegate method, object[] args);
	}
}
