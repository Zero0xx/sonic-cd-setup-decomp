using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C2 RID: 194
	[ComVisible(true)]
	public interface IFormatProvider
	{
		// Token: 0x06000B00 RID: 2816
		object GetFormat(Type formatType);
	}
}
