using System;

namespace System.Globalization
{
	// Token: 0x020003E2 RID: 994
	internal struct HebrewNumberParsingContext
	{
		// Token: 0x060028F8 RID: 10488 RVA: 0x0007E6D5 File Offset: 0x0007D6D5
		public HebrewNumberParsingContext(int result)
		{
			this.state = HebrewNumber.HS.Start;
			this.result = 0;
		}

		// Token: 0x040013C6 RID: 5062
		internal HebrewNumber.HS state;

		// Token: 0x040013C7 RID: 5063
		internal int result;
	}
}
