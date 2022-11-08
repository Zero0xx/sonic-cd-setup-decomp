using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002D0 RID: 720
	[ComVisible(true)]
	public interface ISymbolNamespace
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001BCA RID: 7114
		string Name { get; }

		// Token: 0x06001BCB RID: 7115
		ISymbolNamespace[] GetNamespaces();

		// Token: 0x06001BCC RID: 7116
		ISymbolVariable[] GetVariables();
	}
}
