using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E1 RID: 1249
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	public sealed class ClassInterfaceAttribute : Attribute
	{
		// Token: 0x06003144 RID: 12612 RVA: 0x000A9041 File Offset: 0x000A8041
		public ClassInterfaceAttribute(ClassInterfaceType classInterfaceType)
		{
			this._val = classInterfaceType;
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000A9050 File Offset: 0x000A8050
		public ClassInterfaceAttribute(short classInterfaceType)
		{
			this._val = (ClassInterfaceType)classInterfaceType;
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x000A905F File Offset: 0x000A805F
		public ClassInterfaceType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018FC RID: 6396
		internal ClassInterfaceType _val;
	}
}
