using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace System
{
	// Token: 0x02000396 RID: 918
	internal static class DateTimeFormat
	{
		// Token: 0x06002491 RID: 9361 RVA: 0x0005E738 File Offset: 0x0005D738
		private unsafe static void FormatDigits(StringBuilder outputBuffer, int value, int len)
		{
			if (len > 2)
			{
				len = 2;
			}
			char* ptr = stackalloc char[2 * 16];
			char* ptr2 = ptr + 16;
			int num = value;
			do
			{
				*(--ptr2) = (char)(num % 10 + 48);
				num /= 10;
			}
			while (num != 0 && ptr2 != ptr);
			int num2 = (int)((long)(ptr + 16 - ptr2));
			while (num2 < len && ptr2 != ptr)
			{
				*(--ptr2) = '0';
				num2++;
			}
			outputBuffer.Append(ptr2, num2);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x0005E7A2 File Offset: 0x0005D7A2
		private static void HebrewFormatDigits(StringBuilder outputBuffer, int digits)
		{
			outputBuffer.Append(HebrewNumber.ToString(digits));
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x0005E7B4 File Offset: 0x0005D7B4
		private static int ParseRepeatPattern(string format, int pos, char patternChar)
		{
			int length = format.Length;
			int num = pos + 1;
			while (num < length && format[num] == patternChar)
			{
				num++;
			}
			return num - pos;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x0005E7E3 File Offset: 0x0005D7E3
		private static string FormatDayOfWeek(int dayOfWeek, int repeat, DateTimeFormatInfo dtfi)
		{
			if (repeat == 3)
			{
				return dtfi.GetAbbreviatedDayName((DayOfWeek)dayOfWeek);
			}
			return dtfi.GetDayName((DayOfWeek)dayOfWeek);
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x0005E7F8 File Offset: 0x0005D7F8
		private static string FormatMonth(int month, int repeatCount, DateTimeFormatInfo dtfi)
		{
			if (repeatCount == 3)
			{
				return dtfi.GetAbbreviatedMonthName(month);
			}
			return dtfi.GetMonthName(month);
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x0005E810 File Offset: 0x0005D810
		private static string FormatHebrewMonthName(DateTime time, int month, int repeatCount, DateTimeFormatInfo dtfi)
		{
			if (dtfi.Calendar.IsLeapYear(dtfi.Calendar.GetYear(time)))
			{
				return dtfi.internalGetMonthName(month, MonthNameStyles.LeapYear, repeatCount == 3);
			}
			if (month >= 7)
			{
				month++;
			}
			if (repeatCount == 3)
			{
				return dtfi.GetAbbreviatedMonthName(month);
			}
			return dtfi.GetMonthName(month);
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0005E860 File Offset: 0x0005D860
		internal static int ParseQuoteString(string format, int pos, StringBuilder result)
		{
			int length = format.Length;
			int num = pos;
			char c = format[pos++];
			bool flag = false;
			while (pos < length)
			{
				char c2 = format[pos++];
				if (c2 == c)
				{
					flag = true;
					break;
				}
				if (c2 == '\\')
				{
					if (pos >= length)
					{
						throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
					}
					result.Append(format[pos++]);
				}
				else
				{
					result.Append(c2);
				}
			}
			if (!flag)
			{
				throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_BadQuote"), new object[]
				{
					c
				}));
			}
			return pos - num;
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0005E90E File Offset: 0x0005D90E
		private static int ParseNextChar(string format, int pos)
		{
			if (pos >= format.Length - 1)
			{
				return -1;
			}
			return (int)format[pos + 1];
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x0005E928 File Offset: 0x0005D928
		private static bool IsUseGenitiveForm(string format, int index, int tokenLen, char patternToMatch)
		{
			int num = 0;
			int num2 = index - 1;
			while (num2 >= 0 && format[num2] != patternToMatch)
			{
				num2--;
			}
			if (num2 >= 0)
			{
				while (--num2 >= 0 && format[num2] == patternToMatch)
				{
					num++;
				}
				if (num <= 1)
				{
					return true;
				}
			}
			num2 = index + tokenLen;
			while (num2 < format.Length && format[num2] != patternToMatch)
			{
				num2++;
			}
			if (num2 < format.Length)
			{
				num = 0;
				while (++num2 < format.Length && format[num2] == patternToMatch)
				{
					num++;
				}
				if (num <= 1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0005E9C0 File Offset: 0x0005D9C0
		private static string FormatCustomized(DateTime dateTime, string format, DateTimeFormatInfo dtfi, TimeSpan offset)
		{
			Calendar calendar = dtfi.Calendar;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = calendar.ID == 8;
			bool flag2 = calendar.ID == 3;
			bool timeOnly = true;
			int i = 0;
			while (i < format.Length)
			{
				char c = format[i];
				char c2 = c;
				int num2;
				if (c2 <= 'H')
				{
					if (c2 <= '\'')
					{
						if (c2 != '"')
						{
							switch (c2)
							{
							case '%':
							{
								int num = DateTimeFormat.ParseNextChar(format, i);
								if (num >= 0 && num != 37)
								{
									stringBuilder.Append(DateTimeFormat.FormatCustomized(dateTime, ((char)num).ToString(), dtfi, offset));
									num2 = 2;
									goto IL_63F;
								}
								throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
							}
							case '&':
								goto IL_633;
							case '\'':
								break;
							default:
								goto IL_633;
							}
						}
						StringBuilder stringBuilder2 = new StringBuilder();
						num2 = DateTimeFormat.ParseQuoteString(format, i, stringBuilder2);
						stringBuilder.Append(stringBuilder2);
					}
					else if (c2 != '/')
					{
						if (c2 != ':')
						{
							switch (c2)
							{
							case 'F':
								goto IL_1C5;
							case 'G':
								goto IL_633;
							case 'H':
								num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
								DateTimeFormat.FormatDigits(stringBuilder, dateTime.Hour, num2);
								break;
							default:
								goto IL_633;
							}
						}
						else
						{
							stringBuilder.Append(dtfi.TimeSeparator);
							num2 = 1;
						}
					}
					else
					{
						stringBuilder.Append(dtfi.DateSeparator);
						num2 = 1;
					}
				}
				else if (c2 <= 'h')
				{
					switch (c2)
					{
					case 'K':
						num2 = 1;
						DateTimeFormat.FormatCustomizedRoundripTimeZone(dateTime, offset, stringBuilder);
						break;
					case 'L':
						goto IL_633;
					case 'M':
					{
						num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
						int month = calendar.GetMonth(dateTime);
						if (num2 <= 2)
						{
							if (flag)
							{
								DateTimeFormat.HebrewFormatDigits(stringBuilder, month);
							}
							else
							{
								DateTimeFormat.FormatDigits(stringBuilder, month, num2);
							}
						}
						else if (flag)
						{
							stringBuilder.Append(DateTimeFormat.FormatHebrewMonthName(dateTime, month, num2, dtfi));
						}
						else if ((dtfi.FormatFlags & DateTimeFormatFlags.UseGenitiveMonth) != DateTimeFormatFlags.None && num2 >= 4)
						{
							stringBuilder.Append(dtfi.internalGetMonthName(month, DateTimeFormat.IsUseGenitiveForm(format, i, num2, 'd') ? MonthNameStyles.Genitive : MonthNameStyles.Regular, false));
						}
						else
						{
							stringBuilder.Append(DateTimeFormat.FormatMonth(month, num2, dtfi));
						}
						timeOnly = false;
						break;
					}
					default:
						if (c2 != '\\')
						{
							switch (c2)
							{
							case 'd':
								num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
								if (num2 <= 2)
								{
									int dayOfMonth = calendar.GetDayOfMonth(dateTime);
									if (flag)
									{
										DateTimeFormat.HebrewFormatDigits(stringBuilder, dayOfMonth);
									}
									else
									{
										DateTimeFormat.FormatDigits(stringBuilder, dayOfMonth, num2);
									}
								}
								else
								{
									int dayOfWeek = (int)calendar.GetDayOfWeek(dateTime);
									stringBuilder.Append(DateTimeFormat.FormatDayOfWeek(dayOfWeek, num2, dtfi));
								}
								timeOnly = false;
								break;
							case 'e':
								goto IL_633;
							case 'f':
								goto IL_1C5;
							case 'g':
								num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
								stringBuilder.Append(dtfi.GetEraName(calendar.GetEra(dateTime)));
								break;
							case 'h':
							{
								num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
								int num3 = dateTime.Hour % 12;
								if (num3 == 0)
								{
									num3 = 12;
								}
								DateTimeFormat.FormatDigits(stringBuilder, num3, num2);
								break;
							}
							default:
								goto IL_633;
							}
						}
						else
						{
							int num = DateTimeFormat.ParseNextChar(format, i);
							if (num < 0)
							{
								throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
							}
							stringBuilder.Append((char)num);
							num2 = 2;
						}
						break;
					}
				}
				else if (c2 != 'm')
				{
					switch (c2)
					{
					case 's':
						num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
						DateTimeFormat.FormatDigits(stringBuilder, dateTime.Second, num2);
						break;
					case 't':
						num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
						if (num2 == 1)
						{
							if (dateTime.Hour < 12)
							{
								if (dtfi.AMDesignator.Length >= 1)
								{
									stringBuilder.Append(dtfi.AMDesignator[0]);
								}
							}
							else if (dtfi.PMDesignator.Length >= 1)
							{
								stringBuilder.Append(dtfi.PMDesignator[0]);
							}
						}
						else
						{
							stringBuilder.Append((dateTime.Hour < 12) ? dtfi.AMDesignator : dtfi.PMDesignator);
						}
						break;
					default:
						switch (c2)
						{
						case 'y':
						{
							int year = calendar.GetYear(dateTime);
							num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							if (flag2 && !GregorianCalendarHelper.FormatJapaneseFirstYearAsANumber && year == 1 && ((i + num2 < format.Length && format[i + num2] == "年"[0]) || (i + num2 < format.Length - 1 && format[i + num2] == '\'' && format[i + num2 + 1] == "年"[0])))
							{
								stringBuilder.Append("元"[0]);
							}
							else if (dtfi.HasForceTwoDigitYears)
							{
								DateTimeFormat.FormatDigits(stringBuilder, year, (num2 <= 2) ? num2 : 2);
							}
							else if (calendar.ID == 8)
							{
								DateTimeFormat.HebrewFormatDigits(stringBuilder, year);
							}
							else if (num2 <= 2)
							{
								DateTimeFormat.FormatDigits(stringBuilder, year % 100, num2);
							}
							else
							{
								string format2 = "D" + num2;
								stringBuilder.Append(year.ToString(format2, CultureInfo.InvariantCulture));
							}
							timeOnly = false;
							break;
						}
						case 'z':
							num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							DateTimeFormat.FormatCustomizedTimeZone(dateTime, offset, format, num2, timeOnly, stringBuilder);
							break;
						default:
							goto IL_633;
						}
						break;
					}
				}
				else
				{
					num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
					DateTimeFormat.FormatDigits(stringBuilder, dateTime.Minute, num2);
				}
				IL_63F:
				i += num2;
				continue;
				IL_1C5:
				num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
				if (num2 > 7)
				{
					throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
				}
				long num4 = dateTime.Ticks % 10000000L;
				num4 /= (long)Math.Pow(10.0, (double)(7 - num2));
				if (c == 'f')
				{
					stringBuilder.Append(((int)num4).ToString(DateTimeFormat.fixedNumberFormats[num2 - 1], CultureInfo.InvariantCulture));
					goto IL_63F;
				}
				int num5 = num2;
				while (num5 > 0 && num4 % 10L == 0L)
				{
					num4 /= 10L;
					num5--;
				}
				if (num5 > 0)
				{
					stringBuilder.Append(((int)num4).ToString(DateTimeFormat.fixedNumberFormats[num5 - 1], CultureInfo.InvariantCulture));
					goto IL_63F;
				}
				if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '.')
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
					goto IL_63F;
				}
				goto IL_63F;
				IL_633:
				stringBuilder.Append(c);
				num2 = 1;
				goto IL_63F;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x0005F028 File Offset: 0x0005E028
		private static void FormatCustomizedTimeZone(DateTime dateTime, TimeSpan offset, string format, int tokenLen, bool timeOnly, StringBuilder result)
		{
			bool flag = offset == DateTimeFormat.NullOffset;
			if (flag)
			{
				if (timeOnly && dateTime.Ticks < 864000000000L)
				{
					offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
				}
				else
				{
					if (dateTime.Kind == DateTimeKind.Utc)
					{
						DateTimeFormat.InvalidFormatForUtc(format, dateTime);
						dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
					}
					offset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
				}
			}
			if (offset >= TimeSpan.Zero)
			{
				result.Append('+');
			}
			else
			{
				result.Append('-');
				offset = offset.Negate();
			}
			if (tokenLen <= 1)
			{
				result.AppendFormat(CultureInfo.InvariantCulture, "{0:0}", new object[]
				{
					offset.Hours
				});
				return;
			}
			result.AppendFormat(CultureInfo.InvariantCulture, "{0:00}", new object[]
			{
				offset.Hours
			});
			if (tokenLen >= 3)
			{
				result.AppendFormat(CultureInfo.InvariantCulture, ":{0:00}", new object[]
				{
					offset.Minutes
				});
			}
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x0005F144 File Offset: 0x0005E144
		private static void FormatCustomizedRoundripTimeZone(DateTime dateTime, TimeSpan offset, StringBuilder result)
		{
			if (offset == DateTimeFormat.NullOffset)
			{
				switch (dateTime.Kind)
				{
				case DateTimeKind.Utc:
					result.Append("Z");
					return;
				case DateTimeKind.Local:
					offset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
					break;
				default:
					return;
				}
			}
			if (offset >= TimeSpan.Zero)
			{
				result.Append('+');
			}
			else
			{
				result.Append('-');
				offset = offset.Negate();
			}
			result.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}", new object[]
			{
				offset.Hours,
				offset.Minutes
			});
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x0005F1F4 File Offset: 0x0005E1F4
		internal static string GetRealFormat(string format, DateTimeFormatInfo dtfi)
		{
			char c = format[0];
			if (c > 'U')
			{
				if (c != 'Y')
				{
					switch (c)
					{
					case 'd':
						return dtfi.ShortDatePattern;
					case 'e':
						goto IL_159;
					case 'f':
						return dtfi.LongDatePattern + " " + dtfi.ShortTimePattern;
					case 'g':
						return dtfi.GeneralShortTimePattern;
					default:
						switch (c)
						{
						case 'm':
							goto IL_109;
						case 'n':
						case 'p':
						case 'q':
						case 'v':
						case 'w':
						case 'x':
							goto IL_159;
						case 'o':
							goto IL_112;
						case 'r':
							goto IL_11A;
						case 's':
							return dtfi.SortableDateTimePattern;
						case 't':
							return dtfi.ShortTimePattern;
						case 'u':
							return dtfi.UniversalSortableDateTimePattern;
						case 'y':
							break;
						default:
							goto IL_159;
						}
						break;
					}
				}
				return dtfi.YearMonthPattern;
			}
			switch (c)
			{
			case 'D':
				return dtfi.LongDatePattern;
			case 'E':
				goto IL_159;
			case 'F':
				return dtfi.FullDateTimePattern;
			case 'G':
				return dtfi.GeneralLongTimePattern;
			default:
				switch (c)
				{
				case 'M':
					break;
				case 'N':
				case 'P':
				case 'Q':
				case 'S':
					goto IL_159;
				case 'O':
					goto IL_112;
				case 'R':
					goto IL_11A;
				case 'T':
					return dtfi.LongTimePattern;
				case 'U':
					return dtfi.FullDateTimePattern;
				default:
					goto IL_159;
				}
				break;
			}
			IL_109:
			return dtfi.MonthDayPattern;
			IL_112:
			return "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
			IL_11A:
			return dtfi.RFC1123Pattern;
			IL_159:
			throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x0005F36C File Offset: 0x0005E36C
		private static string ExpandPredefinedFormat(string format, ref DateTime dateTime, ref DateTimeFormatInfo dtfi, ref TimeSpan offset)
		{
			char c = format[0];
			if (c <= 'R')
			{
				if (c != 'O')
				{
					if (c != 'R')
					{
						goto IL_15B;
					}
					goto IL_5A;
				}
			}
			else if (c != 'U')
			{
				switch (c)
				{
				case 'o':
					break;
				case 'p':
				case 'q':
				case 't':
					goto IL_15B;
				case 'r':
					goto IL_5A;
				case 's':
					dtfi = DateTimeFormatInfo.InvariantInfo;
					goto IL_15B;
				case 'u':
					if (offset != DateTimeFormat.NullOffset)
					{
						dateTime -= offset;
					}
					else if (dateTime.Kind == DateTimeKind.Local)
					{
						DateTimeFormat.InvalidFormatForLocal(format, dateTime);
					}
					dtfi = DateTimeFormatInfo.InvariantInfo;
					goto IL_15B;
				default:
					goto IL_15B;
				}
			}
			else
			{
				if (offset != DateTimeFormat.NullOffset)
				{
					throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
				}
				dtfi = (DateTimeFormatInfo)dtfi.Clone();
				if (dtfi.Calendar.GetType() != typeof(GregorianCalendar))
				{
					dtfi.Calendar = GregorianCalendar.GetDefaultInstance();
				}
				dateTime = dateTime.ToUniversalTime();
				goto IL_15B;
			}
			dtfi = DateTimeFormatInfo.InvariantInfo;
			goto IL_15B;
			IL_5A:
			if (offset != DateTimeFormat.NullOffset)
			{
				dateTime -= offset;
			}
			else if (dateTime.Kind == DateTimeKind.Local)
			{
				DateTimeFormat.InvalidFormatForLocal(format, dateTime);
			}
			dtfi = DateTimeFormatInfo.InvariantInfo;
			IL_15B:
			format = DateTimeFormat.GetRealFormat(format, dtfi);
			return format;
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x0005F4DF File Offset: 0x0005E4DF
		internal static string Format(DateTime dateTime, string format, DateTimeFormatInfo dtfi)
		{
			return DateTimeFormat.Format(dateTime, format, dtfi, DateTimeFormat.NullOffset);
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x0005F4F0 File Offset: 0x0005E4F0
		internal static string Format(DateTime dateTime, string format, DateTimeFormatInfo dtfi, TimeSpan offset)
		{
			if (format == null || format.Length == 0)
			{
				bool flag = false;
				if (dateTime.Ticks < 864000000000L)
				{
					int id = dtfi.Calendar.ID;
					switch (id)
					{
					case 3:
					case 4:
					case 6:
					case 8:
						break;
					case 5:
					case 7:
						goto IL_61;
					default:
						if (id != 13 && id != 23)
						{
							goto IL_61;
						}
						break;
					}
					flag = true;
					dtfi = DateTimeFormatInfo.InvariantInfo;
				}
				IL_61:
				if (offset == DateTimeFormat.NullOffset)
				{
					if (flag)
					{
						format = "s";
					}
					else
					{
						format = "G";
					}
				}
				else if (flag)
				{
					format = "yyyy'-'MM'-'ddTHH':'mm':'ss zzz";
				}
				else
				{
					format = dtfi.DateTimeOffsetPattern;
				}
			}
			if (format.Length == 1)
			{
				format = DateTimeFormat.ExpandPredefinedFormat(format, ref dateTime, ref dtfi, ref offset);
			}
			return DateTimeFormat.FormatCustomized(dateTime, format, dtfi, offset);
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x0005F5B4 File Offset: 0x0005E5B4
		internal static string[] GetAllDateTimes(DateTime dateTime, char format, DateTimeFormatInfo dtfi)
		{
			string[] allDateTimePatterns;
			string[] array;
			if (format <= 'U')
			{
				switch (format)
				{
				case 'D':
				case 'F':
				case 'G':
					break;
				case 'E':
					goto IL_153;
				default:
					switch (format)
					{
					case 'M':
					case 'T':
						break;
					case 'N':
					case 'P':
					case 'Q':
					case 'S':
						goto IL_153;
					case 'O':
					case 'R':
						goto IL_127;
					case 'U':
					{
						DateTime dateTime2 = dateTime.ToUniversalTime();
						allDateTimePatterns = dtfi.GetAllDateTimePatterns(format);
						array = new string[allDateTimePatterns.Length];
						for (int i = 0; i < allDateTimePatterns.Length; i++)
						{
							array[i] = DateTimeFormat.Format(dateTime2, allDateTimePatterns[i], dtfi);
						}
						return array;
					}
					default:
						goto IL_153;
					}
					break;
				}
			}
			else if (format != 'Y')
			{
				switch (format)
				{
				case 'd':
				case 'f':
				case 'g':
					break;
				case 'e':
					goto IL_153;
				default:
					switch (format)
					{
					case 'm':
					case 't':
					case 'y':
						break;
					case 'n':
					case 'p':
					case 'q':
					case 'v':
					case 'w':
					case 'x':
						goto IL_153;
					case 'o':
					case 'r':
					case 's':
					case 'u':
						goto IL_127;
					default:
						goto IL_153;
					}
					break;
				}
			}
			allDateTimePatterns = dtfi.GetAllDateTimePatterns(format);
			array = new string[allDateTimePatterns.Length];
			for (int j = 0; j < allDateTimePatterns.Length; j++)
			{
				array[j] = DateTimeFormat.Format(dateTime, allDateTimePatterns[j], dtfi);
			}
			return array;
			IL_127:
			return new string[]
			{
				DateTimeFormat.Format(dateTime, new string(new char[]
				{
					format
				}), dtfi)
			};
			IL_153:
			throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x0005F728 File Offset: 0x0005E728
		internal static string[] GetAllDateTimes(DateTime dateTime, DateTimeFormatInfo dtfi)
		{
			ArrayList arrayList = new ArrayList(132);
			for (int i = 0; i < DateTimeFormat.allStandardFormats.Length; i++)
			{
				string[] allDateTimes = DateTimeFormat.GetAllDateTimes(dateTime, DateTimeFormat.allStandardFormats[i], dtfi);
				for (int j = 0; j < allDateTimes.Length; j++)
				{
					arrayList.Add(allDateTimes[j]);
				}
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(0, array, 0, arrayList.Count);
			return array;
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x0005F798 File Offset: 0x0005E798
		internal static void InvalidFormatForLocal(string format, DateTime dateTime)
		{
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x0005F79A File Offset: 0x0005E79A
		internal static void InvalidFormatForUtc(string format, DateTime dateTime)
		{
			Mda.DateTimeInvalidLocalFormat();
		}

		// Token: 0x04000FA0 RID: 4000
		internal const int MaxSecondsFractionDigits = 7;

		// Token: 0x04000FA1 RID: 4001
		internal const string RoundtripFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";

		// Token: 0x04000FA2 RID: 4002
		internal const string RoundtripDateTimeUnfixed = "yyyy'-'MM'-'ddTHH':'mm':'ss zzz";

		// Token: 0x04000FA3 RID: 4003
		private const int DEFAULT_ALL_DATETIMES_SIZE = 132;

		// Token: 0x04000FA4 RID: 4004
		internal static readonly TimeSpan NullOffset = TimeSpan.MinValue;

		// Token: 0x04000FA5 RID: 4005
		internal static char[] allStandardFormats = new char[]
		{
			'd',
			'D',
			'f',
			'F',
			'g',
			'G',
			'm',
			'M',
			'o',
			'O',
			'r',
			'R',
			's',
			't',
			'T',
			'u',
			'U',
			'y',
			'Y'
		};

		// Token: 0x04000FA6 RID: 4006
		private static string[] fixedNumberFormats = new string[]
		{
			"0",
			"00",
			"000",
			"0000",
			"00000",
			"000000",
			"0000000"
		};
	}
}
