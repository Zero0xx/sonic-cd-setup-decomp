using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004B5 RID: 1205
	[ComVisible(true)]
	[Serializable]
	public sealed class Site : IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06002FEF RID: 12271 RVA: 0x000A4773 File Offset: 0x000A3773
		internal Site()
		{
			this.m_name = null;
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000A4782 File Offset: 0x000A3782
		public Site(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_name = new SiteString(name);
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000A47A4 File Offset: 0x000A37A4
		internal Site(byte[] id, string name)
		{
			this.m_name = Site.ParseSiteFromUrl(name);
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000A47B8 File Offset: 0x000A37B8
		public static Site CreateFromUrl(string url)
		{
			return new Site
			{
				m_name = Site.ParseSiteFromUrl(url)
			};
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000A47D8 File Offset: 0x000A37D8
		private static SiteString ParseSiteFromUrl(string name)
		{
			URLString urlstring = new URLString(name);
			if (string.Compare(urlstring.Scheme, "file", StringComparison.OrdinalIgnoreCase) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSite"));
			}
			return new SiteString(new URLString(name).Host);
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x000A481F File Offset: 0x000A381F
		public string Name
		{
			get
			{
				if (this.m_name != null)
				{
					return this.m_name.ToString();
				}
				return null;
			}
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000A4836 File Offset: 0x000A3836
		internal SiteString GetSiteString()
		{
			return this.m_name;
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000A483E File Offset: 0x000A383E
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new SiteIdentityPermission(this.Name);
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000A484C File Offset: 0x000A384C
		public override bool Equals(object o)
		{
			if (!(o is Site))
			{
				return false;
			}
			Site site = (Site)o;
			if (this.Name == null)
			{
				return site.Name == null;
			}
			return string.Compare(this.Name, site.Name, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000A4894 File Offset: 0x000A3894
		public override int GetHashCode()
		{
			string name = this.Name;
			if (name == null)
			{
				return 0;
			}
			return name.GetHashCode();
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000A48B3 File Offset: 0x000A38B3
		public object Copy()
		{
			return new Site(this.Name);
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000A48C0 File Offset: 0x000A38C0
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Site");
			securityElement.AddAttribute("version", "1");
			if (this.m_name != null)
			{
				securityElement.AddChild(new SecurityElement("Name", this.m_name.ToString()));
			}
			return securityElement;
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000A490C File Offset: 0x000A390C
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position++] = '\u0006';
			string name = this.Name;
			int length = name.Length;
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(length, buffer, position);
				position += 2;
			}
			name.CopyTo(0, buffer, position, length);
			return length + position;
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000A494D File Offset: 0x000A394D
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			if (verbose)
			{
				return this.Name.Length + 3;
			}
			return this.Name.Length + 1;
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000A4970 File Offset: 0x000A3970
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			int intFromCharArray = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			this.m_name = new SiteString(new string(buffer, position, intFromCharArray));
			return position + intFromCharArray;
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000A49A0 File Offset: 0x000A39A0
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000A49AD File Offset: 0x000A39AD
		internal object Normalize()
		{
			return this.m_name.ToString().ToUpper(CultureInfo.InvariantCulture);
		}

		// Token: 0x04001858 RID: 6232
		private SiteString m_name;
	}
}
