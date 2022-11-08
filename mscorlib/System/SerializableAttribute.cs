using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000115 RID: 277
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
	[ComVisible(true)]
	public sealed class SerializableAttribute : Attribute
	{
		// Token: 0x06000FF9 RID: 4089 RVA: 0x0002D954 File Offset: 0x0002C954
		internal static Attribute GetCustomAttribute(Type type)
		{
			if ((type.Attributes & TypeAttributes.Serializable) != TypeAttributes.Serializable)
			{
				return null;
			}
			return new SerializableAttribute();
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0002D970 File Offset: 0x0002C970
		internal static bool IsDefined(Type type)
		{
			return type.IsSerializable;
		}
	}
}
