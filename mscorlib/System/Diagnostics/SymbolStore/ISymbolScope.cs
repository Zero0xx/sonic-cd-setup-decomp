using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002D2 RID: 722
	[ComVisible(true)]
	public interface ISymbolScope
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001BD7 RID: 7127
		ISymbolMethod Method { get; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001BD8 RID: 7128
		ISymbolScope Parent { get; }

		// Token: 0x06001BD9 RID: 7129
		ISymbolScope[] GetChildren();

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001BDA RID: 7130
		int StartOffset { get; }

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001BDB RID: 7131
		int EndOffset { get; }

		// Token: 0x06001BDC RID: 7132
		ISymbolVariable[] GetLocals();

		// Token: 0x06001BDD RID: 7133
		ISymbolNamespace[] GetNamespaces();
	}
}
