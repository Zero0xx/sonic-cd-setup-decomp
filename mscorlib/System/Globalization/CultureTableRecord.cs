using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x020003DD RID: 989
	internal class CultureTableRecord
	{
		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x0600284A RID: 10314 RVA: 0x0007A658 File Offset: 0x00079658
		private static object InternalSyncObject
		{
			get
			{
				if (CultureTableRecord.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref CultureTableRecord.s_InternalSyncObject, value, null);
				}
				return CultureTableRecord.s_InternalSyncObject;
			}
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x0007A684 File Offset: 0x00079684
		private CultureTable GetCustomCultureTable(string name)
		{
			CultureTable cultureTable = null;
			string customCultureFile = this.GetCustomCultureFile(name);
			if (customCultureFile == null)
			{
				return null;
			}
			try
			{
				cultureTable = new CultureTable(customCultureFile, false);
				if (!cultureTable.IsValid)
				{
					int num;
					string text;
					int dataItemFromCultureName = CultureTable.Default.GetDataItemFromCultureName(name, out num, out text);
					if (dataItemFromCultureName < 0)
					{
						CultureTableRecord.InitSyntheticMapping();
						if (CultureTableRecord.SyntheticNameToLcidCache[name] == null)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_CorruptedCustomCultureFile"), new object[]
							{
								name
							}));
						}
					}
					return null;
				}
			}
			catch (FileNotFoundException)
			{
				cultureTable = null;
			}
			return cultureTable;
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x0007A724 File Offset: 0x00079724
		internal CultureTable TryCreateReplacementCulture(string replacementCultureName, out int dataItem)
		{
			string name = CultureTableRecord.ValidateCulturePieceToLower(replacementCultureName, "cultureName", 84);
			CultureTable customCultureTable = this.GetCustomCultureTable(name);
			if (customCultureTable == null)
			{
				dataItem = -1;
				return null;
			}
			int num;
			string text;
			dataItem = customCultureTable.GetDataItemFromCultureName(name, out num, out text);
			if (dataItem < 0)
			{
				return null;
			}
			return customCultureTable;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x0007A764 File Offset: 0x00079764
		internal static void InitSyntheticMapping()
		{
			if (CultureTableRecord.SyntheticLcidToNameCache == null || CultureTableRecord.SyntheticNameToLcidCache == null)
			{
				CultureTableRecord.CacheSyntheticNameLcidMapping();
			}
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x0007A77C File Offset: 0x0007977C
		internal static CultureTableRecord GetCultureTableRecord(string name, bool useUserOverride)
		{
			if (CultureTableRecord.CultureTableRecordCache == null)
			{
				if (name.Length == 0)
				{
					return new CultureTableRecord(name, useUserOverride);
				}
				lock (CultureTableRecord.InternalSyncObject)
				{
					if (CultureTableRecord.CultureTableRecordCache == null)
					{
						CultureTableRecord.CultureTableRecordCache = new Hashtable();
					}
				}
			}
			name = CultureTableRecord.ValidateCulturePieceToLower(name, "name", 84);
			CultureTableRecord[] array = (CultureTableRecord[])CultureTableRecord.CultureTableRecordCache[name];
			if (array != null)
			{
				int num = useUserOverride ? 0 : 1;
				if (array[num] == null)
				{
					int num2 = (num == 0) ? 1 : 0;
					array[num] = array[num2].CloneWithUserOverride(useUserOverride);
				}
				return array[num];
			}
			CultureTableRecord cultureTableRecord = new CultureTableRecord(name, useUserOverride);
			lock (CultureTableRecord.InternalSyncObject)
			{
				if (CultureTableRecord.CultureTableRecordCache[name] == null)
				{
					array = new CultureTableRecord[2];
					array[useUserOverride ? 0 : 1] = cultureTableRecord;
					CultureTableRecord.CultureTableRecordCache[name] = array;
				}
			}
			return cultureTableRecord;
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x0007A878 File Offset: 0x00079878
		internal static CultureTableRecord GetCultureTableRecord(int cultureId, bool useUserOverride)
		{
			if (cultureId == 127)
			{
				return CultureTableRecord.GetCultureTableRecord("", false);
			}
			string text = null;
			if (CultureTable.Default.GetDataItemFromCultureID(cultureId, out text) < 0 && CultureInfo.IsValidLCID(cultureId, 1))
			{
				CultureTableRecord.InitSyntheticMapping();
				text = (string)CultureTableRecord.SyntheticLcidToNameCache[cultureId];
			}
			if (text != null && text.Length > 0)
			{
				return CultureTableRecord.GetCultureTableRecord(text, useUserOverride);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureNotSupported"), new object[]
			{
				cultureId
			}), "culture");
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x0007A910 File Offset: 0x00079910
		internal static CultureTableRecord GetCultureTableRecordForRegion(string regionName, bool useUserOverride)
		{
			if (CultureTableRecord.CultureTableRecordRegionCache == null)
			{
				lock (CultureTableRecord.InternalSyncObject)
				{
					if (CultureTableRecord.CultureTableRecordRegionCache == null)
					{
						CultureTableRecord.CultureTableRecordRegionCache = new Hashtable();
					}
				}
			}
			regionName = CultureTableRecord.ValidateCulturePieceToLower(regionName, "regionName", 84);
			CultureTableRecord[] array = (CultureTableRecord[])CultureTableRecord.CultureTableRecordRegionCache[regionName];
			if (array != null)
			{
				int num = useUserOverride ? 0 : 1;
				if (array[num] == null)
				{
					array[num] = array[(num == 0) ? 1 : 0].CloneWithUserOverride(useUserOverride);
				}
				return array[num];
			}
			int dataItemFromRegionName = CultureTable.Default.GetDataItemFromRegionName(regionName);
			CultureTableRecord cultureTableRecord = null;
			if (dataItemFromRegionName > 0)
			{
				cultureTableRecord = new CultureTableRecord(regionName, dataItemFromRegionName, useUserOverride);
			}
			else
			{
				try
				{
					cultureTableRecord = CultureTableRecord.GetCultureTableRecord(regionName, useUserOverride);
				}
				catch (ArgumentException)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidRegionName"), new object[]
					{
						regionName
					}), "name");
				}
			}
			lock (CultureTableRecord.InternalSyncObject)
			{
				if (CultureTableRecord.CultureTableRecordRegionCache[regionName] == null)
				{
					array = new CultureTableRecord[2];
					array[useUserOverride ? 0 : 1] = cultureTableRecord.CloneWithUserOverride(useUserOverride);
					CultureTableRecord.CultureTableRecordRegionCache[regionName] = array;
				}
			}
			return cultureTableRecord;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x0007AA58 File Offset: 0x00079A58
		internal unsafe CultureTableRecord(int cultureId, bool useUserOverride)
		{
			this.m_bUseUserOverride = useUserOverride;
			int dataItemFromCultureID = CultureTable.Default.GetDataItemFromCultureID(cultureId, out this.m_ActualName);
			if (dataItemFromCultureID < 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureNotSupported"), new object[]
				{
					cultureId
				}), "culture");
			}
			this.m_ActualCultureID = cultureId;
			this.m_CultureTable = CultureTable.Default;
			this.m_pData = (CultureTableData*)(this.m_CultureTable.m_pItemData + (ulong)this.m_CultureTable.m_itemSize * (ulong)((long)dataItemFromCultureID));
			this.m_pPool = this.m_CultureTable.m_pDataPool;
			this.m_CultureName = this.SNAME;
			this.m_CultureID = ((cultureId == 1034) ? cultureId : ((int)this.ILANGUAGE));
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x0007AB20 File Offset: 0x00079B20
		private unsafe CultureTableRecord(string cultureName, bool useUserOverride)
		{
			int num = 0;
			if (cultureName.Length == 0)
			{
				useUserOverride = false;
				num = 127;
			}
			this.m_bUseUserOverride = useUserOverride;
			int num2 = -1;
			if (cultureName.Length > 0)
			{
				string text = cultureName;
				int num3;
				string actualName;
				int dataItemFromCultureName = CultureTable.Default.GetDataItemFromCultureName(text, out num3, out actualName);
				if (dataItemFromCultureName >= 0 && (CultureInfo.GetSortID(num3) > 0 || num3 == 1034))
				{
					int cultureID;
					if (num3 == 1034)
					{
						cultureID = 3082;
					}
					else
					{
						cultureID = CultureInfo.GetLangID(num3);
					}
					string testString;
					if (CultureTable.Default.GetDataItemFromCultureID(cultureID, out testString) >= 0)
					{
						text = CultureTableRecord.ValidateCulturePieceToLower(testString, "cultureName", 84);
					}
				}
				if (!Environment.GetCompatibilityFlag(CompatibilityFlag.DisableReplacementCustomCulture) || CultureTableRecord.IsCustomCultureId(num3))
				{
					this.m_CultureTable = this.GetCustomCultureTable(text);
				}
				if (this.m_CultureTable != null)
				{
					num2 = this.m_CultureTable.GetDataItemFromCultureName(text, out this.m_ActualCultureID, out this.m_ActualName);
					if (dataItemFromCultureName >= 0)
					{
						this.m_ActualCultureID = num3;
						this.m_ActualName = actualName;
					}
				}
				if (num2 < 0 && dataItemFromCultureName >= 0)
				{
					this.m_CultureTable = CultureTable.Default;
					this.m_ActualCultureID = num3;
					this.m_ActualName = actualName;
					num2 = dataItemFromCultureName;
				}
				if (num2 < 0)
				{
					CultureTableRecord.InitSyntheticMapping();
					if (CultureTableRecord.SyntheticNameToLcidCache[text] != null)
					{
						num = (int)CultureTableRecord.SyntheticNameToLcidCache[text];
					}
				}
			}
			if (num2 < 0 && num > 0)
			{
				if (num == 127)
				{
					num2 = CultureTable.Default.GetDataItemFromCultureID(num, out this.m_ActualName);
					if (num2 > 0)
					{
						this.m_ActualCultureID = num;
						this.m_CultureTable = CultureTable.Default;
					}
				}
				else
				{
					CultureTable cultureTable = null;
					string text2 = null;
					if (CultureInfo.GetSortID(num) > 0)
					{
						num2 = CultureTable.Default.GetDataItemFromCultureID(CultureInfo.GetLangID(num), out text2);
					}
					if (num2 < 0)
					{
						text2 = (string)CultureTableRecord.SyntheticLcidToNameCache[CultureInfo.GetLangID(num)];
					}
					string text3 = (string)CultureTableRecord.SyntheticLcidToNameCache[num];
					int num4 = -1;
					if (text3 != null && text2 != null && !Environment.GetCompatibilityFlag(CompatibilityFlag.DisableReplacementCustomCulture))
					{
						cultureTable = this.TryCreateReplacementCulture(text2, out num4);
					}
					if (cultureTable == null)
					{
						if (num2 > 0)
						{
							this.m_CultureTable = CultureTable.Default;
							this.m_ActualCultureID = num;
							this.m_synthetic = true;
							this.m_ActualName = CultureInfo.nativeGetCultureName(num, true, false);
						}
						else if (this.GetSyntheticCulture(num))
						{
							return;
						}
					}
					else
					{
						this.m_CultureTable = cultureTable;
						num2 = num4;
						this.m_ActualName = CultureInfo.nativeGetCultureName(num, true, false);
						this.m_ActualCultureID = num;
					}
				}
			}
			if (num2 >= 0)
			{
				this.m_pData = (CultureTableData*)(this.m_CultureTable.m_pItemData + (ulong)this.m_CultureTable.m_itemSize * (ulong)((long)num2));
				this.m_pPool = this.m_CultureTable.m_pDataPool;
				this.m_CultureName = this.SNAME;
				this.m_CultureID = ((this.m_ActualCultureID == 1034) ? this.m_ActualCultureID : ((int)this.ILANGUAGE));
				this.CheckCustomSynthetic();
				return;
			}
			if (cultureName != null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidCultureName"), new object[]
				{
					cultureName
				}), "name");
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureNotSupported"), new object[]
			{
				num
			}), "culture");
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x0007AE44 File Offset: 0x00079E44
		private unsafe CultureTableRecord(string regionName, int dataItem, bool useUserOverride)
		{
			this.m_bUseUserOverride = useUserOverride;
			this.m_CultureName = regionName;
			this.m_CultureTable = CultureTable.Default;
			this.m_pData = (CultureTableData*)(this.m_CultureTable.m_pItemData + (ulong)this.m_CultureTable.m_itemSize * (ulong)((long)dataItem));
			this.m_pPool = this.m_CultureTable.m_pDataPool;
			this.m_CultureID = (int)this.ILANGUAGE;
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x0007AEB0 File Offset: 0x00079EB0
		private void CheckCustomSynthetic()
		{
			if (this.IsCustomCulture)
			{
				CultureTableRecord.InitSyntheticMapping();
				if (CultureTableRecord.IsCustomCultureId(this.m_CultureID))
				{
					string key = CultureTableRecord.ValidateCulturePieceToLower(this.m_CultureName, "CultureName", 84);
					if (CultureTableRecord.SyntheticNameToLcidCache[key] != null)
					{
						this.m_synthetic = true;
						this.m_ActualCultureID = (this.m_CultureID = (int)CultureTableRecord.SyntheticNameToLcidCache[key]);
						return;
					}
				}
				else
				{
					if (CultureTableRecord.SyntheticLcidToNameCache[this.m_CultureID] != null)
					{
						this.m_synthetic = true;
						this.m_ActualCultureID = this.m_CultureID;
						return;
					}
					if (this.m_CultureID != this.m_ActualCultureID && CultureTableRecord.SyntheticLcidToNameCache[this.m_ActualCultureID] != null)
					{
						this.m_synthetic = true;
					}
				}
			}
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x0007AF78 File Offset: 0x00079F78
		internal static void ResetCustomCulturesCache()
		{
			CultureTableRecord.CultureTableRecordCache = null;
			CultureTableRecord.CultureTableRecordRegionCache = null;
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x0007AF88 File Offset: 0x00079F88
		private static bool GetScriptTag(int lcid, out string script)
		{
			script = null;
			string text = CultureInfo.nativeGetCultureName(lcid, false, true);
			if (text == null)
			{
				return false;
			}
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] >= 'ᐁ' && text[i] <= 'ᙶ')
				{
					script = "cans";
					return true;
				}
				if (text[i] >= 'ሀ' && text[i] <= '፼')
				{
					script = "ethi";
					return true;
				}
				if (text[i] >= '᠀' && text[i] <= '᠙')
				{
					script = "mong";
					return true;
				}
				if (text[i] >= 'ꀀ' && text[i] <= '꓆')
				{
					script = "yiii";
					return true;
				}
				if (text[i] >= 'Ꭰ' && text[i] <= 'Ᏼ')
				{
					script = "cher";
					return true;
				}
				if (text[i] >= 'ក' && text[i] <= '៹')
				{
					script = "khmr";
					return true;
				}
			}
			byte[] array;
			int nativeSortKey = CultureInfo.GetNativeSortKey(lcid, 0, text, text.Length, out array);
			if (nativeSortKey == 0)
			{
				return false;
			}
			int num = 0;
			while (num < nativeSortKey && array[num] != 1)
			{
				byte b = array[num];
				switch (b)
				{
				case 14:
					script = "latn";
					return true;
				case 15:
					script = "grek";
					return true;
				case 16:
					script = "cyrl";
					return true;
				case 17:
					script = "armn";
					return true;
				case 18:
					script = "hebr";
					return true;
				case 19:
					script = "arab";
					return true;
				case 20:
					script = "deva";
					return true;
				case 21:
					script = "beng";
					return true;
				case 22:
					script = "guru";
					return true;
				case 23:
					script = "gujr";
					return true;
				case 24:
					script = "orya";
					return true;
				case 25:
					script = "taml";
					return true;
				case 26:
					script = "telu";
					return true;
				case 27:
					script = "knda";
					return true;
				case 28:
					script = "mlym";
					return true;
				case 29:
					script = "sinh";
					return true;
				case 30:
					script = "thai";
					return true;
				case 31:
					script = "laoo";
					return true;
				case 32:
					script = "tibt";
					return true;
				case 33:
					script = "geor";
					return true;
				case 34:
					script = "kana";
					return true;
				case 35:
					script = "bopo";
					return true;
				case 36:
					script = "hang";
					return true;
				default:
					if (b == 128)
					{
						script = "hani";
						return true;
					}
					num += 2;
					break;
				}
			}
			return false;
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x0007B224 File Offset: 0x0007A224
		private static bool IsBuiltInCulture(int lcid)
		{
			return CultureTable.Default.IsExistingCulture(lcid);
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x0007B234 File Offset: 0x0007A234
		internal static string Concatenate(StringBuilder helper, params string[] stringsToConcat)
		{
			if (helper.Length > 0)
			{
				helper.Remove(0, helper.Length);
			}
			for (int i = 0; i < stringsToConcat.Length; i++)
			{
				helper.Append(stringsToConcat[i]);
			}
			return helper.ToString();
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x0007B278 File Offset: 0x0007A278
		internal static bool GetCultureNamesUsingSNameLCType(int[] lcidArray, Hashtable lcidToName, Hashtable nameToLcid)
		{
			string text = CultureInfo.nativeGetCultureName(lcidArray[0], true, false);
			if (text == null)
			{
				return false;
			}
			if (!CultureTableRecord.IsBuiltInCulture(lcidArray[0]) && !CultureTableRecord.IsCustomCultureId(lcidArray[0]))
			{
				text = CultureTableRecord.ValidateCulturePieceToLower(text, "cultureName", text.Length);
				nameToLcid[text] = lcidArray[0];
				lcidToName[lcidArray[0]] = text;
			}
			for (int i = 1; i < lcidArray.Length; i++)
			{
				if (!CultureTableRecord.IsBuiltInCulture(lcidArray[i]) || CultureTableRecord.IsCustomCultureId(lcidArray[0]))
				{
					text = CultureInfo.nativeGetCultureName(lcidArray[i], true, false);
					if (text != null)
					{
						text = CultureTableRecord.ValidateCulturePieceToLower(text, "cultureName", text.Length);
						nameToLcid[text] = lcidArray[i];
						lcidToName[lcidArray[i]] = text;
					}
				}
			}
			return true;
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x0007B33C File Offset: 0x0007A33C
		internal static void CacheSyntheticNameLcidMapping()
		{
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			int[] array = null;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(typeof(CultureTableRecord), ref flag2);
				flag = CultureInfo.nativeEnumSystemLocales(out array);
			}
			finally
			{
				if (flag2)
				{
					Monitor.Exit(typeof(CultureTableRecord));
				}
			}
			if (flag && !CultureTableRecord.GetCultureNamesUsingSNameLCType(array, hashtable, hashtable2))
			{
				Hashtable namesHashtable = CultureTableRecord.GetNamesHashtable();
				StringBuilder helper = new StringBuilder();
				foreach (int num in array)
				{
					if (!CultureTableRecord.IsBuiltInCulture(num) && !CultureTableRecord.IsCustomCultureId(num))
					{
						CultureTableRecord.AdjustedSyntheticCultureName adjustedSyntheticCultureName;
						CultureTableRecord.GetAdjustedNames(num, out adjustedSyntheticCultureName);
						string text;
						if (adjustedSyntheticCultureName != null)
						{
							text = adjustedSyntheticCultureName.sName;
						}
						else
						{
							text = CultureInfo.nativeGetCultureName(num, false, false);
						}
						if (text != null)
						{
							text = CultureTableRecord.ValidateCulturePieceToLower(text, "cultureName", text.Length);
							if (namesHashtable[text] != null)
							{
								string text2;
								if (CultureTableRecord.GetScriptTag(num, out text2))
								{
									text2 = CultureTableRecord.Concatenate(helper, new string[]
									{
										text,
										"-",
										text2
									});
									text2 = CultureTableRecord.GetQualifiedName(text2);
									hashtable2[text2] = num;
									hashtable[num] = text2;
								}
							}
							else if (hashtable2[text] == null)
							{
								hashtable2[text] = num;
								hashtable[num] = text;
							}
							else
							{
								int num2 = (int)hashtable2[text];
								hashtable2.Remove(text);
								hashtable.Remove(num2);
								namesHashtable[text] = "";
								string text2;
								if (CultureTableRecord.GetScriptTag(num2, out text2))
								{
									text2 = CultureTableRecord.Concatenate(helper, new string[]
									{
										text,
										"-",
										text2
									});
									text2 = CultureTableRecord.GetQualifiedName(text2);
									hashtable2[text2] = num2;
									hashtable[num2] = text2;
								}
								if (CultureTableRecord.GetScriptTag(num, out text2))
								{
									text2 = CultureTableRecord.Concatenate(helper, new string[]
									{
										text,
										"-",
										text2
									});
									text2 = CultureTableRecord.GetQualifiedName(text2);
									hashtable2[text2] = num;
									hashtable[num] = text2;
								}
							}
						}
					}
				}
			}
			lock (CultureTableRecord.InternalSyncObject)
			{
				CultureTableRecord.SyntheticLcidToNameCache = hashtable;
				CultureTableRecord.SyntheticNameToLcidCache = hashtable2;
			}
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x0007B5E8 File Offset: 0x0007A5E8
		private static void AdjustSyntheticCalendars(ref CultureData data, ref CultureTableRecord.CompositeCultureData compositeData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			ushort num2 = data.waCalendars[0];
			stringBuilder.Append((char)num2);
			for (int i = 1; i < data.waCalendars.Length; i++)
			{
				stringBuilder.Append((char)data.waCalendars[i]);
				if (data.waCalendars[i] == (ushort)data.iDefaultCalender)
				{
					num = i;
				}
				if (data.waCalendars[i] > num2)
				{
					num2 = data.waCalendars[i];
				}
			}
			if (num2 > 1)
			{
				string[] array = new string[(int)num2];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = string.Empty;
				}
				for (int k = 0; k < data.waCalendars.Length; k++)
				{
					array[(int)(data.waCalendars[k] - 1)] = data.saNativeCalendarNames[k];
				}
				data.saNativeCalendarNames = array;
			}
			if (num > 0)
			{
				char value = stringBuilder[num];
				stringBuilder[num] = stringBuilder[0];
				stringBuilder[0] = value;
			}
			compositeData.waCalendars = stringBuilder.ToString();
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x0007B6E8 File Offset: 0x0007A6E8
		private unsafe bool GetSyntheticCulture(int cultureID)
		{
			if (CultureTableRecord.SyntheticLcidToNameCache == null || CultureTableRecord.SyntheticNameToLcidCache == null)
			{
				CultureTableRecord.CacheSyntheticNameLcidMapping();
			}
			if (CultureTableRecord.SyntheticLcidToNameCache[cultureID] == null)
			{
				return false;
			}
			if (CultureTableRecord.SyntheticDataCache == null)
			{
				CultureTableRecord.SyntheticDataCache = new Hashtable();
			}
			else
			{
				this.nativeMemoryHandle = (AgileSafeNativeMemoryHandle)CultureTableRecord.SyntheticDataCache[cultureID];
			}
			if (this.nativeMemoryHandle != null)
			{
				this.m_pData = (CultureTableData*)((void*)this.nativeMemoryHandle.DangerousGetHandle());
				this.m_pPool = (ushort*)(this.m_pData + 1);
				this.m_CultureTable = CultureTable.Default;
				this.m_CultureName = this.SNAME;
				this.m_CultureID = cultureID;
				this.m_synthetic = true;
				this.m_ActualCultureID = cultureID;
				this.m_ActualName = this.m_CultureName;
				return true;
			}
			CultureData cultureData = default(CultureData);
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(typeof(CultureTableRecord), ref flag2);
				flag = CultureInfo.nativeGetCultureData(cultureID, ref cultureData);
			}
			finally
			{
				if (flag2)
				{
					Monitor.Exit(typeof(CultureTableRecord));
				}
			}
			if (!flag)
			{
				return false;
			}
			CultureTableRecord.CompositeCultureData compositeCultureData = default(CultureTableRecord.CompositeCultureData);
			int cultureDataSize = this.GetCultureDataSize(cultureID, ref cultureData, ref compositeCultureData);
			IntPtr intPtr = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					intPtr = Marshal.AllocHGlobal(cultureDataSize);
					if (intPtr != IntPtr.Zero)
					{
						this.nativeMemoryHandle = new AgileSafeNativeMemoryHandle(intPtr, true);
					}
				}
			}
			finally
			{
				if (this.nativeMemoryHandle == null && intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
					intPtr = IntPtr.Zero;
				}
			}
			if (intPtr == IntPtr.Zero)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("OutOfMemory_MemFailPoint"));
			}
			this.m_pData = (CultureTableData*)((void*)this.nativeMemoryHandle.DangerousGetHandle());
			this.m_pPool = (ushort*)(this.m_pData + 1);
			this.FillCultureDataMemory(cultureID, ref cultureData, ref compositeCultureData);
			this.m_CultureTable = CultureTable.Default;
			this.m_CultureName = this.SNAME;
			this.m_CultureID = cultureID;
			this.m_synthetic = true;
			this.m_ActualCultureID = cultureID;
			this.m_ActualName = this.m_CultureName;
			lock (CultureTableRecord.SyntheticDataCache)
			{
				if (CultureTableRecord.SyntheticDataCache[cultureID] == null)
				{
					CultureTableRecord.SyntheticDataCache[cultureID] = this.nativeMemoryHandle;
				}
			}
			return true;
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x0600285D RID: 10333 RVA: 0x0007B970 File Offset: 0x0007A970
		private static CultureTableRecord.AdjustedSyntheticCultureName[] AdjustedSyntheticNames
		{
			get
			{
				if (CultureTableRecord.s_adjustedSyntheticNames == null)
				{
					CultureTableRecord.s_adjustedSyntheticNames = new CultureTableRecord.AdjustedSyntheticCultureName[]
					{
						new CultureTableRecord.AdjustedSyntheticCultureName(5146, "bs", "BA", "bs-Latn-BA"),
						new CultureTableRecord.AdjustedSyntheticCultureName(9275, "smn", "FI", "smn-FI"),
						new CultureTableRecord.AdjustedSyntheticCultureName(4155, "smj", "NO", "smj-NO"),
						new CultureTableRecord.AdjustedSyntheticCultureName(5179, "smj", "SE", "smj-SE"),
						new CultureTableRecord.AdjustedSyntheticCultureName(8251, "sms", "FI", "sms-FI"),
						new CultureTableRecord.AdjustedSyntheticCultureName(6203, "sma", "NO", "sma-NO"),
						new CultureTableRecord.AdjustedSyntheticCultureName(7227, "sma", "SE", "sma-SE"),
						new CultureTableRecord.AdjustedSyntheticCultureName(1131, "quz", "BO", "quz-BO"),
						new CultureTableRecord.AdjustedSyntheticCultureName(2155, "quz", "EC", "quz-EC"),
						new CultureTableRecord.AdjustedSyntheticCultureName(3179, "quz", "PE", "quz-PE")
					};
				}
				return CultureTableRecord.s_adjustedSyntheticNames;
			}
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x0007BAB4 File Offset: 0x0007AAB4
		internal static Hashtable GetNamesHashtable()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["bs-ba"] = "";
			hashtable["tg-tj"] = "";
			hashtable["mn-cn"] = "";
			hashtable["iu-ca"] = "";
			return hashtable;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x0007BB08 File Offset: 0x0007AB08
		internal static void GetAdjustedNames(int lcid, out CultureTableRecord.AdjustedSyntheticCultureName adjustedNames)
		{
			for (int i = 0; i < CultureTableRecord.AdjustedSyntheticNames.Length; i++)
			{
				if (CultureTableRecord.AdjustedSyntheticNames[i].lcid == lcid)
				{
					adjustedNames = CultureTableRecord.AdjustedSyntheticNames[i];
					return;
				}
			}
			adjustedNames = null;
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x0007BB44 File Offset: 0x0007AB44
		private unsafe uint FillCultureDataMemory(int cultureID, ref CultureData data, ref CultureTableRecord.CompositeCultureData compositeData)
		{
			uint num = 0U;
			Hashtable hashtable = new Hashtable(30);
			this.m_pPool[num] = 0;
			num += 1U;
			this.SetPoolString("", hashtable, ref num);
			hashtable[""] = 0U;
			this.m_pData->iLanguage = (ushort)cultureID;
			this.m_pData->sName = (uint)((ushort)this.SetPoolString(compositeData.sname, hashtable, ref num));
			this.m_pData->iDigits = (ushort)data.iDigits;
			this.m_pData->iNegativeNumber = (ushort)data.iNegativeNumber;
			this.m_pData->iCurrencyDigits = (ushort)data.iCurrencyDigits;
			this.m_pData->iCurrency = (ushort)data.iCurrency;
			this.m_pData->iNegativeCurrency = (ushort)data.iNegativeCurrency;
			this.m_pData->iLeadingZeros = (ushort)data.iLeadingZeros;
			this.m_pData->iFlags = 1;
			this.m_pData->iFirstDayOfWeek = this.ConvertFirstDayOfWeekMonToSun(data.iFirstDayOfWeek);
			this.m_pData->iFirstWeekOfYear = (ushort)data.iFirstWeekOfYear;
			this.m_pData->iCountry = (ushort)data.iCountry;
			this.m_pData->iMeasure = (ushort)data.iMeasure;
			this.m_pData->iDigitSubstitution = (ushort)data.iDigitSubstitution;
			this.m_pData->waGrouping = (uint)((ushort)this.SetPoolString(data.waGrouping, hashtable, ref num));
			this.m_pData->waMonetaryGrouping = (uint)((ushort)this.SetPoolString(data.waMonetaryGrouping, hashtable, ref num));
			this.m_pData->sListSeparator = (uint)((ushort)this.SetPoolString(data.sListSeparator, hashtable, ref num));
			this.m_pData->sDecimalSeparator = (uint)((ushort)this.SetPoolString(data.sDecimalSeparator, hashtable, ref num));
			this.m_pData->sThousandSeparator = (uint)((ushort)this.SetPoolString(data.sThousandSeparator, hashtable, ref num));
			this.m_pData->sCurrency = (uint)((ushort)this.SetPoolString(data.sCurrency, hashtable, ref num));
			this.m_pData->sMonetaryDecimal = (uint)((ushort)this.SetPoolString(data.sMonetaryDecimal, hashtable, ref num));
			this.m_pData->sMonetaryThousand = (uint)((ushort)this.SetPoolString(data.sMonetaryThousand, hashtable, ref num));
			this.m_pData->sPositiveSign = (uint)((ushort)this.SetPoolString(data.sPositiveSign, hashtable, ref num));
			this.m_pData->sNegativeSign = (uint)((ushort)this.SetPoolString(data.sNegativeSign, hashtable, ref num));
			this.m_pData->sAM1159 = (uint)((ushort)this.SetPoolString(data.sAM1159, hashtable, ref num));
			this.m_pData->sPM2359 = (uint)((ushort)this.SetPoolString(data.sPM2359, hashtable, ref num));
			this.m_pData->saNativeDigits = (uint)((ushort)this.SetPoolStringArrayFromSingleString(data.saNativeDigits, hashtable, ref num));
			this.m_pData->saTimeFormat = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saTimeFormat));
			this.m_pData->saShortDate = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saShortDate));
			this.m_pData->saLongDate = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saLongDate));
			this.m_pData->saYearMonth = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saYearMonth));
			this.m_pData->saDuration = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, new string[]
			{
				""
			}));
			this.m_pData->iDefaultLanguage = this.m_pData->iLanguage;
			this.m_pData->iDefaultAnsiCodePage = (ushort)data.iDefaultAnsiCodePage;
			this.m_pData->iDefaultOemCodePage = (ushort)data.iDefaultOemCodePage;
			this.m_pData->iDefaultMacCodePage = (ushort)data.iDefaultMacCodePage;
			this.m_pData->iDefaultEbcdicCodePage = (ushort)data.iDefaultEbcdicCodePage;
			this.m_pData->iGeoId = (ushort)data.iGeoId;
			this.m_pData->iPaperSize = (ushort)data.iPaperSize;
			this.m_pData->iIntlCurrencyDigits = (ushort)data.iIntlCurrencyDigits;
			this.m_pData->iParent = (ushort)compositeData.parentLcid;
			this.m_pData->waCalendars = (uint)((ushort)this.SetPoolString(compositeData.waCalendars, hashtable, ref num));
			this.m_pData->sAbbrevLang = (uint)((ushort)this.SetPoolString(data.sAbbrevLang, hashtable, ref num));
			this.m_pData->sISO639Language = (uint)((ushort)this.SetPoolString(data.sIso639Language, hashtable, ref num));
			this.m_pData->sEnglishLanguage = (uint)((ushort)this.SetPoolString(data.sEnglishLanguage, hashtable, ref num));
			this.m_pData->sNativeLanguage = (uint)((ushort)this.SetPoolString(data.sNativeLanguage, hashtable, ref num));
			this.m_pData->sEnglishCountry = (uint)((ushort)this.SetPoolString(data.sEnglishCountry, hashtable, ref num));
			this.m_pData->sNativeCountry = (uint)((ushort)this.SetPoolString(data.sNativeCountry, hashtable, ref num));
			this.m_pData->sAbbrevCountry = (uint)((ushort)this.SetPoolString(data.sAbbrevCountry, hashtable, ref num));
			this.m_pData->sISO3166CountryName = (uint)((ushort)this.SetPoolString(data.sIso3166CountryName, hashtable, ref num));
			this.m_pData->sIntlMonetarySymbol = (uint)((ushort)this.SetPoolString(data.sIntlMonetarySymbol, hashtable, ref num));
			this.m_pData->sEnglishCurrency = (uint)((ushort)this.SetPoolString(data.sEnglishCurrency, hashtable, ref num));
			this.m_pData->sNativeCurrency = (uint)((ushort)this.SetPoolString(data.sNativeCurrency, hashtable, ref num));
			this.m_pData->waFontSignature = (uint)((ushort)this.SetPoolString(data.waFontSignature, hashtable, ref num));
			this.m_pData->sISO639Language2 = (uint)((ushort)this.SetPoolString(data.sISO639Language2, hashtable, ref num));
			this.m_pData->sISO3166CountryName2 = (uint)((ushort)this.SetPoolString(data.sISO3166CountryName2, hashtable, ref num));
			this.m_pData->sParent = (uint)((ushort)this.SetPoolString(compositeData.parentName, hashtable, ref num));
			this.m_pData->saDayNames = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saDayNames));
			this.m_pData->saAbbrevDayNames = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saAbbrevDayNames));
			this.m_pData->saMonthNames = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saMonthNames));
			this.m_pData->saAbbrevMonthNames = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saAbbrevMonthNames));
			this.m_pData->saMonthGenitiveNames = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saGenitiveMonthNames));
			this.m_pData->saAbbrevMonthGenitiveNames = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saAbbrevGenitiveMonthNames));
			this.m_pData->saNativeCalendarNames = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saNativeCalendarNames));
			this.m_pData->saAltSortID = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, new string[]
			{
				""
			}));
			this.m_pData->iNegativePercent = (ushort)CultureInfo.InvariantCulture.NumberFormat.PercentNegativePattern;
			this.m_pData->iPositivePercent = (ushort)CultureInfo.InvariantCulture.NumberFormat.PercentPositivePattern;
			this.m_pData->iFormatFlags = 0;
			this.m_pData->iLineOrientations = 0;
			this.m_pData->iTextInfo = this.m_pData->iLanguage;
			this.m_pData->iInputLanguageHandle = this.m_pData->iLanguage;
			this.m_pData->iCompareInfo = (uint)this.m_pData->iLanguage;
			this.m_pData->sEnglishDisplayName = (uint)((ushort)this.SetPoolString(compositeData.englishDisplayName, hashtable, ref num));
			this.m_pData->sNativeDisplayName = (uint)((ushort)this.SetPoolString(compositeData.sNativeDisplayName, hashtable, ref num));
			this.m_pData->sPercent = (uint)((ushort)this.SetPoolString(CultureInfo.InvariantCulture.NumberFormat.PercentSymbol, hashtable, ref num));
			this.m_pData->sNaN = (uint)((ushort)this.SetPoolString(data.sNaN, hashtable, ref num));
			this.m_pData->sPositiveInfinity = (uint)((ushort)this.SetPoolString(data.sPositiveInfinity, hashtable, ref num));
			this.m_pData->sNegativeInfinity = (uint)((ushort)this.SetPoolString(data.sNegativeInfinity, hashtable, ref num));
			this.m_pData->sMonthDay = (uint)((ushort)this.SetPoolString(CultureInfo.InvariantCulture.DateTimeFormat.MonthDayPattern, hashtable, ref num));
			this.m_pData->sAdEra = (uint)((ushort)this.SetPoolString(CultureInfo.InvariantCulture.DateTimeFormat.GetEraName(0), hashtable, ref num));
			this.m_pData->sAbbrevAdEra = (uint)((ushort)this.SetPoolString(CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedEraName(0), hashtable, ref num));
			this.m_pData->sRegionName = this.m_pData->sISO3166CountryName;
			this.m_pData->sConsoleFallbackName = (uint)((ushort)this.SetPoolString(compositeData.consoleFallbackName, hashtable, ref num));
			this.m_pData->saShortTime = this.m_pData->saTimeFormat;
			this.m_pData->saSuperShortDayNames = (uint)((ushort)this.SetPoolStringArray(hashtable, ref num, data.saSuperShortDayNames));
			this.m_pData->saDateWords = this.m_pData->saDuration;
			this.m_pData->sSpecificCulture = this.m_pData->sName;
			this.m_pData->sScripts = 0U;
			return 2U * num;
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x0007C3FC File Offset: 0x0007B3FC
		private unsafe uint SetPoolString(string s, Hashtable offsetTable, ref uint currentOffset)
		{
			uint result = currentOffset;
			if (offsetTable[s] == null)
			{
				offsetTable[s] = currentOffset;
				this.m_pPool[currentOffset] = (ushort)s.Length;
				currentOffset += 1U;
				for (int i = 0; i < s.Length; i++)
				{
					this.m_pPool[currentOffset] = (ushort)s[i];
					currentOffset += 1U;
				}
				if ((currentOffset & 1U) == 0U)
				{
					this.m_pPool[currentOffset] = 0;
					currentOffset += 1U;
				}
				return result;
			}
			return (uint)offsetTable[s];
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x0007C490 File Offset: 0x0007B490
		private unsafe uint SetPoolStringArray(Hashtable offsetTable, ref uint currentOffset, params string[] array)
		{
			uint[] array2 = new uint[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = this.SetPoolString(array[i], offsetTable, ref currentOffset);
			}
			uint result = currentOffset;
			this.m_pPool[currentOffset] = (ushort)array2.Length;
			currentOffset += 1U;
			uint* ptr = (uint*)(this.m_pPool + currentOffset);
			for (int j = 0; j < array2.Length; j++)
			{
				ptr[j] = array2[j];
				currentOffset += 2U;
			}
			if ((currentOffset & 1U) == 0U)
			{
				this.m_pPool[currentOffset] = 0;
				currentOffset += 1U;
			}
			return result;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x0007C528 File Offset: 0x0007B528
		private uint SetPoolStringArrayFromSingleString(string s, Hashtable offsetTable, ref uint currentOffset)
		{
			string[] array = new string[s.Length];
			for (int i = 0; i < s.Length; i++)
			{
				array[i] = s.Substring(i, 1);
			}
			return this.SetPoolStringArray(offsetTable, ref currentOffset, array);
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x0007C568 File Offset: 0x0007B568
		private bool NameHasScriptTag(string tempName)
		{
			int num = 0;
			int num2 = 0;
			while (num2 < tempName.Length && num < 2)
			{
				if (tempName[num2] == '-')
				{
					num++;
				}
				num2++;
			}
			return num > 1;
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x0007C5A0 File Offset: 0x0007B5A0
		private static string GetCasedName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			int i = 0;
			while (i < name.Length && name[i] != '-')
			{
				stringBuilder.Append(name[i]);
				i++;
			}
			stringBuilder.Append("-");
			i++;
			char value = char.ToUpper(name[i], CultureInfo.InvariantCulture);
			stringBuilder.Append(value);
			i++;
			while (i < name.Length && name[i] != '-')
			{
				stringBuilder.Append(name[i]);
				i++;
			}
			stringBuilder.Append("-");
			for (i++; i < name.Length; i++)
			{
				value = char.ToUpper(name[i], CultureInfo.InvariantCulture);
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x0007C678 File Offset: 0x0007B678
		private static string GetQualifiedName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			int i = 0;
			while (i < name.Length && name[i] != '-')
			{
				stringBuilder.Append(name[i]);
				i++;
			}
			stringBuilder.Append("--");
			i++;
			int num = i;
			while (i < name.Length && name[i] != '-')
			{
				stringBuilder.Append(name[i]);
				i++;
			}
			for (i++; i < name.Length; i++)
			{
				stringBuilder.Insert(num, name[i]);
				num++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x0007C724 File Offset: 0x0007B724
		private static void GetSyntheticParentData(ref CultureData data, ref CultureTableRecord.CompositeCultureData compositeData)
		{
			compositeData.parentLcid = CultureInfo.InvariantCulture.LCID;
			compositeData.parentName = CultureInfo.InvariantCulture.Name;
			if (data.sParentName != null)
			{
				string text = CultureTableRecord.ValidateCulturePieceToLower(data.sParentName, "ParentName", 84);
				int parentLcid;
				string parentName;
				int dataItemFromCultureName = CultureTable.Default.GetDataItemFromCultureName(text, out parentLcid, out parentName);
				if (dataItemFromCultureName >= 0)
				{
					compositeData.parentLcid = parentLcid;
					compositeData.parentName = parentName;
					return;
				}
				if (CultureTableRecord.SyntheticNameToLcidCache[text] != null)
				{
					compositeData.parentLcid = (int)CultureTableRecord.SyntheticNameToLcidCache[text];
					compositeData.parentName = data.sParentName;
				}
			}
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x0007C7C0 File Offset: 0x0007B7C0
		private static void GetSyntheticConsoleFallback(ref CultureData data, ref CultureTableRecord.CompositeCultureData compositeData)
		{
			compositeData.consoleFallbackName = CultureInfo.InvariantCulture.GetConsoleFallbackUICulture().Name;
			if (data.sConsoleFallbackName != null)
			{
				string text = CultureTableRecord.ValidateCulturePieceToLower(data.sConsoleFallbackName, "ConsoleFallbackName", 84);
				int num;
				string consoleFallbackName;
				int dataItemFromCultureName = CultureTable.Default.GetDataItemFromCultureName(text, out num, out consoleFallbackName);
				if (dataItemFromCultureName >= 0)
				{
					compositeData.consoleFallbackName = consoleFallbackName;
					return;
				}
				if (CultureTableRecord.SyntheticNameToLcidCache[text] != null)
				{
					compositeData.consoleFallbackName = data.sConsoleFallbackName;
				}
			}
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x0007C834 File Offset: 0x0007B834
		private int GetCultureDataSize(int cultureID, ref CultureData data, ref CultureTableRecord.CompositeCultureData compositeData)
		{
			int num = sizeof(CultureTableData);
			Hashtable offsetTable = new Hashtable(30);
			num += 2;
			num += this.GetPoolStringSize("", offsetTable);
			compositeData.sname = CultureInfo.nativeGetCultureName(cultureID, true, false);
			if (compositeData.sname == null)
			{
				CultureTableRecord.AdjustedSyntheticCultureName adjustedSyntheticCultureName;
				CultureTableRecord.GetAdjustedNames(cultureID, out adjustedSyntheticCultureName);
				if (adjustedSyntheticCultureName != null)
				{
					data.sIso639Language = adjustedSyntheticCultureName.isoLanguage;
					data.sIso3166CountryName = adjustedSyntheticCultureName.isoCountry;
					compositeData.sname = adjustedSyntheticCultureName.sName;
				}
				else
				{
					string text = (string)CultureTableRecord.SyntheticLcidToNameCache[cultureID];
					if (this.NameHasScriptTag(text))
					{
						compositeData.sname = CultureTableRecord.GetCasedName(text);
					}
					else
					{
						compositeData.sname = data.sIso639Language + "-" + data.sIso3166CountryName;
					}
				}
			}
			compositeData.englishDisplayName = data.sEnglishLanguage + " (" + data.sEnglishCountry + ")";
			compositeData.sNativeDisplayName = data.sNativeLanguage + " (" + data.sNativeCountry + ")";
			CultureTableRecord.AdjustSyntheticCalendars(ref data, ref compositeData);
			num += this.GetPoolStringSize(compositeData.sname, offsetTable);
			num += this.GetPoolStringSize(compositeData.englishDisplayName, offsetTable);
			num += this.GetPoolStringSize(compositeData.sNativeDisplayName, offsetTable);
			num += this.GetPoolStringSize(compositeData.waCalendars, offsetTable);
			CultureTableRecord.GetSyntheticParentData(ref data, ref compositeData);
			num += this.GetPoolStringSize(compositeData.parentName, offsetTable);
			num += this.GetPoolStringSize(data.sIso639Language, offsetTable);
			num += this.GetPoolStringSize(data.sListSeparator, offsetTable);
			num += this.GetPoolStringSize(data.sDecimalSeparator, offsetTable);
			num += this.GetPoolStringSize(data.sThousandSeparator, offsetTable);
			num += this.GetPoolStringSize(data.sCurrency, offsetTable);
			num += this.GetPoolStringSize(data.sMonetaryDecimal, offsetTable);
			num += this.GetPoolStringSize(data.sMonetaryThousand, offsetTable);
			num += this.GetPoolStringSize(data.sPositiveSign, offsetTable);
			num += this.GetPoolStringSize(data.sNegativeSign, offsetTable);
			num += this.GetPoolStringSize(data.sAM1159, offsetTable);
			num += this.GetPoolStringSize(data.sPM2359, offsetTable);
			num += this.GetPoolStringSize(data.sAbbrevLang, offsetTable);
			num += this.GetPoolStringSize(data.sEnglishLanguage, offsetTable);
			num += this.GetPoolStringSize(data.sNativeLanguage, offsetTable);
			num += this.GetPoolStringSize(data.sEnglishCountry, offsetTable);
			num += this.GetPoolStringSize(data.sNativeCountry, offsetTable);
			num += this.GetPoolStringSize(data.sAbbrevCountry, offsetTable);
			num += this.GetPoolStringSize(data.sIso3166CountryName, offsetTable);
			num += this.GetPoolStringSize(data.sIntlMonetarySymbol, offsetTable);
			num += this.GetPoolStringSize(data.sEnglishCurrency, offsetTable);
			num += this.GetPoolStringSize(data.sNativeCurrency, offsetTable);
			num += this.GetPoolStringSize(CultureInfo.InvariantCulture.NumberFormat.PercentSymbol, offsetTable);
			if (data.sNaN == null)
			{
				data.sNaN = CultureInfo.InvariantCulture.NumberFormat.NaNSymbol;
			}
			num += this.GetPoolStringSize(data.sNaN, offsetTable);
			if (data.sPositiveInfinity == null)
			{
				data.sPositiveInfinity = CultureInfo.InvariantCulture.NumberFormat.PositiveInfinitySymbol;
			}
			num += this.GetPoolStringSize(data.sPositiveInfinity, offsetTable);
			if (data.sNegativeInfinity == null)
			{
				data.sNegativeInfinity = CultureInfo.InvariantCulture.NumberFormat.NegativeInfinitySymbol;
			}
			num += this.GetPoolStringSize(data.sNegativeInfinity, offsetTable);
			num += this.GetPoolStringSize(CultureInfo.InvariantCulture.DateTimeFormat.MonthDayPattern, offsetTable);
			num += this.GetPoolStringSize(CultureInfo.InvariantCulture.DateTimeFormat.GetEraName(0), offsetTable);
			num += this.GetPoolStringSize(CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedEraName(0), offsetTable);
			CultureTableRecord.GetSyntheticConsoleFallback(ref data, ref compositeData);
			num += this.GetPoolStringSize(compositeData.consoleFallbackName, offsetTable);
			num += this.GetPoolStringArraySize(offsetTable, data.saMonthNames);
			num += this.GetPoolStringArraySize(offsetTable, data.saDayNames);
			num += this.GetPoolStringArraySize(offsetTable, data.saAbbrevDayNames);
			num += this.GetPoolStringArraySize(offsetTable, data.saAbbrevMonthNames);
			data.saGenitiveMonthNames[12] = data.saMonthNames[12];
			num += this.GetPoolStringArraySize(offsetTable, data.saGenitiveMonthNames);
			data.saAbbrevGenitiveMonthNames[12] = data.saAbbrevMonthNames[12];
			num += this.GetPoolStringArraySize(offsetTable, data.saAbbrevGenitiveMonthNames);
			num += this.GetPoolStringArraySize(offsetTable, data.saNativeCalendarNames);
			num += this.GetPoolStringArraySize(offsetTable, data.saTimeFormat);
			num += this.GetPoolStringArraySize(offsetTable, data.saShortDate);
			num += this.GetPoolStringArraySize(offsetTable, data.saLongDate);
			num += this.GetPoolStringArraySize(offsetTable, data.saYearMonth);
			num += this.GetPoolStringArraySize(offsetTable, new string[]
			{
				""
			});
			num += this.GetPoolStringArraySize(offsetTable, new string[]
			{
				""
			});
			data.waGrouping = this.GroupSizesConstruction(data.waGrouping);
			num += this.GetPoolStringSize(data.waGrouping, offsetTable);
			data.waMonetaryGrouping = this.GroupSizesConstruction(data.waMonetaryGrouping);
			num += this.GetPoolStringSize(data.waMonetaryGrouping, offsetTable);
			num += this.GetPoolStringArraySize(data.saNativeDigits, offsetTable);
			num += this.GetPoolStringSize(data.waFontSignature, offsetTable);
			if (data.sISO3166CountryName2 == null)
			{
				data.sISO3166CountryName2 = data.sIso3166CountryName;
			}
			num += this.GetPoolStringSize(data.sISO3166CountryName2, offsetTable);
			if (data.sISO639Language2 == null)
			{
				data.sISO639Language2 = data.sIso639Language;
			}
			num += this.GetPoolStringSize(data.sISO639Language2, offsetTable);
			if (data.saSuperShortDayNames == null)
			{
				data.saSuperShortDayNames = data.saAbbrevDayNames;
			}
			return num + this.GetPoolStringArraySize(offsetTable, data.saSuperShortDayNames);
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x0007CDC8 File Offset: 0x0007BDC8
		private int GetPoolStringSize(string s, Hashtable offsetTable)
		{
			int result = 0;
			if (offsetTable[s] == null)
			{
				offsetTable[s] = "";
				result = 2 * (s.Length + 1 + (1 - (s.Length & 1)));
			}
			return result;
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x0007CE04 File Offset: 0x0007BE04
		private int GetPoolStringArraySize(string s, Hashtable offsetTable)
		{
			string[] array = new string[s.Length];
			for (int i = 0; i < s.Length; i++)
			{
				array[i] = s.Substring(i, 1);
			}
			return this.GetPoolStringArraySize(offsetTable, array);
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x0007CE44 File Offset: 0x0007BE44
		private int GetPoolStringArraySize(Hashtable offsetTable, params string[] array)
		{
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num += this.GetPoolStringSize(array[i], offsetTable);
			}
			return num + 2 * (array.Length * 2 + 1 + 1);
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x0007CE80 File Offset: 0x0007BE80
		private string GroupSizesConstruction(string rawGroupSize)
		{
			int num = rawGroupSize.Length;
			if (rawGroupSize[num - 1] == '0')
			{
				num--;
			}
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			while (i < num)
			{
				stringBuilder.Append(rawGroupSize[i] - '0');
				i++;
				if (i < num)
				{
					i++;
				}
			}
			if (num == rawGroupSize.Length)
			{
				stringBuilder.Append('\0');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x0600286E RID: 10350 RVA: 0x0007CEEA File Offset: 0x0007BEEA
		private string WindowsPath
		{
			get
			{
				if (this.m_windowsPath == null)
				{
					this.m_windowsPath = CultureInfo.nativeGetWindowsDirectory();
				}
				return this.m_windowsPath;
			}
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x0007CF08 File Offset: 0x0007BF08
		private string GetCustomCultureFile(string name)
		{
			StringBuilder stringBuilder = new StringBuilder(this.WindowsPath);
			stringBuilder.Append("\\Globalization\\");
			stringBuilder.Append(name);
			stringBuilder.Append(".nlp");
			string text = stringBuilder.ToString();
			bool flag = CultureInfo.nativeFileExists(text);
			if (flag)
			{
				return text;
			}
			return null;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x0007CF58 File Offset: 0x0007BF58
		private static string ValidateCulturePieceToLower(string testString, string paramName, int maxLength)
		{
			if (testString.Length > maxLength)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NameTooLong"), new object[]
				{
					testString,
					maxLength
				}), paramName);
			}
			StringBuilder stringBuilder = new StringBuilder(testString.Length);
			foreach (char c in testString)
			{
				if (c <= 'Z' && c >= 'A')
				{
					stringBuilder.Append(c - 'A' + 'a');
				}
				else
				{
					if ((c > 'z' || c < 'a') && (c > '9' || c < '0') && c != '_' && c != '-')
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NameContainsInvalidCharacters"), new object[]
						{
							testString
						}), paramName);
					}
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x0007D03C File Offset: 0x0007C03C
		internal static string AnsiToLower(string testString)
		{
			StringBuilder stringBuilder = new StringBuilder(testString.Length);
			foreach (char c in testString)
			{
				stringBuilder.Append((c <= 'Z' && c >= 'A') ? (c - 'A' + 'a') : c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002872 RID: 10354 RVA: 0x0007D090 File Offset: 0x0007C090
		internal bool IsSynthetic
		{
			get
			{
				return this.m_synthetic;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002873 RID: 10355 RVA: 0x0007D098 File Offset: 0x0007C098
		internal bool IsCustomCulture
		{
			get
			{
				return !this.m_CultureTable.fromAssembly;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002874 RID: 10356 RVA: 0x0007D0A8 File Offset: 0x0007C0A8
		internal bool IsReplacementCulture
		{
			get
			{
				return this.IsCustomCulture && !CultureTableRecord.IsCustomCultureId(this.m_CultureID);
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002875 RID: 10357 RVA: 0x0007D0C2 File Offset: 0x0007C0C2
		internal int CultureID
		{
			get
			{
				return this.m_CultureID;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002876 RID: 10358 RVA: 0x0007D0CA File Offset: 0x0007C0CA
		// (set) Token: 0x06002877 RID: 10359 RVA: 0x0007D0D2 File Offset: 0x0007C0D2
		internal string CultureName
		{
			get
			{
				return this.m_CultureName;
			}
			set
			{
				this.m_CultureName = value;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x0007D0DB File Offset: 0x0007C0DB
		internal bool UseUserOverride
		{
			get
			{
				return this.m_bUseUserOverride;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x0007D0E4 File Offset: 0x0007C0E4
		internal unsafe bool UseGetLocaleInfo
		{
			get
			{
				if (!this.m_bUseUserOverride)
				{
					return false;
				}
				int num;
				CultureInfo.nativeGetUserDefaultLCID(&num, 1024);
				if (this.ActualCultureID == 4096 && num == 3072)
				{
					return this.SNAME.Equals(CultureInfo.nativeGetCultureName(num, true, false));
				}
				return this.ActualCultureID == num;
			}
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x0007D140 File Offset: 0x0007C140
		internal bool UseCurrentCalendar(int calID)
		{
			return this.UseGetLocaleInfo && CultureInfo.nativeGetCurrentCalendar() == calID;
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x0007D154 File Offset: 0x0007C154
		internal bool IsValidSortID(int sortID)
		{
			return sortID == 0 || (this.SALTSORTID != null && this.SALTSORTID.Length >= sortID && this.SALTSORTID[sortID - 1].Length != 0);
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x0007D180 File Offset: 0x0007C180
		internal CultureTableRecord CloneWithUserOverride(bool userOverride)
		{
			if (this.m_bUseUserOverride == userOverride)
			{
				return this;
			}
			CultureTableRecord cultureTableRecord = (CultureTableRecord)base.MemberwiseClone();
			cultureTableRecord.m_bUseUserOverride = userOverride;
			return cultureTableRecord;
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x0007D1AC File Offset: 0x0007C1AC
		internal unsafe string CultureNativeDisplayName
		{
			get
			{
				int culture;
				CultureInfo.nativeGetUserDefaultUILanguage(&culture);
				if (CultureInfo.GetLangID(culture) == CultureInfo.GetLangID(CultureInfo.CurrentUICulture.LCID))
				{
					string text = CultureInfo.nativeGetLocaleInfo(this.m_ActualCultureID, 2);
					if (text != null)
					{
						if (text[text.Length - 1] == '\0')
						{
							return text.Substring(0, text.Length - 1);
						}
						return text;
					}
				}
				return this.SNATIVEDISPLAYNAME;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600287E RID: 10366 RVA: 0x0007D214 File Offset: 0x0007C214
		internal unsafe string RegionNativeDisplayName
		{
			get
			{
				int culture;
				CultureInfo.nativeGetUserDefaultUILanguage(&culture);
				if (CultureInfo.GetLangID(culture) == CultureInfo.GetLangID(CultureInfo.CurrentUICulture.LCID))
				{
					string text = CultureInfo.nativeGetLocaleInfo(this.m_ActualCultureID, 6);
					if (text != null)
					{
						if (text[text.Length - 1] == '\0')
						{
							return text.Substring(0, text.Length - 1);
						}
						return text;
					}
				}
				return this.SNATIVECOUNTRY;
			}
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x0007D27C File Offset: 0x0007C27C
		public override bool Equals(object value)
		{
			CultureTableRecord cultureTableRecord = value as CultureTableRecord;
			return cultureTableRecord != null && (this.m_pData == cultureTableRecord.m_pData && this.m_bUseUserOverride == cultureTableRecord.m_bUseUserOverride && this.m_CultureID == cultureTableRecord.m_CultureID && CultureInfo.InvariantCulture.CompareInfo.Compare(this.m_CultureName, cultureTableRecord.m_CultureName, CompareOptions.IgnoreCase) == 0) && this.m_CultureTable.Equals(cultureTableRecord.m_CultureTable);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x0007D2F0 File Offset: 0x0007C2F0
		public override int GetHashCode()
		{
			if (!CultureTableRecord.IsCustomCultureId(this.m_CultureID))
			{
				return this.m_CultureID;
			}
			return this.m_CultureName.GetHashCode();
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x0007D314 File Offset: 0x0007C314
		private unsafe string GetString(uint iOffset)
		{
			char* ptr = (char*)(this.m_pPool + iOffset);
			if (ptr[1] == '\0')
			{
				return string.Empty;
			}
			return new string(ptr + 1, 0, (int)(*ptr));
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002882 RID: 10370 RVA: 0x0007D346 File Offset: 0x0007C346
		private int InteropLCID
		{
			get
			{
				if (this.ActualCultureID != 4096)
				{
					return this.ActualCultureID;
				}
				return 3072;
			}
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x0007D364 File Offset: 0x0007C364
		private string GetOverrideString(uint iOffset, int iWindowsFlag)
		{
			if (this.UseGetLocaleInfo)
			{
				string text = CultureInfo.nativeGetLocaleInfo(this.InteropLCID, iWindowsFlag);
				if (text != null && text.Length > 0)
				{
					return text;
				}
			}
			return this.GetString(iOffset);
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x0007D39C File Offset: 0x0007C39C
		private unsafe string[] GetStringArray(uint iOffset)
		{
			if (iOffset == 0U)
			{
				return new string[0];
			}
			ushort* ptr = this.m_pPool + iOffset;
			int num = (int)(*ptr);
			string[] array = new string[num];
			uint* ptr2 = (uint*)(ptr + 1);
			for (int i = 0; i < num; i++)
			{
				array[i] = this.GetString(ptr2[i]);
			}
			return array;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x0007D3F4 File Offset: 0x0007C3F4
		private unsafe string GetStringArrayDefault(uint iOffset)
		{
			if (iOffset == 0U)
			{
				return string.Empty;
			}
			ushort* ptr = this.m_pPool + iOffset;
			uint* ptr2 = (uint*)(ptr + 1);
			return this.GetString(*ptr2);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x0007D424 File Offset: 0x0007C424
		private string GetOverrideStringArrayDefault(uint iOffset, int iWindowsFlag)
		{
			if (this.UseGetLocaleInfo)
			{
				string text = CultureInfo.nativeGetLocaleInfo(this.InteropLCID, iWindowsFlag);
				if (text != null && text.Length > 0)
				{
					return text;
				}
			}
			return this.GetStringArrayDefault(iOffset);
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x0007D45C File Offset: 0x0007C45C
		private ushort GetOverrideUSHORT(ushort iData, int iWindowsFlag)
		{
			if (this.UseGetLocaleInfo)
			{
				string text = CultureInfo.nativeGetLocaleInfo(this.InteropLCID, iWindowsFlag);
				short num;
				if (text != null && text.Length > 0 && short.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out num))
				{
					return (ushort)num;
				}
			}
			return iData;
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x0007D4A0 File Offset: 0x0007C4A0
		private unsafe int[] GetWordArray(uint iData)
		{
			if (iData == 0U)
			{
				return new int[0];
			}
			ushort* ptr = this.m_pPool + iData;
			int num = (int)(*ptr);
			int[] array = new int[num];
			ptr++;
			for (int i = 0; i < num; i++)
			{
				array[i] = (int)ptr[i];
			}
			return array;
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x0007D4EC File Offset: 0x0007C4EC
		private int[] GetOverrideGrouping(uint iData, int iWindowsFlag)
		{
			if (this.UseGetLocaleInfo)
			{
				string text = CultureInfo.nativeGetLocaleInfo(this.InteropLCID, iWindowsFlag);
				if (text != null && text.Length > 0)
				{
					int[] array = CultureTableRecord.ConvertWin32GroupString(text);
					if (array != null)
					{
						return array;
					}
				}
			}
			return this.GetWordArray(iData);
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x0007D52D File Offset: 0x0007C52D
		internal int ActualCultureID
		{
			get
			{
				if (this.m_ActualCultureID == 0)
				{
					this.m_ActualCultureID = (int)this.ILANGUAGE;
				}
				return this.m_ActualCultureID;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x0007D549 File Offset: 0x0007C549
		internal string ActualName
		{
			get
			{
				if (this.m_ActualName == null)
				{
					this.m_ActualName = this.SNAME;
				}
				return this.m_ActualName;
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x0007D565 File Offset: 0x0007C565
		internal bool IsNeutralCulture
		{
			get
			{
				return (this.IFLAGS & 1U) == 0U;
			}
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x0007D574 File Offset: 0x0007C574
		private bool IsOptionalCalendar(int calendarId)
		{
			for (int i = 0; i < this.IOPTIONALCALENDARS.Length; i++)
			{
				if (this.IOPTIONALCALENDARS[i] == calendarId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x0600288E RID: 10382 RVA: 0x0007D5A2 File Offset: 0x0007C5A2
		internal unsafe ushort IDIGITS
		{
			get
			{
				return this.m_pData->iDigits;
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x0007D5AF File Offset: 0x0007C5AF
		internal unsafe ushort INEGNUMBER
		{
			get
			{
				return this.m_pData->iNegativeNumber;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002890 RID: 10384 RVA: 0x0007D5BC File Offset: 0x0007C5BC
		internal unsafe ushort ICURRDIGITS
		{
			get
			{
				return this.m_pData->iCurrencyDigits;
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002891 RID: 10385 RVA: 0x0007D5C9 File Offset: 0x0007C5C9
		internal unsafe ushort ICURRENCY
		{
			get
			{
				return this.m_pData->iCurrency;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002892 RID: 10386 RVA: 0x0007D5D6 File Offset: 0x0007C5D6
		internal unsafe ushort INEGCURR
		{
			get
			{
				return this.m_pData->iNegativeCurrency;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002893 RID: 10387 RVA: 0x0007D5E4 File Offset: 0x0007C5E4
		internal ushort ICALENDARTYPE
		{
			get
			{
				if (this.m_bUseUserOverride)
				{
					string text = CultureInfo.nativeGetLocaleInfo(this.ActualCultureID, 4105);
					short num;
					if (text != null && text.Length > 0 && short.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out num) && this.IsOptionalCalendar((int)num))
					{
						return (ushort)num;
					}
				}
				return (ushort)this.IOPTIONALCALENDARS[0];
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002894 RID: 10388 RVA: 0x0007D63B File Offset: 0x0007C63B
		internal unsafe ushort IFIRSTWEEKOFYEAR
		{
			get
			{
				return this.GetOverrideUSHORT(this.m_pData->iFirstWeekOfYear, 4109);
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x0007D653 File Offset: 0x0007C653
		internal unsafe ushort IMEASURE
		{
			get
			{
				return this.GetOverrideUSHORT(this.m_pData->iMeasure, 13);
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002896 RID: 10390 RVA: 0x0007D668 File Offset: 0x0007C668
		internal unsafe ushort IDIGITSUBSTITUTION
		{
			get
			{
				return this.GetOverrideUSHORT(this.m_pData->iDigitSubstitution, 4116);
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002897 RID: 10391 RVA: 0x0007D680 File Offset: 0x0007C680
		internal unsafe int[] SGROUPING
		{
			get
			{
				return this.GetOverrideGrouping(this.m_pData->waGrouping, 16);
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002898 RID: 10392 RVA: 0x0007D695 File Offset: 0x0007C695
		internal unsafe int[] SMONGROUPING
		{
			get
			{
				return this.GetOverrideGrouping(this.m_pData->waMonetaryGrouping, 24);
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x0007D6AA File Offset: 0x0007C6AA
		internal unsafe string SLIST
		{
			get
			{
				return this.GetOverrideString(this.m_pData->sListSeparator, 12);
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x0007D6BF File Offset: 0x0007C6BF
		internal unsafe string SDECIMAL
		{
			get
			{
				return this.GetString(this.m_pData->sDecimalSeparator);
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x0007D6D2 File Offset: 0x0007C6D2
		internal unsafe string STHOUSAND
		{
			get
			{
				return this.GetString(this.m_pData->sThousandSeparator);
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x0007D6E5 File Offset: 0x0007C6E5
		internal unsafe string SCURRENCY
		{
			get
			{
				return this.GetString(this.m_pData->sCurrency);
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600289D RID: 10397 RVA: 0x0007D6F8 File Offset: 0x0007C6F8
		internal unsafe string SMONDECIMALSEP
		{
			get
			{
				return this.GetString(this.m_pData->sMonetaryDecimal);
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x0007D70B File Offset: 0x0007C70B
		internal unsafe string SMONTHOUSANDSEP
		{
			get
			{
				return this.GetString(this.m_pData->sMonetaryThousand);
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600289F RID: 10399 RVA: 0x0007D71E File Offset: 0x0007C71E
		internal unsafe string SNEGATIVESIGN
		{
			get
			{
				return this.GetString(this.m_pData->sNegativeSign);
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x0007D731 File Offset: 0x0007C731
		internal unsafe string S1159
		{
			get
			{
				return this.GetString(this.m_pData->sAM1159);
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x0007D744 File Offset: 0x0007C744
		internal unsafe string S2359
		{
			get
			{
				return this.GetString(this.m_pData->sPM2359);
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x0007D757 File Offset: 0x0007C757
		internal unsafe string STIMEFORMAT
		{
			get
			{
				return CultureTableRecord.ReescapeWin32String(this.GetStringArrayDefault(this.m_pData->saTimeFormat));
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x0007D76F File Offset: 0x0007C76F
		internal unsafe string SSHORTTIME
		{
			get
			{
				return CultureTableRecord.ReescapeWin32String(this.GetStringArrayDefault(this.m_pData->saShortTime));
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060028A4 RID: 10404 RVA: 0x0007D787 File Offset: 0x0007C787
		internal unsafe string SSHORTDATE
		{
			get
			{
				return CultureTableRecord.ReescapeWin32String(this.GetStringArrayDefault(this.m_pData->saShortDate));
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060028A5 RID: 10405 RVA: 0x0007D79F File Offset: 0x0007C79F
		internal unsafe string SLONGDATE
		{
			get
			{
				return CultureTableRecord.ReescapeWin32String(this.GetStringArrayDefault(this.m_pData->saLongDate));
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060028A6 RID: 10406 RVA: 0x0007D7B7 File Offset: 0x0007C7B7
		internal unsafe string SYEARMONTH
		{
			get
			{
				return CultureTableRecord.ReescapeWin32String(this.GetStringArrayDefault(this.m_pData->saYearMonth));
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x0007D7CF File Offset: 0x0007C7CF
		internal unsafe string SMONTHDAY
		{
			get
			{
				return CultureTableRecord.ReescapeWin32String(this.GetString(this.m_pData->sMonthDay));
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x0007D7E7 File Offset: 0x0007C7E7
		internal unsafe string[] STIMEFORMATS
		{
			get
			{
				return CultureTableRecord.ReescapeWin32Strings(this.GetStringArray(this.m_pData->saTimeFormat));
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x0007D7FF File Offset: 0x0007C7FF
		internal unsafe string[] SSHORTTIMES
		{
			get
			{
				return CultureTableRecord.ReescapeWin32Strings(this.GetStringArray(this.m_pData->saShortTime));
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x060028AA RID: 10410 RVA: 0x0007D817 File Offset: 0x0007C817
		internal unsafe string[] SSHORTDATES
		{
			get
			{
				return CultureTableRecord.ReescapeWin32Strings(this.GetStringArray(this.m_pData->saShortDate));
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x060028AB RID: 10411 RVA: 0x0007D82F File Offset: 0x0007C82F
		internal unsafe string[] SLONGDATES
		{
			get
			{
				return CultureTableRecord.ReescapeWin32Strings(this.GetStringArray(this.m_pData->saLongDate));
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x0007D847 File Offset: 0x0007C847
		internal unsafe string[] SYEARMONTHS
		{
			get
			{
				return CultureTableRecord.ReescapeWin32Strings(this.GetStringArray(this.m_pData->saYearMonth));
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x0007D860 File Offset: 0x0007C860
		internal unsafe string[] SNATIVEDIGITS
		{
			get
			{
				string text;
				if (this.m_bUseUserOverride && this.CultureID != 3072 && (text = CultureInfo.nativeGetLocaleInfo(this.ActualCultureID, 19)) != null && text.Length == 10)
				{
					string[] array = new string[10];
					for (int i = 0; i < text.Length; i++)
					{
						array[i] = text[i].ToString(CultureInfo.InvariantCulture);
					}
					return array;
				}
				return this.GetStringArray(this.m_pData->saNativeDigits);
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060028AE RID: 10414 RVA: 0x0007D8E0 File Offset: 0x0007C8E0
		internal unsafe ushort ILANGUAGE
		{
			get
			{
				return this.m_pData->iLanguage;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060028AF RID: 10415 RVA: 0x0007D8ED File Offset: 0x0007C8ED
		internal unsafe ushort IDEFAULTANSICODEPAGE
		{
			get
			{
				return this.m_pData->iDefaultAnsiCodePage;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060028B0 RID: 10416 RVA: 0x0007D8FA File Offset: 0x0007C8FA
		internal unsafe ushort IDEFAULTOEMCODEPAGE
		{
			get
			{
				return this.m_pData->iDefaultOemCodePage;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060028B1 RID: 10417 RVA: 0x0007D907 File Offset: 0x0007C907
		internal unsafe ushort IDEFAULTMACCODEPAGE
		{
			get
			{
				return this.m_pData->iDefaultMacCodePage;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x0007D914 File Offset: 0x0007C914
		internal unsafe ushort IDEFAULTEBCDICCODEPAGE
		{
			get
			{
				return this.m_pData->iDefaultEbcdicCodePage;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x0007D921 File Offset: 0x0007C921
		internal unsafe ushort IGEOID
		{
			get
			{
				return this.m_pData->iGeoId;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x0007D92E File Offset: 0x0007C92E
		internal unsafe ushort INEGATIVEPERCENT
		{
			get
			{
				return this.m_pData->iNegativePercent;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x0007D93B File Offset: 0x0007C93B
		internal unsafe ushort IPOSITIVEPERCENT
		{
			get
			{
				return this.m_pData->iPositivePercent;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x0007D948 File Offset: 0x0007C948
		internal unsafe ushort IPARENT
		{
			get
			{
				return this.m_pData->iParent;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x0007D955 File Offset: 0x0007C955
		internal unsafe ushort ILINEORIENTATIONS
		{
			get
			{
				return this.m_pData->iLineOrientations;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060028B8 RID: 10424 RVA: 0x0007D962 File Offset: 0x0007C962
		internal unsafe uint ICOMPAREINFO
		{
			get
			{
				return this.m_pData->iCompareInfo;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060028B9 RID: 10425 RVA: 0x0007D96F File Offset: 0x0007C96F
		internal unsafe uint IFLAGS
		{
			get
			{
				return (uint)this.m_pData->iFlags;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060028BA RID: 10426 RVA: 0x0007D97C File Offset: 0x0007C97C
		internal unsafe int[] IOPTIONALCALENDARS
		{
			get
			{
				return this.GetWordArray(this.m_pData->waCalendars);
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060028BB RID: 10427 RVA: 0x0007D98F File Offset: 0x0007C98F
		internal unsafe string SNAME
		{
			get
			{
				return this.GetString(this.m_pData->sName);
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060028BC RID: 10428 RVA: 0x0007D9A2 File Offset: 0x0007C9A2
		internal unsafe string SABBREVLANGNAME
		{
			get
			{
				return this.GetString(this.m_pData->sAbbrevLang);
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x0007D9B5 File Offset: 0x0007C9B5
		internal unsafe string SISO639LANGNAME
		{
			get
			{
				return this.GetString(this.m_pData->sISO639Language);
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x060028BE RID: 10430 RVA: 0x0007D9C8 File Offset: 0x0007C9C8
		internal unsafe string SENGCOUNTRY
		{
			get
			{
				return this.GetString(this.m_pData->sEnglishCountry);
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x0007D9DB File Offset: 0x0007C9DB
		internal unsafe string SNATIVECOUNTRY
		{
			get
			{
				return this.GetString(this.m_pData->sNativeCountry);
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x060028C0 RID: 10432 RVA: 0x0007D9EE File Offset: 0x0007C9EE
		internal unsafe string SABBREVCTRYNAME
		{
			get
			{
				return this.GetString(this.m_pData->sAbbrevCountry);
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x0007DA01 File Offset: 0x0007CA01
		internal unsafe string SISO3166CTRYNAME
		{
			get
			{
				return this.GetString(this.m_pData->sISO3166CountryName);
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x0007DA14 File Offset: 0x0007CA14
		internal unsafe string SINTLSYMBOL
		{
			get
			{
				return this.GetString(this.m_pData->sIntlMonetarySymbol);
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x060028C3 RID: 10435 RVA: 0x0007DA27 File Offset: 0x0007CA27
		internal unsafe string SENGLISHCURRENCY
		{
			get
			{
				return this.GetString(this.m_pData->sEnglishCurrency);
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x060028C4 RID: 10436 RVA: 0x0007DA3A File Offset: 0x0007CA3A
		internal unsafe string SNATIVECURRENCY
		{
			get
			{
				return this.GetString(this.m_pData->sNativeCurrency);
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x060028C5 RID: 10437 RVA: 0x0007DA4D File Offset: 0x0007CA4D
		internal unsafe string SENGDISPLAYNAME
		{
			get
			{
				return this.GetString(this.m_pData->sEnglishDisplayName);
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060028C6 RID: 10438 RVA: 0x0007DA60 File Offset: 0x0007CA60
		internal unsafe string SISO639LANGNAME2
		{
			get
			{
				return this.GetString(this.m_pData->sISO639Language2);
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x060028C7 RID: 10439 RVA: 0x0007DA74 File Offset: 0x0007CA74
		internal unsafe string SNATIVEDISPLAYNAME
		{
			get
			{
				if (CultureInfo.GetLangID(this.ActualCultureID) == 1028 && CultureInfo.GetLangID(CultureInfo.InstalledUICulture.LCID) == 1028 && !this.IsCustomCulture)
				{
					return CultureInfo.nativeGetLocaleInfo(1028, 4) + " (" + CultureInfo.nativeGetLocaleInfo(1028, 8) + ")";
				}
				return this.GetString(this.m_pData->sNativeDisplayName);
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x060028C8 RID: 10440 RVA: 0x0007DAE8 File Offset: 0x0007CAE8
		internal unsafe string SPERCENT
		{
			get
			{
				return this.GetString(this.m_pData->sPercent);
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x0007DAFB File Offset: 0x0007CAFB
		internal unsafe string SNAN
		{
			get
			{
				return this.GetString(this.m_pData->sNaN);
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x0007DB0E File Offset: 0x0007CB0E
		internal unsafe string SPOSINFINITY
		{
			get
			{
				return this.GetString(this.m_pData->sPositiveInfinity);
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x0007DB21 File Offset: 0x0007CB21
		internal unsafe string SNEGINFINITY
		{
			get
			{
				return this.GetString(this.m_pData->sNegativeInfinity);
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x0007DB34 File Offset: 0x0007CB34
		internal unsafe string SADERA
		{
			get
			{
				return this.GetString(this.m_pData->sAdEra);
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x0007DB47 File Offset: 0x0007CB47
		internal unsafe string SABBREVADERA
		{
			get
			{
				return this.GetString(this.m_pData->sAbbrevAdEra);
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060028CE RID: 10446 RVA: 0x0007DB5A File Offset: 0x0007CB5A
		internal unsafe string SISO3166CTRYNAME2
		{
			get
			{
				return this.GetString(this.m_pData->sISO3166CountryName2);
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x0007DB6D File Offset: 0x0007CB6D
		internal unsafe string SREGIONNAME
		{
			get
			{
				return this.GetString(this.m_pData->sRegionName);
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x0007DB80 File Offset: 0x0007CB80
		internal unsafe string SPARENT
		{
			get
			{
				return this.GetString(this.m_pData->sParent);
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x0007DB93 File Offset: 0x0007CB93
		internal unsafe string SCONSOLEFALLBACKNAME
		{
			get
			{
				return this.GetString(this.m_pData->sConsoleFallbackName);
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x0007DBA6 File Offset: 0x0007CBA6
		internal unsafe string SSPECIFICCULTURE
		{
			get
			{
				return this.GetString(this.m_pData->sSpecificCulture);
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x0007DBB9 File Offset: 0x0007CBB9
		internal unsafe string[] SDAYNAMES
		{
			get
			{
				return this.GetStringArray(this.m_pData->saDayNames);
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x0007DBCC File Offset: 0x0007CBCC
		internal unsafe string[] SABBREVDAYNAMES
		{
			get
			{
				return this.GetStringArray(this.m_pData->saAbbrevDayNames);
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x0007DBDF File Offset: 0x0007CBDF
		internal unsafe string[] SSUPERSHORTDAYNAMES
		{
			get
			{
				return this.GetStringArray(this.m_pData->saSuperShortDayNames);
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x0007DBF2 File Offset: 0x0007CBF2
		internal unsafe string[] SMONTHNAMES
		{
			get
			{
				return this.GetStringArray(this.m_pData->saMonthNames);
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060028D7 RID: 10455 RVA: 0x0007DC05 File Offset: 0x0007CC05
		internal unsafe string[] SABBREVMONTHNAMES
		{
			get
			{
				return this.GetStringArray(this.m_pData->saAbbrevMonthNames);
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x0007DC18 File Offset: 0x0007CC18
		internal unsafe string[] SMONTHGENITIVENAMES
		{
			get
			{
				return this.GetStringArray(this.m_pData->saMonthGenitiveNames);
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x0007DC2B File Offset: 0x0007CC2B
		internal unsafe string[] SABBREVMONTHGENITIVENAMES
		{
			get
			{
				return this.GetStringArray(this.m_pData->saAbbrevMonthGenitiveNames);
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x0007DC3E File Offset: 0x0007CC3E
		internal unsafe string[] SNATIVECALNAMES
		{
			get
			{
				return this.GetStringArray(this.m_pData->saNativeCalendarNames);
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060028DB RID: 10459 RVA: 0x0007DC51 File Offset: 0x0007CC51
		internal unsafe string[] SDATEWORDS
		{
			get
			{
				return this.GetStringArray(this.m_pData->saDateWords);
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060028DC RID: 10460 RVA: 0x0007DC64 File Offset: 0x0007CC64
		internal unsafe string[] SALTSORTID
		{
			get
			{
				return this.GetStringArray(this.m_pData->saAltSortID);
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060028DD RID: 10461 RVA: 0x0007DC77 File Offset: 0x0007CC77
		internal unsafe DateTimeFormatFlags IFORMATFLAGS
		{
			get
			{
				return (DateTimeFormatFlags)this.m_pData->iFormatFlags;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060028DE RID: 10462 RVA: 0x0007DC84 File Offset: 0x0007CC84
		internal unsafe string SPOSITIVESIGN
		{
			get
			{
				string text = this.GetString(this.m_pData->sPositiveSign);
				if (text == null || text.Length == 0)
				{
					text = "+";
				}
				return text;
			}
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x0007DCB5 File Offset: 0x0007CCB5
		internal static bool IsCustomCultureId(int cultureId)
		{
			return cultureId == 3072 || cultureId == 4096;
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x0007DCCA File Offset: 0x0007CCCA
		private ushort ConvertFirstDayOfWeekMonToSun(int iTemp)
		{
			if (iTemp < 0 || iTemp > 6)
			{
				iTemp = 1;
			}
			else if (iTemp == 6)
			{
				iTemp = 0;
			}
			else
			{
				iTemp++;
			}
			return (ushort)iTemp;
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060028E1 RID: 10465 RVA: 0x0007DCE9 File Offset: 0x0007CCE9
		internal unsafe ushort IFIRSTDAYOFWEEK
		{
			get
			{
				return this.m_pData->iFirstDayOfWeek;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060028E2 RID: 10466 RVA: 0x0007DCF6 File Offset: 0x0007CCF6
		internal unsafe ushort IINPUTLANGUAGEHANDLE
		{
			get
			{
				return this.m_pData->iInputLanguageHandle;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x0007DD04 File Offset: 0x0007CD04
		internal unsafe ushort ITEXTINFO
		{
			get
			{
				ushort num = this.m_pData->iTextInfo;
				if (this.CultureID == 1034)
				{
					num = 1034;
				}
				if (num == 3072 || num == 0)
				{
					num = 127;
				}
				return num;
			}
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x0007DD40 File Offset: 0x0007CD40
		private static int[] ConvertWin32GroupString(string win32Str)
		{
			if (win32Str == null || win32Str.Length == 0 || win32Str[0] == '0')
			{
				return new int[]
				{
					3
				};
			}
			int[] array;
			if (win32Str[win32Str.Length - 1] == '0')
			{
				array = new int[win32Str.Length / 2];
			}
			else
			{
				array = new int[win32Str.Length / 2 + 2];
				array[array.Length - 1] = 0;
			}
			int num = 0;
			int num2 = 0;
			while (num < win32Str.Length && num2 < array.Length)
			{
				if (win32Str[num] < '1' || win32Str[num] > '9')
				{
					return new int[]
					{
						3
					};
				}
				array[num2] = (int)(win32Str[num] - '0');
				num += 2;
				num2++;
			}
			return array;
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x0007DDFC File Offset: 0x0007CDFC
		private static string UnescapeWin32String(string str, int start, int end)
		{
			StringBuilder stringBuilder = null;
			bool flag = false;
			int num = start;
			while (num < str.Length && num <= end)
			{
				if (str[num] == '\'')
				{
					if (flag)
					{
						if (num + 1 < str.Length && str[num + 1] == '\'')
						{
							stringBuilder.Append('\'');
							num++;
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						flag = true;
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(str, start, num - start, str.Length);
						}
					}
				}
				else if (stringBuilder != null)
				{
					stringBuilder.Append(str[num]);
				}
				num++;
			}
			if (stringBuilder == null)
			{
				return str.Substring(start, end - start + 1);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x0007DE9C File Offset: 0x0007CE9C
		private static string ReescapeWin32String(string str)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			bool flag = false;
			int i = 0;
			while (i < str.Length)
			{
				if (str[i] == '\'')
				{
					if (!flag)
					{
						flag = true;
						goto IL_91;
					}
					if (i + 1 >= str.Length || str[i + 1] != '\'')
					{
						flag = false;
						goto IL_91;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(str, 0, i, str.Length * 2);
					}
					stringBuilder.Append("\\'");
					i++;
				}
				else
				{
					if (str[i] != '\\')
					{
						goto IL_91;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(str, 0, i, str.Length * 2);
					}
					stringBuilder.Append("\\\\");
				}
				IL_A2:
				i++;
				continue;
				IL_91:
				if (stringBuilder != null)
				{
					stringBuilder.Append(str[i]);
					goto IL_A2;
				}
				goto IL_A2;
			}
			if (stringBuilder == null)
			{
				return str;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x0007DF68 File Offset: 0x0007CF68
		private static string[] ReescapeWin32Strings(string[] array)
		{
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = CultureTableRecord.ReescapeWin32String(array[i]);
				}
			}
			return array;
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x0007DF94 File Offset: 0x0007CF94
		internal unsafe string STIME
		{
			get
			{
				string overrideStringArrayDefault = this.GetOverrideStringArrayDefault(this.m_pData->saTimeFormat, 4099);
				return CultureTableRecord.GetTimeSeparator(overrideStringArrayDefault);
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060028E9 RID: 10473 RVA: 0x0007DFC0 File Offset: 0x0007CFC0
		internal unsafe string SDATE
		{
			get
			{
				string overrideStringArrayDefault = this.GetOverrideStringArrayDefault(this.m_pData->saShortDate, 31);
				return CultureTableRecord.GetDateSeparator(overrideStringArrayDefault);
			}
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x0007DFE8 File Offset: 0x0007CFE8
		private static string GetTimeSeparator(string format)
		{
			string result = string.Empty;
			int num = -1;
			int i = 0;
			while (i < format.Length)
			{
				if (format[i] == 'H' || format[i] == 'h' || format[i] == 'm' || format[i] == 's')
				{
					char c = format[i];
					i++;
					while (i < format.Length && format[i] == c)
					{
						i++;
					}
					if (i < format.Length)
					{
						num = i;
						break;
					}
					break;
				}
				else
				{
					if (format[i] == '\'')
					{
						i++;
						while (i < format.Length && format[i] != '\'')
						{
							i++;
						}
					}
					i++;
				}
			}
			if (num != -1)
			{
				for (i = num; i < format.Length; i++)
				{
					if (format[i] == 'H' || format[i] == 'h' || format[i] == 'm' || format[i] == 's')
					{
						result = CultureTableRecord.UnescapeWin32String(format, num, i - 1);
						break;
					}
					if (format[i] == '\'')
					{
						i++;
						while (i < format.Length && format[i] != '\'')
						{
							i++;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x0007E118 File Offset: 0x0007D118
		private static string GetDateSeparator(string format)
		{
			string result = string.Empty;
			int num = -1;
			int i = 0;
			while (i < format.Length)
			{
				if (format[i] == 'd' || format[i] == 'y' || format[i] == 'M')
				{
					char c = format[i];
					i++;
					while (i < format.Length && format[i] == c)
					{
						i++;
					}
					if (i < format.Length)
					{
						num = i;
						break;
					}
					break;
				}
				else
				{
					if (format[i] == '\'')
					{
						i++;
						while (i < format.Length && format[i] != '\'')
						{
							i++;
						}
					}
					i++;
				}
			}
			if (num != -1)
			{
				for (i = num; i < format.Length; i++)
				{
					if (format[i] == 'y' || format[i] == 'M' || format[i] == 'd')
					{
						result = CultureTableRecord.UnescapeWin32String(format, num, i - 1);
						break;
					}
					if (format[i] == '\'')
					{
						i++;
						while (i < format.Length && format[i] != '\'')
						{
							i++;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x0007E230 File Offset: 0x0007D230
		internal void GetDTFIOverrideValues(ref DTFIUserOverrideValues values)
		{
			bool flag = false;
			if (this.UseGetLocaleInfo)
			{
				flag = CultureInfo.nativeGetDTFIUserValues(this.InteropLCID, ref values);
			}
			if (flag)
			{
				values.firstDayOfWeek = (int)this.ConvertFirstDayOfWeekMonToSun(values.firstDayOfWeek);
				values.shortDatePattern = CultureTableRecord.ReescapeWin32String(values.shortDatePattern);
				values.longDatePattern = CultureTableRecord.ReescapeWin32String(values.longDatePattern);
				values.longTimePattern = CultureTableRecord.ReescapeWin32String(values.longTimePattern);
				values.yearMonthPattern = CultureTableRecord.ReescapeWin32String(values.yearMonthPattern);
				return;
			}
			values.firstDayOfWeek = (int)this.IFIRSTDAYOFWEEK;
			values.calendarWeekRule = (int)this.IFIRSTWEEKOFYEAR;
			values.shortDatePattern = this.SSHORTDATE;
			values.longDatePattern = this.SLONGDATE;
			values.yearMonthPattern = this.SYEARMONTH;
			values.amDesignator = this.S1159;
			values.pmDesignator = this.S2359;
			values.longTimePattern = this.STIMEFORMAT;
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x0007E310 File Offset: 0x0007D310
		internal void GetNFIOverrideValues(NumberFormatInfo nfi)
		{
			bool flag = false;
			if (this.UseGetLocaleInfo)
			{
				flag = CultureInfo.nativeGetNFIUserValues(this.InteropLCID, nfi);
			}
			if (!flag)
			{
				nfi.numberDecimalDigits = (int)this.IDIGITS;
				nfi.numberNegativePattern = (int)this.INEGNUMBER;
				nfi.currencyDecimalDigits = (int)this.ICURRDIGITS;
				nfi.currencyPositivePattern = (int)this.ICURRENCY;
				nfi.currencyNegativePattern = (int)this.INEGCURR;
				nfi.negativeSign = this.SNEGATIVESIGN;
				nfi.numberDecimalSeparator = this.SDECIMAL;
				nfi.numberGroupSeparator = this.STHOUSAND;
				nfi.positiveSign = this.SPOSITIVESIGN;
				nfi.currencyDecimalSeparator = this.SMONDECIMALSEP;
				nfi.currencySymbol = this.SCURRENCY;
				nfi.currencyGroupSeparator = this.SMONTHOUSANDSEP;
				nfi.nativeDigits = this.SNATIVEDIGITS;
				nfi.digitSubstitution = (int)this.IDIGITSUBSTITUTION;
			}
			else if (-1 == nfi.digitSubstitution)
			{
				nfi.digitSubstitution = (int)this.IDIGITSUBSTITUTION;
			}
			nfi.numberGroupSizes = this.SGROUPING;
			nfi.currencyGroupSizes = this.SMONGROUPING;
			nfi.percentDecimalDigits = nfi.numberDecimalDigits;
			nfi.percentDecimalSeparator = nfi.numberDecimalSeparator;
			nfi.percentGroupSizes = nfi.numberGroupSizes;
			nfi.percentGroupSeparator = nfi.numberGroupSeparator;
			nfi.percentNegativePattern = (int)this.INEGATIVEPERCENT;
			nfi.percentPositivePattern = (int)this.IPOSITIVEPERCENT;
			nfi.percentSymbol = this.SPERCENT;
			if (nfi.positiveSign == null || nfi.positiveSign.Length == 0)
			{
				nfi.positiveSign = "+";
			}
			if (nfi.currencyDecimalSeparator.Length == 0)
			{
				nfi.currencyDecimalSeparator = this.SMONDECIMALSEP;
			}
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x0007E4A0 File Offset: 0x0007D4A0
		internal unsafe int EverettDataItem()
		{
			if (this.IsCustomCulture)
			{
				return 0;
			}
			CultureTableRecord.InitEverettCultureDataItemMapping();
			int i = 0;
			int num = CultureTableRecord.m_EverettCultureDataItemMappingsSize / 2 - 1;
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				int num3 = this.m_CultureID - CultureTableRecord.m_EverettCultureDataItemMappings[num2 * 2];
				if (num3 == 0)
				{
					return CultureTableRecord.m_EverettCultureDataItemMappings[num2 * 2 + 1];
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
			return 0;
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x0007E50C File Offset: 0x0007D50C
		internal unsafe int EverettRegionDataItem()
		{
			if (this.IsCustomCulture)
			{
				return 0;
			}
			CultureTableRecord.InitEverettRegionDataItemMapping();
			int i = 0;
			int num = CultureTableRecord.m_EverettRegionDataItemMappingsSize / 2 - 1;
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				int num3 = this.m_CultureID - CultureTableRecord.m_EverettRegionDataItemMappings[num2 * 2];
				if (num3 == 0)
				{
					return CultureTableRecord.m_EverettRegionDataItemMappings[num2 * 2 + 1];
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
			return 0;
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x0007E577 File Offset: 0x0007D577
		internal unsafe static int IdFromEverettDataItem(int iDataItem)
		{
			CultureTableRecord.InitEverettDataItemToLCIDMappings();
			if (iDataItem < 0 || iDataItem >= CultureTableRecord.m_EverettDataItemToLCIDMappingsSize)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidFieldState"));
			}
			return CultureTableRecord.m_EverettDataItemToLCIDMappings[iDataItem];
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x0007E5A5 File Offset: 0x0007D5A5
		internal unsafe static int IdFromEverettRegionInfoDataItem(int iDataItem)
		{
			CultureTableRecord.InitEverettRegionDataItemToLCIDMappings();
			if (iDataItem < 0 || iDataItem >= CultureTableRecord.m_EverettRegionInfoDataItemToLCIDMappingsSize)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidFieldState"));
			}
			return CultureTableRecord.m_EverettRegionInfoDataItemToLCIDMappings[iDataItem];
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x0007E5D4 File Offset: 0x0007D5D4
		private unsafe static void InitEverettRegionDataItemMapping()
		{
			if (CultureTableRecord.m_EverettRegionDataItemMappings == null)
			{
				int* everettRegionDataItemMappings = CultureInfo.nativeGetStaticInt32DataTable(0, out CultureTableRecord.m_EverettRegionDataItemMappingsSize);
				CultureTableRecord.m_EverettRegionDataItemMappings = everettRegionDataItemMappings;
			}
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x0007E5FC File Offset: 0x0007D5FC
		private unsafe static void InitEverettCultureDataItemMapping()
		{
			if (CultureTableRecord.m_EverettCultureDataItemMappings == null)
			{
				int* everettCultureDataItemMappings = CultureInfo.nativeGetStaticInt32DataTable(1, out CultureTableRecord.m_EverettCultureDataItemMappingsSize);
				CultureTableRecord.m_EverettCultureDataItemMappings = everettCultureDataItemMappings;
			}
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x0007E624 File Offset: 0x0007D624
		private unsafe static void InitEverettDataItemToLCIDMappings()
		{
			if (CultureTableRecord.m_EverettDataItemToLCIDMappings == null)
			{
				int* everettDataItemToLCIDMappings = CultureInfo.nativeGetStaticInt32DataTable(2, out CultureTableRecord.m_EverettDataItemToLCIDMappingsSize);
				CultureTableRecord.m_EverettDataItemToLCIDMappings = everettDataItemToLCIDMappings;
			}
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x0007E64C File Offset: 0x0007D64C
		private unsafe static void InitEverettRegionDataItemToLCIDMappings()
		{
			if (CultureTableRecord.m_EverettRegionInfoDataItemToLCIDMappings == null)
			{
				int* everettRegionInfoDataItemToLCIDMappings = CultureInfo.nativeGetStaticInt32DataTable(3, out CultureTableRecord.m_EverettRegionInfoDataItemToLCIDMappingsSize);
				CultureTableRecord.m_EverettRegionInfoDataItemToLCIDMappings = everettRegionInfoDataItemToLCIDMappings;
			}
		}

		// Token: 0x04001347 RID: 4935
		internal const int SPANISH_TRADITIONAL_SORT = 1034;

		// Token: 0x04001348 RID: 4936
		private const int SPANISH_INTERNATIONAL_SORT = 3082;

		// Token: 0x04001349 RID: 4937
		private const int MAXSIZE_LANGUAGE = 8;

		// Token: 0x0400134A RID: 4938
		private const int MAXSIZE_REGION = 8;

		// Token: 0x0400134B RID: 4939
		private const int MAXSIZE_SUFFIX = 64;

		// Token: 0x0400134C RID: 4940
		private const int MAXSIZE_FULLTAGNAME = 84;

		// Token: 0x0400134D RID: 4941
		private const int LOCALE_SLANGUAGE = 2;

		// Token: 0x0400134E RID: 4942
		private const int LOCALE_SCOUNTRY = 6;

		// Token: 0x0400134F RID: 4943
		private const int LOCALE_SNATIVELANGNAME = 4;

		// Token: 0x04001350 RID: 4944
		private const int LOCALE_SNATIVECTRYNAME = 8;

		// Token: 0x04001351 RID: 4945
		private const int LOCALE_ICALENDARTYPE = 4105;

		// Token: 0x04001352 RID: 4946
		private const int INT32TABLE_EVERETT_REGION_DATA_ITEM_MAPPINGS = 0;

		// Token: 0x04001353 RID: 4947
		private const int INT32TABLE_EVERETT_CULTURE_DATA_ITEM_MAPPINGS = 1;

		// Token: 0x04001354 RID: 4948
		private const int INT32TABLE_EVERETT_DATA_ITEM_TO_LCID_MAPPINGS = 2;

		// Token: 0x04001355 RID: 4949
		private const int INT32TABLE_EVERETT_REGION_DATA_ITEM_TO_LCID_MAPPINGS = 3;

		// Token: 0x04001356 RID: 4950
		private static object s_InternalSyncObject;

		// Token: 0x04001357 RID: 4951
		private static Hashtable CultureTableRecordCache;

		// Token: 0x04001358 RID: 4952
		private static Hashtable CultureTableRecordRegionCache;

		// Token: 0x04001359 RID: 4953
		private static Hashtable SyntheticDataCache;

		// Token: 0x0400135A RID: 4954
		internal static Hashtable SyntheticLcidToNameCache;

		// Token: 0x0400135B RID: 4955
		internal static Hashtable SyntheticNameToLcidCache;

		// Token: 0x0400135C RID: 4956
		private CultureTable m_CultureTable;

		// Token: 0x0400135D RID: 4957
		private unsafe CultureTableData* m_pData;

		// Token: 0x0400135E RID: 4958
		private unsafe ushort* m_pPool;

		// Token: 0x0400135F RID: 4959
		private bool m_bUseUserOverride;

		// Token: 0x04001360 RID: 4960
		private int m_CultureID;

		// Token: 0x04001361 RID: 4961
		private string m_CultureName;

		// Token: 0x04001362 RID: 4962
		private int m_ActualCultureID;

		// Token: 0x04001363 RID: 4963
		private string m_ActualName;

		// Token: 0x04001364 RID: 4964
		private bool m_synthetic;

		// Token: 0x04001365 RID: 4965
		private AgileSafeNativeMemoryHandle nativeMemoryHandle;

		// Token: 0x04001366 RID: 4966
		private string m_windowsPath;

		// Token: 0x04001367 RID: 4967
		private static CultureTableRecord.AdjustedSyntheticCultureName[] s_adjustedSyntheticNames = null;

		// Token: 0x04001368 RID: 4968
		private unsafe static int* m_EverettRegionDataItemMappings = null;

		// Token: 0x04001369 RID: 4969
		private static int m_EverettRegionDataItemMappingsSize = 0;

		// Token: 0x0400136A RID: 4970
		private unsafe static int* m_EverettCultureDataItemMappings = null;

		// Token: 0x0400136B RID: 4971
		private static int m_EverettCultureDataItemMappingsSize = 0;

		// Token: 0x0400136C RID: 4972
		private unsafe static int* m_EverettDataItemToLCIDMappings = null;

		// Token: 0x0400136D RID: 4973
		private static int m_EverettDataItemToLCIDMappingsSize = 0;

		// Token: 0x0400136E RID: 4974
		private unsafe static int* m_EverettRegionInfoDataItemToLCIDMappings = null;

		// Token: 0x0400136F RID: 4975
		private static int m_EverettRegionInfoDataItemToLCIDMappingsSize = 0;

		// Token: 0x020003DE RID: 990
		private struct CompositeCultureData
		{
			// Token: 0x04001370 RID: 4976
			internal string sname;

			// Token: 0x04001371 RID: 4977
			internal string englishDisplayName;

			// Token: 0x04001372 RID: 4978
			internal string sNativeDisplayName;

			// Token: 0x04001373 RID: 4979
			internal string waCalendars;

			// Token: 0x04001374 RID: 4980
			internal string consoleFallbackName;

			// Token: 0x04001375 RID: 4981
			internal string parentName;

			// Token: 0x04001376 RID: 4982
			internal int parentLcid;
		}

		// Token: 0x020003DF RID: 991
		internal class AdjustedSyntheticCultureName
		{
			// Token: 0x060028F7 RID: 10487 RVA: 0x0007E6B0 File Offset: 0x0007D6B0
			internal AdjustedSyntheticCultureName(int lcid, string isoLanguage, string isoCountry, string sName)
			{
				this.lcid = lcid;
				this.isoLanguage = isoLanguage;
				this.isoCountry = isoCountry;
				this.sName = sName;
			}

			// Token: 0x04001377 RID: 4983
			internal int lcid;

			// Token: 0x04001378 RID: 4984
			internal string isoLanguage;

			// Token: 0x04001379 RID: 4985
			internal string isoCountry;

			// Token: 0x0400137A RID: 4986
			internal string sName;
		}
	}
}
