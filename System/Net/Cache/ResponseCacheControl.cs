using System;
using System.Text;

namespace System.Net.Cache
{
	// Token: 0x02000560 RID: 1376
	internal class ResponseCacheControl
	{
		// Token: 0x06002A1B RID: 10779 RVA: 0x000B12E4 File Offset: 0x000B02E4
		internal ResponseCacheControl()
		{
			this.MaxAge = (this.SMaxAge = -1);
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002A1C RID: 10780 RVA: 0x000B1308 File Offset: 0x000B0308
		internal bool IsNotEmpty
		{
			get
			{
				return this.Public || this.Private || this.NoCache || this.NoStore || this.MustRevalidate || this.ProxyRevalidate || this.MaxAge != -1 || this.SMaxAge != -1;
			}
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000B135C File Offset: 0x000B035C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Public)
			{
				stringBuilder.Append(" public");
			}
			if (this.Private)
			{
				stringBuilder.Append(" private");
				if (this.PrivateHeaders != null)
				{
					stringBuilder.Append('=');
					for (int i = 0; i < this.PrivateHeaders.Length - 1; i++)
					{
						stringBuilder.Append(this.PrivateHeaders[i]).Append(',');
					}
					stringBuilder.Append(this.PrivateHeaders[this.PrivateHeaders.Length - 1]);
				}
			}
			if (this.NoCache)
			{
				stringBuilder.Append(" no-cache");
				if (this.NoCacheHeaders != null)
				{
					stringBuilder.Append('=');
					for (int j = 0; j < this.NoCacheHeaders.Length - 1; j++)
					{
						stringBuilder.Append(this.NoCacheHeaders[j]).Append(',');
					}
					stringBuilder.Append(this.NoCacheHeaders[this.NoCacheHeaders.Length - 1]);
				}
			}
			if (this.NoStore)
			{
				stringBuilder.Append(" no-store");
			}
			if (this.MustRevalidate)
			{
				stringBuilder.Append(" must-revalidate");
			}
			if (this.ProxyRevalidate)
			{
				stringBuilder.Append(" proxy-revalidate");
			}
			if (this.MaxAge != -1)
			{
				stringBuilder.Append(" max-age=").Append(this.MaxAge);
			}
			if (this.SMaxAge != -1)
			{
				stringBuilder.Append(" s-maxage=").Append(this.SMaxAge);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040028CC RID: 10444
		internal bool Public;

		// Token: 0x040028CD RID: 10445
		internal bool Private;

		// Token: 0x040028CE RID: 10446
		internal string[] PrivateHeaders;

		// Token: 0x040028CF RID: 10447
		internal bool NoCache;

		// Token: 0x040028D0 RID: 10448
		internal string[] NoCacheHeaders;

		// Token: 0x040028D1 RID: 10449
		internal bool NoStore;

		// Token: 0x040028D2 RID: 10450
		internal bool MustRevalidate;

		// Token: 0x040028D3 RID: 10451
		internal bool ProxyRevalidate;

		// Token: 0x040028D4 RID: 10452
		internal int MaxAge;

		// Token: 0x040028D5 RID: 10453
		internal int SMaxAge;
	}
}
