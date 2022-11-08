using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000E2 RID: 226
	internal class Number
	{
		// Token: 0x06000C33 RID: 3123 RVA: 0x00024332 File Offset: 0x00023332
		private Number()
		{
		}

		// Token: 0x06000C34 RID: 3124
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string FormatDecimal(decimal value, string format, NumberFormatInfo info);

		// Token: 0x06000C35 RID: 3125
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string FormatDouble(double value, string format, NumberFormatInfo info);

		// Token: 0x06000C36 RID: 3126
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string FormatInt32(int value, string format, NumberFormatInfo info);

		// Token: 0x06000C37 RID: 3127
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string FormatUInt32(uint value, string format, NumberFormatInfo info);

		// Token: 0x06000C38 RID: 3128
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string FormatInt64(long value, string format, NumberFormatInfo info);

		// Token: 0x06000C39 RID: 3129
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string FormatUInt64(ulong value, string format, NumberFormatInfo info);

		// Token: 0x06000C3A RID: 3130
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string FormatSingle(float value, string format, NumberFormatInfo info);

		// Token: 0x06000C3B RID: 3131
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern bool NumberBufferToDecimal(byte* number, ref decimal value);

		// Token: 0x06000C3C RID: 3132
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern bool NumberBufferToDouble(byte* number, ref double value);

		// Token: 0x06000C3D RID: 3133 RVA: 0x0002433C File Offset: 0x0002333C
		private static bool HexNumberToInt32(ref Number.NumberBuffer number, ref int value)
		{
			uint num = 0U;
			bool result = Number.HexNumberToUInt32(ref number, ref num);
			value = (int)num;
			return result;
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00024358 File Offset: 0x00023358
		private static bool HexNumberToInt64(ref Number.NumberBuffer number, ref long value)
		{
			ulong num = 0UL;
			bool result = Number.HexNumberToUInt64(ref number, ref num);
			value = (long)num;
			return result;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00024378 File Offset: 0x00023378
		private unsafe static bool HexNumberToUInt32(ref Number.NumberBuffer number, ref uint value)
		{
			int num = number.scale;
			if (num > 10 || num < number.precision)
			{
				return false;
			}
			char* ptr = number.digits;
			uint num2 = 0U;
			while (--num >= 0)
			{
				if (num2 > 268435455U)
				{
					return false;
				}
				num2 *= 16U;
				if (*ptr != '\0')
				{
					uint num3 = num2;
					if (*ptr != '\0')
					{
						if (*ptr >= '0' && *ptr <= '9')
						{
							num3 += (uint)(*ptr - '0');
						}
						else if (*ptr >= 'A' && *ptr <= 'F')
						{
							num3 += (uint)(*ptr - 'A' + '\n');
						}
						else
						{
							num3 += (uint)(*ptr - 'a' + '\n');
						}
						ptr++;
					}
					if (num3 < num2)
					{
						return false;
					}
					num2 = num3;
				}
			}
			value = num2;
			return true;
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00024414 File Offset: 0x00023414
		private unsafe static bool HexNumberToUInt64(ref Number.NumberBuffer number, ref ulong value)
		{
			int num = number.scale;
			if (num > 20 || num < number.precision)
			{
				return false;
			}
			char* ptr = number.digits;
			ulong num2 = 0UL;
			while (--num >= 0)
			{
				if (num2 > 1152921504606846975UL)
				{
					return false;
				}
				num2 *= 16UL;
				if (*ptr != '\0')
				{
					ulong num3 = num2;
					if (*ptr != '\0')
					{
						if (*ptr >= '0' && *ptr <= '9')
						{
							num3 += (ulong)((long)(*ptr - '0'));
						}
						else if (*ptr >= 'A' && *ptr <= 'F')
						{
							num3 += (ulong)((long)(*ptr - 'A' + '\n'));
						}
						else
						{
							num3 += (ulong)((long)(*ptr - 'a' + '\n'));
						}
						ptr++;
					}
					if (num3 < num2)
					{
						return false;
					}
					num2 = num3;
				}
			}
			value = num2;
			return true;
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x000244B8 File Offset: 0x000234B8
		private static bool IsWhite(char ch)
		{
			return ch == ' ' || (ch >= '\t' && ch <= '\r');
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000244D0 File Offset: 0x000234D0
		private unsafe static bool NumberToInt32(ref Number.NumberBuffer number, ref int value)
		{
			int num = number.scale;
			if (num > 10 || num < number.precision)
			{
				return false;
			}
			char* digits = number.digits;
			int num2 = 0;
			while (--num >= 0)
			{
				if (num2 > 214748364)
				{
					return false;
				}
				num2 *= 10;
				if (*digits != '\0')
				{
					num2 += (int)(*(digits++) - '0');
				}
			}
			if (number.sign)
			{
				num2 = -num2;
				if (num2 > 0)
				{
					return false;
				}
			}
			else if (num2 < 0)
			{
				return false;
			}
			value = num2;
			return true;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00024544 File Offset: 0x00023544
		private unsafe static bool NumberToInt64(ref Number.NumberBuffer number, ref long value)
		{
			int num = number.scale;
			if (num > 19 || num < number.precision)
			{
				return false;
			}
			char* digits = number.digits;
			long num2 = 0L;
			while (--num >= 0)
			{
				if (num2 > 922337203685477580L)
				{
					return false;
				}
				num2 *= 10L;
				if (*digits != '\0')
				{
					num2 += (long)(*(digits++) - '0');
				}
			}
			if (number.sign)
			{
				num2 = -num2;
				if (num2 > 0L)
				{
					return false;
				}
			}
			else if (num2 < 0L)
			{
				return false;
			}
			value = num2;
			return true;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000245C0 File Offset: 0x000235C0
		private unsafe static bool NumberToUInt32(ref Number.NumberBuffer number, ref uint value)
		{
			int num = number.scale;
			if (num > 10 || num < number.precision || number.sign)
			{
				return false;
			}
			char* digits = number.digits;
			uint num2 = 0U;
			while (--num >= 0)
			{
				if (num2 > 429496729U)
				{
					return false;
				}
				num2 *= 10U;
				if (*digits != '\0')
				{
					uint num3 = num2 + (uint)(*(digits++) - '0');
					if (num3 < num2)
					{
						return false;
					}
					num2 = num3;
				}
			}
			value = num2;
			return true;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0002462C File Offset: 0x0002362C
		private unsafe static bool NumberToUInt64(ref Number.NumberBuffer number, ref ulong value)
		{
			int num = number.scale;
			if (num > 20 || num < number.precision || number.sign)
			{
				return false;
			}
			char* digits = number.digits;
			ulong num2 = 0UL;
			while (--num >= 0)
			{
				if (num2 > 1844674407370955161UL)
				{
					return false;
				}
				num2 *= 10UL;
				if (*digits != '\0')
				{
					ulong num3 = num2 + (ulong)((long)(*(digits++) - '0'));
					if (num3 < num2)
					{
						return false;
					}
					num2 = num3;
				}
			}
			value = num2;
			return true;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x000246A0 File Offset: 0x000236A0
		private unsafe static char* MatchChars(char* p, string str)
		{
			IntPtr intPtr2;
			IntPtr intPtr = intPtr2 = str;
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* str2 = intPtr2;
			return Number.MatchChars(p, str2);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x000246C6 File Offset: 0x000236C6
		private unsafe static char* MatchChars(char* p, char* str)
		{
			if (*str == '\0')
			{
				return null;
			}
			while (*str != '\0')
			{
				if (*p != *str && (*str != '\u00a0' || *p != ' '))
				{
					return null;
				}
				p++;
				str++;
			}
			return p;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x000246F8 File Offset: 0x000236F8
		internal unsafe static decimal ParseDecimal(string value, NumberStyles options, NumberFormatInfo numfmt)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			decimal result = 0m;
			Number.StringToNumber(value, options, ref numberBuffer, numfmt, true);
			if (!Number.NumberBufferToDecimal(numberBuffer.PackForNative(), ref result))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Decimal"));
			}
			return result;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0002474C File Offset: 0x0002374C
		internal unsafe static double ParseDouble(string value, NumberStyles options, NumberFormatInfo numfmt)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			double result = 0.0;
			Number.StringToNumber(value, options, ref numberBuffer, numfmt, false);
			if (!Number.NumberBufferToDouble(numberBuffer.PackForNative(), ref result))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Double"));
			}
			return result;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000247A4 File Offset: 0x000237A4
		internal unsafe static int ParseInt32(string s, NumberStyles style, NumberFormatInfo info)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			int result = 0;
			Number.StringToNumber(s, style, ref numberBuffer, info, false);
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (!Number.HexNumberToInt32(ref numberBuffer, ref result))
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
				}
			}
			else if (!Number.NumberToInt32(ref numberBuffer, ref result))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
			}
			return result;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00024814 File Offset: 0x00023814
		internal unsafe static long ParseInt64(string value, NumberStyles options, NumberFormatInfo numfmt)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			long result = 0L;
			Number.StringToNumber(value, options, ref numberBuffer, numfmt, false);
			if ((options & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (!Number.HexNumberToInt64(ref numberBuffer, ref result))
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_Int64"));
				}
			}
			else if (!Number.NumberToInt64(ref numberBuffer, ref result))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int64"));
			}
			return result;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00024884 File Offset: 0x00023884
		private unsafe static bool ParseNumber(ref char* str, NumberStyles options, ref Number.NumberBuffer number, NumberFormatInfo numfmt, bool parseDecimal)
		{
			number.scale = 0;
			number.sign = false;
			string text = null;
			string text2 = null;
			string str2 = null;
			string str3 = null;
			bool flag = false;
			string str4;
			string str5;
			if ((options & NumberStyles.AllowCurrencySymbol) != NumberStyles.None)
			{
				text = numfmt.CurrencySymbol;
				if (numfmt.ansiCurrencySymbol != null)
				{
					text2 = numfmt.ansiCurrencySymbol;
				}
				str2 = numfmt.NumberDecimalSeparator;
				str3 = numfmt.NumberGroupSeparator;
				str4 = numfmt.CurrencyDecimalSeparator;
				str5 = numfmt.CurrencyGroupSeparator;
				flag = true;
			}
			else
			{
				str4 = numfmt.NumberDecimalSeparator;
				str5 = numfmt.NumberGroupSeparator;
			}
			int num = 0;
			char* ptr = str;
			char c = *ptr;
			for (;;)
			{
				if (!Number.IsWhite(c) || (options & NumberStyles.AllowLeadingWhite) == NumberStyles.None || ((num & 1) != 0 && ((num & 1) == 0 || ((num & 32) == 0 && numfmt.numberNegativePattern != 2))))
				{
					bool flag2;
					char* ptr2;
					if ((flag2 = ((options & NumberStyles.AllowLeadingSign) != NumberStyles.None && (num & 1) == 0)) && (ptr2 = Number.MatchChars(ptr, numfmt.positiveSign)) != null)
					{
						num |= 1;
						ptr = ptr2 - 1;
					}
					else if (flag2 && (ptr2 = Number.MatchChars(ptr, numfmt.negativeSign)) != null)
					{
						num |= 1;
						number.sign = true;
						ptr = ptr2 - 1;
					}
					else if (c == '(' && (options & NumberStyles.AllowParentheses) != NumberStyles.None && (num & 1) == 0)
					{
						num |= 3;
						number.sign = true;
					}
					else
					{
						if ((text == null || (ptr2 = Number.MatchChars(ptr, text)) == null) && (text2 == null || (ptr2 = Number.MatchChars(ptr, text2)) == null))
						{
							break;
						}
						num |= 32;
						text = null;
						text2 = null;
						ptr = ptr2 - 1;
					}
				}
				c = *(++ptr);
			}
			int num2 = 0;
			int num3 = 0;
			for (;;)
			{
				char* ptr2;
				if ((c >= '0' && c <= '9') || ((options & NumberStyles.AllowHexSpecifier) != NumberStyles.None && ((c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))))
				{
					num |= 4;
					if (c != '0' || (num & 8) != 0)
					{
						if (num2 < 50)
						{
							number.digits[(IntPtr)(num2++) * 2] = c;
							if (c != '0' || parseDecimal)
							{
								num3 = num2;
							}
						}
						if ((num & 16) == 0)
						{
							number.scale++;
						}
						num |= 8;
					}
					else if ((num & 16) != 0)
					{
						number.scale--;
					}
				}
				else if ((options & NumberStyles.AllowDecimalPoint) != NumberStyles.None && (num & 16) == 0 && ((ptr2 = Number.MatchChars(ptr, str4)) != null || (flag && (num & 32) == 0 && (ptr2 = Number.MatchChars(ptr, str2)) != null)))
				{
					num |= 16;
					ptr = ptr2 - 1;
				}
				else
				{
					if ((options & NumberStyles.AllowThousands) == NumberStyles.None || (num & 4) == 0 || (num & 16) != 0 || ((ptr2 = Number.MatchChars(ptr, str5)) == null && (!flag || (num & 32) != 0 || (ptr2 = Number.MatchChars(ptr, str3)) == null)))
					{
						break;
					}
					ptr = ptr2 - 1;
				}
				c = *(++ptr);
			}
			bool flag3 = false;
			number.precision = num3;
			number.digits[num3] = '\0';
			if ((num & 4) != 0)
			{
				if ((c == 'E' || c == 'e') && (options & NumberStyles.AllowExponent) != NumberStyles.None)
				{
					char* ptr3 = ptr;
					c = *(++ptr);
					char* ptr2;
					if ((ptr2 = Number.MatchChars(ptr, numfmt.positiveSign)) != null)
					{
						c = *(ptr = ptr2);
					}
					else if ((ptr2 = Number.MatchChars(ptr, numfmt.negativeSign)) != null)
					{
						c = *(ptr = ptr2);
						flag3 = true;
					}
					if (c >= '0' && c <= '9')
					{
						int num4 = 0;
						do
						{
							num4 = num4 * 10 + (int)(c - '0');
							c = *(++ptr);
							if (num4 > 1000)
							{
								num4 = 9999;
								while (c >= '0' && c <= '9')
								{
									c = *(++ptr);
								}
							}
						}
						while (c >= '0' && c <= '9');
						if (flag3)
						{
							num4 = -num4;
						}
						number.scale += num4;
					}
					else
					{
						ptr = ptr3;
						c = *ptr;
					}
				}
				for (;;)
				{
					if (!Number.IsWhite(c) || (options & NumberStyles.AllowTrailingWhite) == NumberStyles.None)
					{
						bool flag2;
						char* ptr2;
						if ((flag2 = ((options & NumberStyles.AllowTrailingSign) != NumberStyles.None && (num & 1) == 0)) && (ptr2 = Number.MatchChars(ptr, numfmt.positiveSign)) != null)
						{
							num |= 1;
							ptr = ptr2 - 1;
						}
						else if (flag2 && (ptr2 = Number.MatchChars(ptr, numfmt.negativeSign)) != null)
						{
							num |= 1;
							number.sign = true;
							ptr = ptr2 - 1;
						}
						else if (c == ')' && (num & 2) != 0)
						{
							num &= -3;
						}
						else
						{
							if ((text == null || (ptr2 = Number.MatchChars(ptr, text)) == null) && (text2 == null || (ptr2 = Number.MatchChars(ptr, text2)) == null))
							{
								break;
							}
							text = null;
							text2 = null;
							ptr = ptr2 - 1;
						}
					}
					c = *(++ptr);
				}
				if ((num & 2) == 0)
				{
					if ((num & 8) == 0)
					{
						if (!parseDecimal)
						{
							number.scale = 0;
						}
						if ((num & 16) == 0)
						{
							number.sign = false;
						}
					}
					str = ptr;
					return true;
				}
			}
			str = ptr;
			return false;
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00024D58 File Offset: 0x00023D58
		internal unsafe static float ParseSingle(string value, NumberStyles options, NumberFormatInfo numfmt)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			double num = 0.0;
			Number.StringToNumber(value, options, ref numberBuffer, numfmt, false);
			if (!Number.NumberBufferToDouble(numberBuffer.PackForNative(), ref num))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Single"));
			}
			float num2 = (float)num;
			if (float.IsInfinity(num2))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Single"));
			}
			return num2;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00024DCC File Offset: 0x00023DCC
		internal unsafe static uint ParseUInt32(string value, NumberStyles options, NumberFormatInfo numfmt)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			uint result = 0U;
			Number.StringToNumber(value, options, ref numberBuffer, numfmt, false);
			if ((options & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (!Number.HexNumberToUInt32(ref numberBuffer, ref result))
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
				}
			}
			else if (!Number.NumberToUInt32(ref numberBuffer, ref result))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return result;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00024E3C File Offset: 0x00023E3C
		internal unsafe static ulong ParseUInt64(string value, NumberStyles options, NumberFormatInfo numfmt)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			ulong result = 0UL;
			Number.StringToNumber(value, options, ref numberBuffer, numfmt, false);
			if ((options & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (!Number.HexNumberToUInt64(ref numberBuffer, ref result))
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
				}
			}
			else if (!Number.NumberToUInt64(ref numberBuffer, ref result))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return result;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00024EAC File Offset: 0x00023EAC
		private unsafe static void StringToNumber(string str, NumberStyles options, ref Number.NumberBuffer number, NumberFormatInfo info, bool parseDecimal)
		{
			if (str == null)
			{
				throw new ArgumentNullException("String");
			}
			fixed (char* ptr = str)
			{
				char* ptr2 = ptr;
				if (!Number.ParseNumber(ref ptr2, options, ref number, info, parseDecimal) || ((long)(ptr2 - ptr) < (long)str.Length && !Number.TrailingZeros(str, (int)((long)(ptr2 - ptr)))))
				{
					throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
				}
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00024F18 File Offset: 0x00023F18
		private static bool TrailingZeros(string s, int index)
		{
			for (int i = index; i < s.Length; i++)
			{
				if (s[i] != '\0')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00024F44 File Offset: 0x00023F44
		internal unsafe static bool TryParseDecimal(string value, NumberStyles options, NumberFormatInfo numfmt, out decimal result)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			result = 0m;
			return Number.TryStringToNumber(value, options, ref numberBuffer, numfmt, true) && Number.NumberBufferToDecimal(numberBuffer.PackForNative(), ref result);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00024F94 File Offset: 0x00023F94
		internal unsafe static bool TryParseDouble(string value, NumberStyles options, NumberFormatInfo numfmt, out double result)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			result = 0.0;
			return Number.TryStringToNumber(value, options, ref numberBuffer, numfmt, false) && Number.NumberBufferToDouble(numberBuffer.PackForNative(), ref result);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00024FE4 File Offset: 0x00023FE4
		internal unsafe static bool TryParseInt32(string s, NumberStyles style, NumberFormatInfo info, out int result)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			result = 0;
			if (!Number.TryStringToNumber(s, style, ref numberBuffer, info, false))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (!Number.HexNumberToInt32(ref numberBuffer, ref result))
				{
					return false;
				}
			}
			else if (!Number.NumberToInt32(ref numberBuffer, ref result))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0002503C File Offset: 0x0002403C
		internal unsafe static bool TryParseInt64(string s, NumberStyles style, NumberFormatInfo info, out long result)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			result = 0L;
			if (!Number.TryStringToNumber(s, style, ref numberBuffer, info, false))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (!Number.HexNumberToInt64(ref numberBuffer, ref result))
				{
					return false;
				}
			}
			else if (!Number.NumberToInt64(ref numberBuffer, ref result))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00025094 File Offset: 0x00024094
		internal unsafe static bool TryParseSingle(string value, NumberStyles options, NumberFormatInfo numfmt, out float result)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			result = 0f;
			double num = 0.0;
			if (!Number.TryStringToNumber(value, options, ref numberBuffer, numfmt, false))
			{
				return false;
			}
			if (!Number.NumberBufferToDouble(numberBuffer.PackForNative(), ref num))
			{
				return false;
			}
			float num2 = (float)num;
			if (float.IsInfinity(num2))
			{
				return false;
			}
			result = num2;
			return true;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000250F8 File Offset: 0x000240F8
		internal unsafe static bool TryParseUInt32(string s, NumberStyles style, NumberFormatInfo info, out uint result)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			result = 0U;
			if (!Number.TryStringToNumber(s, style, ref numberBuffer, info, false))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (!Number.HexNumberToUInt32(ref numberBuffer, ref result))
				{
					return false;
				}
			}
			else if (!Number.NumberToUInt32(ref numberBuffer, ref result))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00025150 File Offset: 0x00024150
		internal unsafe static bool TryParseUInt64(string s, NumberStyles style, NumberFormatInfo info, out ulong result)
		{
			byte* stackBuffer = stackalloc byte[1 * 114];
			Number.NumberBuffer numberBuffer = new Number.NumberBuffer(stackBuffer);
			result = 0UL;
			if (!Number.TryStringToNumber(s, style, ref numberBuffer, info, false))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (!Number.HexNumberToUInt64(ref numberBuffer, ref result))
				{
					return false;
				}
			}
			else if (!Number.NumberToUInt64(ref numberBuffer, ref result))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000251A8 File Offset: 0x000241A8
		private unsafe static bool TryStringToNumber(string str, NumberStyles options, ref Number.NumberBuffer number, NumberFormatInfo numfmt, bool parseDecimal)
		{
			if (str == null)
			{
				return false;
			}
			fixed (char* ptr = str)
			{
				char* ptr2 = ptr;
				if (!Number.ParseNumber(ref ptr2, options, ref number, numfmt, parseDecimal) || ((long)(ptr2 - ptr) < (long)str.Length && !Number.TrailingZeros(str, (int)((long)(ptr2 - ptr)))))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000461 RID: 1121
		private const int NumberMaxDigits = 50;

		// Token: 0x04000462 RID: 1122
		private const int Int32Precision = 10;

		// Token: 0x04000463 RID: 1123
		private const int UInt32Precision = 10;

		// Token: 0x04000464 RID: 1124
		private const int Int64Precision = 19;

		// Token: 0x04000465 RID: 1125
		private const int UInt64Precision = 20;

		// Token: 0x020000E3 RID: 227
		private struct NumberBuffer
		{
			// Token: 0x06000C5A RID: 3162 RVA: 0x000251FF File Offset: 0x000241FF
			public unsafe NumberBuffer(byte* stackBuffer)
			{
				this.baseAddress = stackBuffer;
				this.digits = (char*)(stackBuffer + 12);
				this.precision = 0;
				this.scale = 0;
				this.sign = false;
			}

			// Token: 0x06000C5B RID: 3163 RVA: 0x00025228 File Offset: 0x00024228
			public unsafe byte* PackForNative()
			{
				int* ptr = (int*)this.baseAddress;
				*ptr = this.precision;
				ptr[1] = this.scale;
				ptr[2] = (this.sign ? 1 : 0);
				return this.baseAddress;
			}

			// Token: 0x04000466 RID: 1126
			public const int NumberBufferBytes = 114;

			// Token: 0x04000467 RID: 1127
			private unsafe byte* baseAddress;

			// Token: 0x04000468 RID: 1128
			public unsafe char* digits;

			// Token: 0x04000469 RID: 1129
			public int precision;

			// Token: 0x0400046A RID: 1130
			public int scale;

			// Token: 0x0400046B RID: 1131
			public bool sign;
		}
	}
}
