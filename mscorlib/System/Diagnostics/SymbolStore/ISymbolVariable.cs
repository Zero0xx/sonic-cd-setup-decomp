using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002D3 RID: 723
	[ComVisible(true)]
	public interface ISymbolVariable
	{
		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001BDE RID: 7134
		string Name { get; }

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001BDF RID: 7135
		object Attributes { get; }

		// Token: 0x06001BE0 RID: 7136
		byte[] GetSignature();

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001BE1 RID: 7137
		SymAddressKind AddressKind { get; }

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001BE2 RID: 7138
		int AddressField1 { get; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001BE3 RID: 7139
		int AddressField2 { get; }

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001BE4 RID: 7140
		int AddressField3 { get; }

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001BE5 RID: 7141
		int StartOffset { get; }

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001BE6 RID: 7142
		int EndOffset { get; }
	}
}
