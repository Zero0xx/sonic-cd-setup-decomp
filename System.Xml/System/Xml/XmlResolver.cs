using System;
using System.IO;
using System.Net;

namespace System.Xml
{
	// Token: 0x02000045 RID: 69
	public abstract class XmlResolver
	{
		// Token: 0x060001DE RID: 478
		public abstract object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn);

		// Token: 0x060001DF RID: 479 RVA: 0x00008C60 File Offset: 0x00007C60
		public virtual Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			if (baseUri == null || (!baseUri.IsAbsoluteUri && baseUri.OriginalString.Length == 0))
			{
				Uri uri = new Uri(relativeUri, UriKind.RelativeOrAbsolute);
				if (!uri.IsAbsoluteUri && uri.OriginalString.Length > 0)
				{
					uri = new Uri(Path.GetFullPath(relativeUri));
				}
				return uri;
			}
			if (relativeUri == null || relativeUri.Length == 0)
			{
				return baseUri;
			}
			return new Uri(baseUri, relativeUri);
		}

		// Token: 0x17000041 RID: 65
		// (set) Token: 0x060001E0 RID: 480
		public abstract ICredentials Credentials { set; }
	}
}
