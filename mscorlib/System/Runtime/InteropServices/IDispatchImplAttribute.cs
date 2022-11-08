using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EA RID: 1258
	[Obsolete("This attribute is deprecated and will be removed in a future version.", false)]
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false)]
	public sealed class IDispatchImplAttribute : Attribute
	{
		// Token: 0x06003153 RID: 12627 RVA: 0x000A90EF File Offset: 0x000A80EF
		public IDispatchImplAttribute(IDispatchImplType implType)
		{
			this._val = implType;
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x000A90FE File Offset: 0x000A80FE
		public IDispatchImplAttribute(short implType)
		{
			this._val = (IDispatchImplType)implType;
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06003155 RID: 12629 RVA: 0x000A910D File Offset: 0x000A810D
		public IDispatchImplType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001906 RID: 6406
		internal IDispatchImplType _val;
	}
}
