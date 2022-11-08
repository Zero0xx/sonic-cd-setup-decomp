using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002D1 RID: 721
	[ComVisible(true)]
	public interface ISymbolReader
	{
		// Token: 0x06001BCD RID: 7117
		ISymbolDocument GetDocument(string url, Guid language, Guid languageVendor, Guid documentType);

		// Token: 0x06001BCE RID: 7118
		ISymbolDocument[] GetDocuments();

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001BCF RID: 7119
		SymbolToken UserEntryPoint { get; }

		// Token: 0x06001BD0 RID: 7120
		ISymbolMethod GetMethod(SymbolToken method);

		// Token: 0x06001BD1 RID: 7121
		ISymbolMethod GetMethod(SymbolToken method, int version);

		// Token: 0x06001BD2 RID: 7122
		ISymbolVariable[] GetVariables(SymbolToken parent);

		// Token: 0x06001BD3 RID: 7123
		ISymbolVariable[] GetGlobalVariables();

		// Token: 0x06001BD4 RID: 7124
		ISymbolMethod GetMethodFromDocumentPosition(ISymbolDocument document, int line, int column);

		// Token: 0x06001BD5 RID: 7125
		byte[] GetSymAttribute(SymbolToken parent, string name);

		// Token: 0x06001BD6 RID: 7126
		ISymbolNamespace[] GetNamespaces();
	}
}
