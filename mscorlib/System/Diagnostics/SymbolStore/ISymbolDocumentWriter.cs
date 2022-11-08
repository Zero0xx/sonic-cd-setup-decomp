using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002CE RID: 718
	[ComVisible(true)]
	public interface ISymbolDocumentWriter
	{
		// Token: 0x06001BBE RID: 7102
		void SetSource(byte[] source);

		// Token: 0x06001BBF RID: 7103
		void SetCheckSum(Guid algorithmId, byte[] checkSum);
	}
}
