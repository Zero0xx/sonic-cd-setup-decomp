using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E7 RID: 1255
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ProgIdAttribute : Attribute
	{
		// Token: 0x0600314F RID: 12623 RVA: 0x000A90C1 File Offset: 0x000A80C1
		public ProgIdAttribute(string progId)
		{
			this._val = progId;
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06003150 RID: 12624 RVA: 0x000A90D0 File Offset: 0x000A80D0
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001900 RID: 6400
		internal string _val;
	}
}
