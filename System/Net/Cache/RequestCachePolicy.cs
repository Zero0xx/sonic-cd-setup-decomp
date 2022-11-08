using System;

namespace System.Net.Cache
{
	// Token: 0x0200056A RID: 1386
	public class RequestCachePolicy
	{
		// Token: 0x06002A85 RID: 10885 RVA: 0x000B4C8D File Offset: 0x000B3C8D
		public RequestCachePolicy() : this(RequestCacheLevel.Default)
		{
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000B4C96 File Offset: 0x000B3C96
		public RequestCachePolicy(RequestCacheLevel level)
		{
			if (level < RequestCacheLevel.Default || level > RequestCacheLevel.NoCacheNoStore)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			this.m_Level = level;
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002A87 RID: 10887 RVA: 0x000B4CB8 File Offset: 0x000B3CB8
		public RequestCacheLevel Level
		{
			get
			{
				return this.m_Level;
			}
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000B4CC0 File Offset: 0x000B3CC0
		public override string ToString()
		{
			return "Level:" + this.m_Level.ToString();
		}

		// Token: 0x0400290E RID: 10510
		private RequestCacheLevel m_Level;
	}
}
