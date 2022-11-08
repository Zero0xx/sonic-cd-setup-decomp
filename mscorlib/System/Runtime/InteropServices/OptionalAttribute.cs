using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FB RID: 1275
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	public sealed class OptionalAttribute : Attribute
	{
		// Token: 0x0600317D RID: 12669 RVA: 0x000A94D9 File Offset: 0x000A84D9
		internal static Attribute GetCustomAttribute(ParameterInfo parameter)
		{
			if (!parameter.IsOptional)
			{
				return null;
			}
			return new OptionalAttribute();
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x000A94EA File Offset: 0x000A84EA
		internal static bool IsDefined(ParameterInfo parameter)
		{
			return parameter.IsOptional;
		}
	}
}
