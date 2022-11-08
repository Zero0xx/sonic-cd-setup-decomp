using System;
using System.Collections;
using System.IO;
using System.Net;

namespace System.Xml
{
	// Token: 0x0200002F RID: 47
	internal class XmlDownloadManager
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00007244 File Offset: 0x00006244
		internal Stream GetStream(Uri uri, ICredentials credentials)
		{
			if (uri.Scheme == "file")
			{
				return new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1);
			}
			return this.GetNonFileStream(uri, credentials);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007270 File Offset: 0x00006270
		private Stream GetNonFileStream(Uri uri, ICredentials credentials)
		{
			WebRequest webRequest = WebRequest.Create(uri);
			if (credentials != null)
			{
				webRequest.Credentials = credentials;
			}
			WebResponse response = webRequest.GetResponse();
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest != null)
			{
				lock (this)
				{
					if (this.connections == null)
					{
						this.connections = new Hashtable();
					}
					OpenedHost openedHost = (OpenedHost)this.connections[httpWebRequest.Address.Host];
					if (openedHost == null)
					{
						openedHost = new OpenedHost();
					}
					if (openedHost.nonCachedConnectionsCount < httpWebRequest.ServicePoint.ConnectionLimit - 1)
					{
						if (openedHost.nonCachedConnectionsCount == 0)
						{
							this.connections.Add(httpWebRequest.Address.Host, openedHost);
						}
						openedHost.nonCachedConnectionsCount++;
						return new XmlRegisteredNonCachedStream(response.GetResponseStream(), this, httpWebRequest.Address.Host);
					}
					return new XmlCachedStream(response.ResponseUri, response.GetResponseStream());
				}
			}
			return response.GetResponseStream();
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007378 File Offset: 0x00006378
		internal void Remove(string host)
		{
			lock (this)
			{
				OpenedHost openedHost = (OpenedHost)this.connections[host];
				if (openedHost != null && --openedHost.nonCachedConnectionsCount == 0)
				{
					this.connections.Remove(host);
				}
			}
		}

		// Token: 0x040004B4 RID: 1204
		private Hashtable connections;
	}
}
