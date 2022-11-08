using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002CD RID: 717
	[ComVisible(true)]
	public interface ISymbolDocument
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001BB4 RID: 7092
		string URL { get; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001BB5 RID: 7093
		Guid DocumentType { get; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001BB6 RID: 7094
		Guid Language { get; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001BB7 RID: 7095
		Guid LanguageVendor { get; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001BB8 RID: 7096
		Guid CheckSumAlgorithmId { get; }

		// Token: 0x06001BB9 RID: 7097
		byte[] GetCheckSum();

		// Token: 0x06001BBA RID: 7098
		int FindClosestLine(int line);

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001BBB RID: 7099
		bool HasEmbeddedSource { get; }

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001BBC RID: 7100
		int SourceLength { get; }

		// Token: 0x06001BBD RID: 7101
		byte[] GetSourceRange(int startLine, int startColumn, int endLine, int endColumn);
	}
}
