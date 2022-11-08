using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000502 RID: 1282
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	public sealed class CoClassAttribute : Attribute
	{
		// Token: 0x06003196 RID: 12694 RVA: 0x000A9892 File Offset: 0x000A8892
		public CoClassAttribute(Type coClass)
		{
			this._CoClass = coClass;
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x000A98A1 File Offset: 0x000A88A1
		public Type CoClass
		{
			get
			{
				return this._CoClass;
			}
		}

		// Token: 0x040019A5 RID: 6565
		internal Type _CoClass;
	}
}
