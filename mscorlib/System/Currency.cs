using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x0200009E RID: 158
	[Serializable]
	internal struct Currency
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x0001C490 File Offset: 0x0001B490
		public Currency(decimal value)
		{
			this.m_value = decimal.ToCurrency(value).m_value;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001C4A3 File Offset: 0x0001B4A3
		internal Currency(long value, int ignored)
		{
			this.m_value = value;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001C4AC File Offset: 0x0001B4AC
		public static Currency FromOACurrency(long cy)
		{
			return new Currency(cy, 0);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001C4B5 File Offset: 0x0001B4B5
		public long ToOACurrency()
		{
			return this.m_value;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0001C4C0 File Offset: 0x0001B4C0
		public static decimal ToDecimal(Currency c)
		{
			decimal result = 0m;
			Currency.FCallToDecimal(ref result, c);
			return result;
		}

		// Token: 0x0600094D RID: 2381
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallToDecimal(ref decimal result, Currency c);

		// Token: 0x0400038B RID: 907
		internal long m_value;
	}
}
