using System;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Xml
{
	// Token: 0x0200004B RID: 75
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class XmlSecureResolver : XmlResolver
	{
		// Token: 0x06000207 RID: 519 RVA: 0x0000914D File Offset: 0x0000814D
		public XmlSecureResolver(XmlResolver resolver, string securityUrl) : this(resolver, XmlSecureResolver.CreateEvidenceForUrl(securityUrl))
		{
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000915C File Offset: 0x0000815C
		public XmlSecureResolver(XmlResolver resolver, Evidence evidence) : this(resolver, SecurityManager.ResolvePolicy(evidence))
		{
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000916B File Offset: 0x0000816B
		public XmlSecureResolver(XmlResolver resolver, PermissionSet permissionSet)
		{
			this.resolver = resolver;
			this.permissionSet = permissionSet;
		}

		// Token: 0x17000047 RID: 71
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00009181 File Offset: 0x00008181
		public override ICredentials Credentials
		{
			set
			{
				this.resolver.Credentials = value;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000918F File Offset: 0x0000818F
		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			this.permissionSet.PermitOnly();
			return this.resolver.GetEntity(absoluteUri, role, ofObjectToReturn);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000091AA File Offset: 0x000081AA
		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			return this.resolver.ResolveUri(baseUri, relativeUri);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000091BC File Offset: 0x000081BC
		public static Evidence CreateEvidenceForUrl(string securityUrl)
		{
			Evidence evidence = new Evidence();
			if (securityUrl != null && securityUrl.Length > 0)
			{
				evidence.AddHost(new Url(securityUrl));
				evidence.AddHost(Zone.CreateFromUrl(securityUrl));
				Uri uri = new Uri(securityUrl, UriKind.RelativeOrAbsolute);
				if (uri.IsAbsoluteUri && !uri.IsFile)
				{
					evidence.AddHost(Site.CreateFromUrl(securityUrl));
				}
			}
			return evidence;
		}

		// Token: 0x04000512 RID: 1298
		private XmlResolver resolver;

		// Token: 0x04000513 RID: 1299
		private PermissionSet permissionSet;
	}
}
