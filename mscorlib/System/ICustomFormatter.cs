using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C1 RID: 193
	[ComVisible(true)]
	public interface ICustomFormatter
	{
		// Token: 0x06000AFF RID: 2815
		string Format(string format, object arg, IFormatProvider formatProvider);
	}
}
