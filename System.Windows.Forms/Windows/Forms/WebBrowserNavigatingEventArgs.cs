using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000737 RID: 1847
	public class WebBrowserNavigatingEventArgs : CancelEventArgs
	{
		// Token: 0x0600628F RID: 25231 RVA: 0x00166813 File Offset: 0x00165813
		public WebBrowserNavigatingEventArgs(Uri url, string targetFrameName)
		{
			this.url = url;
			this.targetFrameName = targetFrameName;
		}

		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x06006290 RID: 25232 RVA: 0x00166829 File Offset: 0x00165829
		public Uri Url
		{
			get
			{
				WebBrowser.EnsureUrlConnectPermission(this.url);
				return this.url;
			}
		}

		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x06006291 RID: 25233 RVA: 0x0016683C File Offset: 0x0016583C
		public string TargetFrameName
		{
			get
			{
				WebBrowser.EnsureUrlConnectPermission(this.url);
				return this.targetFrameName;
			}
		}

		// Token: 0x04003B1E RID: 15134
		private Uri url;

		// Token: 0x04003B1F RID: 15135
		private string targetFrameName;
	}
}
