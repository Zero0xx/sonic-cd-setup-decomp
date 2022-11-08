using System;

namespace System.Xml
{
	// Token: 0x020000A7 RID: 167
	public enum WriteState
	{
		// Token: 0x04000827 RID: 2087
		Start,
		// Token: 0x04000828 RID: 2088
		Prolog,
		// Token: 0x04000829 RID: 2089
		Element,
		// Token: 0x0400082A RID: 2090
		Attribute,
		// Token: 0x0400082B RID: 2091
		Content,
		// Token: 0x0400082C RID: 2092
		Closed,
		// Token: 0x0400082D RID: 2093
		Error
	}
}
