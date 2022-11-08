using System;

namespace System.Globalization
{
	// Token: 0x020003AA RID: 938
	internal class TokenHashValue
	{
		// Token: 0x0600259A RID: 9626 RVA: 0x0006886B File Offset: 0x0006786B
		internal TokenHashValue(string tokenString, TokenType tokenType, int tokenValue)
		{
			this.tokenString = tokenString;
			this.tokenType = tokenType;
			this.tokenValue = tokenValue;
		}

		// Token: 0x040010CF RID: 4303
		internal string tokenString;

		// Token: 0x040010D0 RID: 4304
		internal TokenType tokenType;

		// Token: 0x040010D1 RID: 4305
		internal int tokenValue;
	}
}
