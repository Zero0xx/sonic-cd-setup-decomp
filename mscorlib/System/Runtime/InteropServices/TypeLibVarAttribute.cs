using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F2 RID: 1266
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibVarAttribute : Attribute
	{
		// Token: 0x06003163 RID: 12643 RVA: 0x000A926F File Offset: 0x000A826F
		public TypeLibVarAttribute(TypeLibVarFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x000A927E File Offset: 0x000A827E
		public TypeLibVarAttribute(short flags)
		{
			this._val = (TypeLibVarFlags)flags;
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x000A928D File Offset: 0x000A828D
		public TypeLibVarFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001935 RID: 6453
		internal TypeLibVarFlags _val;
	}
}
