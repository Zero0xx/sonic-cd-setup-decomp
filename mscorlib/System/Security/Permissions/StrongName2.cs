using System;
using System.Security.Policy;

namespace System.Security.Permissions
{
	// Token: 0x02000653 RID: 1619
	[Serializable]
	internal sealed class StrongName2
	{
		// Token: 0x06003A5E RID: 14942 RVA: 0x000C472D File Offset: 0x000C372D
		public StrongName2(StrongNamePublicKeyBlob publicKeyBlob, string name, Version version)
		{
			this.m_publicKeyBlob = publicKeyBlob;
			this.m_name = name;
			this.m_version = version;
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x000C474A File Offset: 0x000C374A
		public StrongName2 Copy()
		{
			return new StrongName2(this.m_publicKeyBlob, this.m_name, this.m_version);
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x000C4764 File Offset: 0x000C3764
		public bool IsSubsetOf(StrongName2 target)
		{
			return this.m_publicKeyBlob == null || (this.m_publicKeyBlob.Equals(target.m_publicKeyBlob) && (this.m_name == null || (target.m_name != null && StrongName.CompareNames(target.m_name, this.m_name))) && (this.m_version == null || (target.m_version != null && target.m_version.CompareTo(this.m_version) == 0)));
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x000C47DB File Offset: 0x000C37DB
		public StrongName2 Intersect(StrongName2 target)
		{
			if (target.IsSubsetOf(this))
			{
				return target.Copy();
			}
			if (this.IsSubsetOf(target))
			{
				return this.Copy();
			}
			return null;
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x000C47FE File Offset: 0x000C37FE
		public bool Equals(StrongName2 target)
		{
			return target.IsSubsetOf(this) && this.IsSubsetOf(target);
		}

		// Token: 0x04001E54 RID: 7764
		public StrongNamePublicKeyBlob m_publicKeyBlob;

		// Token: 0x04001E55 RID: 7765
		public string m_name;

		// Token: 0x04001E56 RID: 7766
		public Version m_version;
	}
}
