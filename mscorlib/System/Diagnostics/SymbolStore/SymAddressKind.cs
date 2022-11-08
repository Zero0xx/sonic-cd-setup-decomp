using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002D5 RID: 725
	[ComVisible(true)]
	[Serializable]
	public enum SymAddressKind
	{
		// Token: 0x04000AAE RID: 2734
		ILOffset = 1,
		// Token: 0x04000AAF RID: 2735
		NativeRVA,
		// Token: 0x04000AB0 RID: 2736
		NativeRegister,
		// Token: 0x04000AB1 RID: 2737
		NativeRegisterRelative,
		// Token: 0x04000AB2 RID: 2738
		NativeOffset,
		// Token: 0x04000AB3 RID: 2739
		NativeRegisterRegister,
		// Token: 0x04000AB4 RID: 2740
		NativeRegisterStack,
		// Token: 0x04000AB5 RID: 2741
		NativeStackRegister,
		// Token: 0x04000AB6 RID: 2742
		BitField,
		// Token: 0x04000AB7 RID: 2743
		NativeSectionOffset
	}
}
