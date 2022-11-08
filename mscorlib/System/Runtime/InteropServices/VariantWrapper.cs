using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000568 RID: 1384
	[Serializable]
	public sealed class VariantWrapper
	{
		// Token: 0x060033B9 RID: 13241 RVA: 0x000ADCFB File Offset: 0x000ACCFB
		public VariantWrapper(object obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x000ADD0A File Offset: 0x000ACD0A
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04001B0F RID: 6927
		private object m_WrappedObject;
	}
}
