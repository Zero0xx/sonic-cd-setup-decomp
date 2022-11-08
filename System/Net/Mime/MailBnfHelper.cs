using System;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000699 RID: 1689
	internal static class MailBnfHelper
	{
		// Token: 0x06003420 RID: 13344 RVA: 0x000DBE2C File Offset: 0x000DAE2C
		static MailBnfHelper()
		{
			for (int i = 48; i <= 57; i++)
			{
				MailBnfHelper.s_atext[i] = true;
			}
			for (int j = 65; j <= 90; j++)
			{
				MailBnfHelper.s_atext[j] = true;
			}
			for (int k = 97; k <= 122; k++)
			{
				MailBnfHelper.s_atext[k] = true;
			}
			MailBnfHelper.s_atext[33] = true;
			MailBnfHelper.s_atext[35] = true;
			MailBnfHelper.s_atext[36] = true;
			MailBnfHelper.s_atext[37] = true;
			MailBnfHelper.s_atext[38] = true;
			MailBnfHelper.s_atext[39] = true;
			MailBnfHelper.s_atext[42] = true;
			MailBnfHelper.s_atext[43] = true;
			MailBnfHelper.s_atext[45] = true;
			MailBnfHelper.s_atext[47] = true;
			MailBnfHelper.s_atext[61] = true;
			MailBnfHelper.s_atext[63] = true;
			MailBnfHelper.s_atext[94] = true;
			MailBnfHelper.s_atext[95] = true;
			MailBnfHelper.s_atext[96] = true;
			MailBnfHelper.s_atext[123] = true;
			MailBnfHelper.s_atext[124] = true;
			MailBnfHelper.s_atext[125] = true;
			MailBnfHelper.s_atext[126] = true;
			for (int l = 1; l <= 8; l++)
			{
				MailBnfHelper.s_qtext[l] = true;
			}
			MailBnfHelper.s_qtext[11] = true;
			MailBnfHelper.s_qtext[12] = true;
			for (int m = 14; m <= 31; m++)
			{
				MailBnfHelper.s_qtext[m] = true;
			}
			MailBnfHelper.s_qtext[33] = true;
			for (int n = 35; n <= 91; n++)
			{
				MailBnfHelper.s_qtext[n] = true;
			}
			for (int num = 93; num <= 127; num++)
			{
				MailBnfHelper.s_qtext[num] = true;
			}
			for (int num2 = 1; num2 <= 9; num2++)
			{
				MailBnfHelper.s_fqtext[num2] = true;
			}
			MailBnfHelper.s_fqtext[11] = true;
			MailBnfHelper.s_fqtext[12] = true;
			for (int num3 = 14; num3 <= 33; num3++)
			{
				MailBnfHelper.s_fqtext[num3] = true;
			}
			for (int num4 = 35; num4 <= 91; num4++)
			{
				MailBnfHelper.s_fqtext[num4] = true;
			}
			for (int num5 = 93; num5 <= 127; num5++)
			{
				MailBnfHelper.s_fqtext[num5] = true;
			}
			for (int num6 = 1; num6 <= 8; num6++)
			{
				MailBnfHelper.s_dtext[num6] = true;
			}
			MailBnfHelper.s_dtext[11] = true;
			MailBnfHelper.s_dtext[12] = true;
			for (int num7 = 14; num7 <= 31; num7++)
			{
				MailBnfHelper.s_dtext[num7] = true;
			}
			for (int num8 = 33; num8 <= 90; num8++)
			{
				MailBnfHelper.s_dtext[num8] = true;
			}
			for (int num9 = 94; num9 <= 127; num9++)
			{
				MailBnfHelper.s_dtext[num9] = true;
			}
			for (int num10 = 1; num10 <= 9; num10++)
			{
				MailBnfHelper.s_fdtext[num10] = true;
			}
			MailBnfHelper.s_fdtext[11] = true;
			MailBnfHelper.s_fdtext[12] = true;
			for (int num11 = 14; num11 <= 90; num11++)
			{
				MailBnfHelper.s_fdtext[num11] = true;
			}
			for (int num12 = 94; num12 <= 127; num12++)
			{
				MailBnfHelper.s_fdtext[num12] = true;
			}
			for (int num13 = 33; num13 <= 57; num13++)
			{
				MailBnfHelper.s_ftext[num13] = true;
			}
			for (int num14 = 59; num14 <= 126; num14++)
			{
				MailBnfHelper.s_ftext[num14] = true;
			}
			for (int num15 = 33; num15 <= 126; num15++)
			{
				MailBnfHelper.s_ttext[num15] = true;
			}
			MailBnfHelper.s_ttext[40] = false;
			MailBnfHelper.s_ttext[41] = false;
			MailBnfHelper.s_ttext[60] = false;
			MailBnfHelper.s_ttext[62] = false;
			MailBnfHelper.s_ttext[64] = false;
			MailBnfHelper.s_ttext[44] = false;
			MailBnfHelper.s_ttext[59] = false;
			MailBnfHelper.s_ttext[58] = false;
			MailBnfHelper.s_ttext[92] = false;
			MailBnfHelper.s_ttext[34] = false;
			MailBnfHelper.s_ttext[47] = false;
			MailBnfHelper.s_ttext[91] = false;
			MailBnfHelper.s_ttext[93] = false;
			MailBnfHelper.s_ttext[63] = false;
			MailBnfHelper.s_ttext[61] = false;
			for (int num16 = 48; num16 <= 57; num16++)
			{
				MailBnfHelper.s_digits[num16] = true;
			}
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x000DC33C File Offset: 0x000DB33C
		internal static bool SkipCFWS(string data, ref int offset)
		{
			int num = 0;
			while (offset < data.Length)
			{
				if (data[offset] > '\u007f')
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				if (data[offset] == '\\' && num > 0)
				{
					offset += 2;
				}
				else if (data[offset] == '(')
				{
					num++;
				}
				else if (data[offset] == ')')
				{
					num--;
				}
				else if (data[offset] != ' ' && data[offset] != '\t' && num == 0)
				{
					return true;
				}
				if (num < 0)
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				offset++;
			}
			return false;
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x000DC3ED File Offset: 0x000DB3ED
		internal static bool SkipFWS(string data, ref int offset)
		{
			while (offset < data.Length)
			{
				if (data[offset] != ' ' && data[offset] != '\t')
				{
					return true;
				}
				offset++;
			}
			return false;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000DC41C File Offset: 0x000DB41C
		internal static void ValidateHeaderName(string data)
		{
			int i;
			for (i = 0; i < data.Length; i++)
			{
				if ((int)data[i] > MailBnfHelper.s_ftext.Length || !MailBnfHelper.s_ftext[(int)data[i]])
				{
					throw new FormatException(SR.GetString("InvalidHeaderName"));
				}
			}
			if (i == 0)
			{
				throw new FormatException(SR.GetString("InvalidHeaderName"));
			}
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000DC47C File Offset: 0x000DB47C
		internal static string ReadQuotedString(string data, ref int offset, StringBuilder builder)
		{
			bool unescapedUnicode = false;
			string result = MailBnfHelper.ReadQuotedString(data, ref offset, builder, false, ref unescapedUnicode);
			MailBnfHelper.ThrowForUnescapedUnicode(unescapedUnicode);
			return result;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000DC4A0 File Offset: 0x000DB4A0
		internal static string ReadQuotedString(string data, ref int offset, StringBuilder builder, bool returnQuotes, ref bool containsUnescapedUnicode)
		{
			if (data[offset] != '"')
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
			}
			offset++;
			int num = offset;
			StringBuilder stringBuilder = (builder != null) ? builder : new StringBuilder();
			if (returnQuotes)
			{
				stringBuilder.Append('"');
			}
			while (offset < data.Length)
			{
				if (data[offset] == '\\')
				{
					stringBuilder.Append(data, num, offset - num);
					num = ++offset;
				}
				else if (data[offset] == '"')
				{
					stringBuilder.Append(data, num, offset - num);
					offset++;
					if (returnQuotes)
					{
						stringBuilder.Append('"');
					}
					if (builder == null)
					{
						return stringBuilder.ToString();
					}
					return null;
				}
				else if ((int)data[offset] >= MailBnfHelper.s_fqtext.Length)
				{
					containsUnescapedUnicode = true;
				}
				else if (!MailBnfHelper.s_fqtext[(int)data[offset]])
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				offset++;
			}
			throw new FormatException(SR.GetString("MailHeaderFieldMalformedHeader"));
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x000DC5A8 File Offset: 0x000DB5A8
		internal static string ReadUnQuotedString(string data, ref int offset, StringBuilder builder)
		{
			int num = offset;
			StringBuilder stringBuilder = (builder != null) ? builder : new StringBuilder();
			while (offset < data.Length)
			{
				if (data[offset] == '\\')
				{
					stringBuilder.Append(data, num, offset - num);
					num = ++offset;
				}
				else
				{
					if (data[offset] == '"')
					{
						throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
					}
					if (!MailBnfHelper.s_fqtext[(int)data[offset]])
					{
						throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
					}
				}
				offset++;
			}
			stringBuilder.Append(data, num, offset - num);
			if (builder == null)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x000DC650 File Offset: 0x000DB650
		internal static string ReadPhrase(string data, ref int offset, StringBuilder builder, ref bool containsUnescapedUnicode)
		{
			StringBuilder stringBuilder = (builder != null) ? builder : new StringBuilder();
			bool flag = false;
			MailBnfHelper.SkipCFWS(data, ref offset);
			int num = offset;
			if (MailBnfHelper.SkipCFWS(data, ref offset) && data[offset] == '"')
			{
				string text = MailBnfHelper.ReadQuotedString(data, ref offset, null, true, ref containsUnescapedUnicode);
				if (MailBnfHelper.SkipCFWS(data, ref offset) && (data[offset] == '"' || MailBnfHelper.s_atext[(int)data[offset]] || data[offset] == '.'))
				{
					offset = num;
				}
				else
				{
					if (builder != null)
					{
						builder.Append(text);
						return null;
					}
					return text;
				}
			}
			while (MailBnfHelper.SkipCFWS(data, ref offset))
			{
				if (data[offset] == '"')
				{
					if (flag)
					{
						stringBuilder.Append(' ');
					}
					MailBnfHelper.ReadQuotedString(data, ref offset, stringBuilder, false, ref containsUnescapedUnicode);
					flag = true;
				}
				else
				{
					if (!MailBnfHelper.s_atext[(int)data[offset]])
					{
						break;
					}
					if (flag)
					{
						stringBuilder.Append(' ');
					}
					MailBnfHelper.ReadAtom(data, ref offset, stringBuilder);
					flag = true;
				}
			}
			if (num == offset)
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
			}
			if (builder == null)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000DC758 File Offset: 0x000DB758
		internal static string ReadAtom(string data, ref int offset, StringBuilder builder)
		{
			int num = offset;
			string text;
			while (offset < data.Length)
			{
				if ((int)data[offset] > MailBnfHelper.s_atext.Length)
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				if (!MailBnfHelper.s_atext[(int)data[offset]])
				{
					if (offset == num)
					{
						throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
					}
					text = data.Substring(num, offset - num);
					if (builder != null)
					{
						builder.Append(text);
						return null;
					}
					return text;
				}
				else
				{
					offset++;
				}
			}
			if (offset == num)
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
			}
			text = ((num == 0) ? data : data.Substring(num));
			if (builder != null)
			{
				builder.Append(text);
				return null;
			}
			return text;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000DC80C File Offset: 0x000DB80C
		internal static string ReadDotAtom(string data, ref int offset, StringBuilder builder)
		{
			bool flag = true;
			if (builder == null)
			{
				flag = false;
				builder = new StringBuilder();
			}
			if (data[offset] != '.')
			{
				MailBnfHelper.ReadAtom(data, ref offset, builder);
			}
			while (offset < data.Length && data[offset] == '.')
			{
				builder.Append(data[offset++]);
				MailBnfHelper.ReadAtom(data, ref offset, builder);
			}
			if (flag)
			{
				return null;
			}
			return builder.ToString();
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000DC880 File Offset: 0x000DB880
		internal static string ReadDomainLiteral(string data, ref int offset, StringBuilder builder)
		{
			int num = ++offset;
			StringBuilder stringBuilder = (builder != null) ? builder : new StringBuilder();
			while (offset < data.Length)
			{
				if (data[offset] == '\\')
				{
					stringBuilder.Append(data, num, offset - num);
					num = ++offset;
				}
				else if (data[offset] == ']')
				{
					stringBuilder.Append(data, num, offset - num);
					offset++;
					if (builder == null)
					{
						return stringBuilder.ToString();
					}
					return null;
				}
				else if ((int)data[offset] > MailBnfHelper.s_fdtext.Length || !MailBnfHelper.s_fdtext[(int)data[offset]])
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				offset++;
			}
			throw new FormatException(SR.GetString("MailHeaderFieldMalformedHeader"));
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000DC94B File Offset: 0x000DB94B
		internal static string ReadParameterAttribute(string data, ref int offset, StringBuilder builder)
		{
			if (!MailBnfHelper.SkipCFWS(data, ref offset))
			{
				return null;
			}
			return MailBnfHelper.ReadToken(data, ref offset, null);
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000DC960 File Offset: 0x000DB960
		internal static string ReadToken(string data, ref int offset, StringBuilder builder)
		{
			int num = offset;
			while (offset < data.Length)
			{
				if ((int)data[offset] > MailBnfHelper.s_ttext.Length)
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				if (!MailBnfHelper.s_ttext[(int)data[offset]])
				{
					break;
				}
				offset++;
			}
			if (num == offset)
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
			}
			return data.Substring(num, offset - num);
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x000DC9D4 File Offset: 0x000DB9D4
		internal static string ReadAngleAddress(string data, ref int offset, StringBuilder builder)
		{
			if (offset >= data.Length)
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			MailBnfHelper.SkipCFWS(data, ref offset);
			if (data[offset] == '"')
			{
				bool unescapedUnicode = false;
				MailBnfHelper.ReadQuotedString(data, ref offset, builder, true, ref unescapedUnicode);
				MailBnfHelper.ThrowForUnescapedUnicode(unescapedUnicode);
			}
			else
			{
				MailBnfHelper.ReadDotAtom(data, ref offset, builder);
			}
			MailBnfHelper.SkipCFWS(data, ref offset);
			if (offset >= data.Length || data[offset] != '@')
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			offset++;
			MailBnfHelper.SkipCFWS(data, ref offset);
			string result = MailBnfHelper.ReadAddressSpecDomain(data, ref offset, builder);
			if (!MailBnfHelper.SkipCFWS(data, ref offset) || data[offset++] != '>')
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			return result;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000DCAA0 File Offset: 0x000DBAA0
		internal static string ReadAddressSpecDomain(string data, ref int offset, StringBuilder builder)
		{
			if (offset >= data.Length)
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			builder.Append('@');
			MailBnfHelper.SkipCFWS(data, ref offset);
			if (data[offset] == '[')
			{
				MailBnfHelper.ReadDomainLiteral(data, ref offset, builder);
			}
			else
			{
				MailBnfHelper.ReadDotAtom(data, ref offset, builder);
			}
			MailBnfHelper.SkipCFWS(data, ref offset);
			return builder.ToString();
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000DCB06 File Offset: 0x000DBB06
		internal static void ThrowForUnescapedUnicode(bool unescapedUnicode)
		{
			if (unescapedUnicode)
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
			}
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000DCB1C File Offset: 0x000DBB1C
		internal static string ReadMailAddress(string data, ref int offset, out string displayName)
		{
			string result = null;
			Exception ex = null;
			displayName = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				MailBnfHelper.SkipCFWS(data, ref offset);
				if (offset >= data.Length)
				{
					ex = new FormatException(SR.GetString("MailAddressInvalidFormat"));
				}
				else
				{
					if (data[offset] == '<')
					{
						offset++;
						result = MailBnfHelper.ReadAngleAddress(data, ref offset, stringBuilder);
						return result;
					}
					bool unescapedUnicode = false;
					MailBnfHelper.ReadPhrase(data, ref offset, stringBuilder, ref unescapedUnicode);
					if (offset >= data.Length)
					{
						ex = new FormatException(SR.GetString("MailAddressInvalidFormat"));
					}
					else
					{
						char c = data[offset];
						if (c <= '.')
						{
							if (c != '"')
							{
								if (c == '.')
								{
									MailBnfHelper.ReadDotAtom(data, ref offset, stringBuilder);
									MailBnfHelper.SkipCFWS(data, ref offset);
									if (offset >= data.Length)
									{
										ex = new FormatException(SR.GetString("MailAddressInvalidFormat"));
										goto IL_281;
									}
									if (data[offset] == '@')
									{
										MailBnfHelper.ThrowForUnescapedUnicode(unescapedUnicode);
										offset++;
										result = MailBnfHelper.ReadAddressSpecDomain(data, ref offset, stringBuilder);
										goto IL_234;
									}
									if (data[offset] == '<')
									{
										displayName = stringBuilder.ToString();
										stringBuilder = new StringBuilder();
										offset++;
										result = MailBnfHelper.ReadAngleAddress(data, ref offset, stringBuilder);
										goto IL_234;
									}
									ex = new FormatException(SR.GetString("MailAddressInvalidFormat"));
									goto IL_281;
								}
							}
							else
							{
								offset++;
								if (offset >= data.Length)
								{
									ex = new FormatException(SR.GetString("MailAddressInvalidFormat"));
									goto IL_281;
								}
								MailBnfHelper.SkipCFWS(data, ref offset);
								if (offset >= data.Length)
								{
									ex = new FormatException(SR.GetString("MailAddressInvalidFormat"));
									goto IL_281;
								}
								if (data[offset] == '<')
								{
									offset++;
									result = MailBnfHelper.ReadAngleAddress(data, ref offset, stringBuilder);
									goto IL_234;
								}
								result = MailBnfHelper.ReadAddressSpecDomain(data, ref offset, stringBuilder);
								goto IL_234;
							}
						}
						else
						{
							switch (c)
							{
							case ':':
								ex = new FormatException(SR.GetString("MailAddressUnsupportedFormat"));
								goto IL_281;
							case ';':
								break;
							case '<':
								displayName = stringBuilder.ToString();
								stringBuilder = new StringBuilder();
								offset++;
								result = MailBnfHelper.ReadAngleAddress(data, ref offset, stringBuilder);
								goto IL_234;
							default:
								if (c == '@')
								{
									MailBnfHelper.ThrowForUnescapedUnicode(unescapedUnicode);
									offset++;
									result = MailBnfHelper.ReadAddressSpecDomain(data, ref offset, stringBuilder);
									goto IL_234;
								}
								break;
							}
						}
						ex = new FormatException(SR.GetString("MailAddressInvalidFormat"));
						goto IL_281;
						IL_234:
						if (offset < data.Length)
						{
							MailBnfHelper.SkipCFWS(data, ref offset);
							if (offset < data.Length && data[offset] != ',')
							{
								ex = new FormatException(SR.GetString("MailAddressInvalidFormat"));
							}
						}
					}
				}
			}
			catch (FormatException)
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			IL_281:
			if (ex != null)
			{
				throw ex;
			}
			return result;
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000DCDD0 File Offset: 0x000DBDD0
		internal static MailAddress ReadMailAddress(string data, ref int offset)
		{
			string encodedDisplayName = null;
			string address = MailBnfHelper.ReadMailAddress(data, ref offset, out encodedDisplayName);
			return new MailAddress(address, encodedDisplayName, 0U);
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000DCDF4 File Offset: 0x000DBDF4
		internal static DateTime ReadDateTime(string data, ref int offset)
		{
			if (!MailBnfHelper.SkipCFWS(data, ref offset))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			if (MailBnfHelper.IsValidDOW(data, ref offset))
			{
				if (offset >= data.Length || data[offset] != ',')
				{
					throw new FormatException(SR.GetString("MailDateInvalidFormat"));
				}
				offset++;
			}
			if (!MailBnfHelper.SkipFWS(data, ref offset))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			int day = MailBnfHelper.ReadDateNumber(data, ref offset, 2);
			if (offset >= data.Length || (data[offset] != ' ' && data[offset] != '\t'))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			if (!MailBnfHelper.SkipFWS(data, ref offset))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			int month = MailBnfHelper.ReadMonth(data, ref offset);
			if (offset >= data.Length || (data[offset] != ' ' && data[offset] != '\t'))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			if (!MailBnfHelper.SkipFWS(data, ref offset))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			int year = MailBnfHelper.ReadDateNumber(data, ref offset, 4);
			if (offset >= data.Length || (data[offset] != ' ' && data[offset] != '\t'))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			if (!MailBnfHelper.SkipFWS(data, ref offset))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			int hour = MailBnfHelper.ReadDateNumber(data, ref offset, 2);
			if (offset >= data.Length || data[offset] != ':')
			{
				throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
			}
			offset++;
			int minute = MailBnfHelper.ReadDateNumber(data, ref offset, 2);
			int second = 0;
			if (offset < data.Length && data[offset] == ':')
			{
				offset++;
				second = MailBnfHelper.ReadDateNumber(data, ref offset, 2);
			}
			if (!MailBnfHelper.SkipFWS(data, ref offset))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			if (offset >= data.Length || (data[offset] != '-' && data[offset] != '+'))
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			offset++;
			MailBnfHelper.ReadDateNumber(data, ref offset, 4);
			return new DateTime(year, month, day, hour, minute, second);
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000DD034 File Offset: 0x000DC034
		private static bool IsValidDOW(string data, ref int offset)
		{
			if (offset + 3 >= data.Length)
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			for (int i = 0; i < MailBnfHelper.s_days.Length; i++)
			{
				if (string.Compare(MailBnfHelper.s_days[i], 0, data, offset, 3, true, CultureInfo.InvariantCulture) == 0)
				{
					offset += 3;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000DD094 File Offset: 0x000DC094
		private static int ReadDateNumber(string data, ref int offset, int maxSize)
		{
			int num = 0;
			int num2 = offset + maxSize;
			if (offset >= data.Length)
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			while (offset < data.Length && offset < num2 && data[offset] >= '0' && data[offset] <= '9')
			{
				num = num * 10 + (int)(data[offset] - '0');
				offset++;
			}
			if (num == 0)
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			return num;
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000DD114 File Offset: 0x000DC114
		private static int ReadMonth(string data, ref int offset)
		{
			if (offset >= data.Length - 3)
			{
				throw new FormatException(SR.GetString("MailDateInvalidFormat"));
			}
			char c = data[offset++];
			if (c <= 'S')
			{
				if (c <= 'F')
				{
					if (c == 'A')
					{
						goto IL_21F;
					}
					switch (c)
					{
					case 'D':
						goto IL_371;
					case 'E':
						goto IL_3B2;
					case 'F':
						goto IL_17A;
					default:
						goto IL_3B2;
					}
				}
				else
				{
					switch (c)
					{
					case 'J':
						break;
					case 'K':
					case 'L':
						goto IL_3B2;
					case 'M':
						goto IL_1C0;
					case 'N':
						goto IL_330;
					case 'O':
						goto IL_2E9;
					default:
						if (c != 'S')
						{
							goto IL_3B2;
						}
						goto IL_2A2;
					}
				}
			}
			else if (c <= 'f')
			{
				if (c == 'a')
				{
					goto IL_21F;
				}
				switch (c)
				{
				case 'd':
					goto IL_371;
				case 'e':
					goto IL_3B2;
				case 'f':
					goto IL_17A;
				default:
					goto IL_3B2;
				}
			}
			else
			{
				switch (c)
				{
				case 'j':
					break;
				case 'k':
				case 'l':
					goto IL_3B2;
				case 'm':
					goto IL_1C0;
				case 'n':
					goto IL_330;
				case 'o':
					goto IL_2E9;
				default:
					if (c != 's')
					{
						goto IL_3B2;
					}
					goto IL_2A2;
				}
			}
			char c2 = data[offset++];
			if (c2 <= 'U')
			{
				if (c2 != 'A')
				{
					if (c2 != 'U')
					{
						goto IL_3B2;
					}
					goto IL_132;
				}
			}
			else if (c2 != 'a')
			{
				if (c2 != 'u')
				{
					goto IL_3B2;
				}
				goto IL_132;
			}
			char c3 = data[offset++];
			if (c3 == 'N' || c3 == 'n')
			{
				return 1;
			}
			goto IL_3B2;
			IL_132:
			char c4 = data[offset++];
			switch (c4)
			{
			case 'L':
				return 7;
			case 'M':
				goto IL_3B2;
			case 'N':
				break;
			default:
				switch (c4)
				{
				case 'l':
					return 7;
				case 'm':
					goto IL_3B2;
				case 'n':
					break;
				default:
					goto IL_3B2;
				}
				break;
			}
			return 6;
			IL_17A:
			char c5 = data[offset++];
			if (c5 != 'E' && c5 != 'e')
			{
				goto IL_3B2;
			}
			char c6 = data[offset++];
			if (c6 == 'B' || c6 == 'b')
			{
				return 2;
			}
			goto IL_3B2;
			IL_1C0:
			char c7 = data[offset++];
			if (c7 == 'A' || c7 == 'a')
			{
				char c8 = data[offset++];
				if (c8 <= 'Y')
				{
					if (c8 == 'R')
					{
						return 3;
					}
					if (c8 != 'Y')
					{
						goto IL_3B2;
					}
				}
				else
				{
					if (c8 == 'r')
					{
						return 3;
					}
					if (c8 != 'y')
					{
						goto IL_3B2;
					}
				}
				return 5;
			}
			goto IL_3B2;
			IL_21F:
			char c9 = data[offset++];
			if (c9 <= 'U')
			{
				if (c9 != 'P')
				{
					if (c9 != 'U')
					{
						goto IL_3B2;
					}
					goto IL_27E;
				}
			}
			else if (c9 != 'p')
			{
				if (c9 != 'u')
				{
					goto IL_3B2;
				}
				goto IL_27E;
			}
			char c10 = data[offset++];
			if (c10 == 'R' || c10 == 'r')
			{
				return 4;
			}
			goto IL_3B2;
			IL_27E:
			char c11 = data[offset++];
			if (c11 == 'G' || c11 == 'g')
			{
				return 8;
			}
			goto IL_3B2;
			IL_2A2:
			char c12 = data[offset++];
			if (c12 != 'E' && c12 != 'e')
			{
				goto IL_3B2;
			}
			char c13 = data[offset++];
			if (c13 == 'P' || c13 == 'p')
			{
				return 9;
			}
			goto IL_3B2;
			IL_2E9:
			char c14 = data[offset++];
			if (c14 != 'C' && c14 != 'c')
			{
				goto IL_3B2;
			}
			char c15 = data[offset++];
			if (c15 == 'T' || c15 == 't')
			{
				return 10;
			}
			goto IL_3B2;
			IL_330:
			char c16 = data[offset++];
			if (c16 != 'O' && c16 != 'o')
			{
				goto IL_3B2;
			}
			char c17 = data[offset++];
			if (c17 == 'V' || c17 == 'v')
			{
				return 11;
			}
			goto IL_3B2;
			IL_371:
			char c18 = data[offset++];
			if (c18 == 'E' || c18 == 'e')
			{
				char c19 = data[offset++];
				if (c19 == 'C' || c19 == 'c')
				{
					return 12;
				}
			}
			IL_3B2:
			throw new FormatException(SR.GetString("MailDateInvalidFormat"));
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000DD4E4 File Offset: 0x000DC4E4
		internal static string GetDateTimeString(DateTime value, StringBuilder builder)
		{
			StringBuilder stringBuilder = (builder != null) ? builder : new StringBuilder();
			stringBuilder.Append(value.Day);
			stringBuilder.Append(' ');
			stringBuilder.Append(MailBnfHelper.s_months[value.Month]);
			stringBuilder.Append(' ');
			stringBuilder.Append(value.Year);
			stringBuilder.Append(' ');
			if (value.Hour <= 9)
			{
				stringBuilder.Append('0');
			}
			stringBuilder.Append(value.Hour);
			stringBuilder.Append(':');
			if (value.Minute <= 9)
			{
				stringBuilder.Append('0');
			}
			stringBuilder.Append(value.Minute);
			stringBuilder.Append(':');
			if (value.Second <= 9)
			{
				stringBuilder.Append('0');
			}
			stringBuilder.Append(value.Second);
			string text = TimeZone.CurrentTimeZone.GetUtcOffset(value).ToString();
			if (text[0] != '-')
			{
				stringBuilder.Append(" +");
			}
			else
			{
				stringBuilder.Append(" ");
			}
			string[] array = text.Split(new char[]
			{
				':'
			});
			stringBuilder.Append(array[0]);
			stringBuilder.Append(array[1]);
			if (builder == null)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000DD638 File Offset: 0x000DC638
		internal static string GetTokenOrQuotedString(string data, StringBuilder builder)
		{
			int i = 0;
			int num = 0;
			while (i < data.Length)
			{
				if ((int)data[i] > MailBnfHelper.s_ttext.Length)
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				if (!MailBnfHelper.s_ttext[(int)data[i]] || data[i] == ' ')
				{
					StringBuilder stringBuilder = (builder != null) ? builder : new StringBuilder();
					builder.Append('"');
					while (i < data.Length)
					{
						if ((int)data[i] > MailBnfHelper.s_fqtext.Length)
						{
							throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
						}
						if (!MailBnfHelper.s_fqtext[(int)data[i]])
						{
							builder.Append(data, num, i - num);
							builder.Append('\\');
							num = i;
						}
						i++;
					}
					builder.Append(data, num, i - num);
					builder.Append('"');
					if (builder == null)
					{
						return stringBuilder.ToString();
					}
					return null;
				}
				else
				{
					i++;
				}
			}
			if (data.Length == 0)
			{
				if (builder == null)
				{
					return "\"\"";
				}
				builder.Append("\"\"");
			}
			if (builder != null)
			{
				builder.Append(data);
				return null;
			}
			return data;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000DD754 File Offset: 0x000DC754
		internal static string GetDotAtomOrQuotedString(string data, StringBuilder builder)
		{
			bool flag = data.StartsWith("\"") && data.EndsWith("\"");
			int i = flag ? 1 : 0;
			int num = 0;
			while (i < data.Length)
			{
				if ((int)data[i] > MailBnfHelper.s_atext.Length)
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				if ((data[i] != '.' && !MailBnfHelper.s_atext[(int)data[i]]) || data[i] == ' ')
				{
					StringBuilder stringBuilder = (builder != null) ? builder : new StringBuilder();
					if (!flag)
					{
						builder.Append('"');
					}
					while ((!flag && i < data.Length) || (flag && i < data.Length - 1))
					{
						if ((int)data[i] > MailBnfHelper.s_fqtext.Length)
						{
							throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
						}
						if (!MailBnfHelper.s_fqtext[(int)data[i]])
						{
							builder.Append(data, num, i - num);
							builder.Append('\\');
							num = i;
						}
						i++;
					}
					builder.Append(data, num, i - num);
					builder.Append('"');
					if (builder == null)
					{
						return stringBuilder.ToString();
					}
					return null;
				}
				else
				{
					i++;
				}
			}
			if (builder != null)
			{
				builder.Append(data);
				return null;
			}
			return data;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000DD894 File Offset: 0x000DC894
		internal static string GetDotAtomOrDomainLiteral(string data, StringBuilder builder)
		{
			int i = 0;
			int num = 0;
			while (i < data.Length)
			{
				if ((int)data[i] > MailBnfHelper.s_atext.Length)
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				if (data[i] != '.' && !MailBnfHelper.s_atext[(int)data[i]])
				{
					StringBuilder stringBuilder = (builder != null) ? builder : new StringBuilder();
					builder.Append('[');
					while (i < data.Length)
					{
						if ((int)data[i] > MailBnfHelper.s_fdtext.Length)
						{
							throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
						}
						if (!MailBnfHelper.s_fdtext[(int)data[i]])
						{
							builder.Append(data, num, i - num);
							builder.Append('\\');
							num = i;
						}
						i++;
					}
					builder.Append(data, num, i - num);
					builder.Append(']');
					if (builder == null)
					{
						return stringBuilder.ToString();
					}
					return null;
				}
				else
				{
					i++;
				}
			}
			if (builder != null)
			{
				builder.Append(data);
				return null;
			}
			return data;
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000DD994 File Offset: 0x000DC994
		internal static bool HasCROrLF(string data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] == '\r' || data[i] == '\n')
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002FF7 RID: 12279
		private static bool[] s_atext = new bool[128];

		// Token: 0x04002FF8 RID: 12280
		private static bool[] s_qtext = new bool[128];

		// Token: 0x04002FF9 RID: 12281
		private static bool[] s_fqtext = new bool[128];

		// Token: 0x04002FFA RID: 12282
		private static bool[] s_dtext = new bool[128];

		// Token: 0x04002FFB RID: 12283
		private static bool[] s_fdtext = new bool[128];

		// Token: 0x04002FFC RID: 12284
		private static bool[] s_ftext = new bool[128];

		// Token: 0x04002FFD RID: 12285
		private static bool[] s_ttext = new bool[128];

		// Token: 0x04002FFE RID: 12286
		private static bool[] s_digits = new bool[128];

		// Token: 0x04002FFF RID: 12287
		private static string[] s_months = new string[]
		{
			null,
			"Jan",
			"Feb",
			"Mar",
			"Apr",
			"May",
			"Jun",
			"Jul",
			"Aug",
			"Sep",
			"Oct",
			"Nov",
			"Dec"
		};

		// Token: 0x04003000 RID: 12288
		private static string[] s_days = new string[]
		{
			"Mon",
			"Tue",
			"Wed",
			"Thu",
			"Fri",
			"Sat",
			"Sun"
		};
	}
}
