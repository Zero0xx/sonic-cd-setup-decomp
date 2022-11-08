using System;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Threading;

namespace System.Xml
{
	// Token: 0x02000046 RID: 70
	public class XmlUrlResolver : XmlResolver
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00008CD4 File Offset: 0x00007CD4
		private static XmlDownloadManager DownloadManager
		{
			get
			{
				if (XmlUrlResolver.s_DownloadManager == null)
				{
					object value = new XmlDownloadManager();
					Interlocked.CompareExchange(ref XmlUrlResolver.s_DownloadManager, value, null);
				}
				return (XmlDownloadManager)XmlUrlResolver.s_DownloadManager;
			}
		}

		// Token: 0x17000043 RID: 67
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00008D0D File Offset: 0x00007D0D
		public override ICredentials Credentials
		{
			set
			{
				this._credentials = value;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008D16 File Offset: 0x00007D16
		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			if (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream))
			{
				return XmlUrlResolver.DownloadManager.GetStream(absoluteUri, this._credentials);
			}
			throw new XmlException("Xml_UnsupportedClass", string.Empty);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008D49 File Offset: 0x00007D49
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			return base.ResolveUri(baseUri, relativeUri);
		}

		// Token: 0x040004F1 RID: 1265
		private static object s_DownloadManager;

		// Token: 0x040004F2 RID: 1266
		private ICredentials _credentials;
	}
}
