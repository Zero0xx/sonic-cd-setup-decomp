using System;

namespace System.Windows.Forms
{
	// Token: 0x02000735 RID: 1845
	public class WebBrowserNavigatedEventArgs : EventArgs
	{
		// Token: 0x06006289 RID: 25225 RVA: 0x001667F1 File Offset: 0x001657F1
		public WebBrowserNavigatedEventArgs(Uri url)
		{
			this.url = url;
		}

		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x0600628A RID: 25226 RVA: 0x00166800 File Offset: 0x00165800
		public Uri Url
		{
			get
			{
				WebBrowser.EnsureUrlConnectPermission(this.url);
				return this.url;
			}
		}

		// Token: 0x04003B1D RID: 15133
		private Uri url;
	}
}
