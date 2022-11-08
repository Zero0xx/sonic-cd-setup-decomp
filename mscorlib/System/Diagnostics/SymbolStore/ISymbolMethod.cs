using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002CF RID: 719
	[ComVisible(true)]
	public interface ISymbolMethod
	{
		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001BC0 RID: 7104
		SymbolToken Token { get; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001BC1 RID: 7105
		int SequencePointCount { get; }

		// Token: 0x06001BC2 RID: 7106
		void GetSequencePoints(int[] offsets, ISymbolDocument[] documents, int[] lines, int[] columns, int[] endLines, int[] endColumns);

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001BC3 RID: 7107
		ISymbolScope RootScope { get; }

		// Token: 0x06001BC4 RID: 7108
		ISymbolScope GetScope(int offset);

		// Token: 0x06001BC5 RID: 7109
		int GetOffset(ISymbolDocument document, int line, int column);

		// Token: 0x06001BC6 RID: 7110
		int[] GetRanges(ISymbolDocument document, int line, int column);

		// Token: 0x06001BC7 RID: 7111
		ISymbolVariable[] GetParameters();

		// Token: 0x06001BC8 RID: 7112
		ISymbolNamespace GetNamespace();

		// Token: 0x06001BC9 RID: 7113
		bool GetSourceStartEnd(ISymbolDocument[] docs, int[] lines, int[] columns);
	}
}
