using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FA RID: 1274
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	public sealed class OutAttribute : Attribute
	{
		// Token: 0x0600317A RID: 12666 RVA: 0x000A94B8 File Offset: 0x000A84B8
		internal static Attribute GetCustomAttribute(ParameterInfo parameter)
		{
			if (!parameter.IsOut)
			{
				return null;
			}
			return new OutAttribute();
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x000A94C9 File Offset: 0x000A84C9
		internal static bool IsDefined(ParameterInfo parameter)
		{
			return parameter.IsOut;
		}
	}
}
