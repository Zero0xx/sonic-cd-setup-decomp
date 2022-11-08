using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x02000388 RID: 904
	internal class JapaneseCalendarTable
	{
		// Token: 0x0600235E RID: 9054 RVA: 0x00059514 File Offset: 0x00058514
		private JapaneseCalendarTable(JapaneseCalendarTable.ExtendedEraInfo[] eraInfo)
		{
			this._eraInfo = eraInfo;
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x00059523 File Offset: 0x00058523
		private static JapaneseCalendarTable GetJapaneseCalendarTableInstance()
		{
			if (JapaneseCalendarTable.s_japanese == null)
			{
				JapaneseCalendarTable.s_japanese = new JapaneseCalendarTable(JapaneseCalendarTable.GetAllEras());
			}
			return JapaneseCalendarTable.s_japanese;
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x00059540 File Offset: 0x00058540
		private static JapaneseCalendarTable GetJapaneseLunisolarCalendarTableInstance()
		{
			if (JapaneseCalendarTable.s_japaneseLunisolar == null)
			{
				JapaneseCalendarTable.s_japaneseLunisolar = new JapaneseCalendarTable(JapaneseCalendarTable.TrimErasForLunisolar(JapaneseCalendarTable.GetAllEras()));
			}
			return JapaneseCalendarTable.s_japaneseLunisolar;
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x00059564 File Offset: 0x00058564
		private static JapaneseCalendarTable GetInstance(int calendarId)
		{
			if (calendarId == 3)
			{
				return JapaneseCalendarTable.GetJapaneseCalendarTableInstance();
			}
			if (calendarId != 14)
			{
				return null;
			}
			return JapaneseCalendarTable.GetJapaneseLunisolarCalendarTableInstance();
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0005958B File Offset: 0x0005858B
		internal static bool IsJapaneseCalendar(int calendarId)
		{
			return calendarId == 3 || calendarId == 14;
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x00059598 File Offset: 0x00058598
		private static JapaneseCalendarTable.ExtendedEraInfo[] GetAllEras()
		{
			if (JapaneseCalendarTable.s_allEras == null)
			{
				JapaneseCalendarTable.s_allEras = JapaneseCalendarTable.GetErasFromRegistry();
				if (JapaneseCalendarTable.s_allEras == null)
				{
					JapaneseCalendarTable.s_allEras = new JapaneseCalendarTable.ExtendedEraInfo[]
					{
						new JapaneseCalendarTable.ExtendedEraInfo(4, new DateTime(1989, 1, 8).Ticks, 1988, 1, 8011, "平成", "平", "H"),
						new JapaneseCalendarTable.ExtendedEraInfo(3, new DateTime(1926, 12, 25).Ticks, 1925, 1, 64, "昭和", "昭", "S"),
						new JapaneseCalendarTable.ExtendedEraInfo(2, new DateTime(1912, 7, 30).Ticks, 1911, 1, 15, "大正", "大", "T"),
						new JapaneseCalendarTable.ExtendedEraInfo(1, new DateTime(1868, 1, 1).Ticks, 1867, 1, 45, "明治", "明", "M")
					};
				}
			}
			return JapaneseCalendarTable.s_allEras;
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x000596AC File Offset: 0x000586AC
		[SecuritySafeCritical]
		private static JapaneseCalendarTable.ExtendedEraInfo[] GetErasFromRegistry()
		{
			int num = 0;
			JapaneseCalendarTable.ExtendedEraInfo[] array = null;
			try
			{
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras"));
				permissionSet.Assert();
				RegistryKey registryKey = RegistryKey.GetBaseKey(RegistryKey.HKEY_LOCAL_MACHINE).OpenSubKey("System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras", false);
				if (registryKey == null)
				{
					return null;
				}
				string[] valueNames = registryKey.GetValueNames();
				if (valueNames != null && valueNames.Length > 0)
				{
					array = new JapaneseCalendarTable.ExtendedEraInfo[valueNames.Length];
					for (int i = 0; i < valueNames.Length; i++)
					{
						JapaneseCalendarTable.ExtendedEraInfo eraFromValue = JapaneseCalendarTable.GetEraFromValue(valueNames[i], registryKey.GetValue(valueNames[i]).ToString());
						if (eraFromValue != null)
						{
							array[num] = eraFromValue;
							num++;
						}
					}
				}
			}
			catch (SecurityException)
			{
				return null;
			}
			catch (IOException)
			{
				return null;
			}
			catch (UnauthorizedAccessException)
			{
				return null;
			}
			if (num < 4)
			{
				return null;
			}
			Array.Resize<JapaneseCalendarTable.ExtendedEraInfo>(ref array, num);
			Array.Sort<JapaneseCalendarTable.ExtendedEraInfo>(array, new Comparison<JapaneseCalendarTable.ExtendedEraInfo>(JapaneseCalendarTable.CompareEraRanges));
			for (int j = 0; j < array.Length; j++)
			{
				array[j].Era = array.Length - j;
				if (j == 0)
				{
					array[0].MaxEraYear = 9999 - array[0].YearOffset;
				}
				else
				{
					array[j].MaxEraYear = array[j - 1].YearOffset + 1 - array[j].YearOffset;
				}
			}
			return array;
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0005981C File Offset: 0x0005881C
		private static int CompareEraRanges(JapaneseCalendarTable.ExtendedEraInfo a, JapaneseCalendarTable.ExtendedEraInfo b)
		{
			return b.Ticks.CompareTo(a.Ticks);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x00059840 File Offset: 0x00058840
		private static JapaneseCalendarTable.ExtendedEraInfo GetEraFromValue(string value, string data)
		{
			if (value == null || data == null)
			{
				return null;
			}
			if (value.Length != 10)
			{
				return null;
			}
			int num;
			int month;
			int day;
			if (!Number.TryParseInt32(value.Substring(0, 4), NumberStyles.None, NumberFormatInfo.InvariantInfo, out num) || !Number.TryParseInt32(value.Substring(5, 2), NumberStyles.None, NumberFormatInfo.InvariantInfo, out month) || !Number.TryParseInt32(value.Substring(8, 2), NumberStyles.None, NumberFormatInfo.InvariantInfo, out day))
			{
				return null;
			}
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array.Length != 4)
			{
				return null;
			}
			if (array[0].Length == 0 || array[1].Length == 0 || array[2].Length == 0 || array[3].Length == 0)
			{
				return null;
			}
			return new JapaneseCalendarTable.ExtendedEraInfo(0, new DateTime(num, month, day).Ticks, num - 1, 1, 0, array[0], array[1], array[3]);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x00059916 File Offset: 0x00058916
		internal static int CurrentEra(int calendarId)
		{
			return JapaneseCalendarTable.GetAllEras().Length;
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0005991F File Offset: 0x0005891F
		internal static string[] EraNames(int calendarId)
		{
			return JapaneseCalendarTable.GetInstance(calendarId).EraNames();
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0005992C File Offset: 0x0005892C
		private string[] EraNames()
		{
			if (this._eraNames == null)
			{
				this._eraNames = JapaneseCalendarTable.EraNames(this._eraInfo);
			}
			return this._eraNames;
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x00059950 File Offset: 0x00058950
		private static string[] EraNames(JapaneseCalendarTable.ExtendedEraInfo[] eras)
		{
			string[] array = new string[eras.Length];
			for (int i = 0; i < eras.Length; i++)
			{
				array[i] = eras[eras.Length - i - 1].EraName;
			}
			return array;
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x00059986 File Offset: 0x00058986
		internal static string[] AbbrevEraNames(int calendarId)
		{
			return JapaneseCalendarTable.GetInstance(calendarId).AbbrevEraNames();
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x00059993 File Offset: 0x00058993
		private string[] AbbrevEraNames()
		{
			if (this._abbrevEraNames == null)
			{
				this._abbrevEraNames = JapaneseCalendarTable.AbbrevEraNames(this._eraInfo);
			}
			return this._abbrevEraNames;
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x000599B4 File Offset: 0x000589B4
		private static string[] AbbrevEraNames(JapaneseCalendarTable.ExtendedEraInfo[] eras)
		{
			string[] array = new string[eras.Length];
			for (int i = 0; i < eras.Length; i++)
			{
				array[i] = eras[eras.Length - i - 1].AbbrevEraName;
			}
			return array;
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x000599EA File Offset: 0x000589EA
		internal static string[] EnglishEraNames(int calendarId)
		{
			return JapaneseCalendarTable.GetInstance(calendarId).EnglishEraNames();
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x000599F7 File Offset: 0x000589F7
		private string[] EnglishEraNames()
		{
			if (this._englishEraNames == null)
			{
				this._englishEraNames = JapaneseCalendarTable.EnglishEraNames(this._eraInfo);
			}
			return this._englishEraNames;
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x00059A18 File Offset: 0x00058A18
		private static string[] EnglishEraNames(JapaneseCalendarTable.ExtendedEraInfo[] eras)
		{
			string[] array = new string[eras.Length];
			for (int i = 0; i < eras.Length; i++)
			{
				array[i] = eras[eras.Length - i - 1].EnglishEraName;
			}
			return array;
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x00059A4E File Offset: 0x00058A4E
		internal static int[][] EraRanges(int calendarId)
		{
			return JapaneseCalendarTable.GetInstance(calendarId).EraRanges();
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x00059A5B File Offset: 0x00058A5B
		private int[][] EraRanges()
		{
			if (this._eraRanges == null)
			{
				this._eraRanges = JapaneseCalendarTable.EraRanges(this._eraInfo);
			}
			return this._eraRanges;
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x00059A7C File Offset: 0x00058A7C
		private static int[][] EraRanges(JapaneseCalendarTable.ExtendedEraInfo[] eras)
		{
			int[][] array = new int[eras.Length][];
			for (int i = 0; i < eras.Length; i++)
			{
				JapaneseCalendarTable.ExtendedEraInfo extendedEraInfo = eras[i];
				int[] array2 = array[i] = new int[6];
				array2[0] = extendedEraInfo.Era;
				DateTime dateTime = new DateTime(extendedEraInfo.Ticks);
				array2[1] = dateTime.Year;
				array2[2] = dateTime.Month;
				array2[3] = dateTime.Day;
				array2[4] = extendedEraInfo.YearOffset;
				array2[5] = extendedEraInfo.MinEraYear;
			}
			return array;
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x00059AFC File Offset: 0x00058AFC
		private static JapaneseCalendarTable.ExtendedEraInfo[] TrimErasForLunisolar(JapaneseCalendarTable.ExtendedEraInfo[] baseEras)
		{
			JapaneseCalendarTable.ExtendedEraInfo[] array = new JapaneseCalendarTable.ExtendedEraInfo[baseEras.Length];
			int num = 0;
			for (int i = 0; i < baseEras.Length; i++)
			{
				if (baseEras[i].YearOffset + baseEras[i].MinEraYear < 2049)
				{
					if (baseEras[i].YearOffset + baseEras[i].MaxEraYear < 1960)
					{
						break;
					}
					array[num] = baseEras[i];
					num++;
				}
			}
			if (num == 0)
			{
				return baseEras;
			}
			Array.Resize<JapaneseCalendarTable.ExtendedEraInfo>(ref array, num);
			return array;
		}

		// Token: 0x04000F0B RID: 3851
		private const string c_japaneseErasHive = "System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras";

		// Token: 0x04000F0C RID: 3852
		private const string c_japaneseErasHivePermissionList = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras";

		// Token: 0x04000F0D RID: 3853
		private static JapaneseCalendarTable.ExtendedEraInfo[] s_allEras;

		// Token: 0x04000F0E RID: 3854
		private static JapaneseCalendarTable s_japanese;

		// Token: 0x04000F0F RID: 3855
		private static JapaneseCalendarTable s_japaneseLunisolar;

		// Token: 0x04000F10 RID: 3856
		private JapaneseCalendarTable.ExtendedEraInfo[] _eraInfo;

		// Token: 0x04000F11 RID: 3857
		private string[] _eraNames;

		// Token: 0x04000F12 RID: 3858
		private string[] _abbrevEraNames;

		// Token: 0x04000F13 RID: 3859
		private string[] _englishEraNames;

		// Token: 0x04000F14 RID: 3860
		private int[][] _eraRanges;

		// Token: 0x02000389 RID: 905
		private class ExtendedEraInfo
		{
			// Token: 0x17000634 RID: 1588
			// (get) Token: 0x06002375 RID: 9077 RVA: 0x00059B6A File Offset: 0x00058B6A
			// (set) Token: 0x06002376 RID: 9078 RVA: 0x00059B77 File Offset: 0x00058B77
			public int Era
			{
				get
				{
					return this.EraInfo.era;
				}
				set
				{
					this.EraInfo.era = value;
				}
			}

			// Token: 0x17000635 RID: 1589
			// (get) Token: 0x06002377 RID: 9079 RVA: 0x00059B85 File Offset: 0x00058B85
			public long Ticks
			{
				get
				{
					return this.EraInfo.ticks;
				}
			}

			// Token: 0x17000636 RID: 1590
			// (get) Token: 0x06002378 RID: 9080 RVA: 0x00059B92 File Offset: 0x00058B92
			public int YearOffset
			{
				get
				{
					return this.EraInfo.yearOffset;
				}
			}

			// Token: 0x17000637 RID: 1591
			// (get) Token: 0x06002379 RID: 9081 RVA: 0x00059B9F File Offset: 0x00058B9F
			public int MinEraYear
			{
				get
				{
					return this.EraInfo.minEraYear;
				}
			}

			// Token: 0x17000638 RID: 1592
			// (get) Token: 0x0600237A RID: 9082 RVA: 0x00059BAC File Offset: 0x00058BAC
			// (set) Token: 0x0600237B RID: 9083 RVA: 0x00059BB9 File Offset: 0x00058BB9
			public int MaxEraYear
			{
				get
				{
					return this.EraInfo.maxEraYear;
				}
				set
				{
					this.EraInfo.maxEraYear = value;
				}
			}

			// Token: 0x0600237C RID: 9084 RVA: 0x00059BC7 File Offset: 0x00058BC7
			internal ExtendedEraInfo(int era, long ticks, int yearOffset, int minEraYear, int maxEraYear, string eraName, string abbrevEraName, string englishEraName)
			{
				this.EraInfo = new EraInfo(era, ticks, yearOffset, minEraYear, maxEraYear);
				this.EraName = eraName;
				this.AbbrevEraName = abbrevEraName;
				this.EnglishEraName = englishEraName;
			}

			// Token: 0x04000F15 RID: 3861
			public EraInfo EraInfo;

			// Token: 0x04000F16 RID: 3862
			public string EraName;

			// Token: 0x04000F17 RID: 3863
			public string AbbrevEraName;

			// Token: 0x04000F18 RID: 3864
			public string EnglishEraName;
		}
	}
}
