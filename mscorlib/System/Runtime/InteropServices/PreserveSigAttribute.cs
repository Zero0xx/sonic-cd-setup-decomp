using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F8 RID: 1272
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class PreserveSigAttribute : Attribute
	{
		// Token: 0x06003174 RID: 12660 RVA: 0x000A9464 File Offset: 0x000A8464
		internal static Attribute GetCustomAttribute(RuntimeMethodInfo method)
		{
			if ((method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) == MethodImplAttributes.IL)
			{
				return null;
			}
			return new PreserveSigAttribute();
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x000A947B File Offset: 0x000A847B
		internal static bool IsDefined(RuntimeMethodInfo method)
		{
			return (method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) != MethodImplAttributes.IL;
		}
	}
}
