using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000501 RID: 1281
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
	public sealed class PrimaryInteropAssemblyAttribute : Attribute
	{
		// Token: 0x06003193 RID: 12691 RVA: 0x000A986C File Offset: 0x000A886C
		public PrimaryInteropAssemblyAttribute(int major, int minor)
		{
			this._major = major;
			this._minor = minor;
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003194 RID: 12692 RVA: 0x000A9882 File Offset: 0x000A8882
		public int MajorVersion
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06003195 RID: 12693 RVA: 0x000A988A File Offset: 0x000A888A
		public int MinorVersion
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x040019A3 RID: 6563
		internal int _major;

		// Token: 0x040019A4 RID: 6564
		internal int _minor;
	}
}
