using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002CC RID: 716
	[ComVisible(true)]
	public interface ISymbolBinder1
	{
		// Token: 0x06001BB3 RID: 7091
		ISymbolReader GetReader(IntPtr importer, string filename, string searchPath);
	}
}
