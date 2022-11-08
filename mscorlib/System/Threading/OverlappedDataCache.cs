using System;
using System.Runtime.ConstrainedExecution;

namespace System.Threading
{
	// Token: 0x02000165 RID: 357
	internal sealed class OverlappedDataCache : CriticalFinalizerObject
	{
		// Token: 0x060012DA RID: 4826 RVA: 0x00034358 File Offset: 0x00033358
		private static void GrowOverlappedDataCache()
		{
			OverlappedDataCacheLine value = new OverlappedDataCacheLine();
			if (OverlappedDataCache.m_overlappedDataCache == null && Interlocked.CompareExchange<OverlappedDataCacheLine>(ref OverlappedDataCache.m_overlappedDataCache, value, null) == null)
			{
				new OverlappedDataCache();
				return;
			}
			if (OverlappedDataCache.m_cleanupObjectCount == 0)
			{
				new OverlappedDataCache();
			}
			for (;;)
			{
				OverlappedDataCacheLine overlappedDataCacheLine = OverlappedDataCache.m_overlappedDataCache;
				while (overlappedDataCacheLine != null && overlappedDataCacheLine.m_next != null)
				{
					overlappedDataCacheLine = overlappedDataCacheLine.m_next;
				}
				if (overlappedDataCacheLine == null)
				{
					break;
				}
				if (Interlocked.CompareExchange<OverlappedDataCacheLine>(ref overlappedDataCacheLine.m_next, value, null) == null)
				{
					return;
				}
			}
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x000343C4 File Offset: 0x000333C4
		internal static OverlappedData GetOverlappedData(Overlapped overlapped)
		{
			Interlocked.Exchange(ref OverlappedDataCache.m_overlappedDataCacheAccessed, 1);
			OverlappedDataCacheLine overlappedDataCacheLine;
			OverlappedData overlappedData;
			for (;;)
			{
				overlappedDataCacheLine = OverlappedDataCache.s_firstFreeCacheLine;
				if (overlappedDataCacheLine == null)
				{
					overlappedDataCacheLine = OverlappedDataCache.m_overlappedDataCache;
				}
				while (overlappedDataCacheLine != null)
				{
					for (short num = 0; num < 16; num += 1)
					{
						if (overlappedDataCacheLine.m_items[(int)num] != null)
						{
							overlappedData = Interlocked.Exchange<OverlappedData>(ref overlappedDataCacheLine.m_items[(int)num], null);
							if (overlappedData != null)
							{
								goto Block_3;
							}
						}
					}
					overlappedDataCacheLine = overlappedDataCacheLine.m_next;
				}
				OverlappedDataCache.GrowOverlappedDataCache();
			}
			Block_3:
			OverlappedDataCache.s_firstFreeCacheLine = overlappedDataCacheLine;
			overlappedData.m_overlapped = overlapped;
			return overlappedData;
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00034441 File Offset: 0x00033441
		internal static void CacheOverlappedData(OverlappedData data)
		{
			data.ReInitialize();
			data.m_cacheLine.m_items[(int)data.m_slot] = data;
			OverlappedDataCache.s_firstFreeCacheLine = null;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00034464 File Offset: 0x00033464
		internal OverlappedDataCache()
		{
			if (OverlappedDataCache.m_cleanupObjectCount == 0)
			{
				OverlappedDataCache.m_CleanupThreshold = 0.3f;
				if (Interlocked.Exchange(ref OverlappedDataCache.m_cleanupObjectCount, 1) == 0)
				{
					this.m_ready = true;
				}
			}
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00034494 File Offset: 0x00033494
		protected override void Finalize()
		{
			try
			{
				if (this.m_ready)
				{
					if (OverlappedDataCache.m_overlappedDataCache == null)
					{
						Interlocked.Exchange(ref OverlappedDataCache.m_cleanupObjectCount, 0);
					}
					else
					{
						if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
						{
							GC.ReRegisterForFinalize(this);
						}
						int num = GC.CollectionCount(GC.MaxGeneration);
						if (num != this.m_gen2GCCount)
						{
							this.m_gen2GCCount = num;
							OverlappedDataCacheLine overlappedDataCacheLine = null;
							OverlappedDataCacheLine overlappedDataCacheLine2 = OverlappedDataCache.m_overlappedDataCache;
							OverlappedDataCacheLine overlappedDataCacheLine3 = null;
							OverlappedDataCacheLine overlappedDataCacheLine4 = overlappedDataCacheLine;
							int num2 = 0;
							int num3 = 0;
							while (overlappedDataCacheLine2 != null)
							{
								num2++;
								bool flag = false;
								for (short num4 = 0; num4 < 16; num4 += 1)
								{
									if (overlappedDataCacheLine2.m_items[(int)num4] == null)
									{
										flag = true;
										num3++;
									}
								}
								if (!flag)
								{
									overlappedDataCacheLine4 = overlappedDataCacheLine;
									overlappedDataCacheLine3 = overlappedDataCacheLine2;
								}
								overlappedDataCacheLine = overlappedDataCacheLine2;
								overlappedDataCacheLine2 = overlappedDataCacheLine2.m_next;
							}
							num2 *= 16;
							if (overlappedDataCacheLine3 != null && (float)num2 * OverlappedDataCache.m_CleanupThreshold > (float)num3)
							{
								if (overlappedDataCacheLine4 == null)
								{
									OverlappedDataCache.m_overlappedDataCache = overlappedDataCacheLine3.m_next;
								}
								else
								{
									overlappedDataCacheLine4.m_next = overlappedDataCacheLine3.m_next;
								}
								overlappedDataCacheLine3.Removed = true;
							}
							if (OverlappedDataCache.m_overlappedDataCacheAccessed != 0)
							{
								OverlappedDataCache.m_CleanupThreshold = 0.3f;
								Interlocked.Exchange(ref OverlappedDataCache.m_overlappedDataCacheAccessed, 0);
							}
							else
							{
								OverlappedDataCache.m_CleanupThreshold += 0.05f;
							}
						}
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x04000685 RID: 1669
		private const float m_CleanupStep = 0.05f;

		// Token: 0x04000686 RID: 1670
		private const float m_CleanupInitialThreadhold = 0.3f;

		// Token: 0x04000687 RID: 1671
		private static OverlappedDataCacheLine m_overlappedDataCache;

		// Token: 0x04000688 RID: 1672
		private static int m_overlappedDataCacheAccessed;

		// Token: 0x04000689 RID: 1673
		private static int m_cleanupObjectCount;

		// Token: 0x0400068A RID: 1674
		private static float m_CleanupThreshold;

		// Token: 0x0400068B RID: 1675
		private static volatile OverlappedDataCacheLine s_firstFreeCacheLine;

		// Token: 0x0400068C RID: 1676
		private int m_gen2GCCount;

		// Token: 0x0400068D RID: 1677
		private bool m_ready;
	}
}
