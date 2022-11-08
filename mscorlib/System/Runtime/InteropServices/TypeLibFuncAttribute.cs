using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F1 RID: 1265
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibFuncAttribute : Attribute
	{
		// Token: 0x06003160 RID: 12640 RVA: 0x000A9249 File Offset: 0x000A8249
		public TypeLibFuncAttribute(TypeLibFuncFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x000A9258 File Offset: 0x000A8258
		public TypeLibFuncAttribute(short flags)
		{
			this._val = (TypeLibFuncFlags)flags;
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x000A9267 File Offset: 0x000A8267
		public TypeLibFuncFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001934 RID: 6452
		internal TypeLibFuncFlags _val;
	}
}
