using System;

namespace System.Threading
{
	// Token: 0x02000164 RID: 356
	internal sealed class OverlappedDataCacheLine
	{
		// Token: 0x060012D6 RID: 4822 RVA: 0x000342BC File Offset: 0x000332BC
		internal OverlappedDataCacheLine()
		{
			this.m_items = new OverlappedData[16];
			new object();
			for (short num = 0; num < 16; num += 1)
			{
				this.m_items[(int)num] = new OverlappedData(this);
				this.m_items[(int)num].m_slot = num;
			}
			new object();
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00034314 File Offset: 0x00033314
		~OverlappedDataCacheLine()
		{
			this.m_removed = true;
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x00034344 File Offset: 0x00033344
		// (set) Token: 0x060012D9 RID: 4825 RVA: 0x0003434C File Offset: 0x0003334C
		internal bool Removed
		{
			get
			{
				return this.m_removed;
			}
			set
			{
				this.m_removed = value;
			}
		}

		// Token: 0x04000681 RID: 1665
		internal const short CacheSize = 16;

		// Token: 0x04000682 RID: 1666
		internal OverlappedData[] m_items;

		// Token: 0x04000683 RID: 1667
		internal OverlappedDataCacheLine m_next;

		// Token: 0x04000684 RID: 1668
		private bool m_removed;
	}
}
