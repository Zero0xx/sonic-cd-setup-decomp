using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000841 RID: 2113
	[ComVisible(true)]
	[Serializable]
	public enum OperandType
	{
		// Token: 0x040027AD RID: 10157
		InlineBrTarget,
		// Token: 0x040027AE RID: 10158
		InlineField,
		// Token: 0x040027AF RID: 10159
		InlineI,
		// Token: 0x040027B0 RID: 10160
		InlineI8,
		// Token: 0x040027B1 RID: 10161
		InlineMethod,
		// Token: 0x040027B2 RID: 10162
		InlineNone,
		// Token: 0x040027B3 RID: 10163
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		InlinePhi,
		// Token: 0x040027B4 RID: 10164
		InlineR,
		// Token: 0x040027B5 RID: 10165
		InlineSig = 9,
		// Token: 0x040027B6 RID: 10166
		InlineString,
		// Token: 0x040027B7 RID: 10167
		InlineSwitch,
		// Token: 0x040027B8 RID: 10168
		InlineTok,
		// Token: 0x040027B9 RID: 10169
		InlineType,
		// Token: 0x040027BA RID: 10170
		InlineVar,
		// Token: 0x040027BB RID: 10171
		ShortInlineBrTarget,
		// Token: 0x040027BC RID: 10172
		ShortInlineI,
		// Token: 0x040027BD RID: 10173
		ShortInlineR,
		// Token: 0x040027BE RID: 10174
		ShortInlineVar
	}
}
