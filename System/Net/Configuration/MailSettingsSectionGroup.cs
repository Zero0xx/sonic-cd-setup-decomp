using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000651 RID: 1617
	public sealed class MailSettingsSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06003210 RID: 12816 RVA: 0x000D58A7 File Offset: 0x000D48A7
		public SmtpSection Smtp
		{
			get
			{
				return (SmtpSection)base.Sections["smtp"];
			}
		}
	}
}
