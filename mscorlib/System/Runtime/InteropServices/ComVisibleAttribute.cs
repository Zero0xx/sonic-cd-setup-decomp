using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E2 RID: 1250
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	public sealed class ComVisibleAttribute : Attribute
	{
		// Token: 0x06003147 RID: 12615 RVA: 0x000A9067 File Offset: 0x000A8067
		public ComVisibleAttribute(bool visibility)
		{
			this._val = visibility;
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x000A9076 File Offset: 0x000A8076
		public bool Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018FD RID: 6397
		internal bool _val;
	}
}
