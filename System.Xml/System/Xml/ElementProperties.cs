using System;

namespace System.Xml
{
	// Token: 0x02000062 RID: 98
	internal enum ElementProperties : uint
	{
		// Token: 0x040005AE RID: 1454
		DEFAULT,
		// Token: 0x040005AF RID: 1455
		URI_PARENT,
		// Token: 0x040005B0 RID: 1456
		BOOL_PARENT,
		// Token: 0x040005B1 RID: 1457
		NAME_PARENT = 4U,
		// Token: 0x040005B2 RID: 1458
		EMPTY = 8U,
		// Token: 0x040005B3 RID: 1459
		NO_ENTITIES = 16U,
		// Token: 0x040005B4 RID: 1460
		HEAD = 32U,
		// Token: 0x040005B5 RID: 1461
		BLOCK_WS = 64U,
		// Token: 0x040005B6 RID: 1462
		HAS_NS = 128U
	}
}
