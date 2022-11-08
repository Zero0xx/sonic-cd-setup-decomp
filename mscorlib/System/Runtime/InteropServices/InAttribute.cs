using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F9 RID: 1273
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class InAttribute : Attribute
	{
		// Token: 0x06003177 RID: 12663 RVA: 0x000A9497 File Offset: 0x000A8497
		internal static Attribute GetCustomAttribute(ParameterInfo parameter)
		{
			if (!parameter.IsIn)
			{
				return null;
			}
			return new InAttribute();
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x000A94A8 File Offset: 0x000A84A8
		internal static bool IsDefined(ParameterInfo parameter)
		{
			return parameter.IsIn;
		}
	}
}
