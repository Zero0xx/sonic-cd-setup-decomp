using System;
using System.Reflection;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006BC RID: 1724
	internal static class AsyncMessageHelper
	{
		// Token: 0x06003E0F RID: 15887 RVA: 0x000D42CC File Offset: 0x000D32CC
		internal static void GetOutArgs(ParameterInfo[] syncParams, object[] syncArgs, object[] endArgs)
		{
			int num = 0;
			for (int i = 0; i < syncParams.Length; i++)
			{
				if (syncParams[i].IsOut || syncParams[i].ParameterType.IsByRef)
				{
					endArgs[num++] = syncArgs[i];
				}
			}
		}
	}
}
