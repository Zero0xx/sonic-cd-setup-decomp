using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DC RID: 1244
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, Inherited = false)]
	public sealed class DispIdAttribute : Attribute
	{
		// Token: 0x0600313D RID: 12605 RVA: 0x000A8FED File Offset: 0x000A7FED
		public DispIdAttribute(int dispId)
		{
			this._val = dispId;
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x0600313E RID: 12606 RVA: 0x000A8FFC File Offset: 0x000A7FFC
		public int Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018F1 RID: 6385
		internal int _val;
	}
}
