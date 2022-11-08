using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020000F2 RID: 242
	[ComVisible(true)]
	public interface ICustomAttributeProvider
	{
		// Token: 0x06000C9D RID: 3229
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06000C9E RID: 3230
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06000C9F RID: 3231
		bool IsDefined(Type attributeType, bool inherit);
	}
}
