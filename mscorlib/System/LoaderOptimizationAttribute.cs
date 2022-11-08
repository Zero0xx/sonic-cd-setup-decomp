using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000062 RID: 98
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class LoaderOptimizationAttribute : Attribute
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x00014D7A File Offset: 0x00013D7A
		public LoaderOptimizationAttribute(byte value)
		{
			this._val = value;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00014D89 File Offset: 0x00013D89
		public LoaderOptimizationAttribute(LoaderOptimization value)
		{
			this._val = (byte)value;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x00014D99 File Offset: 0x00013D99
		public LoaderOptimization Value
		{
			get
			{
				return (LoaderOptimization)this._val;
			}
		}

		// Token: 0x040001D5 RID: 469
		internal byte _val;
	}
}
