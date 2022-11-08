using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002D4 RID: 724
	[ComVisible(true)]
	public interface ISymbolWriter
	{
		// Token: 0x06001BE7 RID: 7143
		void Initialize(IntPtr emitter, string filename, bool fFullBuild);

		// Token: 0x06001BE8 RID: 7144
		ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType);

		// Token: 0x06001BE9 RID: 7145
		void SetUserEntryPoint(SymbolToken entryMethod);

		// Token: 0x06001BEA RID: 7146
		void OpenMethod(SymbolToken method);

		// Token: 0x06001BEB RID: 7147
		void CloseMethod();

		// Token: 0x06001BEC RID: 7148
		void DefineSequencePoints(ISymbolDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns);

		// Token: 0x06001BED RID: 7149
		int OpenScope(int startOffset);

		// Token: 0x06001BEE RID: 7150
		void CloseScope(int endOffset);

		// Token: 0x06001BEF RID: 7151
		void SetScopeRange(int scopeID, int startOffset, int endOffset);

		// Token: 0x06001BF0 RID: 7152
		void DefineLocalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset);

		// Token: 0x06001BF1 RID: 7153
		void DefineParameter(string name, ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06001BF2 RID: 7154
		void DefineField(SymbolToken parent, string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06001BF3 RID: 7155
		void DefineGlobalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06001BF4 RID: 7156
		void Close();

		// Token: 0x06001BF5 RID: 7157
		void SetSymAttribute(SymbolToken parent, string name, byte[] data);

		// Token: 0x06001BF6 RID: 7158
		void OpenNamespace(string name);

		// Token: 0x06001BF7 RID: 7159
		void CloseNamespace();

		// Token: 0x06001BF8 RID: 7160
		void UsingNamespace(string fullName);

		// Token: 0x06001BF9 RID: 7161
		void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn);

		// Token: 0x06001BFA RID: 7162
		void SetUnderlyingWriter(IntPtr underlyingWriter);
	}
}
