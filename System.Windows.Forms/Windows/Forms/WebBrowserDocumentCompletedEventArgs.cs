using System;

namespace System.Windows.Forms
{
	// Token: 0x0200072F RID: 1839
	public class WebBrowserDocumentCompletedEventArgs : EventArgs
	{
		// Token: 0x0600627A RID: 25210 RVA: 0x001665A9 File Offset: 0x001655A9
		public WebBrowserDocumentCompletedEventArgs(Uri url)
		{
			this.url = url;
		}

		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x0600627B RID: 25211 RVA: 0x001665B8 File Offset: 0x001655B8
		public Uri Url
		{
			get
			{
				WebBrowser.EnsureUrlConnectPermission(this.url);
				return this.url;
			}
		}

		// Token: 0x04003AFC RID: 15100
		private Uri url;
	}
}
