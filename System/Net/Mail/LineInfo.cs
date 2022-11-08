using System;

namespace System.Net.Mail
{
	// Token: 0x020006C5 RID: 1733
	internal struct LineInfo
	{
		// Token: 0x06003579 RID: 13689 RVA: 0x000E3B0D File Offset: 0x000E2B0D
		internal LineInfo(SmtpStatusCode statusCode, string line)
		{
			this.statusCode = statusCode;
			this.line = line;
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x0600357A RID: 13690 RVA: 0x000E3B1D File Offset: 0x000E2B1D
		internal string Line
		{
			get
			{
				return this.line;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x0600357B RID: 13691 RVA: 0x000E3B25 File Offset: 0x000E2B25
		internal SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
		}

		// Token: 0x040030D8 RID: 12504
		private string line;

		// Token: 0x040030D9 RID: 12505
		private SmtpStatusCode statusCode;
	}
}
