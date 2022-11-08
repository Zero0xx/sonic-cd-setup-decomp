using System;
using System.Collections;
using System.Text;

namespace System.Globalization
{
	// Token: 0x020003AD RID: 941
	internal class DateTimeFormatInfoScanner
	{
		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600259B RID: 9627 RVA: 0x00068888 File Offset: 0x00067888
		private Hashtable KnownWords
		{
			get
			{
				if (DateTimeFormatInfoScanner.m_knownWords == null)
				{
					DateTimeFormatInfoScanner.m_knownWords = new Hashtable
					{
						{
							"/",
							string.Empty
						},
						{
							"-",
							string.Empty
						},
						{
							".",
							string.Empty
						},
						{
							"年",
							string.Empty
						},
						{
							"月",
							string.Empty
						},
						{
							"日",
							string.Empty
						},
						{
							"년",
							string.Empty
						},
						{
							"월",
							string.Empty
						},
						{
							"일",
							string.Empty
						},
						{
							"시",
							string.Empty
						},
						{
							"분",
							string.Empty
						},
						{
							"초",
							string.Empty
						},
						{
							"時",
							string.Empty
						},
						{
							"时",
							string.Empty
						},
						{
							"分",
							string.Empty
						},
						{
							"秒",
							string.Empty
						}
					};
				}
				return DateTimeFormatInfoScanner.m_knownWords;
			}
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000689B0 File Offset: 0x000679B0
		internal static int SkipWhiteSpacesAndNonLetter(string pattern, int currentIndex)
		{
			while (currentIndex < pattern.Length)
			{
				char c = pattern[currentIndex];
				if (c == '\\')
				{
					currentIndex++;
					if (currentIndex >= pattern.Length)
					{
						break;
					}
					c = pattern[currentIndex];
					if (c == '\'')
					{
						continue;
					}
				}
				if (char.IsLetter(c) || c == '\'' || c == '.')
				{
					break;
				}
				currentIndex++;
			}
			return currentIndex;
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x00068A08 File Offset: 0x00067A08
		internal void AddDateWordOrPostfix(string formatPostfix, string str)
		{
			if (str.Length > 0)
			{
				if (str.Equals("."))
				{
					this.AddIgnorableSymbols(".");
					return;
				}
				if (this.KnownWords[str] == null)
				{
					if (this.m_dateWords == null)
					{
						this.m_dateWords = new ArrayList();
					}
					if (formatPostfix == "MMMM")
					{
						string text = '' + str;
						if (!this.m_dateWords.Contains(text))
						{
							this.m_dateWords.Add(text);
							return;
						}
					}
					else
					{
						if (!this.m_dateWords.Contains(str))
						{
							this.m_dateWords.Add(str);
						}
						if (str[str.Length - 1] == '.')
						{
							string text2 = str.Substring(0, str.Length - 1);
							if (!this.m_dateWords.Contains(text2))
							{
								this.m_dateWords.Add(text2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x00068AF0 File Offset: 0x00067AF0
		internal int AddDateWords(string pattern, int index, string formatPostfix)
		{
			int num = DateTimeFormatInfoScanner.SkipWhiteSpacesAndNonLetter(pattern, index);
			if (num != index && formatPostfix != null)
			{
				formatPostfix = null;
			}
			index = num;
			StringBuilder stringBuilder = new StringBuilder();
			while (index < pattern.Length)
			{
				char c = pattern[index];
				if (c == '\'')
				{
					this.AddDateWordOrPostfix(formatPostfix, stringBuilder.ToString());
					index++;
					break;
				}
				if (c == '\\')
				{
					index++;
					if (index < pattern.Length)
					{
						stringBuilder.Append(pattern[index]);
						index++;
					}
				}
				else if (char.IsWhiteSpace(c))
				{
					this.AddDateWordOrPostfix(formatPostfix, stringBuilder.ToString());
					if (formatPostfix != null)
					{
						formatPostfix = null;
					}
					stringBuilder.Length = 0;
					index++;
				}
				else
				{
					stringBuilder.Append(c);
					index++;
				}
			}
			return index;
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x00068BA6 File Offset: 0x00067BA6
		internal static int ScanRepeatChar(string pattern, char ch, int index, out int count)
		{
			count = 1;
			while (++index < pattern.Length && pattern[index] == ch)
			{
				count++;
			}
			return index;
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x00068BCC File Offset: 0x00067BCC
		internal void AddIgnorableSymbols(string text)
		{
			if (this.m_dateWords == null)
			{
				this.m_dateWords = new ArrayList();
			}
			string text2 = '' + text;
			if (!this.m_dateWords.Contains(text2))
			{
				this.m_dateWords.Add(text2);
			}
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x00068C18 File Offset: 0x00067C18
		internal void ScanDateWord(string pattern)
		{
			this.m_ymdFlags = DateTimeFormatInfoScanner.FoundDatePattern.None;
			for (int i = 0; i < pattern.Length; i++)
			{
				char c = pattern[i];
				char c2 = c;
				if (c2 <= 'M')
				{
					if (c2 == '\'')
					{
						i = this.AddDateWords(pattern, i + 1, null);
						continue;
					}
					if (c2 == '.')
					{
						if (this.m_ymdFlags == DateTimeFormatInfoScanner.FoundDatePattern.FoundYMDPatternFlag)
						{
							this.AddIgnorableSymbols(".");
							this.m_ymdFlags = DateTimeFormatInfoScanner.FoundDatePattern.None;
						}
						i++;
						continue;
					}
					if (c2 == 'M')
					{
						int num;
						i = DateTimeFormatInfoScanner.ScanRepeatChar(pattern, 'M', i, out num);
						if (num >= 4 && i < pattern.Length && pattern[i] == '\'')
						{
							i = this.AddDateWords(pattern, i + 1, "MMMM");
						}
						this.m_ymdFlags |= DateTimeFormatInfoScanner.FoundDatePattern.FoundMonthPatternFlag;
						continue;
					}
				}
				else
				{
					if (c2 == '\\')
					{
						i += 2;
						continue;
					}
					if (c2 != 'd')
					{
						if (c2 == 'y')
						{
							int num;
							i = DateTimeFormatInfoScanner.ScanRepeatChar(pattern, 'y', i, out num);
							this.m_ymdFlags |= DateTimeFormatInfoScanner.FoundDatePattern.FoundYearPatternFlag;
							continue;
						}
					}
					else
					{
						int num;
						i = DateTimeFormatInfoScanner.ScanRepeatChar(pattern, 'd', i, out num);
						if (num <= 2)
						{
							this.m_ymdFlags |= DateTimeFormatInfoScanner.FoundDatePattern.FoundDayPatternFlag;
							continue;
						}
						continue;
					}
				}
				if (this.m_ymdFlags == DateTimeFormatInfoScanner.FoundDatePattern.FoundYMDPatternFlag && !char.IsWhiteSpace(c))
				{
					this.m_ymdFlags = DateTimeFormatInfoScanner.FoundDatePattern.None;
				}
			}
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x00068D50 File Offset: 0x00067D50
		internal string[] GetDateWordsOfDTFI(DateTimeFormatInfo dtfi)
		{
			string[] allDateTimePatterns = dtfi.GetAllDateTimePatterns('D');
			for (int i = 0; i < allDateTimePatterns.Length; i++)
			{
				this.ScanDateWord(allDateTimePatterns[i]);
			}
			allDateTimePatterns = dtfi.GetAllDateTimePatterns('d');
			for (int i = 0; i < allDateTimePatterns.Length; i++)
			{
				this.ScanDateWord(allDateTimePatterns[i]);
			}
			allDateTimePatterns = dtfi.GetAllDateTimePatterns('y');
			for (int i = 0; i < allDateTimePatterns.Length; i++)
			{
				this.ScanDateWord(allDateTimePatterns[i]);
			}
			this.ScanDateWord(dtfi.MonthDayPattern);
			allDateTimePatterns = dtfi.GetAllDateTimePatterns('T');
			for (int i = 0; i < allDateTimePatterns.Length; i++)
			{
				this.ScanDateWord(allDateTimePatterns[i]);
			}
			allDateTimePatterns = dtfi.GetAllDateTimePatterns('t');
			for (int i = 0; i < allDateTimePatterns.Length; i++)
			{
				this.ScanDateWord(allDateTimePatterns[i]);
			}
			string[] array = null;
			if (this.m_dateWords != null && this.m_dateWords.Count > 0)
			{
				array = new string[this.m_dateWords.Count];
				for (int i = 0; i < this.m_dateWords.Count; i++)
				{
					array[i] = (string)this.m_dateWords[i];
				}
			}
			return array;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x00068E5D File Offset: 0x00067E5D
		internal static FORMATFLAGS GetFormatFlagGenitiveMonth(string[] monthNames, string[] genitveMonthNames, string[] abbrevMonthNames, string[] genetiveAbbrevMonthNames)
		{
			if (DateTimeFormatInfoScanner.EqualStringArrays(monthNames, genitveMonthNames) && DateTimeFormatInfoScanner.EqualStringArrays(abbrevMonthNames, genetiveAbbrevMonthNames))
			{
				return FORMATFLAGS.None;
			}
			return FORMATFLAGS.UseGenitiveMonth;
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00068E74 File Offset: 0x00067E74
		internal static FORMATFLAGS GetFormatFlagUseSpaceInMonthNames(string[] monthNames, string[] genitveMonthNames, string[] abbrevMonthNames, string[] genetiveAbbrevMonthNames)
		{
			FORMATFLAGS formatflags = FORMATFLAGS.None;
			formatflags |= ((DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(monthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(genitveMonthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(abbrevMonthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(genetiveAbbrevMonthNames)) ? FORMATFLAGS.UseDigitPrefixInTokens : FORMATFLAGS.None);
			return formatflags | ((DateTimeFormatInfoScanner.ArrayElementsHaveSpace(monthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(genitveMonthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(abbrevMonthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(genetiveAbbrevMonthNames)) ? FORMATFLAGS.UseSpacesInMonthNames : FORMATFLAGS.None);
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x00068ED3 File Offset: 0x00067ED3
		internal static FORMATFLAGS GetFormatFlagUseSpaceInDayNames(string[] dayNames, string[] abbrevDayNames)
		{
			if (!DateTimeFormatInfoScanner.ArrayElementsHaveSpace(dayNames) && !DateTimeFormatInfoScanner.ArrayElementsHaveSpace(abbrevDayNames))
			{
				return FORMATFLAGS.None;
			}
			return FORMATFLAGS.UseSpacesInDayNames;
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x00068EE9 File Offset: 0x00067EE9
		internal static FORMATFLAGS GetFormatFlagUseHebrewCalendar(int calID)
		{
			if (calID != 8)
			{
				return FORMATFLAGS.None;
			}
			return (FORMATFLAGS)10;
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x00068EF4 File Offset: 0x00067EF4
		private static bool EqualStringArrays(string[] array1, string[] array2)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array1.Length; i++)
			{
				if (!array1[i].Equals(array2[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x00068F2C File Offset: 0x00067F2C
		private static bool ArrayElementsHaveSpace(string[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < array[i].Length; j++)
				{
					if (char.IsWhiteSpace(array[i][j]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x00068F70 File Offset: 0x00067F70
		private static bool ArrayElementsBeginWithDigit(string[] array)
		{
			int i = 0;
			while (i < array.Length)
			{
				if (array[i].Length > 0 && array[i][0] >= '0' && array[i][0] <= '9')
				{
					int num = 1;
					while (num < array[i].Length && array[i][num] >= '0' && array[i][num] <= '9')
					{
						num++;
					}
					if (num == array[i].Length)
					{
						return false;
					}
					if (num == array[i].Length - 1)
					{
						char c = array[i][num];
						if (c == '月' || c == '월')
						{
							return false;
						}
					}
					return true;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x040010F3 RID: 4339
		internal const char MonthPostfixChar = '';

		// Token: 0x040010F4 RID: 4340
		internal const char IgnorableSymbolChar = '';

		// Token: 0x040010F5 RID: 4341
		internal const string CJKYearSuff = "年";

		// Token: 0x040010F6 RID: 4342
		internal const string CJKMonthSuff = "月";

		// Token: 0x040010F7 RID: 4343
		internal const string CJKDaySuff = "日";

		// Token: 0x040010F8 RID: 4344
		internal const string KoreanYearSuff = "년";

		// Token: 0x040010F9 RID: 4345
		internal const string KoreanMonthSuff = "월";

		// Token: 0x040010FA RID: 4346
		internal const string KoreanDaySuff = "일";

		// Token: 0x040010FB RID: 4347
		internal const string KoreanHourSuff = "시";

		// Token: 0x040010FC RID: 4348
		internal const string KoreanMinuteSuff = "분";

		// Token: 0x040010FD RID: 4349
		internal const string KoreanSecondSuff = "초";

		// Token: 0x040010FE RID: 4350
		internal const string CJKHourSuff = "時";

		// Token: 0x040010FF RID: 4351
		internal const string ChineseHourSuff = "时";

		// Token: 0x04001100 RID: 4352
		internal const string CJKMinuteSuff = "分";

		// Token: 0x04001101 RID: 4353
		internal const string CJKSecondSuff = "秒";

		// Token: 0x04001102 RID: 4354
		internal ArrayList m_dateWords = new ArrayList();

		// Token: 0x04001103 RID: 4355
		internal static Hashtable m_knownWords;

		// Token: 0x04001104 RID: 4356
		private DateTimeFormatInfoScanner.FoundDatePattern m_ymdFlags;

		// Token: 0x020003AE RID: 942
		private enum FoundDatePattern
		{
			// Token: 0x04001106 RID: 4358
			None,
			// Token: 0x04001107 RID: 4359
			FoundYearPatternFlag,
			// Token: 0x04001108 RID: 4360
			FoundMonthPatternFlag,
			// Token: 0x04001109 RID: 4361
			FoundDayPatternFlag = 4,
			// Token: 0x0400110A RID: 4362
			FoundYMDPatternFlag = 7
		}
	}
}
