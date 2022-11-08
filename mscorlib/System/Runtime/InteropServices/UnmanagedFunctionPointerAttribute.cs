using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DA RID: 1242
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class UnmanagedFunctionPointerAttribute : Attribute
	{
		// Token: 0x0600313A RID: 12602 RVA: 0x000A8FCE File Offset: 0x000A7FCE
		public UnmanagedFunctionPointerAttribute(CallingConvention callingConvention)
		{
			this.m_callingConvention = callingConvention;
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x000A8FDD File Offset: 0x000A7FDD
		public CallingConvention CallingConvention
		{
			get
			{
				return this.m_callingConvention;
			}
		}

		// Token: 0x040018EC RID: 6380
		private CallingConvention m_callingConvention;

		// Token: 0x040018ED RID: 6381
		public CharSet CharSet;

		// Token: 0x040018EE RID: 6382
		public bool BestFitMapping;

		// Token: 0x040018EF RID: 6383
		public bool ThrowOnUnmappableChar;

		// Token: 0x040018F0 RID: 6384
		public bool SetLastError;
	}
}
