using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000608 RID: 1544
	[AttributeUsage(AttributeTargets.All)]
	[ComVisible(false)]
	internal sealed class DecoratedNameAttribute : Attribute
	{
		// Token: 0x06003802 RID: 14338 RVA: 0x000BBD3D File Offset: 0x000BAD3D
		public DecoratedNameAttribute(string decoratedName)
		{
		}
	}
}
