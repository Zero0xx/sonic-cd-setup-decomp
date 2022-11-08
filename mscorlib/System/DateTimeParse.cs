using System;
using System.Globalization;
using System.Text;

namespace System
{
	// Token: 0x02000397 RID: 919
	internal static class DateTimeParse
	{
		// Token: 0x060024A6 RID: 9382 RVA: 0x0005F844 File Offset: 0x0005E844
		internal static DateTime ParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParseExact(s, format, dtfi, style, ref dateTimeResult))
			{
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x0005F87C File Offset: 0x0005E87C
		internal static DateTime ParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style, out TimeSpan offset)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			offset = TimeSpan.Zero;
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParseExact(s, format, dtfi, style, ref dateTimeResult))
			{
				offset = dateTimeResult.timeZoneOffset;
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x0005F8E4 File Offset: 0x0005E8E4
		internal static bool TryParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style, out DateTime result)
		{
			result = DateTime.MinValue;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParseExact(s, format, dtfi, style, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				return true;
			}
			return false;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x0005F92C File Offset: 0x0005E92C
		internal static bool TryParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style, out DateTime result, out TimeSpan offset)
		{
			result = DateTime.MinValue;
			offset = TimeSpan.Zero;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParseExact(s, format, dtfi, style, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				offset = dateTimeResult.timeZoneOffset;
				return true;
			}
			return false;
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x0005F9A0 File Offset: 0x0005E9A0
		internal static bool TryParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style, ref DateTimeResult result)
		{
			if (s == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "s");
				return false;
			}
			if (format == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "format");
				return false;
			}
			if (s.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (format.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
				return false;
			}
			return DateTimeParse.DoStrictParse(s, format, style, dtfi, ref result);
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x0005FA18 File Offset: 0x0005EA18
		internal static DateTime ParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParseExactMultiple(s, formats, dtfi, style, ref dateTimeResult))
			{
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x0005FA50 File Offset: 0x0005EA50
		internal static DateTime ParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style, out TimeSpan offset)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			offset = TimeSpan.Zero;
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParseExactMultiple(s, formats, dtfi, style, ref dateTimeResult))
			{
				offset = dateTimeResult.timeZoneOffset;
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x0005FAB8 File Offset: 0x0005EAB8
		internal static bool TryParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style, out DateTime result, out TimeSpan offset)
		{
			result = DateTime.MinValue;
			offset = TimeSpan.Zero;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParseExactMultiple(s, formats, dtfi, style, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				offset = dateTimeResult.timeZoneOffset;
				return true;
			}
			return false;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x0005FB2C File Offset: 0x0005EB2C
		internal static bool TryParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style, out DateTime result)
		{
			result = DateTime.MinValue;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParseExactMultiple(s, formats, dtfi, style, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				return true;
			}
			return false;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x0005FB74 File Offset: 0x0005EB74
		internal static bool TryParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style, ref DateTimeResult result)
		{
			if (s == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "s");
				return false;
			}
			if (formats == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "formats");
				return false;
			}
			if (s.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (formats.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
				return false;
			}
			for (int i = 0; i < formats.Length; i++)
			{
				if (formats[i] == null || formats[i].Length == 0)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
					return false;
				}
				DateTimeResult dateTimeResult = default(DateTimeResult);
				dateTimeResult.Init();
				dateTimeResult.flags = result.flags;
				if (DateTimeParse.TryParseExact(s, formats[i], dtfi, style, ref dateTimeResult))
				{
					result.parsedDate = dateTimeResult.parsedDate;
					result.timeZoneOffset = dateTimeResult.timeZoneOffset;
					return true;
				}
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x0005FC64 File Offset: 0x0005EC64
		private static bool MatchWord(ref __DTString str, string target)
		{
			int length = target.Length;
			if (length > str.Value.Length - str.Index)
			{
				return false;
			}
			if (str.CompareInfo.Compare(str.Value, str.Index, length, target, 0, length, CompareOptions.IgnoreCase) != 0)
			{
				return false;
			}
			int num = str.Index + target.Length;
			if (num < str.Value.Length)
			{
				char c = str.Value[num];
				if (char.IsLetter(c))
				{
					return false;
				}
			}
			str.Index = num;
			if (str.Index < str.len)
			{
				str.m_current = str.Value[str.Index];
			}
			return true;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x0005FD0F File Offset: 0x0005ED0F
		private static bool GetTimeZoneName(ref __DTString str)
		{
			return DateTimeParse.MatchWord(ref str, "GMT") || DateTimeParse.MatchWord(ref str, "Z");
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x0005FD30 File Offset: 0x0005ED30
		internal static bool IsDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x0005FD44 File Offset: 0x0005ED44
		private static bool ParseFraction(ref __DTString str, out double result)
		{
			result = 0.0;
			double num = 0.1;
			int num2 = 0;
			char current;
			while (str.GetNext() && DateTimeParse.IsDigit(current = str.m_current))
			{
				result += (double)(current - '0') * num;
				num *= 0.1;
				num2++;
			}
			return num2 > 0;
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x0005FDA4 File Offset: 0x0005EDA4
		private static bool ParseTimeZone(ref __DTString str, ref TimeSpan result)
		{
			int num = 0;
			DTSubString subString = str.GetSubString();
			if (subString.length != 1)
			{
				return false;
			}
			char c = subString[0];
			if (c != '+' && c != '-')
			{
				return false;
			}
			str.ConsumeSubString(subString);
			subString = str.GetSubString();
			if (subString.type != DTSubStringType.Number)
			{
				return false;
			}
			int value = subString.value;
			int length = subString.length;
			int hours;
			if (length == 1 || length == 2)
			{
				hours = value;
				str.ConsumeSubString(subString);
				subString = str.GetSubString();
				if (subString.length == 1 && subString[0] == ':')
				{
					str.ConsumeSubString(subString);
					subString = str.GetSubString();
					if (subString.type != DTSubStringType.Number || subString.length < 1 || subString.length > 2)
					{
						return false;
					}
					num = subString.value;
					str.ConsumeSubString(subString);
				}
			}
			else
			{
				if (length != 3 && length != 4)
				{
					return false;
				}
				hours = value / 100;
				num = value % 100;
				str.ConsumeSubString(subString);
			}
			if (num < 0 || num >= 60)
			{
				return false;
			}
			result = new TimeSpan(hours, num, 0);
			if (c == '-')
			{
				result = result.Negate();
			}
			return true;
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x0005FEC8 File Offset: 0x0005EEC8
		private static bool Lex(DateTimeParse.DS dps, ref __DTString str, ref DateTimeToken dtok, ref DateTimeRawInfo raw, ref DateTimeResult result, ref DateTimeFormatInfo dtfi)
		{
			dtok.dtt = DateTimeParse.DTT.Unk;
			TokenType tokenType;
			int num;
			str.GetRegularToken(out tokenType, out num, dtfi);
			switch (tokenType)
			{
			case TokenType.NumberToken:
			case TokenType.YearNumberToken:
				if (raw.numCount == 3 || num == -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (dps == DateTimeParse.DS.T_NNt && str.Index < str.len - 1)
				{
					char c = str.Value[str.Index];
					if (c == '.')
					{
						DateTimeParse.ParseFraction(ref str, out raw.fraction);
					}
				}
				if ((dps == DateTimeParse.DS.T_NNt || dps == DateTimeParse.DS.T_Nt) && str.Index < str.len - 1)
				{
					char c2 = str.Value[str.Index];
					int num2 = 0;
					while (char.IsWhiteSpace(c2) && str.Index + num2 < str.len - 1)
					{
						num2++;
						c2 = str.Value[str.Index + num2];
					}
					if (c2 == '+' || c2 == '-')
					{
						str.Index += num2;
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0)
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						result.flags |= ParseFlags.TimeZoneUsed;
						if (!DateTimeParse.ParseTimeZone(ref str, ref result.timeZoneOffset))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
					}
				}
				dtok.num = num;
				if (tokenType != TokenType.YearNumberToken)
				{
					int index;
					char current;
					TokenType separatorToken;
					TokenType tokenType2 = separatorToken = str.GetSeparatorToken(dtfi, out index, out current);
					if (separatorToken > TokenType.SEP_YearSuff)
					{
						if (separatorToken <= TokenType.SEP_HourSuff)
						{
							if (separatorToken == TokenType.SEP_MonthSuff || separatorToken == TokenType.SEP_DaySuff)
							{
								dtok.dtt = DateTimeParse.DTT.NumDatesuff;
								dtok.suffix = tokenType2;
								break;
							}
							if (separatorToken != TokenType.SEP_HourSuff)
							{
								goto IL_5C7;
							}
						}
						else if (separatorToken <= TokenType.SEP_SecondSuff)
						{
							if (separatorToken != TokenType.SEP_MinuteSuff && separatorToken != TokenType.SEP_SecondSuff)
							{
								goto IL_5C7;
							}
						}
						else
						{
							if (separatorToken == TokenType.SEP_LocalTimeMark)
							{
								dtok.dtt = DateTimeParse.DTT.NumLocalTimeMark;
								raw.AddNumber(dtok.num);
								break;
							}
							if (separatorToken != TokenType.SEP_DateOrOffset)
							{
								goto IL_5C7;
							}
							if (DateTimeParse.dateParsingStates[(int)dps][4] == DateTimeParse.DS.ERROR && DateTimeParse.dateParsingStates[(int)dps][3] > DateTimeParse.DS.ERROR)
							{
								str.Index = index;
								str.m_current = current;
								dtok.dtt = DateTimeParse.DTT.NumSpace;
							}
							else
							{
								dtok.dtt = DateTimeParse.DTT.NumDatesep;
							}
							raw.AddNumber(dtok.num);
							break;
						}
						dtok.dtt = DateTimeParse.DTT.NumTimesuff;
						dtok.suffix = tokenType2;
						break;
					}
					if (separatorToken <= TokenType.SEP_Am)
					{
						if (separatorToken == TokenType.SEP_End)
						{
							dtok.dtt = DateTimeParse.DTT.NumEnd;
							raw.AddNumber(dtok.num);
							break;
						}
						if (separatorToken == TokenType.SEP_Space)
						{
							dtok.dtt = DateTimeParse.DTT.NumSpace;
							raw.AddNumber(dtok.num);
							break;
						}
						if (separatorToken != TokenType.SEP_Am)
						{
							goto IL_5C7;
						}
					}
					else if (separatorToken <= TokenType.SEP_Date)
					{
						if (separatorToken != TokenType.SEP_Pm)
						{
							if (separatorToken != TokenType.SEP_Date)
							{
								goto IL_5C7;
							}
							dtok.dtt = DateTimeParse.DTT.NumDatesep;
							raw.AddNumber(dtok.num);
							break;
						}
					}
					else
					{
						if (separatorToken == TokenType.SEP_Time)
						{
							dtok.dtt = DateTimeParse.DTT.NumTimesep;
							raw.AddNumber(dtok.num);
							break;
						}
						if (separatorToken != TokenType.SEP_YearSuff)
						{
							goto IL_5C7;
						}
						dtok.num = dtfi.Calendar.ToFourDigitYear(num);
						dtok.dtt = DateTimeParse.DTT.NumDatesuff;
						dtok.suffix = tokenType2;
						break;
					}
					if (raw.timeMark == DateTimeParse.TM.NotSet)
					{
						raw.timeMark = ((tokenType2 == TokenType.SEP_Am) ? DateTimeParse.TM.AM : DateTimeParse.TM.PM);
						dtok.dtt = DateTimeParse.DTT.NumAmpm;
						raw.AddNumber(dtok.num);
						break;
					}
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					break;
					IL_5C7:
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (raw.year == -1)
				{
					raw.year = num;
					int index;
					char current;
					TokenType separatorToken2;
					TokenType tokenType2 = separatorToken2 = str.GetSeparatorToken(dtfi, out index, out current);
					if (separatorToken2 > TokenType.SEP_YearSuff)
					{
						if (separatorToken2 <= TokenType.SEP_HourSuff)
						{
							if (separatorToken2 == TokenType.SEP_MonthSuff || separatorToken2 == TokenType.SEP_DaySuff)
							{
								goto IL_320;
							}
							if (separatorToken2 != TokenType.SEP_HourSuff)
							{
								goto IL_344;
							}
						}
						else if (separatorToken2 != TokenType.SEP_MinuteSuff && separatorToken2 != TokenType.SEP_SecondSuff)
						{
							if (separatorToken2 != TokenType.SEP_DateOrOffset)
							{
								goto IL_344;
							}
							if (DateTimeParse.dateParsingStates[(int)dps][13] == DateTimeParse.DS.ERROR && DateTimeParse.dateParsingStates[(int)dps][12] > DateTimeParse.DS.ERROR)
							{
								str.Index = index;
								str.m_current = current;
								dtok.dtt = DateTimeParse.DTT.YearSpace;
								return true;
							}
							dtok.dtt = DateTimeParse.DTT.YearDateSep;
							return true;
						}
						dtok.dtt = DateTimeParse.DTT.NumTimesuff;
						dtok.suffix = tokenType2;
						return true;
					}
					if (separatorToken2 <= TokenType.SEP_Am)
					{
						if (separatorToken2 == TokenType.SEP_End)
						{
							dtok.dtt = DateTimeParse.DTT.YearEnd;
							return true;
						}
						if (separatorToken2 == TokenType.SEP_Space)
						{
							dtok.dtt = DateTimeParse.DTT.YearSpace;
							return true;
						}
						if (separatorToken2 != TokenType.SEP_Am)
						{
							goto IL_344;
						}
					}
					else if (separatorToken2 != TokenType.SEP_Pm)
					{
						if (separatorToken2 == TokenType.SEP_Date)
						{
							dtok.dtt = DateTimeParse.DTT.YearDateSep;
							return true;
						}
						if (separatorToken2 != TokenType.SEP_YearSuff)
						{
							goto IL_344;
						}
						goto IL_320;
					}
					if (raw.timeMark == DateTimeParse.TM.NotSet)
					{
						raw.timeMark = ((tokenType2 == TokenType.SEP_Am) ? DateTimeParse.TM.AM : DateTimeParse.TM.PM);
						dtok.dtt = DateTimeParse.DTT.YearSpace;
						return true;
					}
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return true;
					IL_320:
					dtok.dtt = DateTimeParse.DTT.NumDatesuff;
					dtok.suffix = tokenType2;
					return true;
					IL_344:
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			case TokenType.Am:
			case TokenType.Pm:
				if (raw.timeMark != DateTimeParse.TM.NotSet)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				raw.timeMark = (DateTimeParse.TM)num;
				break;
			case TokenType.MonthToken:
			{
				if (raw.month != -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				int index;
				char current;
				TokenType separatorToken3 = str.GetSeparatorToken(dtfi, out index, out current);
				if (separatorToken3 <= TokenType.SEP_Space)
				{
					if (separatorToken3 == TokenType.SEP_End)
					{
						dtok.dtt = DateTimeParse.DTT.MonthEnd;
						goto IL_823;
					}
					if (separatorToken3 == TokenType.SEP_Space)
					{
						dtok.dtt = DateTimeParse.DTT.MonthSpace;
						goto IL_823;
					}
				}
				else
				{
					if (separatorToken3 == TokenType.SEP_Date)
					{
						dtok.dtt = DateTimeParse.DTT.MonthDatesep;
						goto IL_823;
					}
					if (separatorToken3 == TokenType.SEP_DateOrOffset)
					{
						if (DateTimeParse.dateParsingStates[(int)dps][8] == DateTimeParse.DS.ERROR && DateTimeParse.dateParsingStates[(int)dps][7] > DateTimeParse.DS.ERROR)
						{
							str.Index = index;
							str.m_current = current;
							dtok.dtt = DateTimeParse.DTT.MonthSpace;
							goto IL_823;
						}
						dtok.dtt = DateTimeParse.DTT.MonthDatesep;
						goto IL_823;
					}
				}
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
				IL_823:
				raw.month = num;
				break;
			}
			case TokenType.EndOfString:
				dtok.dtt = DateTimeParse.DTT.End;
				break;
			case TokenType.DayOfWeekToken:
				if (raw.dayOfWeek != -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				raw.dayOfWeek = num;
				dtok.dtt = DateTimeParse.DTT.DayOfWeek;
				break;
			case TokenType.TimeZoneToken:
				dtok.dtt = DateTimeParse.DTT.TimeZone;
				result.flags |= ParseFlags.TimeZoneUsed;
				result.timeZoneOffset = new TimeSpan(0L);
				result.flags |= ParseFlags.TimeZoneUtc;
				break;
			case TokenType.EraToken:
				if (result.era == -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.era = num;
				dtok.dtt = DateTimeParse.DTT.Era;
				break;
			case TokenType.UnknownToken:
				if (char.IsLetter(str.m_current))
				{
					result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_UnknowDateTimeWord", str.Index);
					return false;
				}
				if (Environment.GetCompatibilityFlag(CompatibilityFlag.DateTimeParseIgnorePunctuation) && (result.flags & ParseFlags.CaptureOffset) == (ParseFlags)0)
				{
					str.GetNext();
					return true;
				}
				if ((str.m_current == '-' || str.m_current == '+') && (result.flags & ParseFlags.TimeZoneUsed) == (ParseFlags)0)
				{
					int index2 = str.Index;
					if (DateTimeParse.ParseTimeZone(ref str, ref result.timeZoneOffset))
					{
						result.flags |= ParseFlags.TimeZoneUsed;
						return true;
					}
					str.Index = index2;
				}
				if (DateTimeParse.VerifyValidPunctuation(ref str))
				{
					return true;
				}
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			case TokenType.HebrewNumber:
			{
				int index;
				char current;
				if (num < 100)
				{
					dtok.num = num;
					raw.AddNumber(dtok.num);
					TokenType separatorToken4 = str.GetSeparatorToken(dtfi, out index, out current);
					if (separatorToken4 <= TokenType.SEP_Space)
					{
						if (separatorToken4 == TokenType.SEP_End)
						{
							dtok.dtt = DateTimeParse.DTT.NumEnd;
							break;
						}
						if (separatorToken4 != TokenType.SEP_Space)
						{
							goto IL_732;
						}
					}
					else if (separatorToken4 != TokenType.SEP_Date)
					{
						if (separatorToken4 != TokenType.SEP_DateOrOffset)
						{
							goto IL_732;
						}
						if (DateTimeParse.dateParsingStates[(int)dps][4] == DateTimeParse.DS.ERROR && DateTimeParse.dateParsingStates[(int)dps][3] > DateTimeParse.DS.ERROR)
						{
							str.Index = index;
							str.m_current = current;
							dtok.dtt = DateTimeParse.DTT.NumSpace;
							break;
						}
						dtok.dtt = DateTimeParse.DTT.NumDatesep;
						break;
					}
					dtok.dtt = DateTimeParse.DTT.NumDatesep;
					break;
					IL_732:
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (raw.year != -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				raw.year = num;
				TokenType separatorToken5 = str.GetSeparatorToken(dtfi, out index, out current);
				if (separatorToken5 != TokenType.SEP_End)
				{
					if (separatorToken5 != TokenType.SEP_Space)
					{
						if (separatorToken5 == TokenType.SEP_DateOrOffset)
						{
							if (DateTimeParse.dateParsingStates[(int)dps][12] > DateTimeParse.DS.ERROR)
							{
								str.Index = index;
								str.m_current = current;
								dtok.dtt = DateTimeParse.DTT.YearSpace;
								break;
							}
						}
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					dtok.dtt = DateTimeParse.DTT.YearSpace;
				}
				else
				{
					dtok.dtt = DateTimeParse.DTT.YearEnd;
				}
				break;
			}
			case TokenType.JapaneseEraToken:
				result.calendar = JapaneseCalendar.GetDefaultInstance();
				dtfi = DateTimeFormatInfo.GetJapaneseCalendarDTFI();
				if (result.era == -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.era = num;
				dtok.dtt = DateTimeParse.DTT.Era;
				break;
			case TokenType.TEraToken:
				result.calendar = TaiwanCalendar.GetDefaultInstance();
				dtfi = DateTimeFormatInfo.GetTaiwanCalendarDTFI();
				if (result.era == -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.era = num;
				dtok.dtt = DateTimeParse.DTT.Era;
				break;
			}
			return true;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000608F4 File Offset: 0x0005F8F4
		private static bool VerifyValidPunctuation(ref __DTString str)
		{
			char c = str.Value[str.Index];
			if (c == '#')
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < str.len; i++)
				{
					c = str.Value[i];
					if (c == '#')
					{
						if (flag)
						{
							if (flag2)
							{
								return false;
							}
							flag2 = true;
						}
						else
						{
							flag = true;
						}
					}
					else if (c == '\0')
					{
						if (!flag2)
						{
							return false;
						}
					}
					else if (!char.IsWhiteSpace(c) && (!flag || flag2))
					{
						return false;
					}
				}
				if (!flag2)
				{
					return false;
				}
				str.GetNext();
				return true;
			}
			else
			{
				if (c == '\0')
				{
					for (int j = str.Index; j < str.len; j++)
					{
						if (str.Value[j] != '\0')
						{
							return false;
						}
					}
					str.Index = str.len;
					return true;
				}
				return false;
			}
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000609B4 File Offset: 0x0005F9B4
		private static bool GetYearMonthDayOrder(string datePattern, DateTimeFormatInfo dtfi, out int order)
		{
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			int num4 = 0;
			bool flag = false;
			int num5 = 0;
			while (num5 < datePattern.Length && num4 < 3)
			{
				char c = datePattern[num5];
				if (c == '\'' || c == '"')
				{
					flag = !flag;
				}
				if (!flag)
				{
					if (c == 'y')
					{
						num = num4++;
						while (num5 + 1 < datePattern.Length)
						{
							if (datePattern[num5 + 1] != 'y')
							{
								break;
							}
							num5++;
						}
					}
					else if (c == 'M')
					{
						num2 = num4++;
						while (num5 + 1 < datePattern.Length)
						{
							if (datePattern[num5 + 1] != 'M')
							{
								break;
							}
							num5++;
						}
					}
					else if (c == 'd')
					{
						int num6 = 1;
						while (num5 + 1 < datePattern.Length && datePattern[num5 + 1] == 'd')
						{
							num6++;
							num5++;
						}
						if (num6 <= 2)
						{
							num3 = num4++;
						}
					}
				}
				num5++;
			}
			if (num == 0 && num2 == 1 && num3 == 2)
			{
				order = 0;
				return true;
			}
			if (num2 == 0 && num3 == 1 && num == 2)
			{
				order = 1;
				return true;
			}
			if (num3 == 0 && num2 == 1 && num == 2)
			{
				order = 2;
				return true;
			}
			if (num == 0 && num3 == 1 && num2 == 2)
			{
				order = 3;
				return true;
			}
			order = -1;
			return false;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00060AF0 File Offset: 0x0005FAF0
		private static bool GetYearMonthOrder(string pattern, DateTimeFormatInfo dtfi, out int order)
		{
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			bool flag = false;
			int num4 = 0;
			while (num4 < pattern.Length && num3 < 2)
			{
				char c = pattern[num4];
				if (c == '\'' || c == '"')
				{
					flag = !flag;
				}
				if (!flag)
				{
					if (c == 'y')
					{
						num = num3++;
						while (num4 + 1 < pattern.Length)
						{
							if (pattern[num4 + 1] != 'y')
							{
								break;
							}
							num4++;
						}
					}
					else if (c == 'M')
					{
						num2 = num3++;
						while (num4 + 1 < pattern.Length && pattern[num4 + 1] == 'M')
						{
							num4++;
						}
					}
				}
				num4++;
			}
			if (num == 0 && num2 == 1)
			{
				order = 4;
				return true;
			}
			if (num2 == 0 && num == 1)
			{
				order = 5;
				return true;
			}
			order = -1;
			return false;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x00060BBC File Offset: 0x0005FBBC
		private static bool GetMonthDayOrder(string pattern, DateTimeFormatInfo dtfi, out int order)
		{
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			bool flag = false;
			int num4 = 0;
			while (num4 < pattern.Length && num3 < 2)
			{
				char c = pattern[num4];
				if (c == '\'' || c == '"')
				{
					flag = !flag;
				}
				if (!flag)
				{
					if (c == 'd')
					{
						int num5 = 1;
						while (num4 + 1 < pattern.Length && pattern[num4 + 1] == 'd')
						{
							num5++;
							num4++;
						}
						if (num5 <= 2)
						{
							num2 = num3++;
						}
					}
					else if (c == 'M')
					{
						num = num3++;
						while (num4 + 1 < pattern.Length && pattern[num4 + 1] == 'M')
						{
							num4++;
						}
					}
				}
				num4++;
			}
			if (num == 0 && num2 == 1)
			{
				order = 6;
				return true;
			}
			if (num2 == 0 && num == 1)
			{
				order = 7;
				return true;
			}
			order = -1;
			return false;
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x00060C96 File Offset: 0x0005FC96
		private static int AdjustYear(ref DateTimeResult result, int year)
		{
			if (year < 100)
			{
				year = result.calendar.ToFourDigitYear(year);
			}
			return year;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00060CAC File Offset: 0x0005FCAC
		private static bool SetDateYMD(ref DateTimeResult result, int year, int month, int day)
		{
			if (result.calendar.IsValidDay(year, month, day, result.era))
			{
				result.SetDate(year, month, day);
				return true;
			}
			return false;
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00060CD0 File Offset: 0x0005FCD0
		private static bool SetDateMDY(ref DateTimeResult result, int month, int day, int year)
		{
			return DateTimeParse.SetDateYMD(ref result, year, month, day);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x00060CDB File Offset: 0x0005FCDB
		private static bool SetDateDMY(ref DateTimeResult result, int day, int month, int year)
		{
			return DateTimeParse.SetDateYMD(ref result, year, month, day);
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00060CE6 File Offset: 0x0005FCE6
		private static bool SetDateYDM(ref DateTimeResult result, int year, int day, int month)
		{
			return DateTimeParse.SetDateYMD(ref result, year, month, day);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x00060CF1 File Offset: 0x0005FCF1
		private static void GetDefaultYear(ref DateTimeResult result, ref DateTimeStyles styles)
		{
			result.Year = result.calendar.GetYear(DateTimeParse.GetDateTimeNow(ref result, ref styles));
			result.flags |= ParseFlags.YearDefault;
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x00060D20 File Offset: 0x0005FD20
		private static bool GetDayOfNN(ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			DateTimeParse.GetDefaultYear(ref result, ref styles);
			int num;
			if (!DateTimeParse.GetMonthDayOrder(dtfi.MonthDayPattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.MonthDayPattern);
				return false;
			}
			if (num == 6)
			{
				if (DateTimeParse.SetDateYMD(ref result, result.Year, number, number2))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (DateTimeParse.SetDateYMD(ref result, result.Year, number2, number))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x00060DE0 File Offset: 0x0005FDE0
		private static bool GetDayOfNNN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			int number3 = raw.GetNumber(2);
			int num;
			if (!DateTimeParse.GetYearMonthDayOrder(dtfi.ShortDatePattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.ShortDatePattern);
				return false;
			}
			if (num == 0)
			{
				if (DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, number), number2, number3))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 1)
			{
				if (DateTimeParse.SetDateMDY(ref result, number, number2, DateTimeParse.AdjustYear(ref result, number3)))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 2)
			{
				if (DateTimeParse.SetDateDMY(ref result, number, number2, DateTimeParse.AdjustYear(ref result, number3)))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 3 && DateTimeParse.SetDateYDM(ref result, DateTimeParse.AdjustYear(ref result, number), number2, number3))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x00060EFC File Offset: 0x0005FEFC
		private static bool GetDayOfMN(ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int num;
			if (!DateTimeParse.GetMonthDayOrder(dtfi.MonthDayPattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.MonthDayPattern);
				return false;
			}
			if (num == 7)
			{
				int num2;
				if (!DateTimeParse.GetYearMonthOrder(dtfi.YearMonthPattern, dtfi, out num2))
				{
					result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.YearMonthPattern);
					return false;
				}
				if (num2 == 5)
				{
					if (!DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, raw.GetNumber(0)), raw.month, 1))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					return true;
				}
			}
			DateTimeParse.GetDefaultYear(ref result, ref styles);
			if (!DateTimeParse.SetDateYMD(ref result, result.Year, raw.month, raw.GetNumber(0)))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			return true;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x00060FD8 File Offset: 0x0005FFD8
		private static bool GetHebrewDayOfNM(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			int num;
			if (!DateTimeParse.GetMonthDayOrder(dtfi.MonthDayPattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.MonthDayPattern);
				return false;
			}
			result.Month = raw.month;
			if (num == 7 && result.calendar.IsValidDay(result.Year, result.Month, raw.GetNumber(0), result.era))
			{
				result.Day = raw.GetNumber(0);
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x0006105C File Offset: 0x0006005C
		private static bool GetDayOfNM(ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int num;
			if (!DateTimeParse.GetMonthDayOrder(dtfi.MonthDayPattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.MonthDayPattern);
				return false;
			}
			if (num == 6)
			{
				int num2;
				if (!DateTimeParse.GetYearMonthOrder(dtfi.YearMonthPattern, dtfi, out num2))
				{
					result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.YearMonthPattern);
					return false;
				}
				if (num2 == 4)
				{
					if (!DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, raw.GetNumber(0)), raw.month, 1))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					return true;
				}
			}
			DateTimeParse.GetDefaultYear(ref result, ref styles);
			if (!DateTimeParse.SetDateYMD(ref result, result.Year, raw.month, raw.GetNumber(0)))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			return true;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x00061138 File Offset: 0x00060138
		private static bool GetDayOfMNN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			int num;
			if (!DateTimeParse.GetYearMonthDayOrder(dtfi.ShortDatePattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.ShortDatePattern);
				return false;
			}
			if (num == 1)
			{
				int year;
				if (result.calendar.IsValidDay(year = DateTimeParse.AdjustYear(ref result, number2), raw.month, number, result.era))
				{
					result.SetDate(year, raw.month, number);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
				if (result.calendar.IsValidDay(year = DateTimeParse.AdjustYear(ref result, number), raw.month, number2, result.era))
				{
					result.SetDate(year, raw.month, number2);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 0)
			{
				int year;
				if (result.calendar.IsValidDay(year = DateTimeParse.AdjustYear(ref result, number), raw.month, number2, result.era))
				{
					result.SetDate(year, raw.month, number2);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
				if (result.calendar.IsValidDay(year = DateTimeParse.AdjustYear(ref result, number2), raw.month, number, result.era))
				{
					result.SetDate(year, raw.month, number);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 2)
			{
				int year;
				if (result.calendar.IsValidDay(year = DateTimeParse.AdjustYear(ref result, number2), raw.month, number, result.era))
				{
					result.SetDate(year, raw.month, number);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
				if (result.calendar.IsValidDay(year = DateTimeParse.AdjustYear(ref result, number), raw.month, number2, result.era))
				{
					result.SetDate(year, raw.month, number2);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x0006135C File Offset: 0x0006035C
		private static bool GetDayOfYNN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			string datePattern = dtfi.ShortDatePattern;
			if (dtfi.CultureId == 1079)
			{
				datePattern = dtfi.LongDatePattern;
			}
			int num;
			if (DateTimeParse.GetYearMonthDayOrder(datePattern, dtfi, out num) && num == 3)
			{
				if (DateTimeParse.SetDateYMD(ref result, raw.year, number2, number))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (DateTimeParse.SetDateYMD(ref result, raw.year, number, number2))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00061418 File Offset: 0x00060418
		private static bool GetDayOfNNY(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			int num;
			if (!DateTimeParse.GetYearMonthDayOrder(dtfi.ShortDatePattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.ShortDatePattern);
				return false;
			}
			if (num == 1 || num == 0)
			{
				if (DateTimeParse.SetDateYMD(ref result, raw.year, number, number2))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (DateTimeParse.SetDateYMD(ref result, raw.year, number2, number))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000614D4 File Offset: 0x000604D4
		private static bool GetDayOfYMN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (DateTimeParse.SetDateYMD(ref result, raw.year, raw.month, raw.GetNumber(0)))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x0006153C File Offset: 0x0006053C
		private static bool GetDayOfYN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (DateTimeParse.SetDateYMD(ref result, raw.year, raw.GetNumber(0), 1))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000615A0 File Offset: 0x000605A0
		private static bool GetDayOfYM(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (DateTimeParse.SetDateYMD(ref result, raw.year, raw.month, 1))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00061604 File Offset: 0x00060604
		private static void AdjustTimeMark(DateTimeFormatInfo dtfi, ref DateTimeRawInfo raw)
		{
			if (raw.timeMark == DateTimeParse.TM.NotSet && dtfi.AMDesignator != null && dtfi.PMDesignator != null)
			{
				if (dtfi.AMDesignator.Length == 0 && dtfi.PMDesignator.Length != 0)
				{
					raw.timeMark = DateTimeParse.TM.AM;
				}
				if (dtfi.PMDesignator.Length == 0 && dtfi.AMDesignator.Length != 0)
				{
					raw.timeMark = DateTimeParse.TM.PM;
				}
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x0006166C File Offset: 0x0006066C
		private static bool AdjustHour(ref int hour, DateTimeParse.TM timeMark)
		{
			if (timeMark != DateTimeParse.TM.NotSet)
			{
				if (timeMark == DateTimeParse.TM.AM)
				{
					if (hour < 0 || hour > 12)
					{
						return false;
					}
					hour = ((hour == 12) ? 0 : hour);
				}
				else
				{
					if (hour < 0 || hour > 23)
					{
						return false;
					}
					if (hour < 12)
					{
						hour += 12;
					}
				}
			}
			return true;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000616AC File Offset: 0x000606AC
		private static bool GetTimeOfN(DateTimeFormatInfo dtfi, ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if ((result.flags & ParseFlags.HaveTime) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (raw.timeMark == DateTimeParse.TM.NotSet)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Hour = raw.GetNumber(0);
			result.flags |= ParseFlags.HaveTime;
			return true;
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00061708 File Offset: 0x00060708
		private static bool GetTimeOfNN(DateTimeFormatInfo dtfi, ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if ((result.flags & ParseFlags.HaveTime) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Hour = raw.GetNumber(0);
			result.Minute = raw.GetNumber(1);
			result.flags |= ParseFlags.HaveTime;
			return true;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x0006175C File Offset: 0x0006075C
		private static bool GetTimeOfNNN(DateTimeFormatInfo dtfi, ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if ((result.flags & ParseFlags.HaveTime) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Hour = raw.GetNumber(0);
			result.Minute = raw.GetNumber(1);
			result.Second = raw.GetNumber(2);
			result.flags |= ParseFlags.HaveTime;
			return true;
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000617BA File Offset: 0x000607BA
		private static bool GetDateOfDSN(ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if (raw.numCount != 1 || result.Day != -1)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Day = raw.GetNumber(0);
			return true;
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000617EC File Offset: 0x000607EC
		private static bool GetDateOfNDS(ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if (result.Month == -1)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (result.Year != -1)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Year = DateTimeParse.AdjustYear(ref result, raw.GetNumber(0));
			result.Day = 1;
			return true;
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x00061844 File Offset: 0x00060844
		private static bool GetDateOfNNDS(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveYear) != (ParseFlags)0)
			{
				if ((result.flags & ParseFlags.HaveMonth) == (ParseFlags)0 && (result.flags & ParseFlags.HaveDay) == (ParseFlags)0 && DateTimeParse.SetDateYMD(ref result, result.Year = DateTimeParse.AdjustYear(ref result, raw.year), raw.GetNumber(0), raw.GetNumber(1)))
				{
					return true;
				}
			}
			else if ((result.flags & ParseFlags.HaveMonth) != (ParseFlags)0 && (result.flags & ParseFlags.HaveYear) == (ParseFlags)0 && (result.flags & ParseFlags.HaveDay) == (ParseFlags)0)
			{
				int num;
				if (!DateTimeParse.GetYearMonthDayOrder(dtfi.ShortDatePattern, dtfi, out num))
				{
					result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.ShortDatePattern);
					return false;
				}
				if (num == 0)
				{
					if (DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, raw.GetNumber(0)), result.Month, raw.GetNumber(1)))
					{
						return true;
					}
				}
				else if (DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, raw.GetNumber(1)), result.Month, raw.GetNumber(0)))
				{
					return true;
				}
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x00061944 File Offset: 0x00060944
		private static bool ProcessDateTimeSuffix(ref DateTimeResult result, ref DateTimeRawInfo raw, ref DateTimeToken dtok)
		{
			TokenType suffix = dtok.suffix;
			if (suffix <= TokenType.SEP_DaySuff)
			{
				if (suffix != TokenType.SEP_YearSuff)
				{
					if (suffix != TokenType.SEP_MonthSuff)
					{
						if (suffix == TokenType.SEP_DaySuff)
						{
							if ((result.flags & ParseFlags.HaveDay) != (ParseFlags)0)
							{
								return false;
							}
							result.flags |= ParseFlags.HaveDay;
							result.Day = dtok.num;
						}
					}
					else
					{
						if ((result.flags & ParseFlags.HaveMonth) != (ParseFlags)0)
						{
							return false;
						}
						result.flags |= ParseFlags.HaveMonth;
						result.Month = (raw.month = dtok.num);
					}
				}
				else
				{
					if ((result.flags & ParseFlags.HaveYear) != (ParseFlags)0)
					{
						return false;
					}
					result.flags |= ParseFlags.HaveYear;
					result.Year = (raw.year = dtok.num);
				}
			}
			else if (suffix != TokenType.SEP_HourSuff)
			{
				if (suffix != TokenType.SEP_MinuteSuff)
				{
					if (suffix == TokenType.SEP_SecondSuff)
					{
						if ((result.flags & ParseFlags.HaveSecond) != (ParseFlags)0)
						{
							return false;
						}
						result.flags |= ParseFlags.HaveSecond;
						result.Second = dtok.num;
					}
				}
				else
				{
					if ((result.flags & ParseFlags.HaveMinute) != (ParseFlags)0)
					{
						return false;
					}
					result.flags |= ParseFlags.HaveMinute;
					result.Minute = dtok.num;
				}
			}
			else
			{
				if ((result.flags & ParseFlags.HaveHour) != (ParseFlags)0)
				{
					return false;
				}
				result.flags |= ParseFlags.HaveHour;
				result.Hour = dtok.num;
			}
			return true;
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x00061AB4 File Offset: 0x00060AB4
		internal static bool ProcessHebrewTerminalState(DateTimeParse.DS dps, ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			switch (dps)
			{
			case DateTimeParse.DS.DX_NM:
				DateTimeParse.GetDefaultYear(ref result, ref styles);
				if (!dtfi.YearMonthAdjustment(ref result.Year, ref raw.month, true))
				{
					result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
					return false;
				}
				if (!DateTimeParse.GetHebrewDayOfNM(ref result, ref raw, dtfi))
				{
					return false;
				}
				goto IL_15C;
			case DateTimeParse.DS.DX_MNN:
				raw.year = raw.GetNumber(1);
				if (!dtfi.YearMonthAdjustment(ref raw.year, ref raw.month, true))
				{
					result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
					return false;
				}
				if (!DateTimeParse.GetDayOfMNN(ref result, ref raw, dtfi))
				{
					return false;
				}
				goto IL_15C;
			case DateTimeParse.DS.DX_YMN:
				if (!dtfi.YearMonthAdjustment(ref raw.year, ref raw.month, true))
				{
					result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
					return false;
				}
				if (!DateTimeParse.GetDayOfYMN(ref result, ref raw, dtfi))
				{
					return false;
				}
				goto IL_15C;
			case DateTimeParse.DS.DX_YM:
				if (!dtfi.YearMonthAdjustment(ref raw.year, ref raw.month, true))
				{
					result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
					return false;
				}
				if (!DateTimeParse.GetDayOfYM(ref result, ref raw, dtfi))
				{
					return false;
				}
				goto IL_15C;
			case DateTimeParse.DS.TX_N:
				if (!DateTimeParse.GetTimeOfN(dtfi, ref result, ref raw))
				{
					return false;
				}
				goto IL_15C;
			case DateTimeParse.DS.TX_NN:
				if (!DateTimeParse.GetTimeOfNN(dtfi, ref result, ref raw))
				{
					return false;
				}
				goto IL_15C;
			case DateTimeParse.DS.TX_NNN:
				if (!DateTimeParse.GetTimeOfNNN(dtfi, ref result, ref raw))
				{
					return false;
				}
				goto IL_15C;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
			IL_15C:
			if (dps > DateTimeParse.DS.ERROR)
			{
				raw.numCount = 0;
			}
			return true;
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x00061C2C File Offset: 0x00060C2C
		internal static bool ProcessTerminaltState(DateTimeParse.DS dps, ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			switch (dps)
			{
			case DateTimeParse.DS.DX_NN:
				if (!DateTimeParse.GetDayOfNN(ref result, ref styles, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NNN:
				if (!DateTimeParse.GetDayOfNNN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_MN:
				if (!DateTimeParse.GetDayOfMN(ref result, ref styles, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NM:
				if (!DateTimeParse.GetDayOfNM(ref result, ref styles, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_MNN:
				if (!DateTimeParse.GetDayOfMNN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_DSN:
				if (!DateTimeParse.GetDateOfDSN(ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NDS:
				if (!DateTimeParse.GetDateOfNDS(ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NNDS:
				if (!DateTimeParse.GetDateOfNNDS(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_YNN:
				if (!DateTimeParse.GetDayOfYNN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_YMN:
				if (!DateTimeParse.GetDayOfYMN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_YN:
				if (!DateTimeParse.GetDayOfYN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_YM:
				if (!DateTimeParse.GetDayOfYM(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.TX_N:
				if (!DateTimeParse.GetTimeOfN(dtfi, ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.TX_NN:
				if (!DateTimeParse.GetTimeOfNN(dtfi, ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.TX_NNN:
				if (!DateTimeParse.GetTimeOfNNN(dtfi, ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NNY:
				if (!DateTimeParse.GetDayOfNNY(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			}
			if (dps > DateTimeParse.DS.ERROR)
			{
				raw.numCount = 0;
			}
			return true;
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x00061D80 File Offset: 0x00060D80
		internal static DateTime Parse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParse(s, dtfi, styles, ref dateTimeResult))
			{
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x00061DB8 File Offset: 0x00060DB8
		internal static DateTime Parse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles, out TimeSpan offset)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParse(s, dtfi, styles, ref dateTimeResult))
			{
				offset = dateTimeResult.timeZoneOffset;
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x00061E10 File Offset: 0x00060E10
		internal static bool TryParse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles, out DateTime result)
		{
			result = DateTime.MinValue;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParse(s, dtfi, styles, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				return true;
			}
			return false;
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x00061E54 File Offset: 0x00060E54
		internal static bool TryParse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles, out DateTime result, out TimeSpan offset)
		{
			result = DateTime.MinValue;
			offset = TimeSpan.Zero;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParse(s, dtfi, styles, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				offset = dateTimeResult.timeZoneOffset;
				return true;
			}
			return false;
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x00061EC4 File Offset: 0x00060EC4
		internal unsafe static bool TryParse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles, ref DateTimeResult result)
		{
			if (s == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "s");
				return false;
			}
			if (s.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			DateTimeParse.DS ds = DateTimeParse.DS.BEGIN;
			bool flag = false;
			DateTimeToken dateTimeToken = default(DateTimeToken);
			dateTimeToken.suffix = TokenType.SEP_Unk;
			DateTimeRawInfo dateTimeRawInfo = default(DateTimeRawInfo);
			int* numberBuffer = stackalloc int[4 * 3];
			dateTimeRawInfo.Init(numberBuffer);
			result.calendar = dtfi.Calendar;
			result.era = 0;
			__DTString _DTString = new __DTString(s, dtfi);
			_DTString.GetNext();
			while (DateTimeParse.Lex(ds, ref _DTString, ref dateTimeToken, ref dateTimeRawInfo, ref result, ref dtfi))
			{
				if (dateTimeToken.dtt != DateTimeParse.DTT.Unk)
				{
					if (dateTimeToken.suffix != TokenType.SEP_Unk)
					{
						if (!DateTimeParse.ProcessDateTimeSuffix(ref result, ref dateTimeRawInfo, ref dateTimeToken))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						dateTimeToken.suffix = TokenType.SEP_Unk;
					}
					if (dateTimeToken.dtt == DateTimeParse.DTT.NumLocalTimeMark)
					{
						if (ds == DateTimeParse.DS.D_YNd || ds == DateTimeParse.DS.D_YN)
						{
							return DateTimeParse.ParseISO8601(ref dateTimeRawInfo, ref _DTString, styles, ref result);
						}
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					else
					{
						ds = DateTimeParse.dateParsingStates[(int)ds][(int)dateTimeToken.dtt];
						if (ds == DateTimeParse.DS.ERROR)
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						if (ds > DateTimeParse.DS.ERROR)
						{
							if ((dtfi.FormatFlags & DateTimeFormatFlags.UseHebrewRule) != DateTimeFormatFlags.None)
							{
								if (!DateTimeParse.ProcessHebrewTerminalState(ds, ref result, ref styles, ref dateTimeRawInfo, dtfi))
								{
									return false;
								}
							}
							else if (!DateTimeParse.ProcessTerminaltState(ds, ref result, ref styles, ref dateTimeRawInfo, dtfi))
							{
								return false;
							}
							flag = true;
							ds = DateTimeParse.DS.BEGIN;
						}
					}
				}
				if (dateTimeToken.dtt == DateTimeParse.DTT.End || dateTimeToken.dtt == DateTimeParse.DTT.NumEnd || dateTimeToken.dtt == DateTimeParse.DTT.MonthEnd)
				{
					if (!flag)
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					DateTimeParse.AdjustTimeMark(dtfi, ref dateTimeRawInfo);
					if (!DateTimeParse.AdjustHour(ref result.Hour, dateTimeRawInfo.timeMark))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					bool bTimeOnly = result.Year == -1 && result.Month == -1 && result.Day == -1;
					if (!DateTimeParse.CheckDefaultDateTime(ref result, ref result.calendar, styles))
					{
						return false;
					}
					DateTime dateTime;
					if (!result.calendar.TryToDateTime(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second, 0, result.era, out dateTime))
					{
						result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
						return false;
					}
					if (dateTimeRawInfo.fraction > 0.0)
					{
						dateTime = dateTime.AddTicks((long)Math.Round(dateTimeRawInfo.fraction * 10000000.0));
					}
					if (dateTimeRawInfo.dayOfWeek != -1 && dateTimeRawInfo.dayOfWeek != (int)result.calendar.GetDayOfWeek(dateTime))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDayOfWeek", null);
						return false;
					}
					result.parsedDate = dateTime;
					return DateTimeParse.DetermineTimeZoneAdjustments(ref result, styles, bTimeOnly);
				}
			}
			return false;
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x00062180 File Offset: 0x00061180
		private static bool DetermineTimeZoneAdjustments(ref DateTimeResult result, DateTimeStyles styles, bool bTimeOnly)
		{
			if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0)
			{
				return DateTimeParse.DateTimeOffsetTimeZonePostProcessing(ref result, styles);
			}
			if ((result.flags & ParseFlags.TimeZoneUsed) == (ParseFlags)0)
			{
				if ((styles & DateTimeStyles.AssumeLocal) != DateTimeStyles.None)
				{
					if ((styles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.None)
					{
						result.parsedDate = DateTime.SpecifyKind(result.parsedDate, DateTimeKind.Local);
						return true;
					}
					result.flags |= ParseFlags.TimeZoneUsed;
					result.timeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(result.parsedDate);
				}
				else
				{
					if ((styles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.None)
					{
						return true;
					}
					if ((styles & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None)
					{
						result.parsedDate = DateTime.SpecifyKind(result.parsedDate, DateTimeKind.Utc);
						return true;
					}
					result.flags |= ParseFlags.TimeZoneUsed;
					result.timeZoneOffset = TimeSpan.Zero;
				}
			}
			if ((styles & DateTimeStyles.RoundtripKind) != DateTimeStyles.None && (result.flags & ParseFlags.TimeZoneUtc) != (ParseFlags)0)
			{
				result.parsedDate = DateTime.SpecifyKind(result.parsedDate, DateTimeKind.Utc);
				return true;
			}
			if ((styles & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None)
			{
				return DateTimeParse.AdjustTimeZoneToUniversal(ref result);
			}
			return DateTimeParse.AdjustTimeZoneToLocal(ref result, bTimeOnly);
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x00062280 File Offset: 0x00061280
		private static bool DateTimeOffsetTimeZonePostProcessing(ref DateTimeResult result, DateTimeStyles styles)
		{
			if ((result.flags & ParseFlags.TimeZoneUsed) == (ParseFlags)0)
			{
				if ((styles & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
				{
					result.timeZoneOffset = TimeSpan.Zero;
				}
				else
				{
					result.timeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(result.parsedDate);
				}
			}
			long ticks = result.timeZoneOffset.Ticks;
			long num = result.parsedDate.Ticks - ticks;
			if (num < 0L || num > 3155378975999999999L)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_UTCOutOfRange", null);
				return false;
			}
			if (ticks < -504000000000L || ticks > 504000000000L)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_OffsetOutOfRange", null);
				return false;
			}
			if ((styles & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None)
			{
				if ((result.flags & ParseFlags.TimeZoneUsed) == (ParseFlags)0 && (styles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.None)
				{
					bool result2 = DateTimeParse.AdjustTimeZoneToUniversal(ref result);
					result.timeZoneOffset = TimeSpan.Zero;
					return result2;
				}
				result.parsedDate = new DateTime(num, DateTimeKind.Utc);
				result.timeZoneOffset = TimeSpan.Zero;
			}
			return true;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x0006236C File Offset: 0x0006136C
		private static bool AdjustTimeZoneToUniversal(ref DateTimeResult result)
		{
			long num = result.parsedDate.Ticks;
			num -= result.timeZoneOffset.Ticks;
			if (num < 0L)
			{
				num += 864000000000L;
			}
			if (num < 0L || num > 3155378975999999999L)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_DateOutOfRange", null);
				return false;
			}
			result.parsedDate = new DateTime(num, DateTimeKind.Utc);
			return true;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000623D4 File Offset: 0x000613D4
		private static bool AdjustTimeZoneToLocal(ref DateTimeResult result, bool bTimeOnly)
		{
			long num = result.parsedDate.Ticks;
			CurrentSystemTimeZone currentSystemTimeZone = (CurrentSystemTimeZone)TimeZone.CurrentTimeZone;
			bool isAmbiguousDst = false;
			if (num < 864000000000L)
			{
				num -= result.timeZoneOffset.Ticks;
				num += currentSystemTimeZone.GetUtcOffset(bTimeOnly ? DateTime.Now : result.parsedDate).Ticks;
				if (num < 0L)
				{
					num += 864000000000L;
				}
			}
			else
			{
				num -= result.timeZoneOffset.Ticks;
				if (num < 0L || num > 3155378975999999999L)
				{
					num += currentSystemTimeZone.GetUtcOffset(result.parsedDate).Ticks;
				}
				else
				{
					num += currentSystemTimeZone.GetUtcOffsetFromUniversalTime(new DateTime(num), ref isAmbiguousDst);
				}
			}
			if (num < 0L || num > 3155378975999999999L)
			{
				result.parsedDate = DateTime.MinValue;
				result.SetFailure(ParseFailureKind.Format, "Format_DateOutOfRange", null);
				return false;
			}
			result.parsedDate = new DateTime(num, DateTimeKind.Local, isAmbiguousDst);
			return true;
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000624CC File Offset: 0x000614CC
		private static bool ParseISO8601(ref DateTimeRawInfo raw, ref __DTString str, DateTimeStyles styles, ref DateTimeResult result)
		{
			if (raw.year >= 0 && raw.GetNumber(0) >= 0)
			{
				raw.GetNumber(1);
			}
			str.Index--;
			int second = 0;
			double num = 0.0;
			str.SkipWhiteSpaces();
			int hour;
			if (!DateTimeParse.ParseDigits(ref str, 2, out hour))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			str.SkipWhiteSpaces();
			if (!str.Match(':'))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			str.SkipWhiteSpaces();
			int minute;
			if (!DateTimeParse.ParseDigits(ref str, 2, out minute))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			str.SkipWhiteSpaces();
			if (str.Match(':'))
			{
				str.SkipWhiteSpaces();
				if (!DateTimeParse.ParseDigits(ref str, 2, out second))
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (str.Match('.'))
				{
					if (!DateTimeParse.ParseFraction(ref str, out num))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					str.Index--;
				}
				str.SkipWhiteSpaces();
			}
			if (str.GetNext())
			{
				char @char = str.GetChar();
				if (@char == '+' || @char == '-')
				{
					result.flags |= ParseFlags.TimeZoneUsed;
					if (!DateTimeParse.ParseTimeZone(ref str, ref result.timeZoneOffset))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
				}
				else if (@char == 'Z' || @char == 'z')
				{
					result.flags |= ParseFlags.TimeZoneUsed;
					result.timeZoneOffset = TimeSpan.Zero;
					result.flags |= ParseFlags.TimeZoneUtc;
				}
				else
				{
					str.Index--;
				}
				str.SkipWhiteSpaces();
				if (str.Match('#'))
				{
					if (!DateTimeParse.VerifyValidPunctuation(ref str))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					str.SkipWhiteSpaces();
				}
				if (str.Match('\0') && !DateTimeParse.VerifyValidPunctuation(ref str))
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (str.GetNext())
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
			}
			Calendar defaultInstance = GregorianCalendar.GetDefaultInstance();
			DateTime parsedDate;
			if (!defaultInstance.TryToDateTime(raw.year, raw.GetNumber(0), raw.GetNumber(1), hour, minute, second, 0, result.era, out parsedDate))
			{
				result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
				return false;
			}
			parsedDate = parsedDate.AddTicks((long)Math.Round(num * 10000000.0));
			result.parsedDate = parsedDate;
			return DateTimeParse.DetermineTimeZoneAdjustments(ref result, styles, false);
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x00062744 File Offset: 0x00061744
		internal static bool MatchHebrewDigits(ref __DTString str, int digitLen, out int number)
		{
			number = 0;
			HebrewNumberParsingContext hebrewNumberParsingContext = new HebrewNumberParsingContext(0);
			HebrewNumberParsingState hebrewNumberParsingState = HebrewNumberParsingState.ContinueParsing;
			while (hebrewNumberParsingState == HebrewNumberParsingState.ContinueParsing && str.GetNext())
			{
				hebrewNumberParsingState = HebrewNumber.ParseByChar(str.GetChar(), ref hebrewNumberParsingContext);
			}
			if (hebrewNumberParsingState == HebrewNumberParsingState.FoundEndOfHebrewNumber)
			{
				number = hebrewNumberParsingContext.result;
				return true;
			}
			return false;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0006278F File Offset: 0x0006178F
		internal static bool ParseDigits(ref __DTString str, int digitLen, out int result)
		{
			if (digitLen == 1)
			{
				return DateTimeParse.ParseDigits(ref str, 1, 2, out result);
			}
			return DateTimeParse.ParseDigits(ref str, digitLen, digitLen, out result);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000627A8 File Offset: 0x000617A8
		internal static bool ParseDigits(ref __DTString str, int minDigitLen, int maxDigitLen, out int result)
		{
			result = 0;
			int index = str.Index;
			int i;
			for (i = 0; i < maxDigitLen; i++)
			{
				if (!str.GetNextDigit())
				{
					str.Index--;
					break;
				}
				result = result * 10 + str.GetDigit();
			}
			if (i < minDigitLen)
			{
				str.Index = index;
				return false;
			}
			return true;
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x00062800 File Offset: 0x00061800
		private static bool ParseFractionExact(ref __DTString str, int maxDigitLen, ref double result)
		{
			if (!str.GetNextDigit())
			{
				str.Index--;
				return false;
			}
			result = (double)str.GetDigit();
			int i;
			for (i = 1; i < maxDigitLen; i++)
			{
				if (!str.GetNextDigit())
				{
					str.Index--;
					break;
				}
				result = result * 10.0 + (double)str.GetDigit();
			}
			result /= Math.Pow(10.0, (double)i);
			return i == maxDigitLen;
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x00062884 File Offset: 0x00061884
		private static bool ParseSign(ref __DTString str, ref bool result)
		{
			if (!str.GetNext())
			{
				return false;
			}
			char @char = str.GetChar();
			if (@char == '+')
			{
				result = true;
				return true;
			}
			if (@char == '-')
			{
				result = false;
				return true;
			}
			return false;
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000628B8 File Offset: 0x000618B8
		private static bool ParseTimeZoneOffset(ref __DTString str, int len, ref TimeSpan result)
		{
			bool flag = true;
			int num = 0;
			int hours;
			switch (len)
			{
			case 1:
			case 2:
				if (!DateTimeParse.ParseSign(ref str, ref flag))
				{
					return false;
				}
				if (!DateTimeParse.ParseDigits(ref str, len, out hours))
				{
					return false;
				}
				break;
			default:
				if (!DateTimeParse.ParseSign(ref str, ref flag))
				{
					return false;
				}
				if (!DateTimeParse.ParseDigits(ref str, 1, out hours))
				{
					return false;
				}
				if (str.Match(":"))
				{
					if (!DateTimeParse.ParseDigits(ref str, 2, out num))
					{
						return false;
					}
				}
				else
				{
					str.Index--;
					if (!DateTimeParse.ParseDigits(ref str, 2, out num))
					{
						return false;
					}
				}
				break;
			}
			if (num < 0 || num >= 60)
			{
				return false;
			}
			result = new TimeSpan(hours, num, 0);
			if (!flag)
			{
				result = result.Negate();
			}
			return true;
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x00062970 File Offset: 0x00061970
		private static bool MatchAbbreviatedMonthName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			int num = 0;
			result = -1;
			if (str.GetNext())
			{
				int num2 = (dtfi.GetMonthName(13).Length == 0) ? 12 : 13;
				for (int i = 1; i <= num2; i++)
				{
					string abbreviatedMonthName = dtfi.GetAbbreviatedMonthName(i);
					int length = abbreviatedMonthName.Length;
					if ((dtfi.HasSpacesInMonthNames ? str.MatchSpecifiedWords(abbreviatedMonthName, false, ref length) : str.MatchSpecifiedWord(abbreviatedMonthName)) && length > num)
					{
						num = length;
						result = i;
					}
				}
				if ((dtfi.FormatFlags & DateTimeFormatFlags.UseLeapYearMonth) != DateTimeFormatFlags.None)
				{
					int num3 = str.MatchLongestWords(dtfi.internalGetLeapYearMonthNames(), ref num);
					if (num3 >= 0)
					{
						result = num3 + 1;
					}
				}
			}
			if (result > 0)
			{
				str.Index += num - 1;
				return true;
			}
			return false;
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x00062A24 File Offset: 0x00061A24
		private static bool MatchMonthName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			int num = 0;
			result = -1;
			if (str.GetNext())
			{
				int num2 = (dtfi.GetMonthName(13).Length == 0) ? 12 : 13;
				for (int i = 1; i <= num2; i++)
				{
					string monthName = dtfi.GetMonthName(i);
					int length = monthName.Length;
					if ((dtfi.HasSpacesInMonthNames ? str.MatchSpecifiedWords(monthName, false, ref length) : str.MatchSpecifiedWord(monthName)) && length > num)
					{
						num = length;
						result = i;
					}
				}
				if ((dtfi.FormatFlags & DateTimeFormatFlags.UseGenitiveMonth) != DateTimeFormatFlags.None)
				{
					int num3 = str.MatchLongestWords(dtfi.MonthGenitiveNames, ref num);
					if (num3 >= 0)
					{
						result = num3 + 1;
					}
				}
				if ((dtfi.FormatFlags & DateTimeFormatFlags.UseLeapYearMonth) != DateTimeFormatFlags.None)
				{
					int num4 = str.MatchLongestWords(dtfi.internalGetLeapYearMonthNames(), ref num);
					if (num4 >= 0)
					{
						result = num4 + 1;
					}
				}
			}
			if (result > 0)
			{
				str.Index += num - 1;
				return true;
			}
			return false;
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x00062B00 File Offset: 0x00061B00
		private static bool MatchAbbreviatedDayName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			int num = 0;
			result = -1;
			if (str.GetNext())
			{
				for (DayOfWeek dayOfWeek = DayOfWeek.Sunday; dayOfWeek <= DayOfWeek.Saturday; dayOfWeek++)
				{
					string abbreviatedDayName = dtfi.GetAbbreviatedDayName(dayOfWeek);
					int length = abbreviatedDayName.Length;
					if ((dtfi.HasSpacesInDayNames ? str.MatchSpecifiedWords(abbreviatedDayName, false, ref length) : str.MatchSpecifiedWord(abbreviatedDayName)) && length > num)
					{
						num = length;
						result = (int)dayOfWeek;
					}
				}
			}
			if (result >= 0)
			{
				str.Index += num - 1;
				return true;
			}
			return false;
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x00062B74 File Offset: 0x00061B74
		private static bool MatchDayName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			int num = 0;
			result = -1;
			if (str.GetNext())
			{
				for (DayOfWeek dayOfWeek = DayOfWeek.Sunday; dayOfWeek <= DayOfWeek.Saturday; dayOfWeek++)
				{
					string dayName = dtfi.GetDayName(dayOfWeek);
					int length = dayName.Length;
					if ((dtfi.HasSpacesInDayNames ? str.MatchSpecifiedWords(dayName, false, ref length) : str.MatchSpecifiedWord(dayName)) && length > num)
					{
						num = length;
						result = (int)dayOfWeek;
					}
				}
			}
			if (result >= 0)
			{
				str.Index += num - 1;
				return true;
			}
			return false;
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x00062BE8 File Offset: 0x00061BE8
		private static bool MatchEraName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			if (str.GetNext())
			{
				int[] eras = dtfi.Calendar.Eras;
				if (eras != null)
				{
					for (int i = 0; i < eras.Length; i++)
					{
						string text = dtfi.GetEraName(eras[i]);
						if (str.MatchSpecifiedWord(text))
						{
							str.Index += text.Length - 1;
							result = eras[i];
							return true;
						}
						text = dtfi.GetAbbreviatedEraName(eras[i]);
						if (str.MatchSpecifiedWord(text))
						{
							str.Index += text.Length - 1;
							result = eras[i];
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x00062C7C File Offset: 0x00061C7C
		private static bool MatchTimeMark(ref __DTString str, DateTimeFormatInfo dtfi, ref DateTimeParse.TM result)
		{
			result = DateTimeParse.TM.NotSet;
			if (dtfi.AMDesignator.Length == 0)
			{
				result = DateTimeParse.TM.AM;
			}
			if (dtfi.PMDesignator.Length == 0)
			{
				result = DateTimeParse.TM.PM;
			}
			if (str.GetNext())
			{
				string text = dtfi.AMDesignator;
				if (text.Length > 0 && str.MatchSpecifiedWord(text))
				{
					str.Index += text.Length - 1;
					result = DateTimeParse.TM.AM;
					return true;
				}
				text = dtfi.PMDesignator;
				if (text.Length > 0 && str.MatchSpecifiedWord(text))
				{
					str.Index += text.Length - 1;
					result = DateTimeParse.TM.PM;
					return true;
				}
				str.Index--;
			}
			return result != DateTimeParse.TM.NotSet;
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00062D30 File Offset: 0x00061D30
		private static bool MatchAbbreviatedTimeMark(ref __DTString str, DateTimeFormatInfo dtfi, ref DateTimeParse.TM result)
		{
			if (str.GetNext())
			{
				if (str.GetChar() == dtfi.AMDesignator[0])
				{
					result = DateTimeParse.TM.AM;
					return true;
				}
				if (str.GetChar() == dtfi.PMDesignator[0])
				{
					result = DateTimeParse.TM.PM;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x00062D6D File Offset: 0x00061D6D
		private static bool CheckNewValue(ref int currentValue, int newValue, char patternChar, ref DateTimeResult result)
		{
			if (currentValue == -1)
			{
				currentValue = newValue;
				return true;
			}
			if (newValue != currentValue)
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", patternChar);
				return false;
			}
			return true;
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x00062D94 File Offset: 0x00061D94
		private static DateTime GetDateTimeNow(ref DateTimeResult result, ref DateTimeStyles styles)
		{
			if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0)
			{
				if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0)
				{
					return new DateTime(DateTime.UtcNow.Ticks + result.timeZoneOffset.Ticks, DateTimeKind.Unspecified);
				}
				if ((styles & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
				{
					return DateTime.UtcNow;
				}
			}
			return DateTime.Now;
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x00062DF0 File Offset: 0x00061DF0
		private static bool CheckDefaultDateTime(ref DateTimeResult result, ref Calendar cal, DateTimeStyles styles)
		{
			if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0 && (result.Month != -1 || result.Day != -1) && (result.Year == -1 || (result.flags & ParseFlags.YearDefault) != (ParseFlags)0) && (result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_MissingIncompleteDate", null);
				return false;
			}
			if (result.Year == -1 || result.Month == -1 || result.Day == -1)
			{
				DateTime dateTimeNow = DateTimeParse.GetDateTimeNow(ref result, ref styles);
				if (result.Month == -1 && result.Day == -1)
				{
					if (result.Year == -1)
					{
						if ((styles & DateTimeStyles.NoCurrentDateDefault) != DateTimeStyles.None)
						{
							cal = GregorianCalendar.GetDefaultInstance();
							result.Year = (result.Month = (result.Day = 1));
						}
						else
						{
							result.Year = cal.GetYear(dateTimeNow);
							result.Month = cal.GetMonth(dateTimeNow);
							result.Day = cal.GetDayOfMonth(dateTimeNow);
						}
					}
					else
					{
						result.Month = 1;
						result.Day = 1;
					}
				}
				else
				{
					if (result.Year == -1)
					{
						result.Year = cal.GetYear(dateTimeNow);
					}
					if (result.Month == -1)
					{
						result.Month = 1;
					}
					if (result.Day == -1)
					{
						result.Day = 1;
					}
				}
			}
			if (result.Hour == -1)
			{
				result.Hour = 0;
			}
			if (result.Minute == -1)
			{
				result.Minute = 0;
			}
			if (result.Second == -1)
			{
				result.Second = 0;
			}
			if (result.era == -1)
			{
				result.era = 0;
			}
			return true;
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x00062F70 File Offset: 0x00061F70
		private static string ExpandPredefinedFormat(string format, ref DateTimeFormatInfo dtfi, ref ParsingInfo parseInfo, ref DateTimeResult result)
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
					goto IL_65;
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
					goto IL_65;
				case 's':
					dtfi = DateTimeFormatInfo.InvariantInfo;
					parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
					goto IL_15B;
				case 'u':
					parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
					dtfi = DateTimeFormatInfo.InvariantInfo;
					if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0)
					{
						result.flags |= ParseFlags.UtcSortPattern;
						goto IL_15B;
					}
					goto IL_15B;
				default:
					goto IL_15B;
				}
			}
			else
			{
				parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
				result.flags |= ParseFlags.TimeZoneUsed;
				result.timeZoneOffset = new TimeSpan(0L);
				result.flags |= ParseFlags.TimeZoneUtc;
				if (dtfi.Calendar.GetType() != typeof(GregorianCalendar))
				{
					dtfi = (DateTimeFormatInfo)dtfi.Clone();
					dtfi.Calendar = GregorianCalendar.GetDefaultInstance();
					goto IL_15B;
				}
				goto IL_15B;
			}
			parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
			dtfi = DateTimeFormatInfo.InvariantInfo;
			goto IL_15B;
			IL_65:
			parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
			dtfi = DateTimeFormatInfo.InvariantInfo;
			if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0)
			{
				result.flags |= ParseFlags.Rfc1123Pattern;
			}
			IL_15B:
			return DateTimeFormat.GetRealFormat(format, dtfi);
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000630E0 File Offset: 0x000620E0
		private static bool ParseJapaneseEraStart(ref __DTString str, DateTimeFormatInfo dtfi)
		{
			if (GregorianCalendarHelper.EnforceLegacyJapaneseDateParsing || dtfi.Calendar.ID != 3 || !str.GetNext())
			{
				return false;
			}
			if (str.m_current != "元"[0])
			{
				str.Index--;
				return false;
			}
			return true;
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x00063130 File Offset: 0x00062130
		private static bool ParseByFormat(ref __DTString str, ref __DTString format, ref ParsingInfo parseInfo, DateTimeFormatInfo dtfi, ref DateTimeResult result)
		{
			int num = 0;
			int newValue = 0;
			int newValue2 = 0;
			int newValue3 = 0;
			int newValue4 = 0;
			int newValue5 = 0;
			int newValue6 = 0;
			int newValue7 = 0;
			double num2 = 0.0;
			DateTimeParse.TM tm = DateTimeParse.TM.AM;
			char @char = format.GetChar();
			char c = @char;
			if (c <= 'H')
			{
				if (c <= '\'')
				{
					if (c != '"')
					{
						switch (c)
						{
						case '%':
							if (format.Index >= format.Value.Length - 1 || format.Value[format.Index + 1] == '%')
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
								return false;
							}
							return true;
						case '&':
							goto IL_9A1;
						case '\'':
							break;
						default:
							goto IL_9A1;
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					if (!DateTimeParse.TryParseQuoteString(format.Value, format.Index, stringBuilder, out num))
					{
						result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadQuote", @char);
						return false;
					}
					format.Index += num - 1;
					string text = stringBuilder.ToString();
					for (int i = 0; i < text.Length; i++)
					{
						if (text[i] == ' ' && parseInfo.fAllowInnerWhite)
						{
							str.SkipWhiteSpaces();
						}
						else if (!str.Match(text[i]))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
					}
					if ((result.flags & ParseFlags.CaptureOffset) == (ParseFlags)0)
					{
						return true;
					}
					if ((result.flags & ParseFlags.Rfc1123Pattern) != (ParseFlags)0 && text == "GMT")
					{
						result.flags |= ParseFlags.TimeZoneUsed;
						result.timeZoneOffset = TimeSpan.Zero;
						return true;
					}
					if ((result.flags & ParseFlags.UtcSortPattern) != (ParseFlags)0 && text == "Z")
					{
						result.flags |= ParseFlags.TimeZoneUsed;
						result.timeZoneOffset = TimeSpan.Zero;
						return true;
					}
					return true;
				}
				else
				{
					switch (c)
					{
					case '.':
						if (str.Match(@char))
						{
							return true;
						}
						if (format.GetNext() && format.Match('F'))
						{
							format.GetRepeatCount();
							return true;
						}
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					case '/':
						if (!str.Match(dtfi.DateSeparator))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						return true;
					default:
						if (c != ':')
						{
							switch (c)
							{
							case 'F':
								break;
							case 'G':
								goto IL_9A1;
							case 'H':
								num = format.GetRepeatCount();
								if (!DateTimeParse.ParseDigits(ref str, (num < 2) ? 1 : 2, out newValue5))
								{
									result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
									return false;
								}
								if (!DateTimeParse.CheckNewValue(ref result.Hour, newValue5, @char, ref result))
								{
									return false;
								}
								return true;
							default:
								goto IL_9A1;
							}
						}
						else
						{
							if (!str.Match(dtfi.TimeSeparator))
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
								return false;
							}
							return true;
						}
						break;
					}
				}
			}
			else if (c <= 'h')
			{
				switch (c)
				{
				case 'K':
					if (str.Match('Z'))
					{
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0 && result.timeZoneOffset != TimeSpan.Zero)
						{
							result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", 'K');
							return false;
						}
						result.flags |= ParseFlags.TimeZoneUsed;
						result.timeZoneOffset = new TimeSpan(0L);
						result.flags |= ParseFlags.TimeZoneUtc;
						return true;
					}
					else
					{
						if (!str.Match('+') && !str.Match('-'))
						{
							return true;
						}
						str.Index--;
						TimeSpan timeSpan = new TimeSpan(0L);
						if (!DateTimeParse.ParseTimeZoneOffset(ref str, 3, ref timeSpan))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0 && timeSpan != result.timeZoneOffset)
						{
							result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", 'K');
							return false;
						}
						result.timeZoneOffset = timeSpan;
						result.flags |= ParseFlags.TimeZoneUsed;
						return true;
					}
					break;
				case 'L':
					goto IL_9A1;
				case 'M':
					num = format.GetRepeatCount();
					if (num <= 2)
					{
						if (!DateTimeParse.ParseDigits(ref str, num, out newValue2) && (!parseInfo.fCustomNumberParser || !parseInfo.parseNumberDelegate(ref str, num, out newValue2)))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
					}
					else
					{
						if (num == 3)
						{
							if (!DateTimeParse.MatchAbbreviatedMonthName(ref str, dtfi, ref newValue2))
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
								return false;
							}
						}
						else if (!DateTimeParse.MatchMonthName(ref str, dtfi, ref newValue2))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						result.flags |= ParseFlags.ParsedMonthName;
					}
					if (!DateTimeParse.CheckNewValue(ref result.Month, newValue2, @char, ref result))
					{
						return false;
					}
					return true;
				default:
					switch (c)
					{
					case 'Z':
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0 && result.timeZoneOffset != TimeSpan.Zero)
						{
							result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", 'Z');
							return false;
						}
						result.flags |= ParseFlags.TimeZoneUsed;
						result.timeZoneOffset = new TimeSpan(0L);
						result.flags |= ParseFlags.TimeZoneUtc;
						str.Index++;
						if (!DateTimeParse.GetTimeZoneName(ref str))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						str.Index--;
						return true;
					case '[':
						goto IL_9A1;
					case '\\':
						if (!format.GetNext())
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
							return false;
						}
						if (!str.Match(format.GetChar()))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						return true;
					default:
						switch (c)
						{
						case 'd':
							num = format.GetRepeatCount();
							if (num <= 2)
							{
								if (!DateTimeParse.ParseDigits(ref str, num, out newValue3) && (!parseInfo.fCustomNumberParser || !parseInfo.parseNumberDelegate(ref str, num, out newValue3)))
								{
									result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
									return false;
								}
								if (!DateTimeParse.CheckNewValue(ref result.Day, newValue3, @char, ref result))
								{
									return false;
								}
								return true;
							}
							else
							{
								if (num == 3)
								{
									if (!DateTimeParse.MatchAbbreviatedDayName(ref str, dtfi, ref newValue4))
									{
										result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
										return false;
									}
								}
								else if (!DateTimeParse.MatchDayName(ref str, dtfi, ref newValue4))
								{
									result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
									return false;
								}
								if (!DateTimeParse.CheckNewValue(ref parseInfo.dayOfWeek, newValue4, @char, ref result))
								{
									return false;
								}
								return true;
							}
							break;
						case 'e':
							goto IL_9A1;
						case 'f':
							break;
						case 'g':
							num = format.GetRepeatCount();
							if (!DateTimeParse.MatchEraName(ref str, dtfi, ref result.era))
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
								return false;
							}
							return true;
						case 'h':
							parseInfo.fUseHour12 = true;
							num = format.GetRepeatCount();
							if (!DateTimeParse.ParseDigits(ref str, (num < 2) ? 1 : 2, out newValue5))
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
								return false;
							}
							if (!DateTimeParse.CheckNewValue(ref result.Hour, newValue5, @char, ref result))
							{
								return false;
							}
							return true;
						default:
							goto IL_9A1;
						}
						break;
					}
					break;
				}
			}
			else if (c != 'm')
			{
				switch (c)
				{
				case 's':
					num = format.GetRepeatCount();
					if (!DateTimeParse.ParseDigits(ref str, (num < 2) ? 1 : 2, out newValue7))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					if (!DateTimeParse.CheckNewValue(ref result.Second, newValue7, @char, ref result))
					{
						return false;
					}
					return true;
				case 't':
					num = format.GetRepeatCount();
					if (num == 1)
					{
						if (!DateTimeParse.MatchAbbreviatedTimeMark(ref str, dtfi, ref tm))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
					}
					else if (!DateTimeParse.MatchTimeMark(ref str, dtfi, ref tm))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					if (parseInfo.timeMark == DateTimeParse.TM.NotSet)
					{
						parseInfo.timeMark = tm;
						return true;
					}
					if (parseInfo.timeMark != tm)
					{
						result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", @char);
						return false;
					}
					return true;
				default:
					switch (c)
					{
					case 'y':
					{
						num = format.GetRepeatCount();
						bool flag;
						if (DateTimeParse.ParseJapaneseEraStart(ref str, dtfi))
						{
							newValue = 1;
							flag = true;
						}
						else if (dtfi.HasForceTwoDigitYears)
						{
							flag = DateTimeParse.ParseDigits(ref str, 1, 4, out newValue);
						}
						else
						{
							if (num <= 2)
							{
								parseInfo.fUseTwoDigitYear = true;
							}
							flag = DateTimeParse.ParseDigits(ref str, num, out newValue);
						}
						if (!flag && parseInfo.fCustomNumberParser)
						{
							flag = parseInfo.parseNumberDelegate(ref str, num, out newValue);
						}
						if (!flag)
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						if (!DateTimeParse.CheckNewValue(ref result.Year, newValue, @char, ref result))
						{
							return false;
						}
						return true;
					}
					case 'z':
					{
						num = format.GetRepeatCount();
						TimeSpan timeSpan2 = new TimeSpan(0L);
						if (!DateTimeParse.ParseTimeZoneOffset(ref str, num, ref timeSpan2))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0 && timeSpan2 != result.timeZoneOffset)
						{
							result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", 'z');
							return false;
						}
						result.timeZoneOffset = timeSpan2;
						result.flags |= ParseFlags.TimeZoneUsed;
						return true;
					}
					default:
						goto IL_9A1;
					}
					break;
				}
			}
			else
			{
				num = format.GetRepeatCount();
				if (!DateTimeParse.ParseDigits(ref str, (num < 2) ? 1 : 2, out newValue6))
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (!DateTimeParse.CheckNewValue(ref result.Minute, newValue6, @char, ref result))
				{
					return false;
				}
				return true;
			}
			num = format.GetRepeatCount();
			if (num > 7)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (!DateTimeParse.ParseFractionExact(ref str, num, ref num2) && @char == 'f')
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (result.fraction < 0.0)
			{
				result.fraction = num2;
				return true;
			}
			if (num2 != result.fraction)
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", @char);
				return false;
			}
			return true;
			IL_9A1:
			if (@char == ' ')
			{
				if (!parseInfo.fAllowInnerWhite && !str.Match(@char))
				{
					if (parseInfo.fAllowTrailingWhite && format.GetNext() && DateTimeParse.ParseByFormat(ref str, ref format, ref parseInfo, dtfi, ref result))
					{
						return true;
					}
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
			}
			else if (format.MatchSpecifiedWord("GMT"))
			{
				format.Index += "GMT".Length - 1;
				result.flags |= ParseFlags.TimeZoneUsed;
				result.timeZoneOffset = TimeSpan.Zero;
				if (!str.Match("GMT"))
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
			}
			else if (!str.Match(@char))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			return true;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x00063BA8 File Offset: 0x00062BA8
		internal static bool TryParseQuoteString(string format, int pos, StringBuilder result, out int returnValue)
		{
			returnValue = 0;
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
						return false;
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
				return false;
			}
			returnValue = pos - num;
			return true;
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x00063C24 File Offset: 0x00062C24
		private static bool DoStrictParse(string s, string formatParam, DateTimeStyles styles, DateTimeFormatInfo dtfi, ref DateTimeResult result)
		{
			ParsingInfo parsingInfo = default(ParsingInfo);
			parsingInfo.Init();
			parsingInfo.calendar = dtfi.Calendar;
			parsingInfo.fAllowInnerWhite = ((styles & DateTimeStyles.AllowInnerWhite) != DateTimeStyles.None);
			parsingInfo.fAllowTrailingWhite = ((styles & DateTimeStyles.AllowTrailingWhite) != DateTimeStyles.None);
			if (formatParam.Length == 1)
			{
				if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0 && formatParam[0] == 'U')
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
					return false;
				}
				formatParam = DateTimeParse.ExpandPredefinedFormat(formatParam, ref dtfi, ref parsingInfo, ref result);
			}
			result.calendar = parsingInfo.calendar;
			if (parsingInfo.calendar.ID == 8)
			{
				parsingInfo.parseNumberDelegate = DateTimeParse.m_hebrewNumberParser;
				parsingInfo.fCustomNumberParser = true;
			}
			result.Hour = (result.Minute = (result.Second = -1));
			__DTString _DTString = new __DTString(formatParam, dtfi, false);
			__DTString _DTString2 = new __DTString(s, dtfi, false);
			if (parsingInfo.fAllowTrailingWhite)
			{
				_DTString.TrimTail();
				_DTString.RemoveTrailingInQuoteSpaces();
				_DTString2.TrimTail();
			}
			if ((styles & DateTimeStyles.AllowLeadingWhite) != DateTimeStyles.None)
			{
				_DTString.SkipWhiteSpaces();
				_DTString.RemoveLeadingInQuoteSpaces();
				_DTString2.SkipWhiteSpaces();
			}
			while (_DTString.GetNext())
			{
				if (parsingInfo.fAllowInnerWhite)
				{
					_DTString2.SkipWhiteSpaces();
				}
				if (!DateTimeParse.ParseByFormat(ref _DTString2, ref _DTString, ref parsingInfo, dtfi, ref result))
				{
					return false;
				}
			}
			if (_DTString2.Index < _DTString2.Value.Length - 1)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (parsingInfo.fUseTwoDigitYear && (dtfi.FormatFlags & DateTimeFormatFlags.UseHebrewRule) == DateTimeFormatFlags.None)
			{
				if (result.Year >= 100)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.Year = parsingInfo.calendar.ToFourDigitYear(result.Year);
			}
			if (parsingInfo.fUseHour12)
			{
				if (parsingInfo.timeMark == DateTimeParse.TM.NotSet)
				{
					parsingInfo.timeMark = DateTimeParse.TM.AM;
				}
				if (result.Hour > 12)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (parsingInfo.timeMark == DateTimeParse.TM.AM)
				{
					if (result.Hour == 12)
					{
						result.Hour = 0;
					}
				}
				else
				{
					result.Hour = ((result.Hour == 12) ? 12 : (result.Hour + 12));
				}
			}
			bool flag = result.Year == -1 && result.Month == -1 && result.Day == -1;
			if (!DateTimeParse.CheckDefaultDateTime(ref result, ref parsingInfo.calendar, styles))
			{
				return false;
			}
			if (!flag && dtfi.HasYearMonthAdjustment && !dtfi.YearMonthAdjustment(ref result.Year, ref result.Month, (result.flags & ParseFlags.ParsedMonthName) != (ParseFlags)0))
			{
				result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
				return false;
			}
			if (!parsingInfo.calendar.TryToDateTime(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second, 0, result.era, out result.parsedDate))
			{
				result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
				return false;
			}
			if (result.fraction > 0.0)
			{
				result.parsedDate = result.parsedDate.AddTicks((long)Math.Round(result.fraction * 10000000.0));
			}
			if (parsingInfo.dayOfWeek != -1 && parsingInfo.dayOfWeek != (int)parsingInfo.calendar.GetDayOfWeek(result.parsedDate))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDayOfWeek", null);
				return false;
			}
			return DateTimeParse.DetermineTimeZoneAdjustments(ref result, styles, flag);
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x00063FB8 File Offset: 0x00062FB8
		private static Exception GetDateTimeParseException(ref DateTimeResult result)
		{
			switch (result.failure)
			{
			case ParseFailureKind.ArgumentNull:
				return new ArgumentNullException(result.failureArgumentName, Environment.GetResourceString(result.failureMessageID));
			case ParseFailureKind.Format:
				return new FormatException(Environment.GetResourceString(result.failureMessageID));
			case ParseFailureKind.FormatWithParameter:
				return new FormatException(Environment.GetResourceString(result.failureMessageID, new object[]
				{
					result.failureMessageFormatArgument
				}));
			case ParseFailureKind.FormatBadDateTimeCalendar:
				return new FormatException(Environment.GetResourceString(result.failureMessageID, new object[]
				{
					result.calendar
				}));
			default:
				return null;
			}
		}

		// Token: 0x04000FA7 RID: 4007
		internal const int MaxDateTimeNumberDigits = 8;

		// Token: 0x04000FA8 RID: 4008
		internal const string GMTName = "GMT";

		// Token: 0x04000FA9 RID: 4009
		internal const string ZuluName = "Z";

		// Token: 0x04000FAA RID: 4010
		private const int ORDER_YMD = 0;

		// Token: 0x04000FAB RID: 4011
		private const int ORDER_MDY = 1;

		// Token: 0x04000FAC RID: 4012
		private const int ORDER_DMY = 2;

		// Token: 0x04000FAD RID: 4013
		private const int ORDER_YDM = 3;

		// Token: 0x04000FAE RID: 4014
		private const int ORDER_YM = 4;

		// Token: 0x04000FAF RID: 4015
		private const int ORDER_MY = 5;

		// Token: 0x04000FB0 RID: 4016
		private const int ORDER_MD = 6;

		// Token: 0x04000FB1 RID: 4017
		private const int ORDER_DM = 7;

		// Token: 0x04000FB2 RID: 4018
		internal static DateTimeParse.MatchNumberDelegate m_hebrewNumberParser = new DateTimeParse.MatchNumberDelegate(DateTimeParse.MatchHebrewDigits);

		// Token: 0x04000FB3 RID: 4019
		private static DateTimeParse.DS[][] dateParsingStates = new DateTimeParse.DS[][]
		{
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.BEGIN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.TX_N,
				DateTimeParse.DS.N,
				DateTimeParse.DS.D_Nd,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_M,
				DateTimeParse.DS.D_M,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.BEGIN,
				DateTimeParse.DS.D_Y,
				DateTimeParse.DS.D_Y,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.BEGIN,
				DateTimeParse.DS.BEGIN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.NN,
				DateTimeParse.DS.D_NNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NM,
				DateTimeParse.DS.D_NM,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.D_NDS,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.N,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.D_YNd,
				DateTimeParse.DS.DX_YN,
				DateTimeParse.DS.N,
				DateTimeParse.DS.N,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_NN,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.TX_N,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.NN,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.NN,
				DateTimeParse.DS.NN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NN,
				DateTimeParse.DS.D_NNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NM,
				DateTimeParse.DS.D_MN,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_Nd,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.D_YNd,
				DateTimeParse.DS.DX_YN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_Nd,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_NN,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.TX_N,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_NN,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NNd,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NNd,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_MN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_MN,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_M,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.D_YMd,
				DateTimeParse.DS.DX_YM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_M,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_MN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_MN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_MN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_NM,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_NM,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NM,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_NDS,
				DateTimeParse.DS.DX_NNDS,
				DateTimeParse.DS.DX_NNDS,
				DateTimeParse.DS.DX_NNDS,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NDS,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_NDS,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NDS,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.D_YNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YM,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.D_YMd,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_Y,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_Y,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_YN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_YM,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.DX_DSN,
				DateTimeParse.DS.TX_N,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.TX_TS,
				DateTimeParse.DS.TX_TS,
				DateTimeParse.DS.TX_TS,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.D_Nd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.TX_NN,
				DateTimeParse.DS.TX_NN,
				DateTimeParse.DS.TX_NN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_NNt,
				DateTimeParse.DS.DX_NM,
				DateTimeParse.DS.D_NM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.TX_NN
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.TX_NNN,
				DateTimeParse.DS.TX_NNN,
				DateTimeParse.DS.TX_NNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.T_NNt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_NNt,
				DateTimeParse.DS.T_NNt,
				DateTimeParse.DS.TX_NNN
			}
		};

		// Token: 0x02000398 RID: 920
		// (Invoke) Token: 0x060024F8 RID: 9464
		internal delegate bool MatchNumberDelegate(ref __DTString str, int digitLen, out int result);

		// Token: 0x02000399 RID: 921
		internal enum DTT
		{
			// Token: 0x04000FB5 RID: 4021
			End,
			// Token: 0x04000FB6 RID: 4022
			NumEnd,
			// Token: 0x04000FB7 RID: 4023
			NumAmpm,
			// Token: 0x04000FB8 RID: 4024
			NumSpace,
			// Token: 0x04000FB9 RID: 4025
			NumDatesep,
			// Token: 0x04000FBA RID: 4026
			NumTimesep,
			// Token: 0x04000FBB RID: 4027
			MonthEnd,
			// Token: 0x04000FBC RID: 4028
			MonthSpace,
			// Token: 0x04000FBD RID: 4029
			MonthDatesep,
			// Token: 0x04000FBE RID: 4030
			NumDatesuff,
			// Token: 0x04000FBF RID: 4031
			NumTimesuff,
			// Token: 0x04000FC0 RID: 4032
			DayOfWeek,
			// Token: 0x04000FC1 RID: 4033
			YearSpace,
			// Token: 0x04000FC2 RID: 4034
			YearDateSep,
			// Token: 0x04000FC3 RID: 4035
			YearEnd,
			// Token: 0x04000FC4 RID: 4036
			TimeZone,
			// Token: 0x04000FC5 RID: 4037
			Era,
			// Token: 0x04000FC6 RID: 4038
			NumUTCTimeMark,
			// Token: 0x04000FC7 RID: 4039
			Unk,
			// Token: 0x04000FC8 RID: 4040
			NumLocalTimeMark,
			// Token: 0x04000FC9 RID: 4041
			Max
		}

		// Token: 0x0200039A RID: 922
		internal enum TM
		{
			// Token: 0x04000FCB RID: 4043
			NotSet = -1,
			// Token: 0x04000FCC RID: 4044
			AM,
			// Token: 0x04000FCD RID: 4045
			PM
		}

		// Token: 0x0200039B RID: 923
		internal enum DS
		{
			// Token: 0x04000FCF RID: 4047
			BEGIN,
			// Token: 0x04000FD0 RID: 4048
			N,
			// Token: 0x04000FD1 RID: 4049
			NN,
			// Token: 0x04000FD2 RID: 4050
			D_Nd,
			// Token: 0x04000FD3 RID: 4051
			D_NN,
			// Token: 0x04000FD4 RID: 4052
			D_NNd,
			// Token: 0x04000FD5 RID: 4053
			D_M,
			// Token: 0x04000FD6 RID: 4054
			D_MN,
			// Token: 0x04000FD7 RID: 4055
			D_NM,
			// Token: 0x04000FD8 RID: 4056
			D_MNd,
			// Token: 0x04000FD9 RID: 4057
			D_NDS,
			// Token: 0x04000FDA RID: 4058
			D_Y,
			// Token: 0x04000FDB RID: 4059
			D_YN,
			// Token: 0x04000FDC RID: 4060
			D_YNd,
			// Token: 0x04000FDD RID: 4061
			D_YM,
			// Token: 0x04000FDE RID: 4062
			D_YMd,
			// Token: 0x04000FDF RID: 4063
			D_S,
			// Token: 0x04000FE0 RID: 4064
			T_S,
			// Token: 0x04000FE1 RID: 4065
			T_Nt,
			// Token: 0x04000FE2 RID: 4066
			T_NNt,
			// Token: 0x04000FE3 RID: 4067
			ERROR,
			// Token: 0x04000FE4 RID: 4068
			DX_NN,
			// Token: 0x04000FE5 RID: 4069
			DX_NNN,
			// Token: 0x04000FE6 RID: 4070
			DX_MN,
			// Token: 0x04000FE7 RID: 4071
			DX_NM,
			// Token: 0x04000FE8 RID: 4072
			DX_MNN,
			// Token: 0x04000FE9 RID: 4073
			DX_DS,
			// Token: 0x04000FEA RID: 4074
			DX_DSN,
			// Token: 0x04000FEB RID: 4075
			DX_NDS,
			// Token: 0x04000FEC RID: 4076
			DX_NNDS,
			// Token: 0x04000FED RID: 4077
			DX_YNN,
			// Token: 0x04000FEE RID: 4078
			DX_YMN,
			// Token: 0x04000FEF RID: 4079
			DX_YN,
			// Token: 0x04000FF0 RID: 4080
			DX_YM,
			// Token: 0x04000FF1 RID: 4081
			TX_N,
			// Token: 0x04000FF2 RID: 4082
			TX_NN,
			// Token: 0x04000FF3 RID: 4083
			TX_NNN,
			// Token: 0x04000FF4 RID: 4084
			TX_TS,
			// Token: 0x04000FF5 RID: 4085
			DX_NNY
		}
	}
}
