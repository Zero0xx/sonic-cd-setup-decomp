using System;
using System.Text;

namespace System.Net
{
	// Token: 0x020004BC RID: 1212
	internal class ResponseDescription
	{
		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x00095FA5 File Offset: 0x00094FA5
		internal bool PositiveIntermediate
		{
			get
			{
				return this.Status >= 100 && this.Status <= 199;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x00095FC3 File Offset: 0x00094FC3
		internal bool PositiveCompletion
		{
			get
			{
				return this.Status >= 200 && this.Status <= 299;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x00095FE4 File Offset: 0x00094FE4
		internal bool TransientFailure
		{
			get
			{
				return this.Status >= 400 && this.Status <= 499;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x00096005 File Offset: 0x00095005
		internal bool PermanentFailure
		{
			get
			{
				return this.Status >= 500 && this.Status <= 599;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x00096026 File Offset: 0x00095026
		internal bool InvalidStatusCode
		{
			get
			{
				return this.Status < 100 || this.Status > 599;
			}
		}

		// Token: 0x04002541 RID: 9537
		internal const int NoStatus = -1;

		// Token: 0x04002542 RID: 9538
		internal bool Multiline;

		// Token: 0x04002543 RID: 9539
		internal int Status = -1;

		// Token: 0x04002544 RID: 9540
		internal string StatusDescription;

		// Token: 0x04002545 RID: 9541
		internal StringBuilder StatusBuffer = new StringBuilder();

		// Token: 0x04002546 RID: 9542
		internal string StatusCodeString;
	}
}
