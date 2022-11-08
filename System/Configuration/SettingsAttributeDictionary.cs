using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000701 RID: 1793
	[Serializable]
	public class SettingsAttributeDictionary : Hashtable
	{
		// Token: 0x06003731 RID: 14129 RVA: 0x000EACE1 File Offset: 0x000E9CE1
		public SettingsAttributeDictionary()
		{
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x000EACE9 File Offset: 0x000E9CE9
		public SettingsAttributeDictionary(SettingsAttributeDictionary attributes) : base(attributes)
		{
		}
	}
}
