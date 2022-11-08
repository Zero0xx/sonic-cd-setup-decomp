using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using Microsoft.Win32;

namespace System.Net.Cache
{
	// Token: 0x02000566 RID: 1382
	internal class RequestCacheEntry
	{
		// Token: 0x06002A60 RID: 10848 RVA: 0x000B4528 File Offset: 0x000B3528
		internal RequestCacheEntry()
		{
			this.m_ExpiresUtc = (this.m_LastAccessedUtc = (this.m_LastModifiedUtc = (this.m_LastSynchronizedUtc = DateTime.MinValue)));
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000B4564 File Offset: 0x000B3564
		internal RequestCacheEntry(_WinInetCache.Entry entry, bool isPrivateEntry)
		{
			this.m_IsPrivateEntry = isPrivateEntry;
			this.m_StreamSize = ((long)entry.Info.SizeHigh << 32 | (long)entry.Info.SizeLow);
			this.m_ExpiresUtc = (entry.Info.ExpireTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.ExpireTime.ToLong()));
			this.m_HitCount = entry.Info.HitRate;
			this.m_LastAccessedUtc = (entry.Info.LastAccessTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastAccessTime.ToLong()));
			this.m_LastModifiedUtc = (entry.Info.LastModifiedTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastModifiedTime.ToLong()));
			this.m_LastSynchronizedUtc = (entry.Info.LastSyncTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastSyncTime.ToLong()));
			this.m_MaxStale = TimeSpan.FromSeconds((double)entry.Info.U.ExemptDelta);
			if (this.m_MaxStale == WinInetCache.s_MaxTimeSpanForInt32)
			{
				this.m_MaxStale = TimeSpan.MaxValue;
			}
			this.m_UsageCount = entry.Info.UseCount;
			this.m_IsPartialEntry = ((entry.Info.EntryType & _WinInetCache.EntryType.Sparse) != (_WinInetCache.EntryType)0);
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x000B46E8 File Offset: 0x000B36E8
		// (set) Token: 0x06002A63 RID: 10851 RVA: 0x000B46F0 File Offset: 0x000B36F0
		internal bool IsPrivateEntry
		{
			get
			{
				return this.m_IsPrivateEntry;
			}
			set
			{
				this.m_IsPrivateEntry = value;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x000B46F9 File Offset: 0x000B36F9
		// (set) Token: 0x06002A65 RID: 10853 RVA: 0x000B4701 File Offset: 0x000B3701
		internal long StreamSize
		{
			get
			{
				return this.m_StreamSize;
			}
			set
			{
				this.m_StreamSize = value;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x000B470A File Offset: 0x000B370A
		// (set) Token: 0x06002A67 RID: 10855 RVA: 0x000B4712 File Offset: 0x000B3712
		internal DateTime ExpiresUtc
		{
			get
			{
				return this.m_ExpiresUtc;
			}
			set
			{
				this.m_ExpiresUtc = value;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x000B471B File Offset: 0x000B371B
		// (set) Token: 0x06002A69 RID: 10857 RVA: 0x000B4723 File Offset: 0x000B3723
		internal DateTime LastAccessedUtc
		{
			get
			{
				return this.m_LastAccessedUtc;
			}
			set
			{
				this.m_LastAccessedUtc = value;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000B472C File Offset: 0x000B372C
		// (set) Token: 0x06002A6B RID: 10859 RVA: 0x000B4734 File Offset: 0x000B3734
		internal DateTime LastModifiedUtc
		{
			get
			{
				return this.m_LastModifiedUtc;
			}
			set
			{
				this.m_LastModifiedUtc = value;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x000B473D File Offset: 0x000B373D
		// (set) Token: 0x06002A6D RID: 10861 RVA: 0x000B4745 File Offset: 0x000B3745
		internal DateTime LastSynchronizedUtc
		{
			get
			{
				return this.m_LastSynchronizedUtc;
			}
			set
			{
				this.m_LastSynchronizedUtc = value;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002A6E RID: 10862 RVA: 0x000B474E File Offset: 0x000B374E
		// (set) Token: 0x06002A6F RID: 10863 RVA: 0x000B4756 File Offset: 0x000B3756
		internal TimeSpan MaxStale
		{
			get
			{
				return this.m_MaxStale;
			}
			set
			{
				this.m_MaxStale = value;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x000B475F File Offset: 0x000B375F
		// (set) Token: 0x06002A71 RID: 10865 RVA: 0x000B4767 File Offset: 0x000B3767
		internal int HitCount
		{
			get
			{
				return this.m_HitCount;
			}
			set
			{
				this.m_HitCount = value;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x000B4770 File Offset: 0x000B3770
		// (set) Token: 0x06002A73 RID: 10867 RVA: 0x000B4778 File Offset: 0x000B3778
		internal int UsageCount
		{
			get
			{
				return this.m_UsageCount;
			}
			set
			{
				this.m_UsageCount = value;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x000B4781 File Offset: 0x000B3781
		// (set) Token: 0x06002A75 RID: 10869 RVA: 0x000B4789 File Offset: 0x000B3789
		internal bool IsPartialEntry
		{
			get
			{
				return this.m_IsPartialEntry;
			}
			set
			{
				this.m_IsPartialEntry = value;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002A76 RID: 10870 RVA: 0x000B4792 File Offset: 0x000B3792
		// (set) Token: 0x06002A77 RID: 10871 RVA: 0x000B479A File Offset: 0x000B379A
		internal StringCollection EntryMetadata
		{
			get
			{
				return this.m_EntryMetadata;
			}
			set
			{
				this.m_EntryMetadata = value;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002A78 RID: 10872 RVA: 0x000B47A3 File Offset: 0x000B37A3
		// (set) Token: 0x06002A79 RID: 10873 RVA: 0x000B47AB File Offset: 0x000B37AB
		internal StringCollection SystemMetadata
		{
			get
			{
				return this.m_SystemMetadata;
			}
			set
			{
				this.m_SystemMetadata = value;
			}
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000B47B4 File Offset: 0x000B37B4
		internal virtual string ToString(bool verbose)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append("\r\nIsPrivateEntry   = ").Append(this.IsPrivateEntry);
			stringBuilder.Append("\r\nIsPartialEntry   = ").Append(this.IsPartialEntry);
			stringBuilder.Append("\r\nStreamSize       = ").Append(this.StreamSize);
			stringBuilder.Append("\r\nExpires          = ").Append((this.ExpiresUtc == DateTime.MinValue) ? "" : this.ExpiresUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastAccessed     = ").Append((this.LastAccessedUtc == DateTime.MinValue) ? "" : this.LastAccessedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastModified     = ").Append((this.LastModifiedUtc == DateTime.MinValue) ? "" : this.LastModifiedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastSynchronized = ").Append((this.LastSynchronizedUtc == DateTime.MinValue) ? "" : this.LastSynchronizedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nMaxStale(sec)    = ").Append((this.MaxStale == TimeSpan.MinValue) ? "" : ((int)this.MaxStale.TotalSeconds).ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\nHitCount         = ").Append(this.HitCount.ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\nUsageCount       = ").Append(this.UsageCount.ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\n");
			if (verbose)
			{
				stringBuilder.Append("EntryMetadata:\r\n");
				if (this.m_EntryMetadata != null)
				{
					foreach (string value in this.m_EntryMetadata)
					{
						stringBuilder.Append(value).Append("\r\n");
					}
				}
				stringBuilder.Append("---\r\nSystemMetadata:\r\n");
				if (this.m_SystemMetadata != null)
				{
					foreach (string value2 in this.m_SystemMetadata)
					{
						stringBuilder.Append(value2).Append("\r\n");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040028F2 RID: 10482
		private bool m_IsPrivateEntry;

		// Token: 0x040028F3 RID: 10483
		private long m_StreamSize;

		// Token: 0x040028F4 RID: 10484
		private DateTime m_ExpiresUtc;

		// Token: 0x040028F5 RID: 10485
		private int m_HitCount;

		// Token: 0x040028F6 RID: 10486
		private DateTime m_LastAccessedUtc;

		// Token: 0x040028F7 RID: 10487
		private DateTime m_LastModifiedUtc;

		// Token: 0x040028F8 RID: 10488
		private DateTime m_LastSynchronizedUtc;

		// Token: 0x040028F9 RID: 10489
		private TimeSpan m_MaxStale;

		// Token: 0x040028FA RID: 10490
		private int m_UsageCount;

		// Token: 0x040028FB RID: 10491
		private bool m_IsPartialEntry;

		// Token: 0x040028FC RID: 10492
		private StringCollection m_EntryMetadata;

		// Token: 0x040028FD RID: 10493
		private StringCollection m_SystemMetadata;
	}
}
