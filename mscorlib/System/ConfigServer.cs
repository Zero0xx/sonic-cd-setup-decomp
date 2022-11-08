using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000082 RID: 130
	internal static class ConfigServer
	{
		// Token: 0x06000752 RID: 1874
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RunParser(IConfigHandler factory, string fileName);
	}
}
