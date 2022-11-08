using System;

namespace System.Xml
{
	// Token: 0x02000012 RID: 18
	internal static class Bits
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00002BFC File Offset: 0x00001BFC
		public static int Count(uint num)
		{
			num = (num & Bits.MASK_0101010101010101) + (num >> 1 & Bits.MASK_0101010101010101);
			num = (num & Bits.MASK_0011001100110011) + (num >> 2 & Bits.MASK_0011001100110011);
			num = (num & Bits.MASK_0000111100001111) + (num >> 4 & Bits.MASK_0000111100001111);
			num = (num & Bits.MASK_0000000011111111) + (num >> 8 & Bits.MASK_0000000011111111);
			num = (num & Bits.MASK_1111111111111111) + (num >> 16);
			return (int)num;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002C64 File Offset: 0x00001C64
		public static bool ExactlyOne(uint num)
		{
			return num != 0U && (num & num - 1U) == 0U;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C73 File Offset: 0x00001C73
		public static bool MoreThanOne(uint num)
		{
			return (num & num - 1U) != 0U;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002C80 File Offset: 0x00001C80
		public static uint ClearLeast(uint num)
		{
			return num & num - 1U;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002C87 File Offset: 0x00001C87
		public static int LeastPosition(uint num)
		{
			if (num == 0U)
			{
				return 0;
			}
			return Bits.Count(num ^ num - 1U);
		}

		// Token: 0x0400045A RID: 1114
		private static readonly uint MASK_0101010101010101 = 1431655765U;

		// Token: 0x0400045B RID: 1115
		private static readonly uint MASK_0011001100110011 = 858993459U;

		// Token: 0x0400045C RID: 1116
		private static readonly uint MASK_0000111100001111 = 252645135U;

		// Token: 0x0400045D RID: 1117
		private static readonly uint MASK_0000000011111111 = 16711935U;

		// Token: 0x0400045E RID: 1118
		private static readonly uint MASK_1111111111111111 = 65535U;
	}
}
