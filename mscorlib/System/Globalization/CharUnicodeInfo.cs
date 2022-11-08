using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x0200038D RID: 909
	public sealed class CharUnicodeInfo
	{
		// Token: 0x06002392 RID: 9106 RVA: 0x00059EB4 File Offset: 0x00058EB4
		unsafe static CharUnicodeInfo()
		{
			CharUnicodeInfo.UnicodeDataHeader* pDataTable = (CharUnicodeInfo.UnicodeDataHeader*)CharUnicodeInfo.m_pDataTable;
			CharUnicodeInfo.m_pCategoryLevel1Index = (ushort*)(CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToCategoriesIndex);
			CharUnicodeInfo.m_pCategoriesValue = CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToCategoriesValue;
			CharUnicodeInfo.m_pNumericLevel1Index = (ushort*)(CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToNumbericIndex);
			CharUnicodeInfo.m_pNumericValues = CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToNumbericValue;
			CharUnicodeInfo.m_pDigitValues = (CharUnicodeInfo.DigitValues*)(CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToDigitValue);
			CharUnicodeInfo.nativeInitTable(CharUnicodeInfo.m_pDataTable);
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x00059F44 File Offset: 0x00058F44
		private CharUnicodeInfo()
		{
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x00059F4C File Offset: 0x00058F4C
		internal static int InternalConvertToUtf32(string s, int index)
		{
			if (index < s.Length - 1)
			{
				int num = (int)(s[index] - '\ud800');
				if (num >= 0 && num <= 1023)
				{
					int num2 = (int)(s[index + 1] - '\udc00');
					if (num2 >= 0 && num2 <= 1023)
					{
						return num * 1024 + num2 + 65536;
					}
				}
			}
			return (int)s[index];
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x00059FB4 File Offset: 0x00058FB4
		internal static int InternalConvertToUtf32(string s, int index, out int charLength)
		{
			charLength = 1;
			if (index < s.Length - 1)
			{
				int num = (int)(s[index] - '\ud800');
				if (num >= 0 && num <= 1023)
				{
					int num2 = (int)(s[index + 1] - '\udc00');
					if (num2 >= 0 && num2 <= 1023)
					{
						charLength++;
						return num * 1024 + num2 + 65536;
					}
				}
			}
			return (int)s[index];
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0005A024 File Offset: 0x00059024
		internal static bool IsWhiteSpace(string s, int index)
		{
			switch (CharUnicodeInfo.GetUnicodeCategory(s, index))
			{
			case UnicodeCategory.SpaceSeparator:
			case UnicodeCategory.LineSeparator:
			case UnicodeCategory.ParagraphSeparator:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0005A058 File Offset: 0x00059058
		internal static bool IsWhiteSpace(char c)
		{
			switch (CharUnicodeInfo.GetUnicodeCategory(c))
			{
			case UnicodeCategory.SpaceSeparator:
			case UnicodeCategory.LineSeparator:
			case UnicodeCategory.ParagraphSeparator:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0005A088 File Offset: 0x00059088
		internal unsafe static double InternalGetNumericValue(int ch)
		{
			ushort num = CharUnicodeInfo.m_pNumericLevel1Index[ch >> 8];
			num = CharUnicodeInfo.m_pNumericLevel1Index[(int)num + (ch >> 4 & 15)];
			byte* ptr = (byte*)(CharUnicodeInfo.m_pNumericLevel1Index + num);
			byte* ptr2 = CharUnicodeInfo.m_pNumericValues + (IntPtr)ptr[ch & 15] * 8;
			if (ptr2 % 8L != null)
			{
				double result;
				byte* dest = (byte*)(&result);
				Buffer.memcpyimpl(ptr2, dest, 8);
				return result;
			}
			return *(double*)(CharUnicodeInfo.m_pNumericValues + (IntPtr)ptr[ch & 15] * 8);
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0005A100 File Offset: 0x00059100
		internal unsafe static CharUnicodeInfo.DigitValues* InternalGetDigitValues(int ch)
		{
			ushort num = CharUnicodeInfo.m_pNumericLevel1Index[ch >> 8];
			num = CharUnicodeInfo.m_pNumericLevel1Index[(int)num + (ch >> 4 & 15)];
			byte* ptr = (byte*)(CharUnicodeInfo.m_pNumericLevel1Index + num);
			return CharUnicodeInfo.m_pDigitValues + ptr[ch & 15];
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0005A150 File Offset: 0x00059150
		internal unsafe static sbyte InternalGetDecimalDigitValue(int ch)
		{
			return CharUnicodeInfo.InternalGetDigitValues(ch)->decimalDigit;
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0005A15D File Offset: 0x0005915D
		internal unsafe static sbyte InternalGetDigitValue(int ch)
		{
			return CharUnicodeInfo.InternalGetDigitValues(ch)->digit;
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0005A16A File Offset: 0x0005916A
		public static double GetNumericValue(char ch)
		{
			return CharUnicodeInfo.InternalGetNumericValue((int)ch);
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0005A172 File Offset: 0x00059172
		public static double GetNumericValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return CharUnicodeInfo.InternalGetNumericValue(CharUnicodeInfo.InternalConvertToUtf32(s, index));
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0005A1B0 File Offset: 0x000591B0
		public static int GetDecimalDigitValue(char ch)
		{
			return (int)CharUnicodeInfo.InternalGetDecimalDigitValue((int)ch);
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x0005A1B8 File Offset: 0x000591B8
		public static int GetDecimalDigitValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return (int)CharUnicodeInfo.InternalGetDecimalDigitValue(CharUnicodeInfo.InternalConvertToUtf32(s, index));
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0005A1F6 File Offset: 0x000591F6
		public static int GetDigitValue(char ch)
		{
			return (int)CharUnicodeInfo.InternalGetDigitValue((int)ch);
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x0005A1FE File Offset: 0x000591FE
		public static int GetDigitValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return (int)CharUnicodeInfo.InternalGetDigitValue(CharUnicodeInfo.InternalConvertToUtf32(s, index));
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0005A23C File Offset: 0x0005923C
		public static UnicodeCategory GetUnicodeCategory(char ch)
		{
			return CharUnicodeInfo.InternalGetUnicodeCategory((int)ch);
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0005A244 File Offset: 0x00059244
		public static UnicodeCategory GetUnicodeCategory(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return CharUnicodeInfo.InternalGetUnicodeCategory(s, index);
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0005A26F File Offset: 0x0005926F
		internal static UnicodeCategory InternalGetUnicodeCategory(int ch)
		{
			return (UnicodeCategory)CharUnicodeInfo.InternalGetCategoryValue(ch, 0);
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0005A278 File Offset: 0x00059278
		internal unsafe static byte InternalGetCategoryValue(int ch, int offset)
		{
			ushort num = CharUnicodeInfo.m_pCategoryLevel1Index[ch >> 8];
			num = CharUnicodeInfo.m_pCategoryLevel1Index[(int)num + (ch >> 4 & 15)];
			byte* ptr = (byte*)(CharUnicodeInfo.m_pCategoryLevel1Index + num);
			byte b = ptr[ch & 15];
			return CharUnicodeInfo.m_pCategoriesValue[(int)(b * 2) + offset];
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0005A2C8 File Offset: 0x000592C8
		internal static BidiCategory GetBidiCategory(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return (BidiCategory)CharUnicodeInfo.InternalGetCategoryValue(CharUnicodeInfo.InternalConvertToUtf32(s, index), 1);
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0005A2F9 File Offset: 0x000592F9
		internal static UnicodeCategory InternalGetUnicodeCategory(string value, int index)
		{
			return CharUnicodeInfo.InternalGetUnicodeCategory(CharUnicodeInfo.InternalConvertToUtf32(value, index));
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0005A307 File Offset: 0x00059307
		internal static UnicodeCategory InternalGetUnicodeCategory(string str, int index, out int charLength)
		{
			return CharUnicodeInfo.InternalGetUnicodeCategory(CharUnicodeInfo.InternalConvertToUtf32(str, index, out charLength));
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0005A316 File Offset: 0x00059316
		internal static bool IsCombiningCategory(UnicodeCategory uc)
		{
			return uc == UnicodeCategory.NonSpacingMark || uc == UnicodeCategory.SpacingCombiningMark || uc == UnicodeCategory.EnclosingMark;
		}

		// Token: 0x060023AA RID: 9130
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void nativeInitTable(byte* bytePtr);

		// Token: 0x04000F24 RID: 3876
		internal const char HIGH_SURROGATE_START = '\ud800';

		// Token: 0x04000F25 RID: 3877
		internal const char HIGH_SURROGATE_END = '\udbff';

		// Token: 0x04000F26 RID: 3878
		internal const char LOW_SURROGATE_START = '\udc00';

		// Token: 0x04000F27 RID: 3879
		internal const char LOW_SURROGATE_END = '\udfff';

		// Token: 0x04000F28 RID: 3880
		internal const int UNICODE_CATEGORY_OFFSET = 0;

		// Token: 0x04000F29 RID: 3881
		internal const int BIDI_CATEGORY_OFFSET = 1;

		// Token: 0x04000F2A RID: 3882
		internal const string UNICODE_INFO_FILE_NAME = "charinfo.nlp";

		// Token: 0x04000F2B RID: 3883
		internal const int UNICODE_PLANE01_START = 65536;

		// Token: 0x04000F2C RID: 3884
		private unsafe static byte* m_pDataTable = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(CharUnicodeInfo).Assembly, "charinfo.nlp");

		// Token: 0x04000F2D RID: 3885
		private unsafe static ushort* m_pCategoryLevel1Index;

		// Token: 0x04000F2E RID: 3886
		private unsafe static byte* m_pCategoriesValue;

		// Token: 0x04000F2F RID: 3887
		private unsafe static ushort* m_pNumericLevel1Index;

		// Token: 0x04000F30 RID: 3888
		private unsafe static byte* m_pNumericValues;

		// Token: 0x04000F31 RID: 3889
		private unsafe static CharUnicodeInfo.DigitValues* m_pDigitValues;

		// Token: 0x0200038E RID: 910
		[StructLayout(LayoutKind.Explicit)]
		internal struct UnicodeDataHeader
		{
			// Token: 0x04000F32 RID: 3890
			[FieldOffset(0)]
			internal char TableName;

			// Token: 0x04000F33 RID: 3891
			[FieldOffset(32)]
			internal ushort version;

			// Token: 0x04000F34 RID: 3892
			[FieldOffset(40)]
			internal uint OffsetToCategoriesIndex;

			// Token: 0x04000F35 RID: 3893
			[FieldOffset(44)]
			internal uint OffsetToCategoriesValue;

			// Token: 0x04000F36 RID: 3894
			[FieldOffset(48)]
			internal uint OffsetToNumbericIndex;

			// Token: 0x04000F37 RID: 3895
			[FieldOffset(52)]
			internal uint OffsetToDigitValue;

			// Token: 0x04000F38 RID: 3896
			[FieldOffset(56)]
			internal uint OffsetToNumbericValue;
		}

		// Token: 0x0200038F RID: 911
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct DigitValues
		{
			// Token: 0x04000F39 RID: 3897
			internal sbyte decimalDigit;

			// Token: 0x04000F3A RID: 3898
			internal sbyte digit;
		}
	}
}
