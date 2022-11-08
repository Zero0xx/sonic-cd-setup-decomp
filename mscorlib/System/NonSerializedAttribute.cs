using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000DD RID: 221
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class NonSerializedAttribute : Attribute
	{
		// Token: 0x06000C1B RID: 3099 RVA: 0x000240FE File Offset: 0x000230FE
		internal static Attribute GetCustomAttribute(RuntimeFieldInfo field)
		{
			if ((field.Attributes & FieldAttributes.NotSerialized) == FieldAttributes.PrivateScope)
			{
				return null;
			}
			return new NonSerializedAttribute();
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00024115 File Offset: 0x00023115
		internal static bool IsDefined(RuntimeFieldInfo field)
		{
			return (field.Attributes & FieldAttributes.NotSerialized) != FieldAttributes.PrivateScope;
		}
	}
}
