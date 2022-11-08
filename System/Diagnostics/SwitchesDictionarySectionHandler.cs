using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x02000747 RID: 1863
	internal class SwitchesDictionarySectionHandler : DictionarySectionHandler
	{
		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x060038CD RID: 14541 RVA: 0x000F0054 File Offset: 0x000EF054
		protected override string KeyAttributeName
		{
			get
			{
				return "name";
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x060038CE RID: 14542 RVA: 0x000F005B File Offset: 0x000EF05B
		internal override bool ValueRequired
		{
			get
			{
				return true;
			}
		}
	}
}
