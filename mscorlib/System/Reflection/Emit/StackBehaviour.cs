using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000840 RID: 2112
	[ComVisible(true)]
	[Serializable]
	public enum StackBehaviour
	{
		// Token: 0x0400278F RID: 10127
		Pop0,
		// Token: 0x04002790 RID: 10128
		Pop1,
		// Token: 0x04002791 RID: 10129
		Pop1_pop1,
		// Token: 0x04002792 RID: 10130
		Popi,
		// Token: 0x04002793 RID: 10131
		Popi_pop1,
		// Token: 0x04002794 RID: 10132
		Popi_popi,
		// Token: 0x04002795 RID: 10133
		Popi_popi8,
		// Token: 0x04002796 RID: 10134
		Popi_popi_popi,
		// Token: 0x04002797 RID: 10135
		Popi_popr4,
		// Token: 0x04002798 RID: 10136
		Popi_popr8,
		// Token: 0x04002799 RID: 10137
		Popref,
		// Token: 0x0400279A RID: 10138
		Popref_pop1,
		// Token: 0x0400279B RID: 10139
		Popref_popi,
		// Token: 0x0400279C RID: 10140
		Popref_popi_popi,
		// Token: 0x0400279D RID: 10141
		Popref_popi_popi8,
		// Token: 0x0400279E RID: 10142
		Popref_popi_popr4,
		// Token: 0x0400279F RID: 10143
		Popref_popi_popr8,
		// Token: 0x040027A0 RID: 10144
		Popref_popi_popref,
		// Token: 0x040027A1 RID: 10145
		Push0,
		// Token: 0x040027A2 RID: 10146
		Push1,
		// Token: 0x040027A3 RID: 10147
		Push1_push1,
		// Token: 0x040027A4 RID: 10148
		Pushi,
		// Token: 0x040027A5 RID: 10149
		Pushi8,
		// Token: 0x040027A6 RID: 10150
		Pushr4,
		// Token: 0x040027A7 RID: 10151
		Pushr8,
		// Token: 0x040027A8 RID: 10152
		Pushref,
		// Token: 0x040027A9 RID: 10153
		Varpop,
		// Token: 0x040027AA RID: 10154
		Varpush,
		// Token: 0x040027AB RID: 10155
		Popref_popi_pop1
	}
}
