using System;

namespace System.ComponentModel
{
	// Token: 0x0200010D RID: 269
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class LicenseProviderAttribute : Attribute
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x0001CA02 File Offset: 0x0001BA02
		public LicenseProviderAttribute() : this(null)
		{
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0001CA0B File Offset: 0x0001BA0B
		public LicenseProviderAttribute(string typeName)
		{
			this.licenseProviderName = typeName;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0001CA1A File Offset: 0x0001BA1A
		public LicenseProviderAttribute(Type type)
		{
			this.licenseProviderType = type;
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0001CA29 File Offset: 0x0001BA29
		public Type LicenseProvider
		{
			get
			{
				if (this.licenseProviderType == null && this.licenseProviderName != null)
				{
					this.licenseProviderType = Type.GetType(this.licenseProviderName);
				}
				return this.licenseProviderType;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0001CA54 File Offset: 0x0001BA54
		public override object TypeId
		{
			get
			{
				string fullName = this.licenseProviderName;
				if (fullName == null && this.licenseProviderType != null)
				{
					fullName = this.licenseProviderType.FullName;
				}
				return base.GetType().FullName + fullName;
			}
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0001CA90 File Offset: 0x0001BA90
		public override bool Equals(object value)
		{
			if (value is LicenseProviderAttribute && value != null)
			{
				Type licenseProvider = ((LicenseProviderAttribute)value).LicenseProvider;
				if (licenseProvider == this.LicenseProvider)
				{
					return true;
				}
				if (licenseProvider != null && licenseProvider.Equals(this.LicenseProvider))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0001CAD3 File Offset: 0x0001BAD3
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000996 RID: 2454
		public static readonly LicenseProviderAttribute Default = new LicenseProviderAttribute();

		// Token: 0x04000997 RID: 2455
		private Type licenseProviderType;

		// Token: 0x04000998 RID: 2456
		private string licenseProviderName;
	}
}
