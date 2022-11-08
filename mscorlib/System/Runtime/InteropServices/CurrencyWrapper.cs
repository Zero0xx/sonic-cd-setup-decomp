using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051E RID: 1310
	[ComVisible(true)]
	[Serializable]
	public sealed class CurrencyWrapper
	{
		// Token: 0x060032D6 RID: 13014 RVA: 0x000ABB1F File Offset: 0x000AAB1F
		public CurrencyWrapper(decimal obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000ABB2E File Offset: 0x000AAB2E
		public CurrencyWrapper(object obj)
		{
			if (!(obj is decimal))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDecimal"), "obj");
			}
			this.m_WrappedObject = (decimal)obj;
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x000ABB5F File Offset: 0x000AAB5F
		public decimal WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x040019FA RID: 6650
		private decimal m_WrappedObject;
	}
}
