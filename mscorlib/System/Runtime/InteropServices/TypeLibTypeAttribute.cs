using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F0 RID: 1264
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, Inherited = false)]
	public sealed class TypeLibTypeAttribute : Attribute
	{
		// Token: 0x0600315D RID: 12637 RVA: 0x000A9223 File Offset: 0x000A8223
		public TypeLibTypeAttribute(TypeLibTypeFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x000A9232 File Offset: 0x000A8232
		public TypeLibTypeAttribute(short flags)
		{
			this._val = (TypeLibTypeFlags)flags;
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x0600315F RID: 12639 RVA: 0x000A9241 File Offset: 0x000A8241
		public TypeLibTypeFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001933 RID: 6451
		internal TypeLibTypeFlags _val;
	}
}
