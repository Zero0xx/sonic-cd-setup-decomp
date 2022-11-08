using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000504 RID: 1284
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class TypeLibVersionAttribute : Attribute
	{
		// Token: 0x0600319B RID: 12699 RVA: 0x000A98CF File Offset: 0x000A88CF
		public TypeLibVersionAttribute(int major, int minor)
		{
			this._major = major;
			this._minor = minor;
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x0600319C RID: 12700 RVA: 0x000A98E5 File Offset: 0x000A88E5
		public int MajorVersion
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x0600319D RID: 12701 RVA: 0x000A98ED File Offset: 0x000A88ED
		public int MinorVersion
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x040019A8 RID: 6568
		internal int _major;

		// Token: 0x040019A9 RID: 6569
		internal int _minor;
	}
}
