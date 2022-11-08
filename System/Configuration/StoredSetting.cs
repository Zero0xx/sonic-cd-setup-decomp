using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006E8 RID: 1768
	internal struct StoredSetting
	{
		// Token: 0x060036A5 RID: 13989 RVA: 0x000E9411 File Offset: 0x000E8411
		internal StoredSetting(SettingsSerializeAs serializeAs, XmlNode value)
		{
			this.SerializeAs = serializeAs;
			this.Value = value;
		}

		// Token: 0x0400318B RID: 12683
		internal SettingsSerializeAs SerializeAs;

		// Token: 0x0400318C RID: 12684
		internal XmlNode Value;
	}
}
