using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DE RID: 1246
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class InterfaceTypeAttribute : Attribute
	{
		// Token: 0x0600313F RID: 12607 RVA: 0x000A9004 File Offset: 0x000A8004
		public InterfaceTypeAttribute(ComInterfaceType interfaceType)
		{
			this._val = interfaceType;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000A9013 File Offset: 0x000A8013
		public InterfaceTypeAttribute(short interfaceType)
		{
			this._val = (ComInterfaceType)interfaceType;
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x000A9022 File Offset: 0x000A8022
		public ComInterfaceType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018F6 RID: 6390
		internal ComInterfaceType _val;
	}
}
