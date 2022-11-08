using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F6 RID: 1270
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class ComImportAttribute : Attribute
	{
		// Token: 0x0600316F RID: 12655 RVA: 0x000A941A File Offset: 0x000A841A
		internal static Attribute GetCustomAttribute(RuntimeType type)
		{
			if ((type.Attributes & TypeAttributes.Import) == TypeAttributes.NotPublic)
			{
				return null;
			}
			return new ComImportAttribute();
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x000A9431 File Offset: 0x000A8431
		internal static bool IsDefined(RuntimeType type)
		{
			return (type.Attributes & TypeAttributes.Import) != TypeAttributes.NotPublic;
		}
	}
}
