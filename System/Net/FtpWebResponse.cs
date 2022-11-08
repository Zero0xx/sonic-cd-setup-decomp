using System;
using System.IO;

namespace System.Net
{
	// Token: 0x020003C1 RID: 961
	public class FtpWebResponse : WebResponse, IDisposable
	{
		// Token: 0x06001E40 RID: 7744 RVA: 0x00074118 File Offset: 0x00073118
		internal FtpWebResponse(Stream responseStream, long contentLength, Uri responseUri, FtpStatusCode statusCode, string statusLine, DateTime lastModified, string bannerMessage, string welcomeMessage, string exitMessage)
		{
			this.m_ResponseStream = responseStream;
			if (responseStream == null && contentLength < 0L)
			{
				contentLength = 0L;
			}
			this.m_ContentLength = contentLength;
			this.m_ResponseUri = responseUri;
			this.m_StatusCode = statusCode;
			this.m_StatusLine = statusLine;
			this.m_LastModified = lastModified;
			this.m_BannerMessage = bannerMessage;
			this.m_WelcomeMessage = welcomeMessage;
			this.m_ExitMessage = exitMessage;
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0007417C File Offset: 0x0007317C
		internal FtpWebResponse(HttpWebResponse httpWebResponse)
		{
			this.m_HttpWebResponse = httpWebResponse;
			base.InternalSetFromCache = this.m_HttpWebResponse.IsFromCache;
			base.InternalSetIsCacheFresh = this.m_HttpWebResponse.IsCacheFresh;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000741AD File Offset: 0x000731AD
		internal void UpdateStatus(FtpStatusCode statusCode, string statusLine, string exitMessage)
		{
			this.m_StatusCode = statusCode;
			this.m_StatusLine = statusLine;
			this.m_ExitMessage = exitMessage;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000741C4 File Offset: 0x000731C4
		public override Stream GetResponseStream()
		{
			Stream result;
			if (this.HttpProxyMode)
			{
				result = this.m_HttpWebResponse.GetResponseStream();
			}
			else if (this.m_ResponseStream != null)
			{
				result = this.m_ResponseStream;
			}
			else
			{
				result = (this.m_ResponseStream = new FtpWebResponse.EmptyStream());
			}
			return result;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0007420A File Offset: 0x0007320A
		internal void SetResponseStream(Stream stream)
		{
			if (stream == null || stream == Stream.Null || stream is FtpWebResponse.EmptyStream)
			{
				return;
			}
			this.m_ResponseStream = stream;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x00074228 File Offset: 0x00073228
		public override void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Close", "");
			}
			if (this.HttpProxyMode)
			{
				this.m_HttpWebResponse.Close();
			}
			else
			{
				Stream responseStream = this.m_ResponseStream;
				if (responseStream != null)
				{
					responseStream.Close();
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "Close", "");
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x00074292 File Offset: 0x00073292
		public override long ContentLength
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.ContentLength;
				}
				return this.m_ContentLength;
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000742AE File Offset: 0x000732AE
		internal void SetContentLength(long value)
		{
			if (this.HttpProxyMode)
			{
				return;
			}
			this.m_ContentLength = value;
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x000742C0 File Offset: 0x000732C0
		public override WebHeaderCollection Headers
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.Headers;
				}
				if (this.m_FtpRequestHeaders == null)
				{
					lock (this)
					{
						if (this.m_FtpRequestHeaders == null)
						{
							this.m_FtpRequestHeaders = new WebHeaderCollection(WebHeaderCollectionType.FtpWebResponse);
						}
					}
				}
				return this.m_FtpRequestHeaders;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001E49 RID: 7753 RVA: 0x00074324 File Offset: 0x00073324
		public override Uri ResponseUri
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.ResponseUri;
				}
				return this.m_ResponseUri;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x00074340 File Offset: 0x00073340
		public FtpStatusCode StatusCode
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return (FtpStatusCode)this.m_HttpWebResponse.StatusCode;
				}
				return this.m_StatusCode;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x0007435C File Offset: 0x0007335C
		public string StatusDescription
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.StatusDescription;
				}
				return this.m_StatusLine;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x00074378 File Offset: 0x00073378
		public DateTime LastModified
		{
			get
			{
				if (this.HttpProxyMode)
				{
					return this.m_HttpWebResponse.LastModified;
				}
				return this.m_LastModified;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x00074394 File Offset: 0x00073394
		public string BannerMessage
		{
			get
			{
				return this.m_BannerMessage;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001E4E RID: 7758 RVA: 0x0007439C File Offset: 0x0007339C
		public string WelcomeMessage
		{
			get
			{
				return this.m_WelcomeMessage;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001E4F RID: 7759 RVA: 0x000743A4 File Offset: 0x000733A4
		public string ExitMessage
		{
			get
			{
				return this.m_ExitMessage;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001E50 RID: 7760 RVA: 0x000743AC File Offset: 0x000733AC
		private bool HttpProxyMode
		{
			get
			{
				return this.m_HttpWebResponse != null;
			}
		}

		// Token: 0x04001E37 RID: 7735
		internal Stream m_ResponseStream;

		// Token: 0x04001E38 RID: 7736
		private long m_ContentLength;

		// Token: 0x04001E39 RID: 7737
		private Uri m_ResponseUri;

		// Token: 0x04001E3A RID: 7738
		private FtpStatusCode m_StatusCode;

		// Token: 0x04001E3B RID: 7739
		private string m_StatusLine;

		// Token: 0x04001E3C RID: 7740
		private WebHeaderCollection m_FtpRequestHeaders;

		// Token: 0x04001E3D RID: 7741
		private HttpWebResponse m_HttpWebResponse;

		// Token: 0x04001E3E RID: 7742
		private DateTime m_LastModified;

		// Token: 0x04001E3F RID: 7743
		private string m_BannerMessage;

		// Token: 0x04001E40 RID: 7744
		private string m_WelcomeMessage;

		// Token: 0x04001E41 RID: 7745
		private string m_ExitMessage;

		// Token: 0x020003C2 RID: 962
		internal class EmptyStream : MemoryStream
		{
			// Token: 0x06001E51 RID: 7761 RVA: 0x000743BA File Offset: 0x000733BA
			internal EmptyStream() : base(new byte[0], false)
			{
			}
		}
	}
}
