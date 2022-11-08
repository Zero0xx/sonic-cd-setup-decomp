using System;

namespace System.Security
{
	// Token: 0x02000611 RID: 1553
	internal interface ISecurityElementFactory
	{
		// Token: 0x0600381C RID: 14364
		SecurityElement CreateSecurityElement();

		// Token: 0x0600381D RID: 14365
		object Copy();

		// Token: 0x0600381E RID: 14366
		string GetTag();

		// Token: 0x0600381F RID: 14367
		string Attribute(string attributeName);
	}
}
