using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E4 RID: 1252
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class LCIDConversionAttribute : Attribute
	{
		// Token: 0x0600314B RID: 12619 RVA: 0x000A909A File Offset: 0x000A809A
		public LCIDConversionAttribute(int lcid)
		{
			this._val = lcid;
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x0600314C RID: 12620 RVA: 0x000A90A9 File Offset: 0x000A80A9
		public int Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018FF RID: 6399
		internal int _val;
	}
}
