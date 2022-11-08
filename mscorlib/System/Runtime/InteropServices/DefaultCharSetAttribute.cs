using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000507 RID: 1287
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Module, Inherited = false)]
	public sealed class DefaultCharSetAttribute : Attribute
	{
		// Token: 0x060031A5 RID: 12709 RVA: 0x000A9951 File Offset: 0x000A8951
		public DefaultCharSetAttribute(CharSet charSet)
		{
			this._CharSet = charSet;
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x000A9960 File Offset: 0x000A8960
		public CharSet CharSet
		{
			get
			{
				return this._CharSet;
			}
		}

		// Token: 0x040019B0 RID: 6576
		internal CharSet _CharSet;
	}
}
