using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004BA RID: 1210
	[ComVisible(true)]
	[Serializable]
	public sealed class Url : IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06003042 RID: 12354 RVA: 0x000A5C1E File Offset: 0x000A4C1E
		internal Url()
		{
			this.m_url = null;
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x000A5C2D File Offset: 0x000A4C2D
		internal Url(SerializationInfo info, StreamingContext context)
		{
			this.m_url = new URLString((string)info.GetValue("Url", typeof(string)));
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000A5C5A File Offset: 0x000A4C5A
		internal Url(string name, bool parsed)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_url = new URLString(name, parsed);
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x000A5C7D File Offset: 0x000A4C7D
		public Url(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_url = new URLString(name);
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x000A5C9F File Offset: 0x000A4C9F
		public string Value
		{
			get
			{
				if (this.m_url == null)
				{
					return null;
				}
				return this.m_url.ToString();
			}
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x000A5CB6 File Offset: 0x000A4CB6
		internal URLString GetURLString()
		{
			return this.m_url;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x000A5CBE File Offset: 0x000A4CBE
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new UrlIdentityPermission(this.m_url);
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x000A5CCC File Offset: 0x000A4CCC
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (!(o is Url))
			{
				return false;
			}
			Url url = (Url)o;
			if (this.m_url == null)
			{
				return url.m_url == null;
			}
			return url.m_url != null && this.m_url.Equals(url.m_url);
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x000A5D1C File Offset: 0x000A4D1C
		public override int GetHashCode()
		{
			if (this.m_url == null)
			{
				return 0;
			}
			return this.m_url.GetHashCode();
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000A5D34 File Offset: 0x000A4D34
		public object Copy()
		{
			return new Url
			{
				m_url = this.m_url
			};
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000A5D54 File Offset: 0x000A4D54
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Url");
			securityElement.AddAttribute("version", "1");
			if (this.m_url != null)
			{
				securityElement.AddChild(new SecurityElement("Url", this.m_url.ToString()));
			}
			return securityElement;
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000A5DA0 File Offset: 0x000A4DA0
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000A5DB0 File Offset: 0x000A4DB0
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position++] = '\u0004';
			string value = this.Value;
			int length = value.Length;
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(length, buffer, position);
				position += 2;
			}
			value.CopyTo(0, buffer, position, length);
			return length + position;
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000A5DF1 File Offset: 0x000A4DF1
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			if (verbose)
			{
				return this.Value.Length + 3;
			}
			return this.Value.Length + 1;
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000A5E14 File Offset: 0x000A4E14
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			int intFromCharArray = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			this.m_url = new URLString(new string(buffer, position, intFromCharArray));
			return position + intFromCharArray;
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000A5E44 File Offset: 0x000A4E44
		internal object Normalize()
		{
			return this.m_url.NormalizeUrl();
		}

		// Token: 0x04001867 RID: 6247
		private URLString m_url;
	}
}
