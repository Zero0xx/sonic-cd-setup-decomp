using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005E3 RID: 1507
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class DecimalConstantAttribute : Attribute
	{
		// Token: 0x060037DD RID: 14301 RVA: 0x000BBB69 File Offset: 0x000BAB69
		[CLSCompliant(false)]
		public DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low)
		{
			this.dec = new decimal((int)low, (int)mid, (int)hi, sign != 0, scale);
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x000BBB89 File Offset: 0x000BAB89
		public DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low)
		{
			this.dec = new decimal(low, mid, hi, sign != 0, scale);
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x060037DF RID: 14303 RVA: 0x000BBBA9 File Offset: 0x000BABA9
		public decimal Value
		{
			get
			{
				return this.dec;
			}
		}

		// Token: 0x04001CE7 RID: 7399
		private decimal dec;
	}
}
