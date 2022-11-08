using System;

namespace System.Configuration
{
	// Token: 0x0200070B RID: 1803
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsSerializeAsAttribute : Attribute
	{
		// Token: 0x06003743 RID: 14147 RVA: 0x000EADAB File Offset: 0x000E9DAB
		public SettingsSerializeAsAttribute(SettingsSerializeAs serializeAs)
		{
			this._serializeAs = serializeAs;
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06003744 RID: 14148 RVA: 0x000EADBA File Offset: 0x000E9DBA
		public SettingsSerializeAs SerializeAs
		{
			get
			{
				return this._serializeAs;
			}
		}

		// Token: 0x040031B2 RID: 12722
		private readonly SettingsSerializeAs _serializeAs;
	}
}
