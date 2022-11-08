using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x0200002E RID: 46
	public class XmlConvert
	{
		// Token: 0x060000F3 RID: 243 RVA: 0x00005471 File Offset: 0x00004471
		public static string EncodeName(string name)
		{
			return XmlConvert.EncodeName(name, true, false);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000547B File Offset: 0x0000447B
		public static string EncodeNmToken(string name)
		{
			return XmlConvert.EncodeName(name, false, false);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005485 File Offset: 0x00004485
		public static string EncodeLocalName(string name)
		{
			return XmlConvert.EncodeName(name, true, true);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005490 File Offset: 0x00004490
		public static string DecodeName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return name;
			}
			StringBuilder stringBuilder = null;
			int length = name.Length;
			int num = 0;
			int num2 = name.IndexOf('_');
			if (num2 < 0)
			{
				return name;
			}
			if (XmlConvert.c_DecodeCharPattern == null)
			{
				XmlConvert.c_DecodeCharPattern = new Regex("_[Xx]([0-9a-fA-F]{4}|[0-9a-fA-F]{8})_");
			}
			MatchCollection matchCollection = XmlConvert.c_DecodeCharPattern.Matches(name, num2);
			IEnumerator enumerator = matchCollection.GetEnumerator();
			int num3 = -1;
			if (enumerator != null && enumerator.MoveNext())
			{
				Match match = (Match)enumerator.Current;
				num3 = match.Index;
			}
			for (int i = 0; i < length - XmlConvert.c_EncodedCharLength + 1; i++)
			{
				if (i == num3)
				{
					if (enumerator.MoveNext())
					{
						Match match2 = (Match)enumerator.Current;
						num3 = match2.Index;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(length + 20);
					}
					stringBuilder.Append(name, num, i - num);
					if (name[i + 6] != '_')
					{
						int num4 = XmlConvert.FromHex(name[i + 2]) * 268435456 + XmlConvert.FromHex(name[i + 3]) * 16777216 + XmlConvert.FromHex(name[i + 4]) * 1048576 + XmlConvert.FromHex(name[i + 5]) * 65536 + XmlConvert.FromHex(name[i + 6]) * 4096 + XmlConvert.FromHex(name[i + 7]) * 256 + XmlConvert.FromHex(name[i + 8]) * 16 + XmlConvert.FromHex(name[i + 9]);
						if (num4 >= 65536)
						{
							if (num4 <= 1114111)
							{
								num = i + XmlConvert.c_EncodedCharLength + 4;
								char c = (char)((num4 - 65536) / 1024 + 55296);
								char value = (char)(num4 - (int)((c - '\ud800') * 'Ѐ') - 65536 + 56320);
								stringBuilder.Append(c);
								stringBuilder.Append(value);
							}
						}
						else
						{
							num = i + XmlConvert.c_EncodedCharLength + 4;
							stringBuilder.Append((char)num4);
						}
						i += XmlConvert.c_EncodedCharLength - 1 + 4;
					}
					else
					{
						num = i + XmlConvert.c_EncodedCharLength;
						stringBuilder.Append((char)(XmlConvert.FromHex(name[i + 2]) * 4096 + XmlConvert.FromHex(name[i + 3]) * 256 + XmlConvert.FromHex(name[i + 4]) * 16 + XmlConvert.FromHex(name[i + 5])));
						i += XmlConvert.c_EncodedCharLength - 1;
					}
				}
			}
			if (num == 0)
			{
				return name;
			}
			if (num < length)
			{
				stringBuilder.Append(name, num, length - num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005750 File Offset: 0x00004750
		private static string EncodeName(string name, bool first, bool local)
		{
			if (name == null)
			{
				return name;
			}
			if (name.Length == 0)
			{
				if (!first)
				{
					throw new XmlException("Xml_InvalidNmToken", name);
				}
				return name;
			}
			else
			{
				StringBuilder stringBuilder = null;
				int length = name.Length;
				int num = 0;
				int i = 0;
				XmlCharType instance = XmlCharType.Instance;
				int num2 = name.IndexOf('_');
				IEnumerator enumerator = null;
				if (num2 >= 0)
				{
					if (XmlConvert.c_EncodeCharPattern == null)
					{
						XmlConvert.c_EncodeCharPattern = new Regex("(?<=_)[Xx]([0-9a-fA-F]{4}|[0-9a-fA-F]{8})_");
					}
					MatchCollection matchCollection = XmlConvert.c_EncodeCharPattern.Matches(name, num2);
					enumerator = matchCollection.GetEnumerator();
				}
				int num3 = -1;
				if (enumerator != null && enumerator.MoveNext())
				{
					Match match = (Match)enumerator.Current;
					num3 = match.Index - 1;
				}
				if (first && ((!instance.IsStartNCNameChar(name[0]) && (local || (!local && name[0] != ':'))) || num3 == 0))
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(length + 20);
					}
					stringBuilder.Append("_x");
					if (length > 1 && name[0] >= '\ud800' && name[0] <= '\udbff' && name[1] >= '\udc00' && name[1] <= '\udfff')
					{
						int num4 = (int)name[0];
						int num5 = (int)name[1];
						stringBuilder.Append(((num4 - 55296) * 1024 + (num5 - 56320) + 65536).ToString("X8", CultureInfo.InvariantCulture));
						i++;
						num = 2;
					}
					else
					{
						stringBuilder.Append(((int)name[0]).ToString("X4", CultureInfo.InvariantCulture));
						num = 1;
					}
					stringBuilder.Append("_");
					i++;
					if (num3 == 0 && enumerator.MoveNext())
					{
						Match match2 = (Match)enumerator.Current;
						num3 = match2.Index - 1;
					}
				}
				while (i < length)
				{
					if ((local && !instance.IsNCNameChar(name[i])) || (!local && !instance.IsNameChar(name[i])) || num3 == i)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(length + 20);
						}
						if (num3 == i && enumerator.MoveNext())
						{
							Match match3 = (Match)enumerator.Current;
							num3 = match3.Index - 1;
						}
						stringBuilder.Append(name, num, i - num);
						stringBuilder.Append("_x");
						if (length > i + 1 && name[i] >= '\ud800' && name[i] <= '\udbff' && name[i + 1] >= '\udc00' && name[i + 1] <= '\udfff')
						{
							int num6 = (int)name[i];
							int num7 = (int)name[i + 1];
							stringBuilder.Append(((num6 - 55296) * 1024 + (num7 - 56320) + 65536).ToString("X8", CultureInfo.InvariantCulture));
							num = i + 2;
							i++;
						}
						else
						{
							stringBuilder.Append(((int)name[i]).ToString("X4", CultureInfo.InvariantCulture));
							num = i + 1;
						}
						stringBuilder.Append("_");
					}
					i++;
				}
				if (num == 0)
				{
					return name;
				}
				if (num < length)
				{
					stringBuilder.Append(name, num, length - num);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005AA4 File Offset: 0x00004AA4
		private static int FromHex(char digit)
		{
			if (digit > '9')
			{
				return (int)(((digit <= 'F') ? (digit - 'A') : (digit - 'a')) + '\n');
			}
			return (int)(digit - '0');
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005AC2 File Offset: 0x00004AC2
		internal static byte[] FromBinHexString(string s)
		{
			return XmlConvert.FromBinHexString(s, true);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005ACB File Offset: 0x00004ACB
		internal static byte[] FromBinHexString(string s, bool allowOddCount)
		{
			return BinHexDecoder.Decode(s.ToCharArray(), allowOddCount);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005AD9 File Offset: 0x00004AD9
		internal static string ToBinHexString(byte[] inArray)
		{
			return BinHexEncoder.Encode(inArray, 0, inArray.Length);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005AE8 File Offset: 0x00004AE8
		public unsafe static string VerifyName(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			XmlCharType instance = XmlCharType.Instance;
			char c = name[0];
			if ((instance.charProperties[c] & 4) == 0 && c != ':')
			{
				throw new XmlException("Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(c));
			}
			for (int i = 1; i < name.Length; i++)
			{
				if ((instance.charProperties[name[i]] & 8) == 0 && name[i] != ':')
				{
					throw new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[i]));
				}
			}
			return name;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005B88 File Offset: 0x00004B88
		internal static Exception TryVerifyName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return new XmlException("Xml_EmptyName", string.Empty);
			}
			XmlCharType instance = XmlCharType.Instance;
			char c = name[0];
			if (!instance.IsStartNameChar(c) && c != ':')
			{
				return new XmlException("Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(c));
			}
			for (int i = 1; i < name.Length; i++)
			{
				c = name[i];
				if (!instance.IsNameChar(c) && c != ':')
				{
					return new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(c));
				}
			}
			return null;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005C18 File Offset: 0x00004C18
		internal static string VerifyQName(string name)
		{
			return XmlConvert.VerifyQName(name, ExceptionType.XmlException);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005C24 File Offset: 0x00004C24
		internal unsafe static string VerifyQName(string name, ExceptionType exceptionType)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException("name");
			}
			XmlCharType instance = XmlCharType.Instance;
			int length = name.Length;
			int num = 0;
			int num2 = -1;
			while ((instance.charProperties[name[num]] & 4) != 0)
			{
				num++;
				while (num < length && (instance.charProperties[name[num]] & 8) != 0)
				{
					num++;
				}
				if (num == length)
				{
					return name;
				}
				if (name[num] != ':' || num2 != -1 || num + 1 >= length)
				{
					break;
				}
				num2 = num;
				num++;
			}
			throw XmlConvert.CreateException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[num]), exceptionType);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005CC8 File Offset: 0x00004CC8
		public static string VerifyNCName(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			return ValidateNames.ParseNCNameThrow(name);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005CE8 File Offset: 0x00004CE8
		internal static Exception TryVerifyNCName(string name)
		{
			int num = ValidateNames.ParseNCName(name, 0);
			if (num == 0 || num != name.Length)
			{
				return ValidateNames.GetInvalidNameException(name, 0, num);
			}
			return null;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005D14 File Offset: 0x00004D14
		public static string VerifyTOKEN(string token)
		{
			if (token == null || token.Length == 0)
			{
				return token;
			}
			if (token[0] == ' ' || token[token.Length - 1] == ' ' || token.IndexOfAny(XmlConvert.crt) != -1 || token.IndexOf("  ", StringComparison.Ordinal) != -1)
			{
				throw new XmlException("Sch_NotTokenString", token);
			}
			return token;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005D78 File Offset: 0x00004D78
		internal static Exception TryVerifyTOKEN(string token)
		{
			if (token == null || token.Length == 0)
			{
				return null;
			}
			if (token[0] == ' ' || token[token.Length - 1] == ' ' || token.IndexOfAny(XmlConvert.crt) != -1 || token.IndexOf("  ", StringComparison.Ordinal) != -1)
			{
				return new XmlException("Sch_NotTokenString", token);
			}
			return null;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005DD9 File Offset: 0x00004DD9
		public static string VerifyNMTOKEN(string name)
		{
			return XmlConvert.VerifyNMTOKEN(name, ExceptionType.XmlException);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005DE4 File Offset: 0x00004DE4
		internal static string VerifyNMTOKEN(string name, ExceptionType exceptionType)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw XmlConvert.CreateException("Xml_InvalidNmToken", name, exceptionType);
			}
			XmlCharType instance = XmlCharType.Instance;
			for (int i = 0; i < name.Length; i++)
			{
				if (!instance.IsNameChar(name[i]))
				{
					throw XmlConvert.CreateException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[i]), exceptionType);
				}
			}
			return name;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005E54 File Offset: 0x00004E54
		internal static Exception TryVerifyNMTOKEN(string name)
		{
			if (name == null || name.Length == 0)
			{
				return new XmlException("Xml_EmptyName", string.Empty);
			}
			XmlCharType instance = XmlCharType.Instance;
			for (int i = 0; i < name.Length; i++)
			{
				if (!instance.IsNameChar(name[i]))
				{
					return new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[i]));
				}
			}
			return null;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005EBB File Offset: 0x00004EBB
		internal static string VerifyNormalizedString(string str)
		{
			if (str.IndexOfAny(XmlConvert.crt) != -1)
			{
				throw new XmlSchemaException("Sch_NotNormalizedString", str);
			}
			return str;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005ED8 File Offset: 0x00004ED8
		internal static Exception TryVerifyNormalizedString(string str)
		{
			if (str.IndexOfAny(XmlConvert.crt) != -1)
			{
				return new XmlSchemaException("Sch_NotNormalizedString", str);
			}
			return null;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005EF5 File Offset: 0x00004EF5
		public static string ToString(bool value)
		{
			if (!value)
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005F05 File Offset: 0x00004F05
		public static string ToString(char value)
		{
			return value.ToString(null);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005F0F File Offset: 0x00004F0F
		public static string ToString(decimal value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005F1E File Offset: 0x00004F1E
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005F2D File Offset: 0x00004F2D
		public static string ToString(short value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005F3C File Offset: 0x00004F3C
		public static string ToString(int value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005F4B File Offset: 0x00004F4B
		public static string ToString(long value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005F5A File Offset: 0x00004F5A
		public static string ToString(byte value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005F69 File Offset: 0x00004F69
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005F78 File Offset: 0x00004F78
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005F87 File Offset: 0x00004F87
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(null, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005F96 File Offset: 0x00004F96
		public static string ToString(float value)
		{
			if (float.IsNegativeInfinity(value))
			{
				return "-INF";
			}
			if (float.IsPositiveInfinity(value))
			{
				return "INF";
			}
			if (XmlConvert.IsNegativeZero((double)value))
			{
				return "-0";
			}
			return value.ToString("R", NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005FD4 File Offset: 0x00004FD4
		public static string ToString(double value)
		{
			if (double.IsNegativeInfinity(value))
			{
				return "-INF";
			}
			if (double.IsPositiveInfinity(value))
			{
				return "INF";
			}
			if (XmlConvert.IsNegativeZero(value))
			{
				return "-0";
			}
			return value.ToString("R", NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006014 File Offset: 0x00005014
		public static string ToString(TimeSpan value)
		{
			return new XsdDuration(value).ToString();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006035 File Offset: 0x00005035
		[Obsolete("Use XmlConvert.ToString() that takes in XmlDateTimeSerializationMode")]
		public static string ToString(DateTime value)
		{
			return XmlConvert.ToString(value, "yyyy-MM-ddTHH:mm:ss.fffffffzzzzzz");
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00006042 File Offset: 0x00005042
		public static string ToString(DateTime value, string format)
		{
			return value.ToString(format, DateTimeFormatInfo.InvariantInfo);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006054 File Offset: 0x00005054
		public static string ToString(DateTime value, XmlDateTimeSerializationMode dateTimeOption)
		{
			switch (dateTimeOption)
			{
			case XmlDateTimeSerializationMode.Local:
				value = XmlConvert.SwitchToLocalTime(value);
				break;
			case XmlDateTimeSerializationMode.Utc:
				value = XmlConvert.SwitchToUtcTime(value);
				break;
			case XmlDateTimeSerializationMode.Unspecified:
				value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
				break;
			case XmlDateTimeSerializationMode.RoundtripKind:
				break;
			default:
				throw new ArgumentException(Res.GetString("Sch_InvalidDateTimeOption", new object[]
				{
					dateTimeOption
				}));
			}
			XsdDateTime xsdDateTime = new XsdDateTime(value, XsdDateTimeFlags.DateTime);
			return xsdDateTime.ToString();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000060D8 File Offset: 0x000050D8
		public static string ToString(DateTimeOffset value)
		{
			XsdDateTime xsdDateTime = new XsdDateTime(value);
			return xsdDateTime.ToString();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000060FA File Offset: 0x000050FA
		public static string ToString(DateTimeOffset value, string format)
		{
			return value.ToString(format, DateTimeFormatInfo.InvariantInfo);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006109 File Offset: 0x00005109
		public static string ToString(Guid value)
		{
			return value.ToString();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006118 File Offset: 0x00005118
		public static bool ToBoolean(string s)
		{
			s = XmlConvert.TrimString(s);
			if (s == "1" || s == "true")
			{
				return true;
			}
			if (s == "0" || s == "false")
			{
				return false;
			}
			throw new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
			{
				s,
				"Boolean"
			}));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006188 File Offset: 0x00005188
		internal static Exception TryToBoolean(string s, out bool result)
		{
			s = XmlConvert.TrimString(s);
			if (s == "0" || s == "false")
			{
				result = false;
				return null;
			}
			if (s == "1" || s == "true")
			{
				result = true;
				return null;
			}
			result = false;
			return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
			{
				s,
				"Boolean"
			}));
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006201 File Offset: 0x00005201
		public static char ToChar(string s)
		{
			return char.Parse(s);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000620C File Offset: 0x0000520C
		internal static Exception TryToChar(string s, out char result)
		{
			if (!char.TryParse(s, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Char"
				}));
			}
			return null;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006247 File Offset: 0x00005247
		public static decimal ToDecimal(string s)
		{
			return decimal.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006258 File Offset: 0x00005258
		internal static Exception TryToDecimal(string s, out decimal result)
		{
			if (!decimal.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Decimal"
				}));
			}
			return null;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000629A File Offset: 0x0000529A
		internal static decimal ToInteger(string s)
		{
			return decimal.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000062A8 File Offset: 0x000052A8
		internal static Exception TryToInteger(string s, out decimal result)
		{
			if (!decimal.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Integer"
				}));
			}
			return null;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000062E9 File Offset: 0x000052E9
		[CLSCompliant(false)]
		public static sbyte ToSByte(string s)
		{
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000062F8 File Offset: 0x000052F8
		internal static Exception TryToSByte(string s, out sbyte result)
		{
			if (!sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"SByte"
				}));
			}
			return null;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006339 File Offset: 0x00005339
		public static short ToInt16(string s)
		{
			return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006348 File Offset: 0x00005348
		internal static Exception TryToInt16(string s, out short result)
		{
			if (!short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Int16"
				}));
			}
			return null;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006389 File Offset: 0x00005389
		public static int ToInt32(string s)
		{
			return int.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006398 File Offset: 0x00005398
		internal static Exception TryToInt32(string s, out int result)
		{
			if (!int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Int32"
				}));
			}
			return null;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000063D9 File Offset: 0x000053D9
		public static long ToInt64(string s)
		{
			return long.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000063E8 File Offset: 0x000053E8
		internal static Exception TryToInt64(string s, out long result)
		{
			if (!long.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Int64"
				}));
			}
			return null;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006429 File Offset: 0x00005429
		public static byte ToByte(string s)
		{
			return byte.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006438 File Offset: 0x00005438
		internal static Exception TryToByte(string s, out byte result)
		{
			if (!byte.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Byte"
				}));
			}
			return null;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006479 File Offset: 0x00005479
		[CLSCompliant(false)]
		public static ushort ToUInt16(string s)
		{
			return ushort.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006488 File Offset: 0x00005488
		internal static Exception TryToUInt16(string s, out ushort result)
		{
			if (!ushort.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"UInt16"
				}));
			}
			return null;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000064C9 File Offset: 0x000054C9
		[CLSCompliant(false)]
		public static uint ToUInt32(string s)
		{
			return uint.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000064D8 File Offset: 0x000054D8
		internal static Exception TryToUInt32(string s, out uint result)
		{
			if (!uint.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"UInt32"
				}));
			}
			return null;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006519 File Offset: 0x00005519
		[CLSCompliant(false)]
		public static ulong ToUInt64(string s)
		{
			return ulong.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006528 File Offset: 0x00005528
		internal static Exception TryToUInt64(string s, out ulong result)
		{
			if (!ulong.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"UInt64"
				}));
			}
			return null;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000656C File Offset: 0x0000556C
		public static float ToSingle(string s)
		{
			s = XmlConvert.TrimString(s);
			if (s == "-INF")
			{
				return float.NegativeInfinity;
			}
			if (s == "INF")
			{
				return float.PositiveInfinity;
			}
			float num = float.Parse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo);
			if (num == 0f && s[0] == '-')
			{
				return --0f;
			}
			return num;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000065D4 File Offset: 0x000055D4
		internal static Exception TryToSingle(string s, out float result)
		{
			s = XmlConvert.TrimString(s);
			if (s == "-INF")
			{
				result = float.NegativeInfinity;
				return null;
			}
			if (s == "INF")
			{
				result = float.PositiveInfinity;
				return null;
			}
			if (!float.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Single"
				}));
			}
			if (result == 0f && s[0] == '-')
			{
				result = --0f;
			}
			return null;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006668 File Offset: 0x00005668
		public static double ToDouble(string s)
		{
			s = XmlConvert.TrimString(s);
			if (s == "-INF")
			{
				return double.NegativeInfinity;
			}
			if (s == "INF")
			{
				return double.PositiveInfinity;
			}
			double num = double.Parse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
			if (num == 0.0 && s[0] == '-')
			{
				return --0.0;
			}
			return num;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000066E0 File Offset: 0x000056E0
		internal static Exception TryToDouble(string s, out double result)
		{
			s = XmlConvert.TrimString(s);
			if (s == "-INF")
			{
				result = double.NegativeInfinity;
				return null;
			}
			if (s == "INF")
			{
				result = double.PositiveInfinity;
				return null;
			}
			if (!double.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Double"
				}));
			}
			if (result == 0.0 && s[0] == '-')
			{
				result = --0.0;
			}
			return null;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006784 File Offset: 0x00005784
		internal static double ToXPathDouble(object o)
		{
			string text = o as string;
			if (text != null)
			{
				text = XmlConvert.TrimString(text);
				double result;
				if (text.Length != 0 && text[0] != '+' && double.TryParse(text, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out result))
				{
					return result;
				}
				return double.NaN;
			}
			else
			{
				if (o is double)
				{
					return (double)o;
				}
				if (!(o is bool))
				{
					try
					{
						return Convert.ToDouble(o, NumberFormatInfo.InvariantInfo);
					}
					catch (FormatException)
					{
					}
					catch (OverflowException)
					{
					}
					catch (ArgumentNullException)
					{
					}
					return double.NaN;
				}
				if (!(bool)o)
				{
					return 0.0;
				}
				return 1.0;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006850 File Offset: 0x00005850
		internal static string ToXPathString(object value)
		{
			string text = value as string;
			if (text != null)
			{
				return text;
			}
			if (value is double)
			{
				return ((double)value).ToString("R", NumberFormatInfo.InvariantInfo);
			}
			if (!(value is bool))
			{
				return Convert.ToString(value, NumberFormatInfo.InvariantInfo);
			}
			if (!(bool)value)
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000068B4 File Offset: 0x000058B4
		internal static double XPathRound(double value)
		{
			double num = Math.Round(value);
			if (value - num != 0.5)
			{
				return num;
			}
			return num + 1.0;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000068E4 File Offset: 0x000058E4
		public static TimeSpan ToTimeSpan(string s)
		{
			XsdDuration xsdDuration;
			try
			{
				xsdDuration = new XsdDuration(s);
			}
			catch (Exception)
			{
				throw new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"TimeSpan"
				}));
			}
			return xsdDuration.ToTimeSpan();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006938 File Offset: 0x00005938
		internal static Exception TryToTimeSpan(string s, out TimeSpan result)
		{
			XsdDuration xsdDuration;
			Exception ex = XsdDuration.TryParse(s, out xsdDuration);
			if (ex != null)
			{
				result = TimeSpan.MinValue;
				return ex;
			}
			return xsdDuration.TryToTimeSpan(out result);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00006966 File Offset: 0x00005966
		private static string[] AllDateTimeFormats
		{
			get
			{
				if (XmlConvert.s_allDateTimeFormats == null)
				{
					XmlConvert.CreateAllDateTimeFormats();
				}
				return XmlConvert.s_allDateTimeFormats;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000697C File Offset: 0x0000597C
		private static void CreateAllDateTimeFormats()
		{
			if (XmlConvert.s_allDateTimeFormats == null)
			{
				XmlConvert.s_allDateTimeFormats = new string[]
				{
					"yyyy-MM-ddTHH:mm:ss.FFFFFFFzzzzzz",
					"yyyy-MM-ddTHH:mm:ss.FFFFFFF",
					"yyyy-MM-ddTHH:mm:ss.FFFFFFFZ",
					"HH:mm:ss.FFFFFFF",
					"HH:mm:ss.FFFFFFFZ",
					"HH:mm:ss.FFFFFFFzzzzzz",
					"yyyy-MM-dd",
					"yyyy-MM-ddZ",
					"yyyy-MM-ddzzzzzz",
					"yyyy-MM",
					"yyyy-MMZ",
					"yyyy-MMzzzzzz",
					"yyyy",
					"yyyyZ",
					"yyyyzzzzzz",
					"--MM-dd",
					"--MM-ddZ",
					"--MM-ddzzzzzz",
					"---dd",
					"---ddZ",
					"---ddzzzzzz",
					"--MM--",
					"--MM--Z",
					"--MM--zzzzzz"
				};
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006A70 File Offset: 0x00005A70
		[Obsolete("Use XmlConvert.ToDateTime() that takes in XmlDateTimeSerializationMode")]
		public static DateTime ToDateTime(string s)
		{
			return XmlConvert.ToDateTime(s, XmlConvert.AllDateTimeFormats);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006A7D File Offset: 0x00005A7D
		public static DateTime ToDateTime(string s, string format)
		{
			return DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006A8C File Offset: 0x00005A8C
		public static DateTime ToDateTime(string s, string[] formats)
		{
			return DateTime.ParseExact(s, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006A9C File Offset: 0x00005A9C
		public static DateTime ToDateTime(string s, XmlDateTimeSerializationMode dateTimeOption)
		{
			XsdDateTime xdt = new XsdDateTime(s, XsdDateTimeFlags.AllXsd);
			DateTime dateTime = xdt;
			switch (dateTimeOption)
			{
			case XmlDateTimeSerializationMode.Local:
				dateTime = XmlConvert.SwitchToLocalTime(dateTime);
				break;
			case XmlDateTimeSerializationMode.Utc:
				dateTime = XmlConvert.SwitchToUtcTime(dateTime);
				break;
			case XmlDateTimeSerializationMode.Unspecified:
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Unspecified);
				break;
			case XmlDateTimeSerializationMode.RoundtripKind:
				break;
			default:
				throw new ArgumentException(Res.GetString("Sch_InvalidDateTimeOption", new object[]
				{
					dateTimeOption
				}));
			}
			return dateTime;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006B1C File Offset: 0x00005B1C
		public static DateTimeOffset ToDateTimeOffset(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			XsdDateTime xdt = new XsdDateTime(s, XsdDateTimeFlags.AllXsd);
			return xdt;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006B4C File Offset: 0x00005B4C
		public static DateTimeOffset ToDateTimeOffset(string s, string format)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return DateTimeOffset.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006B69 File Offset: 0x00005B69
		public static DateTimeOffset ToDateTimeOffset(string s, string[] formats)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return DateTimeOffset.ParseExact(s, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006B86 File Offset: 0x00005B86
		public static Guid ToGuid(string s)
		{
			return new Guid(s);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006B90 File Offset: 0x00005B90
		internal static Exception TryToGuid(string s, out Guid result)
		{
			Exception result2 = null;
			result = Guid.Empty;
			try
			{
				result = new Guid(s);
			}
			catch (ArgumentException)
			{
				result2 = new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Guid"
				}));
			}
			catch (FormatException)
			{
				result2 = new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Guid"
				}));
			}
			return result2;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00006C24 File Offset: 0x00005C24
		private static DateTime SwitchToLocalTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Local);
			case DateTimeKind.Utc:
				return value.ToLocalTime();
			case DateTimeKind.Local:
				return value;
			default:
				return value;
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006C68 File Offset: 0x00005C68
		private static DateTime SwitchToUtcTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Utc);
			case DateTimeKind.Utc:
				return value;
			case DateTimeKind.Local:
				return value.ToUniversalTime();
			default:
				return value;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00006CAC File Offset: 0x00005CAC
		internal static Uri ToUri(string s)
		{
			if (s != null && s.Length > 0)
			{
				s = XmlConvert.TrimString(s);
				if (s.Length == 0 || s.IndexOf("##", StringComparison.Ordinal) != -1)
				{
					throw new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
					{
						s,
						"Uri"
					}));
				}
			}
			Uri result;
			if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out result))
			{
				throw new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Uri"
				}));
			}
			return result;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006D38 File Offset: 0x00005D38
		internal static Exception TryToUri(string s, out Uri result)
		{
			result = null;
			if (s != null && s.Length > 0)
			{
				s = XmlConvert.TrimString(s);
				if (s.Length == 0 || s.IndexOf("##", StringComparison.Ordinal) != -1)
				{
					return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
					{
						s,
						"Uri"
					}));
				}
			}
			if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out result))
			{
				return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[]
				{
					s,
					"Uri"
				}));
			}
			return null;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006DC8 File Offset: 0x00005DC8
		internal static bool StrEqual(char[] chars, int strPos1, int strLen1, string str2)
		{
			if (strLen1 != str2.Length)
			{
				return false;
			}
			int num = 0;
			while (num < strLen1 && chars[strPos1 + num] == str2[num])
			{
				num++;
			}
			return num == strLen1;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006DFE File Offset: 0x00005DFE
		internal static string TrimString(string value)
		{
			return value.Trim(XmlConvert.WhitespaceChars);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006E0B File Offset: 0x00005E0B
		internal static string[] SplitString(string value)
		{
			return value.Split(XmlConvert.WhitespaceChars, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006E19 File Offset: 0x00005E19
		internal static bool IsNegativeZero(double value)
		{
			return value == 0.0 && BitConverter.DoubleToInt64Bits(value) == BitConverter.DoubleToInt64Bits(--0.0);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006E40 File Offset: 0x00005E40
		internal unsafe static void VerifyCharData(string data, ExceptionType exceptionType)
		{
			if (data == null || data.Length == 0)
			{
				return;
			}
			XmlCharType instance = XmlCharType.Instance;
			int num = 0;
			int length = data.Length;
			for (;;)
			{
				if (num >= length || (instance.charProperties[data[num]] & 16) == 0)
				{
					if (num == length)
					{
						break;
					}
					char c = data[num];
					if (c < '\ud800' || c > '\udbff')
					{
						goto IL_A0;
					}
					if (num + 1 == length)
					{
						goto Block_6;
					}
					c = data[num + 1];
					if (c < '\udc00' || c > '\udfff')
					{
						goto IL_89;
					}
					num += 2;
				}
				else
				{
					num++;
				}
			}
			return;
			Block_6:
			throw XmlConvert.CreateException("Xml_InvalidSurrogateMissingLowChar", exceptionType);
			IL_89:
			throw XmlConvert.CreateInvalidSurrogatePairException(data[num + 1], data[num], exceptionType);
			IL_A0:
			throw XmlConvert.CreateInvalidCharException(data[num]);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006EFC File Offset: 0x00005EFC
		internal unsafe static void VerifyCharData(char[] data, int offset, int len, ExceptionType exceptionType)
		{
			if (data == null || len == 0)
			{
				return;
			}
			XmlCharType instance = XmlCharType.Instance;
			int num = offset;
			int num2 = offset + len;
			for (;;)
			{
				if (num >= num2 || (instance.charProperties[data[num]] & 16) == 0)
				{
					if (num == num2)
					{
						break;
					}
					char c = data[num];
					if (c < '\ud800' || c > '\udbff')
					{
						goto IL_84;
					}
					if (num + 1 == num2)
					{
						goto Block_6;
					}
					c = data[num + 1];
					if (c < '\udc00' || c > '\udfff')
					{
						goto IL_75;
					}
					num += 2;
				}
				else
				{
					num++;
				}
			}
			return;
			Block_6:
			throw XmlConvert.CreateException("Xml_InvalidSurrogateMissingLowChar", exceptionType);
			IL_75:
			throw XmlConvert.CreateInvalidSurrogatePairException(data[num + 1], data[num], exceptionType);
			IL_84:
			throw XmlConvert.CreateInvalidCharException(data[num]);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006F98 File Offset: 0x00005F98
		internal static string EscapeValueForDebuggerDisplay(string value)
		{
			StringBuilder stringBuilder = null;
			int i = 0;
			int num = 0;
			while (i < value.Length)
			{
				char c = value[i];
				if (c < ' ' || c == '"')
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value.Length + 4);
					}
					if (i - num > 0)
					{
						stringBuilder.Append(value, num, i - num);
					}
					num = i + 1;
					char c2 = c;
					switch (c2)
					{
					case '\t':
						stringBuilder.Append("\\t");
						goto IL_AE;
					case '\n':
						stringBuilder.Append("\\n");
						goto IL_AE;
					case '\v':
					case '\f':
						break;
					case '\r':
						stringBuilder.Append("\\r");
						goto IL_AE;
					default:
						if (c2 == '"')
						{
							stringBuilder.Append("\\\"");
							goto IL_AE;
						}
						break;
					}
					stringBuilder.Append(c);
				}
				IL_AE:
				i++;
			}
			if (stringBuilder == null)
			{
				return value;
			}
			if (i - num > 0)
			{
				stringBuilder.Append(value, num, i - num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007080 File Offset: 0x00006080
		internal static Exception CreateException(string res, ExceptionType exceptionType)
		{
			switch (exceptionType)
			{
			case ExceptionType.ArgumentException:
				return new ArgumentException(Res.GetString(res));
			}
			return new XmlException(res, string.Empty);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000070B8 File Offset: 0x000060B8
		internal static Exception CreateException(string res, string arg, ExceptionType exceptionType)
		{
			switch (exceptionType)
			{
			case ExceptionType.ArgumentException:
				return new ArgumentException(Res.GetString(res, new object[]
				{
					arg
				}));
			}
			return new XmlException(res, arg);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000070F8 File Offset: 0x000060F8
		internal static Exception CreateException(string res, string[] args, ExceptionType exceptionType)
		{
			switch (exceptionType)
			{
			case ExceptionType.ArgumentException:
				return new ArgumentException(Res.GetString(res, args));
			}
			return new XmlException(res, args);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000712B File Offset: 0x0000612B
		internal static Exception CreateInvalidSurrogatePairException(char low, char hi)
		{
			return XmlConvert.CreateInvalidSurrogatePairException(low, hi, ExceptionType.ArgumentException);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007138 File Offset: 0x00006138
		internal static Exception CreateInvalidSurrogatePairException(char low, char hi, ExceptionType exceptionType)
		{
			string[] array = new string[2];
			string[] array2 = array;
			int num = 0;
			uint num2 = (uint)hi;
			array2[num] = num2.ToString("X", CultureInfo.InvariantCulture);
			string[] array3 = array;
			int num3 = 1;
			uint num4 = (uint)low;
			array3[num3] = num4.ToString("X", CultureInfo.InvariantCulture);
			string[] args = array;
			return XmlConvert.CreateException("Xml_InvalidSurrogatePairWithArgs", args, exceptionType);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007186 File Offset: 0x00006186
		internal static Exception CreateInvalidHighSurrogateCharException(char hi)
		{
			return XmlConvert.CreateInvalidHighSurrogateCharException(hi, ExceptionType.ArgumentException);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007190 File Offset: 0x00006190
		internal static Exception CreateInvalidHighSurrogateCharException(char hi, ExceptionType exceptionType)
		{
			string res = "Xml_InvalidSurrogateHighChar";
			uint num = (uint)hi;
			return XmlConvert.CreateException(res, num.ToString("X", CultureInfo.InvariantCulture), exceptionType);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000071BB File Offset: 0x000061BB
		internal static Exception CreateInvalidCharException(char invChar)
		{
			return XmlConvert.CreateInvalidCharException(invChar, ExceptionType.ArgumentException);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000071C4 File Offset: 0x000061C4
		internal static Exception CreateInvalidCharException(char invChar, ExceptionType exceptionType)
		{
			return XmlConvert.CreateException("Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(invChar), exceptionType);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000071D7 File Offset: 0x000061D7
		internal static ArgumentException CreateInvalidNameArgumentException(string name, string argumentName)
		{
			if (name != null)
			{
				return new ArgumentException(Res.GetString("Xml_EmptyName"), argumentName);
			}
			return new ArgumentNullException(argumentName);
		}

		// Token: 0x040004A9 RID: 1193
		internal const int SurHighStart = 55296;

		// Token: 0x040004AA RID: 1194
		internal const int SurHighEnd = 56319;

		// Token: 0x040004AB RID: 1195
		internal const int SurLowStart = 56320;

		// Token: 0x040004AC RID: 1196
		internal const int SurLowEnd = 57343;

		// Token: 0x040004AD RID: 1197
		internal const int SurMask = 64512;

		// Token: 0x040004AE RID: 1198
		internal static char[] crt = new char[]
		{
			'\n',
			'\r',
			'\t'
		};

		// Token: 0x040004AF RID: 1199
		private static readonly int c_EncodedCharLength = 7;

		// Token: 0x040004B0 RID: 1200
		private static Regex c_EncodeCharPattern;

		// Token: 0x040004B1 RID: 1201
		private static Regex c_DecodeCharPattern;

		// Token: 0x040004B2 RID: 1202
		private static string[] s_allDateTimeFormats;

		// Token: 0x040004B3 RID: 1203
		internal static readonly char[] WhitespaceChars = new char[]
		{
			' ',
			'\t',
			'\n',
			'\r'
		};
	}
}
