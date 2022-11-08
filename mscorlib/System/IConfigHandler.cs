﻿using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000081 RID: 129
	[Guid("afd0d21f-72f8-4819-99ad-3f255ee5006b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IConfigHandler
	{
		// Token: 0x0600074C RID: 1868
		void NotifyEvent(ConfigEvents nEvent);

		// Token: 0x0600074D RID: 1869
		void BeginChildren(int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength);

		// Token: 0x0600074E RID: 1870
		void EndChildren(int fEmpty, int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength);

		// Token: 0x0600074F RID: 1871
		void Error(int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength);

		// Token: 0x06000750 RID: 1872
		void CreateNode(int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength);

		// Token: 0x06000751 RID: 1873
		void CreateAttribute(int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength);
	}
}
