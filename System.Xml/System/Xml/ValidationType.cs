using System;

namespace System.Xml
{
	// Token: 0x0200006A RID: 106
	public enum ValidationType
	{
		// Token: 0x040005D0 RID: 1488
		None,
		// Token: 0x040005D1 RID: 1489
		[Obsolete("Validation type should be specified as DTD or Schema.")]
		Auto,
		// Token: 0x040005D2 RID: 1490
		DTD,
		// Token: 0x040005D3 RID: 1491
		[Obsolete("XDR Validation through XmlValidatingReader is obsoleted")]
		XDR,
		// Token: 0x040005D4 RID: 1492
		Schema
	}
}
