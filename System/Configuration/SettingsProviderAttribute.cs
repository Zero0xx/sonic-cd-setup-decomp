using System;

namespace System.Configuration
{
	// Token: 0x0200070A RID: 1802
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsProviderAttribute : Attribute
	{
		// Token: 0x06003740 RID: 14144 RVA: 0x000EAD7D File Offset: 0x000E9D7D
		public SettingsProviderAttribute(string providerTypeName)
		{
			this._providerTypeName = providerTypeName;
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x000EAD8C File Offset: 0x000E9D8C
		public SettingsProviderAttribute(Type providerType)
		{
			if (providerType != null)
			{
				this._providerTypeName = providerType.AssemblyQualifiedName;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06003742 RID: 14146 RVA: 0x000EADA3 File Offset: 0x000E9DA3
		public string ProviderTypeName
		{
			get
			{
				return this._providerTypeName;
			}
		}

		// Token: 0x040031B1 RID: 12721
		private readonly string _providerTypeName;
	}
}
