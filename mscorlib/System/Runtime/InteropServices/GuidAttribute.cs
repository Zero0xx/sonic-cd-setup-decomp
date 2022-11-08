using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F7 RID: 1271
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	[ComVisible(true)]
	public sealed class GuidAttribute : Attribute
	{
		// Token: 0x06003172 RID: 12658 RVA: 0x000A944D File Offset: 0x000A844D
		public GuidAttribute(string guid)
		{
			this._val = guid;
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x000A945C File Offset: 0x000A845C
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001991 RID: 6545
		internal string _val;
	}
}
