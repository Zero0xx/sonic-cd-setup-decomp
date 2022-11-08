using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003AF RID: 943
	[ComVisible(true)]
	[Serializable]
	public class DaylightTime
	{
		// Token: 0x060025AB RID: 9643 RVA: 0x0006902F File Offset: 0x0006802F
		private DaylightTime()
		{
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x00069037 File Offset: 0x00068037
		public DaylightTime(DateTime start, DateTime end, TimeSpan delta)
		{
			this.m_start = start;
			this.m_end = end;
			this.m_delta = delta;
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x00069054 File Offset: 0x00068054
		public DateTime Start
		{
			get
			{
				return this.m_start;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x0006905C File Offset: 0x0006805C
		public DateTime End
		{
			get
			{
				return this.m_end;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x00069064 File Offset: 0x00068064
		public TimeSpan Delta
		{
			get
			{
				return this.m_delta;
			}
		}

		// Token: 0x0400110B RID: 4363
		internal DateTime m_start;

		// Token: 0x0400110C RID: 4364
		internal DateTime m_end;

		// Token: 0x0400110D RID: 4365
		internal TimeSpan m_delta;
	}
}
