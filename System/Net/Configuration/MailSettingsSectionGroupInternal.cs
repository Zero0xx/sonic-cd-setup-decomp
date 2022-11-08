using System;

namespace System.Net.Configuration
{
	// Token: 0x02000652 RID: 1618
	internal sealed class MailSettingsSectionGroupInternal
	{
		// Token: 0x06003211 RID: 12817 RVA: 0x000D58BE File Offset: 0x000D48BE
		internal MailSettingsSectionGroupInternal()
		{
			this.smtp = SmtpSectionInternal.GetSection();
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x000D58D1 File Offset: 0x000D48D1
		internal SmtpSectionInternal Smtp
		{
			get
			{
				return this.smtp;
			}
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x000D58D9 File Offset: 0x000D48D9
		internal static MailSettingsSectionGroupInternal GetSection()
		{
			return new MailSettingsSectionGroupInternal();
		}

		// Token: 0x04002EFD RID: 12029
		private SmtpSectionInternal smtp;
	}
}
