using System;

namespace System
{
	// Token: 0x0200039E RID: 926
	internal struct DTSubString
	{
		// Token: 0x17000670 RID: 1648
		internal char this[int relativeIndex]
		{
			get
			{
				return this.s[this.index + relativeIndex];
			}
		}

		// Token: 0x04001003 RID: 4099
		internal string s;

		// Token: 0x04001004 RID: 4100
		internal int index;

		// Token: 0x04001005 RID: 4101
		internal int length;

		// Token: 0x04001006 RID: 4102
		internal DTSubStringType type;

		// Token: 0x04001007 RID: 4103
		internal int value;
	}
}
