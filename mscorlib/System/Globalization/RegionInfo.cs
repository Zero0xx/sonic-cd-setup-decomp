using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization
{
	// Token: 0x020003C4 RID: 964
	[ComVisible(true)]
	[Serializable]
	public class RegionInfo
	{
		// Token: 0x0600270F RID: 9999 RVA: 0x00074CFC File Offset: 0x00073CFC
		public RegionInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidRegionName", new object[]
				{
					name
				}), "name");
			}
			this.m_name = name.ToUpper(CultureInfo.InvariantCulture);
			this.m_cultureId = 0;
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecordForRegion(name, true);
			if (this.m_cultureTableRecord.IsNeutralCulture)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNeutralRegionName", new object[]
				{
					name
				}), "name");
			}
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x00074D9C File Offset: 0x00073D9C
		public RegionInfo(int culture)
		{
			if (culture == 127)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NoRegionInvariantCulture"));
			}
			if (CultureTableRecord.IsCustomCultureId(culture))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CustomCultureCannotBePassedByNumber", new object[]
				{
					"culture"
				}));
			}
			if (CultureInfo.GetSubLangID(culture) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CultureIsNeutral", new object[]
				{
					culture
				}), "culture");
			}
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(culture, true);
			if (this.m_cultureTableRecord.IsNeutralCulture)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CultureIsNeutral", new object[]
				{
					culture
				}), "culture");
			}
			this.m_name = this.m_cultureTableRecord.SREGIONNAME;
			this.m_cultureId = culture;
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x00074E72 File Offset: 0x00073E72
		internal RegionInfo(CultureTableRecord table)
		{
			this.m_cultureTableRecord = table;
			this.m_name = this.m_cultureTableRecord.SREGIONNAME;
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x00074E94 File Offset: 0x00073E94
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_name == null)
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(CultureTableRecord.IdFromEverettRegionInfoDataItem(this.m_dataItem), true);
				this.m_name = this.m_cultureTableRecord.SREGIONNAME;
				return;
			}
			if (this.m_cultureId != 0)
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.m_cultureId, true);
				return;
			}
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecordForRegion(this.m_name, true);
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x00074EFF File Offset: 0x00073EFF
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.m_dataItem = this.m_cultureTableRecord.EverettRegionDataItem();
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002714 RID: 10004 RVA: 0x00074F14 File Offset: 0x00073F14
		public static RegionInfo CurrentRegion
		{
			get
			{
				RegionInfo regionInfo = RegionInfo.m_currentRegionInfo;
				if (regionInfo == null)
				{
					regionInfo = new RegionInfo(CultureInfo.CurrentCulture.m_cultureTableRecord);
					if (regionInfo.m_cultureTableRecord.IsCustomCulture)
					{
						regionInfo.m_name = regionInfo.m_cultureTableRecord.SNAME;
					}
					RegionInfo.m_currentRegionInfo = regionInfo;
				}
				return regionInfo;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x00074F5F File Offset: 0x00073F5F
		public virtual string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.m_cultureTableRecord.SREGIONNAME;
				}
				return this.m_name;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002716 RID: 10006 RVA: 0x00074F80 File Offset: 0x00073F80
		public virtual string EnglishName
		{
			get
			{
				return this.m_cultureTableRecord.SENGCOUNTRY;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002717 RID: 10007 RVA: 0x00074F90 File Offset: 0x00073F90
		public virtual string DisplayName
		{
			get
			{
				if (this.m_cultureTableRecord.IsCustomCulture)
				{
					if (!this.m_cultureTableRecord.IsReplacementCulture)
					{
						return this.m_cultureTableRecord.SNATIVECOUNTRY;
					}
					if (this.m_cultureTableRecord.IsSynthetic)
					{
						return this.m_cultureTableRecord.RegionNativeDisplayName;
					}
					return Environment.GetResourceString("Globalization.ri_" + this.m_cultureTableRecord.SREGIONNAME);
				}
				else
				{
					if (this.m_cultureTableRecord.IsSynthetic)
					{
						return this.m_cultureTableRecord.RegionNativeDisplayName;
					}
					return Environment.GetResourceString("Globalization.ri_" + this.m_cultureTableRecord.SREGIONNAME);
				}
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002718 RID: 10008 RVA: 0x0007502A File Offset: 0x0007402A
		[ComVisible(false)]
		public virtual string NativeName
		{
			get
			{
				return this.m_cultureTableRecord.SNATIVECOUNTRY;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002719 RID: 10009 RVA: 0x00075037 File Offset: 0x00074037
		public virtual string TwoLetterISORegionName
		{
			get
			{
				return this.m_cultureTableRecord.SISO3166CTRYNAME;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x00075044 File Offset: 0x00074044
		public virtual string ThreeLetterISORegionName
		{
			get
			{
				return this.m_cultureTableRecord.SISO3166CTRYNAME2;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600271B RID: 10011 RVA: 0x00075054 File Offset: 0x00074054
		public virtual bool IsMetric
		{
			get
			{
				int imeasure = (int)this.m_cultureTableRecord.IMEASURE;
				return imeasure == 0;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x00075071 File Offset: 0x00074071
		[ComVisible(false)]
		public virtual int GeoId
		{
			get
			{
				return (int)this.m_cultureTableRecord.IGEOID;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x0600271D RID: 10013 RVA: 0x0007507E File Offset: 0x0007407E
		public virtual string ThreeLetterWindowsRegionName
		{
			get
			{
				return this.m_cultureTableRecord.SABBREVCTRYNAME;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x0600271E RID: 10014 RVA: 0x0007508B File Offset: 0x0007408B
		[ComVisible(false)]
		public virtual string CurrencyEnglishName
		{
			get
			{
				return this.m_cultureTableRecord.SENGLISHCURRENCY;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x0600271F RID: 10015 RVA: 0x00075098 File Offset: 0x00074098
		[ComVisible(false)]
		public virtual string CurrencyNativeName
		{
			get
			{
				return this.m_cultureTableRecord.SNATIVECURRENCY;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002720 RID: 10016 RVA: 0x000750A5 File Offset: 0x000740A5
		public virtual string CurrencySymbol
		{
			get
			{
				return this.m_cultureTableRecord.SCURRENCY;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002721 RID: 10017 RVA: 0x000750B2 File Offset: 0x000740B2
		public virtual string ISOCurrencySymbol
		{
			get
			{
				return this.m_cultureTableRecord.SINTLSYMBOL;
			}
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000750C0 File Offset: 0x000740C0
		public override bool Equals(object value)
		{
			RegionInfo regionInfo = value as RegionInfo;
			return regionInfo != null && this.Name.Equals(regionInfo.Name);
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000750EA File Offset: 0x000740EA
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000750F7 File Offset: 0x000740F7
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x040011D3 RID: 4563
		internal string m_name;

		// Token: 0x040011D4 RID: 4564
		[OptionalField(VersionAdded = 2)]
		private int m_cultureId;

		// Token: 0x040011D5 RID: 4565
		[NonSerialized]
		internal CultureTableRecord m_cultureTableRecord;

		// Token: 0x040011D6 RID: 4566
		internal static RegionInfo m_currentRegionInfo;

		// Token: 0x040011D7 RID: 4567
		internal int m_dataItem;
	}
}
