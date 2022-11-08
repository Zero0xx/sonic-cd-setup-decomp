using System;
using System.Collections;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Globalization
{
	// Token: 0x020003D3 RID: 979
	internal class CultureTable : BaseInfoTable
	{
		// Token: 0x06002838 RID: 10296 RVA: 0x00079750 File Offset: 0x00078750
		internal unsafe CultureTable(string fileName, bool fromAssembly) : base(fileName, fromAssembly)
		{
			if (!base.IsValid)
			{
				return;
			}
			this.hashByName = Hashtable.Synchronized(new Hashtable());
			this.hashByLcid = Hashtable.Synchronized(new Hashtable());
			this.hashByRegionName = Hashtable.Synchronized(new Hashtable());
			this.m_pCultureNameIndex = (CultureNameOffsetItem*)(this.m_pDataFileStart + this.m_pCultureHeader->cultureNameTableOffset);
			this.m_pRegionNameIndex = (RegionNameOffsetItem*)(this.m_pDataFileStart + this.m_pCultureHeader->regionNameTableOffset);
			this.m_pCultureIDIndex = (IDOffsetItem*)(this.m_pDataFileStart + this.m_pCultureHeader->cultureIDTableOffset);
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x000797E6 File Offset: 0x000787E6
		internal static CultureTable Default
		{
			get
			{
				if (CultureTable.m_defaultInstance == null)
				{
					throw new TypeLoadException("Failure has occurred while loading a type.");
				}
				return CultureTable.m_defaultInstance;
			}
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x00079800 File Offset: 0x00078800
		internal unsafe override void SetDataItemPointers()
		{
			if (this.Validate())
			{
				this.m_itemSize = (uint)this.m_pCultureHeader->sizeCultureItem;
				this.m_numItem = (uint)this.m_pCultureHeader->numCultureItems;
				this.m_pDataPool = (ushort*)(this.m_pDataFileStart + this.m_pCultureHeader->offsetToDataPool);
				this.m_pItemData = this.m_pDataFileStart + this.m_pCultureHeader->offsetToCultureItemData;
				return;
			}
			this.m_valid = false;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x00079870 File Offset: 0x00078870
		private unsafe static string CheckAndGetTheString(ushort* pDataPool, uint offsetInPool, int poolSize)
		{
			if ((ulong)(offsetInPool + 2U) > (ulong)((long)poolSize))
			{
				return null;
			}
			char* ptr = (char*)(pDataPool + offsetInPool);
			int num = (int)(*ptr);
			if ((ulong)offsetInPool + (ulong)((long)num) + 2UL > (ulong)((long)poolSize))
			{
				return null;
			}
			return new string(ptr + 1, 0, num);
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000798AC File Offset: 0x000788AC
		private unsafe static bool ValidateString(ushort* pDataPool, uint offsetInPool, int poolSize)
		{
			if ((ulong)(offsetInPool + 2U) > (ulong)((long)poolSize))
			{
				return false;
			}
			char* ptr = (char*)(pDataPool + offsetInPool);
			int num = (int)(*ptr);
			return (ulong)offsetInPool + (ulong)((long)num) + 2UL <= (ulong)((long)poolSize);
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000798DC File Offset: 0x000788DC
		private unsafe static bool ValidateUintArray(ushort* pDataPool, uint offsetInPool, int poolSize)
		{
			if (offsetInPool == 0U)
			{
				return true;
			}
			if ((ulong)(offsetInPool + 2U) > (ulong)((long)poolSize))
			{
				return false;
			}
			ushort* ptr = pDataPool + offsetInPool;
			if ((ptr & 2) != 2)
			{
				return false;
			}
			int num = (int)(*ptr);
			return (ulong)offsetInPool + (ulong)((long)(num * 2)) + 2UL <= (ulong)((long)poolSize);
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x0007991C File Offset: 0x0007891C
		private unsafe static bool ValidateStringArray(ushort* pDataPool, uint offsetInPool, int poolSize)
		{
			if (!CultureTable.ValidateUintArray(pDataPool, offsetInPool, poolSize))
			{
				return false;
			}
			ushort* ptr = pDataPool + offsetInPool;
			int num = (int)(*ptr);
			if (num == 0)
			{
				return true;
			}
			uint* ptr2 = (uint*)(ptr + 1);
			for (int i = 0; i < num; i++)
			{
				if (!CultureTable.ValidateString(pDataPool, ptr2[i], poolSize))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x00079968 File Offset: 0x00078968
		private static bool IsValidLcid(int lcid, bool canBeCustomLcid)
		{
			if (canBeCustomLcid && CultureTableRecord.IsCustomCultureId(lcid))
			{
				return true;
			}
			if (CultureTable.Default.IsExistingCulture(lcid))
			{
				return true;
			}
			CultureTableRecord.InitSyntheticMapping();
			return CultureTableRecord.SyntheticLcidToNameCache[lcid] != null;
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x000799A0 File Offset: 0x000789A0
		internal unsafe bool Validate()
		{
			if (this.memoryMapFile == null)
			{
				return true;
			}
			long fileSize = this.memoryMapFile.FileSize;
			if ((long)(sizeof(EndianessHeader) + sizeof(CultureTableHeader) + sizeof(CultureTableData) + 8) > fileSize)
			{
				return false;
			}
			EndianessHeader* pDataFileStart = (EndianessHeader*)this.m_pDataFileStart;
			if ((ulong)pDataFileStart->leOffset > (ulong)fileSize)
			{
				return false;
			}
			if ((ulong)(this.m_pCultureHeader->offsetToCultureItemData + (uint)this.m_pCultureHeader->sizeCultureItem) > (ulong)fileSize)
			{
				return false;
			}
			if ((ulong)this.m_pCultureHeader->cultureIDTableOffset > (ulong)fileSize)
			{
				return false;
			}
			if ((ulong)(this.m_pCultureHeader->cultureNameTableOffset + 8U) > (ulong)fileSize)
			{
				return false;
			}
			if ((ulong)this.m_pCultureHeader->regionNameTableOffset > (ulong)fileSize)
			{
				return false;
			}
			if ((ulong)(this.m_pCultureHeader->offsetToCalendarItemData + (uint)this.m_pCultureHeader->sizeCalendarItem) > (ulong)fileSize)
			{
				return false;
			}
			if ((ulong)this.m_pCultureHeader->offsetToDataPool > (ulong)fileSize)
			{
				return false;
			}
			ushort* ptr = (ushort*)(this.m_pDataFileStart + this.m_pCultureHeader->offsetToDataPool);
			int num = (int)(fileSize - (long)(ptr - this.m_pDataFileStart)) / 2;
			if (num <= 0)
			{
				return false;
			}
			uint num2 = (uint)(*(ushort*)(this.m_pDataFileStart + this.m_pCultureHeader->cultureNameTableOffset));
			CultureTableData* ptr2 = (CultureTableData*)(this.m_pDataFileStart + this.m_pCultureHeader->offsetToCultureItemData);
			if (ptr2->iLanguage == 127 || !CultureTable.IsValidLcid((int)ptr2->iLanguage, true))
			{
				return false;
			}
			string text = CultureTable.CheckAndGetTheString(ptr, ptr2->sName, num);
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (num2 != ptr2->sName && !text.Equals(CultureTable.CheckAndGetTheString(ptr, num2, num)))
			{
				return false;
			}
			string text2 = CultureTable.CheckAndGetTheString(ptr, ptr2->sParent, num);
			return text2 != null && !text2.Equals(text, StringComparison.OrdinalIgnoreCase) && CultureTable.IsValidLcid((int)ptr2->iTextInfo, false) && CultureTable.IsValidLcid((int)ptr2->iCompareInfo, false) && CultureTable.ValidateString(ptr, ptr2->waGrouping, num) && CultureTable.ValidateString(ptr, ptr2->waMonetaryGrouping, num) && CultureTable.ValidateString(ptr, ptr2->sListSeparator, num) && CultureTable.ValidateString(ptr, ptr2->sDecimalSeparator, num) && CultureTable.ValidateString(ptr, ptr2->sThousandSeparator, num) && CultureTable.ValidateString(ptr, ptr2->sCurrency, num) && CultureTable.ValidateString(ptr, ptr2->sMonetaryDecimal, num) && CultureTable.ValidateString(ptr, ptr2->sMonetaryThousand, num) && CultureTable.ValidateString(ptr, ptr2->sPositiveSign, num) && CultureTable.ValidateString(ptr, ptr2->sNegativeSign, num) && CultureTable.ValidateString(ptr, ptr2->sAM1159, num) && CultureTable.ValidateString(ptr, ptr2->sPM2359, num) && CultureTable.ValidateStringArray(ptr, ptr2->saNativeDigits, num) && CultureTable.ValidateStringArray(ptr, ptr2->saTimeFormat, num) && CultureTable.ValidateStringArray(ptr, ptr2->saShortDate, num) && CultureTable.ValidateStringArray(ptr, ptr2->saLongDate, num) && CultureTable.ValidateStringArray(ptr, ptr2->saYearMonth, num) && CultureTable.ValidateStringArray(ptr, ptr2->saDuration, num) && CultureTable.ValidateString(ptr, ptr2->waCalendars, num) && CultureTable.ValidateString(ptr, ptr2->sAbbrevLang, num) && CultureTable.ValidateString(ptr, ptr2->sISO639Language, num) && CultureTable.ValidateString(ptr, ptr2->sEnglishLanguage, num) && CultureTable.ValidateString(ptr, ptr2->sNativeLanguage, num) && CultureTable.ValidateString(ptr, ptr2->sEnglishCountry, num) && CultureTable.ValidateString(ptr, ptr2->sNativeCountry, num) && CultureTable.ValidateString(ptr, ptr2->sAbbrevCountry, num) && CultureTable.ValidateString(ptr, ptr2->sISO3166CountryName, num) && CultureTable.ValidateString(ptr, ptr2->sIntlMonetarySymbol, num) && CultureTable.ValidateString(ptr, ptr2->sEnglishCurrency, num) && CultureTable.ValidateString(ptr, ptr2->sNativeCurrency, num) && CultureTable.ValidateString(ptr, ptr2->waFontSignature, num) && CultureTable.ValidateString(ptr, ptr2->sISO639Language2, num) && CultureTable.ValidateString(ptr, ptr2->sISO3166CountryName2, num) && CultureTable.ValidateStringArray(ptr, ptr2->saDayNames, num) && CultureTable.ValidateStringArray(ptr, ptr2->saAbbrevDayNames, num) && CultureTable.ValidateStringArray(ptr, ptr2->saMonthNames, num) && CultureTable.ValidateStringArray(ptr, ptr2->saAbbrevMonthNames, num) && CultureTable.ValidateStringArray(ptr, ptr2->saMonthGenitiveNames, num) && CultureTable.ValidateStringArray(ptr, ptr2->saAbbrevMonthGenitiveNames, num) && CultureTable.ValidateStringArray(ptr, ptr2->saNativeCalendarNames, num) && CultureTable.ValidateStringArray(ptr, ptr2->saAltSortID, num) && CultureTable.ValidateString(ptr, ptr2->sEnglishDisplayName, num) && CultureTable.ValidateString(ptr, ptr2->sNativeDisplayName, num) && CultureTable.ValidateString(ptr, ptr2->sPercent, num) && CultureTable.ValidateString(ptr, ptr2->sNaN, num) && CultureTable.ValidateString(ptr, ptr2->sPositiveInfinity, num) && CultureTable.ValidateString(ptr, ptr2->sNegativeInfinity, num) && CultureTable.ValidateString(ptr, ptr2->sMonthDay, num) && CultureTable.ValidateString(ptr, ptr2->sAdEra, num) && CultureTable.ValidateString(ptr, ptr2->sAbbrevAdEra, num) && CultureTable.ValidateString(ptr, ptr2->sRegionName, num) && CultureTable.ValidateString(ptr, ptr2->sConsoleFallbackName, num) && CultureTable.ValidateStringArray(ptr, ptr2->saShortTime, num) && CultureTable.ValidateStringArray(ptr, ptr2->saSuperShortDayNames, num) && CultureTable.ValidateStringArray(ptr, ptr2->saDateWords, num) && CultureTable.ValidateString(ptr, ptr2->sSpecificCulture, num);
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x00079F50 File Offset: 0x00078F50
		internal unsafe int GetDataItemFromCultureName(string name, out int culture, out string actualName)
		{
			culture = -1;
			actualName = "";
			CultureTableItem cultureTableItem = (CultureTableItem)this.hashByName[name];
			if (cultureTableItem != null && cultureTableItem.culture != 0)
			{
				culture = cultureTableItem.culture;
				actualName = cultureTableItem.name;
				return cultureTableItem.dataItem;
			}
			int i = 0;
			int num = (int)(this.m_pCultureHeader->numCultureNames - 1);
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				int num3 = base.CompareStringToStringPoolStringBinary(name, (int)this.m_pCultureNameIndex[num2].strOffset);
				if (num3 == 0)
				{
					cultureTableItem = new CultureTableItem();
					int result = cultureTableItem.dataItem = (int)this.m_pCultureNameIndex[num2].dataItemIndex;
					culture = (cultureTableItem.culture = this.m_pCultureNameIndex[num2].actualCultureID);
					actualName = (cultureTableItem.name = base.GetStringPoolString((uint)this.m_pCultureNameIndex[num2].strOffset));
					this.hashByName[name] = cultureTableItem;
					return result;
				}
				if (num3 < 0)
				{
					num = num2 - 1;
				}
				else
				{
					i = num2 + 1;
				}
			}
			culture = -1;
			return -1;
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x0007A078 File Offset: 0x00079078
		internal unsafe int GetDataItemFromRegionName(string name)
		{
			object obj;
			if ((obj = this.hashByRegionName[name]) != null)
			{
				return (int)obj;
			}
			int i = 0;
			int num = (int)(this.m_pCultureHeader->numRegionNames - 1);
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				int num3 = base.CompareStringToStringPoolStringBinary(name, (int)this.m_pRegionNameIndex[num2].strOffset);
				if (num3 == 0)
				{
					int dataItemIndex = (int)this.m_pRegionNameIndex[num2].dataItemIndex;
					this.hashByRegionName[name] = dataItemIndex;
					return dataItemIndex;
				}
				if (num3 < 0)
				{
					num = num2 - 1;
				}
				else
				{
					i = num2 + 1;
				}
			}
			return -1;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x0007A118 File Offset: 0x00079118
		internal unsafe int GetDataItemFromCultureID(int cultureID, out string actualName)
		{
			CultureTableItem cultureTableItem = (CultureTableItem)this.hashByLcid[cultureID];
			if (cultureTableItem != null && cultureTableItem.culture != 0)
			{
				actualName = cultureTableItem.name;
				return cultureTableItem.dataItem;
			}
			int i = 0;
			int num = (int)(this.m_pCultureHeader->numCultureNames - 1);
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				int num3 = cultureID - this.m_pCultureIDIndex[num2].actualCultureID;
				if (num3 == 0)
				{
					cultureTableItem = new CultureTableItem();
					int result = cultureTableItem.dataItem = (int)this.m_pCultureIDIndex[num2].dataItemIndex;
					cultureTableItem.culture = cultureID;
					actualName = (cultureTableItem.name = base.GetStringPoolString((uint)this.m_pCultureIDIndex[num2].strOffset));
					this.hashByLcid[cultureID] = cultureTableItem;
					return result;
				}
				if (num3 < 0)
				{
					num = num2 - 1;
				}
				else
				{
					i = num2 + 1;
				}
			}
			actualName = "";
			return -1;
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x0007A218 File Offset: 0x00079218
		internal static bool IsInstalledLCID(int cultureID)
		{
			if ((Environment.OSInfo & Environment.OSName.Win9x) != Environment.OSName.Invalid)
			{
				return CultureInfo.IsWin9xInstalledCulture(string.Format(CultureInfo.InvariantCulture, "{0,8:X08}", new object[]
				{
					cultureID
				}), cultureID);
			}
			return CultureInfo.IsValidLCID(cultureID, 1);
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x0007A260 File Offset: 0x00079260
		internal bool IsExistingCulture(int lcid)
		{
			string text;
			return lcid != 0 && this.GetDataItemFromCultureID(lcid, out text) >= 0;
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x0007A281 File Offset: 0x00079281
		internal static bool IsOldNeutralChineseCulture(CultureInfo ci)
		{
			return (ci.LCID == 31748 && ci.Name.Equals("zh-CHT")) || (ci.LCID == 4 && ci.Name.Equals("zh-CHS"));
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x0007A2C0 File Offset: 0x000792C0
		internal static bool IsNewNeutralChineseCulture(CultureInfo ci)
		{
			return (ci.LCID == 31748 && ci.Name.Equals("zh-Hant")) || (ci.LCID == 4 && ci.Name.Equals("zh-Hans"));
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x0007A300 File Offset: 0x00079300
		internal unsafe CultureInfo[] GetCultures(CultureTypes types)
		{
			if (types <= (CultureTypes)0 || (types & ~(CultureTypes.NeutralCultures | CultureTypes.SpecificCultures | CultureTypes.InstalledWin32Cultures | CultureTypes.UserCustomCulture | CultureTypes.ReplacementCultures | CultureTypes.WindowsOnlyCultures | CultureTypes.FrameworkCultures)) != (CultureTypes)0)
			{
				throw new ArgumentOutOfRangeException("types", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					CultureTypes.NeutralCultures,
					CultureTypes.FrameworkCultures
				}));
			}
			ArrayList arrayList = new ArrayList();
			bool flag = (types & CultureTypes.SpecificCultures) != (CultureTypes)0;
			bool flag2 = (types & CultureTypes.NeutralCultures) != (CultureTypes)0;
			bool flag3 = (types & CultureTypes.InstalledWin32Cultures) != (CultureTypes)0;
			bool flag4 = (types & CultureTypes.UserCustomCulture) != (CultureTypes)0;
			bool flag5 = (types & CultureTypes.ReplacementCultures) != (CultureTypes)0;
			bool flag6 = (types & CultureTypes.FrameworkCultures) != (CultureTypes)0;
			bool flag7 = (types & CultureTypes.WindowsOnlyCultures) != (CultureTypes)0;
			StringBuilder stringBuilder = new StringBuilder(260);
			stringBuilder.Append(Environment.InternalWindowsDirectory);
			stringBuilder.Append("\\Globalization\\");
			string path = stringBuilder.ToString();
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path).Assert();
			try
			{
				if (Directory.Exists(path))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.nlp"))
					{
						if (fileInfo.Name.Length > 4)
						{
							try
							{
								CultureInfo cultureInfo = new CultureInfo(fileInfo.Name.Substring(0, fileInfo.Name.Length - 4), true);
								CultureTypes cultureTypes = cultureInfo.CultureTypes;
								if (!CultureTable.IsNewNeutralChineseCulture(cultureInfo) && ((flag4 && (cultureTypes & CultureTypes.UserCustomCulture) != (CultureTypes)0) || (flag5 && (cultureTypes & CultureTypes.ReplacementCultures) != (CultureTypes)0) || (flag && (cultureTypes & CultureTypes.SpecificCultures) != (CultureTypes)0) || (flag2 && (cultureTypes & CultureTypes.NeutralCultures) != (CultureTypes)0) || (flag6 && (cultureTypes & CultureTypes.FrameworkCultures) != (CultureTypes)0) || (flag3 && (cultureTypes & CultureTypes.InstalledWin32Cultures) != (CultureTypes)0) || (flag7 && (cultureTypes & CultureTypes.WindowsOnlyCultures) != (CultureTypes)0)))
								{
									arrayList.Add(cultureInfo);
								}
							}
							catch (ArgumentException)
							{
							}
						}
					}
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (flag2 || flag || flag6 || flag3)
			{
				for (int j = 0; j < (int)this.m_pCultureHeader->numCultureNames; j++)
				{
					int actualCultureID = this.m_pCultureIDIndex[j].actualCultureID;
					if (CultureInfo.GetSortID(actualCultureID) == 0 && actualCultureID != 1034)
					{
						CultureInfo cultureInfo2 = new CultureInfo(actualCultureID);
						CultureTypes cultureTypes2 = cultureInfo2.CultureTypes;
						if ((cultureTypes2 & CultureTypes.ReplacementCultures) == (CultureTypes)0 && (flag6 || (flag && cultureInfo2.Name.Length > 0 && (cultureTypes2 & CultureTypes.SpecificCultures) != (CultureTypes)0) || (flag2 && ((cultureTypes2 & CultureTypes.NeutralCultures) != (CultureTypes)0 || cultureInfo2.Name.Length == 0)) || (flag3 && (cultureTypes2 & CultureTypes.InstalledWin32Cultures) != (CultureTypes)0)))
						{
							arrayList.Add(cultureInfo2);
						}
					}
					if (actualCultureID == 4 || actualCultureID == 31748)
					{
						j++;
					}
				}
			}
			if (flag7 || flag || flag3)
			{
				CultureTableRecord.InitSyntheticMapping();
				foreach (object obj in CultureTableRecord.SyntheticLcidToNameCache.Keys)
				{
					int num = (int)obj;
					if (CultureInfo.GetSortID(num) == 0)
					{
						CultureInfo cultureInfo3 = new CultureInfo(num);
						if ((cultureInfo3.CultureTypes & CultureTypes.ReplacementCultures) == (CultureTypes)0)
						{
							arrayList.Add(cultureInfo3);
						}
					}
				}
			}
			CultureInfo[] array = new CultureInfo[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x04001282 RID: 4738
		internal const int ILANGUAGE = 0;

		// Token: 0x04001283 RID: 4739
		internal const string TraditionalChineseCultureName = "zh-CHT";

		// Token: 0x04001284 RID: 4740
		internal const string SimplifiedChineseCultureName = "zh-CHS";

		// Token: 0x04001285 RID: 4741
		internal const string NewTraditionalChineseCultureName = "zh-Hant";

		// Token: 0x04001286 RID: 4742
		internal const string NewSimplifiedChineseCultureName = "zh-Hans";

		// Token: 0x04001287 RID: 4743
		private const CultureTypes CultureTypesMask = ~(CultureTypes.NeutralCultures | CultureTypes.SpecificCultures | CultureTypes.InstalledWin32Cultures | CultureTypes.UserCustomCulture | CultureTypes.ReplacementCultures | CultureTypes.WindowsOnlyCultures | CultureTypes.FrameworkCultures);

		// Token: 0x04001288 RID: 4744
		internal const string TypeLoadExceptionMessage = "Failure has occurred while loading a type.";

		// Token: 0x04001289 RID: 4745
		private const uint sizeofNameOffsetItem = 8U;

		// Token: 0x0400128A RID: 4746
		private Hashtable hashByName;

		// Token: 0x0400128B RID: 4747
		private Hashtable hashByRegionName;

		// Token: 0x0400128C RID: 4748
		private Hashtable hashByLcid;

		// Token: 0x0400128D RID: 4749
		private unsafe CultureNameOffsetItem* m_pCultureNameIndex;

		// Token: 0x0400128E RID: 4750
		private unsafe RegionNameOffsetItem* m_pRegionNameIndex;

		// Token: 0x0400128F RID: 4751
		private unsafe IDOffsetItem* m_pCultureIDIndex;

		// Token: 0x04001290 RID: 4752
		private static CultureTable m_defaultInstance = new CultureTable("culture.nlp", true);
	}
}
