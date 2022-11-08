using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	// Token: 0x02000434 RID: 1076
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	[ComVisible(true)]
	public sealed class NeutralResourcesLanguageAttribute : Attribute
	{
		// Token: 0x06002BDA RID: 11226 RVA: 0x00092AB7 File Offset: 0x00091AB7
		public NeutralResourcesLanguageAttribute(string cultureName)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName");
			}
			this._culture = cultureName;
			this._fallbackLoc = UltimateResourceFallbackLocation.MainAssembly;
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x00092ADC File Offset: 0x00091ADC
		public NeutralResourcesLanguageAttribute(string cultureName, UltimateResourceFallbackLocation location)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName");
			}
			if (!Enum.IsDefined(typeof(UltimateResourceFallbackLocation), location))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidNeutralResourcesLanguage_FallbackLoc", new object[]
				{
					location
				}));
			}
			this._culture = cultureName;
			this._fallbackLoc = location;
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002BDC RID: 11228 RVA: 0x00092B43 File Offset: 0x00091B43
		public string CultureName
		{
			get
			{
				return this._culture;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002BDD RID: 11229 RVA: 0x00092B4B File Offset: 0x00091B4B
		public UltimateResourceFallbackLocation Location
		{
			get
			{
				return this._fallbackLoc;
			}
		}

		// Token: 0x0400155B RID: 5467
		private string _culture;

		// Token: 0x0400155C RID: 5468
		private UltimateResourceFallbackLocation _fallbackLoc;
	}
}
