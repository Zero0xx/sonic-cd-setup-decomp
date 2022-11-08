using System;

namespace System.Net
{
	// Token: 0x02000392 RID: 914
	internal struct HeaderVariantInfo
	{
		// Token: 0x06001C88 RID: 7304 RVA: 0x0006C28D File Offset: 0x0006B28D
		internal HeaderVariantInfo(string name, CookieVariant variant)
		{
			this.m_name = name;
			this.m_variant = variant;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x0006C29D File Offset: 0x0006B29D
		internal string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x0006C2A5 File Offset: 0x0006B2A5
		internal CookieVariant Variant
		{
			get
			{
				return this.m_variant;
			}
		}

		// Token: 0x04001D27 RID: 7463
		private string m_name;

		// Token: 0x04001D28 RID: 7464
		private CookieVariant m_variant;
	}
}
