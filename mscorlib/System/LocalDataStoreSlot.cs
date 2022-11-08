using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	// Token: 0x020000D0 RID: 208
	[ComVisible(true)]
	public sealed class LocalDataStoreSlot
	{
		// Token: 0x06000B97 RID: 2967 RVA: 0x0002330E File Offset: 0x0002230E
		internal LocalDataStoreSlot(LocalDataStoreMgr mgr, int slot)
		{
			this.m_mgr = mgr;
			this.m_slot = slot;
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00023324 File Offset: 0x00022324
		internal LocalDataStoreMgr Manager
		{
			get
			{
				return this.m_mgr;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x0002332C File Offset: 0x0002232C
		internal int Slot
		{
			get
			{
				return this.m_slot;
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00023334 File Offset: 0x00022334
		internal bool IsValid()
		{
			return LocalDataStoreSlot.m_helper.Get(ref this.m_slot) != -1;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002334C File Offset: 0x0002234C
		protected override void Finalize()
		{
			try
			{
				int slot = this.m_slot;
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(this.m_mgr, ref flag);
					this.m_slot = -1;
					this.m_mgr.FreeDataSlot(slot);
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.m_mgr);
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x04000424 RID: 1060
		private static LdsSyncHelper m_helper = new LdsSyncHelper();

		// Token: 0x04000425 RID: 1061
		private LocalDataStoreMgr m_mgr;

		// Token: 0x04000426 RID: 1062
		private int m_slot;
	}
}
