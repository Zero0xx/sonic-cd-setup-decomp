using System;

namespace System.Runtime.Remoting
{
	// Token: 0x02000762 RID: 1890
	internal class RemoteAppEntry
	{
		// Token: 0x0600433C RID: 17212 RVA: 0x000E5B54 File Offset: 0x000E4B54
		internal RemoteAppEntry(string appName, string appURI)
		{
			this._remoteAppName = appName;
			this._remoteAppURI = appURI;
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x000E5B6A File Offset: 0x000E4B6A
		internal string GetAppURI()
		{
			return this._remoteAppURI;
		}

		// Token: 0x040021CE RID: 8654
		private string _remoteAppName;

		// Token: 0x040021CF RID: 8655
		private string _remoteAppURI;
	}
}
