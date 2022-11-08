using System;
using System.Security;
using System.Threading;

namespace System
{
	// Token: 0x0200079D RID: 1949
	internal static class ClientUtils
	{
		// Token: 0x06003C00 RID: 15360 RVA: 0x00100AE3 File Offset: 0x000FFAE3
		public static bool IsCriticalException(Exception ex)
		{
			return ex is NullReferenceException || ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is ExecutionEngineException || ex is IndexOutOfRangeException || ex is AccessViolationException;
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x00100B20 File Offset: 0x000FFB20
		public static bool IsSecurityOrCriticalException(Exception ex)
		{
			return ex is SecurityException || ClientUtils.IsCriticalException(ex);
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x00100B34 File Offset: 0x000FFB34
		public static int GetBitCount(uint x)
		{
			int num = 0;
			while (x > 0U)
			{
				x &= x - 1U;
				num++;
			}
			return num;
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x00100B58 File Offset: 0x000FFB58
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x00100B78 File Offset: 0x000FFB78
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue, int maxNumberOfBitsOn)
		{
			bool flag = value >= minValue && value <= maxValue;
			return flag && ClientUtils.GetBitCount((uint)value) <= maxNumberOfBitsOn;
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00100BAC File Offset: 0x000FFBAC
		public static bool IsEnumValid_Masked(Enum enumValue, int value, uint mask)
		{
			return ((long)value & (long)((ulong)mask)) == (long)value;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x00100BC4 File Offset: 0x000FFBC4
		public static bool IsEnumValid_NotSequential(Enum enumValue, int value, params int[] enumValues)
		{
			for (int i = 0; i < enumValues.Length; i++)
			{
				if (enumValues[i] == value)
				{
					return true;
				}
			}
			return false;
		}
	}
}
